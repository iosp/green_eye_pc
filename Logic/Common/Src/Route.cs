using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Logic
{
    public enum LEG_TYPE {LEG_TYPE_TO = 0, LEG_TYPE_MISSION, LEG_TYPE_FROM};

    /// <summary>
    /// Command to the platform
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Waypoint
    {
        public int Id;
        public Point2d Position;
        //public List<IAction>    Actions;
        public double ASL;
        public bool IsHoldState;
        public int HoldDuration;
        public LEG_TYPE LegType;

        public Waypoint(Waypoint a_Waypoint)
        {
            Id = a_Waypoint.Id;
            Position = a_Waypoint.Position;
            ASL = a_Waypoint.ASL;
            IsHoldState = a_Waypoint.IsHoldState;
            HoldDuration = a_Waypoint.HoldDuration;
            LegType = a_Waypoint.LegType;
        }
    };

    public class Route
    {
        public int Id;
        public List<Waypoint> m_Route;
        public int AssignTo;

        public Route()
        {
            Id = -1;
            m_Route = new List<Waypoint>();
            AssignTo = -1;
        }
    }
}
