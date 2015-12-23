using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Common.Utility;
using Common.Entities;

namespace Logic.Controls
{
    public class SimViewFilter
    {
        public bool m_IsShowRoute;

        public bool m_IsShowLauncher;

        public SimViewFilter()
        {
            ResetFilter();
        }

        public void ResetFilter()
        {
            m_IsShowLauncher = true;
            m_IsShowRoute = true;
        }
    }

    public partial class SimView : UserControl
    {
        const int MIN_DISTANCE_TH = 50;

        private int m_XOffset, m_YOffset;

        private double m_Resolution;

        private Scenario m_Scenario;        

        private List<Point> m_EditPolygon = new List<Point>();

        private Bitmap m_RecoveryImage;
        private Bitmap m_PlatformImage;
        private Bitmap m_LauncherImage;

        public SimViewFilter m_ViewFilter;

       

        public SimView()
        {
            InitializeComponent();                                   
            Init();
        }

        private void Init()
        {
            m_XOffset = this.Width / 2;
            m_YOffset = this.Height / 2;
            m_Resolution = 1;
            m_ViewFilter = new SimViewFilter();
            m_RecoveryImage = Properties.Resources.RecoveryPoint;
            m_RecoveryImage.MakeTransparent(Color.White);
            m_PlatformImage = Properties.Resources.quadrotor;
            m_PlatformImage.MakeTransparent(Color.White);
            m_LauncherImage = Properties.Resources.launcher;
            m_LauncherImage.MakeTransparent(Color.White);
        }

        public void SetScenario(Scenario a_Scenario)
        {
            m_Scenario = a_Scenario;
        }

        private void Simulator_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (m_Scenario == null)
                return;
           
            foreach (IPlatform platform in m_Scenario.Platforms.Values)            
                DrawPlatform(g, platform);            
           
            DrawMission(g, m_Scenario.Mission);
            DrawDeployments(g, m_Scenario.Targets);
        }

        private PointF ConvertUTMToPixel(Point2d a_Point)
        {
            PointF pixel = new PointF();
            pixel.X = (float)(m_XOffset + m_Resolution * (a_Point.East - m_Scenario.Config.UTMOffset.East));
            pixel.Y = (float)(m_YOffset - m_Resolution * (a_Point.North - m_Scenario.Config.UTMOffset.North));
            return pixel;
        }

        public Point2d ConvertPixelToUTM(PointF a_Pixel)
        {
            return ConvertPixelToUTM(a_Pixel.X, a_Pixel.Y);
        }

        public Point2d ConvertPixelToUTM(double X, double Y)
        {
            Point2d UTM = new Point2d();
            UTM.East = m_Scenario.Config.UTMOffset.East + (double)((X - m_XOffset) / m_Resolution);
            UTM.North = m_Scenario.Config.UTMOffset.North - (double)((Y - m_YOffset) / m_Resolution);

            return UTM;
        }

        private void DrawPlatform(Graphics g, IPlatform a_Platform)
        {
            PointF pixel;
            PlatformState state = a_Platform.State;
            try
            {
                pixel = ConvertUTMToPixel(state.Position);
            }
            catch (Exception ex)
            {
                int d = 5;
                return;
            }
            
            float DrawSize = Math.Min(25 + (float)m_Resolution + ((int)state.ASL) * 3, 45);
            pixel.X -= DrawSize / 2;
            pixel.Y -= DrawSize / 2;
            g.DrawImage(m_PlatformImage, pixel.X, pixel.Y, DrawSize, DrawSize);
            if (m_ViewFilter.m_IsShowRoute)            
                DrawRoute(g, a_Platform.Route);            
        }

        private void DrawRoute(Graphics g, List<Waypoint> pRoute)
        {
            if (pRoute.Count > 1)
            {
                int iter = 0;
                Waypoint FromWP = pRoute[0];
                Waypoint ToWP;
                Pen routePen = new Pen(new SolidBrush(Color.Red));
                for (iter = 1; iter < pRoute.Count; iter++)
                {
                    ToWP = pRoute[iter];
                    g.DrawLine(routePen, ConvertUTMToPixel(FromWP.Position), ConvertUTMToPixel(ToWP.Position));

                    FromWP = ToWP;
                }
            }
        }

