using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using UniDB;
//using System.Data.OleDb;
/**********************************************************************************
    WaterSimDCDC Scenario Ensemble Builder Analyzer Tool
	Version 5.0

	openTablenameDialog Version 7  11/21/11  Quay

    Copyright (C) 2011 , The Arizona Board of Regents
	on behalf of Arizona State University
	
    All rights reserved.

	Developed by the Decision Center for a Desert City
	Lead Model Development - David Sampson <dasamps1@asu.edu>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License version 3 as published by
    the Free Software Foundation.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
    
    Dilaog for selecting or specifying a new table in a database.
 * 
 * TablenameDialog
 * 

 *******************************************************************************/
namespace   UniDB.Dialogs
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary> Dialog for getting a tablename.  </summary>
    ///
    /// <remarks>A dialog that uses an open OleDbConnection to retireive a list of tablenames, allows user to select and existing table or create a new table name.  Returns the table name.</remarks>
    ///-------------------------------------------------------------------------------------------------

    public partial class TablenameDialog : Form
    {  
        private UniDbConnection _dbConnect;
        private Boolean orgOpen = false;
        private string _TableName = "";
        private bool _newTable = false;
        private bool _isNew = false;
        private List<string> _tablenames = new List<string>();
        private bool _caseSensitive = false;
        private bool _MustExist = false;
       
        //-----------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the tablename. </summary>
        ///
        /// <value> The tablename. </value>
        ///-------------------------------------------------------------------------------------------------
        public string Tablename { get { return _TableName; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the title of the dialog box. </summary>
        ///
        /// <value> The title. </value>
        ///-------------------------------------------------------------------------------------------------
        public string Title { get { return this.Text; } set { this.Text = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Require Unique Tablename. </summary>
        /// <remarks>If set to true, requires that a unique table name be entered.  Uses a list of tablenames from OleDbConnection</remarks>
        /// <value> true if require unique tablename, false if not. </value>
        ///-------------------------------------------------------------------------------------------------
        public bool RequireUniqueTablename { get { return _newTable; } set { _newTable = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets a value indicating whether the Tablename is a new tablename. </summary>
        ///
        /// <value> true if this object is new tablename, false if not. </value>
        ///-------------------------------------------------------------------------------------------------
        public bool isNewTablename { get { return _isNew; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets a value indicating whether this object is case sensitive. </summary>
        /// <remarks>If set to true, then test for new or unique table name is case sensitive</remarks>
        /// <value> true if case sensitive, false if not. </value>
        ///-------------------------------------------------------------------------------------------------
        public bool isCaseSensitive { get { return _caseSensitive; } set { _caseSensitive = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets a value indicating whether the tablename must exist. </summary>
        /// <remarks>Opposite of RequireUniqueTablename.  If set to true, requires that an existing table name be entered.  Uses a list of tablenames from OleDbConnection</remarks>
        /// <value> true if tablename must exist, false if not. </value>
        ///-------------------------------------------------------------------------------------------------
        public bool TablenameMustExist { get { return _MustExist; } set { _MustExist = value; } }
        //-----------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Constructor. </summary>
        ///
        /// <remarks> </remarks>
        ///-------------------------------------------------------------------------------------------------

        public TablenameDialog()
        {
            InitializeComponent();
        }

        //-----------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Sets the OleDbConnetion object. </summary>
        ///
        /// <value> The UniDBConnection. </value>
        ///-------------------------------------------------------------------------------------------------

        public UniDbConnection dbConnect
        {
            //get { return _dbConnect; }
            set
            {
                string Tablename = "";
                _dbConnect = value;
                orgOpen = (_dbConnect.State == System.Data.ConnectionState.Open);
                try
                {
                    if (!orgOpen) _dbConnect.Open();
                    DataSetInfoLabel.Text = _dbConnect.DataSource.ToString();
                    DataTable dbSchema = new DataTable();
                    dbSchema = _dbConnect.GetSchema("TABLES");
                    for (int i = 0; i < dbSchema.Rows.Count; i++)
                    {
                        if (dbSchema.Rows[i]["TABLE_TYPE"].ToString() == "TABLE")
                        {
                            Tablename = dbSchema.Rows[i]["TABLE_NAME"].ToString();
                            TablenameComboBox.Items.Add(Tablename);
                            if (!_caseSensitive)  { Tablename = Tablename.ToUpper(); }
                            _tablenames.Add(Tablename);
 
                        }
                    }
                }
                catch(Exception)
                {
                    throw(new Exception("OleDbConnection failed"));
                }
            }
        }
        //-----------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Shows the dialog in dialog mode. </summary>
        ///
        /// <remarks>  </remarks>
        ///
        /// <param name="Connection">    The OlDbConnection. </param>
        /// <param name="RequireUnique"> true to require unique tablename. </param>
        /// <param name="MustExist">     true if tablename must exist. </param>
        ///
        /// <returns> . </returns>
        ///-------------------------------------------------------------------------------------------------

        public DialogResult ShowDialog( UniDbConnection Connection, bool RequireUnique, bool MustExist) {
            bool iserror = true;
            string ErrMessage = "";
            _dbConnect = Connection;
            orgOpen = (_dbConnect.State == System.Data.ConnectionState.Open);
            RequireUniqueTablename = RequireUnique;
            TablenameMustExist = MustExist;
            try
            {
                
                if (!orgOpen) _dbConnect.Open();
                TablenameComboBox.Items.Clear();
                _tablenames.Clear();
                DataSetInfoLabel.Text = _dbConnect.DataSource.ToString();
                _tablenames = _dbConnect.GetTableNames(ref iserror, ref ErrMessage);
                foreach (string str in _tablenames)
                    TablenameComboBox.Items.Add(str);
                //DataTable dbSchema = new DataTable();
                //dbSchema = _dbConnect.GetSchema("TABLES");
                //for (int i = 0; i < dbSchema.Rows.Count; i++)
                //{
                //    if (dbSchema.Rows[i]["TABLE_TYPE"].ToString() == "TABLE")
                //    {
                //        temp = dbSchema.Rows[i]["TABLE_NAME"].ToString();
                //         TablenameComboBox.Items.Add(temp);
                //         _tablenames.Add(temp);
                //    }
                //}
                return ShowDialog();
            }
            catch (Exception)
            {
                return DialogResult.Abort;
            }

        }
        //--------------------------------------------------------------
        private bool Contains(string value)
        {
            bool isIn = false;
            value = value.ToUpper();
            string temp = _tablenames.Find(delegate(string findme) { if (findme.ToUpper() == value) return true; else return false; });
            if (temp!=null)
               isIn = (temp.ToUpper() == value);

            return isIn;
        }
        //--------------------------------------------------------------
        private void TablenameComboBox_TextChanged(object sender, EventArgs e)
        {
                _TableName = TablenameComboBox.Text;
        }
        //--------------------------------------------------------------

        private void OKButton_Click(object sender, EventArgs e)
        {
            _isNew = !Contains(_TableName);

            if ((_newTable) & (!_isNew)) { MessageBox.Show("Tablename must be unique!  " + _TableName + " is an existing table in the database."); }
            else
                if ((_MustExist) & (_isNew)) { MessageBox.Show("Tablename must exist!  " + _TableName + " is not an existing table in the database."); }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }
        //-----------------------------------------------------------------

    }
}
