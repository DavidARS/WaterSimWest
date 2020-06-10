﻿namespace WaterSim_West_v_1
{
    partial class Form1
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplaySankeyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tasksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runModelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Parameters = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panelUserControls = new System.Windows.Forms.Panel();
            this.panelInput = new System.Windows.Forms.Panel();
            this.flowLayoutPanelMPs = new System.Windows.Forms.FlowLayoutPanel();
            this.treeViewInput = new System.Windows.Forms.TreeView();
            this.panelIndicators = new System.Windows.Forms.Panel();
            this.listBoxIndicators = new System.Windows.Forms.ListBox();
            this.SanKeyGarphControlPanel = new System.Windows.Forms.Panel();
            this.SankeyGraphUnitNameLabel = new System.Windows.Forms.Label();
            this.SanKeyGraphcomboBox = new System.Windows.Forms.ComboBox();
            this.SankeyGraphPanel = new System.Windows.Forms.Panel();
            this.sankeyGraphUnit = new ConsumerResourceModelFramework.SankeyGraph();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.regionTreeViewClass1 = new WaterSimDCDC.WestVisual.RegionTreeViewClass();
            this.comboBoxParameters = new System.Windows.Forms.ComboBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ActionPage = new System.Windows.Forms.TabPage();
            this.buttonDoDrought = new System.Windows.Forms.Button();
            this.tabPageAssessment = new System.Windows.Forms.TabPage();
            this.panelGWColor = new System.Windows.Forms.Panel();
            this.labelGWValue = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.panelEnvColor = new System.Windows.Forms.Panel();
            this.labelEnvValue = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panelEcoCOlor = new System.Windows.Forms.Panel();
            this.labelEconomicValue = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.listBoxIndicators2 = new System.Windows.Forms.ListBox();
            this.panelAssessmentColor = new System.Windows.Forms.Panel();
            this.labelAssessmentPhrase = new System.Windows.Forms.Label();
            this.labelAssessmentValue = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panelIndicatorColor = new System.Windows.Forms.Panel();
            this.labelIndicatorPhrase = new System.Windows.Forms.Label();
            this.labelIndicatorValue = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panelBalanceColor = new System.Windows.Forms.Panel();
            this.labelBalancePhrase = new System.Windows.Forms.Label();
            this.labelBalanceValue = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.parameterTreeView1 = new WaterSimDCDC.WestVisual.ParameterTreeView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripTasks = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemNewFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.Parameters.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panelUserControls.SuspendLayout();
            this.panelInput.SuspendLayout();
            this.panelIndicators.SuspendLayout();
            this.SanKeyGarphControlPanel.SuspendLayout();
            this.SankeyGraphPanel.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.ActionPage.SuspendLayout();
            this.tabPageAssessment.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStripTasks.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.tasksToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(11, 4, 0, 4);
            this.menuStrip1.Size = new System.Drawing.Size(1932, 42);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItemNewFile,
            this.toolStripMenuItem1,
            this.DisplaySankeyMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(56, 34);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(283, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(286, 34);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // DisplaySankeyMenuItem
            // 
            this.DisplaySankeyMenuItem.Name = "DisplaySankeyMenuItem";
            this.DisplaySankeyMenuItem.Size = new System.Drawing.Size(286, 34);
            this.DisplaySankeyMenuItem.Text = "Display Unit Sankey";
            this.DisplaySankeyMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.DisplaySankeyMenuItem_DropDownItemClicked);
            // 
            // tasksToolStripMenuItem
            // 
            this.tasksToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runModelMenuItem,
            this.resetModelToolStripMenuItem});
            this.tasksToolStripMenuItem.Name = "tasksToolStripMenuItem";
            this.tasksToolStripMenuItem.Size = new System.Drawing.Size(73, 34);
            this.tasksToolStripMenuItem.Text = "&Tasks";
            // 
            // runModelMenuItem
            // 
            this.runModelMenuItem.Name = "runModelMenuItem";
            this.runModelMenuItem.Size = new System.Drawing.Size(219, 34);
            this.runModelMenuItem.Text = "&Run Model";
            this.runModelMenuItem.Click += new System.EventHandler(this.runModelToolStripMenuItem_Click);
            // 
            // resetModelToolStripMenuItem
            // 
            this.resetModelToolStripMenuItem.Name = "resetModelToolStripMenuItem";
            this.resetModelToolStripMenuItem.Size = new System.Drawing.Size(219, 34);
            this.resetModelToolStripMenuItem.Text = "Re&set Model";
            this.resetModelToolStripMenuItem.Click += new System.EventHandler(this.resetModelToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(98, 34);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // Parameters
            // 
            this.Parameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Parameters.Controls.Add(this.tabPage1);
            this.Parameters.Controls.Add(this.tabPage2);
            this.Parameters.Controls.Add(this.ActionPage);
            this.Parameters.Controls.Add(this.tabPageAssessment);
            this.Parameters.Controls.Add(this.tabPage3);
            this.Parameters.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Parameters.Location = new System.Drawing.Point(22, 122);
            this.Parameters.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Parameters.Name = "Parameters";
            this.Parameters.SelectedIndex = 0;
            this.Parameters.Size = new System.Drawing.Size(1910, 884);
            this.Parameters.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panelUserControls);
            this.tabPage1.Controls.Add(this.SankeyGraphPanel);
            this.tabPage1.Location = new System.Drawing.Point(4, 41);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPage1.Size = new System.Drawing.Size(1902, 839);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "SanKey Graph";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panelUserControls
            // 
            this.panelUserControls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.panelUserControls.Controls.Add(this.panelInput);
            this.panelUserControls.Controls.Add(this.panelIndicators);
            this.panelUserControls.Controls.Add(this.SanKeyGarphControlPanel);
            this.panelUserControls.Location = new System.Drawing.Point(761, 4);
            this.panelUserControls.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelUserControls.Name = "panelUserControls";
            this.panelUserControls.Size = new System.Drawing.Size(1038, 827);
            this.panelUserControls.TabIndex = 4;
            this.panelUserControls.Resize += new System.EventHandler(this.panelUserControls_Resize);
            // 
            // panelInput
            // 
            this.panelInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelInput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelInput.Controls.Add(this.flowLayoutPanelMPs);
            this.panelInput.Controls.Add(this.treeViewInput);
            this.panelInput.Location = new System.Drawing.Point(18, 465);
            this.panelInput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelInput.Name = "panelInput";
            this.panelInput.Size = new System.Drawing.Size(994, 345);
            this.panelInput.TabIndex = 4;
            this.panelInput.Resize += new System.EventHandler(this.panelInput_Resize);
            // 
            // flowLayoutPanelMPs
            // 
            this.flowLayoutPanelMPs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelMPs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelMPs.BackColor = System.Drawing.Color.LightGray;
            this.flowLayoutPanelMPs.Location = new System.Drawing.Point(361, 4);
            this.flowLayoutPanelMPs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flowLayoutPanelMPs.Name = "flowLayoutPanelMPs";
            this.flowLayoutPanelMPs.Size = new System.Drawing.Size(576, 312);
            this.flowLayoutPanelMPs.TabIndex = 1;
            this.flowLayoutPanelMPs.Resize += new System.EventHandler(this.flowLayoutPanelMPs_Resize);
            // 
            // treeViewInput
            // 
            this.treeViewInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewInput.BackColor = System.Drawing.Color.LemonChiffon;
            this.treeViewInput.Location = new System.Drawing.Point(4, 9);
            this.treeViewInput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.treeViewInput.Name = "treeViewInput";
            this.treeViewInput.Size = new System.Drawing.Size(352, 303);
            this.treeViewInput.TabIndex = 0;
            this.treeViewInput.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewInput_AfterCheck);
            this.treeViewInput.Resize += new System.EventHandler(this.treeViewInput_Resize_1);
            // 
            // panelIndicators
            // 
            this.panelIndicators.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelIndicators.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelIndicators.Controls.Add(this.listBoxIndicators);
            this.panelIndicators.Location = new System.Drawing.Point(18, 76);
            this.panelIndicators.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelIndicators.Name = "panelIndicators";
            this.panelIndicators.Size = new System.Drawing.Size(1007, 384);
            this.panelIndicators.TabIndex = 3;
            // 
            // listBoxIndicators
            // 
            this.listBoxIndicators.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxIndicators.FormattingEnabled = true;
            this.listBoxIndicators.ItemHeight = 32;
            this.listBoxIndicators.Location = new System.Drawing.Point(15, 4);
            this.listBoxIndicators.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBoxIndicators.Name = "listBoxIndicators";
            this.listBoxIndicators.Size = new System.Drawing.Size(978, 292);
            this.listBoxIndicators.TabIndex = 0;
            // 
            // SanKeyGarphControlPanel
            // 
            this.SanKeyGarphControlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SanKeyGarphControlPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SanKeyGarphControlPanel.Controls.Add(this.SankeyGraphUnitNameLabel);
            this.SanKeyGarphControlPanel.Controls.Add(this.SanKeyGraphcomboBox);
            this.SanKeyGarphControlPanel.Location = new System.Drawing.Point(18, 6);
            this.SanKeyGarphControlPanel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.SanKeyGarphControlPanel.Name = "SanKeyGarphControlPanel";
            this.SanKeyGarphControlPanel.Size = new System.Drawing.Size(1012, 70);
            this.SanKeyGarphControlPanel.TabIndex = 2;
            // 
            // SankeyGraphUnitNameLabel
            // 
            this.SankeyGraphUnitNameLabel.AutoSize = true;
            this.SankeyGraphUnitNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SankeyGraphUnitNameLabel.Location = new System.Drawing.Point(7, 7);
            this.SankeyGraphUnitNameLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.SankeyGraphUnitNameLabel.Name = "SankeyGraphUnitNameLabel";
            this.SankeyGraphUnitNameLabel.Size = new System.Drawing.Size(109, 38);
            this.SankeyGraphUnitNameLabel.TabIndex = 1;
            this.SankeyGraphUnitNameLabel.Text = "label1";
            // 
            // SanKeyGraphcomboBox
            // 
            this.SanKeyGraphcomboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SanKeyGraphcomboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SanKeyGraphcomboBox.FormattingEnabled = true;
            this.SanKeyGraphcomboBox.Location = new System.Drawing.Point(323, 11);
            this.SanKeyGraphcomboBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.SanKeyGraphcomboBox.Name = "SanKeyGraphcomboBox";
            this.SanKeyGraphcomboBox.Size = new System.Drawing.Size(417, 40);
            this.SanKeyGraphcomboBox.TabIndex = 0;
            this.SanKeyGraphcomboBox.SelectedIndexChanged += new System.EventHandler(this.SanKeyGraphcomboBox_SelectionChangeCommitted);
            // 
            // SankeyGraphPanel
            // 
            this.SankeyGraphPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.SankeyGraphPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SankeyGraphPanel.Controls.Add(this.sankeyGraphUnit);
            this.SankeyGraphPanel.Location = new System.Drawing.Point(11, 11);
            this.SankeyGraphPanel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.SankeyGraphPanel.Name = "SankeyGraphPanel";
            this.SankeyGraphPanel.Size = new System.Drawing.Size(739, 774);
            this.SankeyGraphPanel.TabIndex = 0;
            this.SankeyGraphPanel.Resize += new System.EventHandler(this.SankeyGraphPanel_Resize);
            // 
            // sankeyGraphUnit
            // 
            this.sankeyGraphUnit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sankeyGraphUnit.FlowBarDrawOrder = ConsumerResourceModelFramework.SankeyGraph.DrawOrder.doBottomUp;
            this.sankeyGraphUnit.Location = new System.Drawing.Point(9, 2);
            this.sankeyGraphUnit.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.sankeyGraphUnit.Name = "sankeyGraphUnit";
            this.sankeyGraphUnit.NegativeColor = System.Drawing.Color.Red;
            this.sankeyGraphUnit.Network = null;
            this.sankeyGraphUnit.NetworkBackground = System.Drawing.Color.DarkGray;
            this.sankeyGraphUnit.PositiveColor = System.Drawing.Color.ForestGreen;
            this.sankeyGraphUnit.Size = new System.Drawing.Size(711, 764);
            this.sankeyGraphUnit.TabIndex = 0;
            this.sankeyGraphUnit.Resize += new System.EventHandler(this.sankeyGraphUnit_Resize);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.regionTreeViewClass1);
            this.tabPage2.Controls.Add(this.comboBoxParameters);
            this.tabPage2.Controls.Add(this.chart1);
            this.tabPage2.Location = new System.Drawing.Point(4, 41);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPage2.Size = new System.Drawing.Size(1902, 841);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Parameters by Provider";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // regionTreeViewClass1
            // 
            this.regionTreeViewClass1.CallBackHandler = null;
            this.regionTreeViewClass1.CheckBoxes = true;
            this.regionTreeViewClass1.Location = new System.Drawing.Point(1304, 20);
            this.regionTreeViewClass1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.regionTreeViewClass1.Name = "regionTreeViewClass1";
            this.regionTreeViewClass1.Size = new System.Drawing.Size(561, 790);
            this.regionTreeViewClass1.TabIndex = 4;
            // 
            // comboBoxParameters
            // 
            this.comboBoxParameters.FormattingEnabled = true;
            this.comboBoxParameters.Location = new System.Drawing.Point(9, 9);
            this.comboBoxParameters.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxParameters.Name = "comboBoxParameters";
            this.comboBoxParameters.Size = new System.Drawing.Size(1034, 40);
            this.comboBoxParameters.TabIndex = 3;
            this.comboBoxParameters.SelectedIndexChanged += new System.EventHandler(this.comboBoxParameters_SelectedIndexChanged);
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea3.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chart1.Legends.Add(legend3);
            this.chart1.Location = new System.Drawing.Point(9, 70);
            this.chart1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chart1.Name = "chart1";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chart1.Series.Add(series3);
            this.chart1.Size = new System.Drawing.Size(1305, 714);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "chart1";
            // 
            // ActionPage
            // 
            this.ActionPage.Controls.Add(this.buttonDoDrought);
            this.ActionPage.Location = new System.Drawing.Point(4, 41);
            this.ActionPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ActionPage.Name = "ActionPage";
            this.ActionPage.Size = new System.Drawing.Size(1902, 841);
            this.ActionPage.TabIndex = 2;
            this.ActionPage.Text = "Actions";
            this.ActionPage.UseVisualStyleBackColor = true;
            // 
            // buttonDoDrought
            // 
            this.buttonDoDrought.Location = new System.Drawing.Point(106, 548);
            this.buttonDoDrought.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonDoDrought.Name = "buttonDoDrought";
            this.buttonDoDrought.Size = new System.Drawing.Size(339, 50);
            this.buttonDoDrought.TabIndex = 0;
            this.buttonDoDrought.Text = "Evoke Drought";
            this.buttonDoDrought.UseVisualStyleBackColor = true;
            // 
            // tabPageAssessment
            // 
            this.tabPageAssessment.Controls.Add(this.panelGWColor);
            this.tabPageAssessment.Controls.Add(this.labelGWValue);
            this.tabPageAssessment.Controls.Add(this.label12);
            this.tabPageAssessment.Controls.Add(this.panelEnvColor);
            this.tabPageAssessment.Controls.Add(this.labelEnvValue);
            this.tabPageAssessment.Controls.Add(this.label9);
            this.tabPageAssessment.Controls.Add(this.panelEcoCOlor);
            this.tabPageAssessment.Controls.Add(this.labelEconomicValue);
            this.tabPageAssessment.Controls.Add(this.label5);
            this.tabPageAssessment.Controls.Add(this.listBoxIndicators2);
            this.tabPageAssessment.Controls.Add(this.panelAssessmentColor);
            this.tabPageAssessment.Controls.Add(this.labelAssessmentPhrase);
            this.tabPageAssessment.Controls.Add(this.labelAssessmentValue);
            this.tabPageAssessment.Controls.Add(this.label7);
            this.tabPageAssessment.Controls.Add(this.panelIndicatorColor);
            this.tabPageAssessment.Controls.Add(this.labelIndicatorPhrase);
            this.tabPageAssessment.Controls.Add(this.labelIndicatorValue);
            this.tabPageAssessment.Controls.Add(this.label4);
            this.tabPageAssessment.Controls.Add(this.panelBalanceColor);
            this.tabPageAssessment.Controls.Add(this.labelBalancePhrase);
            this.tabPageAssessment.Controls.Add(this.labelBalanceValue);
            this.tabPageAssessment.Controls.Add(this.label1);
            this.tabPageAssessment.Location = new System.Drawing.Point(4, 41);
            this.tabPageAssessment.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageAssessment.Name = "tabPageAssessment";
            this.tabPageAssessment.Size = new System.Drawing.Size(1902, 841);
            this.tabPageAssessment.TabIndex = 3;
            this.tabPageAssessment.Text = "Assessment";
            this.tabPageAssessment.UseVisualStyleBackColor = true;
            // 
            // panelGWColor
            // 
            this.panelGWColor.Location = new System.Drawing.Point(490, 637);
            this.panelGWColor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelGWColor.Name = "panelGWColor";
            this.panelGWColor.Size = new System.Drawing.Size(128, 33);
            this.panelGWColor.TabIndex = 24;
            // 
            // labelGWValue
            // 
            this.labelGWValue.AutoSize = true;
            this.labelGWValue.Location = new System.Drawing.Point(332, 639);
            this.labelGWValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGWValue.Name = "labelGWValue";
            this.labelGWValue.Size = new System.Drawing.Size(93, 32);
            this.labelGWValue.TabIndex = 22;
            this.labelGWValue.Text = "label2";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(117, 639);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(179, 32);
            this.label12.TabIndex = 21;
            this.label12.Text = "Groundwater";
            // 
            // panelEnvColor
            // 
            this.panelEnvColor.Location = new System.Drawing.Point(490, 539);
            this.panelEnvColor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelEnvColor.Name = "panelEnvColor";
            this.panelEnvColor.Size = new System.Drawing.Size(128, 33);
            this.panelEnvColor.TabIndex = 20;
            // 
            // labelEnvValue
            // 
            this.labelEnvValue.AutoSize = true;
            this.labelEnvValue.Location = new System.Drawing.Point(332, 539);
            this.labelEnvValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEnvValue.Name = "labelEnvValue";
            this.labelEnvValue.Size = new System.Drawing.Size(93, 32);
            this.labelEnvValue.TabIndex = 18;
            this.labelEnvValue.Text = "label2";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(117, 539);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(198, 32);
            this.label9.TabIndex = 17;
            this.label9.Text = "Environmental";
            // 
            // panelEcoCOlor
            // 
            this.panelEcoCOlor.Location = new System.Drawing.Point(490, 438);
            this.panelEcoCOlor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelEcoCOlor.Name = "panelEcoCOlor";
            this.panelEcoCOlor.Size = new System.Drawing.Size(128, 33);
            this.panelEcoCOlor.TabIndex = 16;
            // 
            // labelEconomicValue
            // 
            this.labelEconomicValue.AutoSize = true;
            this.labelEconomicValue.Location = new System.Drawing.Point(332, 439);
            this.labelEconomicValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEconomicValue.Name = "labelEconomicValue";
            this.labelEconomicValue.Size = new System.Drawing.Size(93, 32);
            this.labelEconomicValue.TabIndex = 14;
            this.labelEconomicValue.Text = "label2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(117, 439);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(140, 32);
            this.label5.TabIndex = 13;
            this.label5.Text = "Economic";
            // 
            // listBoxIndicators2
            // 
            this.listBoxIndicators2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxIndicators2.FormattingEnabled = true;
            this.listBoxIndicators2.ItemHeight = 32;
            this.listBoxIndicators2.Location = new System.Drawing.Point(1155, 74);
            this.listBoxIndicators2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBoxIndicators2.Name = "listBoxIndicators2";
            this.listBoxIndicators2.Size = new System.Drawing.Size(712, 548);
            this.listBoxIndicators2.TabIndex = 12;
            // 
            // panelAssessmentColor
            // 
            this.panelAssessmentColor.Location = new System.Drawing.Point(490, 338);
            this.panelAssessmentColor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelAssessmentColor.Name = "panelAssessmentColor";
            this.panelAssessmentColor.Size = new System.Drawing.Size(128, 33);
            this.panelAssessmentColor.TabIndex = 11;
            // 
            // labelAssessmentPhrase
            // 
            this.labelAssessmentPhrase.AutoSize = true;
            this.labelAssessmentPhrase.Location = new System.Drawing.Point(647, 338);
            this.labelAssessmentPhrase.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAssessmentPhrase.Name = "labelAssessmentPhrase";
            this.labelAssessmentPhrase.Size = new System.Drawing.Size(93, 32);
            this.labelAssessmentPhrase.TabIndex = 10;
            this.labelAssessmentPhrase.Text = "label2";
            // 
            // labelAssessmentValue
            // 
            this.labelAssessmentValue.AutoSize = true;
            this.labelAssessmentValue.Location = new System.Drawing.Point(332, 338);
            this.labelAssessmentValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAssessmentValue.Name = "labelAssessmentValue";
            this.labelAssessmentValue.Size = new System.Drawing.Size(93, 32);
            this.labelAssessmentValue.TabIndex = 9;
            this.labelAssessmentValue.Text = "label2";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(117, 338);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(169, 32);
            this.label7.TabIndex = 8;
            this.label7.Text = "Assessment";
            // 
            // panelIndicatorColor
            // 
            this.panelIndicatorColor.Location = new System.Drawing.Point(490, 247);
            this.panelIndicatorColor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelIndicatorColor.Name = "panelIndicatorColor";
            this.panelIndicatorColor.Size = new System.Drawing.Size(128, 33);
            this.panelIndicatorColor.TabIndex = 7;
            // 
            // labelIndicatorPhrase
            // 
            this.labelIndicatorPhrase.AutoSize = true;
            this.labelIndicatorPhrase.Location = new System.Drawing.Point(647, 249);
            this.labelIndicatorPhrase.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelIndicatorPhrase.Name = "labelIndicatorPhrase";
            this.labelIndicatorPhrase.Size = new System.Drawing.Size(93, 32);
            this.labelIndicatorPhrase.TabIndex = 6;
            this.labelIndicatorPhrase.Text = "label2";
            // 
            // labelIndicatorValue
            // 
            this.labelIndicatorValue.AutoSize = true;
            this.labelIndicatorValue.Location = new System.Drawing.Point(332, 249);
            this.labelIndicatorValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelIndicatorValue.Name = "labelIndicatorValue";
            this.labelIndicatorValue.Size = new System.Drawing.Size(93, 32);
            this.labelIndicatorValue.TabIndex = 5;
            this.labelIndicatorValue.Text = "label2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(117, 249);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(180, 32);
            this.label4.TabIndex = 4;
            this.label4.Text = "Avg Indicator";
            // 
            // panelBalanceColor
            // 
            this.panelBalanceColor.Location = new System.Drawing.Point(490, 164);
            this.panelBalanceColor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelBalanceColor.Name = "panelBalanceColor";
            this.panelBalanceColor.Size = new System.Drawing.Size(128, 33);
            this.panelBalanceColor.TabIndex = 3;
            // 
            // labelBalancePhrase
            // 
            this.labelBalancePhrase.AutoSize = true;
            this.labelBalancePhrase.Location = new System.Drawing.Point(647, 164);
            this.labelBalancePhrase.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelBalancePhrase.Name = "labelBalancePhrase";
            this.labelBalancePhrase.Size = new System.Drawing.Size(93, 32);
            this.labelBalancePhrase.TabIndex = 2;
            this.labelBalancePhrase.Text = "label2";
            // 
            // labelBalanceValue
            // 
            this.labelBalanceValue.AutoSize = true;
            this.labelBalanceValue.Location = new System.Drawing.Point(332, 164);
            this.labelBalanceValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelBalanceValue.Name = "labelBalanceValue";
            this.labelBalanceValue.Size = new System.Drawing.Size(93, 32);
            this.labelBalanceValue.TabIndex = 1;
            this.labelBalanceValue.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(117, 164);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Balance";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.parameterTreeView1);
            this.tabPage3.Location = new System.Drawing.Point(4, 41);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPage3.Size = new System.Drawing.Size(1902, 841);
            this.tabPage3.TabIndex = 4;
            this.tabPage3.Text = "Parameter Tree Example";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // parameterTreeView1
            // 
            this.parameterTreeView1.AllowGroupCheck = true;
            this.parameterTreeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parameterTreeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.parameterTreeView1.Location = new System.Drawing.Point(1331, 0);
            this.parameterTreeView1.Margin = new System.Windows.Forms.Padding(9, 9, 9, 9);
            this.parameterTreeView1.Name = "parameterTreeView1";
            this.parameterTreeView1.ParameterManager = null;
            this.parameterTreeView1.ShowFieldNames = WaterSimDCDC.WestVisual.eShowFieldName.sfHide;
            this.parameterTreeView1.Size = new System.Drawing.Size(576, 833);
            this.parameterTreeView1.TabIndex = 2;
            this.parameterTreeView1.UseCheckBoxes = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1022);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 26, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1932, 35);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripTasks
            // 
            this.toolStripTasks.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStripTasks.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3});
            this.toolStripTasks.Location = new System.Drawing.Point(0, 42);
            this.toolStripTasks.Name = "toolStripTasks";
            this.toolStripTasks.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripTasks.Size = new System.Drawing.Size(1932, 39);
            this.toolStripTasks.TabIndex = 5;
            this.toolStripTasks.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.AutoSize = false;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(32, 32);
            this.toolStripButton1.Text = "Run Models";
            this.toolStripButton1.Click += new System.EventHandler(this.runModelToolStripMenuItem_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton2.Text = "Reset Models";
            this.toolStripButton2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripButton2.Click += new System.EventHandler(this.resetModelToolStripMenuItem_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton3.Text = "toolStripButton3";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(283, 6);
            // 
            // toolStripMenuItemNewFile
            // 
            this.toolStripMenuItemNewFile.Name = "toolStripMenuItemNewFile";
            this.toolStripMenuItemNewFile.Size = new System.Drawing.Size(286, 34);
            this.toolStripMenuItemNewFile.Text = "Create &New File";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(286, 34);
            this.toolStripMenuItem2.Text = "Use &Existing File";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(206, 30);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1932, 1057);
            this.Controls.Add(this.toolStripTasks);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.Parameters);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.Parameters.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panelUserControls.ResumeLayout(false);
            this.panelInput.ResumeLayout(false);
            this.panelIndicators.ResumeLayout(false);
            this.SanKeyGarphControlPanel.ResumeLayout(false);
            this.SanKeyGarphControlPanel.PerformLayout();
            this.SankeyGraphPanel.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ActionPage.ResumeLayout(false);
            this.tabPageAssessment.ResumeLayout(false);
            this.tabPageAssessment.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStripTasks.ResumeLayout(false);
            this.toolStripTasks.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tasksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runModelMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DisplaySankeyMenuItem;
        private System.Windows.Forms.TabControl Parameters;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel SankeyGraphPanel;
        private ConsumerResourceModelFramework.SankeyGraph sankeyGraphUnit;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStripTasks;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem resetModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ComboBox comboBoxParameters;
        private System.Windows.Forms.TabPage ActionPage;
        private System.Windows.Forms.Panel panelUserControls;
        private System.Windows.Forms.Panel panelInput;
        private System.Windows.Forms.TreeView treeViewInput;
        private System.Windows.Forms.Panel panelIndicators;
        private System.Windows.Forms.ListBox listBoxIndicators;
        private System.Windows.Forms.Panel SanKeyGarphControlPanel;
        private System.Windows.Forms.Label SankeyGraphUnitNameLabel;
        private System.Windows.Forms.ComboBox SanKeyGraphcomboBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelMPs;
        private System.Windows.Forms.Button buttonDoDrought;
        private System.Windows.Forms.TabPage tabPageAssessment;
        private System.Windows.Forms.Panel panelAssessmentColor;
        private System.Windows.Forms.Label labelAssessmentPhrase;
        private System.Windows.Forms.Label labelAssessmentValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panelIndicatorColor;
        private System.Windows.Forms.Label labelIndicatorPhrase;
        private System.Windows.Forms.Label labelIndicatorValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panelBalanceColor;
        private System.Windows.Forms.Label labelBalancePhrase;
        private System.Windows.Forms.Label labelBalanceValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxIndicators2;
        private System.Windows.Forms.Panel panelGWColor;
        private System.Windows.Forms.Label labelGWValue;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panelEnvColor;
        private System.Windows.Forms.Label labelEnvValue;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panelEcoCOlor;
        private System.Windows.Forms.Label labelEconomicValue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabPage3;
        private WaterSimDCDC.WestVisual.ParameterTreeView parameterTreeView1;
        private WaterSimDCDC.Controls.WaterSimChartControl ExampleChart;
        private WaterSimDCDC.WestVisual.RegionTreeViewClass regionTreeViewClass1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemNewFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

