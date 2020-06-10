// ===========================================================
//     WaterSimDCDC Regional Water Demand and Supply Model Version 5.0

//       A Component that provides controls to support a parameter chart
//      Charts multiple parameters for one provider over time

//       WaterSimDCDC.Controls_ProviderGraph
//       Version 1
//       Keeper Ray Quay  ray.quay@asu.edu
//       Copyright (C) 2011,2012 , The Arizona Board of Regents
//              on behalf of Arizona State University

//       All rights reserved.

//       Developed by the Decision Center for a Desert City
//       Lead Model Development - David A. Sampson <david.a.sampson@asu.edu>

//       This program is free software: you can redistribute it and/or modify
//       it under the terms of the GNU General Public License version 3 as published by
//       the Free Software Foundation.

//       This program is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU General Public License for more details.

//       You should have received a copy of the GNU General Public License
//       along with this program.  If not, please see <http://www.gnu.org/licenses/>.
//
//====================================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using WaterSimDCDC;
using System.Data.OleDb;
using UniDB;

namespace WaterSimDCDC.Controls
{
    /// <summary>   Provider dash board chart. </summary>
    public partial class ProviderDashBoardChart : UserControl
    {
        UniDbConnection FDbConnection = null;
        ShadowParameterManager FPM;
        DataTable FDataTable = null;
        bool FShowTablenameComboBox = true;
        string FTablename = "";
        string FSelectedScenario = "";
        SQLServer FSQLServer = SQLServer.stAccess;

        /// <summary>   Default constructor. </summary>
        public ProviderDashBoardChart()
        {
            InitializeComponent();
            RefreshTableNameStatus();
            parameterTreeView1.Clear();
            parameterTreeView1.UseCheckBoxes = true;
        }

        //------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="MyConnection"> connection to database </param>
        ///-------------------------------------------------------------------------------------------------

        public ProviderDashBoardChart(UniDbConnection MyConnection)
        {
            InitializeComponent();
            FDbConnection = MyConnection;
            parameterTreeView1.Clear();
            parameterTreeView1.UseCheckBoxes = true;
        }
        //------------------------------------------------
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the database connection. </summary>
        ///<remarks> This control must have an open database connection assigned to it</remarks>
        /// <value> The database connection. </value>
        ///-------------------------------------------------------------------------------------------------
        public UniDbConnection AssignDbConnection
        {
            get { return FDbConnection; }
            set { FDbConnection = value; }
        }
        //------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the SQL server. </summary>
        ///
        /// <value> The SQL server. </value>
        ///-------------------------------------------------------------------------------------------------

