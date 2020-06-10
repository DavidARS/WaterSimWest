using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using WaterSimDCDC;
using WaterSimDCDC.Documentation;
using System.IO;
using ConsumerResourceModelFramework;
using WaterSimDCDC.America;

namespace WaterSimDCDC.Processes
{
    #region Feedback Process
    public class AlterWaterManagementFeedbackProcess : WaterSimDCDC.AnnualFeedbackProcess
    {
       
        int FSURFManagementFactor = 0;
        int FGWManagementFactor = 0;
        int FPolicyStartYear = 2016;
        //int FLAKEManagementFactor = 0;
        //double FDESALManagementFactor = 0;
        //
        int FDroughtManagementFactor = 0;
        //
        public AlterWaterManagementFeedbackProcess(string aName)
            : base(aName)
        {

        }            
        //      //WSA = new CRF_Network_WaterSim_America(WS.DataDirectory + "\\" + StateData, null, State);
        //}
        public override bool ProcessInitialized(WaterSimManagerClass WS)
        {
            return base.ProcessInitialized(WS);
        }
        public override bool ProcessStarted(int year, WaterSimManagerClass WSimClass)
        {
            WaterSimManager WS = (WSimClass as WaterSimManager);
            //
            ProcessWaterManagement(year, WS);
            ProcessDrought(year, WS);
            ProcessIndicators(year, WS);
            //
            return base.ProcessStarted(year, WS);
        }
        void ProcessWaterManagement(int year, WaterSimManager WS)
        {

            // OK Simulation Starting, Parameters have been set, fetch 
            FSURFManagementFactor = WS.ParamManager.Model_ParameterBaseClass(eModelParam.epSurfaceWaterManagement).Value;
            FGWManagementFactor = WS.ParamManager.Model_ParameterBaseClass(eModelParam.epGroundwaterManagement).Value;
            FPolicyStartYear = WS.ParamManager.Model_ParameterBaseClass(eModelParam.epPolicyStartYear).Value;
            //
            //FLAKEManagementFactor = WS.ParamManager.Model_ParameterBaseClass(eModelParam.epLakeWaterManagement).Value;
            //FDESALManagementFactor = WS.WaterSimAmerica.Desal;
            //
            // test this commented out
            //PreProcessMyWaterManagementProcess(year, WS);
           // double temp = 1.0;
            double tempS = 1.0;
            double tempGW = 1.0;
            if (year == WS.Simulation_Start_Year)
            {
                initialFlowSurface = WS.WaterSimAmerica.geti_SurfaceWaterFresh();// WSA.SurfaceFresh.Limit;
                initialFlowModGW = WS.WaterSimAmerica.geti_Groundwater(); // .Groundwater.Limit;
                initialFlowLake = WS.WaterSimAmerica.geti_SurfaceLake();
            }
            //
            int surf = FSURFManagementFactor;
            if (0 < surf)
            {
                switch (surf)
                {
                    case 1:
                         tempS = 1.15;
                        break;
                    case 2:
                         tempS = 1.3;
                        break;
                    case 3:
                        tempS = 1.45;
                        break;
                    case 4:
                        tempS = 1.4;
                        break;
                    default:
                        if (2015 < year && year < 2020)
                            tempS = 1.1;
                        break;
                }

                modSurface = tempS;


            } // END of 0 < use
            int gw = FGWManagementFactor;
            if (0 < gw)
            {
                switch (gw)
                {
                    case 1:
                        tempGW = 0.7;
                        break;
                    case 2:
                        tempGW = 0.9;
                        break;
                    case 3:
                        tempGW = 1.2;
                        break;
                    case 4:
                        tempGW = 1.4;
                        break;
                    case 5:
                        tempGW = 1.3;
                        break;

                    default:
                       
                            tempGW = 1.0;
                        break;


                }
                modGroundwater = tempGW;
            }      
        }
        // -------------------------------------------------------------
        void ProcessDrought(int year, WaterSimManager WS)
        {
            double tempD = 1.0;
            FDroughtManagementFactor = WS.ParamManager.Model_ParameterBaseClass(eModelParam.epClimateDrought).Value;
            int temp = FDroughtManagementFactor;

            switch (temp)
            {
                // Bypass the Process Drought Case Structure
                case 0:
                     break;
                // Use Canned Drought Scenarios
                case 1:
                    WS.startDrought = 2030;
                    WS.endDrought = 2065;
                    tempD = 0.9;
                    break;
                case 2:
                    WS.startDrought = 2020;
                    WS.endDrought = 2065;
                    tempD = 0.9;

                    break;
                case 3:
                    WS.startDrought = 2030;
                    WS.endDrought = 2065;
                    tempD = 0.8;
                    break;
                case 4:
                    WS.startDrought = 2020;
                    WS.endDrought = 2065;
                    tempD = 0.8;
                    break;
                default:
                    if (2015 < year && year < 2020)
                        tempD = 1.0;
                    break;
            }
                modDrought = tempD;
             // END of 0 < use

        }
       // -------------------------------------------------------------
        void ProcessIndicators(int year, WaterSimManager WS)
        {

            
        }
        // -----------------------------------------------
        // Initial values of Resources so that we can
        // return to them once management has ended
        // 03.07.16
        // ----------------------------
        double _initialFlowSurface = 0;
        double initialFlowSurface
        {
            get { return _initialFlowSurface; }
            set { _initialFlowSurface = value; }
        }
        double _initialFlowModGW = 0;
        double initialFlowModGW
        {
            get { return _initialFlowModGW; }
            set { _initialFlowModGW = value; }
        }
        double _initialFlowLake = 0;
        double initialFlowLake
        {
            get { return _initialFlowLake; }
            set { _initialFlowLake = value; }
        }
        double _initialDesal = 0;
        double initialDesal
        {
            get { return _initialDesal; }
            set { _initialDesal = value; }
        }
        // ==============================================
        double _modSurface = 0;
        double modSurface {
            get { return _modSurface; }
            set { _modSurface = value; }
        }
        double _modGroundwater = 0;
        double modGroundwater
        {
            get { return _modGroundwater; }
            set { _modGroundwater = value; }
        }
        double _modLakewater = 0;
        double modLakewater
        {
            get { return _modLakewater; }
            set { _modLakewater = value; }
        }
        // ====================================
        // Process Drought
        double _modDrought = 0;
        double modDrought
        {
            get { return _modDrought; }
            set { _modDrought = value; }
        }
        // ----------------------------------------------------
        // =======================================================================
        //
        public override bool PreProcess(int year, WaterSimManagerClass WSimClass)
        {
            WaterSimManager WSim = (WSimClass as WaterSimManager);
            PreProcessMyWaterManagementProcess(year, WSim);
            PreProcessMyDroughtManagementProcess(year, WSim);
            PreProcessIndicatorsProcess(year, WSim);
            return base.PreProcess(year, WSim);
        }
        //

