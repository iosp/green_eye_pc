namespace Logic.Forms
{
    partial class MissionConfigForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.FlightHeightTxt = new System.Windows.Forms.NumericUpDown();
            this.MissionHeightTxt = new System.Windows.Forms.NumericUpDown();
            this.ProjectionTxt = new System.Windows.Forms.NumericUpDown();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.MissionDurationTxt = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.LaunchDelayTxt = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.FlightHeightTxt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MissionHeightTxt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProjectionTxt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MissionDurationTxt)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LaunchDelayTxt)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Flight Height [m]";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Misison Height [m]";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Projection [m]";
            // 
            // FlightHeightTxt
            // 
            this.FlightHeightTxt.DecimalPlaces = 2;
            this.FlightHeightTxt.Location = new System.Drawing.Point(112, 23);
            this.FlightHeightTxt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FlightHeightTxt.Name = "FlightHeightTxt";
            this.FlightHeightTxt.Size = new System.Drawing.Size(67, 20);
            this.FlightHeightTxt.TabIndex = 3;
            this.FlightHeightTxt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // MissionHeightTxt
            // 
            this.MissionHeightTxt.DecimalPlaces = 2;
            this.MissionHeightTxt.Location = new System.Drawing.Point(112, 49);
            this.MissionHeightTxt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MissionHeightTxt.Name = "MissionHeightTxt";
            this.MissionHeightTxt.Size = new System.Drawing.Size(67, 20);
            this.MissionHeightTxt.TabIndex = 4;
            this.MissionHeightTxt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ProjectionTxt
            // 
            this.ProjectionTxt.DecimalPlaces = 2;
            this.ProjectionTxt.Location = new System.Drawing.Point(112, 75);
            this.ProjectionTxt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ProjectionTxt.Name = "ProjectionTxt";
            this.ProjectionTxt.Size = new System.Drawing.Size(67, 20);
            this.ProjectionTxt.TabIndex = 5;
            this.ProjectionTxt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(64, 292);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(75, 23);
            this.SaveBtn.TabIndex = 6;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // MissionDurationTxt
            // 
            this.MissionDurationTxt.DecimalPlaces = 2;
            this.MissionDurationTxt.Location = new System.Drawing.Point(112, 24);
            this.MissionDurationTxt.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.MissionDurationTxt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MissionDurationTxt.Name = "MissionDurationTxt";
            this.MissionDurationTxt.Size = new System.Drawing.Size(67, 20);
            this.MissionDurationTxt.TabIndex = 8;
            this.MissionDurationTxt.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Duration [sec]";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.FlightHeightTxt);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.MissionHeightTxt);
            this.groupBox1.Controls.Add(this.ProjectionTxt);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 108);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Flight";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.MissionDurationTxt);
            this.groupBox2.Location = new System.Drawing.Point(12, 126);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 82);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mission";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.LaunchDelayTxt);
            this.groupBox3.Location = new System.Drawing.Point(12, 214);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 60);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Launcher";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Delay [s]";
            // 
            // LaunchDelayTxt
            // 
            this.LaunchDelayTxt.DecimalPlaces = 2;
            this.LaunchDelayTxt.Location = new System.Drawing.Point(112, 24);
            this.LaunchDelayTxt.Name = "LaunchDelayTxt";
            this.LaunchDelayTxt.Size = new System.Drawing.Size(67, 20);
            this.LaunchDelayTxt.TabIndex = 8;
            this.LaunchDelayTxt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // MissionConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(229, 326);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.SaveBtn);
            this.Name = "MissionConfigForm";
            this.Text = "MissionConfigForm";
            ((System.ComponentModel.ISupportInitialize)(this.FlightHeightTxt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MissionHeightTxt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProjectionTxt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MissionDurationTxt)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LaunchDelayTxt)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown FlightHeightTxt;
        private System.Windows.Forms.NumericUpDown MissionHeightTxt;
        private System.Windows.Forms.NumericUpDown ProjectionTxt;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.NumericUpDown MissionDurationTxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown LaunchDelayTxt;
    }
}