using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using WaterSimDCDC;
using System.Collections.Generic;
using WaterSimDCDC.Generic;
using DemandModel_Base;
using UniDB;
using System.Data;

namespace WaterSim_Base
{

   
    // ===================================================================================================================
    #region Rural Demand Class- LCLU Urban
    /// <summary>
    /// 
    /// </summary>
    public class RuralDemand_LCLU_urban : DemandModel
    {
        // objects
        WaterSimCRFModel CRF;
        //
        public RateDataClass FRDC;
        public DataClassLCLU FDClclu;
        //
        // Version 2 ICLUS
        public DataClassLcluArea FDLCLU;
        //
        double Fdemand;
        // constants
        // This is the coeffecient to convert USGS MGD Consumer/resource numbers to gallons
        const double convertDemand = 1000000;
        //
        int FUnitCode = 0;
        /// <summary>
        /// 
        /// </summary>
        public bool isInstantiated = false;
        //
        /// <summary>
        ///  The region names
        /// </summary>
        public string FUnitName = "";

        // edits 09.21.21 das
        // LCLU classes acreage density 
        // urban classes - density  thresholds
        // Urban high intensity             > 10 DUA (dwelling units per acre)
        // Urban Low              1.6  < UL < 10 DUA
        // Suburban               0.4  < S  < 1.6 DUA
        // Exurban high intensity 0.1  < EH < 0.4
        // Exurban low   " "      0.02 < EL < 0.1


        //
        #region Constructors
        // ==================================================================================

        double FBaseRate = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="crf"></param>
        /// <param name="TheRateData"></param>
        /// <param name="TheAcreData"></param>
        public RuralDemand_LCLU_urban(WaterSimCRFModel crf, RateDataClass TheRateData, DataClassLCLU TheAcreData)
        {
            CRF = crf;
            FRDC = TheRateData;
            FDClclu = TheAcreData;
            //
            SetBaseValues();
            isInstantiated = true;
            // assigns itself to the owner
            crf.URBAN = this;
            CRF.URBAN = this; // does this make a difference?
        }
        /// <summary>
        /// "FD - the version 2 of ICLUS lclu data - five urban classes"
        /// </summary>
        /// <param name="crf"></param>
        /// <param name="TheRateData"></param>
        /// <param name="TheAcreData - the original data we used: Ag, Industry, and urban classes"></param>
        /// <param name="FD" ></param>
        public RuralDemand_LCLU_urban(WaterSimCRFModel crf, RateDataClass TheRateData, DataClassLCLU TheAcreData, DataClassLcluArea FD)
        {
            CRF = crf;
            FRDC = TheRateData;
            FDClclu = TheAcreData;
            FDLCLU = FD;
            //
            SetBaseValues();
            isInstantiated = true;
            // assigns itself to the owner
            crf.URBAN = this;
            CRF.URBAN = this; // does this make a difference?
        }
        #endregion Constructors
        // ================================================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal void Demand(int currentYear)
        {
            double temp = 0;
            preProcessDemand(currentYear);
            double period = (currentYear - CRF.startYear) + 1;
            double outValue;
            double NewDemand = 0;
            string region = CRF.UnitName;
            //
            //double NewDemand = EstimateLCLUDemand(Lacres, LBaseRate, LurbanConservation, LurbanLCLUChangeCoef, LminUrban, period);
             if (FDLCLU != null)
            {
                NewDemand = demand;
            }
            else
            {
                NewDemand = EstimateConsumerDemands(Lacres, LBaseRate, LurbanConservation, LurbanLCLUChangeCoef, period, out outValue);
            }
            //
            temp = NewDemand;
            demandUrban = temp;
        }
        // ================================================================================================================================================

        // Properties
        #region properties
        // =================================================================================================

        double _acres = 0;
        double Lacres
        {
            get { return _acres; }
            set { _acres = value; }
        }
        double _rate = 0;
        double LBaseRate
        {
            get { return _rate; }
            set { _rate = value; }
        }
        // =========================================
        // acres of each urban class
        double UH
        {
            get; set;
        }
        double UL
        {
            get; set;
        }
        double Sub
        {
            get; set;
        }
        double ExUH
        {
            get; set;
        }
        double ExUL
        {
            get; set;
        }
        // ==============
        double rUH
        { get; set; }
        double rUL
        { get; set; }
        double rSUB
        { get; set; }
        double rExUH
        { get; set; }
        double rExUL
        { get; set; }
        double demand
        { get; set; }
        #endregion properties
        // =========================================
        // =========================================
        // Conservation
        double _urbanConservation = 0.99;
        double LurbanConservation
        {
            get { return _urbanConservation; }
            set { _urbanConservation = value; }
        }
       // ******************************************

