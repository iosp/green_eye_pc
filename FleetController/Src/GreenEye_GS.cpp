#include "GreenEye_GS.h"
//#include "../../Common/GEMessages.h"

#include <WinBase.h>



GreenEye_GS::GreenEye_GS(int m_Id) : IGroundStation(m_Id)
{
	Reset();
	m_LastState = new IPlatform_State();
}

GreenEye_GS::GreenEye_GS(int m_Id, IPlatform *pPlatform) : IGroundStation(m_Id)
{
	Reset();
}

GreenEye_GS::~GreenEye_GS()
{
	delete (m_LastState);
	if (m_hThread != 0)
		CloseHandle(m_hThread);
	if (m_Channel != 0)
	{				
		delete (m_Channel);
	}
	m_IsConnected = false;
	m_Channel = 0;
	m_hThread = 0;
}

bool GreenEye_GS::Reset()
{
	m_Channel = 0;
	m_hThread = 0;
	m_IsConnected = false;
	m_LastRxMsgNum = 0;
	m_LastTxMsgNum = 0;

	return true;
}


bool GreenEye_GS::SetCommunication ()
{
	int		TxPort,
			RxPort;

	if (m_hThread != 0)
		CloseHandle(m_hThread);
	if (m_Channel != 0)
	{
		delete (m_Channel);	
	}

	TxPort = UDP_TX_BASE_PORT + m_pPlatform->GetId();
	RxPort = UDP_RX_BASE_PORT + m_pPlatform->GetId();

	m_Channel = new UDPChannel(false);	
	m_Channel->SetAddress(TxPort, PLATFORM_IP, RxPort, FLEET_CONTROLLER_IP, 0);

	m_Channel->SendBuf("test", 4);
	
	//_beginthread(&GreenEye_GS::static_thread, 0, this);
	m_hThread = (HANDLE)_beginthreadex(NULL, 0, &GreenEye_GS::static_thread, this, 0, 0);
	m_IsConnected = true;
	m_LastRxMsgNum = -1;
	m_LastTxMsgNum = -1;

	return true;
}

//////////////////////////////////////////////////////////////
//															//
//					Send Functions							//
//															//
//////////////////////////////////////////////////////////////
GEHeaderMsg GreenEye_GS::CreateHeader (int Opcode, int MsgLength)
{
	GEHeaderMsg header;
	header.StartBytes = GE_START_BYTES;
	header.MsgId = m_LastTxMsgNum++;
	header.Src = -1;
	header.Dst = m_pPlatform->GetId();
	header.MsgLength = MsgLength;
	header.Opcode = Opcode;

	return header;
}

bool GreenEye_GS::SendWayPoint(Waypoint wp)
{
	GECommandMsg msg;
	msg.Header.StartBytes = GE_START_BYTES;
	msg.Header.MsgId = m_LastTxMsgNum++;	
	msg.Header.Src = -1;
	msg.Header.Dst = m_pPlatform->GetId();
	msg.Header.MsgLength = sizeof(GECommandMsg);
	msg.Header.Opcode = GE_OP_CMD_SET_WP;

	msg.WP = wp;	

	m_Channel->SendBuf(&msg, sizeof(GECommandMsg));

	return true;
}

bool GreenEye_GS::SendRoute (Waypoint* a_route, int a_Length)
{
	GERouteMsg msg;
	msg.Header = CreateHeader(GE_OP_CMD_ROUTE, sizeof(msg));
	msg.NumWayPoints = a_Length;
	memcpy(&msg.WP[0], a_route, a_Length * sizeof(Waypoint));
	//msg.WP = a_route;

	m_Channel->SendBuf((char*)&msg, sizeof(GERouteMsg));
	return true;
}

bool GreenEye_GS::SendStatusReqMsg ()
{
	GEReqStatusMsg msg;
	
	msg.Header.StartBytes = GE_START_BYTES;
	msg.Header.MsgId = m_LastTxMsgNum++;	
	msg.Header.Src = -1;
	msg.Header.Dst = m_pPlatform->GetId();
	msg.Header.MsgLength = sizeof(GEReqStatusMsg);
	msg.Header.Opcode = GE_OP_REQ_STATUS;

	m_Channel->SendBuf(&msg, sizeof(GEReqStatusMsg));

	return true;
}

bool GreenEye_GS::SendSimCommandMsg (int SimCmd)
{
	GESimCmdMsg msg;
	msg.Header.StartBytes = GE_START_BYTES;
	msg.Header.MsgId = m_LastTxMsgNum++;
	msg.Header.Src = -1;
	msg.Header.Dst = m_pPlatform->GetId();
	msg.Header.MsgLength = sizeof(GESimCmdMsg);
	msg.Header.Opcode = GE_OP_SIM_CMD;

	msg.SimCommand = (GE_Sim_Opcodes)SimCmd;

	m_Channel->SendBuf((char*)&msg, sizeof(GESimCmdMsg));

	return true;
}

bool GreenEye_GS::SendMode (PlatformMode mode)
{
	GEPlatformModeMsg msg;
	msg.Header.StartBytes = GE_START_BYTES;
	msg.Header.MsgId = m_LastTxMsgNum++;
	msg.Header.Src = -1;
	msg.Header.Dst = m_pPlatform->GetId();
	msg.Header.MsgLength = sizeof(GESimCmdMsg);
	msg.Header.Opcode = GE_OP_SET_MODE;

	msg.Mode = mode;

	m_Channel->SendBuf((char*)&msg, sizeof(GEPlatformModeMsg));


	return true;
}

//////////////////////////////////////////////////////////////
//															//
//					Receive Functions						//
//															//
//////////////////////////////////////////////////////////////
void GreenEye_GS::RcvThread()
{
	int			BytesRecv;
	char		buf [BUFFER_LENGTH];			

	while (m_hThread != 0 && m_IsConnected)
	{
		if (m_Channel != 0)
			BytesRecv = m_Channel->RcvBuffer(buf, BUFFER_LENGTH);
		if (BytesRecv > 0)
		{		
			GEHeaderMsg* msg = (GEHeaderMsg*) &buf[0];
			if (ValidateMsg(msg))
			{
				m_LastRxMsgNum = msg->MsgId;
				// Decode message
				switch (msg->Opcode)
				{
				case GE_OP_ACK:
					RcvAckMsg (&buf[0]);
					break;
				case GE_OP_STATUS:
					RcvStatus (&buf[0]);
					break;
				}

			}

		}
	}
	_endthreadex(0);	
}

bool GreenEye_GS::ValidateMsg (GEHeaderMsg* RxMsg)
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
			else 
				if (RxMsg->Dst != GS_ADDRESS)				
					ErrorCode = 4;				
				else
					if (RxMsg->Src != m_pPlatform->GetId())				
						ErrorCode = 5;				


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
// #TEMP:
		return true;
		printf ("RcvMessage(), Receive older message");
		break;
	case 4:
		printf ("RcvMessage(), Invalid Destination");
		break;
	case 5:
		printf ("RcvMessage(), Invalid Source address");
		break;

	}

	return false;
}

void GreenEye_GS::RcvAckMsg (char* buf)
{

}

void GreenEye_GS::RcvStatus (char* buf)
{
	GEStatusMsg*	msg;
	msg = (GEStatusMsg*) buf;	

// ### Update 21/10/15 - Copy whole state
	m_LastState = &msg->State;	
	//m_LastState->SetASL(msg->State.GetASL());
	//m_LastState->SetPosition(msg->State.GetPosition());

	m_IsStateUpdate = true;
		
}

