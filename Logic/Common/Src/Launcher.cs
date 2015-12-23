using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public class CLauncher
    {
        public int Id;
        public Point2d Position;
        public double ASL;
        public double TimeToPrepare;

        private List<int> m_PlatformsList;

        public CLauncher(int a_Id)
        {
            Id = a_Id;
            Position = new Point2d(0, 0);
            ASL = 0;
            TimeToPrepare = 20;
            m_PlatformsList = new List<int>();
        }

        public int GetPlatformIndex(int pPlatformId)
        {
            int index = m_PlatformsList.IndexOf(pPlatformId);
            if (index == -1)
            {
                index = m_PlatformsList.Count;
                m_PlatformsList.Add(pPlatformId);
            }
            return index;
        }

        public double GetLaunchTime(int pPlatformId)
        {
            double TimeToHold = 0;
            int index = GetPlatformIndex(pPlatformId);

            TimeToHold = index * TimeToPrepare;

            return TimeToHold;
        }

        /// <summary>
        /// Add new platform to launcher list.
        /// </summary>        
        public Point2d GetLandingLocation(int pPlatformId)
        {
            Point2d LandPos = Position;
            int index = GetPlatformIndex(pPlatformId);

            LandPos.East += index -5;

            return LandPos;
        }

        public bool RemovePlatform(int pPlatformId)
        {
            return true;
        }
    }
}
