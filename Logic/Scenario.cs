using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Logic.Algorithm;
using System.Windows.Forms;
using Common.Utility;
using Common.Entities;

namespace Logic
{
    public class Scenario
    {
        private CMission m_Mission;      

        private List<CTarget> m_Targets;

        private List<CRoute> m_WorkPlan;

        private Dictionary<int, IPlatform> m_Platforms;        

        private GECom m_FleetCom;

        //private Point2d m_UTMOffset;   

        public Scenario(GECom a_GECom)
        {
            m_FleetCom = a_GECom;                                
            m_Platforms = new Dictionary<int, IPlatform>();
            m_Mission = new CMission();
            
//            m_UTMOffset = new Point2d(0,0);
            m_WorkPlan = new List<CRoute>();
            m_Targets = new List<CTarget>();
            
            m_FleetCom.ReceiveStateMsgEvent += new GECom.ReceiveStateMsg(ReceiveState);
            //m_FleetCom.SendSimCommand(GEMessages.SIM_OPCODES.GE_SIM_RESET);                                    
        }              

        public Area MissionArea
        {
            set 
            { 
                m_Mission.MissionArea = value;
                ReCalcWorkplan();
            }
        }

        public List<Point2d> UTMPolygon
        {
            set 
            { 
                m_Mission.MissionArea.Polygon = value;
                if (m_Mission.Launchers.Count == 0 && value.Count > 0)
                {
                    FixPlatformsPosition(m_Mission.Config.UTMOffset, value[0]);
                    m_Mission.Config.UTMOffset = value[0];
                }
                ReCalcWorkplan();
            }

            get
            {
                return m_Mission.MissionArea.Polygon;
            }
        }

        public MissionConfig Config
        {
            set 
            { 
                m_Mission.Config = value;
                ReCalcWorkplan();                
            }
            get { return m_Mission.Config; }
        }

        public Dictionary<int, IPlatform> Platforms
        {
            get { return m_Platforms; }
        }

        public CMission Mission
        {
            get { return m_Mission; }
            set
            {
                m_Mission = value;
                if (m_Mission.Launchers.Count > 0)
                {
                    m_Mission.Config.UTMOffset = m_Mission.Launchers[0].Position;                    
                }
                ReCalcWorkplan();
                //UpdateWorkPlan();
            }
        }


        public List<CTarget> Targets
        {
            get { return m_Targets; }
        }

        public List<CRoute> WorkPlan
        {
            get { return m_WorkPlan; }
        }

        /// <summary>
        /// Receive state from World Server
        /// </summary>        
        private void ReceiveState(GEMessages.GEStatusMsg a_StateMsg)
        {
            int platformId = a_StateMsg.Header.Src;
            PlatformState state = a_StateMsg.State;
            ReceiveStatus(platformId, state);
        }

        static bool IsAppServerException = false;

        /// <summary>
        /// Receive status from platform
        /// </summary>        
        private void ReceiveStatus(int a_PlatformId, PlatformState a_State)
        {            
            IPlatform platform;
            if (m_Platforms.ContainsKey(a_PlatformId))
            {
                platform = m_Platforms[a_PlatformId];
                platform.State = a_State;
            }
            else
            {
                platform = new IPlatform(a_PlatformId);
                m_Platforms.Add(a_PlatformId, platform);
            }

            platform.State = a_State;
            platform.State.Position.East += m_Mission.Config.UTMOffset.East;
            platform.State.Position.North +=  m_Mission.Config.UTMOffset.North;
            platform.State.Position.Zone = m_Mission.Config.UTMOffset.Zone;
            platform.State.Position.Hemisphere = m_Mission.Config.UTMOffset.Hemisphere;

            try
            {
                if (!IsAppServerException)
                    GreenEyeAPI.BL.SampleWorldBLImplementation.Instance().UpdateAppServer(platform, m_Mission.Id);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                IsAppServerException = true;
            }
        }

        /// <summary>
        /// Correct platform position by new offset
        /// </summary>
        public void FixPlatformsPosition(Point2d pOldOffset, Point2d pNewOffset)
        {
            foreach (IPlatform platform in m_Platforms.Values)
            {
                platform.State.Position.East += pNewOffset.East - pOldOffset.East;
                platform.State.Position.North += pNewOffset.North - pOldOffset.North;
                platform.State.Position.Zone = pNewOffset.Zone;
                platform.State.Position.Hemisphere = pNewOffset.Hemisphere;
            }
        }



