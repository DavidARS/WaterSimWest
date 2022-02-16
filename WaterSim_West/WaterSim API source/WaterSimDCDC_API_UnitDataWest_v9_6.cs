using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Drawing;
using UniDB;
using ConsumerResourceModelFramework;
// WaterSimDCDC API UNITDATA WEST 
// Version 8.1
// Routines to load data specific to the west model.  
// This includes USGS region data, Rate data for each region
// Gneeraic routinred for loading Unit based data are included
// 
// Notes
// 2 13 18  QUAY Removed default data file name, only needed for testing
// 9 12 18  QUAY Added fields for Colorado Water
// 4 28 20  QUAY Moved to general source directory and added link for project
// 6 18 20  QUAY Added method to UNITDATA that retrives and int array of data from all units for a specified fieldname
namespace WaterSimDCDC.Generic

{
    //=============================================================================================================
    // FLUX LINK INFO SDResourceConsumerLink  
    // =============================================================================================================

    /// <summary>   Resource consumer link Info 
    ///             class is used to create a list of field info that is used to retrieve data to create the flux list
    ///             </summary>
    public class SDResourceConsumerLink
    {
        string FRes = "";
        string FCons = "";
        string FFlux = "";

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="ResourceField">    The resource field. </param>
        /// <param name="ConsumerField">    The consumer field. </param>
        /// <param name="FluxField">        The flux field. </param>
        ///-------------------------------------------------------------------------------------------------

