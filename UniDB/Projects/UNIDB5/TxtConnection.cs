using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace UniDb2
{

    //===============================================================
    //==============================================================
    public class TxtDbTransaction : System.Data.Common.DbTransaction
    #region TxtDbtransationRegion
    {
        TxtDbConnection FConnection = null;
        public TxtDbTransaction() :base()
        {
        }

        public DbConnection Connection {
            get { return FConnection; }
        }
        //
        // Summary:
        //     Specifies the System.Data.Common.DbConnection object associated with the
        //     transaction.
        //
        // Returns:
        //     The System.Data.Common.DbConnection object associated with the transaction.
        protected override DbConnection DbConnection
        {
            get { return Connection; }
        }
        //
        // Summary:
        //     Specifies the System.Data.IsolationLevel for this transaction.
        //
        // Returns:
        //     The System.Data.IsolationLevel for this transaction.
        public override IsolationLevel IsolationLevel {
            get { return 0; }
        }

        // Summary:
        //     Commits the database transaction.
        public override void Commit()
        {
        }
        //
        // Summary:
        //     Releases the unmanaged resources used by the System.Data.Common.DbTransaction.
        public void Dispose()
        {
        }
        //
        // Summary:
        //     Releases the unmanaged resources used by the System.Data.Common.DbTransaction
        //     and optionally releases the managed resources.
        //
        // Parameters:
        //   disposing:
        //     If true, this method releases all resources held by any managed objects that
        //     this System.Data.Common.DbTransaction references.
        protected virtual void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        //
        // Summary:
        //     Rolls back a transaction from a pending state.
        public override void Rollback()
        {
        }
    }
    #endregion
    //===============================================================
    //==============================================================
    public class TxtDbCommand : System.Data.Common.DbCommand
    #region TxtDbCommandRegion
    {
        // the Command to Execute
        string FCommandText = "";
        // Timeout value
        int FTimeOut = 0;
        // The Command Text Type
        CommandType FCommandType = CommandType.Text;
        // The Connection
        TxtDbConnection FTxtDbConnection = null;
        // The Transaction
        TxtDbTransaction FTransaction = null;
        // Design Time Visible
        bool FDesignTimeVisible = true;
        // How row is updated
        UpdateRowSource FUpdateRowSOurce = UpdateRowSource.None;
        public TxtDbCommand() : base()
        {
        }

       //Summary:
        //     Gets or sets the text command to run against the data source.
        //
        // Returns:
        //     The text command to execute. The default value is an empty string ("").
        //[DefaultValue("")]
        //[RefreshProperties(RefreshProperties.All)] 
        public override string CommandText {
            get { return FCommandText; }
            set { FCommandText = value; }
        }
        //
        // Summary:
        //     Gets or sets the wait time before terminating the attempt to execute a command
        //     and generating an error.
        //
        // Returns:
        //     The time in seconds to wait for the command to execute.
        public override int CommandTimeout {
            get { return FTimeOut; }
            set { FTimeOut = value; }
        }
        //
        // Summary:
        //     Indicates or specifies how the System.Data.Common.DbCommand.CommandText property
        //     is interpreted.
        //
        // Returns:
        //     One of the System.Data.CommandType values. The default is Text.
        //[RefreshProperties(RefreshProperties.All)]
        public override CommandType CommandType {
            get { return FCommandType; }
            set { FCommandType = value; }
        }
        //
        // Summary:
        //     Gets or sets the System.Data.Common.DbConnection used by this System.Data.Common.DbCommand.
        //
        // Returns:
        //     The connection to the data source.
        //[Browsable(false)]
        //[DefaultValue("")]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DbConnection Connection { get; set; }
        //
        // Summary:
        //     Gets or sets the System.Data.Common.DbConnection used by this System.Data.Common.DbCommand.
        //
        // Returns:
        //     The connection to the data source.
        protected override DbConnection DbConnection {
            get { return FTxtDbConnection; }
            set 
            {
                if (value is TxtDbConnection)
                {
                    FTxtDbConnection = value as TxtDbConnection;
                }
                else
                {
                    throw new Exception("Connection type must be TxtDbConnection");
                }
            }
        }
        //
        // Summary:
        //     Gets the collection of System.Data.Common.DbParameter objects.
        //
        // Returns:
        //     The parameters of the SQL statement or stored procedure.
        protected override DbParameterCollection DbParameterCollection {
            get { return null; } 
        }
        //
        // Summary:
        //     Gets or sets the System.Data.Common.DbCommand.DbTransaction within which
        //     this System.Data.Common.DbCommand object executes.
        //
        // Returns:
        //     The transaction within which a Command object of a .NET Framework data provider
        //     executes. The default value is a null reference (Nothing in Visual Basic).
        protected override DbTransaction DbTransaction
        {
            get { return FTransaction; }
            set
            {
                if (value is TxtDbTransaction)
                {
                    FTransaction = value as TxtDbTransaction;
                }
                else
                {
                    throw new Exception("Must be TxtDbTransaction");
                }
            }
        }

        // Summary:
        //     Gets or sets a value indicating whether the command object should be visible
        //     in a customized interface control.
        //
        // Returns:
        //     true, if the command object should be visible in a control; otherwise false.
        //     The default is true.
        //[Browsable(false)]
        //[DefaultValue(true)]
        //[DesignOnly(true)]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        public override bool DesignTimeVisible {
            get { return FDesignTimeVisible; }
            set { FDesignTimeVisible = value; }
        }
        //
        // Summary:
        //     Gets the collection of System.Data.Common.DbParameter objects. For more information
        //     on parameters, see Configuring Parameters and Parameter Data Types (ADO.NET).
        //
        // Returns:
        //     The parameters of the SQL statement or stored procedure.
        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DbParameterCollection Parameters
        {
            get { return null; }
        }
        //
        // Summary:
        //     Gets or sets the System.Data.Common.DbTransaction within which this System.Data.Common.DbCommand
        //     object executes.
        //
        // Returns:
        //     The transaction within which a Command object of a .NET Framework data provider
        //     executes. The default value is a null reference (Nothing in Visual Basic).
        //[Browsable(false)]
        //[DefaultValue("")]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DbTransaction Transaction {
            get { return FTransaction; }
            set
            {
                if (value is TxtDbTransaction)
                {
                    FTransaction = value as TxtDbTransaction;
                }
                else
                {
                    throw new Exception("Must be TxtDbTransaction");
                }
            }
        }
        //
        // Summary:
        //     Gets or sets how command results are applied to the System.Data.DataRow when
        //     used by the Update method of a System.Data.Common.DbDataAdapter.
        //
        // Returns:
        //     One of the System.Data.UpdateRowSource values. The default is Both unless
        //     the command is automatically generated. Then the default is None.
        public override UpdateRowSource UpdatedRowSource {
            get { return FUpdateRowSOurce; }
            set { FUpdateRowSOurce = value; }
        }

        // Summary:
        //     Attempts to cancels the execution of a System.Data.Common.DbCommand.
        public override void Cancel()
        {
        }
        //
        // Summary:
        //     Creates a new instance of a System.Data.Common.DbParameter object.
        //
        // Returns:
        //     A System.Data.Common.DbParameter object.
        protected override DbParameter CreateDbParameter()
        {
            return null;
        }
        //
        // Summary:
        //     Creates a new instance of a System.Data.Common.DbParameter object.
        //
        // Returns:
        //     A System.Data.Common.DbParameter object.
        public DbParameter CreateParameter()
        {
            return CreateDbParameter();
        }
        //
        // Summary:
        //     Executes the command text against the connection.
        //
        // Parameters:
        //   behavior:
        //     An instance of System.Data.CommandBehavior.
        //
        // Returns:
        //     A task representing the operation.
        //
        // Exceptions:
        //   System.Data.Common.DbException:
        //     An error occurred while executing the command text.
        //
        //   System.ArgumentException:
        //     An invalid System.Data.CommandBehavior value.
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return ExecuteReader(behavior);
        }
        //
        // Summary:
        //     Providers should implement this method to provide a non-default implementation
        //     for Overload:System.Data.Common.DbCommand.ExecuteReader overloads.The default
        //     implementation invokes the synchronous System.Data.Common.DbCommand.ExecuteReader()
        //     method and returns a completed task, blocking the calling thread. The default
        //     implementation will return a cancelled task if passed an already cancelled
        //     cancellation token. Exceptions thrown by ExecuteReader will be communicated
        //     via the returned Task Exception property.This method accepts a cancellation
        //     token that can be used to request the operation to be cancelled early. Implementations
        //     may ignore this request.
        //
        // Parameters:
        //   behavior:
        //     Options for statement execution and data retrieval.
        //
        //   cancellationToken:
        //     The token to monitor for cancellation requests.
        //
        // Returns:
        //     A task representing the asynchronous operation.
        //
        // Exceptions:
        //   System.Data.Common.DbException:
        //     An error occurred while executing the command text.
        //
        //   System.ArgumentException:
        //     An invalid System.Data.CommandBehavior value.
        //protected virtual Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken);
        //
        // Summary:
        //     Executes a SQL statement against a connection object.
        //
        // Returns:
        //     The number of rows affected.
        public override int ExecuteNonQuery()
        {
            return 0;
        }
        //
        // Summary:
        //     An asynchronous version of System.Data.Common.DbCommand.ExecuteNonQuery(),
        //     which executes a SQL statement against a connection object.Invokes System.Data.Common.DbCommand.ExecuteNonQueryAsync(System.Threading.CancellationToken)
        //     with CancellationToken.None.
        //
        // Returns:
        //     A task representing the asynchronous operation.
        //
        // Exceptions:
        //   System.Data.Common.DbException:
        //     An error occurred while executing the command text.
        //public Task<int> ExecuteNonQueryAsync()
        //{
        //    return null;
        //}
        ////
        // Summary:
        //     This is the asynchronous version of System.Data.Common.DbCommand.ExecuteNonQuery().
        //     Providers should override with an appropriate implementation. The cancellation
        //     token may optionally be ignored.The default implementation invokes the synchronous
        //     System.Data.Common.DbCommand.ExecuteNonQuery() method and returns a completed
        //     task, blocking the calling thread. The default implementation will return
        //     a cancelled task if passed an already cancelled cancellation token. Exceptions
        //     thrown by System.Data.Common.DbCommand.ExecuteNonQuery() will be communicated
        //     via the returned Task Exception property.Do not invoke other methods and
        //     properties of the DbCommand object until the returned Task is complete.
        //
        // Parameters:
        //   cancellationToken:
        //     The token to monitor for cancellation requests.
        //
        // Returns:
        //     A task representing the asynchronous operation.
        //
        // Exceptions:
        //   System.Data.Common.DbException:
        //     An error occurred while executing the command text.
        //public virtual Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken);
        //
        // Summary:
        //     Executes the System.Data.Common.DbCommand.CommandText against the System.Data.Common.DbCommand.Connection,
        //     and returns an System.Data.Common.DbDataReader.
        //
        // Returns:
        //     A System.Data.Common.DbDataReader object.
        public DbDataReader ExecuteReader()
        {
            return null;
        }
        //
        // Summary:
        //     Executes the System.Data.Common.DbCommand.CommandText against the System.Data.Common.DbCommand.Connection,
        //     and returns an System.Data.Common.DbDataReader using one of the System.Data.CommandBehavior
        //     values.
        //
        // Parameters:
        //   behavior:
        //     One of the System.Data.CommandBehavior values.
        //
        // Returns:
        //     An System.Data.Common.DbDataReader object.
        public DbDataReader ExecuteReader(CommandBehavior behavior)
        {
            return null;
        }
        //
        // Summary:
        //     An asynchronous version of Overload:System.Data.Common.DbCommand.ExecuteReader,
        //     which executes the System.Data.Common.DbCommand.CommandText against the System.Data.Common.DbCommand.Connection
        //     and returns a System.Data.Common.DbDataReader.Invokes System.Data.Common.DbCommand.ExecuteDbDataReaderAsync(System.Data.CommandBehavior,System.Threading.CancellationToken)
        //     with CancellationToken.None.
        //
        // Returns:
        //     A task representing the asynchronous operation.
        //
        // Exceptions:
        //   System.Data.Common.DbException:
        //     An error occurred while executing the command text.
        //
        //   System.ArgumentException:
        //     An invalid System.Data.CommandBehavior value.
        // public Task<DbDataReader> ExecuteReaderAsync();
        //
        // Summary:
        //     An asynchronous version of Overload:System.Data.Common.DbCommand.ExecuteReader,
        //     which executes the System.Data.Common.DbCommand.CommandText against the System.Data.Common.DbCommand.Connection
        //     and returns a System.Data.Common.DbDataReader. This method propagates a notification
        //     that operations should be canceled.Invokes System.Data.Common.DbCommand.ExecuteDbDataReaderAsync(System.Data.CommandBehavior,System.Threading.CancellationToken).
        //
        // Parameters:
        //   cancellationToken:
        //     The token to monitor for cancellation requests.
        //
        // Returns:
        //     A task representing the asynchronous operation.
        //
        // Exceptions:
        //   System.Data.Common.DbException:
        //     An error occurred while executing the command text.
        //
        //   System.ArgumentException:
        //     An invalid System.Data.CommandBehavior value.
        //public Task<DbDataReader> ExecuteReaderAsync(CancellationToken cancellationToken);
        //
        // Summary:
        //     An asynchronous version of Overload:System.Data.Common.DbCommand.ExecuteReader,
        //     which executes the System.Data.Common.DbCommand.CommandText against the System.Data.Common.DbCommand.Connection
        //     and returns a System.Data.Common.DbDataReader.Invokes System.Data.Common.DbCommand.ExecuteDbDataReaderAsync(System.Data.CommandBehavior,System.Threading.CancellationToken).
        //
        // Parameters:
        //   behavior:
        //     One of the System.Data.CommandBehavior values.
        //
        // Returns:
        //     A task representing the asynchronous operation.
        //
        // Exceptions:
        //   System.Data.Common.DbException:
        //     An error occurred while executing the command text.
        //
        //   System.ArgumentException:
        //     An invalid System.Data.CommandBehavior value.
        //public Task<DbDataReader> ExecuteReaderAsync(CommandBehavior behavior);
        //
        // Summary:
        //     Invokes System.Data.Common.DbCommand.ExecuteDbDataReaderAsync(System.Data.CommandBehavior,System.Threading.CancellationToken).
        //
        // Parameters:
        //   behavior:
        //     One of the System.Data.CommandBehavior values.
        //
        //   cancellationToken:
        //     The token to monitor for cancellation requests.
        //
        // Returns:
        //     A task representing the asynchronous operation.
        //
        // Exceptions:
        //   System.Data.Common.DbException:
        //     An error occurred while executing the command text.
        //
        //   System.ArgumentException:
        //     An invalid System.Data.CommandBehavior value.
        //public Task<DbDataReader> ExecuteReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken);
        //
        // Summary:
        //     Executes the query and returns the first column of the first row in the result
        //     set returned by the query. All other columns and rows are ignored.
        //
        // Returns:
        //     The first column of the first row in the result set.
        public override object ExecuteScalar()
        {
            return null;
        }
        //
        // Summary:
        //     An asynchronous version of System.Data.Common.DbCommand.ExecuteScalar(),
        //     which executes the query and returns the first column of the first row in
        //     the result set returned by the query. All other columns and rows are ignored.Invokes
        //     System.Data.Common.DbCommand.ExecuteScalarAsync(System.Threading.CancellationToken)
        //     with CancellationToken.None.
        //
        // Returns:
        //     A task representing the asynchronous operation.
        //
        // Exceptions:
        //   System.Data.Common.DbException:
        //     An error occurred while executing the command text.
        //public Task<object> ExecuteScalarAsync();
        //
        // Summary:
        //     This is the asynchronous version of System.Data.Common.DbCommand.ExecuteScalar().
        //     Providers should override with an appropriate implementation. The cancellation
        //     token may optionally be ignored.The default implementation invokes the synchronous
        //     System.Data.Common.DbCommand.ExecuteScalar() method and returns a completed
        //     task, blocking the calling thread. The default implementation will return
        //     a cancelled task if passed an already cancelled cancellation token. Exceptions
        //     thrown by ExecuteScalar will be communicated via the returned Task Exception
        //     property.Do not invoke other methods and properties of the DbCommand object
        //     until the returned Task is complete.
        //
        // Parameters:
        //   cancellationToken:
        //     The token to monitor for cancellation requests.
        //
        // Returns:
        //     A task representing the asynchronous operation.
        //
        // Exceptions:
        //   System.Data.Common.DbException:
        //     An error occurred while executing the command text.
        //public virtual Task<object> ExecuteScalarAsync(CancellationToken cancellationToken);
        //
        // Summary:
        //     Creates a prepared (or compiled) version of the command on the data source.
        public override void Prepare()
        {
        }
    }
    #endregion
    //===============================================================
    //==============================================================

    public class TxtDbConnection :  System.Data.Common.DbConnection
    #region TxtDbConnection
    {
        const string VERSION = "UniDb Ver 2";
        // The Connection string is a fully qualified path that points to the directory where text files (tables) will be found
        string FConnectionString = "";
        ConnectionState FConnectionState = ConnectionState.Closed;

        //----------------------------------------------------
        public TxtDbConnection()
            : base()
        {
        }
        //
        // Summary:
        //     Initializes a new instance of the System.Data.TxtDb.TxtDbConnection class
        //     with the specified connection string.
        //
        // Parameters:
        //   connectionString:
        //     The connection used to open the database.
        public TxtDbConnection(string connectionString)
        {
            FConnectionString = connectionString;
        }

        // Summary:
        //     Gets or sets the string used to open a database.
        //
        // Returns:
        //     The Txt DB provider connection string that includes the data source name,
        //     and other parameters needed to establish the initial connection. The default
        //     value is an empty string.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     An invalid connection string argument has been supplied or a required connection
        //     string argument has not been supplied.
        //[DefaultValue("")]
        //[Editor("Microsoft.VSDesigner.Data.ADO.Design.TxtDbConnectionStringEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        //[RecommendedAsConfigurable(true)]
        //[RefreshProperties(RefreshProperties.All)]
        //[SettingsBindable(true)]
        public override string ConnectionString { 
            get { return FConnectionString; }
            set { FConnectionString=value;}
        }
        //
        // Summary:
        //     Gets the time to wait while trying to establish a connection before terminating
        //     the attempt and generating an error.
        //
        // Returns:
        //     The time in seconds to wait for a connection to open. The default value is
        //     15 seconds.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     The value set is less than 0.
            //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override int ConnectionTimeout { 
            get { return 15; }
        }
        //
        // Summary:
        //     Gets the name of the current database or the database to be used after a
        //     connection is opened.
        //
        // Returns:
        //     The name of the current database or the name of the database to be used after
        //     a connection is opened. The default value is an empty string.
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Database { 
            get{ return FConnectionString;}
        }
        //
        // Summary:
        //     Gets the server name or file name of the data source.
        //
        // Returns:
        //     The server name or file name of the data source. The default value is an
        //     empty string.
        //[Browsable(true)]
        public override string DataSource { 
            get { return "";}
        }
        //
        // Summary:
        //     Gets the name of the Txt DB provider specified in the "Provider= " clause
        //     of the connection string.
        //
        // Returns:
        //     The name of the provider as specified in the "Provider= " clause of the connection
        //     string. The default value is an empty string.
        //[Browsable(true)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Provider { 
            get {return "";}
        }
        //
        // Summary:
        //     Gets a string that contains the version of the server to which the client
        //     is connected.
        //
        // Returns:
        //     The version of the connected server.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The connection is closed.
        public override string ServerVersion { 
            get { return VERSION;}
        }



        //
        // Summary:
        //     Gets the current state of the connection.
        //
        // Returns:
        //     A bitwise combination of the System.Data.ConnectionState values. The default
        //     is Closed.
        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ConnectionState State 
        {
            get { return FConnectionState; }
        }

        // Summary:
        //     Occurs when the provider sends a warning or an informational message.
        //public event TxtDbInfoMessageEventHandler InfoMessage;

        protected override System.Data.Common.DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return new TxtDbTransaction();
        }
        //
        // Summary:
        //     Starts a database transaction with the current System.Data.IsolationLevel
        //     value.
        //
        // Returns:
        //     An object representing the new transaction.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     Parallel transactions are not supported.
        public TxtDbTransaction BeginTransaction()
        {
            return new TxtDbTransaction();
        }
        //
        // Summary:
        //     Starts a database transaction with the specified isolation level.
        //
        // Parameters:
        //   isolationLevel:
        //     The isolation level under which the transaction should run.
        //
        // Returns:
        //     An object representing the new transaction.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     Parallel transactions are not supported.
        public TxtDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return new TxtDbTransaction();
        }
        //
        // Summary:
        //     Changes the current database for an open System.Data.TxtDb.TxtDbConnection.
        //
        // Parameters:
        //   value:
        //     The database name.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     The database name is not valid.
        //
        //   System.InvalidOperationException:
        //     The connection is not open.
        //
        //   System.Data.TxtDb.TxtDbException:
        //     Cannot change the database.
        public override void ChangeDatabase(string value)
        {
            FConnectionString = value;
        }
        //
        // Summary:
        //     Closes the connection to the data source.
        public override void Close()
        {
        }
        //
        // Summary:
        //     Creates and returns an System.Data.TxtDb.TxtDbCommand object associated with
        //     the System.Data.TxtDb.TxtDbConnection.
        //
        // Returns:
        //     An System.Data.TxtDb.TxtDbCommand object.
        public TxtDbCommand CreateCommand()
        {
            return new TxtDbCommand();
        }
        //--------------------------------------------------------
        protected override System.Data.Common.DbCommand CreateDbCommand()
        {
            return new TxtDbCommand();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        //
        // Summary:
        //     Enlists in the specified transaction as a distributed transaction.
        //
        // Parameters:
        //   transaction:
        //     A reference to an existing System.EnterpriseServices.ITransaction in which
        //     to enlist.
        //public void EnlistDistributedTransaction(System.EnterpriseServices.ITransaction transaction);
        ////
        //// Summary:
        ////     Enlists in the specified transaction as a distributed transaction.
        ////
        //// Parameters:
        ////   transaction:
        ////     A reference to an existing System.Transactions.Transaction in which to enlist.
        //public override void EnlistTransaction(System.Transactions.Transaction transaction);
        //
        // Summary:
        //     Returns schema information from a data source as indicated by a GUID, and
        //     after it applies the specified restrictions.
        //
        // Parameters:
        //   schema:
        //     One of the System.Data.TxtDb.TxtDbSchemaGuid values that specifies the schema
        //     table to return.
        //
        //   restrictions:
        //     An System.Object array of restriction values. These are applied in the order
        //     of the restriction columns. That is, the first restriction value applies
        //     to the first restriction column, the second restriction value applies to
        //     the second restriction column, and so on.
        //
        // Returns:
        //     A System.Data.DataTable that contains the requested schema information.
        //
        // Exceptions:
        //   System.Data.TxtDb.TxtDbException:
        //     The specified set of restrictions is invalid.
        //
        //   System.InvalidOperationException:
        //     The System.Data.TxtDb.TxtDbConnection is closed.
        //
        //   System.ArgumentException:
        //     The specified schema rowset is not supported by the Txt DB provider.-or-
        //     The schema parameter contains a value of System.Data.TxtDb.TxtDbSchemaGuid.DbInfoLiterals
        //     and the restrictions parameter contains one or more restrictions.
        public DataTable GetTxtDbSchemaTable(Guid schema, object[] restrictions)
        {
            DataTable MySchema = new DataTable("Text Schema");
            return MySchema;
        }
        //
        // Summary:
        //     Returns schema information for the data source of this System.Data.TxtDb.TxtDbConnection.
        //
        // Returns:
        //     A System.Data.DataTable that contains schema information.
        public override DataTable GetSchema()
        {
            Guid MyGuid = new Guid();
            return GetTxtDbSchemaTable(MyGuid, null);
        }
        //
        // Summary:
        //     Returns schema information for the data source of this System.Data.TxtDb.TxtDbConnection
        //     using the specified string for the schema name.
        //
        // Parameters:
        //   collectionName:
        //     Specifies the name of the schema to return.
        //
        // Returns:
        //     A System.Data.DataTable that contains schema information.
        public override DataTable GetSchema(string collectionName)
        {
            return GetSchema();
        }
        //
        // Summary:
        //     Returns schema information for the data source of this System.Data.TxtDb.TxtDbConnection
        //     using the specified string for the schema name and the specified string array
        //     for the restriction values.
        //
        // Parameters:
        //   collectionName:
        //     Specifies the name of the schema to return.
        //
        //   restrictionValues:
        //     Specifies a set of restriction values for the requested schema.
        //
        // Returns:
        //     A System.Data.DataTable that contains schema information.
        public override DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            return GetSchema();
        }
        //
        // Summary:
        //     Opens a database connection with the property settings specified by the System.Data.TxtDb.TxtDbConnection.ConnectionString.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The connection is already open.
        //
        //   System.Data.TxtDb.TxtDbException:
        //     A connection-level error occurred while opening the connection.
        public override void Open()
        {
        }
        //
        // Summary:
        //     Indicates that the System.Data.TxtDb.TxtDbConnection object pool can be released
        //     when the last underlying connection is released.
        public static void ReleaseObjectPool()
        {
        }
        //
        // Summary:
        //     Updates the System.Data.TxtDb.TxtDbConnection.State property of the System.Data.TxtDb.TxtDbConnection
        //     object.
        //[EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetState()
        {
        }
    }
    #endregion
    
}