        void PreProcessMyWaterManagementProcess(int year, WaterSimManager WS)
        {
            int surf = FSURFManagementFactor;
            int gw = FGWManagementFactor;
            int policy = FPolicyStartYear;
            // Assumes the same management period for all management actions
            //
            //if (WS.startSGWM == year)
            if (policy == year)
            {
                if(0 < surf)WS.ParamManager.Model_Parameter("SWMC").Value = Convert.ToInt32(modSurface * 100);
                if(0 < gw)WS.ParamManager.Model_Parameter("GWMC").Value = Convert.ToInt32(modGroundwater * 100);
                WS.WaterSimAmerica.invokePolicies = true;
            }
            //if (WS.startSGWM+1 == year)
            if(policy + 1 == year)
            {
                WS.ParamManager.Model_Parameter("SWMC").Value =100;
                WS.ParamManager.Model_Parameter("GWMC").Value = 100;
                WS.ParamManager.Model_Parameter("LWM").Value = 0;
                WS.ParamManager.Model_Parameter("DESAL").Value = 0;
            }

            if (WS.endSGWM == year)
            {
                // This may need un-commenting 
                // 03.09.16
                //WS.ParamManager.Model_Parameter("SUR").Value = Convert.ToInt32(initialFlowSurface);
                //WS.ParamManager.Model_Parameter("GW").Value = Convert.ToInt32(initialFlowModGW);
                //WS.ParamManager.Model_Parameter("SURL").Value = Convert.ToInt32(initialFlowLake);
                //WS.ParamManager.Model_Parameter("DESAL").Value = Convert.ToInt32(initialDesal);
            }
            //
        }
        void PreProcessMyDroughtManagementProcess(int year, WaterSimManager WS)
        {
                if (WS.startDrought == year)
                {
                    //WS.ParamManager.Model_Parameter("DC").Value = Convert.ToInt32(modDrought * 100);

                }
                if (WS.startDrought + 1 == year)
                {
                    WS.ParamManager.Model_Parameter("DC").Value = 100;
                }

                if (WS.endDrought == year)
                {
              //      WS.ParamManager.Model_Parameter("SUR").Value = Convert.ToInt32(initialFlowSurface);

                }
            //}

        }
        void PreProcessIndicatorsProcess(int year, WaterSimManager WS)
        {
            //WS.ParamManager.Model_Parameter("GWIND").Value = 0;
        }
        public override bool PostProcess(int year, WaterSimManagerClass WS)
        {
            FSURFManagementFactor = WS.ParamManager.Model_ParameterBaseClass(eModelParam.epSurfaceWaterManagement).Value;
            FGWManagementFactor = WS.ParamManager.Model_ParameterBaseClass(eModelParam.epGroundwaterManagement).Value;
            FDroughtManagementFactor = WS.ParamManager.Model_ParameterBaseClass(eModelParam.epClimateDrought).Value;

            return base.PostProcess(year, WS);
        }
    }
    #endregion
}
