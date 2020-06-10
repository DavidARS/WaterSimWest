namespace UniDB.Dialogs
{
    partial class SqlServerDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SqlServerDialog));
            this.labelServerLocation = new System.Windows.Forms.Label();
            this.labelDatabasename = new System.Windows.Forms.Label();
            this.labelPort = new System.Windows.Forms.Label();
            this.labelUserId = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelOptions = new System.Windows.Forms.Label();
            this.comboBoxLocation = new System.Windows.Forms.ComboBox();
            this.comboBoxDatabase = new System.Windows.Forms.ComboBox();
            this.comboBoxPort = new System.Windows.Forms.ComboBox();
            this.comboBoxUser = new System.Windows.Forms.ComboBox();
            this.comboBoxPassword = new System.Windows.Forms.ComboBox();
            this.comboBoxOptions = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelConnectString = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.listBoxOptions = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxServerType = new System.Windows.Forms.ComboBox();
            this.buttonGetFileName = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelServerLocation
            // 
            this.labelServerLocation.AutoSize = true;
            this.labelServerLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelServerLocation.Location = new System.Drawing.Point(48, 63);
            this.labelServerLocation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelServerLocation.Name = "labelServerLocation";
            this.labelServerLocation.Size = new System.Drawing.Size(192, 24);
            this.labelServerLocation.TabIndex = 0;
            this.labelServerLocation.Text = "Server Location / URL";
            // 
            // labelDatabasename
            // 
            this.labelDatabasename.AutoSize = true;
            this.labelDatabasename.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDatabasename.Location = new System.Drawing.Point(39, 107);
            this.labelDatabasename.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDatabasename.Name = "labelDatabasename";
            this.labelDatabasename.Size = new System.Drawing.Size(201, 24);
            this.labelDatabasename.TabIndex = 1;
            this.labelDatabasename.Text = "Database Name / Path ";
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPort.Location = new System.Drawing.Point(205, 151);
            this.labelPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(43, 24);
            this.labelPort.TabIndex = 2;
            this.labelPort.Text = "Port";
            // 
            // labelUserId
            // 
            this.labelUserId.AutoSize = true;
            this.labelUserId.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUserId.Location = new System.Drawing.Point(180, 196);
            this.labelUserId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelUserId.Name = "labelUserId";
            this.labelUserId.Size = new System.Drawing.Size(69, 24);
            this.labelUserId.TabIndex = 3;
            this.labelUserId.Text = "User Id";
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPassword.Location = new System.Drawing.Point(153, 240);
            this.labelPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(92, 24);
            this.labelPassword.TabIndex = 4;
            this.labelPassword.Text = "Password";
            // 
            // labelOptions
            // 
            this.labelOptions.AutoSize = true;
            this.labelOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOptions.Location = new System.Drawing.Point(12, 284);
            this.labelOptions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelOptions.Name = "labelOptions";
            this.labelOptions.Size = new System.Drawing.Size(229, 24);
            this.labelOptions.TabIndex = 5;
            this.labelOptions.Text = "Other Connection Options";
            // 
            // comboBoxLocation
            // 
            this.comboBoxLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxLocation.FormattingEnabled = true;
            this.comboBoxLocation.Location = new System.Drawing.Point(261, 59);
            this.comboBoxLocation.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxLocation.Name = "comboBoxLocation";
            this.comboBoxLocation.Size = new System.Drawing.Size(423, 30);
            this.comboBoxLocation.TabIndex = 7;
            // 
            // comboBoxDatabase
            // 
            this.comboBoxDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxDatabase.FormattingEnabled = true;
            this.comboBoxDatabase.Location = new System.Drawing.Point(261, 97);
            this.comboBoxDatabase.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxDatabase.Name = "comboBoxDatabase";
            this.comboBoxDatabase.Size = new System.Drawing.Size(385, 30);
            this.comboBoxDatabase.TabIndex = 8;
            // 
            // comboBoxPort
            // 
            this.comboBoxPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxPort.FormattingEnabled = true;
            this.comboBoxPort.Location = new System.Drawing.Point(261, 142);
            this.comboBoxPort.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxPort.Name = "comboBoxPort";
            this.comboBoxPort.Size = new System.Drawing.Size(423, 30);
            this.comboBoxPort.TabIndex = 9;
            // 
            // comboBoxUser
            // 
            this.comboBoxUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxUser.FormattingEnabled = true;
            this.comboBoxUser.Location = new System.Drawing.Point(261, 186);
            this.comboBoxUser.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxUser.Name = "comboBoxUser";
            this.comboBoxUser.Size = new System.Drawing.Size(423, 30);
            this.comboBoxUser.TabIndex = 10;
            // 
            // comboBoxPassword
            // 
            this.comboBoxPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxPassword.FormattingEnabled = true;
            this.comboBoxPassword.Location = new System.Drawing.Point(261, 230);
            this.comboBoxPassword.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxPassword.Name = "comboBoxPassword";
            this.comboBoxPassword.Size = new System.Drawing.Size(423, 30);
            this.comboBoxPassword.TabIndex = 11;
            // 
            // comboBoxOptions
            // 
            this.comboBoxOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxOptions.FormattingEnabled = true;
            this.comboBoxOptions.Location = new System.Drawing.Point(261, 274);
            this.comboBoxOptions.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxOptions.Name = "comboBoxOptions";
            this.comboBoxOptions.Size = new System.Drawing.Size(423, 30);
            this.comboBoxOptions.TabIndex = 12;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabelConnectString,
            this.toolStripStatusLabelMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 450);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(716, 25);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(127, 20);
            this.toolStripStatusLabel1.Text = "Connection String";
            // 
            // toolStripStatusLabelConnectString
            // 
            this.toolStripStatusLabelConnectString.Name = "toolStripStatusLabelConnectString";
            this.toolStripStatusLabelConnectString.Size = new System.Drawing.Size(15, 20);
            this.toolStripStatusLabelConnectString.Text = "-";
            // 
            // toolStripStatusLabelMessage
            // 
            this.toolStripStatusLabelMessage.Name = "toolStripStatusLabelMessage";
            this.toolStripStatusLabelMessage.Size = new System.Drawing.Size(0, 20);
            // 
            // listBoxOptions
            // 
            this.listBoxOptions.FormattingEnabled = true;
            this.listBoxOptions.ItemHeight = 16;
            this.listBoxOptions.Location = new System.Drawing.Point(261, 314);
            this.listBoxOptions.Margin = new System.Windows.Forms.Padding(4);
            this.listBoxOptions.Name = "listBoxOptions";
            this.listBoxOptions.Size = new System.Drawing.Size(423, 84);
            this.listBoxOptions.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(92, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 24);
            this.label1.TabIndex = 15;
            this.label1.Text = "SQL Server Type";
            // 
            // comboBoxServerType
            // 
            this.comboBoxServerType.AutoCompleteCustomSource.AddRange(new string[] {
            "Access - SQLServer.stMsAccess",
            "MySQL - SQLServer.stMySQL",
            "PostGreSQL - SQLServer.stPostGreSQL"});
            this.comboBoxServerType.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxServerType.FormattingEnabled = true;
            this.comboBoxServerType.Location = new System.Drawing.Point(261, 16);
            this.comboBoxServerType.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxServerType.Name = "comboBoxServerType";
            this.comboBoxServerType.Size = new System.Drawing.Size(423, 30);
            this.comboBoxServerType.TabIndex = 16;
            this.comboBoxServerType.SelectedIndexChanged += new System.EventHandler(this.comboBoxServerType_SelectedIndexChanged);
            // 
            // buttonGetFileName
            // 
            this.buttonGetFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGetFileName.Image = ((System.Drawing.Image)(resources.GetObject("buttonGetFileName.Image")));
            this.buttonGetFileName.Location = new System.Drawing.Point(655, 92);
            this.buttonGetFileName.Margin = new System.Windows.Forms.Padding(4);
            this.buttonGetFileName.Name = "buttonGetFileName";
            this.buttonGetFileName.Size = new System.Drawing.Size(45, 42);
            this.buttonGetFileName.TabIndex = 17;
            this.buttonGetFileName.UseVisualStyleBackColor = true;
            this.buttonGetFileName.Click += new System.EventHandler(this.buttonGetFileName_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(16, 416);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(100, 28);
            this.buttonOK.TabIndex = 18;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(124, 416);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 28);
            this.buttonCancel.TabIndex = 19;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Access *.mdb,*.accdb|*.mdb;*.accdb";
            // 
            // SqlServerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 475);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonGetFileName);
            this.Controls.Add(this.comboBoxServerType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxOptions);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.comboBoxOptions);
            this.Controls.Add(this.comboBoxPassword);
            this.Controls.Add(this.comboBoxUser);
            this.Controls.Add(this.comboBoxPort);
            this.Controls.Add(this.comboBoxDatabase);
            this.Controls.Add(this.comboBoxLocation);
            this.Controls.Add(this.labelOptions);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.labelUserId);
            this.Controls.Add(this.labelPort);
            this.Controls.Add(this.labelDatabasename);
            this.Controls.Add(this.labelServerLocation);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SqlServerDialog";
            this.Text = "Open SQL Server";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SqlServerDialog_FormClosed);
            this.Load += new System.EventHandler(this.SqlServerDialog_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelServerLocation;
        private System.Windows.Forms.Label labelDatabasename;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.Label labelUserId;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelOptions;
        private System.Windows.Forms.ComboBox comboBoxLocation;
        private System.Windows.Forms.ComboBox comboBoxDatabase;
        private System.Windows.Forms.ComboBox comboBoxPort;
        private System.Windows.Forms.ComboBox comboBoxUser;
        private System.Windows.Forms.ComboBox comboBoxPassword;
        private System.Windows.Forms.ComboBox comboBoxOptions;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelConnectString;
        private System.Windows.Forms.ListBox listBoxOptions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxServerType;
        private System.Windows.Forms.Button buttonGetFileName;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMessage;
    }
}