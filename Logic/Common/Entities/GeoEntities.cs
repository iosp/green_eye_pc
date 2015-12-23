using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Common.Entities
{
    /// <summary>
    /// 3d point
    /// </summary>
    public struct double3d
    {
	    public double		x;
        public double y;
        public double z;
    };

    /// <summary>
    /// UTM point
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Point2d
    {    
	    public double	North;
	    public double	East;
	    public int		Zone;
	    public int		Hemisphere;

        public Point2d(double aNorth, double aEast)
        {
            North = aNorth;
            East = aEast;
            Zone = 36;
            Hemisphere = 0;
        }

        public Point2d(double aNorth, double aEast, int aZone, int aHemisphere)
        {
            North = aNorth;
            East = aEast;
            Zone = aZone;
            Hemisphere = aHemisphere;

        }
    } 

    /// <summary>
    /// Rectangle define in UTM Cord.
    /// </summary>
    public class Rect
    {
        public Point2d		UpperLeft;
	    public Point2d		BottomRight;

        public Rect()
        {
            UpperLeft = new Point2d();
            BottomRight = new Point2d();
        }
    }

    

    /// <summary>
    /// Mission Area contains polygion and gateways
    /// </summary>
    public class Area
    {
        private List<Point2d> m_Polygon;
        public List<CGateway> GwInList;
        public List<CGateway> GwOutList;
        public List<CGateway> GwEmergencyList;

        public Area()
        {
            m_Polygon = new List<Point2d>();
            GwInList = new List<CGateway>();
            GwOutList = new List<CGateway>();
            GwEmergencyList = new List<CGateway>();
        }

        public List<Point2d> Polygon
        {
            get
            {
                return m_Polygon;
            }

            set
            {
                m_Polygon = value;
            }


        }
    }



      

    

    

   
    
    
}



