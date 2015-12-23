namespace Logic
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.confi = new System.Windows.Forms.ToolStripMenuItem();
            this.ComConfigBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.TestComBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewSimulatorBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RunDemoBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TestReturnHomeBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.MissionHmiBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem,
            this.confi,
            this.projectToolStripMenuItem,
            this.testToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(266, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExitBtn});
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.exitToolStripMenuItem.Text = "File";
            // 
            // ExitBtn
            // 
            this.ExitBtn.Name = "ExitBtn";
            this.ExitBtn.Size = new System.Drawing.Size(92, 22);
            this.ExitBtn.Text = "Exit";
            this.ExitBtn.Click += new System.EventHandler(this.ExitBtn_Click);
            // 
            // confi
            // 
            this.confi.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ComConfigBtn,
            this.TestComBtn});
            this.confi.Name = "confi";
            this.confi.Size = new System.Drawing.Size(55, 20);
            this.confi.Text = "Config";
            // 
            // ComConfigBtn
            // 
            this.ComConfigBtn.Name = "ComConfigBtn";
            this.ComConfigBtn.Size = new System.Drawing.Size(164, 22);
            this.ComConfigBtn.Text = "Fleet Comm";
            this.ComConfigBtn.Click += new System.EventHandler(this.ComConfigBtn_Click);
            // 
            // TestComBtn
            // 
            this.TestComBtn.Name = "TestComBtn";
            this.TestComBtn.Size = new System.Drawing.Size(164, 22);
            this.TestComBtn.Text = "Test Fleet Comm";
            this.TestComBtn.Click += new System.EventHandler(this.TestComBtn_Click);
            // 
            // projectToolStripMenuItem
            // 
            this.projectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewSimulatorBtn,
            this.loadToolStripMenuItem,
            this.RunDemoBtn,
            this.MissionHmiBtn});
            this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            this.projectToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.projectToolStripMenuItem.Text = "Mission";
            // 
            // NewSimulatorBtn
            // 
            this.NewSimulatorBtn.Name = "NewSimulatorBtn";
            this.NewSimulatorBtn.Size = new System.Drawing.Size(152, 22);
            this.NewSimulatorBtn.Text = "New";
            this.NewSimulatorBtn.Click += new System.EventHandler(this.NewSimulatorBtn_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadToolStripMenuItem.Text = "Load";
            // 
            // RunDemoBtn
            // 
            this.RunDemoBtn.Name = "RunDemoBtn";
            this.RunDemoBtn.Size = new System.Drawing.Size(152, 22);
            this.RunDemoBtn.Text = "Demo";
            this.RunDemoBtn.Click += new System.EventHandler(this.RunDemoBtn_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TestReturnHomeBtn});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.testToolStripMenuItem.Text = "Test";
            // 
            // TestReturnHomeBtn
            // 
            this.TestReturnHomeBtn.Name = "TestReturnHomeBtn";
            this.TestReturnHomeBtn.Size = new System.Drawing.Size(142, 22);
            this.TestReturnHomeBtn.Text = "ReturnHome";
            this.TestReturnHomeBtn.Click += new System.EventHandler(this.TestReturnHomeBtn_Click);
            // 
            // MissionHmiBtn
            // 
            this.MissionHmiBtn.Name = "MissionHmiBtn";
            this.MissionHmiBtn.Size = new System.Drawing.Size(152, 22);
            this.MissionHmiBtn.Text = "HMI";
            this.MissionHmiBtn.Click += new System.EventHandler(this.MissionHmiBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 257);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Algorithm Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem confi;
        private System.Windows.Forms.ToolStripMenuItem ComConfigBtn;
        private System.Windows.Forms.ToolStripMenuItem ExitBtn;
        private System.Windows.Forms.ToolStripMenuItem TestComBtn;
        private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NewSimulatorBtn;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RunDemoBtn;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TestReturnHomeBtn;
        private System.Windows.Forms.ToolStripMenuItem MissionHmiBtn;
    }
}

