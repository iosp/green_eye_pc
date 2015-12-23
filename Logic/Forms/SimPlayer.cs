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
    public partial class SimPlayer : Form
    {
        public SimPlayer()
        {
            InitializeComponent();

            ToolTip toolTip1 = new System.Windows.Forms.ToolTip();
            toolTip1.SetToolTip(RefreshBtn, "ReCalcute a new deployment and workplan");
        }

        public delegate void PlayerStateChanged(GEMessages.SIM_OPCODES a_SimCmd);

        public event PlayerStateChanged PlayerStateEvent;

        private void PlanBtn_Click(object sender, EventArgs e)
        {
            //if (PlayerStateEvent != null)
                //PlayerStateEvent(GEMessages.SIM_OPCODES.GE_SIM_RESET);
        }

        private void PlayBtn_Click(object sender, EventArgs e)
        {
            if (PlayerStateEvent != null)
                PlayerStateEvent(GEMessages.SIM_OPCODES.GE_SIM_PLAY);
        }

        private void PauseBtn_Click(object sender, EventArgs e)
        {
            if (PlayerStateEvent != null)
                PlayerStateEvent(GEMessages.SIM_OPCODES.GE_SIM_STOP);
        }

        private void AbortBtn_Click(object sender, EventArgs e)
        {
            if (PlayerStateEvent != null)
                PlayerStateEvent(GEMessages.SIM_OPCODES.GE_SIM_ABORT);
        }
    }
}
