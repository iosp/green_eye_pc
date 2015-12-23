#pragma once

#include <map>
#include "IGroundStation.h"
#include "../Common/Src/PlatformIF.h"
#include "GEMessages.h"

#include "FMInfra.h"
#include "GEConstants.h"


#include <Windows.h>
#include <process.h>



class FleetController
{
private:
	UDPChannel	*m_LogicChannel;		
	HANDLE		m_hThread;
	int			m_LastRxMsgNum;
	int			m_LastTxMsgNum;
	std::map<int, IGroundStation*> m_GroundStations;

	void Reset();
// Com. with LOGIC:	
	bool ConnectToLogic();
	
	void RcvThread();
	static unsigned __stdcall static_thread (void *args)
	{
		static_cast<FleetController*>(args)->RcvThread();
		return 0;
	}
	
	bool ValidateMsg (GEHeaderMsg* RxMsg);
	void RcvNewRoute (char* buf);
	void RcvSimCmd (char* buf);
	void RcvPing (char* buf);

// Com. with GroundStation
	bool ConnectToPlatform (IPlatform* platform);	

	bool SendCmdTo (IPlatform* platform, Waypoint wp);	
	bool SendBroadcast ();
			

public:
	FleetController(void);
	~FleetController(void);	

	bool Init ();
// Manage fleet
	bool CreateFleet(int a_GroupSize);
	bool AddPlatform(int id);

// Messages to platforms	
	bool SendCmdTo (int PlatformId, Waypoint wp);
	bool SendRouteTo (int PlatformId, Waypoint* a_route, int a_Length);
	bool SendSimulatorCommand(int a_SimCommand);	
	bool SetMode (int PlatformId, PlatformMode mode);
// Get last report status of platform 
	void SendStatusToLogic();
	bool GetStatusFrom (int PlatformId, IPlatform_State* pState);


};

