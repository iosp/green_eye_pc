using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;

using Common.Entities;


namespace MultiAgentScheduler
{
    public partial class SchedulerControl : UserControl
    {
        private Dictionary<int, CWorkPlan> m_WorkPlans;

        public SchedulerControl()
        {
            InitializeComponent();
            m_WorkPlans = new Dictionary<int, CWorkPlan>();            
        }

        /// <summary>
        /// Add/Update workplan
        /// </summary>
        /// <param name="pWorkPlan"></param>
        public void SetWorkPlan(CWorkPlan pWorkPlan)
        {
            int key = pWorkPlan.AssignTo;

            if (m_WorkPlans.ContainsKey(key))
                m_WorkPlans[key] = pWorkPlan;
            else
                m_WorkPlans.Add(key, pWorkPlan);
            RefreshGraph();
        }

        /// <summary>
        /// Check if mission hold in request policy, collisions and recovery points attributes
        /// </summary>
        public void ValidateMission()
        {
            ScheduleRecoveryPoints();
            ScheduleGWOut();
        }

        private void ScheduleRecoveryPoints()
        {
        }

        private void ScheduleGWOut()
        {
        }     


        /// <summary>
        /// Redraw graph for all register workplans
        /// </summary>
        private void RefreshGraph()
        {
            m_Graph.Series.Clear();            

            foreach (CWorkPlan WorkPlan in m_WorkPlans.Values)
            {
                DrawWorkPlan(WorkPlan);
            }
        }

        /// <summary>
        /// Draw the workplan as gantt chart
        /// </summary>
        /// <param name="pWorkPlan"></param>
        private void DrawWorkPlan(CWorkPlan pWorkPlan)
        {
            int PlatformId = pWorkPlan.AssignTo;
            List<CLegAnalysis> Analysis = pWorkPlan.Analysis;
            int CurrentTime = pWorkPlan.LaunchTime;
            

            Series gantt = new Series("platform" + PlatformId.ToString());
            gantt.ChartType = SeriesChartType.RangeBar;
            
            foreach (CLegAnalysis LegAnalysis in Analysis)
            {
                gantt.Points.Add(CreatePoint(PlatformId, CurrentTime, CurrentTime + LegAnalysis.TimeTravel, GetColor(LegAnalysis.LegType)));
                CurrentTime += LegAnalysis.TimeTravel;
                if (LegAnalysis.HoldTime > 0)
                {
                    gantt.Points.Add(CreatePoint(PlatformId, CurrentTime, CurrentTime + LegAnalysis.HoldTime, Color.Yellow));
                    CurrentTime += LegAnalysis.HoldTime;
                }
 
            }
            m_Graph.Series.Add(gantt);

        }

        private Color GetColor(LEG_TYPE pLegType)
        {
            switch (pLegType)
            {
                case LEG_TYPE.LEG_TYPE_NA:
                    return Color.Black;
                    
                case LEG_TYPE.LEG_TYPE_TO:
                    return Color.Blue;

                case LEG_TYPE.LEG_TYPE_MISSION:
                    return Color.Green;

                case LEG_TYPE.LEG_TYPE_FROM:
                    return Color.Red;

                default:
                    return Color.Black;
            }
        }

        /// <summary>
        /// Temp function, to delete
        /// </summary>
        private void TestGraph()
        {
            Series gantt = new Series("1");
            gantt.ChartType = SeriesChartType.RangeBar;
            
            gantt.Points.Add(CreatePoint(1, 2, 6, Color.Red));
            gantt.Points.Add(CreatePoint(1, 6, 15, Color.Green));
            gantt.Points.Add(CreatePoint(1, 15, 18, Color.Blue));            
            m_Graph.Series.Add(gantt);
            /*
            gantt = new Series("2");
            gantt.Points.Add(CreatePoint(2, 3, 7, Color.Red));
            gantt.Points.Add(CreatePoint(2, 7, 9, Color.Green));
            gantt.Points.Add(CreatePoint(2, 9, 11, Color.Blue));
            m_Graph.Series.Add(gantt);
            gantt = new Series("3");
            gantt.Points.Add(CreatePoint(3, 1, 4, Color.Red));
            gantt.Points.Add(CreatePoint(3, 4, 6, Color.Green));
            gantt.Points.Add(CreatePoint(3, 7, 9, Color.Blue));
            m_Graph.Series.Add(gantt);
             */
        }

        /// <summary>
        /// Temp function
        /// </summary>
        
        private DataPoint CreatePoint (int x, double y0, double y1, Color color)
        {
            DataPoint point = new DataPoint();
            point.Color = color;
            point.XValue = x;
            point.YValues = new double[2];
            point.YValues[0] = y0;
            point.YValues[1] = y1;
            return point;
        }
    }
}
