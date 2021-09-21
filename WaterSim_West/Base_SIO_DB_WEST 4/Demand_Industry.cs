using System;

using WaterSimDCDC;
using WaterSimDCDC.Generic;
using DemandModel_Base;

namespace WaterSim_Base
{
  
    /// <summary>
    /// 
    /// </summary>
    public class IndustryDemand_employee : DemandModel
    {
        // objects
        WaterSimCRFModel CRF;
        //
        double Fdemand;
        // Industry
        // Number of Industrial Employees
        double FIndEmployees = 0;
        // The Initial number of employees
        double FIndInitialEmploy = 0;
        // Indistrial Change Coef for Employee Growth
        double FindEmpChangeCoef = 1;
        // Initial Industrial Galloans per Employee per day
        double FIndInitialGPED = 0;
        // Industrial Gallons per Employee per Day
        double FIndGPED = 0;
        // indistrial Minimum percent decline in GPED
        double FIndMinGPEDPercent = 0.10;
        // Indiustrial GPEDCHangeCoef
        double FindGPEDChangeCoef = 1;

    
        // This is the coeffecient to convert employee to persons
        public const double convertEmployee = 1000;
        // This is the coeffecient to convert USGS MGD Consumer/resource numbers to gallons
        public const double convertDemand = 1000000;

        int FUnitCode = 0;
        string FComment = "";

        public bool isInstantiated=false;
        //

        #region Constructors
        // ==================================================================================
        /// <summary>
        /// 
        /// </summary>
        /// <param name="crf"></param>
        public IndustryDemand_employee(WaterSimCRFModel crf)
        {
            CRF = crf;
            //
            Initialize_Variables();
            isInstantiated = true;
        }
        #endregion Constructors
        // ================================================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal void Demand(int currentYear)
        {
            double result;
            double temp = 0;
            preProcessEmployees();
            double period = (currentYear - CRF.startYear) + 1;
            LIndustrialEmployees = Industrial_Employees(period);
            LFindEmployees = LIndustrialEmployees;
            
            //
            preProcessDemand();
            temp = IndustrialDemand(period, LIndustrialEmployees);
            // this is gallons convert to MGD
            result = temp / convertDemand;
            demand = result;
        }


        // ================================================================================================================================================

        // Properties
        // =================================================================================================
        double _population = 0;
        double Lpopulation
        {
            get { return _population; }
            set { _population = value; }
        }
        //
        double _industrialEmployees = 0;
        internal double LIndustrialEmployees
        {
            set { _industrialEmployees = value; }
            get { return _industrialEmployees; }
        }
        //
        double _findEmployees = 0.0;
        double LFindEmployees
        {
            get { return _findEmployees; }
            set { _findEmployees = value; }
        }
        //
        double _findInitialEmploy = 0.0;
        double LFindInitialEmploy
        {
            get { return _findInitialEmploy; }
            set { _findInitialEmploy = value; }
        }
        //
        double _industrialGrowthRate = 0.0;
        double LIndustrialGrowthRate
        {
            get { return _industrialGrowthRate; }
            set { _industrialGrowthRate = value; }
        }
        // FindEmpChangeCoef
        double _findEmpChangeCoef = 1.0;
        double LFindEmpChangeCoef
        {
            get { return _findEmpChangeCoef; }
            set { _findEmpChangeCoef = value; }
        }
        // IndustryConservation
        double _industryConservation = 0.99;
        double LIndustryConservation
        {
            get { return _industryConservation; }
            set { _industryConservation = value; }
        }
        // PFIndInitialGPED
        double _findInitialGPED = 0.99;
        double LFindInitialGPED
        {
            get { return _findInitialGPED; }
            set { _findInitialGPED = value; }
        }
        // LFindGPEDChangeCoef
        double _findGPEDChangeCoef = 0.99;
        double LFindGPEDChangeCoef
        {
            get { return _findGPEDChangeCoef; }
            set { _findGPEDChangeCoef = value; }
        }


        // ====================================
        double _demand = 0;
        double demand
        {
            get { return _demand; }
            set { _demand = value; }
        }
        // =================================================================================================


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
            temp = demand;
            return temp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        internal double Industrial_Employees(double period)
        {
            double temp = 0;
            temp = utilities.Forecast_CU(LFindInitialEmploy, LIndustrialGrowthRate, LFindEmpChangeCoef, period, 1, 1);
            return temp;
            
        }
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="period"></param>
        /// <param name="Employees"></param>
        /// <returns></returns>
        internal double IndustrialDemand(double period, double Employees)
        {
            double temp = 0;
            SetDemandFactors();
            temp = EstimateConsumerDemand((Employees * convertEmployee), LFindInitialGPED, LIndustryConservation, LFindGPEDChangeCoef, FIndMinGPEDPercent, period);
            return temp;
        }

       

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize_Variables()
        {
            LFindInitialEmploy = CRF.PFIndInitialEmploy;
            LFindInitialGPED = CRF.PFIndInitialGPED;
        }
        /// <summary>
        ///  NOT SURE THIS WORKS YET...
        /// </summary>
        public override void SetDemandFactors()
        {
            LFindGPEDChangeCoef = CRF.PFindGPEDChangeCoef;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void SetBaseValues()
        {
            throw new NotImplementedException();
        }
        public override void switchUrbanLCLU(int year) { }
        // =============================================================================================================
        //
        // Process

        // ====================
        //  Pre Process
        /// <summary>
        /// 
        /// </summary>
        /// <param name="yr"></param>
        void preProcessEmployees()
        {
            Lpopulation = CRF.population;
            LIndustrialGrowthRate = CRF.IndustrialGrowthRate;
        }
        public override void preProcessDemand(int year)
        {
        }
        /// <summary>
        ///   
        /// </summary>
        /// <param name="yr"></param>
        public override void preProcessDemand()
        {
            double LFindEmployees = CRF.PIndustrialEmployees;
            
        }



    }
}
