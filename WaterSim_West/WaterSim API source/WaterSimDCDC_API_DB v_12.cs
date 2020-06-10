// ===========================================================
//     WaterSimDCDC Regional Water Demand and Supply Model Version 5.0

//       A Class the adds Database support to the WaterSimDCDC.WaterSim Class

//       WaterSimDCDC_API_DB Version 5.0
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
//      8/14/2015  Fixed FasterAppend to include Aggregates
//      
//      8/16/2018  Fixed datatable.clear() line in Simulation.Initialize
//====================================================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using DAO = Microsoft.Office.Interop.Access.Dao;
using UniDB;





/*******************************************
 * WaterSimManager DB
 * Version 11
 * Keeper Quay
 *   Requires WaterSimManagerClass to be deined in WaterSimDCDC namespace
 *   6/10/18  Added support for text files using unidb, must have a CSV created with desired headers to work
 * ******************************************/
namespace WaterSimDCDC
{
    /// <summary>   WaterSimManager with database support.  </summary>
    /// <remarks>This class wraps support for database  input and output of Simulation results around the WaterSimManager simulation methods.</remarks>

    public partial class WaterSimManager_DB : WaterSimDCDC.WaterSimManager_SIO
    {
        // Class Fields

        /// <summary> Used to Seperate Fieldcodes from Field Labels </summary>
        public const string CODELABELSEPERATOR = " : ";

        /// <summary> The database connection </summary>
        protected UniDbConnection FDbConnection;

        /// <summary> Filename of the data base file </summary>
        //protected string FDataBaseFilename = "";

        /// <summary> True if database is open </summary>
        protected bool FDbOpen = false;

        /// <summary>   Flag for adding region to database. </summary>
        protected bool FAddRegion = false;

        /// <summary>   Set to true when an error occurs </summary>
        static bool FisError = false;

        /// <summary>   Message describing the error. </summary>
        static string FErrorMessage = "";

        /// <summary> The SQL Server used of oledb Connection. </summary>
        protected static SQLServer FSQLServer = SQLServer.stAccess;

        // OK Create a DAO DBEngine and open the database
        DAO.DBEngine DAOdbEngine;
        DAO.Database DAOdbDatabase;

        // New UniDB Fields
        string FServerLocation = "";
        string FDatabasename = "";
        string FUserID = "";
        string FPassword = "";
        string FOtherConnectionOptions = "";

        public string WriteMode = "";
        public double FIDCounter = Tools.CreatePrimaryIDSeed();

        //AnnualSimulationResults AnnualResults;
        //SimulationResults DbRun_AnnualResults;

        SimulationStrings db_fieldnames;
        //--------------------------------------------------------------------
        // Constructor
        /// <summary>
        /// Constructs a WaterSimManager_DB object.  Creates a new DbConnection and opens the Database if a filename is provided.  Otherwise DBConnection is not created.
        /// </summary>
        /// <param name="TempDirectoryName">Path for temporary directory</param>
        /// <param name="DataDirectoryName">Path to WaterSimManager data</param>
        /// <param name="DatabaseFilename">Path and filename for the Database to be opened.</param>
        /// <exception cref="WaterSimManager_DB_Exception"> if unable to open the database.</exception>
        public WaterSimManager_DB(string DataDirectoryName, string TempDirectoryName)
            : base(DataDirectoryName, TempDirectoryName)
        {
            
        }

        public WaterSimManager_DB(SQLServer ServerType, string DataDirectoryName, string TempDirectoryName, string Databasename, string ServerLocation,
                                  string UserID, string Password, string OtherConnectionOptions)
            : base(DataDirectoryName, TempDirectoryName)
        {
            FSQLServer = ServerType;
            FDatabasename = Databasename;
            FServerLocation = ServerLocation;
            FUserID = UserID;
            FPassword = Password;
            FOtherConnectionOptions = OtherConnectionOptions;
            try
            {
                FDbConnection = new UniDbConnection(FSQLServer, FServerLocation, FDatabasename, FUserID, FPassword, FOtherConnectionOptions);
                FDbConnection.Open();
                if (FSQLServer == SQLServer.stText) { FDbConnection.UseFieldHeaders = true; }
                FDbOpen = true;
            }
            catch (Exception ex)
            {
                throw new WaterSim_Exception(WS_Strings.values[WS_Strings.wsBadServer] + " : " + ex.Message);
            }
        }
        //--------------------------------------------------------------------
        // Dispose

