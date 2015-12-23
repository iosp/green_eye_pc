using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GreenEyeAPI.Core;
using GreenEyeAPI.Core.DataStructures;
using GreenEyeAPI.Core.BLInterface;
using GreenEyeAPI.Core.GeoUtils;
using System.Configuration;
using System.Collections;
using System.Web.Script.Serialization;
using System.Net;

//using GreenEyeAPI.Core.DataStructures;
//using GreenEyeAPI.Core.GeoUtils;

namespace GreenEyeAPI.BL
{
    public class SampleWorldBLImplementation
    {
        // Added by Nir for Platform distribution
        private Hashtable mEntities = new Hashtable();
        private int mEntitiesPublishInterval = int.Parse(ConfigurationManager.AppSettings["EntityPublishInterval"]); // 1000 ms (1 sec)
        private string[] mEntitiesPublishRecipients = ConfigurationManager.AppSettings["EntityPublishRecipients"].Split(';');
        private DateTime mLastPublish = DateTime.Now;

        private static SampleWorldBLImplementation m_pMyself = null;

        private SampleWorldBLImplementation()
        {
            WorldServerBLInterface.Instance().OnAbortMission += this.AbortMissionHandler;
            WorldServerBLInterface.Instance().OnSendCommand += this.SendCommandHandler;
            WorldServerBLInterface.Instance().OnStartMission += this.StartMissionHandler;
            WorldServerBLInterface.Instance().OnUpdateMission += this.UpdateMissionHandler;
        }

        public static SampleWorldBLImplementation Instance()
        {
            if (m_pMyself == null)
                m_pMyself = new SampleWorldBLImplementation();
            return m_pMyself;
        }

        public MissionPlan StartMissionHandler(MissionPlan missionPlan)
        {
            return missionPlan;
        }

        public void UpdateMissionHandler(string missionId, MissionPlan missionPlan)
        {
        }

        public void AbortMissionHandler(string missionId)
        {
        }

        public MissionPlan SendCommandHandler(string missionId, int platformId, Command command)
        {
            return null;
        }

        public void UpdateAppServer(Common.Entities.IPlatform pPlatform, string pMissionId)
        {
            // *************** Build the updated platform, in the API data structure ******************
            Platform UpdatePlatform = new Platform();
            UpdatePlatform.id = pPlatform.Id;

            // *************** Platform State ******************
            UpdatePlatform.state = new GreenEyeAPI.Core.DataStructures.PlatformState()
            {

                alt = (long)pPlatform.State.ASL,
                deployState = "Active",//pPlatform.State.DeployState.ToString(),
                energy = pPlatform.State.Energy,
                failureState = "None",//pPlatform.State.FailureState.ToString(),
                missionState = "Active"//pPlatform.State.MissionState.ToString()
            };

            // Copy Position
            double PosLat, PosLng;
            GeoConverter geoConverter = new GeoConverter();

            geoConverter.UTMToGeo(pPlatform.State.Position.East, pPlatform.State.Position.North, pPlatform.State.Position.Zone, (pPlatform.State.Position.Hemisphere == 0 ? "N" : "S"), out PosLat, out PosLng);
            Position PlatformPos = new Position();
            //PlatformPos.Add(PosLat);
            //PlatformPos.Add(PosLng);
            PlatformPos.Add(PosLng);
            PlatformPos.Add(PosLat);

            UpdatePlatform.state.position = PlatformPos;
            

            // Copy Velocity
            Velocity PlatformVelocity = new Velocity();
            PlatformVelocity.Add(pPlatform.State.Velocity.x);
            PlatformVelocity.Add(pPlatform.State.Velocity.y);
            PlatformVelocity.Add(pPlatform.State.Velocity.z);

            //ASWorkPlan.platforms[CurrentPlatform].sensors
            UpdatePlatform.state.velocity = PlatformVelocity;

            // *************** Platform Route ******************
            UpdatePlatform.route = new GreenEyeAPI.Core.DataStructures.Route();
            if (pPlatform.Route != null)
            {
                foreach (Common.Entities.Waypoint LogicWaypoint in pPlatform.Route)
                {
                    GreenEyeAPI.Core.DataStructures.Waypoint PlatfromWaypoint = new GreenEyeAPI.Core.DataStructures.Waypoint()
                    {
                        id = LogicWaypoint.Id,
                        actions = null,
                        alt = LogicWaypoint.m_ASL
                    };

                    // Copy Position
                    geoConverter.UTMToGeo(LogicWaypoint.Position.East, LogicWaypoint.Position.North, LogicWaypoint.Position.Zone, (LogicWaypoint.Position.Hemisphere == 0 ? "N" : "S"), out PosLat, out PosLng);
                    PlatformPos = new Position();
                    //PlatformPos.Add(PosLat);
                    //PlatformPos.Add(PosLng);
                    PlatformPos.Add(PosLng);
                    PlatformPos.Add(PosLat);
                    
                    PlatfromWaypoint.position = PlatformPos;

                    UpdatePlatform.route.Add(PlatfromWaypoint);
                }
            }

            // *************** Platform Sensors ******************
            if (pPlatform.Sensors != null)
            {
                UpdatePlatform.sensors = new Sensor[pPlatform.Sensors.Count];
                int CurrentSensor = 0;
                foreach (Common.Entities.ISensor LogicSensor in pPlatform.Sensors)
                {
                    UpdatePlatform.sensors[CurrentSensor] = new Sensor()
                    {
                        id = LogicSensor.Id,
                        type = LogicSensor.Type.ToString(),
                        active = (LogicSensor.State > 0),
                        hfov = LogicSensor.HFov,
                        vfov = LogicSensor.VFov
                    };
                    CurrentSensor++;
                }
            }

            if (mEntities.ContainsKey(UpdatePlatform.id.ToString()) == true)
                mEntities[UpdatePlatform.id.ToString()] = UpdatePlatform;
            else
                mEntities.Add(UpdatePlatform.id.ToString(), UpdatePlatform);

            UpdatePlatform.state.missionState = "Active";

            if (mLastPublish.AddMilliseconds(mEntitiesPublishInterval) < DateTime.Now)
            {
                WebClient WC = new WebClient();
                WC.Headers["Content-Type"] = "application/json";
                WC.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJhZG1pbkBpYWkuY28uaWwiLCJpYXQiOjE0NDIzODk2NzN9.v8ImKujUei0Y4s47SBtZ0PQ27cfzCwjuJynxcAuJ0t4";
                // Prepare the data
                World W = new World();
                foreach (Platform P in mEntities.Values)
                {
                    W.Add(P);
                }

                // Prepare the Json
                JavaScriptSerializer oSer = new JavaScriptSerializer();
                string JsonW = oSer.Serialize(W);

                for (int i = 0; i < mEntitiesPublishRecipients.Length; i++)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(mEntitiesPublishRecipients[i]) == false)
                            WC.UploadString(mEntitiesPublishRecipients[i].Replace("{missionid}", pMissionId.ToString()), "PUT", JsonW);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Platform Publisher - Error sending world update command: " + ex.Message);
                    }
                }
                mLastPublish = DateTime.Now;
            }
        }
    }
}
