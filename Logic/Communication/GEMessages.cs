using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Common.Entities;

namespace Logic
{
    public class GEMessages
    {
        public static int m_TxCounter = 0;

        public enum GE_OPCODES {GE_OP_CMD_SET_WP = 1,
	                        GE_OP_CMD_ROUTE,
	                        GE_OP_CMD_SET_SENSOR,
	                        GE_OP_REQ_STATUS,
	                        GE_OP_SET_MODE,
	                        GE_OP_SIM_CMD,	
                            GE_OP_PING,
	                        GE_OP_ACK	= 1001,
	                        GE_OP_STATUS};
    
        public enum SIM_OPCODES {GE_SIM_EXIT = 1,
	                            GE_SIM_RESET,
	                            GE_SIM_STOP,
	                            GE_SIM_PLAY,
                                GE_SIM_ABORT};

        [StructLayout(LayoutKind.Sequential)]
        public struct GEHeaderMsg
        {
            public int StartBytes;
            public int QueryId;
            public int Src;
            public int Dst;
            public int MsgLength;
            public int Opcode;      
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct WaypointStruct
        {
            public int Id;
            public Point2d Position;
            //public List<IAction>    Actions;
            public double ASL;
            public bool IsHoldState;
            public int HoldDuration;
            public LEG_TYPE LegType;

            public WaypointStruct(int id)
            {
                Id = id;
                Position = new Point2d();
                ASL = 0;
                IsHoldState = false;
                HoldDuration = 0;
                LegType = LEG_TYPE.LEG_TYPE_FROM;
            }

            public WaypointStruct(Waypoint wp)
            {
                Id = wp.Id;
                Position = wp.Position;
                ASL = wp.m_ASL;
                IsHoldState = wp.m_IsHoldState;
                HoldDuration = wp.m_HoldDuration;
                LegType = wp.m_LegType;
            }

        };

//////////////////////////////////////////////////////////////
//				GS Messages -> Platform						//
//////////////////////////////////////////////////////////////            
        public struct GECommandMsg
        {
            public GEHeaderMsg Header;
            public WaypointStruct WP;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct GERouteMsg
        {
            public GEHeaderMsg Header;
            public int NumWayPoints;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GEConstants.MAX_ROUTE)]
            public WaypointStruct[] Route;

            public GERouteMsg(int a_PlatformId, int a_NumWayPoints, List<Waypoint> a_Route)
            {
                int iter;
                a_NumWayPoints = Math.Min(a_NumWayPoints, GEConstants.MAX_ROUTE);

                this.Header.StartBytes = GEConstants.GE_START_BYTES;
                this.Header.Opcode = (int)GE_OPCODES.GE_OP_CMD_ROUTE;
                this.Header.Src = 0;
                this.Header.Dst = a_PlatformId;
                this.Header.QueryId = m_TxCounter++;
                this.Header.MsgLength = 0;
                this.NumWayPoints = a_NumWayPoints;

                Route = new WaypointStruct[GEConstants.MAX_ROUTE];
                for (iter = 0; iter < a_NumWayPoints; iter++)
                    Route[iter] = new WaypointStruct(a_Route[iter]);
                for (iter = a_NumWayPoints; iter < GEConstants.MAX_ROUTE; iter++)
                    Route[iter] = new WaypointStruct(-1);
            }

            /// <summary>
            /// Convert structure to array
            /// </summary>        
            public byte[] ToByteArray()
            {
                byte[] arr = null;
                IntPtr ptr = IntPtr.Zero;
                try
                {
                    Int16 size = (Int16)Marshal.SizeOf(this);
                    Header.MsgLength = size;
                    arr = new byte[size];
                    ptr = Marshal.AllocHGlobal(size);
                    Marshal.StructureToPtr(this, ptr, true);
                    Marshal.Copy(ptr, arr, 0, size);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }

                return arr;
            }
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct GEReqStatusMsg
        {
            public GEHeaderMsg Header;

            public GEReqStatusMsg(int a_PlatformId)
            {
                this.Header.StartBytes = GEConstants.GE_START_BYTES;
                this.Header.Opcode = (int)GE_OPCODES.GE_OP_REQ_STATUS;
                this.Header.Src = 0;
                this.Header.Dst = a_PlatformId;
                this.Header.QueryId = m_TxCounter++;
                this.Header.MsgLength = 0;
            }

            /// <summary>
            /// Convert structure to array
            /// </summary>        
            public byte[] ToByteArray()
            {
                byte[] arr = null;
                IntPtr ptr = IntPtr.Zero;
                try
                {
                    Int16 size = (Int16)Marshal.SizeOf(this);
                    Header.MsgLength = size;
                    arr = new byte[size];
                    ptr = Marshal.AllocHGlobal(size);
                    Marshal.StructureToPtr(this, ptr, true);
                    Marshal.Copy(ptr, arr, 0, size);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }

                return arr;
            }
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct GESimCmdMsg
        {
            public GEHeaderMsg Header;
            public SIM_OPCODES SimCommand;

            public GESimCmdMsg(SIM_OPCODES a_SimCommand)
            {
                this.Header.StartBytes = GEConstants.GE_START_BYTES;
                this.Header.Opcode = (int)GE_OPCODES.GE_OP_SIM_CMD;
                this.Header.Src = 0;
                this.Header.Dst = GEConstants.GE_FLEET_ADDRESS;
                this.Header.QueryId = m_TxCounter++;
                this.Header.MsgLength = 0;

                this.SimCommand = a_SimCommand;
            }

            /// <summary>
            /// Convert structure to array
            /// </summary>        
            public byte[] ToByteArray()
            {
                byte[] arr = null;
                IntPtr ptr = IntPtr.Zero;
                try
                {
                    Int16 size = (Int16)Marshal.SizeOf(this);
                    Header.MsgLength = size;
                    arr = new byte[size];
                    ptr = Marshal.AllocHGlobal(size);
                    Marshal.StructureToPtr(this, ptr, true);
                    Marshal.Copy(ptr, arr, 0, size);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }

                return arr;
            }
        };

        public struct GEPlatformModeMsg
        {
            public GEHeaderMsg Header;
            //PlatformMode		Mode;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct GEPingMsg
        {
            public GEHeaderMsg Header;

            public GEPingMsg(int a_Dst)
            {
                Header.Dst = a_Dst;
                Header.QueryId = m_TxCounter++;
                Header.MsgLength = 0;
                Header.Opcode = (int)GE_OPCODES.GE_OP_PING;
                Header.Src = GEConstants.GE_LOGIC_ADDRESS;
                Header.StartBytes = GEConstants.GE_START_BYTES;
            }

            /// <summary>
            /// Convert structure to array
            /// </summary>        
            public byte[] ToByteArray()
            {
                byte[] arr = null;
                IntPtr ptr = IntPtr.Zero;
                try
                {
                    Int16 size = (Int16)Marshal.SizeOf(this);
                    Header.MsgLength = size;
                    arr = new byte[size];
                    ptr = Marshal.AllocHGlobal(size);
                    Marshal.StructureToPtr(this, ptr, true);
                    Marshal.Copy(ptr, arr, 0, size);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }

                return arr;
            }
        };
        
//////////////////////////////////////////////////////////////      
//				Platform -> GS Messages						//        
//////////////////////////////////////////////////////////////
        [StructLayout(LayoutKind.Sequential)]
        public struct GEStatusMsg
        {
            public GEHeaderMsg Header;
            public PlatformState State;
        };

        public struct GEAckMsg
        {
            public GEHeaderMsg Header;
            public int QueryId;
            public bool NAck;
        };


    }; // end of GEMessages class

    
}
