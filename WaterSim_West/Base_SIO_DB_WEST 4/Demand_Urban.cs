using System;
using System.IO;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using WaterSimDCDC;
using WaterSimDCDC.Generic;
using DemandModel_Base;


namespace WaterSim_Base
{
  
    /// <summary>
    /// 
    /// </summary>
    public class UrbanDemand_GPCD : DemandModel
    {
        // objects
        WaterSimCRFModel CRF;
        //
        //StreamWriter sw;
        //
        readonly double Fdemand;
        // constants
        // This is the coeffecient to convert USGS MGD Consumer/resource numbers to gallons
        const double convertDemand = 1000000;
        //  This is as low as Urban GPCD should be allowed to go
        const double _MinimumUrbanGPCD = 30.0;

        //  This is the initial GPCD value, the 120 is a default, this is calclated based on data from the USGS data file
        //double FBaseUrbanGPCD = 120;

        //double FMinUrbanGPCDPercent = _MinimumUrbanGPCD / 120;

        //double FUrbanGPCDChangeCoef = 1;
        /// <summary> The base population, thisis read from the USGS data file.</summary>

        readonly double FBasePopulation = 0;
        /// <summary>   The pop growth rate, this is read from the Rate file. </summary>
        //
        readonly int FUnitCode = 0;
        readonly string FComment = "";

        public bool isInstantiated=false;
        //

        #region Constructors
        // ==================================================================================


        /// <summary>
        /// 
        /// </summary>
        /// <param name="crf"></param>
        public UrbanDemand_GPCD(WaterSimCRFModel crf)
        {
            CRF = crf;
            //
            Initialize_Variables();
            isInstantiated = true;
            // assigns itself to the owner
            crf.URBAN = this;
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
            preProcessDemand();
            double period = (currentYear - CRF.startYear) + 1;
            //
            double NewDemand = EstimateConsumerDemand(Lpopulation, LbaseUrbanGPCD, LurbanConservation, LurbanGPCDChangeCoef, LminUrbanGPCDPercent, period);
            temp = NewDemand / convertDemand; 

            demand = temp;
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
        double _baseUrbanGPCD = 0;
        double LbaseUrbanGPCD
        {
            get { return _baseUrbanGPCD; }
            set { _baseUrbanGPCD = value; }
        }
        double _urbanConservation = 0.99;
        double LurbanConservation
        {
            get { return _urbanConservation; }
            set { _urbanConservation = value; }
        }
        double _urbanGPCDChangeCoef = 1.0;
        double LurbanGPCDChangeCoef
        {
            get { return _urbanGPCDChangeCoef; }
            set { _urbanGPCDChangeCoef = value; }
        }
        double _minUrbanGPCDPercent = 1.0;
        double LminUrbanGPCDPercent
        {
            get { return _minUrbanGPCDPercent; }
            set { _minUrbanGPCDPercent = value; }
        }


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
        public override void Initialize_Variables()
        {
            LbaseUrbanGPCD = CRF.PFBaseUrbanGPCD;
        }
        public override void SetDemandFactors()
        {

        }
        public override void SetBaseValues()
        {
            throw new NotImplementedException();
        }
        public override void switchUrbanLCLU(int year) { }
        // =============================================================================================================
        //
        // Process
        public override void preProcessDemand(int year)
        {
        }
        // ====================
        // UrbanDemand Pre Process
        /// <summary>
        /// 
        /// </summary>
        /// <param name="yr"></param>
        public override void preProcessDemand()
        {
           Lpopulation = CRF.population;
           LurbanConservation = CRF.UrbanConservation;
           LurbanGPCDChangeCoef = CRF.PUrbanGPCDChangeCoef;
           LminUrbanGPCDPercent = CRF.PMinUrbanGPCDPercent;
        }

    }
    //public class UrbanDemand_GPCDa : DemandModel
    //{
    //    // objects
    //    WaterSimCRFModel CRF;
    //    //
    //    //DataClassTemperature DCT;
    //    //
    //    double Fdemand;
    //    // constants
    //    // This is the coeffecient to convert USGS MGD Consumer/resource numbers to gallons
    //    const double convertDemand = 1000000;
    //    //  This is as low as Urban GPCD should be allowed to go
    //    const double _MinimumUrbanGPCD = 30.0;

