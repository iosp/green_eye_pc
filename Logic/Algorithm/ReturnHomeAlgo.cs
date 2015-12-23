using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using Common.Entities;

namespace Logic.Algorithm
{
    public class ReturnHomeAlgo
    {

        public ReturnHomeAlgo()
        {
        }

        /// <summary>
        /// Calculate route for each platform given GW, Home location and launcher time to recover
        /// </summary>        
        public List<CRoute> CreateWorkplan(List<IPlatform> pPlatforms, CMission pMission)
        {
            List<CRoute> Workplan = new List<CRoute>();
            List<IPlatform> ClosestPlatforms;

            if (pMission.RecoverySites.Count > 0)            
            {
                CRecoverySite RecoverySite = pMission.RecoverySites[0];                        
                // Sort the platforms from the closest to GwOut
                Waypoint RefPoint = new Waypoint();
                RefPoint.m_ASL = RecoverySite.ASL;
                RefPoint.Position = RecoverySite.Position;                
                
                if (pMission.MissionArea.GwOutList.Count > 0)
                {
                    CGateway GwOut = pMission.MissionArea.GwOutList[0];
                    RefPoint.m_ASL = GwOut.MaxHeight;
                    RefPoint.Position = GwOut.Start;                    
                }
                                         
                ClosestPlatforms = pPlatforms.OrderBy(o => (MathHelper.GetDistance(o.State.Position.East,
                                                                                o.State.Position.North,
                                                                                RefPoint.Position.East,
                                                                                RefPoint.Position.North))).ToList();                

                int iter = 0;
                Waypoint LandingWP = new Waypoint();

                foreach (IPlatform platform in ClosestPlatforms)
                {
                    if (platform.State.MissionState == MISSION_STATE.MISSION ||
                        platform.State.MissionState == MISSION_STATE.FROM ||
                        platform.State.MissionState == MISSION_STATE.TO)
                    {
                        CRoute route = new CRoute();

                        LandingWP.Position = RecoverySite.GetLandingLocation(platform.Id);
                        LandingWP.m_ASL = RecoverySite.ASL + 0.5;

                        double TimeToWait = iter * 5;
                        //LandingPosition.East += iter;
                        route.Id = 1;
                        route.m_WPList = CreateReturnHomeRoute(platform.State.Position, pMission.MissionArea, LandingWP, pMission.Config, TimeToWait);
                        route.AssignTo = platform.Id;

                        Workplan.Add(route);
                        iter++;
                    }
                }

            }

            return Workplan;
        }

        /// <summary>
        /// Create Route from current position to recoevry site througt GwOut (if available)
        /// </summary>
        /// <param name="pCurrentPos"></param>
        /// <param name="pArea"></param>
        /// <param name="pEndPos"></param>
        /// <param name="pConfig"></param>
        /// <param name="pStartAfter">Num of seconds to wait before start active this route</param>
        /// <returns></returns>
        public List<Waypoint> CreateReturnHomeRoute(Point2d pCurrentPos, Area pArea, Waypoint pEndWP, MissionConfig pConfig, double pStartAfter)
        {
            List<Waypoint> route = new List<Waypoint>();
            Waypoint waypoint = new Waypoint();
            Waypoint newWP;

            // WP0 - wait for launch
            waypoint.m_ASL = GEConstants.KEEP_LAST_VALUE;//  a_Mission.Config.TransportHeight;            
            waypoint.Position = pCurrentPos;            
            waypoint.m_IsHoldState = true;
            waypoint.m_HoldDuration = (int)pStartAfter;            
            waypoint.m_LegType = LEG_TYPE.LEG_TYPE_FROM;
            newWP = new Waypoint(waypoint);
            route.Add(newWP);
            // WP1 - Move to Fligt Height
            waypoint.m_ASL = pConfig.FlightHeight;
            waypoint.Position = pCurrentPos;                        
            waypoint.m_IsHoldState = false;
            waypoint.m_HoldDuration = 0;
            waypoint.m_LegType = LEG_TYPE.LEG_TYPE_FROM;
            newWP = new Waypoint(waypoint);
            route.Add(newWP);
            // WP2 - Move to GWOut
            if (pArea.GwOutList.Count > 0)
            {
                CGateway GwOut = pArea.GwOutList[0];
                waypoint.m_ASL = GwOut.MinHeight;
                waypoint.Position = GwOut.Start;
                waypoint.m_IsHoldState = false;
                waypoint.m_HoldDuration = 0;
                waypoint.m_LegType = LEG_TYPE.LEG_TYPE_FROM;
                newWP = new Waypoint(waypoint);
                route.Add(newWP);
            }            
            // WP3 - Move to Site
            waypoint.m_ASL = pEndWP.m_ASL;
            waypoint.Position = pEndWP.Position;            
            waypoint.m_IsHoldState = false;
            waypoint.m_HoldDuration = 0;
            waypoint.m_LegType = LEG_TYPE.LEG_TYPE_FROM;
            newWP = new Waypoint(waypoint);
            route.Add(newWP);
            // WP4 - Landing
            waypoint.m_ASL = pEndWP.m_ASL;
            waypoint.Position = pEndWP.Position;            
            waypoint.m_IsHoldState = false;
            waypoint.m_HoldDuration = 0;
            waypoint.m_LegType = LEG_TYPE.LEG_TYPE_FROM;
            newWP = new Waypoint(waypoint);
            route.Add(newWP);
            
            return route;
        }


    }
}
