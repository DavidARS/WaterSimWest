// ===========================================================
//     WaterSimDCDC Regional Water Demand and Supply Model Version 5.0

//       A component that provides controls to support a provider parameter chart
//      Charts a single parameter over time for multiple providers

//       WaterSimDCDC.Controls_ParamGraph
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
using WaterSimDCDC;
using UniDB;
using System.Data.OleDb;


namespace WaterSimDCDC.Controls
{
    
    /// <summary>   Parameter dashboard chart. </summary>
    public partial class ParameterDashboardChart : UserControl
    {
    
        UniDbConnection FDbConnection = null;
        ShadowParameterManager FPM;
        DataTable FDataTable = null;
        bool FShowTablenameComboBox = true;
        string FTablename = "";
        string FSelectedScenario = "";
        SQLServer FSQLServer = SQLServer.stAccess;

        /// <summary>   Default constructor. </summary>
        public ParameterDashboardChart()
        {
            InitializeComponent();
            RefreshTableNameStatus();

        }
        //------------------------------------------------
        public ParameterDashboardChart(UniDbConnection MyConnection)
        {
            InitializeComponent();
            FDbConnection = MyConnection;
        }
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
        // ------------------------------------------------. 

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
                     if (value!=null)
                         FPM = new ShadowParameterManager(value);
                    else
                         FPM = null;
            }
            get { return FPM; }
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

