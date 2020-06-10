namespace WaterSim_West_v_1
{
    partial class WaterSimSankeyForm
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
            this.SankeyPanel = new System.Windows.Forms.Panel();
            this.sankeyGraph1 = new ConsumerResourceModelFramework.SankeyGraph();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listBoxData = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NetworkNameLabel = new System.Windows.Forms.Label();
            this.tasksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.animateModelRunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SankeyPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SankeyPanel
            // 
            this.SankeyPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SankeyPanel.Controls.Add(this.sankeyGraph1);
            this.SankeyPanel.Location = new System.Drawing.Point(12, 60);
            this.SankeyPanel.Name = "SankeyPanel";
            this.SankeyPanel.Size = new System.Drawing.Size(578, 400);
            this.SankeyPanel.TabIndex = 0;
            // 
            // sankeyGraph1
            // 
            this.sankeyGraph1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sankeyGraph1.FlowBarDrawOrder = ConsumerResourceModelFramework.SankeyGraph.DrawOrder.doBottomUp;
            this.sankeyGraph1.Location = new System.Drawing.Point(4, 3);
            this.sankeyGraph1.Name = "sankeyGraph1";
            this.sankeyGraph1.NegativeColor = System.Drawing.Color.Red;
            this.sankeyGraph1.Network = null;
            this.sankeyGraph1.NetworkBackground = System.Drawing.Color.Black;
            this.sankeyGraph1.PositiveColor = System.Drawing.Color.YellowGreen;
            this.sankeyGraph1.Size = new System.Drawing.Size(571, 394);
            this.sankeyGraph1.TabIndex = 0;
            this.sankeyGraph1.Load += new System.EventHandler(this.sankeyGraph1_Load);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.listBoxData);
            this.panel1.Location = new System.Drawing.Point(596, 60);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(377, 400);
            this.panel1.TabIndex = 1;
            // 
            // listBoxData
            // 
            this.listBoxData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxData.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxData.FormattingEnabled = true;
            this.listBoxData.ItemHeight = 18;
            this.listBoxData.Location = new System.Drawing.Point(13, 15);
            this.listBoxData.Name = "listBoxData";
            this.listBoxData.ScrollAlwaysVisible = true;
            this.listBoxData.Size = new System.Drawing.Size(361, 382);
            this.listBoxData.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.tasksToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(983, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(100, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "&Close";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // NetworkNameLabel
            // 
            this.NetworkNameLabel.AutoSize = true;
            this.NetworkNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NetworkNameLabel.Location = new System.Drawing.Point(12, 24);
            this.NetworkNameLabel.Name = "NetworkNameLabel";
            this.NetworkNameLabel.Size = new System.Drawing.Size(60, 24);
            this.NetworkNameLabel.TabIndex = 3;
            this.NetworkNameLabel.Text = "label1";
            // 
            // tasksToolStripMenuItem
            // 
            this.tasksToolStripMenuItem.Name = "tasksToolStripMenuItem";
            this.tasksToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.tasksToolStripMenuItem.Text = "&Tasks";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.animateModelRunToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // animateModelRunToolStripMenuItem
            // 
            this.animateModelRunToolStripMenuItem.Name = "animateModelRunToolStripMenuItem";
            this.animateModelRunToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.animateModelRunToolStripMenuItem.Text = "&Animate Model Run";
            this.animateModelRunToolStripMenuItem.Click += new System.EventHandler(this.animateModelRunToolStripMenuItem_Click);
            // 
            // WaterSimSankeyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 471);
            this.Controls.Add(this.NetworkNameLabel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.SankeyPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "WaterSimSankeyForm";
            this.Text = "WaterSimSankeyForm";
            this.SankeyPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel SankeyPanel;
        private ConsumerResourceModelFramework.SankeyGraph sankeyGraph1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label NetworkNameLabel;
        private System.Windows.Forms.ListBox listBoxData;
        private System.Windows.Forms.ToolStripMenuItem tasksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem animateModelRunToolStripMenuItem;
    }
}