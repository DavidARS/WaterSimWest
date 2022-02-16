using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.Common;
using UniDB;

namespace txtdb
{
    public class TxtDbDataAdapter : DbDataAdapter
    {
        public const string DeleteCommandString = "DELETE";
        public const string InsertCommandString = "INSERT";
        public const string SelectCommandString = "SELECT";
        public const string UpdateCommandString = "UPDATE";
        public const string CreateCommandString = "CREATE";

        public const string SelectForString = "FOR";
        public const string SelectFromString = "FROM";

        TxtDbCommand FDeleteCommand = null;
        TxtDbCommand FInsertCommand = null;
        TxtDbCommand FSelectCommand = null;
        TxtDbCommand FUpdateCommand = null;

        TxtDbConnection FdbConnection = null;

        string FCommandString = "";

        string FFromString = "";
        string FForString = "";
        string FSQLCommand = "";
        string FWhereString = "";

        // Text file info
        //Tools.DataFormat FFileDataFormat = Tools.DataFormat.Unknown;
        string FDataFilename = "";
        bool FTxtFileReady = false;

        string FErrMEssage = "";
        bool FIsErr = false;

        // File Configuration Info
        Tools.DataFormat FFileFormat = Tools.DataFormat.Unknown;

        bool FUseHeader = false;

