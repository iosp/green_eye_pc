#ifndef _ENTITYIF_H
#define _ENTITYIF_H

#include <vector.h>
#include "TopologyIF.h"

using namespace std;


typedef struct
{
	int			Id;
	Point2d		Position;
	double		ASL;
	double		Direction;
	int			TimeToLoad;
	vector<int>	PlatformsIds;
} Launcher;






#endif