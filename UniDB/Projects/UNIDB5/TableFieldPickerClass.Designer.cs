namespace UniDb2
{
    partial class TableFieldPickerClass
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
            this.listBoxMultiField = new System.Windows.Forms.ListBox();
            this.comboBoxSingleField = new System.Windows.Forms.ComboBox();
            this.labelPickField = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxMultiField
            // 
            this.listBoxMultiField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxMultiField.FormattingEnabled = true;
            this.listBoxMultiField.ItemHeight = 32;
            this.listBoxMultiField.Location = new System.Drawing.Point(302, 12);
            this.listBoxMultiField.Name = "listBoxMultiField";
            this.listBoxMultiField.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxMultiField.Size = new System.Drawing.Size(602, 260);
            this.listBoxMultiField.Sorted = true;
            this.listBoxMultiField.TabIndex = 0;
            // 
            // comboBoxSingleField
            // 
            this.comboBoxSingleField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxSingleField.FormattingEnabled = true;
            this.comboBoxSingleField.Location = new System.Drawing.Point(302, 12);
            this.comboBoxSingleField.Name = "comboBoxSingleField";
            this.comboBoxSingleField.Size = new System.Drawing.Size(602, 40);
            this.comboBoxSingleField.TabIndex = 1;
            // 
            // labelPickField
            // 
            this.labelPickField.AutoSize = true;
            this.labelPickField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPickField.Location = new System.Drawing.Point(19, 20);
            this.labelPickField.Name = "labelPickField";
            this.labelPickField.Size = new System.Drawing.Size(256, 32);
            this.labelPickField.TabIndex = 2;
            this.labelPickField.Text = "Pick Column Name";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.Location = new System.Drawing.Point(25, 74);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(102, 61);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.Location = new System.Drawing.Point(150, 77);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(125, 58);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // TableFieldPickerClass
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(1094, 537);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.labelPickField);
            this.Controls.Add(this.comboBoxSingleField);
            this.Controls.Add(this.listBoxMultiField);
            this.Name = "TableFieldPickerClass";
            this.Text = "TableFieldPickerClass";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxMultiField;
        private System.Windows.Forms.ComboBox comboBoxSingleField;
        private System.Windows.Forms.Label labelPickField;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
    }
}