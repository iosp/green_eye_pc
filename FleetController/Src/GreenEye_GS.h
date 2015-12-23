#pragma once


#include "IGroundStation.h"
#include "../Common/Src/PlatformIF.h"
#include "GEConstants.h"
#include "GEMessages.h"

#include "FMInfra.h"
#include <Windows.h>
#include <process.h>

class GreenEye_GS : public IGroundStation
{
private:
	UDPChannel	*m_Channel;	
	HANDLE		m_hThread;

	int			m_LastRxMsgNum;
	int			m_LastTxMsgNum;

	bool		m_IsConnected;	
	
	bool ValidateMsg (GEHeaderMsg* RxMsg);
	void RcvAckMsg (char* buf);
	void RcvStatus (char* buf);
	void RcvThread();

	static unsigned __stdcall static_thread (void *args)
	{
		static_cast<GreenEye_GS*>(args)->RcvThread();
		return 0;
	}
	
	GEHeaderMsg CreateHeader (int Opcode, int MsgLength);

public:
	GreenEye_GS(int m_Id);
	GreenEye_GS(int m_Id, IPlatform *pPlatform);
	~GreenEye_GS();

	bool Reset();
	bool SendWayPoint(Waypoint wp);
	bool SendStatusReqMsg ();
	bool SendSimCommandMsg (int SimCmd);

	bool SendRoute (Waypoint* a_route, int a_Length);

	bool SendMode (PlatformMode mode);

    IPlatform_State* GetState() { return m_LastState;};


protected:
	bool SetCommunication ();
	

};

