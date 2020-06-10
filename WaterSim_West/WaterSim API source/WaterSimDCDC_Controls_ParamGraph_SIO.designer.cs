namespace WaterSimDCDC.Controls
{
    partial class ParameterDashboardChart
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
            this.ParmGraphPanel = new System.Windows.Forms.Panel();
            this.PanelDashboard = new System.Windows.Forms.Panel();
            this.waterSimChartControl1 = new WaterSimDCDC.Controls.WaterSimChartControl();
            this.ProviderPanel = new System.Windows.Forms.Panel();
            this.ProviderCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.ExpandProviderListPanel = new System.Windows.Forms.Button();
            this.SelectAllProvButton = new System.Windows.Forms.Button();
            this.ClearAllButton = new System.Windows.Forms.Button();
            this.DrawParmGraphButton = new System.Windows.Forms.Button();
            this.ParmLabel = new System.Windows.Forms.Label();
            this.ParmComboBox = new System.Windows.Forms.ComboBox();
            this.toolStripTableScenarioName = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelTableName = new System.Windows.Forms.ToolStripLabel();
            this.ParmTablenameComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabelScenarioName = new System.Windows.Forms.ToolStripLabel();
            this.ScenarioNamesComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ParmGraphPanel.SuspendLayout();
            this.PanelDashboard.SuspendLayout();
            this.ProviderPanel.SuspendLayout();
            this.toolStripTableScenarioName.SuspendLayout();
            this.SuspendLayout();
            // 
            // ParmGraphPanel
            // 
            this.ParmGraphPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ParmGraphPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ParmGraphPanel.Controls.Add(this.PanelDashboard);
            this.ParmGraphPanel.Controls.Add(this.toolStripTableScenarioName);
            this.ParmGraphPanel.Controls.Add(this.statusStrip1);
            this.ParmGraphPanel.Location = new System.Drawing.Point(3, 3);
            this.ParmGraphPanel.Name = "ParmGraphPanel";
            this.ParmGraphPanel.Size = new System.Drawing.Size(613, 339);
            this.ParmGraphPanel.TabIndex = 0;
            // 
            // PanelDashboard
            // 
            this.PanelDashboard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelDashboard.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PanelDashboard.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.PanelDashboard.Controls.Add(this.waterSimChartControl1);
            this.PanelDashboard.Controls.Add(this.ProviderPanel);
            this.PanelDashboard.Controls.Add(this.SelectAllProvButton);
            this.PanelDashboard.Controls.Add(this.ClearAllButton);
            this.PanelDashboard.Controls.Add(this.DrawParmGraphButton);
            this.PanelDashboard.Controls.Add(this.ParmLabel);
            this.PanelDashboard.Controls.Add(this.ParmComboBox);
            this.PanelDashboard.Location = new System.Drawing.Point(3, 29);
            this.PanelDashboard.Name = "PanelDashboard";
            this.PanelDashboard.Size = new System.Drawing.Size(607, 285);
            this.PanelDashboard.TabIndex = 16;
            // 
            // waterSimChartControl1
            // 
            this.waterSimChartControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.waterSimChartControl1.ChartData = null;
            this.waterSimChartControl1.ChartTitle = "";
            this.waterSimChartControl1.Location = new System.Drawing.Point(338, 38);
            this.waterSimChartControl1.Name = "waterSimChartControl1";
            this.waterSimChartControl1.Size = new System.Drawing.Size(266, 244);
            this.waterSimChartControl1.TabIndex = 15;
            // 
            // ProviderPanel
            // 
            this.ProviderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ProviderPanel.Controls.Add(this.ProviderCheckedListBox);
            this.ProviderPanel.Controls.Add(this.ExpandProviderListPanel);
            this.ProviderPanel.Location = new System.Drawing.Point(3, 35);
            this.ProviderPanel.Name = "ProviderPanel";
            this.ProviderPanel.Size = new System.Drawing.Size(329, 247);
            this.ProviderPanel.TabIndex = 14;
            // 
            // ProviderCheckedListBox
            // 
            this.ProviderCheckedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProviderCheckedListBox.CheckOnClick = true;
            this.ProviderCheckedListBox.FormattingEnabled = true;
            this.ProviderCheckedListBox.Location = new System.Drawing.Point(0, 3);
            this.ProviderCheckedListBox.Name = "ProviderCheckedListBox";
            this.ProviderCheckedListBox.Size = new System.Drawing.Size(298, 169);
            this.ProviderCheckedListBox.TabIndex = 6;
            // 
            // ExpandProviderListPanel
            // 
            this.ExpandProviderListPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExpandProviderListPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ExpandProviderListPanel.Location = new System.Drawing.Point(301, 3);
            this.ExpandProviderListPanel.Name = "ExpandProviderListPanel";
            this.ExpandProviderListPanel.Size = new System.Drawing.Size(25, 244);
            this.ExpandProviderListPanel.TabIndex = 5;
            this.ExpandProviderListPanel.Text = "<";
            this.ExpandProviderListPanel.UseVisualStyleBackColor = true;
            this.ExpandProviderListPanel.Click += new System.EventHandler(this.ExpandProviderList_Click);
            // 
            // SelectAllProvButton
            // 
            this.SelectAllProvButton.Location = new System.Drawing.Point(0, 5);
            this.SelectAllProvButton.Name = "SelectAllProvButton";
            this.SelectAllProvButton.Size = new System.Drawing.Size(75, 23);
            this.SelectAllProvButton.TabIndex = 12;
            this.SelectAllProvButton.Text = "Select All";
            this.SelectAllProvButton.UseVisualStyleBackColor = true;
            this.SelectAllProvButton.Click += new System.EventHandler(this.SelectAllProvButton_Click);
            // 
            // ClearAllButton
            // 
            this.ClearAllButton.Location = new System.Drawing.Point(79, 5);
            this.ClearAllButton.Name = "ClearAllButton";
            this.ClearAllButton.Size = new System.Drawing.Size(75, 23);
            this.ClearAllButton.TabIndex = 13;
            this.ClearAllButton.Text = "Clear All";
            this.ClearAllButton.UseVisualStyleBackColor = true;
            this.ClearAllButton.Click += new System.EventHandler(this.ClearAllButton_Click);
            // 
            // DrawParmGraphButton
            // 
            this.DrawParmGraphButton.Location = new System.Drawing.Point(158, 5);
            this.DrawParmGraphButton.Name = "DrawParmGraphButton";
            this.DrawParmGraphButton.Size = new System.Drawing.Size(75, 23);
            this.DrawParmGraphButton.TabIndex = 11;
            this.DrawParmGraphButton.Text = "Draw Graph";
            this.DrawParmGraphButton.UseVisualStyleBackColor = true;
            this.DrawParmGraphButton.Click += new System.EventHandler(this.DrawGraphButton_Click);
            // 
            // ParmLabel
            // 
            this.ParmLabel.AutoSize = true;
            this.ParmLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ParmLabel.Location = new System.Drawing.Point(245, 8);
            this.ParmLabel.Name = "ParmLabel";
            this.ParmLabel.Size = new System.Drawing.Size(74, 17);
            this.ParmLabel.TabIndex = 10;
            this.ParmLabel.Text = "Parameter";
            // 
            // ParmComboBox
            // 
            this.ParmComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ParmComboBox.FormattingEnabled = true;
            this.ParmComboBox.Location = new System.Drawing.Point(325, 5);
            this.ParmComboBox.Name = "ParmComboBox";
            this.ParmComboBox.Size = new System.Drawing.Size(255, 21);
            this.ParmComboBox.TabIndex = 9;
            // 
            // toolStripTableScenarioName
            // 
            this.toolStripTableScenarioName.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStripTableScenarioName.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelTableName,
            this.ParmTablenameComboBox,
            this.toolStripLabelScenarioName,
            this.ScenarioNamesComboBox});
            this.toolStripTableScenarioName.Location = new System.Drawing.Point(0, 0);
            this.toolStripTableScenarioName.Name = "toolStripTableScenarioName";
            this.toolStripTableScenarioName.Size = new System.Drawing.Size(613, 25);
            this.toolStripTableScenarioName.TabIndex = 15;
            this.toolStripTableScenarioName.Text = "toolStrip1";
            // 
            // toolStripLabelTableName
            // 
            this.toolStripLabelTableName.Name = "toolStripLabelTableName";
            this.toolStripLabelTableName.Size = new System.Drawing.Size(71, 22);
            this.toolStripLabelTableName.Text = "Table Name";
            // 
            // ParmTablenameComboBox
            // 
            this.ParmTablenameComboBox.DropDownWidth = 300;
            this.ParmTablenameComboBox.Name = "ParmTablenameComboBox";
            this.ParmTablenameComboBox.Size = new System.Drawing.Size(200, 25);
            this.ParmTablenameComboBox.SelectedIndexChanged += new System.EventHandler(this.TablenameComboBox_SelectedIndexChanged);
            // 
            // toolStripLabelScenarioName
            // 
            this.toolStripLabelScenarioName.Name = "toolStripLabelScenarioName";
            this.toolStripLabelScenarioName.Size = new System.Drawing.Size(87, 22);
            this.toolStripLabelScenarioName.Text = "Scenario Name";
            // 
            // ScenarioNamesComboBox
            // 
            this.ScenarioNamesComboBox.DropDownWidth = 400;
            this.ScenarioNamesComboBox.Name = "ScenarioNamesComboBox";
            this.ScenarioNamesComboBox.Size = new System.Drawing.Size(200, 25);
            this.ScenarioNamesComboBox.Click += new System.EventHandler(this.ScenarioNamesComboBox_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 317);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(613, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ParameterDashboardChart
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.ParmGraphPanel);
            this.Name = "ParameterDashboardChart";
            this.Size = new System.Drawing.Size(623, 342);
            this.ParmGraphPanel.ResumeLayout(false);
            this.ParmGraphPanel.PerformLayout();
            this.PanelDashboard.ResumeLayout(false);
            this.PanelDashboard.PerformLayout();
            this.ProviderPanel.ResumeLayout(false);
            this.toolStripTableScenarioName.ResumeLayout(false);
            this.toolStripTableScenarioName.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ParmGraphPanel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStripTableScenarioName;
        private System.Windows.Forms.ToolStripLabel toolStripLabelTableName;
        private System.Windows.Forms.ToolStripComboBox ParmTablenameComboBox;
        private System.Windows.Forms.ToolStripLabel toolStripLabelScenarioName;
        private System.Windows.Forms.ToolStripComboBox ScenarioNamesComboBox;
        private System.Windows.Forms.Panel PanelDashboard;
        private System.Windows.Forms.Panel ProviderPanel;
        private System.Windows.Forms.CheckedListBox ProviderCheckedListBox;
        private System.Windows.Forms.Button ExpandProviderListPanel;
        private System.Windows.Forms.Button SelectAllProvButton;
        private System.Windows.Forms.Button ClearAllButton;
        private System.Windows.Forms.Button DrawParmGraphButton;
        private System.Windows.Forms.Label ParmLabel;
        private System.Windows.Forms.ComboBox ParmComboBox;
        private WaterSimChartControl waterSimChartControl1;
    }
}
