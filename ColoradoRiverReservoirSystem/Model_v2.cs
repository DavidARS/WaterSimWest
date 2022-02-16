using System;
using System.IO;
// EDIT QUAY 9/10/20
// REMOVED REFERENCE TO WATERSIM NAMESPACE
// ADDED WaterSim Class Objects
// using WaterSimDCDC.Generic;
// END EDIT
using CORiverDesignations;

// EDIT QUAY 9/10/20
// Restored CORiverModel namespace
namespace CORiverModel
//namespace WaterSimDCDC.Generic
// END EDit
{
    public class COriverModel
    {
        
        //UnitData2 FUnitData2;
        Powell_mead PM;
        IndianMunicipalAg IMA;
        BasinDCP BDP;
        // internal StreamWriter sw;
        readonly DateTime now = DateTime.Now;
        //
        // WaterSimModel WASM = null;
         // bool startSimulation = false;
        //string UnitDataFIDContempory = "COflowDataExtended.csv";
        //string UnitDataFIDPaleo = "COflowDataExtended.csv";
        //string ICSfileName = "ICS.csv";
        readonly string ICSfileName = "ICS_Regions.csv";
        string FICSdataFileID= "ICS_Regions.csv";
        //
        #region constructors
        public COriverModel(string DataDirectoryName, string TempDirectoryName, bool UTwaterTransfers)
        {
            string COriverFileName = GetDefineCOriverFileName();
            // this file will need a getter/setter in the WS manager 06.09.20 DAS
            string DataFileID = "RightsP1.txt";
            string PathFileID = DataDirectoryName + "\\" + DataFileID;
            try
            {
                //BDP = new BasinDCP(PathFileID, DataDirectoryName, ICSfileName);
                PM = new Powell_mead(DataDirectoryName, DataFileID, COriverFileName, ICSfileName);
                //PM = new Powell_mead(DataDirectoryName, DataFileID, COriverFileName, Gets_ICSdataFileID(), UTwaterTransfers);
                IMA = new IndianMunicipalAg(PathFileID);
                Initialize();
                RunToDefaultYearCOempirical();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectoryName"></param>
        /// <param name="TempDirectoryName"></param>
        public COriverModel(string DataDirectoryName, string TempDirectoryName)
        {
            string COriverFileName = GetDefineCOriverFileName();
            // this file will need a getter/setter in the WS manager 06.09.20 DAS
            string DataFileID = "RightsP1.txt";
            string PathFileID = DataDirectoryName + "\\" + DataFileID;
            try
            {
                //BDP = new BasinDCP(PathFileID, DataDirectoryName, ICSfileName);
                //PM = new Powell_mead(DataDirectoryName, DataFileID, COriverFileName, ICSfileName);
                 PM = new Powell_mead(DataDirectoryName, DataFileID, COriverFileName, Gets_ICSdataFileID());
                IMA = new IndianMunicipalAg(PathFileID);
                Initialize();
                RunToDefaultYearCOempirical();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Constructors
        //
      
        public Powell_mead PMead
        {
            get { return PM; }
        }
        //
        void Initialize()
        {
            // Historical and contemporary ICS storage
            Seti_ContemporaryICSyear(2019);
            Seti_OverRideICSDataFile(false);
        }
        #region properties
        // 10.13.20 das
        public string Gets_ICSdataFileID()
        { return FICSdataFileID; }

        // 10.13.20 das
        public void Sets_ICSdataFileID(string value)
        { FICSdataFileID = value; }
        // end 10.13.20 das
        // 09.03.20 das
        public int COriverTraceStartYear
        {
            get; set;
        }
        //==============================
        //public int NumberRegions()
        //{
            
        //}

        //==============================
        // this will need a parameter in the WS manager. 06.09.20 DAS
        readonly string _defineCOriverFileName = "COflowDataExtended.csv";

        //string _defineCOriverFileName = "COriver_paleo.csv";
        public string GetDefineCOriverFileName()
        { return _defineCOriverFileName; }
        //string _defineCOriverFileName = "COriver_paleo.csv";
        public void SetDefineCOriverFileName(string value)
        { value = _defineCOriverFileName; }
        public void Seti_COempiricalStopYear(int value)
        {
            PM.TheRiver.DefaultYearCOempirical = value;
        }

        private bool _pass = false;
        private bool GetPass()
        { return _pass; }
        private void SetPass(bool value)
        { _pass = value; }
        //
        public double CAPwaterAZ => PM.GetCapwater();
        public double OnRiverAZ => PM.GetOnRiverAZ();
        public double UBtotal => PM.GetUBtotal();
        #endregion properties

        // 09.15.20 das
        // need to run the river model (LB reservoirs up to the start year of the CRF models)
        // I first sent this in WaterSimModel
        void RunToDefaultYearCOempirical()
        {
            Loop(PM.TheRiver.DefaultYearCOempirical);
        }

        // =========================================
        public virtual bool RunCOoneYear(int year)
        {
            if (GetPass()) { RunMeadPowellReservoirs(year); }
            else { Loop(year); }
            return GetPass();
        }

        internal void Loop(int Syear)
        {
            int year = 2000;
            //int i = 0;
            // string directory = "";
            //StreamW(directory);
            //  PM.Initialize();
            do
            {
                RunMeadPowellReservoirs(year);
                year +=1;
               // i++;
            } while (year <= Syear);
            SetPass(true);
           // sw.Flush();

         }
        /// <summary>
        /// Insertion of CO river model
        /// </summary>
        /// <param name="year"></param>
        //public bool RunModel(int year)
        public bool RunMeadPowellReservoirs(int year)
        {
            //int test = COriverTraceStartYear;
            IMA.WaterForTheDelta = false;
            IMA.CAPneeded = 500000;
            // need to add the drought affect on flows
            // need to add the climate change effect on flows
            UpStream(year);
            PM.StreamPowellMead(year);      //StreamPowellMead(year);
            IMA.Allocate(year, PM.GetCapwater());
            //
           // sw.WriteLine(year + "," + (PM.StateMead- Constants.meadDeadPool)  + "," +( PM.StatePowell-Constants.powellDeadPool));
            DownStream();
            return true;
        }

        // EDIT QUAY 9/10/20
        // BRINGING FORWARD CO River TRace STart Year

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti co river trace start year.</summary>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------

        public void Seti_CoRiverTraceStartYear(int value)
        {
            PM.Seti_CoRiverTraceStartYear(value);
        }
        //
        /// <summary>
        /// This is the last year of the current ICS storage estimates
        /// i.e., historical and contemporary data
        /// </summary>
        /// <param name="value"></param>
        public void Seti_ContemporaryICSyear(int value)
        {
            PM.Seti_ContemporaryICSyear(value);
        }

        /// <summary>
        /// 10.13.20 das
        ///  Override the csv file that has the ICS data
        ///  This would nomially be used for scenarios
        /// </summary>
        /// <param name="value"></param>
        public void Seti_OverRideICSDataFile(bool value)
        {
            PM.Setb_OverRideICSDataFile(value);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti co river trace start year.</summary>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int Geti_CoRiverTraceStartYear()
        {
            return PM.Geti_CoRiverTraceStartYear();
        }
        // 09.16.20 das
        public double DroughtManagerLeeFerry
        {
            set => PM.DroughtManagerLeeFerryPM = value;
            //set => PM.TheRiver.DroughtManagerForCORiver = value; 
        }






        // 09.16.20 das end


        // END EDIT 9/10/20
        

        // Enable COriverAccounting class access to these DATA
        //edits 06.12.20 das
        internal double _pmUpperBasin = 0;

        public double GetPMupperBasinTotal()
        { return PM.GetUBtotal(); }
        //
        // 09.21.20 das
        public double CAshareCO => PM.GetCAshareCO();
        public double NVshareCO => PM.GetNVshareCO();
        // 09.21.20 das end
        //
        internal void UpStream(int year)
        {
        }
        internal void DownStream()
        {
            //startSimulation = true;
           // if (streamCO) sw.Close();
        }

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
        private bool _streamWcolorado = false;
        public bool StreamCO
        { set { _streamWcolorado = value; }
            get { return _streamWcolorado; }
        }
        #endregion streamwriter
    }
    // Class for System
    
    //

    //
}