        public SQLServer SQLServer
        {
            get { return FSQLServer; }
            set { FSQLServer = value; }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the WaterSim ParameterManager. </summary>
        ///<remarks> This control must have a ParameterManager assigned to it</remarks>
        /// <value> The parameter manager. </value>
        ///-------------------------------------------------------------------------------------------------
        public ParameterManagerClass AssignParameterManager
        {
            set {
                if (value != null)
                    FPM = new ShadowParameterManager(value);
                else
                    FPM = null;
            }
            get { return FPM; }
        }
     
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the tablename box. </summary>
        ///
        /// <value> The tablename box. </value>
        ///-------------------------------------------------------------------------------------------------

        public ToolStripComboBox TablenameBox
        {
            get { return TablenameComboBox; }
        }
        //------------------------------------------------
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the tablename box is shown. </summary>
        /// <remarks> This control can either manager what table to display data for, or the user can assign the current
        ///           tablename externally.  Setting this to true displays a tablename combobox that will be filled when the DBConnection is 
        ///           assigned, otherwise this combobox is hidden and the user controls externally what table will be displayed
        ///           </remarks>
        /// <value> true if show tablename box, false if not. </value>
        ///-------------------------------------------------------------------------------------------------
        public bool ShowTablenameComboBox
        {
            get { return FShowTablenameComboBox; }
            set
            {
                FShowTablenameComboBox = value;
                RefreshTableNameStatus();
            }
        }
        //------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the parameter data table. </summary>
        ///
        /// <value> The parameter data table. </value>
        ///-------------------------------------------------------------------------------------------------

        public DataTable ProviderDataTable
        {
            get { return FDataTable; }
            set
            {
                if (FDataTable != null)
                    FDataTable.Dispose();
                FDataTable = value;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the tablename. </summary>
        ///
        /// <value> The tablename. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Tablename
        {
            get
            {
                return FTablename;
            }
            set
            {
                FTablename = value;
                if (FShowTablenameComboBox)
                {
                    TablenameComboBox.Text = value;
                }
                else
                {
                    TablenameBox.Text = value;
                }
                string tablename = value;
                if (tablename != "")
                {
                    string errMessage = "";
                    if (!SetTablename(tablename, ref errMessage))
                    {
                        MessageBox.Show(errMessage);
                    }
                }

            }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Sets the Current tablename and Parameter and Provider Lists. </summary>
        /// <param name="Tablename"> The tablename. </param>
        /// <param name="ParmList">  List of parameters. </param>
        /// <param name="ProvList">  List of providers. </param>
        ///
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool SetTablename(string Tablename, DataTable TheTable, List<string> ParmList, List<string> ProvList)
        {
            bool result = false;

            string errString = "";
            // Set Parameters
            //parameterTreeView1.Items.Clear();
            foreach (string str in ParmList)
            {
                parameterTreeView1.FieldSet(str); //parameterTreeView1.Items.Add(str);
            }
            // Set Providers
            ProvidernamesComboBox.Items.Clear();
            foreach (string str in ProvList)
            {
                ProvidernamesComboBox.Items.Add(str);
            }
            FTablename = Tablename;
           
            if (TheTable == null)
            {
                FDataTable = UniDB.Tools.LoadTable(FDbConnection, Tablename, ref result, ref errString);
            }
            else
            {
                ProviderDataTable = TheTable;
            }
            return result;
        }
        //------------------------------------------------
        internal void RefreshTableNameStatus()
        {
            if (FShowTablenameComboBox)
            {
               //ProviderDashBoardTablenameLabel.Visible = true;

                toolStripTablename.Show();
            }
            else
            {
                //ProviderDashBoardTablenameLabel.Visible = false;
                toolStripTablename.Hide();
            }
        }        //------------------------------------------------
        public List<string> Tablenames
        {
            set
            {
                TablenameComboBox.Items.Clear();
                foreach (string tn in value)
                {
                    TablenameComboBox.Items.Add(tn);
                }
            }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the selected scenario. </summary>
        ///
        /// <value> The selected scenario. </value>
        ///-------------------------------------------------------------------------------------------------

        public string SelectedScenario
        {
            get { return FSelectedScenario; }
            set
            {
                FSelectedScenario = value;
                
            }
        }
        //------------------------------------------------
//        public List<string> Providernames
  //      {

            //set 
            //{
            //    ProvidernamesComboBox.Items.Clear();
            //    foreach (string pn in value)
            //    {
            //        ProvidernamesComboBox.Items.Add(pn);
            //    }
            //}
        
    //    }
    //    
       
        //------------------------------------------------
        const string NOSCENARIOS = "No Scenarios Names In Table";
        const string FIRSTSCENARIO = "First Scenario in Table";

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Sets a tablename. </summary>
        ///
        
        /// <param name="tablename">  The tablename. </param>
        /// <param name="errMessage"> [in,out] Message describing the error. </param>
        ///
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool SetTablename(string tablename, ref string errMessage)
        {
            bool iserror = true;
            string errString = "";

            // Fetch the Parameters and Names for this table
            DataTable SelectedTable;
            
            SelectedTable =  UniDB.Tools.LoadTable( FDbConnection, tablename, 1 , ref iserror, ref errString);

            parameterTreeView1.ParameterManager = FPM;
            parameterTreeView1.Clear();
            if (!iserror)
            {
                //parameterTreeView1.Items.Clear();

                // get all the different paramters in order
                List<string> parmlist = WaterSimManager_DB.ParameterFieldsInTable(SelectedTable, FPM, false, false, false, modelParamtype.mptUnknown);
                parameterTreeView1.SetFieldsActive(parmlist);

            //    parmlist = WaterSimManager_DB.ParameterFieldsInTable(SelectedTable, FPM, true, false, true, modelParamtype.mptOutputProvider);
            //    foreach (string str in parmlist)
            //    {
            //        //parameterTreeView1.Items.Add(str);
            //    }
            //    parmlist = WaterSimManager_DB.ParamtersInTable(SelectedTable, FPM, true, false, true, modelParamtype.mptInputBase);
            //    foreach (string str in parmlist)
            //    {
            //        //parameterTreeView1.Items.Add(str);
            //    }
            //    parmlist = WaterSimManager_DB.ParamtersInTable(SelectedTable, FPM, true, false, true, modelParamtype.mptInputProvider);
            //    foreach (string str in parmlist)
            //    {
            //        //parameterTreeView1.Items.Add(str);
            //    }
            //    parmlist = WaterSimManager_DB.ParamtersInTable(SelectedTable, FPM, false, true, false, 0);
            //    foreach (string str in parmlist)
            //    {
            //        //parameterTreeView1.Items.Add(str);
            //    }
            }

            char stringquote = Tools.SQLStringQuote(FDbConnection.SQLServerType);
            // get the year
            string ScnName =  SelectedTable.Rows[0][WaterSimManager_DB.rdbfScenarioName].ToString(); 
            string firstyear = SelectedTable.Rows[0][WaterSimManager_DB.rdbfSimYear].ToString();                // ok get the provider codes in the table
            string JustFirstYear = WaterSimManager_DB.rdbfSimYear+" = "+firstyear+" AND "+WaterSimManager_DB.rdbfScenarioName + " = " +stringquote+ ScnName + stringquote;
            SelectedTable = UniDB.Tools.LoadTable(FDbConnection, tablename, JustFirstYear, ref iserror, ref errString);
            if (!iserror)
            {
                List<string> ProvList = WaterSimManager_DB.ProvidersInTable(SelectedTable);
                ProvidernamesComboBox.Items.Clear();
                foreach (string str in ProvList)
                {
                    ProvidernamesComboBox.Items.Add(str);
                }
            }

            // get the scenarios. load table with just the required fields which all is need to fethc scenario names
            List<string> GetTheseFields = WaterSimManager_DB.RequiredFieldsList();
            SelectedTable = UniDB.Tools.LoadTable(FDbConnection, tablename, GetTheseFields, ref iserror, ref errString);
                // Get the scenario names, if they are there
             if (!iserror)
             {
                List<string> ScnList = WaterSimManager_DB.ScenarioNamesInTable(SelectedTable);
                ScenarioNamesComboBox.Items.Clear();
                if (ScnList.Count > 0)
                {
                    ScenarioNamesComboBox.Items.Add(FIRSTSCENARIO);
                    foreach (string str in ScnList)
                    {
                        ScenarioNamesComboBox.Items.Add(str);
                    }
                    
                }
                else
                    ScenarioNamesComboBox.Items.Add(NOSCENARIOS);
            }

            // OK, if no error so far, then load the whole table
             if (!iserror)
             {
                 FDataTable = UniDB.Tools.LoadTable(FDbConnection, tablename, ref iserror, ref errString);
             }
            return !iserror;
        }
        
        private void TablenameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tablename = TablenameComboBox.SelectedItem.ToString();
            Tablename = tablename;
        }
        //----------------------------------------------
        internal string ExtractCode(string value, ref string Label)
        {
            string temp = "";
            int index = value.IndexOf(WaterSimManager_DB.CODELABELSEPERATOR);
            if (index > 1)
            {
                temp = value.Substring(0, index).Trim();
                Label = value.Substring(index + WaterSimManager_DB.CODELABELSEPERATOR.Length );

            }
            else
            {
                temp = value;
                Label = value;
            }
            return temp;
        }

        private void DrawGraphButton_Click(object sender, EventArgs e)
        {
            string pcode = "";
            string pLabel = "";
            List<fieldinfo> fiList = new List<fieldinfo>();

            // Are we ready?
            // dbConnect?
            if (FDbConnection != null)
            {
                // Data Table Selected ?
                if (FDataTable != null)
                {
                    // Provider selected
                    if (ProvidernamesComboBox.SelectedIndex > -1)
                    {
                        pcode = ExtractCode(ProvidernamesComboBox.SelectedItem.ToString(), ref pLabel);
                        if (pcode != "")
                        {
                            List<ParmTreeNode> myCheckedItems = parameterTreeView1.CheckedItems;
                            // Parameters checked
                            if (myCheckedItems.Count > 0)
                            {
                                // Build the field list
                                foreach (ParmTreeNode PTN in myCheckedItems)
                                {
                                    if (PTN.isParamItemNode)
                                    {
                                    string fldstr = PTN.ParmItem.Fieldname;
                                    string lblstr = PTN.ParmItem.Label;
                                    fiList.Add(new fieldinfo(fldstr, lblstr));
                                }}
                                string SelectedScenario = FSelectedScenario;
                                if ((SelectedScenario == NOSCENARIOS) || (SelectedScenario == FIRSTSCENARIO))
                                    SelectedScenario = "";
                                
                                waterSimChartControl1.Manager.BuildAnnualProviderGraph(FDataTable, FDbConnection, SelectedScenario, fiList, pcode, "Model Parametes for " + pcode + " " + pLabel);
                            }
                            else
                            {
                                MessageBox.Show("At least one parameter field must be checked!", "Attention");
                                parameterTreeView1.Focus();
                            }

                        }

                    }
                    else
                    {
                        MessageBox.Show("A provider must be selected!", "Attention");
                        ProvidernamesComboBox.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("A data table must be selected", "Attention!");
                    TablenameComboBox.Focus();
                }
            }
            else 
            {
                MessageBox.Show("Database must be opened", "Error");
            }
            // DataTable
            // ItemsChecked
            // providerselected
            // Get provider code

            // get list of fields


        }

        const int PANELINCREASE = 300;
        private void ExpandParameterList_Click(object sender, EventArgs e)
        {
            if (ParameterPanel.Width<PANELINCREASE)
            {
                ParameterPanel.Width += PANELINCREASE;
                waterSimChartControl1.Location = new Point(425,waterSimChartControl1.Location.Y);
                waterSimChartControl1.Width = waterSimChartControl1.Width - PANELINCREASE;
                ExpandParameterList.Text = "<";
            }
                else
            {
                ParameterPanel.Width = 100;
                waterSimChartControl1.Location = new Point(125, waterSimChartControl1.Location.Y);
                waterSimChartControl1.Width += PANELINCREASE;
                ExpandParameterList.Text = ">";
            }
        }

        private void ScenarioNamesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FSelectedScenario = ScenarioNamesComboBox.Text;
        }
    }
}
