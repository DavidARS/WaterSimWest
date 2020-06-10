namespace WaterSim_West_v_1
{
    partial class ShowMultipleSankeyV1
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
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.regionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectRegionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelControl = new System.Windows.Forms.Panel();
            this.treeViewRegions = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanelSankey = new System.Windows.Forms.FlowLayoutPanel();
            this.menuStrip1.SuspendLayout();
            this.panelControl.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.regionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(2333, 38);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveImageToolStripMenuItem,
            this.toolStripMenuItem1,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(56, 34);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // saveImageToolStripMenuItem
            // 
            this.saveImageToolStripMenuItem.Name = "saveImageToolStripMenuItem";
            this.saveImageToolStripMenuItem.Size = new System.Drawing.Size(239, 34);
            this.saveImageToolStripMenuItem.Text = "Save Image";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(236, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(239, 34);
            this.closeToolStripMenuItem.Text = "&Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // regionsToolStripMenuItem
            // 
            this.regionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectRegionsToolStripMenuItem});
            this.regionsToolStripMenuItem.Name = "regionsToolStripMenuItem";
            this.regionsToolStripMenuItem.Size = new System.Drawing.Size(98, 34);
            this.regionsToolStripMenuItem.Text = "Regions";
            // 
            // selectRegionsToolStripMenuItem
            // 
            this.selectRegionsToolStripMenuItem.Name = "selectRegionsToolStripMenuItem";
            this.selectRegionsToolStripMenuItem.Size = new System.Drawing.Size(239, 34);
            this.selectRegionsToolStripMenuItem.Text = "SelectRegions";
            // 
            // panelControl
            // 
            this.panelControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelControl.Controls.Add(this.treeViewRegions);
            this.panelControl.Location = new System.Drawing.Point(0, 41);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(656, 1136);
            this.panelControl.TabIndex = 1;
            // 
            // treeViewRegions
            // 
            this.treeViewRegions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewRegions.CheckBoxes = true;
            this.treeViewRegions.Location = new System.Drawing.Point(12, 144);
            this.treeViewRegions.Name = "treeViewRegions";
            this.treeViewRegions.Size = new System.Drawing.Size(617, 968);
            this.treeViewRegions.TabIndex = 0;
            this.treeViewRegions.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewRegions_AfterCheck);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.flowLayoutPanelSankey);
            this.panel1.Location = new System.Drawing.Point(662, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1671, 1136);
            this.panel1.TabIndex = 2;
            // 
            // flowLayoutPanelSankey
            // 
            this.flowLayoutPanelSankey.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelSankey.Location = new System.Drawing.Point(24, 23);
            this.flowLayoutPanelSankey.Name = "flowLayoutPanelSankey";
            this.flowLayoutPanelSankey.Size = new System.Drawing.Size(1624, 1088);
            this.flowLayoutPanelSankey.TabIndex = 0;
            // 
            // ShowMultipleSankeyV1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2333, 1178);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ShowMultipleSankeyV1";
            this.Text = "ShowMultipleSankeyV1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelControl.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem regionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectRegionsToolStripMenuItem;
        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.TreeView treeViewRegions;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelSankey;
    }
}