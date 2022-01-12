using System;
using System.Linq;
using System.Windows.Forms;
using CORiverModel;

namespace WaterSimDCDC.Generic
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary> A data Model for the water simulation of the Colroado River.</summary>
    ///
    /// <remarks> Uses the CORiverModel Namespace.</remarks>
    ///
    /// <seealso cref="ResourceModel"/>
    ///-------------------------------------------------------------------------------------------------

    public class WaterSim_CORiverModel : BasinRiverModel
    {
        // The External Model
        COriverModel FCORiverModel;
        COBasinModelManager FCOModels;
     
        readonly COriverAccounting FCOA;
        //
        /// <summary>
        /// 
        /// </summary>
        public int FnumModels = 24;
        // 
  
        // string UnitDataFilename = "West Regions USGS with Colorado Ver 5.csv";

        // NOTE! Though Owner:WaerSimModel is not defined here it is deined in ResourceModel and will be set when model is
        //       added to the ResourceModel list.

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the WaterSimDCDC.Generic.WaterSim_CORiverModel
        ///     class.</summary>
        ///
        ///-------------------------------------------------------------------------------------------------
        ///
        //==================================================================================================================================
        public WaterSim_CORiverModel(string DataDirectoryName, string TempDirectoryName, string UnitDataFilename, bool UTtransfers)
        {
            // 10.27.21 das
            //string COdata = "DataCOriver";
            try
            {
                //FCORiverModel = new COriverModel(DataDirectoryName + "\\" + COdata + "\\", TempDirectoryName);
                FCORiverModel = new COriverModel(DataDirectoryName, TempDirectoryName, UTwaterTransfers);

                FCOA = new COriverAccounting(DataDirectoryName, UnitDataFilename, FCORiverModel);
                FCOModels = new COBasinModelManager();
                ResetModel();
            }
            catch (Exception ex)
            {
                FCORiverModel = null;
                FCOA = null;
                MessageBox.Show(" WaterSim_CORiverModel constructor was not created" + ex);
            }
            // end 09.14.20 das
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectoryName"></param>
        /// <param name="TempDirectoryName"></param>
        /// <param name="UnitDataFilename"></param>
        public WaterSim_CORiverModel(string DataDirectoryName, string TempDirectoryName, string UnitDataFilename)
        {
            // 09.14.20 das
            string COdata = "DataCOriver";
            try
            {
                FCORiverModel = new COriverModel(DataDirectoryName + COdata, TempDirectoryName);
                FCOA = new COriverAccounting(DataDirectoryName, UnitDataFilename, FCORiverModel);
                FCOModels = new COBasinModelManager();
                ResetModel();
              }
            catch (Exception ex)
            {
                FCORiverModel = null;
                FCOA = null;
                MessageBox.Show(" WaterSim_CORiverModel constructor was not created" + ex);
            }
            // end 09.14.20 das
        }
        public COriverModel COriverModel
        {
            get
            {
                return FCORiverModel;
            }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the colorado river model.</summary>
        ///
        /// <value> The colorado river model.</value>
        ///-------------------------------------------------------------------------------------------------

        public COriverModel CORiverModel
        {
            get { return FCORiverModel; }
        }
        public COriverModel CRM
        {
            get { return FCORiverModel; }
        }
        //
        public bool UTwaterTransfers
        {
            get; set;
        }

        //
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the manager for co model.</summary>
        ///
        /// <value> The co model manager.</value>
        ///-------------------------------------------------------------------------------------------------

        public COBasinModelManager COModelManager
        {
            get { return FCOModels; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the co accounting model.</summary>
        ///
        /// <value> The co accounting model.</value>
        ///-------------------------------------------------------------------------------------------------

        public COriverAccounting COAccountingModel
        {
            get { return FCOA; }
        }

        // Bring forward CO river trace start year

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti co river trace start year.</summary>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------

        public void Seti_CoRiverTraceStartYear(int value)
        {
            FCORiverModel.Seti_CoRiverTraceStartYear(value);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti co river trace start year.</summary>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int Geti_CoRiverTraceStartYear()
        {
            return FCORiverModel.geti_CoRiverTraceStartYear();
        }
        // =========================================================
        /// <summary>
        /// 
        /// </summary>
        public double DroughtManagerForCOriverAtLeeFerry
        {
            set => FCORiverModel.DroughtManagerLeeFerry = value;
        }
        // =========================================================

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Resets the model.</summary>
        ///-------------------------------------------------------------------------------------------------
        public override void ResetModel()
        {
            foreach (int reg in Fregion)
            {
                if (Owner != null) Owner.DroughtControl[reg] = 1;
            }
            DroughtStartYearLF = DefaultDroughtStartYearLF;
            DroughtLengthLF = DefaultDroughtLengthLF;
            DroughtDepthLF = DefaultDroughtDepthLF;
            DroughtActiveLF = DefaultDroughtActiveLF;
            DroughtManagerForCOriverAtLeeFerry = 1;
        }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> Runs the Resource Model with the given input data.</summary>
    /// <remarks> Intended for use on models that manage the ResourceModelInput and ResourceModelOutput external to the model but 
    ///           Manage the year internal to the model.</remarks>
    /// <param name="InputData"> Information describing the input.</param>
    /// <param name="isErr">     [out] The error code 0=no error.</param>
    /// <param name="errStr">    [out] The error string.</param>
    ///
    /// <returns> A ResourceModelOutput.</returns>
    ///-------------------------------------------------------------------------------------------------

    public override ResourceModelOutput Run(ResourceModelInput InputData, out int isErr, out string errStr)
        {
            throw new NotImplementedException();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Executes a model run for one year as specified.</summary>
        ///
        /// <remarks> Intended for use on models that keep the ResourceModelInput locally in the model but
        ///           manage the RespurceModelOUtput external to the model .</remarks>
        ///
        /// <param name="year">    The year as 0 being the first year.</param>
        /// <param name="endyear"> The endyear.</param>
        /// <param name="error">   [out] The error code, 0 = no error.</param>
        /// <param name="ErrStr">  [out] The error string.</param>
        ///
        /// <returns> A ResourceModelOutput object with model results.</returns>
        ///-------------------------------------------------------------------------------------------------

        public override ResourceModelOutput RunYear(int year, int endyear, out int error, out string ErrStr)
        {

            throw new NotImplementedException();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Executes a model run for one year as specified.</summary>
        ///
        /// <remarks> Intended for use on models that keep the ResourceModelInput and ResourceModelOutput
        ///     locally in the model </remarks>
        ///
        /// <param name="error">  [out] The error code, 0 = no error.</param>
        /// <param name="ErrStr"> [out] The error string.</param>
        ///
        /// <returns> A ResourceModelOutput object with model results.</returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool RunYear(out int error, out string ErrStr)
        {
            throw new NotImplementedException();
        }

        //
        /// <summary>
        ///  09.15.20 das- added this override method
        /// </summary>
        /// <param name="year"></param>
        /// <param name="error"></param>
        /// <param name="ErrStr"></param>
        /// <returns></returns>
        public override bool RunYear(int year, out int error, out string ErrStr)
        {
            bool result = false;
            error = 0;
            ErrStr = "";
            try
            {
                COdroughtAtLeeFerry(year);
                CORiverModel.RunMeadPowellReservoirs(year);
                result = true;
            }
            catch (Exception ex)
            {
                string message = "Overloaded RunYear override in WaterSimModel for MeadPowell error";
                MessageBox.Show(message + ex);
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Executes a model run for one year as specified.</summary>
        ///
        ///
        /// <param name="Input">   The ResourceModel input data.</param>
        /// <param name="year">    The year as 0 being the first year.</param>
        /// <param name="endyear"> The endyear.</param>
        /// <param name="error">   [out] The error code, 0 = no error.</param>
        /// <param name="ErrStr">  [out] The error string.</param>
        ///
        /// <returns> A ResourceModelOutput object with model results.</returns>
        ///-------------------------------------------------------------------------------------------------

        public override ResourceModelOutput RunYear(ResourceModelInput Input, int year, int endyear, out int error, out string ErrStr)
        {
            throw new NotImplementedException();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Allocates this models output to the various regions managed by WaterSimModel.</summary>
        ///
        /// <remarks> Intended for models that need river model out put passed to it, such as a sperate
        ///     model
        ///            does allocation
        ///     This needs to be implemented , this allocates to the various models Pseudo ocde is
        ///     provided.</remarks>
        ///
        /// <param name="Output"> The output.</param>
        /// <param name="year">   The year.</param>
        ///
        /// <returns> True if it succeeds, false if it fails.</returns>
        ///-------------------------------------------------------------------------------------------------

        public override BasinWater Allocate(ResourceModelOutput Output, int year)
        {
            throw new NotImplementedException();
        }
        // =================================================================================================


        // ===========================
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Allocates this models output to the various regions managed by WaterSimModel.</summary>
        ///
        /// <remarks> This is intended for models where allocation is done outside of model and an array of
        ///     values is needed
        ///      but river model results is maintained in the river model
        ///     This needs to be implemented , this allocates to the various models Pseudo ocde is
        ///     provided.</remarks>
        ///
        /// <param name="runningYear"> A list of watersim crf models.</param>
        ///
        /// <returns> True if it succeeds, false if it fails.</returns>
        ///-------------------------------------------------------------------------------------------------
        public override BasinWater Allocate(int runningYear)
        {
            // get number of regions
            int NumberofRegions = Owner.ModelCount;
            // Arrays for proportions and raw CO limit data from USGS
            //int[] BasinRaw = new int[NumberofRegions];
            //double[] COratio = new double[NumberofRegions];
            int[] COLdata = new int[NumberofRegions];
            double[] COdiff = new double[NumberofRegions];

            // create Basin Water object
            BasinWater Temp = new BasinWater(NumberofRegions);

            // Raw CO limit data from USGS- assume their right to othe water (for now)- BasinRaw
            // Calculate the ratio of the region to the sum of the water from the reservoir - 
            // Calculate if there is enough water from the source to meet the USGS limit
            // Calculate the colorado river water to distribute -           COLdata
            // Calculate the difference between the limit and the source -  COdiff
            CalculateCOavailabledata(out COLdata, out COdiff);     
            //
            for (int i = 0; i < Owner.ModelUnitData.UnitNames.Count; i++)
            {
                string t = Owner.ModelUnitData.UnitNames[i];
            }
            // gather some data ie amount of colorado river water for each of the 24 regions

  
              // Assign to BasinWater AvailableWater property
                 Temp.AvailableWater = COLdata;
            // Return the Basin Water object

            return Temp;
        }
        //
        internal double returnWater(double water)
        {
            double result = 0;
            // million acre feet annually to MGD
            result = water * Constants.MAFtoMGD;
            return result;
        }
        //
        internal void CalculateCOavailabledata(out int[] ColData, out double[] difference)
        {
            // get number of regions
            int NumberofRegions = Owner.ModelCount;
            int[] BasinRaw = new int[NumberofRegions];
            // Raw CO limit data from USGS- assume their right to othe water (for now)
            RetrieveCObaseData(Bcode, BasinRaw); 
            //
            int[] Col = new int[NumberofRegions];
            double[] Ratio = new double[NumberofRegions];
            double[] diff = new double[NumberofRegions];
            double water = 0;
            water = returnWater(CRM.CAPwaterAZ);
            CreateCOavail(a, 0, water, BasinRaw, Ratio, Col, diff);
            //
            water = returnWater(CRM.OnRiverAZ);
            CreateCOavail(a, 1, water, BasinRaw, Ratio, Col, diff);
            //
            water = returnWater(CRM.CAshareCO);
            CreateCOavail(a, 2, water, BasinRaw, Ratio, Col, diff);
            //
            water = returnWater(CRM.CAshareCO);
            CreateCOavail(a, 3, water, BasinRaw, Ratio, Col, diff);
            //
            water = returnWater(CRM.NVshareCO);
            CreateCOavail(a, 4, water, BasinRaw, Ratio, Col, diff);
            //
            water = returnWater(CRM.UBtotal);
            CreateCOavail(a, 5, water, BasinRaw, Ratio, Col, diff);
            //
            ColData = Col;
            difference = diff;
        }
  
        void RetrieveRawFluxData(string flux, int[] data)
        {
            string ErrStr = "";
            int NumberofRegions = Owner.ModelCount;
            data = new int[NumberofRegions];

            foreach (SDResourceConsumerLink field in Owner.ModelUnitData.Fluxes)
            {
                if (field.Resource == flux)
                {
                    bool aTry = Owner.ModelUnitData.GetIntValues(field.Resource, out data, out ErrStr);
                }
            }

        }
        private void RetrieveCObaseData(int[] Bcodes, int[] dataOut)
        {
            int sum = 0;
            for (int i = 0; i < Bcodes.Length; i++)
            {
                sum = Owner.FastUnitModel(i).geti_SurfaceColorado();
                dataOut[i] = sum;
            }
        }
        // =============================================================================================================
        //
        private void CreateCOavail(double[,] a, int c, double sum, int[] raw, double[] ratios, int[] COwater, double[] diff)
        {
            double sumUBraw = 0;
            double minDiff = 0.001;
            for (int i = 0; i < raw.Length; i++)
            {
                // only bother to loop if the regionhas Colorado River Water
                 if (0 < raw[i])
                 {
                    if (0 < COwater[i]) continue;
                    for (int j = 0; j < 6; j++)
                    {
                        double temp = 0;
                        if (j == c)
                        {
                            double Raw = raw[i];
                            if( c != 5)
                            {
                                if (0 < sum)
                                {
                                    ratios[i] = Raw * a[i, c] / sum;
                                }
                            }
                            else
                            {
                                sumUBraw = CalculateUBLimitSum(raw);
                                if (0 < sumUBraw)
                                {
                                    ratios[i] = Raw * a[i, c] / sumUBraw;
                                }
                            }
                            temp = sum * ratios[i]; // est of potentially available CO water
                            COwater[i] = Convert.ToInt32(Math.Min(temp, raw[i]));
                            diff[i] = temp - raw[i];
                            if (Math.Abs(diff[i]) < minDiff)
                            {
                                diff[i] = 0;
                            }
                        }
                    }
                 }
            }
        }
        // =============================================================
        internal int CalculateUBLimitSum(int[] raw)
        {
            int result = 0;
            for(int i = 0; i < raw.Length; i++)
            {             
                int UB = UBcode[i];
                if (0 < UB) result += raw[i];
            }
            return result;
        }
        //
        //===============================================================================================================

        //===========================================================================================
        private double[,] a = new double[24, 6] {
            {1,0,0,0,0,0},   /*  CAP	ONRIVERAZ	IMPERIAL	MWD	SNWA	UBASIN */
            {0,1,0,0,0,0},
            {0,1,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,0,1,0,0},
            {0,0,1,0,0,0},
            {0,0,0,0,0,0},
            {0,0,0,0,0,1},
            {0,0,0,0,0,1},
            {0,0,0,0,0,1},
            {0,0,0,0,1,0},
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,0,0,0,1},
            {0,0,0,0,0,1},
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,0,0,0,1},
            {0,0,0,0,0,1},
            {0,0,0,0,0,0},
            {0,0,0,0,0,1},
            {0,0,0,0,0,1},
            {0,0,0,0,0,0}
            };


        // Allocation Parameter, read only
        /// <summary>
        /// 
        /// </summary>
        public providerArrayProperty StreamAllocation;
        /// <summary>
        /// Not clear, yet, on why we have this property [09.11.20 das]
        /// </summary>
        /// <returns></returns>
        public int[] geti_StreamAllocation()
        {
            return FCOModels.Geti_StreamAllocation();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Seti_StreamWithdraw(int value)
        {
            FCOModels.Seti_StreamWithdraw(value);
        }
        /// <summary>
        /// Currently comented out in WaterSim Manager initialize variables
        /// </summary>
        /// <returns></returns>
        public int Geti_StreamWithdraw()
        {
            return FCOModels.Geti_StremWithdraw();
        }
        //
        //
        //
        //
        //
        const int LBproviders = 15;
        const int UBproviders = 9;
        private readonly string[] UBasinProviders = new string[UBproviders] {
            "Colorado Front Range", "Colorado In Basin", "Colorado Not In Basin", "Utah Salt Lake",
            "Utah In Basin","Utah Not In Basin","Wyoming Southwest","Wyoming In Basin"," Wyoming Not In Basin"};

        private readonly string[] LBasinProviders = new string[LBproviders] {
            "Arizona Central South", " Arizona West","Arizona North", "Arizona Southeast",
            "Arizona Central North",  "California Southwest", "California Southeast", "California North",
            "Nevada South","Nevada In Basin","Nevada Not In Basin","New Mexico Central", "New Mexico In Basin",
            "New Mexico Not In Basin","New Mexico Gila"};

        static readonly int[] LBcode = new int[LBproviders + UBproviders] { 1, 2, 3, 4, 5, 6, 7, 8, 0, 0, 0, 12, 13, 14, 15, 16, 17, 18, 0, 0, 0, 0, 0, 0 };
        private readonly int[] UBcode = new int[LBproviders + UBproviders] { 0, 0, 0, 0, 0, 0, 0, 0, 9, 10, 11, 0, 0, 0, 0, 0, 0, 0, 19, 20, 21, 22, 23, 24 };
        private readonly int[] Bcode = new int[LBproviders + UBproviders] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
        //
        //static readonly int[] LBasin = new int[LBproviders + UBproviders] { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 };
        //private readonly int[] UBasin = new int[LBproviders + UBproviders] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1 };
 

    }
}
