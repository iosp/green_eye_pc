using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;

namespace Logic.Algorithm
{
    public class RouteGenerator
    {
        const int DEFAULT_ZONE = 36;
        const int DEFAULT_HEMI = 0;

        public RouteGenerator()
        {
        }

        /// <summary>
        /// Create Route for depoly position
        /// </summary>
        public static List<Waypoint> CreateRoute(Point2d pCurrentPos, CMission pMission, Waypoint pDeployWP, int aStartAfter, Waypoint pLandingWP)
        {
            List<Waypoint> Route = new List<Waypoint>();            
            MissionConfig pConfig = pMission.Config;
            
            // WP0 - wait for launch
            Waypoint waypoint = new Waypoint();            
            waypoint.m_ASL = GEConstants.KEEP_LAST_VALUE;            
            waypoint.Position = pCurrentPos;            
            waypoint.m_IsHoldState = true;
            waypoint.m_HoldDuration = aStartAfter;
            waypoint.m_LegType = LEG_TYPE.LEG_TYPE_TO;
            Route.Add(waypoint);                        
            // WP1 - raise to fly height
            waypoint = new Waypoint();            
            waypoint.m_ASL = pConfig.FlightHeight;            
            waypoint.Position = pCurrentPos;            
            waypoint.m_IsHoldState = false;
            waypoint.m_HoldDuration = 0;
            waypoint.m_LegType = LEG_TYPE.LEG_TYPE_TO;
            Route.Add(waypoint);
            // WP2 - move to gateway if available
            if (pMission.MissionArea.GwInList.Count > 0)
            {
                waypoint = new Waypoint();
                CGateway GatewayIn = pMission.MissionArea.GwInList[0];
                waypoint.m_ASL = pConfig.FlightHeight;
                waypoint.Position = GatewayIn.Start;
                waypoint.m_IsHoldState = false;
                waypoint.m_HoldDuration = 0;
                waypoint.m_LegType = LEG_TYPE.LEG_TYPE_TO;                
                Route.Add(waypoint);
            }
            // WP3 - move to deploy position    
            waypoint = new Waypoint(pDeployWP);
            waypoint.m_ASL = pConfig.FlightHeight;            
            waypoint.m_IsHoldState = false;
            waypoint.m_HoldDuration = 0;
            waypoint.m_LegType = LEG_TYPE.LEG_TYPE_MISSION;            
            Route.Add(waypoint);
            // WP4 - change to mission height                                           
            waypoint = new Waypoint(pDeployWP);
            waypoint.m_LegType = LEG_TYPE.LEG_TYPE_MISSION;   
            Route.Add(waypoint);
            // WP5 - change to transport height
            waypoint = new Waypoint();
            waypoint.m_ASL = pConfig.FlightHeight;
            waypoint.Position = pDeployWP.Position;            
            waypoint.m_IsHoldState = false;
            waypoint.m_HoldDuration = 0;
            waypoint.m_LegType = LEG_TYPE.LEG_TYPE_FROM;            
            Route.Add(waypoint);
            // WP6 - fly to exit gateway if available
            if (pMission.MissionArea.GwOutList.Count > 0)
            {
                waypoint = new Waypoint();
                CGateway GwOut = pMission.MissionArea.GwOutList[0];
                waypoint.m_ASL = pConfig.FlightHeight;
                waypoint.Position = GwOut.Start;
                waypoint.m_IsHoldState = false;
                waypoint.m_HoldDuration = 0;
                waypoint.m_LegType = LEG_TYPE.LEG_TYPE_FROM;                
                Route.Add(waypoint);
            }
            // WP7 - return home
            waypoint = new Waypoint(pLandingWP);
            waypoint.m_ASL = pConfig.FlightHeight;            
            waypoint.m_IsHoldState = false;
            waypoint.m_HoldDuration = 0;
            waypoint.m_LegType = LEG_TYPE.LEG_TYPE_FROM;            
            Route.Add(waypoint);
            // WP8 - Landing
            waypoint = new Waypoint(pLandingWP);
            waypoint.m_ASL = pLandingWP.m_ASL;                     
            waypoint.m_IsHoldState = false;
            waypoint.m_HoldDuration = 0;
            waypoint.m_LegType = LEG_TYPE.LEG_TYPE_FROM;
            Route.Add(waypoint);
            return Route;
        }    
    }
}
