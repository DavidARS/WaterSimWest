using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// EDIT QUAY 9/10/20
// Restored CORiverModel namespace
namespace CORiverModel
//namespace WaterSimDCDC.Generic
// END EDit
{
    public class ColoradoRiver
    {
        // EDIT QUAY 9/10/20
        // Restored CORiverModel namespace
        // WaterSimDCDC.Generic.COriverModel West;
        //COriverModel West;
        int FDroughtActive = 0;
        readonly UnitDataCO FUnitData;
        public UnitDataCO FUDC;
        //public const int defaultYearCO = 2014;
        // Data file is COflowDataExtended.csv
        // From: CO river flow data at Lee Ferry using the file NaturalFlows1906-2015_withExtensions.xlsx - 
        // Total Natural Flow Tab, USGS stream guage 09380000 (Colorado R At Lees Ferry, AZ)
        // Inflows use 09421500 (Colorado River Below Hoover Dam, AZ-NV) minus Colorado R At Lees Ferry

        public ColoradoRiver(string DataDirectoryName, string CORiverFile)
        {

            FUnitData = new UnitDataCO(DataDirectoryName, CORiverFile);

            FUDC = FUnitData;
            Initialize();
        }
        public void Initialize()
        {
            // EDIT QUAY 10/10/20
            // This routine will now use ehatever value FRiverTraceStartYear has at the time of initialization
            // This can be set within watersim model
            DefaultCO();
            SeekTrace();
            DefaultCOinflow();
            SeekInFlow();
            // END EDIT
        }
        public void InitializeTraceYears()
        {
            // 09.15.20 das added to change trace year from the interface
            SeekTrace();
            SeekInFlow();
            // end 09.15.20 das
        }
        private void resetvariables()
        {
        }
        #region properties 
        public void Seti_RiverTraceStartYr(int value)
        {
            RiverTraceStartYear = value;
        }

        // properties
        // EDIT QUAY 9/10/20
        // SETUP STart year to feed back to Powel and Rivermodel

        /// <summary> variable and Default value for River Trace State Year.</summary>
        int FRiverTraceStartYear = 1950;
        //
        int FContemporaryICSyear = 2019;
        bool FOverRideICSDataFile = false;
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti co river trace start year.</summary>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------

        public void Seti_CoRiverTraceStartYear(int value)
        {
            FRiverTraceStartYear = value;
            // NOT SURE IF THIS IS NEED, DELETE OTHERWISE
            //   09.15.20 das Yup, I think this is needed.
            //Initialize();
            InitializeTraceYears();
            // end 09.15.20 das
        }
        const int maxEmpirical = 2018;
        public void Seti_COdefaultYearCOempirical(int value)
        {
            DefaultYearCOempirical = value;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti co river trace start year.</summary>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int Geti_CoRiverTraceStartYear()
        {
            return FRiverTraceStartYear;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the river trace start year.</summary>
        ///
        /// <value> The river trace start year.</value>
        ///-------------------------------------------------------------------------------------------------

        public int RiverTraceStartYear
        {
            get { return Geti_CoRiverTraceStartYear(); }
            set { Seti_CoRiverTraceStartYear(value); }
        }

        // END EDIT 9/10/20
        // 
        // Auto-implemented property
        public double COdroughtModifier { set; get; }
        //
        public UnitDataCO ModelUnitData
        {
            get { return FUnitData; }
        }
        //
        int _defaultYearCOemperical = 2015;
        public int DefaultYearCOempirical
        {
            set { _defaultYearCOemperical = value; }
            get { return _defaultYearCOemperical; }

        }
        // 09.17.20 das
        public double DroughtManagerForCORiver { get; set; } = 1;
        // 09.17.20 das end

        // 10.13.20 das
        public void Seti_ContemporaryICSYear(int value)
        {
            FContemporaryICSyear = value;
        }
        public int Geti_ContemporaryICSYear()
        {
            return FContemporaryICSyear;
        }
        public void Setb_OverRideICSDataFile(bool value)
        {
            FOverRideICSDataFile = value;
        }
        public bool Getb_OverRideICSDataFile()
        {
            return FOverRideICSDataFile;
        }



        // end 10.13.20 das
        //
        public int OverrideICSfile { get; set; } = 0;
        #endregion properties
        //
        #region trace estimates

        const int _defaultCO = 17;
        public double[] COriverTraceEmpirical = new double[_defaultCO];
        internal void DefaultCO()
        {
            int year = 2000;
            int i = 0;
            do
            {
                COriverTraceEmpirical[i] = FUDC.FastFlow(year) * (1 / Constants.oneMillion);
                // CORiverTrace[i] = COriverTraceEmpirical[i];
                i++;
                year++;
            } while (year >= 2000 && year <= DefaultYearCOempirical);
        }
        // 
        private const int _CO = 30;
        private readonly double[,] COriverTrace = new double[_CO, _CO];
        //
        private static readonly int tracePeriod = 30;
        //private readonly int startTrace = 1950;

        public double[] COriverTraceArray = new double[tracePeriod];
        public void SeekTrace()
        {
            //int year = startTrace;
            int year = RiverTraceStartYear;
            int i = 0;
            do
            {
                COriverTraceArray[i] = FUDC.FastFlow(year) * (1 / Constants.oneMillion);
                i++;
                year++;
            } while (year < RiverTraceStartYear + tracePeriod);

        }
        //==================================================
        // 09.15.20 das
        //  const int _defaultCO = 17;
        //  DefaultYearCOempirical is passed in from the WaterSim Manager
        // 09.16.20 end
        public double[] COriverInFlowEmpirical = new double[_defaultCO];
        internal void DefaultCOinflow()
        {
            int year = 2000;
            int i = 0;
            do
            {
                COriverInFlowEmpirical[i] = FUDC.FastInFlow(year) * (1 / Constants.oneMillion);
                i++;
                year++;
            } while (year >= 2000 && year <= DefaultYearCOempirical);
        }


        public double[] COriverInFlowArray = new double[tracePeriod];
        //private double _cAPwater = 0;
        //private double _onRiverAZ;

        public void SeekInFlow()
        {
            int year = RiverTraceStartYear;
            int i = 0;
            do
            {
                COriverInFlowArray[i] = FUDC.FastInFlow(year) * (1 / Constants.oneMillion);
                i++;
                year++;
            } while (year < RiverTraceStartYear + tracePeriod);

        }



        #endregion trace estimates
    }
}
