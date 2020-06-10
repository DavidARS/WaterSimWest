namespace WaterSim_West_v_1
{
    partial class CreateNewFileDialog
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
            this.parameterTreeView1 = new WaterSimDCDC.WestVisual.ParameterTreeView();
            this.buttonBuildFile = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // parameterTreeView1
            // 
            this.parameterTreeView1.AllowGroupCheck = true;
            this.parameterTreeView1.Location = new System.Drawing.Point(607, 11);
            this.parameterTreeView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.parameterTreeView1.Name = "parameterTreeView1";
            this.parameterTreeView1.ParameterManager = null;
            this.parameterTreeView1.ShowFieldNames = WaterSimDCDC.WestVisual.eShowFieldName.sfHide;
            this.parameterTreeView1.Size = new System.Drawing.Size(275, 521);
            this.parameterTreeView1.TabIndex = 0;
            this.parameterTreeView1.UseCheckBoxes = true;
            // 
            // buttonBuildFile
            // 
            this.buttonBuildFile.Location = new System.Drawing.Point(32, 25);
            this.buttonBuildFile.Name = "buttonBuildFile";
            this.buttonBuildFile.Size = new System.Drawing.Size(152, 60);
            this.buttonBuildFile.TabIndex = 1;
            this.buttonBuildFile.Text = "Create New WaterSim File/Table";
            this.buttonBuildFile.UseVisualStyleBackColor = true;
            this.buttonBuildFile.Click += new System.EventHandler(this.buttonBuildFile_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(32, 407);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(221, 39);
            this.buttonReset.TabIndex = 2;
            this.buttonReset.Text = "Clear Checked Parameters";
            this.buttonReset.UseVisualStyleBackColor = true;
            // 
            // CreateNewFileDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 558);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonBuildFile);
            this.Controls.Add(this.parameterTreeView1);
            this.Name = "CreateNewFileDialog";
            this.Text = "CreateNewFileDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private WaterSimDCDC.WestVisual.ParameterTreeView parameterTreeView1;
        private System.Windows.Forms.Button buttonBuildFile;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}