using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Entities;

namespace Logic.Forms
{
    public partial class PlatformForm : Form
    {
        IPlatform m_Platform;

        public PlatformForm()
        {
            InitializeComponent();
        }

        public void SetPlatform(IPlatform platform)
        {
            m_Platform = platform;
            if (m_Platform != null && m_Platform.Id >= 1)
            {
                m_PlatformIdTxt.Text = "Platform " + m_Platform.Id.ToString();
                m_PosXTxt.Text = m_Platform.State.Position.East.ToString();
                m_PosYTxt.Text = m_Platform.State.Position.North.ToString();
                m_PosHeightTxt.Text = m_Platform.State.ASL.ToString();
                
                m_MissionStateTxt.Text = m_Platform.State.MissionState.ToString();
                double3d velocity = platform.State.Velocity;
                m_VelTxt.Text = "(" + velocity.x.ToString("0.0") + ", " +
                                     velocity.y.ToString("0.0") + ", " +
                                     velocity.z.ToString("0.0") + ")";
            }
            else
            {
                m_PlatformIdTxt.Text = "NA";
                m_PosXTxt.Text = "NA";
                m_PosYTxt.Text = "NA";
                m_PosHeightTxt.Text = "NA";
                m_MissionStateTxt.Text = "NA";
            }
           
        }

        private void ReturnHomeButton_Click(object sender, EventArgs e)
        {
            if (m_Platform != null)
            {
                
            }
        }

        

    }
}
