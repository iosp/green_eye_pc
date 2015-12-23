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

    public partial class Simulator : Form
    {
        public enum SimMode { SA, AppServer };
        private enum EditMode { None, EditPolygon, EditLauncher, EditRecovry, EditGatewayIn, EditGatewayOut };
        private static Simulator m_pMyself = null;

        private Scenario m_Scenario;

        private IPlatform m_SelectedPlatform = null;

        private Forms.SimPlayer m_SimPlayer;

        private Forms.PlatformForm m_PlatformForm;
        //private bool m_IsFirstPaint = true;

        private EditMode m_EditMode = EditMode.None;

        private SimMode m_SimMode = SimMode.SA;

        private Simulator(Scenario a_Scenario, SimMode a_SimMode)
        {
            InitializeComponent();
            int FormOffsetX = 100;
            int FormOffsetY = 100;
            m_Scenario = a_Scenario;
            m_SimMode = a_SimMode;
            

            if (m_SimMode == SimMode.SA)
            {
                //m_SimPlayer = new Forms.SimPlayer();
                //m_SimPlayer.Show();
                //m_SimPlayer.PlayerStateEvent += new Forms.SimPlayer.PlayerStateChanged(m_SimPlayer_PlayerStateEvent);
                
                //m_SimPlayer.Location = new Point(FormOffsetX + m_PlatformForm.Width, FormOffsetY);
                //m_PlatformForm.Location = new Point(FormOffsetX, FormOffsetY + m_SimPlayer.Size.Height);
                //this.Location = new Point(FormOffsetX + m_PlatformForm.Width, FormOffsetY + m_SimPlayer.Height);                
            }
            else
            {
                //this.Location = new Point(300, 200);
            }

            m_PlatformForm = new Forms.PlatformForm();
            m_PlatformForm.Show();
            m_PlatformForm.Location = new Point(FormOffsetX, FormOffsetY);
            this.Location = new Point(FormOffsetX + m_PlatformForm.Width, FormOffsetY);

            m_SimView.SetScenario(a_Scenario);
            this.WindowState = FormWindowState.Normal;

            this.MouseWheel += new MouseEventHandler(Simulator_MouseWheel);
            Init();            
        }

        public static Simulator Instance(Scenario a_Scenario, SimMode a_SimMode)
        {
            if (m_pMyself == null)
                m_pMyself = new Simulator(a_Scenario, a_SimMode);
            return m_pMyself;
        }

        void Simulator_MouseWheel(object sender, MouseEventArgs e)
        {
            int zoom = e.Delta / 120;
            m_SimView.SetZoom(zoom);
        }                

        private void Init()
        {                        
            m_RefreshTimer.Start();
        }

        private void m_RefreshTimer_Tick(object sender, EventArgs e)
        {
            this.Refresh();
            if (m_SelectedPlatform != null && m_PlatformForm != null)
                m_PlatformForm.SetPlatform(m_SelectedPlatform);
        }        
     
        private void m_SimPlayer_PlayerStateEvent(GEMessages.SIM_OPCODES a_SimCmd)
        {
            //switch (a_SimCmd)
            //{
            //    case GEMessages.SIM_OPCODES.GE_SIM_EXIT:
            //        break;
            //    case GEMessages.SIM_OPCODES.GE_SIM_RESET:                    
            //        // m_Scenario.RefreshMissionPlan(); 
            //        break;
            //    case GEMessages.SIM_OPCODES.GE_SIM_STOP:                    
            //        break;
            //    case GEMessages.SIM_OPCODES.GE_SIM_PLAY:                    
            //        break;
            //    default:
            //        break;
            //}
            m_Scenario.ReceiveSimCommand(a_SimCmd);
        }      
     

        private void Simulator_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_Scenario.ReceiveSimCommand(GEMessages.SIM_OPCODES.GE_SIM_STOP);
            m_Scenario.ReceiveSimCommand(GEMessages.SIM_OPCODES.GE_SIM_EXIT);
            if (m_SimPlayer != null)
                m_SimPlayer.Close();
            if (m_PlatformForm != null)
                m_PlatformForm.Close();            
            m_pMyself = null;
        }

        private void m_SimView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                m_SimView.SetOffset(e.X, e.Y);
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                switch (m_EditMode)
                {
                    case EditMode.None:
                        m_SelectedPlatform = m_SimView.GetTheClosestPlatform(e.X, e.Y);
                        break;
                    case EditMode.EditPolygon:
                        CreateMissionArea(e.X, e.Y);
                        break;
                    case EditMode.EditLauncher:
                        CreateLauncher(e.X, e.Y, 1);
                        break;
                    case EditMode.EditRecovry:
                        CreateRecovrySite(e.X, e.Y);
                        break;
                    case EditMode.EditGatewayIn:
                        CreateGateWay(e.X, e.Y, true);
                        break;
                    case EditMode.EditGatewayOut:
                        CreateGateWay(e.X, e.Y, false);
                        break;
                }               
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (m_EditMode != EditMode.None)
                    m_EditMode = EditMode.None;
            }
        }

        private void CreateMissionArea(int X, int Y)
        {                            
            List<Point2d> polygon = m_Scenario.Mission.MissionArea.Polygon;
            // In case of first point - add point at the end of list
            if (polygon.Count == 0)
                polygon.Add(m_SimView.ConvertPixelToUTM(X, Y));
            // add new point before the last point                                  
            int index = polygon.Count - 1;            
            polygon.Insert(index, m_SimView.ConvertPixelToUTM(X, Y));            
            m_Scenario.UTMPolygon = polygon;
        }

        private void CreateLauncher(int X, int Y, int Id)
        {
            CLauncher launcher = new CLauncher(Id);
            launcher.ASL = 0;            
            launcher.Position = m_SimView.ConvertPixelToUTM(X, Y);
            m_Scenario.Mission.Launchers.Add(launcher);
            m_EditMode = EditMode.None;   
        }

        private void CreateGateWay(int X, int Y, bool pIsGwIn)
        {
            CGateway gateway = new CGateway();
            gateway.MinHeight = gateway.MaxHeight = m_Scenario.Config.FlightHeight;
            gateway.Start = gateway.End = m_SimView.ConvertPixelToUTM(X, Y);
            
            if (pIsGwIn)
                m_Scenario.Mission.MissionArea.GwInList.Add(gateway);
            else
                m_Scenario.Mission.MissionArea.GwOutList.Add(gateway);
            m_EditMode = EditMode.None;
        }

        private void CreateRecovrySite(int X, int Y)
        {
            CRecoverySite recovery = new CRecoverySite();
            recovery.ASL = 0;
            recovery.Id = 1;
            recovery.Position = m_SimView.ConvertPixelToUTM(X, Y);            
            m_Scenario.Mission.RecoverySites.Add(recovery);
            m_EditMode = EditMode.None;
        }

        private void m_EditPolygonBtn_Click(object sender, EventArgs e)
        {
            if (m_EditMode == EditMode.EditPolygon)
            {
                m_EditMode = EditMode.None;                
            }
            else
            {
                m_EditMode = EditMode.EditPolygon;
                List<Point2d> polygon = m_Scenario.Mission.MissionArea.Polygon;
                polygon.Clear();
                m_Scenario.UTMPolygon = polygon;
                m_SimView.m_ViewFilter.ResetFilter();     
            }            
        }

        private void m_EditLauncherBtn_Click(object sender, EventArgs e)
        {
            if (m_EditMode == EditMode.EditLauncher)
            {
                m_EditMode = EditMode.None;
            }
            else
            {
                m_EditMode = EditMode.EditLauncher;
                m_Scenario.Mission.Launchers.Clear();
                m_SimView.m_ViewFilter.ResetFilter();
            }
        }

        private void m_EditRecoveryBtn_Click(object sender, EventArgs e)
        {
            if (m_EditMode == EditMode.EditRecovry)
            {
                m_EditMode = EditMode.None;
            }
            else
            {
                m_EditMode = EditMode.EditRecovry;
                m_Scenario.Mission.RecoverySites.Clear();
                m_SimView.m_ViewFilter.ResetFilter();
            }
        }


        private void m_EditorExit_Click(object sender, EventArgs e)
        {            
            m_EditMode = EditMode.None;
        }

        private void MissionConfigBtn_Click(object sender, EventArgs e)
        {
            Forms.MissionConfigForm dlg = new Forms.MissionConfigForm(m_Scenario.Config);
            dlg.ShowDialog();
            m_Scenario.Config = dlg.GetConfig();
        }

        private void Simulator_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                m_SimPlayer.WindowState = FormWindowState.Minimized;
                m_PlatformForm.WindowState = FormWindowState.Minimized;
            }
            if (WindowState == FormWindowState.Normal)
            {
                m_SimPlayer.WindowState = FormWindowState.Normal;
                m_PlatformForm.WindowState = FormWindowState.Normal;
                m_SimPlayer.Focus();
                m_PlatformForm.Focus();
                this.Focus();
            }
        }

        private void ViewRouteBtn_Click(object sender, EventArgs e)
        {
            ViewRouteBtn.Checked = !ViewRouteBtn.Checked;
            m_SimView.m_ViewFilter.m_IsShowRoute = ViewRouteBtn.Checked;
        }

        private void ViewLauncherBtn_Click(object sender, EventArgs e)
        {
            ViewLauncherBtn.Checked = !ViewLauncherBtn.Checked;
            m_SimView.m_ViewFilter.m_IsShowLauncher = ViewLauncherBtn.Checked;
        }

        private void m_EditGatewayInBtn_Click(object sender, EventArgs e)
        {
            if (m_EditMode == EditMode.EditGatewayIn)
            {
                m_EditMode = EditMode.None;
            }
            else
            {
                m_EditMode = EditMode.EditGatewayIn;
                m_SimView.m_ViewFilter.ResetFilter();
            }
        }

        private void m_EditGatewayOutBtn_Click(object sender, EventArgs e)
        {
            if (m_EditMode == EditMode.EditGatewayOut)
            {
                m_EditMode = EditMode.None;
            }
            else
            {
                m_EditMode = EditMode.EditGatewayOut;
                m_SimView.m_ViewFilter.ResetFilter();
            }
        }

