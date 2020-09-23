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
        string ICSfileName = "ICS_Regions.csv";


        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectoryName"></param>
        /// <param name="TempDirectoryName"></param>
        public Model(string DataDirectoryName, string TempDirectoryName)
        {
            string COriverFileName = DefineCOriverFileName;
              try
            {
                //PM = new Powell_mead(DataDirectoryName); // FUnitData, FUnitData2);
                //PM = new Powell_mead(DataDirectoryName, COriverFileName) ;
                PM = new Powell_mead(DataDirectoryName, COriverFileName,ICSfileName);
                IMA = new IndianMunicipalAg();
              //  PM.IcsArizona = 100000;
                Loop();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        string _defineCOriverFileName= "COflowDataExtended.csv";
        //string _defineCOriverFileName = "COriver_paleo.csv";
        public string DefineCOriverFileName
        {
            set { value = _defineCOriverFileName; }
            get { return _defineCOriverFileName; }

        }
         internal void Loop()
        {
            int start = 1999;
            int i = 1;
            int year = 1999;
            //foreach (int value in years){
            string directory = "";
            StreamW(directory);
          //  PM.Initialize();
            do
            {
                year = start + i;
                RunModel(year);
                i++;
            } while (i < 90  );
            sw.Flush();
            sw.Close();
        }
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
        #endregion streamwriter
    }
    // Class for System
    
    //

    //
}


