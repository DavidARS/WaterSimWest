namespace WaterSimDCDC.WestVisual
{
    partial class ParameterTreeView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParameterTreeView));
            this.treeViewParameters = new System.Windows.Forms.TreeView();
            this.contextMenuParameterTreeview = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showFieldnamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showFirstToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLastToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byFieldnameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListTreeNodes = new System.Windows.Forms.ImageList(this.components);
            this.statusStripTreeView = new System.Windows.Forms.StatusStrip();
            this.ItemStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelTreeViewKey = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelInputBaseKey = new System.Windows.Forms.Panel();
            this.labelInputBaseKey = new System.Windows.Forms.Label();
            this.picBoxInputBaseKey = new System.Windows.Forms.PictureBox();
            this.panelInputProviderKey = new System.Windows.Forms.Panel();
            this.labelInputProviderKey = new System.Windows.Forms.Label();
            this.picBoxInputProviderKey = new System.Windows.Forms.PictureBox();
            this.panelOutputBaseKey = new System.Windows.Forms.Panel();
            this.labelOutputBaseKey = new System.Windows.Forms.Label();
            this.picBoxOutputBaseKey = new System.Windows.Forms.PictureBox();
            this.panelOutputProviderKey = new System.Windows.Forms.Panel();
            this.labelOutputProviderKey = new System.Windows.Forms.Label();
            this.picBoxOutputProviderKey = new System.Windows.Forms.PictureBox();
            this.panelGroupKey = new System.Windows.Forms.Panel();
            this.labelGroupKey = new System.Windows.Forms.Label();
            this.picBoxGroupKey = new System.Windows.Forms.PictureBox();
            this.panelNotActiveKey = new System.Windows.Forms.Panel();
            this.labelNotActiveKey = new System.Windows.Forms.Label();
            this.picBoxNotActiveKey = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuParameterTreeview.SuspendLayout();
            this.statusStripTreeView.SuspendLayout();
            this.panelTreeViewKey.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panelInputBaseKey.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxInputBaseKey)).BeginInit();
            this.panelInputProviderKey.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxInputProviderKey)).BeginInit();
            this.panelOutputBaseKey.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxOutputBaseKey)).BeginInit();
            this.panelOutputProviderKey.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxOutputProviderKey)).BeginInit();
            this.panelGroupKey.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxGroupKey)).BeginInit();
            this.panelNotActiveKey.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxNotActiveKey)).BeginInit();
            this.SuspendLayout();
            // 
            // treeViewParameters
            // 
            this.treeViewParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewParameters.ContextMenuStrip = this.contextMenuParameterTreeview;
            this.treeViewParameters.Location = new System.Drawing.Point(4, 3);
            this.treeViewParameters.Margin = new System.Windows.Forms.Padding(4);
            this.treeViewParameters.Name = "treeViewParameters";
            this.treeViewParameters.Size = new System.Drawing.Size(481, 643);
            this.treeViewParameters.TabIndex = 0;
            this.treeViewParameters.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewParameters_BeforeCheck);
            this.treeViewParameters.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewParameters_AfterCheck);
            this.treeViewParameters.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewParameters_BeforeSelect);
            this.treeViewParameters.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewParameters_AfterSelect);
            this.treeViewParameters.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewParameters_NodeMouseClick);
            this.treeViewParameters.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewParameters_NodeMouseDoubleClick);
            this.treeViewParameters.DoubleClick += new System.EventHandler(this.treeViewParameters_DoubleClick);
            this.treeViewParameters.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeViewParameters_MouseClick);
            this.treeViewParameters.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeViewParameters_MouseDoubleClick);
            // 
            // contextMenuParameterTreeview
            // 
            this.contextMenuParameterTreeview.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.contextMenuParameterTreeview.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayToolStripMenuItem,
            this.sortToolStripMenuItem,
            this.treeToolStripMenuItem});
            this.contextMenuParameterTreeview.Name = "contextMenuParameterTreeview";
            this.contextMenuParameterTreeview.Size = new System.Drawing.Size(154, 106);
            // 
            // displayToolStripMenuItem
            // 
            this.displayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hideKeyToolStripMenuItem,
            this.showFieldnamesToolStripMenuItem});
            this.displayToolStripMenuItem.Name = "displayToolStripMenuItem";
            this.displayToolStripMenuItem.Size = new System.Drawing.Size(153, 34);
            this.displayToolStripMenuItem.Text = "Display";
            // 
            // hideKeyToolStripMenuItem
            // 
            this.hideKeyToolStripMenuItem.Name = "hideKeyToolStripMenuItem";
            this.hideKeyToolStripMenuItem.Size = new System.Drawing.Size(264, 34);
            this.hideKeyToolStripMenuItem.Text = "Hide Key";
            this.hideKeyToolStripMenuItem.Click += new System.EventHandler(this.hideKeyToolStripMenuItem_Click);
            // 
            // showFieldnamesToolStripMenuItem
            // 
            this.showFieldnamesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showFirstToolStripMenuItem,
            this.showLastToolStripMenuItem,
            this.hideToolStripMenuItem});
            this.showFieldnamesToolStripMenuItem.Name = "showFieldnamesToolStripMenuItem";
            this.showFieldnamesToolStripMenuItem.Size = new System.Drawing.Size(264, 34);
            this.showFieldnamesToolStripMenuItem.Text = "Show Fieldnames";
            // 
            // showFirstToolStripMenuItem
            // 
            this.showFirstToolStripMenuItem.CheckOnClick = true;
            this.showFirstToolStripMenuItem.Name = "showFirstToolStripMenuItem";
            this.showFirstToolStripMenuItem.Size = new System.Drawing.Size(198, 34);
            this.showFirstToolStripMenuItem.Text = "Show First";
            this.showFirstToolStripMenuItem.CheckedChanged += new System.EventHandler(this.showFirstToolStripMenuItem_CheckedChanged);
            // 
            // showLastToolStripMenuItem
            // 
            this.showLastToolStripMenuItem.CheckOnClick = true;
            this.showLastToolStripMenuItem.Name = "showLastToolStripMenuItem";
            this.showLastToolStripMenuItem.Size = new System.Drawing.Size(198, 34);
            this.showLastToolStripMenuItem.Text = "Show Last";
            this.showLastToolStripMenuItem.CheckedChanged += new System.EventHandler(this.showLastToolStripMenuItem_CheckedChanged);
            // 
            // hideToolStripMenuItem
            // 
            this.hideToolStripMenuItem.CheckOnClick = true;
            this.hideToolStripMenuItem.Name = "hideToolStripMenuItem";
            this.hideToolStripMenuItem.Size = new System.Drawing.Size(198, 34);
            this.hideToolStripMenuItem.Text = "Hide";
            this.hideToolStripMenuItem.CheckedChanged += new System.EventHandler(this.hideToolStripMenuItem_CheckedChanged);
            // 
            // sortToolStripMenuItem
            // 
            this.sortToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.byLabelToolStripMenuItem,
            this.byFieldnameToolStripMenuItem});
            this.sortToolStripMenuItem.Name = "sortToolStripMenuItem";
            this.sortToolStripMenuItem.Size = new System.Drawing.Size(153, 34);
            this.sortToolStripMenuItem.Text = "Sort";
            // 
            // byLabelToolStripMenuItem
            // 
            this.byLabelToolStripMenuItem.Name = "byLabelToolStripMenuItem";
            this.byLabelToolStripMenuItem.Size = new System.Drawing.Size(227, 34);
            this.byLabelToolStripMenuItem.Text = "By Label";
            this.byLabelToolStripMenuItem.Click += new System.EventHandler(this.byLabelToolStripMenuItem_Click);
            // 
            // byFieldnameToolStripMenuItem
            // 
            this.byFieldnameToolStripMenuItem.Name = "byFieldnameToolStripMenuItem";
            this.byFieldnameToolStripMenuItem.Size = new System.Drawing.Size(227, 34);
            this.byFieldnameToolStripMenuItem.Text = "By Fieldname";
            this.byFieldnameToolStripMenuItem.Click += new System.EventHandler(this.byFieldnameToolStripMenuItem_Click);
            // 
            // treeToolStripMenuItem
            // 
            this.treeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.expandAllToolStripMenuItem,
            this.collapseAllToolStripMenuItem});
            this.treeToolStripMenuItem.Name = "treeToolStripMenuItem";
            this.treeToolStripMenuItem.Size = new System.Drawing.Size(153, 34);
            this.treeToolStripMenuItem.Text = "Tree";
            // 
            // expandAllToolStripMenuItem
            // 
            this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
            this.expandAllToolStripMenuItem.Size = new System.Drawing.Size(212, 34);
            this.expandAllToolStripMenuItem.Text = "Expand All";
            // 
            // collapseAllToolStripMenuItem
            // 
            this.collapseAllToolStripMenuItem.Name = "collapseAllToolStripMenuItem";
            this.collapseAllToolStripMenuItem.Size = new System.Drawing.Size(212, 34);
            this.collapseAllToolStripMenuItem.Text = "Collapse All";
            // 
            // imageListTreeNodes
            // 
            this.imageListTreeNodes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeNodes.ImageStream")));
            this.imageListTreeNodes.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTreeNodes.Images.SetKeyName(0, "green arrow left.png");
            this.imageListTreeNodes.Images.SetKeyName(1, "green round arrow going right 32 x 32.png");
            this.imageListTreeNodes.Images.SetKeyName(2, "red arrow left.png");
            this.imageListTreeNodes.Images.SetKeyName(3, "red round arroe going left 32 x 32psd.png");
            this.imageListTreeNodes.Images.SetKeyName(4, "blue buttondouble arrow left right  32 x 32.png");
            this.imageListTreeNodes.Images.SetKeyName(5, "blue circle with arrow 32 x 32 .png");
            this.imageListTreeNodes.Images.SetKeyName(6, "red round question 32 x 32.png");
            this.imageListTreeNodes.Images.SetKeyName(7, "grey arrow right 32 x 32.png");
            this.imageListTreeNodes.Images.SetKeyName(8, "Grey Circle with Arrow Right 32 x 32.png");
            this.imageListTreeNodes.Images.SetKeyName(9, "grey arrow left 32 x 32.png");
            this.imageListTreeNodes.Images.SetKeyName(10, "grey circle arrow left 32 x 32.png");
            this.imageListTreeNodes.Images.SetKeyName(11, "blue buttondouble arrow left right  32 x 32 selected.png");
            this.imageListTreeNodes.Images.SetKeyName(12, "Grey Circle with Arrow Right 32 x 32.png");
            this.imageListTreeNodes.Images.SetKeyName(13, "grey  round question 32 x 32.png");
            // 
            // statusStripTreeView
            // 
            this.statusStripTreeView.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.statusStripTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ItemStatusLabel});
            this.statusStripTreeView.Location = new System.Drawing.Point(0, 815);
            this.statusStripTreeView.Name = "statusStripTreeView";
            this.statusStripTreeView.Padding = new System.Windows.Forms.Padding(2, 0, 18, 0);
            this.statusStripTreeView.Size = new System.Drawing.Size(489, 22);
            this.statusStripTreeView.TabIndex = 1;
            this.statusStripTreeView.Text = "statusStrip1";
            // 
            // ItemStatusLabel
            // 
            this.ItemStatusLabel.Name = "ItemStatusLabel";
            this.ItemStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // panelTreeViewKey
            // 
            this.panelTreeViewKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelTreeViewKey.AutoScroll = true;
            this.panelTreeViewKey.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelTreeViewKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTreeViewKey.Controls.Add(this.flowLayoutPanel1);
            this.panelTreeViewKey.Controls.Add(this.label1);
            this.panelTreeViewKey.Location = new System.Drawing.Point(4, 654);
            this.panelTreeViewKey.Margin = new System.Windows.Forms.Padding(4);
            this.panelTreeViewKey.Name = "panelTreeViewKey";
            this.panelTreeViewKey.Size = new System.Drawing.Size(477, 157);
            this.panelTreeViewKey.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.panelInputBaseKey);
            this.flowLayoutPanel1.Controls.Add(this.panelInputProviderKey);
            this.flowLayoutPanel1.Controls.Add(this.panelOutputBaseKey);
            this.flowLayoutPanel1.Controls.Add(this.panelOutputProviderKey);
            this.flowLayoutPanel1.Controls.Add(this.panelGroupKey);
            this.flowLayoutPanel1.Controls.Add(this.panelNotActiveKey);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(72, 4);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(402, 142);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // panelInputBaseKey
            // 
            this.panelInputBaseKey.AutoSize = true;
            this.panelInputBaseKey.Controls.Add(this.labelInputBaseKey);
            this.panelInputBaseKey.Controls.Add(this.picBoxInputBaseKey);
            this.panelInputBaseKey.Location = new System.Drawing.Point(4, 4);
            this.panelInputBaseKey.Margin = new System.Windows.Forms.Padding(4);
            this.panelInputBaseKey.Name = "panelInputBaseKey";
            this.panelInputBaseKey.Size = new System.Drawing.Size(160, 38);
            this.panelInputBaseKey.TabIndex = 0;
            // 
            // labelInputBaseKey
            // 
            this.labelInputBaseKey.AutoSize = true;
            this.labelInputBaseKey.Location = new System.Drawing.Point(51, 13);
            this.labelInputBaseKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelInputBaseKey.Name = "labelInputBaseKey";
            this.labelInputBaseKey.Size = new System.Drawing.Size(105, 25);
            this.labelInputBaseKey.TabIndex = 1;
            this.labelInputBaseKey.Text = "Input Base";
            // 
            // picBoxInputBaseKey
            // 
            this.picBoxInputBaseKey.Image = global::WestVisual.Properties.Resources.green_arrow_left;
            this.picBoxInputBaseKey.Location = new System.Drawing.Point(11, 9);
            this.picBoxInputBaseKey.Margin = new System.Windows.Forms.Padding(4);
            this.picBoxInputBaseKey.Name = "picBoxInputBaseKey";
            this.picBoxInputBaseKey.Size = new System.Drawing.Size(22, 24);
            this.picBoxInputBaseKey.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxInputBaseKey.TabIndex = 0;
            this.picBoxInputBaseKey.TabStop = false;
            // 
            // panelInputProviderKey
            // 
            this.panelInputProviderKey.AutoSize = true;
            this.panelInputProviderKey.Controls.Add(this.labelInputProviderKey);
            this.panelInputProviderKey.Controls.Add(this.picBoxInputProviderKey);
            this.panelInputProviderKey.Location = new System.Drawing.Point(172, 4);
            this.panelInputProviderKey.Margin = new System.Windows.Forms.Padding(4);
            this.panelInputProviderKey.Name = "panelInputProviderKey";
            this.panelInputProviderKey.Size = new System.Drawing.Size(187, 37);
            this.panelInputProviderKey.TabIndex = 1;
            // 
            // labelInputProviderKey
            // 
            this.labelInputProviderKey.AutoSize = true;
            this.labelInputProviderKey.Location = new System.Drawing.Point(51, 10);
            this.labelInputProviderKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelInputProviderKey.Name = "labelInputProviderKey";
            this.labelInputProviderKey.Size = new System.Drawing.Size(132, 25);
            this.labelInputProviderKey.TabIndex = 1;
            this.labelInputProviderKey.Text = "Input Provider";
            // 
            // picBoxInputProviderKey
            // 
            this.picBoxInputProviderKey.Image = global::WestVisual.Properties.Resources.green_round_arrow_going_right_32_x_32;
            this.picBoxInputProviderKey.Location = new System.Drawing.Point(11, 9);
            this.picBoxInputProviderKey.Margin = new System.Windows.Forms.Padding(4);
            this.picBoxInputProviderKey.Name = "picBoxInputProviderKey";
            this.picBoxInputProviderKey.Size = new System.Drawing.Size(22, 24);
            this.picBoxInputProviderKey.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxInputProviderKey.TabIndex = 0;
            this.picBoxInputProviderKey.TabStop = false;
            // 
            // panelOutputBaseKey
            // 
            this.panelOutputBaseKey.AutoSize = true;
            this.panelOutputBaseKey.Controls.Add(this.labelOutputBaseKey);
            this.panelOutputBaseKey.Controls.Add(this.picBoxOutputBaseKey);
            this.panelOutputBaseKey.Location = new System.Drawing.Point(4, 50);
            this.panelOutputBaseKey.Margin = new System.Windows.Forms.Padding(4);
            this.panelOutputBaseKey.Name = "panelOutputBaseKey";
            this.panelOutputBaseKey.Size = new System.Drawing.Size(176, 37);
            this.panelOutputBaseKey.TabIndex = 2;
            // 
            // labelOutputBaseKey
            // 
            this.labelOutputBaseKey.AutoSize = true;
            this.labelOutputBaseKey.Location = new System.Drawing.Point(51, 11);
            this.labelOutputBaseKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelOutputBaseKey.Name = "labelOutputBaseKey";
            this.labelOutputBaseKey.Size = new System.Drawing.Size(121, 25);
            this.labelOutputBaseKey.TabIndex = 1;
            this.labelOutputBaseKey.Text = "Output Base";
            // 
            // picBoxOutputBaseKey
            // 
            this.picBoxOutputBaseKey.Image = global::WestVisual.Properties.Resources.red_arrow_left;
            this.picBoxOutputBaseKey.Location = new System.Drawing.Point(11, 9);
            this.picBoxOutputBaseKey.Margin = new System.Windows.Forms.Padding(4);
            this.picBoxOutputBaseKey.Name = "picBoxOutputBaseKey";
            this.picBoxOutputBaseKey.Size = new System.Drawing.Size(22, 24);
            this.picBoxOutputBaseKey.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxOutputBaseKey.TabIndex = 0;
            this.picBoxOutputBaseKey.TabStop = false;
            // 
            // panelOutputProviderKey
            // 
            this.panelOutputProviderKey.AutoSize = true;
            this.panelOutputProviderKey.Controls.Add(this.labelOutputProviderKey);
            this.panelOutputProviderKey.Controls.Add(this.picBoxOutputProviderKey);
            this.panelOutputProviderKey.Location = new System.Drawing.Point(188, 50);
            this.panelOutputProviderKey.Margin = new System.Windows.Forms.Padding(4);
            this.panelOutputProviderKey.Name = "panelOutputProviderKey";
            this.panelOutputProviderKey.Size = new System.Drawing.Size(203, 38);
            this.panelOutputProviderKey.TabIndex = 3;
            // 
            // labelOutputProviderKey
            // 
            this.labelOutputProviderKey.AutoSize = true;
            this.labelOutputProviderKey.Location = new System.Drawing.Point(51, 13);
            this.labelOutputProviderKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelOutputProviderKey.Name = "labelOutputProviderKey";
            this.labelOutputProviderKey.Size = new System.Drawing.Size(148, 25);
            this.labelOutputProviderKey.TabIndex = 1;
            this.labelOutputProviderKey.Text = "Output Provider";
            // 
            // picBoxOutputProviderKey
            // 
            this.picBoxOutputProviderKey.Image = global::WestVisual.Properties.Resources.red_round_arroe_going_left_32_x_32psd;
            this.picBoxOutputProviderKey.Location = new System.Drawing.Point(11, 9);
            this.picBoxOutputProviderKey.Margin = new System.Windows.Forms.Padding(4);
            this.picBoxOutputProviderKey.Name = "picBoxOutputProviderKey";
            this.picBoxOutputProviderKey.Size = new System.Drawing.Size(22, 24);
            this.picBoxOutputProviderKey.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxOutputProviderKey.TabIndex = 0;
            this.picBoxOutputProviderKey.TabStop = false;
            // 
            // panelGroupKey
            // 
            this.panelGroupKey.AutoSize = true;
            this.panelGroupKey.Controls.Add(this.labelGroupKey);
            this.panelGroupKey.Controls.Add(this.picBoxGroupKey);
            this.panelGroupKey.Location = new System.Drawing.Point(4, 96);
            this.panelGroupKey.Margin = new System.Windows.Forms.Padding(4);
            this.panelGroupKey.Name = "panelGroupKey";
            this.panelGroupKey.Size = new System.Drawing.Size(175, 38);
            this.panelGroupKey.TabIndex = 4;
            // 
            // labelGroupKey
            // 
            this.labelGroupKey.AutoSize = true;
            this.labelGroupKey.Location = new System.Drawing.Point(51, 13);
            this.labelGroupKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGroupKey.Name = "labelGroupKey";
            this.labelGroupKey.Size = new System.Drawing.Size(120, 25);
            this.labelGroupKey.TabIndex = 1;
            this.labelGroupKey.Text = "Topic Group";
            // 
            // picBoxGroupKey
            // 
            this.picBoxGroupKey.Image = global::WestVisual.Properties.Resources.blue_buttondouble_arrow_left_right__32_x_32;
            this.picBoxGroupKey.Location = new System.Drawing.Point(11, 9);
            this.picBoxGroupKey.Margin = new System.Windows.Forms.Padding(4);
            this.picBoxGroupKey.Name = "picBoxGroupKey";
            this.picBoxGroupKey.Size = new System.Drawing.Size(22, 24);
            this.picBoxGroupKey.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxGroupKey.TabIndex = 0;
            this.picBoxGroupKey.TabStop = false;
            // 
            // panelNotActiveKey
            // 
            this.panelNotActiveKey.AutoSize = true;
            this.panelNotActiveKey.Controls.Add(this.labelNotActiveKey);
            this.panelNotActiveKey.Controls.Add(this.picBoxNotActiveKey);
            this.panelNotActiveKey.Location = new System.Drawing.Point(187, 96);
            this.panelNotActiveKey.Margin = new System.Windows.Forms.Padding(4);
            this.panelNotActiveKey.Name = "panelNotActiveKey";
            this.panelNotActiveKey.Size = new System.Drawing.Size(156, 38);
            this.panelNotActiveKey.TabIndex = 5;
            // 
            // labelNotActiveKey
            // 
            this.labelNotActiveKey.AutoSize = true;
            this.labelNotActiveKey.Location = new System.Drawing.Point(51, 13);
            this.labelNotActiveKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNotActiveKey.Name = "labelNotActiveKey";
            this.labelNotActiveKey.Size = new System.Drawing.Size(101, 25);
            this.labelNotActiveKey.TabIndex = 1;
            this.labelNotActiveKey.Text = "Not Active";
            // 
            // picBoxNotActiveKey
            // 
            this.picBoxNotActiveKey.Image = global::WestVisual.Properties.Resources.red_round_question_32_x_32;
            this.picBoxNotActiveKey.Location = new System.Drawing.Point(11, 9);
            this.picBoxNotActiveKey.Margin = new System.Windows.Forms.Padding(4);
            this.picBoxNotActiveKey.Name = "picBoxNotActiveKey";
            this.picBoxNotActiveKey.Size = new System.Drawing.Size(22, 24);
            this.picBoxNotActiveKey.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxNotActiveKey.TabIndex = 0;
            this.picBoxNotActiveKey.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Key:";
            // 
            // ParameterTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelTreeViewKey);
            this.Controls.Add(this.statusStripTreeView);
            this.Controls.Add(this.treeViewParameters);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ParameterTreeView";
            this.Size = new System.Drawing.Size(489, 837);
            this.Resize += new System.EventHandler(this.ParameterTreeView_Resize);
            this.contextMenuParameterTreeview.ResumeLayout(false);
            this.statusStripTreeView.ResumeLayout(false);
            this.statusStripTreeView.PerformLayout();
            this.panelTreeViewKey.ResumeLayout(false);
            this.panelTreeViewKey.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panelInputBaseKey.ResumeLayout(false);
            this.panelInputBaseKey.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxInputBaseKey)).EndInit();
            this.panelInputProviderKey.ResumeLayout(false);
            this.panelInputProviderKey.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxInputProviderKey)).EndInit();
            this.panelOutputBaseKey.ResumeLayout(false);
            this.panelOutputBaseKey.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxOutputBaseKey)).EndInit();
            this.panelOutputProviderKey.ResumeLayout(false);
            this.panelOutputProviderKey.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxOutputProviderKey)).EndInit();
            this.panelGroupKey.ResumeLayout(false);
            this.panelGroupKey.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxGroupKey)).EndInit();
            this.panelNotActiveKey.ResumeLayout(false);
            this.panelNotActiveKey.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxNotActiveKey)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewParameters;
        private System.Windows.Forms.ImageList imageListTreeNodes;
        private System.Windows.Forms.StatusStrip statusStripTreeView;
        private System.Windows.Forms.Panel panelTreeViewKey;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panelInputBaseKey;
        private System.Windows.Forms.Label labelInputBaseKey;
        private System.Windows.Forms.PictureBox picBoxInputBaseKey;
        private System.Windows.Forms.Panel panelInputProviderKey;
        private System.Windows.Forms.Label labelInputProviderKey;
        private System.Windows.Forms.PictureBox picBoxInputProviderKey;
        private System.Windows.Forms.Panel panelOutputBaseKey;
        private System.Windows.Forms.Label labelOutputBaseKey;
        private System.Windows.Forms.PictureBox picBoxOutputBaseKey;
        private System.Windows.Forms.Panel panelOutputProviderKey;
        private System.Windows.Forms.Label labelOutputProviderKey;
        private System.Windows.Forms.PictureBox picBoxOutputProviderKey;
        private System.Windows.Forms.Panel panelGroupKey;
        private System.Windows.Forms.Label labelGroupKey;
        private System.Windows.Forms.PictureBox picBoxGroupKey;
        private System.Windows.Forms.Panel panelNotActiveKey;
        private System.Windows.Forms.Label labelNotActiveKey;
        private System.Windows.Forms.PictureBox picBoxNotActiveKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuParameterTreeview;
        private System.Windows.Forms.ToolStripMenuItem displayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showFieldnamesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showFirstToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLastToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byLabelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byFieldnameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem treeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel ItemStatusLabel;
    }
}
