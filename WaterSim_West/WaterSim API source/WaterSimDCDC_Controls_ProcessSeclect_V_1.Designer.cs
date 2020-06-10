namespace WaterSimDCDC.Controls
{
    partial class ProcessSeclectControl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ProcessManagerListBox = new System.Windows.Forms.ListBox();
            this.ProcessRegistryListBox = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.StatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Controls.Add(this.ProcessManagerListBox);
            this.panel1.Controls.Add(this.ProcessRegistryListBox);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(761, 407);
            this.panel1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 385);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(761, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ProcessManagerListBox
            // 
            this.ProcessManagerListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProcessManagerListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProcessManagerListBox.FormattingEnabled = true;
            this.ProcessManagerListBox.ItemHeight = 16;
            this.ProcessManagerListBox.Location = new System.Drawing.Point(321, 28);
            this.ProcessManagerListBox.Name = "ProcessManagerListBox";
            this.ProcessManagerListBox.Size = new System.Drawing.Size(437, 356);
            this.ProcessManagerListBox.TabIndex = 1;
            this.ProcessManagerListBox.SelectedIndexChanged += new System.EventHandler(this.ProcessManagerListBox_SelectedIndexChanged);
            // 
            // ProcessRegistryListBox
            // 
            this.ProcessRegistryListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ProcessRegistryListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProcessRegistryListBox.FormattingEnabled = true;
            this.ProcessRegistryListBox.Location = new System.Drawing.Point(3, 27);
            this.ProcessRegistryListBox.Name = "ProcessRegistryListBox";
            this.ProcessRegistryListBox.Size = new System.Drawing.Size(312, 310);
            this.ProcessRegistryListBox.TabIndex = 0;
            this.ProcessRegistryListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ProcessRegistryListBox_ItemCheck);
            this.ProcessRegistryListBox.Click += new System.EventHandler(this.ProcessRegistryListBox_Click);
            this.ProcessRegistryListBox.SelectedIndexChanged += new System.EventHandler(this.ProcessRegistryListBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Available Feedback Processes";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(318, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(192, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Process Manager Active Que";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(14, 340);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(193, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Check Process to add to Que";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(14, 357);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(248, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Uncheck Process to remove from Que";
            // 
            // StatusLabel1
            // 
            this.StatusLabel1.Font = new System.Drawing.Font("Tahoma", 9F);
            this.StatusLabel1.Name = "StatusLabel1";
            this.StatusLabel1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.StatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // ProcessSeclectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.panel1);
            this.Name = "ProcessSeclectControl";
            this.Size = new System.Drawing.Size(767, 413);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ListBox ProcessManagerListBox;
        private System.Windows.Forms.CheckedListBox ProcessRegistryListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel1;
    }
}
