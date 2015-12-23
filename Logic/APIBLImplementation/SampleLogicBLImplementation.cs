using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GreenEyeAPI.Core;
using GreenEyeAPI.Core.DataStructures;
using GreenEyeAPI.Core.BLInterface;
using GreenEyeAPI.Core.GeoUtils;

namespace GreenEyeAPI.BL
{
    public class SampleLogicBLImplementation
    {
        private Logic.Scenario m_Scenario;               


        private Dictionary<string, MissionPlan> m_ActiveMissions = new Dictionary<string, MissionPlan>(); 

        public SampleLogicBLImplementation(Logic.Scenario a_Scenario)
        {
            LogicServerBLInterface.Instance().OnStartMission += this.StartMissionHandler;
            LogicServerBLInterface.Instance().OnStopMission += this.StopMissionHandler;
            LogicServerBLInterface.Instance().OnAbortMission += this.AbortMissionHandler;
            LogicServerBLInterface.Instance().OnGetAreas += this.GetAreasHandler;
            LogicServerBLInterface.Instance().OnUpdateMissionWorld += this.UpdateMissionWorldHandler;

            m_Scenario = a_Scenario;
            m_Scenario.ReceiveSimCommand(Logic.GEMessages.SIM_OPCODES.GE_SIM_STOP);     
        }



        public Cell[] GetAreasHandler(PoligonGeoJson polygon, MissionConfig missionConfig, Platform[] platforms)
        {                        
            List<Common.Entities.Point2d> UTMPolygon = ConvertPolygonToUTM(polygon);
            List<Common.Entities.Point2d> AlgoResults = new List<Common.Entities.Point2d>();
            m_Scenario.UTMPolygon = UTMPolygon;
            double ProjectionLength = m_Scenario.Config.ProjectionLength;

            List<Common.Entities.CTarget> Targets = m_Scenario.Targets;
            foreach (Common.Entities.CTarget target in Targets)
            {
                Common.Entities.Point2d pos = target.Position;
                AlgoResults.Add(pos);
            }            

            // Convert UTM position to cell in GEO
            GeoConverter geoConverter = new GeoConverter();
            List<Cell> GeoCells = new List<Cell>();
            Common.Entities.Point2d UpLeft;
            Common.Entities.Point2d BottomRight;

            double lat;
            double lng;

            foreach (Common.Entities.Point2d UTMCenter in AlgoResults)
            {
                // For each cell...
                // Calculate the cell area
                UpLeft.East = UTMCenter.East - ProjectionLength / 2.0;
                UpLeft.North = UTMCenter.North + ProjectionLength / 2.0;
                UpLeft.Zone = UTMCenter.Zone;
                UpLeft.Hemisphere = UTMCenter.Hemisphere;
                BottomRight.East = UTMCenter.East + ProjectionLength / 2.0;
                BottomRight.North = UTMCenter.North - ProjectionLength / 2.0;
                BottomRight.Zone = UTMCenter.Zone;
                BottomRight.Hemisphere = UTMCenter.Hemisphere;

                // Create a single cell to the return value
                Cell cell = new Cell();

                // Convert the upper left corner from UTM to Geo
                geoConverter.UTMToGeo(UpLeft.East, UpLeft.North, UpLeft.Zone, (UpLeft.Hemisphere == 0 ? "N" : "S"), out lat, out lng);
                Position pos = new Position();
// ### Edit by DF 22/10/15:                
                //pos.Add(lat);
                //pos.Add(lng);
                pos.Add(lng);
                pos.Add(lat);
                
                // Add the new upper left point to the cell
                cell.Add(pos);
                // Convert the bottom right corner from UTM to geo
                geoConverter.UTMToGeo(BottomRight.East, BottomRight.North, BottomRight.Zone, (BottomRight.Hemisphere == 0 ? "N" : "S"), out lat, out lng);
                pos = new Position();
// ### Edit by DF 22/10/15:
                //pos.Add(lat);
                //pos.Add(lng);                
                pos.Add(lng);
                pos.Add(lat);
                // Add the new bottom right point to the cell
                cell.Add(pos);
                // Add the cell to the cells list
                GeoCells.Add(cell);
            }

            return GeoCells.ToArray<Cell>();                       
        }


       

