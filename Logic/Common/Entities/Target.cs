using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities
{
    public class CTarget
    {
        Point2d m_Position;

        int m_AssignTo = -1;

        public CTarget()
        {
            m_AssignTo = -1;
        }

        public Point2d Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        public int AssignTo
        {
            get { return m_AssignTo; }
            set { m_AssignTo = value; }
        }


    }
}
