using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Common.Entities
{
    public enum LEG_TYPE {LEG_TYPE_NA = -1, LEG_TYPE_TO = 0, LEG_TYPE_MISSION, LEG_TYPE_FROM};

    /// <summary>
    /// Route that assigned to a specfic platform
    /// </summary>
    public class CWorkPlan
    {
        private CRoute m_Route;
        private int m_LaunchTime = 0;    // [sec]
        private int m_AssignTo = -1;
        private int m_Id = 0;

        /// <summary>
        /// analysis each leg in the route
        /// </summary>
        private List<CLegAnalysis> m_Analysis;

        public CWorkPlan(int pId, int pAssignTo)
        {
            m_Route = new CRoute();
            m_Id = pId;
            m_AssignTo = pAssignTo;
            m_LaunchTime = 0;
        }

        public int Id
        {
            get { return m_Id; }
        }

        public CRoute Route
        {
            get { return m_Route; }
            set 
            { 
                m_Route = value;
                m_Analysis = new List<CLegAnalysis>();
                if (m_Route.m_WPList.Count <= 1)
                    return;
                // analysis route                
                Waypoint FromWp = m_Route.m_WPList[0];
                Waypoint ToWp;
                CLegAnalysis LegAnalysis;
                for (int iter = 1; iter < m_Route.m_WPList.Count; iter++)
                {
                    ToWp = m_Route.m_WPList[iter];
                    LegAnalysis = new CLegAnalysis();
                    LegAnalysis.SetLeg(FromWp, ToWp);
                    m_Analysis.Add(LegAnalysis);
                    FromWp = ToWp;                    
                }


            }
        }

        public List<CLegAnalysis> Analysis
        {
            get { return m_Analysis; }
        }

        public int LaunchTime
        {
            get { return m_LaunchTime; }
            set
            {
                if (value >= 0)
                    m_LaunchTime = value;
                else
                    m_LaunchTime = 0;
            }
        }

        public int AssignTo
        {
            get { return m_AssignTo; }
            set
            {
                if (value > 0)
                    m_AssignTo = value;
                else
                    m_AssignTo = -1;
            }
        }
    }


    /// <summary>
    /// Command to the platform
    /// </summary>
    public class Waypoint
    {
        const double MIN_VELOCITY = 0.001;

        const double MAX_VELOCITY = 100;

        private int m_Id;

        private Point2d m_Position;
        //public List<IAction>    Actions;
        public double m_ASL;
        public bool m_IsHoldState;
        public int m_HoldDuration;
        public LEG_TYPE m_LegType;
        
        private double m_Velocity;

        public Waypoint(int pId = -1)
        {
            m_Id = pId;
            m_Position = new Point2d(-999, -999);
            m_ASL = -999;
            m_IsHoldState = false;
            m_LegType = LEG_TYPE.LEG_TYPE_NA;
            m_Velocity = MIN_VELOCITY;      
        }

        public Waypoint(Waypoint pSrc)
        {
            m_Position = pSrc.m_Position;
            m_ASL = pSrc.m_ASL;
            m_IsHoldState = pSrc.m_IsHoldState;
            m_HoldDuration = pSrc.m_HoldDuration;
            m_LegType = pSrc.m_LegType;
            m_Velocity = pSrc.m_Velocity;
        }

        public int Id
        {
            get { return m_Id; }
        }

        /// <summary>
        /// get/set velcoity of platform. Assign only non-negative value
        /// </summary>
        public double Velocity
        {
            get
            {
                return m_Velocity;
            }
            set
            {
                // Check if velocity between MIN_V to MAX_V
                m_Velocity = Math.Min(Math.Max(value, MIN_VELOCITY), MAX_VELOCITY);                
            }
        }

        public Point2d Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }
    };

    public class CRoute
    {
        public int Id;
        public List<Waypoint> m_WPList;
        public int AssignTo;

        public CRoute()
        {
            Id = -1;
            m_WPList = new List<Waypoint>();
            AssignTo = -1;
        }
    }
}