        /// <summary>
        /// Start Mission
        /// </summary>
        public MissionPlan StartMissionHandler(Mission ASMission)
        {
            Common.Entities.CMission mission = ConvertToLogicMission(ASMission);
            m_Scenario.ReceiveSimCommand(Logic.GEMessages.SIM_OPCODES.GE_SIM_STOP);
            // Set UTM offset to first launcher
            if (mission.Launchers.Count > 0)
            {
                Common.Entities.Point2d NewOffset = mission.Launchers[0].Position;
                m_Scenario.FixPlatformsPosition(mission.Config.UTMOffset, NewOffset);
                mission.Config.UTMOffset = NewOffset;
            }
                    
            List<Common.Entities.CRoute> WorkPlan = m_Scenario.UpdateWorkPlan();
            m_Scenario.ReceiveSimCommand(Logic.GEMessages.SIM_OPCODES.GE_SIM_PLAY);
            
            return (ConvertToASWorkPlan(WorkPlan));
            
        }

        public MissionPlan StopMissionHandler(string missionId)
        {
            MissionPlan RetVal = m_ActiveMissions[missionId];

            if (RetVal != null)
                m_ActiveMissions.Remove(missionId);

            return RetVal;
        }

        public void AbortMissionHandler(string missionId)
        {
            m_Scenario.ReceiveSimCommand(Logic.GEMessages.SIM_OPCODES.GE_SIM_ABORT);
            m_ActiveMissions.Remove(missionId);
        }

        public MissionPlan UpdateMissionWorldHandler(string missionId, World world)
        {
            return m_ActiveMissions[missionId];
        }

        /// <summary>
        /// Convert GEO Polygon to UTM polygon
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        private List<Common.Entities.Point2d> ConvertPolygonToUTM(PoligonGeoJson polygon)
        {
            List<Common.Entities.Point2d> LogicPolygon = new List<Common.Entities.Point2d>();
            GeoConverter geoConverter = new GeoConverter();
            double lat, lng;

            foreach (List<Position> LinearRings in polygon.geometry.coordinates)
            {
                foreach (Position pos in LinearRings)
                {
// ### Edit by DF 22/10/15:      
                    //lat = pos[0];
                    //lng = pos[1];
                    lat = pos[1];
                    lng = pos[0];

                    Common.Entities.Point2d LogicPoint = new Common.Entities.Point2d();

                    string OutHemisphere;
                    geoConverter.GeoToUTM(lat, lng, GeoConverter.EnumMapDatum.WGS_84, out LogicPoint.East, out LogicPoint.North, out LogicPoint.Zone, out OutHemisphere);
                    LogicPoint.Hemisphere = (OutHemisphere == "N") ? 0 : 1;
                    LogicPolygon.Add(LogicPoint);
                }
            }
            return LogicPolygon;
        }

