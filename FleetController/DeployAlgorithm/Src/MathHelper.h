#pragma once

#include "TopologyIF.h"

// Check if Anypoint is left (>0), right (<0) or on (=0) a given vector
inline double GetSideOfPoint (Point2d StartPoint, Point2d EndPoint, Point2d AnyPoint)
{
	return ((EndPoint.North - StartPoint.North) * (AnyPoint.East - StartPoint.East) -
			(AnyPoint.North - StartPoint.North) * (EndPoint.East - StartPoint.East));	
}