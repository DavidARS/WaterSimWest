/**************************************************************************
 *   UniDB Unversal Data Provider Class
 * 
 *   Version 4 Copyright Ray Quay 2018
 *   
 *   Unified SQL Support 
 *   
 *   Class UniDbConnection
 *   Class UniDataAdapter
 *   
 *   The goal of these classes is to provide a single data provider class
 *   that supports multiple SQL Servers.  
 *   
 *   SQL Servers Supported
 *   MySQL, PostGreSQL, Microsoft Access, text csv
 *         
 *   The following files/DLLs will be required by any application using this
 *      PostGreSQL
 *         Npgsql.dll                     Add to References 
 *         Npgsql.resources.dll
 *         Npgsgl.XML
 *         policy.2.0.Npgsql.dll
 *         policy.2.0.Npgsql.config
 *         Mono.Security.dll         
 *      MySql
 *         MySql.data.dll                Add to References
 *      Ms Access
 *        Systenm.Data.OleDb
 * 
 *       This program is free software: you can redistribute it and/or modify
 *       it under the terms of the GNU General Public License version 3 as published by
 *       the Free Software Foundation.
 *
 *       This program is distributed in the hope that it will be useful,
 *       but WITHOUT ANY WARRANTY; without even the implied warranty of
 *       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *       GNU General Public License for more details.
 *
 *       You should have received a copy of the GNU General Public License
 *       along with this program.  If not, please see <http://www.gnu.org/licenses/>.
 *
 *   DB Support
 *   
 *   Class  Tools
 *   
 *      This class provides a number of static methods used to support db and file ativities
 *      
 *   Version:
 *   5/6/15 Added support for TxtDb classes
 *   2/6/18  More tools added for opening and copying tables
 *   8/10/18 Adding mode support for text files with in SqlServer.stText
 *   8/10/18 added a Tools.writeTable method that supports appending to a file. 
 *   
 *   
 * *************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using MySql.Data.MySqlClient;
using Npgsql;
using txtdb;
using UniDB;


namespace UniDB
{

    public static class UniDbVersion
    {
        public static string Version()
        {
            return "Version 5.0 4 20 2020";
        }
    }
    
    /// <summary>   Values that represent SQLServer. </summary>
    public enum SQLServer
    {

        /// <summary> Microsoft JET Access </summary>
        stAccess,
        /// <summary> MySQL. </summary>
        stMySQL,
        /// <summary> PostGreSQL. </summary>
        stPostgreSQL,
        /// <summary>   Text Mode Comma Delimited with Fields as first line  </summary>
        stText
    
    };

    
    /// <summary>   Uni database connection. </summary>
    /// <remarks> This class provides a single DbConnection class that can be used with three SQLServers
    ///           MySQL, PostGreSQL, and Microsoft Access.  This allows code to be written that works with all three
    ///           SQLSwervers simply by change the SQLServer type used to open the COnnection, and providing the
    ///           Server LOcation (none for access), User ID, and Password to the constructor.</remarks>
    [System.ComponentModel.DesignerCategory("")]
    public class UniDbConnection : DbConnection
    {

        private SQLServer FSQLServer;
        private System.Data.Common.DbConnection FDbConnection = null;
        private OleDbConnection FOleDbConnection;
        private NpgsqlConnection FNpgsqlConnection;
        private MySqlConnection FMysqlConnection;
        private TxtDbConnection FTxtDbConnection;
        

        /// <summary> The post gre SQL port.</summary>
        public static string PostGreSQLPort = "5432";

        string FDatabaseName = "";
        string FServerAddress = "";
        string FUserName = "";
        string FPassword = "";


        // this is just for the txtConnection
        // CLEAN UP bool FUseFileHeader = false;
        
        // Progress Report
        Tools.ProgressReport FPrgReport = null;
        int FProgressReportIncrement = 100;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor that prevents a default instance of this class from being
        ///             created. </summary>
        ///-------------------------------------------------------------------------------------------------

        private UniDbConnection()
        {

        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="SQLServerType">    Type of the SQL server. </param>
        /// <param name="DatabaseName">     Name of the database. </param>
        /// <param name="UserName">         Name of the user. </param>
        /// <param name="PassWord">         The pass word. </param>
        /// <param name="OtherConnectStringOptions">         If other connection string parameters are desired, this string is attached to the end of the connection string </param>
        ///-------------------------------------------------------------------------------------------------

        public UniDbConnection(SQLServer SQLServerType, string ServerAddress, string DatabaseName, string UserName, string PassWord, string OtherConnectStringOptions)
            : base()
        {
            // Get fields
            FSQLServer = SQLServerType;
            FDatabaseName = DatabaseName;
            FServerAddress = ServerAddress; ;
            FUserName = UserName;
            FPassword = PassWord;
            // create connection
            switch (FSQLServer)
            {
                case SQLServer.stAccess:
                    {
                        FOleDbConnection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0; data source=" + DatabaseName + ";User Id=" + UserName + ";Password=" + PassWord + ";" + OtherConnectStringOptions);
                        FDbConnection = FOleDbConnection;
                        break;
                    }
                case SQLServer.stPostgreSQL:
                    {
                        FNpgsqlConnection = new NpgsqlConnection("Server=" + ServerAddress + ";Port=" + PostGreSQLPort + ";User Id=" + UserName + ";Password=" + PassWord + ";Database=" + DatabaseName + ";" + OtherConnectStringOptions);
                        FDbConnection = FNpgsqlConnection;
                        break;
                    }
                case SQLServer.stMySQL:
                    {
                        FMysqlConnection = new MySqlConnection("Server=" + ServerAddress + ";Database=" + DatabaseName + ";UId=" + UserName + ";Password=" + PassWord + ";" + OtherConnectStringOptions);
                        FDbConnection = FMysqlConnection;
                        break;
                    }
                case SQLServer.stText:
                    {
                        FTxtDbConnection = new TxtDbConnection("Database=" + DatabaseName);
                        FDbConnection = FTxtDbConnection;
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }
        //-----------------------------------------------------------------------------


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <exception cref="NotImplementedException">  Thrown when the requested operation is
        ///                                             unimplemented. </exception>
        ///
        /// <param name="SQLServerType">    Type of the SQL server. </param>
        /// <param name="ConnectionString"> The connection string used to establish the initial
        ///                                 connection. The exact contents of the connection string
        ///                                 depend on the specific data source for this connection. The
        ///                                 default value is an empty string. </param>
        ///-------------------------------------------------------------------------------------------------

        public UniDbConnection(SQLServer SQLServerType, string ConnectionString)
        {
            // Get fields if you can
            FSQLServer = SQLServerType;
            FPassword = Extract_Password(ConnectionString);
            FUserName = Extract_UserName(ConnectionString);
            FDatabaseName = Extract_DatabaseName(ConnectionString);
            FServerAddress = Extract_ServerAddress(ConnectionString);
                    // ProgressReporting
 
            // create connection
            switch (FSQLServer)
            {
                case SQLServer.stAccess:
                    {
                        FDbConnection = new OleDbConnection(ConnectionString);
                        break;
                    }
                case SQLServer.stPostgreSQL:
                    {
                        FDbConnection = new NpgsqlConnection(ConnectionString);
                        break;
                    }
                case SQLServer.stMySQL:
                    {
                        FDbConnection = new MySqlConnection(ConnectionString);
                        break;
                    }
                case SQLServer.stText:
                    {
                        FTxtDbConnection = new TxtDbConnection(ConnectionString);
                        FDbConnection = FTxtDbConnection;
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the progress report. </summary>
        ///
        /// <value> The progress report. </value>
        ///-------------------------------------------------------------------------------------------------

        public Tools.ProgressReport ProgressReport
        {
            get { return FPrgReport; }
            set
            {
                // create connection
                switch (FSQLServer)
                {
                    case SQLServer.stAccess:
                        {
                            break;
                        }
                    case SQLServer.stPostgreSQL:
                        {
                            break;
                        }
                    case SQLServer.stMySQL:
                        {
                            break;
                        }
                    case SQLServer.stText:
                        {
                            FPrgReport = value;
                            (FDbConnection as TxtDbConnection).ProgressReport = value;
                            break;
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the progress report increment. </summary>
        ///
        /// <value> The progress report increment. </value>
        ///-------------------------------------------------------------------------------------------------

        public int ProgressReportIncrement
        {
            get { return FProgressReportIncrement; }
            set
            {
                // create connection
                switch (FSQLServer)
                {
                    case SQLServer.stAccess:
                        {
                            break;
                        }
                    case SQLServer.stPostgreSQL:
                        {
                            break;
                        }
                    case SQLServer.stMySQL:
                        {
                            break;
                        }
                    case SQLServer.stText:
                        {
                            FProgressReportIncrement = value;
                            (FDbConnection as TxtDbConnection).ProgressReportIncrement = value;
                            break;
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
            }
        }


        //-----------------------------------------------------------------------------
        internal string Extract_Password(string ConnectionString)
        {
            string temp = "";
            string CS = ConnectionString.ToUpper();
            switch (FSQLServer)
            {
                    // Same for all
                case SQLServer.stAccess: 
                case SQLServer.stPostgreSQL:
                case SQLServer.stMySQL:
                    {
                        int index = CS.IndexOf("PASSWORD");
                        if (index > 0)
                        {
                            CS = ConnectionString.Substring(index + 8);

                            index = CS.IndexOf("=");
                            if (index > -1)
                            {
                                int indexSemi = CS.IndexOf(";");
                                if (indexSemi>index)
                                {
                                    temp = CS.Substring(index+1,((indexSemi - index )-1)); 
                                 }
                            }
                        }
                        break;
                    }
                case SQLServer.stText:
                    temp = "";
                    break;
             default:
                    {
                        throw new NotImplementedException();
                    }
            }
            return temp;
        }
        //-----------------------------------------------------------------------------
        internal string Extract_UserName(string ConnectionString)
        {
            string temp = "";
            string CS = ConnectionString.ToUpper();
            switch (FSQLServer)
            {
                // Same for these
                case SQLServer.stAccess:
                case SQLServer.stPostgreSQL:
                    {
                        int index = CS.IndexOf("USER ID");
                        if (index > 0)
                        {
                            CS = ConnectionString.Substring(index + 7);

                            index = CS.IndexOf("=");
                            if (index > -1)
                            {
                                int indexSemi = CS.IndexOf(";");
                                if (indexSemi > index)
                                {
                                    temp = CS.Substring(index + 1, ((indexSemi - index) - 1));
                                }
                            }
                        }
                        break;
                    }
                case SQLServer.stMySQL:
                    {
                        int index = CS.IndexOf("UID");
                        if (index > 0)
                        {
                            CS = ConnectionString.Substring(index + 3);

                            index = CS.IndexOf("=");
                            if (index > -1)
                            {
                                int indexSemi = CS.IndexOf(";");
                                if (indexSemi > index)
                                {
                                    temp = CS.Substring(index + 1, ((indexSemi - index) - 1));
                                }
                            }
                        }
                        break;
                    }
                case SQLServer.stText:
                    {
                        temp = "";
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            } return temp;
        }
        //-----------------------------------------------------------------------------
        internal string Extract_DatabaseName(string ConnectionString)
        {
            string temp = "";
            string CS = ConnectionString.ToUpper();
            switch (FSQLServer)
            {
                // Same for all
                case SQLServer.stAccess:
                    {
                        int index = CS.IndexOf("DATA SOURCE");
                        if (index > 0)
                        {
                            CS = ConnectionString.Substring(index + 11);

                            index = CS.IndexOf("=");
                            if (index > -1)
                            {
                                int indexSemi = CS.IndexOf(";");
                                if (indexSemi > index)
                                {
                                    temp = CS.Substring(index + 1, ((indexSemi - index) - 1));
                                }
                            }
                        }
                        break;
                    }
                case SQLServer.stPostgreSQL:
                case SQLServer.stMySQL:
                case SQLServer.stText:
                    {
                        int index = CS.IndexOf("DATABASE");
                        if (index > 0)
                        {
                            CS = ConnectionString.Substring(index + 8);

                            index = CS.IndexOf("=");
                            if (index > -1)
                            {
                                int indexSemi = CS.IndexOf(";");
                                if (indexSemi > index)
                                {
                                    temp = CS.Substring(index + 1, ((indexSemi - index) - 1));
                                }
                            }
                        }
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
            return temp;
        }
        //-----------------------------------------------------------------------------
        internal string Extract_ServerAddress(string ConnectionString)
        {
            string temp = "";
            string CS = ConnectionString.ToUpper();
            switch (FSQLServer)
            {
                // Same for all
                case SQLServer.stAccess:
                    {
                        int index = CS.IndexOf("PROVIDER");
                        if (index > 0)
                        {
                            CS = ConnectionString.Substring(index + 8);

                            index = CS.IndexOf("=");
                            if (index > -1)
                            {
                                int indexSemi = CS.IndexOf(";");
                                if (indexSemi > index)
                                {
                                    temp = CS.Substring(index + 1, ((indexSemi - index) - 1));
                                }
                            }
                        }
                        break;
                    }
                case SQLServer.stPostgreSQL:
                case SQLServer.stMySQL:
                    {
                        int index = CS.IndexOf("SERVER");
                        if (index > 0)
                        {
                            CS = ConnectionString.Substring(index + 6);

                            index = CS.IndexOf("=");
                            if (index > -1)
                            {
                                int indexSemi = CS.IndexOf(";");
                                if (indexSemi > index)
                                {
                                    temp = CS.Substring(index + 1, ((indexSemi - index) - 1));
                                }
                            }
                        }
                        break;
                    }
                case SQLServer.stText:
                    {
                        temp = "";
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
            return temp;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the type of the SQL server. </summary>
        ///
        /// <value> The type of the SQL server. </value>
        ///-------------------------------------------------------------------------------------------------

        public SQLServer SQLServerType
        { get { return FSQLServer; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the database connection. </summary>
        ///
        /// <value> The database connection. </value>
        ///-------------------------------------------------------------------------------------------------

        public System.Data.Common.DbConnection DbConnection
        { get { return FDbConnection; } }


        public string DatabaseName
        {
            get
            {
                string temp = "";
                if (FDbConnection != null)
                {
                    temp = FDbConnection.Database;
                }
                else
                {
                    temp = FDatabaseName;
                }
                return temp;
            }
        }

        public string ServerName
        {
            get 
            {
                string temp = "";
                if (FDbConnection != null)
                {
                    temp = FDbConnection.ServerVersion;
                }
                else
                {
                    temp = FServerAddress;
                }
                return temp;
            }
        }

        public string UserName
        {
            get { return FUserName; }
        }

        public string Password
        {
            get {return FPassword; }
        }

        public string ConnectionLabel
        {
            get
            {
                string temp = "";
                if (DatabaseName != "")
                {
                    temp = DatabaseName;
                }
                else
                {
                    if (ServerName != "")
                    {
                        temp = ServerName;
                    }
                }
                return temp;
            }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Opens a database connection with the settings specified by the
        ///             <see cref="P:System.Data.Common.DbConnection.ConnectionString" />. </summary>
        ///-------------------------------------------------------------------------------------------------

        public override void Open()
        {
            switch (FSQLServer)
            {
                case SQLServer.stAccess:
                    {
                        (FDbConnection as OleDbConnection).Open();
                        break;
                    }
                case SQLServer.stMySQL:
                    {
                        (FDbConnection as MySqlConnection).Open();
                        break;
                    }
                case SQLServer.stPostgreSQL:
                    {
                        (FDbConnection as NpgsqlConnection).Open();

                        break;
                    }
                case SQLServer.stText:
                    {
                        (FDbConnection as TxtDbConnection).Open();
                        break;
                    }
            }
        }
        //----------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Closes the connection to the database. This is the preferred method of closing
        ///             any open connection. </summary>
        ///
        /// ### <exception cref="T:System.Data.Common.DbException"> The connection-level error that
        ///                                                         occurred while opening the
        ///                                                         connection. </exception>
        ///-------------------------------------------------------------------------------------------------

        public override void Close()
        {
            switch (FSQLServer)
            {
                case SQLServer.stAccess:
                    {
                        (FDbConnection as OleDbConnection).Close();
                        break;
                    }
                case SQLServer.stMySQL:
                    {
                        (FDbConnection as MySqlConnection).Close();
                        break;
                    }
                case SQLServer.stPostgreSQL:
                    {
                        (FDbConnection as NpgsqlConnection).Close();
                        break;
                    }
                case SQLServer.stText:
                    {
                        (FDbConnection as TxtDbConnection).Close();
                        break;
                    }
            }
        }
        //----------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Starts a database transaction. </summary>
        ///
        /// <exception cref="NotImplementedException">  Thrown when the requested operation is
        ///                                             unimplemented. </exception>
        ///
        /// <param name="isolationLevel">   Specifies the isolation level for the transaction. </param>
        ///
        /// <returns>   An object representing the new transaction. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            switch (FSQLServer)
            {
                case SQLServer.stAccess:
                    {
                        return (FDbConnection as OleDbConnection).BeginTransaction(isolationLevel);
                    }
                case SQLServer.stPostgreSQL:
                    {
                        return (FDbConnection as NpgsqlConnection).BeginTransaction(isolationLevel);
                    }
                case SQLServer.stMySQL:
                    {
                        return (FDbConnection as MySqlConnection).BeginTransaction(isolationLevel);
                    }
                case SQLServer.stText:
                    {
                        return (FDbConnection as TxtDbConnection).BeginTransaction(isolationLevel);
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }
        //----------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Starts a database transaction. </summary>
        /// <exception cref="NotImplementedException">  Thrown when the requested operation is
        ///                                             unimplemented. </exception>
        ///
        /// <returns>   An object representing the new transaction. </returns>
        ///-------------------------------------------------------------------------------------------------

        public new DbTransaction BeginTransaction()
        //public DbTransaction BeginTransaction()
        {
            switch (FSQLServer)
            {
                case SQLServer.stAccess:
                    {
                        return (FDbConnection as OleDbConnection).BeginTransaction();
                    }
                case SQLServer.stPostgreSQL:
                    {
                        return (FDbConnection as NpgsqlConnection).BeginTransaction();
                    }
                case SQLServer.stMySQL:
                    {
                        return (FDbConnection as MySqlConnection).BeginTransaction();
                    }
                case SQLServer.stText:
                    {
                        return (FDbConnection as TxtDbConnection).BeginTransaction();
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }
        //----------------------------------------------
        public DbParameter CreateDbParameter(string aName, Type aDataType, int aSize)
        {
            switch (FSQLServer)
            {
                case SQLServer.stAccess:
                    {
                        OleDbParameter dbParm = new OleDbParameter(aName, Tools.ConvertNetTypeToOledbType(aDataType), aSize);
                        return dbParm;
                    }
                case SQLServer.stPostgreSQL:
                    {
                        NpgsqlParameter dbParm = new NpgsqlParameter(aName, Tools.ConvertNetTypeToPostGreSQLType(aDataType), aSize);
                        return dbParm;
                    }
                case SQLServer.stMySQL:
                    {
                        MySqlParameter dbParm = new MySqlParameter(aName, Tools.ConvertNetTypeToMySQLType(aDataType), aSize);
                        return dbParm;
                    }
                case SQLServer.stText:
                    {
                        TxtDbParameter dbParm = new TxtDbParameter(aName, Tools.ConvertNetTypeToDbType(aDataType), aSize);
                        return dbParm;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a database parameter. </summary>
        ///
        /// <exception cref="NotImplementedException"> Thrown when the requested operation is
        ///     unimplemented. </exception>
        ///
        /// <param name="ParmName">     Name of the parm. </param>
        /// <param name="aDataType">    Type of the data. </param>
        /// <param name="aSize">        The size. </param>
        /// <param name="SrcName">      Name of the source. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public DbParameter CreateDbParameter(string ParmName, Type aDataType, int aSize, string SrcName)
        {
            switch (FSQLServer)
            {
                case SQLServer.stAccess:
                    {
                        OleDbParameter dbParm = new OleDbParameter(ParmName, Tools.ConvertNetTypeToOledbType(aDataType), aSize, SrcName);
                        return dbParm;
                    }
                case SQLServer.stPostgreSQL:
                    {
                        NpgsqlParameter dbParm = new NpgsqlParameter(ParmName, Tools.ConvertNetTypeToPostGreSQLType(aDataType), aSize, SrcName);
                        return dbParm;
                    }
                case SQLServer.stMySQL:
                    {
                        MySqlParameter dbParm = new MySqlParameter(ParmName, Tools.ConvertNetTypeToMySQLType(aDataType), aSize, SrcName);
                        return dbParm;
                    }
                case SQLServer.stText:
                    {
                        TxtDbParameter dbParm = new TxtDbParameter(ParmName, Tools.ConvertNetTypeToDbType(aDataType), aSize, SrcName);
                        return dbParm;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }
        //----------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Changes the current database for an open connection. </summary>
        ///
        /// <exception cref="NotImplementedException">  Thrown when the requested operation is
        ///                                             unimplemented. </exception>
        ///
        /// <param name="databaseName"> Specifies the name of the database for the connection to use. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void ChangeDatabase(string databaseName)
        {
            switch (FSQLServer)
            {
                case SQLServer.stAccess:
                    {
                        (FDbConnection as OleDbConnection).ChangeDatabase(databaseName);
                        break;
                    }
                case SQLServer.stPostgreSQL:
                    {
                        (FDbConnection as NpgsqlConnection).ChangeDatabase(databaseName);
                        break;
                    }
                case SQLServer.stMySQL:
                    {
                        (FDbConnection as MySqlConnection).ChangeDatabase(databaseName);
                        break;
                    }
                case SQLServer.stText:
                    {
                        (FDbConnection as TxtDbConnection).ChangeDatabase(databaseName);
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }
        //---------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates and returns a <see cref="T:System.Data.Common.DbCommand" /> object
        ///             associated with the current connection. </summary>
        ///
        /// <exception cref="NotImplementedException">  Thrown when the requested operation is
        ///                                             unimplemented. </exception>
        ///
        /// <returns>   A <see cref="T:System.Data.Common.DbCommand" /> object. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected override DbCommand CreateDbCommand()
        {

            throw new NotImplementedException();
        }
        //---------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a ConnectionState value that describes the state of the connection. </summary>
        ///
        /// <value> The state of the connection.  </value>
        ///-------------------------------------------------------------------------------------------------

        public override ConnectionState State
        {
            get
            {
                switch (FSQLServer)
                {
                    case SQLServer.stAccess:
                        {
                            return (FDbConnection as OleDbConnection).State;
                        }
                    case SQLServer.stMySQL:
                        {
                            return (FDbConnection as MySqlConnection).State;
                        }
                    case SQLServer.stPostgreSQL:
                        {
                            return (FDbConnection as NpgsqlConnection).State;
                        }
                    case SQLServer.stText:
                        {
                            return (FDbConnection as TxtDbConnection).State;
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
            }
        }
        //----------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a string that represents the version of the server to which the object is
        ///             connected. </summary>
        ///
        /// <value> The version of the database. The format of the string returned depends on the
        ///         specific type of connection you are using. </value>
        ///-------------------------------------------------------------------------------------------------

        public override string ServerVersion
        {
            get
            {
                switch (FSQLServer)
                {
                    case SQLServer.stAccess:
                        {
                            return (FDbConnection as OleDbConnection).ServerVersion;
                        }
                    case SQLServer.stPostgreSQL:
                        {
                            return (FDbConnection as NpgsqlConnection).ServerVersion;
                        }
                    case SQLServer.stMySQL:
                        {
                            return (FDbConnection as MySqlConnection).ServerVersion;
                        }
                    case SQLServer.stText:
                        {
                            return (FDbConnection as TxtDbConnection).ServerVersion;
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
            }
        }
        //----------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the name of the database server to which to connect. </summary>
        ///
        /// <value> The name of the database server to which to connect. The default value is an empty
        ///         string. </value>
        ///-------------------------------------------------------------------------------------------------

        public override string DataSource
        {
            get
            {
                switch (FSQLServer)
                {
                    case SQLServer.stAccess:
                        {
                            return (FDbConnection as OleDbConnection).DataSource;
                        }
                    case SQLServer.stPostgreSQL:
                        {
                            return (FDbConnection as NpgsqlConnection).DataSource;
                        }
                    case SQLServer.stMySQL:
                        {
                            return (FDbConnection as MySqlConnection).DataSource;
                        }
                    case SQLServer.stText:
                        {
                            return (FDbConnection as TxtDbConnection).DataSource;
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
            }
        }
        //----------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the name of the current database after a connection is opened, or the
        ///             database name specified in the connection string before the connection is opened. </summary>
        ///
        /// <value> The name of the current database or the name of the database to be used after a
        ///         connection is opened. The default value is an empty string. </value>
        ///-------------------------------------------------------------------------------------------------

        public override string Database
        {
            get
            {
                switch (FSQLServer)
                {
                    case SQLServer.stAccess:
                        {
                            return (FDbConnection as OleDbConnection).Database;
                        }
                    case SQLServer.stPostgreSQL:
                        {
                            return (FDbConnection as NpgsqlConnection).Database;
                        }
                    case SQLServer.stMySQL:
                        {
                            return (FDbConnection as MySqlConnection).Database;
                        }
                    case SQLServer.stText:
                        {
                            return (FDbConnection as TxtDbConnection).Database;
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
            }
        }
        //----------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the string used to open the connection. </summary>
        ///
        /// <value> The connection string used to establish the initial connection. The exact contents of
        ///         the connection string depend on the specific data source for this connection. The
        ///         default value is an empty string. </value>
        ///-------------------------------------------------------------------------------------------------

        public override string ConnectionString
        {
            get
            {
                switch (FSQLServer)
                {
                    case SQLServer.stAccess:
                        {
                            return (FDbConnection as OleDbConnection).ConnectionString;
                        }
                    case SQLServer.stPostgreSQL:
                        {
                            return (FDbConnection as NpgsqlConnection).ConnectionString;
                        }
                    case SQLServer.stMySQL:
                        {
                            return (FDbConnection as MySqlConnection).ConnectionString;
                        }
                    case SQLServer.stText:
                        {
                            return (FDbConnection as TxtDbConnection).ConnectionString;
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
            }
            set
            {
                switch (FSQLServer)
                {
                    case SQLServer.stAccess:
                        {
                            (FDbConnection as OleDbConnection).ConnectionString = value;
                            break;
                        }
                    case SQLServer.stPostgreSQL:
                        {
                            (FDbConnection as NpgsqlConnection).ConnectionString = value;
                            break;
                        }
                    case SQLServer.stMySQL:
                        {
                            (FDbConnection as MySqlConnection).ConnectionString = value;
                            break;
                        }
                    case SQLServer.stText:
                        {
                            (FDbConnection as TxtDbConnection).ConnectionString = value;
                            break;
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
            }
        }
        // --------------------------------------. 

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns schema information for the data source of this
        ///             <see cref="T:System.Data.Common.DbConnection" />. </summary>
        ///
        /// <exception cref="NotImplementedException">  Thrown when the requested operation is
        ///                                             unimplemented. </exception>
        ///
        /// <returns>   A <see cref="T:System.Data.DataTable" /> that contains schema information. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override DataTable GetSchema()
        {
            switch (FSQLServer)
            {
                case SQLServer.stAccess:
                    {
                        return (FDbConnection as OleDbConnection).GetSchema();
                    }
                case SQLServer.stPostgreSQL:
                    {
                        return (FDbConnection as NpgsqlConnection).GetSchema();
                    }
                case SQLServer.stMySQL:
                    {
                        return (FDbConnection as MySqlConnection).GetSchema();
                    }
                case SQLServer.stText:
                    {
                        return (FDbConnection as TxtDbConnection).GetSchema();
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }
        // --------------------------------------. 


        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns schema information for the data source of this
        ///     <see cref="T:System.Data.Common.DbConnection" /> using the specified string for the
        ///     schema name.
        /// </summary>
        /// <exception cref="NotImplementedException">  Thrown when the requested operation is
        ///                                             unimplemented. </exception>
        ///
        /// <param name="CollectionName">   Name of the collection. </param>
        ///
        /// <returns>   A <see cref="T:System.Data.DataTable" /> that contains schema information. </returns>
        ///
        /// ### <param name="collectionName">   Specifies the name of the schema to return. </param>
        ///
        /// ### <exception cref="T:System.ArgumentException">   <paramref name="collectionName" /> is
        ///                                                     specified as null. </exception>
        ///-------------------------------------------------------------------------------------------------

        public override DataTable GetSchema(string CollectionName)
        {
            switch (FSQLServer)
            {
                case SQLServer.stAccess:
                    {
                        return (FDbConnection as OleDbConnection).GetSchema(CollectionName);
                    }
                case SQLServer.stPostgreSQL:
                    {
                        return (FDbConnection as NpgsqlConnection).GetSchema(CollectionName);
                    }
                case SQLServer.stMySQL:
                    {
                        return (FDbConnection as MySqlConnection).GetSchema(CollectionName);
                    }
                case SQLServer.stText:
                    {
                        return (FDbConnection as TxtDbConnection).GetSchema(CollectionName);
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }
        //--------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns schema information for the data source of this
        ///     <see cref="T:System.Data.Common.DbConnection" /> using the specified string for the
        ///     schema name and the specified string array for the restriction values.
        /// </summary>
        /// <exception cref="NotImplementedException">  Thrown when the requested operation is
        ///                                             unimplemented. </exception>
        ///
        /// <param name="CollectionName">   Name of the collection. </param>
        /// <param name="Restrictions">     The restrictions. </param>
        ///
        /// <returns>   A <see cref="T:System.Data.DataTable" /> that contains schema information. </returns>
        ///
        /// ### <param name="collectionName">   Specifies the name of the schema to return. </param>
        ///
        /// ### <param name="restrictionValues">    Specifies a set of restriction values for the
        ///                                         requested schema. </param>
        /// ### <exception cref="T:System.ArgumentException">   <paramref name="collectionName" /> is
        ///                                                     specified as null. </exception>
        ///-------------------------------------------------------------------------------------------------

        public override DataTable GetSchema(string CollectionName, string[] Restrictions)
        {
            switch (FSQLServer)
            {
                case SQLServer.stAccess:
                    {
                        return (FDbConnection as OleDbConnection).GetSchema(CollectionName, Restrictions);
                    }
                case SQLServer.stPostgreSQL:
                    {
                        return (FDbConnection as NpgsqlConnection).GetSchema(CollectionName, Restrictions);
                    }
                case SQLServer.stMySQL:
                    {
                        return (FDbConnection as MySqlConnection).GetSchema(CollectionName, Restrictions);
                    }
                case SQLServer.stText:
                    {
                        return (FDbConnection as TxtDbConnection).GetSchema(CollectionName, Restrictions);
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }
        //--------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates and returns a <see cref="T:System.Data.Common.DbCommand" /> object associated
        ///     with the current connection.
        /// </summary>
        /// <exception cref="NotImplementedException">  Thrown when the requested operation is
        ///                                             unimplemented. </exception>
        ///
        /// <returns>   A <see cref="T:System.Data.Common.DbCommand" /> object. </returns>
        ///-------------------------------------------------------------------------------------------------
        public new DbCommand CreateCommand()

 //       public DbCommand CreateCommand()
        {
            switch (FSQLServer)
            {
                case SQLServer.stAccess:
                    {
                        return (FDbConnection as OleDbConnection).CreateCommand();
                    }
                case SQLServer.stPostgreSQL:
                    {
                        return (FDbConnection as NpgsqlConnection).CreateCommand();
                    }
                case SQLServer.stMySQL:
                    {
                        return (FDbConnection as MySqlConnection).CreateCommand();
                    }
                case SQLServer.stText:
                    {
                        return (FDbConnection as TxtDbConnection).CreateCommand();
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }


        //--------------------------------------
        //       //==========================================================
        /// <summary>
        /// Gets a list of tablenames from the Database connected to Connection
        /// </summary>
        /// <param name="Connection"> </param>
        /// <param name="error">If there is an exception then this returns true, returns message in errString</param>
        /// <param name="errString">If there is an exception this returns Exception.Message</param>
        /// <returns>a List of strings of tablenames that are TABLE_TYPE TABLE, ie no system tables </returns>
        /// <remarks> if Connection is not null, but not open, this routine opens them</remarks>
        /// <remarks>   Mcquay, 6/21/2012. </remarks>

        public List<string> GetTableNames(ref Boolean error, ref string errString)
        {
            List<string> _tablenames = new List<string>();
            error = true;
            errString = "";
            bool orgOpen = (FDbConnection.State == System.Data.ConnectionState.Open);
            try
            {
                string temp = "";
                if (!orgOpen) FDbConnection.Open();
                _tablenames.Clear();
                DataTable dbSchema = new DataTable();
                dbSchema = FDbConnection.GetSchema("Tables");
                string TableID = "";
                string TableType = "";
                string TableNameStr = "";
                switch (FSQLServer)
                {
                    case SQLServer.stText:
                        TableID = "TABLE";
                        TableType = "TABLE_TYPE";
                        TableNameStr = "TABLE_NAME";
                        break;
                    case SQLServer.stAccess:
                        TableID = "TABLE";
                        TableType = "TABLE_TYPE";
                        TableNameStr = "TABLE_NAME";
                        break;
                    case SQLServer.stMySQL:
                        TableID = "BASE TABLE";
                        TableType = "TABLE_TYPE";
                        TableNameStr = "TABLE_NAME";
                        break;
                    case SQLServer.stPostgreSQL:
                        TableID = "public";
                        TableType = "table_schema";
                        TableNameStr = "table_name";
                        break;
                }
                for (int i = 0; i < dbSchema.Rows.Count; i++)
                {
                    if (dbSchema.Rows[i][TableType].ToString() == TableID)
                    {
                        temp = dbSchema.Rows[i][TableNameStr].ToString();
                        _tablenames.Add(temp);
                    }
                }
                error = false;
            }
            catch (Exception e)
            {
                errString = e.Message;
            }
            return _tablenames;
        }

       public Tools.DataFormat FileFormat
        {
            get
            {
                if (FSQLServer != SQLServer.stText)
                {
                    return Tools.DataFormat.Unknown;
                }
                else
                {
                    return (FDbConnection as TxtDbConnection).FileDataFormat;
                }
            }
            set
            {
                if (FSQLServer == SQLServer.stText)
                {
                    (FDbConnection as TxtDbConnection).FileDataFormat = value;
                }

            }
        }
       public bool UseFieldHeaders
       {
            get
            {
                if (FSQLServer != SQLServer.stText)
                {
                    return false;
                }
                else
                {
                    return (FDbConnection as TxtDbConnection).UseFieldHeader;
                }
            }
            set
            {
                if (FSQLServer == SQLServer.stText)
                {
                    (FDbConnection as TxtDbConnection).UseFieldHeader = value ;
                }
            }
        }
    
    }

    /// <summary>   Uni data adapter. </summary>
    /// <remarks> Each SQL Server provider has a different class derived from DataAdapter.  This class provides
    ///           a single DataAQdapter class that can be used with multiple SQL server provider assemblies</remarks>
    public class UniDataAdapter : System.Data.Common.DbDataAdapter
    {
        SQLServer FSQLServer;
        System.Data.Common.DataAdapter FDataAdapter;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <exception cref="NotImplementedException">  Thrown when the requested operation is
        ///                                             unimplemented. </exception>
        ///
        /// <param name="Command">  The command. </param>
        /// <param name="dbCon">    The database con. </param>
        ///-------------------------------------------------------------------------------------------------

        public UniDataAdapter(string Command, UniDbConnection dbCon)
        {
            FSQLServer = dbCon.SQLServerType;
            
            switch (FSQLServer)
            {

                case SQLServer.stAccess:
                    {
                        FDataAdapter = new OleDbDataAdapter(Command, (dbCon.DbConnection as OleDbConnection));
                        break;
                    }
                case SQLServer.stPostgreSQL:
                    {
                        FDataAdapter = new NpgsqlDataAdapter(Command, (dbCon.DbConnection as NpgsqlConnection));
                        break;
                    }
                case SQLServer.stMySQL:
                    {
                        FDataAdapter = new MySqlDataAdapter(Command, (dbCon.DbConnection as MySqlConnection));
                        break;
                    }
                case SQLServer.stText:
                    {
                        FDataAdapter = new TxtDbDataAdapter(Command, (dbCon.DbConnection as TxtDbConnection));
                        break;
                    }

                default:
                    {
                        throw new NotImplementedException();
                    }
            }
            
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a command for deleting records from the data set. </summary>
        ///
        /// <value> An <see cref="T:System.Data.IDbCommand" /> used during
        ///         <see cref="M:System.Data.IDataAdapter.Update(System.Data.DataSet)" /> to delete
        ///         records in the data source for deleted rows in the data set. </value>
        ///
        /// <seealso cref="System.Data.Common.DbDataAdapter.DeleteCommand"/>
        ///-------------------------------------------------------------------------------------------------

        public new DbCommand DeleteCommand
        //public DbCommand DeleteCommand
        {
            get
            {
                switch (FSQLServer)
                {

                    case SQLServer.stAccess:
                        {
                            return (FDataAdapter as OleDbDataAdapter).DeleteCommand;
                        }
                    case SQLServer.stPostgreSQL:
                        {
                            return   (FDataAdapter as NpgsqlDataAdapter).DeleteCommand;
                        }
                    case SQLServer.stMySQL:
                        {
                            return (FDataAdapter as MySqlDataAdapter).DeleteCommand;
                        }
                    case SQLServer.stText:
                        {
                            return (FDataAdapter as TxtDbDataAdapter).DeleteCommand;
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
            }
            set
            {
                switch (FSQLServer)
                {
                    case SQLServer.stAccess:
                        {
                            (FDataAdapter as OleDbDataAdapter).DeleteCommand = (value as OleDbCommand);
                            break;
                        }
                    case SQLServer.stPostgreSQL:
                        {
                            (FDataAdapter as NpgsqlDataAdapter).DeleteCommand = (value as NpgsqlCommand);
                            break;
                        }
                    case SQLServer.stMySQL:
                        {
                            (FDataAdapter as MySqlDataAdapter).DeleteCommand = (value as MySqlCommand);
                            break;
                        }
                    case SQLServer.stText:
                        {
                            (FDataAdapter as TxtDbDataAdapter).DeleteCommand = (value as TxtDbCommand);
                            break;
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
            }
        }

        public OleDbDataAdapter asOleDbAdapter
        {
            get { return (FDataAdapter as OleDbDataAdapter); }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a command used to insert new records into the data source. </summary>
        ///
        /// <value> A <see cref="T:System.Data.IDbCommand" /> used during
        ///         <see cref="M:System.Data.IDataAdapter.Update(System.Data.DataSet)" /> to insert
        ///         records in the data source for new rows in the data set. </value>
        ///
        /// <seealso cref="System.Data.Common.DbDataAdapter.InsertCommand"/>
        ///-------------------------------------------------------------------------------------------------

        public new DbCommand InsertCommand
        //public DbCommand InsertCommand
        {
            get
            {
                switch (FSQLServer)
                {

                    case SQLServer.stAccess:
                        {
                            return (FDataAdapter as OleDbDataAdapter).InsertCommand;
                        }
                    case SQLServer.stPostgreSQL:
                        {
                            return (FDataAdapter as NpgsqlDataAdapter).InsertCommand;
                        }
                    case SQLServer.stMySQL:
                        {
                            return (FDataAdapter as MySqlDataAdapter).InsertCommand;
                        }
                    case SQLServer.stText:
                        {
                            return (FDataAdapter as TxtDbDataAdapter).InsertCommand;
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
            }
            set
            {
                switch (FSQLServer)
                {
                    case SQLServer.stAccess:
                        {
                            (FDataAdapter as OleDbDataAdapter).InsertCommand = (value as OleDbCommand);
                            break;
                        }
                    case SQLServer.stPostgreSQL:
                        {
                            (FDataAdapter as NpgsqlDataAdapter).InsertCommand = (value as NpgsqlCommand);
                            break;
                        }
                    case SQLServer.stMySQL:
                        {
                            (FDataAdapter as MySqlDataAdapter).InsertCommand = (value as MySqlCommand);
                            break;
                        }
                    case SQLServer.stText:
                        {
                            (FDataAdapter as TxtDbDataAdapter).InsertCommand= (value as TxtDbCommand);
                            break;
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a command used to update records in the data source. </summary>
        ///
        /// <value> A <see cref="T:System.Data.IDbCommand" /> used during
        ///         <see cref="M:System.Data.IDataAdapter.Update(System.Data.DataSet)" /> to update
        ///         records in the data source for modified rows in the data set. </value>
        ///
        /// <seealso cref="System.Data.Common.DbDataAdapter.UpdateCommand"/>
        ///-------------------------------------------------------------------------------------------------

        public new DbCommand UpdateCommand

        //public DbCommand UpdateCommand
        {
            get
            {
                switch (FSQLServer)
                {

                    case SQLServer.stAccess:
                        {
                            return (FDataAdapter as OleDbDataAdapter).UpdateCommand;
                        }
                    case SQLServer.stPostgreSQL:
                        {
                            return (FDataAdapter as NpgsqlDataAdapter).UpdateCommand;
                        }
                    case SQLServer.stMySQL:
                        {
                            return (FDataAdapter as MySqlDataAdapter).UpdateCommand;
                        }
                    case SQLServer.stText:
                        {
                            return (FDataAdapter as TxtDbDataAdapter).UpdateCommand;
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
            }
            set
            {
                switch (FSQLServer)
                {
                    case SQLServer.stAccess:
                        {
                            (FDataAdapter as OleDbDataAdapter).UpdateCommand = (value as OleDbCommand);
                            break;
                        }
                    case SQLServer.stPostgreSQL:
                        {
                            (FDataAdapter as NpgsqlDataAdapter).UpdateCommand = (value as NpgsqlCommand);
                            break;
                        }
                    case SQLServer.stMySQL:
                        {
                            (FDataAdapter as MySqlDataAdapter).UpdateCommand = (value as MySqlCommand);
                            break;
                        }
                    case SQLServer.stText:
                        {
                            (FDataAdapter as TxtDbDataAdapter).UpdateCommand = (value as TxtDbCommand);
                            break;
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a command used to select records in the data source. </summary>
        ///
        /// <value> A <see cref="T:System.Data.IDbCommand" /> that is used during
        ///         <see cref="M:System.Data.IDataAdapter.Update(System.Data.DataSet)" /> to select
        ///         records from data source for placement in the data set. </value>
        ///
        /// <seealso cref="System.Data.Common.DbDataAdapter.SelectCommand"/>
        ///-------------------------------------------------------------------------------------------------

        public new DbCommand SelectCommand
        //public DbCommand SelectCommand
        {
            get
            {
                switch (FSQLServer)
                {

                    case SQLServer.stAccess:
                        {
                            return (FDataAdapter as OleDbDataAdapter).SelectCommand;
                        }
                    case SQLServer.stPostgreSQL:
                        {
                            return (FDataAdapter as NpgsqlDataAdapter).SelectCommand;
                        }
                    case SQLServer.stMySQL:
                        {
                            return (FDataAdapter as MySqlDataAdapter).SelectCommand;
                        }
                    case SQLServer.stText:
                        {
                            return (FDataAdapter as TxtDbDataAdapter).SelectCommand;
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
            }
            set
            {
                switch (FSQLServer)
                {
                    case SQLServer.stAccess:
                        {
                            (FDataAdapter as OleDbDataAdapter).SelectCommand = (value as OleDbCommand);
                            break;
                        }
                    case SQLServer.stPostgreSQL:
                        {
                            (FDataAdapter as NpgsqlDataAdapter).SelectCommand = (value as NpgsqlCommand);
                            break;
                        }
                    case SQLServer.stMySQL:
                        {
                            (FDataAdapter as MySqlDataAdapter).SelectCommand = (value as MySqlCommand);
                            break;
                        }
                    case SQLServer.stText:
                        {
                            (FDataAdapter as TxtDbDataAdapter).SelectCommand = (value as TxtDbCommand);
                            break;
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
            }
        }

        public void SetupEditCommandsForTable(DataTable DT, string TableName, UniDbConnection dbCon)
        {
            DbCommand DeleteCommand = Tools.CreateDeleteCommand(DT, TableName, dbCon);
            DbCommand InsertCommand = Tools.CreateInsertCommand(DT, TableName, dbCon);
            DbCommand UpdateCommand = Tools.CreateUpdateCommand(DT, TableName, dbCon);
            DbCommand SelectCommand = Tools.CreateSelectCommand(DT, TableName, dbCon);
            switch (FSQLServer)
            {
                case SQLServer.stAccess:
                    {
                        (FDataAdapter as OleDbDataAdapter).DeleteCommand = (DeleteCommand as OleDbCommand) ;
                        (FDataAdapter as OleDbDataAdapter).InsertCommand = (InsertCommand as OleDbCommand);
                        (FDataAdapter as OleDbDataAdapter).UpdateCommand = (UpdateCommand as OleDbCommand);
                        if ((FDataAdapter as OleDbDataAdapter).SelectCommand == null)
                        {
                            (FDataAdapter as OleDbDataAdapter).SelectCommand = (SelectCommand as OleDbCommand);
                        }
                        break;
                    }
                case SQLServer.stPostgreSQL:
                    {
                        {
                            (FDataAdapter as NpgsqlDataAdapter).DeleteCommand = (DeleteCommand as NpgsqlCommand);
                            (FDataAdapter as NpgsqlDataAdapter).InsertCommand = (InsertCommand as NpgsqlCommand);
                            (FDataAdapter as NpgsqlDataAdapter).UpdateCommand = (UpdateCommand as NpgsqlCommand);
                            if ((FDataAdapter as NpgsqlDataAdapter).SelectCommand == null)
                            {
                                (FDataAdapter as NpgsqlDataAdapter).SelectCommand = (SelectCommand as NpgsqlCommand);
                            }
                            break;
                        }
                    }
                case SQLServer.stMySQL:
                    {
                            (FDataAdapter as MySqlDataAdapter).DeleteCommand = (DeleteCommand as MySqlCommand);
                            (FDataAdapter as MySqlDataAdapter).InsertCommand = (InsertCommand as MySqlCommand);
                            (FDataAdapter as MySqlDataAdapter).UpdateCommand = (UpdateCommand as MySqlCommand);
                            if ((FDataAdapter as MySqlDataAdapter).SelectCommand == null)
                            {
                                (FDataAdapter as MySqlDataAdapter).SelectCommand = (SelectCommand as MySqlCommand);
                            }
                            break;
                    }
                case SQLServer.stText:
                    {
                        (FDataAdapter as TxtDbDataAdapter).DeleteCommand = (DeleteCommand as TxtDbCommand);
                        (FDataAdapter as TxtDbDataAdapter).InsertCommand = (InsertCommand as TxtDbCommand);
                        (FDataAdapter as TxtDbDataAdapter).UpdateCommand = (UpdateCommand as TxtDbCommand);
                        if ((FDataAdapter as TxtDbDataAdapter).SelectCommand == null)
                        {
                            (FDataAdapter as TxtDbDataAdapter).SelectCommand = (SelectCommand as TxtDbCommand);
                        }
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Fills the given dt. </summary>
        ///
        /// <exception cref="NotImplementedException">  Thrown when the requested operation is
        ///                                             unimplemented. </exception>
        ///
        /// <param name="DT">   The dt. </param>
        ///-------------------------------------------------------------------------------------------------

        public new void Fill(DataTable DT)
       // public void Fill(DataTable DT)
        {
            switch (FSQLServer)
            {

                case SQLServer.stAccess:
                    {
                        (FDataAdapter as OleDbDataAdapter).Fill(DT);
                        break;
                    }
                case SQLServer.stPostgreSQL:
                    {
                        (FDataAdapter as NpgsqlDataAdapter).Fill(DT);
                        break;
                    }
                case SQLServer.stMySQL:
                    {
                        (FDataAdapter as MySqlDataAdapter).Fill(DT);
                        break;
                    }
                case SQLServer.stText:
                    {
                        (FDataAdapter as TxtDbDataAdapter).Fill(DT);
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        //-------------------------------------------------
        public new int Update(DataTable DT)
        //public int Update(DataTable DT)
        {
            int recs = 0;
            switch (FSQLServer)
            {

                case SQLServer.stAccess:
                    {
                        recs = (FDataAdapter as OleDbDataAdapter).Update(DT);
                        break;
                    }
                case SQLServer.stPostgreSQL:
                    {
                        recs = (FDataAdapter as NpgsqlDataAdapter).Update(DT);
                        break;
                    }
                case SQLServer.stMySQL:
                    {
                        recs = (FDataAdapter as MySqlDataAdapter).Update(DT);
                        break;
                    }
                case SQLServer.stText:
                    {
                        recs = (FDataAdapter as TxtDbDataAdapter).Update(DT);
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
            return recs;
        }
        //-------------------------------------------------
      
        public bool UseFieldHeaders
        {
            get
            {
                if (FSQLServer != SQLServer.stText)
                {
                    return false;
                }
                else
                {
                    return (FDataAdapter as TxtDbDataAdapter).UseFieldHeaders;
                }
            }
            set
            {
                if (FSQLServer == SQLServer.stText)
                {
                    (FDataAdapter as TxtDbDataAdapter).UseFieldHeaders = value;
                }
            }
        }
    }

    /// <summary>   Tools. </summary>
    /// <remarks> A Set of methods that provide basic db utilities for use with UniDbConnection and building SQL Statememts 
    ///           based on the type of SQL Server being used.</remarks>
    public static class Tools
    {
        static string[] SqlServerStringValues = new string[4] { "Access", "MySql", "PostgreSQL","dbText" };
       
        static public string SQLServerString(SQLServer value)
        {
            return SqlServerStringValues[(int)value];
        }
        //-----------------------------------------------------------------------------

        public static string BracketIt(SQLServer Server, string WHatToBracket)
        {
            switch (Server)
            {
                case SQLServer.stAccess:
                    return "[" + WHatToBracket + "]";
                case SQLServer.stMySQL:
                    return (char)96 + WHatToBracket + (char)96;  // back quote
                case SQLServer.stPostgreSQL:
                case SQLServer.stText:
                    return (char)34 + WHatToBracket + (char)34;  // back quote
                default:
                    return "[" + WHatToBracket + "]";

            }

        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Static method to load all fields all records of a table from a database using
        ///     UniDbConnection that is already open uses sql command   "Select * from [tablename]".
        ///     </summary>
        /// <param name="dbConnection"> oleDbConection that is connected to database. </param>
        /// <param name="tablename">    name of the table to load. </param>
        /// <param name="error">        If there is an exception then this returns true, returns message
        ///                             in errString. </param>
        /// <param name="errString">    If there is an exception this returns Exception.Message. </param>
        ///
        /// <returns> DataTable. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static DataTable LoadTable(UniDbConnection dbConnection, string tablename, ref bool error, ref string errString)
        {
            DataTable DT = new DataTable();
            error = true;
            errString = "";
            if (dbConnection != null)
            {
                if (dbConnection.State == ConnectionState.Open)
                {
                    if (tablename != "")
                    {
                        string sqltext = "";
                        try
                        {
                            sqltext = "Select * from " + BracketIt(dbConnection.SQLServerType, tablename) + ";";
                            UniDataAdapter NewRawTableAdapter = new UniDataAdapter(sqltext, dbConnection);
                            NewRawTableAdapter.Fill(DT);
                            error = false;
                        }
                        catch (Exception e)
                        {
                            errString = e.Message + " reading table [" + tablename + "] from " + dbConnection.ConnectionString + " using <" + sqltext + ">";
                        }
                    }
                    else
                    {
                        errString = "Fatal Error reading table [" + tablename + "] Tablename must not be null";
                    }
                }
                else
                {
                    errString = "Fatal Error reading table [" + tablename + "] . dbConnection must be Open";
                }
            }
            else
            {
                errString = "Fatal Error reading table [" + tablename + "]. dbConnection cannot be null";
            }
            return DT;
        }

        //-----------------------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Static method to load all fields and all records meeting Conditions of a table from a
        ///     database using UniDbConnection that is already open uses sql command   "Select * from
        ///     [tablename] Where [Conditions]".
        ///     </summary>
        /// <param name="dbConnection"> oleDbConection that is connected to database. </param>
        /// <param name="tablename">    name of the table to load. </param>
        /// <param name="Conditions">   The conditions. </param>
        /// <param name="error">        If there is an exception then this returns true, returns message
        ///                             in errString. </param>
        /// <param name="errString">    If there is an exception this returns Exception.Message. </param>
        ///
        /// <returns> DataTable. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static DataTable LoadTable(UniDbConnection dbConnection, string tablename, string Conditions, ref bool error, ref string errString)
        {
            DataTable DT = new DataTable();
            error = true;
            errString = "";
            if (dbConnection != null)
            {
                if (dbConnection.State == ConnectionState.Open)
                {
                    if (tablename != "")
                    {
                        string sqltext = "";
                        try
                        {
                            sqltext = "Select * from " + BracketIt(dbConnection.SQLServerType, tablename) + " Where " + Conditions + ";";
                            UniDataAdapter NewRawTableAdapter = new UniDataAdapter(sqltext, dbConnection);
                            NewRawTableAdapter.Fill(DT);
                            error = false;
                        }
                        catch (Exception e)
                        {
                            errString = e.Message + " reading table [" + tablename + "] from " + dbConnection.ConnectionString + " using <" + sqltext + ">";
                        }
                    }
                    else
                    {
                        errString = "Fatal Error reading table [" + tablename + "] Tablename must not be null";
                    }
                }
                else
                {
                    errString = "Fatal Error reading table [" + tablename + "] . dbConnection must be Open";
                }
            }
            else
            {
                errString = "Fatal Error reading table [" + tablename + "]. dbConnection cannot be null";
            }
            return DT;
        }
        //-----------------------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Static method to load all fields all records of a delimited Text file uses sql command   "Select * from [tablename]
        /// where tablename is the filename and the directory is the database, and using the spcified file format
        /// </summary>
        ///
        /// <remarks>   7/2/2019. </remarks>
        ///
        /// <param name="CSVFilename">      Filename of the CSV file. </param>
        /// <param name="theFilesFormat">   the files format. </param>
        /// <param name="isErr">            [out] True if is error, false if not. </param>
        /// <param name="errMsg">           [out] Message describing the error. </param>
        ///
        /// <returns>   DataTable. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static DataTable LoadTable(string CSVFilename, DataFormat theFilesFormat, out bool isErr, out string errMsg)
        {
            string DatabaseName = Path.GetDirectoryName(CSVFilename);
            string Tablename = Path.GetFileName(CSVFilename);
            DataTable result = new DataTable(Tablename);
            isErr = false;
            errMsg = "";
            UniDbConnection aDbCon = new UniDbConnection(SQLServer.stText, "", DatabaseName, "", "", "");
            try
            {
                aDbCon.Open();
                aDbCon.UseFieldHeaders = true;
                aDbCon.FileFormat = theFilesFormat;
                result = Tools.LoadTable(aDbCon, Tablename, ref isErr, ref errMsg);
            }
            catch (Exception ex)
            {
                isErr = true;
                errMsg = ex.Message;
            }
            return result;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Static method to load all fields all records of a delimited Text file uses sql command   "Select * from [tablename]
        /// where tablename is the fielname and the directory is the database
        /// </summary>
        ///
        /// <remarks>   7/2/2019. </remarks>
        ///
        /// <param name="CSVFilename">  Filename of the CSV file. </param>
        /// <param name="isErr">        [out] True if is error, false if not. </param>
        /// <param name="errMsg">       [out] Message describing the error. </param>
        ///
        /// <returns>   DataTable. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static DataTable LoadTable(string CSVFilename, out bool isErr, out string errMsg)
        {
            string DatabaseName = Path.GetDirectoryName(CSVFilename);
            string Tablename = Path.GetFileName(CSVFilename);
            DataTable result = new DataTable(Tablename);
            isErr = false;
            errMsg = "";
            UniDbConnection aDbCon = new UniDbConnection(SQLServer.stText, "", DatabaseName, "", "", "");
            try
            {
                aDbCon.Open();
                aDbCon.UseFieldHeaders = true;
                aDbCon.FileFormat = DataFormat.Unknown;
                result = Tools.LoadTable(aDbCon, Tablename, ref isErr, ref errMsg);
            }
            catch (Exception ex)
            {
                isErr = true;
                errMsg = ex.Message;
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Static method to load all fields and for the top N Rows of a table from a database using UniDbConnection that is already open
        /// uses sql command   "Select * from [tablename] [TOP or LIMIT]"
        /// </summary>
        ///
        /// <param name="Server">       The server. </param>
        /// <param name="dbConnection"> oleDbConection that is connected to database. </param>
        /// <param name="tablename">    name of the table to load. </param>
        /// <param name="NumberOfRows"> Number of rows from beginning to return in select. </param>
        /// <param name="error">        If there is an exception then this returns true, returns message
        ///                             in errString. </param>
        /// <param name="errString">    If there is an exception this returns Exception.Message. </param>
        ///
        /// <returns> DataTable. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static DataTable LoadTable(UniDbConnection dbConnection, string tablename, int NumberOfRows, ref bool error, ref string errString)
        {
            SQLServer Server = dbConnection.SQLServerType;
            DataTable DT = new DataTable();
            error = true;
            errString = "";
            if (dbConnection != null)
            {
                if (dbConnection.State == ConnectionState.Open)
                {
                    if (tablename != "")
                    {
                        string sqltext = "";
                        try
                        {
                            //                            string limittext = SQLLimitRowsForSelect(Server, 1);
                            sqltext = Tools.SelectLimitedRows(Server, "*", BracketIt(Server, tablename), 1);
                            //                                "Select " +limittext+" * from " + BracketIt(dbConnection.SQLServerType,tablename)+";";
                            UniDataAdapter NewRawTableAdapter = new UniDataAdapter(sqltext, dbConnection);
                            NewRawTableAdapter.Fill(DT);
                            error = false;
                        }
                        catch (Exception e)
                        {
                            errString = e.Message + " reading table [" + tablename + "] from " + dbConnection.ConnectionString + " using <" + sqltext + ">";
                        }
                    }
                    else
                    {
                        errString = "Fatal Error reading table [" + tablename + "] Tablename must not be null";
                    }
                }
                else
                {
                    errString = "Fatal Error reading table [" + tablename + "] . dbConnection must be Open";
                }
            }
            else
            {
                errString = "Fatal Error reading table [" + tablename + "]. dbConnection cannot be null";
            }
            return DT;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Static method to specified columns for all records of a table from a database using
        ///             UniDbConnection that is already open uses sql command   "Select [FieldList] from
        ///             [tablename]". </summary>
        ///
        /// <param name="dbConnection"> oleDbConection that is connected to database. </param>
        /// <param name="tablename">    name of the table to load. </param>
        /// <param name="FieldList">    List of Columns. </param>
        /// <param name="error">        If there is an exception then this returns true, returns message
        ///                             in errString. </param>
        /// <param name="errString">    If there is an exception this returns Exception.Message. </param>
        ///
        /// <returns>   DataTable. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static DataTable LoadTable(UniDbConnection dbConnection, string tablename, List<string> FieldList, ref bool error, ref string errString)
        {
            DataTable DT = new DataTable();
            error = true;
            errString = "";
            if (dbConnection != null)
            {
                if (dbConnection.State == ConnectionState.Open)
                {
                    if (tablename != "")
                    {
                        string sqltext = "";
                        try
                        {
                            string ColList = FieldList[0];
                            if (FieldList.Count > 1)
                                for (int i = 1; i < FieldList.Count; i++)
                                    ColList += " , " + FieldList[i];
                            sqltext = "Select " + ColList + " from " + BracketIt(dbConnection.SQLServerType, tablename) + ";";
                            UniDataAdapter NewRawTableAdapter = new UniDataAdapter(sqltext, dbConnection);
                            NewRawTableAdapter.Fill(DT);
                            error = false;
                        }
                        catch (Exception e)
                        {
                            errString = e.Message + " reading table [" + tablename + "] from " + dbConnection.ConnectionString + " using <" + sqltext + ">";
                        }
                    }
                    else
                    {
                        errString = "Fatal Error reading table [" + tablename + "] Tablename must not be null";
                    }
                }
                else
                {
                    errString = "Fatal Error reading table [" + tablename + "] . dbConnection must be Open";
                }
            }
            else
            {
                errString = "Fatal Error reading table [" + tablename + "]. dbConnection cannot be null";
            }
            return DT;
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Static method to load specified Columns for all records meeting Conditions of a table from a database using
        ///             UniDbConnection that is already open uses sql command   "Select [FielList] from
        ///             [tablename] Where [Conditions]". </summary>
        ///
        /// <param name="dbConnection"> oleDbConection that is connected to database. </param>
        /// <param name="tablename">    name of the table to load. </param>
        /// <param name="FieldList">    List of Columns. </param>
        /// <param name="Conditions">   The conditions. </param>
        /// <param name="error">        If there is an exception then this returns true, returns message
        ///                             in errString. </param>
        /// <param name="errString">    If there is an exception this returns Exception.Message. </param>
        ///
        /// <returns>   DataTable. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static DataTable LoadTable(UniDbConnection dbConnection, string tablename, List<string> FieldList, string Conditions, ref bool error, ref string errString)
        {
            DataTable DT = new DataTable();
            error = true;
            errString = "";
            if (dbConnection != null)
            {
                if (dbConnection.State == ConnectionState.Open)
                {
                    if ((tablename != "") & (Conditions != "") & (FieldList.Count > 0))
                    {
                        string sqltext = "";
                        try
                        {
                            // Build ColumnList
                            string ColList = FieldList[0];
                            if (FieldList.Count > 1)
                                for (int i = 1; i < FieldList.Count; i++)
                                    ColList += " , " + FieldList[i];
                            sqltext = "Select " + ColList + " from " + BracketIt(dbConnection.SQLServerType, tablename) + " Where " + Conditions;
                            UniDataAdapter NewRawTableAdapter = new UniDataAdapter(sqltext, dbConnection);
                            NewRawTableAdapter.Fill(DT);
                            error = false;
                        }
                        catch (Exception e)
                        {
                            errString = e.Message + " reading table [" + tablename + "] from " + dbConnection.ConnectionString + " using <" + sqltext + ">";
                        }
                    }
                    else
                    {
                        errString = "Fatal Error reading table [" + tablename + "] Tablename, Conditions, and Fieldlist must not be null";
                    }
                }
                else
                {
                    errString = "Fatal Error reading table [" + tablename + "] . dbConnection must be Open";
                }
            }
            else
            {
                errString = "Fatal Error reading table [" + tablename + "]. dbConnection cannot be null";
            }
            return DT;
        }

      
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Writes a table. </summary>
        ///
        /// <param name="theDT">        the dt. </param>
        /// <param name="TxtFilename">  Filename of the text file. </param>
        /// <param name="CreateHeader"> true to create header. </param>
        /// <param name="theFormat">    the format. </param>
        /// <param name="ErrMessage">   [out] Message describing the error. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        static public bool WriteTable(DataTable theDT, string TxtFilename, bool CreateHeader, DataFormat theFormat, out string ErrMessage)
        {
            bool result = false;
            ErrMessage = "";
            try
            {
                if (theDT == null)
                {
                    ErrMessage = "DataTable can not be null";
                }
                else
                {
                    if (theDT.Columns.Count < 1)
                    {
                        ErrMessage = "DataTable must have columns defined";
                    }
                    else
                    {
                        // OK is this is fixed fields have to calcualte the width of the columns
                        int[] Colwidth = new int[theDT.Columns.Count];
                        if (theFormat == DataFormat.FixedFields)
                        {

                            for(int i=0;i<theDT.Columns.Count;i++)
                            {
                                Colwidth[i] = theDT.Columns[i].ColumnName.Length+1;
                            }
                            foreach (DataRow DR in theDT.Rows)
                            {
                                for (int i = 0; i < theDT.Columns.Count; i++)
                                {
                                    int width = DR[i].ToString().Length+1;
                                    if (width > Colwidth[i])
                                    {
                                        Colwidth[i] = width;
                                    }
                                }

                            }
                        }
                        // ok now ready to write
                        using (StreamWriter SW = new StreamWriter(TxtFilename))
                        {
                            string DataSep = "";
                            switch (theFormat)
                            {
                                case DataFormat.CommaDelimited:
                                    DataSep = Comma;
                                    break;
                                case DataFormat.FixedFields:
                                    DataSep = "";
                                    break;
                                case DataFormat.SpaceDelimited:
                                    DataSep = " ";
                                    break;
                                case DataFormat.TabDelimited:
                                    DataSep = "\t";
                                    break;
                                default:
                                    DataSep = Comma;
                                    break;
                            }
                            int colcnt = theDT.Columns.Count;
                            if (CreateHeader)
                            {
                                string temp = theDT.Columns[0].ColumnName;
                                if (theFormat == DataFormat.FixedFields)
                                {
                                    temp = temp.PadRight(Colwidth[0]);
                                }

                                for (int i = 1; i < theDT.Columns.Count; i++)
                                {
                                    temp += "," + theDT.Columns[i].ColumnName;
                                    if (theFormat == DataFormat.FixedFields)
                                    {
                                        temp = temp.PadRight(Colwidth[i]);
                                    }
                                }

                                SW.WriteLine(temp);

                            }
                            foreach (DataRow DR in theDT.Rows)
                            {
                                string temp = DR[0].ToString();
                                for (int i = 1; i < theDT.Columns.Count;i++ )
                                {
                                    temp += DataSep + DR[i].ToString();
                                    if (theFormat == DataFormat.FixedFields)
                                    {
                                        temp = temp.PadRight(Colwidth[i]);
                                    }
                                }
                                SW.WriteLine(temp);
                            }
                            result = true;
                        }
                    }
                }
            }
                
            catch (Exception ex)
            {
                result = false;
                ErrMessage = ex.Message;
            }

            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Writes a table. </summary>
        ///
        /// <remarks>   8/10/2018. </remarks>
        ///
        /// <param name="theDT">        the dt. </param>
        /// <param name="TxtFilename">  Filename of the text file. </param>
        /// <param name="Append">       True to append. </param>
        /// <param name="Fast">         True to fast. </param>
        /// <param name="CreateHeader"> true to create header. </param>
        /// <param name="theFormat">    the format. </param>
        /// <param name="ErrMessage">   [out] Message describing the error. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------
        static public bool WriteTable(DataTable theDT, string TxtFilename, bool Append, bool Fast, bool CreateHeader, DataFormat theFormat, out string ErrMessage)
        {
            return WriteTableFull(theDT, TxtFilename, Append, Fast, CreateHeader, theFormat, null, out ErrMessage);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Writes a table.</summary>
        ///
        /// <remarks> Quay, 5/29/2019.</remarks>
        ///
        /// <param name="theDT">        the dt.</param>
        /// <param name="TxtFilename">  Filename of the text file.</param>
        /// <param name="Append">       True to append.</param>
        /// <param name="Fast">         True to fast.</param>
        /// <param name="CreateHeader"> true to create header.</param>
        /// <param name="theFormat">    the format, see DataFormat.</param>
        /// <param name="Missing">      The missing phrase to use of column is null.</param>
        /// <param name="ErrMessage">   [out] Message describing the error.</param>
        ///
        /// <returns> true if it succeeds, false if it fails.</returns>
        ///-------------------------------------------------------------------------------------------------

        static public bool WriteTableFull(DataTable theDT, string TxtFilename, bool Append, bool Fast, bool CreateHeader, DataFormat theFormat, string Missing, out string ErrMessage)
        {
            bool result = false;
            ErrMessage = "";
            try
            {
                if (theDT == null)
                {
                    ErrMessage = "DataTable can not be null";
                }
                else
                {

                    // ok unless marked fast, check to make sure existing file has same fields as DataTable
                    bool GoodToGo = true;
                    if (Append && (! Fast))
                    {
                       // Need to add this
                    }


                    if (!GoodToGo)
                    {
                        ErrMessage = "DataTable must have same column structure as the text file field headers";
                    }
                    else
                    {
                        if (theDT.Columns.Count < 1)
                        {
                            ErrMessage = "DataTable must have columns defined";
                        }
                        else
                        {
                            // OK is this is fixed fields have to calcualte the width of the columns
                            int[] Colwidth = new int[theDT.Columns.Count];
                            if (theFormat == DataFormat.FixedFields)
                            {

                                for (int i = 0; i < theDT.Columns.Count; i++)
                                {
                                    Colwidth[i] = theDT.Columns[i].ColumnName.Length + 1;
                                }
                                foreach (DataRow DR in theDT.Rows)
                                {
                                    for (int i = 0; i < theDT.Columns.Count; i++)
                                    {
                                        int width = DR[i].ToString().Length + 1;
                                        if (width > Colwidth[i])
                                        {
                                            Colwidth[i] = width;
                                        }
                                    }

                                }
                            }
                            // ok now ready to write
                            using (StreamWriter SW = new StreamWriter(TxtFilename, Append))
                            {
                                string DataSep = "";
                                switch (theFormat)
                                {
                                    case DataFormat.CommaDelimited:
                                        DataSep = Comma;
                                        break;
                                    case DataFormat.FixedFields:
                                        DataSep = "";
                                        break;
                                    case DataFormat.SpaceDelimited:
                                        DataSep = " ";
                                        break;
                                    case DataFormat.TabDelimited:
                                        DataSep = "\t";
                                        break;
                                    default:
                                        DataSep = Comma;
                                        break;
                                }
                                int colcnt = theDT.Columns.Count;
                                if (CreateHeader && (!Append))
                                {
                                    string temp = theDT.Columns[0].ColumnName;
                                    if (theFormat == DataFormat.FixedFields)
                                    {
                                        temp = temp.PadRight(Colwidth[0]);
                                    }

                                    for (int i = 1; i < theDT.Columns.Count; i++)
                                    {
                                        temp += "," + theDT.Columns[i].ColumnName;
                                        if (theFormat == DataFormat.FixedFields)
                                        {
                                            temp = temp.PadRight(Colwidth[i]);
                                        }
                                    }

                                    SW.WriteLine(temp);

                                }
                                foreach (DataRow DR in theDT.Rows)
                                {
                                    string temp = DR[0].ToString();
                                    for (int i = 1; i < theDT.Columns.Count; i++)
                                    {

                                        // QUAY EDIT 5 / 29 /19 =======================
                                        // Added code to support writing a special missing value
                                        if ((Missing != null) && (DR[i] == null))
                                        {
                                            temp += DataSep + Missing;
                                        }
                                        else
                                        { 
                                            temp += DataSep + DR[i].ToString();
                                        }
                                        // ========================
                                        temp += DataSep + DR[i].ToString();
                                        if (theFormat == DataFormat.FixedFields)
                                        {
                                            temp = temp.PadRight(Colwidth[i]);
                                        }
                                    }
                                    SW.WriteLine(temp);
                                }
                                result = true;
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                result = false;
                ErrMessage = ex.Message;
            }

            return result;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Progress report. </summary>
        ///
        /// <remarks>  A delegate used by some routines to report database task progress. </remarks>
        ///
        /// <param name="report"> The Message to report. </param>
        /// <param name="progress">  Progress towards MAX value </param>
        /// <param name="max">    The maximum reporting value. </param>
        ///-------------------------------------------------------------------------------------------------

        public delegate void ProgressReport(string report, int progress, int max);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Convert net DataType to oledb data type. </summary>
        ///
        /// <param name="DataType"> System DataType of the data. </param>
        ///
        /// <returns>   The net datatype converted type to oledb type. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static OleDbType ConvertNetTypeToOledbType(Type DataType)
        {
            OleDbType dbDT = OleDbType.Empty;
            if (DataType == System.Type.GetType("System.Int32"))
            {
                dbDT = OleDbType.Integer;
            }
            else
                //        UnsignedInt A 32-bit unsigned integer (DBTYPE_UI4). This maps to UInt32. 
                if (DataType == System.Type.GetType("System.UInt32"))
                {
                    dbDT = OleDbType.UnsignedInt;

                }
                else
                    //       SmallInt A 16-bit signed integer (DBTYPE_I2). This maps to Int16. 
                    if (DataType == System.Type.GetType("System.Int16"))
                    {
                        dbDT = OleDbType.SmallInt;
                    }
                    else
                        //        UnsignedSmallInt A 16-bit unsigned integer (DBTYPE_UI2). This maps to UInt16. 
                        if (DataType == System.Type.GetType("System.UInt16"))
                        {
                            dbDT = OleDbType.UnsignedSmallInt;

                        }
                        else
                            if (DataType == System.Type.GetType("System.Int64"))
                            {
                                dbDT = OleDbType.BigInt;
                            }
                            else
                                //        UnsignedBigInt A 64-bit unsigned integer (DBTYPE_UI8). This maps to UInt64. 
                                if (DataType == System.Type.GetType("System.UInt64"))
                                {
                                    dbDT = OleDbType.UnsignedBigInt;
                                }
                                else
                                    //        Double A floating point number within the range of -1.79E +308 through 1.79E +308 (DBTYPE_R8). This maps to Double. 
                                    if (DataType == System.Type.GetType("System.Double"))
                                    {
                                        dbDT = OleDbType.Double;
                                    }
                                    else
                                    //        Float A floating point number within the range of -1.79E +308 through 1.79E +308 (DBTYPE_R8). This maps to Double. 
                                    if (DataType == System.Type.GetType("System.Float"))
                                    {
                                        dbDT = OleDbType.Double;
                                    }
                                    else
                                        //        Char A character string (DBTYPE_STR). This maps to String. 
                                        //        VarChar A variable-length stream of non-Unicode characters (OleDbParameter only). This maps to String. 
                                        if (DataType == System.Type.GetType("System.String"))
                                        {
                                            // using varchar here becuase char type often limited to 255 chars
                                            dbDT = OleDbType.VarChar;
                                        }
                                        else
                                            //  Date Date data, stored as a double (DBTYPE_DATE). The whole portion is the number of days since December 30, 1899, while the fractional portion is a fraction of a day. This maps to DateTime. 
                                            //  DBDate Date data in the format yyyymmdd (DBTYPE_DBDATE). This maps to DateTime. 
                                            //  DBTime Time data in the format hhmmss (DBTYPE_DBTIME). This maps to TimeSpan. 
                                            //  DBTimeStamp Data and time data in the format yyyymmddhhmmss (DBTYPE_DBTIMESTAMP). This maps to DateTime. 
                                            //        Filetime A 64-bit unsigned integer representing the number of 100-nanosecond intervals since January 1, 1601 (DBTYPE_FILETIME). This maps to DateTime. 
                                            if (DataType == System.Type.GetType("System.DateTime"))
                                            {
                                                // using DBTimeSTamp to conve full info
                                                dbDT = OleDbType.DBTimeStamp;
                                            }
                                            else
                                                // Boolean A Boolean value (DBTYPE_BOOL). This maps to Boolean. 
                                                if (DataType == System.Type.GetType("System.Boolean"))
                                                {
                                                    dbDT = OleDbType.Boolean;
                                                }
                                                else
                                                    //  Decimal A fixed precision and scale numeric value between -1038 -1 and 10 38 -1 (DBTYPE_DECIMAL). This maps to Decimal. 
                                                    //  Currency A currency value ranging from -263 (or -922,337,203,685,477.5808) to 2 63 -1 (or +922,337,203,685,477.5807) with an accuracy to a ten-thousandth of a currency unit (DBTYPE_CY). This maps to Decimal. 
                                                    //  Numeric An exact numeric value with a fixed precision and scale (DBTYPE_NUMERIC). This maps to Decimal. 
                                                    if (DataType == System.Type.GetType("System.Decimal"))
                                                    {
                                                    }
                                                    else
                                                        // Single A floating point number within the range of -3.40E +38 through 3.40E +38 (DBTYPE_R4). This maps to Single. 
                                                        if (DataType == System.Type.GetType("System.Single"))
                                                        {
                                                            dbDT = OleDbType.Single;
                                                        }
                                                        else
                                                            //        UnsignedTinyInt A 8-bit unsigned integer (DBTYPE_UI1). This maps to Byte. 
                                                            if (DataType == System.Type.GetType("System.Byte"))
                                                            {
                                                                dbDT = OleDbType.UnsignedTinyInt;
                                                            }
                                                            else
                                                                //  TinyInt A 8-bit signed integer (DBTYPE_I1). This maps to SByte. 
                                                                if (DataType == System.Type.GetType("System.SByte"))
                                                                {
                                                                    dbDT = OleDbType.TinyInt;
                                                                }

            return dbDT;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Convert net DataType to mySQL data type. </summary>
        ///
        /// <param name="DataType"> System DataType of the data. </param>
        ///
        /// <returns>   The net converted type to mySQL type. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static MySql.Data.MySqlClient.MySqlDbType ConvertNetTypeToMySQLType(Type DataType)
        {
            MySql.Data.MySqlClient.MySqlDbType dbDT = new MySqlDbType();
            if (DataType == System.Type.GetType("System.Int32"))
            {
                dbDT = MySql.Data.MySqlClient.MySqlDbType.Int32;
            }
            else
                //        UnsignedInt A 32-bit unsigned integer (DBTYPE_UI4). This maps to UInt32. 
                if (DataType == System.Type.GetType("System.UInt32"))
                {
                    dbDT = MySql.Data.MySqlClient.MySqlDbType.UInt32;

                }
                else
                    //       SmallInt A 16-bit signed integer (DBTYPE_I2). This maps to Int16. 
                    if (DataType == System.Type.GetType("System.Int16"))
                    {
                        dbDT = MySql.Data.MySqlClient.MySqlDbType.Int16;
                    }
                    else
                        //        UnsignedSmallInt A 16-bit unsigned integer (DBTYPE_UI2). This maps to UInt16. 
                        if (DataType == System.Type.GetType("System.UInt16"))
                        {
                            dbDT = MySql.Data.MySqlClient.MySqlDbType.UInt16;

                        }
                        else
                            if (DataType == System.Type.GetType("System.Int64"))
                            {
                                dbDT = MySql.Data.MySqlClient.MySqlDbType.Int64;
                            }
                            else
                                //        UnsignedBigInt A 64-bit unsigned integer (DBTYPE_UI8). This maps to UInt64. 
                                if (DataType == System.Type.GetType("System.UInt64"))
                                {
                                    dbDT = MySql.Data.MySqlClient.MySqlDbType.UInt64;
                                }
                                else
                                    //        Double A floating point number within the range of -1.79E +308 through 1.79E +308 (DBTYPE_R8). This maps to Double. 
                                    if (DataType == System.Type.GetType("System.Double"))
                                    {
                                        dbDT = MySql.Data.MySqlClient.MySqlDbType.Double;
                                    }
                                    else
                                    //        Float A floating point number within the range of -1.79E +308 through 1.79E +308 (DBTYPE_R8). This maps to Double. 
                                    if (DataType == System.Type.GetType("System.Float"))
                                    {
                                        dbDT = MySql.Data.MySqlClient.MySqlDbType.Double;
                                    }
                                    else
                                        //        Char A character string (DBTYPE_STR). This maps to String. 
                                        //        VarChar A variable-length stream of non-Unicode characters (OleDbParameter only). This maps to String. 
                                        if (DataType == System.Type.GetType("System.String"))
                                        {
                                            // using varchar here becuase char type often limited to 255 chars
                                            dbDT = MySql.Data.MySqlClient.MySqlDbType.VarChar;
                                        }
                                        else
                                            //  Date Date data, stored as a double (DBTYPE_DATE). The whole portion is the number of days since December 30, 1899, while the fractional portion is a fraction of a day. This maps to DateTime. 
                                            //  DBDate Date data in the format yyyymmdd (DBTYPE_DBDATE). This maps to DateTime. 
                                            //  DBTime Time data in the format hhmmss (DBTYPE_DBTIME). This maps to TimeSpan. 
                                            //  DBTimeStamp Data and time data in the format yyyymmddhhmmss (DBTYPE_DBTIMESTAMP). This maps to DateTime. 
                                            //  Filetime A 64-bit unsigned integer representing the number of 100-nanosecond intervals since January 1, 1601 (DBTYPE_FILETIME). This maps to DateTime. 
                                            if (DataType == System.Type.GetType("System.DateTime"))
                                            {
                                                // using DBTimeSTamp to conve full info
                                                dbDT = MySql.Data.MySqlClient.MySqlDbType.DateTime;
                                            }
                                            else
                                                // Boolean A Boolean value (DBTYPE_BOOL). This maps to Boolean.
                                                // MySQL does not have a Boolean 
                                                if (DataType == System.Type.GetType("System.Boolean"))
                                                {
                                                    dbDT = MySql.Data.MySqlClient.MySqlDbType.UByte;
                                                }
                                                else
                                                    //  Decimal A fixed precision and scale numeric value between -1038 -1 and 10 38 -1 (DBTYPE_DECIMAL). This maps to Decimal. 
                                                    //  Currency A currency value ranging from -263 (or -922,337,203,685,477.5808) to 2 63 -1 (or +922,337,203,685,477.5807) with an accuracy to a ten-thousandth of a currency unit (DBTYPE_CY). This maps to Decimal. 
                                                    //  Numeric An exact numeric value with a fixed precision and scale (DBTYPE_NUMERIC). This maps to Decimal. 
                                                    if (DataType == System.Type.GetType("System.Decimal"))
                                                    {
                                                        dbDT = MySql.Data.MySqlClient.MySqlDbType.Decimal;
                                                    }
                                                    else
                                                        // Single A floatinging point number within the range of -3.40E +38 through 3.40E +38 (DBTYPE_R4). This maps to Single. 
                                                        if (DataType == System.Type.GetType("System.Single"))
                                                        {
                                                            dbDT = MySql.Data.MySqlClient.MySqlDbType.Float;
                                                        }
                                                        else
                                                            //        UnsignedTinyInt A 8-bit unsigned integer (DBTYPE_UI1). This maps to Byte. 
                                                            if (DataType == System.Type.GetType("System.Byte"))
                                                            {
                                                                dbDT = MySql.Data.MySqlClient.MySqlDbType.UByte;
                                                            }
                                                            else
                                                                //  TinyInt A 8-bit signed integer (DBTYPE_I1). This maps to SByte. 
                                                                if (DataType == System.Type.GetType("System.SByte"))
                                                                {
                                                                    dbDT = MySql.Data.MySqlClient.MySqlDbType.Byte;
                                                                }

            return dbDT;
        }

        
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Convert net date type to postgreSQL data type. </summary>
        ///
        /// <param name="DataType"> System DataType of the data. </param>
        ///
        /// <returns>   The net converted type to postgrSQL type (NPGSQL). </returns>
        ///-------------------------------------------------------------------------------------------------

        public static NpgsqlTypes.NpgsqlDbType ConvertNetTypeToPostGreSQLType(Type DataType)
        {
            NpgsqlTypes.NpgsqlDbType dbDT = new NpgsqlTypes.NpgsqlDbType();

            if (DataType == System.Type.GetType("System.Int32"))
            {
                dbDT = NpgsqlTypes.NpgsqlDbType.Integer;
            }
            else
                //        UnsignedInt A 32-bit unsigned integer (DBTYPE_UI4). This maps to UInt32. 
                if (DataType == System.Type.GetType("System.UInt32"))
                {
                    dbDT = NpgsqlTypes.NpgsqlDbType.Bigint;

                }
                else
                    //       SmallInt A 16-bit signed integer (DBTYPE_I2). This maps to Int16. 
                    if (DataType == System.Type.GetType("System.Int16"))
                    {
                        dbDT = NpgsqlTypes.NpgsqlDbType.Smallint;
                    }
                    else
                        //        UnsignedSmallInt A 16-bit unsigned integer (DBTYPE_UI2). This maps to UInt16. 
                        //        Postgres does not have a UNINT16
                        if (DataType == System.Type.GetType("System.UInt16"))
                        {
                            dbDT = NpgsqlTypes.NpgsqlDbType.Integer;

                        }
                        else
                            if (DataType == System.Type.GetType("System.Int64"))
                            {
                                dbDT = NpgsqlTypes.NpgsqlDbType.Bigint;
                            }
                            else
                                //        UnsignedBigInt A 64-bit unsigned integer (DBTYPE_UI8). This maps to UInt64. 
                                //        Postgres does not have a UINT64
                                if (DataType == System.Type.GetType("System.UInt64"))
                                {
                                    dbDT = NpgsqlTypes.NpgsqlDbType.Bigint;
                                }
                                else
                                    //        Double A floating point number within the range of -1.79E +308 through 1.79E +308 (DBTYPE_R8). This maps to Double. 
                                    if (DataType == System.Type.GetType("System.Double"))
                                    {
                                        dbDT = NpgsqlTypes.NpgsqlDbType.Double;
                                    }
                                    else
                                    //        Float A floating point number within the range of -1.79E +308 through 1.79E +308 (DBTYPE_R8). This maps to Double. 
                                    if (DataType == System.Type.GetType("System.Float"))
                                        {
                                            dbDT = NpgsqlTypes.NpgsqlDbType.Double;
                                        }
                                        else
                                        //        Char A character string (DBTYPE_STR). This maps to String. 
                                        //        VarChar A variable-length stream of non-Unicode characters (OleDbParameter only). This maps to String. 
                                        if (DataType == System.Type.GetType("System.String"))
                                        {
                                            // using varchar here becuase char type often limited to 255 chars
                                            dbDT = NpgsqlTypes.NpgsqlDbType.Varchar;
                                        }
                                        else
                                            // Postgres does not have a Datetime field, has Date and a Time
                                            if (DataType == System.Type.GetType("System.DateTime"))
                                            {
                                                // using DBTimeSTamp to conve full info
                                                dbDT = NpgsqlTypes.NpgsqlDbType.Date;
                                            }
                                            else
                                                // Boolean A Boolean value (DBTYPE_BOOL). This maps to Boolean. 
                                                if (DataType == System.Type.GetType("System.Boolean"))
                                                {
                                                    dbDT = NpgsqlTypes.NpgsqlDbType.Boolean;
                                                }
                                                else
                                                    //  Decimal A fixed precision and scale numeric value between -1038 -1 and 10 38 -1 (DBTYPE_DECIMAL). This maps to Decimal. 
                                                    //  Currency A currency value ranging from -263 (or -922,337,203,685,477.5808) to 2 63 -1 (or +922,337,203,685,477.5807) with an accuracy to a ten-thousandth of a currency unit (DBTYPE_CY). This maps to Decimal. 
                                                    //  Numeric An exact numeric value with a fixed precision and scale (DBTYPE_NUMERIC). This maps to Decimal. 
                                                    // Postgres does not have a decimal
                                                    if (DataType == System.Type.GetType("System.Decimal"))
                                                    {
                                                        dbDT = NpgsqlTypes.NpgsqlDbType.Numeric;
                                                    }
                                                    else
                                                        // Single A floating point number within the range of -3.40E +38 through 3.40E +38 (DBTYPE_R4). This maps to Single. 
                                                        if (DataType == System.Type.GetType("System.Single"))
                                                        {
                                                            dbDT = NpgsqlTypes.NpgsqlDbType.Real;
                                                        }
                                                        else
                                                            //        UnsignedTinyInt A 8-bit unsigned integer (DBTYPE_UI1). This maps to Byte. 
                                                            // Postgres does not have a simple BYTE type
                                                            if (DataType == System.Type.GetType("System.Byte"))
                                                            {
                                                                dbDT = NpgsqlTypes.NpgsqlDbType.Smallint;
                                                            }
                                                            else
                                                                //  TinyInt A 8-bit signed integer (DBTYPE_I1). This maps to SByte. 
                                                                // Postgres does not have a simple SBYTE type
                                                                if (DataType == System.Type.GetType("System.SByte"))
                                                                {
                                                                    dbDT = NpgsqlTypes.NpgsqlDbType.Smallint;
                                                                }

            return dbDT;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Convert net type to database type. </summary>
        ///
        /// <param name="DataType"> System DataType of the data. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public static DbType ConvertNetTypeToDbType(Type DataType)
        {
            DbType dbDT = new  DbType();

            if (DataType == System.Type.GetType("System.Int32"))
            {
                dbDT = DbType.Int32;
            }
            else
                //        UnsignedInt A 32-bit unsigned integer (DBTYPE_UI4). This maps to UInt32. 
                if (DataType == System.Type.GetType("System.UInt32"))
                {
                    dbDT = DbType.UInt32;

                }
                else
                    //       SmallInt A 16-bit signed integer (DBTYPE_I2). This maps to Int16. 
                    if (DataType == System.Type.GetType("System.Int16"))
                    {
                        dbDT = DbType.Int16;
                    }
                    else
                        //        UnsignedSmallInt A 16-bit unsigned integer (DBTYPE_UI2). This maps to UInt16. 
                        //        Postgres does not have a UNINT16
                        if (DataType == System.Type.GetType("System.UInt16"))
                        {
                            dbDT = DbType.UInt16;

                        }
                        else
                            if (DataType == System.Type.GetType("System.Int64"))
                            {
                                dbDT = DbType.Int64;
                            }
                            else
                                //        UnsignedBigInt A 64-bit unsigned integer (DBTYPE_UI8). This maps to UInt64. 
                                //        Postgres does not have a UINT64
                                if (DataType == System.Type.GetType("System.UInt64"))
                                {
                                    dbDT = DbType.UInt64;
                                }
                                else
                                    //        Double A floating point number within the range of -1.79E +308 through 1.79E +308 (DBTYPE_R8). This maps to Double. 
                                    if (DataType == System.Type.GetType("System.Double"))
                                    {
                                        dbDT = DbType.Double;
                                    }
                                    else
                                    //        Float A floating point number within the range of -1.79E +308 through 1.79E +308 (DBTYPE_R8). This maps to Double. 
                                    if (DataType == System.Type.GetType("System.Float"))
                                        {
                                            dbDT = DbType.Double;
                                        }
                                        else
                                        //        Char A character string (DBTYPE_STR). This maps to String. 
                                        //        VarChar A variable-length stream of non-Unicode characters (OleDbParameter only). This maps to String. 
                                        if (DataType == System.Type.GetType("System.String"))
                                        {
                                            // using varchar here becuase char type often limited to 255 chars
                                            dbDT = DbType.String;
                                        }
                                        else
                                            // Postgres does not have a Datetime field, has Date and a Time
                                            if (DataType == System.Type.GetType("System.DateTime"))
                                            {
                                                // using DBTimeSTamp to conve full info
                                                dbDT = DbType.DateTime;
                                            }
                                            else
                                                // Boolean A Boolean value (DBTYPE_BOOL). This maps to Boolean. 
                                                if (DataType == System.Type.GetType("System.Boolean"))
                                                {
                                                    dbDT = DbType.Boolean;
                                                }
                                                else
                                                    //  Decimal A fixed precision and scale numeric value between -1038 -1 and 10 38 -1 (DBTYPE_DECIMAL). This maps to Decimal. 
                                                    //  Currency A currency value ranging from -263 (or -922,337,203,685,477.5808) to 2 63 -1 (or +922,337,203,685,477.5807) with an accuracy to a ten-thousandth of a currency unit (DBTYPE_CY). This maps to Decimal. 
                                                    //  Numeric An exact numeric value with a fixed precision and scale (DBTYPE_NUMERIC). This maps to Decimal. 
                                                    // Postgres does not have a decimal
                                                    if (DataType == System.Type.GetType("System.Decimal"))
                                                    {
                                                        dbDT = DbType.Decimal;
                                                    }
                                                    else
                                                        // Single A doubleing point number within the range of -3.40E +38 through 3.40E +38 (DBTYPE_R4). This maps to Single. 
                                                        if (DataType == System.Type.GetType("System.Single"))
                                                        {
                                                            dbDT = DbType.Single;
                                                        }
                                                        else
                                                            //        UnsignedTinyInt A 8-bit unsigned integer (DBTYPE_UI1). This maps to Byte. 
                                                            // Postgres does not have a simple BYTE type
                                                            if (DataType == System.Type.GetType("System.Byte"))
                                                            {
                                                                dbDT = DbType.Byte;
                                                            }
                                                            else
                                                                //  TinyInt A 8-bit signed integer (DBTYPE_I1). This maps to SByte. 
                                                                // Postgres does not have a simple SBYTE type
                                                                if (DataType == System.Type.GetType("System.SByte"))
                                                                {
                                                                    dbDT = DbType.SByte;
                                                                }

            return dbDT;
        }

        static int DoubleCharSize = Double.MinValue.ToString().Length + 1;
        static int Int64CharSize = UInt64.MaxValue.ToString().Length + 1;
        static int Int32CharSize = UInt32.MaxValue.ToString().Length + 1;
        static int Int16CharSize = UInt16.MaxValue.ToString().Length + 1;
        static int BoolCharSize = false.ToString().Length +1;
        static int SingleCharSize = Single.MinValue.ToString().Length + 1;
        static int DateTimeCharSize = DateTime.MaxValue.ToString().Length + 1;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Character sizeof data type. </summary>
        ///
        /// <param name="theType">  Type of the. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public static int CharSizeofDataType(Type theType)
        {
            int theSize = 0;
            if (theType == Type.GetType("bool"))
            {
            }
            else
                if (theType == Type.GetType("Int64"))
                {
                    theSize = Int64CharSize;
                }
                else
                    if (theType == Type.GetType("Int32"))
                    {
                        theSize = Int32CharSize;
                    }
                    else
                        if (theType == Type.GetType("Int16"))
                        {
                            theSize = Int16CharSize;
                        }
                        else
                            if (theType == Type.GetType("Int64"))
                            {
                                theSize = Int64CharSize;
                            }
                            else
                                if (theType == Type.GetType("Int32"))
                                {
                                    theSize = Int32CharSize;
                                }
                                else
                                    if (theType == Type.GetType("Int16"))
                                    {
                                        theSize = Int16CharSize;
                                    }
                                    else
                                        if (theType == Type.GetType("Byte"))
                                        {
                                            theSize = 3;
                                        }
                                        else
                                            if (theType == Type.GetType("DateTime"))
                                            {
                                                theSize = DateTimeCharSize;
                                            }
                                            else
                                                if (theType == Type.GetType("Decimal"))
                                                {
                                                    theSize = SingleCharSize;
                                                }
                                                else
                                                    if (theType == Type.GetType("Double"))
                                                    {
                                                        theSize = DoubleCharSize;
                                                    }
                                                    if (theType == Type.GetType("Float"))
                                                    {
                                                        theSize = DoubleCharSize;
                                                    }
                                                    else
                                                        if (theType == Type.GetType("TimeSpan"))
                                                        {
                                                            theSize = DateTimeCharSize;
                                                        }
                                                        else
                                                            if (theType == Type.GetType("bool"))
                                                            {
                                                                theSize = -1;
                                                            }
            return theSize;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Character sizeof database type. </summary>
        ///
        /// <param name="theType">  Type of the. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public static int CharSizeofDbType(DbType theType)
        {
            int theSize = 0;
            switch (theType)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.Binary:
                case DbType.Object:
                case DbType.Guid:
                case DbType.String:
                case DbType.StringFixedLength:
                case DbType.VarNumeric:
                case DbType.Xml:
                    {
                        theSize = -1;
                        break;
                    }
                case DbType.Boolean:
                case DbType.SByte:
                case DbType.Byte:
                    {
                        theSize = BoolCharSize;
                        break;
                    }
                case DbType.Currency:
                case DbType.Decimal:
                    {
                        theSize = SingleCharSize;
                        break;
                    }
                case DbType.Time:
                case DbType.Date:
                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                    {
                        theSize = DateTimeCharSize;  // Not sure if this is right
                        break;
                    }
                case DbType.Double:
                    {
                        theSize = DoubleCharSize;
                        break;
                    }
                case DbType.UInt16:
                case DbType.Int16:
                    {
                        theSize = Int16CharSize;
                        break;
                    }
                case DbType.UInt32:
                case DbType.Int32:
                    {
                        theSize = Int32CharSize;
                        break;
                    }
                case DbType.Int64:
                case DbType.UInt64:
                    {
                        theSize = Int64CharSize;
                        break;
                    }
                case DbType.Single:
                    {
                        theSize = SingleCharSize;
                        break;
                    }

            }
            return theSize;
        }
       
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sizeof database type. </summary>
        ///
        /// <param name="theType">  Type of the. </param>
        ///
        /// <returns>  Size of the Datatype . </returns>
        ///-------------------------------------------------------------------------------------------------

        public static int SizeofDbType(DbType theType)
        {
            int theSize = 0;
            switch (theType)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.Binary:
                case DbType.Object:
                case DbType.Guid:
                case DbType.String:
                case DbType.StringFixedLength:
                case DbType.VarNumeric:
                case DbType.Xml:
                    {
                        theSize = -1;
                        break;
                    }
                case DbType.Boolean:
                case DbType.SByte:
                case DbType.Byte:
                    {
                        theSize = sizeof(bool);
                        break;
                    }
                case DbType.Currency:
                case DbType.Decimal:
                    {
                        theSize = sizeof(decimal);
                        break;
                    }
                case DbType.Time:
                case DbType.Date:
                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                    {
                        theSize = 8;  // Not sure if this is right
                        break;
                    }
                case DbType.Double:
                    {
                        theSize = sizeof(double);
                        break;
                    }
                case DbType.UInt16:
                case DbType.Int16:
                    {
                        theSize = sizeof(Int16);
                        break;
                    }
                case DbType.UInt32:
                case DbType.Int32:
                    {
                        theSize = sizeof(Int32);
                        break;
                    }
                case DbType.Int64:
                case DbType.UInt64:
                    {
                        theSize = sizeof(Int64);
                        break;
                    }
                case DbType.Single:
                    {
                        theSize = sizeof(Single);
                        break;
                    }

            }
            return theSize;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Select limited rows. </summary>
        ///
        /// <param name="Server">       The server. </param>
        /// <param name="Fields">       The fields. </param>
        /// <param name="Tablename">    The tablename. </param>
        /// <param name="RowNumber">    The row number. </param>
        ///<remarks>Returns a SQL statement to select a limited number of rows based in the SQLServer type</remarks>
        /// <returns>  SQL Select statement . </returns>
        ///-------------------------------------------------------------------------------------------------

        public static string SelectLimitedRows(SQLServer Server, string Fields, string Tablename, int RowNumber)
        {
            switch (Server)
            {
                case SQLServer.stAccess:
                    return ("SELECT TOP " + RowNumber.ToString() + " " + Fields + " FROM " + Tablename + ";");
                case SQLServer.stPostgreSQL:
                case SQLServer.stMySQL:
                    return ("SELECT " + Fields + " FROM " + Tablename + " LIMIT " + RowNumber.ToString() + ";");
                default:
                    return ("SELECT " + Fields + " FROM " + Tablename + ";");
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   SQL string quote. </summary>
        ///
        /// <param name="ServerType">  returns a Quote char based on type of the server. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public static char SQLStringQuote(SQLServer ServerType)
        {
            char quote = (char)34;
            switch (ServerType)
            {
                case SQLServer.stAccess:
                case SQLServer.stMySQL:
                    quote = (char)34;
                    break;
                case SQLServer.stPostgreSQL:
                    quote = (char)39;
                    break;
            }
            return quote;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> OLE database connection string for SQL server. </summary>
        ///
        /// <param name="Server"> The server. </param>
        /// <remarks> String.Format, with parameters as follows
        ///           Server = {0}
        ///           Port = {1}
        ///           Database SOurce = {2}
        ///           User Id = {3}
        ///           Passwrod = {4}
        ///           All other parameters (delimted by ";" = {5}
        ///           </remarks>
        /// <returns> a connection string to be used with String.Format</returns>
        ///-------------------------------------------------------------------------------------------------
        public static string UniDbConnectionStringForSQLServer(SQLServer Server)
        {
            string temp = "";
            switch (Server)
            {
                case SQLServer.stAccess:
                    {
                        temp = "Provider=Microsoft.JET.OLEDB.4.0; data source={2};User Id={3};Password={4};";
                        break;
                    }
                case SQLServer.stMySQL:
                    {
                        temp = "Server={0};Database={2};UId={3};Password={4};{5};";
                        break;
                    }
                case SQLServer.stPostgreSQL:
                    {
                        //                        temp = "Provider=PostgreSQL OLE DB Provider;Data Source=LocalHost;location=;User ID=rayquay;password=object";

                        temp = "Server={1};Port={1};User Id={3};Password={4};Database={2};{5};";
                        break;
                    }
            }
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a list of database names. </summary>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="Server">           The server. </param>
        /// <param name="ServerReference">  The server Location (IP address or LocalHots). </param>
        /// <param name="UserID">           Identifier for the user. </param>
        /// <param name="Password">         The password. </param>
        ///
        /// <returns>   The database names. </returns>
        ///-------------------------------------------------------------------------------------------------

        static public List<string> GetDatabaseNames(SQLServer Server, string ServerLocation, string UserID, string Password)
        {
            List<string> DbNameList = new List<string>();
            try
            {
                string connectStr = UniDbConnectionStringForSQLServer(Server);
                UniDbConnection Fdb = new UniDbConnection(Server, ServerLocation, "", UserID, Password, "");
                switch (Server)
                {
                    case SQLServer.stAccess:
                        break;
                    case SQLServer.stPostgreSQL:
                        Fdb.Open();
                        UniDataAdapter FDbAdapter = new UniDataAdapter("SELECT datname FROM pg_database;", Fdb);
                        DataTable DT = new DataTable();
                        FDbAdapter.Fill(DT);
                        foreach (DataRow DR in DT.Rows)
                        {
                            DbNameList.Add(DR[0].ToString());
                        }
                        Fdb.Close();
                        break;
                    case SQLServer.stMySQL:
                        break;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Unable to list Databases: " + e.Message);
            }
            return DbNameList;
        }

        /// <summary>
        /// Creates a double value that provides a unique Seed for ID.  Based on Clock Ticks since 1/1/2013
        /// </summary>
        /// <returns>Unique Double Value</returns>
        public static Double CreatePrimaryIDSeed()
        {
            double TheKey = 0;

            long SpanTime = (new TimeSpan(0, 0, 1)).Ticks;
            DateTime NowTime = new DateTime();
            NowTime = DateTime.Now;
            DateTime keyDT = new DateTime(2013, 1, 1);
            long TheBigTicks = NowTime.Ticks - keyDT.Ticks;
            TheKey = TheBigTicks;
            //TheKey = Convert.ToDouble(TheBigTicks / SpanTime);
            return TheKey;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Extracts the database name DbConnection is connected to. </summary>
        ///
        /// <remarks>  DbConnection must have a connection string assigned </remarks>
        ///
        /// <param name="DbConnection"> The database connection. </param>
        ///
        /// <returns>   The extracted database name. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static string ExtractDatabaseName(UniDbConnection DbConnection)
        {
            // extract the database name from DbConnection
            const string DATASOURCESTR = "DATA SOURCE=";
            string databaseName = "";
            string ConString = DbConnection.ConnectionString.ToUpper();
            int index = ConString.IndexOf(DATASOURCESTR) + DATASOURCESTR.Length;
            if (index > 0)
            {
                string temp = ConString.Substring(index, ConString.Length - index);
                index = temp.IndexOf(";");
                if (index > 0)
                {
                    databaseName = temp.Substring(0, index);
                }

            }
            return databaseName;
        }


        // SQL SUpport Methods

        #region SQL Support
        // SQL Support
        /// <summary> The sqlchar. </summary>
        /// <remarks> String up to 355 chars</remarks>
        public const int siSQLCHAR = 1;  // string upto 255 char
        /// <summary> The sqltext. </summary>
        /// <remarks> String >255 chars, varies among servers from 65,000 max chars to 2 gb</remarks>
        public const int siSQLTEXT = 2;  // string >255 char
        /// <summary> The sqldecimal. </summary>
        /// <remarks>Use for System.Decimal at 128 bit, some server, such as jet Access on have 64 bits</remarks>
        public const int siSQLDECIMAL = 3; // System.Decimal 128 bit
        /// <summary> The sqldouble. </summary>
        /// <remarks> doubleing point System.Single 32 bit</remarks>
        public const int siSQLdouble = 4; // System.Single 32 bit
        /// <summary> The sqldouble. </summary>
        /// <remarks>doubleing point System.Double 64 bit</remarks>
        public const int siSQLDOUBLE = 5; // System.Double 64 bit
        /// <summary> The sqlint16. </summary>
        /// <remarks> 16 bit integer</remarks>
        public const int siSQLINT16 = 6;
        /// <summary> The sqlint32. </summary>
        /// <remarks> 32 bit integer</remarks>
        public const int siSQLINT32 = 7;
        /// <summary> The siSQLINT64. </summary>
        /// <remarks> 64 bit integer, not supported by all servers, for example JET Access has no such type</remarks>
        public const int siSQLINT64 = 8;
        /// <summary> The sqldate. </summary>
        /// <renarks> Just the date, some servers do not support, in that case DATETIME is used</renarks>
        public const int siSQLDATE = 9;
        /// <summary> The sqltime. </summary>
        /// <renarks> Just the time, some servers do not support, in that case DATETIME is used</renarks>
        public const int siSQLTIME = 10;
        /// <summary> The sqlbyte. </summary>
        /// <remarks> an 8 bit byte, not all servers support, in this case siSQLINT16 is used</remarks>
        public const int siSQLBYTE = 11;
        /// <summary> The sqlbool. </summary>
        public const int siSQLBOOL = 12;
        /// <summary> The sqldatetime. </summary>
        /// <remarks> Both Data and Time</remarks>
        public const int siSQLDATETIME = 13;

        const int SQLTYPENUMBER = 14;

        const string DATATYPENOTSUPPORTED = "NOSUPPORT";

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Values that represent SQLServer. </summary>
        ///
        /// <remarks> SQLServer.stAccess, 
        ///                    SQLServer.MySQL, 
        ///                    SQLServer.PostgreSQL </remarks>
        ///-------------------------------------------------------------------------------------------------

        //public  enum SQLServer { 
        //    /// <summary> JET Access </summary>
        //    stAccess, 
        //    /// <summary> MySQL. </summary>
        //    stMySQL, 
        //    /// <summary> PostGreSQL. </summary>
        //    stPostgreSQL,
        //    /// <summary> Microsft SQL Server. </summary>
        //    stMSSQL, 
        //    /// <summary> Oracle Server. </summary>
        //    /// <remarks>Not Yet Supported</remarks>
        //    stOracle, 
        //    /// <summary> IBM Informix Server. </summary>
        //    /// <remarks>Not Yet Supported</remarks>
        //    stInformix, 
        //    /// <summary> DB2 Server. </summary>
        //    /// <remarks>Not Yet Supported</remarks>
        //    stDB2
        //};

        ///-------------------------------------------------------------------------------------------------
        /// <summary> OLE database connection string for SQL server. </summary>
        ///
        /// <param name="Server"> The server. </param>
        ///
        /// <returns> a connection string with source marked as "source=;". </returns>
        ///-------------------------------------------------------------------------------------------------

        //internal string FSQLServer = "";
        //internal string FSQLID = "";
        //internal string FSQLPassword = "";

        //        public static string UniDbConnectionStringForSQLServer(SQLServer Server)
        //        {
        //            string temp = "";
        //            switch (Server)
        //            {
        //                case SQLServer.stAccess:
        //                    {
        //                        temp = "Provider=Microsoft.JET.OLEDB.4.0; data source=;";//{0};User Id={1};Password={2};";
        //                        //temp = "Provider=Microsoft.JET.OLEDB.4.0; data source=;User Id=admin;Password=;";
        //                        break;
        //                    }
        //                case SQLServer.stMySQL:
        //                    {
        //                        temp = "Server = myServerAddress; Database = myDataBase; Uid = myUsername; Pwd = myPassword;
        //                        break;
        //                    }
        //                case SQLServer.stPostgreSQL:
        //                    {
        ////                        temp = "Provider=PostgreSQL OLE DB Provider;Data Source=LocalHost;location=;User ID=rayquay;password=object";

        //                        temp = "Provider=PostgreSQL OLE DB Provider;Data Source={3};location={0};User ID={1};password={2}";
        //                        break;
        //                    }
        //            }
        //            return temp;
        //        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary> Inserts a filename into a connection string which includes "source=;" in the string. </summary>
        /// <param name="filename">               Filename of the database file. </param>
        /// <param name="SourceConnectionString"> Source connection string. </param>
        ///
        /// <returns> . </returns>
        ///-------------------------------------------------------------------------------------------------

        public static string InsertFilenameIntoConnection(string filename, string SourceConnectionString)
        {
            string temp = "";
            int index = SourceConnectionString.IndexOf("source=;");
            if (index > 0)
            {
                string front = SourceConnectionString.Substring(0, index) + "source=";
                string back = SourceConnectionString.Substring(index + 7, SourceConnectionString.Length - (index + 7));
                temp = front + filename + back;
            }
            return temp;
        }
        // ACCESS CAN NOT STORE INTEGERS LARGER THAN 32 bit.  THIS USES DOUBLE FOR INT64 (WHICH CAN HAVE 19 DIGITS), BUT PRECISION IS LIMITED TO THE FIRST 17 DIGITS
        static string[] SQL_ACCESS = new string[SQLTYPENUMBER] { "ACCESS", "VARCHAR", "MEMO", "CURRENCY", "SINGLE", "DOUBLE", "INTEGER", "LONG", "DOUBLE", "DATE/TIME", "DATE/TIME", "BYTE", "YES//NO", "DATE/TIME" };
        static string[] SQL_POSTGRE = new string[SQLTYPENUMBER] { "POSTGRE", "VARCHAR", "TEXT", "DECIMAL", "REAL", "DOUBLE PRECISION", "TINYINT", "INTEGER", "BIGINT", "DATE", "TIME", "TINYINT", "BOOLEAN", "DATETIME" };
        static string[] SQL_MYSQL = new string[SQLTYPENUMBER] { "MYSQL", "VARCHAR", "MEDIUMTEXT", "DECIMAL", "double", "DOUBLE", "SMALLINT", "INT", "BIGINT", "DATE", "TIME", "TINYINT", "BOOLEAN", "DATETIME" };

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets a SQL type. </summary>
        ///
        /// <param name="index">  Zero-based index of the. </param>
        /// <param name="Server"> The server. </param>
        ///
        /// <returns> The SQL type. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static string GetSQLType(int index, SQLServer Server)
        {
            string temp = "";
            if ((index >= 0) && (index < SQLTYPENUMBER))
            {
                switch (Server)
                {
                    case SQLServer.stAccess:
                        temp = SQL_ACCESS[index];
                        break;
                    case SQLServer.stMySQL:
                        temp = SQL_MYSQL[index];
                        break;
                    case SQLServer.stPostgreSQL:
                        temp = SQL_POSTGRE[index];
                        break;
                }
            }
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> SQL limit rows for select. </summary>
        ///
        /// <remarks> Diffiferent servers using different Syntax, thus server type is required. </remarks>
        ///
        /// <param name="Server">       The server. </param>
        /// <param name="NumberOfRows"> Number of rows from beginning to return in select. </param>
        ///
        /// <returns> . </returns>
        ///-------------------------------------------------------------------------------------------------

        public static string SQLLimitRowsForSelect(SQLServer Server, int NumberOfRows)
        {
            string temp = "";
            switch (Server)
            {
                case SQLServer.stAccess:
                    //case SQLServer.stMSSQL:
                    {
                        temp = "TOP " + NumberOfRows.ToString();
                        break;
                    }
                case SQLServer.stMySQL:
                case SQLServer.stPostgreSQL:
                    {
                        temp = "LIMIT " + NumberOfRows.ToString();
                        break;
                    }
            }
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Values that represent SQLConstraints </summary>
        ///
        /// <remarks> Ray, 1/24/2013. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public enum SQLConstraint
        {
            /// <summary> Column can not contain a null value. </summary>
            scNotNull,
            /// <summary> Column values most be unique across Rows. </summary>
            scUnique,
            /// <summary> PrimaryKey. </summary>
            /// <remarks> Unique and NotNull</remarks>
            scPrimaryKey,
            /// <summary> Column will be an autincrement column, no need to assign data. </summary>
            scAutoIncrement
        };

        ///-------------------------------------------------------------------------------------------------
        /// <summary> SQL constraint define. </summary>
        ///
        /// <remarks> Generates a string that can be used as a SQL contraint define, based on the server
        ///     specified.
        ///     </remarks>
        ///
        /// <param name="Server">            The server. </param>
        /// <param name="Constaint">         The constaint. </param>
        /// <param name="ColumnName">        The column nane. </param>
        /// <param name="value">             true to value. </param>
        /// <param name="ClosingConstraint"> The closing constraint. </param>
        ///
        /// <returns> . </returns>
        ///-------------------------------------------------------------------------------------------------

        public static string SQLConstraintDefine(SQLServer Server, SQLConstraint Constaint, string ColumnName, bool value, ref string ClosingConstraint)
        {
            // This returns a string that adds a constraint modifier  to the field definition, if that is the appropriate SQLServer Syntax
            // Other wise this adds a constraint to a string that closes the Table Column Define list.  Some SQLServer Syntax uses the 
            // CONSTRAINT key word to create constraints on fields after the fields have been defined.  This CONSTAINT definition is 
            // added to the ClosingConstraint string passed to the routuine.
            string ConstraintStr = "";
            string ClosingConstraintStr = "";
            switch (Constaint)
            {
                // NOT NULL
                case SQLConstraint.scNotNull:
                    {
                        // for scNotNull if value is true, then NOT NULL constraint is added
                        if (value)
                        {
                            switch (Server)
                            {
                                case SQLServer.stAccess:
                                    {
                                        ConstraintStr = " NOT NULL";
                                        break;
                                    }
                                case SQLServer.stMySQL:
                                    {
                                        ConstraintStr = " NOT NULL";
                                        break;
                                    }
                                case SQLServer.stPostgreSQL:
                                    {
                                        ConstraintStr = " NOT NULL";
                                        break;
                                    }
                            }
                        }
                        break;
                    }
                // PRIMARY KEY
                case SQLConstraint.scPrimaryKey:
                    {
                        if (value)
                        // for scPrimaryKey if value is true, then the field is made the primary field
                        // NOTE: If you set more than one field as a PrimaryKey thyen this will create a SQL error
                        // 
                        {
                            switch (Server)
                            {
                                case SQLServer.stAccess:
                                    {
                                        ConstraintStr = " PRIMARY KEY";
                                        break;
                                    }
                                case SQLServer.stMySQL:
                                    {
                                        ClosingConstraintStr = " PRIMARY KEY (" + ColumnName + ")";
                                        break;
                                    }
                                case SQLServer.stPostgreSQL:
                                    {
                                        ConstraintStr = " PRIMARY KEY";
                                        break;
                                    }
                            }
                        }
                        break;
                    }
                // UNIQUE
                case SQLConstraint.scUnique:
                    {
                        if (value)
                        // for scUnique if value is true, then the field is made the primary field
                        {
                            switch (Server)
                            {
                                case SQLServer.stAccess:
                                    {
                                        ConstraintStr = " UNIQUE";
                                        break;
                                    }
                                case SQLServer.stMySQL:
                                    {
                                        ClosingConstraintStr = " UNIQUE (" + ColumnName + ")";
                                        break;
                                    }
                                case SQLServer.stPostgreSQL:
                                    {
                                        ConstraintStr = " UNIQUE";
                                        break;
                                    }
                            }
                        }
                        break;
                    }
                case SQLConstraint.scAutoIncrement:
                    {
                        if (value)
                        // for scUnique if value is true, then the field is made the primary field
                        {
                            switch (Server)
                            {
                                case SQLServer.stAccess:
                                    {
                                        ConstraintStr = " COUNTER";
                                        break;
                                    }
                                case SQLServer.stPostgreSQL:
                                    {
                                        ConstraintStr = " SERIAL";
                                        break;
                                    }
                                case SQLServer.stMySQL:
                                    {
                                        ConstraintStr = " AUTO_INCREMENT";
                                        break;
                                    }
                            }
                        }
                        break;
                    }

            }
            if (ClosingConstraintStr != "")
            {
                if (ClosingConstraint != "")
                    ClosingConstraint += " , ";
                ClosingConstraint += ClosingConstraintStr;
            }
            return ConstraintStr;

        }

        /////-------------------------------------------------------------------------------------------------
        ///// <summary>   Net type to oledb type. </summary>
        /////
        ///// <remarks>   3/10/2013. </remarks>
        /////
        ///// <param name="DataType"> Net Type of the data. </param>
        /////
        ///// <returns>   . </returns>
        /////-------------------------------------------------------------------------------------------------

        //public static OleDbType ConvertNetTypeToOledbType(Type DataType)
        //{
        //    OleDbType dbDT = OleDbType.Empty;
        //    if (DataType == System.Type.GetType("System.Int32"))
        //    {
        //        dbDT = OleDbType.Integer;
        //    }
        //    else
        //        //        UnsignedInt A 32-bit unsigned integer (DBTYPE_UI4). This maps to UInt32. 
        //        if (DataType == System.Type.GetType("System.UInt32"))
        //        {
        //            dbDT = OleDbType.UnsignedInt;

        //        }
        //        else
        //            //       SmallInt A 16-bit signed integer (DBTYPE_I2). This maps to Int16. 
        //            if (DataType == System.Type.GetType("System.Int16"))
        //            {
        //                dbDT = OleDbType.SmallInt;
        //            }
        //            else
        //                //        UnsignedSmallInt A 16-bit unsigned integer (DBTYPE_UI2). This maps to UInt16. 
        //                if (DataType == System.Type.GetType("System.UInt16"))
        //                {
        //                    dbDT = OleDbType.UnsignedSmallInt;

        //                }
        //                else
        //                    if (DataType == System.Type.GetType("System.Int64"))
        //                    {
        //                        dbDT = OleDbType.BigInt;
        //                    }
        //                    else
        //                        //        UnsignedBigInt A 64-bit unsigned integer (DBTYPE_UI8). This maps to UInt64. 
        //                        if (DataType == System.Type.GetType("System.UInt64"))
        //                        {
        //                            dbDT = OleDbType.UnsignedBigInt;
        //                        }
        //                        else
        //                            //        Double A doubleing point number within the range of -1.79E +308 through 1.79E +308 (DBTYPE_R8). This maps to Double. 
        //                            if (DataType == System.Type.GetType("System.Double"))
        //                            {
        //                                dbDT = OleDbType.Double;
        //                            }
        //                            else
        //                                //        Char A character string (DBTYPE_STR). This maps to String. 
        //                                //        VarChar A variable-length stream of non-Unicode characters (OleDbParameter only). This maps to String. 
        //                                if (DataType == System.Type.GetType("System.String"))
        //                                {
        //                                    // using varchar here becuase char type often limited to 255 chars
        //                                    dbDT = OleDbType.VarChar;
        //                                }
        //                                else
        //                                    //  Date Date data, stored as a double (DBTYPE_DATE). The whole portion is the number of days since December 30, 1899, while the fractional portion is a fraction of a day. This maps to DateTime. 
        //                                    //  DBDate Date data in the format yyyymmdd (DBTYPE_DBDATE). This maps to DateTime. 
        //                                    //  DBTime Time data in the format hhmmss (DBTYPE_DBTIME). This maps to TimeSpan. 
        //                                    //  DBTimeStamp Data and time data in the format yyyymmddhhmmss (DBTYPE_DBTIMESTAMP). This maps to DateTime. 
        //                                    //        Filetime A 64-bit unsigned integer representing the number of 100-nanosecond intervals since January 1, 1601 (DBTYPE_FILETIME). This maps to DateTime. 
        //                                    if (DataType == System.Type.GetType("System.DateTime"))
        //                                    {
        //                                        // using DBTimeSTamp to conve full info
        //                                        dbDT = OleDbType.DBTimeStamp;
        //                                    }
        //                                    else
        //                                        // Boolean A Boolean value (DBTYPE_BOOL). This maps to Boolean. 
        //                                        if (DataType == System.Type.GetType("System.Boolean"))
        //                                        {
        //                                            dbDT = OleDbType.Boolean;
        //                                        }
        //                                        else
        //                                            //  Decimal A fixed precision and scale numeric value between -1038 -1 and 10 38 -1 (DBTYPE_DECIMAL). This maps to Decimal. 
        //                                            //  Currency A currency value ranging from -263 (or -922,337,203,685,477.5808) to 2 63 -1 (or +922,337,203,685,477.5807) with an accuracy to a ten-thousandth of a currency unit (DBTYPE_CY). This maps to Decimal. 
        //                                            //  Numeric An exact numeric value with a fixed precision and scale (DBTYPE_NUMERIC). This maps to Decimal. 
        //                                            if (DataType == System.Type.GetType("System.Decimal"))
        //                                            {
        //                                            }
        //                                            else
        //                                                // Single A doubleing point number within the range of -3.40E +38 through 3.40E +38 (DBTYPE_R4). This maps to Single. 
        //                                                if (DataType == System.Type.GetType("System.Single"))
        //                                                {
        //                                                    dbDT = OleDbType.Single;
        //                                                }
        //                                                else
        //                                                    //        UnsignedTinyInt A 8-bit unsigned integer (DBTYPE_UI1). This maps to Byte. 
        //                                                    if (DataType == System.Type.GetType("System.Byte"))
        //                                                    {
        //                                                        dbDT = OleDbType.UnsignedTinyInt;
        //                                                    }
        //                                                    else
        //                                                        //  TinyInt A 8-bit signed integer (DBTYPE_I1). This maps to SByte. 
        //                                                        if (DataType == System.Type.GetType("System.SByte"))
        //                                                        {
        //                                                            dbDT = OleDbType.TinyInt;
        //                                                        }

        //    return dbDT;
        //}
        /*
        Other OleDbTypes
         
        Binary A stream of binary data (DBTYPE_BYTES). This maps to an Array of type Byte. 
        BSTR A null-terminated character string of Unicode characters (DBTYPE_BSTR). This maps to String. 
        Empty No value (DBTYPE_EMPTY). 
        Error A 32-bit error code (DBTYPE_ERROR). This maps to Exception. 
        Guid A globally unique identifier (or GUID) (DBTYPE_GUID). This maps to Guid. 
        IDispatch A pointer to an IDispatch interface (DBTYPE_IDISPATCH). This maps to Object. 
        Note   This data type is not currently supported by ADO.NET. Usage may cause unpredictable results.
        IUnknown A pointer to an IUnknown interface (DBTYPE_UNKNOWN). This maps to Object. 
        Note   This data type is not currently supported by ADO.NET. Usage may cause unpredictable results.
        LongVarBinary A long binary value (OleDbParameter only). This maps to an Array of type Byte. 
        LongVarChar A long string value (OleDbParameter only). This maps to String. 
        LongVarWChar A long null-terminated Unicode string value (OleDbParameter only). This maps to String. 
        PropVariant An automation PROPVARIANT (DBTYPE_PROP_VARIANT). This maps to Object. 
        VarBinary A variable-length stream of binary data (OleDbParameter only). This maps to an Array of type Byte. 
        Variant A special data type that can contain numeric, string, binary, or date data, as well as the special values Empty and Null (DBTYPE_VARIANT). This type is assumed if no other is specified. This maps to Object. 
        VarNumeric A variable-length numeric value (OleDbParameter only). This maps to Decimal. 
        VarWChar A variable-length, null-terminated stream of Unicode characters (OleDbParameter only). This maps to String. 
        WChar A null-terminated stream of Unicode characters (DBTYPE_WSTR). This maps to String. 
 
        Other Net Types    
        
        Char
        Guid
        TimeSpan



         * */
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   SQL data define. </summary>
        /// <remarks> creates a string for a column definition based on a datatype and a server</remarks>
        /// <param name="DT">       The datatype. </param>
        /// <param name="Server">   The server. </param>
        /// <param name="Width">    The width. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public static string SQLDataDefine(Type DT, SQLServer Server, int Width)
        {
            string temp = "";
            if (DT == System.Type.GetType("System.String"))
            {
                if (Width < 256)
                {
                    temp = GetSQLType(siSQLCHAR, Server) + "(" + Width.ToString() + ")";
                }
                else
                {
                    temp = GetSQLType(siSQLTEXT, Server);
                }
            }
            else
                if (DT == System.Type.GetType("System.Int16"))
                {
                    temp = GetSQLType(siSQLINT16, Server);
                }
                else
                    if ((DT == System.Type.GetType("System.Int32")) || (DT == System.Type.GetType("System.Int")))
                    {
                        temp = GetSQLType(siSQLINT32, Server);
                    }
                    else
                        if (DT == System.Type.GetType("System.Int64"))
                        {
                            temp = GetSQLType(siSQLINT64, Server);
                        }
                        else
                            if (DT == System.Type.GetType("System.Decimal"))
                            {
                                temp = GetSQLType(siSQLDECIMAL, Server);
                            }
                            else
                                if (DT == System.Type.GetType("System.Single"))
                                {
                                    temp = GetSQLType(siSQLdouble, Server);
                                }
                                else
                                    if (DT == System.Type.GetType("System.Double"))
                                    {
                                        temp = GetSQLType(siSQLDOUBLE, Server);
                                    }
                                    else
                                    if (DT == System.Type.GetType("System.Float"))
                                    {
                                        temp = GetSQLType(siSQLDOUBLE, Server);
                                    }

                                    else
                                        if (DT == System.Type.GetType("System.DateTime"))
                                        {
                                            temp = GetSQLType(siSQLDATE, Server);
                                        }



            return temp;
        }

        //----------------------------
        public const char DoubleQuote = (char)34;
        public const char SingleQuote = (char)39;
        public const string Comma = " , ";
        public const char QMark = '?';
        //-----------------------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary> SQL data string. </summary>
        /// <param name="Server"> The server. </param>
        /// <param name="value">   data value. </param>
        /// <returns> string of data to SQLServer specification. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static string SQLColumnDataDouble(SQLServer Server, double value)
        {
            string temp = "";
            switch (Server)
            {
                case SQLServer.stAccess:
                case SQLServer.stMySQL:
                case SQLServer.stPostgreSQL:
                    {
                        temp = value.ToString();
                        break;
                    }
            }
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> SQL data string. </summary>
        /// <param name="Server"> The server. </param>
        /// <param name="value">  data value. </param>
        /// <returns> string of data to SQLServer specification. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static string SQLColumnDataInteger(SQLServer Server, Int64 value)
        {
            string temp = "";
            switch (Server)
            {
                case SQLServer.stAccess:
                case SQLServer.stMySQL:
                case SQLServer.stPostgreSQL:
                    {
                        temp = value.ToString();
                        break;
                    }
            }
            return temp;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> SQL data string. </summary>
        /// <param name="Server"> The server. </param>
        /// <param name="value">  data value. </param>
        /// <returns> string of data to SQLServer specification. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static string SQLColumnDataString(SQLServer Server, string value)
        {
            string temp = "";
            switch (Server)
            {
                case SQLServer.stAccess:
                case SQLServer.stMySQL:
                case SQLServer.stPostgreSQL:
                    {
                        temp = SingleQuote + value.ToString() + SingleQuote;
                        break;
                    }
            }
            return temp;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> SQL data string. </summary>
        /// <param name="Server"> The server. </param>
        /// <param name="value">  data value. </param>
        /// <returns> string of data to SQLServer specification. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static string SQLColumnDataDate(SQLServer Server, DateTime value)
        {
            string temp = "";
            switch (Server)
            {
                case SQLServer.stAccess:
                case SQLServer.stMySQL:
                case SQLServer.stPostgreSQL:
                    {
                        temp = SingleQuote + value.Date.ToString() + SingleQuote;
                        break;
                    }
            }
            return temp;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> SQL data string. </summary>
        /// <param name="Server"> The server. </param>
        /// <param name="value">  data value. </param>
        /// <returns> string of data to SQLServer specification. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static string SQLColumnDataTime(SQLServer Server, DateTime value)
        {
            string temp = "";
            switch (Server)
            {
                case SQLServer.stAccess:
                case SQLServer.stMySQL:
                case SQLServer.stPostgreSQL:
                    {
                        temp = SingleQuote + value.TimeOfDay.ToString() + SingleQuote;
                        break;
                    }
            }
            return temp;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> SQL data string. </summary>
        /// <param name="Server"> The server. </param>
        /// <param name="value">  data value. </param>
        /// <returns> string of data to SQLServer specification. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static string SQLColumnDataDateTime(SQLServer Server, DateTime value)
        {
            string temp = "";
            switch (Server)
            {
                case SQLServer.stAccess:
                case SQLServer.stMySQL:
                case SQLServer.stPostgreSQL:
                    {
                        temp = SingleQuote + value.ToString() + SingleQuote;
                        break;
                    }
            }
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> SQL data string. </summary>
        /// <param name="Server"> The server. </param>
        /// <param name="value">  data value. </param>
        /// <returns> string of data to SQLServer specification. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static string SQLColumnDataDecimal(SQLServer Server, Decimal value)
        {
            string temp = "";
            switch (Server)
            {
                case SQLServer.stAccess:
                case SQLServer.stMySQL:
                case SQLServer.stPostgreSQL:
                    {
                        temp = value.ToString();
                        break;
                    }
            }
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> SQL String for data. </summary>
        /// <remarks> value can be one of the follow data types, int, int16, int32, int64, single, double, decimal, string, DataTime</remarks>
        /// <param name="Server"> The server. </param>
        /// <param name="value">  A net data object. </param>
        /// <returns> Data in a String to SQL specificiations. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static string SQLColumnData(SQLServer Server, Object value)
        {
            string temp = "";
            Type DT = value.GetType();

            if (DT == System.Type.GetType("System.String"))
            {
                temp = SQLColumnDataString(Server, (value as string));
            }
            else
                if (DT == System.Type.GetType("System.Int16"))
                {
                    temp = SQLColumnDataInteger(Server, (Int16)value);
                }
                else
                    if ((DT == System.Type.GetType("System.Int32")) || (DT == System.Type.GetType("System.Int")))
                    {
                        temp = SQLColumnDataInteger(Server, (Int32)value);
                    }
                    else
                        if (DT == System.Type.GetType("System.Int64"))
                        {
                            temp = SQLColumnDataInteger(Server, (Int64)value);
                        }
                        else
                            if (DT == System.Type.GetType("System.Decimal"))
                            {
                                temp = SQLColumnDataDecimal(Server, (Decimal)value);
                            }
                            else
                                if (DT == System.Type.GetType("System.Single"))
                                {
                                    temp = SQLColumnDataDouble(Server, (Single)value);
                                }
                                else
                                    if (DT == System.Type.GetType("System.Double"))
                                    {
                                        temp = SQLColumnDataDouble(Server, (Double)value);
                                    }

                                    else
                                    if (DT == System.Type.GetType("System.Float"))
                                    {
                                        temp = SQLColumnDataDouble(Server, (Double)value);
                                    }

                                    else
                                        if (DT == System.Type.GetType("System.DateTime"))
                                        {
                                            temp = SQLColumnDataDateTime(Server, (DateTime)value);
                                        }


            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Buikds SQL insert Statement. </summary>
        ///
        /// <param name="Server">      The server. </param>
        /// <param name="Tablename">   The tablename. </param>
        /// <param name="ColumnNames"> List of names of the columns. </param>
        /// <param name="ColumnData">  Data for columns as string. </param>
        ///
        /// <returns> SQL INSERT Statement as string. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static string SQLInsertDefine(SQLServer Server, string Tablename, List<string> ColumnNames, List<string> ColumnData)
        {
            string temp = "";
            // build first part of insert
            temp = "INSERT INTO [" + Tablename + "] ( " + ColumnNames[0].ToString();
            // add the rest of the columns
            if (ColumnNames.Count > 1)
            {
                for (int i = 1; i < ColumnNames.Count; i++)
                {
                    temp += Comma + ColumnNames[i].ToString();
                }
            }
            // close off column define
            temp += " )  VALUES ( " + ColumnData[0].ToString();
            // add rest of data
            if (ColumnData.Count > 1)
            {
                for (int i = 1; i < ColumnData.Count; i++)
                {
                    temp += Comma + ColumnData[i].ToString();
                }
            }
            // close out
            temp += " ) ";
            return temp;
        }

        #endregion





        // Text 

        #region Text File Data Support
        // Test File Data Support
        // 

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Values that represent TxtFieldMode. </summary>
        ///
        /// <remarks> Not Sure what this is duplicating DataFormat Is used for ConvertDbTableToText() routine. </remarks>
        ///-------------------------------------------------------------------------------------------------

        //public enum TxtFieldMode
        //{
        //     <summary> Comma Delimited </summary>
        //    cmCommaDelimited,
        //     <summary> Fixed Field. </summary>
        //    cmFixedFieldWidth,
        //     <summary> Space Delimited. </summary>
        //    cmSpaceDelimited
        //};


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Reads all lines from a text file and returns them in a  List. </summary>
        /// <param name="Filename">     Filename of the database file. </param>
        /// <param name="errString">    [in,out] If there is an exception this returns Exception.Message. </param>
        ///
        /// <returns>   The lines from text file. </returns>
        ///-------------------------------------------------------------------------------------------------

        static public List<string> ReadLinesFromTextFile(string Filename, ref string errString) // Including path
        {
            List<String> Lines = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(Filename))
                {
                    String line;
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        Lines.Add(line);
                    }
                }
                return Lines;
            }
            catch (Exception e)
            {
                errString = e.Message;
                return Lines;
            }
        }

        private static void CountDelimiters(string data, ref int aQuoteCnt, ref int aCommaCnt, ref int aTabCnt, ref int aSpaceCnt)
        {
            // test for commas, quotes, tabs, and spaces
            int CommaCnt = 0;
            int QuoteCnt = 0;
            int TabCnt = 0;
            int SpaceCnt = 0;
            bool inQuote = false;
            // ok loop through the string and count
            if (data != null)
            {
                foreach (Char c in data)
                {
                    // Quotes
                    if (c == DoubleQuote)
                    {
                        QuoteCnt++;
                        // toogle of inside double quote
                        inQuote = !inQuote;
                    }
                    else
                        // Commas
                        if (c == ',')
                        {
                            // only count this if not in a quote
                            if (!inQuote)
                            {
                                CommaCnt++;
                            }
                        }
                        else
                            // Tabs
                            if (c == '\t')
                            {
                                // only count this if not in a quote
                                if (!inQuote)
                                {
                                    TabCnt++;
                                }
                            }
                            else
                                // SPaces
                                if (c == ' ')
                                {
                                    // only count this if not in a quote
                                    if (!inQuote)
                                    {
                                        SpaceCnt++;
                                    }
                                }
                }
            }
            aQuoteCnt = QuoteCnt;
            aCommaCnt = CommaCnt;
            aTabCnt = TabCnt;
            aSpaceCnt = SpaceCnt;
        }
        public static DataFormat GuessDataFormat(string filename)
        {
            DataFormat result = DataFormat.Unknown;
            try
            {
                string Line1 = "";
                string Line2 = "";
                using (StreamReader SR = new StreamReader(filename))
                {
                    // First line may be field defines, skip that if their is more than one line otehr wise use the first line
                    Line1 = SR.ReadLine();
                    Line2 = SR.ReadLine();
                    string teststr = null;
                    if (Line2 != null)
                    {
                        teststr = Line2;
                    }
                    else
                    {
                        if (Line1 != null)
                        {
                            teststr = Line1;
                        }
                    }
                    if (teststr != null)
                    {
                        // test for commas, quotes, tabs, and spaces
                        int CommaCnt = 0;
                        int QuoteCnt = 0;
                        int TabCnt = 0;
                        int SpaceCnt = 0;
                        int TestCommaCnt = 0;
                        int TestQuoteCnt = 0;
                        int TestTabCnt = 0;
                        int TestSpaceCnt = 0;
                        CountDelimiters(teststr, ref QuoteCnt, ref CommaCnt, ref TabCnt, ref SpaceCnt);
                        bool QuotesEven = ((QuoteCnt % 2) == 0);
                        int MaxLinetestCnt = 100;
                        // OK take a guess
                        // does it have tabs
                        if (TabCnt > 0)
                        {
                            // ok, lets try and test this, read some more lines and test TABCount to see if it changes
                            int linecnt = 0;
                            bool samevalue = true;
                            teststr = SR.ReadLine();
                            while ((teststr != null) && (linecnt < MaxLinetestCnt) &&(samevalue))
                            {
                                CountDelimiters(teststr, ref TestQuoteCnt, ref TestCommaCnt, ref TestTabCnt, ref TestSpaceCnt);
                                samevalue = (TabCnt == TestTabCnt);
                                teststr = SR.ReadLine();
                            }
                            if (samevalue)
                            {
                                result = DataFormat.TabDelimited;
                            }
                        }
                        else
                        // Note tabs, try Comma
                        {
                            // does it have commas and no tabs
                            if (CommaCnt > 0)
                            {
                                // ok, lets try and test this, read some more lines and test CommaCount to see if it changes
                                int linecnt = 0;
                                bool samevalue = true;
                                teststr = SR.ReadLine();
                                while ((teststr != null) && (linecnt < MaxLinetestCnt)&&(samevalue))
                                {
                                    CountDelimiters(teststr, ref TestQuoteCnt, ref TestCommaCnt, ref TestTabCnt, ref TestSpaceCnt);
                                    samevalue = (CommaCnt == TestCommaCnt);
                                    teststr = SR.ReadLine();
                                }
                                if (samevalue)
                                {
                                    result = DataFormat.CommaDelimited;
                                }
                            }
                            else
                            // Not comma, try space
                            {
                                if (SpaceCnt > 0)
                                {
                                    // ok, lets try and test this, read some more lines and test CommaCount to see if it changes
                                    int linecnt = 0;
                                    bool samevalue = true;
                                    bool samelength = false;
                                    int linelen = teststr.Length;
                                    teststr = SR.ReadLine();
                                    while ((teststr != null) && (linecnt < MaxLinetestCnt) && (samevalue))
                                    {
                                        CountDelimiters(teststr, ref TestQuoteCnt, ref TestCommaCnt, ref TestTabCnt, ref TestSpaceCnt);
                                        samevalue = (SpaceCnt == TestSpaceCnt);
                                        samelength = (linelen == teststr.Length);
                                        teststr = SR.ReadLine();
                                    }
                                    if (samevalue)
                                    {
                                        result = DataFormat.SpaceDelimited;
                                    }
                                    else
                                    {
                                        // ouch, this is tough
                                        // No Commas, No Tabs, Spacecnt is not the same
                                        // >  NN  NN   N NNN  NN        fixed format, space cnt could be different but length is always the same?
                                        if (samelength)
                                        {
                                            result = DataFormat.FixedFields;
                                        }
                                    }
                                }
                                else
                                {
                                    // ouch this is tough
                                    // No Commas, No Tabs, No Spaces
                                    // > AAABBBCCCDDD  No spaces ficed format, but length aleays the same
                                    int linecnt = 0;
                                    bool samelength = true;
                                    int linelen = teststr.Length;
                                    teststr = SR.ReadLine();
                                    while ((teststr != null) && (linecnt < MaxLinetestCnt) && (samelength))
                                    {
                                        CountDelimiters(teststr, ref TestQuoteCnt, ref TestCommaCnt, ref TestTabCnt, ref TestSpaceCnt);
                                        samelength = (linelen == teststr.Length);
                                        teststr = SR.ReadLine();
                                    }
                                    if (samelength)
                                    {
                                        result = DataFormat.FixedFields;
                                    }
                                } // space
                            } // commas
                        } // tabs
                    } // teststr not null
                } // using
            }  // try
            catch (Exception ex)
            {
                throw new Exception("File Read Error: " + ex.Message);
            }
            return result;
        }


        public static List<string> GetFieldNamesFromFileHeader(string FileHeaderLine, DataFormat FormatType)
        {
            List<string> Fields = new List<string>();
            List<DynamicTextData> DataItems = FetchDataFromTextLine(FileHeaderLine, FormatType);
            foreach(DynamicTextData DTD in DataItems)
            {
                string temp = DTD.ToString();
                Fields.Add(temp);
            }
            return Fields;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Dynamic text data </summary>
        /// <remarks> This class is used to parce a text field an determines what type of data types can the field be.
        ///           Though this class provides a lot of flexibility in reading and converting text fields, it can also 
        ///           be quite slow and resource intensive.  The more you know about your data, and the less you have
        ///           to explore what data types the text field will support, the faster this will be.  
        ///           The reason for this is that this class uses try catch blocsk to trapp exceptions thrown by System.Convert() in order to
        ///           determine what data types are not valid for the text field.  Thus multiple calls to different the "Canbe" methods
        ///           that fail can generate a lot of system exceptions which can slow things down substantially.  </remarks>
        ///-------------------------------------------------------------------------------------------------

        public class DynamicTextData
        {
            string FTextValue = "";

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Constructor. </summary>
            /// <param name="Text"> The text of the data field </param>
            ///-------------------------------------------------------------------------------------------------

            public DynamicTextData(string Text)
            {
                FTextValue = Text.Trim();
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary> Gets the value as string. </summary>
            /// <returns> . </returns>
            ///-------------------------------------------------------------------------------------------------

            public string ValueString
            {
                get { return FTextValue; }

            }

            //===============================
            //  INT32 
            int Fint = 0;
            bool FIntTested = false;
            bool FCanbeInt = false;
            //-------------
            internal void TestInt()
            {
                if (!FIntTested)
                {
                    try
                    {
                        Fint = Convert.ToInt32(FTextValue);
                        FCanbeInt = true;
                        FIntTested = true;
                    }
                    catch
                    {
                        FCanbeInt = false;
                        FIntTested = true;
                    }
                }
            }
            //-----------------

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Determine if field can be int. </summary>
            /// <returns>   true if we can be int, false if not. </returns>
            ///-------------------------------------------------------------------------------------------------

            public bool CanBeInt()
            {
                TestInt();
                return FCanbeInt;
            }
            //------------------

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Gets an int value for field. </summary>
            /// <value> The value as int. </value>
            /// <exception cref="Exception">Throws Exception if not an int value</exception>
            /// <seealso cref="ValueLong"/>
            /// <seealso cref="ValueShort"/>
            ///-------------------------------------------------------------------------------------------------

            public int ValueInt
            {
                get
                {
                    TestInt();
                    if (FCanbeInt)
                        return Fint;
                    else
                        throw new Exception("Not a int32 (int) value");
                }
            }

            //===============================
            //  INT16 
            short Fint16 = 0;
            bool FInt16Tested = false;
            bool FCanbeInt16 = false;
            //-------------
            internal void TestInt16()
            {
                if (!FInt16Tested)
                {
                    try
                    {
                        Fint16 = Convert.ToInt16(FTextValue);
                        FCanbeInt16 = true;
                        FInt16Tested = true;
                    }
                    catch
                    {
                        FCanbeInt16 = false;
                        FInt16Tested = true;
                    }
                }
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Determine if value can be int16 (short). </summary>
            /// <returns>   true if we can be int 16, false if not. </returns>
            ///-------------------------------------------------------------------------------------------------

            public bool CanBeInt16()
            {
                TestInt16();
                return FCanbeInt16;
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Gets the value as short. </summary>
            /// <value> The value as short. </value>
            /// <exception cref="Exception">Throws Exception if not a int16 (short) int value</exception>
            /// <seealso cref="ValueLong"/>
            /// <seealso cref="ValueInt"/>
            ///-------------------------------------------------------------------------------------------------

            public short ValueShort
            {
                get
                {
                    TestInt16();
                    if (FCanbeInt16)
                        return Fint16;
                    else
                        throw new Exception("Not a int16 (short) value");
                }
            }
            //===============================
            //  INT64 
            long Fint64 = 0;
            bool FInt64Tested = false;
            bool FCanbeInt64 = false;
            //-------------
            internal void TestInt64()
            {
                if (!FInt64Tested)
                {
                    try
                    {
                        Fint64 = Convert.ToInt64(FTextValue);
                        FCanbeInt64 = true;
                        FInt64Tested = true;
                    }
                    catch
                    {
                        FCanbeInt64 = false;
                        FInt64Tested = true;
                    }
                }
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Determine if we can be int64 (long). </summary>
            /// <returns>   true if we can be int64, false if not. </returns>
            ///-------------------------------------------------------------------------------------------------

            public bool CanBeInt64()
            {
                TestInt64();
                return FCanbeInt64;
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Gets the value as long. </summary>
            /// <value> The long value. </value>
            /// <exception cref="Exception">Throws Exception if not a int64 (long) int value</exception>
            /// <seealso cref="ValueShort"/>
            /// <seealso cref="ValueInt"/>
            ///-------------------------------------------------------------------------------------------------

            public long ValueLong
            {
                get
                {
                    TestInt64();
                    if (FCanbeInt64)
                        return Fint64;
                    else
                        throw new Exception("Not a int64 (long) value");
                }
            }
            //===============================
            //  Double 
            double FDouble = 0;
            bool FDoubleTested = false;
            bool FCanbeDouble = false;

            internal void TestDouble()
            {
                if (!FDoubleTested)
                {
                    try
                    {
                        FDouble = Convert.ToDouble(FTextValue);
                        FCanbeDouble = true;
                        FDoubleTested = true;
                    }
                    catch
                    {
                        FCanbeDouble = false;
                        FDoubleTested = true;
                    }
                }
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Determine if we field can be double. </summary>
            /// <returns>   true if we can be double, false if not. </returns>
            ///-------------------------------------------------------------------------------------------------

            public bool CanBeDouble()
            {
                TestDouble();
                return FCanbeDouble;
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Gets the value as double. </summary>
            /// <value> The value as double. </value>
            /// <exception cref="Exception">Throws Exception if not a double value</exception>
            /// <seealso cref="ValueDecimal"/>
            /// <seealso cref="Valuedouble"/>
            ///-------------------------------------------------------------------------------------------------

            public double ValueDouble
            {
                get
                {
                    TestDouble();
                    if (FCanbeDouble)
                        return Fint;
                    else
                        throw new Exception("Not a double value");
                }
            }

            //===============================
            //  Decimal 
            decimal FDecimal = 0.0M;
            bool FDecimalTested = false;
            bool FCanbeDecimal = false;
            //-------------
            internal void TestDecimal()
            {
                if (!FDecimalTested)
                {
                    try
                    {
                        FDecimal = Convert.ToDecimal(FTextValue);
                        FCanbeDecimal = true;
                        FDecimalTested = true;
                    }
                    catch
                    {
                        FCanbeDecimal = false;
                        FDecimalTested = true;
                    }
                }
            }
            // -----------------

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Determine if value can be decimal. </summary>
            /// <returns>   true if we can be decimal, false if not. </returns>
            ///-------------------------------------------------------------------------------------------------

            public bool CanBeDecimal()
            {
                TestDecimal();
                return FCanbeDecimal;
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Gets the value as decimal. </summary>
            /// <value> The value as decimal. </value>
            /// <exception cref="Exception">Throws Exception if not a decimal value</exception>
            /// <seealso cref="ValueDouble"/>
            /// <seealso cref="Valuedouble"/>
            ///-------------------------------------------------------------------------------------------------

            public decimal ValueDecimal
            {
                get
                {
                    TestDecimal();
                    if (FCanbeDecimal)
                        return FDecimal;
                    else
                        throw new Exception("Not a decimal value");
                }
            }
            //===============================
            //  double 
            double Fdouble = 0.0f;
            bool FdoubleTested = false;
            bool FCanbedouble = false;
            //-------------
            internal void Testdouble()
            {
                if (!FdoubleTested)
                {
                    try
                    {
                        Fdouble = Convert.ToSingle(FTextValue);
                        FCanbedouble = true;
                        FdoubleTested = true;
                    }
                    catch
                    {
                        FCanbedouble = false;
                        FdoubleTested = true;
                    }
                }
            }
            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Determine if value can be double. </summary>
            /// <returns>   true if we can be double, false if not. </returns>
            ///-------------------------------------------------------------------------------------------------

            public bool CanBedouble()
            {
                Testdouble();
                return FCanbedouble;
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Gets the value as double. </summary>
            /// <value> The value as double. </value>
            ///-------------------------------------------------------------------------------------------------

            public double Valuedouble
            {
                get
                {
                    Testdouble();
                    if (FCanbedouble)
                        return Fdouble;
                    else
                        throw new Exception("Not a double (single) value");
                }
            }

            //===============================
            //  BOOL 
            bool FBool = false;
            bool FBoolTested = false;
            bool FCanbeBool = false;
            //-------------
            internal void TestBool()
            {
                if (!FBoolTested)
                {
                    // OK, check Bool with (False,true,F,T,Yes,No,Y,N)
                    string Text = FTextValue.ToUpper();
                    if ((Text == "FALSE") || (Text == "F") || (Text == "NO"))
                    {
                        FCanbeBool = true;
                        FBool = false;
                    }
                    else
                        if ((Text == "True") || (Text == "T") || (Text == "YES"))
                        {
                            FCanbeBool = true;
                            FBool = true;
                        }
                    FBoolTested = true;
                }
            }
            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Determine if value can be bool. </summary>
            /// <returns>   true if we can be bool, false if not. </returns>
            ///-------------------------------------------------------------------------------------------------

            public bool CanBeBool()
            {
                TestBool();
                return FCanbeBool;
            }
            //------------------

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Gets value as bool. </summary>
            ///
            /// <value> value as bool</value>
            /// <exception cref="Exception">Throws Exception if not a bool value</exception>
            ///-------------------------------------------------------------------------------------------------

            public bool ValueBool
            {
                get
                {
                    TestBool();
                    if (FCanbeBool)
                        return FBool;
                    else
                        throw new Exception("Not a bool (boolean) value");
                }
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Values that represent different text file data formats. </summary>
        /// <seealso cref="FetchDataFromTextLine"/>
        ///-------------------------------------------------------------------------------------------------


        public enum DataFormat
        {
            /// <summary> Unknown Data Format </summary>
            /// <remarks> Format of Text Data Unknown</remarks>
            Unknown,
            /// <summary> Standard Data Format </summary>
            /// <remarks>Fixed field widths, text data</remarks>
            FixedFields,
            /// <summary> Space Delimited. </summary>
            /// <remarks> Data in Text sperated by spaces, string data can not have space except between quotes</remarks>
            SpaceDelimited,
            /// <summary> Comma Delimited. </summary>
            /// <remarks> Data in Text sperated by commas, string data can not have commas unless string is inside a double quote pair</remarks>
            CommaDelimited,
            /// <summary> Tab Delimited. </summary>
            /// <remarks> Data in Text sperated by tabs, string data can not have tabs unless string is inside a double quote pair</remarks>
            TabDelimited,
            ///// <summary> Comma Delimited with Quotes. </summary>
            ///// <remarks> Data in Text sperated by commas, strings can be enclosed in double quotes pairs and if so contain a comma</remarks>
            //CommaDelimitedQuoted,
            ///// <summary> Tab Delimited with Quotes. </summary>
            ///// <remarks> Data in Text sperated by tabs, strings can be enclosed in double quotes and if so contain a Tab</remarks>
            //TabDelimitedQuoted,
        };

        
        /// <summary>
        ///  Converts a text DataFormat string to a DataFormat value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        
        static public DataFormat StringToDataFormat(string value)
        {
            value = value.ToUpper();
            switch (value)
            {
                case "FIXEDFIELDS":
                    return Tools.DataFormat.FixedFields;
                case "SPACEDELIMITED":
                    return Tools.DataFormat.SpaceDelimited;
                case "COMMADELIMITED":
                    return Tools.DataFormat.CommaDelimited;
                case "TABDELIMITED":
                    return Tools.DataFormat.TabDelimited;
                default:
                    return Tools.DataFormat.Unknown;
            }
        }

        static public string DataFormtToString(DataFormat value)
        {
            switch (value)
            {
                case Tools.DataFormat.FixedFields:
                    return "FIXEDFIELDS";
                case Tools.DataFormat.SpaceDelimited:
                    return "SPACEDELIMITED";
                case Tools.DataFormat.CommaDelimited:
                    return "COMMADELIMITED";
                case Tools.DataFormat.TabDelimited:
                    return "TABDELIMITED";
                default:
                    return "UNKNOWN";
            }
        }

        static private string ParseNext(string theLine, int StartIndex, char ParseChar, out int NewStart)
        {
            // check if starts with a quote 
            string Thetext = "";
            string temp = theLine.Substring(StartIndex).TrimStart();
            // EDIT QUAY 5 29 18
            // OUCH A BUG, if a comma delimed line ends in a   ", " this chrashes this line
            //if (theLine.Substring(StartIndex).TrimStart()[0] == DoubleQuote)
            // becuase the trim renders it "" which has no [0] position!
            string TheSubLine = theLine.Substring(StartIndex).TrimStart(); // NOt to be confused with sublime
            if (TheSubLine != "")
            {
                if (TheSubLine[0] == DoubleQuote)
                {
            // END EDIT 5 29 18
                    // ok cycle through and get all the text to the next quote
                    int StartQuoteIndex = theLine.IndexOf(DoubleQuote, StartIndex);
                    int nextQuoteIndex = theLine.IndexOf(DoubleQuote, StartQuoteIndex + 1);
                    if (nextQuoteIndex > -1)
                    {
                        Thetext = theLine.Substring(StartQuoteIndex + 1, (nextQuoteIndex - StartQuoteIndex) - 1);
                        int ParceCharIndex = theLine.IndexOf(ParseChar, nextQuoteIndex);
                        if (ParceCharIndex > -1)
                        {
                            // ok strip of seperator
                            NewStart = ParceCharIndex + 1;
                        }
                        else
                        {
                            NewStart = nextQuoteIndex + 1;
                        }

                    }
                    else
                    {
                        Thetext = theLine.Substring(StartQuoteIndex + 1);
                        NewStart = theLine.Length;
                    }
                }
                else
                {
                    // Ok does not start with a quote, use seperator
                    int ParceCharIndex = theLine.IndexOf(ParseChar, StartIndex);
                    if (ParceCharIndex < 0)
                    {
                        // No seperator, takeit all
                        Thetext = theLine.Substring(StartIndex);
                        NewStart = theLine.Length;
                    }
                    else
                    {
                        Thetext = theLine.Substring(StartIndex, ParceCharIndex - StartIndex);
                        NewStart = ParceCharIndex + 1;
                    }

                }
            }
            // EDIT QUAY 5 29 19
            // See Bug fix above, this is the rest
            else // OK there is a field but no content and we are at the end
            {
                Thetext = "";
                NewStart = theLine.Length;

            }
            // END EDIT 5 29 19

            return Thetext;

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Parses data from text line. </summary>
        /// <param name="Line">         The text line to parse </param>
        /// <param name="FormatType">   Type of data file format. </param>
        /// <returns>   A List of DynamicTextData, one for each text field found in the text line. </returns>
        ///-------------------------------------------------------------------------------------------------

        static public List<DynamicTextData> FetchDataFromTextLine(string Line, DataFormat FormatType)
        {
            List<DynamicTextData> Data = new List<DynamicTextData>();
            try
            {
                switch (FormatType)
                {
                    case DataFormat.FixedFields:
                        {

                            break;
                        }
                    case DataFormat.SpaceDelimited:
                        {
                            int nextspace = -1;
                            string temp = "";
                            int index = 0;
                            while (index < Line.Length)
                            {
                                // Go to next field
                                while ((Line[index] == ' ') && (index < Line.Length))
                                    index++;
                                if (index < Line.Length)
                                {
                                    // Find end of this field
                                    nextspace = Line.IndexOf(' ', index);
                                    // check if lsat field
                                    if (nextspace == -1)
                                        nextspace = Line.Length;
                                    // grab field text
                                    temp = Line.Substring(index, nextspace - index);
                                    Data.Add(new DynamicTextData(temp));
                                    index = nextspace;
                                }
                                else
                                {
                                    temp = "";
                                    Data.Add(new DynamicTextData(temp));
                                }
                            }
                        }
                        break;
                    case DataFormat.CommaDelimited:
                        {
                            //int nextcomma = -1;
                            //string temp = "";
                            //int index = 0;
                            //while (index < Line.Length)
                            //{
                            //    // Find end of this field
                            //    nextcomma = Line.IndexOf(',', index);
                            //    // check if lsat field
                            //    if (nextcomma == -1)
                            //        nextcomma = Line.Length;
                            //    // grab field text
                            //    temp = Line.Substring(index, nextcomma - index).Trim();
                            //    Data.Add(new DynamicTextData(temp));
                            //    index = nextcomma + 1;
                            //}
                            int index = 0;
                            while (index < Line.Length)
                            {
                                // fetch the field
                                string temp = ParseNext(Line, index, ',', out index);
                                Data.Add(new DynamicTextData(temp));
                            }
                            // Find end of this field
                            //    nextcomma = Line.IndexOf(',', index);
                            //    // check if lsat field
                            //    if (nextcomma == -1)
                            //        nextcomma = Line.Length;
                            //    // grab field text
                            //    temp = Line.Substring(index, nextcomma - index).Trim();
                            //    Data.Add(new DynamicTextData(temp));
                            //    index = nextcomma + 1;

                            break;
                        }
                    case DataFormat.TabDelimited:
                        {
                            int nextspace = -1;
                            string temp = "";
                            int index = 0;
                            while (index < Line.Length)
                            {
                                // Find end of this field
                                nextspace = Line.IndexOf('\t', index);
                                // check if lsat field
                                if (nextspace == -1)
                                    nextspace = Line.Length;
                                // grab field text
                                temp = Line.Substring(index, nextspace - index).Trim();
                                Data.Add(new DynamicTextData(temp));
                                index = nextspace + 1;
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                // ouch what does this mean, just quit?
            }
            return Data;
        }
        //-------------------------------------------------------------------------------
        /// <summary>
        /// Parses a command to its key elements, all in a list
        /// </summary>
        /// <param name="CommandLine"></param>
        /// <param name="isError"></param>
        /// <param name="errMessage"></param>
        /// <returns></returns>
        static public List<string> ParseCommand(string CommandLine, out bool isError, out string errMessage)
        {
            List<string> Parsed = new List<string>();
            isError = false;
            errMessage = "";
            string TempLine = CommandLine;
            int ParseIndex = 0;
            int inblock = 0;
            string BlockTemp = "";
            while (ParseIndex < TempLine.Length)
            {
                
                string temp = ParseNext(TempLine, ParseIndex, ' ', out ParseIndex);
                // Check and see of empty
                if (temp != "")
                {
                    // Check to see of defining a block
                    if (temp[0] == '(')
                    {
                        inblock++;
                    }
                            // check if there is an end block
                    int EndB = temp.IndexOf(')');
                    if (EndB>-1)
                    {
                       inblock--;
                    }
                    BlockTemp += " " + temp;
                    if (inblock<1)
                    {
                        Parsed.Add(BlockTemp);
                        BlockTemp = "";
                    }
                    
                }

            }
            return Parsed;
        } 
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Test of Oledb column is string DataType. </summary>
        /// <param name="DC"> The DataColumn. </param>
        ///
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static bool oledbColumnIsString(DataColumn DC)
        {
            bool result = false;
            result = (DC.DataType == System.Type.GetType("System.String"));
            return result;
        }


        
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Convert DataTable to text file. </summary>
        ///
        /// <param name="dbCon">                The database connection </param>
        /// <param name="tablename">            name of the data table to load </param>
        /// <param name="mode">                 The mode for writing the text file </param>
        /// <param name="newtablename">         The newtablename. </param>
        /// <param name="PR">                   Use this delegate to report progress </param>
        /// <param name="CreateDataDefineFile"> true to create data definition file for the text . </param>
        /// <param name="errMessage">           Message describing the error if returns false. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static bool ConvertdbTableToText(UniDbConnection dbCon, string tablename, Tools.DataFormat mode, string newtablename, ProgressReport PR, bool CreateDataDefineFile, ref string errMessage)
        {
            bool result = false;
            bool iserr = false;
            List<int> FieldWidths = new List<int>();
            string Delimiter = "";
            string ModeStr = "";

            switch (mode)
            {
                case DataFormat.CommaDelimited: // TxtFieldMode.cmCommaDelimited:
                    Delimiter = ", ";
                    ModeStr = "Comma Delimited";
                    break;
                case DataFormat.SpaceDelimited: // TxtFieldMode.cmSpaceDelimited:
                    Delimiter = " ";
                    ModeStr = "Space Delimited";
                    break;
                case DataFormat.FixedFields: // TxtFieldMode.cmFixedFieldWidth:
                    Delimiter = "";
                    ModeStr = "Fixed Field Width";
                    break;

            }
            DataTable InputData = LoadTable(dbCon, tablename, ref iserr, ref errMessage);
            if (iserr)
            {
                result = false;
            }
            else
            // table loaded lets go
            {
                int ReportMaximum = InputData.Rows.Count;
                string ext = Path.GetExtension(newtablename);
                int extindex = newtablename.IndexOf(ext);
                string pathfilename = newtablename.Substring(0, extindex);
                string docfilename = pathfilename + "_Data_Definition" + ext;
                try  // Textwriter
                {
                    int ReportOnRowCnt = InputData.Rows.Count / 100;
                    if (ReportOnRowCnt == 0)
                        ReportOnRowCnt = 1;

                    using (TextWriter DataWwriter = File.CreateText(newtablename))
                    {
                        int RowCnt = 0;
                        foreach (DataRow DR in InputData.Rows)
                        {
                            string RowStr = "";
                            int colcnt = 0;
                            foreach (DataColumn DC in InputData.Columns)
                            {
                                string FldDataStr = "";
                                if (colcnt > 0)
                                    FldDataStr += Delimiter;
                                bool IsString = oledbColumnIsString(DC);
                                if (IsString)
                                    FldDataStr += '"';
                                FldDataStr += DR[DC.ColumnName].ToString();
                                if (IsString)
                                    FldDataStr += '"';
                                RowStr += FldDataStr;
                                colcnt++;
                            }
                            DataWwriter.WriteLine(RowStr);
                            if ((PR != null) && ((RowCnt % ReportOnRowCnt) == 0))
                                PR("Writing", RowCnt, ReportMaximum);
                            RowCnt++;
                        }
                    }
                }
                catch (Exception e)
                {
                    result = false;
                    errMessage = "Error writing new file" + newtablename + ".  Error:" + e.Message;

                }
                try
                {
                    if (CreateDataDefineFile)
                    {
                        using (TextWriter DocWriter = File.CreateText(docfilename))
                        {
                            DocWriter.WriteLine("Data Definition for " + newtablename);
                            DocWriter.WriteLine(DateTime.Today.ToShortDateString() + "  " + DateTime.Today.ToShortTimeString());
                            string outstr = "Database: " + dbCon.ConnectionString + "  Tablename: " + tablename;
                            DocWriter.WriteLine(outstr);
                            DocWriter.WriteLine("Format: " + ModeStr);

                            DocWriter.WriteLine("# ColumnName");
                            int cnt = 0;
                            foreach (DataColumn DC in InputData.Columns)
                            {

                                outstr = cnt.ToString().PadRight(7) + DC.ColumnName;
                                DocWriter.WriteLine(outstr);
                                cnt++;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    result = false;
                    errMessage = "Error writing Data Definition file" + docfilename + ".  Error:" + e.Message;
                }

            }


            return result;
        }

        public static bool ConvertdbTableToText(DataTable InputData, Tools.DataFormat mode, string newtablename, ProgressReport PR, bool CreateDataDefineFile, ref string errMessage)
        {
            bool result = false;
            bool iserr = false;
            List<int> FieldWidths = new List<int>();
            
            
            // set format dependendt fields
            string Delimiter = "";
            string ModeStr = "";
            switch (mode)
            {
                case DataFormat.CommaDelimited: // TxtFieldMode.cmCommaDelimited:
                    Delimiter = ", ";
                    ModeStr = "Comma Delimited";
                    break;
                case DataFormat.SpaceDelimited: // TxtFieldMode.cmSpaceDelimited:
                    Delimiter = " ";
                    ModeStr = "Space Delimited";
                    break;
                case DataFormat.FixedFields: // TxtFieldMode.cmFixedFieldWidth:
                    Delimiter = "";
                    ModeStr = "Fixed Field Width";
                    break;

            }
            int ReportMaximum = InputData.Rows.Count;
            string ext = Path.GetExtension(newtablename);
            int extindex = newtablename.IndexOf(ext);
            string pathfilename = newtablename.Substring(0, extindex);
            string docfilename = pathfilename + "_Data_Definition" + ext;
         
            try  // Textwriter
            {
                int ReportOnRowCnt = InputData.Rows.Count / 100;
                if (ReportOnRowCnt == 0)
                    ReportOnRowCnt = 1;

                using (TextWriter DataWwriter = File.CreateText(newtablename))
                {
                    // if not using a define file, then create a header for this file.
                    if (!CreateDataDefineFile)
                    {
                        string HeaderStr = InputData.Columns[0].ColumnName;
                        int colcnt = InputData.Columns.Count;
                        for (int i = 1; i < colcnt; i++)
                        {
                            HeaderStr += "," + InputData.Columns[i].ColumnName;
                        }
                        DataWwriter.WriteLine(HeaderStr);
                    }
                    // loop through all the records and write
                    int RowCnt = 0;

                    foreach (DataRow DR in InputData.Rows)
                    {
                        string RowStr = "";
                        int colcnt = 0;
                        foreach (DataColumn DC in InputData.Columns)
                        {
                            string FldDataStr = "";
                            if (colcnt > 0)
                                FldDataStr += Delimiter;
                            bool IsString = oledbColumnIsString(DC);

                            bool QuotedValue = false;
                            string TempDataStr = DR[DC.ColumnName].ToString();
                            if (IsString)
                            {
                                int aQuoteCnt = 0;
                                int aCommaCnt = 0;
                                int aTabCnt = 0;
                                int aSpaceCnt = 0;
                                CountDelimiters(TempDataStr, ref aQuoteCnt, ref aCommaCnt, ref aTabCnt, ref aSpaceCnt);
                                if ((aQuoteCnt + aCommaCnt + aTabCnt + aSpaceCnt) > 0)
                                {
                                    if (aQuoteCnt > 0)
                                    {
                                        TempDataStr.Replace('"','\'');
                                    }
                                    FldDataStr += '"';
                                    QuotedValue = true;
                                }
                            }
                            FldDataStr += TempDataStr;// DR[DC.ColumnName].ToString();
                            if (QuotedValue)
                                FldDataStr += '"';
                            // OK, see if fixed fields, if so pad the data
                            if (mode == DataFormat.FixedFields)
                            {
                                int charwidth = CharSizeofDataType(DC.DataType);
                                if (charwidth < 1)
                                {
                                    charwidth = DC.MaxLength + 4;
                                }
                                if (charwidth < 1)
                                {
                                    charwidth = 255 + 4;
                                    if (FldDataStr.Length > 255)
                                    {
                                        bool addquote = (FldDataStr[FldDataStr.Length - 1] == DoubleQuote);
                                        FldDataStr = FldDataStr.Substring(0, 255);
                                        if (addquote)
                                        {
                                            FldDataStr += DoubleQuote;
                                        }
                                    }
                                }
                                FldDataStr = FldDataStr.PadLeft(charwidth);
                            }
                            // Build the full string
                            RowStr += FldDataStr;
                            colcnt++;
                        }
                        // all done write the data string
                        DataWwriter.WriteLine(RowStr);
                        // OK report progress
                        if ((PR != null) && ((RowCnt % ReportOnRowCnt) == 0))
                            PR("Writing "+RowCnt.ToString("N0"), RowCnt, ReportMaximum);
                        RowCnt++;
                    }
                }
                result = true;
            }
            catch (Exception e)
            {
                result = false;
                errMessage = "Error writing new file" + newtablename + ".  Error:" + e.Message;

            }
            try
            {
                if (CreateDataDefineFile)
                {
                    using (TextWriter DocWriter = File.CreateText(docfilename))
                    {
                        DocWriter.WriteLine("Data Definition for " + newtablename);
                        DocWriter.WriteLine(DateTime.Today.ToShortDateString() + "  " + DateTime.Today.ToShortTimeString());
                       
                        DocWriter.WriteLine("Format: " + ModeStr);

                        DocWriter.WriteLine("# ColumnName");
                        int cnt = 0;
                        foreach (DataColumn DC in InputData.Columns)
                        {

                            string outstr = cnt.ToString().PadRight(7) + DC.ColumnName;
                            DocWriter.WriteLine(outstr);
                            cnt++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                errMessage = "Error writing Data Definition file" + docfilename + ".  Error:" + e.Message;
            }


            return result;


        }
        //--------------------------------------------------------------
        #endregion

        #region Data Type Conversion Support
        //-----------------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Converts this object to an int 32. </summary>
        ///
        /// <remarks>Uses the Net Convert class methods, but traps exceptions from Convert and indicates errors with iserror and errMessage.
        ///                   If valueStr is not in a Integer or double format, or the format exceeds the MaxSize or MinSize of Int32, then IsError true and result 0, 
        ///                   with a message of what happened in errMessage.  If in a double format, this is first converted to a Double, then to an Int32,
        ///                   which rounds the decimal value.
        ///                   </remarks>
        /// <param name="valueStr">     The value string. </param>
        /// <param name="IsError">      The is error. </param>
        /// <param name="errMessage">   Message describing the error. </param>
        ///
        /// <returns>   The given data converted to an int 32. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static int ConvertToInt32(string valueStr, ref bool IsError, ref string errMessage)
        {
            int result = 0;
            IsError = true;
            if (valueStr != "")
            {
                try
                {
                    int periodIndex = valueStr.LastIndexOf(".");
                    if (periodIndex >= 0)
                    {
                        Double TempD = Convert.ToDouble(valueStr);
                        result = Convert.ToInt32(TempD);
                    }
                    else
                    {
                        result = Convert.ToInt32(valueStr);
                    }
                    IsError = false;
                    errMessage = "";
                }
                catch (Exception ex)
                {
                    errMessage = "Conversion Error: " + ex.Message;
                }
            }
            else
            {
                IsError = true;
                errMessage = "Value is Null String"; 
            }
            return result;
        }
        //-----------------------------------------------------------------------
        //
        // edits 06.15.20 das
        public static String ConvertToString(string valueStr, ref bool IsError, ref string errMessage)
        {
            string result = "";
            IsError = true;
            if (valueStr != "")
            {
                try
                {
                    int periodIndex = valueStr.LastIndexOf(".");
                    if (periodIndex >= 0)
                    {
                        result = valueStr;
                        //String TempD = Convert.ToString(valueStr);
                       // string TempD = valueStr;
                        //result = Convert.ToString(TempD);
                    }
                    else
                    {
                        // result = Convert.ToInt16(valueStr);
                        result = valueStr;
                    }
                    IsError = false;
                    errMessage = "";
                }
                catch (Exception ex)
                {
                    errMessage = "Conversion Error: " + ex.Message;
                }
            }
            else
            {
                IsError = true;
                errMessage = "Value is Null String";
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Converts this object to an int 16. </summary>
        /// <remarks>Uses the Net Convert class methods, but traps exceptions from Convert and indicates errors with iserror and errMessage.
        ///                   If valueStr is not in a Integer or double format, or the format exceeds the MaxSize or MinSize of Int16, then IsError true and result 0, 
        ///                   with a message of what happened in errMessage.  If in a double format, this is first converted to a Double, then to an Int16,
        ///                   which rounds the decimal value.
        ///                   </remarks>
        /// <param name="valueStr">   The value string. </param>
        /// <param name="IsError">    The is error. </param>
        /// <param name="errMessage"> Message describing the error. </param>
        ///
        /// <returns> The given data converted to an int 16. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Int16 ConvertToInt16(string valueStr, ref bool IsError, ref string errMessage)
        {
            Int16 result = 0;
            IsError = true;
            if (valueStr != "")
            {
                try
                {
                    int periodIndex = valueStr.LastIndexOf(".");
                    if (periodIndex >= 0)
                    {
                        Double TempD = Convert.ToDouble(valueStr);
                        result = Convert.ToInt16(TempD);
                    }
                    else
                    {
                        result = Convert.ToInt16(valueStr);
                    }
                    IsError = false;
                    errMessage = "";
                }
                catch (Exception ex)
                {
                    errMessage = "Conversion Error: " + ex.Message;
                }
            }
            else
            {
                IsError = true;
                errMessage = "Value is Null String";
            }
            return result;
        }
        //-----------------------------------------------------------------------
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Converts this object to an int64. </summary>
        /// <remarks>Uses the Net Convert class methods, but traps exceptions from Convert and indicates errors with iserror and errMessage.
        ///                   If valueStr is not in a Integer or double format, or the format exceeds the MaxSize or MinSize of Int64, then IsError true and result 0, 
        ///                   with a message of what happened in errMessage.  If in a double format, this is first converted to a Double, then to an Int64,
        ///                   which rounds the decimal value.
        ///                   </remarks>
        /// <param name="valueStr">   The value string. </param>
        /// <param name="IsError">    The is error. </param>
        /// <param name="errMessage"> Message describing the error. </param>
        ///
        /// <returns> The given data converted to an int 16. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Int64 ConvertToInt64(string valueStr, ref bool IsError, ref string errMessage)
        {
            Int64 result = 0;
            IsError = true;
            if (valueStr != "")
            {
                try
                {
                    int periodIndex = valueStr.LastIndexOf(".");
                    if (periodIndex >= 0)
                    {
                        Double TempD = Convert.ToDouble(valueStr);
                        result = Convert.ToInt64(TempD);
                    }
                    else
                    {
                        result = Convert.ToInt64(valueStr);
                    }
                    IsError = false;
                    errMessage = "";
                }
                catch (Exception ex)
                {
                    errMessage = "Conversion Error: " + ex.Message;
                }
            }
            else
            {
                IsError = true;
                errMessage = "Value is Null String";
            }

            return result;
        }
        //-----------------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Converts this object to a double. </summary>
        /// <remarks>Uses the Net Convert class methods, but traps exceptions from Convert and indicates errors with iserror and errMessage.
        ///                   If valueStr is not in a double format, or the format exceeds the MaxSize or MinSize of Double then IsError true and result 0, 
        ///                   with a message of what happened in errMessage. 
        ///                   </remarks>		
        /// <param name="valueStr">     The value string. </param>
        /// <param name="IsError">      [in,out] The is error. </param>
        /// <param name="errMessage">   [in,out] Message describing the error. </param>
        ///
        /// <returns>   The given data converted to a double. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static double ConvertToDouble(string valueStr, ref bool IsError, ref string errMessage)
        {
            double result = 0;
            IsError = true;
            if (valueStr != "")
            {
                try
                {
                    result = Convert.ToDouble(valueStr);
                    IsError = false;
                    errMessage = "";
                }
                catch (Exception ex)
                {
                    errMessage = "Conversion Error: " + ex.Message;
                }
            }
            else
            {
                IsError = true;
                errMessage = "Value is Null String";
            }

            return result;
        }

        #endregion

        /// <summary>
        /// A List of ColumnInfo objects for a DataTable
        /// </summary>
        public class ColumnInfoList : List<ColumnInfo>
        {
//            List<ColumnInfo> FColInfoList = new List<ColumnInfo>();

            /// <summary>   Default constructor. </summary>
            public ColumnInfoList()
            {

            }
            
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="DT"></param>
            public ColumnInfoList(DataTable DT)
            {
                if (DT!=null)
                {
                    foreach (DataColumn DC in DT.Columns)
                    {
                        ColumnInfo CI = new ColumnInfo(DC);
                        this.Add(CI);
                    }
                }
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Indexer to get items within this collection using array index syntax. </summary>
            ///
            /// <param name="index">    . </param>
            ///
            /// <value> . </value>
            ///-------------------------------------------------------------------------------------------------
            
            //public ColumnInfo this[int index]
            //{
            //    get
            //    {
            //        if ((index>=0)&&(index<FColInfoList.Count))
            //        {
            //            return FColInfoList[index];
            //        }
            //        else
            //        {
            //            return null;
            //        }
            //    }
            //}

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Gets the column infos. </summary>
            ///
            /// <value> The column infos. </value>
            ///-------------------------------------------------------------------------------------------------

            public List<ColumnInfo> ColumnInfos
            {
                get { return this; } // FColInfoList; }
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Gets the fieldnames. </summary>
            ///
            /// <value> The fieldnames. </value>
            ///-------------------------------------------------------------------------------------------------

            public List<string> Fieldnames
            {
                get
                {
                    List<String> temp = new List<string>();
                    foreach (ColumnInfo CI in this) // FColInfoList)
                    {
                        temp.Add(CI.ColumName);
                    }
                    return temp;
                }
            }

            //public void Clear()
            //{
            //    FColInfoList.Clear();
            //}

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Adds a ColumnInfo object </summary>
            ///
            /// <param name="CI">   The ColumnInfo to add. </param>
            ///-------------------------------------------------------------------------------------------------

            //public void Add(ColumnInfo CI)
            //{
            //    FColInfoList.Add(CI);
            //}

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Searches for the first match for the given fieldname string. </summary>
            ///
            /// <param name="Fieldname">    The fieldname. </param>
            ///
            /// <returns>  if found return ColumnInfo object else null . </returns>
            ///-------------------------------------------------------------------------------------------------

            public ColumnInfo Find(string Fieldname)
            {
                ColumnInfo TheTarget = null;
                Fieldname = Fieldname.ToUpper();
                foreach (ColumnInfo CI in this )//FColInfoList)
                {
                    if (CI.ColumName.ToUpper() == Fieldname)
                    {
                        TheTarget = CI;
                        break;
                    }
                }
                return TheTarget;
            }
        }
        //===============================================
        /// <summary>   Information about the column. </summary>
        public class ColumnInfo
        {
            string FName;
            System.Type FDataType;
            int FWidth;
            bool FIsPrimary;
            bool FIsNotNull;
            bool FUnique;
            bool FAutoIncrement;

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Constructor. </summary>
            ///
            /// <param name="Column">   The column. </param>
            ///-------------------------------------------------------------------------------------------------

            public ColumnInfo(DataColumn Column)
            {
                FName = Column.ColumnName;
                FDataType = Column.DataType;
                FWidth = Column.MaxLength;
                FIsPrimary = false;
                FIsNotNull = !Column.AllowDBNull;
                FUnique = Column.Unique;
                FAutoIncrement = Column.AutoIncrement;
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary> Constructor. </summary>
            ///
            /// <remarks> Ray, 1/24/2013. </remarks>
            ///
            /// <param name="name">        The name. </param>
            /// <param name="datatype">    The datatype. </param>
            /// <param name="width">       The width. </param>
            /// <param name="MakePrimary"> true to make primary. </param>
            /// <param name="NotNull">     true if nullable, false if not. </param>
            /// <param name="Unique">      true if unique, false if not. </param>
            ///-------------------------------------------------------------------------------------------------

            public ColumnInfo(string name, System.Type datatype, int width, bool MakePrimary, bool NotNull, bool Unique)
            {
                FName = name;
                FDataType = datatype;
                FWidth = width;
                FIsPrimary = MakePrimary;
                FIsNotNull = NotNull;
                FUnique = Unique;
                FAutoIncrement = false;
            }


            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Gets the name of the colum. </summary>
            ///
            /// <value> The name of the colum. </value>
            ///-------------------------------------------------------------------------------------------------

            public string ColumName
            { get { return FName; } }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Gets the type of the data. </summary>
            ///
            /// <value> The type of the data. </value>
            ///-------------------------------------------------------------------------------------------------

            public System.Type DataType
            { get { return FDataType; } }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Gets the width. </summary>
            ///
            /// <value> The width. </value>
            ///-------------------------------------------------------------------------------------------------

            public int Width
            { get { return FWidth; } }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Gets a value indicating whether this column can include null values. </summary>
            ///
            /// <value> true if nullable, false if not. </value>
            ///-------------------------------------------------------------------------------------------------

            public bool NotNull
            { get { return FIsNotNull; } }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Gets a value indicating whether the unique column requires unique values. </summary>
            ///
            /// <value> true if unique, false if not. </value>
            ///-------------------------------------------------------------------------------------------------

            public bool Unique
            { get { return FUnique; } }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Gets or sets a value indicating whether the column is a primary index. </summary>
            /// <remarks>   Always returns false if created from another DataColumn unless specifically set to atrue value after created. </remarks>
            /// <value> true if primary, false if not. </value>
            ///-------------------------------------------------------------------------------------------------

            public bool Primary
            {
                get { return FIsPrimary; }
                set { FIsPrimary = value; }
            }

            public bool AutoIncrement
            {
                get { return FAutoIncrement; }
                set { FAutoIncrement = value; }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a new table from column list. </summary>
        ///
        /// <remarks> Ray, 10/84/2015. </remarks>
        ///
        ///
        /// <param name="Name">             The name. </param>
        /// <param name="ColumnDefines">    The column defines. </param>
        /// <param name="errMessage">       Message describing the error if returns false. </param>
        ///
        /// <returns>  DataTable unless error then return null . </returns>
        ///-------------------------------------------------------------------------------------------------

        public static DataTable NewTableFromColumnList(string Name, List<ColumnInfo> ColumnDefines, ref string errMessage)
        {
            DataTable DT = new DataTable(Name);
            try
            {
                foreach (ColumnInfo CI in ColumnDefines)
                {
                    DataColumn DC = new DataColumn(CI.ColumName, CI.DataType);
                    DT.Columns.Add(DC);
                }
                errMessage = "";
            }
            catch (Exception e)
            {
                errMessage = e.Message;
                DT = null;
            }

            return DT;
        }

        /// <summary>
        /// This Rebuilds and existing Table according to a list of ColumnInfo objects
        /// </summary>
        /// <remarks>NOTE! The datatable can not be null, nor canit have any Columns defined, must be a newly minted emoty table </remarks>
        /// <param name="DT"></param>
        /// <param name="ColumnDefines"></param>
        /// <param name="errMessage"></param>
        public static void BuildTableFromColumnList(DataTable DT, List<ColumnInfo> ColumnDefines, ref string errMessage)
        {
            if (DT == null)
            {
                throw new Exception("DataTable can not be null");
            }
            else
            {
                if (DT.Columns.Count > 0)
                {
                    throw new Exception("DataTable must be an empty table, ir no columns defined");
                }
                else
                try
                {
                    foreach (ColumnInfo CI in ColumnDefines)
                    {
                        DataColumn DC = new DataColumn(CI.ColumName, CI.DataType);
                        DT.Columns.Add(DC);
                    }
                    errMessage = "";
                }
                catch (Exception e)
                {
                    errMessage = e.Message;
                    DT = null;
                }
            }

        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Creates a table from column list. </summary>
        ///
        /// <remarks> Ray, 1/24/2013. </remarks>
        ///
        /// <param name="DbConnnection"> The database connnection. </param>
        /// <param name="NewTablename">  The new tablename. </param>
        /// <param name="ColumnDefines"> The column defines. </param>
        /// <param name="errMessage">    Message describing the error if returns false. </param>
        /// <param name="Server">        The server. </param>
        ///
        /// <returns> The new table from column list. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static bool createTableFromColumnList(UniDbConnection DbConnnection, string NewTablename, List<ColumnInfo> ColumnDefines, ref string errMessage, SQLServer Server)
        {
            bool createResult = false;
            // base string for create
            string tc = "CREATE TABLE ";
            // Add TableName
            tc += '[' + NewTablename + ']' + " (";
            // set up for loop to create string for each column in column list
            string ColList = "";
            int cnt = 0;
            int colnum = ColumnDefines.Count;
            string tempClosing = "";
            foreach (ColumnInfo CI in ColumnDefines)
            {
                string ConstraintStr = "";

                // Add name and Datatype
                string temp = CI.ColumName + " " + SQLDataDefine(CI.DataType, Server, CI.Width);
                string tempConstaint = SQLConstraintDefine(Server, SQLConstraint.scNotNull, CI.ColumName, CI.NotNull, ref tempClosing);
                if (tempConstaint != "") ConstraintStr = tempConstaint;
                tempConstaint = SQLConstraintDefine(Server, SQLConstraint.scPrimaryKey, CI.ColumName, CI.Primary, ref tempClosing);
                if (tempConstaint != "")
                {
                    if (ConstraintStr != "") ConstraintStr += " ";
                    ConstraintStr += tempConstaint;
                }              // Check if nullable
                tempConstaint = SQLConstraintDefine(Server, SQLConstraint.scUnique, CI.ColumName, CI.Unique, ref tempClosing);
                if (tempConstaint != "")
                {
                    if (ConstraintStr != "") ConstraintStr += " ";
                    ConstraintStr += tempConstaint;
                }              // Check if nullable
                // Check if unique
                temp += ConstraintStr;
                // Check primary
                cnt++;
                if (cnt < colnum) temp += " , ";
                ColList += temp;
            }
            if (tempClosing != "") tc += ", " + tempClosing;
            tc += ColList + ")";
            // OK Execute create command
            DbCommand cmd = DbConnnection.CreateCommand();
            cmd.CommandText = tc;
            try
            {
                cmd.ExecuteNonQuery();
                createResult = true;
                cmd.Dispose();
            } // try
            catch (Exception ex)
            {
                errMessage = "Error creating table " + NewTablename + ". Error:" + ex.Message;
                cmd.Dispose();
            } // catch


            return createResult;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a table from table. </summary>
        ///
        /// <param name="DbConnnection">    The database connnection. </param>
        /// <param name="Source">           Source for the. </param>
        /// <param name="NewTablename">     The new tablename. </param>
        /// <param name="errMessage">       Message describing the error if returns false. </param>
        /// <param name="Server">           The server. </param>
        ///
        /// <returns>   The new table from table. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static bool createTableFromTable(UniDbConnection DbConnnection, DataTable Source, string NewTablename, ref string errMessage, SQLServer Server)
        {

            bool result = false;
            if (NewTablename.Length > 0)
            {
                string tc = "CREATE TABLE ";
                DbCommand cmd = DbConnnection.CreateCommand();
                tc += '[' + NewTablename + ']' + " (";
                string ColList = "";
                int cnt = 0;
                foreach (DataColumn DC in Source.Columns)
                {
                    if (cnt > 0)
                        ColList += " , ";
                    ColList += SQLDataDefine(DC.DataType, Server, DC.MaxLength);

                }
                tc += ColList + "  )";
                cmd.CommandText = tc;
                try
                {
                    cmd.ExecuteNonQuery();
                    result = true;
                    cmd.Dispose();
                } // try
                catch (Exception ex)
                {
                    errMessage = "Error creating table " + NewTablename + ". Error:" + ex.Message;
                    cmd.Dispose();
                } // catch
            } // if newtable
            return result;
        }

        
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a SQL string to define Column and values definitions for an Insert Command for a DbCommand 
        ///             that is approporate for DT Datatable. </summary>
        ///
        /// <param name="DT">   The DataTable. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        static public string ColumnValueSQLDefine(DataTable DT)
        {
            // SetUp Value strings
            string ColumnValueList = "VALUES { ";

            foreach (DataColumn DC in DT.Columns)
            {
                ColumnValueList += " ? , ";
            }

            // Ok get rid of the last commas
            ColumnValueList = ColumnValueList.Substring(0, ColumnValueList.Length - 3) + " ) ";
            return ColumnValueList;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a SQL string to define Column definitions for an Select Command for a DbCommand
        ///             that is approporate for DT Datatable. </summary>
        ///
        /// <param name="DT">   The DataTable. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        static public string ColumnSelectSQLDefine(DataTable DT)
        {
            // SetUp Column Define strings
            string ColumnDefineList = "";
            List<ColumnInfo> ColInfoList = new List<ColumnInfo>();
            foreach (DataColumn DC in DT.Columns)
            {
                ColInfoList.Add(new UniDB.Tools.ColumnInfo(DC));
            }

            foreach (UniDB.Tools.ColumnInfo CI in ColInfoList)
            {
                ColumnDefineList += CI.ColumName + " , ";
            }
            ColumnDefineList = " ( "+ColumnDefineList.Substring(0, ColumnDefineList.Length - 3)+ " ) ";
            return ColumnDefineList;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a SQL string to define Column Set definitions for an Update Command for a DbCommand
        ///             that is approporate for DT Datatable. </summary>
        ///
        /// <param name="DT">   The DataTable. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        static public string ColumnSetSQLDefine(DataTable DT)
        {
            // SetUp Column Define strings
            string ColumnSetList = " SET ";
            List<ColumnInfo> ColInfoList = new List<ColumnInfo>();
            foreach (DataColumn DC in DT.Columns)
            {
                ColInfoList.Add(new UniDB.Tools.ColumnInfo(DC));
            }

            foreach (UniDB.Tools.ColumnInfo CI in ColInfoList)
            {
                ColumnSetList += CI.ColumName + " = ? , ";//  " = @" + CI.ColumName+ " , ";
            }
            ColumnSetList = ColumnSetList.Substring(0, ColumnSetList.Length - 3) + " ";
            return ColumnSetList;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a SQL string to define Column and values definitions for an Insert Command for a DbCommand
        ///             that is approporate for DT Datatable. </summary>
        ///
        /// <param name="DT">   The DataTable. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        static public string ColumnAndValue_SQLDefine(DataTable DT)
        {
            // SetUp Column Define and Value strings
            string ColumnDefineList = "";
            string ColumnValueList = "";

            List<ColumnInfo> ColInfoList = new List<ColumnInfo>();
            foreach (DataColumn DC in DT.Columns)
            {
                ColInfoList.Add(new UniDB.Tools.ColumnInfo(DC));
            }

            foreach (UniDB.Tools.ColumnInfo CI in ColInfoList)
            {
                ColumnDefineList += CI.ColumName + " , ";
                ColumnValueList += " ? , ";
            }
            // Ok get reid of the last commas
            ColumnDefineList = ColumnDefineList.Substring(0, ColumnDefineList.Length - 3);
            ColumnValueList = ColumnValueList.Substring(0, ColumnValueList.Length - 3);
            // Build COmmand Prepare String
            string CAV_SQLDefine = "("+ ColumnDefineList + " ) VALUES ( " + ColumnValueList + " ); ";
            return CAV_SQLDefine;
        }

        //-------------------------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a select command  for use with a UniDbAdapter object. </summary>
        ///
        /// <param name="DT">           The DataTable. </param>
        /// <param name="Tablename">    The tablename. </param>
        /// <param name="DbCon">        The database con. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        static public DbCommand CreateSelectCommand(DataTable DT, string Tablename, UniDbConnection DbCon)
        {
            DbCommand TempCom = DbCon.CreateCommand();
            string PrepareString = "SELECT "; 
            string ColSQLDefineString = ColumnSelectSQLDefine(DT);
            string CommandSelectString = PrepareString + ColSQLDefineString + "FROM " + UniDB.Tools.BracketIt(DbCon.SQLServerType, Tablename);
            TempCom.CommandText = CommandSelectString;
            return TempCom;
        }
        //-------------------------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a delete command  for use with a UniDbAdapter object. </summary>
        ///
        /// <param name="DT">       The DataTable. </param>
        /// <param name="DbCon">    The database con. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        static public DbCommand CreateDeleteCommand(DataTable DT, string Tablename, UniDbConnection DbCon)
        {
            DbCommand TempCom = DbCon.CreateCommand();
            TempCom.CommandText = "DELETE FROM " + UniDB.Tools.BracketIt(DbCon.SQLServerType, Tablename);
            return TempCom;
        }
        //-------------------------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates an insert command  for use with a UniDbAdapter object. </summary>
        ///
        /// <param name="DT">           The DataTable. </param>
        /// <param name="Tablename">    The tablename. </param>
        /// <param name="DbCon">        The database con. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        static public DbCommand CreateInsertCommand(DataTable DT, string Tablename, UniDbConnection DbCon)
        {
            DbCommand TempCom = DbCon.CreateCommand();
           
            // Set up Prepare String
            string PrepareString = "";
            PrepareString = "INSERT INTO " + UniDB.Tools.BracketIt(DbCon.SQLServerType, Tablename);
            //string ColAndVal_SQLDefineString = ColumnAndValue_SQLDefine(DT);
            // SetUp Column Define and Value strings
            string ColumnDefineList = "";
            string ColumnValueList = "";

            List<ColumnInfo> ColInfoList = new List<ColumnInfo>();
            foreach (DataColumn DC in DT.Columns)
            {
                ColInfoList.Add(new UniDB.Tools.ColumnInfo(DC));
            }

            foreach (UniDB.Tools.ColumnInfo CI in ColInfoList)
            {
                ColumnDefineList += CI.ColumName + " , ";
                ColumnValueList += "@" + CI.ColumName + " , ";//" ? , ";
            }
            // Ok get reid of the last commas
            ColumnDefineList = ColumnDefineList.Substring(0, ColumnDefineList.Length - 3);
            ColumnValueList = ColumnValueList.Substring(0, ColumnValueList.Length - 3);
            // Build COmmand Prepare String
            string CAV_SQLDefine = "(" + ColumnDefineList + " ) VALUES ( " + ColumnValueList + " ); ";
            string CommandInsertString = PrepareString + CAV_SQLDefine;
            TempCom.CommandText = CommandInsertString;
            foreach (UniDB.Tools.ColumnInfo CI in ColInfoList)
            {
                TempCom.Parameters.Add(DbCon.CreateDbParameter("@"+CI.ColumName, CI.DataType, CI.Width, CI.ColumName));
            }
 
            return TempCom;
        }
        //-------------------------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates an update command for use with a UniDbAdapter object. </summary>
        ///
        /// <param name="DT">           The DataTable. </param>
        /// <param name="Tablename">    The tablename. </param>
        /// <param name="DbCon">        The database con. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        static public DbCommand CreateUpdateCommand(List<ColumnInfo> ColInfoList, string WHereClause ,string Tablename, UniDbConnection DbCon)
        {
            DbCommand TempCom = DbCon.CreateCommand();
            // Set up Prepare String
            string PrepareString = "";
            PrepareString = "UPDATE " + UniDB.Tools.BracketIt(DbCon.SQLServerType, Tablename);
            string ColumnSetList = " SET ";

            foreach (UniDB.Tools.ColumnInfo CI in ColInfoList)
            {
                ColumnSetList += CI.ColumName +"=  @" + CI.ColumName+ " , ";
            }
            ColumnSetList = ColumnSetList.Substring(0, ColumnSetList.Length - 3) + " ";

            //string ColSet_SQLDefineString =ColumnSetSQLDefine(DT);
            string TempWhere = "";
            if (WHereClause != "")
            {
                TempWhere = " WHERE " + WHereClause;
            }
            string CommandInsertString = PrepareString + ColumnSetList + TempWhere;
            TempCom.CommandText = CommandInsertString;
            
            foreach (UniDB.Tools.ColumnInfo CI in ColInfoList)
            {
                TempCom.Parameters.Add(DbCon.CreateDbParameter("@"+CI.ColumName, CI.DataType, CI.Width, CI.ColumName));
//                  TempCom.Parameters.Add(DbCon.CreateDbParameter(CI.ColumName, CI.DataType, CI.Width));
            }
            return TempCom;
        }

        static public DbCommand CreateUpdateCommand(DataTable DT, string Tablename, UniDbConnection DbCon)
        {
            //DbCommand TempCom = DbCon.CreateCommand();
            //// Set up Prepare String
            //string PrepareString = "";
            //PrepareString = "UPDATE " + UniDB.Tools.BracketIt(DbCon.SQLServerType, Tablename);
            //string ColumnSetList = " SET ";
            List<ColumnInfo> ColInfoList = new List<ColumnInfo>();
            foreach (DataColumn DC in DT.Columns)
            {
                ColInfoList.Add(new UniDB.Tools.ColumnInfo(DC));
            }
            //foreach (UniDB.Tools.ColumnInfo CI in ColInfoList)
            //{
            //    ColumnSetList += CI.ColumName + "=  @" + CI.ColumName + " , ";
            //}
            //ColumnSetList = ColumnSetList.Substring(0, ColumnSetList.Length - 3) + " ";

            ////string ColSet_SQLDefineString =ColumnSetSQLDefine(DT);
            //string CommandInsertString = PrepareString + ColumnSetList;
            //TempCom.CommandText = CommandInsertString;

            //foreach (UniDB.Tools.ColumnInfo CI in ColInfoList)
            //{
            //    TempCom.Parameters.Add(DbCon.CreateDbParameter("@" + CI.ColumName, CI.DataType, CI.Width, CI.ColumName));
            //}
            // 
            return CreateUpdateCommand(ColInfoList,"",Tablename,DbCon);
        }
    }

    // File Support Class
    // 
#region FileSupport
    
    public static class FileSupport 
    {
        // 
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Combines Two File Paths. </summary>
        ///
        ///
        /// <param name="NewPathtoAdd"> The new path to add. </param>
        /// <param name="OriginalPath"> Original file Path (may contain filename). </param>
        ///
        /// <returns> Full Qualified Path </returns>
        ///-------------------------------------------------------------------------------------------------

        static public string AddPaths(string NewPathtoAdd, string OriginalPath)
        {
            string BackPath = OriginalPath; //NormalizePathDelimiters(OriginalPath);

            string FrontPath = NewPathtoAdd;//NormalizePathDelimiters(NewPathtoAdd);

            if (FrontPath.Length > 0)
                if (FrontPath[FrontPath.Length - 1] != '\\')
                {
                    FrontPath += '\\';
                }

            return FrontPath + BackPath;

        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a directory. </summary>
        /// <param name="directoryName">    Pathname of the directory. </param>
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        static public bool CreateDirectory(string directoryName)
        {
            bool createdok = false;
            try
            {
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(directoryName);
                if (!dir.Exists)
                {
                    dir.Create();
                    createdok = true;
                }
            }
            catch
            {
            }
            return createdok;
        }
    }


#endregion
}