        private void DrawLauncher(Graphics g, CLauncher a_Launcher)
        {
            PointF pixel = ConvertUTMToPixel(a_Launcher.Position);

            int DrawSize = 15 + (int) m_Resolution;
            g.DrawImage(m_LauncherImage, pixel.X, pixel.Y, DrawSize, DrawSize);            
        }

        private void DrawRecovery(Graphics g, CRecoverySite a_Recovery)
        {
            PointF pixel = ConvertUTMToPixel(a_Recovery.Position);
            int DrawSize = 25 + (int)m_Resolution;

            g.DrawImage(m_RecoveryImage, pixel.X - DrawSize / 2, pixel.Y - DrawSize / 2, DrawSize, DrawSize);
        }

        private void DrawGateWay(Graphics g, CGateway pGateway, bool IsGatewayIn)
        {
            PointF pixel = ConvertUTMToPixel(pGateway.Start);
            pixel.X -= 10;
            pixel.Y -= 10;
            if (IsGatewayIn)
                g.DrawString("I", new Font("Arial", 16), new SolidBrush(Color.Black), pixel);
            else
                g.DrawString("O", new Font("Arial", 16), new SolidBrush(Color.Black), pixel);
        }
        
        private void DrawMission(Graphics g, CMission a_Mission)
        {
            List<Point2d> Polygon = a_Mission.MissionArea.Polygon;
            //float FromX, FromY;
            //float ToX, ToY;
            PointF FromPixel, ToPixel;
            bool IsFirstIter = true;
            Pen LinePen = new Pen(Color.Black);
            ToPixel = new PointF(0, 0);
            FromPixel = new PointF(0, 0);

            if (a_Mission.MissionArea != null)
            {
                // Draw Polygon
                if (Polygon.Count > 0)
                {
                    foreach (Point2d point in Polygon)
                    {
                        ToPixel = ConvertUTMToPixel(point);
                        g.DrawEllipse(LinePen, ToPixel.X, ToPixel.Y, 2, 2);                       
                        if (!IsFirstIter)
                            g.DrawLine(LinePen, FromPixel.X, FromPixel.Y, ToPixel.X, ToPixel.Y);                            
                        IsFirstIter = false;
                        FromPixel = ToPixel;                        
                    }                    
                }

                // Draw Launcher
                if (m_ViewFilter.m_IsShowLauncher)
                {
                    foreach (CLauncher launcher in a_Mission.Launchers)
                        DrawLauncher(g, launcher);
                }
                foreach (CRecoverySite recovery in a_Mission.RecoverySites)
                    DrawRecovery(g, recovery);
                foreach (CGateway gatewayIn in a_Mission.MissionArea.GwInList)
                    DrawGateWay(g, gatewayIn, true);
                foreach (CGateway gatewayOut in a_Mission.MissionArea.GwOutList)
                    DrawGateWay(g, gatewayOut, false);

            }
        }

        private void DrawDeployments(Graphics g, List<CTarget> pTargets)
        {
            int DrawSize = 4;
            if (pTargets == null)
                return;
            foreach (CTarget target in pTargets)
            {
                PointF pixel = ConvertUTMToPixel(target.Position);
                g.DrawEllipse(new Pen(Color.Blue), pixel.X, pixel.Y, DrawSize, DrawSize);
            }
        }       

        public void SetZoom (int a_Zoom)
        {
            m_Resolution += a_Zoom;
            if (m_Resolution < 1)
                m_Resolution = 1;
        }

        public void SetOffset(int X, int Y)
        {
            m_XOffset += (this.Width / 2 - X);
            m_YOffset += (this.Height / 2 - Y);
        }

        public IPlatform GetTheClosestPlatform(int XPixel, int YPixel)
        {            
            IPlatform ClosestPlatform = new IPlatform(0);
            double MinDistance = double.MaxValue;
            double Distance;
            PlatformState state;
            PointF pixel;

            foreach (IPlatform platform in m_Scenario.Platforms.Values)
            {                
                state = platform.State;
                pixel = ConvertUTMToPixel(state.Position);
                Distance = MathHelper.GetDistance(XPixel, YPixel, pixel.X, pixel.Y);
                if (Distance < MinDistance)
                {
                    MinDistance = Distance;
                    if (Distance < MIN_DISTANCE_TH)
                    {
                        ClosestPlatform = platform;
                    }
                }               
            }

            return ClosestPlatform;
        }

    }
}
