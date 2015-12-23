using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Common.Entities
{
 
    public enum FLIGHT_MODE 
    {
	    FLIGHT_MODE_MANUAL = 0,
	    FLIGHT_MODE_AUTO
    };

    public enum MISSION_STATE
    {
        HOME = 0,
        STANDBY,
        TO,
        MISSION,
        FROM,
        RECOVER
    }

    

    public class IAction
    {
        public int Type;

        public IAction()
        {
        }
    }

    public class ISensor
    {
    
	    public int			Id;
	    public int			Type;
	    public int			State;
	    public Point2d		Position;
	    public double		HFov;
	    public double		VFov;


	    ISensor()
        {}
    };

    public class IPlatform_Config
    {
	    public int			m_Type;
	    public double		m_Energy;
	    public double		m_MinVelocity;
	    public double		m_MaxVelocity;
	    public double		m_EnergyCost;

	    public IPlatform_Config()
        {
        }	
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct PlatformState
    {
	    public int			Type;
	    public Point2d		Position;
	    public double		ASL;
	    public double		Heading;
	    public double		Roll;
	    public double		Pitch;
	    public double3d	    Velocity;
        public int           DeployState;
	    public MISSION_STATE MissionState;
	    public int			FailureState;
	    public double		Energy;
    
    };

    public class IPlatform
    {        
	    private int					m_Id;
	    public int					Type;
	    public PlatformState	    State;
	    public IPlatform_Config	    Config;
	    public List<ISensor>	    Sensors;
	    
        public List<Waypoint>	    Route;        
        private CTarget m_Target;

	// ICommunication*	Comm		// TBD
        public IPlatform(int Id) 
        { 
            m_Id = Id;
            m_Target = new CTarget();
            Route = new List<Waypoint>();
        }

        public int Id
        {
            get { return m_Id; }
        }

        public CTarget Target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }

    };

    public class PlatformMode
    {    
	    public FLIGHT_MODE		m_FlightMode;

	    public PlatformMode() 
        {
            m_FlightMode = FLIGHT_MODE.FLIGHT_MODE_MANUAL;
        }
	
        void SetFlightMode (FLIGHT_MODE mode) 
        {
            m_FlightMode = mode;
        }

        FLIGHT_MODE GetFlightMode()
        {
            return m_FlightMode;
        }
};


    
}
