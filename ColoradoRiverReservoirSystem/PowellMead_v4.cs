using System;
using System.IO;
using System.Collections.Generic;
using CORiverDesignations;
using COReservoir_Base;

// EDIT QUAY 9/10/20
// Restored CORiverModel namespace
namespace CORiverModel

//namespace WaterSimDCDC.Generic
// END EDit

{
    public class Powell_mead : COReservoirs
    {
        readonly UnitDataCO FUnitData;
        readonly UnitData2 FUnitData2;
        readonly UnitData_ICS FUnitData3;

        public UnitDataCO FUDC;
        public UnitData2 FUBD;
        public UnitData_ICS FICS;
        public BasinDCP FBDCP;
        //
        ColoradoRiver COriver;
        COriverModel CRM;
        BasinDCP BDP;
        internal StreamWriter sw;

        //
        //string UnitDataFIDContempory = "COflowDataExtended.csv";
        //string UnitDataFIDPaleo = "COflowDataExtended.csv";
        readonly string UnitData2Filename = "UpperBasin_deliveries2020.csv";
        //string Unitdata3Filename = "ICS.csv";
        //
        //internal StreamWriter sw;
        readonly DateTime now = DateTime.Now;
        //
        //const int defaultYearCO = 2014;

        List<int> FICSRegions = new List<int>();

        //bool startSimulation = false;
        //internal StreamWriter sw;
        //DateTime now = DateTime.Now;
        int FMeadElevation=1095;
        // =========================================
        #region constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectoryName"></param>
        /// <param name="CORiverFile"></param>
        /// <param name="ICSdataFile"></param>
        public Powell_mead(string DataDirectoryName, string FileID,string CORiverFile, string ICSdataFile)
        {
            string PathFileID = DataDirectoryName + "\\" + FileID;
            FUnitData = new UnitDataCO(DataDirectoryName, CORiverFile);
            FUnitData2 = new UnitData2(DataDirectoryName, UnitData2Filename);
            FUnitData3 = new UnitData_ICS(DataDirectoryName, ICSdataFile);
            //
            COriver = new ColoradoRiver(DataDirectoryName, CORiverFile);
            //
            BDP = new BasinDCP(PathFileID, DataDirectoryName, FUnitData3);
            //
            FUDC = FUnitData;
            FUBD = FUnitData2;
            FICS = FUnitData3;
            FBDCP = BDP;
            Initialize();
            StreamW("out");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectoryName"></param>
        /// <param name="CORiverFile"></param>
        public Powell_mead(string DataDirectoryName, string CORiverFile)
        {

            FUnitData = new UnitDataCO(DataDirectoryName, CORiverFile);
            FUnitData2 = new UnitData2(DataDirectoryName, UnitData2Filename);

            FUDC = FUnitData;
            FUBD = FUnitData2;
            Initialize();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FUnitData"></param>
        public Powell_mead(UnitDataCO FUnitData)
        {
            FUDC = FUnitData;
            Initialize();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FUnitData"></param>
        /// <param name="FUnitData2"></param>
        public Powell_mead(UnitDataCO FUnitData, UnitData2 FUnitData2)
        {
            FUDC = FUnitData;
            FUBD = FUnitData2;
            Initialize();
        }
        #endregion constructors
        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            StatePowell = Constants.initialPowellStorage;
            StateMead = Constants.initialMeadStorage;
            PanEvapMead = Constants.pan_mead;
            PanEvapPowell = Constants.pan_powell;
            PanEvapPowellMeadReach = Constants.pan_reach;
            DroughtManagerLeeFerryPM = 1;
            IntentioinallyCreatedSurplusRegions();
        }
        public COriverModel Owner
        {
            get { return CRM; }
            set { CRM = value; }
        }

        public UnitDataCO ModelUnitData
        {
            get { return FUnitData; }
        }
        //
        /// 09.02.20 das
        public ColoradoRiver TheRiver
        {
            get { return COriver; }
        }

        #region properties
        // 10.07.20 das
        //==============================================
        // this needs fixing
        double[,] _ics = new double[65,24];
        /// <summary>
        /// Set the Intentionally Created Surplus estimate for each region
        /// </summary>
        /// <param name="year"></param>
        /// <param name="value"></param>
        public void Setd_ICS(int year,double[] value)
        {
            foreach (int i in FICSRegions)
            {
                _ics[year,i] = (double)value[i];
            }
        }
        /// <summary>
        ///  Get the Intentionally Created Surplus for a region
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public double[,] Getd_ICS(int year)
        {
            int ArraySize = FICSRegions.Count;
            double[,] result = new double[65,ArraySize];
            for(int i =0; i < ArraySize; i++)
            {
                result[year,i] =_ics[year,i];
            }
            return result;
        }
        //=============================================
        // end 10.07.20 das

        // ===========================================
        public double DroughtManagerLeeFerryPM { get; set; } = 1;
        // ===========================================



        int _loopCount = 0;
        internal int LoopCount
        {
            set { _loopCount = value; }
            get { return _loopCount; }
        }

        private int _UBchoice = 3;
        /// <summary>
        /// This property sets which estimte of Upper
        /// basin consumptive use estimate is used.
        /// edits 06.10.20 DAS
        /// </summary>
        public int UBchoice
        {
            set { _UBchoice = value; }
            get { return _UBchoice; }
        }
        //
        // EDIT QUAY 9/10/20
        // Added River Start Trace year, to bring this through to WaterSimModel

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti co river trace start year.</summary>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------

        public void Seti_CoRiverTraceStartYear(int value)
        {
            COriver.Seti_CoRiverTraceStartYear(value);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti co river trace start year.</summary>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int Geti_CoRiverTraceStartYear()
        {
            return COriver.Geti_CoRiverTraceStartYear();
        }
        //
        public void Seti_ContemporaryICSyear(int value)
        {
            COriver.Seti_ContemporaryICSYear(value);
        }
        public int Geti_ContemporaryICSyear()
        {
            return COriver.Geti_ContemporaryICSYear();
        }
        //
        public void Setb_OverRideICSDataFile(bool value)
        {
            COriver.Setb_OverRideICSDataFile(value);
        }
        public bool Getb_OverRideICSDataFile()
        {
            return COriver.Getb_OverRideICSDataFile();
        }
        //
        #endregion properties
        //
        public override void UpStream(int year)
        {
            IcsTotal = IntentionallyCreatedSurplus(year);
        }
        public void StreamPowellMead(int year)
        {
            // edits 06.10.20 DAS
            //int use = 3; // Upper Basin Schedules. This may need a parameter in WaterSim Manager
            // upper basin data are in thousand acre-feet
            UBchoice = 3;
            //
            UpStream(year);
            //
            Flows(year);
            UpperBasinDeliveries(year);
            SelectUBuse(year);
            Reservoirs(year);
            Designations(year);
            //
            DownStream(year);
        }
        public override void DownStream(int year)
        {
            // last

            CoInFlowTminus1 = COInFlow;
            // startSimulation = true;
        }
        //
        // =================================================
        // River flow data, Upper Basin designations
        // start here
        #region trace estimates OLD


        #endregion trace estimates
        // ===================================================
        const double intercept = 244976;
        const double slope = 0.04002;
        #region flows and additions
        // ==================================
        public override void Flows(int year)
        {
            // Units in million acre-feet
            //if (year <=  defaultYearCO)
            if (year <= COriver.DefaultYearCOempirical)
            {
                COflow = COriver.COriverTraceEmpirical[year - 2000];
                COInFlow = COriver.COriverInFlowEmpirical[year - 2000];
            }
            else
            {
                ReturnFlowsEstimate();
            }
        }
        // ==============================================
        //
        internal void ReturnFlowsEstimate()
        {
            if (LoopCount < COriver.COriverTraceArray.Length)
            {
            }
            else
            {
                LoopCount = 0;
            }
            COflow = COriver.COriverTraceArray[LoopCount] * DroughtManagerLeeFerryPM;
            COInFlow = COriver.COriverInFlowArray[LoopCount] * DroughtManagerLeeFerryPM;

            LoopCount++;
        }
        //
           // ==============================================

        /// <summary>
        ///  If no inflow data are available, estimate inflow from flow data
        ///  SAS program file on disk (r2 is low) 04.18.19 das
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        double InFlowEstimate(int year)
        {
            double estimate = 0;
            estimate = FUDC.FastInFlow(year) * (1 / Constants.oneMillion);
            if (estimate != 0) { }
            else
            {
                estimate = slope * FUDC.FastFlow(year) + intercept * (1 / Constants.oneMillion);
            }
            return estimate;
        }
        #region Upper Basin
        // projections
        //http://www.ucrcommission.com/RepDoc/DepSchedules/CurFutDemandSchedule.pdf
        // historic
        // https://www.usbr.gov/uc/envdocs/plans.html
        //
        // edits DAS 06.10.20. I created a new Upper basin consumptive use file based on
        // historic and projections of use listed in the top URL. Historic are from reports
        // in the second URL. I used PROC EXPAND in SAS
        // to create the time series...file is  "UpperBasin_deliveries2020.csv";
        // end edits DAS 06.10.20
        /// <summary>
        /// Initialize the canned upper basin deliveries
        /// </summary>
        /// <param name="year"></param>
        internal void UpperBasinDeliveries(int year)
        {
            double temp = 1.0;
            double actual = 0;
            // Units in million acre-feet
            UpperBasin_1 = temp * FUBD.FastUB1(year) * (1 / Constants.oneThousandth);
            UpperBasin_2 = FUBD.FastUB2(year) * (1 / Constants.oneThousandth);
            // edits 06.10.20 DAS
            // Upper basin 3 is actual up until 2018 (except for 2000)
            // end edits 06.10.20 DAS
            UpperBasin_3 = FUBD.FastUB3(year) * (1 / Constants.oneThousandth);

            if (year <= 2012)
            {
                actual = CORiverUtilities.UpperBasinActual(year);
                UpperBasin_1 = actual;
            }
            else
            {
                temp = 0.86;
                UpperBasin_1 = temp * FUBD.FastUB1(year) * (1 / Constants.oneThousandth);
            }
          }
        public override void ModifyFlows(int year)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        ///  Subtract Upper basin deliveries from the flow estimates
        /// </summary>
        /// <param name="year"></param>
        /// <param name="choice"></param>
        public override void SelectUBuse(int year)
        {
            // Base Units in thousand acre-feet; modified in MAF
            // 1 = 2007 Upper CO River Comission Schedule
            //2 = Arizona Upper Basin Depletion schedule-Don Gross - ADWR, 30 November 2012
            double UB = 0;
            double UBout = 0;
            int choice = UBchoice;
            switch (choice)
            {

                case 1:
                    COflow -= UpperBasin_1;
                    UB = UpperBasin_1;
                    break;
                case 2:
                    COflow -= UpperBasin_2;
                    UB = UpperBasin_2;
                    break;
                case 3:
                    COflow -= UpperBasin_3;
                    UB = UpperBasin_3;
                    break;
            }
            RemoveICSfromUB(UB,out UBout);
            SetUBtotal(UBout);
        }
        //
        // Need to remove ICS from Upper Basin deliveries
        // 10.14.20 das
        // placeholder for now
        internal void RemoveICSfromUB(double UB,out double conveyout)
        {
            // UB is in units MAF
            double UBTribal = Constants.UBTribalDiversions * 1 / Constants.oneMillion;
            conveyout = UB -= UBTribal;
        }

        //
        #endregion Upper Basin

        //public override void ModifyFlows(int year)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion flows and additions
        //
        // =========================================
        //
         // =======================================
        // Intentionally created Surplus
        #region ICS
        internal double IntentionallyCreatedSurplus(int year)
        {
            double result = 0;
            double Result = 0;
            //https://www.usbr.gov/lc/region/programs/PilotSysConsProg/pilotsystem.html
            //
            bool OverRide = COriver.Getb_OverRideICSDataFile();
            if(OverRide || year <= COriver.Geti_ContemporaryICSYear())
            {
                //result = FICS.Fast_ICS(1, year) * Constants.acreFeetToMAF;
                //result += FICS.Fast_ICS(2, year) * Constants.acreFeetToMAF;
                //result += FICS.Fast_ICS(5, year) * Constants.acreFeetToMAF;
                //result += FICS.Fast_ICS(8, year) * Constants.acreFeetToMAF;
                //
                // What are the units HERE?
                //
                for (int i = 0; i < 24; i++)
                {
                    if (FICS.Fast_CODE(i))
                    {
                        result += FICS.Fast_ICS(i, year) * Constants.acreFeetToMAF;                
                    }
                }
                if (OverRide)
                {
                    Result = DCPICS(year);
                }
                if(0 < Result) result = Math.Min(result, Result);
            }
            else
            {
                result = DCPICS(year);
            }
            return result;
        }
        //
        internal double DCPICS(int year)
        {
            double result = 0;
            int elm = Geti_MeadElevation();
            double dcp = 0;
            for (int i = 0; i < 24; i++)
            {
                if (FICSRegions.Contains(i))
                {
                    FBDCP.Allottments(year, elm, i, out dcp);
                    ICS(i, dcp);
                    result += dcp;
                }
            }
            return result;
        }
        internal double stateLevel_DCPICS(int year, int state)
        {
            double result = 0;
            bool update = true;
            int elm = Geti_MeadElevation();
            //int elm = Convert.ToInt32(ElevationMead);
            double dcp = 0;
            // 05.06.21 edits das
            if (state != -1)
            {
                if (FICSRegions.Contains(state))
                {

                    if (update == true)
                    {
                        FBDCP.Allottments(year, elm, state, update, out dcp);
                        ICS(state, dcp);
                        result = dcp;
                    }
                    else
                    {
                        FBDCP.Allottments(year, elm, state, out dcp);
                        ICS(state, dcp);
                        result = dcp;
                    }
                }
            }
            else
            {
                // 05.06.21 added because Mexico is NOT in our data stream
                FBDCP.Allottments(year, elm, state, update, out dcp);
                ICS(state, dcp);
                result = dcp;
            }
           
            //else
            //{
            //    throw new Exception("state level DCP asked for, but states not provided - line 510 in PowellMead.cs");
            //}
            return result;
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="state"></param>
        /// <param name="dcp"></param>
        internal void ICS(int state, double dcp)
        {
            if (state == 4) IcsArizona = dcp;
            if (state == 6) IcsCalifornia = dcp;
            if (state == 32)
            {
                IcsNevada = dcp;
                IcsSNWA = 0;
            }
            if(state == -1) { }
        }
        //
        internal void IntentioinallyCreatedSurplusRegions()
        {

            for (int i = 0; i < FICS.FDataList.Count; i++)
            {

                if (FICS.Fast_SC(i))
                {
                   FICSRegions.Add(i);
                }
            }
        }
        #endregion ICS
            // ---------------------------------------
            //
            // All properties
            // =======================================
            #region Properties
            // ---------------------------------------------------------------------
            // ---------------------------------------------------------------------
            // outside constructor 
            double _flow = 0;
        double CORiverFlow
        {
            get { return _flow; }
            set { _flow = value; }
        }

        // Release from Powell and Mead
        // =============================
        double _fluxOutPowell = 0;
        public double Powellflux
        {
            set { _fluxOutPowell = value; }
            get { return _fluxOutPowell; }
        }
        public void PowellFlux(double flow, double evap)
        {
            Powellflux = flow - evap;
        }
        double _fluxOutMead = 0;
        public double FluxOutputMead
        {
            set { _fluxOutMead = value; }
            get { return _fluxOutMead; }
        }
        public void FluxOutMead(double inflows, double evap)
        {
            FluxOutputMead = -(evap + Constants.normalReleaseMead) + inflows;
        }
        // ---------------------------------------------------------------------
        //
        // ---------------------------------------------------------------------
        // Flows and InFlows on the Colorado River
        // =======================================
        double _coflow = 0;
        public double COflow
        {
            set { _coflow = value; }
            get { return _coflow; }
        }
        double _coInflow = 0;
        public double COInFlow
        {
            set { _coInflow = value; }
            get { return _coInflow; }
        }
        double _coInFlowTminusOne = 0;
        public double CoInFlowTminus1
        {
            set { _coInFlowTminusOne = value; }
            get { return _coInFlowTminusOne; }
        }
        // ---------------------------------------------------------------------
        //
        // ---------------------------------------------------------------------
        // Upper Basin DIstributions
        // =======================================
        double _ub1 = 0;
        public double UpperBasin_1
        {
            set { _ub1 = value; }
            get { return _ub1; }
        }
        double _ub2 = 0;
        public double UpperBasin_2
        {
            set { _ub2 = value; }
            get { return _ub2; }
        }
        double _ubdynamic = 0;
        public double UpperBasin_3
        {
            set { _ubdynamic = value; }
            get { return _ubdynamic; }
        }
        // ---------------------------------------------------------------------
        //
        // ---------------------------------------------------------------------
        // Balancing fluxes
        // =======================
        double _balanceAmount = 0;
        public double BalanceAmount
        {
            set { _balanceAmount = value; }
            get { return _balanceAmount; }
        }
        double _powellToMead = 0;
        public double PowelToMeadFlux
        {
            set { _powellToMead = value; }
            get { return _powellToMead; }
        }
        double _preBankFluxPowell = 0;
        public double PrebankFluxPowell
        {
            set { _preBankFluxPowell = value; }
            get { return _preBankFluxPowell; }
        }
        double _bankStoragePowell = 0;
        public double BankStoragePowell
        {
            set { _bankStoragePowell = value; }
            get { return _bankStoragePowell; }
        }
        double _fluxPowell = 0;
        public double FluxPowell
        {
            set { _fluxPowell = value; }
            get { return _fluxPowell; }
        }
        double _flux = 0;
        public double Flux
        {
            set { _flux = value; }
            get { return _flux; }
        }
        // ---------------------------------------------------------------------
        //
        // ---------------------------------------------------------------------
        // Predicted Powell and Mead
        // ====================================
        double _predPowell = 0;
        public double PredictedPowell
        {
            set { _predPowell = value; }
            get { return _predPowell; }
        }

        double _predMead = 0;
        public double PredictedMead
        {
            set { _predMead = value; }
            get { return _predMead; }
        }
        // ===================================
        double _overFlowPowellToMead = 0;
        double OverFlowPowellToMead
        {
            set { _overFlowPowellToMead = value; }
            get { return _overFlowPowellToMead; }
        }
        double _overFlowMead = 0;
        double OverFlowMead
        {
            set { _overFlowMead = value; }
            get { return _overFlowMead; }
        }
        double ElevationMead
        {
            get; set;
        }
        // ---------------------------------------------------------------------
        //
        // ---------------------------------------------------------------------
        // Release From Powell
        // ===================================
        double _powellRelease = 0;
        double PowellRelease
        {
            set { _powellRelease = value; }
            get { return _powellRelease; }
        }
        // States for Powell and Mead
        // ===========================
        double _statePowell = 0;
        public double StatePowell
        {
            set { _statePowell = value; }
            get { return _statePowell; }
        }
        //
        public void ModifyStoragePowell(int year)
        {
            FutureStatePowell(year);
        }
        //
        // --------------------------------------
        //
        double _stateMead = 0;
        public double StateMead
        {
            set { _stateMead = value; }
            get { return _stateMead; }
        }
        public void ModifyStorageMean(int year)
        {

        }
        //--------------------------------------
        // States T+1
        double _updatestatePowell = 0;
        double UpdatestatePowell
        {
            set { _updatestatePowell = value; }
            get { return _updatestatePowell; }
        }
        double _updatestateMead = 0;
        double UpdatestateMead
        {
            set { _updatestateMead = value; }
            get { return _updatestateMead; }
        }
        // ---------------------------------------------------------------------
        //
        // ---------------------------------------------------------------------
        // Shortage 
        // ==============================
        double _azshortage = 0;
        public double AzShortage
        {
            set { _azshortage = value; }
            get { return _azshortage; }
        }
        double _nvshortage = 0;
        public double NvShortage
        {
            set { _nvshortage = value; }
            get { return _nvshortage; }
        }
        double _mxshortage = 0;
        public double MxShortage
        {
            set { _mxshortage = value; }
            get { return _mxshortage; }
        }
        double _cashortage = 0;
        public double CaShortage
        {
            set { _cashortage = value; }
            get { return _cashortage; }
        }
        double _shortage = 0;
        public double Shortage
        {
            set { _shortage = value; }
            get { return _shortage; }
        }
        // User Input Allocations
        double _newLB = 0;
        public double NewLowerBasin
        {
            set { _newLB = value; }
            get { return _newLB; }
        }

        // ---------------------------------------------------------------------
        //
        // ---------------------------------------------------------------------
        // Allocations
        // ===========================
        double _lowerBasinRights = 9.0;
        public double LowerBasinRights
        {
            set { _lowerBasinRights = value; }
            get { return _lowerBasinRights; }
        }
        double _aznormal = 0.311111;
        public double AzRights
        {
            set { _aznormal = value; }
            get { return _aznormal; }
        }
        double _nvnormal = 0.033333;
        public double NvRights
        {
            set { _nvnormal = value; }
            get { return _nvnormal; }
        }
        double _mxnormal = 0.166667;
        public double MxRights
        {
            set { _mxnormal = value; }
            get { return _mxnormal; }
        }
        double _canormal = 0.488889;
        public double CaRights
        {
            set { _canormal = value; }
            get { return _canormal; }
        }
        // ---------------------------------------------------------------------
        //
        // ---------------------------------------------------------------------
        // Pan evaporation rates modify
        // units are M year-1
        // ------------------------
        internal double _panEvapPowell = 0;
        public double PanEvapPowell
        {
            set { _panEvapPowell = value; }
            get { return _panEvapPowell; }
        }
        internal double _panEvapMead = 0;
        public double PanEvapMead
        {
            set { _panEvapMead = value; }
            get { return _panEvapMead; }
        }
        internal double _panEvapPowellMeadReach = 0;
        public double PanEvapPowellMeadReach
        {
            set { _panEvapPowellMeadReach = value; }
            get { return _panEvapPowellMeadReach; }
        }
        // ---------------------------------------------------------------------
        //
        // ---------------------------------------------------------------------
        // ICS amounts - units in Acre-Feet per year
        // ==============================
        double _icsArizona = 0;
        public double IcsArizona
        {
            set { _icsArizona = value; }
            get { return _icsArizona; }
        }
        double _icsCalifornia = 0;
        public double IcsCalifornia
        {
            set { _icsCalifornia = value; }
            get { return _icsCalifornia; }
        }
        double _icsNevada = 0;
        public double IcsNevada
        {
            set { _icsNevada = value; }
            get { return _icsNevada; }
        }
        //
        double _icsTON = 0; // Tohona O'odham Nation
        public double IcsTON
        {
            set { _icsTON = value; }
            get { return _icsTON; }
        }
        double _icssnwa = 0;
        public double IcsSNWA
        {
            set { _icssnwa = value; }
            get { return _icssnwa; }
        }
        double _icsOther = 0;
        public double IcsOther
        {
            set { _icsOther = value; }
            get { return _icsOther; }
        }
        double _icstotal = 0;
        double IcsTotal
        {
            set { _icstotal = value; }
            get { return _icstotal; }
        }
        // ---------------------------------------------------------------------
        //
        // ---------------------------------------------------------------------
        // Miscellaneous
        // ==============================
        double _azshareCO = 0;
        public double AZshareCO
        {
            set { _azshareCO = value; }
            get { return _azshareCO; }
        }
        public double GetAZshareCO()
        { return _azshareCO; }
        public void SetAZshareCO(double value)
        { _azshareCO = value; }


        double _azshortageCO = 0;
        public double AzShortageCOwater
        {
            set { _azshortageCO = value; }
            get { return _azshortageCO; }
        }
        double _capwater = 0;
        double _onriver = 0;
        double _ubtotal = 0;
        double _cashare = 0;
        double _nvshare = 0;

        public double GetCapwater()
        { return _capwater; }
        public void SetCapwater(double value)
        { _capwater = value; }
        //

        public double GetOnRiverAZ()
        { return _onriver; }
        public void SetOnRiverAZ(double value)
        { _onriver = value; }

        // million acre-feet annually
        public double GetUBtotal()
        { return _ubtotal; }
        public void SetUBtotal(double value)
        { _ubtotal = value; }
        //
        public double GetCAshareCO()
        { return _cashare; }
        public void SetCAshareCO(double value)
        { _cashare = value; }
        //
        public double GetNVshareCO()
        { return _nvshare; }
        public void SetNVshareCO(double value)
        { _nvshare = value; }
        //
        //
        // 10.13.20 das
        public void Seti_MeadElevation(int value)
        {
            FMeadElevation = value;
        }
        public int Geti_MeadElevation()
        {
            return FMeadElevation;
        }
        // end 10.13.20 das


        #endregion  Properties
        // ---------------------------------------
        //

        // =====================================================================
        // Reservoir Operations
        #region Reservoirs - Powell and Mead
        public override void Reservoirs(int year)
        {
            FutureStatePowell(year);
            if (year < 2019)
            {
                FutureStateMeadInterimGuidelines(year);
            }
            else
            { FutureStateMead(year); }
        }
        public override void FutureState(int year)
        {
            throw new NotImplementedException();
        }
        // Total Pool simulations - Mead and Powell
        // ----------------------------------------
        #region Powell
        public void FutureStatePowell(int year)
        {
            // From: aModifyStorage_ba in FORTRAN
            // ==================================
            double temp = 0;
            double State = StatePowell;
            double annual = 0;
            double diffBanked = 0;
            // -------------------------
            //
            temp = CORiverUtilities.EvaporationPowell(State, PanEvapPowell);
            double predPowell = ExpectedStoragePowell(year, State, COflow);
            //
            double predMead = ExpectedStorageMead(year, StateMead, COInFlow);
            //
            double diffStorage = State - predPowell;
            diffBanked = diffStorage * Constants.bankStorage;
            //
            BalanceAmount = (predPowell - predMead) * 0.5;
            //
            // threshold storage and associated release;
            // Estimate Ssr releases from Powell here
            PowelToMeadFlux = Threshold(year);
            //
            PrebankFluxPowell = COflow - PowelToMeadFlux - temp;
            BankStoragePowell = CORiverUtilities.BankStorage(diffBanked, Constants.bankStorage_powell);
            FluxPowell = PrebankFluxPowell + BankStoragePowell;
            annual = State + FluxPowell;
            // ----------------------------------------
            if (annual < Constants.maxPowellTotalPool)
            {
                if (Constants.powellDeadPool < annual)
                {
                    State = annual;
                }
                else
                {
                    PowellRelease = Math.Max(0, State + COflow + BankStoragePowell - temp);
                    State = Constants.powellDeadPool;
                }
            }
            else
            {
                State = Constants.maxPowellTotalPool;
                OverFlowPowellToMead = annual - Constants.maxPowellTotalPool;
            }
            StatePowell = State;
            //
        }
        public double Threshold(int year)
        {
            double result = 0;
            double Ssr = 0;
            double pe = CORiverUtilities.PowellElevation(StatePowell);
            double me = CORiverUtilities.MeadElevation(StateMead);
            if (pe < Constants.tierElevationOne)
            {
                Ssr = BalanceContent(year, Constants.powellSevenZero, Constants.powellNineFive);
            }
            else
            {
                Ssr = Constants.powellSevenFourEight; // default Why? Why was this 7.48? What rule determines this?
                if (pe < Constants.tierElevationTwo)
                {
                    if (me < Constants.tierElevationMeadOne) Ssr = Constants.powellTargetRelease;
                    if (year == 2005) Ssr = Constants.powellNineZero; // powellNineFive; ONE FIXED FUDGE .... 11.09.18 remove when ICS is finalized?
                }
                else
                {
                    if (pe < Constants.tierElevationThree)
                    {
                        Ssr = TargetAmount(year, BalanceAmount);
                        // start here i.e., target Amount line 1677 in FORTRAN
                    }
                    else
                    {
                        if (pe < Constants.tierElevationFour)
                        {
                            Ssr = TargetAmount(year, BalanceAmount);
                        }
                        else
                        {
                            Ssr = BalanceContent(year, Constants.powellTargetRelease, Constants.powellNineFive);
                        }
                    }
                }
            }
            PowellRelease = Ssr;
            result = Ssr;
            return result;
        }
        // balance the two reservoirs
        // ==========================================================
        public double BalanceContent(int year, double min, double max)
        {
            double temp = 0;
            double result = 0;
            double addWater = 0;
            double sP = 0;
            double sM = 0;
            double initialP = 0;
            double initialM = 0;
            double diff = 0;
            bool pass = false;
            const double first = 0.0025;
            // -------------------------

            // ===========================================================
            temp = StatePowell; //
            sP = Math.Min(temp, Constants.maxPowellTotalPool);
            if (sP < temp) OverFlowPowellToMead = temp - sP;
            //
            temp = StateMead;//
            sM = Math.Min(temp, Constants.maxMeadTotalPool);//
            //
            double balanceIn = (sP - sM) * 0.5;
            double balanceUse = Math.Max(balanceIn, BalanceAmount);
            float closure = 0;
            double holdrelease = 0;
            int i = 0;
            double fluxP = Powellflux;
            double fluxM = FluxOutputMead;
            initialP = sP;
            initialM = sM;
            //
            // das testing
            //
            do
            {
                closure = i;
                temp = Math.Min(max, min + balanceUse + addWater);
                //  temp = Math.Max(max, min + balanceUse + addWater);
                // temp = (max + (min + balanceUse + addWater)) / 2;
                //
                diff = temp - min;
                sP = initialP + fluxP - temp;
                sM = initialM + fluxM + temp;
                //
                double truncatePowell = Math.Truncate(sP * 1000);
                double truncateMead = Math.Truncate(sM * 1000);
                //
                if (truncatePowell <= truncateMead) pass = true;
                if (truncatePowell < truncateMead)
                {
                    addWater = 0;
                }
                else
                {
                    addWater += first;
                }
                if (balanceUse < 0)
                {
                    // Testing!!!! 10.22.18
                    temp = Math.Max(min, Constants.powellTargetRelease + balanceUse);
                    pass = true;
                }
                else
                {
                    if (500 < closure)
                    {
                        if (holdrelease < temp)
                        {
                            if (temp < holdrelease + 0.1) pass = true;
                        }
                        else
                        {
                            if (holdrelease - 0.1 < temp) pass = true;
                        }
                    }
                    if (4995 < closure)
                    {
                        temp = Math.Max(Constants.powellTargetRelease, Math.Min(max, temp));
                    }
                }
                holdrelease = temp;
                //
                if (pass == true) break;
                i++;
            } while (i < 5000);
            //
            result = temp;
            StatePowell = initialP + Powellflux - temp;
            return result;
        } // end of balance content
        // 
        public double TargetAmount(int year, double balanceAmount)
        {
            double result = 0;
            double Ssr = 0;
            double ue = 0;

            // ---------------
            //
            Ssr = Constants.powellTargetRelease;
            double sP = StatePowell + Powellflux - Ssr;
            double pe = CORiverUtilities.PowellElevation(sP);
            if (year < 2027)
            {
                if (year < 2008)
                {
                    // NOT in the 2007 Interim Guidelines
                    // Added on 10.22.18 by DAS - thursday
                    // Experiment
                    if (2004 <= year)
                    {
                        Ssr = BalanceContent(year, Constants.powellTargetRelease, Constants.powellNineFive);
                        //Ssr = BalanceContent(year, Constants.powellNineZero, Constants.powellNineFive);
                    }
                    else
                    {
                        Ssr = BalanceContent(year, Constants.powellSevenFourEight, Constants.powellNineFive);
                    }
                }
                else
                {
                    ue = CORiverUtilities.TargetElevations(year);
                    if (ue <= pe)
                    {
                        Ssr = EqualizationTier(year, pe, ue, balanceAmount);
                    }
                    else
                    {
                        Ssr = UpperElevationTier(year);
                    }
                }
            }
            else
            {
                Ssr = BalanceContent(year, Constants.powellSevenZero, Constants.powellNineZero);
            }
            result = Ssr;
            return result;
        } // end of target Amount
          //
        internal double EqualizationTier(int year, double pe, double ue, double balanceAmount)
        {
            double result = 0;
            double Ssr = 0;
            double tempSsr = 0;
            double holdRelease = 0;
            double sP = 0;
            double sM = 0;
            //
            const double meadelevationSpecial = 1105;
            //
            double x = 0;
            double y = 0;
            int count = 0;
            int k = 0;
            int j = 0;
            bool pass_k = false;
            bool pass_j = false;
            bool pass_i = false;
            // ----------------
            //
            double baseStatePowell = StatePowell;
            double baseStateMead = StateMead;
            double state = CORiverUtilities.StateFromElevationPowell(ue);
            if (ue <= pe)
            {
                // tempSsr = 8.23;
                do
                {
                    holdRelease = tempSsr;
                    // 10.19.18 NOT SURE ON THIS CHANGE. May give better estimates                   
                    //tempSsr = Math.Max(Math.Max(Constants.normalReleasePowell, tempSsr + y), tempSsr + balanceAmount);
                    tempSsr = Math.Max(Constants.normalReleasePowell, tempSsr + y);
                    sP = baseStatePowell + Powellflux - tempSsr;
                    sM = baseStateMead + FluxOutputMead + tempSsr;
                    //
                    if ((sP <= state) || (sP < sM))
                    {
                        if (1 < count)
                        {
                            tempSsr = holdRelease;
                            pass_i = true;
                            break;
                        }
                        Ssr = Constants.normalReleasePowell;
                        double pE = CORiverUtilities.PowellElevation(sP);
                        double mE = CORiverUtilities.MeadElevation(sM);
                        do
                        {
                            if (mE <= meadelevationSpecial)
                            {
                                do
                                {
                                    Ssr += x;
                                    sP = baseStatePowell + Powellflux - Ssr;
                                    double ppE = CORiverUtilities.PowellElevation(sP);
                                    sM = baseStateMead + FluxOutputMead + Ssr;
                                    double mmE = CORiverUtilities.MeadElevation(sM);
                                    double tP = Math.Truncate(sP * 1000);
                                    double tM = Math.Truncate(sM * 1000);
                                    if (tP <= tM)
                                    {
                                        pass_k = true;
                                        break;
                                    }
                                    if (mmE >= meadelevationSpecial)
                                    {
                                        pass_k = true;
                                        break;
                                    }
                                    if (ppE <= ue - 20)
                                    {
                                        pass_k = true;
                                        break;
                                    }
                                    x += 0.0025;


                                } while (k < 500);
                            }
                            else
                            { pass_j = true; }
                            if (pass_j == true || pass_k == true) break;

                        } while (j < 500);

                    }
                    else
                    {
                        y += 0.001;
                    }
                    Ssr = tempSsr;
                    if (pass_i == true || pass_j == true || pass_k == true) break;
                    count++;
                } while (count < 500);

            }
            else
            {
                // powell below target elevation
                Ssr = BalanceContent(year, Constants.powellSevenZero, Constants.powellNineZero);
            }
            //
            result = Ssr;
            return result;
        }
        internal double UpperElevationTier(int year)
        {
            double result = 0;
            double Ssr = 0;
            // ---------------
            //
            double baseStatePowell = StatePowell;
            double baseStateMead = StateMead;
            //
            Ssr = Constants.normalReleasePowell;
            double uE = CORiverUtilities.TargetElevations(year);
            double tS = CORiverUtilities.StateFromElevationPowell(uE);
            double sP = baseStatePowell + Powellflux - Ssr;
            double eP = CORiverUtilities.PowellElevation(sP);

            double sM = baseStateMead + FluxOutputMead + Ssr;
            double eM = CORiverUtilities.MeadElevation(sM);
            // --------------------------------------------------------
            //
            if (eP < uE)
            {
                if (Constants.tierElevationTwo <= eP)
                {
                    // 6.B.1 page 54
                    if (Constants.tierElevationMeadTwo < eM)
                    {
                        Ssr = Constants.powellTargetRelease;
                    }
                    else
                    {
                        // 6.B.2 page 54
                        Ssr = BalanceContent(year, Constants.powellSevenZero, Constants.powellNineZero);
                    }
                    // 6.B.4
                    if (eM < Constants.tierElevationMeadTwo)
                    {
                        if (Constants.tierElevationTwo <= eP)
                        {
                            Ssr = BalanceContent(year, Constants.powellTargetRelease, Constants.powellNineZero);
                        }
                    }
                }
                else
                {
                    // Mid-Elevation Tier 6.C.1 page 54
                    // 10.22.18 NOT in FORTRAN model
                    if (Constants.tierElevationOne <= eP)
                    {
                        if (Constants.tierElevationMeadOne < eM)
                        {
                            Ssr = 7.48;
                            // correct
                        }
                        else
                        {
                            Ssr = 8.23;
                        }
                    }
                    else
                    {
                        // 6.D.1 page 55
                        // 10.22.18 Also NOT in FORTRAN ?
                        //Ssr = BalanceContent(year, Constants.powellSevenZero, Constants.powellNineFive);
                        Ssr = BalanceContent(year, Constants.powellSevenZero, Constants.powellSevenEight);
                    }
                }
            }
            else
            {
                // 6.B.3 page 54
                Ssr = EqualizationTier(year, eP, uE, BalanceAmount);
            }
            //
            result = Ssr;
            return result;
        }
        #endregion Powell
        #region Mead
        //  years < 2019 
        public void FutureStateMeadInterimGuidelines(int year)
        {
            //double sM = 0;
            //double AZshareCOriverWater = 0;
            double allocationNormal = 0;
            double[] diffStorageMead = new double[2];
            bool normalShortage = false;
            bool abnormalDifference = false;
            // ---------------------------
            bool deltaBurden = false;
            double waterToCOdelta = 0;
            double totalCOallocatedLB = 0;
            double totalCOallocatedUB = 0;
            double requestCOdelta_nv = 0;
            double requestCOdelta_az = 0;
            double requestCOdelta_ca = 0;
            double requestCOdelta_mx = 0;
            // ---------------------------
            double allocation_CA = 0;
            double allocation_AZ = 0;
            double allocation_NV = 0;
            double allocation_MX = 0;

            double az = 0;
            double nv = 0;
            double mx = 0;
            //double ca = 0;
            // ------------------------------
            //
            //
            double totalCOallocated_LB = Constants.allocation_az + Constants.allocation_ca + Constants.allocation_mx + Constants.allocation_nv;
            //
            // Send water to the CO delta?
            // ----------------------------
            if (deltaBurden)
            {
                requestCOdelta_az = waterToCOdelta * (Constants.allocation_az / (totalCOallocatedLB + totalCOallocatedUB));
                requestCOdelta_nv = waterToCOdelta * (Constants.allocation_nv / (totalCOallocatedLB + totalCOallocatedUB));
                requestCOdelta_ca = waterToCOdelta * (Constants.allocation_ca / (totalCOallocatedLB + totalCOallocatedUB));
                requestCOdelta_mx = waterToCOdelta * (Constants.allocation_mx / (totalCOallocatedLB + totalCOallocatedUB));
            }
            // ============================================================================================================
            //
            normalShortage = false;
            abnormalDifference = false;
            //
            allocationNormal = totalCOallocated_LB;
            double evapM = CORiverUtilities.EvaporationMead(StateMead, PanEvapMead);
            PredictedMead = ExpectedStorageMead(year, StateMead, COInFlow);
            // Add ICS water to Lake Mead
            //StateMead += IcsTotal;

            double baseStateMead = StateMead; // "state" variable in FORTRAN
            double l_state = baseStateMead - evapM;
            diffStorageMead[0] = l_state - PredictedMead; // zero is 2, one is three in FORTRAN code
            diffStorageMead[1] = diffStorageMead[0] * 1.5;
            // =====================================================================================
            // 
            double meadAddFlow = OverFlowPowellToMead + COInFlow;

            double fluxPreBank = PowellRelease + meadAddFlow - evapM;
            double bankStorage = CORiverUtilities.BankStorage(diffStorageMead[1], Constants.bankStorage_mead);
            double diffInFlow = 0;
            if (0 < CoInFlowTminus1) diffInFlow = Math.Max(0, COInFlow - CoInFlowTminus1);
            double inFlowBanked = CORiverUtilities.BankStorage(-diffInFlow, Constants.cbankStorageChannel);
            double channelEvap = CORiverUtilities.EvaporationReach(PanEvapPowellMeadReach);
            //
            Flux = fluxPreBank + bankStorage - channelEvap + inFlowBanked;
            //
            //double State_bb = baseStateMead + Flux;
            double State_bb = baseStateMead + Flux; // + IcsTotal; //04.29.21 override of code 04.22.19 - unsure yet how to deal with this...
            // 04.29.21 das
            double fluxMead = 0;
            fluxMead = Flux;
            double available = 0;
            /// for this code this will always be the case for 2019
            if (year < Constants.DCPstartYear)
            {
                State_bb += State_bb + IcsTotal;
                fluxMead = Flux;// - IcsTotal;
                available = baseStateMead - Constants.meadDeadPool + Flux + IcsTotal; // ???????
            }
            else
            {
                // 2020 and on
                available = baseStateMead - Constants.meadDeadPool + Flux;
            }
            // end 04.29.21 das
           // double available = baseStateMead - Constants.meadDeadPool + Flux;

            double preCAavailable = available;
            double eM = CORiverUtilities.MeadElevation(baseStateMead - evapM);
            // 04.29.21 das
            ElevationMead = eM;
            // end 04.29.21 das

            // 10.13.20 das
            Seti_MeadElevation(Convert.ToInt32(eM));
            // end 10.13.20 das
            // ===========================
            // ----------------
            // for testing ONLY
            //double test = Math.Pow((2), (2));
            //eM = 1015;
            //sM = CORiverUtilities.StateFromElevationMead(eM);
            // ----------------
            // ===========================
            double sss = SevenStates(eM);
            //
            StatesShortageRatios(sss);
            az = AzShortage; nv = NvShortage; mx = MxShortage;
            //
            double shortfall = Math.Max(0, -available + allocationNormal - sss);
            // start here
            double azShortageT = sss * az;
            double caDCP = 0;
            double azDCP = 0;
            double nvDCP = 0;
            double mxDCP = 0;
            //
            // New code - 02.11.2021 - Tier zero takes precedence. State Codes: AZ=4, CA=6, CO=8, NV=32, NM=35, UT=49, WY=56
            // 02.15.21 - note: by this point state_bb has already accounted for ICS, so this code was moved out of
            // The if block. Elevation is still valid
            if (eM <= Constants.meadTierZero)
            {
                caDCP = stateLevel_DCPICS(year, Constants.CA) * Constants.acreFeetToMAF;
                azDCP = stateLevel_DCPICS(year, Constants.AZ) * Constants.acreFeetToMAF;
                nvDCP = stateLevel_DCPICS(year, Constants.NV) * Constants.acreFeetToMAF;
                mxDCP = stateLevel_DCPICS(year, Constants.MX) * Constants.acreFeetToMAF;
            }
            // 
            if (Constants.meadDeadPool < State_bb+ (caDCP * Constants.acreFeetToMAF))
            {
                if (Constants.meadDeadPool < State_bb + caDCP - (Constants.allocation_ca - caDCP))
                {
                    // 02.15.21 I think this is now correct - NOPE!
                    // 04.29.21 das start 
                    if (eM >= Constants.mead1045) {
                        StateMead = State_bb - Constants.allocation_ca;
                    }
                    else
                    {
                        // 04.29.21 - this was addded
                        StateMead = State_bb  - Constants.allocation_ca;
                        if (year >= Constants.DCPstartYear)
                        {
                            StateMead = State_bb + caDCP - (Constants.allocation_ca - caDCP );
                            if (Constants.allocation_ca - caDCP < Flux)
                            {
                                allocation_CA = Constants.allocation_ca - caDCP ;
                            }
                            else
                            {
                                allocation_CA = Math.Max(0, Flux);
                            }
                        }
                    }
                    // end 04.29.21 das   
                }
                else
                {
                    StateMead = Constants.meadDeadPool;
                    allocation_CA = Flux;
                    allocation_AZ = 0;
                    allocation_NV = 0;
                    allocation_MX = 0;
                }
            }
            else
            {
                StateMead = Constants.meadDeadPool;
                allocation_CA = 0;
            }
            //
            allocation_NV = 0;
            allocation_AZ = 0;
            allocation_MX = 0;
            //
            double postCAavailable = 0;
            // NOTE: the DCPstartYear reflects a major change in how I amhandeling DCP ICS water
            // based on a 04.29.21 briefing by CAP and AZDWR
            //
            if (year >= Constants.DCPstartYear)
            {
                postCAavailable = Math.Max(0, (StateMead + (azDCP+nvDCP+mxDCP )- Constants.meadDeadPool));
            }
            else
            {
                postCAavailable = Math.Max(0, StateMead - Constants.meadDeadPool);
            }
            //
            if (allocation_CA < preCAavailable)
            { }
            else
            {
                // okay
                allocation_CA = preCAavailable;
                postCAavailable = 0;
            }
            // if shortage sharing is happening, do someting!
            if (0 < sss)
            {
                if (Constants.meadDeadPool < StateMead)
                {
                    double postMXavailable = 0;
                    if (0 < shortfall)
                    {
                        // Abnormal operations on the reservoir
                        if (0 < postCAavailable)
                        {
                            //postMXavailable = postCAavailable;
                            //allocation_NV = Math.Max(0, postMXavailable * NvShortage  );
                            //allocation_AZ = Math.Max(0, postMXavailable * AzShortage  ) ;
                            //allocation_MX = Math.Max(0, (postMXavailable - (allocation_NV + allocation_AZ)));
                            if (postCAavailable < allocation_AZ + allocation_NV + allocation_MX)
                            {
                                postMXavailable = postCAavailable;
                                allocation_NV = Math.Max(0, postMXavailable * NvShortage);
                                allocation_AZ = Math.Max(0, postMXavailable * AzShortage);
                                allocation_MX = Math.Max(0, (postMXavailable - (allocation_NV + allocation_AZ)));
                            }
                            else
                            {
                                // should never be here - 04.29.21 das
                                throw new Exception("Should not be in this shortage loop- abnormal allocations");
                            }

                        }
                    }
                    else
                    {
                        // 04.29.21 das added this if block
                        if (0 < postCAavailable)
                        {
                            normalShortage = true;
                            allocation_MX = Math.Max(0, Constants.allocation_mx - MxShortage * sss);
                            allocation_NV = Math.Max(0, Constants.allocation_nv - NvShortage * sss);
                            allocation_AZ = Math.Min(2.8, Math.Max(0, Constants.allocation_az - AzShortage * sss));
                        }
                    }
                }
                else
                {
                    StateMead = Constants.meadDeadPool;
                    abnormalDifference = true;
                }


            }
            else
            {
                // no shortages- dcp added in postCAavailable
                if (postCAavailable >= Constants.allocation_az + Constants.allocation_nv + Constants.allocation_mx)
                {
                    // th DCP's should be equal to zero at this point
                    // 04.29.21 added the DCP subtractions
                    allocation_NV = Constants.allocation_nv;// - nvDCP ;
                    allocation_AZ = Constants.allocation_az;// - azDCP ;
                    allocation_MX = Constants.allocation_mx;// - mxDCP ;
                }
                else
                {
                    // would we ever be here? 
                    throw new Exception("no shortages, but postCAavailable < allocations");
                }
            }
            //
            if (abnormalDifference)
            { } // Mead at Dead Pool
            else
            {
                StateMead -= (allocation_NV + allocation_AZ + allocation_MX);
            }
            //
            if (0 < sss)
            {
                if (normalShortage)
                { }
                else
                {
                    // throw new System.ArgumentException("Mead Error in allocations- futureStateMead");
                }
            }
            else
            {
                if (Constants.maxMeadTotalPool < StateMead)
                {
                    StateMead = Constants.maxMeadTotalPool;
                }
            }
            //
            if (deltaBurden)
            {
                // ensure that units are the same here
                allocation_NV = Math.Max(0, allocation_NV - requestCOdelta_nv);
                if(allocation_NV < 1) { throw new Exception("CO delta request for NV water > allocation"); }
                allocation_MX = Math.Max(0, allocation_MX - requestCOdelta_mx);
                if (allocation_MX < 1) { throw new Exception("CO delta request for MX water > allocation"); }
                allocation_CA = Math.Max(0, allocation_CA - requestCOdelta_ca);
                if (allocation_CA < 1) { throw new Exception("CO delta request for CA water > allocation"); }
                allocation_AZ = Math.Max(0, allocation_AZ - requestCOdelta_az);
                if (allocation_AZ < 1) { throw new Exception("CO delta request for AZ water > allocation"); }
            }


            // ================================================================
            // Subtract the ICS values from allocations
            //  10.26.2018
            // -----------------------------
            // 04.29.21 das added
            if (year >= Constants.DCPstartYear)
            {
                if(year > Constants.MitigationstartYear)
                {
                    if (eM > Constants.mead1050)
                    {
                        if (eM < Constants.mead1075)
                        {
                           double temp = Constants.azMitigation_Tier1_AgPool + Constants.azMitigation_Tier1_NIA;
                            /// allocations already have dcp removed after the DCPstartYear
                        }
                    }
                }
            }
            else
            { 
                // subtracting dcp contributions
                allocation_NV -= (IcsNevada * Constants.acreFeetToMAF);
                allocation_MX -= 0;
                allocation_CA -= (IcsCalifornia * 1 / Constants.oneMillion);
                allocation_AZ -= (IcsArizona * 1 / Constants.oneMillion);
            }
            // end 04.29.21 das
            // -----------------------------

            // ================================================================
            // Note differences in the reservoir states being used
            // ensure that the Powell State is set
            //double eP = CORiverUtilities.PowellElevation(StatePowell);
            //eM = CORiverUtilities.MeadElevation(StateMead);
            //
            // ================================================================
            // Need to now do something with the allocations
            double total = allocation_NV + allocation_MX + allocation_CA + allocation_AZ;
            // ----------------------------
            AzShortageCOwater = Constants.allocation_az - allocation_AZ;
            //AZshareCO = allocation_AZ;
            SetAZshareCO(allocation_AZ);
            SetCAshareCO(allocation_CA);
            SetNVshareCO(allocation_NV);

            //double deadM = Constants.meadDeadPool;
            //double deadP = Constants.powellDeadPool;
            //double mead = StateMead - deadM;
            //double powell = StatePowell - deadP;
            //double evapP = (CORiverUtilities.EvaporationPowell(StatePowell, PanEvapPowell) * 1000000);
            //sw.WriteLine(year + "," + mead + "," + powell + "," + PowellRelease + "," + total + "," + eM + "," + eP + "," + (1000000 * evapM) + "," + evapP);
        }
        //
        // ==================================================================================================================================

        public void FutureStateMead(int year)
        {
            double allocationNormal = 0;
            double[] diffStorageMead = new double[2];
            //bool normalShortage = false;
            //bool abnormalDifference = false;
            // ---------------------------
            bool deltaBurden = false;
            double waterToCOdelta = 0;
            double totalCOallocatedLB = 0;
            double totalCOallocatedUB = 0;
            double requestCOdelta_nv = 0;
            double requestCOdelta_az = 0;
            double requestCOdelta_ca = 0;
            double requestCOdelta_mx = 0;
            // ---------------------------
            double allocation_CA = 0;
            double allocation_AZ = 0;
            double allocation_NV = 0;
            double allocation_MX = 0;
            // Normal rights
            double az = 0;
            double nv = 0;
            double mx = 0;
            double ca = 0;
            // ------------------------------
            //
            double totalCOallocated_LB = Constants.allocation_az + Constants.allocation_ca + Constants.allocation_mx + Constants.allocation_nv;
            //
            // Add ICS water to Lake Mead
            //StateMead += IcsTotal;

            // Send water to the CO delta?
            // ----------------------------
            if (deltaBurden)
            {
                requestCOdelta_az = waterToCOdelta * (Constants.allocation_az / (totalCOallocatedLB + totalCOallocatedUB));
                requestCOdelta_nv = waterToCOdelta * (Constants.allocation_nv / (totalCOallocatedLB + totalCOallocatedUB));
                requestCOdelta_ca = waterToCOdelta * (Constants.allocation_ca / (totalCOallocatedLB + totalCOallocatedUB));
                requestCOdelta_mx = waterToCOdelta * (Constants.allocation_mx / (totalCOallocatedLB + totalCOallocatedUB));
            }
            // ============================================================================================================
            //
            //normalShortage = false;
            //abnormalDifference = false;
            //
            allocationNormal = totalCOallocated_LB;
            double evapM = CORiverUtilities.EvaporationMead(StateMead, PanEvapMead);
            PredictedMead = ExpectedStorageMead(year, StateMead, COInFlow);
            double baseStateMead = StateMead; // "state" variable in FORTRAN
            double l_state = baseStateMead - evapM;
            diffStorageMead[0] = l_state - PredictedMead; // zero is 2, one is three in FORTRAN code
            diffStorageMead[1] = diffStorageMead[0] * 1.5;
            // =====================================================================================
            // 
            double meadAddFlow = OverFlowPowellToMead + COInFlow;

            double fluxPreBank = PowellRelease + meadAddFlow - evapM;
            double bankStorage = CORiverUtilities.BankStorage(diffStorageMead[1], Constants.bankStorage_mead);
            double diffInFlow = 0;
            if (0 < CoInFlowTminus1) diffInFlow = Math.Max(0, COInFlow - CoInFlowTminus1);
            double inFlowBanked = CORiverUtilities.BankStorage(-diffInFlow, Constants.cbankStorageChannel);
            double channelEvap = CORiverUtilities.EvaporationReach(PanEvapPowellMeadReach);
            //
            Flux = fluxPreBank + bankStorage - channelEvap + inFlowBanked;
            //
            // 04.22.19 added ICS here... not sure yet on this fix
            // 04.29.21 NEW CODE
            // edits das
            // 
            // 05.04.21 das edits
            double State_bb = 0;
            double available = 0;
            double fluxMead = 0;
            if (year < Constants.DCPstartYear)
            {
                // Runs for 2019 only - in this if block
                State_bb = baseStateMead + Flux + IcsTotal;
                available = baseStateMead - Constants.meadDeadPool + Flux + IcsTotal;
                fluxMead = Flux - IcsTotal;
            }
            else
            {
                // This will always be the case with  this code for 2020 and on
                State_bb = baseStateMead + Flux;
                available = baseStateMead - Constants.meadDeadPool + Flux;
                fluxMead = Flux;
            }
            // end edits 05.04.21 das
            //      
            double preCAavailable = available;
            double postCAavailable = 0;
            //
            double eM = CORiverUtilities.MeadElevation(baseStateMead - evapM);
            ElevationMead = eM;
            // 10.13.20 das
            Seti_MeadElevation(Convert.ToInt32(eM));
            // end 10.13.20 das
            //
            // edits 05.07.21 das
            double caDCP = 0;
            double azDCP = 0;
            double nvDCP = 0;
            double mxDCP = 0;
            //         
            // end edits 05.07.21 das
           // ===========================
           // ----------------
           // for testing ONLY
           //double test = Math.Pow((2), (2));
           //eM = 1015;
           //sM = CORiverUtilities.StateFromElevationMead(eM);
           // ----------------
           // ==============================================
           // Use Guidlines shortage SevenStates(eM)
           //-----------------------
           // sss is a ratio - NEED a way to override this variable
            double sss = 0;
            sss = SevenStates(eM);
            //
            // ---------------------------------
            //
            // ===================================================================
            // -----------------------
            az = AzRights; nv = NvRights; mx = MxRights; ca = CaRights;
            // The
            StatesShortageRatios(sss);
            //
            // ------------------------------------------------------------------------------------
            // Begin
            // =========================
            //double MeadavailableFlux = Flux; // preCAavailable;
            //if (0 < sss) { MeadavailableFlux = sss; }
            //
            // ===========================================
            // Presumes that California has senior rights
            // Both caps will be used to define rights in MAF
            // ----------------------------
            // 05.07.21 das
            bool skipCode = false;
            // end edits 05.07.21
            double CA = 0;
            CA = LowerBasinRights * ca;
            //
            double AZ = 0; double NV = 0; double MX = 0;
            // Modifyable rights
            // Maximum rights that will be adjusted, if needed below
            AZ = LowerBasinRights * az;
            NV = LowerBasinRights * nv;
            MX = LowerBasinRights * mx;
            //
            // testing
            //eM = 1024;
            //Seti_MeadElevation(Convert.ToInt32(eM));
            //
            if (eM <= Constants.meadTierZero)
            {
                // calculate DCP regardless of elevation...estimated elevation is seen by the method
                caDCP = stateLevel_DCPICS(year, Constants.CA) * Constants.acreFeetToMAF;
                azDCP = stateLevel_DCPICS(year, Constants.AZ) * Constants.acreFeetToMAF;
                nvDCP = stateLevel_DCPICS(year, Constants.NV) * Constants.acreFeetToMAF;
                mxDCP = stateLevel_DCPICS(year, Constants.MX) * Constants.acreFeetToMAF;
            }

            // 1 January of every year - state for Mead reflects initial pool
            if (Constants.meadDeadPool < State_bb) //                                                 
             {
                // New code - 02.11.2021 - Tier zero takes precedence. State Codes: AZ=4, CA=6, CO=8, NV=32, NM=35, UT=49, WY=56
                //if (eM <= Constants.meadTierZero)
                //{
                //    // calculate DCP regardless of elevation...estimated elevation is seen by the method
                //    caDCP = stateLevel_DCPICS(year, Constants.CA) * Constants.acreFeetToMAF;
                //    azDCP = stateLevel_DCPICS(year, Constants.AZ) * Constants.acreFeetToMAF;
                //    nvDCP = stateLevel_DCPICS(year, Constants.NV) * Constants.acreFeetToMAF;
                //    mxDCP = stateLevel_DCPICS(year, Constants.MX) * Constants.acreFeetToMAF;
                //}
                //05.04.21
                // edits... I do not know why I was subtracting out the caDCP
                //if (Constants.meadDeadPool < State_bb - CA - caDCP)
                // edits 05.05.21 das
                CA -= caDCP;
                // end edits 05.05.21 das
                if (Constants.meadDeadPool < State_bb - CA )
                {
                    // Normal Opperations
                    //
                    // Give CA water regardless
                    // caDCP remains in the lake
                    StateMead = State_bb - CA ;
                 }
                else
                {
                    double difference = 0;
                    difference = State_bb - Constants.meadDeadPool;
                    // 05.04.21 das
                    CA =  difference;                  
                    // end edits 05.04.21 das
                    StateMead = Constants.meadDeadPool;
                    // 05.04.21 das
                    AZ = 0;
                    NV = 0;
                    MX = 0;
                    // end edits 05.04.21 das
                    skipCode = true;
                }
            }
            else
            {
                CA = 0;
                AZ = 0;
                NV = 0;
                MX = 0;
                // 05.07.21 edits das
                StateMead = caDCP + azDCP + nvDCP + mxDCP;
                // end edits 05.07.21 das
                // 05.07.21
                skipCode = true;
                // end edits 05.07.21
            }
            //
            // edits 05.07.21 added this skipCode loop
            // Mead at dead pool or nearly so.
            if (skipCode)
            {
                allocation_CA = CA;
                allocation_AZ = AZ;
                allocation_NV = NV;
                allocation_MX = MX;
            }
            else
            {
                // Normal River Obligations
                // ===============================================================
                postCAavailable = Math.Max(0, StateMead - Constants.meadDeadPool);
                //
                // 05.04.21 das edits
                AZ -= azDCP;
                NV -= nvDCP;
                MX -= mxDCP;
                // end edits 05.04.21 das
                if (0 < sss)
                {
                    if (Constants.meadDeadPool < StateMead) //
                    {
                        double normalShortageConditions = Math.Max(0, -(StateMead - Constants.meadDeadPool) + totalCOallocated_LB);
                        if (0 < normalShortageConditions)
                        {
                            // abnormal shortage
                            // should never be here - 04.29.21 das
                            // 05.07.21 das
                            // I am no longer clear as to why I wrote this if block....

                            //MX = LowerBasinRights * mx - MxShortage * sss;
                            //AZ = LowerBasinRights * az - AzShortage * sss;
                            //NV = LowerBasinRights * nv - MxShortage * sss;
                            //StateMead -= (AZ + NV + MX);

                            throw new Exception("Should not be in this shortage loop- abnormal allocations- line 2048- PowellMead");
                        }
                        else
                        {
                            // 05.07.21 das
                            // ===============================
                            if (year >= Constants.DCPstartYear)
                            {
                                StateMead -= (AZ + NV + MX);
                            }
                            else
                            {
                                // 05.04.21 das edits
                                //
                                MX = LowerBasinRights * mx - MxShortage * sss;
                                AZ = LowerBasinRights * az - AzShortage * sss;
                                NV = LowerBasinRights * nv - MxShortage * sss;
                                StateMead -= (AZ + NV + MX);
                                // end edits 05.05.21 das
                            }
                        }
                    }
                    else
                    {
                        // mead at dead pool
                        // edits 05.04.21 das added this line
                        StateMead = Constants.meadDeadPool;
                        // edits 05.07.21 das
                        // add the DCP back in
                        StateMead += caDCP + azDCP + nvDCP + mxDCP;
                        // end edits 05.07.21 das
                    }
                }
                else
                {
                    // no shortage.... but I have added the DCP code above.. so Tier one accounted for
                    // This code is new to the C# program (not in the FORTRAN code)
                    if (AZ + NV + MX < (StateMead - Constants.meadDeadPool))
                    {
                        StateMead -= (AZ + NV + MX);
                    }
                    else
                    {
                        // divy up that which is available
                        double difference = 0;
                        // 05.07.21 das edits
                        difference = StateMead - Constants.meadDeadPool;
                        AZ = Math.Min(difference * az, AZ);
                        NV = Math.Min(difference * nv, NV);
                        MX = Math.Min(difference * mx, MX);
                        //
                        double Atotal = AZ + NV + MX;
                        if (Atotal < difference)
                        {
                            StateMead = Constants.meadDeadPool + (difference - Atotal);
                        }
                        else
                        {
                            StateMead = Constants.meadDeadPool;
                        }
                        //difference = StateMead - Constants.meadDeadPool;
                        //AZ = difference * az;
                        //NV = difference * nv;
                        //MX = difference * mx;
                        //StateMead = Constants.meadDeadPool;

                        // end edits 05.07.21 das
                    }
                }
                //
                // 05.04.21 this code never check to see if CA rights were available......
                allocation_CA = CA;
                allocation_AZ = AZ;
                allocation_NV = NV;
                allocation_MX = MX;
                //
                if (deltaBurden)
                {
                    // ensure that units are the same here
                    allocation_NV = Math.Max(0, allocation_NV - requestCOdelta_nv);
                    if (allocation_NV < 1) { throw new Exception("CO delta request for NV water > allocation"); }
                    allocation_MX = Math.Max(0, allocation_MX - requestCOdelta_mx);
                    if (allocation_MX < 1) { throw new Exception("CO delta request for MX water > allocation"); }
                    allocation_CA = Math.Max(0, allocation_CA - requestCOdelta_ca);
                    if (allocation_CA < 1) { throw new Exception("CO delta request for CA water > allocation"); }
                    allocation_AZ = Math.Max(0, allocation_AZ - requestCOdelta_az);
                    if (allocation_AZ < 1) { throw new Exception("CO delta request for AZ water > allocation"); }

                    // ensure that units are the same here
                    //allocation_NV = Math.Max(0, allocation_NV - requestCOdelta_nv);
                    //allocation_MX = Math.Max(0, allocation_MX - requestCOdelta_mx);
                    //allocation_CA = Math.Max(0, allocation_CA - requestCOdelta_ca);
                    //allocation_AZ = Math.Max(0, allocation_AZ - requestCOdelta_az);
                }
                // ================================================================
                // Subtract the ICS values from allocations
                //  10.26.2018
                // -----------------------------
                // 05.07.21 NOT sure about this code
                if (year < Constants.DCPstartYear)
                {
                    allocation_NV -= IcsNevada * Constants.acreFeetToMAF;
                    allocation_MX -= 0;
                    allocation_CA -= IcsCalifornia * Constants.acreFeetToMAF;
                    allocation_AZ -= IcsArizona * Constants.acreFeetToMAF;
                }
                else
                {
                    // 05.05.21 edits by DAS added the if block
                }
                // -----------------------------
            } // skipCode block end

            // Note differences in the reservoir states being used
            // ensure that the Powell State is set
            double eP = CORiverUtilities.PowellElevation(StatePowell);
            eM = CORiverUtilities.MeadElevation(StateMead);
            //
            // ================================================================
            // Need to now do something with the allocations
            double total = allocation_NV + allocation_MX + allocation_CA + allocation_AZ;
            // ----------------------------

            //
            AzShortageCOwater = Constants.allocation_az - allocation_AZ;
            // edits 05.05.21 das
            //AZshareCO = allocation_AZ;
            SetAZshareCO(allocation_AZ);
            // end edits 05.05.21 das
            SetCAshareCO(allocation_CA);
            SetNVshareCO(allocation_NV);
            //

            //double evapP = (CORiverUtilities.EvaporationPowell(StatePowell, PanEvapPowell) * 1000000);
            // sw.WriteLine(year + "," + StateMead + "," + StatePowell + "," + PowellRelease + "," + total + "," + eM + "," + eP + "," + (1000000 * evapM) + "," + evapP);
        }
        //
        #region streamwriter
        //public void StreamW(string TempDirectoryName)
        //{
        //    //string filename = string.Concat(TempDirectoryName + "Output" + now.Month.ToString()
        //    //    + now.Day.ToString() + now.Minute.ToString() + now.Second.ToString()
        //    //    + "_" + ".csv");
        //    string filename = string.Concat("Outputs/Output" + now.Month.ToString()
        //       + now.Day.ToString() + now.Minute.ToString() + now.Second.ToString()
        //       + "_" + ".csv");
        //    sw = File.AppendText(filename);
        //}
        #endregion streamwriter
        // ==================================================================================================================================
        //
        #region shortage
        internal double SevenStates(double elevMead)
        {
            double result = 0;
            double sevenStatesShortage = 0;
            // -----------------------------
            //
            if (elevMead < Constants.meadStepThree)
            {
                sevenStatesShortage = 0.625;
            }
            else
            {
                if (elevMead <= Constants.meadStepTwo)
                {
                    sevenStatesShortage = 0.487;

                }
                else
                {
                    if (elevMead <= Constants.meadStepOne)
                    {
                        sevenStatesShortage = 0.383;
                    }
                    else
                    { sevenStatesShortage = 0; }
                }
            }
            result = sevenStatesShortage;
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shortage"></param>
        /// <param name="az"></param>
        /// <param name="nv"></param>
        /// <param name="mx"></param>
        /// <returns></returns>
        internal void StatesShortageRatios(double shortage)
        {
            double shortage2 = 0.487;
            double shortage3 = 0.625;
            //
            if (0 < shortage)
            {
                if (shortage < shortage2)
                {
                    AzShortage = 0.835510;
                    NvShortage = 0.033943;
                }
                if (shortage < shortage3)
                {
                    AzShortage = 0.821357;
                    NvShortage = 0.034908;
                }
                else
                {
                    AzShortage = 0.768;
                    NvShortage = 0.032;
                }
                MxShortage = 1.0 - AzShortage - NvShortage;
            }
        }
        #endregion shortage
        #endregion Mead
        //
        #endregion Reservoirs = Powell and Mead
        #region mitigation
        internal double Mitigation(int year)
        {
            double addToCAP = 0;
            if (ElevationMead < Constants.mead1075)
            {
                if (ElevationMead > Constants.mead1050)
                {
                    if (year > Constants.MitigationstartYear)
                    {
                        // these may need changing to properties if they are subjst to be changed as elevations drop
                        addToCAP = Constants.azMitigation_Tier1_AgPool + Constants.azMitigation_Tier1_NIA;
                    }
                }
                else
                {
                    addToCAP = Constants.azMitigation_Tier1_AgPool + Constants.azMitigation_Tier1_NIA;
                    //throw new Exception("Mitigation of CO River water not possible under current formulation");
                }
            }
            return addToCAP;
        }
        #endregion mitigation
        //// =====================================================================
        //// designations from operations
        #region CAP designations
        public double onRiver = 1.2;
        internal void Designations(int year)
        {
            double tempOnRiverAZ = 0;
            tempOnRiverAZ=onRiver;
            //double onRiver_CA = 0;
            // double CAPwater = 0;
            double preCAP = 0;
            // --------------------
            //Added on 06.09.10 based on a talk last night by Dee Fuerst at the monthly
            // AHS meeting in PHX - CAP has to take 90 % of the AZ shortage..On - river take 10 %

            SetOnRiverAZ( Math.Min(AZshareCO, tempOnRiverAZ - (0.1 * AzShortageCOwater)));
            // edit 04.29.21 das
             // edit 05.06.21
            double azMitigation_CAP = Mitigation(year);
             // end edits 05.06.21 das
            // end edits 04.29.21 das
            // edits 04.29.21 das - added CAP mitigation to the equation
            preCAP = Math.Max(0, (AZshareCO + azMitigation_CAP) - GetOnRiverAZ()) ;
            // end edits 04.29.21 das

            // System losses added on 10.15.20 das
            // Units MAF (after modifying)
            // 04.29.21 
            // Mitigation water added here (now)
            //
           if (preCAP <= Constants.CAPcapacity)
            {
                SetCapwater(preCAP - (Constants.CAPsystemLosses * 1/ Constants.oneMillion));
            }
            else
            {
                SetCapwater(Constants.CAPcapacity- (Constants.CAPsystemLosses * 1/ Constants.oneMillion));
            }
            // 09.19.20 das
           // COriver.CAPwater = GetCapwater();
           // COriver.OnRiverAZ = OnRiverAZ;
            // --------------------
        }
        #endregion designations
        #region Expected Storage
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="state"></param>
        /// <param name="flow"></param>
        /// <returns></returns>
        public double ExpectedStoragePowell(int year, double state, double flow)
        {
            double result = 0;
            double temp = 0;
            double flux = 0;
            temp = CORiverUtilities.EvaporationPowell(state, PanEvapPowell);
            flux = Math.Max(0, state + flow - (Constants.normalReleasePowell + temp));
            if (Constants.powellDeadPool < flux)
            {
                result = flux;
            }
            else
            {
                result = Constants.powellDeadPool;
            }
            PowellFlux(flow, temp);
            return result;
        }
        public double ExpectedStorageMead(int year, double state, double inflows)
        {
            double result = 0;
            double temp = 0;
            double flux = 0;
            temp = CORiverUtilities.EvaporationMead(state, PanEvapMead);
            flux = Math.Max(0, (state + Constants.normalReleasePowell + inflows - (Constants.normalReleaseMead + temp)));
            if (Constants.meadDeadPool < flux)
            {
                result = flux;
            }
            else
            {
                result = Constants.meadDeadPool;
            }
            FluxOutMead(inflows, temp);
            return result;
        }



        #endregion Expected Storage
        //#region streamwriter
        public void StreamW(string TempDirectoryName)
        {
            //string filename = string.Concat(TempDirectoryName + "Output" + now.Month.ToString()
            //    + now.Day.ToString() + now.Minute.ToString() + now.Second.ToString()
            //    + "_" + ".csv");
            string filename = string.Concat("Outputs/Output" + now.Month.ToString()
               + now.Day.ToString() + now.Minute.ToString() + now.Second.ToString()
               + "_" + ".csv");
            sw = File.AppendText(filename);
        }
        //#endregion streamwriter



    }
}
