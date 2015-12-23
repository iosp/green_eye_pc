using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Utility;

namespace Logic.Algorithm
{
    public class StaticDeployAlgo
    {
        const int TIME_TO_WAIT = 30;

        public StaticDeployAlgo()
        {          
        }
                        
        /// <summary>
        /// Calculate deployment of polygon
        /// </summary>
        public List<Point2d> CalcDeploy (List<Point2d> pPolygon, double pProjectionLength)
        {
            List<Point2d> m_Deployment = new List<Point2d>();

            if (pProjectionLength == 0)
                return m_Deployment;
            if (pPolygon.Count() <= 2)
                return m_Deployment;

            // Get rectangle that block the polygon
            Rect rect = CalculateRect(pPolygon);

            // Cut the polygon to regions
            m_Deployment = SplitRect(rect, pProjectionLength);

            // Find which regions are inside polygon
            m_Deployment = GetMinDeployment(pPolygon, m_Deployment);

            //foreach (Point2d pos in m_Deployment)
            int iter = 0;
            for (iter = 0; iter < m_Deployment.Count; iter++)            
            {
                Point2d pos = m_Deployment[iter];
                pos.Zone = pPolygon[0].Zone;
                pos.Hemisphere = pPolygon[0].Hemisphere;
                m_Deployment[iter] = pos;
            }
            return m_Deployment;
        }        

         
        
        /// <summary>
        /// Calcaulte blocking rectangle of the polygon
        /// </summary>
        /// <param name="Polygon"></param>
        /// <returns></returns>
        private Rect CalculateRect (List<Point2d> Polygon)
        {
	        double		MinX,
			        	MinY,
				        MaxX,
				        MaxY;
	        Rect		rect;

            rect = new Rect();

	        rect.UpperLeft.North = rect.UpperLeft.East = -1;
	        rect.BottomRight.North = rect.BottomRight.East = -1;

            if (Polygon.Count() > 0)
            {
                MinX = MaxX = Polygon[0].North;
                MinY = MaxY = Polygon[0].East;
            }
            else 
                return rect;

            int Zone = Polygon[0].Zone; 
            int Hemis = Polygon[0].Hemisphere;

	        for (int i = 1; i < (int)Polygon.Count(); i++)
	        {
		        MinX = Math.Min(MinX, Polygon[i].North);
                MinY = Math.Min(MinY, Polygon[i].East);
                MaxX = Math.Max(MaxX, Polygon[i].North);
                MaxY = Math.Max(MaxY, Polygon[i].East);
                Zone = Polygon[i].Zone;
                Hemis = Polygon[i].Hemisphere;
	        }

	        rect.UpperLeft.North = MinX;
	        rect.UpperLeft.East = MinY;
            rect.UpperLeft.Zone = Zone;
            rect.UpperLeft.Hemisphere = Hemis;
            rect.BottomRight.North = MaxX;
	        rect.BottomRight.East = MaxY;
            rect.BottomRight.Zone = Zone;
            rect.BottomRight.Hemisphere = Hemis;

	        return rect;
        }

        /// <summary>
        /// Split Rectangle to even regions
        /// </summary>
        /// <param name="MissionRect"></param>
        /// <returns></returns>
        private List<Point2d> SplitRect(Rect MissionRect, double pProjectionLength)
        {
	        List<Point2d>		DeployCenters;
	        Point2d				StartPoint;

            DeployCenters = new List<Point2d>();
            StartPoint = new Point2d();

            StartPoint.North = MissionRect.UpperLeft.North + pProjectionLength / 2.0;
            StartPoint.East = MissionRect.UpperLeft.East + pProjectionLength / 2.0;
            StartPoint.Zone = MissionRect.UpperLeft.Zone;
            StartPoint.Hemisphere = MissionRect.UpperLeft.Hemisphere;
    
	        while (StartPoint.North < MissionRect.BottomRight.North)
	        {
		        while (StartPoint.East < MissionRect.BottomRight.East)
		        {
			        DeployCenters.Add(StartPoint);
                    StartPoint.East += pProjectionLength;
		        }
                StartPoint.East = MissionRect.UpperLeft.East + pProjectionLength / 2.0;
                StartPoint.North += pProjectionLength;
                StartPoint.Zone = MissionRect.UpperLeft.Zone;
                StartPoint.Hemisphere = MissionRect.UpperLeft.Hemisphere; 
	        }
	        return DeployCenters;
        }

        	        
        /// <summary>
        /// Check if center of region is inside polygon
        /// </summary>
        /// <param name="Polygon"></param>
        /// <param name="Centers"></param>
        /// <returns>Only center of regions that inside the polygon</returns>
        private List<Point2d> GetMinDeployment(List<Point2d> Polygon, List<Point2d> Centers)
        {
            List<Point2d>   MinCenters;
	        double			IsLeft,
					        IsLeftTemp;
	        bool			IsInside;

            MinCenters = new List<Point2d>();

	        for (int i=0; i < Centers.Count(); i++)
	        {
		        IsInside = true;
		        IsLeft = MathHelper.GetSideOfPoint (Polygon[0], Polygon[1], Centers[i]);
		        if (IsLeft == 0)
		        {
			        IsInside = true;
		        }
		        else
		        {
			        for (int j = 1; j < (int)Polygon.Count; j++)
			        {
				        if (j == (int)Polygon.Count() - 1)
                            IsLeftTemp = MathHelper.GetSideOfPoint(Polygon[j], Polygon[0], Centers[i]);
				        else
                            IsLeftTemp = MathHelper.GetSideOfPoint(Polygon[j], Polygon[j + 1], Centers[i]);
				        if (IsLeftTemp == 0)
				        {
					        IsInside = true;
					        break;
				        }
				        if (Math.Sign (IsLeft ) != Math.Sign(IsLeftTemp))
				        {
					        IsInside = false;
					        break;
				        }			    
                    }
		        }
		        if (IsInside)
			        MinCenters.Add(Centers[i]);	    
            }
            return MinCenters;   
        }

        

    }
}
