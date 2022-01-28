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
        public const double A = 74875.1;
        public const double B = -0.4306;
        public const double C = -0.0548;
        //
        public const double MandD_slope = 67.25;
        public const double MandD_power = 0.6541;
        //
        public const double UHunitsAcre = 10;
        public const double ULunitsAcre = 5.8;
        public const double SUBunitsAcre = 1;
        public const double ExUHunitsAcre = 0.25;
        public const double ExULunitsAcre = 0.06;
        //
        public const double UHunitsSMFacre = 16;
        public const double UHunitsWMFacre = 24.3;
        public const double UHunitsMMFacre = 71.8;
        public const double UHunitsHMFacre = 115.2;
        public const double ULunitsLSFacre = 2.8;
        public const double ULunitsTSFacre = 5.1;
        public const double ULunitsSSFacre = 8.6;


        //
    }

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
        const int initialYear = 2015;
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
            // Updated LCLU now with Denver Water Data analyses
            if (FDLCLU != null)
            {
                DUAdemand = true;
                switchLCLU(region, currentYear);
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
        /// ICLUS version 2 with five urban classes- This is invoked with the default urban
        /// setting is equal to a value of 3
        /// </summary>
        /// <param name="region"></param>
        /// <param name="currentYear"></param>
        public void switchLCLU(string region, int currentYear)
        {
            double period = (currentYear - CRF.startYear);// + 1;
            assignLCLU(region, currentYear);
            relativeAcres(region, currentYear);
            modifyArea(region, currentYear);
            convertAcresToUnits();
            calculateDemand(region, currentYear, LurbanConservation, LurbanLCLUChangeCoef, period);
        }
        /// <summary>
        ///   On 01.28.22 I have modified the code to reflect the Denver Water Data density classes
        ///   
        /// </summary>
        /// <param name="region"></param>
        /// <param name="currentYear"></param>
        public void assignLCLU(string region, int currentYear)
        {
            UH = FDLCLU.FastArea_UN(region, "EigthAcre", currentYear);
                HighIntensityPercentages(UH);
            UL = FDLCLU.FastArea_UN(region, "QuarterAcre", currentYear);
                LowIntensityPercentages(UL);
            SUB = FDLCLU.FastArea_UN(region, "ThirdAcre", currentYear);
            ExUH = FDLCLU.FastArea_UN(region, "HalfAcre", currentYear);
            ExUL = FDLCLU.FastArea_UN(region, "Acre", currentYear);
        }
        //
        // =============================================================
        // edits 01.28.22 das
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
        // =============================================================

        internal double totalAcres(string region, int year)
        {
            return FDLCLU.FastTotalUrbanArea_UN(region, year);
        }
        internal void relativeAcres(string region, int year)
        {
            rUH = UH / totalAcres(region, year);
            rUL = UL / totalAcres(region, year);
            rSUB = SUB / totalAcres(region, year);
            rExUH = ExUH / totalAcres(region, year);
            rExUL = ExUH / totalAcres(region, year);
        }
        //
        internal double Update(double A, double B, double E, double F)
        {
            double temp = 0;
            temp = (A - B) * (E / F);
            return temp;
        }
        //
        //Urban high intensity: 10 DUA < UH  (assume 10)
        //Urban low intensity: 1.6 < UL< 10 DUA (assume 5.8)
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


        // ========================================================================
        // NOTE: THe rule would have to be that you can
        // only change one density class at a time
        // This is the LAST METHOD to update
        // 01.28.22 das
        // ==============================================
        internal void modifyArea(string region, int year)
        {
            double TAcres = totalAcres(region, year);
            //double densityUH= Convert.ToDouble(CRF.geti_UrbanHighDensity()) / 100;
            double densityUH = CRF.UrbanHighDensityChange;
            //double densityUL = Convert.ToDouble(CRF.geti_UrbanLowDensity()) / 100;
            double densityUL = CRF.UrbanLowDensityChange;
            // double densitySUB = Convert.ToDouble(CRF.geti_SuburbanDensity()) / 100;
            double densitySUB = CRF.SuburbanDensityChange;
            //double densityExUH = Convert.ToDouble(CRF.geti_ExurbanHighDensity()) / 100;
            double densityExUH = CRF.ExurbanHighDensityChange;
            double densityExUL = CRF.ExurbanLowDensityChange;
            //
            if (year == 2020)
            {
                bool stop = true;
            }
            //
            if (densityUH != 1)
            {
                double newUH = UH * densityUH;
                double aSum = rExUL + rSUB + rExUH + rExUL;
                UH = newUH;
                UL = Update(TAcres, newUH, rUL, aSum);
                SUB = Update(TAcres, newUH, rSUB, aSum);
                ExUH = Update(TAcres, newUH, rExUH, aSum);
                ExUL = Update(TAcres, newUH, rExUL, aSum);
                LurbanLCLUChangeCoef = CRF.FUrbanHighDensityChangeCoef;
            }
            if (densityUL != 1)
            {
                double newUL = UL * densityUL;
                double aSum = rUH + rSUB + rExUH + rExUL;

                UL = newUL;
                UH = Update(TAcres, newUL, rUH, aSum);
                SUB = Update(TAcres, newUL, rSUB, aSum);
                ExUH = Update(TAcres, newUL, rExUH, aSum);
                ExUL = Update(TAcres, newUL, rExUL, aSum);
                LurbanLCLUChangeCoef = CRF.FUrbanLowDensityChangeCoef;
            }
            if (densitySUB != 1)
            {
                double newSUB = SUB * densitySUB;
                double aSum = rUH + rUL + rExUH + rExUL;

                SUB = newSUB;
                UH = Update(TAcres, newSUB, rUH, aSum);
                UL = Update(TAcres, newSUB, rUL, aSum);
                ExUH = Update(TAcres, newSUB, rExUH, aSum);
                ExUL = Update(TAcres, newSUB, rExUL, aSum);
                LurbanLCLUChangeCoef = CRF.FSuburbanDensityChangeCoef;
            }
            if (densityExUH != 1) // ok
            {
                double newExUH = ExUH * densityExUH;
                double aSum = rUH + rUL + rSUB + rExUL;

                ExUH = newExUH;
                UH = Update(TAcres, newExUH, rUH, aSum);
                UL = Update(TAcres, newExUH, rUL, aSum);
                SUB = Update(TAcres, newExUH, rSUB, aSum);
                ExUL = Update(TAcres, newExUH, rExUL, aSum);
                LurbanLCLUChangeCoef = CRF.FExurbanHighDensityChangeCoef;
            }
            if (densityExUL != 1) // okay
            {
                double newExUL = ExUL * densityExUL;
                double aSum = rUH + rUL + rSUB + rExUH;

                ExUL = newExUL;
                UH = Update(TAcres, newExUL, rUH, aSum);
                UL = Update(TAcres, newExUL, rUL, aSum);
                SUB = Update(TAcres, newExUL, rSUB, aSum);
                ExUH = Update(TAcres, newExUL, rExUH, aSum);
                LurbanLCLUChangeCoef = CRF.FExurbanLowDensityChangeCoef;
            }
            Lacres = UH + UL + SUB + ExUH + ExUL;
        }
        // =====================================================
        //
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
        /// 10.04.21 das
        /// </summary>
        /// <param name="LCLUclass"></param>
        /// <returns></returns>
        ///         
        double OutdoorDemandFromDUA(string LCLUclass)
        {
            double temp = 0;
            double result = 0;
            // const double A = 74875.1; const double B = -0.4306; const double C = -0.0548;
            temp = DUA(LCLUclass);
            if (0 < temp) result = LCLUconstants.A * Math.Pow(temp, LCLUconstants.B) * Math.Exp(LCLUconstants.C / temp);
            return result;
        }
        /// <summary>
        /// 
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
        ///  Gallons per unit per year
        /// </summary>
        /// <param name="LCLUclass"></param>
        /// <returns></returns>
        public double DemandFromDUA(string LCLUclass)
        {
            double result = 0;
            result = OutdoorDemandFromDUA(LCLUclass) + IndoorDemandFromPPH(LCLUclass);
            return result;
        }
        internal bool DUAdemand
        {
            get; set;
        }
        // =====================================================
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
                if (DUAdemand)
                {
                    // Calculate water demand from the Denver Water data outdoor water use per DUA (equation that I created) plus 
                    // the Meyer and DeOreo equation for indoor water use from the 2016 WRF_ResidentialEndUse2016.pdf document.
                    // Figure 8.7using the 2016 data
                    // units: units * gallons per unit = gallons
                    // ========================================================
                    result = UHunits * DemandFromDUA("EigthAcre") * ModifyRate;
                    //
                        result = UH_SMFunits * DemandFromDUA("SMF") * ModifyRate;
                        result = UH_WMFunits * DemandFromDUA("WMF") * ModifyRate;
                        result = UH_MMFunits * DemandFromDUA("MMF") * ModifyRate;
                        result = UH_HMFunits * DemandFromDUA("HMF") * ModifyRate;
                    //
                    result += ULunits * DemandFromDUA("QuarterAcre") * ModifyRate;
                    //
                        result = UL_LSFunits * DemandFromDUA("LSF") * ModifyRate;
                        result = UL_TSFunits * DemandFromDUA("TSF") * ModifyRate;
                        result = UL_SSFunits * DemandFromDUA("SSF") * ModifyRate;
                    //
                    result += Subunits * DemandFromDUA("ThirdAcre") * ModifyRate;
                    result += ExUHunits * DemandFromDUA("HalfAcre") * ModifyRate;
                    result += ExULunits * DemandFromDUA("Acre") * ModifyRate;
                }
                else
                {
                    // Use the rates that are read in from the CSV file for each LCLU class
                    // File is WestModelGrowthRates_5.csv
                    // NOTE: these data need updating.......
                    result = UH * FRDC.FastUrbanHighRate(region) * ModifyRate;
                    //
                    result += UL * FRDC.FastUrbanLowRate(region) * ModifyRate;
                    //
                    result += SUB * FRDC.FastSuburbanRate(region) * ModifyRate;
                    result += ExUH * FRDC.FastExurbanHighRate(region) * ModifyRate;
                    result += ExUL * FRDC.FastExurbanLowRate(region) * ModifyRate;
                }

            }
            catch (Exception ex)
            {
                // Ouch Only thing going here is the Change Increment Function
            }
            //demand = result; // set the property labeled "demand"
            double days = utilities.daysInAYear(year);
            demand = (result * utilities.convertGallonsMG) / days;  // set the property labeled "demand"
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
        /// <param name="yr"></param>
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
        public RateDataClass FRDC;
        public DataClassLCLU FDClclu;
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
        //   
        //public double[] AGRate()
        //{
        //    return GetDataArray(FAGRateFieldStr);
        //}


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