    //    //  This is the initial GPCD value, the 120 is a default, this is calclated based on data from the USGS data file
    //    //double FBaseUrbanGPCD = 120;
    //    double FMinUrbanGPCDPercent = _MinimumUrbanGPCD / 120;

    //    double FUrbanGPCDChangeCoef = 1;
    //    /// <summary> The base population, thisis read from the USGS data file.</summary>
    //    double FBasePopulation = 0;
    //    /// <summary>   The pop growth rate, this is read from the Rate file. </summary>
    //    //
    //    int FUnitCode = 0;
    //    string FComment = "";

    //    public bool isInstantiated = false;
    //    //

    //    #region Constructors
        // ==================================================================================


    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="crf"></param>
    //    public UrbanDemand_GPCDa(WaterSimCRFModel crf)
    //    {
    //        CRF = crf;
    //       // DCT = aDCT;
    //        //
    //        Initialize_Variables();
    //        isInstantiated = true;
    //        // assigns itself to the owner
    //        crf.URBAN = this;
    //    }
    //    #endregion Constructors
    //    // ================================================================================================================================================
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <returns></returns>
    //    internal void Demand(int currentYear)
    //    {
    //        double temp = 0;
    //        preProcessDemand();
    //        double period = (currentYear - CRF.startYear) + 1;
    //        //
    //        double NewDemand = EstimateConsumerDemand(Lpopulation, LbaseUrbanGPCD, LurbanConservation, LurbanGPCDChangeCoef, LminUrbanGPCDPercent, period);
    //        temp = NewDemand;

    //        double gpcd = NewDemand * (1 / Lpopulation);

    //        demand = temp;
    //    }
    //    // ===================================================================================================================================================================

    //    // Properties
    //    // =================================================================================================

    //    double _population = 0;
    //    double Lpopulation
    //    {
    //        get { return _population; }
    //        set { _population = value; }
    //    }
    //    double _baseUrbanGPCD = 0;
    //    double LbaseUrbanGPCD
    //    {
    //        get { return _baseUrbanGPCD; }
    //        set { _baseUrbanGPCD = value; }
    //    }
    //    double _urbanConservation = 0.99;
    //    double LurbanConservation
    //    {
    //        get { return _urbanConservation; }
    //        set { _urbanConservation = value; }
    //    }
    //    double _urbanGPCDChangeCoef = 1.0;
    //    double LurbanGPCDChangeCoef
    //    {
    //        get { return _urbanGPCDChangeCoef; }
    //        set { _urbanGPCDChangeCoef = value; }
    //    }
    //    double _minUrbanGPCDPercent = 1.0;
    //    double LminUrbanGPCDPercent
    //    {
    //        get { return _minUrbanGPCDPercent; }
    //        set { _minUrbanGPCDPercent = value; }
    //    }


    //    double _demand = 0;
    //    double demand
    //    {
    //        get { return _demand; }
    //        set { _demand = value; }
    //    }

    //    // Functions & Methods
    //    // =================================================================================================
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="currentYear"></param>
    //    /// <returns></returns>
    //    public override double GetDemand(int currentYear)
    //    {
    //        double temp = 0;
    //        Demand(currentYear);
    //        temp = demand;
    //        return temp;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public override void Initialize_Variables()
    //    {
    //        LbaseUrbanGPCD = CRF.PFBaseUrbanGPCD;
    //    }
    //    public override void SetDemandFactors()
    //    {

    //    }
    //    public override void SetBaseValues()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    // =============================================================================================================
    //    //
    //    // Process
    //    public override void preProcessDemand(int year)
    //    {
    //    }
    //    // ====================
    //    // UrbanDemand Pre Process
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="yr"></param>
    //    public override void preProcessDemand()
    //    {
    //        Lpopulation = CRF.population;
    //        LurbanConservation = CRF.UrbanConservation;
    //        LurbanGPCDChangeCoef = CRF.PUrbanGPCDChangeCoef;
    //        LminUrbanGPCDPercent = CRF.PMinUrbanGPCDPercent;
    //    }

    //}
}
