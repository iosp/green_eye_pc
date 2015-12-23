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
    public partial class MissionConfigForm : Form
    {
        private MissionConfig m_Config;

        public MissionConfigForm(MissionConfig pConfig)
        {
            InitializeComponent();
            m_Config = pConfig;
            Init();              
        }

        private void Init()
        {
            MissionHeightTxt.Value = (decimal) m_Config.MissionHeight;
            FlightHeightTxt.Value = (decimal)m_Config.FlightHeight;
            ProjectionTxt.Value = (decimal) m_Config.ProjectionLength;
            MissionDurationTxt.Value = (decimal)m_Config.MissionDuration;
            LaunchDelayTxt.Value = (decimal)m_Config.LaunchDelay; 
          
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            double lMissionHeight;
            double lFlightHeight;
            double lProjection;

            lMissionHeight = (double)MissionHeightTxt.Value;
            lFlightHeight = (double)FlightHeightTxt.Value;
            lProjection = (double)ProjectionTxt.Value;            
// #COMPLETE Validate values
            m_Config.FlightHeight = lFlightHeight;
            m_Config.MissionHeight = lMissionHeight;
            m_Config.ProjectionLength = lProjection;
            m_Config.MissionDuration = (int)MissionDurationTxt.Value;
            m_Config.LaunchDelay = (int)LaunchDelayTxt.Value;
            this.Close();
               
        }

        public MissionConfig GetConfig()
        {
            return m_Config;
        }




  

        
    }
}
