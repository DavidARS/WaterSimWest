namespace TestUniDb2
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemOpenDB = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSelectTable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSchema = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemGetTables = new System.Windows.Forms.ToolStripMenuItem();
            this.testReadTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.dataGridViewTest = new System.Windows.Forms.DataGridView();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTest)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.toolStripTextBox1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1690, 41);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemOpenDB,
            this.toolStripMenuItemSelectTable,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(56, 35);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // toolStripMenuItemOpenDB
            // 
            this.toolStripMenuItemOpenDB.Name = "toolStripMenuItemOpenDB";
            this.toolStripMenuItemOpenDB.Size = new System.Drawing.Size(248, 34);
            this.toolStripMenuItemOpenDB.Text = "&Open Database";
            this.toolStripMenuItemOpenDB.Click += new System.EventHandler(this.toolStripMenuItemOpenDB_Click);
            // 
            // toolStripMenuItemSelectTable
            // 
            this.toolStripMenuItemSelectTable.Name = "toolStripMenuItemSelectTable";
            this.toolStripMenuItemSelectTable.Size = new System.Drawing.Size(248, 34);
            this.toolStripMenuItemSelectTable.Text = "&Select Table";
            this.toolStripMenuItemSelectTable.Click += new System.EventHandler(this.toolStripMenuItemSelectTable_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(245, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(248, 34);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testConnectionToolStripMenuItem,
            this.ToolStripMenuItemSchema,
            this.ToolStripMenuItemGetTables,
            this.testReadTableToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(72, 35);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // testConnectionToolStripMenuItem
            // 
            this.testConnectionToolStripMenuItem.Name = "testConnectionToolStripMenuItem";
            this.testConnectionToolStripMenuItem.Size = new System.Drawing.Size(293, 34);
            this.testConnectionToolStripMenuItem.Text = "&Test Connection";
            // 
            // ToolStripMenuItemSchema
            // 
            this.ToolStripMenuItemSchema.Name = "ToolStripMenuItemSchema";
            this.ToolStripMenuItemSchema.Size = new System.Drawing.Size(293, 34);
            this.ToolStripMenuItemSchema.Text = "Test Schema Table";
            this.ToolStripMenuItemSchema.Click += new System.EventHandler(this.ToolStripMenuItemSchema_Click);
            // 
            // ToolStripMenuItemGetTables
            // 
            this.ToolStripMenuItemGetTables.Name = "ToolStripMenuItemGetTables";
            this.ToolStripMenuItemGetTables.Size = new System.Drawing.Size(293, 34);
            this.ToolStripMenuItemGetTables.Text = "Test Get Tablenames";
            this.ToolStripMenuItemGetTables.Click += new System.EventHandler(this.ToolStripMenuItemGetTables_Click);
            // 
            // testReadTableToolStripMenuItem
            // 
            this.testReadTableToolStripMenuItem.Name = "testReadTableToolStripMenuItem";
            this.testReadTableToolStripMenuItem.Size = new System.Drawing.Size(293, 34);
            this.testReadTableToolStripMenuItem.Text = "Test ReadTable";
            this.testReadTableToolStripMenuItem.Click += new System.EventHandler(this.testReadTableToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.statusStrip1.Location = new System.Drawing.Point(0, 923);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1690, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(34, 62);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(107, 68);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 24;
            this.listBox1.Location = new System.Drawing.Point(16, 138);
            this.listBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(777, 388);
            this.listBox1.TabIndex = 3;
            // 
            // dataGridViewTest
            // 
            this.dataGridViewTest.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTest.Location = new System.Drawing.Point(888, 138);
            this.dataGridViewTest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridViewTest.Name = "dataGridViewTest";
            this.dataGridViewTest.RowTemplate.Height = 24;
            this.dataGridViewTest.Size = new System.Drawing.Size(766, 390);
            this.dataGridViewTest.TabIndex = 4;
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 35);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1690, 945);
            this.Controls.Add(this.dataGridViewTest);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTest)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOpenDB;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectTable;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testConnectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSchema;
        private System.Windows.Forms.DataGridView dataGridViewTest;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemGetTables;
        private System.Windows.Forms.ToolStripMenuItem testReadTableToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
    }
}

