#include "DeployAlgorithm.h"
#include "MathHelper.h"

#include <math.h>

CDeployAlgorithm::CDeployAlgorithm(void)
{
	m_MissionArea = 0;
	m_ProjectionLength = 0;
}


CDeployAlgorithm::~CDeployAlgorithm(void)
{
}

void CDeployAlgorithm::SetParameters(Area* MissionArea, double ProjectionLength)
{
	m_MissionArea = MissionArea;
	m_ProjectionLength = ProjectionLength;
}

Rect CDeployAlgorithm::CalculateRect (vector<Point2d> Polygon)
{
	double		MinX,
				MinY,
				MaxX,
				MaxY;
	Rect		rect;

	rect.UpperLeft.North = rect.UpperLeft.East = -1;
	rect.BottomRight.North = rect.BottomRight.East = -1;

	if (Polygon.size() > 0)
	{
		MinX = MaxX = Polygon[0].North;
		MinY = MaxY = Polygon[0].East;
	}

	for (int i = 1; i < (int)Polygon.size(); i++)
	{
		MinX = min(MinX, Polygon[i].North);
		MinY = min(MinY, Polygon[i].East);
		MaxX = max(MaxX, Polygon[i].North);
		MaxY = max(MaxY, Polygon[i].East);
	}

	rect.UpperLeft.North = MinX;
	rect.UpperLeft.East = MinY;
	rect.BottomRight.North = MaxX;
	rect.BottomRight.East = MaxY;

	return rect;
}


vector<Point2d> CDeployAlgorithm::SplitRect(Rect MissionRect)
{
	vector<Point2d>		DeployCenters;
	Point2d				StartPoint;

	StartPoint.North = MissionRect.UpperLeft.North + m_ProjectionLength / 2.0;
	StartPoint.East = MissionRect.UpperLeft.East + m_ProjectionLength / 2.0;

	while (StartPoint.North < MissionRect.BottomRight.North)
	{
		while (StartPoint.East < MissionRect.BottomRight.East)
		{
			DeployCenters.push_back(StartPoint);
			StartPoint.East += m_ProjectionLength;
		}
		StartPoint.East = MissionRect.UpperLeft.East + m_ProjectionLength / 2.0;
		StartPoint.North += m_ProjectionLength;
	}

	return DeployCenters;
}

/*
	Check if center of region is inside polygon
*/
vector<Point2d> CDeployAlgorithm::GetMinDeployment(vector<Point2d> Polygon, vector<Point2d> Centers)
{
	vector<Point2d> MinCenters;
	double			IsLeft,
					IsLeftTemp;
	bool			IsInside;

	for (int i=0; i < (int)Centers.size(); i++)
	{
		IsInside = true;
		IsLeft = GetSideOfPoint (Polygon[0], Polygon[1], Centers[i]);
		if (IsLeft == 0)
		{
			IsInside = true;
		}
		else
		{
			for (int j = 1; j < (int)Polygon.size(); j++)
			{
				if (j == (int)Polygon.size() - 1)
					IsLeftTemp = GetSideOfPoint (Polygon[j], Polygon[0], Centers[i]);
				else
					IsLeftTemp = GetSideOfPoint (Polygon[j], Polygon[j+1], Centers[i]);
				if (IsLeftTemp == 0)
				{
					IsInside = true;
					break;
				}
				if (abs(IsLeft)/IsLeft != abs(IsLeftTemp)/IsLeftTemp)
				{
					IsInside = false;
					break;
				}
			}
		}
		if (IsInside)
			MinCenters.push_back(Centers[i]);
	}

	return MinCenters;
}

vector<Point2d> CDeployAlgorithm::Run()
{
	vector<Point2d> Deployment;
	if (m_ProjectionLength == 0 || m_MissionArea == 0)
		return Deployment;
	if (m_MissionArea->Polygon.size() <= 2)
		return Deployment;

	// Get rectangle that block the polygon
	Rect rect = CalculateRect (m_MissionArea->Polygon);

	// Cut the polygon to regions
	Deployment = SplitRect (rect);

	// Find which regions are inside polygon
	Deployment = GetMinDeployment (m_MissionArea->Polygon, Deployment);


	return Deployment;
}