#region Player Buttons

        private void PlayerReplanBtn_Click(object sender, EventArgs e)
        {
            //m_Scenario.ReceiveSimCommand(GEMessages.SIM_OPCODES.GE_SIM_RESET);
            m_Scenario.ReceiveSimCommand(GEMessages.SIM_OPCODES.GE_SIM_PLAY);
        }

        private void PlayerPlayBtn_Click(object sender, EventArgs e)
        {
            m_Scenario.ReceiveSimCommand(GEMessages.SIM_OPCODES.GE_SIM_PLAY);
        }

        private void PlayerPauseBtn_Click(object sender, EventArgs e)
        {
            m_Scenario.ReceiveSimCommand(GEMessages.SIM_OPCODES.GE_SIM_STOP);
        }

        private void PlayerAbortBtn_Click(object sender, EventArgs e)
        {
            m_Scenario.ReceiveSimCommand(GEMessages.SIM_OPCODES.GE_SIM_ABORT);
        }

        private void PlayerReset_Click(object sender, EventArgs e)
        {
            m_Scenario.ReceiveSimCommand(GEMessages.SIM_OPCODES.GE_SIM_RESET);
        }

 #endregion

        private void EditorNewBtn_Click(object sender, EventArgs e)
        {
            m_Scenario.Mission = new CMission();
        }

        private void PlayerResetLogic_Click(object sender, EventArgs e)
        {
            m_Scenario.ReCalcWorkplan();
        }

        

        





    }
}
