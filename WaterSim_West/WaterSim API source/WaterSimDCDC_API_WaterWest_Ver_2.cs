using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Drawing;
using ConsumerResourceModelFramework;
using System.Collections.Generic;
// WaterSimDCDC_API_WaterWest
// Version 1.01
// 
// 9-12-2018 Additioal CRF_Resource and Water Unit Network Classes for the WaterSim West model
// 4-28-2020 Moved to API source files and linked to project
// 
// 11.16.21 DAS I added Desal to this file
//
namespace WaterSimDCDC.Generic
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A crf resource colorado surface. </summary>
    ///
    /// <remarks>   9/11/2018. 
    ///             This is a new Resource dor just the West model that encapsulates Colorado River Supplies
    ///             Utilizes specialized fields in the modified USGS data</remarks>
    ///-------------------------------------------------------------------------------------------------

    class CRF_Resource_ColoradoSurface : CRF_Resource_SurfaceFresh
    {
        public CRF_Resource_ColoradoSurface()
            : base()
        {
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_ColoradoSurface(string aName)
            : base(aName)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_ColoradoSurface(string aName, string aLabel, Color aColor)
            : base(aName, aLabel, aColor)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">            The name. </param>
        /// <param name="aLabel">           The label. </param>
        /// <param name="aColor">           The color. </param>
        /// <param name="AvailableSupply">  The available supply. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_ColoradoSurface(string aName, string aLabel, Color aColor, double AvailableSupply)
            : base(aName, aLabel, aColor, AvailableSupply)
        {
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Change flux. </summary>
        ///
        /// <remarks>
        /// This is enforce the DataItems rule about changing fluxes.  A class could deny altering a flux
        /// TO a certiain class of dataitem.
        /// </remarks>
        ///
        /// <param name="FluxToItem">   The flux item. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool AllowFluxChangeTo(CRF_DataItem FluxToItem)
        {
            return true;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Determine if we allow flux change from. </summary>
        /// <remarks>   Does not allow unused resources to be sent back</remarks>
        /// <param name="FluxFromItem"> The flux from item. </param>
        ///
        /// <returns>   true if we allow flux change from, false if not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool AllowFluxChangeFrom(CRF_DataItem FluxFromItem)
        {
            return true;//false;
        }
    }
    // =================================================================================================
    class CRF_Resource_Desalination : CRF_Resource_Augmented
    {
        //
        int FPipeline=0;

 
        //

        public CRF_Resource_Desalination()
            : base()
        {
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_Desalination(string aName)
            : base(aName)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_Desalination(string aName, string aLabel, Color aColor)
            : base(aName, aLabel, aColor)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">            The name. </param>
        /// <param name="aLabel">           The label. </param>
        /// <param name="aColor">           The color. </param>
        /// <param name="AvailableSupply">  The available supply. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_Desalination(string aName, string aLabel, Color aColor, double AvailableSupply)
            : base(aName, aLabel, aColor, AvailableSupply)
        {
        }

        public CRF_Resource_Desalination(string aName, string aLabel, Color aColor, double AvailableSupply, int PipeLineCode)
                   : base(aName, aLabel, aColor, AvailableSupply)
        {
            FPipeline = PipeLineCode;
        }
        //
        public int PipelineMethod { get => FPipeline; set => FPipeline = value; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Change flux. </summary>
        ///
        /// <remarks>
        /// This is enforce the DataItems rule about changing fluxes.  A class could deny altering a flux
        /// TO a certiain class of dataitem.
        /// </remarks>
        ///
        /// <param name="FluxToItem">   The flux item. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool AllowFluxChangeTo(CRF_DataItem FluxToItem)
        {
            bool result = true;
            bool from = false;
            // edits 01.07.22 das
            //  need a switch from the controller here
            //  bool result = false;
            
            //   I am a utility, and I am seeking new desal sources from the pipeline 

            // need an control to set this to SeekNew or NOT- has to come from the interface somehow
            FMStyle = CRF_Utility.ManagementStyle.msSeekNew;
            //
            switch (FMStyle)
            {
                // Sure if I have extra,
                case CRF_Utility.ManagementStyle.msCooperate:             
                case CRF_Utility.ManagementStyle.msProtect:
                case CRF_Utility.ManagementStyle.msExpand:
                    result = true;
                    break;
                case CRF_Utility.ManagementStyle.msSeekNew:             

                    switch (PipelineMethod)
                    {
                        // No desalination option
                        case 0:
                            break;
                        // direct use of desal water from the ocean
                        case 1:
                            result = false;
                                break;
                        // exhange of desal water 
                        case 2:
                            break;
                        // desal water is piped to the region
                        case 3:
                               // too expensive for agriculture ??? need to have a policy control on this
                            if (FluxToItem is CRF_Consumer_Agriculture)
                            {
                                result = false;
                               
                            }
                            else
                            if(FluxToItem is CRF_Consumer_Urban)
                            {
                                if (Net < 0)
                                {
                                    result = true;
                                }
                            }
                            break;
                        // No desal
                        default:
                            break;

                    }
                    // the following  line is likely not needed. Check and remove if so
                    //if (FluxToItem is CRF_Resource_Desalination)
                    //{
                        
                    //}
                    break;

            }
            // end edits 01.07.22 das

            //if (FluxToItem is CRF_Consumer_Agriculture)
            //{
            //    return true;
            //}
            return result;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Determine if we allow flux change from. </summary>
        /// <remarks>   Does not allow unused resources to be sent back</remarks>
        /// <param name="FluxFromItem"> The flux from item. </param>
        ///
        /// <returns>   true if we allow flux change from, false if not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool AllowFluxChangeFrom(CRF_DataItem FluxFromItem)
        {
          

            return true;//false;
        }
        //


        //
        protected override void ResetLimit(double NewLimit)
        {
            // check of NewLimit is larger or smaller
            //
            // edits 01.07.21 das - for Desal. IF not using piped desal no new limit is created
            if (NewLimit == 0)
            {

            }
            else
            {
                if (NewLimit > FValue)
                {
                    // OK, need to adjust each flux so original new allocated values stays the same as old
                    // Set Allocation will adjust the Allocated value of flux based on method of allocation being used 
                    // get the oldvalues
                    List<double> OldValues = new List<double>();
                    foreach (CRF_Flux Flux in ToFluxs) //FFluxList)
                    {
                        OldValues.Add(Flux.Allocated());
                    }
                    // set the new limit
                    FValue = NewLimit;

                    // loop through the fluxes and set values
                    int index = 0;
                    foreach (CRF_Flux Flux in ToFluxs) //FFluxList)
                    {
                        // Check if a flux transfer is allowed, base CRF Resource should say no unless their is a need
                        // if not, then reset value to old value
                        if (!CRF_Utility.AllowFluxChange(this, Flux.Target))
                        {
                            Flux.SetAllocation(OldValues[index]);
                            index++;
                        }
                    }

                }
                else
                {
                    if (NewLimit == FValue) { }
                    else
                    {
                        // set the new limit
                        FValue = NewLimit;
                        List<double> OldValues = new List<double>();
                        List<double> NewValues = new List<double>();
                        foreach (CRF_Flux Flux in ToFluxs) //FFluxList)
                        {
                            if (Flux.Source.Label == "Desal Water")
                            {
                                OldValues.Add(Flux.Allocated());
                                //if(OldValues)
                                Flux.SetAllocation(FValue);
                                NewValues.Add(Flux.Allocated());
                            };
                            // NewValues.Add(Flux.Allocated());
                        }

                        // loop through the fluxes and set values
                        int index = 0;
                        foreach (CRF_Flux Flux in ToFluxs) //FFluxList)
                        {
                            // Check if a flux transfer is allowed, base CRF Resource should say no unless their is a need
                            // if not, then reset value to old value
                            if (!CRF_Utility.AllowFluxChange(this, Flux.Target))
                            {
                                //Flux.SetAllocation(OldValues[index]);
                                Flux.SetAllocation(NewValues[index]);
                                index++;
                            }
                        }
                    }
                    //bool stop = true;
                }
            }
            // ok just do it if less everyone lives with consequences, if ratio not reset, they get alot more
            FValue = NewLimit;


            //
        }

        public void DesalSource(bool pipeline)
        {
            double newLimit = 0;
            double AFday = 66; // 328 days, 21700 AF produced (Reclamation Yuma desalting plant pilot run final report-2012
            // Carlsbad (CA) desal is 50 MGD, or 153.4246575 AF day-1;
            const double AFtoMillionGallons = 0.32585142784201;
            double MGD = 0;
            // Puerto Penasco coast
            if (pipeline)
            {
                MGD = AFday * AFtoMillionGallons;
                newLimit = MGD;
            }
            else
            {

                newLimit =50;
            }

            ResetLimit(newLimit);
        }
    }
    //
    //
    class CRF_Resource_AirWater : CRF_Resource_Augmented
    {
        public CRF_Resource_AirWater()
            : base()
        {
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_AirWater(string aName)
            : base(aName)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_AirWater(string aName, string aLabel, Color aColor)
            : base(aName, aLabel, aColor)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">            The name. </param>
        /// <param name="aLabel">           The label. </param>
        /// <param name="aColor">           The color. </param>
        /// <param name="AvailableSupply">  The available supply. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_AirWater(string aName, string aLabel, Color aColor, double AvailableSupply)
            : base(aName, aLabel, aColor, AvailableSupply)
        {
        }





    }

    // =================================================================================================
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A west crf unit network. </summary>
    ///
    /// <remarks>   9/11/2018. 
    ///             This is a revised CRF_UNIT_Network for the West model
    ///             that includes Colroado as a Resource
    ///             </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class West_CRF_Unit_Network : CRF_Unit_Network
    {
        // the Colorado Resource
        CRF_Resource_ColoradoSurface FColorado;
        // The Colorado Color for Sankey
        Color ColoradoResourceColor = Color.MediumSpringGreen;

        CRF_Resource_Desalination FDesal;
        // edits das
        Color DesalinationResourceColor = Color.Coral;
        // end edits das

        public West_CRF_Unit_Network(string USGSDataFilename, string InitialStateName) : base(USGSDataFilename, InitialStateName)
        {
            //FData = new UnitData(USGSDataFilename);
            //if (!FData.DataLoaded)
            //{
            //    //YIKES THIS is  AN ERROR
            //    throw (new Exception("USGS data file did not load"));
            //}
            int test = 1;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <exception cref="Exception">    Thrown when exception. Unitdata not loaded</exception>
        ///
        /// <param name="TheUnitData">     The Data (with datatable) being used to define each unit </param>
        /// <param name="InitialUnitName">  Name of the initial unit. </param>
        ///-------------------------------------------------------------------------------------------------

        public West_CRF_Unit_Network(UnitData TheUnitData, string InitialUnitName): base(TheUnitData,InitialUnitName) 
        {

            //FData = TheUnitData;
            //// value = FData.GetValue(InitialUnitName, UnitData.FData.FldName);
            //if (!FData.DataLoaded)
            //{
            //    //YIKES THIS is  AN ERROR
            //    throw (new Exception("USGS data file did not load"));
            //}
            int test = 1;
          }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   9/11/2018. </remarks>
        ///
        /// <param name="USGSDataFilename"> Filename of the usgs data file. </param>
        /// <param name="aCallback">        The callback. </param>
        /// <param name="InitialStateName"> Name of the initial state. </param>
        ///-------------------------------------------------------------------------------------------------

        public West_CRF_Unit_Network(string USGSDataFilename, NetworkResetDelegate aCallback, string InitialStateName)
            : base(USGSDataFilename, aCallback, InitialStateName)
        {
            int test = 1;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds the resources. </summary>
        ///
        /// <remarks>   9/11/2018. </remarks>
        ///
        /// <param name="aUnitName">    Name of the unit. </param>
        ///-------------------------------------------------------------------------------------------------

        protected override void BuildResources(string aUnitName)
        {
            // ok let the base network add all the normal resources
            base.BuildResources(aUnitName);
            // now we add the Colorado resource
            UDI.eResource er;
            string FldName;
            int value;
            int PipeLineMethod;
            string errMsg = "";

            er = UDI.eResource.erColorado;
            FldName = FData.ResourceField(er);

            FData.GetValue(aUnitName, FldName, out value, out errMsg);
            FColorado = new CRF_Resource_ColoradoSurface(FldName, FData.ResourceLabel(er), ColoradoResourceColor, value);
            if (errMsg != "")
            {
                // ok so what is happening here.  If there was not an error retrieving this data, "value" is a good value and errMsg = "",
                // However, if there was an error, "value" = UDI.BadValue and errMsg has the error message.
                // So in this case we add the errmsg to the log for this data_item.
                FColorado.AddError(errMsg);
            }
            FResources.Add(FColorado);
            // =======================================================
            // edits 11.16.21 das
            er = UDI.eResource.erDesalination;
            FldName = FData.ResourceField(er);

            FData.GetValue(aUnitName, FldName, out value, out errMsg);
            //FDesal = new CRF_Resource_Desalination(FldName, FData.ResourceLabel(er), DesalinationResourceColor, value);

            // edits 01.10.22
            FData.GetValue(aUnitName,UDI.PipelineMethod, out PipeLineMethod,out errMsg);
            FDesal = new CRF_Resource_Desalination(FldName, FData.ResourceLabel(er), DesalinationResourceColor, value, PipeLineMethod);
            //double A =FResources
            // end edits 01.10.22 das
            if (errMsg != "")
            {
                // ok so what is happening here.  If there was not an error retrieving this data, "value" is a good value and errMsg = "",
                // However, if there was an error, "value" = UDI.BadValue and errMsg has the error message.
                // So in this case we add the errmsg to the log for this data_item.
                FDesal.AddError(errMsg);
            }
            FResources.Add(FDesal);
            // =====================================================================
            // end edits 11.16.21 das

            // edits 12.17.21 das
            // ===========================================================================================================================
            // Add new water (from condensate) here

            // ===========================================================================================================================
            // end edits 12.17.21 das
        }

        protected override void BuildConsumers(string aUnitName)
        {
            base.BuildConsumers(aUnitName);
            // no changes, just slightly slower
        }

        protected override void AssignFluxes(string StateName)
        {
            base.AssignFluxes(StateName);
            // I do not think I need to do anything here since I edite the UNITDATA construct fluxes method
        }
        /// <summary>
        /// 
        /// </summary>
        public  CRF_Resource Colorado
        {
            get { return FColorado; }
        }
        //
        /// <summary>
        /// 
        /// </summary>
        public CRF_Resource Desalination
        {
            get { return FDesal; }
        }
    }
}
