﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterSimDCDC.Generic;

namespace WaterSim_Base
{
    public class NewWater
    {
        internal struct NWconstants
        {      
            public const double panelsHouse = 9;
            public const double installations=20;
            public const double ratePerPanel = 3.5; // Liters per day
            internal const double LitersToGallons = 0.26417205236;
            internal const double ppH = 2.85;
            //
            // Parameter  A, Sunny = AS. Parameter A, partly cloudy = APC, Parameter A, Cloudy = AC
            // Parameter B, Sunny = BS, etc
            // equation=y = A *exp(-exp(b-c*x)) - Gompertz (Sit and Poulin (1994));
            //
            //                                      95% CI's
            internal const double AS = 6.8797; //5.2941 8.4653
            internal const double BS = 0.1790; //0.0688 0.2892
            internal const double CS = 0.2310; // 0.1162 0.3458
            internal const double APC = 5.5500; // 4.7634 6.3365
            internal const double BPC = 0.2375; // 0.1674 0.3076
            internal const double CPC = 0.2591; // 0.1747 0.3436
            internal const double AC = 4.4804; // 3.5146 5.4462
            internal const double BC = 0.2015; // 0.1003 0.3028
            internal const double CC = 0.2447; // 0.1203 0.3691

        }
        //
        UnitData FUnitData;
        bool FactionAirWater=false;
        NewWater NW = null;
        public NewWater(UnitData UnitData)
        {
            FUnitData = UnitData;
            initialize();
        }
        //
        void initialize()
        {

        }
        #region getsAndsets
        internal double AirWaterMax
        {
            get; set;
        }
        internal double DailyRateLitersAllHouseholds
        {
            get; set;
        }
        internal double Households
        {
            get; set;
        }
        internal double TotalPanels
        {
            get; set;
        }
        #endregion  getsAndsets
        // ===========================================================
        internal void calculateHouseholds(WaterSimCRFModel CRF)
        {
            Households = CRF.population * 1 / NWconstants.ppH;
        }
        internal void calculatePanelsTotal(WaterSimCRFModel CRF)
        {
            if (AirWaterInstallations(CRF))
            {
                TotalPanels = CRF.AirWaterInstallations * Households * NWconstants.panelsHouse;
            }
            else
            {
                TotalPanels = AirWaterMax * Households * NWconstants.panelsHouse;
            }
        }
        internal void calculateDailyRate(double rh)
        {
            double dailyRate = 0;
            int solar = 3;
            dailyRate = DailyWaterRatePerPanel(rh, solar);
            DailyRateLitersAllHouseholds = TotalPanels * dailyRate;
        }    
        // ==============================================
        internal double DailyWaterRatePerPanel(double rh, int sun)
        {
            // y = A *exp(-exp(b-c*x));
            double dX = rh * 0.1; // Relative humidity in the equaton is proportion (% * 0.1)
            double result = 0;
            result = NWconstants.ratePerPanel;
            if (0 < rh)
            {
                switch (sun)
                {
                    case 1:
                        result = NWconstants.AS * Math.Exp(-Math.Exp(NWconstants.BS - NWconstants.CS * dX)); // Sunny day
                        break;
                    case 2:
                        result = NWconstants.APC * Math.Exp(-Math.Exp(NWconstants.BPC - NWconstants.CPC * dX)); // Partly cloudy day
                        break;
                    case 3:
                        result = NWconstants.AC * Math.Exp(-Math.Exp(NWconstants.BC - NWconstants.CC * dX)); // Cloudy day
                        break;
                }
            }
            return result;
        }

        // ===========================================================    
        internal bool AirWaterInstallations(WaterSimCRFModel CRF)
        {
            bool temp = false;
            if (0 < CRF.AirWaterInstallations) { temp = true; }
            return temp;
        }
        // ------------------------------------------------------------
        // Public call to this class
        // =========================
        /// <summary>
        ///  rh=relative humidity in percent
        /// </summary>
        /// <param name="CRF"></param>
        /// <param name="rh"></param>
        /// <returns></returns>
        public double AirWaterUse(WaterSimCRFModel CRF, double rh)
        {           
            double result = 0; double temp = 0;
            calculateHouseholds(CRF);
            calculatePanelsTotal(CRF);
            calculateDailyRate(rh);
            temp = DailyRateLitersAllHouseholds *NWconstants.LitersToGallons;
            result = temp / 1e6;
            return result;
        }
        // ============================================================         
      
    }
}