        /// <summary>
        /// Conver AppServer.Mission to Logic.Mission
        /// </summary>
        /// <param name="pASMission"></param>
        private Common.Entities.CMission ConvertToLogicMission(Mission pASMission)
        {
            double lng, lat;
            string TmpHemis ="N";


            Common.Entities.CMission mission = m_Scenario.Mission;
// ### Fix at 19/11/15 - reset mission
            mission.Launchers.Clear();
            mission.RecoverySites.Clear();
            mission.MissionArea.GwInList.Clear();
            mission.MissionArea.GwOutList.Clear();

            
            GeoConverter geoConverter = new GeoConverter();
            mission.Id = pASMission.id;

            // Mission Config
            mission.Config.MaxActivePlatforms = (int)pASMission.config.maxActivePlatforms;
// ### Temp 19/11/2015:
            //mission.Config.MissionDuration = (int)pASMission.config.duration;
            mission.Config.MissionDuration = (int)pASMission.config.duration / 1000;
            //mission.Config.MissionDuration = 600; // 10 min
            
            // Convert Launcher object
            foreach (Launcher ASLauncher in pASMission.launchers)
            {
                Common.Entities.CLauncher launcher = new Common.Entities.CLauncher(ASLauncher.id);
                launcher.Id = ASLauncher.id;
                launcher.ASL = ASLauncher.alt;                
                // ### Edit by DF 22/10/15                
                //geoConverter.GeoToUTM (ASLauncher.position[0], ASLauncher.position[1], GeoConverter.EnumMapDatum.WGS_84, out launcher.Position.East, out launcher.Position.North, out launcher.Position.Zone, out TmpHemis);
                geoConverter.GeoToUTM(ASLauncher.position[1], ASLauncher.position[0], GeoConverter.EnumMapDatum.WGS_84, out launcher.Position.East, out launcher.Position.North, out launcher.Position.Zone, out TmpHemis);
                if (TmpHemis == "N")
                    launcher.Position.Hemisphere = 0;
                else
                    launcher.Position.Hemisphere = 1;
                launcher.TimeToPrepare = ASLauncher.cleanupTime;
                mission.Launchers.Add(launcher);
            }

            // Convert RecoveryPoint objects
            foreach (RecoveryPoint ASRecovery in pASMission.recoveryPoints)
            {
                Common.Entities.CRecoverySite recovery = new Common.Entities.CRecoverySite();
                recovery.ASL = ASRecovery.alt;
                recovery.Id = (int)ASRecovery.id;
                
                // ### Edit by DF 22/10/15                    
                lng = ASRecovery.position[0];
                lat = ASRecovery.position[1];
                geoConverter.GeoToUTM(lat,
                                            lng,
                                            GeoConverter.EnumMapDatum.WGS_84,
                                            out recovery.Position.East,
                                            out recovery.Position.North,
                                            out recovery.Position.Zone,
                                            out TmpHemis);
                recovery.Position.Hemisphere = TmpHemis == "N" ? 0 : 1;
                recovery.TimeToPrepare = (int)ASRecovery.cleanupTime;
                mission.RecoverySites.Add(recovery);
            }

            foreach (Gateway ASgwIn in pASMission.area.inGateways)
            {
                Common.Entities.CGateway gwIn = new Common.Entities.CGateway();
                gwIn.Id = ASgwIn.id;                
                gwIn.MinHeight = ASgwIn.minAlt;
                gwIn.MaxHeight = ASgwIn.maxAlt;
                lng = ASgwIn.start[0];
                lat = ASgwIn.start[1];
                geoConverter.GeoToUTM(lat,
                                            lng,
                                            GeoConverter.EnumMapDatum.WGS_84,
                                            out gwIn.Start.East,
                                            out gwIn.Start.North,
                                            out gwIn.Start.Zone,
                                            out TmpHemis);
                gwIn.Start.Hemisphere = TmpHemis == "N" ? 0 : 1;
                lng = ASgwIn.end[0];
                lat = ASgwIn.end[1];
                geoConverter.GeoToUTM(lat,
                                            lng,
                                            GeoConverter.EnumMapDatum.WGS_84,
                                            out gwIn.End.East,
                                            out gwIn.End.North,
                                            out gwIn.End.Zone,
                                            out TmpHemis);
                gwIn.End.Hemisphere = TmpHemis == "N" ? 0 : 1;
                mission.MissionArea.GwInList.Add(gwIn);            
            }

            foreach (Gateway ASgwOut in pASMission.area.outGateways)
            {
                Common.Entities.CGateway gwOut = new Common.Entities.CGateway();
                gwOut.Id = ASgwOut.id;
                gwOut.MinHeight = ASgwOut.minAlt;
                gwOut.MaxHeight = ASgwOut.maxAlt;
                lng = ASgwOut.start[0];
                lat = ASgwOut.start[1];
                geoConverter.GeoToUTM(lat,
                                            lng,
                                            GeoConverter.EnumMapDatum.WGS_84,
                                            out gwOut.Start.East,
                                            out gwOut.Start.North,
                                            out gwOut.Start.Zone,
                                            out TmpHemis);
                gwOut.Start.Hemisphere = TmpHemis == "N" ? 0 : 1;
                lng = ASgwOut.end[0];
                lat = ASgwOut.end[1];
                geoConverter.GeoToUTM(lat,
                                            lng,
                                            GeoConverter.EnumMapDatum.WGS_84,
                                            out gwOut.End.East,
                                            out gwOut.End.North,
                                            out gwOut.End.Zone,
                                            out TmpHemis);
                gwOut.End.Hemisphere = TmpHemis == "N" ? 0 : 1;
                mission.MissionArea.GwOutList.Add(gwOut);
            }

            return mission;
        }

