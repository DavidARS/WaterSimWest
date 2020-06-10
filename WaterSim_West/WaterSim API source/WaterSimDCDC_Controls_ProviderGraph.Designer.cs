namespace WaterSimDCDC.Controls
{
    /// <summary>   A chart for displaying WaterSim output and input for a specific provider. </summary>
    public partial class ProviderDashBoardChart
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
            this.ProviderDashBoardPanel = new System.Windows.Forms.Panel();
            this.ExpandParameterList = new System.Windows.Forms.Button();
            this.ProvidernamesComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ParameterPanel = new System.Windows.Forms.Panel();
            this.DrawGraphButton = new System.Windows.Forms.Button();
            this.ClearParmListButton = new System.Windows.Forms.Button();
            this.toolStripTablename = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.TablenameComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.ScenarioNamesComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.waterSimChartControl1 = new WaterSimDCDC.Controls.WaterSimChartControl();
            this.parameterTreeView1 = new WaterSimDCDC.Controls.ParameterTreeView();
            this.ParameterPanel.SuspendLayout();
            this.toolStripTablename.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProviderDashBoardPanel
            // 
            this.ProviderDashBoardPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProviderDashBoardPanel.AutoSize = true;
            this.ProviderDashBoardPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ProviderDashBoardPanel.Location = new System.Drawing.Point(0, 0);
            this.ProviderDashBoardPanel.Name = "ProviderDashBoardPanel";
            this.ProviderDashBoardPanel.Size = new System.Drawing.Size(0, 0);
            this.ProviderDashBoardPanel.TabIndex = 0;
            // 
            // ExpandParameterList
            // 
            this.ExpandParameterList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExpandParameterList.Location = new System.Drawing.Point(390, 3);
            this.ExpandParameterList.Name = "ExpandParameterList";
            this.ExpandParameterList.Size = new System.Drawing.Size(25, 419);
            this.ExpandParameterList.TabIndex = 4;
            this.ExpandParameterList.Text = "<";
            this.ExpandParameterList.UseVisualStyleBackColor = true;
            this.ExpandParameterList.Click += new System.EventHandler(this.ExpandParameterList_Click);
            // 
            // ProvidernamesComboBox
            // 
            this.ProvidernamesComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProvidernamesComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProvidernamesComboBox.FormattingEnabled = true;
            this.ProvidernamesComboBox.Location = new System.Drawing.Point(427, 31);
            this.ProvidernamesComboBox.Name = "ProvidernamesComboBox";
            this.ProvidernamesComboBox.Size = new System.Drawing.Size(449, 24);
            this.ProvidernamesComboBox.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(253, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Provider/Uility";
            // 
            // ParameterPanel
            // 
            this.ParameterPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ParameterPanel.Controls.Add(this.parameterTreeView1);
            this.ParameterPanel.Controls.Add(this.ExpandParameterList);
            this.ParameterPanel.Location = new System.Drawing.Point(6, 64);
            this.ParameterPanel.Name = "ParameterPanel";
            this.ParameterPanel.Size = new System.Drawing.Size(415, 425);
            this.ParameterPanel.TabIndex = 8;
            // 
            // DrawGraphButton
            // 
            this.DrawGraphButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DrawGraphButton.Location = new System.Drawing.Point(124, 34);
            this.DrawGraphButton.Name = "DrawGraphButton";
            this.DrawGraphButton.Size = new System.Drawing.Size(112, 30);
            this.DrawGraphButton.TabIndex = 10;
            this.DrawGraphButton.Text = "Draw Graph";
            this.DrawGraphButton.UseVisualStyleBackColor = true;
            this.DrawGraphButton.Click += new System.EventHandler(this.DrawGraphButton_Click);
            // 
            // ClearParmListButton
            // 
            this.ClearParmListButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClearParmListButton.Location = new System.Drawing.Point(9, 35);
            this.ClearParmListButton.Name = "ClearParmListButton";
            this.ClearParmListButton.Size = new System.Drawing.Size(105, 29);
            this.ClearParmListButton.TabIndex = 18;
            this.ClearParmListButton.Text = "Clear";
            this.ClearParmListButton.UseVisualStyleBackColor = true;
            // 
            // toolStripTablename
            // 
            this.toolStripTablename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStripTablename.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripTablename.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.TablenameComboBox,
            this.toolStripLabel2,
            this.ScenarioNamesComboBox});
            this.toolStripTablename.Location = new System.Drawing.Point(0, 0);
            this.toolStripTablename.Name = "toolStripTablename";
            this.toolStripTablename.Size = new System.Drawing.Size(769, 25);
            this.toolStripTablename.TabIndex = 19;
            this.toolStripTablename.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(66, 22);
            this.toolStripLabel1.Text = "Tablename";
            // 
            // TablenameComboBox
            // 
            this.TablenameComboBox.Name = "TablenameComboBox";
            this.TablenameComboBox.Size = new System.Drawing.Size(300, 25);
            this.TablenameComboBox.SelectedIndexChanged += new System.EventHandler(this.TablenameComboBox_SelectedIndexChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(87, 22);
            this.toolStripLabel2.Text = "Screnaio Name";
            // 
            // ScenarioNamesComboBox
            // 
            this.ScenarioNamesComboBox.Name = "ScenarioNamesComboBox";
            this.ScenarioNamesComboBox.Size = new System.Drawing.Size(300, 25);
            this.ScenarioNamesComboBox.SelectedIndexChanged += new System.EventHandler(this.ScenarioNamesComboBox_SelectedIndexChanged);
            // 
            // waterSimChartControl1
            // 
            this.waterSimChartControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.waterSimChartControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.waterSimChartControl1.ChartData = null;
            this.waterSimChartControl1.ChartTitle = "";
            this.waterSimChartControl1.Location = new System.Drawing.Point(427, 62);
            this.waterSimChartControl1.Name = "waterSimChartControl1";
            this.waterSimChartControl1.Size = new System.Drawing.Size(449, 412);
            this.waterSimChartControl1.TabIndex = 9;
            // 
            // parameterTreeView1
            // 
            this.parameterTreeView1.AllowGroupCheck = true;
            this.parameterTreeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.parameterTreeView1.Location = new System.Drawing.Point(3, 5);
            this.parameterTreeView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.parameterTreeView1.Name = "parameterTreeView1";
            this.parameterTreeView1.ParameterManager = null;
            this.parameterTreeView1.ShowFieldNames = WaterSimDCDC.Controls.eShowFieldName.sfHide;
            this.parameterTreeView1.Size = new System.Drawing.Size(381, 417);
            this.parameterTreeView1.TabIndex = 5;
            this.parameterTreeView1.UseCheckBoxes = false;
            // 
            // ProviderDashBoardChart
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.toolStripTablename);
            this.Controls.Add(this.ClearParmListButton);
            this.Controls.Add(this.DrawGraphButton);
            this.Controls.Add(this.waterSimChartControl1);
            this.Controls.Add(this.ParameterPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ProvidernamesComboBox);
            this.Controls.Add(this.ProviderDashBoardPanel);
            this.MinimumSize = new System.Drawing.Size(726, 150);
            this.Name = "ProviderDashBoardChart";
            this.Size = new System.Drawing.Size(893, 492);
            this.ParameterPanel.ResumeLayout(false);
            this.toolStripTablename.ResumeLayout(false);
            this.toolStripTablename.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel ProviderDashBoardPanel;
        private System.Windows.Forms.Button ExpandParameterList;
        private System.Windows.Forms.ComboBox ProvidernamesComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel ParameterPanel;
        private WaterSimDCDC.Controls.WaterSimChartControl waterSimChartControl1;
        private System.Windows.Forms.Button DrawGraphButton;
        private System.Windows.Forms.Button ClearParmListButton;
        private System.Windows.Forms.ToolStrip toolStripTablename;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox TablenameComboBox;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox ScenarioNamesComboBox;
        private ParameterTreeView parameterTreeView1;
    }
}
