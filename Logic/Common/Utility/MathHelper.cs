using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;

namespace Common.Utility
{
    public class MathHelper
    {
        // Check if Anypoint is left (>0), right (<0) or on (=0) a given vector
        public static double GetSideOfPoint (Point2d StartPoint, Point2d EndPoint, Point2d AnyPoint)
        {
            double VecMul = ((EndPoint.North - StartPoint.North) * (AnyPoint.East - StartPoint.East) -
			                (AnyPoint.North - StartPoint.North) * (EndPoint.East - StartPoint.East));
            return VecMul;
        }

        public static double GetDistance(Point2d From, Point2d To)
        {
            return GetDistance(From.North, From.East, To.North, To.East);
        }

        public static double GetDistance(double X0, double Y0, double X1, double Y1)
        {
            double Dist = 0;
            double DeltaX, DeltaY;

            DeltaX = X1 - X0;
            DeltaY = Y1 - Y0;

            Dist = Math.Sqrt(DeltaX * DeltaX + DeltaY * DeltaY);
            return Dist;
        }
    }
}