        /// <summary>
        /// Convert Logic.Workplan to AppServer.MissionPlan
        /// </summary>                
        private MissionPlan ConvertToASWorkPlan(List<Common.Entities.CRoute> missionPlan)
        {
            MissionPlan ASWorkPlan = new MissionPlan();
            GeoConverter geoConverter = new GeoConverter();
            Dictionary<int, Common.Entities.IPlatform> Platforms = m_Scenario.Platforms;

            ASWorkPlan.platforms = new Platform[missionPlan.Count];
            ASWorkPlan.missionId = m_Scenario.Mission.Id.ToString();

            int CurrentPlatform = 0;
            foreach (Common.Entities.CRoute route in missionPlan)
            {
                if (route.AssignTo >= 1)
                {
                    Common.Entities.IPlatform LogicPlatform = Platforms[route.AssignTo];
                    ASWorkPlan.platforms[CurrentPlatform] = new Platform();
                    ASWorkPlan.platforms[CurrentPlatform].id = route.AssignTo;

                    // *************** Platform State ******************
                    ASWorkPlan.platforms[CurrentPlatform].state = new PlatformState()
                    {
                        alt = (long)LogicPlatform.State.ASL,
                        deployState = LogicPlatform.State.DeployState.ToString(),
                        energy = LogicPlatform.State.Energy,
                        failureState = LogicPlatform.State.FailureState.ToString(),
                        //missionState = LogicPlatform.State.MissionState.ToString()
                        missionState = "Active"
                    };

                    // Copy Position
                    double PosLat, PosLng;

                    geoConverter.UTMToGeo(LogicPlatform.State.Position.East, LogicPlatform.State.Position.North, LogicPlatform.State.Position.Zone, (LogicPlatform.State.Position.Hemisphere == 0 ? "N" : "S"), out PosLat, out PosLng);
                    Position PlatformPos = new Position();
                    // ### Edit by DF 22/10/15:
                    PlatformPos.Add(PosLng);
                    PlatformPos.Add(PosLat);
                    //PlatformPos.Add (PosLat);
                    //PlatformPos.Add (PosLng);

                    ASWorkPlan.platforms[CurrentPlatform].state.position = PlatformPos;

                    // Copy Velocity
                    Velocity PlatformVelocity = new Velocity();
                    PlatformVelocity.Add(LogicPlatform.State.Velocity.x);
                    PlatformVelocity.Add(LogicPlatform.State.Velocity.y);
                    PlatformVelocity.Add(LogicPlatform.State.Velocity.z);

                    //ASWorkPlan.platforms[CurrentPlatform].sensors
                    ASWorkPlan.platforms[CurrentPlatform].state.velocity = PlatformVelocity;

                    // *************** Platform Route ******************
                    ASWorkPlan.platforms[CurrentPlatform].route = new Route();
                    foreach (Common.Entities.Waypoint LogicWaypoint in LogicPlatform.Route)
                    {
                        Waypoint PlatfromWaypoint = new Waypoint()
                        {
                            id = LogicWaypoint.Id,
                            actions = null,
                            alt = LogicWaypoint.m_ASL
                        };

                        // Copy Position
                        geoConverter.UTMToGeo(LogicWaypoint.Position.East, LogicWaypoint.Position.North, LogicWaypoint.Position.Zone, (LogicWaypoint.Position.Hemisphere == 0 ? "N" : "S"), out PosLat, out PosLng);
                        PlatformPos = new Position();
                        // ### Edit by DF 22/10/15:                        
                        PlatformPos.Add(PosLng);
                        PlatformPos.Add(PosLat);
                        //PlatformPos.Add(PosLat);
                        //PlatformPos.Add(PosLng);
                        PlatfromWaypoint.position = PlatformPos;

                        ASWorkPlan.platforms[CurrentPlatform].route.Add(PlatfromWaypoint);
                    }

                    // *************** Platform Sensors ******************
                    if (LogicPlatform.Sensors != null)
                    {
                        ASWorkPlan.platforms[CurrentPlatform].sensors = new Sensor[LogicPlatform.Sensors.Count];
                        int CurrentSensor = 0;
                        foreach (Common.Entities.ISensor LogicSensor in LogicPlatform.Sensors)
                        {
                            ASWorkPlan.platforms[CurrentPlatform].sensors[CurrentSensor] = new Sensor()
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

                    // *************** Next platform ******************
                    CurrentPlatform++;
                }
            }

// ### Removed at 26/10/15 by DF:
           // while (m_ActiveMissions.ContainsKey(ASWorkPlan.missionId) == true)
           //     ASWorkPlan.missionId = (int.Parse(ASWorkPlan.missionId) + 1).ToString();

            m_ActiveMissions.Add(ASWorkPlan.missionId, ASWorkPlan);
            return ASWorkPlan;
        }
       

        
    }
}
