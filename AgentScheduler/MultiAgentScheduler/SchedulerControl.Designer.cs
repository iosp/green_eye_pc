namespace MultiAgentScheduler
{
    partial class SchedulerControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.m_Graph = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.m_Graph)).BeginInit();
            this.SuspendLayout();
            // 
            // m_Graph
            // 
            chartArea1.Name = "ChartArea1";
            this.m_Graph.ChartAreas.Add(chartArea1);
            this.m_Graph.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.m_Graph.Legends.Add(legend1);
            this.m_Graph.Location = new System.Drawing.Point(0, 0);
            this.m_Graph.Name = "m_Graph";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.RangeBar;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.YValuesPerPoint = 2;
            this.m_Graph.Series.Add(series1);
            this.m_Graph.Size = new System.Drawing.Size(445, 393);
            this.m_Graph.TabIndex = 0;
            this.m_Graph.Text = "chart1";
            // 
            // SchedulerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_Graph);
            this.Name = "SchedulerControl";
            this.Size = new System.Drawing.Size(445, 393);
            ((System.ComponentModel.ISupportInitialize)(this.m_Graph)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart m_Graph;
    }
}
