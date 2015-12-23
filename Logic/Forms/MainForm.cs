using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Entities;


namespace Logic
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainForm : Form
    {                              
        private GECom m_FleetCom = null;

        private const int PROJECTION_LENGTH = 10;

        private bool m_IsFullMode = false;
        
        public MainForm(bool pIsFullMode)
        {
            InitializeComponent();                       
            m_FleetCom = new GECom(GEConstants.GetHostIp(), GEConstants.TX_PORT, GEConstants.RX_PORT);
            m_IsFullMode = pIsFullMode;

            if (m_IsFullMode)
            {
                MissionHmiBtn_Click(null, null);
            }
        }
        
        /// <summary>
        /// Create default mission to test system without HMI
        /// </summary>
        /// <returns></returns>
        private CMission CreateDefaultMission()
        {            
            CMission mission = new CMission();
            Area m_Area = new Area();
            Point2d UTMOffset = mission.Config.UTMOffset;


            double AREA_OFFSET_X = 30 + UTMOffset.East;
            double AREA_OFFSET_Y = 0 + UTMOffset.North;
            int SQUARE_SIZE = 200;
            
            Point2d PointOnPolygon = new Point2d();
            PointOnPolygon.East = AREA_OFFSET_X;
            PointOnPolygon.North = AREA_OFFSET_Y;
            m_Area.Polygon.Add(PointOnPolygon);
            PointOnPolygon = new Point2d();
            PointOnPolygon.East = AREA_OFFSET_X + SQUARE_SIZE;
            PointOnPolygon.North = AREA_OFFSET_Y;
            m_Area.Polygon.Add(PointOnPolygon);
            PointOnPolygon = new Point2d();
            PointOnPolygon.East = AREA_OFFSET_X + SQUARE_SIZE;
            PointOnPolygon.North = AREA_OFFSET_Y + SQUARE_SIZE;
            m_Area.Polygon.Add(PointOnPolygon);
            PointOnPolygon = new Point2d();
            PointOnPolygon.East = AREA_OFFSET_X;
            PointOnPolygon.North = AREA_OFFSET_Y + SQUARE_SIZE;
            m_Area.Polygon.Add(PointOnPolygon);

            m_Area.Polygon.Add(m_Area.Polygon[0]);

            mission.MissionArea = m_Area;

            CRecoverySite recovery = new CRecoverySite();
            recovery.ASL = 0.1;
            recovery.Position.East = -10 + UTMOffset.East;
            recovery.Position.North = 100 + UTMOffset.North;
            mission.RecoverySites = new List<CRecoverySite>();
            mission.RecoverySites.Add(recovery);

            CLauncher launcher = new CLauncher(1);
            launcher.Position = new Point2d(0.5 + UTMOffset.North, 0.5 + UTMOffset.East);
            mission.Launchers = new List<CLauncher>();
            mission.Launchers.Add(launcher);

            CGateway gatewayIn = new CGateway();
            gatewayIn.Start.East = AREA_OFFSET_X ;
            gatewayIn.Start.North = AREA_OFFSET_Y + SQUARE_SIZE / 3;
            gatewayIn.MinHeight = gatewayIn.MaxHeight = mission.Config.FlightHeight;
            CGateway gatewayOut = new CGateway();
            gatewayOut.Start.East = AREA_OFFSET_X;
            gatewayOut.Start.North = AREA_OFFSET_Y + 2 * SQUARE_SIZE / 3;
            gatewayOut.MinHeight = gatewayOut.MaxHeight = mission.Config.FlightHeight;

            mission.MissionArea.GwInList.Add(gatewayIn);
            mission.MissionArea.GwOutList.Add(gatewayOut);
            return mission;
        }

        private Dictionary<int, IPlatform> CreateFleet(int a_FleetSize, Point2d a_StartPos)
        {
            Dictionary<int, IPlatform> Platforms = new Dictionary<int, IPlatform>();
            Point2d StartPosition = a_StartPos;
            double ASL = 0;
            IPlatform platform;
            int NumPlatforms = Platforms.Count;

            // Add new platforms
            for (int id = 1 + NumPlatforms; id < 1 + NumPlatforms + a_FleetSize; id++)
            {
                platform = new IPlatform(id);
                platform.State.Position = StartPosition;
                platform.State.ASL = ASL;
                Platforms.Add(id, platform);
            }
            return Platforms;
        }

                               

        private void TestComBtn_Click(object sender, EventArgs e)
        {
            if (m_FleetCom != null)
                m_FleetCom.SendPing(0);
        }
       
        private void ComConfigBtn_Click(object sender, EventArgs e)
        {
            if (m_FleetCom != null)
            {
            }
            Forms.ComConfigForm configForm = new Forms.ComConfigForm();
            configForm.ShowDialog();
            if (m_FleetCom != null)
            {
                m_FleetCom = configForm.GetCommInterface();
                //m_FleetCom.SendSimCommand(GEMessages.SIM_OPCODES.GE_SIM_RESET);
                
            }

        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void NewSimulatorBtn_Click(object sender, EventArgs e)
        {            
            Scenario scenario = new Scenario(m_FleetCom);
            Simulator simulator = Simulator.Instance(scenario, Simulator.SimMode.SA);            
            simulator.Show();
        }

        private void RunDemoBtn_Click(object sender, EventArgs e)
        {
            Scenario scenario = new Scenario(m_FleetCom);

            CMission DemoMission;

            //// Create Mission              
            DemoMission = CreateDefaultMission();
            DemoMission.Config = new MissionConfig();
            scenario.Mission = DemoMission;
            //this.WindowState = FormWindowState.Minimized;               
            Simulator simulator = Simulator.Instance(scenario, Simulator.SimMode.SA);
            simulator.Show();

        }

        private void MissionHmiBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            Scenario scenario = new Scenario(m_FleetCom);
            Simulator simulator = Simulator.Instance(scenario, Simulator.SimMode.AppServer);

            GreenEyeAPI.BL.SampleLogicBLImplementation LogicBL = new GreenEyeAPI.BL.SampleLogicBLImplementation(scenario);
            GreenEyeAPI.WebServer.Start(false, false);
            simulator.Show();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_FleetCom != null)
                m_FleetCom.Close();
        }

        private void TestReturnHomeBtn_Click(object sender, EventArgs e)
        {
            Logic.Test.TestAlgorithms test = new Test.TestAlgorithms();
            test.TestReturnHomeAlgo();
        }

        


       
        
    }
}