        Tools.ColumnInfoList TheColumns = new Tools.ColumnInfoList();

      
               /// <summary>   Default constructor. </summary>
        public TxtDbDataAdapter()
            : base()
        {
            BuildCommands();
        }

      
        private string PrepareAsFieldName(string value)
        {
            string temp = value.Replace("\"","");
            temp = temp.Trim();
            return temp;

        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aCommand">         The command. </param>
        /// <param name="theConnection">    the connection. </param>
        /// <remarks> This dataadapter will assume:
        ///           1) That the file format is unkown, unless otherwise specified with FleFormat  
        ///           2) that the firslline is not fieldname headers, unless spcified with UseFieldHeaders  
        ///            
        ///           if the file format is unknown, then it will try and figure out the format, using second line
        ///           
        ///           All fields are read as text, and numeric data will have to be converted after words
        ///           </remarks>
        ///-------------------------------------------------------------------------------------------------

        public TxtDbDataAdapter(string aCommand, TxtDbConnection theConnection)
            : base()
        {
            FdbConnection = theConnection;
            FUseHeader = theConnection.UseFieldHeader;
            FileFormat = theConnection.FileDataFormat;
            bool iserr = false;
            string errMessage = "";
            SetupAdapter(aCommand, theConnection, out iserr, out errMessage);
            if (iserr)
            {
                throw new Exception(errMessage);
            }
        }

        private void SetupAdapter(string aCommand, TxtDbConnection theConnection, out bool IsErr, out string ErrMessage)
        {
            FdbConnection = theConnection;
            FCommandString = aCommand;
            BuildCommands();
            // parse the command string
            if (ParseSqlSelectText(aCommand, out FSQLCommand, out FForString, out FFromString, out FWhereString))
            {
                FErrMEssage = "";
                FIsErr = false;
                // assume the from string is the filename and Database name is the directory
                FDataFilename = FdbConnection.Database + "\\" + FFromString;
                // figure out the format of the text file
                if (FDataFilename != null)
                {
                    if (File.Exists(FDataFilename))
                    {

                        // check if unknown file format, if unknown try and guess
                        if (FFileFormat == Tools.DataFormat.Unknown)
                        {
                            try
                            {
                                FFileFormat = Tools.GuessDataFormat(FDataFilename);
                            }
                            finally
                            {
                                // OK, if were able to read table and gues dtaformat, it is ready to use
                                if (FFileFormat != Tools.DataFormat.Unknown)
                                    FTxtFileReady = true;
                            }
                        }
                        else
                        {
                            FTxtFileReady = true;
                        }
                    }
                }
                // Ok, lets try and set up for a new DataTable
                string FirstLine = "";
                if (FTxtFileReady)
                {
                    try
                    {
                        using (StreamReader STR = new StreamReader(FDataFilename))
                        {
                            FirstLine = STR.ReadLine();
                        }
                    }
                    catch (Exception e)
                    {
                        FErrMEssage = e.Message;
                        FIsErr = true;
                    }


                    if (FirstLine != "")
                    {
                        List<Tools.DynamicTextData> DataList = Tools.FetchDataFromTextLine(FirstLine, FFileFormat);

                        if (FUseHeader)
                        {
                            TheColumns.Clear();
                            foreach (Tools.DynamicTextData TxtData in DataList)
                            {
                                string Fieldname = PrepareAsFieldName(TxtData.ValueString);

                                // Need to add some checks here!!
                                TheColumns.Add(new Tools.ColumnInfo(Fieldname, Type.GetType("System.String"), 0, false, false, false))
    ;
                            }
                        }
                        else
                        {
                            TheColumns.Clear();
                            int cnt = DataList.Count;
                            for (int i = 0; i < cnt; i++)
                            {
                                TheColumns.Add(new Tools.ColumnInfo("Field" + i.ToString(), Type.GetType("System.String"), 0, false, false, false));
                            }
                        }
                    }//                List<DynamicTextData> DataList = FetchDataFromTextLine(string Line, DataFormat FormatType)

                }
                else
                {
                    FErrMEssage = "Could Not Find File "+FDataFilename;
                    FIsErr = true;
                }
            }
            else
            {
                FErrMEssage = "Bad Command String";
                FIsErr = true;
            }

            IsErr = FIsErr;
            ErrMessage = FErrMEssage;
            
        }

        public Tools.DataFormat FileFormat
        {
            get { return FFileFormat; }
            set { FFileFormat = value; }
        }

        public bool UseFieldHeaders
        {
            get { return FUseHeader; }
            set { FUseHeader = value; }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether this object is error. </summary>
        ///
        /// <value> true if this object is error, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IsError
        {
            get { return FIsErr; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a message describing the error. </summary>
        ///
        /// <value> A message describing the error. </value>
        ///-------------------------------------------------------------------------------------------------

        public string ErrMessage
        {
            get
            {
                if (FIsErr)
                {
                    return FErrMEssage;
                }
                else
                {
                    return "";
                }
            }
        }

        protected virtual void BuildCommands()
        {
            FDeleteCommand = new TxtDbCommand(DeleteCommandString, FdbConnection);
            FInsertCommand = new TxtDbCommand(InsertCommandString, FdbConnection);
            FSelectCommand = new TxtDbCommand(SelectCommandString, FdbConnection);
            FUpdateCommand = new TxtDbCommand(UpdateCommandString, FdbConnection);
        }
        
        // Summary:
        //     The default name used by the System.Data.Common.DataAdapter object for table
        //     mappings.
        
        
        public const string DefaultSourceTableName = "Table";

        //
        // Summary:
        //     Initializes a new instance of a DataAdapter class from an existing object
        //     of the same type.
        //
        // Parameters:
        //   adapter:
        //     A DataAdapter object used to create the new DataAdapter.
        protected TxtDbDataAdapter(TxtDbDataAdapter adapter)
        {
            FCommandString = adapter.FCommandString;
            FdbConnection = adapter.FdbConnection;
            BuildCommands();

        }

        // Summary:
        //     Gets or sets a command for deleting records from the data set.
        //
        // Returns:
        //     An System.Data.IDbCommand used during System.Data.IDataAdapter.Update(System.Data.DataSet)
        //     to delete records in the data source for deleted rows in the data set.

        public DbCommand DeleteCommand {
            get { return FDeleteCommand; }
            set 
            {
                if (value is TxtDbCommand)
                {
                    FDeleteCommand = value as TxtDbCommand;
                }
                else
                {
                    throw new Exception("Must be TxtDbCommand class"); 
                }
            }
        }
        //
        // Summary:
        //     Gets or sets the behavior of the command used to fill the data adapter.
        //
        // Returns:
        //     The System.Data.CommandBehavior of the command used to fill the data adapter.
        protected internal CommandBehavior FillCommandBehavior { get; set; }
        //
        // Summary:
        //     Gets or sets a command used to insert new records into the data source.
        //
        // Returns:
        //     A System.Data.IDbCommand used during System.Data.IDataAdapter.Update(System.Data.DataSet)
        //     to insert records in the data source for new rows in the data set.

        public DbCommand InsertCommand {
            get { return FInsertCommand; }
            set
            {
                if (value is TxtDbCommand)
                {
                    FInsertCommand = value as TxtDbCommand;
                }
                else
                {
                    throw new Exception("Must be TxtDbCommand class");
                }
            }
    
        }
      
        //
        // Summary:
        //     Gets or sets a command used to select records in the data source.
        //
        // Returns:
        //     A System.Data.IDbCommand that is used during System.Data.IDataAdapter.Update(System.Data.DataSet)
        //     to select records from data source for placement in the data set.

        public DbCommand SelectCommand {
            get { return FSelectCommand; }
            set
            {
                if (value is TxtDbCommand)
                {
                    FSelectCommand = value as TxtDbCommand;
                }
                else
                {
                    throw new Exception("Must be TxtDbCommand class");
                }
            }
        }
       
        //
        // Summary:
        //     Gets or sets a value that enables or disables batch processing support, and
        //     specifies the number of commands that can be executed in a batch.
        //
        // Returns:
        //     The number of rows to process per batch. Value isEffect0There is no limit
        //     on the batch size.1Disables batch updating.> 1Changes are sent using batches
        //     of System.Data.Common.DbDataAdapter.UpdateBatchSize operations at a time.When
        //     setting this to a value other than 1 ,all the commands associated with the
        //     System.Data.Common.DbDataAdapter must have their System.Data.IDbCommand.UpdatedRowSource
        //     property set to None or OutputParameters. An exception will be thrown otherwise.
        //[DefaultValue(1)]
        public override int UpdateBatchSize {
            get { return 1; }
            set {}
        }

        //
        // Summary:
        //     Gets or sets a command used to update records in the data source.
        //
        // Returns:
        //     A System.Data.IDbCommand used during System.Data.IDataAdapter.Update(System.Data.DataSet)
        //     to update records in the data source for modified rows in the data set.
 
        public DbCommand UpdateCommand {
            get { return FUpdateCommand; }
            set
            {
                if (value is TxtDbCommand)
                {
                    FUpdateCommand = value as TxtDbCommand;
                }
                else
                {
                    throw new Exception("Must be TxtDbCommand class");
                }
            }
        }

        // Summary:
        //     Adds a System.Data.IDbCommand to the current batch.
        //
        // Parameters:
        //   command:
        //     The System.Data.IDbCommand to add to the batch.
        //
        // Returns:
        //     The number of commands in the batch before adding the System.Data.IDbCommand.
        //
        // Exceptions:
        //   System.NotSupportedException:
        //     The adapter does not support batches.

        protected override int AddToBatch(IDbCommand command)
        {
            throw new Exception("AddToBatch(IDbCommand command) not implemented");
        }
        
        //
        // Summary:
        //     Removes all System.Data.IDbCommand objects from the batch.
        //
        // Exceptions:
        //   System.NotSupportedException:
        //     The adapter does not support batches.

        protected override void ClearBatch()
        {
            throw new Exception("ClearBAtch() not implemented");
        }
 
        //
        // Summary:
        //     Initializes a new instance of the System.Data.Common.RowUpdatedEventArgs
        //     class.
        //
        // Parameters:
        //   dataRow:
        //     The System.Data.DataRow used to update the data source.
        //
        //   command:
        //     The System.Data.IDbCommand executed during the System.Data.IDataAdapter.Update(System.Data.DataSet).
        //
        //   statementType:
        //     Whether the command is an UPDATE, INSERT, DELETE, or SELECT statement.
        //
        //   tableMapping:
        //     A System.Data.Common.DataTableMapping object.
        //
        // Returns:
        //     A new instance of the System.Data.Common.RowUpdatedEventArgs class.
        protected override RowUpdatedEventArgs CreateRowUpdatedEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
        {
            throw new Exception("CreateRowUpdatedEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping) not implemented");
        }
        
        //
        // Summary:
        //     Initializes a new instance of the System.Data.Common.RowUpdatingEventArgs
        //     class.
        //
        // Parameters:
        //   dataRow:
        //     The System.Data.DataRow that updates the data source.
        //
        //   command:
        //     The System.Data.IDbCommand to execute during the System.Data.IDataAdapter.Update(System.Data.DataSet).
        //
        //   statementType:
        //     Whether the command is an UPDATE, INSERT, DELETE, or SELECT statement.
        //
        //   tableMapping:
        //     A System.Data.Common.DataTableMapping object.
        //
        // Returns:
        //     A new instance of the System.Data.Common.RowUpdatingEventArgs class.

        protected override RowUpdatingEventArgs CreateRowUpdatingEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
        {
            throw new Exception("CreateRowUpdatingEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping) not implemented");
        }
        //
        // Summary:
        //     Releases the unmanaged resources used by the System.Data.Common.DbDataAdapter
        //     and optionally releases the managed resources.
        //
        // Parameters:
        //   disposing:
        //     true to release both managed and unmanaged resources; false to release only
        //     unmanaged resources.

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        //
        // Summary:
        //     Executes the current batch.
        //
        // Returns:
        //     The return value from the last command in the batch.

        protected override int ExecuteBatch()
        {
            throw new Exception("ExecuteBatch() not implemented");
        }

        //
        // Summary:
        //     Adds or refreshes rows in the System.Data.DataSet.
        //
        // Parameters:
        //   dataSet:
        //     A System.Data.DataSet to fill with records and, if necessary, schema.
        //
        // Returns:
        //     The number of rows successfully added to or refreshed in the System.Data.DataSet.
        //     This does not include rows affected by statements that do not return rows.

        public override int Fill(DataSet dataSet)
        {

           throw new Exception("Fill(DataSet dataSet) not implemented");
        }

        //
        // Summary:
        //     Adds or refreshes rows in a specified range in the System.Data.DataTable to
        //     match those in the data source using the System.Data.DataTable name.
        //
        // Parameters:
        //   dataTable:
        //     The name of the System.Data.DataTable to use for table mapping.
        //
        // Returns:
        //     The number of rows successfully added to or refreshed in the System.Data.DataSet.
        //     This does not include rows affected by statements that do not return rows.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The source table is invalid.

        public int Fill(DataTable dataTable)
        {
            // right now this code assumes all the fields in the text file are added to the datatable
            // NO SELCTION OF SPECIFIC FIELDS IS SUPPORTED
            // NO FILTERING OF RECORDS IS SUPPORTED
            // 

            bool DoReport = (FdbConnection.ProgressReport != null);
            int RowsRead = 0;
            string ErrMessage = "";
            // check to see if this dataTable was predefined or not 
            if (dataTable == null)
            {
                throw new Exception("dataTable can not be null");
            }
            else
            {
                if (TheColumns.Count == 0)
                {
                   //  Not sure how this can happen  for now throw and exception
                    throw new Exception("Odd error, no columns could be defined from the text data file");
                }
                // This sets up the datatable, based on column information gathered in adapter setup when created
                Tools.BuildTableFromColumnList(dataTable, TheColumns.ColumnInfos, ref ErrMessage);
                // Set the list of fields
                List<Tools.DynamicTextData> DataFields;

                if (FTxtFileReady)
                {
                    try
                    {

                        using (StreamReader SR = new StreamReader(FDataFilename))
                        {
                            int TotalRecords = 0;
                            if (DoReport)
                            {
                                while (!SR.EndOfStream)
                                {
                                    SR.ReadLine();
                                    TotalRecords++;
                                }
                            }
                            SR.BaseStream.Seek(0,SeekOrigin.Begin);
                            
                            string line = "";
                            // skiop the first line if using headers
                            if (FUseHeader)
                            {
                                SR.ReadLine();
                            }
                            // lop through all the lines in the text fiule
                            while ((line = SR.ReadLine()) != null)
                            {
                                RowsRead++;
                                // get the data
                                DataFields = Tools.FetchDataFromTextLine(line, FFileFormat);
                                // create a row
                                DataRow DR = dataTable.NewRow();
                                // loop through the DynamicData list and assign the fields in the data table
                                //int fldcnt = DataFields.Count;
                                int colcnt = dataTable.Columns.Count;
                                int index = 0;
                                foreach (Tools.DynamicTextData DTD in DataFields)
                                {
                                    if (index < colcnt)
                                    {
                                        //if (DTD.ValueString != "")
                                        //{
                                            DR[index] = DTD.ValueString;
                                        //}
                                    }
                                    index++;
                                }
                                // ok now add the row
                                dataTable.Rows.Add(DR);
                                if (DoReport)
                                {
                                    if ((RowsRead % FdbConnection.ProgressReportIncrement) == 0)
                                    {
                                        FdbConnection.ProgressReport(dataTable.TableName+" Line " + RowsRead.ToString("N0"), RowsRead, TotalRecords);
                                    }
                                }
                            }

                        }
                        // all done, no erros
                        FIsErr = false;
                        FErrMEssage = "";
                    }
                    catch (Exception Ex)
                    {
                        // Ouch something happoened will reading
                        // There could be records in the datatable, not sure
                        FIsErr = true;
                        FErrMEssage = "Read Error, File " + FDataFilename + ":" + Ex.Message; ;
                    }
                }
                else
                {
                    // Ouch, file setup did not go well, no records in the datatable
                    FIsErr = true;
                    FErrMEssage = "File " + FDataFilename + " is not ready.";
                }
            }
            return RowsRead;
//            throw new Exception("Fill(DataTable dataTable) not implemented");
        }

        //
        // Summary:
        //     Adds or refreshes rows in the System.Data.DataSet to match those in the data
        //     source using the System.Data.DataSet and System.Data.DataTable names.
        //
        // Parameters:
        //   dataSet:
        //     A System.Data.DataSet to fill with records and, if necessary, schema.
        //
        //   srcTable:
        //     The name of the source table to use for table mapping.
        //
        // Returns:
        //     The number of rows successfully added to or refreshed in the System.Data.DataSet.
        //     This does not include rows affected by statements that do not return rows.
        //
        // Exceptions:
        //   System.SystemException:
        //     The source table is invalid.

        public int Fill(DataSet dataSet, string srcTable)
        {
            throw new Exception("Fill(DataSet dataSet, string srcTable) not implemented");
        }

        //
        // Summary:
        //     Adds or refreshes rows in a System.Data.DataTable to match those in the data
        //     source using the specified System.Data.DataTable, System.Data.IDbCommand
        //     and System.Data.CommandBehavior.
        //
        // Parameters:
        //   dataTable:
        //     A System.Data.DataTable to fill with records and, if necessary, schema.
        //
        //   command:
        //     The SQL SELECT statement used to retrieve rows from the data source.
        //
        //   behavior:
        //     One of the System.Data.CommandBehavior values.
        //
        // Returns:
        //     The number of rows successfully added to or refreshed in the System.Data.DataTable.
        //     This does not include rows affected by statements that do not return rows.

        protected override int Fill(DataTable dataTable, IDbCommand command, CommandBehavior behavior)
        {
            return 0;
        }
        //
        // Summary:
        //     Adds or refreshes rows in a System.Data.DataTable to match those in the data
        //     source starting at the specified record and retrieving up to the specified
        //     maximum number of records.
        //
        // Parameters:
        //   startRecord:
        //     The zero-based record number to start with.
        //
        //   maxRecords:
        //     The maximum number of records to retrieve.
        //
        //   dataTables:
        //     The System.Data.DataTable objects to fill from the data source.
        //
        // Returns:
        //     The number of rows successfully added to or refreshed in the System.Data.DataTable.
        //     This value does not include rows affected by statements that do not return
        //     rows.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startRecord"></param>
        /// <param name="maxRecords"></param>
        /// <param name="dataTables"></param>
        /// <returns></returns>
        public int Fill(int startRecord, int maxRecords, params DataTable[] dataTables)
        {
            throw new Exception("Fill(int startRecord, int maxRecords, params DataTable[] dataTables) not implemented");
            //return 0;
        }
        //
        // Summary:
        //     Adds or refreshes rows in a specified range in the System.Data.DataSet to
        //     match those in the data source using the System.Data.DataSet and System.Data.DataTable
        //     names.
        //
        // Parameters:
        //   dataSet:
        //     A System.Data.DataSet to fill with records and, if necessary, schema.
        //
        //   startRecord:
        //     The zero-based record number to start with.
        //
        //   maxRecords:
        //     The maximum number of records to retrieve.
        //
        //   srcTable:
        //     The name of the source table to use for table mapping.
        //
        // Returns:
        //     The number of rows successfully added to or refreshed in the System.Data.DataSet.
        //     This does not include rows affected by statements that do not return rows.
        //
        // Exceptions:
        //   System.SystemException:
        //     The System.Data.DataSet is invalid.
        //
        //   System.InvalidOperationException:
        //     The source table is invalid.-or- The connection is invalid.
        //
        //   System.InvalidCastException:
        //     The connection could not be found.
        //
        //   System.ArgumentException:
        //     The startRecord parameter is less than 0.-or- The maxRecords parameter is
        //     less than 0.
        
        public int Fill(DataSet dataSet, int startRecord, int maxRecords, string srcTable)
        {
            throw new Exception("Fill(DataSet dataSet, int startRecord, int maxRecords, string srcTable) not implemented");
            return 0;
        }

        //
        // Summary:
        //     Adds or refreshes rows in a specified range in the System.Data.DataSet to
        //     match those in the data source using the System.Data.DataSet and System.Data.DataTable
        //     names.
        //
        // Parameters:
        //   dataTables:
        //     The System.Data.DataTable objects to fill from the data source.
        //
        //   startRecord:
        //     The zero-based record number to start with.
        //
        //   maxRecords:
        //     The maximum number of records to retrieve.
        //
        //   command:
        //     The System.Data.IDbCommand executed to fill the System.Data.DataTable objects.
        //
        //   behavior:
        //     One of the System.Data.CommandBehavior values.
        //
        // Returns:
        //     The number of rows added to or refreshed in the data tables.
        //
        // Exceptions:
        //   System.SystemException:
        //     The System.Data.DataSet is invalid.
        //
        //   System.InvalidOperationException:
        //     The source table is invalid.-or- The connection is invalid.
        //
        //   System.InvalidCastException:
        //     The connection could not be found.
        //
        //   System.ArgumentException:
        //     The startRecord parameter is less than 0.-or- The maxRecords parameter is
        //     less than 0.
        
        protected virtual int Fill(DataTable[] dataTables, int startRecord, int maxRecords, IDbCommand command, CommandBehavior behavior)
        {
            throw new Exception("Fill(DataTable[] dataTables, int startRecord, int maxRecords, IDbCommand command, CommandBehavior behavior) not implemented");
            return 0;
        }

    //
        // Summary:
        //     Adds or refreshes rows in a specified range in the System.Data.DataSet to
        //     match those in the data source using the System.Data.DataSet and source table
        //     names, command string, and command behavior.
        //
        // Parameters:
        //   dataSet:
        //     A System.Data.DataSet to fill with records and, if necessary, schema.
        //
        //   startRecord:
        //     The zero-based record number to start with.
        //
        //   maxRecords:
        //     The maximum number of records to retrieve.
        //
        //   srcTable:
        //     The name of the source table to use for table mapping.
        //
        //   command:
        //     The SQL SELECT statement used to retrieve rows from the data source.
        //
        //   behavior:
        //     One of the System.Data.CommandBehavior values.
        //
        // Returns:
        //     The number of rows successfully added to or refreshed in the System.Data.DataSet.
        //     This does not include rows affected by statements that do not return rows.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The source table is invalid.
        //
        //   System.ArgumentException:
        //     The startRecord parameter is less than 0.-or- The maxRecords parameter is
        //     less than 0.
        
        protected virtual int Fill(DataSet dataSet, int startRecord, int maxRecords, string srcTable, IDbCommand command, CommandBehavior behavior)
        {
            throw new Exception("(DataSet dataSet, int startRecord, int maxRecords, string srcTable, IDbCommand command, CommandBehavior behavior) not implemented");
            return 0;
        }

        //
        // Summary:
        //     Adds a System.Data.DataTable named "Table" to the specified System.Data.DataSet
        //     and configures the schema to match that in the data source based on the specified
        //     System.Data.SchemaType.
        //
        // Parameters:
        //   dataSet:
        //     A System.Data.DataSet to insert the schema in.
        //
        //   schemaType:
        //     One of the System.Data.SchemaType values that specify how to insert the schema.
        //
        // Returns:
        //     A reference to a collection of System.Data.DataTable objects that were added
        //     to the System.Data.DataSet.
        
        public override DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType)
        {
            throw new Exception("FillSchema(DataSet dataSet, SchemaType schemaType) not implemented");
        }

            //
        // Summary:
        //     Configures the schema of the specified System.Data.DataTable based on the
        //     specified System.Data.SchemaType.
        //
        // Parameters:
        //   dataTable:
        //     The System.Data.DataTable to be filled with the schema from the data source.
        //
        //   schemaType:
        //     One of the System.Data.SchemaType values.
        //
        // Returns:
        //     A System.Data.DataTable that contains schema information returned from the
        //     data source.
        
        public DataTable FillSchema(DataTable dataTable, SchemaType schemaType)
        {
            throw new Exception("FillSchema(DataSet dataSet, SchemaType schemaType) not implemented");
        }
        //
        // Summary:
        //     Adds a System.Data.DataTable to the specified System.Data.DataSet and configures
        //     the schema to match that in the data source based upon the specified System.Data.SchemaType
        //     and System.Data.DataTable.
        //
        // Parameters:
        //   dataSet:
        //     A System.Data.DataSet to insert the schema in.
        //
        //   schemaType:
        //     One of the System.Data.SchemaType values that specify how to insert the schema.
        //
        //   srcTable:
        //     The name of the source table to use for table mapping.
        //
        // Returns:
        //     A reference to a collection of System.Data.DataTable objects that were added
        //     to the System.Data.DataSet.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     A source table from which to get the schema could not be found.
        
        public DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType, string srcTable)
        {
            throw new Exception("FillSchema(DataSet dataSet, SchemaType schemaType, string srcTable) not implemented");
        }
        //
        // Summary:
        //     Configures the schema of the specified System.Data.DataTable based on the
        //     specified System.Data.SchemaType, command string, and System.Data.CommandBehavior
        //     values.
        //
        // Parameters:
        //   dataTable:
        //     The System.Data.DataTable to be filled with the schema from the data source.
        //
        //   schemaType:
        //     One of the System.Data.SchemaType values.
        //
        //   command:
        //     The SQL SELECT statement used to retrieve rows from the data source.
        //
        //   behavior:
        //     One of the System.Data.CommandBehavior values.
        //
        // Returns:
        //     A of System.Data.DataTable object that contains schema information returned
        //     from the data source.
        
        protected virtual DataTable FillSchema(DataTable dataTable, SchemaType schemaType, IDbCommand command, CommandBehavior behavior)
        {
            throw new Exception("FillSchema(DataTable dataTable, SchemaType schemaType, IDbCommand command, CommandBehavior behavior)) not implemented");
        }
        
        //
        // Summary:
        //     Adds a System.Data.DataTable to the specified System.Data.DataSet and configures
        //     the schema to match that in the data source based on the specified System.Data.SchemaType.
        //
        // Parameters:
        //   dataSet:
        //     The System.Data.DataSet to be filled with the schema from the data source.
        //
        //   schemaType:
        //     One of the System.Data.SchemaType values.
        //
        //   command:
        //     The SQL SELECT statement used to retrieve rows from the data source.
        //
        //   srcTable:
        //     The name of the source table to use for table mapping.
        //
        //   behavior:
        //     One of the System.Data.CommandBehavior values.
        //
        // Returns:
        //     An array of System.Data.DataTable objects that contain schema information
        //     returned from the data source.
        
        protected virtual DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType, IDbCommand command, string srcTable, CommandBehavior behavior)
        {
            throw new Exception("FillSchema(DataSet dataSet, SchemaType schemaType, IDbCommand command, string srcTable, CommandBehavior behavior) not implemented");
        }
        //
        // Summary:
        //     Returns a System.Data.IDataParameter from one of the commands in the current
        //     batch.
        //
        // Parameters:
        //   commandIdentifier:
        //     The index of the command to retrieve the parameter from.
        //
        //   parameterIndex:
        //     The index of the parameter within the command.
        //
        // Returns:
        //     The System.Data.IDataParameter specified.
        //
        // Exceptions:
        //   System.NotSupportedException:
        //     The adapter does not support batches.

        protected virtual IDataParameter GetBatchedParameter(int commandIdentifier, int parameterIndex)
        {
            throw new Exception("GetBatchedParameter(int commandIdentifier, int parameterIndex) not implemented");
        }
        //
        // Summary:
        //     Returns information about an individual update attempt within a larger batched
        //     update.
        //
        // Parameters:
        //   commandIdentifier:
        //     The zero-based column ordinal of the individual command within the batch.
        //
        //   recordsAffected:
        //     The number of rows affected in the data store by the specified command within
        //     the batch.
        //
        //   error:
        //     An System.Exception thrown during execution of the specified command. Returns
        //     null (Nothing in Visual Basic) if no exception is thrown.
        //
        // Returns:
        //     Information about an individual update attempt within a larger batched update.
        
        protected virtual bool GetBatchedRecordsAffected(int commandIdentifier, out int recordsAffected, out Exception error)
        {
            throw new Exception("GetBatchedRecordsAffected(int commandIdentifier, out int recordsAffected, out Exception error) not implemented");
        }
        
        //
        // Summary:
        //     Gets the parameters set by the user when executing an SQL SELECT statement.
        //
        // Returns:
        //     An array of System.Data.IDataParameter objects that contains the parameters
        //     set by the user.

        public override IDataParameter[] GetFillParameters()
        {
            throw new Exception("GetFillParameters() not implemented");
        }

        //
        // Summary:
        //     Initializes batching for the System.Data.Common.DbDataAdapter.
        //
        // Exceptions:
        //   System.NotSupportedException:
        //     The adapter does not support batches.

        protected virtual void InitializeBatching()
        {
            throw new Exception("InitializeBatching() not implemented");
        }

        //
        // Summary:
        //     Raises the RowUpdated event of a .NET Framework data provider.
        //
        // Parameters:
        //   value:
        //     A System.Data.Common.RowUpdatedEventArgs that contains the event data.

        protected virtual void OnRowUpdated(RowUpdatedEventArgs value)
        {
            throw new Exception("OnRowUpdated(RowUpdatedEventArgs value) not implemented");
        }
 
        //
        // Summary:
        //     Raises the RowUpdating event of a .NET Framework data provider.
        //
        // Parameters:
        //   value:
        //     An System.Data.Common.RowUpdatingEventArgs that contains the event data.

        protected virtual void OnRowUpdating(RowUpdatingEventArgs value)
        {
            throw new Exception("OnRowUpdating(RowUpdatedEventArgs value) not implemented");
        }

        //
        // Summary:
        //     Ends batching for the System.Data.Common.DbDataAdapter.
        //
        // Exceptions:
        //   System.NotSupportedException:
        //     The adapter does not support batches.

        protected virtual void TerminateBatching()
        {
            throw new Exception("TerminateBatching() not implemented");
        }

        //
        // Summary:
        //     Calls the respective INSERT, UPDATE, or DELETE statements for each inserted,
        //     updated, or deleted row in the specified array of System.Data.DataRow objects.
        //
        // Parameters:
        //   dataRows:
        //     An array of System.Data.DataRow objects used to update the data source.
        //
        // Returns:
        //     The number of rows successfully updated from the System.Data.DataSet.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The System.Data.DataSet is invalid.
        //
        //   System.InvalidOperationException:
        //     The source table is invalid.
        //
        //   System.SystemException:
        //     No System.Data.DataRow exists to update.-or- No System.Data.DataTable exists
        //     to update.-or- No System.Data.DataSet exists to use as a source.
        //
        //   System.Data.DBConcurrencyException:
        //     An attempt to execute an INSERT, UPDATE, or DELETE statement resulted in
        //     zero records affected.

        public int Update(DataRow[] dataRows)
        {
            throw new Exception("Update(DataRow[] dataRows) not implemented");
        }
        //
        // Summary:
        //     Calls the respective INSERT, UPDATE, or DELETE statements for each inserted,
        //     updated, or deleted row in the specified System.Data.DataSet.
        //
        // Parameters:
        //   dataSet:
        //     The System.Data.DataSet used to update the data source.
        //
        // Returns:
        //     The number of rows successfully updated from the System.Data.DataSet.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The source table is invalid.
        //
        //   System.Data.DBConcurrencyException:
        //     An attempt to execute an INSERT, UPDATE, or DELETE statement resulted in
        //     zero records affected.

        public override int Update(DataSet dataSet)
        {
            throw new Exception("Update(DataSet dataset) not implemented");
        }
 
        //
        // Summary:
        //     Calls the respective INSERT, UPDATE, or DELETE statements for each inserted,
        //     updated, or deleted row in the specified System.Data.DataTable.
        //
        // Parameters:
        //   dataTable:
        //     The System.Data.DataTable used to update the data source.
        //
        // Returns:
        //     The number of rows successfully updated from the System.Data.DataTable.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The System.Data.DataSet is invalid.
        //
        //   System.InvalidOperationException:
        //     The source table is invalid.
        //
        //   System.SystemException:
        //     No System.Data.DataRow exists to update.-or- No System.Data.DataTable exists
        //     to update.-or- No System.Data.DataSet exists to use as a source.
        //
        //   System.Data.DBConcurrencyException:
        //     An attempt to execute an INSERT, UPDATE, or DELETE statement resulted in
        //     zero records affected.

        public int Update(DataTable dataTable)
        {
            throw new Exception("Update(DataTable dataTable) not implemented");
        }

        //
        // Summary:
        //     Calls the respective INSERT, UPDATE, or DELETE statements for each inserted,
        //     updated, or deleted row in the specified array of System.Data.DataRow objects.
        //
        // Parameters:
        //   dataRows:
        //     An array of System.Data.DataRow objects used to update the data source.
        //
        //   tableMapping:
        //     The System.Data.IDataAdapter.TableMappings collection to use.
        //
        // Returns:
        //     The number of rows successfully updated from the System.Data.DataSet.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The System.Data.DataSet is invalid.
        //
        //   System.InvalidOperationException:
        //     The source table is invalid.
        //
        //   System.SystemException:
        //     No System.Data.DataRow exists to update.-or- No System.Data.DataTable exists
        //     to update.-or- No System.Data.DataSet exists to use as a source.
        //
        //   System.Data.DBConcurrencyException:
        //     An attempt to execute an INSERT, UPDATE, or DELETE statement resulted in
        //     zero records affected.

        protected virtual int Update(DataRow[] dataRows, DataTableMapping tableMapping)
        {
            throw new Exception("Update(DataRow[] dataRows, DataTableMapping tableMapping) not implemented");
        }
 
        //
        // Summary:
        //     Calls the respective INSERT, UPDATE, or DELETE statements for each inserted,
        //     updated, or deleted row in the System.Data.DataSet with the specified System.Data.DataTable
        //     name.
        //
        // Parameters:
        //   dataSet:
        //     The System.Data.DataSet to use to update the data source.
        //
        //   srcTable:
        //     The name of the source table to use for table mapping.
        //
        // Returns:
        //     The number of rows successfully updated from the System.Data.DataSet.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The System.Data.DataSet is invalid.
        //
        //   System.InvalidOperationException:
        //     The source table is invalid.
        //
        //   System.Data.DBConcurrencyException:
        //     An attempt to execute an INSERT, UPDATE, or DELETE statement resulted in
        //     zero records affected.
  
        public int Update(DataSet dataSet, string srcTable)
        {
            throw new Exception("Update(DataSet dataSet, string srcTable) not implemented");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Parse sql text. </summary>
        ///
        /// <param name="sqlText">      The sql text. </param>
        /// <param name="CommandStr">   [in,out] The command string. </param>
        /// <param name="ForStr">       [in,out] for string. </param>
        /// <param name="SourceStr">    [in,out] Source string. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        private bool ParseSqlSelectText(string sqlText, out string CommandStr, out String FromStr, out string SourceStr, out string WhereStr)
        {
            bool result = false;
            FromStr = "";
            SourceStr = "";
            WhereStr = "";
            CommandStr = "";
            string MoreBeyondSource = "";
            // get first Word
            int index = sqlText.IndexOf(" ");
            string str1 = sqlText.Substring(0, index).Trim();
            // Test for command
            CommandStr = str1.ToUpper();
            switch (CommandStr)
            {
                case SelectCommandString:
                    {
                        // EXTRACT SELECT
                        string temp = sqlText.Substring(index + 1).TrimStart();
                        // FIND FROM
                        index = temp.ToUpper().IndexOf(SelectFromString);
                        if (index < 0)
                        {
                            FromStr = "Not Found";
                        }
                        else
                        {
                            // GET FROM SELECTOR
                            FromStr = temp.Substring(0, index).Trim();
                            // Now get the rest
                            temp = temp.Substring(index + 4).TrimStart();
                            // OUCH this is a BUG
                            // This could be bracketed in quotes
                            // Test for quotes
                            temp = temp.Trim();
                            int qindex = 1;
                            if (temp[0] == '\"')
                            {
                                // find ending quote
                                while ((temp[qindex] != '\"') && (qindex < temp.Length))
                                {
                                    qindex++;
                                }
                                // ok now look for space
                                index = temp.IndexOf(" ", qindex);

                            }
                            else
                            {
                                index = temp.IndexOf(" ");
                            }
                            // test to see of there is more after the source string
                            if (index < 1)
                            {
                                index = temp.IndexOf(";");
                            }
                            else
                            {
                                // ok, keep track that there is more to this
                                MoreBeyondSource = temp.Substring(index + 1);
                            }
                            // if not a space or ; then bad form
                            if (index < 1)
                            {
                                SourceStr = "Bad form, should end in a ';'";
                            }
                            else
                            {
                                temp = temp.Substring(0, index).Trim();
                                // ok strip off the quotes if they are there
                                index = temp.IndexOf('\"');
                                if (index > -1)
                                {
                                    temp = temp.Substring(index + 1);
                                    index = temp.IndexOf('\"');
                                    if (index > -1)
                                    {
                                        temp = temp.Substring(0, index);
                                    }
                                }
                                SourceStr = temp;
                                if (MoreBeyondSource == "")
                                {
                                    result = true;
                                }
                                else
                                {
                                    index = temp.IndexOf(";");
                                    if (index < 1)
                                    {
                                        WhereStr = "Bad Form, Should end in a ';'";
                                    }
                                    else
                                    {
                                        WhereStr = temp.Substring(0, index);
                                        result = true;
                                    }

                                }
                            }
                        }
                        break;
                        
                    }
                default:
                    CommandStr = "Not Select Command";
                    break;

            }
            return result;
        }

        
        
    }
    //===============================================================
    //==============================================================
    public class TxtDbParameter : DbParameter
    {
        DbType FDbType = DbType.Object;
        ParameterDirection FParameterDirection = ParameterDirection.Input;
        bool FIsNullable = true;
        string FName = "";
        int FMaxSize = 0;
        string FSourceColumn = "";
        bool FSourceColumnNullMapping = false;
        DataRowVersion FSourceVersion = DataRowVersion.Current;
        object FValue = null;
        byte FPrecision = 0;
        byte FScale = 0;
        //-----------------------------------------
        public TxtDbParameter(): base ()
        {
        }
        //------------------------------------------------------------------
        public TxtDbParameter(string aName, DbType aDataType, int aSize) : base()
        {
            FDbType = aDataType;
            FName = aName;
            int size = Tools.SizeofDbType(aDataType);
            if (size < 0)
            {
                FMaxSize = aSize;
            }
            else
            {
                FMaxSize = size;
            }
        }
        //---------------------------------------------------------------------
        public TxtDbParameter(string parameterName, DbType dbType)
        {
            FDbType = dbType;
            FName = parameterName;
            int size = Tools.SizeofDbType(dbType);
            if (size < 0)
            {
                FMaxSize = 0;
            }
            else
            {
                FMaxSize = size;
            }
        }
        //---------------------------------------------------------------------
        //public TxtDbParameter(string parameterName, object value);
        //---------------------------------------------------------------------      
        public TxtDbParameter(string parameterName, DbType dbType, int size, string sourceColumn)
        {
            FDbType = dbType;
            FName = parameterName;
            FSourceColumn = sourceColumn;
            int testsize = Tools.SizeofDbType(dbType);
            if (testsize < 0)
            {
                FMaxSize = size;
            }
            else
            {
                FMaxSize = testsize;
            }
        }
        //---------------------------------------------------------------------
        public TxtDbParameter(string parameterName, DbType dbType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            FDbType = dbType;
            FName = parameterName;
            FSourceColumn = sourceColumn;
            int testsize = Tools.SizeofDbType(dbType);
            if (testsize < 0)
            {
                FMaxSize = size;
            }
            else
            {
                FMaxSize = testsize;
            }
            FParameterDirection = direction;
            FIsNullable = isNullable;
            FSourceVersion = sourceVersion;
            FValue = value;
            FPrecision = precision;
            FScale = scale;

        }
        // Summary:
        //     Gets or sets the System.Data.DbType of the parameter.
        //
        // Returns:
        //     One of the System.Data.DbType values. The default is System.Data.DbType.String.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     The property is not set to a valid System.Data.DbType.

        public override DbType DbType {
            get { return FDbType; }
            set { FDbType = value; }
        }
        //
        // Summary:
        //     Gets or sets a value that indicates whether the parameter is input-only,
        //     output-only, bidirectional, or a stored procedure return value parameter.
        //
        // Returns:
        //     One of the System.Data.ParameterDirection values. The default is Input.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     The property is not set to one of the valid System.Data.ParameterDirection
        //     values.
        public override ParameterDirection Direction {
            get { return FParameterDirection; }
            set { FParameterDirection = value; }
        }

        
        //
        // Summary:
        //     Gets or sets a value that indicates whether the parameter accepts null values.
        //
        // Returns:
        //     true if null values are accepted; otherwise false. The default is false.
        public override bool IsNullable {
            get { return FIsNullable; }
            set { FIsNullable = value; }
        }
        //
        // Summary:
        //     Gets or sets the name of the System.Data.Common.DbParameter.
        //
        // Returns:
        //     The name of the System.Data.Common.DbParameter. The default is an empty string
        //     ("").

        public override string ParameterName {
            get { return FName; }
            set { FName = value; }
        }
        //
        // Summary:
        //     Gets or sets the maximum size, in bytes, of the data within the column.
        //
        // Returns:
        //     The maximum size, in bytes, of the data within the column. The default value
        //     is inferred from the parameter value.
        public override int Size {
            get { return FMaxSize; }
            set { FMaxSize = value; }
        }

        //
        // Summary:
        //     Gets or sets the name of the source column mapped to the System.Data.DataSet
        //     and used for loading or returning the System.Data.Common.DbParameter.Value.
        //
        // Returns:
        //     The name of the source column mapped to the System.Data.DataSet. The default
        //     is an empty string.

        public override string SourceColumn {
            get { return FSourceColumn; }
            set { FSourceColumn = value; }
        }
        //
        // Summary:
        //     Sets or gets a value which indicates whether the source column is nullable.
        //     This allows System.Data.Common.DbCommandBuilder to correctly generate Update
        //     statements for nullable columns.
        //
        // Returns:
        //     true if the source column is nullable; false if it is not.

        public override bool SourceColumnNullMapping {
            get { return FSourceColumnNullMapping; }
            set { FSourceColumnNullMapping = value; }
        }
        //
        // Summary:
        //     Gets or sets the System.Data.DataRowVersion to use when you load System.Data.Common.DbParameter.Value.
        //
        // Returns:
        //     One of the System.Data.DataRowVersion values. The default is Current.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     The property is not set to one of the System.Data.DataRowVersion values.

        public override DataRowVersion SourceVersion {
            get { return FSourceVersion; }
            set { FSourceVersion = value; }
        }
        //
        // Summary:
        //     Gets or sets the value of the parameter.
        //
        // Returns:
        //     An System.Object that is the value of the parameter. The default value is
        //     null.

        public override object Value {
            get { return FValue; }
            set { FValue = value ;}
        }

        // Summary:
        //     Resets the DbType property to its original settings.
        public override void ResetDbType()
        {
            FDbType = DbType.Object;
        }
    }
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
        public const string CreateCommand = "CREATE";
        public const string CreateTableCommandString = "TABLE";
        public const string CreateDatabaseCommandString = "DATABASE";
        public const string CreateViewCommandString = "VIEW";

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
       
        /// <summary>   Default constructor. </summary>
        public TxtDbCommand() : base()
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="theCommandText">   the command text. </param>
        ///-------------------------------------------------------------------------------------------------

        public TxtDbCommand(string theCommandText) : base()
        {
           FCommandText = theCommandText;
        }

        public TxtDbCommand(string theCommandText, TxtDbConnection theConnection) : base()
        {
          FCommandText = theCommandText;
          FTxtDbConnection = theConnection;
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

        public DbConnection Connection {
            get { return DbConnection; }
            set { DbConnection = value; }
        }
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
        //[DefaultValue("")]
 
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
            return new TxtDbParameter();
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
            bool isError = false;
            string errMessage = "";
            List<string> Tokens = Tools.ParseCommand(FCommandText, out isError, out errMessage);
            if (Tokens.Count > 0)
            {
                string CommandStr = Tokens[0].ToUpper().Trim();
                switch (CommandStr)
                {
                    case CreateCommand:
                        {
                            string CreateWhat = Tokens[1].ToUpper().Trim();

                            switch (CreateWhat)
                            {
                                case CreateTableCommandString:
                                    string Tablename = Tokens[2];
                                    string Defines = Tokens[3];
                                    break;
                                case CreateDatabaseCommandString:
                                    break;
                                case CreateViewCommandString:
                                    break;
                                default:
                                    break;
                            }
                            break;
                        }
                }
            }
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
        const string SCHEMATABLESTRING = "TABLES";

        const string VERSION = "UniDb Ver 2";
        // The Connection string of structure "database=AAA" where AAA is a fully qualified path that points to the directory where text files (tables) will be found
        string FConnectionString = "";
        // fully qualified path that points to the directory where text files (tables) will be found
        string FDBPath = "";
        // list of filters to apply for schema table search
        List<string> FFilters = new List<string>();
        // Default File Formats
        Tools.DataFormat FFileDataFormat = Tools.DataFormat.CommaDelimited;
        // Connection state
        ConnectionState FConnectionState = ConnectionState.Closed;
        // Use FileHeader (first line) as filednames when reading and writing these tables
        bool FUseFileHeader = false;
        // ProgressReporting
        Tools.ProgressReport FPrgReport = null;
        int FProgressReportIncrement = 100;
        
        //----------------------------------------------------
        public TxtDbConnection()
            : base()
        {
            SetDefaultFilters();
        }
        //
        // Summary:
        //     Initializes a new instance of the System.Data.TxtDb.TxtDbConnection class
        //     with the specified connection string.
        //
        // Parameters:
        //   connectionString:
        //     The connection used to open the database.

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Mcquay, 10/20/2015. 
        ///             "DATABASE=A_PATH_TO_TEXTFILES;"
        ///             </remarks>
        ///
        /// <param name="connectionString"> The connection string. </param>
        ///-------------------------------------------------------------------------------------------------

        public TxtDbConnection(string connectionString)
        {
            ParseConnectionString(connectionString);
            SetDefaultFilters();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this object use file header. </summary>
        ///
        /// <value> true if use file header, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool UseFieldHeader
        {
            get { return FUseFileHeader; }
            set { FUseFileHeader = value; }

        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the progress report. </summary>
        ///
        /// <value> The progress report. </value>
        ///-------------------------------------------------------------------------------------------------

        public Tools.ProgressReport ProgressReport
        {
            get { return FPrgReport; }
            set { FPrgReport = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the progress report increment. </summary>
        ///
        /// <value> The progress report increment. </value>
        ///-------------------------------------------------------------------------------------------------

        public int ProgressReportIncrement
        {
            get { return FProgressReportIncrement; }
            set { FProgressReportIncrement = value; }
        }




        private void SetDefaultFilters()
        {
            FFilters.Clear();
            FFilters.Add("*.txt");

            FFilters.Add("*.csv");
        }


        private void ParseConnectionString(string connectStr)
        {
            FConnectionString = connectStr;
            string FileFormatStr = "";
            string temp = connectStr.ToUpper();
            // find the database definition for the pathname
            int index = temp.IndexOf("DATABASE");
            if (index > -1)
            {
                temp = temp.Substring(index);
                index = temp.IndexOf("=");
                if (index > -1)
                {
                    // check and see if a semi colon is included
                    int semiindex = temp.IndexOf(";");
                    if (semiindex > index)
                    {
                        FDBPath = temp.Substring(index+1, (semiindex - index)-1);
                        temp = temp.Substring(semiindex + 1);
                        semiindex = temp.IndexOf(";");
                        index = temp.IndexOf("DATAFORMAT");
                        if (index > -1)
                        {
                            temp = temp.Substring(index);
                            index = temp.IndexOf("=");
                            if (index > -1)
                            {

                                if (semiindex > index)
                                {
                                    FileFormatStr = temp.Substring(index + 1, semiindex - index);

                                }
                                else
                                {
                                    FileFormatStr = temp.Substring(index + 1);
                                }
                                FFileDataFormat = Tools.StringToDataFormat(FileFormatStr);
                            }

                        }
                    }
                    else
                    // ok, no semicolon, just database defined, assume filformat is commadelimited
                    {
                        FDBPath = temp.Substring(index + 1);
                    }
                }
            }
            
        }

        public Tools.DataFormat FileDataFormat
        {
            get { return FFileDataFormat; }
            set { FFileDataFormat = value; }
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
            set
            { 
                ParseConnectionString(value);
            }
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
        //     Gets or Sets the name of the current database or the database to be used after a
        //     connection is opened.
        //
        // Returns:
        //     The name of the current database or the name of the database to be used after
        //     a connection is opened. The default value is an empty string.
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Database 
        { 
            get{ return FDBPath;}
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
            ParseConnectionString("Database=" + value);
        }
        //
        // Summary:
        //     Closes the connection to the data source.
        public override void Close()
        {
            FConnectionState = ConnectionState.Closed;
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

        private DataTable BuildRootSchema()
        {
            DataTable TempTable = new DataTable();
            DataColumn DC = new DataColumn("CollectionName");
            TempTable.Columns.Add(DC);
            
            DataRow DR = TempTable.NewRow();
            DR["CollectionName"] = "Tables";
            TempTable.Rows.Add(DR);
            return TempTable;
        }
        //
        // Summary:
        //     Returns schema information for the data source of this System.Data.TxtDb.TxtDbConnection.
        //
        // Returns:
        //     A System.Data.DataTable that contains schema information.
        public override DataTable GetSchema()
        {
            //Guid MyGuid = new Guid();
            //return GetTxtDbSchemaTable(MyGuid, null);
            return BuildRootSchema();
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

            DataTable TempTable = new DataTable();
            switch (collectionName.ToUpper())
            {
                case SCHEMATABLESTRING:
                    // ok create a DataTable with list of files
                    string TableID = "TABLE";
                    string TableType = "TABLE_TYPE";
                    string TableNameStr = "TABLE_NAME";
                    TempTable.Columns.Add(new DataColumn(TableType, System.Type.GetType("System.String")));
                    TempTable.Columns.Add(new DataColumn(TableNameStr, System.Type.GetType("System.String")));

                    foreach (string Srchstr in FFilters)
                    {
                        string[] TheFiles = Directory.GetFiles(FDBPath,Srchstr);
                        foreach (string Filestr in TheFiles)
                        {
                            string TableName = Path.GetFileName(Filestr);
                            DataRow DR = TempTable.NewRow();
                            DR[TableType] = TableID;
                            DR[TableNameStr] = TableName;
                            TempTable.Rows.Add(DR);
                        }
                    }
                    break;
            }
            return TempTable ;
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
            return GetSchema(collectionName);
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
            if (Directory.Exists(FDBPath))
            {
                FConnectionState = ConnectionState.Open;
            }
            else
            {
                throw new Exception("Database " + FDBPath + " (Directory) not found");
            }
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
