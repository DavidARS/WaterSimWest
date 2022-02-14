using System;
using System.IO;
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
    #region LCLU constants
    /// <summary>
    /// 
    /// </summary>
    public struct LCLUconstants
    {
        int Fyear;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        public LCLUconstants(int year)
        {
            Fyear = year;
        }
        /// <summary>
        /// 
        /// </summary>
        public const double A = 74875.1;
        /// <summary>
        /// 
        /// </summary>
        public const double B = -0.4306;
        /// <summary>
        /// 
        /// </summary>
        public const double C = -0.0548;
        //
        /// <summary>
        /// 
        /// </summary>
        public const double MandD_slope = 67.25;
        /// <summary>
        /// 
        /// </summary>
        public const double MandD_power = 0.6541;
        //
        /// <summary>
        ///  Various numberof units per acre for the difference density classes
        /// </summary>
        public const double UHunitsAcre = 10;
        /// <summary>
        /// 
        /// </summary>
        public const double ULunitsAcre = 5.8;/// <summary>
        /// 
        /// </summary>
        public const double SUBunitsAcre = 1;/// <summary>
        /// 
        /// </summary>
        public const double ExUHunitsAcre = 0.25;/// <summary>
        /// 
        /// </summary>
        public const double ExULunitsAcre = 0.06;/// <summary>
        /// 
        /// </summary>
        //
        public const double UHunitsSMFacre = 16;/// <summary>
        /// 
        /// </summary>
        public const double UHunitsWMFacre = 24.3;/// <summary>
        /// 
        /// </summary>
        public const double UHunitsMMFacre = 71.8;/// <summary>
        /// 
        /// </summary>
        public const double UHunitsHMFacre = 115.2;/// <summary>
        /// 
        /// </summary>
        public const double ULunitsLSFacre = 2.8;/// <summary>
        /// 
        /// </summary>
        public const double ULunitsTSFacre = 5.1;/// <summary>
        /// 
        /// </summary>
        public const double ULunitsSSFacre = 8.6;


        //
    }
    #endregion LCLU constants
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
        StreamWriter SW;
        //
        /// <summary>
        /// 
        /// </summary>
        public RateDataClass FRDC;
        /// <summary>
        ///  land-cover land-use original three-variable data
        /// </summary>
        public DataClassLCLU FDClclu;
        //
        // Version 2 ICLUS
        /// <summary>
        ///  Updated LCLU for manymore classes
        /// </summary>
        public DataClassLcluArea FDLCLU;
        //
        UrbanDensityDataClass FUDDataClass;
        //double Fdemand;
        // constants
        // This is the coeffecient to convert USGS MGD Consumer/resource numbers to gallons
        const double convertDemand = 1000000;
        const int initialYear = 2015;
        //
        // int FUnitCode = 0;
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
        //Small multi-Family SMF		16.0
        //Three story walkup WMF		24.3
        //Mid-range multi-family MMF	71.8
        //High density multi-family HMF 115.2
        // Urban Low              1.6  < UL < 10 DUA
        //Large single family LSF		2.8
        //Traditional single family 	5.1
        //Small single Family SSF		8.6
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
            // crf.URBAN = this;
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
            //crf.URBAN = this;
            CRF.URBAN = this; // does this make a difference?
            AddDensityClassesToList();
        }
        /// <summary>
        ///  02.09.22 das possible last constructor
        /// </summary>
        /// <param name="crf"></param>
        /// <param name="TheRateData"></param>
        /// <param name="TheAcreData"></param>
        /// <param name="FD"></param>
        /// <param name="UD"></param>
        public RuralDemand_LCLU_urban(WaterSimCRFModel crf, RateDataClass TheRateData, DataClassLCLU TheAcreData,
            DataClassLcluArea FD, UrbanDensityDataClass UD)
        {
            CRF = crf;
            FRDC = TheRateData;
            FDClclu = TheAcreData;
            FDLCLU = FD;
            FUDDataClass = UD;
            //
            SetBaseValues();
            isInstantiated = true;
            // assigns itself to the owner
            //crf.URBAN = this;
            CRF.URBAN = this; // does this make a difference?
            AddDensityClassesToList();
        }
        #endregion Constructors
        // ================================================================================================================================================
        internal void AddDensityClassesToList()
        {
            FDensityClassList.Add("UL_LSF");
            FDensityClassList.Add("UL_TSF");
            FDensityClassList.Add("UL_SSF");
            FDensityClassList.Add("UH_SMF");
            FDensityClassList.Add("UH_WMF");
            FDensityClassList.Add("UH_MMF");
            FDensityClassList.Add("UH_HMF");
            //
            FDensityClassList.Add("SUB");
            FDensityClassList.Add("ExUH");
            FDensityClassList.Add("ExUL");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal void Demand(int currentYear, StreamWriter sw)
        {
            double temp = 0;
            double NewDemand = 0;
            //
            preProcessDemand(currentYear, sw);

            double period = (currentYear - CRF.startYear) + 1;
            double outValue;
            // string region = CRF.UnitName;
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
        internal void Demand(int currentYear)
        {
            double temp = 0;
            double NewDemand = 0;
            //
            preProcessDemand(currentYear);

            double period = (currentYear - CRF.startYear) + 1;
            double outValue;
            // string region = CRF.UnitName;
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
        //=============
        double UH_SMF
        {
            get; set;
        }
        double UH_WMF
        {
            get; set;
        }
        double UH_MMF
        {
            get; set;
        }
        double UH_HMF
        {
            get; set;
        }
        // ============

        //
        double UL
        {
            get; set;
        }
        //
        double UL_LSF
        {
            get; set;
        }
        double UL_TSF
        {
            get; set;
        }
        double UL_SSF
        {
            get; set;
        }
        // ============




        //
        double SUB
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
        double UHunits
        {
            get; set;
        }
        //
        double UH_SMFunits
        {
            get; set;
        }
        double UH_WMFunits
        {
            get; set;
        }
        double UH_MMFunits
        {
            get; set;
        }
        double UH_HMFunits
        {
            get; set;
        }
        //
        double ULunits
        {
            get; set;
        }
        //
        double UL_LSFunits
        {
            get; set;
        }
        double UL_TSFunits
        {
            get; set;
        }
        double UL_SSFunits
        {
            get; set;
        }

        //
        double Subunits
        {
            get; set;
        }
        double ExUHunits
        {
            get; set;
        }
        double ExULunits
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
        //
        double rUL_LSF
        { get; set; }
        double rUL_TSF
        { get; set; }
        double rUL_SSF
        { get; set;
        }
        double rUH_SMF
        { get; set; }
        double rUH_WMF
        { get; set; }
        double rUH_MMF
        { get; set; }
        double rUH_HMF
        { get; set; }
        //

        //
        double demand
        { get; set; }
        #endregion properties
        // =========================================
        // =========================================
        // Conservation
        double _urbanConservation = 1.000;
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
        ///  Temporary method for data error checking
        /// </summary>
        /// <param name="currentYear"></param>
        /// <param name="sw"></param>
        /// <returns></returns>
        public override double GetDemand(int currentYear, StreamWriter sw)
        {
            double temp = 0;
            Demand(currentYear, sw);
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
        /// <summary>
        /// 
        /// </summary>
        public override void SetDemandFactors()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
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
        /// <summary>
        ///  Delete when done testing
        /// </summary>
        /// <param name="okay"></param>
        public override void preProcessDemand(int okay)
        { }
      
        // ====================
        //public override void preProcessDemand(int currentYear, StreamWriter sw)
        //{ }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentYear"></param>
        /// <param name="sw"></param>
        public void preProcessDemand(int currentYear, StreamWriter sw)
        {
            // Old data - Ag, Urban, Industry
            string region = CRF.UnitName;
            LurbanConservation = CRF.UrbanConservation;
            // Updated LCLU now with Denver Water Data analyses
            if (FDLCLU != null)
            {
                DUAdemand = true;
                switchLCLU(region, currentYear, sw);
            }
            else
            {
                DUAdemand = false;
                LurbanLCLUChangeCoef = CRF.PUrbanLCLUChangeCoef;
                Lacres = FDClclu.FastUrbanAcres(CRF.UnitName, currentYear);
                LBaseRate = FRDC.FastUrbanRateLCLU(CRF.FUnitName);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="currentYear"></param>
        /// <param name="sw"></param>
        public void switchLCLU(string region, int currentYear, StreamWriter sw)
        {
            double period = (currentYear - CRF.startYear) + 1;
            //
            AssignLCLU(region, currentYear);
            ProcessRequest(region, currentYear, sw);
            convertAcresToUnits();
            calculateDemand(region, currentYear, LurbanConservation, period, sw);
        }
      

        // ---------------------------------------------------
        /// <summary>
        ///   On 01.28.22 I have modified the code to reflect the Denver Water Data density classes
        ///   
        /// </summary>
        /// <param name="region"></param>
        /// <param name="currentYear"></param>
        public void AssignLCLU(string region, int currentYear)
        {
            UH = FDLCLU.FastArea_UN(region, "EigthAcre", currentYear);
            if (FUDDataClass != null)
            {
                HighIntensityDynamicPercentages(region, UH);
            }
            else
            {
                HighIntensityPercentages(UH);
            }
            UL = FDLCLU.FastArea_UN(region, "QuarterAcre", currentYear);
            if (FUDDataClass != null)
            {
                LowIntensityDynamicPercentages(region, UL);
            }
            else
            {
                LowIntensityPercentages(UL);
            }
            SUB = FDLCLU.FastArea_UN(region, "ThirdAcre", currentYear);
            ExUH = FDLCLU.FastArea_UN(region, "HalfAcre", currentYear);
            ExUL = FDLCLU.FastArea_UN(region, "Acre", currentYear);
        }
        //
        void dataCheck(string region, int currentYear, StreamWriter sw)
        {
            double Tacres = totalAcres(region, currentYear);
            double Aacres = UL_LSF + UL_TSF + UL_SSF + UH_SMF + UH_WMF + UH_MMF + UH_HMF + SUB + ExUH + ExUL;
            //double Init = init_LSF + init_TSF + init_SSF + init_SMF + init_WMF + init_MMF + init_HMF + init_SUB + init_ExH + init_ExL;
            //sw.WriteLine(region + "," + currentYear + "," + Tacres + "," + Aacres + "," + Init);
            sw.WriteLine(region + "," + currentYear + "," + UL_LSF + "," + UL_TSF + "," + UL_SSF + "," + UH_SMF + ","
                + UH_WMF + "," + UH_MMF + "," + UH_HMF + "," + SUB + "," + ExUH + "," + ExUL + "," + Tacres + "," + Aacres);
            // sw.WriteLine(region + "," + currentYear + "," + rUL_LSF + "," + rUL_TSF + "," + rUL_SSF + "," + rUH_SMF + ","
            //+ rUH_WMF + "," + rUH_MMF + "," + rUH_HMF + "," + rSUB + "," + rExUH + "," + rExUL + "," + Tacres + "," + Aacres);
        }

        // =============================================================
        // edits 01.28.22 das
        #region percentages for urban density
        internal void HighIntensityPercentages(double gross)
        {
            UH_SMF = gross * 0.35;
            UH_WMF = gross * 0.453;
            UH_MMF = gross * 0.124;
            UH_HMF = gross * 0.073;
        }
        internal void LowIntensityPercentages(double gross)
        {
            UL_LSF = gross * 0.207;
            UL_TSF = gross * 0.708;
            UL_SSF = gross * 0.085;
        }
        // end edits 01.28.22 das
        // edits 02.09.22 das
        internal void HighIntensityDynamicPercentages(string region, double gross)
        {
            UH_SMF = gross * FUDDataClass.FastSMFproportion(region);
            UH_WMF = gross * FUDDataClass.FastWMFproportion(region);
            UH_MMF = gross * FUDDataClass.FastMMFproportion(region);
            UH_HMF = gross * FUDDataClass.FastHMFproportion(region);
        }
        internal void LowIntensityDynamicPercentages(string region, double gross)
        {
            UL_LSF = gross * FUDDataClass.FastLSFproportion(region);
            UL_TSF = gross * FUDDataClass.FastTSFproportion(region);
            UL_SSF = gross * FUDDataClass.FastSSFproportion(region);
        }
        #endregion
        // end edits 02.09.22 das
        // =============================================================
        internal double totalAcres(string region, int year)
        {
            return FDLCLU.FastTotalUrbanArea_UN(region, year);
        }

        //
        //Urban high intensity: 10 DUA < UH  (assume 10)
        //Urban low intensity: 1.6 < UL< 10 DUA (assume 5.8)
        //
        //Suburban: 0.4 < S< 1.6 DUA (assume 1)
        //Exurban high intensity: 0.1 < EH< 0.4 DUA (assume 0.25)
        //Exurban low intensity: 0.02 < EL< 0.1 DUA (assume 0.06)
        //
        internal void convertAcresToUnits()
        {
            UHunits = LCLUconstants.UHunitsAcre * UH;
            UH_SMFunits = LCLUconstants.UHunitsSMFacre * UH_SMF;
            UH_WMFunits = LCLUconstants.UHunitsWMFacre * UH_WMF;
            UH_MMFunits = LCLUconstants.UHunitsMMFacre * UH_MMF;
            UH_HMFunits = LCLUconstants.UHunitsHMFacre * UH_HMF;
            //
            ULunits = LCLUconstants.ULunitsAcre * UL;
            UL_LSFunits = LCLUconstants.ULunitsLSFacre * UL_LSF;
            UL_TSFunits = LCLUconstants.ULunitsTSFacre * UL_TSF;
            UL_SSFunits = LCLUconstants.ULunitsSSFacre * UL_SSF;
            //
            Subunits = LCLUconstants.SUBunitsAcre * SUB;
            ExUHunits = LCLUconstants.ExUHunitsAcre * ExUH;
            ExULunits = LCLUconstants.ExULunitsAcre * ExUL;
        }
        internal void convertAcresToUnits(StreamWriter sw, int year, string region)
        {
            UHunits = LCLUconstants.UHunitsAcre * UH;
            UH_SMFunits = LCLUconstants.UHunitsSMFacre * UH_SMF;
            UH_WMFunits = LCLUconstants.UHunitsWMFacre * UH_WMF;
            UH_MMFunits = LCLUconstants.UHunitsMMFacre * UH_MMF;
            UH_HMFunits = LCLUconstants.UHunitsHMFacre * UH_HMF;
            //
            ULunits = LCLUconstants.ULunitsAcre * UL;
            UL_LSFunits = LCLUconstants.ULunitsLSFacre * UL_LSF;
            UL_TSFunits = LCLUconstants.ULunitsTSFacre * UL_TSF;
            UL_SSFunits = LCLUconstants.ULunitsSSFacre * UL_SSF;
            //
            Subunits = LCLUconstants.SUBunitsAcre * SUB;
            ExUHunits = LCLUconstants.ExUHunitsAcre * ExUH;
            ExULunits = LCLUconstants.ExULunitsAcre * ExUL;
            //
            if (region == "Arizona Central South")
            {
                //dataCheck(region, year, sw);


                sw.WriteLine(FUnitName + "," + year + "," + UH_SMFunits + "," + UH_WMFunits + "," + UH_MMFunits + "," +
                    UH_HMFunits + "," + UL_LSFunits + "," + UL_TSFunits + "," + UL_SSFunits + "," + Subunits
                     + "," + ExUHunits + "," + ExULunits);
            }
            //
        }
        // ========================================================================
        // NOTE: THe rule would have to be that you can
        // only change one density class at a time
        // This is the LAST METHOD to update
        // 01.28.22 das
        List<string> FDensityClassList = new List<string>();
        // ==============================================
        internal void ProcessRequest(string region, int year)
        {
            throw new NotImplementedException();
        }
        //
        // =======================================================================
        internal void ProcessRequest(string region, int year, StreamWriter sw)
        {
            double TAcres = totalAcres(region, year);

            if (CRF.UrbanDensityChangeCheck == 0)
            {
                // No changes to the density parameters, so skip this method to speed
                // up runtime.
            }
            else
            {
                double sum = Math.Round((UL_LSF + UL_TSF + UL_SSF + UH_SMF + UH_WMF + UH_MMF + UH_HMF + SUB + ExUH + ExUL));
                REL(sum);
                double density = 0;
                double difference = 0;
                //
                foreach (string DClass in FDensityClassList)
                {
                    bool[] follow = { false, false, false, false, false, false, false, false, false, false };
                    Ratecheck(follow);

                    if (DClass == "UL_LSF")
                    {
                        if (follow[0])
                        {
                            double a = UL_LSF;
                            density = UL_LSF = UL_LSF * CRF.UrbanLowDensityChangeLSF;
                            difference = Math.Abs(a - density);
                        }
                    }
                    if (DClass == "UL_TSF")
                    {
                        if (follow[1])
                        {
                            double a = UL_TSF;
                            density = UL_TSF = UL_TSF * CRF.UrbanLowDensityChangeTSF;
                            difference = Math.Abs(a - density);
                        }
                    }
                    if (DClass == "UL_SSF")
                    {
                        if (follow[2])
                        {
                            double a = UL_SSF;
                            density = UL_SSF = UL_SSF * CRF.UrbanLowDensityChangeSSF;
                            difference = Math.Abs(a - density);
                        }
                    }
                    //
                    if (DClass == "UH_SMF")
                    {
                        if (follow[3])
                        {
                            double a = UH_SMF;
                            density = UH_SMF = UH_SMF * CRF.UrbanHighDensityChangeSMF;
                            difference = Math.Abs(a - density);
                        }
                    }
                    if (DClass == "UH_WMF")
                    {
                        if (follow[4])
                        {
                            double a = UH_WMF;
                            density = UH_WMF = UH_WMF * CRF.UrbanHighDensityChangeWMF;
                            difference = Math.Abs(a - density);
                        }
                    }
                    if (DClass == "UH_MMF")
                    {
                        if (follow[5])
                        {
                            double a = UH_MMF;
                            density = UH_MMF = UH_MMF * CRF.UrbanHighDensityChangeMMF;
                            difference = Math.Abs(a - density);
                        }
                    }
                    if (DClass == "UH_HMF")
                    {
                        if (follow[6])
                        {
                            double a = UH_HMF;
                            density = UH_HMF = UH_HMF * CRF.UrbanHighDensityChangeHMF;
                            difference = Math.Abs(a - density);
                        }
                    }
                    //
                    if (DClass == "SUB")
                    {
                        if (follow[7])
                        {
                            double a = SUB;
                            density = SUB = SUB * CRF.UrbanHighDensityChangeWMF;
                            difference = Math.Abs(a - density);
                        }
                    }
                    if (DClass == "ExUH")
                    {
                        if (follow[8])
                        {
                            double a = ExUH;
                            density = ExUH = ExUH * CRF.UrbanHighDensityChangeMMF;
                            difference = Math.Abs(a - density);
                        }
                    }
                    if (DClass == "ExUL")
                    {
                        if (follow[9])
                        {
                            double a = ExUL;
                            density = ExUL = ExUL * CRF.UrbanHighDensityChangeHMF;
                            difference = Math.Abs(a - density);
                        }
                    }
                    difference = Math.Round(difference, 2);
                    AcreExchange(follow, TAcres, difference);
                }
            }
            if (region == "Arizona Central South")
            {
                //dataCheck(region, year, sw);
            }
        }
        void Ratecheck(bool[] follow)
        {
            if (CRF.UrbanLowDensityChangeLSF != 1) { follow[0] = true; }
            if (CRF.UrbanLowDensityChangeTSF != 1) { follow[1] = true; }
            if (CRF.UrbanLowDensityChangeSSF != 1) { follow[2] = true; }
            if (CRF.UrbanHighDensityChangeSMF != 1) { follow[3] = true; }
            if (CRF.UrbanHighDensityChangeWMF != 1) { follow[4] = true; }
            if (CRF.UrbanHighDensityChangeMMF != 1) { follow[5] = true; }
            if (CRF.UrbanHighDensityChangeHMF != 1) { follow[6] = true; }
            //
            if (CRF.SuburbanDensityChange != 1) { follow[7] = true; }
            if (CRF.ExurbanHighDensityChange != 1) { follow[8] = true; }
            if (CRF.ExurbanLowDensityChange != 1) { follow[9] = true; }
        }
        void AcreExchange(bool[] follow, double TAcres, double diff)
        {
            ProcessUpdates(follow, TAcres, diff);
        }

        void ProcessUpdates(bool[] follow, double TAcres, double diff)
        {
            if (!follow[0]) { UL_LSF = update(TAcres, rUL_LSF, diff); }
            if (!follow[1]) { UL_TSF = update(TAcres, rUL_TSF, diff); }
            if (!follow[2]) { UL_SSF = update(TAcres, rUL_SSF, diff); }
            if (!follow[3]) { UH_SMF = update(TAcres, rUH_SMF, diff); }
            if (!follow[4]) { UH_WMF = update(TAcres, rUH_WMF, diff); }
            if (!follow[5]) { UH_MMF = update(TAcres, rUH_MMF, diff); }
            if (!follow[6]) { UH_HMF = update(TAcres, rUH_HMF, diff); }
            if (!follow[7]) { SUB = update(TAcres, rSUB, diff); }
            if (!follow[8]) { ExUH = update(TAcres, rExUH, diff); }
            if (!follow[9]) { ExUL = update(TAcres, rExUL, diff); }
        }
        internal double update(double A, double B, double C)
        {
            double temp = 0;
            temp = Math.Round(A * B + C * B, 2);
            return temp;
        }

        // =================================================================================

        #region relative acres estimates
        void REL(double sum)
        {
            rUL_LSF = UL_LSF / sum;
            rUL_TSF = UL_TSF / sum; rUL_SSF = UL_SSF / sum; rUH_SMF = UH_SMF / sum;
            rUH_WMF = UH_WMF / sum; rUH_MMF = UH_MMF / sum; rUH_HMF = UH_HMF / sum;
            rSUB = SUB / sum; rExUH = ExUH / sum; rExUL = ExUL / sum;
        }

        #endregion relative acres
        // =================================================================================

        // ============================================================
        // the properties to hold relative area for each density class
        #region more properties-relative Acres & initial acres per class     
        // ==========================================================
        // Initial estimats of acres in each density class
        double density
        { get; set; }
        double difference
        { get; set; }
        #endregion properties - relative acres and initial acres
        // ==========================================================

        // =======================================================================
        // Add the DUA estimates for each density class
        // ICLUS data version 2.
        //Urban high intensity: 10 DUA < UH 
        //Urban low intensity: 1.6 < UL< 10 DUA
        //Suburban: 0.4 < S< 1.6 DUA
        //Exurban high intensity: 0.1 < EH< 0.4 DUA
        //Exurban low intensity: 0.02 < EL< 0.1 DUA

        double DUA(string urban)
        {
            double temp = 0;
            double result = 0;
            temp = FDLCLU.FastDUA_(urban);

            result = temp;
            return result;
        }
        double PPH(string urban)
        {
            double temp = 0;
            double result = 0;
            temp = FDLCLU.FastPPH_(urban);

            result = temp;
            return result;
        }
        // 
        /// <summary>
        /// Gallons per year by LCLU Urban density as dwelling units per acre
        /// Equation from Denver Water data.
        /// </summary>
        /// <param name="LCLUclass"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        double OutdoorDemandFromDUA(string LCLUclass, string region)
        {
            double temp = 0;
            double result = 0;
            // const double A = 74875.1; const double B = -0.4306; const double C = -0.0548;
            if (FUDDataClass != null)
            {
                temp = DynamicDUA(LCLUclass, region);         
            }
            else
            {
                temp = DUA(LCLUclass);
            }
            if (0 < temp) result = LCLUconstants.A * Math.Pow(temp, LCLUconstants.B) * Math.Exp(LCLUconstants.C / temp);
            return result;
        }
        /// <summary>
        ///  From Mayer and DeOreo
        /// </summary>
        /// <param name="LCLUclass"></param>
        /// <returns></returns>
        double IndoorDemandFromPPH(string LCLUclass)
        {
            double temp = 0;
            double result = 0;
            temp = PPH(LCLUclass);
            //result = 67.25 * Math.Pow(temp,0.6541);
            result = LCLUconstants.MandD_slope * Math.Pow(temp, LCLUconstants.MandD_power);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="LCLUclass"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public double DemandFromDUA(string LCLUclass, string region)
        {
            double result = 0;
            result = OutdoorDemandFromDUA(LCLUclass, region) + IndoorDemandFromPPH(LCLUclass);
            return result;
        }
        internal bool DUAdemand
        {
            get; set;
        }
        // =============================================================================================
        internal void calculateDemand(string region, int year, double AdjustEfficiency, double period, StreamWriter sw)
        {
            double result = 0;
            double temp = 0;
            //double Temp = 0;
            double Base = 0;
            double Standard = 0;
            double resultLSF = 0; double resultTSF = 0; double resultSSF = 0; double resultSMF = 0; double resultWMF = 0;
            double resultMMF = 0; double resultHMF = 0; double resultSUB = 0; double resultExH = 0; double resultExL = 0;
            double ModifyRate = 1;
            try
            {
                // Get the modify factor to apply for this period using the coefficient AdjustDamper
                //double ModifyRate = ChangeIncrement(1, period, AdjustDamper, MinValue); // NOT MinValue ... its the Limit in his code
                double startValue = 1;

                if (DUAdemand)
                {
                    double days = utilities.daysInAYear(year);

                    // Calculate water demand from the Denver Water data outdoor water use per DUA (equation that I created) plus 
                    // the Meyer and DeOreo equation for indoor water use from the 2016 WRF_ResidentialEndUse2016.pdf document.
                    // Figure 8.7 using the 2016 data
                    // units: units * gallons per unit = gallons
                    // ========================================================
                    //
                    result = temp = AdjustEfficiency * UH_SMFunits * DemandFromDUA("SMF", region) *
                            utilities.AnnualExponentialChange(startValue, period, CRF.FUrbanHD_SMFChangeCoef, CRF.UrbanHighDensityChangeSMF);
                    resultSMF = tempTesting(temp, days);
                    //
                    Base = (AdjustEfficiency * UH_WMFunits * DemandFromDUA("WMF", region) * utilities.convertGallonsMG) / days;
                    temp = AdjustEfficiency * UH_WMFunits * DemandFromDUA("WMF", region) *
                            utilities.AnnualExponentialChange(startValue, period, CRF.FUrbanHD_WMFChangeCoef, CRF.UrbanHighDensityChangeWMF);
                    result += temp;
                    resultWMF = tempTesting(temp, days);
                    //
                    temp = AdjustEfficiency * UH_MMFunits * DemandFromDUA("MMF", region) *
                            utilities.AnnualExponentialChange(startValue, period, CRF.FUrbanHD_MMFChangeCoef, CRF.UrbanHighDensityChangeMMF);
                    result += temp;
                    resultMMF = tempTesting(temp, days);
                    //
                    temp = AdjustEfficiency * UH_HMFunits * DemandFromDUA("HMF", region) *
                            utilities.AnnualExponentialChange(startValue, period, CRF.FUrbanHD_HMFChangeCoef, CRF.UrbanHighDensityChangeHMF);
                    result += temp;
                    resultHMF = tempTesting(temp, days);

                    //
                    temp = AdjustEfficiency * UL_LSFunits * DemandFromDUA("LSF", region) *
                            utilities.AnnualExponentialChange(startValue, period, CRF.FUrbanLD_LSFChangeCoef, CRF.UrbanLowDensityChangeLSF);
                    result += temp;
                    resultLSF = tempTesting(temp, days);
                    //
                    // ===============================================================================================
                    //
                    // Base = (AdjustEfficiency * UL_TSFunits * DemandFromDUA("TSF") * utilities.convertGallonsMG) / days;
                    Standard = ((AdjustEfficiency * UL_TSFunits * DemandFromDUA("TSF", region) * CRF.UrbanLowDensityChangeTSF) * utilities.convertGallonsMG) / days;
                    temp = AdjustEfficiency * UL_TSFunits * DemandFromDUA("TSF", region) *
                        utilities.AnnualExponentialChange(startValue, period, CRF.FUrbanLD_TSFChangeCoef, CRF.UrbanLowDensityChangeTSF);
                    result += temp;
                    resultTSF = tempTesting(temp, days);
                    //
                    // ==============================================================================================
                    //
                    temp = AdjustEfficiency * UL_SSFunits * DemandFromDUA("SSF", region) *
                            utilities.AnnualExponentialChange(startValue, period, CRF.FUrbanLD_SSFChangeCoef, CRF.UrbanLowDensityChangeSSF);
                    result += temp;
                    resultSSF = tempTesting(temp, days);

                    //
                    temp = AdjustEfficiency * Subunits * DemandFromDUA("ThirdAcre", region) *
                            utilities.AnnualExponentialChange(startValue, period, CRF.FSuburbanDensityChangeCoef, CRF.SuburbanDensityChange);
                    result += temp;
                    resultSUB = tempTesting(temp, days);

                    temp = AdjustEfficiency * ExUHunits * DemandFromDUA("HalfAcre", region) *
                            utilities.AnnualExponentialChange(startValue, period, CRF.FExurbanHighDensityChangeCoef, CRF.ExurbanHighDensityChange);
                    result += temp;
                    resultExH = tempTesting(temp, days);

                    temp = AdjustEfficiency * ExULunits * DemandFromDUA("Acre", region) *
                            utilities.AnnualExponentialChange(startValue, period, CRF.FExurbanLowDensityChangeCoef, CRF.ExurbanLowDensityChange);
                    result += temp;
                    resultExL = tempTesting(temp, days);
                }
                else
                {
                    ModifyRate = utilities.AnnualExponentialChange(startValue, period, CRF.FUrbanHighDensityChangeCoef, CRF.UrbanHighDensityChange);

                    // Use the rates that are read in from the CSV file for each LCLU class
                    // File is WestModelGrowthRates_5.csv
                    // NOTE: these data need updating.......
                    result = AdjustEfficiency * UH * FRDC.FastUrbanHighRate(region) * ModifyRate;
                    //
                    ModifyRate = utilities.AnnualExponentialChange(startValue, period, CRF.FUrbanLowDensityChangeCoef, CRF.UrbanLowDensityChange);
                    result += AdjustEfficiency * UL * FRDC.FastUrbanLowRate(region) * ModifyRate;
                    //
                    ModifyRate = utilities.AnnualExponentialChange(startValue, period, CRF.FSuburbanDensityChangeCoef, CRF.SuburbanDensityChange);
                    result += AdjustEfficiency * SUB * FRDC.FastSuburbanRate(region) * ModifyRate;
                    //
                    ModifyRate = utilities.AnnualExponentialChange(startValue, period, CRF.FExurbanHighDensityChangeCoef, CRF.ExurbanHighDensityChange);
                    result += AdjustEfficiency * ExUH * FRDC.FastExurbanHighRate(region) * ModifyRate;
                    //
                    ModifyRate = utilities.AnnualExponentialChange(startValue, period, CRF.FExurbanLowDensityChangeCoef, CRF.ExurbanLowDensityChange);
                    result += AdjustEfficiency * ExUL * FRDC.FastExurbanLowRate(region) * ModifyRate;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("what modify rate in rural demand LCLU",ex);
                // Ouch Only thing going here is the Change Increment Function
            }
            //demand = result; // set the property labeled "demand"
            double Days = utilities.daysInAYear(year);
            demand = (result * utilities.convertGallonsMG) / Days;  // set the property labeled "demand"
            //if (region == "Arizona Central South")
            //{
            //    //sw.WriteLine(region + "," + year + "," + resultWMF + ","  + Base  );
            //    sw.WriteLine(region + "," + year + "," + resultLSF + "," + resultTSF + "," + resultSSF + "," + resultSMF + ","
            //     + resultWMF + "," + resultMMF + "," + resultHMF + "," + resultSUB + "," + resultExH + "," + resultExL + "," + demand);

            //    //DemandCheck(region, year, demand, sw);
            //}
        }
        // Testing
        internal double tempTesting(double root, double days)
        {
            double result = 0;
            result = root * utilities.convertGallonsMG / days;
            return result;
        }
        void DemandCheck(string region, int currentYear, double demand, StreamWriter sw)
        {
            sw.WriteLine(region + "," + currentYear + "," + resultLSF + "," + resultTSF + "," + resultSSF + "," + resultSMF + ","
               + resultWMF + "," + resultMMF + "," + resultHMF + "," + resultSUB + "," + resultExH + "," + resultExL + "," + demand);
        }
        double resultLSF
        { get; set; }
        double resultTSF
        { get; set; }
        double resultSSF
        { get; set; }
        double resultSMF
        { get; set; }
        double resultWMF
        { get; set; }
        double resultMMF
        { get; set; }
        double resultHMF
        { get; set; }
        double resultSUB
        { get; set; }
        double resultExH
        { get; set; }
        double resultExL
        { get; set; }

        // ============================================================================================================
    
        // ==============================================================================================================================================

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lcluClass"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public double DynamicDUA(string lcluClass, string region)
        {
            double temp = 0;
            double DUA = 0;
            switch (lcluClass)
            {
                case "EigthAcre":
                    temp = 10;
                    break;
                case "SMF":
                    temp = FUDDataClass.FastSMFDUA(region);
                    break;
                case "WMF":
                    temp = FUDDataClass.FastWMFDUA(region);
                    break;
                case "MMF":
                    temp = FUDDataClass.FastMMFDUA(region);
                    break;
                case "HMF":
                    temp = FUDDataClass.FastHMFDUA(region);
                    break;

                case "QuarterAcre":
                    temp = 5.8;
                    break;
                case "LSF":
                    temp = FUDDataClass.FastLSFDUA(region);
                    break;
                case "TSF":
                    temp = FUDDataClass.FastTSFDUA(region);
                    break;
                case "SSF":
                    temp = FUDDataClass.FastSSFDUA(region);
                    break;

                case "ThirdAcre":
                    temp = 1;
                    break;
                case "HalfAcre":
                    temp = 0.25;
                    break;
                case "Acre":
                    temp = 0.06;
                    break;
            }
            DUA = temp;
            return DUA;
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
        /// <summary>
        /// Water use rate data for the difference land classifications
        /// </summary>
        public RateDataClass FRDC;
        // 
        /// <summary>
        /// Generic (three-way land classes- industry, urban, ag)
        /// </summary>
        public DataClassLCLU FDClclu;
        //
        //
        StreamWriter SW;
        //

        // Version 2 ICLUS
        /// <summary>
        /// ICLUS land cover land use acres
        /// </summary>
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
        /// <summary>
        ///  with the version II of ICLUS data for ag
        /// </summary>
        /// <param name="crf"></param>
        /// <param name="TheRateData"></param>
        /// <param name="TheAcreData"></param>
        /// <param name="FD"></param>
        public RuralDemand_LCLU_ag(WaterSimCRFModel crf, RateDataClass TheRateData, DataClassLCLU TheAcreData, DataClassLcluArea FD)
        {
            CRF = crf;
            FRDC = TheRateData;
            FDClclu = TheAcreData;
            FDLCLU = FD;
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
            demandAg = temp;
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
        /// <param name="currentYear"></param>
        /// <param name="sw"></param>
        /// <returns></returns>
        public override double GetDemand(int currentYear, StreamWriter sw)
        {
            double temp = 0;
            Demand(currentYear);

            temp = Math.Round(demandAg);
            return temp;
        }      /// <summary>
               /// 
               /// </summary>
        public override void Initialize_Variables()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        public override void SetDemandFactors()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        public override void SetBaseValues()
        {
            int year = 2010;
            LBaseRate = FRDC.FastAgRateLCLU(CRF.UnitName);
            Lacres = FDClclu.FastAgAcres(CRF.UnitName, year);
        }
        /// <summary>
        /// Either use the 3-way land classes, or the multiple land use from ICLUS
        /// </summary>
        /// <param name="year"></param>
        public override void switchUrbanLCLU(int year) { }
        // ========================================================================================
        //
        //public override void preProcessDemand(int currentYear)
        // Process
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentYear"></param>
        public override void preProcessDemand(int currentYear)
        {

            Lacres = FDClclu.FastAgAcres(CRF.UnitName, currentYear);
            // edits 10.20.21 das
            if (FDLCLU != null)
            {
                Lacres = FDLCLU.FastAgArea_UN(CRF.UnitName, currentYear);
            }
            // end edits 10.20.21 das
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
        public override void preProcessDemand()
        {
            throw new NotImplementedException();
        }
    }
    #endregion Rural Demand Class- LCLU Ag
    // ==============================================================================================================================================

    //public struct IndustryStruct
    //{
    //    int Fyear;
    //    string Fregion;
    //    double Fvalue;

    //    internal IndustryStruct(int A, string B,double C)
    //    {
    //        Fyear = A;
    //        Fregion = B;
    //        Fvalue = C;
    //    }
    //    public int YEAR
    //    {
    //        get { return Fyear; }
    //        set { Fyear = value; }
    //    }
    //    internal string Region
    //    {
    //        get { return Fregion; } set { Fregion = value; }

    //    }
    //    internal double Value
    //    { get { return Fvalue; } set{Fvalue = value; }
    //}


    // ==============================================================================================================================================
    #region Rural Demand Class- LCLU Industry
    /// <summary>
    /// 
    /// </summary>
    public class RuralDemand_LCLU_industry : DemandModel
    {
        // objects
        WaterSimCRFModel CRF;
        /// <summary>
        /// 
        /// </summary>
        public RateDataClass FRDC;
        /// <summary>
        /// 
        /// </summary>
        public DataClassLCLU FDClclu;
        //
        //
        StreamWriter SW;
        //

        double Fdemand;
        // List<Industry> industryList = new List<Industry>();
        internal const double irate = 0.03;
        // constants
        // This is the coeffecient to convert USGS MGD Consumer/resource numbers to gallons
        const double convertDemand = 1000000;
        const int initialYear = 2015;
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
            demandIndustry = count(currentYear, temp);
        }
        /// <summary>
        /// Keep Industrial demand from exceeding an annual growth
        /// rate of "irate"
        /// 11.22.21 das
        /// </summary>
        /// <param name="Y"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        internal double count(int Y, double values)
        {
            double result = 0;
            result = values;
          
             if (2015 < Y)
            {
                if (CRF.IndustryRegion == CRF.UnitName)
                {
                    if (values > CRF.IndustryValue * (1+ irate))
                    {
                        result = CRF.IndustryValue * (1 + irate);
                    }
                }
            }
            CRF.IndustryRegion = CRF.UnitName;
            CRF.IndustryValue = values;
            return result;
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
        /// <param name="currentYear"></param>
        /// <param name="sw"></param>
        /// <returns></returns>
        public override double GetDemand(int currentYear, StreamWriter sw)
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
        /// <summary>
        /// 
        /// </summary>
        public override void SetDemandFactors()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        public override void SetBaseValues()
        {
            int year = 2010;
            LBaseRate = FRDC.FastIndRateLCLU(CRF.UnitName);
            Lacres = FDClclu.FastIndAcres(CRF.UnitName, year);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        public override void switchUrbanLCLU(int year) { }
        // ========================================================================================
        //
        // Process
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentYear"></param>
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
        public DataLCLU(string aUnitName, string aUnitCode, double anAcerageAg, double anAcerageUrban, double anAcerageInd, int aYear)
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
        /// <summary>
        ///  Ag acres
        /// </summary>
        public double AcerageAg
        {
            get { return FAcerageAg; }
        }
        /// <summary>
        ///  Industrial acerage
        /// </summary>
        public double AcerageInd
        {
            get { return FAcerageInd; }
        }
        /// <summary>
        ///  The year of the data
        /// </summary>
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
        string FAcerageAgFieldStr = "AGACRES";
        string FAcerageIndFieldStr = "INDACRES";
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
            FDataDirectory = DataDirectory ;
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
                                    DataLCLU AD = new DataLCLU(namestr, codestr, TempAg, TempUrban, TempInd, TempYear);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnitName"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public double FastUrbanAcres(string UnitName, int year)
        {
            double temp = InvalidRate;
            DataLCLU TheData = FRuralDataList.Find(delegate (DataLCLU AD)
            {
                return ((AD.TheYear == year) && (AD.UnitName == UnitName));
            });

            if (TheData.UnitName == UnitName)
            {
                temp = TheData.AcerageUrban;
            }
            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnitName"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public double FastAgAcres(string UnitName, int year)
        {
            double temp = InvalidRate;
            DataLCLU TheData = FRuralDataList.Find(delegate (DataLCLU AD)
            {
                return ((AD.TheYear == year) && (AD.UnitName == UnitName));
            });
            if (TheData.UnitName == UnitName)
            {
                temp = TheData.AcerageAg;
            }
            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnitName"></param>
        /// <param name="year"></param>
        /// <returns></returns>
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
    /// <summary>
    /// 
    /// </summary>
    public struct IndustryStruct
    {
        int Fyear;
        string Fregion;
        double Fvalue;

        internal IndustryStruct(int A, string B, double C)
        {
            Fyear = A;
            Fregion = B;
            Fvalue = C;
        }
        /// <summary>
        /// Simulation year
        /// </summary>
        public int YEAR
        {
            get { return Fyear; }
            set { Fyear = value; }
        }
        internal string Region
        {
            get { return Fregion; }
            set { Fregion = value; }

        }
        internal double Value
        {
            get { return Fvalue; }
            set { Fvalue = value; }
        }
    }
}
