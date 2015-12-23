namespace Logic
{
    partial class Simulator
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
            this.components = new System.ComponentModel.Container();
            this.m_RefreshTimer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.missionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditorNewBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.m_EditPolygonBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.m_EditLauncherBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.m_EditRecoveryBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.m_EditGatewayBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.m_EditGatewayInBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.m_EditGatewayOutBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.m_EditorExit = new System.Windows.Forms.ToolStripMenuItem();
            this.missionToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.MissionConfigBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MissionReplanBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.MissionAbortBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewRouteBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewLauncherBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.playerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PlayerPlayBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.PlayerPauseBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.PlayerReset = new System.Windows.Forms.ToolStripMenuItem();
            this.m_SimView = new Logic.Controls.SimView();
            this.PlayerResetLogic = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_RefreshTimer
            // 
            this.m_RefreshTimer.Interval = 1000;
            this.m_RefreshTimer.Tick += new System.EventHandler(this.m_RefreshTimer_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.missionToolStripMenuItem,
            this.missionToolStripMenuItem1,
            this.viewToolStripMenuItem,
            this.playerToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(767, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // missionToolStripMenuItem
            // 
            this.missionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditorNewBtn,
            this.toolStripSeparator2,
            this.m_EditPolygonBtn,
            this.m_EditLauncherBtn,
            this.m_EditRecoveryBtn,
            this.m_EditGatewayBtn,
            this.m_EditorExit});
            this.missionToolStripMenuItem.Name = "missionToolStripMenuItem";
            this.missionToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.missionToolStripMenuItem.Text = "Editor";
            // 
            // EditorNewBtn
            // 
            this.EditorNewBtn.Name = "EditorNewBtn";
            this.EditorNewBtn.Size = new System.Drawing.Size(123, 22);
            this.EditorNewBtn.Text = "New";
            this.EditorNewBtn.Click += new System.EventHandler(this.EditorNewBtn_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(120, 6);
            // 
            // m_EditPolygonBtn
            // 
            this.m_EditPolygonBtn.Name = "m_EditPolygonBtn";
            this.m_EditPolygonBtn.Size = new System.Drawing.Size(123, 22);
            this.m_EditPolygonBtn.Text = "Polygon";
            this.m_EditPolygonBtn.Click += new System.EventHandler(this.m_EditPolygonBtn_Click);
            // 
            // m_EditLauncherBtn
            // 
            this.m_EditLauncherBtn.Name = "m_EditLauncherBtn";
            this.m_EditLauncherBtn.Size = new System.Drawing.Size(123, 22);
            this.m_EditLauncherBtn.Text = "Launcher";
            this.m_EditLauncherBtn.Click += new System.EventHandler(this.m_EditLauncherBtn_Click);
            // 
            // m_EditRecoveryBtn
            // 
            this.m_EditRecoveryBtn.Name = "m_EditRecoveryBtn";
            this.m_EditRecoveryBtn.Size = new System.Drawing.Size(123, 22);
            this.m_EditRecoveryBtn.Text = "Recovery";
            this.m_EditRecoveryBtn.Click += new System.EventHandler(this.m_EditRecoveryBtn_Click);
            // 
            // m_EditGatewayBtn
            // 
            this.m_EditGatewayBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_EditGatewayInBtn,
            this.m_EditGatewayOutBtn});
            this.m_EditGatewayBtn.Name = "m_EditGatewayBtn";
            this.m_EditGatewayBtn.Size = new System.Drawing.Size(123, 22);
            this.m_EditGatewayBtn.Text = "Gateway";
            // 
            // m_EditGatewayInBtn
            // 
            this.m_EditGatewayInBtn.Name = "m_EditGatewayInBtn";
            this.m_EditGatewayInBtn.Size = new System.Drawing.Size(94, 22);
            this.m_EditGatewayInBtn.Text = "In";
            this.m_EditGatewayInBtn.Click += new System.EventHandler(this.m_EditGatewayInBtn_Click);
            // 
            // m_EditGatewayOutBtn
            // 
            this.m_EditGatewayOutBtn.Name = "m_EditGatewayOutBtn";
            this.m_EditGatewayOutBtn.Size = new System.Drawing.Size(94, 22);
            this.m_EditGatewayOutBtn.Text = "Out";
            this.m_EditGatewayOutBtn.Click += new System.EventHandler(this.m_EditGatewayOutBtn_Click);
            // 
            // m_EditorExit
            // 
            this.m_EditorExit.Name = "m_EditorExit";
            this.m_EditorExit.Size = new System.Drawing.Size(123, 22);
            this.m_EditorExit.Text = "Exit";
            this.m_EditorExit.Click += new System.EventHandler(this.m_EditorExit_Click);
            // 
            // missionToolStripMenuItem1
            // 
            this.missionToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MissionConfigBtn,
            this.toolStripSeparator1,
            this.MissionReplanBtn,
            this.MissionAbortBtn});
            this.missionToolStripMenuItem1.Name = "missionToolStripMenuItem1";
            this.missionToolStripMenuItem1.Size = new System.Drawing.Size(60, 20);
            this.missionToolStripMenuItem1.Text = "Mission";
            // 
            // MissionConfigBtn
            // 
            this.MissionConfigBtn.Name = "MissionConfigBtn";
            this.MissionConfigBtn.Size = new System.Drawing.Size(145, 22);
            this.MissionConfigBtn.Text = "Config";
            this.MissionConfigBtn.Click += new System.EventHandler(this.MissionConfigBtn_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(142, 6);
            // 
            // MissionReplanBtn
            // 
            this.MissionReplanBtn.Name = "MissionReplanBtn";
            this.MissionReplanBtn.Size = new System.Drawing.Size(145, 22);
            this.MissionReplanBtn.Text = "Start \\ Replan";
            this.MissionReplanBtn.Click += new System.EventHandler(this.PlayerReplanBtn_Click);
            // 
            // MissionAbortBtn
            // 
            this.MissionAbortBtn.Name = "MissionAbortBtn";
            this.MissionAbortBtn.Size = new System.Drawing.Size(145, 22);
            this.MissionAbortBtn.Text = "Abort";
            this.MissionAbortBtn.Click += new System.EventHandler(this.PlayerAbortBtn_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewRouteBtn,
            this.ViewLauncherBtn});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // ViewRouteBtn
            // 
            this.ViewRouteBtn.Checked = true;
            this.ViewRouteBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ViewRouteBtn.Name = "ViewRouteBtn";
            this.ViewRouteBtn.Size = new System.Drawing.Size(123, 22);
            this.ViewRouteBtn.Text = "Route";
            this.ViewRouteBtn.Click += new System.EventHandler(this.ViewRouteBtn_Click);
            // 
            // ViewLauncherBtn
            // 
            this.ViewLauncherBtn.Checked = true;
            this.ViewLauncherBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ViewLauncherBtn.Name = "ViewLauncherBtn";
            this.ViewLauncherBtn.Size = new System.Drawing.Size(123, 22);
            this.ViewLauncherBtn.Text = "Launcher";
            this.ViewLauncherBtn.Click += new System.EventHandler(this.ViewLauncherBtn_Click);
            // 
            // playerToolStripMenuItem
            // 
            this.playerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PlayerPlayBtn,
            this.PlayerPauseBtn,
            this.PlayerReset,
            this.PlayerResetLogic});
            this.playerToolStripMenuItem.Name = "playerToolStripMenuItem";
            this.playerToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.playerToolStripMenuItem.Text = "Player";
            // 
            // PlayerPlayBtn
            // 
            this.PlayerPlayBtn.Enabled = false;
            this.PlayerPlayBtn.Name = "PlayerPlayBtn";
            this.PlayerPlayBtn.Size = new System.Drawing.Size(152, 22);
            this.PlayerPlayBtn.Text = "Play";
            this.PlayerPlayBtn.Click += new System.EventHandler(this.PlayerPlayBtn_Click);
            // 
            // PlayerPauseBtn
            // 
            this.PlayerPauseBtn.Enabled = false;
            this.PlayerPauseBtn.Name = "PlayerPauseBtn";
            this.PlayerPauseBtn.Size = new System.Drawing.Size(152, 22);
            this.PlayerPauseBtn.Text = "Pause";
            this.PlayerPauseBtn.Click += new System.EventHandler(this.PlayerPauseBtn_Click);
            // 
            // PlayerReset
            // 
            this.PlayerReset.Name = "PlayerReset";
            this.PlayerReset.Size = new System.Drawing.Size(152, 22);
            this.PlayerReset.Text = "Reset";
            this.PlayerReset.Click += new System.EventHandler(this.PlayerReset_Click);
            // 
            // m_SimView
            // 
            this.m_SimView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_SimView.Location = new System.Drawing.Point(0, 24);
            this.m_SimView.Name = "m_SimView";
            this.m_SimView.Size = new System.Drawing.Size(767, 603);
            this.m_SimView.TabIndex = 0;
            this.m_SimView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_SimView_MouseDown);
            // 
            // PlayerResetLogic
            // 
            this.PlayerResetLogic.Name = "PlayerResetLogic";
            this.PlayerResetLogic.Size = new System.Drawing.Size(152, 22);
            this.PlayerResetLogic.Text = "Reset Logic";
            this.PlayerResetLogic.Click += new System.EventHandler(this.PlayerResetLogic_Click);
            // 
            // Simulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 627);
            this.Controls.Add(this.m_SimView);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Simulator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Simulator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Simulator_FormClosing);
            this.SizeChanged += new System.EventHandler(this.Simulator_SizeChanged);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer m_RefreshTimer;
        private Controls.SimView m_SimView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem missionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_EditPolygonBtn;
        private System.Windows.Forms.ToolStripMenuItem m_EditLauncherBtn;
        private System.Windows.Forms.ToolStripMenuItem m_EditRecoveryBtn;
        private System.Windows.Forms.ToolStripMenuItem m_EditGatewayBtn;
        private System.Windows.Forms.ToolStripMenuItem m_EditorExit;
        private System.Windows.Forms.ToolStripMenuItem missionToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem MissionConfigBtn;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ViewRouteBtn;
        private System.Windows.Forms.ToolStripMenuItem ViewLauncherBtn;
        private System.Windows.Forms.ToolStripMenuItem playerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PlayerPlayBtn;
        private System.Windows.Forms.ToolStripMenuItem PlayerPauseBtn;
        private System.Windows.Forms.ToolStripMenuItem MissionReplanBtn;
        private System.Windows.Forms.ToolStripMenuItem MissionAbortBtn;
        private System.Windows.Forms.ToolStripMenuItem PlayerReset;
        private System.Windows.Forms.ToolStripMenuItem m_EditGatewayInBtn;
        private System.Windows.Forms.ToolStripMenuItem m_EditGatewayOutBtn;
        private System.Windows.Forms.ToolStripMenuItem EditorNewBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem PlayerResetLogic;
    }
}