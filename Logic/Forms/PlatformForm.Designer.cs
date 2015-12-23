namespace Logic.Forms
{
    partial class PlatformForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label5 = new System.Windows.Forms.Label();
            this.m_PlatformIdTxt = new System.Windows.Forms.Label();
            this.m_PosHeightTxt = new System.Windows.Forms.Label();
            this.m_PosYTxt = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_PosXTxt = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.m_FailuresCombo = new System.Windows.Forms.CheckedListBox();
            this.ReturnHomeButton = new System.Windows.Forms.Button();
            this.State = new System.Windows.Forms.GroupBox();
            this.m_MissionStateTxt = new System.Windows.Forms.Label();
            this.PlatformIdCombo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_VelTxt = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.State.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Height";
            // 
            // m_PlatformIdTxt
            // 
            this.m_PlatformIdTxt.AutoSize = true;
            this.m_PlatformIdTxt.Location = new System.Drawing.Point(24, 9);
            this.m_PlatformIdTxt.Name = "m_PlatformIdTxt";
            this.m_PlatformIdTxt.Size = new System.Drawing.Size(45, 13);
            this.m_PlatformIdTxt.TabIndex = 3;
            this.m_PlatformIdTxt.Text = "Platform";
            // 
            // m_PosHeightTxt
            // 
            this.m_PosHeightTxt.AutoSize = true;
            this.m_PosHeightTxt.Location = new System.Drawing.Point(62, 72);
            this.m_PosHeightTxt.Name = "m_PosHeightTxt";
            this.m_PosHeightTxt.Size = new System.Drawing.Size(31, 13);
            this.m_PosHeightTxt.TabIndex = 5;
            this.m_PosHeightTxt.Text = "1234";
            // 
            // m_PosYTxt
            // 
            this.m_PosYTxt.AutoSize = true;
            this.m_PosYTxt.Location = new System.Drawing.Point(62, 46);
            this.m_PosYTxt.Name = "m_PosYTxt";
            this.m_PosYTxt.Size = new System.Drawing.Size(31, 13);
            this.m_PosYTxt.TabIndex = 3;
            this.m_PosYTxt.Text = "1234";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "y";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.m_PosHeightTxt);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.m_PosYTxt);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.m_PosXTxt);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(18, 128);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(120, 98);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Position";
            // 
            // m_PosXTxt
            // 
            this.m_PosXTxt.AutoSize = true;
            this.m_PosXTxt.Location = new System.Drawing.Point(62, 19);
            this.m_PosXTxt.Name = "m_PosXTxt";
            this.m_PosXTxt.Size = new System.Drawing.Size(31, 13);
            this.m_PosXTxt.TabIndex = 1;
            this.m_PosXTxt.Text = "1234";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "x";
            // 
            // m_FailuresCombo
            // 
            this.m_FailuresCombo.FormattingEnabled = true;
            this.m_FailuresCombo.Items.AddRange(new object[] {
            "Engine Error",
            "GPS Error",
            "Tx Failure",
            "Rx Failure"});
            this.m_FailuresCombo.Location = new System.Drawing.Point(16, 232);
            this.m_FailuresCombo.Name = "m_FailuresCombo";
            this.m_FailuresCombo.Size = new System.Drawing.Size(120, 94);
            this.m_FailuresCombo.TabIndex = 5;
            // 
            // ReturnHomeButton
            // 
            this.ReturnHomeButton.Location = new System.Drawing.Point(16, 332);
            this.ReturnHomeButton.Name = "ReturnHomeButton";
            this.ReturnHomeButton.Size = new System.Drawing.Size(97, 23);
            this.ReturnHomeButton.TabIndex = 6;
            this.ReturnHomeButton.Text = "Return Home";
            this.ReturnHomeButton.UseVisualStyleBackColor = true;
            this.ReturnHomeButton.Click += new System.EventHandler(this.ReturnHomeButton_Click);
            // 
            // State
            // 
            this.State.Controls.Add(this.m_VelTxt);
            this.State.Controls.Add(this.label2);
            this.State.Controls.Add(this.m_MissionStateTxt);
            this.State.Location = new System.Drawing.Point(18, 38);
            this.State.Name = "State";
            this.State.Size = new System.Drawing.Size(118, 84);
            this.State.TabIndex = 7;
            this.State.TabStop = false;
            this.State.Text = "State";
            // 
            // m_MissionStateTxt
            // 
            this.m_MissionStateTxt.AutoSize = true;
            this.m_MissionStateTxt.Location = new System.Drawing.Point(15, 16);
            this.m_MissionStateTxt.Name = "m_MissionStateTxt";
            this.m_MissionStateTxt.Size = new System.Drawing.Size(67, 13);
            this.m_MissionStateTxt.TabIndex = 0;
            this.m_MissionStateTxt.Text = "MissionState";
            // 
            // PlatformIdCombo
            // 
            this.PlatformIdCombo.FormattingEnabled = true;
            this.PlatformIdCombo.Location = new System.Drawing.Point(83, 9);
            this.PlatformIdCombo.Name = "PlatformIdCombo";
            this.PlatformIdCombo.Size = new System.Drawing.Size(52, 21);
            this.PlatformIdCombo.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "velocity";
            // 
            // m_VelTxt
            // 
            this.m_VelTxt.AutoSize = true;
            this.m_VelTxt.Location = new System.Drawing.Point(15, 68);
            this.m_VelTxt.Name = "m_VelTxt";
            this.m_VelTxt.Size = new System.Drawing.Size(88, 13);
            this.m_VelTxt.TabIndex = 2;
            this.m_VelTxt.Text = "(0,00, 0.00, 0.00)";
            // 
            // PlatformForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(153, 367);
            this.ControlBox = false;
            this.Controls.Add(this.PlatformIdCombo);
            this.Controls.Add(this.State);
            this.Controls.Add(this.ReturnHomeButton);
            this.Controls.Add(this.m_PlatformIdTxt);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.m_FailuresCombo);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PlatformForm";
            this.Text = "PlatformForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.State.ResumeLayout(false);
            this.State.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label m_PlatformIdTxt;
        private System.Windows.Forms.Label m_PosHeightTxt;
        private System.Windows.Forms.Label m_PosYTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label m_PosXTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox m_FailuresCombo;
        private System.Windows.Forms.Button ReturnHomeButton;
        private System.Windows.Forms.GroupBox State;
        private System.Windows.Forms.Label m_MissionStateTxt;
        private System.Windows.Forms.ComboBox PlatformIdCombo;
        private System.Windows.Forms.Label m_VelTxt;
        private System.Windows.Forms.Label label2;

    }
}