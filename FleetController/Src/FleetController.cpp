#include "FleetController.h"
#include "GreenEye_GS.h"


FleetController::FleetController()
{	
}


FleetController::~FleetController()
{
	// Remove all communication with platforms
	for (map<int, IGroundStation*>::iterator it = m_GroundStations.begin(); it != m_GroundStations.end(); ++it)
	{
		delete (it->second);
	}
	m_GroundStations.erase(m_GroundStations.begin(), m_GroundStations.end());
	// Remove commmunication with LOGIC
	if (m_hThread != 0)
		CloseHandle(m_hThread);
	if (m_LogicChannel != 0)
	{				
		delete (m_LogicChannel);
	}
	m_LogicChannel = 0;
	m_hThread = 0;
}

bool FleetController::Init()
{
	bool status;
	Reset();
	status = ConnectToLogic();
	return status;
}

void FleetController::Reset()
{
	// Reset communication with LOGIC
	m_LogicChannel = 0;
	m_hThread = 0;
	m_LastRxMsgNum = 0;
	m_LastTxMsgNum = 0;	
}

/*
**********************************************************************
							Communication with LOGIC
************************************************************************
*/
bool FleetController::ConnectToLogic ()
{
	int		TxPort,
			RxPort;

	if (m_hThread != 0)
		CloseHandle(m_hThread);
	if (m_LogicChannel != 0)
	{
		delete (m_LogicChannel);	
	}
	
	m_LogicChannel = new UDPChannel(false);	
	m_LogicChannel->SetAddress(TO_LOGIC_PORT, LOGIC_IP, FROM_LOGIC_PORT, FLEET_CONTROLLER_IP, 0);

	printf ("Create communication with LOGIC ip=%s \n", LOGIC_IP);

	m_LogicChannel->SendBuf("Hello", 5);
		
	m_hThread = (HANDLE)_beginthreadex(NULL, 0, &FleetController::static_thread, this, 0, 0);

	m_LastRxMsgNum = -1;
	m_LastTxMsgNum = -1;

	return true;
}

void FleetController::SendStatusToLogic()
{
	// run over all GS and read last state
	// send it to Logic
	IPlatform_State* state;

	for (map<int, IGroundStation*>::iterator it = m_GroundStations.begin(); it != m_GroundStations.end(); ++it)
	{
		state = it->second->GetState();
		if (state == 0)
			continue;
		/*
		double value =  55 + 100 * it->first;
		state.SetASL(value);
		Point2d point;
		point.East = value;
		point.North = -value;
		state.SetPosition(point);
		*/

		GEStatusMsg msg;
	
		msg.Header.StartBytes = GE_START_BYTES;
		msg.Header.MsgId = m_LastTxMsgNum++;	
		msg.Header.Src = it->first;
		msg.Header.Dst = 0;
		msg.Header.MsgLength = sizeof(GEStatusMsg);
		msg.Header.Opcode = GE_OP_STATUS;
// ### Update 21/10/15 - Copy the state struct		
		msg.State = *state;
		//msg.State.SetASL(state->GetASL());
		//msg.State.SetPosition(state->GetPosition());

		m_LogicChannel->SendBuf(&msg, sizeof(GEStatusMsg));		
	}
}

/*
	Receive packet from LOGIC
*/
void FleetController::RcvThread()
{
	int			BytesRecv;
	char		buf [BUFFER_LENGTH];
		
	

	while (m_hThread != 0)
	{
		if (m_LogicChannel != 0)
			BytesRecv = m_LogicChannel->RcvBuffer(buf, BUFFER_LENGTH);
		if (BytesRecv > 0)
		{					
			GEHeaderMsg* msg = (GEHeaderMsg*) &buf[0];
			if (ValidateMsg(msg))
			{
				m_LastRxMsgNum = msg->MsgId;
				// Decode message
				switch (msg->Opcode)
				{
				case GE_OP_CMD_ROUTE:
					RcvNewRoute(&buf[0]);
					break;
				case GE_OP_SIM_CMD:
					RcvSimCmd(&buf[0]);
					break;	
				case GE_OP_PING:
					RcvPing(&buf[0]);
					printf ("Receive Ping from LOGIC \n");
					break;					
				}

			}
		}
	}
	_endthreadex(0);
}

bool FleetController::ValidateMsg (GEHeaderMsg* RxMsg)
{
	int ErrorCode = -1;

	if (RxMsg->StartBytes != GE_START_BYTES)	
		ErrorCode = 1;	
	else
		if (RxMsg->MsgLength > BUFFER_LENGTH)		
			ErrorCode = 2;		
		else
			if (RxMsg->MsgId < m_LastRxMsgNum)			
				ErrorCode = 3;										

	switch (ErrorCode)
	{
	case -1:
		return true;
	case 1:
		printf ("RcvMessage(), Start bytes error");
		break;
	case 2:
		printf ("RcvMessage(), Message length error");
		break;
	case 3:
		printf ("RcvMessage(), Receive older message");
		return true;
		break;
	}

	return false;
}

