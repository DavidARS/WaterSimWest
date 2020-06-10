using System;
using System.IO;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using PowellMead;
using CORiverDesignations;

namespace WaterSimDCDC.Generic
{
    public class Model
    {
        //UnitData FUnitData;
        //UnitData2 FUnitData2;
        Powell_mead PM;
        IndianMunicipalAg IMA;
        internal StreamWriter sw;
        DateTime now = DateTime.Now;
        // bool startSimulation = false;
        //string UnitDataFIDContempory = "COflowDataExtended.csv";
        //string UnitDataFIDPaleo = "COflowDataExtended.csv";
        //string ICSfileName = "ICS.csv";
        readonly string ICSfileName = "ICS_Regions.csv";

        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectoryName"></param>
        /// <param name="TempDirectoryName"></param>
        public Model(string DataDirectoryName, string TempDirectoryName)
        {
            string COriverFileName = DefineCOriverFileName;
            // this file will need a getter/setter in the WS manager 06.09.20 DAS
            string DataFileID = "RightsP1.txt";
            try
            {
                //PM = new Powell_mead(DataDirectoryName); // FUnitData, FUnitData2);
                //PM = new Powell_mead(DataDirectoryName, COriverFileName) ;
                PM = new Powell_mead(DataDirectoryName, COriverFileName, ICSfileName);
                IMA = new IndianMunicipalAg(DataDirectoryName + "\\" + DataFileID);
                //  PM.IcsArizona = 100000;
 
                //Loop();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // this will need a parameter in the WS manager. 06.09.20 DAS
        string _defineCOriverFileName = "COflowDataExtended.csv";
        //string _defineCOriverFileName = "COriver_paleo.csv";
        public string DefineCOriverFileName
        {
            set { value = _defineCOriverFileName; }
            get { return _defineCOriverFileName; }

        }

        public virtual bool RunCOoneYear(int year)
        {
            if (pass) { RunModel(year); }
            else { Loop(year); }            
            return pass;
        }
        private bool _pass = false;
        private bool pass
        {
            get { return _pass; }
            set { _pass = value; }
        }
        // =========================================
         internal void Loop(int Syear)
        {
            int year = 2000;
            //int i = 0;
             string directory = "";
            StreamW(directory);
            //  PM.Initialize();
            do
            {
                RunModel(year);
                year +=1;
               // i++;
            } while (year <= Syear-1);
            pass = true;
            sw.Flush();

         }
        /// <summary>
        /// Insertion of CO river model
        /// </summary>
        /// <param name="year"></param>
        public void RunModel(int year)
        {
            IMA.WaterForTheDelta = false;
            IMA.CAPneeded = 500000;
            // need to add the drought affect on flows
            // need to add the climate change effect on flows
            UpStream(year);
            PM.StreamPowellMead(year);      //StreamPowellMead(year);
            IMA.Allocate(year, PM.Capwater);
            //
            sw.WriteLine(year + "," + (PM.StateMead- Constants.meadDeadPool)  + "," +( PM.StatePowell-Constants.powellDeadPool));
            DownStream();
        }
        internal void UpStream(int year)
        {
        }
        internal void DownStream()
        {
            //startSimulation = true;
            if (streamCO) sw.Close();
        }
        #region streamwriter
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
        private bool _streamWcolorado = false;
        public bool streamCO
        { set { _streamWcolorado = value; }
            get { return _streamWcolorado; }
        }
        #endregion streamwriter
    }
    // Class for System
    
    //

    //
}


