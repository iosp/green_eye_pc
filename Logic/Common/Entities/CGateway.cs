using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities
{
    /// <summary>
    /// Gateway to/from mission area
    /// </summary>
    public class CGateway
    {
        public int Id;
        public Point2d Start;
        public Point2d End;
        public double MinHeight;
        public double MaxHeight;
        public double Direction;

        public CGateway()
        {
            Start = new Point2d();
            End = new Point2d();
        }
    }
}
