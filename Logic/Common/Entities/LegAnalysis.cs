using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;

namespace Common.Entities
{
    /// <summary>
    /// Analysis leg 
    /// </summary>
    public class CLegAnalysis
    {
        private double m_FuelConsume = 0;

        private int m_TravelTime = 0;

        private int m_HoldTime = 0;

        private LEG_TYPE m_LegType = LEG_TYPE.LEG_TYPE_NA;

        public CLegAnalysis()
        {
        }

        public double FuelConsume
        {
            get { return m_FuelConsume; }
        }

        public int TimeTravel
        {
            get { return m_TravelTime; }                
        }

        public int HoldTime
        {
            get { return m_HoldTime; }
        }

        public LEG_TYPE LegType
        {
            get { return m_LegType; }
        }
    
        /// <summary>
        /// Calculate leg parameters
        /// </summary>        
        public void SetLeg(Waypoint FromWp, Waypoint ToWp)
        {            
            m_TravelTime = AnalysisTravelTime(FromWp, ToWp);            
            if (ToWp.m_IsHoldState)
                m_HoldTime = ToWp.m_HoldDuration;
            m_LegType = FromWp.m_LegType;
            // m_FuelConsume = 
        }

        /// <summary>
        /// Calculate time to travel from one waypoint to another
        /// </summary>        
        private int AnalysisTravelTime(Waypoint FromWp, Waypoint ToWp)
        {
            int FlightTime = 0;                        
            double Velocity = ToWp.Velocity;
            double Distance = MathHelper.GetDistance(FromWp.Position, ToWp.Position);

            if (Velocity != 0)
                FlightTime = (int)Math.Abs(Distance / Velocity);
            else
                FlightTime = int.MaxValue;
            return FlightTime;            

        }
    }
}
