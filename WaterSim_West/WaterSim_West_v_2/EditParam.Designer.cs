namespace WaterSim_West_v_1
{
    partial class EditParam
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditParam));
            this.labelParam = new System.Windows.Forms.Label();
            this.numericUpDownParAM = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.buttonDoall = new System.Windows.Forms.Button();
            this.toolTipAll = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownParAM)).BeginInit();
            this.SuspendLayout();
            // 
            // labelParam
            // 
            this.labelParam.AutoSize = true;
            this.labelParam.Location = new System.Drawing.Point(4, 5);
            this.labelParam.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelParam.Name = "labelParam";
            this.labelParam.Size = new System.Drawing.Size(35, 13);
            this.labelParam.TabIndex = 0;
            this.labelParam.Text = "label1";
            // 
            // numericUpDownParAM
            // 
            this.numericUpDownParAM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownParAM.Location = new System.Drawing.Point(179, 2);
            this.numericUpDownParAM.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numericUpDownParAM.Name = "numericUpDownParAM";
            this.numericUpDownParAM.Size = new System.Drawing.Size(58, 20);
            this.numericUpDownParAM.TabIndex = 2;
            this.numericUpDownParAM.ValueChanged += new System.EventHandler(this.numericUpDownParAM_ValueChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(262, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(23, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "X       Cancel the edit";
            this.toolTipAll.SetToolTip(this.button1, "Cancel the value entry");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Do all2.jpg");
            this.imageList1.Images.SetKeyName(1, "do one.jpg");
            // 
            // buttonDoall
            // 
            this.buttonDoall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDoall.Image = global::WaterSim_West_v_1.Properties.Resources.do_one;
            this.buttonDoall.Location = new System.Drawing.Point(239, -1);
            this.buttonDoall.Name = "buttonDoall";
            this.buttonDoall.Size = new System.Drawing.Size(23, 23);
            this.buttonDoall.TabIndex = 4;
            this.toolTipAll.SetToolTip(this.buttonDoall, "Click to set all parameters () or just current parameter []");
            this.buttonDoall.UseVisualStyleBackColor = true;
            this.buttonDoall.Click += new System.EventHandler(this.buttonDoall_Click);
            // 
            // EditParam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.buttonDoall);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.numericUpDownParAM);
            this.Controls.Add(this.labelParam);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "EditParam";
            this.Size = new System.Drawing.Size(286, 22);
            this.Resize += new System.EventHandler(this.EditParam_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownParAM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelParam;
        private System.Windows.Forms.NumericUpDown numericUpDownParAM;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonDoall;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolTip toolTipAll;
    }
}
