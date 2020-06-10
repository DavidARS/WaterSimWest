namespace WaterSimDCDC.Controls
{
    /// <summary>   Chart for display WaterSim results. </summary>
    public partial class WaterSimChartControl
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.WaterSimChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.PopUpContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lineWidthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.increaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decreaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chartTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LineChartMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StackedChartMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyChartMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveChartToFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.WaterSimChart)).BeginInit();
            this.PopUpContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // WaterSimChart
            // 
            this.WaterSimChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WaterSimChart.BorderlineColor = System.Drawing.Color.Black;
            chartArea1.Name = "ChartArea1";
            this.WaterSimChart.ChartAreas.Add(chartArea1);
            this.WaterSimChart.ContextMenuStrip = this.PopUpContextMenuStrip;
            legend1.Name = "Legend1";
            this.WaterSimChart.Legends.Add(legend1);
            this.WaterSimChart.Location = new System.Drawing.Point(3, 3);
            this.WaterSimChart.Name = "WaterSimChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.YValuesPerPoint = 6;
            this.WaterSimChart.Series.Add(series1);
            this.WaterSimChart.Size = new System.Drawing.Size(357, 257);
            this.WaterSimChart.TabIndex = 0;
            this.WaterSimChart.Text = "Chart";
            this.WaterSimChart.GetToolTipText += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs>(this.WaterSimChart_GetToolTipText);
            this.WaterSimChart.Click += new System.EventHandler(this.WaterSimChart_Click);
            // 
            // PopUpContextMenuStrip
            // 
            this.PopUpContextMenuStrip.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.PopUpContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lineWidthToolStripMenuItem,
            this.chartTypeToolStripMenuItem,
            this.CopyChartMenuItem,
            this.saveAsMenuItem});
            this.PopUpContextMenuStrip.Name = "PopUpContextMenuStrip";
            this.PopUpContextMenuStrip.Size = new System.Drawing.Size(190, 140);
            // 
            // lineWidthToolStripMenuItem
            // 
            this.lineWidthToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.increaseToolStripMenuItem,
            this.decreaseToolStripMenuItem});
            this.lineWidthToolStripMenuItem.Name = "lineWidthToolStripMenuItem";
            this.lineWidthToolStripMenuItem.Size = new System.Drawing.Size(189, 34);
            this.lineWidthToolStripMenuItem.Text = "Line Width";
            // 
            // increaseToolStripMenuItem
            // 
            this.increaseToolStripMenuItem.Name = "increaseToolStripMenuItem";
            this.increaseToolStripMenuItem.Size = new System.Drawing.Size(189, 34);
            this.increaseToolStripMenuItem.Text = "Increase";
            this.increaseToolStripMenuItem.Click += new System.EventHandler(this.increaseToolStripMenuItem_Click);
            // 
            // decreaseToolStripMenuItem
            // 
            this.decreaseToolStripMenuItem.Name = "decreaseToolStripMenuItem";
            this.decreaseToolStripMenuItem.Size = new System.Drawing.Size(189, 34);
            this.decreaseToolStripMenuItem.Text = "Decrease";
            this.decreaseToolStripMenuItem.Click += new System.EventHandler(this.decreaseToolStripMenuItem_Click);
            // 
            // chartTypeToolStripMenuItem
            // 
            this.chartTypeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LineChartMenuItem,
            this.StackedChartMenuItem});
            this.chartTypeToolStripMenuItem.Name = "chartTypeToolStripMenuItem";
            this.chartTypeToolStripMenuItem.Size = new System.Drawing.Size(189, 34);
            this.chartTypeToolStripMenuItem.Text = "Chart Type";
            // 
            // LineChartMenuItem
            // 
            this.LineChartMenuItem.Name = "LineChartMenuItem";
            this.LineChartMenuItem.Size = new System.Drawing.Size(280, 34);
            this.LineChartMenuItem.Text = "Line Chart";
            this.LineChartMenuItem.Click += new System.EventHandler(this.LineChartMenuItem_Click);
            // 
            // StackedChartMenuItem
            // 
            this.StackedChartMenuItem.Name = "StackedChartMenuItem";
            this.StackedChartMenuItem.Size = new System.Drawing.Size(280, 34);
            this.StackedChartMenuItem.Text = "Stacked Area Chart";
            this.StackedChartMenuItem.Click += new System.EventHandler(this.StackedChartMenuItem_Click);
            // 
            // CopyChartMenuItem
            // 
            this.CopyChartMenuItem.Name = "CopyChartMenuItem";
            this.CopyChartMenuItem.Size = new System.Drawing.Size(189, 34);
            this.CopyChartMenuItem.Text = "Copy Chart";
            this.CopyChartMenuItem.Click += new System.EventHandler(this.CopyChartMenuItem_Click);
            // 
            // saveAsMenuItem
            // 
            this.saveAsMenuItem.Name = "saveAsMenuItem";
            this.saveAsMenuItem.Size = new System.Drawing.Size(189, 34);
            this.saveAsMenuItem.Text = "Save as";
            this.saveAsMenuItem.Click += new System.EventHandler(this.saveAsMenuItem_Click);
            // 
            // SaveChartToFileDialog
            // 
            this.SaveChartToFileDialog.Filter = "jpg files|*.jpg|bitmap files|*.bmp|emf files|*.emf|png files|*.png|tif files|*.ti" +
    "f";
            this.SaveChartToFileDialog.Title = "Save Chart To File";
            // 
            // WaterSimChartControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.WaterSimChart);
            this.Name = "WaterSimChartControl";
            this.Size = new System.Drawing.Size(409, 314);
            ((System.ComponentModel.ISupportInitialize)(this.WaterSimChart)).EndInit();
            this.PopUpContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart WaterSimChart;
        private System.Windows.Forms.ContextMenuStrip PopUpContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem lineWidthToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem increaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decreaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chartTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LineChartMenuItem;
        private System.Windows.Forms.ToolStripMenuItem StackedChartMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CopyChartMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsMenuItem;
        private System.Windows.Forms.SaveFileDialog SaveChartToFileDialog;
    }
}