        /// <summary>
        ///     Called by Dispose() and ~WaterSimManager_DB() Finalizer  Prevents Finalizer from closing model
        ///     files.
        /// </summary>
        /// <param name="disposing">    Finalizer will call with a false. </param>

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing) if (FDbConnection != null) FDbConnection.Close();
        }
        //-----------------------------------------------------------------------------
        // Properties
        /// <summary>
        /// Property 
        /// This is the database connection, can be opened seperately and assigned with this property, must be in the Open state, otherwise and exception is thrown
        /// </summary>
        /// <remarks> OleDbConnection read/write</remarks>
        /// <exception cref="WaterSimManager_DB_Exception"> if OleDbConnection value passed is not valid or open.</exception>
        /// <exception cref="WaterSimManager_DB_Exception"> if OleDbConnection value is null.</exception>
        public UniDbConnection DbConnection
        {
            get { return FDbConnection; }
            set
            {
                if (value != null)
                {
                    if (value.State == ConnectionState.Open)
                    {
                        FDbConnection = value;
                        FSQLServer = value.SQLServerType;
                        FDatabasename = Tools.ExtractDatabaseName(FDbConnection);
                        if (FSQLServer == SQLServer.stAccess)
                        {
                            // OK Create a DAO DBEngine and open the database
                            OpenDAO(FDatabasename);
                        }
                        FDbOpen = true;
                    }
                    else
                    {
                        throw new WaterSimManager_DB_Exception(WS_Strings.wsUableOpenDbConnection);
                    }
                }
                else
                {
                    throw new WaterSimManager_DB_Exception(WS_Strings.wsdbConNull);
                }
            }
        }
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Property
        /// Datbase Filename for DbConnection.  If different than current database (or database not open) closes DbConnection and opens new using this filename.
        /// </summary>
        /// <remarks> string read/write</remarks>
        public string Databasename
        {
            get { return FDatabasename; }
            set
            {
                OpenDatabase(value);
            }
        }
        //-----------------------------------------------------------------------------
        public SQLServer ServerType
        {
            get { return FSQLServer; }
        }

        /// <summary>
        /// Property
        /// Returns true if the Database is open
        /// </summary>
        /// <value> bool read only</value>
        public bool DataBaseOpen
        { get { return FDbOpen; } }
        //-----------------------------------------------------------------------------
        //        public List<string> Tablenames
        //        { get { return FTablenames; } }
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Property
        /// Returns true if t
        /// </summary>
        public bool isError
        { get { return FisError; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets a value indicating whether a region summary should (can) be added to database. </summary>
        ///
        /// <value> true if add region to database, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool AddRegionToDB
        {
            get { return FAddRegion; }
            set { FAddRegion = value; }
        }


        //*********************************************
        // DATABASE SUPPORT ROUTINES
        // OpenDatabase
        // GetError Message
        // LoadTable
        // LoadWaterSimTable
        // CreateNewDataTable
        // ValidDataTable
        // *************************************************
        // 
        #region DBUtils

        protected void OpenDAO(string Filename)
        {
         //         DAOdbEngine = new DAO.DBEngine();
         //      DAOdbDatabase = DAOdbEngine.OpenDatabase(Filename);
        }



        //-----------------------------------------------------------------------------

        /// <summary>   Opens a database. </summary>
        /// <exception cref="Exception">if unable to open database </exception>
        ///<remarks>This uses dbConnection to open a database.  Uses the following Connection string
        ///			<code>
        ///         string Connectstr = "Provider=Microsoft.JET.OLEDB.4.0; data source=;User Id=admin;Password=;"
        ///			</code>
        ///			Filenmame is inserted after 'data source=' in the connection string</remarks>
        /// <param name="Filename"> Filename of the database. </param>


        protected virtual void OpenDatabase(string NewDatabasename)
        {
            try
            {
                FDbConnection = new UniDbConnection(FSQLServer, FServerLocation, NewDatabasename, FUserID, FPassword, FOtherConnectionOptions);

                FDatabasename = NewDatabasename;
                if (FSQLServer == SQLServer.stAccess)
                {
                    // OK Create a DAO DBEngine and open the database
                    OpenDAO(FDatabasename);
                }

                FDbOpen = true;
            }
            catch (Exception e)
            {
                FDbOpen = false;
                throw new Exception("WaterSimManager DB Error:  " + e.Message);
            }
        }

        public virtual bool OpenSQLServerDatabase(SQLServer ServerType, string Databasename, string ServerLocation, string UserID, string Password, string OtherConnectionOptions)
        {
            try
            {
                FDbConnection = new UniDbConnection(FSQLServer, FServerLocation, FDatabasename, FUserID, FPassword, FOtherConnectionOptions);
                FDbConnection.Open();
                return true;
            }
            catch (Exception ex)
            {
                throw new WaterSim_Exception(WS_Strings.values[WS_Strings.wsBadServer] + " : " + ex.Message);
            }
        }
        //-----------------------------------------------------------------------------
        internal static void SetError(string aMessage)
        {
            FErrorMessage = aMessage;
            FisError = true;
        }
        //-----------------------------------------------------------------------------

        /// <summary>   Gets the error message. </summary>
        /// <returns>   An error message if isError is true, otherwise returns "". </returns>
        /// <value> ErrorMessage</value>
        public static string GetErrorMessage()
        {
            string temp = FErrorMessage;
            FisError = false;
            FErrorMessage = "";
            return temp;
        }
        //-----------------------------------------------------------------------------
        // Required Fields

        /// <summary> Number of required fields for WaterSimManager_DB DataTables</summary>
        public const int NumberOfRequiredFields = 5;

        /// <summary> Scenario name db fieldname</summary>
        /// <value>a fieldname string const</value>
        /// <remarks> This column will contain the scenario name.  This is one of five fields required to be in a WaterSimManager_DB valid datatable</remarks>
        /// <seealso cref="Valid_DataTable"/>
        public const string rdbfScenarioName = "SCN_NAME";

        /// <summary> Simulation year db fieldname</summary>
        /// <value>a fieldname string const</value>
        /// <remarks>This column will contain the year of the scenario.  This is one of five fields required to be in a WaterSimManager_DB valid datatable</remarks>
        /// <seealso cref="Valid_DataTable"/>
        public const string rdbfSimYear = "SIMYEAR";

        /// <summary> Provider label db fieldname</summary>
        /// <value>a fieldname string const</value>
        /// <remarks> This is one of five fields required to be in a WaterSimManager_DB valid datatable</remarks>
        /// <seealso cref="Valid_DataTable"/>
        public const string rdbfProviderLabel = "PRVDLABEL";

        /// <summary> Provider code db fieldname</summary>
        /// <value>a fieldname string const</value>
        /// <remarks> This is a column for the provider fieldname values.  Each row of each year will have a different provider firldname in this column.  This is one of five fields required to be in a WaterSimManager_DB valid datatable</remarks>
        /// <seealso cref="Valid_DataTable"/>
        /// <seealso cref="ProviderClass"/>
        /// <seealso cref="ProviderClass.FieldName"/>
        public const string rdbfProviderCode = "PRVDCODE";

        /// <summary> Primary key db fieldname </summary>
        /// <value>a fieldname string const</value>
        /// <remarks>This is the primary key, a unique long integer value. This is one of five fields required to be in a WaterSimManager_DB valid datatable</remarks>
        /// <seealso cref="Valid_DataTable"/>
        public const string rdbfID = "ID";

        /// <summary> The required fields for a WaterSimManager_DB DataTable</summary>
        public static string[] RequiredFields = new string[NumberOfRequiredFields] { rdbfID, rdbfScenarioName, rdbfSimYear, rdbfProviderCode, rdbfProviderLabel };

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the required fields list. </summary>
        ///
        /// <returns>  List of Strings </returns>
        ///-------------------------------------------------------------------------------------------------

        public static List<string> RequiredFieldsList()
        {
            List<string> temp = new List<string>();
            foreach (string value in RequiredFields)
                temp.Add(value);
            return temp;
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a SQL string that identifies required fields. </summary>
        ///
        /// <returns>  a SQL string </returns>
        ///-------------------------------------------------------------------------------------------------

        public static string RequiredFieldsSQL(SQLServer ServerType)
        {
            string temp = RequiredFields[0] + " " + UniDB.Tools.SQLDataDefine(System.Type.GetType(SQLRequiredDefineTypes[0]), ServerType, SQLRequiredDefineWidth[0]);
            for (int i = 1; i < NumberOfRequiredFields; i++)
            {
                temp += ", " + RequiredFields[i] + " " + UniDB.Tools.SQLDataDefine(System.Type.GetType(SQLRequiredDefineTypes[i]), ServerType, SQLRequiredDefineWidth[i]);
            }
            return temp;
        }
        /// <summary> The required SQL FIeld defines </summary>
        protected static string[] RequiredDefines = new string[NumberOfRequiredFields] { "DOUBLE", "CHAR(100)", "INT", "CHAR(5)", "CHAR(25)" };
        protected static string[] SQLRequiredDefineTypes = new string[NumberOfRequiredFields] { "System.Double", "System.String", "System.Integer", "System.String", "System.String" };
        protected static int[] SQLRequiredDefineWidth = new int[NumberOfRequiredFields] { 0, 100, 0, 5, 25 };
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Static method to load all fields all records of a table from a database using OleDbConnection that is already open
        /// uses sql command   "Select * from [tablename]"
        /// </summary>
        /// <param name="dbConnection">oleDbConection that is connected to database</param>
        /// <param name="tablename"> name of the table to load</param>
        /// <returns>DataTable </returns>
        //public static DataTable LoadTable(OleDbConnection dbConnection, string tablename)
        //{
        //    DataTable DT = new DataTable();
        //    if (dbConnection!=null)
        //    { 
        //        if (dbConnection.State == ConnectionState.Open)
        //        {
        //            if  (tablename!="") 
        //            {
        //                string sqltext = "";
        //                try
        //                {
        //                    sqltext = "Select * from " + '[' + tablename + ']';
        //                    OleDbDataAdapter NewRawTableAdapter = new OleDbDataAdapter(sqltext, dbConnection);
        //                    NewRawTableAdapter.Fill(DT);
        //                }
        //                catch
        //                {
        //                    SetError("Fatal Error reading table ["+tablename+"] from "+dbConnection.ConnectionString+" using <"+sqltext+">");
        //                }
        //            }
        //            else
        //            {
        //                    SetError("Fatal Error reading table ["+tablename+"]. Tablename must not be null");
        //            }
        //       }
        //        else
        //        {
        //                    SetError("Fatal Error reading table ["+tablename+"]. dbConnection must be Open");
        //        }
        //    }
        //    else
        //    {
        //                    SetError("Fatal Error reading table ["+tablename+"]. dbConnection cannot be null");
        //    }
        //    return DT;
        //}


        //-----------------------------------------------------------------------------
        /// <summary>
        /// Method to load all fields all records of a table from a database using WaterSim DbConnection
        /// If DbConnect is not opne then generates exception
        /// uses sql command   "Select * from [tablename]"
        /// </summary>
        /// <param name="tablename"> name of the table to load</param>
        /// <returns>DataTable </returns>
        public DataTable LoadWaterSimTable(string tablename)
        {
            bool iserror = true;
            string ErrMEssage = "";
            DataTable DT = UniDB.Tools.LoadTable(FDbConnection, tablename, ref iserror, ref ErrMEssage);
            if (iserror)
                throw new WaterSim_Exception("WaterSim DB Error " + ErrMEssage);
            return DT;
        }

        //------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the required fields for WaterSimDB output file in a SQL data definition string. </summary>
        ///
        /// <remarks> Ray, 1/14/2013. </remarks>
        ///
        /// <returns> . </returns>
        ///-------------------------------------------------------------------------------------------------

        //public static string DataDefineRequired()
        //{
        //    string temp = RequiredFields[0] + " " + RequiredDefines[0];
        //    for (int i = 1; i < NumberOfRequiredFields; i++)
        //        temp += " , " + RequiredFields[i] + " " + RequiredDefines[i];

        //    return temp;
        //}


        //-----------------------------------------------------------------------------
        /// <summary>   SQL data define. </summary>
        /// <remarks>Creates SQL syntax for the field list portion of a CREATE TABLE command ie CREATETABLE [TABLENAME] (FIELDLIST)</remarks>
        /// <returns>  SQL syntax string </returns>

        protected string DataDefine()
        {
            string temp;
            temp = RequiredFieldsSQL(FDbConnection.SQLServerType);

            ModelParameterBaseClass MP;
            foreach (int eP in _pm.eModelParameters())
            {
                MP = _pm.Model_ParameterBaseClass(eP);
                temp += ", " + MP.Fieldname + " INT";
            }
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> SQL data define. </summary>
        /// <param name="ModelParams"> List of eModelParams to include as fields </param>
        ///
        /// <returns> SQL syntax string. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected string DataDefine(List<int> ModelParams)
        {

            string temp;
            temp = RequiredFieldsSQL(FDbConnection.SQLServerType);

            ModelParameterBaseClass MP;

            foreach (int emp in ModelParams)
            {
                MP = _pm.Model_ParameterBaseClass(emp);
                temp += ", " + MP.Fieldname + " INT";
            }
            return temp;
        }
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates a new table in the open database. Creates all possible fields.
        /// </summary>
        /// <param name="NewTablename">tablename Must be a unique name. </param>
        /// <returns>true if created, otherwise false.  If false check isError and GetErrorMessage()</returns>
        /// <seealso cref="isError"/>
        /// <seealso cref="GetErrorMessage"/>
        /// <remarks>Must be a unique name. Creates all possible fields, including Provider label</remarks>
        public bool CreateNewDataTable(string NewTablename)
        {
            List<string> FTablenames = new List<string>();
            Boolean IsDone = false;
            bool iserror = true;
            string ErrMessgae = "";
            // Get Table Names from Server
            FTablenames = FDbConnection.GetTableNames(ref iserror, ref ErrMessgae);
            //DataTable REGIONSchema = new DataTable();
            //REGIONSchema = FDbConnection.GetSchema("TABLES");
            //for (int i = 0; i < REGIONSchema.Rows.Count; i++)
            //{
            //    if (REGIONSchema.Rows[i]["TABLE_TYPE"].ToString() == "TABLE")
            //    {
            //        FTablenames.Add(REGIONSchema.Rows[i]["TABLE_NAME"].ToString());
            //    }
            //}
            // check and see if it is in the tablename list
            if (FTablenames.Find(delegate(string value) { if (value == NewTablename) return true; else return false; }) != NewTablename)
            {
                string tc = "CREATE TABLE ";
                string dataDef = DataDefine();
                DbCommand cmd = FDbConnection.CreateCommand();

                if (NewTablename.Length > 0)
                {
                    tc += UniDB.Tools.BracketIt(FDbConnection.SQLServerType, NewTablename) + " (" + dataDef + ");";
                    cmd.CommandText = tc;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        if (FSQLServer == SQLServer.stAccess)
                        {
                            bool done = false;

                            List<string> names = FDbConnection.GetTableNames(ref iserror, ref ErrMessgae);
                            int cnt = 0;
                            while ((!done) && (cnt < 5))
                            {
                                //System.Threading.Thread.Sleep(1000);
                                try
                                {
                                    DAO.Recordset temp = DAOdbDatabase.OpenRecordset(NewTablename);
                                    done = true;
                                }
                                catch
                                {
                                    cnt++;
                                    System.Threading.Thread.Sleep(1000);
                                }

                            }
                        } IsDone = true;
                    }
                    catch (Exception ex)
                    {
                        SetError(ex.Message);
                        cmd.Dispose();
                    }
                }
            }
            else
            {
                SetError(NewTablename + " is existing table, must be unique tablename");
            }
            return IsDone;
        }
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates a new table in the open database using only specified fields
        /// </summary>
        /// <param name="NewTablename">tablename Must be a unique name. </param>
        /// <param name="ModelParams"> List of eModelParam that is used to determine waht fields to include in the datanase</param>
        /// <returns>true if created, otherwise false.  If false check isError and GetErrorMessage()</returns>
        /// <seealso cref="isError"/>
        /// <seealso cref="GetErrorMessage"/>
        /// <remarks>Must be a unique name. Uses ModelParams to create fields, DOES NOT INCLUDE PROVIDER LABEL</remarks>
        public bool CreateNewDataTable(string NewTablename, List<int> ModelParams)
        {
            List<string> FTablenames = new List<string>();
            Boolean IsDone = false;
            bool iserror = true;
            string ErrMessage = "";

            FTablenames = FDbConnection.GetTableNames(ref iserror, ref ErrMessage);

            // Get Table Names from the Schema
            //DataTable REGIONSchema = new DataTable();
            //REGIONSchema = FDbConnection.GetSchema("TABLES");
            //for (int i = 0; i < REGIONSchema.Rows.Count; i++)
            //{
            //    if (REGIONSchema.Rows[i]["TABLE_TYPE"].ToString() == "TABLE")
            //    {
            //        FTablenames.Add(REGIONSchema.Rows[i]["TABLE_NAME"].ToString());
            //    }
            //}
            // check and see if it is in the tablename list
            if (FTablenames.Find(delegate(string value) { if (value == NewTablename) return true; else return false; }) != NewTablename)
            {
                string tc = "CREATE TABLE ";
                string dataDef = DataDefine(ModelParams);
                DbCommand cmd = FDbConnection.CreateCommand();

                if (NewTablename.Length > 0)
                {
                    tc += UniDB.Tools.BracketIt(FDbConnection.SQLServerType, NewTablename) + " (" + dataDef + ")";
                    cmd.CommandText = tc;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        IsDone = true;
                        // This is a hack to Force a wait until this table is committed to the Database tablenames
                        // Only need if doing DAO.  Should see if there is a way with DAO to wait for this
                        if (FSQLServer == SQLServer.stAccess)
                        {
                            bool done = false;

                            List<string> names = FDbConnection.GetTableNames(ref iserror, ref ErrMessage);
                            int cnt = 0;
                            while ((!done) && (cnt < 5))
                            {
                                //System.Threading.Thread.Sleep(1000);
                                try
                                {
                                    DAO.Recordset temp = DAOdbDatabase.OpenRecordset(NewTablename);
                                    done = true;
                                }
                                catch
                                {
                                    cnt++;
                                    System.Threading.Thread.Sleep(1000);
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SetError(ex.Message);
                        cmd.Dispose();
                    }
                }
            }
            else
            {
                SetError(NewTablename + " is existing table, must be unique tablename");
            }
            return IsDone;
        }
        //-----------------------------------------------------------------------------
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Check if Valid WaterSimManager_DB datatable
        /// </summary>
        /// <param name="Target">a DataTable to check</param>
        /// <returns>true if valid otherwise false</returns>
        /// <remarks>Checks if the DataTable has the required fields to be a valid WaterSimManager data table.  Five fields are required and constants are provided for these five fields,
        ///       rdbfID,  rdbfScenarioName,  rdbfSimYear ,  rdbfProviderLabel, and  rdbfProviderCode.  Below is SQL command syntax for creating such a table.
        ///      <code> 
        ///           OleDbConnection dbConnect = new OleDbConnection(MyConnectionstring);
        ///           OleDbCommand cmd = dbConnect.CreateCommand();
        ///            cmd.CommandText = "CREATE TABLE [MYTABLENAME] ("+
        ///                 rdbfID                        + "COUNTER PRIMARY KEY, "+ 
        ///                rdbfScenarioName    + " CHAR(50), "+
        ///                rdbfSimYear              + " INT, "+
        ///                rdbfProviderLabel      + " CHAR(25), "+
        ///                rdbfProviderCode      + " CHAR(5)  )"
        ///            cmd.ExecuteNonQuery();
        ///            </code>
        ///            </remarks>

        public static bool Valid_DataTable(DataTable Target)
        {
            bool isValid = false;

            for (int i = 0; i < NumberOfRequiredFields; i++)
            {
                isValid = Target.Columns.Contains(RequiredFields[i]);
                if (!isValid) break;
            }

            return isValid;
        }
        //------------------------------------------------------------------------------
        /// <summary>   Loads a scenario nput Parameters from a database. </summary>
        /// <remarks> This method will load Input Model Parameters (BaseInput and ProviderInput) into a SimulationInputs struct.  
        /// 		 The Datatable must be a valid WaterSimManager_DB datatable (see Valid_DataTable  
        /// 		 However fields for all input parameters do not need to be in the Datatable.  This method will load whatever ModelParameter.Fieldname 
        /// 		 fields it finds.  It does not set values in sInputs struct for Model Parameters it finds.  It is good practice to fill
        /// 		 the sInputs struct with the models current input values before passing it to this method.  See example</remarks>
        /// <param name="ScenarioName"> Name of the scenario, if empty (="") then the first scenario found is used. </param>
        /// <param name="year">         The year. If 0 then the first year found is used</param>
        /// <param name="Source">       Datatable Source. </param>
        /// <param name="sInputs">      [in,out] SimulationInputs. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails.  If false check isError and GetErrorMessage() </returns>
        /// <exception cref="WaterSim_Exception">when Source datatable is not a valid WaterSimManager_DB datatable. </exception>
        /// <example><code>
        ///          try
        ///                {
        ///                    OleDbDataAdapter TableAdapter = new OleDbDataAdapter("Select * from " + '[' + tablename + ']', WSim.DbConnection);
        ///                    TableAdapter.Fill(DT);
        ///                    TableAdapter.Dispose();
        ///                    SimulationInputs SI = WSim.ParamManager.GetSimulationInputs();
        ///                    if (WSim.LoadScenario_DB("", 0, DT, ref SI))
        ///                     {
        ///                        if (WSim.ParamManager.SetSimulationInputs(SI))
        ///                        {
        ///                            WS_InputPanel.Refresh_Inputs();
        ///                        }
        ///                    }
        ///                }
        ///                catch (Exception e)
        ///                {
        ///                    MessageBox.Show("Whoops " + e.Message);
        ///                }
        /// 		 </code></example>
        /// <seealso cref="isError"/>
        /// <seealso cref="GetErrorMessage"/>
        /// <seealso cref="Valid_DataTable"/>
        /// <seealso cref="SimulationInputs"/>
        /// <seealso cref="ParameterManagerClass.GetSimulationInputs"/>
        /// <seealso cref="ParameterManagerClass.SetSimulationInputs"/>
        public bool LoadScenario_DB(string ScenarioName, int year, DataTable Source, ref SimulationInputs sInputs)
        {
            return LoadScenario_DB(ScenarioName, year, Source, ref sInputs, ParamManager);
        }
        //------------------------------------------------------------------------------

        /// <summary>   Loads a scenario from a database. </summary>
        /// <remarks>This is a static method. This method will load Input Model Parameters (BaseInput and ProviderInput) into a SimulationInputs struct.  
        /// 		 The Datatable must be a valid WaterSimManager_DB datatable (see Valid_DataTable  
        /// 		 However fields for all input parameters do not need to be in the Datatable.  This method will load whatever ModelParameter.Fieldname 
        /// 		 fields it finds.  It does not set values in sInputs struct for Model Parameters it finds.  It is good practice to fill
        /// 		 the sInputs struct with the models current input values before passing it to this method.  See example</remarks>
        /// <param name="ScenarioName"> Name of the scenario, if empty (="") then the first scenario found is used. </param>
        /// <param name="year">         The year. If 0 then the first year found is used</param>
        /// <param name="Source">       Datatable Source. </param>
        /// <param name="sInputs">      [in,out] SimulationInputs. </param>
        /// <param name="PM">           a ParameterManagerClass object to iteratre through known parameters and see if they are in the datatable</param>
        ///
        /// <returns>   true if it succeeds, false if it fails.  If false check isError and GetErrorMessage() </returns>
        /// <exception cref="WaterSim_Exception">when Source datatable is not a valid WaterSimManager_DB datatable. </exception>
        /// <example><code>
        ///          try
        ///                {
        ///                    OleDbDataAdapter TableAdapter = new OleDbDataAdapter("Select * from " + '[' + tablename + ']', WSim.DbConnection);
        ///                    TableAdapter.Fill(DT);
        ///                    TableAdapter.Dispose();
        ///                    SimulationInputs SI = WSim.ParamManager.GetSimulationInputs();
        ///                    if (WSim.LoadScenario_DB("", 0, DT, ref SI))
        ///                     {
        ///                        if (WSim.ParamManager.SetSimulationInputs(SI))
        ///                        {
        ///                            WS_InputPanel.Refresh_Inputs();
        ///                        }
        ///                    }
        ///                }
        ///                catch (Exception e)
        ///                {
        ///                    MessageBox.Show("Whoops " + e.Message);
        ///                }
        /// 		 </code></example>
        /// <seealso cref="isError"/>
        /// <seealso cref="GetErrorMessage"/>
        /// <seealso cref="Valid_DataTable"/>
        /// <seealso cref="SimulationInputs"/>
        /// <seealso cref="ParameterManagerClass.GetSimulationInputs"/>
        /// <seealso cref="ParameterManagerClass.SetSimulationInputs"/>


        public static bool LoadScenario_DB(string ScenarioName, int year, DataTable Source, ref SimulationInputs sInputs, ParameterManagerClass PM)
        {
            bool loadSuccess = false;
            string yearstr = "";
            int tempyr = 0;
            string tempMPvaluestr = "";
            int tempMPvalue = 0;
            string epCodestr = "";
            eProvider ep;
            int index = 0;
            if (!Valid_DataTable(Source)) throw new WaterSimManager_DB_Exception(WS_Strings.wsInvalidDataTable);

            try
            {
                // OK, first if scenarioname has not been picked then get the first one
                if (ScenarioName == "")
                {
                    DataRow DR = Source.Rows[0];
                    ScenarioName = DR[rdbfScenarioName].ToString();
                }
                // OK, if year has not been picked then get the first one
                if (year == 0)
                {
                    DataRow DR = Source.Rows[0];
                    yearstr = DR[rdbfSimYear].ToString();
                    if (yearstr == "")
                        year = 0;
                    else
                        year = Convert.ToInt16(yearstr);
                }
                // set this much
                sInputs.ScenarioName = ScenarioName;
                sInputs.When = DateTime.Now;

                // uppercase scenarioname
                ScenarioName = ScenarioName.ToUpper();
                // only need to do base inputs once, so can use the first row found.  This is set true once first row is found
                bool FirstRowFound = false;
                // go thru table row by row looking for scenario name and year

                foreach (DataRow DR in Source.Rows)
                {
                    // get year fiekd
                    yearstr = DR[rdbfSimYear].ToString();
                    // test if null
                    if (yearstr == "")
                        tempyr = 0;
                    else
                        tempyr = Convert.ToInt16(yearstr);
                    // test if this record matches scenario and year
                    if ((DR[rdbfScenarioName].ToString().ToUpper().Trim() == ScenarioName) & (tempyr == year))
                    {
                        // found a row, get the provider code
                        epCodestr = DR[rdbfProviderCode].ToString();
                        // test it
                        if (ProviderClass.valid(epCodestr))
                        {
                            // Get Provider
                            ep = ProviderClass.provider(epCodestr);
                            // Now get the base dataif first row otherwise skip
                            if (!FirstRowFound)
                            {
                                index = 0;
                                foreach (ModelParameterClass MP in PM.BaseInputs())
                                {
                                    if (Source.Columns.Contains(MP.Fieldname))
                                    {
                                        // found ModelPAram in Table
                                        // convert value
                                        tempMPvaluestr = DR[MP.Fieldname].ToString();
                                        tempMPvalue = Convert.ToInt16(tempMPvaluestr);
                                        // save value
                                        sInputs.BaseInput[index] = tempMPvalue;
                                        // save modeparam
                                        sInputs.BaseInputModelParam[index] = MP.ModelParam;
                                    }
                                    index++;
                                }
                                FirstRowFound = true;
                            }
                            index = 0;
                            foreach (ModelParameterClass MP in PM.ProviderInputs())
                            {
                                if (Source.Columns.Contains(MP.Fieldname))
                                {
                                    // found ModelPAram in Table
                                    tempMPvaluestr = DR[MP.Fieldname].ToString();
                                    tempMPvalue = Convert.ToInt16(tempMPvaluestr);
                                    sInputs.ProviderInput.Values[index][ep] = tempMPvalue;
                                    sInputs.ProviderInputModelParam[index] = MP.ModelParam;
                                }
                                index++;
                            }
                        }
                    }
                }
                loadSuccess = true;
            }
            catch (Exception e)
            {
                SetError(e.Message);
            }
            return loadSuccess;
        }

        //--------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets a list of the providers in table. </summary>
        /// <param name="MyConnection"> my connection. </param>
        /// <param name="Tablename">    The tablename. </param>
        ///
        /// <returns> A List of Providers in the table in the following format
        ///     "CODE"+CODELABELSEPERATOR+"LABEL", ie CODE seperated by CODELABELSEPERATOR from LABLE.
        ///     </returns>
        ///-------------------------------------------------------------------------------------------------

        static public List<string> ProvidersInTable(UniDbConnection MyConnection, string Tablename, ref bool iserror, ref string errString)
        {
            char stringquote = Tools.SQLStringQuote(MyConnection.SQLServerType);
            DataTable SelectedTable = new DataTable();
            List<string> TempList = new List<string>();
            // load one row to get scenario name
            SelectedTable = UniDB.Tools.LoadTable(MyConnection, Tablename, 1, ref iserror, ref errString);
            if (!iserror)
            {
                string ScnName = SelectedTable.Rows[0][WaterSimManager_DB.rdbfScenarioName].ToString();
                string firstyear = SelectedTable.Rows[0][WaterSimManager_DB.rdbfSimYear].ToString();
                // ok get the provider codes in the table
                string JustFirstYear = WaterSimManager_DB.rdbfSimYear + " = " + firstyear + " AND " + WaterSimManager_DB.rdbfScenarioName + " = " + stringquote + ScnName + stringquote;
                SelectedTable = UniDB.Tools.LoadTable(MyConnection, Tablename, JustFirstYear, ref iserror, ref errString);
                if (!iserror)
                {
                    // ok get the provider codes in the table
                    TempList = ProvidersInTable(SelectedTable);
                }
            }
            return TempList;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a list of the providers in table. </summary>
        ///
        /// <exception cref="WaterSim_Exception">   Thrown when DataTabke is not a valid WaterSim datatable, ie does not have required fields </exception>
        /// <returns> A List of Providers in the table in the following format "CODE"+CODELABELSEPERATOR+"LABEL", ie CODE seperated by CODELABELSEPERATOR from LABLE. </returns>
        ///-------------------------------------------------------------------------------------------------

        static public List<string> ProvidersInTable(DataTable DT)
        {
            List<string> temp = new List<string>();
            if (Valid_DataTable(DT))
            {
                if (DT.Rows.Count > 0)
                {
                    string code = "";
                    string label = "";
                    // OK get the first year row, do not need to do all years
                    DataRow first = DT.Rows[0];
                    string yearstr = first[rdbfSimYear].ToString();

                    foreach (DataRow DR in DT.Rows)
                    {
                        // only do the first year rows
                        if (DR[rdbfSimYear].ToString() != yearstr)
                        {
                            break;
                        }
                        code = DR[rdbfProviderCode].ToString();
                        label = DR[rdbfProviderLabel].ToString();
                        temp.Add(code + CODELABELSEPERATOR + label);
                    }
                }
            }
            else
            {
                throw new WaterSim_Exception(WS_Strings.wsInvalidDataTable);
            }
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Years in table. </summary>
        /// <exception cref="WaterSim_Exception">   Thrown when DataTabke is not a valid WaterSim datatable, ie does not have required fields </exception>
        ///
        /// <param name="DT"> The dataTable. </param>
        ///
        /// <returns> A string list of years in the datatable. </returns>
        ///-------------------------------------------------------------------------------------------------

        static public List<string> YearsInTable(DataTable DT)
        {
            List<string> temp = new List<string>();
            if (Valid_DataTable(DT))
            {
                string thenewyear = "";
                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow DR in DT.Rows)
                    {
                        thenewyear = DR[rdbfSimYear].ToString().Trim();
                        if (temp.FindIndex(delegate(string value) { return value == thenewyear; }) == -1)
                        {
                            temp.Add(thenewyear);
                        }
                    }
                }
            }
            else
            {
                throw new WaterSim_Exception(WS_Strings.wsInvalidDataTable);
            }
            return temp;
        }

        // --------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Years in table. </summary>
        ///
        /// <remarks> Returns a list of the years in the Table. </remarks>
        ///
        /// <param name="DbConnection"> The database connection. </param>
        /// <param name="Tablename">    The tablename. </param>
        ///
        /// <returns> A string list of years in the datatable. </returns>
        ///-------------------------------------------------------------------------------------------------

        static public List<string> YearsInTable(UniDbConnection DbConnection, string Tablename, ref bool iserror, ref string errString)
        {
            DataTable SelectedTable = new DataTable();
            List<string> TempList = new List<string>();
            // just load One Scenario
            SelectedTable = UniDB.Tools.LoadTable(DbConnection, Tablename, 1, ref iserror, ref errString);
            if (!iserror)
            {
                string ScnName = SelectedTable.Rows[0][WaterSimManager_DB.rdbfScenarioName].ToString();
                // ok get the provider codes in the table
                string JustFirstScenario = WaterSimManager_DB.rdbfScenarioName + " = " + '"' + ScnName + '"';
                SelectedTable = UniDB.Tools.LoadTable(DbConnection, Tablename, JustFirstScenario, ref iserror, ref errString);
                if (!iserror)
                {
                    TempList = YearsInTable(SelectedTable);
                }
            }
            return TempList;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets a list of Scenario names in table. </summary>
        /// <exception cref="WaterSim_Exception">   Thrown when a water simulation error condition occurs. </exception>
        /// <param name="DT"> The dataTable. </param>
        /// <returns> a string list. </returns>
        ///-------------------------------------------------------------------------------------------------

        static public List<string> ScenarioNamesInTable(DataTable DT)
        {
            List<string> temp = new List<string>();
            if (Valid_DataTable(DT))
            {
                string ScnName = "";
                // OK get the first year row, do not need to do all years
                string oldname = "%%%%%%%";
                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow DR in DT.Rows)
                    {
                        ScnName = DR[rdbfScenarioName].ToString();
                        if ((ScnName != oldname) && (ScnName != ""))
                        {
                            if (temp.FindIndex(delegate(string name) { return name == ScnName; }) == -1)
                            {
                                temp.Add(ScnName);
                                oldname = ScnName;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new WaterSim_Exception(WS_Strings.wsInvalidDataTable);
            }
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a list of Scenario names in table. </summary>
        ///
        /// <param name="DbConnect">    The database connect. </param>
        /// <param name="tablename">    name of the table to load. </param>
        ///
        /// <returns>   a string list. </returns>
        ///-------------------------------------------------------------------------------------------------

        static public List<string> ScenarioNamesInTable(UniDbConnection DbConnect, string tablename, ref bool iserr, ref string errMessage)
        {
            List<string> ColumnList = new List<string> { rdbfID, rdbfProviderCode, rdbfProviderLabel, rdbfSimYear, rdbfScenarioName };
            DataTable DT = UniDB.Tools.LoadTable(DbConnect, tablename, ColumnList, ref iserr, ref errMessage);
            if (iserr)
                return null;
            else
                return ScenarioNamesInTable(DT);

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Parameter fields in table. </summary>
        ///
        /// <param name="DT">                       The dataTable. </param>
        /// <param name="PM">                    a ParameterManagerClass object to iteratre through known
        ///     parameters and see if they are in the datatable. </param>
        /// <param name="OnlyWaterSimParamters"> true to fetch only paramters inthe
        ///     WaterSim.ParameterManager list. </param>
        /// <param name="OnlyNonWaterSim">          true to only non WaterSim parameters. </param>
        /// <param name="UseTypeFilter">            true to use ModelParameter type filter. </param>
        /// <param name="mptype">                   The ModelParamter type to filter for. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        static public List<string> ParameterFieldsInTable(DataTable DT, ParameterManagerClass PM, bool OnlyWaterSimParamters, bool OnlyNonWaterSim, bool UseTypeFilter, modelParamtype mptype)
        {
            List<string> temp = new List<string>();
            string fieldname = "";
            string label = "";
            foreach (DataColumn DC in DT.Columns)
            {
                fieldname = DC.ColumnName.ToUpper();
                label = "";
                if ((fieldname == rdbfScenarioName) || (fieldname == rdbfSimYear) || (fieldname == rdbfProviderLabel) || (fieldname == rdbfProviderCode) || (fieldname == rdbfID))
                {
                    // do nothing
                }
                else
                {
                    // not a required field 
                    // ok check to make sure it is an int field
                    if ((DC.DataType == System.Type.GetType("System.Int16")) || (DC.DataType == System.Type.GetType("System.Int32")))
                    {
                        // see if it is a defined paramter
                        try
                        {
                            ModelParameterBaseClass MP = PM.Model_ParameterBaseClass(fieldname);
                            if (!OnlyNonWaterSim)
                            {
                                if ((!UseTypeFilter) || (MP.ParamType == mptype))
                                {
                                    label = MP.Label;
                                    temp.Add(fieldname);
                                }
                            }
                        }
                        catch
                        {
                            if (!OnlyWaterSimParamters)
                            {
                                temp.Add(fieldname);
                            }
                        }
                    }
                    else
                    {
                        if (!OnlyWaterSimParamters)
                        {
                            if (DC.DataType == System.Type.GetType("System.Double"))
                                temp.Add(fieldname);
                        }
                    }

                }
            }
            return temp;
        }

       ///-------------------------------------------------------------------------------------------------
       /// <summary>    Parameter fields in table. </summary>
       ///
       /// <param name="DbConnect">             The database connect. </param>
       /// <param name="tablename">             name of the table to load. </param>
       /// <param name="PM">                    a ParameterManagerClass object to iteratre through known
       ///  parameters and see if they are in the datatable. </param>
       /// <param name="OnlyWaterSimParamters"> true to fetch only paramters inthe
       ///  WaterSim.ParameterManager list. </param>
       /// <param name="OnlyNonWaterSim">       true to only non WaterSim parameters. </param>
       /// <param name="UseTypeFilter">         true to use ModelParameter type filter. </param>
       /// <param name="mptype">                The ModelParamter type to filter for. </param>
       /// <param name="iserror">               Set to true when an error occurs. </param>
       /// <param name="errString">             [in,out] The error string. </param>
       ///
       /// <returns>    . </returns>
       ///-------------------------------------------------------------------------------------------------

       static public List<string> ParameterFieldsInTable(UniDbConnection DbConnect, string tablename, ParameterManagerClass PM, bool OnlyWaterSimParamters, bool OnlyNonWaterSim, bool UseTypeFilter, modelParamtype mptype, ref bool iserror, ref string errString)
        {
            List<string> ParmList = new List<string>();
            DataTable DT;
            try
            {

                DT = UniDB.Tools.LoadTable(DbConnect, tablename, 1, ref iserror, ref errString);

                if (!iserror)
                {
                    ParmList = ParameterFieldsInTable(DT, PM, OnlyWaterSimParamters, OnlyNonWaterSim, UseTypeFilter, mptype);
                }
            }
            catch (Exception e)
            {
                iserror = true;
                errString = e.Message;
            }
            return ParmList;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets a list of Paramters in table. </summary>
        /// <param name="DT">                    The dataTable. </param>
        /// <param name="PM">                       a ParameterManagerClass object to iteratre through known parameters and see if they are in the datatable. </param>
        /// <param name="OnlyWaterSimParamters"> true to fetch only paramters inthe WaterSim.ParameterManager list. </param>
        /// <param name="OnlyNonWaterSim">       true to only non WaterSim parameters. </param>
        /// <param name="UseTypeFilter">         true to use ModelParameter type filter. </param>
        /// <param name="mptype">                The ModelParamter type to filter for. </param>
        ///
        /// <returns> . </returns>
        ///-------------------------------------------------------------------------------------------------

        static public List<string> ParamtersInTable(DataTable DT, ParameterManagerClass PM, bool OnlyWaterSimParamters, bool OnlyNonWaterSim, bool UseTypeFilter, modelParamtype mptype)
        {
            List<string> temp = new List<string>();
            string fieldname = "";
            string label = "";
            foreach (DataColumn DC in DT.Columns)
            {
                fieldname = DC.ColumnName.ToUpper();
                label = "";
                if ((fieldname == rdbfScenarioName) || (fieldname == rdbfSimYear) || (fieldname == rdbfProviderLabel) || (fieldname == rdbfProviderCode) || (fieldname == rdbfID))
                {
                    // do nothing
                }
                else
                {
                    // not a required field 
                    // ok check to make sure it is an int field
                    if ((DC.DataType == System.Type.GetType("System.Int16")) || (DC.DataType == System.Type.GetType("System.Int32")))
                    {
                        // see if it is a defined paramter
                        try
                        {
                            ModelParameterBaseClass MP = PM.Model_ParameterBaseClass(fieldname);
                            if (!OnlyNonWaterSim)
                            {
                                if ((!UseTypeFilter) || (MP.ParamType == mptype))
                                {
                                    label = MP.Label;
                                    temp.Add(fieldname + CODELABELSEPERATOR + label);
                                }
                            }
                        }
                        catch
                        {
                            if (!OnlyWaterSimParamters)
                            {
                                temp.Add(fieldname + CODELABELSEPERATOR);
                            }
                        }
                    }
                    else
                    {
                        if (!OnlyWaterSimParamters)
                        {
                            if (DC.DataType == System.Type.GetType("System.Double"))
                                temp.Add(fieldname + CODELABELSEPERATOR + "*");
                        }
                    }

                }
            }
            return temp;
        }
        #endregion

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets a list of Paramters in table. </summary>
        /// <remarks> Ray, 1/20/2013. </remarks>
        ///
        /// <param name="DbConnect">             The database connect. </param>
        /// <param name="tablename">             name of the table to load. </param>
        /// <param name="PM">                       a ParameterManagerClass object to iteratre through
        ///                                         known parameters and see if they are in the datatable. </param>
        /// <param name="OnlyWaterSimParamters">    true to fetch only paramters inthe
        ///                                         WaterSim.ParameterManager list. </param>
        /// <param name="OnlyNonWaterSim">       true to only non WaterSim parameters. </param>
        /// <param name="UseTypeFilter">         true to use ModelParameter type filter. </param>
        /// <param name="mptype">                The ModelParamter type to filter for. </param>
        ///
        /// <returns> . </returns>
        ///-------------------------------------------------------------------------------------------------

        static public List<string> ParamtersInTable(UniDbConnection DbConnect, string tablename, ParameterManagerClass PM, bool OnlyWaterSimParamters, bool OnlyNonWaterSim, bool UseTypeFilter, modelParamtype mptype, ref bool iserror, ref string errString)
        {
            List<string> ParmList = new List<string>();
            DataTable DT;
            try
            {

                DT = UniDB.Tools.LoadTable(DbConnect, tablename, 1, ref iserror, ref errString);

                if (!iserror)
                {
                    ParmList = ParamtersInTable(DT, PM, OnlyWaterSimParamters, OnlyNonWaterSim, UseTypeFilter, mptype);
                }
            }
            catch (Exception e)
            {
                iserror = true;
                errString = e.Message;
            }
            return ParmList;
        }
        #region SimulationDB
        //-----------------------------------------------------------------------------
        // Variables used in the Simulation_DB routines
        // OleDbConnection _dbConnect;
        DataTable _dbTable;
        string _DataTableName = "";


        internal bool _Simulation_DB_initialized = false;
        internal bool _inDBRun = false;
        internal int _SimulationDB_Years = 0;
        internal string _ScnName = "";
        //-----------------------------------------------------------------------------        
        internal void fetchData(int year)
        {
            fetchData(year,ref FAnnualResults);
        }

        //-----------------------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes the data. </summary>
        ///
        /// <param name="Years">    The years. </param>
        ///-------------------------------------------------------------------------------------------------

        internal void initializeData(int Years)
        {
            initializeData(Years, Sim_StartYear, ref FAnnualResults);

        }


        //-----------------------------------------------------------------------------        
        internal static bool FindStringNoCase(string value, string[] values, ref int index)
        {
            index = -1;
            bool found = false;
            for (int i = 0; i < values.Length; i++)
            {
                found = (values[i].ToUpper() == value.ToUpper());
                if (found)
                {
                    index = i;
                    break;
                }
            }
            return found;
        }
        //-----------------------------------------------------------------------------
        internal void initilaizeFieldStrings()
        {
            // load fieldnames
            db_fieldnames = new SimulationStrings(ParamManager.NumberOfParameters(modelParamtype.mptOutputBase),
                                                    ParamManager.NumberOfParameters(modelParamtype.mptOutputProvider),
                                                    ParamManager.NumberOfParameters(modelParamtype.mptInputBase),
                                                    ParamManager.NumberOfParameters(modelParamtype.mptInputProvider));

            // BaseOutputs
            int index = 0;
            foreach (ModelParameterClass MP in ParamManager.BaseOutputs())
            {
                db_fieldnames.BaseOutput[index] = MP.Fieldname;
                index++;
            }
            // Base Inputs
            index = 0;
            foreach (ModelParameterClass MP in ParamManager.BaseInputs())
            {
                db_fieldnames.BaseInput[index] = MP.Fieldname;
                index++;
            }
            // Provider Outputs
            index = 0;
            foreach (ModelParameterClass MP in ParamManager.ProviderOutputs())
            {
                db_fieldnames.ProviderOutput[index] = MP.Fieldname;
                index++;
            }
            // Provider Inputs
            index = 0;
            foreach (ModelParameterClass MP in ParamManager.ProviderInputs())
            {
                db_fieldnames.ProviderInput[index] = MP.Fieldname;
                index++;
            }
        }

        //----------------------------
        const char DoubleQuote = (char)34;
        const char SingleQuote = (char)39;
        const string Comma = " , ";
        const string CommaDQuote = @" , """;
        const char QMark = '?';
        //-----------------------------------------------------------------------------
        internal string BuildProviderFieldValues(eProvider ep, string BaseInsertString, string ScenarioName, int cyear, SimulationResults TheAnnualResults )
        {
            char StringQuote = ' ';
            switch (FDbConnection.SQLServerType)
            {
                case SQLServer.stAccess:
                case SQLServer.stMySQL:
                    StringQuote = DoubleQuote;
                    break;
                case SQLServer.stPostgreSQL:
                    StringQuote = SingleQuote;
                    break;

            }
            string IDS = BaseInsertString;
            // add ID value
            //IDS += DoubleQuote + FIDCounter.ToString("F1") + DoubleQuote + Comma;
            IDS += FIDCounter.ToString("F1") + Comma;
            FIDCounter++;
            string outstr = "";

            IDS += StringQuote + ScenarioName + StringQuote;

            string yearStr = TheAnnualResults[cyear].year.ToString();

            IDS += Comma + yearStr;

            int pindex = ProviderClass.index(ep, FAddRegion);
            string ProviderCode = ProviderClass.FieldName(ep);
            string ProviderLabel = ProviderClass.Label(ep);
            if (ProviderLabel.Length > 25) ProviderLabel = ProviderLabel.Substring(0, 25);
            IDS += Comma + StringQuote + ProviderCode + StringQuote;
            IDS += Comma + StringQuote + ProviderLabel + StringQuote;

            // BaseOutput
            for (int i = 0; i < db_fieldnames.BaseOutput.Length; i++)
            {
                if (_dbTable.Columns.Contains(db_fieldnames.BaseOutput[i]))
                {
                    outstr = TheAnnualResults[cyear].Outputs.BaseOutput[i].ToString();
                    IDS += Comma + outstr;
                }
            }
            // ProviderOutput
            for (int i = 0; i < db_fieldnames.ProviderOutput.Length; i++)
            {
                if (_dbTable.Columns.Contains(db_fieldnames.ProviderOutput[i]))
                {
                    outstr = TheAnnualResults[cyear].Outputs.ProviderOutput[i].Values[pindex].ToString();
                    IDS += Comma + outstr;
                }
            }
            // BaseInput
            for (int i = 0; i < db_fieldnames.BaseInput.Length; i++)
            {
                if (_dbTable.Columns.Contains(db_fieldnames.BaseInput[i]))
                {
                    outstr = TheAnnualResults[cyear].Inputs.BaseInput[i].ToString();
                    IDS += Comma + outstr;
                }
            }
            // ProviderInput
            for (int i = 0; i < db_fieldnames.ProviderInput.Length; i++)
            {
                if (_dbTable.Columns.Contains(db_fieldnames.ProviderInput[i]))
                {
                    outstr = TheAnnualResults[cyear].Inputs.ProviderInput[i].Values[pindex].ToString();
                    IDS += Comma + outstr;
                }
            }

            IDS += " ); ";

            return IDS;
        }

        //Tools.ColumnInfoList BuildAllFieldsList()
        //{

        //}
        //// ---------------------------------------------------------------------------
        //internal DataTable BuildAllFieldsDataTable(string TableName)
        //{
        //    string errMessage = "";
        //    Tools.ColumnInfoList ColumnList = new Tools.ColumnInfoList();
            
        //    DataTable result = Tools.createTableFromColumnList(FDbConnection, TableName, ColumnList, ref errMessage, FDbConnection.SQLServerType);
        //}
        //// ---------------------------------------------------------------------------
        //=============================================================

        //------------------------
        static Predicate<UniDB.Tools.ColumnInfo> Pred_FindColumnName(string value)
        {
            return delegate(UniDB.Tools.ColumnInfo FACI)
            {
                return FACI.ColumName == value;
            };
        }
        //------------------------

        internal class FastAppendColumnInfoList
        {
            List<UniDB.Tools.ColumnInfo> FColList = new List<UniDB.Tools.ColumnInfo>();

            internal FastAppendColumnInfoList()
            {
            }
            //----------------------------------------------
            internal FastAppendColumnInfoList(DataTable aDataTable)  // aData must be Filled
            {
                int index = 0;
                foreach (DataColumn DC in aDataTable.Columns)
                {
                    FColList.Add(new UniDB.Tools.ColumnInfo(DC));
                    index++;
                }
            }


            //-----------------------------------------------
            internal void Add(string name, System.Type datatype, int width, bool MakePrimary, bool NotNull, bool Unique)
            {
                UniDB.Tools.ColumnInfo CI = new UniDB.Tools.ColumnInfo(name, datatype, width, MakePrimary, NotNull, Unique);

                FColList.Add(CI);
            }
            //-------------------------------------
            internal UniDB.Tools.ColumnInfo FindColumnName(string aColumnName, ref int index)
            {
                UniDB.Tools.ColumnInfo CI = FColList.Find(Pred_FindColumnName(aColumnName));
                if (CI != null) index = FColList.IndexOf(CI); else index = -1;
                return CI;
            }
            internal List<UniDB.Tools.ColumnInfo> ColumnInfoList
            {
                get { return FColList; }

            }


            }
            //===========================================================
            internal class OutPutParam
            {
                int FParam = -1;
                bool FUse = false;
                int FFldIndex = -1;

                internal OutPutParam(int MParam, bool InDb, int FldIndex)
                {
                    FParam = MParam;
                    FUse = InDb;
                    FFldIndex = FldIndex;
                }

                internal bool IsInDb { get { return FUse; } }

                internal int ModelParam { get { return FParam; } }

                internal int FieldIndex { get { return FFldIndex; } }
            }
            //=============================================================
            internal static int FindField(DAO.Fields TheFields, string FieldName)
            {
                int i = 0;
                int index = -1;
                bool Found = false;
                FieldName = FieldName.ToUpper();
                while ((i < TheFields.Count) && (!Found))
                {
                    if (TheFields[i].Name.ToUpper() == FieldName)
                    {
                        index = i;
                        Found = true;
                    }
                    i++;
                }
                return index;
            }

        
            //==================================================================
            internal bool dbFastestAppendAnnualData(string Tablename, string ScenarioName, SimulationResults ASRList)
            {
                WriteMode = "Fastest";
                // set error check
                bool isError = true;

                // set up unique ID
                double IdKey = UniDB.Tools.CreatePrimaryIDSeed();
                try
                {

                    // Now open the file
                    DAO.Recordset ScnRS = DAOdbDatabase.OpenRecordset(Tablename);

                    // OK, to max speed, in advance decide which Model Params to use and get field index
                    OutPutParam[] BaseOutputParms = new OutPutParam[ASRList[0].Outputs.BaseOutput.Length];
                    OutPutParam[] ProviderOutputParms = new OutPutParam[ASRList[0].Outputs.ProviderOutput.Length];
                    OutPutParam[] BaseInputParms = new OutPutParam[ASRList[0].Inputs.BaseInput.Length];
                    OutPutParam[] ProviderInputParms = new OutPutParam[ASRList[0].Inputs.ProviderInput.Length];
                    bool UseThis = false;
                    int Mparm = -1;
                    string fldname = "";
                    int dbFieldIndex = -1;
                    for (int i = 0; i < ASRList[0].Outputs.BaseOutput.Length; i++)
                    {
                        // Get the model Paramter
                        Mparm = ASRList[0].Outputs.BaseOutputModelParam[i];
                        // Get ist field name
                        fldname = ParamManager.Model_ParameterBaseClass(Mparm).Fieldname;
                        // see of it is one of the fields in the table, if so get its index and set UseThis
                        dbFieldIndex = FindField(ScnRS.Fields, fldname);
                        if (dbFieldIndex > -1)
                            UseThis = true;
                        else
                            UseThis = false;
                        // create an OutPutParm for this Parameter
                        BaseOutputParms[i] = new OutPutParam(Mparm, UseThis, dbFieldIndex);
                    }
                    for (int i = 0; i < ASRList[0].Outputs.ProviderOutput.Length; i++)
                    {
                        Mparm = ASRList[0].Outputs.ProviderOutputModelParam[i];
                        fldname = ParamManager.Model_ParameterBaseClass(Mparm).Fieldname;
                        dbFieldIndex = FindField(ScnRS.Fields, fldname);
                        if (dbFieldIndex > -1)
                            UseThis = true;
                        else
                            UseThis = false;
                        ProviderOutputParms[i] = new OutPutParam(Mparm, UseThis, dbFieldIndex);
                    }
                    for (int i = 0; i < ASRList[0].Inputs.BaseInput.Length; i++)
                    {
                        Mparm = ASRList[0].Inputs.BaseInputModelParam[i];
                        fldname = ParamManager.Model_ParameterBaseClass(Mparm).Fieldname;
                        dbFieldIndex = FindField(ScnRS.Fields, fldname);
                        if (dbFieldIndex > -1)
                            UseThis = true;
                        else
                            UseThis = false;
                        BaseInputParms[i] = new OutPutParam(Mparm, UseThis, dbFieldIndex);
                    }

                    for (int i = 0; i < ASRList[0].Inputs.ProviderInput.Length; i++)
                    {
                        Mparm = ASRList[0].Inputs.ProviderInputModelParam[i];
                        fldname = ParamManager.Model_ParameterBaseClass(Mparm).Fieldname;
                        dbFieldIndex = FindField(ScnRS.Fields, fldname);
                        if (dbFieldIndex > -1)
                            UseThis = true;
                        else
                            UseThis = false;
                        ProviderInputParms[i] = new OutPutParam(Mparm, UseThis, dbFieldIndex);
                    }
                    // Also get index to Pre Fields
                    int IDFieldIndex = FindField(ScnRS.Fields, rdbfID);
                    int ScnFieldIndex = FindField(ScnRS.Fields, rdbfScenarioName);
                    int YearFieldIndex = FindField(ScnRS.Fields, rdbfSimYear);
                    int PcodeFieldIndex = FindField(ScnRS.Fields, rdbfProviderCode);
                    int PLabelFieldIndex = FindField(ScnRS.Fields, rdbfProviderLabel);
                    double IDValue = IdKey;
                    // Ok all set, lets blast through these records
                    foreach (AnnualSimulationResults ASR in ASRList)
                    {
                        // set the basics
                        int TheYear = ASR.year;
                        bool InlcudeAggregates = ASR.Outputs.AggregatesIncluded;
                        foreach (eProvider EP in ProviderClass.providers(IncludeAggregates))
                        {
                            // Tell it we are added a new record
                            ScnRS.AddNew();
                            // Set ID
                            ScnRS.Fields[IDFieldIndex].Value = IDValue;
                            IDValue++;

                            ScnRS.Fields[YearFieldIndex].Value = TheYear;
                            ScnRS.Fields[ScnFieldIndex].Value = ScenarioName;

                            // Set Provider Values
                            ScnRS.Fields[PcodeFieldIndex].Value = ProviderClass.FieldName(EP);
                            ScnRS.Fields[PLabelFieldIndex].Value = ProviderClass.Label(EP);

                            for (int i = 5; i < ScnRS.Fields.Count; i++)
                                ScnRS.Fields[i].Value = i;
                            //// Set base values 
                            for (int i = 0; i < ASR.Outputs.BaseOutput.Length; i++)
                            {
                                if (BaseOutputParms[i].IsInDb)
                                    ScnRS.Fields[BaseOutputParms[i].FieldIndex].Value = ASR.Outputs.BaseOutput[i];
                            }
                            for (int i = 0; i < ASR.Inputs.BaseInput.Length; i++)
                            {
                                if (BaseInputParms[i].IsInDb)
                                    ScnRS.Fields[BaseInputParms[i].FieldIndex].Value = ASR.Inputs.BaseInput[i];
                            }

                            //// OK set Provider Input and Output
                            for (int i = 0; i < ASR.Outputs.ProviderOutput.Length; i++)
                            {
                                try
                                {
                                    if (ProviderOutputParms[i].IsInDb)
                                    {
                                        int NewValue = ASR.Outputs.ProviderOutput[i][EP];
                                        //string myfldname = ScnRS.Fields[ProviderOutputParms[i].FieldIndex].Name;
                                        ScnRS.Fields[ProviderOutputParms[i].FieldIndex].Value = ASR.Outputs.ProviderOutput[i][EP];
                                    }
                                }
                                catch (Exception e)
                                {
                                    ModelParameterBaseClass MP = ParamManager.Model_ParameterBaseClass(ASR.Outputs.ProviderOutputModelParam[i]);
                                    string MPFieldName = MP.Fieldname;
                                    string message = e.Message;
                                }
                            }
                            bool DoAggregates = IncludeAggregates;
                            for (int i = 0; i < ASR.Inputs.ProviderInput.Length; i++)
                            {
                                if (ProviderInputParms[i].IsInDb)
                                {
                                    //if (EP > ProviderClass.LastProvider)
                                    //{
                                    //    ScnRS.Fields[ProviderInputParms[i].FieldIndex].Value = SpecialValues.MissingIntValue;
                                    //}
                                    //else
                                    //{
                                    if (ASR.Inputs.ProviderInput[i].Length > ProviderClass.index(EP,DoAggregates))
                                    {

                                        ScnRS.Fields[ProviderInputParms[i].FieldIndex].Value = ASR.Inputs.ProviderInput[i][EP];
                                    }
                                    else
                                    {
                                        ScnRS.Fields[ProviderInputParms[i].FieldIndex].Value = SpecialValues.MissingIntValue;

                                    }
                                    //}
                                }
                            }
                            // OK Commit this to DB
                            ScnRS.Update();
                        }
                    }
                    ScnRS.Close();

                    isError = false;
                }
                catch (Exception e)
                {
                    isError = true;
                    SetError(e.Message);
                }
                return !isError;
            }
            //===================================================================
            internal bool dbFasterAppendAnnualData(string Tablename, string ScenarioName, AnnualSimulationResults[] ASRList)
            {
                // set error check
                bool isError = false;
                string errMessage = "";
                // set up unique ID
                double IdKey = UniDB.Tools.CreatePrimaryIDSeed();
                // Create FastAppendFieldList

                DataTable GetColNamesTable = UniDB.Tools.LoadTable(DbConnection, Tablename, 1, ref isError, ref errMessage);
                if (!isError)
                {
                    // Get Column Info
                    FastAppendColumnInfoList FastColList = new FastAppendColumnInfoList(GetColNamesTable);


                    // Create Command
                    DbCommand dbCommand = DbConnection.CreateCommand();
                    // Set up Prepare String
                    string PrepareString = "";
                    PrepareString = "INSERT INTO " + UniDB.Tools.BracketIt(DbConnection.SQLServerType, Tablename) + " ( ";
                    // SetUp Column Define and Value strings
                    string ColumnDefineList = "";
                    string ColumnValueList = "";
                    foreach (UniDB.Tools.ColumnInfo CI in FastColList.ColumnInfoList)
                    {
                        ColumnDefineList += CI.ColumName + " , ";
                        ColumnValueList += " ? , ";
                    }
                    // Ok get reid of the last commas
                    ColumnDefineList = ColumnDefineList.Substring(0, ColumnDefineList.Length - 3);
                    ColumnValueList = ColumnValueList.Substring(0, ColumnValueList.Length - 3);
                    // Build COmmand Prepare String
                    string CommandPrepareString = PrepareString + ColumnDefineList + " ) VALUES ( " + ColumnValueList + " ); ";
                    dbCommand.CommandText = CommandPrepareString;

                    // Ok now define all the fields
                    int index = 0;
                    foreach (UniDB.Tools.ColumnInfo CI in FastColList.ColumnInfoList)
                    {
                        dbCommand.Parameters.Add(FDbConnection.CreateDbParameter(CI.ColumName, CI.DataType, CI.Width));
                        index++;
                    }
                    try
                    {
                        dbCommand.Prepare();
                        DbTransaction MyTran;
                        foreach (AnnualSimulationResults ASR in ASRList)
                        {
                            MyTran = FDbConnection.BeginTransaction();

                            int ColIndex = 0;
                            // get the simyear index and set
                            UniDB.Tools.ColumnInfo CI = FastColList.FindColumnName(rdbfSimYear, ref ColIndex);
                            dbCommand.Parameters[ColIndex].Value = ASR.year;
                            // get scenario name index and set
                            CI = FastColList.FindColumnName(rdbfScenarioName, ref ColIndex);
                            dbCommand.Parameters[ColIndex].Value = ScenarioName;
                            // Get the ID Index
                            int IDIndex = 0;
                            CI = FastColList.FindColumnName(rdbfID, ref IDIndex);

                            // Get ProviderLabel and Code Index
                            int PrvdCodeIndex = 0;
                            CI = FastColList.FindColumnName(rdbfProviderCode, ref PrvdCodeIndex);
                            int PrvdLabelIndex = 0;
                            CI = FastColList.FindColumnName(rdbfProviderLabel, ref PrvdLabelIndex);


                            // loop through provider codes for this year
                            bool InlcudeAggregates = ASR.Outputs.AggregatesIncluded;
                            foreach (eProvider ep in ProviderClass.providers(IncludeAggregates))
                            {
                                int pindex = ProviderClass.index(ep, FAddRegion);
                                // set ID
                                IdKey++;
                                dbCommand.Parameters[IDIndex].Value = IdKey;
                                // set proider code and label
                                string ProviderCode = ProviderClass.FieldName(ep);
                                string ProviderLabel = ProviderClass.Label(ep);
                                if (ProviderLabel.Length > 25) ProviderLabel = ProviderLabel.Substring(0, 25);

                                dbCommand.Parameters[PrvdCodeIndex].Value = ProviderCode;
                                dbCommand.Parameters[PrvdLabelIndex].Value = ProviderLabel;


                                // BaseOutput
                                for (int i = 0; i < ASR.Outputs.BaseOutputModelParam.Length; i++)
                                {
                                    int PColIndex = 0;
                                    int ModelParm = ASR.Outputs.BaseOutputModelParam[i];
                                    ModelParameterBaseClass MP = ParamManager.Model_ParameterBaseClass(ModelParm);
                                    string FldName = MP.Fieldname;
                                    CI = FastColList.FindColumnName(FldName, ref PColIndex);
                                    if (CI != null)
                                        dbCommand.Parameters[PColIndex].Value = ASR.Outputs.BaseOutput[i];
                                }
                                // ProviderOutput
                                for (int i = 0; i < ASR.Outputs.ProviderOutputModelParam.Length; i++)
                                {
                                    int PColIndex = 0;
                                    int ModelParm = ASR.Outputs.ProviderOutputModelParam[i];
                                    ModelParameterBaseClass MP = ParamManager.Model_ParameterBaseClass(ModelParm);
                                    string FldName = MP.Fieldname;
                                    CI = FastColList.FindColumnName(FldName, ref PColIndex);
                                    if (CI != null)
                                    {
                                        dbCommand.Parameters[PColIndex].Value = ASR.Outputs.ProviderOutput[i].Values[pindex];
                                    }
                                }
                                // BaseInput
                                for (int i = 0; i < ASR.Inputs.BaseInputModelParam.Length; i++)
                                {
                                    int PColIndex = 0;
                                    int ModelParm = ASR.Inputs.BaseInputModelParam[i];
                                    ModelParameterBaseClass MP = ParamManager.Model_ParameterBaseClass(ModelParm);
                                    string FldName = MP.Fieldname;
                                    CI = FastColList.FindColumnName(FldName, ref PColIndex);
                                    if (CI != null)
                                        dbCommand.Parameters[PColIndex].Value = ASR.Inputs.BaseInput[i];
                                }
                                // ProviderInput
                                for (int i = 0; i < ASR.Inputs.ProviderInputModelParam.Length; i++)
                                {
                                    int PColIndex = 0;
                                    int ModelParm = ASR.Inputs.ProviderInputModelParam[i];
                                    ModelParameterBaseClass MP = ParamManager.Model_ParameterBaseClass(ModelParm);
                                    string FldName = MP.Fieldname;
                                    CI = FastColList.FindColumnName(FldName, ref PColIndex);
                                    if (CI != null)
                                    {
                                        dbCommand.Parameters[PColIndex].Value = ASR.Inputs.ProviderInput[i].Values[pindex];
                                    }
                                }
                                dbCommand.ExecuteNonQuery();

                            }
                            MyTran.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        isError = true;
                        SetError(e.Message);
                    }
                }
                return !isError;
            }
            // ==================================================
            //---------------------------------------------------------------------------
            internal bool MySqldbFastAppendAnnualData(string Tablename, string ScenarioName)
            {
                WriteMode = "Fast";
                bool appenddone = false;
                int fldcnt = 0;
                string Placeholder = " ? ";
                // create base insert string
                string BaseInsertString;
                BaseInsertString = @"insert into " + UniDB.Tools.BracketIt(FDbConnection.SQLServerType, Tablename) + " ( " + RequiredFields[0] + Comma + RequiredFields[1]; //RequiredFields[0];
                Placeholder += Comma;
                fldcnt = 1;
                for (int i = 2; i < NumberOfRequiredFields; i++)
                {
                    BaseInsertString += Comma + @RequiredFields[i];
                    fldcnt++;
                }
                for (int i = 0; i < db_fieldnames.BaseOutput.Length; i++)
                {
                    if (_dbTable.Columns.Contains(db_fieldnames.BaseOutput[i]))
                    {
                        BaseInsertString += Comma + db_fieldnames.BaseOutput[i];
                    }
                }
                // ProviderOutput
                for (int i = 0; i < db_fieldnames.ProviderOutput.Length; i++)
                {
                    if (_dbTable.Columns.Contains(db_fieldnames.ProviderOutput[i]))
                    {
                        BaseInsertString += Comma + db_fieldnames.ProviderOutput[i];
                    }
                }
                // BaseInput
                for (int i = 0; i < db_fieldnames.BaseInput.Length; i++)
                {
                    if (_dbTable.Columns.Contains(db_fieldnames.BaseInput[i]))
                    {
                        BaseInsertString += Comma + db_fieldnames.BaseInput[i];
                    }
                }
                // ProviderInput
                for (int i = 0; i < db_fieldnames.ProviderInput.Length; i++)
                {
                    if (_dbTable.Columns.Contains(db_fieldnames.ProviderInput[i]))
                    {
                        BaseInsertString += Comma + db_fieldnames.ProviderInput[i];
                    }
                }
                BaseInsertString += " ) values ( ";

                try
                {
                    DbCommand cmd = DbConnection.CreateCommand();
                    for (int yeari = 0; yeari < _SimulationDB_Years; yeari++)
                    {
                        String IDS = "";
                        foreach (eProvider ep in ProviderClass.providers())
                        {
                            IDS += BuildProviderFieldValues(ep, BaseInsertString, ScenarioName, yeari, FAnnualResults);

                            //OleDbCommand cmd = new OleDbCommand(@IDS,  DbConnection);

                        } // provider
                        cmd.CommandText = IDS;
                        cmd.ExecuteNonQuery();

                    } // year
                    return appenddone = true;
                } // try
                catch (Exception e)
                {
                    SetError("DataTable append failed Code " + e.Message);
                    return appenddone;
                }
            }



            //---------------------------------------------------------------------------
            internal bool dbFastAppendAnnualData(string Tablename, string ScenarioName, SimulationResults TheAnnualResults)
            {
                WriteMode = "Fast";
                bool appenddone = false;
                int fldcnt = 0;
                string Placeholder = " ? ";
                // create base insert string
                string BaseInsertString;
                BaseInsertString = "insert into " + UniDB.Tools.BracketIt(FDbConnection.SQLServerType, Tablename) + " ( " + RequiredFields[0] + Comma + RequiredFields[1]; //RequiredFields[0];
                Placeholder += Comma;
                fldcnt = 1;
                for (int i = 2; i < NumberOfRequiredFields; i++)
                {
                    BaseInsertString += Comma + @RequiredFields[i];
                    fldcnt++;
                }
                for (int i = 0; i < db_fieldnames.BaseOutput.Length; i++)
                {
                    if (_dbTable.Columns.Contains(db_fieldnames.BaseOutput[i]))
                    {
                        BaseInsertString += Comma + db_fieldnames.BaseOutput[i];
                    }
                }
                // ProviderOutput
                for (int i = 0; i < db_fieldnames.ProviderOutput.Length; i++)
                {
                    if (_dbTable.Columns.Contains(db_fieldnames.ProviderOutput[i]))
                    {
                        BaseInsertString += Comma + db_fieldnames.ProviderOutput[i];
                    }
                }
                // BaseInput
                for (int i = 0; i < db_fieldnames.BaseInput.Length; i++)
                {
                    if (_dbTable.Columns.Contains(db_fieldnames.BaseInput[i]))
                    {
                        BaseInsertString += Comma + db_fieldnames.BaseInput[i];
                    }
                }
                // ProviderInput
                for (int i = 0; i < db_fieldnames.ProviderInput.Length; i++)
                {
                    if (_dbTable.Columns.Contains(db_fieldnames.ProviderInput[i]))
                    {
                        BaseInsertString += Comma + db_fieldnames.ProviderInput[i];
                    }
                }
                BaseInsertString += " ) values ( ";

                try
                {
                    DbCommand cmd = DbConnection.CreateCommand();
                    for (int yeari = 0; yeari < _SimulationDB_Years; yeari++)
                    {
                        foreach (eProvider ep in ProviderClass.providers())
                        {
                            string IDS = BuildProviderFieldValues(ep, BaseInsertString, ScenarioName, yeari, TheAnnualResults);

                            //OleDbCommand cmd = new OleDbCommand(@IDS,  DbConnection);
                            cmd.CommandText = IDS;
                            cmd.ExecuteNonQuery();

                        } // provider

                    } // year
                    return appenddone = true;
                } // try
                catch (Exception e)
                {
                    SetError("DataTable append failed Code " + e.Message);
                    return appenddone;
                }
            }
        internal bool dbTextAdddAnnualData(string Tablename, string ScenarioName, SimulationResults TheAnnualResults)
        {
            bool appenddone = false;

            string Filename = FDbConnection.DatabaseName + "//" + Tablename;
            
            string[] RequiredFieldValues = new string[RequiredFields.Length];

            // Do base Fields, which should all be the same for each record
            // only doing the first two for now, these wont change, doing others later
            // 
            for (int i=0; i<RequiredFields.Length; i++)
            {
               switch(RequiredFields[i].ToUpper())
                {
                    case "ID":
                        double IdKey = UniDB.Tools.CreatePrimaryIDSeed();
                        string temp = IdKey.ToString();
                        RequiredFieldValues[i] = temp;
                        break;
                    case "SCN_NAME":
                        RequiredFieldValues[i] = ScenarioName;
                        break;
                }
            }
            
            try
            {

               
                for (int yeari = 0; yeari < _SimulationDB_Years; yeari++)
                {
                    // OK, Set the Year Field
                    bool found = false;
                    for (int i = 0; i < RequiredFields.Length; i++)
                    {
                        switch (RequiredFields[i].ToUpper())
                        {
                            case "SIMYEAR":
                                // ok get the year value from Annual Results
                                RequiredFieldValues[i] = TheAnnualResults[yeari].year.ToString();
                                found = true;
                                // found it
                                break;
                        }
                        // if found it, exit
                        if (found) { break; }
                    }

                    foreach (eProvider ep in ProviderClass.providers())
                    {

                        // OK, Set the Provider Fields
                        //  Use this to exit
                        int prvcnt = 0;
                        for (int i = 0; i < RequiredFields.Length; i++)
                        {
                            switch (RequiredFields[i].ToUpper())
                            {
                                case "PRVDCODE":
                                    RequiredFieldValues[i] = ProviderClass.FieldName(ep);
                                    prvcnt++;
                                    break;
                                case "PRVDLABEL":
                                    RequiredFieldValues[i] = ProviderClass.Label(ep);
                                    prvcnt++;
                                    break;
                            }
                            // ok if found both, then exit
                            if (prvcnt == 2) { break; }
                        }

                        DataRow NewBdRow = _dbTable.NewRow();

                        // OK, first set all the required fields
                        for (int i = 0; i < RequiredFields.Length; i++)
                        {
                            NewBdRow[RequiredFields[i]] = RequiredFieldValues[i];
                        }

                        // ok, now loop hrough all the data, fnd the fieldnames in the data table and grab those values
                        string outstr = "";
                        int pindex = ProviderClass.index(ep, FAddRegion);

                        // BaseOutput
                        for (int i = 0; i < db_fieldnames.BaseOutput.Length; i++)
                        {
                            string fldname = db_fieldnames.BaseOutput[i];
                            if (_dbTable.Columns.Contains(fldname))
                            {
                                outstr = TheAnnualResults[yeari].Outputs.BaseOutput[i].ToString();
                                NewBdRow[fldname] = outstr;
                            }
                        }
                        // ProviderOutput
                        for (int i = 0; i < db_fieldnames.ProviderOutput.Length; i++)
                        {
                            string fldname = db_fieldnames.ProviderOutput[i];
                            if (_dbTable.Columns.Contains(fldname))
                            {
                                outstr = TheAnnualResults[yeari].Outputs.ProviderOutput[i].Values[pindex].ToString();
                                NewBdRow[fldname] = outstr;
                            }
                        }
                        // BaseInput
                        for (int i = 0; i < db_fieldnames.BaseInput.Length; i++)
                        {
                            string fldname = db_fieldnames.BaseInput[i];
                            if (_dbTable.Columns.Contains(fldname))
                            {
                                outstr = TheAnnualResults[yeari].Inputs.BaseInput[i].ToString();
                                NewBdRow[fldname] = outstr;
                            }
                        }
                        // ProviderInput
                        for (int i = 0; i < db_fieldnames.ProviderInput.Length; i++)
                        {
                            string fldname = db_fieldnames.ProviderInput[i];
                            if (_dbTable.Columns.Contains(fldname))
                            {
                                outstr = TheAnnualResults[yeari].Inputs.ProviderInput[i].Values[pindex].ToString();
                                NewBdRow[fldname] = outstr;
                            }
                        }

                        // should be all done 
                        _dbTable.Rows.Add(NewBdRow);

                    } // provider

                } // year

                // OK append this file
                if (UniDB.Tools.WriteTable(_dbTable, Tablename, true, true, false, UniDB.Tools.DataFormat.CommaDelimited, out FErrorMessage))
                {
                    appenddone = true;
                }
                else

                {
                    appenddone = false;
                }
            } // try
            catch (Exception e)
            {
                SetError("DataTable append failed Code " + e.Message);
                appenddone = false;
            }
            return appenddone;
        }


        //-----------------------------------------------------------------------------

        internal bool dbAppendAnnualData()
            {
                bool appenddone = false;
                string ProviderCode = "";
                string ProviderLabel = "";
                string yearStr = "";
                int pindex = 0;

                DataRow DR;
                try
                {
                    for (int yeari = 0; yeari < _SimulationDB_Years; yeari++)
                    {
                        foreach (eProvider ep in ProviderClass.providers())
                        {
                            pindex = ProviderClass.index(ep);
                            ProviderCode = ProviderClass.FieldName(ep);
                            ProviderLabel = ProviderClass.Label(ep);
                            if (ProviderLabel.Length > 25) ProviderLabel = ProviderLabel.Substring(0, 25);
                            yearStr = FAnnualResults[yeari].year.ToString();

                            DR = _dbTable.NewRow();
                            //        protected string[] RequiredFields = new string[NumberOfRequiredFields] { "ID", "SCN_NAME", "SIMYEAR", "PRVDLABEL", "PRVDCODE" };

                            DR["SCN_NAME"] = _ScnName;
                            DR["SIMYEAR"] = yearStr;
                            DR["PRVDLABEL"] = ProviderLabel;
                            DR["PRVDCODE"] = ProviderCode;

                            // BaseOutput
                            string wsfield = "";

                            for (int i = 0; i < db_fieldnames.BaseOutput.Length; i++)
                            {
                                wsfield = db_fieldnames.BaseOutput[i];
                                if (_dbTable.Columns.Contains(db_fieldnames.BaseOutput[i]))
                                {
                                    DR[db_fieldnames.BaseOutput[i]] = FAnnualResults[yeari].Outputs.BaseOutput[i].ToString();
                                    //DR[db_fieldnames.BaseOutput[i]] = DbRun_AnnualResults[yeari].BaseOutput[i].ToString();
                                }
                            }
                            // ProviderOutput
                            for (int i = 0; i < db_fieldnames.ProviderOutput.Length; i++)
                            {
                                wsfield = db_fieldnames.ProviderOutput[i];
                                if (_dbTable.Columns.Contains(db_fieldnames.ProviderOutput[i]))
                                {
                                    DR[db_fieldnames.ProviderOutput[i]] = FAnnualResults[yeari].Outputs.ProviderOutput[i].Values[pindex].ToString();
                                    //                                DR[db_fieldnames.ProviderOutput[i]] = DbRun_AnnualResults[yeari].ProviderOutput.Values[i].Values[pindex].ToString();
                                }
                            }
                            // BaseInput
                            for (int i = 0; i < db_fieldnames.BaseInput.Length; i++)
                            {
                                wsfield = db_fieldnames.BaseInput[i];
                                if (_dbTable.Columns.Contains(db_fieldnames.BaseInput[i]))
                                {
                                    DR[db_fieldnames.BaseInput[i]] = FAnnualResults[yeari].Inputs.BaseInput[i].ToString();
                                    // DR[db_fieldnames.BaseInput[i]] = DbRun_AnnualResults[yeari].BaseInput[i].ToString();
                                }
                            }
                            // ProviderInput
                            for (int i = 0; i < db_fieldnames.ProviderInput.Length; i++)
                            {
                                wsfield = db_fieldnames.ProviderInput[i];
                                if (_dbTable.Columns.Contains(db_fieldnames.ProviderInput[i]))
                                {
                                    DR[db_fieldnames.ProviderInput[i]] = FAnnualResults[yeari].Inputs.ProviderInput[i].Values[pindex].ToString();
                                    //DR[db_fieldnames.ProviderInput[i]] = DbRun_AnnualResults[yeari].ProviderInput.Values[i].Values[pindex].ToString();
                                }
                            }

                            _dbTable.Rows.Add(DR);

                        }

                    }
                    return appenddone = true;
                }
                catch (Exception e)
                {
                    SetError("DataTable append failed Code " + e.Message);
                    return appenddone;
                }
            }
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Initializes a DB Simulation.  Opens the datatable using DbConnection and datatablename,  results will be written to temp DataTable and then updated in the Database.
        /// </summary>
        /// <param name="aDataTablename">a tablename to use in the database for output</param>
        /// <param name="Scenario_Name">the name of the scenario to label the output</param>
        /// <remarks>The scenario name is important becuase it can be used to retrieve the Simulation inputs from the datatable later.</remarks>
        /// <returns> true if datatable was opended and simulation initialized, false otherwise, check isError and GetErrorMessage()</returns>
        /// <exception cref="WaterSimManager_DB_Exception">if data base is not open</exception>
        /// <seealso cref="WaterSimDCDC.WaterSimManager_DB.LoadScenario_DB(string, int, System.Data.DataTable, ref WaterSimDCDC.SimulationInputs)"/>
        /// <seealso cref=" WaterSimDCDC.WaterSimManager_DB.LoadScenario_DB(string, int, System.Data.DataTable, ref WaterSimDCDC.SimulationInputs, WaterSimDCDC.ParameterManagerClass)"/>
        /// <seealso cref="isError"/>
        /// <seealso cref="GetErrorMessage"/>
        public bool Simulation_Initialize(string aDataTablename, string Scenario_Name)
        {
            bool iserror = true;
            _Simulation_DB_initialized = false;

            string ErrMessage = "";
            bool dbdone = false;
            if (!FDbOpen) throw new WaterSimManager_DB_Exception("Database must be open to initialize simulation");
            // ok load the table
            // Ok fill the data table with all teh relvant fields
            _DataTableName = aDataTablename;
            // does the table exist?
            try
            {
                _dbTable = UniDB.Tools.LoadTable(FDbConnection, _DataTableName, ref iserror, ref ErrMessage);
                if (!Valid_DataTable(_dbTable))
                {
                    SetError("WaterSimManager DB Error: Invalid Data Table Structure, Missing required fields");
                    dbdone = false;
                }
                else
                {
                    // Somehow this got deleted
                    _dbTable.Clear();
                    //-------------------------------------------------
                    _ScnName = Scenario_Name;
                    // initialize the field strings
                    initilaizeFieldStrings();

                    // initialize the Simulation
                    Simulation_Initialize();
                    _Simulation_DB_initialized = true;
                    dbdone = true;
                }
            }
            catch (Exception ex)
            {
                SetError("WaterSimManager DB Error: "+ex.Message);
                dbdone = false;
            }
            return dbdone;
            }
            //-----------------------------------------------------------------------------
            /// <summary>
            /// Initializes a DB Simulation. results will be appended to the specified DataTable
            /// </summary>
            /// <param name="aDataTable">a DataTable used for output</param>
            /// <param name="Scenario_Name">the name of the scenario to label the output</param>
            /// <remarks>The datatable must exist in the dbConnection database.  The scenario name is important becuase it can be used to retrieve the Simulation inputs from the datatable later.</remarks>
            /// <returns> true if datatable was opended and simulation initialized, false otherwise, check isError and GetErrorMessage()</returns>
            /// <seealso cref="WaterSimDCDC.WaterSimManager_DB.LoadScenario_DB(string, int, System.Data.DataTable, ref WaterSimDCDC.SimulationInputs)"/>
            /// <seealso cref=" WaterSimDCDC.WaterSimManager_DB.LoadScenario_DB(string, int, System.Data.DataTable, ref WaterSimDCDC.SimulationInputs, WaterSimDCDC.ParameterManagerClass)"/>
            /// <seealso cref="isError"/>
            /// <seealso cref="GetErrorMessage"/>

            public bool Simulation_Initialize(DataTable aDataTable, string Scenario_Name)
            {
                // Verify Table
                if (!Valid_DataTable(aDataTable))
                {
                    SetError("WaterSimManager DB Error: Invalid Data Table Structure, Missing required fields");
                    return false;
                }
                else
                {
                    _dbTable = aDataTable;
                    _ScnName = Scenario_Name;
                    // initialize the field strings
                    initilaizeFieldStrings();

                    // initialize the Simulation
                    Simulation_Initialize();
                    _Simulation_DB_initialized = true;
                    _inDBRun = false;

                    return true;
                }
            }
//-----------------------------------------------------------------------------
/*Message);
            if (isError)
            {

            }
            else
            {
                string match = _DataTableName.ToUpper().Trim();
string dbTableNames.Find(delegate (string value) { if (value.ToUpper() == match) return true; else return false; })
                if (dbTableNames.Find(delegate (string value) { if (value.ToUpper() == match) return true; else return false; }) != match)
                {
                    CreateNewDataTable(_DataTableName);
                }
                else
                {

_dbTable = UniDB.Tools.LoadTable(FDbConnection, _DataTableName, ref iserror, ref ErrMessage);
                }
            }
        
            if (iserror)
                {
                    SetError("Data Table Open Failed: " + ErrMessage);
                }
                else
                {

                    dbdone = Simulation_Initialize(_dbTable, Scenario_Name);
                }
            */


bool _inDBSimulationNextYear = false;

            /// <summary>
            /// Runs the next (or first) year of the Simulation.  If initialized with one of the DB Simulation_Initilaize() overloads, will append data to the datatable, otehrwise just runs the model one year.
            /// </summary>
            /// <returns>the year of Simulation</returns>
            public override int Simulation_NextYear()
            {
                int year = 0;
                if (!_inDBSimulationNextYear)
                {
                    _inDBSimulationNextYear = true;
                    if (_Simulation_DB_initialized)
                    {
                        // run s=imulation
                        if (!_inDBRun)
                        {
                            // Set years for Simulation
                            _SimulationDB_Years = (Simulation_End_Year - Simulation_Start_Year) + 1;
                            //_SimulationDB_Start_Year = Simulation_Start_Year;
                            // initialize data structure
                            initializeData(_SimulationDB_Years);
                            // Ok exist and start running years
                            _inDBRun = true;
                        }
                        year = base.Simulation_NextYear();
                        //Fetch Data
                        fetchData(year);
                    }
                    // Did not use DB version of initialize so just run normal
                    else
                        year = base.Simulation_NextYear();
                }
                _inDBSimulationNextYear = false;
                return year;
            }
            //-----------------------------------------------------------------------------

            bool _inSimulationAllYears = false;
            /// <summary>
            /// Runs all the years of the Simulation based on Simulation_STart_Year and Simulation_End_year, If initialized with one of the DB Simulation_Initilaize() overloads, will append data to the datatable, otehrwise just runs the model all years.
            /// </summary>
            public override void Simulation_AllYears()
            {
                if (!_inSimulationAllYears)
                {
                    _inSimulationAllYears = true;
                    foreach (int year in simulationYears())
                    {
                        Simulation_NextYear();  // calls DBSimulation which test if in a DB Simulation otherwise just calls root
                    }
                    _inSimulationAllYears = false;

                }
            }
            //-----------------------------------------------------------------------------

            public bool AppendAnnualData(string DataTableName, string ScnName, SimulationResults AnnualResults)
            {
                switch (FSQLServer)
                {
                    case SQLServer.stAccess:
                        {
                            if (dbFastAppendAnnualData(DataTableName, ScnName, AnnualResults)) 
                            {
                            }
                            else
                            {
                                if (isError)
                                {
                                    throw new WaterSimManager_DB_Exception(" Whille updating database " + GetErrorMessage());
                                }
                            }
                        }
                        break;
                    case SQLServer.stMySQL:
                        {
                            if (dbFasterAppendAnnualData(DataTableName, ScnName, AnnualResults.GetAllYears())) // DbRun_AnnualResults.GetAllYears()))
                            {
                            }
                            else
                            {
                                if (isError)
                                {
                                    throw new WaterSimManager_DB_Exception(" Whille updating database " + GetErrorMessage());
                                }
                            }
                        }
                        break;
                    case SQLServer.stPostgreSQL:
                        {

                            if (dbFastAppendAnnualData(DataTableName, ScnName, AnnualResults)) //, DbRun_AnnualResults))
                            {
                            }
                            else
                            {
                                if (isError)
                                {
                                    throw new WaterSimManager_DB_Exception(" Whille updating database " + GetErrorMessage());
                                }
                            }

                        }
                        break;
                case SQLServer.stText:
                    {

                        if (dbTextAdddAnnualData(DataTableName, ScnName, AnnualResults)) //, DbRun_AnnualResults))
                        {
                        }
                        else
                        {
                            if (isError)
                            {
                                throw new WaterSimManager_DB_Exception(" Whille updating database " + GetErrorMessage());
                            }
                        }

                    }
                    break;

            }
            return true;
            }
            /// <summary>
            /// Stops the Simulation.  If initialized with one of the DB Simulation_Initilaize() overloads, will update the datatable in the database.
            /// </summary>
            public override void Simulation_Stop()
            {
                // stop simulation
                base.Simulation_Stop();
                // if DB initiualized close it down
                if (_Simulation_DB_initialized)
                {
                    _inDBRun = false;
                    _Simulation_DB_initialized = false;

                    // Now append this data to datatable
                    // 
                    try
                    {
                        AppendAnnualData(_DataTableName, _ScnName, FAnnualResults);
                    }
                    catch (Exception e)
                    {
                        FisError = true;
                        FErrorMessage = e.Message;
                    }
                    //  if (dbAppendAnnualData(_DataTableName, _ScnName))// Version 1 very slow method
                    //  if (dbFastAppendAnnualData(_DataTableName, _ScnName))// Version 2 slow method
                    //switch (FSQLServer)
                    //{
                    //    case SQLServer.stAccess:
                    //        {
                    //            if (dbFastestAppendAnnualData(_DataTableName, _ScnName, FAnnualResults ))//DbRun_AnnualResults))// Very fast only works with Microsoft ACE driver 
                    //            {
                    //            }
                    //            else
                    //            {
                    //                if (isError)
                    //                {
                    //                    throw new WaterSimManager_DB_Exception(" Whille updating database " + GetErrorMessage());
                    //                }
                    //            }
                    //        }
                    //        break;
                    //    case SQLServer.stMySQL:
                    //        {
                    //            if (dbFasterAppendAnnualData(_DataTableName, _ScnName,FAnnualResults.GetAllYears())) // DbRun_AnnualResults.GetAllYears()))
                    //            {
                    //            }
                    //            else
                    //            {
                    //                if (isError)
                    //                {
                    //                    throw new WaterSimManager_DB_Exception(" Whille updating database " + GetErrorMessage());
                    //                }
                    //            }
                    //        }
                    //        break;
                    //    case SQLServer.stPostgreSQL:
                    //        {
                                
                    //            if (dbFastAppendAnnualData(_DataTableName, _ScnName)) //, DbRun_AnnualResults))
                    //            {
                    //            }
                    //            else
                    //            {
                    //                if (isError)
                    //                {
                    //                    throw new WaterSimManager_DB_Exception(" Whille updating database " + GetErrorMessage());
                    //                }
                    //            }

                    //        }
                    //        break;

                    //}
                    //if (FSQLServer != SQLServer.stAccess) // Version 3 faster and works with multiple servers
                    //{
                    //    if (dbFasterAppendAnnualData(_DataTableName, _ScnName, DbRun_AnnualResults))//  dbAppendAnnualData())
                    //    {
                    //    }
                    //    else
                    //    {
                    //        if (isError)
                    //        {
                    //            throw new WaterSimManager_DB_Exception(" Whille updating database " + GetErrorMessage());
                    //        }
                    //    }
                    //}
                    //else
                    //    if (dbFastestAppendAnnualData(_DataTableName, _ScnName, DbRun_AnnualResults))// Very fast only works with Microsoft ACE driver 
                    //    {
                    //        // check if should uopdate database
                    //        //if (_DataTableName != "")
                    //        //{
                    //        //    OleDbCommandBuilder TableUpdateBuilder = new OleDbCommandBuilder(RawTableAdapter);
                    //        //    RawTableAdapter.Update(_dbTable);
                    //        //}
                    //    }
                    //    else
                    //    {
                    //        if (isError)
                    //        {
                    //            throw new WaterSimManager_DB_Exception(" Whille updating database " + GetErrorMessage());
                    //        }
                    //    }
                }
            }



        }// End of WaterSimManager_DB

        //-------------------------------------------------------------
        internal class WaterSimManager_DB_Exception : WaterSim_Exception
        {
            new const string Pre = "WaterSimManager DB Error: ";
            internal WaterSimManager_DB_Exception(string message) : base(message) { }
            internal WaterSimManager_DB_Exception(int index) : base(index) { }
        }
        #endregion
    
}
