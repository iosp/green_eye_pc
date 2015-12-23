// TestApp.cpp : Defines the entry point for the console application.
//

//#include "stdafx.h"

#include <vector>
#include <process.h>
#include <functional>
#include <conio.h>


#include "TopologyIF.h"
//#include "DeployAlgorithm.h"
#include "FleetController.h"


using namespace std;

FleetController *fleet;

void PrintStartScreen ()
{
	printf("****************************************************\n");
	printf("'c' for clear, 'q' for quit, 's' for test status \n");
	printf("****************************************************\n");
}

void timer_start(std::function<void(void)> func)
{
	char key = ' ';
	
	while (key != 'q')	
	{
		if (kbhit() != 0)
			key = _getch();				
		switch (key)
		{
		case 'c':
			{
				system("cls");
				PrintStartScreen();
				break;
			}
		case 's':
			{
				IPlatform_State state;				
				fleet->SendStatusToLogic();
				
				break;
			}
		
		case 'q':
			{
				printf ("Quit Program \n");
				break;					
			}

		}
		key = ' ';

		Sleep(1000);
		func();

		//testCh->SendBuf((void*)&msg, msg.Header.MsgLength);
	}
	
}



void RefreshStatus()
{
	fleet->SendStatusToLogic();
}

/*
	Test Fleet Controller - 
	Control n platforms and receive status from them
*/
void StartFleetController(int pFleetSize)
{
	char		key;
				
	PrintStartScreen();
	fleet = new FleetController();
	fleet->Init();
	
	fleet->CreateFleet(pFleetSize);

	timer_start(RefreshStatus);
		
	



	delete(fleet);

}

//int _tmain(int argc, char* argv[])
int main(int argc, char* argv[])
{
	int FleetSize = 10;
	if (argc > 1)
		FleetSize = atoi(argv[1]);
	if (FleetSize > MAX_FLEET_SIZE || FleetSize < 1)
		FleetSize = 10;
	StartFleetController(FleetSize);

	return 0;
}

