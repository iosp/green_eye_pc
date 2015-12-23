using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Logic.Forms
{
    public partial class ComConfigForm : Form
    {
        private GECom m_Comm;

        public ComConfigForm()
        {
            InitializeComponent();
            
            m_HostIp.Text = GEConstants.GetHostIp();
            m_LocalIp.Text = GEConstants.GetLocalIP();
            m_TxPortTxt.Text = GEConstants.TX_PORT.ToString();
            m_RxPortTxt.Text = GEConstants.RX_PORT.ToString();            
        }

        private void SaveExitBtn_Click(object sender, EventArgs e)
        {            
            SaveExitBtn.Enabled = false;            
            int TxPort, RxPort;
            int.TryParse(m_TxPortTxt.Text, out TxPort);
            int.TryParse(m_RxPortTxt.Text, out RxPort);

            m_Comm = new GECom(m_HostIp.Text, TxPort, RxPort);                        
            
            this.Close();
        }

        public GECom GetCommInterface()
        {
            return m_Comm;
        }


    }
}