        /// <summary>
        /// Receive command from simulator
        /// </summary>        
        public void ReceiveSimCommand(GEMessages.SIM_OPCODES a_SimCmd)
        {
            m_FleetCom.SendSimCommand(a_SimCmd);

            switch (a_SimCmd)
            {
                case GEMessages.SIM_OPCODES.GE_SIM_EXIT:
                    break;
                case GEMessages.SIM_OPCODES.GE_SIM_RESET:                    
                    m_Platforms.Clear();
                    //m_WorkPlan.Clear();
                    //m_Targets.Clear();
                    ReCalcWorkplan();
                    m_FleetCom.SendSimCommand(a_SimCmd);
                    break;
                case GEMessages.SIM_OPCODES.GE_SIM_STOP:
                    break;
                case GEMessages.SIM_OPCODES.GE_SIM_PLAY:
                    {                       
                       if (m_WorkPlan.Count == 0)
                            UpdateWorkPlan();
                       m_FleetCom.SendSimCommand(a_SimCmd);
                        break;
                    }
                case GEMessages.SIM_OPCODES.GE_SIM_ABORT:
                    AbortMission();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 1. Clear workplan and active routes
        /// 2. Create a new list of optional targets 
        /// </summary>        
        public List<CTarget> ReCalcWorkplan()
        {
            m_Targets.Clear();
            foreach (IPlatform platform in m_Platforms.Values)
            {
                platform.Target.AssignTo = -1;
                platform.Route.Clear();
            }
            m_WorkPlan.Clear();

            Algorithm.StaticDeployAlgo DeployAlgo = new StaticDeployAlgo();
            List<Point2d> DeployPoints = DeployAlgo.CalcDeploy(m_Mission.MissionArea.Polygon, m_Mission.Config.ProjectionLength);
            foreach (Point2d Pos in DeployPoints)
            {
                CTarget target = new CTarget();
                target.Position = Pos;
                m_Targets.Add(target);
            }
            return m_Targets;
        }

        /// <summary>
        /// Create a list of active routes
        /// </summary>        
        private List<CRoute> GetMissionPlan()       
        {
            m_WorkPlan.Clear();
            foreach (IPlatform platform in m_Platforms.Values)
            {
                if (platform.Route.Count > 0)
                {
                    CRoute route = new CRoute();
                    route.m_WPList = platform.Route;
                    route.AssignTo = platform.Id;
                    m_WorkPlan.Add(route);
                }
            }            
            return m_WorkPlan;
        }

        /// <summary>
        /// Cancel mission by sending all active platforms back home
        /// </summary>
        private void AbortMission()
        {
            ReturnHomeAlgo algo = new ReturnHomeAlgo();
            IPlatform platform;
            m_Targets.Clear();
            foreach (CRecoverySite site in m_Mission.RecoverySites)
                site.ClearLanding();

            m_WorkPlan = algo.CreateWorkplan(m_Platforms.Values.ToList(), m_Mission);
            foreach (CRoute route in m_WorkPlan)
            {
                if (route.AssignTo > 0)
                {
                    platform = m_Platforms[route.AssignTo];
                    platform.Route = route.m_WPList;
                    if (m_FleetCom != null)
                        m_FleetCom.SendRouteToPlatform(platform.Id, route.m_WPList, m_Mission.Config.UTMOffset);
                }
            }
            GetMissionPlan();
        }

        /// <summary>
        /// Assign free platforms for each unhandled target
        /// </summary>
        public List<CRoute> UpdateWorkPlan()
        {
            bool IsRemarkFlag = false;
            if (m_Platforms.Count == 0)
            {
                //MessageBox.Show("No Platforms");
                return m_WorkPlan;
            }            
           
            // Order targets from the closest to the exit point to the farest (exit point = gateway out/recovery site)
            Point2d RefPoint = new Point2d(0,0);
            bool IsOrderNeeded = false;
            if (m_Mission.MissionArea.GwOutList.Count > 0)
            {
                RefPoint = m_Mission.MissionArea.GwOutList[0].Start;
                IsOrderNeeded = true;
            }
            else
                if (m_Mission.RecoverySites.Count > 0)
                {
                    RefPoint = m_Mission.RecoverySites[0].Position;
                    IsOrderNeeded = true;
                }
            if (IsOrderNeeded)
                m_Targets = m_Targets.OrderBy(o => (MathHelper.GetDistance(o.Position.East,
                                                                            o.Position.North,
                                                                            RefPoint.East,
                                                                            RefPoint.North))).ToList();
            // Assing platform for each target
            int iter = 0;
            
            foreach (CTarget target in m_Targets)
            {
                if (target.AssignTo == -1)
                {
                    Waypoint DeployPoint = new Waypoint();
                    DeployPoint.m_ASL = m_Mission.Config.MissionHeight;                    
                    DeployPoint.m_IsHoldState = true;
                    DeployPoint.m_HoldDuration = m_Mission.Config.MissionDuration + iter * m_Mission.Config.EndMissionTimeSpacing;
                    DeployPoint.m_LegType = LEG_TYPE.LEG_TYPE_MISSION;
                    DeployPoint.Position = target.Position;
                    iter++;

                    if (!AssignTarget(DeployPoint, target) && !IsRemarkFlag)
                    {
                        //MessageBox.Show("Not enough platforms to cover targets");
                        IsRemarkFlag = true;
                    }
                    
                }
            }
            
            return GetMissionPlan();
        }

        /// <summary>
        /// Search for an available platform and assign it to a target
        /// </summary>
        /// <param name="pTarget"></param>
        private bool AssignTarget(Waypoint pDeployWP, CTarget pTarget)
        {                                
            if (m_Mission.RecoverySites.Count > 0)
            {
                CRecoverySite LandingSite = m_Mission.RecoverySites[0];
                Waypoint LandWP = new Waypoint();                                

                foreach (IPlatform platform in m_Platforms.Values)
                {
                    if (platform.Route.Count == 0)
                    {
                        LandWP.Position = LandingSite.GetLandingLocation(platform.Id);
                        LandWP.m_ASL = LandingSite.ASL + 0.5;

                        int TimeToStart = (int)(LandingSite.GetLaunchIndex(platform.Id) * m_Mission.Config.LaunchDelay);

                        platform.Route = Algorithm.RouteGenerator.CreateRoute(platform.State.Position, m_Mission, pDeployWP, TimeToStart, LandWP);
                        platform.Target = pTarget;
                        pTarget.AssignTo = platform.Id;
                        if (m_FleetCom != null)
                            m_FleetCom.SendRouteToPlatform(platform.Id, platform.Route, m_Mission.Config.UTMOffset);
                        return true;                        
                    }
                }                
            }
            return false;
        }

        /// <summary>
        /// Create route for each online platform
        /// </summary>
        /*
        private void CreateWorkPlan()
        {
            const int MAX_PLATFORMS = 10;
            const int LAUNCH_PERIOD = 5;

            m_WorkPlan = new List<Route>();
            Point2d endPoint = new Point2d();
            Algorithm.StaticDeployAlgo DeployAlgo = new StaticDeployAlgo();                            

            for (int i = 0; i < Math.Min(MAX_PLATFORMS, m_Targets.Count); i++)
            {
                endPoint.North = m_Mission.RecoverySites[0].Position.North + i;
                endPoint.East = m_Mission.RecoverySites[0].Position.East + i;
                Route route = new Route();
                route.m_Route = DeployAlgo.CreateRoute(m_Mission, m_Targets[i].Position, i * LAUNCH_PERIOD, endPoint);
                route.AssignTo = -1;
                m_WorkPlan.Add(route);                
            }

            AssignRoutes();
        }

        /// <summary>
        /// Assign a platform for each route
        /// </summary>
        private void AssignRoutes()
        {
            Route route;            
            IPlatform Platform;            
            int NumPlatforms = Math.Min(m_Platforms.Count, m_WorkPlan.Count);
            
            // run over work plan and assign to platforms
            for (int i = 0; i < NumPlatforms; i++)
            {
                Platform = (IPlatform)m_Platforms.Values.ElementAt(i);// .ElementAt<int>(i);
                route = m_WorkPlan[i];
                Platform.Route = route.m_Route;
                route.AssignTo = Platform.Id;
                if (m_FleetCom != null)
                    m_FleetCom.SendRouteToPlatform(Platform.Id, route.m_Route);
            }
        }
         */

        

    }
}
