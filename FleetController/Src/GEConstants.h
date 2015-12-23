#pragma once


#define		LAB_COMPUTER			1

#if (LAB_COMPUTER)
//#define		PLATFORM_IP				"172.23.1.134"
#define		PLATFORM_IP				"172.23.1.136"
#define		FLEET_CONTROLLER_IP		"172.23.1.138"
#define		LOGIC_IP				"172.23.1.138"
#else
#define		PLATFORM_IP				"169.254.49.135" //"172.23.1.134"
#define		FLEET_CONTROLLER_IP		"169.254.49.135" //"172.23.1.138"
#define		LOGIC_IP				"169.254.49.135" //"172.23.1.134"
#endif

#define		UDP_TX_BASE_PORT		5000		// platform port = BASE_PORT + PLATFORM_ID
#define		UDP_RX_BASE_PORT		6000
#define		TO_LOGIC_PORT		4000		// platform port = BASE_PORT + PLATFORM_ID
#define		FROM_LOGIC_PORT		4001

#define		BUFFER_LENGTH			1024
#define		GE_START_BYTES			0x5050
#define		GS_ADDRESS				-1

#define		MAX_FLEET_SIZE			50

