using System;
using System.IO;
using WaterSimDCDC;
using WaterSimDCDC.Generic;
using DemandModel_Base;

namespace WaterSim_Base
{
  
    /// <summary>
    /// 
    /// </summary>
    public class AgriculturalDemand_income : DemandModel
    {
        // objects
        WaterSimCRFModel CRF;
        //
        StreamWriter sw;
        //
        double Fdemand;
       // holder for ag production
       // double FAgProduction = 0;
        const double MinimumAgGPDDPercentChange = 0.20;
        const double Damper = 1.01;
        const double AgGrowthDamper = 0.980;
        const double AgAdjustDamper = 0.990;
        const double AgEfficiencyDamper = 0.98;

        // AG
        double FMinAgGPDDPercent = 0.10;
        //double AgGPDDChangeCoef = 1;

        // constants
        // This is the coeffecient to convert USGS MGD Consumer/resource numbers to gallons
        const double convertDemand = 1000000;
 
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
        public AgriculturalDemand_income(WaterSimCRFModel crf)
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
            double temp = 0;
            preProcessProduction(currentYear);
            LAgProduction = Calc_AgProduction(currentYear, CRF.startYear);
            //
            preProcessDemand();
            int AgDemand = Calc_agDemand(currentYear, CRF.startYear, LAgProduction, LAgConservation);
            temp = AgDemand;
            demand = temp;
        }
  

        // ================================================================================================================================================

        // Properties
        // =================================================================================================

        double _agProduction = 0;
        internal double LAgProduction
        {
            set { _agProduction = value; }
            get { return _agProduction; }
        }
        //
        double _agNET = 0;
        double LagNet
        {
            get { return _agNET; }
            set { _agNET = value; }
        }
        //
        double _agricultureGrowthRate = 0;
        double LAgricultureGrowthRate
        {
            get { return _agricultureGrowthRate; }
            set { _agricultureGrowthRate = value; }
        }
        double _agAdjustProductionRate = 1.0;
        double LAgAdjustProductionRate
        {
            get { return _agAdjustProductionRate; }
            set { _agAdjustProductionRate = value; }
        }
        double _agConservation = 1.0;
        double LAgConservation
        {
            get { return _agConservation; }
            set { _agConservation = value; }
        }
        double _agInitialGPCD = 1.0;
        double LAgInitialGPCD
        {
            get { return _agInitialGPCD; }
            set { _agInitialGPCD = value; }
        }
        double _agGPDDChangeCoef = 1.0;
        double LAgGPDDChangeCoef
        {
            get { return _agGPDDChangeCoef; }
            set { _agGPDDChangeCoef = value; }
        }


        //AgGPDDChangeCoef

        double _demand = 0;
        double demand
        {
            get { return _demand; }
            set { _demand = value; }
        }

        // Functions & Methods
        // =================================================================================================
        public override double GetDemand(int currentYear, StreamWriter sw)
        {
            double temp = 0;
            Demand(currentYear);
            temp = demand;
            return temp;
        }
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
        /// <param name="theCurrentYear"></param>
        /// <param name="theStartYear"></param>
        /// <returns></returns>
        public double Calc_AgProduction(int theCurrentYear, int theStartYear)
        {
            double period = (theCurrentYear - theStartYear) + 1;
            double Production = utilities.Forecast_CU(LagNet, LAgricultureGrowthRate, AgGrowthDamper, period, LAgAdjustProductionRate, AgAdjustDamper);
            return Production;
        }
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="theCurrentYear"></param>
        /// <param name="theStartYear"></param>
        /// <param name="NewProduction"></param>
        /// <param name="AdjustEfficiency"></param>
        /// <returns></returns>
        internal int Calc_agDemand(int theCurrentYear, int theStartYear, double NewProduction, double AdjustEfficiency)
        {

            double period = (theCurrentYear - theStartYear) + 1;
            //
            SetDemandFactors();
            double AgDemand = EstimateConsumerDemand(NewProduction, LAgInitialGPCD, AdjustEfficiency, LAgGPDDChangeCoef, FMinAgGPDDPercent, period);
            return (int)AgDemand;
        }

       

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize_Variables()
        {
            LAgInitialGPCD = CRF.AgricultureInitialGPDD;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void SetDemandFactors()
        {
            LAgConservation = CRF.AgConservation;
            LAgGPDDChangeCoef = CRF.PAgGPDDChangeCoef;

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
        // UrbanDemand Pre Process
        /// <summary>
        /// 
        /// </summary>
        /// <param name="yr"></param>
        void preProcessProduction(int yr)
        {
           LagNet = CRF.AgricultureAGNet;
           LAgricultureGrowthRate = CRF.AgricultureGrowthRate;
           LAgAdjustProductionRate = CRF.AgAdjustProductionRate;
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
          
        }



    }
}