        public SDResourceConsumerLink(string ResourceField, string ConsumerField, string FluxField)
        {
            FRes = ResourceField;
            FCons = ConsumerField;
            FFlux = FluxField;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the resource. </summary>
        ///
        /// <value> The resource. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Resource
        {
            get { return FRes; }
            set { FRes = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the consumer. </summary>
        ///
        /// <value> The consumer. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Consumer
        {
            get { return FCons; }
            set { FCons = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the flux. </summary>
        ///
        /// <value> The flux. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Flux
        {
            get { return FFlux; }
            set { FFlux = value; }
        }



    }


    //=============================================================================================================
    // UDI UNIT DATA INFO STATIC CLASS  
    // =============================================================================================================

    /// <summary>   Udi. Unit Data Info
    ///             This is used to define teh unique aspects of the USGS data being used
    ///             Key fields would be UnitnameField and unitCodeField.  These Identify the geaographic unit being used in the Data.
    ///             </summary>

    public static class UDI // 
    {
        // ID Fields
        //public const string UnitNameField = "SN";
        //public const string UnitCodeField = "SC";
        public const string RegionUnitNameField = "RN";
        public const string RegionUnitCodeField = "RC";
        public const string StateUnitNameField = "SN";
        public const string StateUnitCodeField = "SC";
        public const string CountyUnitNameField = "CN";
        public const string CountyUnitCodeField = "CC";
        // EDIT QUAY 2 13 18
        // The Model now is using region codes instead of state code
        public const string UnitNameField = RegionUnitNameField;
        public const string UnitCodeField = RegionUnitCodeField;
        // 
        //public const string UnitNameField = StateUnitNameField;
        //public const string UnitCodeField = StateUnitCodeField;
        // Other ID Fields
        public const string LakeField = "LAKE";
        public const string YearField = "YR";
        // Resources
        public const string GroundWaterFld = "GW";
        public const string SurfaceWaterFld = "SUR";
        public const string SurfaceWaterLakeFld = "SURL";
        public const string SurfaceWaterColoradoFld = "SURC";
        public const string ReclaimedWaterFld = "REC";
        public const string SaltWaterFld = "SAL";
        public const string AugmentedFld = "AUG";

        // EDIT QUAY 9/11/18 Colorado
        public const string ColoradoFld = "COL";
        // END EDIT   

        //edits 11.16.21 das
        public const string DesalinationFld = "DSAL";
        //  edits 01.12.22 das
        //edits 01.14.21 das
        // public const string EnvironmentFld = "ENV";
        //  edits 01.14.22 das
        public const string UrbanDesalinationFld = "UDSAL";
        public const string AgDesalinationFld = "ADSAL";
        public const string IndDesalinationFld = "IDSAL";
        public const string PowerDesalinationFld = "PDSAL";
        //  end edits 01.12.22 das
        //end edits 11.16.21 das

        // edits 01.10.22 das
        // 
        public const string PipelineMethodFld = "PIPEMTHD"; // 1=direct use, 2=exchange, 3=pipeline
        public const string PipelineDistanceFld = "PIPEDIST"; // pipeline distance in miles
        // end edits 01.22.22 das

        // end edits 01.22.22 das


        // Consumers
        public const string UrbanDemandFld = "UTOT";
        public const string RuralDemandFld = "RTOT";
        public const string AgricultureDemandFld = "ATOT";
        public const string IndustrialDemandFld = "ITOT";
        public const string PowerDemandFld = "PTOT";

        // edits 01.28.22 das
        public const string EnvironmentDemandFld = "ETOT";
        // end edits 01.28.22 das


        // Other
        public const string PowerGenFld = "PGEN";
        public const string PopulationFld = "POP";

        // Edit : 3/2/2018 - SUHAS
        // Added fields for OPSF, OPGW and OPSURL
        public const string OPSF = "OPSF";
        public const string OPSURL = "OPSURL";
        public const string OPGW = "OPGW";
        //Edit  end : SUHAS - 3/2/2018


        // QUAY EDIT 8/11/18 Colorado
        // Added Colorado to this list
        //public enum eResource { erSurfaceFresh, erSurfaceLake, erGroundwater, erReclained, erSurfaceSaline, erAugmented, erColorado };
        // 01.12.22 das
        // added desalination to this list
        public enum eResource { erSurfaceFresh, erSurfaceLake, erGroundwater, erReclained, erSurfaceSaline, erAugmented, erColorado, erDesalination };

        // END EDIT
        // edits 01.28.22 das
        //public enum eConsumer { ecUrban, ecRural, ecAg, ecInd, ecPower };
        public enum eConsumer { ecUrban, ecRural, ecAg, ecInd, ecPower, ecEnv };
        // end edits 01.28.22 das
        public enum eOther { eoPopulation, eoPowerGen };

        // QUAY EDIT 8/11/18 Colorado
        // Added Colorado to this list
        static public string[] ResourceList = new string[] { SurfaceWaterFld, SurfaceWaterLakeFld, GroundWaterFld, ReclaimedWaterFld, SaltWaterFld, AugmentedFld, ColoradoFld, DesalinationFld };
        // END EDIT


        public enum eDesalData { erPipeLineMethod, erPipeLineDistance }
        static public string[] DesalList = new string[] { PipelineMethodFld, PipelineDistanceFld };

        // EDIT QUAY 3/30/18
        // Changed these labels for Web App Sankey chart, hmmm, not really good
        // Need to think about this
        //static public string[] ResourceListLabel = new string[] { "Surface Water (Fresh)", "Surface Water Lake ", "Groundwater", "Reclaimed Water (effluent)", "Surface Water (Saline)", "Augmented (desal or other)" };

        /// <summary> The resource list label.</summary>
        // QUAY EDIT 8/11/18 Colorado
        // Added Colorado to this list
        static public string[] ResourceListLabel = new string[] { "Surface Water", "Surface Water Lake ", "Groundwater", "Reclaimed Water", "Saline Surface Water", "Augmented", "Colorado", "Desalination" };
        // END EDIT

        // edits 01.28.22 das
        //static public string[] ConsumerList = new string[] { UrbanDemandFld, RuralDemandFld, AgricultureDemandFld, IndustrialDemandFld, PowerDemandFld };
        //static public string[] ConsumerListLabel = new string[] { "Urban Public Supply Demand", "Non-Urban Residential Demand", "Agricultural Demand", "Industrial Demand", "Power Generation Demand" };

        static public string[] ConsumerList = new string[] { UrbanDemandFld, RuralDemandFld, AgricultureDemandFld, IndustrialDemandFld, PowerDemandFld, EnvironmentDemandFld };
        static public string[] ConsumerListLabel = new string[] { "Urban Public Supply Demand", "Non-Urban Residential Demand", "Agricultural Demand", "Industrial Demand", "Power Generation Demand", "Environmental Demand" };

        // end edits 01.28.22 das
        //
        static public string[] OtherListLabel = new string[] { "Population", "Power Generation" };
        static public string[] OtherList = new string[] { PopulationFld, PowerGenFld };

        static public string[] IDFieldList = new string[] { RegionUnitNameField, RegionUnitCodeField, StateUnitNameField, StateUnitCodeField, CountyUnitNameField, CountyUnitCodeField, LakeField, YearField };

        public const string USGSDataFilename = "Just11StatesLakeNoRuralPower.csv";

        public const string DefaultUnitName = "Florida";

        public static int BadIntValue = int.MinValue;
        public static double BadDoubleValue = double.NaN;

    }

    //=============================================================================================================
    // UNIT DATA MANAGER UNITDATA  
    // =============================================================================================================

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Exception for signalling unit data errors. </summary>
    ///
    /// <seealso cref="System.Exception"/>
    ///-------------------------------------------------------------------------------------------------

    public class UnitDataException : Exception
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="message">  The message. </param>
        ///-------------------------------------------------------------------------------------------------

        public UnitDataException(string message)
            : base(message)
        {
        }
    }

    /// <summary>   Unit data. 
    ///             This is the USGS data management class
    ///             Loads the USGS data and provides access to it
    ///             </summary>
    ///             <remarks>  This class is not designed for speed, rather convience.  Thus values from this class should be referenced as little as possible.
    ///                        Thus values access frequently should be accessed once using GetValue and then stored locally
    ///                        Exceptions to this are the Fluxzes and Unitnames lists which are stored localy and provide fast access to a list. 
    ///                        </remarks>
    public class UnitData
    {


        DataTable FDataTable;
        bool FDataLoaded = false;
        string errMessage = "";
        List<SDResourceConsumerLink> FFluxes = new List<SDResourceConsumerLink>();

        protected List<string> FUnitNames = new List<string>();
        // das 08.25.21
        protected List<string> FUnitCodes = new List<string>();
        // end edits
        Color[] ResColors = new Color[] { Color.Aqua, Color.Blue, Color.Cyan, Color.LightSeaGreen, Color.Crimson, Color.DarkGreen, Color.Cornsilk, Color.LightCoral };
        Color[] ConsColors = new Color[] { Color.LightGray, Color.LightCoral, Color.DarkGreen, Color.SandyBrown, Color.LightSkyBlue };

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="Filename"> Filename of the file. </param>
        ///-------------------------------------------------------------------------------------------------

        public UnitData(string Filename)
        {
            bool isErr = false;
            string Database = Path.GetDirectoryName(Filename);
            string TableName = Path.GetFileName(Filename);
            UniDbConnection DbConnect = new UniDbConnection(SQLServer.stText, "", Database, "", "", "");
            DbConnect.Open();
            DbConnect.UseFieldHeaders = true;
            FDataTable = Tools.LoadTable(DbConnect, TableName, ref isErr, ref errMessage);
            if (isErr)
            {
                FDataTable = null;
                FDataLoaded = false;
            }
            else
            {
                FDataLoaded = true;
                FFluxes = ConstructFluxList();
                FUnitNames.Clear();
                foreach (DataRow DR in FDataTable.Rows)
                {
                    string TempName = DR[UDI.UnitNameField].ToString();
                    FUnitNames.Add(TempName);
                }
                //
                FUnitCodes.Clear();
                foreach (DataRow DR in FDataTable.Rows)
                {
                    string TempCode = DR[UDI.UnitCodeField].ToString();
                    FUnitCodes.Add(TempCode);
                }
            }
        }

        public UnitData(string Filename, string SumCodeField, string SumNameField)
        {
            bool isErr = false;
            DataTable RawDataTable = null;
            string Database = Path.GetDirectoryName(Filename);
            string TableName = Path.GetFileName(Filename);
            UniDbConnection DbConnect = new UniDbConnection(SQLServer.stText, "", Database, "", "", "");
            DbConnect.Open();
            DbConnect.UseFieldHeaders = true;
            RawDataTable = Tools.LoadTable(DbConnect, TableName, ref isErr, ref errMessage);
            if (isErr)
            {
                FDataTable = null;
                FDataLoaded = false;
            }
            else
            {
                try
                {
                    FDataTable = BuildSummaryTable(RawDataTable, SumCodeField, SumNameField);
                    FDataLoaded = true;
                    FFluxes = ConstructFluxList();
                    FUnitNames.Clear();
                    foreach (DataRow DR in FDataTable.Rows)
                    {
                        string TempName = DR[UDI.UnitNameField].ToString();
                        FUnitNames.Add(TempName);
                    }
                }
                catch (Exception ex)
                {
                    throw new UnitDataException(ex.Message);
                }
            }

        }


        private bool IsIdField(string aFieldname)
        {
            bool result = false;
            foreach (string fldname in UDI.IDFieldList)
            {
                if (fldname == aFieldname)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private DataTable BuildSummaryTable(DataTable SourceTable, string SumCodeField, string SumNameField)
        {
            DataTable SumTable = null;
            string errMessage = "";
            // ok do some basic erro checks
            if ((!SourceTable.Columns.Contains(SumCodeField)) || (!SourceTable.Columns.Contains(SumNameField)))
            {
                throw new UnitDataException("Unit Data Error: SumField " + SumCodeField + " or " + SumNameField + " Not found!");
            }
            else
            {
                try
                {

                    // create a duolicate table
                    Tools.ColumnInfoList CIList = new Tools.ColumnInfoList(SourceTable);
                    SumTable = Tools.NewTableFromColumnList(SourceTable.TableName + "_Summary", CIList, ref errMessage);
                    // build field list
                    List<string> UniqueUnitCodes = new List<string>();
                    List<string> UniqueUnitNames = new List<string>();
                    // loop through the records finding unique summary field codes
                    foreach (DataRow DR in SourceTable.Rows)
                    {
                        string unitCode = DR[SumCodeField].ToString();
                        if (!UniqueUnitCodes.Contains(unitCode))
                        {
                            UniqueUnitCodes.Add(unitCode);
                            UniqueUnitNames.Add(DR[SumNameField].ToString());

                        }
                    }
                    // Ok make sure there is soemthing there
                    if (UniqueUnitCodes.Count == 0)
                    {
                        throw new UnitDataException("No Unique UnitNames Found for Summary!");
                    }
                    // OK create a sumfield list
                    List<string> SumFields = new List<string>();
                    foreach (DataColumn DC in SumTable.Columns)
                    {
                        if (!IsIdField(DC.ColumnName))
                        {
                            SumFields.Add(DC.ColumnName);
                        }
                    }
                    // EDIT 3/10/17 Bug Found by DAS Moved inside CodeIndex for loop, needs cleared for each new unit
                    //// create a holder for the sum values;
                    //int[] SumValues = new int[SumFields.Count];

                    // Create Summary Records using field list
                    // OK loop through the unitnames
                    for (int CodeIndex = 0; CodeIndex < UniqueUnitCodes.Count; CodeIndex++)
                    {
                        string TargetUnitCode = UniqueUnitCodes[CodeIndex];
                        string TargetUnitName = UniqueUnitNames[CodeIndex];

                        // create a holder for the sum values;
                        int[] SumValues = new int[SumFields.Count];

                        DataRow SumDR = SumTable.NewRow();
                        // now loop through all the data rows, for each unitname
                        foreach (DataRow DR in SourceTable.Rows)
                        {
                            // check if this row is a TargetUnitname
                            if (DR[SumCodeField].ToString() == TargetUnitCode)
                            {
                                // now loop through the SumFields and add them up
                                for (int SumIndex = 0; SumIndex < SumFields.Count; SumIndex++)
                                {
                                    // Setup error
                                    bool isErr = false;
                                    string errMsg = "";
                                    string valueStr = DR[SumFields[SumIndex]].ToString();
                                    int TempVal = Tools.ConvertToInt32(valueStr, ref isErr, ref errMsg);
                                    if (!isErr)
                                    {
                                        SumValues[SumIndex] += TempVal;
                                    }
                                } // for Sumfields
                            } // if code matches
                        } // For Datarows
                        // Ok, now done with all matching unbit records
                        // Loop through the SunFields, assign the value, and add the row,
                        for (int SumIndex = 0; SumIndex < SumFields.Count; SumIndex++)
                        {
                            SumDR[SumFields[SumIndex]] = SumValues[SumIndex].ToString();
                        }
                        // Now assign the UnitCode and Name
                        SumDR[SumCodeField] = TargetUnitCode;
                        SumDR[SumNameField] = TargetUnitName;
                        // Now add it to the table
                        SumTable.Rows.Add(SumDR);
                    } // For the Unit Codes

                } // Try
                catch (Exception ex)
                {
                    throw new UnitDataException("Unit Data Error: " + ex.Message);
                }
            }

            return SumTable;

        }
        public string DesalinationField(UDI.eDesalData value)
        {
            return UDI.DesalList[(int)value];
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Other field. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public string OtherField(UDI.eOther value)
        {
            return UDI.OtherList[(int)value];
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Resource field. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public string ResourceField(UDI.eResource value)
        {
            return UDI.ResourceList[(int)value];
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Resource label. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public string ResourceLabel(UDI.eResource value)
        {
            return UDI.ResourceListLabel[(int)value];
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Consumer field. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public string ConsumerField(UDI.eConsumer value)
        {
            return UDI.ConsumerList[(int)value];
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Consumer label. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public string ConsumerLabel(UDI.eConsumer value)
        {
            return UDI.ConsumerListLabel[(int)value];
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the data was loaded. </summary>
        ///
        /// <value> true if data loaded, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool DataLoaded
        {
            get { return FDataLoaded; }
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the power generated fieldname. </summary>
        ///
        /// <value> The power generated fieldname. </value>
        ///-------------------------------------------------------------------------------------------------

        static public string PowerGeneratedFieldname
        {
            get { return UDI.PowerGenFld; }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the surface water fieldname. </summary>
        ///
        /// <value> The surface water fieldname. </value>
        ///-------------------------------------------------------------------------------------------------

        static public string SurfaceWaterFieldname
        {
            get { return UDI.SurfaceWaterFld; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the ground water fieldname. </summary>
        ///
        /// <value> The ground water fieldname. </value>
        ///-------------------------------------------------------------------------------------------------

        static public string GroundWaterFieldname
        {
            get { return UDI.GroundWaterFld; }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the saline water fieldname. </summary>
        ///
        /// <value> The saline water fieldname. </value>
        ///-------------------------------------------------------------------------------------------------

        static public string SalineWaterFieldname
        {
            get { return UDI.SaltWaterFld; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the reclaimed water fieldname. </summary>
        ///
        /// <value> The reclaimed water fieldname. </value>
        ///-------------------------------------------------------------------------------------------------

        static public string ReclaimedWaterFieldname
        {
            get { return UDI.ReclaimedWaterFld; }
        }

        // QUAY EDIT 9/11/18 Colorado
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the name of the colorado water field. </summary>
        ///
        /// <value> The name of the colorado water field. </value>
        ///-------------------------------------------------------------------------------------------------

        static public string ColoradoWaterFieldName
        {
            get { return UDI.ColoradoFld; }
        }
        // EDNT EDIT

        /// <summary>
        /// 
        /// </summary>
        static public string DesalinationFieldName
        {
            get { return UDI.DesalinationFld; }
        }
        // edits 01.13.22 das
        /// <summary>
        /// 
        /// </summary>
        static public string AugmentedFieldName
        {
            get { return UDI.AugmentedFld; }
        }
        // end edits 01.12.22 das

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the urban water fieldname. </summary>
        ///
        /// <value> The urban water fieldname. </value>
        ///-------------------------------------------------------------------------------------------------

        static public string UrbanWaterFieldname
        {
            get { return UDI.UrbanDemandFld; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the rural water fieldname. </summary>
        ///
        /// <value> The rural water fieldname. </value>
        ///-------------------------------------------------------------------------------------------------

        static public string RuralWaterFieldname
        {
            get { return UDI.RuralDemandFld; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the agriculture fieldname. </summary>
        ///
        /// <value> The agriculture fieldname. </value>
        ///-------------------------------------------------------------------------------------------------

        static public string AgricultureFieldname
        {
            get { return UDI.AgricultureDemandFld; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the industrial water fieldname. </summary>
        ///
        /// <value> The industrial water fieldname. </value>
        ///-------------------------------------------------------------------------------------------------

        static public string IndustrialWaterFieldname
        {
            get { return UDI.IndustrialDemandFld; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the power water fieldname. </summary>
        ///
        /// <value> The power water fieldname. </value>
        ///-------------------------------------------------------------------------------------------------

        static public string PowerWaterFieldname
        {
            get { return UDI.PowerDemandFld; }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the population fieldname. </summary>
        ///
        /// <value> The pop fieldname. </value>
        ///-------------------------------------------------------------------------------------------------

        static public string PopulationFieldname
        {
            get { return UDI.PopulationFld; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the number of units. </summary>
        ///
        /// <value> The number of units. </value>
        ///-------------------------------------------------------------------------------------------------

        public int UnitCount
        {
            get { return FUnitNames.Count; }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a list of names of the units. </summary>
        ///
        /// <value> A list of names of the units. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<string> UnitNames
        {
            get
            {
                return FUnitNames;
            }

        }
        public List<string> UnitCodes
        {
            get
            {
                return FUnitNames;
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Construct flux list. 
        ///             Builds a list of fluxes based on predefined Water Resource Types and Consumer Types
        ///             </summary>
        ///
        ///-------------------------------------------------------------------------------------------------

        public List<SDResourceConsumerLink> ConstructFluxList()
        {
            List<SDResourceConsumerLink> FluxList = new List<SDResourceConsumerLink>();
            SDResourceConsumerLink Temp;
            Temp = new SDResourceConsumerLink("SUR", "UTOT", "USUR"); FluxList.Add(Temp);
            //Temp = new SDResourceConsumerLink("SUR", "RTOT", "RSUR"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("SUR", "ATOT", "ASUR"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("SUR", "ITOT", "ISUR"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("SUR", "PTOT", "PSUR"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("SURL", "UTOT", "USURL"); FluxList.Add(Temp);
            //Temp = new SDResourceConsumerLink("SURL", "RTOT", "RSURL"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("SURL", "ATOT", "ASURL"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("SURL", "ITOT", "ISURL"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("SURL", "PTOT", "PSURL"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("SAL", "UTOT", "USAL"); FluxList.Add(Temp);
            //Temp = new SDResourceConsumerLink("SAL", "RTOT", "RSAL"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("SAL", "ATOT", "ASAL"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("SAL", "ITOT", "ISAL"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("SAL", "PTOT", "PSAL"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("GW", "UTOT", "UGW"); FluxList.Add(Temp);
            //Temp = new SDResourceConsumerLink("GW", "RTOT", "RGW"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("GW", "ATOT", "AGW"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("GW", "ITOT", "IGW"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("GW", "PTOT", "PGW"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("REC", "UTOT", "UREC"); FluxList.Add(Temp);
            //Temp = new SDResourceConsumerLink("REC", "RTOT", "RREC"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("REC", "ATOT", "AREC"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("REC", "ITOT", "IREC"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("REC", "PTOT", "PREC"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("AUG", "UTOT", "UREC"); FluxList.Add(Temp);
            //Temp = new SDResourceConsumerLink("AUG", "RTOT", "RREC"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("AUG", "ATOT", "AREC"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("AUG", "ITOT", "IREC"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("AUG", "PTOT", "PREC"); FluxList.Add(Temp);

            // QUAY EDIT 9/11/18
            // Added Colorado
            Temp = new SDResourceConsumerLink("COL", "UTOT", "UCOL"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("COL", "ATOT", "ACOL"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("COL", "ITOT", "ICOL"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("COL", "PTOT", "PCOL"); FluxList.Add(Temp);
            // END EDIT
            // edits 11.16.21 das
            //Temp = new SDResourceConsumerLink("DSAL", "UTOT", "USAL"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("DSAL", "UTOT", "UDSAL"); FluxList.Add(Temp);
            ////   edits 01.11.22 das
            ///     edits 01.12.22 das
            Temp = new SDResourceConsumerLink("DSAL", "ATOT", "ADSAL"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("DSAL", "ITOT", "IDSAL"); FluxList.Add(Temp);
            //      end edits 01.12.22 das
            Temp = new SDResourceConsumerLink("DSAL", "PTOT", "PDSAL"); FluxList.Add(Temp);
            //   end edits 01.11.22 das
            // end edits 11.16.21 das
            // edits 01.28.22 das
            Temp = new SDResourceConsumerLink("SUR", "ETOT", "ESUR"); FluxList.Add(Temp);
            Temp = new SDResourceConsumerLink("REC", "ETOT", "EREC"); FluxList.Add(Temp);
            // end edits 01.28.22 das


            return FluxList;
        }
        /// <summary>
        /// 
        /// </summary>
        public List<SDResourceConsumerLink> Fluxes
        {
            get { return FFluxes; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value. </summary>
        /// <remarks>   Returns a value from the DataBase based on the Unitname and Fielsname</remarks>
        /// <param name="UnitName">     Name of the unit. </param>
        /// <param name="Field">        The field. </param>
        /// <param name="value">        [out] The value. </param>
        /// <param name="ErrMessage">   [out] Message describing the error. </param>
        ///
        /// <returns>   True if this worked, false if not, if not returns UDI.Badvalue in value and err in ErrMEssage </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool GetValue(string UnitName, string Field, out int value, out string ErrMessage)
        {
            int result = UDI.BadIntValue;
            bool error = true;
            string errMessage = "Data Not Loaded";
            if (FDataLoaded)
            {
                if (FDataTable.Columns.Contains(Field))
                {
                    errMessage = "UnitName Not Found";
                    foreach (DataRow DR in FDataTable.Rows)
                    {
                        string DBUnitName = DR[UDI.UnitNameField].ToString().Trim();
                        if (DBUnitName == UnitName)
                        {
                            errMessage = "Pass";
                            string ValStr = DR[Field].ToString();
                            //double tempD = Tools.ConvertToDouble(ValStr, ref isErr, ref errMessage);
                            int tempint = Tools.ConvertToInt32(ValStr, ref error, ref errMessage);
                            if (!error)
                            {
                                result = tempint;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    errMessage = "Field Not Found";
                }
            }
            else
            {
                ErrMessage = "Data Not Loaded";
            }
            value = result;
            ErrMessage = errMessage;
            return !error;
        }
        // ==================================================================================================================
        // edits 01.27.22 das
        /// <summary>
        ///  Written for the desalination code.If the user wants to use desal water, but the USGS spreadsheet has a value
        /// and method that conflicts, this needed to be rectified. This code changes the datatable to match desire
        ///  from the UI (as of this writing).
        /// </summary>
        /// <param name="UnitName"></param>
        /// <param name="Field"></param>
        /// <param name="wanted"></param>
        /// <param name="dataCheck"></param>
        /// <param name="value"></param>
        /// <param name="ErrMessage"></param>
        /// <returns></returns>
        public bool SetValue(string UnitName, string Field, int wanted, int dataCheck, out int value, out string ErrMessage)
        {
            int result = UDI.BadIntValue;
            bool error = true;
            string errMessage = "Data Not Loaded";
            if (FDataLoaded)
            {
                if (FDataTable.Columns.Contains(Field))
                {
                    errMessage = "UnitName Not Found";
                    foreach (DataRow DR in FDataTable.Rows)
                    {
                        string DBUnitName = DR[UDI.UnitNameField].ToString().Trim();
                        if (DBUnitName == UnitName)
                        {
                            string ValueInt = DR[Field].ToString();
                            int a = Convert.ToInt32(ValueInt);
                            if (a == dataCheck)
                            {
                                DR[Field] = Convert.ToString(wanted);
                            }
                            // data check
                            string valueInt = DR[Field].ToString();

                            errMessage = "Pass";
                            if (!error)
                            {
                                result = wanted;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    errMessage = "Field Not Found";
                }
            }
            else
            {
                ErrMessage = "Data Not Loaded";
            }
            value = result;
            ErrMessage = errMessage;
            return !error;
        }
        /// end edits 01.27.22 das
        /// 


        // =====================================================================================================
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets all int values for a field in datatable.</summary>
        ///
        /// <remarks> Returns an int array of all valumes in a specified column by column name</remarks>
        /// <param name="Field">      The field.</param>
        /// <param name="values">     [out] The values.</param>
        /// <param name="ErrMessage"> [out] Message describing the error.</param>
        ///
        /// <returns> True if it succeeds, false if it fails.</returns>
        ///-------------------------------------------------------------------------------------------------

        public bool GetIntValues(string Field, out int[] values, out string ErrMessage)
        {
            // Setup For Failure
            values = null;
            bool error = true;
            string errMessage = "Data Not Loaded";
            // CHeck if Data laoded
            if (FDataLoaded)
            {
                // Ceck if this is a valid field
                if (FDataTable.Columns.Contains(Field))
                {
                    // create new value array
                    int[] NewValues = new int[FDataTable.Rows.Count];
                    int index = 0;
                    // go through each row and load value
                    foreach (DataRow DR in FDataTable.Rows)
                    {
                        // get field string
                        string ValStr = DR[Field].ToString();
                        // try converting to an int
                        int tempint = Tools.ConvertToInt32(ValStr, ref error, ref errMessage);
                        // conversion good set value in new array
                        if (!error)
                        {
                            NewValues[index] = tempint;
                        }
                        // otherwise put bad value in array
                        else
                        {
                            NewValues[index] = UDI.BadIntValue;
                        }
                        index++;
                    }
                    // OK this worked
                    // Assign new values to out variable
                    values = NewValues;
                    error = false;
                }
                else
                {
                    errMessage = "Field Not Found";
                }
            }
            else
            {
                ErrMessage = "Data Not Loaded";
            }
            // set errormessage
            ErrMessage = errMessage;
            return !error;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets double values.</summary>
        ///
        /// <remarks> Returns a double array of all values in a specified column by column name</remarks>
        /// <param name="Field">      The field.</param>
        /// <param name="values">     [out] The values.</param>
        /// <param name="ErrMessage"> [out] Message describing the error.</param>
        ///
        /// <returns> True if it succeeds, false if it fails.</returns>
        ///-------------------------------------------------------------------------------------------------

        public bool GetDoubleValues(string Field, out double[] values, out string ErrMessage)
        {
            // Setup For Failure
            values = null;
            bool error = true;
            string errMessage = "Data Not Loaded";
            // CHeck if Data laoded
            if (FDataLoaded)
            {
                // Ceck if this is a valid field
                if (FDataTable.Columns.Contains(Field))
                {
                    // create new value array
                    double[] NewValues = new double[FDataTable.Rows.Count];
                    int index = 0;
                    // go through each row and load value
                    foreach (DataRow DR in FDataTable.Rows)
                    {
                        // get field string
                        string ValStr = DR[Field].ToString();
                        // try converting to an int
                        double tempdouble = Tools.ConvertToDouble(ValStr, ref error, ref errMessage);
                        // conversion good set value in new array
                        if (!error)
                        {
                            NewValues[index] = tempdouble;
                        }
                        // otherwise put bad value in array
                        else
                        {
                            NewValues[index] = UDI.BadIntValue;
                        }
                        index++;

                    }
                    // OK this worked
                    // Assign new values to out variable
                    values = NewValues;
                    error = false;
                }
                else
                {
                    errMessage = "Field Not Found";
                }
            }
            else
            {
                ErrMessage = "Data Not Loaded";
            }
            // set errormessage
            ErrMessage = errMessage;
            return !error;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds a network. </summary>
        ///
        /// <param name="UnitName"> Name of the unit. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Network BuildNetwork(string UnitName)
        {
            bool isError = false;
            string errMessage = "";
            CRF_Network TheNetwork = null;
            int Value = 0;
            if (FDataLoaded)
            {
                // build resources list
                CRF_ResourceList ResList = new CRF_ResourceList();
                for (int i = 0; i < UDI.ResourceList.Length; i++)
                {
                    if (GetValue(UnitName, UDI.ResourceList[i], out Value, out errMessage))
                    {
                        CRF_Resource TempRes = new CRF_Resource(UDI.ResourceList[i], UDI.ResourceListLabel[i], ResColors[i], Value);
                        ResList.Add(TempRes);
                    }
                    else
                    {
                        int Test = 1;
                    }
                }
                // Build Consumer List
                CRF_ConsumerList ConsList = new CRF_ConsumerList();
                for (int i = 0; i < UDI.ConsumerList.Length; i++)
                {
                    if (GetValue(UnitName, UDI.ConsumerList[i], out Value, out errMessage))
                    {
                        CRF_Consumer TempCons = new CRF_Consumer(UDI.ConsumerList[i], UDI.ConsumerListLabel[i], ConsColors[i], Value);
                        ConsList.Add(TempCons);
                    }
                }
                // Ok now the hard part, add fluxes
                // Go through each of the resources
                foreach (CRF_Resource Res in ResList)
                {
                    // go through each of the SDResourceConsumerLinks looking for a match
                    foreach (SDResourceConsumerLink RCL in FFluxes)
                    {
                        // found one
                        if (Res.Name == RCL.Resource)
                        {
                            // lookin for a match consumer
                            foreach (CRF_Consumer Cons in ConsList)
                            {
                                // found it. add this flux
                                if (RCL.Consumer == Cons.Name)
                                {
                                    if (GetValue(UnitName, RCL.Flux, out Value, out errMessage))
                                    {
                                        Res.AddConsumer(Cons, Value, CRF_Flux.Method.amAbsolute);
                                    }
                                }
                            }
                        }
                    }
                }
                TheNetwork = new CRF_Network("WaterSim_" + UnitName, ResList, ConsList, null);
            }
            return TheNetwork;
        }

        //+++++++++++++++++++++++++++++++++++++++
        // NOTE QUAY 9/8/20
        // I am not sure what this method was for, it is not used anywhere
        // IT only returns fields in IDFieldList = new string[] { RegionUnitNameField, RegionUnitCodeField, StateUnitNameField, StateUnitCodeField, CountyUnitNameField, CountyUnitCodeField, LakeField, YearField };
        // Have no idead what that is.  Left it here but created new method getValues()
        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Base unit data.</summary>
        ///
        /// <remarks> retrieves an array of int values for specified fields in the UnitData data table.
        ///           This is intended to be fast and assumes that the fieldname specified is  an column of integre values.
        ///           if not then returns a -1;
        ///           if Unitdata empty (no units) or fieldname is not a field, then returns a null value</remarks>
        ///
        /// <param name="fieldname"> The fieldname.</param>
        ///
        /// <returns> An int[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] BaseUnitData(string fieldname)
        {
            int[] data = null;
            if ((UnitCount >= 0) && IsIdField(fieldname))
            {
                data = new int[UnitCount];
                int rowcnt = 0;
                foreach (DataRow DR in FDataTable.Rows)
                {
                    try
                    {
                        data[rowcnt] = (int)DR[fieldname];
                    }
                    catch
                    {
                        data[rowcnt] = -1;
                    }
                    rowcnt++;
                }
            }
            return data;
        }
    }

    //=============================================================================================================
    // RATE DATA MANAGER
    // =============================================================================================================

    /// <summary>   Rate data. </summary>
    public struct RateData
    {
        string FUnitName;
        string FUnitCodeStr;
        int FUnitCode;
        double FAgRate;
        double FIndRate;
        double FPopRate;
        double FAgNet;
        // =================
        // 06.29.18
        double FAgRateLCLU;
        double FIndRateLCLU;
        double FUrbanRateLCLU;

        // =================

        // edits 09.21.21 das
        // ======================
        private double FUHigh;
        double FULow;
        double FSub;
        double FExUH;
        double FExUL;

        // ===========
        readonly double fUHigh;
        readonly double fULow;
        readonly double fSub;
        readonly double fExUH;
        readonly double fExUL;
        // ======================
        // end edits das 09.21.21


        // =====================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aUnitName"></param>
        /// <param name="aUnitCode"></param>
        /// <param name="anAgRate"></param>
        /// <param name="anIndRate"></param>
        /// <param name="aPopRate"></param>
        /// <param name="anAgNet"></param>
        /// <param name="anAgRateLCLU"></param>
        /// <param name="anUrbanRateLCLU"></param>
        /// <param name="anIndRateLCLU"></param>
        /// <param name="fUHigh"></param>
        /// <param name="fULow"></param>
        /// <param name="fSub"></param>
        /// <param name="fExUH"></param>
        /// <param name="fExUL"></param>
        public RateData(string aUnitName, string aUnitCode, double anAgRate, double anIndRate, double aPopRate,
            double anAgNet, double anAgRateLCLU, double anUrbanRateLCLU, double anIndRateLCLU, double fUHigh,
            double fULow, double fSub, double fExUH, double fExUL) : this()
        {
            bool isErr = false;
            string errMsg = "";

            FUnitName = aUnitName;
            FUnitCodeStr = aUnitCode;

            int temp = Tools.ConvertToInt32(FUnitCodeStr, ref isErr, ref errMsg);
            if (!isErr)
            {
                FUnitCode = temp;
            }
            else
            {
                FUnitCode = UDI.BadIntValue;
            }
            //
            FAgRate = anAgRate;
            FIndRate = anIndRate;
            FPopRate = aPopRate;
            FAgNet = anAgNet;
            //
            FAgRateLCLU = anAgRateLCLU;
            FIndRateLCLU = anIndRateLCLU;
            FUrbanRateLCLU = anUrbanRateLCLU;
            //
            FUHigh = fUHigh;
            FULow = fULow;
            FSub = fSub;
            FExUH = fExUH;
            FExUL = fExUL;
            //
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the name of the unit. </summary>
        ///
        /// <value> The name of the unit. </value>
        ///-------------------------------------------------------------------------------------------------

        public string UnitName
        {
            get { return FUnitName; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the unit code string. </summary>
        ///
        /// <value> The f unit code string. </value>
        ///-------------------------------------------------------------------------------------------------

        public string UnitCodeStr
        {
            get { return FUnitCodeStr; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the code. </summary>
        ///
        /// <value> The code. </value>
        ///-------------------------------------------------------------------------------------------------

        public int Code
        {
            get { return FUnitCode; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the ag rate. </summary>
        ///
        /// <value> The ag rate. </value>
        ///-------------------------------------------------------------------------------------------------

        public double AgRate
        {
            get { return FAgRate; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the ind rate. </summary>
        ///
        /// <value> The ind rate. </value>
        ///-------------------------------------------------------------------------------------------------

        public double IndRate
        {
            get { return FIndRate; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the pop rate. </summary>
        ///
        /// <value> The pop rate. </value>
        ///-------------------------------------------------------------------------------------------------

        public double PopRate
        {
            get { return FPopRate; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the ag net. </summary>
        ///
        /// <value> The ag net. </value>
        ///-------------------------------------------------------------------------------------------------

        public double AgNet
        {
            get { return FAgNet; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double AgRateLCLU
        {
            get { return FAgRateLCLU; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double IndRateLCLU
        {
            get { return FIndRateLCLU; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double UrbanRateLCLU
        {
            get { return FUrbanRateLCLU; }
        }
        // =========================================
        public double UrbanRateHigh
        {
            get { return FUHigh; }
        }
        public double UrbanRateLow
        {
            get { return FULow; }
        }
        public double SuburbanRate
        {
            get { return FSub; }
        }
        public double ExurbanRateHigh
        {
            get { return FExUH; }
        }
        public double ExurbanRateLow
        {
            get { return FExUL; }
        }
        // =========================================
    }

    /// <summary>
    /// 
    /// </summary>
    public class RateDataClass
    {
        // DataTable Parameters
        DataTable TheData = null;
        string FDataDirectory = "";
        // EDIT QUAY 2 13 18
        // Removed default filename, there is non now, only needed for testing
        //string FFilename = "ElevenStateGrowthRates2.csv";
        //string FFilename = "ElevenStateGrowthRates3.csv";
        //string FFilename = "WestRegionRates_2.csv";
        string FFilename = "";
        // EDIT end 2 13 18

        const string FScodeFieldStr = "SC";
        const string FSnameFieldStr = "SN";
        // EDIT QUAY 2/19/18
        // Added Region Filed strings and setup to use State or Region
        const string FRnameFieldStr = "RN";
        const string FRcodeFieldStr = "RC";

        string FNameFieldStr = FRnameFieldStr;
        string FCodeFieldStr = FRcodeFieldStr;
        // END QUAY 2/19/18 Edit

        string FPOPRateFieldStr = "POPGRATE";
        string FINDRateFieldStr = "INDGRATE";
        string FAGRateFieldStr = "AGGRATE";
        string FAgNetFieldStr = "AGNET";
        //
        string FUHRateFieldStr = "UrbanHigh";
        string FULRateFieldStr = "UrbanLow";
        string FSUBRateFieldStr = "Suburban";
        string FExUHRateFieldStr = "ExurbanHigh";
        string FExULRateFieldStr = "ExurbanLow";
        //
        // 10.22.21 - From Ray Quay email
        //
        //I have a “WestModelGrowthRates_4.csv” (9/6/18) file that I created with base growth rates 
        //for Population, Ag, and Industrial.It has the following Columns RC, RN, POPGRATE, INDGRATE, 
        //AGGRATE, AGNET., RURBRATE, RAGRATE, RINDRATE.Population is based on projections we found for 
        //planning agency’s in the various regions and states, POPGRATE.Agriculture is based on USDA 
        //Net Farm Income (AGNET) growth (AGGRATE) rates over last few decades.  Ag growth is in Net Farm 
        //Income and the water per $ income is estimated in the first year using USGS initial AG water use.
        //I thought industrial was based on industrial job growth rate (INDGRATE) and water per job was 
        //again estimated at the start, but I do not think that is the case.




        // ========================================
        // 06.29.18 LCLU rates
        // 05.28.21 das
        // What are the units? Where did these come from? 05.28.21
        // Million gallons per day per acre
        // SSp2 scenario acerage from ICLUS 2
        //----------------------------------------
        string FUubanRateLCLUFieldStr = "RURBRATE";
        string FINDRateLCLUFieldStr = "RINDRATE";
        string FAGRateLCLUFieldStr = "RAGRATE";
        // ========================================
        //

        // Data Array Parameters

        Dictionary<string, int> StateCodes = new Dictionary<string, int>();
        const double InvalidRate = -1;//double.NaN;

        double[] FAgRateArray = null;
        double[] FIndRateArray = null;
        double[] FPopRateArray = null;
        double[] FAgNetArray = null;
        // ===========================
        // 06.29.18 LCLU arrays
        double[] FRAgRateLCLUArray = null;
        double[] FRIndRateLCLUArray = null;
        double[] FRUrbanRateLCLUArray = null;
        // ===========================
        double[] FRUrbanHighRateArray = null;
        double[] FRUrbanLowRateArray = null;
        double[] FRSuburbanRateArray = null;
        double[] FRExurbanHighRateArray = null;
        double[] FRExurbanLowRateArray = null;

        // ====================================

        List<RateData> FRateDataList = new List<RateData>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectory"></param>
        /// <param name="Filename"></param>
        public RateDataClass(string DataDirectory, string Filename)
        {
            string errMessage = "";
            bool isErr = false;
            FDataDirectory = DataDirectory;
            FFilename = Filename;
            UniDbConnection DbCon = new UniDbConnection(SQLServer.stText, "", FDataDirectory, "", "", "");
            DbCon.UseFieldHeaders = true;
            DbCon.Open();
            TheData = Tools.LoadTable(DbCon, FFilename, ref isErr, ref errMessage);
            if (isErr)
            {
                throw new Exception("Error loading Rate Data. " + errMessage);
            }
            // build data arrays
            int arraysize = TheData.Rows.Count;
            FAgRateArray = new double[arraysize];
            FIndRateArray = new double[arraysize];
            FPopRateArray = new double[arraysize];
            FAgNetArray = new double[arraysize];
            // ===================================
            // 06.29.18
            FRAgRateLCLUArray = new double[arraysize];
            FRIndRateLCLUArray = new double[arraysize];
            FRUrbanRateLCLUArray = new double[arraysize];
            // ============================================
            FRUrbanHighRateArray = new double[arraysize];
            FRUrbanLowRateArray = new double[arraysize];
            FRSuburbanRateArray = new double[arraysize];
            FRExurbanHighRateArray = new double[arraysize];
            FRExurbanLowRateArray = new double[arraysize];
            // ===========================================
            int CodeI = 0;
            foreach (DataRow DR in TheData.Rows)
            {
                // Get name and code
                // EDIT QUAY 2/19/18
                // Setup to use region or state codes
                //string namestr = DR[FSnameFieldStr].ToString();
                //string codestr = DR[FScodeFieldStr].ToString();
                string namestr = DR[FNameFieldStr].ToString();
                string codestr = DR[FCodeFieldStr].ToString();
                // END EDIT

                // Decided not to use code in DataTable
                // int codeint = Tools.ConvertToInt32(codestr,ref isErr,ref errMessage);



                if (!isErr)
                {

                    string agratestr = DR[FAGRateFieldStr].ToString();
                    string popratestr = DR[FPOPRateFieldStr].ToString();
                    string indratestr = DR[FINDRateFieldStr].ToString();
                    string agnetstr = DR[FAgNetFieldStr].ToString();
                    //
                    // =======================================================
                    // 06.29.18
                    string agrateLCLUstr = DR[FAGRateLCLUFieldStr].ToString();
                    string urbanrateLCLUstr = DR[FUubanRateLCLUFieldStr].ToString();
                    string indrateLCLUstr = DR[FINDRateLCLUFieldStr].ToString();
                    // =======================================================
                    string urbanHighstr = DR[FUHRateFieldStr].ToString();
                    string urbanLowstr = DR[FULRateFieldStr].ToString();
                    string suburbanstr = DR[FSUBRateFieldStr].ToString();
                    string exurbanHighstr = DR[FExUHRateFieldStr].ToString();
                    string exurbanLowstr = DR[FExULRateFieldStr].ToString();

                    //
                    double TempAg = Tools.ConvertToDouble(agratestr, ref isErr, ref errMessage);
                    if (!isErr)
                    {
                        double TempPop = Tools.ConvertToDouble(popratestr, ref isErr, ref errMessage);
                        if (!isErr)
                        {
                            double TempInd = Tools.ConvertToDouble(indratestr, ref isErr, ref errMessage);
                            if (!isErr)
                            {
                                double TempAgNet = Tools.ConvertToDouble(agnetstr, ref isErr, ref errMessage);
                                if (!isErr)
                                {
                                    double TempAgLCLU = Tools.ConvertToDouble(agrateLCLUstr, ref isErr, ref errMessage);
                                    if (!isErr)
                                    {
                                        double TempULCLU = Tools.ConvertToDouble(urbanrateLCLUstr, ref isErr, ref errMessage);
                                        if (!isErr)
                                        {
                                            double TempIndLCLU = Tools.ConvertToDouble(indrateLCLUstr, ref isErr, ref errMessage);
                                            if (!isErr)
                                            {
                                                double TempUH = Tools.ConvertToDouble(urbanHighstr, ref isErr, ref errMessage);
                                                if (!isErr)
                                                {
                                                    double TempUL = Tools.ConvertToDouble(urbanLowstr, ref isErr, ref errMessage);
                                                    if (!isErr)
                                                    {
                                                        double TempS = Tools.ConvertToDouble(suburbanstr, ref isErr, ref errMessage);
                                                        if (!isErr)
                                                        {
                                                            double TempExH = Tools.ConvertToDouble(exurbanHighstr, ref isErr, ref errMessage);
                                                            if (!isErr)
                                                            {
                                                                double TempExL = Tools.ConvertToDouble(exurbanLowstr, ref isErr, ref errMessage);
                                                                if (!isErr)
                                                                {
                                                                    // OK Everything is GOOD let's do it
                                                                    RateData RD = new RateData(namestr, codestr, TempAg, TempInd, TempPop,
                                                                        TempAgNet, TempAgLCLU, TempULCLU, TempIndLCLU,
                                                                        TempUH, TempUL, TempS, TempExH, TempExL);
                                                                    FRateDataList.Add(RD);
                                                                    //// add to dictionary 
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }

                            }
                        }
                    }
                }

            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Ag rate.</summary>
        ///
        /// <param name="State"> The state.</param>
        ///
        /// <returns> A double.</returns>
        ///-------------------------------------------------------------------------------------------------

        public double AGRate(int State)
        {
            double result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string statecode = DR[FScodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(statecode, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == State)
                    {
                        string valstr = DR[FAGRateFieldStr].ToString();
                        double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = tempDbl;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets data array From the DataTable for all Units.</summary>
        ///
        /// <remarks> Quay, 3/3/2018.</remarks>
        ///
        /// <param name="FieldStr"> The Column Name of the DataFIeld.</param>
        ///
        /// <returns> An array of double.</returns>
        ///-------------------------------------------------------------------------------------------------

        private double[] GetDataArray(string FieldStr)
        {
            double[] result = new double[TheData.Rows.Count];
            int cnt = 0;
            double tempDbl = 0;
            foreach (DataRow DR in TheData.Rows)
            {
                string valstr = DR[FieldStr].ToString();
                if (double.TryParse(valstr, out tempDbl)) result[cnt] = tempDbl;
                else result[cnt] = double.NaN;
                cnt++;
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Get the Rate of Growth For AGriculture for all units..</summary>
        ///
        /// <remarks> Quay, 3/3/2018.</remarks>
        ///
        /// <returns> A double.</returns>
        ///-------------------------------------------------------------------------------------------------

        public double[] AGRate()
        {
            return GetDataArray(FAGRateFieldStr);
        }




        public double POPRate(int State)
        {
            double result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string statecode = DR[FScodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(statecode, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == State)
                    {
                        string valstr = DR[FPOPRateFieldStr].ToString();
                        double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = tempDbl;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Get the Rate of Growth for Population for all units. </summary>
        ///
        /// <remarks> Quay, 3/3/2018.</remarks>
        ///
        /// <returns> A double[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public double[] POPRate()
        {
            return GetDataArray(FPOPRateFieldStr);
        }

        public double INDRate(int State)
        {
            double result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string statecode = DR[FScodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(statecode, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == State)
                    {
                        string valstr = DR[FINDRateFieldStr].ToString();
                        double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = tempDbl;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Get the Industry Growth rate Value for all units..</summary>
        ///
        /// <remarks> Quay, 3/3/2018.</remarks>
        ///
        /// <returns> A double[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public double[] INDRate()
        {
            return GetDataArray(FINDRateFieldStr);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Fast ag net. </summary>
        ///
        /// <param name="Code"> The code. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double FastAgNet(int Code)
        {
            double temp = InvalidRate;
            RateData TheData = FRateDataList.Find(delegate (RateData RD) { return RD.Code == Code; });
            if (TheData.Code == Code)
            {
                temp = TheData.AgNet;
            }
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the Agriculture Net Value for all units.</summary>
        ///
        /// <remarks> Quay, 3/3/2018.</remarks>
        ///
        /// <returns> A double[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public double[] AGNet()
        {
            return GetDataArray(FAgNetFieldStr);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Fast ag net. </summary>
        ///
        /// <param name="UnitName"> Name of the unit. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double FastAgNet(string UnitName)
        {
            double temp = InvalidRate;
            RateData TheData = FRateDataList.Find(delegate (RateData RD) { return RD.UnitName == UnitName; });
            if (TheData.UnitName == UnitName)
            {
                temp = TheData.AgNet;
            }
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Fast ag rate. </summary>
        ///
        /// <param name="Code"> The code. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double FastAgRate(int Code)
        {
            double temp = InvalidRate;
            RateData TheData = FRateDataList.Find(delegate (RateData RD) { return RD.Code == Code; });
            if (TheData.Code == Code)
            {
                temp = TheData.AgRate;
            }
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Fast ag rate. </summary>
        ///
        /// <param name="UnitName"> Name of the unit. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double FastAgRate(string UnitName)
        {
            double temp = InvalidRate;
            RateData TheData = FRateDataList.Find(delegate (RateData RD)
            {
                return RD.UnitName == UnitName;
            });
            if (TheData.UnitName == UnitName)
            {
                temp = TheData.AgRate;
            }
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Fast ind rate. </summary>
        ///
        /// <param name="Code"> The code. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double FastIndRate(int Code)
        {
            double temp = InvalidRate;
            RateData TheData = FRateDataList.Find(delegate (RateData RD) { return RD.Code == Code; });
            if (TheData.Code == Code)
            {
                temp = TheData.IndRate;
            }
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Fast ind rate. </summary>
        ///
        /// <param name="UnitName"> Name of the unit. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double FastIndRate(string UnitName)
        {
            double temp = InvalidRate;
            RateData TheData = FRateDataList.Find(delegate (RateData RD) { return RD.UnitName == UnitName; });
            if (TheData.UnitName == UnitName)
            {
                temp = TheData.IndRate;
            }
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Fast pop rate. </summary>
        ///
        /// <param name="Code"> The code. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double FastPopRate(int Code)
        {
            double temp = InvalidRate;
            RateData TheData = FRateDataList.Find(delegate (RateData RD) { return RD.Code == Code; });
            if (TheData.Code == Code)
            {
                temp = TheData.PopRate;
            }
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Fast pop rate. </summary>
        ///
        /// <param name="UnitName"> Name of the unit. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double FastPopRate(string UnitName)
        {
            double temp = InvalidRate;
            RateData TheData = FRateDataList.Find(delegate (RateData RD) { return RD.UnitName == UnitName; });
            if (TheData.UnitName == UnitName)
            {
                temp = TheData.PopRate;
            }
            return temp;
        }
        // ====================================================================================================
        // 06.29.18
        // -----------------------------------
        public double FastAgRateLCLU(int Code)
        {
            double temp = InvalidRate;
            RateData TheData = FRateDataList.Find(delegate (RateData RD) { return RD.Code == Code; });
            if (TheData.Code == Code)
            {
                temp = TheData.AgRateLCLU;
            }
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Fast ag rate. </summary>
        ///
        /// <param name="UnitName"> Name of the unit. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double FastAgRateLCLU(string UnitName)
        {
            double temp = InvalidRate;
            RateData TheData = FRateDataList.Find(delegate (RateData RD)
            {
                return RD.UnitName == UnitName;
            });
            if (TheData.UnitName == UnitName)
            {
                temp = TheData.AgRateLCLU;
            }
            return temp;
        }
        //
        // -----------------------------------
        public double FastUrbanRateLCLU(int Code)
        {
            double temp = InvalidRate;
            RateData TheData = FRateDataList.Find(delegate (RateData RD) { return RD.Code == Code; });
            if (TheData.Code == Code)
            {
                temp = TheData.UrbanRateLCLU;
            }
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Fast ag rate. </summary>
        ///
        /// <param name="UnitName"> Name of the unit. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double FastUrbanRateLCLU(string UnitName)
        {
            double temp = InvalidRate;
            RateData TheData = FRateDataList.Find(delegate (RateData RD)
            {
                return RD.UnitName == UnitName;
            });
            if (TheData.UnitName == UnitName)
            {
                temp = TheData.UrbanRateLCLU;
            }
            return temp;
        }

        // -----------------------------------
        public double FastIndRateLCLU(int Code)
        {
            double temp = InvalidRate;
            RateData TheData = FRateDataList.Find(delegate (RateData RD) { return RD.Code == Code; });
            if (TheData.Code == Code)
            {
                temp = TheData.IndRateLCLU;
            }
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Fast ag rate. </summary>
        ///
        /// <param name="UnitName"> Name of the unit. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double FastIndRateLCLU(string UnitName)
        {
            double temp = InvalidRate;
            RateData TheData = FRateDataList.Find(delegate (RateData RD)
            {
                return RD.UnitName == UnitName;
            });
            if (TheData.UnitName == UnitName)
            {
                temp = TheData.IndRateLCLU;
            }
            return temp;
        }
        // ==================================================================================================
        // edits 09.21.21 das
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnitName"></param>
        /// <returns></returns>
        public double FastUrbanHighRate(string UnitName)
        {
            double temp = InvalidRate;
            RateData TheData = FRateDataList.Find(delegate (RateData RD)
            {
                return RD.UnitName == UnitName;
            });
            if (TheData.UnitName == UnitName)
            {
                temp = TheData.UrbanRateHigh;
            }
            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnitName"></param>
        /// <returns></returns>
        public double FastUrbanLowRate(string UnitName)
        {
            double temp = InvalidRate;
            RateData TheData = FRateDataList.Find(delegate (RateData RD)
            {
                return RD.UnitName == UnitName;
            });
            if (TheData.UnitName == UnitName)
            {
                temp = TheData.UrbanRateLow;
            }
            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnitName"></param>
        /// <returns></returns>
        public double FastSuburbanRate(string UnitName)
        {
            double temp = InvalidRate;
            RateData TheData = FRateDataList.Find(delegate (RateData RD)
            {
                return RD.UnitName == UnitName;
            });
            if (TheData.UnitName == UnitName)
            {
                temp = TheData.SuburbanRate;
            }
            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnitName"></param>
        /// <returns></returns>
        public double FastExurbanHighRate(string UnitName)
        {
            double temp = InvalidRate;
            RateData TheData = FRateDataList.Find(delegate (RateData RD)
            {
                return RD.UnitName == UnitName;
            });
            if (TheData.UnitName == UnitName)
            {
                temp = TheData.ExurbanRateHigh;
            }
            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnitName"></param>
        /// <returns></returns>
        public double FastExurbanLowRate(string UnitName)
        {
            double temp = InvalidRate;
            RateData TheData = FRateDataList.Find(delegate (RateData RD)
            {
                return RD.UnitName == UnitName;
            });
            if (TheData.UnitName == UnitName)
            {
                temp = TheData.ExurbanRateLow;
            }
            return temp;
        }


    }
    // =========================================================================================================
    // edits 01.19.22 das
    // Colorado - desalination exchange data from file
    /// <summary>
    /// 
    /// </summary>
    public struct ColoradoDesalExchangeStruct
    {

        string Source;
        string Partner;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="partner"></param>
        public ColoradoDesalExchangeStruct(string source, string partner)
        {
            Source = source;
            Partner = partner;
        }
        public string FindSource
        {
            get { return Source; }
        }
        public string FindPartner
        {
            get { return Partner; }
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class ColoradoDesalExchangeClass
    {
        // DataTable Parameters
        DataTable TheData = null;
        string FDataDirectory = "";
        string FFilename = "";
        // EDIT end 2 13 18

        const string FPartnerFieldStr = "Partner";
        const string FSourceFieldStr = "Source";
        // Dictionary<string, int> StateCodes = new Dictionary<string, int>();
        const double InvalidRate = -1;//double.NaN;
        const int InvalidTarget = -1;//double.NaN;

        List<ColoradoDesalExchangeStruct> FExchangeDataList = new List<ColoradoDesalExchangeStruct>();
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectory"></param>
        /// <param name="Filename"></param>
        public ColoradoDesalExchangeClass(string DataDirectory, string Filename)
        {
            string errMessage = "";
            bool isErr = false;
            FDataDirectory = DataDirectory;
            FFilename = Filename;
            UniDbConnection DbCon = new UniDbConnection(SQLServer.stText, "", FDataDirectory, "", "", "");
            DbCon.UseFieldHeaders = true;
            DbCon.Open();
            TheData = Tools.LoadTable(DbCon, FFilename, ref isErr, ref errMessage);
            if (isErr)
            {
                throw new Exception("Error loading Water Exchange File and Data. " + errMessage);
            }
            // build data arrays
            int arraysize = TheData.Rows.Count;
            foreach (DataRow DR in TheData.Rows)
            {
                if (!isErr)
                {
                    string part = DR[FPartnerFieldStr].ToString();
                    string source = DR[FSourceFieldStr].ToString();
                    //
                    ColoradoDesalExchangeStruct RD = new ColoradoDesalExchangeStruct(source, part);
                    FExchangeDataList.Add(RD);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public string FastSource(string Source)
        {
            string temp = "";
            ColoradoDesalExchangeStruct TheData = FExchangeDataList.Find(delegate (ColoradoDesalExchangeStruct RD) { return RD.FindSource == Source; });
            if (TheData.FindSource == Source)
            {
                temp = TheData.FindSource;
            }
            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string FastTarget(string source)
        {
            string temp = "";
            ColoradoDesalExchangeStruct TheData = FExchangeDataList.Find(delegate (ColoradoDesalExchangeStruct RD)
            { return RD.FindPartner == source; });
            if (TheData.FindPartner == source)
            {
                temp = TheData.FindPartner;
            }
            return temp;
        }

    }
    // =============================================================================================================
    // edits 02.08.22 das
    // ===================================
    /// <summary>
    /// 
    /// </summary>
    public struct UrbanDensityClassStruct
    {
        string FUnitName;
        string FUnitCodeStr;
        int FUnitCode;
        //
        double _SMF;
        double _WMF;
        double _MMF;
        double _HMF;
        double _LSF;
        double _TSF;
        double _SSF;
        //
        double _SMFDUA;
        double _WMFDUA;
        double _MMFDUA;
        double _HMFDUA;
        double _LSFDUA;
        double _TSFDUA;
        double _SSFDUA;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aUnitName"></param>
        /// <param name="aUnitCode"></param>
        /// <param name="SMF"></param>
        /// <param name="WMF"></param>
        /// <param name="MMF"></param>
        /// <param name="HMF"></param>
        /// <param name="LSF"></param>
        /// <param name="TSF"></param>
        /// <param name="SSF"></param>
        /// <param name="SMFDUA"></param>
        /// <param name="WMFDUA"></param>
        /// <param name="MMFDUA"></param>
        /// <param name="HMFDUA"></param>
        /// <param name="LSFDUA"></param>
        /// <param name="TSFDUA"></param>
        /// <param name="SSFDUA"></param>
        public UrbanDensityClassStruct(string aUnitName, string aUnitCode, double SMF, double WMF, double MMF,
            double HMF, double LSF, double TSF, double SSF,
            double SMFDUA, double WMFDUA, double MMFDUA,
            double HMFDUA, double LSFDUA, double TSFDUA, double SSFDUA
            )
        {
            bool isErr = false;
            string errMsg = "";

            FUnitName = aUnitName;
            FUnitName = aUnitName;
            FUnitCodeStr = aUnitCode;

            int temp = Tools.ConvertToInt32(FUnitCodeStr, ref isErr, ref errMsg);
            if (!isErr)
            {
                FUnitCode = temp;
            }
            else
            {
                FUnitCode = UDI.BadIntValue;
            }
            //
            _SMF = SMF;
            _WMF = WMF;
            _MMF = MMF;
            _HMF = HMF;
            _LSF = LSF;
            _TSF = TSF;
            _SSF = SSF;
            //
            _SMFDUA = SMFDUA;
            _WMFDUA = WMFDUA;
            _MMFDUA = MMFDUA;
            _HMFDUA = HMFDUA;
            _LSFDUA = LSFDUA;
            _TSFDUA = TSFDUA;
            _SSFDUA = SSFDUA;
            //
        }
        /// <summary>
        /// Region name
        /// </summary>
        public string UnitName
        {
            get { return FUnitName; }
        }
        /// <summary>
        ///  Region code
        /// </summary>
        public int UnitCode
        {
            get { return FUnitCode; }
        }
        //
        //UH_SMF = gross* 0.35;
        //UH_WMF = gross* 0.453;
        //UH_MMF = gross* 0.124;
        //UH_HMF = gross* 0.073;
        //UL_LSF = gross * 0.207;
        //UL_TSF = gross * 0.708;
        //UL_SSF = gross * 0.085;
        public double UH_SMF
        { get { return _SMF; } set { _SMF = value; } }
        public double UH_WMF
        { get { return _WMF; } set { _WMF = value; } }
        public double UH_MMF
        { get { return _MMF; } set { _MMF = value; } }
        public double UH_HMF
        { get { return _HMF; } set { _HMF = value; } }
        public double UL_LSF
        { get { return _LSF; } set { _LSF = value; } }
        public double UL_TSF
        { get { return _TSF; } set { _TSF = value; } }
        public double UL_SSF
        { get { return _SSF; } set { _SSF = value; } }
        //
        public double UH_SMFDUA
        { get { return _SMFDUA; } set { _SMFDUA = value; } }
        public double UH_WMFDUA
        { get { return _WMFDUA; } set { _WMFDUA = value; } }
        public double UH_MMFDUA
        { get { return _MMFDUA; } set { _MMFDUA = value; } }
        public double UH_HMFDUA
        { get { return _HMFDUA; } set { _HMFDUA = value; } }
        public double UL_LSFDUA
        { get { return _LSFDUA; } set { _LSFDUA = value; } }
        public double UL_TSFDUA
        { get { return _TSFDUA; } set { _TSFDUA = value; } }
        public double UL_SSFDUA
        { get { return _SSFDUA; } set { _SSFDUA = value; } }
    }
    /// <summary>
    /// Class to read in the density proportions by region to use
    /// for the urban land cover land use data for housing density
    /// 02.08.22 das
    /// </summary>
    public class UrbanDensityDataClass
    {
        // DataTable Parameters
        DataTable TheData = null;
        string FDataDirectory = "";
        string FFilename = "";
        // EDIT end 2 13 18

        const string FScodeFieldStr = "SC";
        const string FSnameFieldStr = "SN";
        const string FRnameFieldStr = "RN";
        const string FRcodeFieldStr = "RC";

        string FNameFieldStr = FRnameFieldStr;
        string FCodeFieldStr = FRcodeFieldStr;

        string FSMFFieldStr = "SMF";
        string FWMFFieldStr = "WMF";
        string FMMFFieldStr = "MMF";
        string FHMFFieldStr = "HMF";
        //
        string FLSFFieldStr = "LSF";
        string FTSFFieldStr = "TSF";
        string FSSFFieldStr = "SSF";
        //
        string FSMFduaFieldStr = "SMFDUA";
        string FWMFduaFieldStr = "WMFDUA";
        string FMMFduaFieldStr = "MMFDUA";
        string FHMFduaFieldStr = "HMFDUA";
        //
        string FLSFduaFieldStr = "LSFDUA";
        string FTSFduaFieldStr = "TSFDUA";
        string FSSFduaFieldStr = "SSFDUA";

        //string FSUBFieldStr = "SUB";
        //string FExUHFieldStr = "ExUH";
        //string FExULFeldStr = "ExUL";
        //
        // ========================================

        // Data Array Parameters

        Dictionary<string, int> StateCodes = new Dictionary<string, int>();
        const double InvalidRate = -1;//double.NaN;

        double[] FSMFArray = null;
        double[] FWMFArray = null;
        double[] FMMFArray = null;
        double[] FHMFArray = null;
        // =================
        // 06.29.18 LCLU arrays
        double[] FLSFArray = null;
        double[] FTSFArray = null;
        double[] FSSFArray = null;
        // ===========================
        double[] FSMFduaArray = null;
        double[] FWMFduaArray = null;
        double[] FMMFduaArray = null;
        double[] FHMFduaArray = null;
        // ===================
        // 06.29.18 LCLU arrays
        double[] FLSFduaArray = null;
        double[] FTSFduaArray = null;
        double[] FSSFduaArray = null;


        // ====================================
        //double[] FSubArray = null;
        //double[] FExUHighArray = null;
        //double[] FExULowArray = null;
        // ====================================

        List<UrbanDensityClassStruct> FDensityDataList = new List<UrbanDensityClassStruct>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectory"></param>
        /// <param name="Filename"></param>
        public UrbanDensityDataClass(string DataDirectory, string Filename)
        {
            string errMessage = "";
            bool isErr = false;
            FDataDirectory = DataDirectory;
            FFilename = Filename;
            UniDbConnection DbCon = new UniDbConnection(SQLServer.stText, "", FDataDirectory, "", "", "");
            DbCon.UseFieldHeaders = true;
            DbCon.Open();
            TheData = Tools.LoadTable(DbCon, FFilename, ref isErr, ref errMessage);
            if (isErr)
            {
                throw new Exception("Error loading Urban Density and Dwelling Units per acre Data. " + errMessage);
            }
            // build data arrays
            // ============================================
            int arraysize = TheData.Rows.Count;
            FSMFArray = new double[arraysize];
            FWMFArray = new double[arraysize];
            FMMFArray = new double[arraysize];
            FHMFArray = new double[arraysize];
            // ====================
            // 06.29.18
            FLSFArray = new double[arraysize];
            FTSFArray = new double[arraysize];
            FSSFArray = new double[arraysize];
            // ============================================
            FSMFduaArray = new double[arraysize];
            FWMFduaArray = new double[arraysize];
            FMMFduaArray = new double[arraysize];
            FHMFduaArray = new double[arraysize];
            // ====================
            // 06.29.18
            FLSFduaArray = new double[arraysize];
            FTSFduaArray = new double[arraysize];
            FSSFduaArray = new double[arraysize];

            // ===========================================
            //FSubArray = new double[arraysize];
            //FExUHighArray = new double[arraysize];
            //FExULowArray = new double[arraysize];
            // ===========================================
            //int CodeI = 0;
            foreach (DataRow DR in TheData.Rows)
            {
                // Get name and code
                // EDIT QUAY 2/19/18
                // Setup to use region or state codes
                //string namestr = DR[FSnameFieldStr].ToString();
                //string codestr = DR[FScodeFieldStr].ToString();
                string namestr = DR[FNameFieldStr].ToString();
                string codestr = DR[FCodeFieldStr].ToString();
                // END EDIT

                // Decided not to use code in DataTable
                // int codeint = Tools.ConvertToInt32(codestr,ref isErr,ref errMessage);



                if (!isErr)
                {

                    string smfstr = DR[FSMFFieldStr].ToString();
                    string wmfstr = DR[FWMFFieldStr].ToString();
                    string mmfstr = DR[FMMFFieldStr].ToString();
                    string hmfstr = DR[FHMFFieldStr].ToString();
                    //
                    // =============================
                    // 02.02.22
                    string lsfstr = DR[FLSFFieldStr].ToString();
                    string tsfstr = DR[FTSFFieldStr].ToString();
                    string ssfstr = DR[FSSFFieldStr].ToString();
                    // =========================================
                    string smfDUAstr = DR[FSMFduaFieldStr].ToString();
                    string wmfDUAstr = DR[FWMFduaFieldStr].ToString();
                    string mmfDUAstr = DR[FMMFduaFieldStr].ToString();
                    string hmfDUAstr = DR[FHMFduaFieldStr].ToString();
                    //
                    // =============================
                    // 02.02.22
                    string lsfDUAstr = DR[FLSFduaFieldStr].ToString();
                    string tsfDUAstr = DR[FTSFduaFieldStr].ToString();
                    string ssfDUAstr = DR[FSSFduaFieldStr].ToString();
                    // ===============================================

                    // =========================================
                    //string substr = DR[FSUBFieldStr].ToString();
                    //string exhstr = DR[FExUHFieldStr].ToString();
                    //string exlstr = DR[FExULFeldStr].ToString();
                    //
                    double Tempsmf = Tools.ConvertToDouble(smfstr, ref isErr, ref errMessage);
                    if (!isErr)
                    {
                        double Tempwmf = Tools.ConvertToDouble(wmfstr, ref isErr, ref errMessage);
                        if (!isErr)
                        {
                            double Tempmmf = Tools.ConvertToDouble(mmfstr, ref isErr, ref errMessage);
                            if (!isErr)
                            {
                                double Temphmf = Tools.ConvertToDouble(hmfstr, ref isErr, ref errMessage);
                                if (!isErr)
                                {
                                    double Templsf = Tools.ConvertToDouble(lsfstr, ref isErr, ref errMessage);
                                    if (!isErr)
                                    {
                                        double Temptsf = Tools.ConvertToDouble(tsfstr, ref isErr, ref errMessage);
                                        if (!isErr)
                                        {
                                            double Tempssf = Tools.ConvertToDouble(ssfstr, ref isErr, ref errMessage);
                                            if (!isErr)
                                            {
                                                double TempDUAsmf = Tools.ConvertToDouble(smfDUAstr, ref isErr, ref errMessage);
                                                if (!isErr)
                                                {
                                                    double TempDUAwmf = Tools.ConvertToDouble(wmfDUAstr, ref isErr, ref errMessage);
                                                    if (!isErr)
                                                    {
                                                        double TempDUAmmf = Tools.ConvertToDouble(mmfDUAstr, ref isErr, ref errMessage);
                                                        if (!isErr)
                                                        {
                                                            double TempDUAhmf = Tools.ConvertToDouble(hmfDUAstr, ref isErr, ref errMessage);
                                                            if (!isErr)
                                                            {
                                                                double TempDUAlsf = Tools.ConvertToDouble(lsfDUAstr, ref isErr, ref errMessage);
                                                                if (!isErr)
                                                                {
                                                                    double TempDUAtsf = Tools.ConvertToDouble(tsfDUAstr, ref isErr, ref errMessage);
                                                                    if (!isErr)
                                                                    {
                                                                        double TempDUAssf = Tools.ConvertToDouble(ssfDUAstr, ref isErr, ref errMessage);
                                                                        if (!isErr)
                                                                        {
                                                                            UrbanDensityClassStruct DCS = new UrbanDensityClassStruct(namestr, codestr, Tempsmf, Tempwmf,
                                                                               Tempmmf, Temphmf, Templsf, Temptsf, Tempssf, TempDUAsmf, TempDUAwmf, TempDUAmmf, TempDUAhmf,
                                                                               TempDUAlsf, TempDUAtsf, TempDUAssf);
                                                                            FDensityDataList.Add(DCS);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public double FastSMFproportion(string region)
        {
            double temp = InvalidRate;
            UrbanDensityClassStruct TheData = FDensityDataList.Find(delegate (UrbanDensityClassStruct DD) { return DD.UnitName == region; });
            if (TheData.UnitName == region)
            {
                temp = TheData.UH_SMF;
            }
            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public double FastWMFproportion(string region)
        {
            double temp = InvalidRate;
            UrbanDensityClassStruct TheData = FDensityDataList.Find(delegate (UrbanDensityClassStruct DD) { return DD.UnitName == region; });
            if (TheData.UnitName == region)
            {
                temp = TheData.UH_WMF;
            }
            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public double FastMMFproportion(string region)
        {
            double temp = InvalidRate;
            UrbanDensityClassStruct TheData = FDensityDataList.Find(delegate (UrbanDensityClassStruct DD) { return DD.UnitName == region; });
            if (TheData.UnitName == region)
            {
                temp = TheData.UH_MMF;
            }
            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public double FastHMFproportion(string region)
        {
            double temp = InvalidRate;
            UrbanDensityClassStruct TheData = FDensityDataList.Find(delegate (UrbanDensityClassStruct DD) { return DD.UnitName == region; });
            if (TheData.UnitName == region)
            {
                temp = TheData.UH_HMF;
            }
            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public double FastLSFproportion(string region)
        {
            double temp = InvalidRate;
            UrbanDensityClassStruct TheData = FDensityDataList.Find(delegate (UrbanDensityClassStruct DD) { return DD.UnitName == region; });
            if (TheData.UnitName == region)
            {
                temp = TheData.UL_LSF;
            }
            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public double FastTSFproportion(string region)
        {
            double temp = InvalidRate;
            UrbanDensityClassStruct TheData = FDensityDataList.Find(delegate (UrbanDensityClassStruct DD) { return DD.UnitName == region; });
            if (TheData.UnitName == region)
            {
                temp = TheData.UL_TSF;
            }
            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public double FastSSFproportion(string region)
        {
            double temp = InvalidRate;
            UrbanDensityClassStruct TheData = FDensityDataList.Find(delegate (UrbanDensityClassStruct DD) { return DD.UnitName == region; });
            if (TheData.UnitName == region)
            {
                temp = TheData.UL_SSF;
            }
            return temp;
        }
        // ===========================================================
        // Dwelling Units per acre
        // ====================================
        public double FastSMFDUA(string region)
        {
            double temp = InvalidRate;
            UrbanDensityClassStruct TheData = FDensityDataList.Find(delegate (UrbanDensityClassStruct DD) { return DD.UnitName == region; });
            if (TheData.UnitName == region)
            {
                temp = TheData.UL_SSFDUA;
            }
            return temp;
        }


        public double FastWMFDUA(string region)
        {
            double temp = InvalidRate;
            UrbanDensityClassStruct TheData = FDensityDataList.Find(delegate (UrbanDensityClassStruct DD) { return DD.UnitName == region; });
            if (TheData.UnitName == region)
            {
                temp = TheData.UL_SSFDUA;
            }
            return temp;
        }


        public double FastMMFDUA(string region)
        {
            double temp = InvalidRate;
            UrbanDensityClassStruct TheData = FDensityDataList.Find(delegate (UrbanDensityClassStruct DD) { return DD.UnitName == region; });
            if (TheData.UnitName == region)
            {
                temp = TheData.UL_SSFDUA;
            }
            return temp;
        }

        public double FastHMFDUA(string region)
        {
            double temp = InvalidRate;
            UrbanDensityClassStruct TheData = FDensityDataList.Find(delegate (UrbanDensityClassStruct DD) { return DD.UnitName == region; });
            if (TheData.UnitName == region)
            {
                temp = TheData.UL_SSFDUA;
            }
            return temp;
        }

        // ------------------------------------
        public double FastLSFDUA(string region)
        {
            double temp = InvalidRate;
            UrbanDensityClassStruct TheData = FDensityDataList.Find(delegate (UrbanDensityClassStruct DD) { return DD.UnitName == region; });
            if (TheData.UnitName == region)
            {
                temp = TheData.UL_SSFDUA;
            }
            return temp;
        }

        public double FastTSFDUA(string region)
        {
            double temp = InvalidRate;
            UrbanDensityClassStruct TheData = FDensityDataList.Find(delegate (UrbanDensityClassStruct DD) { return DD.UnitName == region; });
            if (TheData.UnitName == region)
            {
                temp = TheData.UL_SSFDUA;
            }
            return temp;
        }

        public double FastSSFDUA(string region)
        {
            double temp = InvalidRate;
            UrbanDensityClassStruct TheData = FDensityDataList.Find(delegate (UrbanDensityClassStruct DD) { return DD.UnitName == region; });
            if (TheData.UnitName == region)
            {
                temp = TheData.UL_SSFDUA;
            }
            return temp;
        }

        // ===========================================================

        //public double FastSUBproportion(int Code)
        //{
        //    double temp = InvalidRate;
        //    UrbanDensityClassStruct TheData = FDensityDataList.Find(delegate (UrbanDensityClassStruct DD) { return DD.UnitCode == Code; });
        //    if (TheData.UnitCode == Code)
        //    {
        //        temp = TheData.SUB;
        //    }
        //    return temp;
        //}

        //public double FastExHproportion(int Code)
        //{
        //    double temp = InvalidRate;
        //    UrbanDensityClassStruct TheData = FDensityDataList.Find(delegate (UrbanDensityClassStruct DD) { return DD.UnitCode == Code; });
        //    if (TheData.UnitCode == Code)
        //    {
        //        temp = TheData.ExH;
        //    }
        //    return temp;
        //}

        //public double FastExLproportion(int Code)
        //{
        //    double temp = InvalidRate;
        //    UrbanDensityClassStruct TheData = FDensityDataList.Find(delegate (UrbanDensityClassStruct DD) { return DD.UnitCode == Code; });
        //    if (TheData.UnitCode == Code)
        //    {
        //        temp = TheData.ExL;
        //    }
        //    return temp;
        //}



    }
}






/*
 * 
    temp = new CombItem("USUR" , "Urban (Public Supply) Surface", new string[] {"PSSF"});  CombList.Add(temp); 
    temp = new CombItem("USAL" , "Urban Surface Saline", new string[] {"PSSS"});  CombList.Add(temp); 
    temp = new CombItem("UGW" , "Urban Groundwater", new string[] {"PSGW"});  CombList.Add(temp); 
    temp = new CombItem("UREC" , "Urban Reclaimed", new string[] {"PSRW"});  CombList.Add(temp); 
    temp = new CombItem("URES" , "Urban Residential", new string[] {"PSDD"});  CombList.Add(temp); 
    temp = new CombItem("UIND" , "Urban Industrial", new string[] {"PSDI"});  CombList.Add(temp); 
    temp = new CombItem("UTOT" , "Urban Total", new string[] {"PSD"});  CombList.Add(temp); 
    temp = new CombItem("UPOP" , "Urban Population Served", new string[] {"PSPOP"});  CombList.Add(temp); 
    temp = new CombItem("RSUR" , "Rural (Self Supplied Residential ) Surface", new string[] {"DMSF"});  CombList.Add(temp); 
    temp = new CombItem("RSAL" , "Rural (Self Supplied Residential ) Surface Saline", new string[] {"DMSS"});  CombList.Add(temp); 
    temp = new CombItem("RGW" , "Rural (Self Supplied Residential ) Groundwater", new string[] {"DMGW"});  CombList.Add(temp); 
    temp = new CombItem("RTOT" , "Rural (Self Supplied Residential ) Total", new string[] {"DMD"});  CombList.Add(temp); 
    temp = new CombItem("ASUR" , "Agriculture Surface", new string[] {"LSSF","LVGS","LASF","AQSF","IRSF"});  CombList.Add(temp); 
    temp = new CombItem("ASAL" , "Agriculture Surface Saline", new string[] {"LSSS","LASS","AQSS"});  CombList.Add(temp); 
    temp = new CombItem("AGW" , "Agriculture Groundwater", new string[] {"LSGW","LVGF","LAGW","AQGW","IRGF"});  CombList.Add(temp); 
    temp = new CombItem("AREC" , "Agriculture Reclaimed", new string[] {"IRRW"});  CombList.Add(temp); 
    temp = new CombItem("ATOT" , "Agriculture Total", new string[] {"LSD","LVD","LAD","AQD","IRD"});  CombList.Add(temp); 
    temp = new CombItem("ISUR" , "Industry / Mining (non urban - self supplied) Surface Water", new string[] {"IDSF","MNSF"});  CombList.Add(temp); 
    temp = new CombItem("ISAL" , "Industry / Mining (non urban - self supplied) Surface Saline", new string[] {"IDSS","MNSS"});  CombList.Add(temp); 
    temp = new CombItem("IGW" , "Industry / Mining  (non urban - self supplied) Groundwater", new string[] {"IDGW","MNGW"});  CombList.Add(temp); 
    temp = new CombItem("IREC" , "Industry / Mining  (non urban - self supplied) Reclaimed", new string[] {"IDRW","MNRW"});  CombList.Add(temp); 
    temp = new CombItem("ITOT" , "Industry / Mining  (non urban - self supplied) Total", new string[] {"IDD","MND"});  CombList.Add(temp); 
    temp = new CombItem("PSUR" , "Power Surface Water", new string[] {"TPSF","HPSF"});  CombList.Add(temp); 
    temp = new CombItem("PSAL" , "Power Surface Saline", new string[] {"TPSS","HPSS"});  CombList.Add(temp); 
    temp = new CombItem("PGW" , "Power Groundwater", new string[] {"TPGW"});  CombList.Add(temp); 
    temp = new CombItem("PREC" , "Power Reclaimed", new string[] {"TPRW"});  CombList.Add(temp); 
    temp = new CombItem("PTOT" , "Power Total", new string[] {"TPD","HPSW"});  CombList.Add(temp); 
            
 * 
 * temp = new CombItem("SUR" , "Surface Water Total", new string[] {"PSSF","DMSF","LSSF","LVGS","LASF","AQSF","IRSF","IDSF","MNSF","TPSF","HPSF"});  CombList.Add(temp); 
    temp = new CombItem("SAL" , "Surface Saline Total", new string[] {"PSSS","DMSS","LSSS","LASS","AQSS","IDSS","MNSS","TPSS","HPSS"});  CombList.Add(temp); 
    temp = new CombItem("GW" , "GroundWater Total", new string[] {"PSGW","DMGW","LSGW","LVGF","LAGW","AQGW","IRGF","IDGW","MNGW","TPGW"});  CombList.Add(temp); 
    temp = new CombItem("REC" , "Reclaimed  Total", new string[] {"PSRW","IRRW","IDRW","MNRW","TPRW"});  CombList.Add(temp);
    temp = new CombItem("ALL", "All Water Total", new string[] { "PSSF", "PSSS", "PSGW", "PSRW", "DMSF", "DMSS", "DMGW", "LSSF", "LVGS", "LASF", "AQSF", "IR
    
 */

