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
    public class PowerDemand_wp : DemandModel
    {
        // objects
        WaterSimCRFModel CRF;
        //
        StreamWriter sw;
        //
        double Fdemand;

        /// <summary> The watts per person per day.</summary>
        ///  
        double FInitialWattPerPerson = 0;
        double FMinWattPerPersonPercent = 0.50;
        double FWattPerPersonChangeCoef = 1;
        double FWattsPerPersonPerDay = 0;
        //
        /// <summary> The gallons per watt per day.</summary>
        double FGallonsPerMegaWattPerDay = 0;
        // Minimum Gallons Per Mega Watt per day
        double FMinPwGPMWDPercent = 0.1;
        double FPwGPMWChangeCoef = 1;

        // constants
        // This is the coeffecient to convert USGS MegaWatts MW Consumer numbers to watts
        public const double convertPower = 1000000;
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
        public PowerDemand_wp(WaterSimCRFModel crf)
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
            preProcessProduction();
            double period = (currentYear - CRF.startYear) + 1;
            LPowerProduction = PowerProduction(period);
            CRF.seti_PowerEnergy((int)LPowerProduction);
            //
            preProcessDemand();
            temp = PowerDemand(period, LPowerProduction, LPowerConservation);
            result = temp / convertDemand;
            demand = result;
            //CRF.seti_PowerWater((int)result);
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
        double _powerProduction = 0;
        internal double LPowerProduction
        {
            set { _powerProduction = value; }
            get { return _powerProduction; }
        }
        //
        double _powerConservation = 1.0;
        double LPowerConservation
        {
            get { return _powerConservation; }
            set { _powerConservation = value; }
        }
        //
        double _wattsPerPersonPerDay = 0.0;
        double LWattsPerPersonPerDay
        {
            get { return _wattsPerPersonPerDay; }
            set { _wattsPerPersonPerDay = value; }
        }
        //
        double _pwGPMWChangeCoef = 0.0;
        double LPwGPMWChangeCoef
        {
            get { return _pwGPMWChangeCoef; }
            set { _pwGPMWChangeCoef = value; }
        }
        //
        double _gallonsPerMegaWattPerDay = 0.0;
        double LGallonsPerMegaWattPerDay
        {
            get { return _gallonsPerMegaWattPerDay; }
            set { _gallonsPerMegaWattPerDay = value; }
        }

        //
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
        /// <param name="period"></param>
        /// <returns></returns>
        internal double PowerProduction(double period)
        {
            double result = 0;
            double temp = EstimateConsumerDemand(Lpopulation, LWattsPerPersonPerDay, FMinWattPerPersonPercent, FWattPerPersonChangeCoef, FMinWattPerPersonPercent, period);
            // Note that this produces Total watts and must be converted to MegaWatts
            result = temp / convertPower;
            return result;
            
        }
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="period"></param>
        /// <param name="PowerProduced"></param>
        /// <param name="PowerConservationGoal"></param>
        /// <returns></returns>
        internal double PowerDemand(double period, double PowerProduced, double PowerConservationGoal)
        {
            double temp = 0;
            SetDemandFactors();
            temp = EstimateConsumerDemand(PowerProduced, LGallonsPerMegaWattPerDay, PowerConservationGoal, LPwGPMWChangeCoef, FMinPwGPMWDPercent, period);
            return temp;
        }

       

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize_Variables()
        {
           
        }
        /// <summary>
        ///  NOT SURE THIS WORKS YET...
        /// </summary>
        public override void SetDemandFactors()
        {
            LPwGPMWChangeCoef = CRF.PPwGPMWChangeCoef;
            LGallonsPerMegaWattPerDay = CRF.PGallonsPerMegaWattPerDay;
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
        void preProcessProduction()
        {
            Lpopulation = CRF.population;
            // not sure this is gonna work .....!
            LWattsPerPersonPerDay = CRF.PWattsPerPersonPerDay;
            
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
            LPowerConservation = CRF.PowerConservation;
        }



    }
}
