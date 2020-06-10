namespace UniDB.Dialogs
{
    partial class TablenameDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TablenameDialog));
            this.TablenameComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DataSetInfoLabel = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.dialogCancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TablenameComboBox
            // 
            this.TablenameComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TablenameComboBox.FormattingEnabled = true;
            this.TablenameComboBox.Location = new System.Drawing.Point(80, 12);
            this.TablenameComboBox.Name = "TablenameComboBox";
            this.TablenameComboBox.Size = new System.Drawing.Size(387, 26);
            this.TablenameComboBox.TabIndex = 0;
            this.TablenameComboBox.TextChanged += new System.EventHandler(this.TablenameComboBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tables";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "DataSet";
            // 
            // DataSetInfoLabel
            // 
            this.DataSetInfoLabel.AutoSize = true;
            this.DataSetInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataSetInfoLabel.Location = new System.Drawing.Point(79, 53);
            this.DataSetInfoLabel.MinimumSize = new System.Drawing.Size(100, 18);
            this.DataSetInfoLabel.Name = "DataSetInfoLabel";
            this.DataSetInfoLabel.Size = new System.Drawing.Size(100, 18);
            this.DataSetInfoLabel.TabIndex = 3;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(302, 83);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 4;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // dialogCancelButton
            // 
            this.dialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dialogCancelButton.Location = new System.Drawing.Point(392, 83);
            this.dialogCancelButton.Name = "dialogCancelButton";
            this.dialogCancelButton.Size = new System.Drawing.Size(75, 23);
            this.dialogCancelButton.TabIndex = 5;
            this.dialogCancelButton.Text = "Cancel";
            this.dialogCancelButton.UseVisualStyleBackColor = true;
            // 
            // TablenameDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 117);
            this.Controls.Add(this.dialogCancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.DataSetInfoLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TablenameComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TablenameDialog";
            this.Text = "OpenTable Dialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox TablenameComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label DataSetInfoLabel;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button dialogCancelButton;
    }
}