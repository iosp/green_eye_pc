using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities
{
    public class CRecoverySite
    {
        public int Id;
        public Point2d Position;
        public double ASL;
        public int State;
        public int TimeToPrepare;

        private List<int> m_ExpectedPlatforms;

        public CRecoverySite()
        {
            Position = new Point2d();
            TimeToPrepare = 2;
            m_ExpectedPlatforms = new List<int>();
        }

        public int GetLaunchIndex(int pPlatformId)
        {
            int index = m_ExpectedPlatforms.IndexOf(pPlatformId);
            if (index < 0)
            {
                index = m_ExpectedPlatforms.Count;
                m_ExpectedPlatforms.Add(pPlatformId);
            }
            return index;
        }

        /// <summary>
        /// Add new platform to landing list. Each platform will receive different position to land to
        /// </summary>        
        public Point2d GetLandingLocation(int pPlatformId)
        {
            Point2d LandPos = Position;
            int index = GetLaunchIndex(pPlatformId);
            
            LandPos.East += index;

            return LandPos;
        }
        
        public double GetWaitingTime(int pPlatformId)
        {
            double HoldTime = 0;
            int index = GetLaunchIndex(pPlatformId);
            
            HoldTime += index * TimeToPrepare;

            return HoldTime;
        }

        /// <summary>
        /// Clear landing list
        /// </summary>
        public void ClearLanding()
        {
            m_ExpectedPlatforms.Clear();
        }


        
    }
}
