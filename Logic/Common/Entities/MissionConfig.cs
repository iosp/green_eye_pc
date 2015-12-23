using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities
{
    /// <summary>
    /// Mission configuration
    /// </summary>
    public class MissionConfig
    {
        // Default values:
        const double DEFAULT_LAUNCH_DELAY = 10;
        const int DEFAULT_MISSION_DURATION = 120;
        const int END_MISSION_TIME_SPACING = 20;

        const int FLIGHT_HEIGHT = 10;
        const int MISSION_HEIGHT = 7;
        const double PROJECTION_LENGTH = 50;        
        
        public int MaxActivePlatforms;
        //DateTime	StartTime;
        //DateTime	EndTime;
        private double m_MissionHeight;
        private double m_FlightHeight;

        private double m_ProjectionLength;

        /// <summary>
        /// Time to leave mission (relative to start mission time) [sec]
        /// </summary>
        private int m_MissionDuration;

        private double m_LaunchDelay;

        public Point2d UTMOffset;

        public MissionConfig()
        {
            m_MissionHeight = MISSION_HEIGHT;
            m_FlightHeight = FLIGHT_HEIGHT;
            m_ProjectionLength = PROJECTION_LENGTH;
            m_LaunchDelay = DEFAULT_LAUNCH_DELAY;
            MissionDuration = DEFAULT_MISSION_DURATION;
            UTMOffset = new Point2d(3554244.79, 669953.72);

        }

        public MissionConfig(double pMissionHeight, double pFlightHeight)
        {
            m_MissionHeight = pMissionHeight;
            m_FlightHeight = pFlightHeight;
        }

        public double MissionHeight
        {
            get { return m_MissionHeight; }
            set { m_MissionHeight = value; }
        }

        public double FlightHeight
        {
            get { return m_FlightHeight; }
            set { m_FlightHeight = value; }
        }

        public double ProjectionLength
        {
            get { return m_ProjectionLength; }
            set { m_ProjectionLength = value; }
        }

        public double LaunchDelay
        {
            get { return m_LaunchDelay; }
            set { m_LaunchDelay = value; }
        }

        public int MissionDuration
        {
            get { return m_MissionDuration; }
            set { m_MissionDuration = value; }
        }

        public int EndMissionTimeSpacing
        {
            get { return END_MISSION_TIME_SPACING; }
        }


    };
}
