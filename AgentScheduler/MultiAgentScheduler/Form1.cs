using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Entities;

namespace MultiAgentScheduler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CWorkPlan workplan = CreateWorkplan(-1, 1, 0);
            m_Scheduler.SetWorkPlan(workplan);

            workplan = CreateWorkplan(1, 2, 25);
            m_Scheduler.SetWorkPlan(workplan);

            workplan = CreateWorkplan(1, 3, 50);
            m_Scheduler.SetWorkPlan(workplan);
  
        }

        private CWorkPlan CreateWorkplan(int pID, int pAssignTo, int pLaunchTime)
        {
            CWorkPlan Workplan = new CWorkPlan(pID, pAssignTo);
            Workplan.LaunchTime = pLaunchTime;
            Workplan.Route = CreateSimpleRoute();
            
            return Workplan;
        }

        private CRoute CreateSimpleRoute()
        {            
            CRoute route = new CRoute();
            Waypoint wp;
            
            wp = new Waypoint();
            wp.Position = new Point2d(0, 0);
            wp.m_ASL = 0;
            wp.m_HoldDuration = 0;
            wp.m_IsHoldState = false;
            wp.m_LegType = LEG_TYPE.LEG_TYPE_TO;
            wp.Velocity = 1;
            route.m_WPList.Add(wp);

            wp = new Waypoint();
            wp.Position = new Point2d(50, 50);
            wp.m_ASL = 10;            
            wp.m_IsHoldState = false;
            wp.m_HoldDuration = 0;
            wp.m_LegType = LEG_TYPE.LEG_TYPE_MISSION;
            wp.Velocity = 1;
            route.m_WPList.Add(wp);

            wp = new Waypoint();
            wp.Position = new Point2d(100, 100);
            wp.m_ASL = 10;
            wp.m_IsHoldState = true;
            wp.m_HoldDuration = 30;
            wp.m_LegType = LEG_TYPE.LEG_TYPE_MISSION;
            wp.Velocity = 1;
            route.m_WPList.Add(wp);

            wp = new Waypoint();
            wp.Position = new Point2d(150, 150);
            wp.m_ASL = 10;
            wp.m_HoldDuration = 0;
            wp.m_IsHoldState = false;
            wp.m_LegType = LEG_TYPE.LEG_TYPE_FROM;
            wp.Velocity = 1;
            route.m_WPList.Add(wp);

            wp = new Waypoint();
            wp.Position = new Point2d(0, 0);
            wp.m_ASL = 10;
            wp.m_HoldDuration = 0;
            wp.m_IsHoldState = false;
            wp.m_LegType = LEG_TYPE.LEG_TYPE_FROM;
            wp.Velocity = 1;
            route.m_WPList.Add(wp);

            return route;
        }
    }
}
