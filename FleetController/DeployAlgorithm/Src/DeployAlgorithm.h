#pragma once

#include "TopologyIF.h"
#include "MissionIF.h"



class CDeployAlgorithm
{
private:
	double	m_ProjectionLength;
	Area*	m_MissionArea;

	Rect CalculateRect (vector<Point2d> Polygon);

	vector<Point2d> SplitRect(Rect rect);

	vector<Point2d> GetMinDeployment(vector<Point2d> Polygon, vector<Point2d> Centers);

public:
	CDeployAlgorithm(void);
	~CDeployAlgorithm(void);

	void SetParameters(Area* MissionArea, double ProjectionLength);

	vector<Point2d> Run();

	
	
	
};