       // ===========================================
       // Change Coefficient
        double _urbanLCLUChangeCoef = 1.0;
        double LurbanLCLUChangeCoef
        {
            get { return _urbanLCLUChangeCoef; }
            set { _urbanLCLUChangeCoef = value; }
        }
        // *******************************************
        // Minimum Values
                double _minUrban = 1.0;
        double LminUrban
        {
            get { return _minUrban; }
            set { _minUrban = value; }
        }


        // ===========================================
        // Demand
        double _demandUrban = 0;
        double demandUrban
        {
            get { return _demandUrban; }
            set { _demandUrban = value; }
        }
        // ******************************************



        // Functions & Methods
        // =================================================================================================
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentYear"></param>
        /// <returns></returns>
        public override double GetDemand(int currentYear)
        {
            double temp = 0;
            Demand(currentYear);
            temp = demandUrban;
            return temp;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize_Variables()
        {
            throw new NotImplementedException();
        }
        public override void SetDemandFactors()
        {
            throw new NotImplementedException();
        }
        public override void switchUrbanLCLU(int year)
        { 


        }
        /// <summary>
        /// 
        /// </summary>
        public override void SetBaseValues()
        {
            int year = 2010;
            LBaseRate = FRDC.FastUrbanRateLCLU(CRF.FUnitName);
            Lacres = FDClclu.FastUrbanAcres(CRF.FUnitName, year);
        }
        void SetInitialRates()
        {
           // LBaseRate = FRDC.FastUrbanRateLCLU(CRF.FUnitName);
        }
        // =============================================================================================================
        //
        // Process
        /// <summary>
        /// 
        /// </summary>
        public override void preProcessDemand()
        { }
        // ====================
        // UrbanDemand Pre Process
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentYear"></param>
        public override void preProcessDemand(int currentYear)
        {
            // Old data - Ag, Urban, Industry
            string region = CRF.UnitName;
            LurbanConservation = CRF.UrbanConservation;
            LurbanLCLUChangeCoef = CRF.PUrbanLCLUChangeCoef;
            //
            if (FDLCLU != null)
            {
                switchLCLU(region, currentYear);
            }
            else
            {
                Lacres = FDClclu.FastUrbanAcres(CRF.UnitName, currentYear);
                LBaseRate = FRDC.FastUrbanRateLCLU(CRF.FUnitName);
            }
         }
        /// <summary>
        /// ICLUS version 2 with five urban classes
        /// </summary>
        /// <param name="region"></param>
        /// <param name="currentYear"></param>
        public void switchLCLU( string region, int currentYear)
        {
            double period = (currentYear - CRF.startYear) + 1;
            if (currentYear == 2020)
            {
                assignLCLU(region, currentYear);
                relativeAcres(region, currentYear);
                modifyArea(region, currentYear);
                calculateDemand(region, currentYear, LurbanConservation, LurbanLCLUChangeCoef, period);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="currentYear"></param>
        public void assignLCLU(string region, int currentYear)
        {
            UH = FDLCLU.FastArea_UN(region, "EigthAcre", currentYear);
            UL = FDLCLU.FastArea_UN(region, "QuarterAcre", currentYear);
            Sub = FDLCLU.FastArea_UN(region, "ThirdAcre", currentYear);
            ExUH = FDLCLU.FastArea_UN(region, "HalfAcre", currentYear);
            ExUL = FDLCLU.FastArea_UN(region, "Acre", currentYear);
        }
        internal double totalAcres(string region, int year)
        {
            return FDLCLU.FastTotalArea_UN(region, year);
        }
        internal void relativeAcres(string region, int year)
        {
            rUH = UH / totalAcres(region, year);
            rUL = UL / totalAcres(region, year);
            rSUB = Sub / totalAcres(region, year);
            rExUH = ExUH / totalAcres(region, year);
            rExUL = ExUH / totalAcres(region, year);
        }
        internal void modifyArea(string region, int year)
        {
            bool IstrueUH = false;
            bool IstrueUL = false;
            double densityUH= Convert.ToDouble(CRF.geti_UrbanHighDensity()) / 100;
            double densityUL = Convert.ToDouble(CRF.geti_UrbanLowDensity()) / 100;
            //
            if (densityUH != 1)
            {
                IstrueUH = true;
                double newUH = UH * densityUH;
                UH = newUH;
                UL = (totalAcres(region, year) - newUH) * rUL;
                Sub = (totalAcres(region, year) - newUH) * rSUB;
                ExUH = (totalAcres(region, year) - newUH) * rExUH;
                ExUL = (totalAcres(region, year) - newUH) * rExUL;
            }
            if (densityUL != 1)
            {
                IstrueUL = true;
                double newUL = UL * densityUL;
                if (IstrueUH)
                {
                    UL = newUL;
                    Sub = (totalAcres(region, year) - UH - newUL) * rSUB;
                    ExUH = (totalAcres(region, year) - UH - newUL) * rExUH;
                    ExUL = (totalAcres(region, year) - UH - newUL) * rExUL;
                }
                else
                {
                    UH = (totalAcres(region, year) - newUL) * rUH;
                    Sub = (totalAcres(region, year) - newUL) * rSUB;
                    ExUH = (totalAcres(region, year) - newUL) * rExUH;
                    ExUL = (totalAcres(region, year) - newUL) * rExUL;
                }
            }
            if (densityUL != 1)
            {

            }

            Lacres = UH + UL + Sub + ExUH + ExUL;
        }
        internal void calculateDemand(string region, int year, double AdjustEfficiency, double AdjustDamper, double period)
        {
            double result = 0;
            double ModifyRate = 1;
            try
            {
                // Get the modify factor to apply for this period using the coefficient AdjustDamper
                //double ModifyRate = ChangeIncrement(1, period, AdjustDamper, MinValue); // NOT MinValue ... its the Limit in his code
                double startValue = 1;
                ModifyRate = utilities.AnnualExponentialChange(startValue, period, AdjustDamper, AdjustEfficiency);
                result = UH * FRDC.FastUrbanHighRate(region) * ModifyRate;
                // result += RD.U      
            }
            catch (Exception ex)
            {
                // Ouch Only thing going here is the Change Increment Function
            }
            demand = result; // set the property labeled "demand"
        }

    }
    #endregion Rural Demand Class- LCLU Urban
    // ==============================================================================================================================================

    // ==============================================================================================================================================
    #region Rural Demand Class- LCLU Ag
    /// <summary>
    /// 
    /// </summary>
    public class RuralDemand_LCLU_ag : DemandModel
    {
        // objects
        WaterSimCRFModel CRF;
        public RateDataClass FRDC;
        public DataClassLCLU FDClclu;

        //
        double Fdemand;
        // constants
        // This is the coeffecient to convert USGS MGD Consumer/resource numbers to gallons
        const double convertDemand = 1000000;
        //
        int FUnitCode = 0;
        /// <summary>
        /// 
        /// </summary>
        public bool isInstantiated = false;
        //

        #region Constructors
        // ========================================================================================
        double FBaseRate = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="crf"></param>
        /// <param name="TheRateData"></param>
        /// <param name="TheAcreData"></param>
        public RuralDemand_LCLU_ag(WaterSimCRFModel crf, RateDataClass TheRateData, DataClassLCLU TheAcreData)
        {
            CRF = crf;
            FRDC = TheRateData;
            FDClclu = TheAcreData;
            //
            SetBaseValues();
            isInstantiated = true;
            // assigns itself to the owner
            crf.AG = this;
        }
        #endregion Constructors
        // =================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal void Demand(int currentYear)
        {
            double temp = 0;
            preProcessDemand(currentYear);
            double period = (currentYear - CRF.startYear) + 1;
            //
            double NewDemand = EstimateLCLUDemand(Lacres, LBaseRate, LagConservation, LagLCLUChangeCoef, LminAg, period);
            temp = NewDemand;
            demandAg = temp ; 
        }
        // =================================================================================================================

        // Properties
        // ========================================================================================

        double _acres = 0;
        double Lacres
        {
            get { return _acres; }
            set { _acres = value; }
        }
        double _rate = 0;
        double LBaseRate
        {
            get { return _rate; }
            set { _rate = value; }
        }
        int _unit = 0;
        int LunitCode
        {
            get { return _unit; }
            set { _unit = value; }
        }
        // =========================================
        // Conservation
        double _agConservation = 0.99;
        double LagConservation
        {
            get { return _agConservation; }
            set { _agConservation = value; }
        }
        // ******************************************

        // ===========================================
        // Change Coefficient
        double _agLCLUChangeCoef = 1.0;
        double LagLCLUChangeCoef
        {
            get { return _agLCLUChangeCoef; }
            set { _agLCLUChangeCoef = value; }
        }
        // *******************************************
        // Minimum Values
        double _minAg = 1.0;
        double LminAg
        {
            get { return _minAg; }
            set { _minAg = value; }
        }


        // ===========================================
        // Demand
        double _demandAg = 0;
        double demandAg
        {
            get { return _demandAg; }
            set { _demandAg = value; }
        }
       // ******************************************



        // Functions & Methods
        // ========================================================================================
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentYear"></param>
        /// <returns></returns>
        public override double GetDemand(int currentYear)
        {
            double temp = 0;
            Demand(currentYear);

            temp = Math.Round(demandAg);
            return temp;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize_Variables()
        {
            throw new NotImplementedException();
        }
        public override void SetDemandFactors()
        {
            throw new NotImplementedException();
        }
        public override void SetBaseValues()
        {
            int year = 2010;
            LBaseRate = FRDC.FastAgRateLCLU(CRF.UnitName);
            Lacres = FDClclu.FastAgAcres(CRF.UnitName, year);
        }
        public override void switchUrbanLCLU(int year) { }
        // ========================================================================================
        //
        // Process
        public override void preProcessDemand(int currentYear) {

            Lacres = FDClclu.FastAgAcres(CRF.UnitName, currentYear);
            LagConservation = CRF.AgConservation;
            LagLCLUChangeCoef = CRF.PAgLCLUChangeCoef;
            LminAg = CRF.PminAgLCLU;
            LBaseRate = FRDC.FastAgRateLCLU(CRF.UnitName);
            LunitCode = CRF.unitCode;

        }
        // ====================
        // UrbanDemand Pre Process
        /// <summary>
        /// 
        /// </summary>
        /// <param name="yr"></param>
        public override void preProcessDemand()
        {
            throw new NotImplementedException();
        }
    }
    #endregion Rural Demand Class- LCLU Ag
    // ==============================================================================================================================================

    // ==============================================================================================================================================
    #region Rural Demand Class- LCLU Industry
    /// <summary>
    /// 
    /// </summary>
    public class RuralDemand_LCLU_industry : DemandModel
    {
        // objects
        WaterSimCRFModel CRF;
        public RateDataClass FRDC;
        public DataClassLCLU FDClclu;
        //
        double Fdemand;
        // constants
        // This is the coeffecient to convert USGS MGD Consumer/resource numbers to gallons
        const double convertDemand = 1000000;
        //
        int FUnitCode = 0;
        /// <summary>
        /// 
        /// </summary>
        public bool isInstantiated = false;
        //

        #region Constructors
        // ========================================================================================
  
        /// <summary>
        /// 
        /// </summary>
        /// <param name="crf"></param>
        /// <param name="TheRatedata"></param>
        /// <param name="TheAcreData"></param>
        public RuralDemand_LCLU_industry(WaterSimCRFModel crf, RateDataClass TheRatedata, DataClassLCLU TheAcreData)
        {
            CRF = crf;
            FRDC = TheRatedata;
            FDClclu = TheAcreData;
            SetBaseValues();
            isInstantiated = true;
            // assigns itself to the owner
            crf.INDUSTRY = this;
        }
        #endregion Constructors
        // =================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal void Demand(int currentYear)
        {
            double temp = 0;
            preProcessDemand(currentYear);
            double period = (currentYear - CRF.startYear) + 1;
            //
            double NewDemand = EstimateLCLUDemand(Lacres, LBaseRate, LindustrialConservation, LindustryLCLUChangeCoef, LminIndustry, period);
            temp = NewDemand;
            demandIndustry = temp;
        }
        // =================================================================================================================

        // Properties
        // ========================================================================================

        double _acres = 0;
        double Lacres
        {
            get { return _acres; }
            set { _acres = value; }
        }
        double _rate = 0;
        double LBaseRate
        {
            get { return _rate; }
            set { _rate = value; }
        }

        // =========================================
        // Conservation
        double _industrialConservation = 0.99;
        double LindustrialConservation
        {
            get { return _industrialConservation; }
            set { _industrialConservation = value; }
        }
        // ******************************************

        // ===========================================
        // Change Coefficient
        double _industryLCLUChangeCoef = 1.0;
        double LindustryLCLUChangeCoef
        {
            get { return _industryLCLUChangeCoef; }
            set { _industryLCLUChangeCoef = value; }
        }
        // *******************************************
        // Minimum Values
        double _minIndustry = 1.0;
        double LminIndustry
        {
            get { return _minIndustry; }
            set { _minIndustry = value; }
        }


        // ===========================================
        // Demand
        double _demandIndustry = 0;
        double demandIndustry
        {
            get { return _demandIndustry; }
            set { _demandIndustry = value; }
        }
        // ******************************************



        // Functions & Methods
        // ========================================================================================
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentYear"></param>
        /// <returns></returns>
        public override double GetDemand(int currentYear)
        {
            double temp = 0;
            Demand(currentYear);
            temp = demandIndustry;
            return temp;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize_Variables()
        {
            throw new NotImplementedException();
        }
        public override void SetDemandFactors()
        {
            throw new NotImplementedException();
        }
        public override void SetBaseValues()
        {
            int year = 2010;
            LBaseRate = FRDC.FastIndRateLCLU(CRF.UnitName);
            Lacres = FDClclu.FastIndAcres(CRF.UnitName, year);
        }
        public override void switchUrbanLCLU(int year) { }
        // ========================================================================================
        //
        // Process

        public override void preProcessDemand(int currentYear)
        {
            Lacres = FDClclu.FastIndAcres(CRF.UnitName, currentYear);
            LindustrialConservation = CRF.IndustryConservation;
            LindustryLCLUChangeCoef = CRF.PIndustryLCLUChangeCoef;
            LminIndustry = CRF.PminIndustryLCLU;
            LBaseRate = 0;
            LBaseRate = FRDC.FastIndRateLCLU(CRF.UnitName);
         }
        // ====================
        // UrbanDemand Pre Process
        /// <summary>
        /// 
        /// </summary>
        /// <param name="yr"></param>
        public override void preProcessDemand()
        {
            throw new NotImplementedException();
        }
    }
    #endregion Rural Demand Class- LCLU Industry
    // ==============================================================================================================================================






    // ==============================================================================================================================================
    #region Rate Data LCLU
    #region struct
    /// <summary>
    /// 
    /// </summary>
    public struct DataLCLU
    {
        string FUnitName;
        string FUnitCodeStr;
        int FUnitCode;

        int FYear;
        double FAcerageUrban;
        double FAcerageAg;
        double FAcerageInd;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="aUnitName"></param>
        /// <param name="aUnitCode"></param>
        /// <param name="anAcerageUrban"></param>
        /// <param name="anAcerageAg"></param>
        /// <param name="anAcerageInd"></param>
        /// <param name="aYear"></param>
        public DataLCLU(string aUnitName, string aUnitCode, double anAcerageAg, double anAcerageUrban,  double anAcerageInd, int aYear)
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
            FAcerageUrban = anAcerageUrban;
            FAcerageAg = anAcerageAg;
            FAcerageInd = anAcerageInd;
            FYear = aYear;
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
        public double AcerageUrban
        {
            get { return FAcerageUrban; }
        }
        public double AcerageAg
        {
            get { return FAcerageAg; }
        }
        public double AcerageInd
        {
            get { return FAcerageInd; }
        }
        public int TheYear
        {
            get { return FYear; }
        }



    }
    #endregion struct

    #region class
    /// <summary>
    /// 
    /// </summary>
    public class DataClassLCLU
    {
        // DataTable Parameters
        DataTable TheData = null;
        string FDataDirectory = "";
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

        string FAcerageUrbanFieldStr = "URBACRES";
        string FAcerageAgFieldStr    = "AGACRES";
        string FAcerageIndFieldStr   = "INDACRES";
        string FcurrentYearFieldStr = "YEAR";

        // Data Array Parameters

        Dictionary<string, int> StateCodes = new Dictionary<string, int>();
        const double InvalidRate = -1;//double.NaN;

        double[] FAcresUrbanArray = null;
        double[] FAcresAgArray = null;
        double[] FAcresIndArray = null;
        double[] FYearArray = null;

        List<DataLCLU> FRuralDataList = new List<DataLCLU>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectory"></param>
        /// <param name="Filename"></param>
        public DataClassLCLU(string DataDirectory, string Filename)
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
            FAcresUrbanArray = new double[arraysize];
            FAcresAgArray = new double[arraysize];
            FAcresIndArray = new double[arraysize];
            FYearArray = new double[arraysize];
            
            //int CodeI = 0;
            foreach (DataRow DR in TheData.Rows)
            {
                // Get name and code
                // EDIT QUAY 2/19/18
                // Setup to use region or state codes
                string namestr = DR[FNameFieldStr].ToString();
                string codestr = DR[FCodeFieldStr].ToString();
                // END EDIT

                // Decided not to use code in DataTable
                // int codeint = Tools.ConvertToInt32(codestr,ref isErr,ref errMessage);


                if (!isErr)
                {

                    string ragacresstr = DR[FAcerageAgFieldStr].ToString();
                    string rurbanacresstr = DR[FAcerageUrbanFieldStr].ToString();
                    string rindacresstr = DR[FAcerageIndFieldStr].ToString();
                    string ryearsstr = DR[FcurrentYearFieldStr].ToString();

                    double TempAg = Tools.ConvertToDouble(ragacresstr, ref isErr, ref errMessage);
                    if (!isErr)
                    {
                        double TempUrban = Tools.ConvertToDouble(rurbanacresstr, ref isErr, ref errMessage);
                        if (!isErr)
                        {
                            double TempInd = Tools.ConvertToDouble(rindacresstr, ref isErr, ref errMessage);
                            if (!isErr)
                            {
                                int TempYear = Tools.ConvertToInt32(ryearsstr, ref isErr, ref errMessage);
                                if (!isErr)
                                {
                                    // OK 
                                    //string aUnitName, string aUnitCode, double anAcerageUrban, double anAcerageAg, double anAcerageInd, int aYear
                                    DataLCLU AD = new DataLCLU(namestr, codestr, TempAg, TempUrban, TempInd, TempYear );
                                    FRuralDataList.Add(AD);
                                    //// add to dictionary 
                                   }
                            }

                        }
                    }
                }
            }

        }
        // ==============================================================
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

        // ==============================================================
        //   
        //public double[] AGRate()
        //{
        //    return GetDataArray(FAGRateFieldStr);
        //}


        public double FastUrbanAcres(string UnitName, int year)
        {
            double temp = InvalidRate;
            DataLCLU TheData = FRuralDataList.Find(delegate (DataLCLU AD) {
                return ((AD.TheYear == year) && (AD.UnitName == UnitName)); });
          
            if (TheData.UnitName == UnitName)
            {
                temp = TheData.AcerageUrban;
            }
            return temp;
        }
        public double FastAgAcres(string UnitName, int year)
        {
            double temp = InvalidRate;
            DataLCLU TheData = FRuralDataList.Find(delegate (DataLCLU AD) {
                return ((AD.TheYear == year) && (AD.UnitName == UnitName));
            });
            if (TheData.UnitName == UnitName)
            {
                temp = TheData.AcerageAg;
            }
            return temp;
        }
        public double FastIndAcres(string UnitName, int year)
        {
            double temp = InvalidRate;
            DataLCLU TheData = FRuralDataList.Find(delegate (DataLCLU AD)
            {
                return ((AD.TheYear == year) && (AD.UnitName == UnitName));
            });
            if (TheData.UnitName == UnitName)
            {
                temp = TheData.AcerageInd;
            }
            return temp;
        }
        #endregion class
        #endregion Data LCLU

        // =============================================================================================================
    }   // End of Class DataClassLCLU
}
