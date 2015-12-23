using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using Common.Entities;

namespace Logic
{
    public class GECom
    {
        private bool m_IsActive;

        private UdpClient m_RxChannel;

        private IPEndPoint m_IPEndPoint;

        private UdpClient m_TxChannel;

        private Thread ReceiveThread;

        private int m_TxCounter, m_RxCounter;

        public delegate void ReceiveStateMsg(GEMessages.GEStatusMsg Msg);

        public event ReceiveStateMsg ReceiveStateMsgEvent;

        public GECom(string HostIp, int a_TxPort, int a_RxPort)
        {
            m_IsActive = true;
            Init(HostIp, a_TxPort, a_RxPort);
        }

        public void Close()
        {
            m_IsActive = false;
            m_RxChannel.Close();
        }

        private bool Init(string a_HostIp, int a_TxPort, int a_RxPort)
        {                  
            try
            {
                m_TxChannel = new UdpClient(a_HostIp, a_TxPort);
                m_RxChannel = new UdpClient(a_RxPort);//"192.168.0.8", RxPort);
                m_IPEndPoint = new IPEndPoint(IPAddress.Any, a_RxPort);
              
                ReceiveThread = new Thread(new ThreadStart(ReceiveFleetMessage));
                ReceiveThread.Start();
                m_TxCounter = 0;
                m_RxCounter = 0;

                // Send reset signal to World
                //SendSimCommand(GEMessages.SIM_OPCODES.GE_SIM_RESET);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine (ex.Message);
            }
            
            return false;        
        }

        /// <summary>
        /// Receive message from FleetController
        /// </summary>
        private void ReceiveFleetMessage()
        {
            IntPtr ptr = IntPtr.Zero;
            byte[] buffer;
            GEMessages.GEHeaderMsg header = new GEMessages.GEHeaderMsg();
            int objSize = Marshal.SizeOf(header);
            ptr = Marshal.AllocHGlobal(objSize);

            while (m_IsActive)
            {
                try
                {
                    buffer = m_RxChannel.Receive(ref m_IPEndPoint);
                    if (buffer.Length > 0)
                    {
                        DecodeBuffer(buffer);
                    }
                }
                catch (Exception ex)
                {
                    int d = 5;
                    
                }
            }
        }

        private bool DecodeBuffer(byte[] buffer)
        {
            IntPtr ptr = IntPtr.Zero;
            GEMessages.GEHeaderMsg header = new GEMessages.GEHeaderMsg();
            int objSize = Marshal.SizeOf(header);
            if (buffer.Length < objSize)
                return false;
            ptr = Marshal.AllocHGlobal(objSize);
            Marshal.Copy(buffer, 0, ptr, objSize);
            header = (GEMessages.GEHeaderMsg)Marshal.PtrToStructure(ptr, typeof(GEMessages.GEHeaderMsg));
            if (header.StartBytes == GEConstants.GE_START_BYTES)
            {
                switch ((GEMessages.GE_OPCODES)header.Opcode)
                {
                    case GEMessages.GE_OPCODES.GE_OP_ACK:
                        {
                            GEMessages.GEAckMsg ackMsg = new GEMessages.GEAckMsg();
                            objSize = Marshal.SizeOf(ackMsg);
                            ptr = Marshal.AllocHGlobal(objSize);
                            Marshal.Copy(buffer, 0, ptr, objSize);
                            ackMsg = (GEMessages.GEAckMsg)Marshal.PtrToStructure(ptr, typeof(GEMessages.GEAckMsg));
                            ReceiveAck(ackMsg);
                            break;
                        }
                    case GEMessages.GE_OPCODES.GE_OP_STATUS:
                        {
                            GEMessages.GEStatusMsg StatusMsg = new GEMessages.GEStatusMsg();
                            objSize = Marshal.SizeOf(StatusMsg);
                            ptr = Marshal.AllocHGlobal(objSize);
                            Marshal.Copy(buffer, 0, ptr, objSize);
                            StatusMsg = (GEMessages.GEStatusMsg)Marshal.PtrToStructure(ptr, typeof(GEMessages.GEStatusMsg));
                            ReceiveStatus(StatusMsg);
                            break;
                        }

                    default:
                        break;

                }

            }

            return true;

        }

        private void ReceiveStatus(GEMessages.GEStatusMsg a_StateMsg)
        {
            if (ReceiveStateMsgEvent != null)
                ReceiveStateMsgEvent(a_StateMsg);
            //m_Simulator.SetPlatformPosition(a_StateMsg.Header.Src, a_StateMsg.State);
        }

        private void ReceiveAck(GEMessages.GEAckMsg a_AckMsg)
        {

        }

        /// <summary>
        /// Send Route to request platform
        /// </summary>        
        public void SendRouteToPlatform(int a_PlatformId, List<Waypoint> a_Route, Point2d pOffsetPos)
        {
            List<Waypoint> RouteForGazebo = new List<Waypoint>();
            Waypoint wp, offsetWP;

            Point2d tempPos;
            for (int iter = 0 ; iter < a_Route.Count; iter++)            
            {
                wp = a_Route[iter];
                offsetWP = new Waypoint(wp);
                tempPos = wp.Position;                
                if (wp.Position.East > 0)                                    
                    tempPos.East -= pOffsetPos.East;                                        
                if (wp.Position.North > 0)
                    tempPos.North -= pOffsetPos.North;

                offsetWP.Position = tempPos;

                RouteForGazebo.Add(offsetWP);
            }
            //GEMessages.GERouteMsg routeMsg = new GEMessages.GERouteMsg(a_PlatformId, a_Route.Count, a_Route);
            GEMessages.GERouteMsg routeMsg = new GEMessages.GERouteMsg(a_PlatformId, RouteForGazebo.Count, RouteForGazebo);
            byte[] TxBuffer = routeMsg.ToByteArray();
            m_TxChannel.Send(TxBuffer, TxBuffer.Length);
        }

        public void SendSimCommand(GEMessages.SIM_OPCODES a_SimCmd)
        {
            GEMessages.GESimCmdMsg simMsg = new GEMessages.GESimCmdMsg(a_SimCmd);
            byte[] TxBuffer = simMsg.ToByteArray();
            m_TxChannel.Send(TxBuffer, TxBuffer.Length);
        }

        public void SendPing(int a_PlatformId)
        {
            GEMessages.GEPingMsg pingMsg = new GEMessages.GEPingMsg(a_PlatformId);
            byte[] TxBuffer = pingMsg.ToByteArray();
            m_TxChannel.Send(TxBuffer, TxBuffer.Length);
        }
    }
}