void FleetController::RcvNewRoute (char* buf)
{
	GERouteMsg* msg;
	msg = (GERouteMsg*) buf;
	SendRouteTo(msg->Header.Dst, msg->WP, msg->NumWayPoints);
	printf ("Send new route to %d \n", msg->Header.Dst);
	Waypoint* wp;
	for (int i = 0; i < msg->NumWayPoints; i++)
	{
		wp =  &msg->WP[i];
		printf ("WP%d=(%f,%f,%f) Hold=%d \n", i, wp->Position.East, wp->Position.North, wp->ASL, wp->TimeToWait);
	}
}

void FleetController::RcvSimCmd (char* buf)
{
	GESimCmdMsg* msg;
	GE_Sim_Opcodes simCmd;
	msg = (GESimCmdMsg*) buf;
	
	simCmd = msg->SimCommand;
	switch (simCmd)
	{
	case GE_SIM_RESET:
		m_LastRxMsgNum = 0;
		m_LastTxMsgNum = 0;	
		break;
	}

	SendSimulatorCommand((int)simCmd);
}

void FleetController::RcvPing (char* buf)
{
	GEPingMsg* msg;
	msg = (GEPingMsg*) buf;

}

/*
**********************************************************************
							Communication with Ground station
************************************************************************
/*
	Create n GroundStations to communicate with the platforms
*/
bool FleetController::CreateFleet(int a_GroupSize)
{
	
	IPlatform* platform;
	for (int id = 0; id <= a_GroupSize; id++)
	{
		platform = new IPlatform();
		if (platform->SetId(id))
		{
			if (ConnectToPlatform (platform))
			{
				printf ("Create Comm with platform %d\n", platform->GetId());
			}
		}
	}
	return true;
}

/*
	Add new platform
*/
bool FleetController::AddPlatform(int id)
{
// #Temp - should read from AppServer and create GS for each platform	
	// platforms = AppServer->GetPlatforms();
	// foreach (platform)
	// CreateComm (platform)

	IPlatform* platform = new IPlatform();
	if (platform->SetId(id))
		ConnectToPlatform (platform);
	
	return true;
}

bool FleetController::ConnectToPlatform (IPlatform *platform)
{
	int			PlatformId;
					
	PlatformId = platform->GetId();
	// Check if there is already GS for request platform
	if (m_GroundStations.count(PlatformId) > 0)
	{		
		return false;
	}

	m_GroundStations[PlatformId] = new GreenEye_GS(PlatformId);
	m_GroundStations[PlatformId]->SetPlatform (platform);


	return true;
}

bool FleetController::SendCmdTo (IPlatform* platform, Waypoint wp)
{
	int					PlatformId;
						
	PlatformId = platform->GetId();

	return SendCmdTo(PlatformId, wp);				
}

bool FleetController::SendCmdTo (int PlatformId, Waypoint wp)
{
	IGroundStation		*GS;

	// Check if there is a GS for request platform
	if (m_GroundStations.count(PlatformId) == 0)
	{
		return false;
	}

	GS = m_GroundStations[PlatformId];
	GS->SendWayPoint(wp);
	return true;
}

bool FleetController::SendRouteTo (int PlatformId, Waypoint* a_route, int a_Length)
{
	IGroundStation		*GS;
	if (m_GroundStations.count(PlatformId) == 0)
	{
		return false;
	}

	GS = m_GroundStations[PlatformId];
	GS->SendRoute(a_route, a_Length);
	return true;
}

bool FleetController::SendSimulatorCommand(int a_SimCommand)
{
	for (map<int, IGroundStation*>::iterator it = m_GroundStations.begin(); it != m_GroundStations.end(); ++it)
	{
		it->second->SendSimCommandMsg(a_SimCommand);
	}

	return true;
}


bool FleetController::SetMode (int PlatformId, PlatformMode mode)
{
	IGroundStation		*GS;
	if (m_GroundStations.count(PlatformId) == 0)
	{
		return false;
	}

	GS = m_GroundStations[PlatformId];
	GS->SendMode(mode);
	return true;
}

bool FleetController::GetStatusFrom (int PlatformId, IPlatform_State* pState)
{
	IGroundStation		*GS;
	IPlatform_State		state;
	if (m_GroundStations.count(PlatformId) == 0)
	{
		return false;
	}

	GS = m_GroundStations[PlatformId];
	pState = GS->GetState();
	//pState = state;

	return true;
}