        public DataTable ParameterDataTable
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
        /// <summary>   Gets or sets the tablename. </summary>
        /// <remarks> This control can either manager what table to display data for using its own control, or the user can assign the current
        ///           tablename externally.  Setting this sets the current tablename to use for displaying data.
        ///           </remarks>
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
                if (!FShowTablenameComboBox)
                {
                    FTablename = value;
                    string errMessage = "";
                    if (!SetTablename(value, ref errMessage))
                    {
                        MessageBox.Show(errMessage);
                    }
                }
            }
        }
        //---

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
        // ---------------------------------------------
        int PanelY = 28;
        internal void RefreshTableNameStatus()
        {
            if (FShowTablenameComboBox)
            {
                //labelTablename.Visible = true;
                toolStripTableScenarioName.Show();
                PanelDashboard.Location = new Point(PanelDashboard.Location.X, PanelY);
                int newheight = this.Height - (toolStripTableScenarioName.Height + statusStrip1.Height + 5);
                PanelDashboard.Size = new System.Drawing.Size(PanelDashboard.Size.Width, newheight);

            }
            else
            {
                //labelTablename.Visible = false;
                PanelY = PanelDashboard.Location.Y;
                toolStripTableScenarioName.Hide();
                PanelDashboard.Location = new Point(PanelDashboard.Location.X, 0);
                int newheight = this.Height - (statusStrip1.Height + 5);
                PanelDashboard.Size = new System.Drawing.Size(PanelDashboard.Size.Width, newheight);
            }
        }
        //------------------------------------------------
        public ToolStripComboBox TablenameBox
        {
            get { return ParmTablenameComboBox; }
        }
        //------------------------------------------------


        internal List<string> Tablenames
        {
            set
            {
                ParmTablenameComboBox.Items.Clear();
                foreach (string tn in value)
                {
                    ParmTablenameComboBox.Items.Add(tn);
                }
            }
        }
        //--------------------------------------------------------------------------------------------
        const int PANELINCREASE = 300;
        private void ExpandProviderList_Click(object sender, EventArgs e)
        {
            if (ProviderPanel.Width < PANELINCREASE)
            {
                ProviderPanel.Width += PANELINCREASE;
                waterSimChartControl1.Location = new Point(425, waterSimChartControl1.Location.Y);
                waterSimChartControl1.Width = waterSimChartControl1.Width - PANELINCREASE;
                ExpandProviderListPanel.Text = "<";
            }
            else
            {
                ProviderPanel.Width = 100;
                waterSimChartControl1.Location = new Point(125, waterSimChartControl1.Location.Y);
                waterSimChartControl1.Width += PANELINCREASE;
                ExpandProviderListPanel.Text = ">";
            }
        }
        //------------------------------------------------
        const string NOSCENARIOS = "No Scenarios Names In Table";
        const string FIRSTSCENARIO = "First Scenario in Table";
 
        private bool SetTablename(string tablename, ref string errString)
        {
            char stringquote = Tools.SQLStringQuote(FDbConnection.SQLServerType);
            bool iserror = true;
            // Fetch the Parameters and Names for this table
            DataTable SelectedTable;
            
            SelectedTable = UniDB.Tools.LoadTable( FDbConnection, tablename, ref iserror, ref errString);
  
            if (!iserror)
            {
                ParmComboBox.Items.Clear();

                // get all the different paramters in order
                List<string> parmlist = WaterSimManager_DB.ParamtersInTable(SelectedTable, FPM, true, false, true, modelParamtype.mptOutputBase);
                foreach (string str in parmlist)
                {
                    ParmComboBox.Items.Add(str);
                }

                parmlist = WaterSimManager_DB.ParamtersInTable(SelectedTable, FPM, true, false, true, modelParamtype.mptOutputProvider);
                foreach (string str in parmlist)
                {
                    ParmComboBox.Items.Add(str);
                }
                parmlist = WaterSimManager_DB.ParamtersInTable(SelectedTable, FPM, true, false, true, modelParamtype.mptInputBase);
                foreach (string str in parmlist)
                {
                    ParmComboBox.Items.Add(str);
                }
                parmlist = WaterSimManager_DB.ParamtersInTable(SelectedTable, FPM, true, false, true, modelParamtype.mptInputProvider);
                foreach (string str in parmlist)
                {
                    ParmComboBox.Items.Add(str);
                }
                parmlist = WaterSimManager_DB.ParamtersInTable(SelectedTable, FPM, false, true, false, 0);
                foreach (string str in parmlist)
                {
                    ParmComboBox.Items.Add(str);
                }

                List<string> Scnlist = WaterSimManager_DB.ScenarioNamesInTable(SelectedTable);

                string ScnName = SelectedTable.Rows[0][WaterSimManager_DB.rdbfScenarioName].ToString();
                string firstyear = SelectedTable.Rows[0][WaterSimManager_DB.rdbfSimYear].ToString();                // ok get the provider codes in the table
                string JustFirstYear = WaterSimManager_DB.rdbfSimYear + " = " + firstyear + " AND " + WaterSimManager_DB.rdbfScenarioName + " = " + stringquote + ScnName + stringquote;
                SelectedTable = UniDB.Tools.LoadTable(FDbConnection, tablename, JustFirstYear, ref iserror, ref errString);
                if (!iserror)
                {               // ok get the provider codes in the table
                    ProviderCheckedListBox.Items.Clear();
                    List<string> Provlist = WaterSimManager_DB.ProvidersInTable(SelectedTable);
                    foreach (string str in Provlist)
                    {
                        ProviderCheckedListBox.Items.Add(str);
                    }


                    // Get the scenario names, if they are there
                    ScenarioNamesComboBox.Items.Clear();
                    if (Scnlist.Count > 0)
                    {
                        ScenarioNamesComboBox.Enabled = true;
                        ScenarioNamesComboBox.Items.Add(FIRSTSCENARIO);
                        foreach (string str in Scnlist)
                        {
                            ScenarioNamesComboBox.Items.Add(str);
                        }
                    }
                    else
                    {
                        ScenarioNamesComboBox.Items.Add(NOSCENARIOS);
                        ScenarioNamesComboBox.Text = NOSCENARIOS;
                        ScenarioNamesComboBox.Enabled = false;
                    }
                }
                else
                    errString = "Error Finding Providers in Datatable";

            } else errString = "Error Finding WaterSim Parameters in DataTable";

            if (!iserror)
            {
                FTablename = tablename;
                FDataTable = UniDB.Tools.LoadTable(FDbConnection, tablename, ref iserror, ref errString);
            }   
            return !iserror;
        }
        // --------------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Sets the Current tablename and Parameter and Provider Lists. </summary>
        /// <param name="Tablename"> The tablename. </param>
        /// <param name="ParmList">  List of parameters. </param>
        /// <param name="ProvList">  List of providers. </param>
        ///
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool  SetTablename(string Tablename, DataTable TheTable, List<string> ParmList, List<string> ProvList)
        {
            bool result = false;
       
            string errString = "";
            // Set Parameters
            ParmComboBox.Items.Clear();
            foreach (string str in ParmList)
            {
                ParmComboBox.Items.Add(str);
            }
            // Set Providers
            ProviderCheckedListBox.Items.Clear();
            foreach (string str in ProvList)
            {
                ProviderCheckedListBox.Items.Add(str);
            }
            FTablename = Tablename;
            
            if (TheTable == null)
            {
                FDataTable = UniDB.Tools.LoadTable(FDbConnection, Tablename, ref result, ref errString);
            }
            else
            {
                ParameterDataTable = TheTable;
            }
            return result;
        }
        //------------------------------------------------
        private void TablenameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tablename = ParmTablenameComboBox.SelectedItem.ToString();
            if (tablename != "")
            {
                string errMessage = "";
                if (!SetTablename(tablename, ref errMessage))
                {
                    MessageBox.Show(errMessage);
                }
            }
        }
        //----------------------------------------------
        internal string ExtractCode(string value, ref string Label)
        {
            string temp = "";
            int index = value.IndexOf(WaterSimManager_DB.CODELABELSEPERATOR);
            if (index > 1)
            {
                temp = value.Substring(0, index).Trim();
                Label = value.Substring(index + WaterSimManager_DB.CODELABELSEPERATOR.Length);

            }
            else
            {
                temp = value;
                Label = value;
            }
            return temp;
        }

        //-------------------------------------------------------------------
        private void DrawGraphButton_Click(object sender, EventArgs e)
        {
            string pcode = "";
            string pLabel = "";
            string fldstr = "";
            string lblstr = "";

            List<providerinfo> pInfoList = new List<providerinfo>();
            // Are we ready?
            // dbConnect?
            if (FDbConnection != null)
            {
                // Data Table Selected ?
                if (FDataTable != null)
                {
                    // Provider selected
                    if (ParmComboBox.SelectedIndex > -1)
                    {
                        fldstr = ExtractCode(ParmComboBox.SelectedItem.ToString(), ref lblstr);
                        if (fldstr != "")
                        {
                            // Parameters checked
                            if (ProviderCheckedListBox.CheckedItems.Count > 0)
                            {
                                // Build the field list
                                foreach (string str in ProviderCheckedListBox.CheckedItems)
                                {
                                   pcode = ExtractCode(str, ref pLabel);
                                   pInfoList.Add(new providerinfo(pcode,pLabel));
                                }
                                // Get the Scenario
                               
                                if ((FSelectedScenario == NOSCENARIOS) || (FSelectedScenario == FIRSTSCENARIO))
                                     FSelectedScenario = "";
                                waterSimChartControl1.Manager.BuildAnnualParameterGraph(FDataTable, FDbConnection, FSelectedScenario, pInfoList, fldstr, ParmComboBox.SelectedItem.ToString());
                            }
                            else
                            {
                                MessageBox.Show("At least one provider field must be checked!", "Attention");
                                ProviderCheckedListBox.Focus();
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("A parameter must be selected!", "Attention");
                        ParmComboBox.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("A data table must be selected", "Attention!");
                    ParmTablenameComboBox.Focus();
                }
            }
            else
            {
                MessageBox.Show("Database must be opened", "Error");
            }
        }

        private void SelectAllProvButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ProviderCheckedListBox.Items.Count; i++)
            {
                ProviderCheckedListBox.SetItemChecked(i, true); 
            }
           
           
        }

        private void ClearAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ProviderCheckedListBox.Items.Count; i++)
            {
                if (ProviderCheckedListBox.GetItemChecked(i)) { ProviderCheckedListBox.SetItemChecked(i, false); } 
            }
           
        }

        private void ScenarioNamesComboBox_Click(object sender, EventArgs e)
        {
            FSelectedScenario = ScenarioNamesComboBox.Text;
        }
    }
}
