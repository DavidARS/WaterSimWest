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
    public struct Constants
    {
        int Fyear;

        public Constants(int year)
        {
            Fyear = year;
        }
        //
        public int TheYear
        {
            get { return Fyear; }
        }
        // =====================================================
        // Powell and Mead
        // --------------------
        // Million Acre-feet - using 1 January 2000 as the initial starting point
        // 
        public static double initialPowellStorage = 23.333075;
        public static double initialMeadStorage = 27.012167;
        //
        public static double powellDeadPool = 1.89500;
        public static double meadDeadPool = 2.0;
        // These estimates were re-evaluated on 10.01.18 and corrected from FORTRAN
        public static double maxPowellLivePool = 24.3;//26.322 ;
        public static double maxPowellTotalPool = 26.195;//29.217;

        public static double maxMeadLivePool = 25.865918;//27.865918;
        public static double maxMeadTotalPool = 27.865918;//29.418237;
        //
        public const double maxMeadElevation = 1229;
        public const double maxPowellElevation = 3710;
        //
        public static double powellTargetRelease = 8.23;
        public static double powellTenFive = 10.5;
        public static double powellNineFive = 9.5;
        public static double powellNineZero = 9.0;
        public static double powellSevenEight = 7.8;
        public static double powellSevenFourEight = 7.48;
        public static double powellSevenZero = 7.0;
        //
        // ==========================================================================
        // DCP updated. 04.12.21
        // DCP 10.13.20 das
        // trigger elevations
        // units feet msl
        public static int meadTierZero = 1090;
        public static int mead1045 = 1045;
        public static int mead1040 = 1040;
        public static int mead1035 = 1035;
        public static int mead1030 = 1030;
        // units acre-feet year-1
        // ICS contributions
        // ===================================
        public static int azMead192k = 192000;
        public static int azMead240k = 240000;
        //
        public static int nvMead8k = 8000;
        public static int nvMead10k = 10000;
        //
        public static int caMead200k = 200000;
        public static int caMead250k = 250000;
        public static int caMead300k = 300000;
        public static int caMead350k = 350000;
        // ===================================
        public static double dcpReduction2027 = 0.03;
        // ===================================
        // Misc subtractions
        // 2015 Coloraod Research Group 2016 Tribes and Colorado River Rights.pdf
        // units are in acre-feet year-1
        public static int UBTribalDiversions = 1358385;
        // units are in acre-feet year-1
        // Data from:
        //https://knowyourwaternews.com/cap-water-orders-planning-for-1-4-million-acre-feet-of-deliveries/
        public static int CAPsystemLosses = 75000;
        //
        // Forebearance and conservation agreements
        // units acre-feet year-1
        public static int LBAgForebearance3 = 57000;
        public static int MWDconservationAgree = 3500;

        // ==========================================================================
        // Evaporation Rates
        // units are meters
        //(http://pubs.usgs.gov/sir/2006/5252/pdf/sir20065252.pdf)
        public static double pan_mead = 2.28092;
        public static double pan_reach = 4.5;
        // (http://www.riversimulator.org/Resources/Hydrology/EvaporationAtLakePowellUCRCopt.pdf)
        public static double pan_powell = 1.763776;
  
        // Bank Storage
        //public static double bankStorage = 1.5;
        public static double bankStorage = 1.0;
        public static double bankStorage_powell = 0.08;
        public static double bankStorage_mead = 0.065;
        //
        public static double allocation_nv = 0.3;
        public static double allocation_ca = 4.4;
        public static double allocation_az = 2.8;
        public static double allocation_mx = 1.5;
        public const double allocation_LowerBasin = 9.0;
        public const double allocation_UpperBasin = 7.5;


        //
        public const double CAPcapacity = 2.17;
        // 
        public static double shortage_2 = 0.487;
        public static double shortage_3 = 0.625;
        public static double shortageRatio_1_az = 0.835510;
        public static double shortageRatio_2_az = 0.821357;
        public static double shortageRatio_3_az = 0.768;
        public static double shortageRatio_1_nv = 0.033943;
        public static double shortageRatio_2_nv = 0.034908;
        public static double shortageRatio_3_nv = 0.032;
        //
        //  public static double shortageRatio_mx = 1-az-nv
        //
        //public static double meadVolThreshhold_2 = 7.83602; // mead < 1025 feet msl
        //public static double meadVolThreshhold_3 = 7.83602; // mead at 1025 feet msl
        //public static double meadVolThreshhold_4 = 9.50590; // mead at 1050 feet msl
        //public static double meadVolThreshhold_5 = 11.4049; // mead at 1075 feet msl

        // Shortage Elevations on Lake Mead
        public static double meadStepOne = 1075;
        public static double meadStepTwo = 1050;
        public static double meadStepThree = 1025;
        //
        // Shortage Ratios
        public const double sevenStatesLCshortage_3 = 0.625;
        public const double sevenStatesLCshortage_2 = 0.487;
        public const double sevenStatesLCshortage_1 = 0.383;
        //
        // Bank Storage rates
        public const double cbankStorageMead = 0.065;
        public const double cbankStoragePowell = 0.08;
        public const double cbankStorageChannel = 0.08;
        //
        // Release from Powell estimates
        public const double normalReleasePowell = 8.23; // million acre-feet;
        public const double normalReleaseMead = 9.0; // million acre-feet;
        //
        // tier elevations
        public const double tierElevationOne = 3525; // live storage;
        public const double tierElevationTwo = 3575;
        public const double tierElevation602a = 3630;
        public const double tierElevationThree = 3636;
        public const double tierElevationFour = 3666;
        //
        public const double tierElevationMeadOne = 1025;
        public const double tierElevationMeadTwo = 1075;
        // =====================================================
        // Others
        // total pools
        public const double totalCapacity_BlueMesa = 940700; // af - Gunnison River
        public const double totalCapacity_NavajoLake = 1708600;
        public const double totalCapacity_FlamingGorge = 3788900;
        public const double totalCapacity_LakeHavasu = 648000;
        public const double totalCapacity_LakeMohave = 1818300;
        // live pools
        public const double liveCapacity_BlueMesa = 784430; // af
        public const double liveCapacity_NavajoLake = 1036100;
        public const double liveCapacity_FlamingGorge = 3515700;
        public const double liveCapacity_LakeHavasu = 619400;
        public const double liveCapacity_LakeMohave = 1809950;
        // dead pools
        public const double deadStorage_BlueMesa = 111232; // af
        public const double deadStorage_NavajoLake = 12600;
        public const double deadStorage_FlamingGorge = 40000;
        public const double deadStorage_LakeHavasu = 28600;
        public const double deadStorage_LakeMohave = 8350;
        // flood control bottom
        public const double floodBottom_BlueMesa = 7519.4; // ft
        public const double floodBottom_NavajoLake = 6085.0;
        public const double floodBottom_FlamingGorge = 6040.0;
        //public const double floodBottom_LakeHavasu = ;
        public const double floodBottom_LakeMohave = 647.0;
        // flood control top
        public const double floodTop_BlueMesa = 7528.0; // ft
        public const double floodTop_NavajoLake = 6108.0;
        public const double floodTop_FlamingGorge = 6045.0;
        //public const double floodBottom_LakeHavasu = ;
        public const double floodTop_LakeMohave = 647.0;
        //
        // =====================================================
        // CO River Priority Constants
        public const double p3_AkChin=47500;
        public const double p3_7cities = 20900; 
        public const double p3_threshold = 68400;
        //
        public const double p4_threshold = 853079;
        public const double p4_indianPercent = 63.62482;
        public const double pIndian_total = 343079;
        public const double pMandI_total=638823;
        //
        public const double pNIA_threshold = 1050302;
        public const double pNIA_indian = 93302.729;
        public const double pNIA_capMinIPW = 25.43800;
        public const double tribalNIA_total= 216100;
        public const double mandINIA_total = 148598;
        //
        public const double pAgPool_threshold = 1415000;
        public const double pAgPool_total = 300000;
        // =====================================================
        // CO River Delta Constants
        //
        public const double deltaBurden = 158088;

        //
        // =====================================================
        // CO River Provider Designations
        // CAP IV
        public const double CAP_one = 620678; // was 607999; // 356922 (Phoenix Metro) from 08/07/2018 document. (620678 AF)
        // CAP V
        public const double CAP_two = 168316; // This perhaps should be 148,598 AF (52,303 AF-Phoenix Metro)
        //
        public const double CAP_agSurface = 465572;


        // =====================================================
        // conversions
        public const double acreToSquareMeters = 4046.8564224;
        public const double cubicMetersToAcreFeet = 0.00081071318212;
        public const double oneMillion = 1000000;
        public const double oneThousandth = 1000;
        public const double acreFeetToMAF = 0.000001;
        //
        public const double AFtoMGD = (325851.43326/ oneMillion) / 365;
        public const double MAFtoMGD = 325851.43326  / 365;
        //
        public const int AZ = 4;
        public const int CA = 6;
        public const int NV = 32;
        //
        public const int DCPstartYear = 2020;
        //


        //
    }
    //
    public static class CORiverUtilities
    {
        public static double MeadElevation(double state)
        {
            const double slope = 0.3420;
            const double scaling = 178.1;
            const double intercept = 664;
            double result = 0;
            double temp = 0;
            //
            if (Constants.meadDeadPool <= state)
            {
                if (state <= Constants.maxMeadTotalPool)
                {
                    temp = intercept + scaling * Math.Pow(state, slope);
                }
                else
                {
                    temp = intercept + scaling * Math.Pow(Constants.maxMeadTotalPool, slope);
                }
            }
            else
            {
                temp = intercept + scaling * Math.Pow(Constants.meadDeadPool, slope);
            }
            result = temp;
            if (Constants.maxMeadElevation < temp) result = Constants.maxMeadElevation;
            //
            return result;
        }
        //
        public static double PowellElevation(double state)
        {
            const double slope = 0.2136;
            const double scaling = 382.8;
            const double intercept = 2931;
            double result = 0;
            double temp = 0;
            //
            if (Constants.powellDeadPool <= state)
            {
                if (state <= Constants.maxPowellTotalPool)
                {
                    temp = intercept + scaling * Math.Pow(state, slope);
                }
                else
                {
                    temp = intercept + scaling * Math.Pow(Constants.maxPowellTotalPool, slope);
                }
            }
            else
            {
                temp = intercept + scaling * Math.Pow(Constants.powellDeadPool, slope);
            }
            result = temp;
            if (Constants.maxPowellElevation < temp) result = Constants.maxPowellElevation;
            //
            return result;
        }
        //
        public static double StateFromElevationPowell(double elevation)
        {
            double temp = 0;
            double result = 0;
            const double slope = 0.2136;
            const double scaling = 382.8;
            const double intercept = 2931.0;

            temp = Math.Pow(((elevation - intercept) * (1 / scaling)), (1 / slope));
            result = temp;
            return result;
        }
        //
        //
        public static double StateFromElevationMead(double elevation)
        {
            double temp = 0;
            double result = 0;
            const double slope = 0.3420;
            const double scaling = 178.1;
            const double intercept = 665.5;

            temp =  Math.Pow( ((elevation - intercept) * (1 / scaling)), (1 / slope) );
            result = temp;
            return result;
        }
        //

        public static double TargetElevations(int year)
        {
            double temp = 0;
            double result = 0;
            const int index = 19;
            const int yearTarget = 2008;
            const int yearMax = 2027;
            int[] targets = new int[index] {3636,3639,3642,3643,3645,3646,3648,3649,3651,3652,3654,3655,3657,3659,3660,3662,
            3663,3664,3666 };
            //
            temp = 3636;
            if (yearTarget <= year)
            {
                temp = targets[18];
                if (year < yearMax)
                {
                    int counter = 0;
                    counter = year - yearTarget;
                    temp = targets[counter];
                }
            }
            result = temp;
            return result;
        }
        //
        // Bank storage from both Lake Mead and Lake Powell
        // ----------------------------------------------------------
        /// <summary>
        /// Read in the difference in storage from time t to time t+1
        ///  For Powell, Mead, and the reach between the two.
        /// </summary>
        /// <param name="diffStorage"></param>
        /// <param name="constant"></param>
        /// <returns></returns>
        public static double BankStorage(double diffStorage, double constant)
        {
            double result = 0;

            result = diffStorage * constant;// Constants.cbankStorageMead;
            return result;
        }
        //
        //public static double bankStoragePowell(float diffStorage)
        //{
        //    double result = 0;

        //    result = diffStorage * Constants.cbankStoragePowell;
        //    return result;
        //}
        // ----------------------------------------------------------
        //
        // Evaporation from surface Waters
        #region Evaporation
        // Evaporation from Lake Powell
        // ------------------------------------------------------------------------------
        public static double EvaporationPowell(double volumeTotal, double panEvaporation)
        {
            double B0 = 1422.6;
            double B1 = 11421.8;
            double B2 = 0.8066;
            double maxPowellSurfaceArea = 168927;
            double minPowellSurfaceArea = 20303;
            double result = 0;
            double temp = 0;
            // area in acres
            if(Constants.powellDeadPool < volumeTotal)
            {
                if (volumeTotal < Constants.maxPowellTotalPool)
                {
                    temp = B0 + B1 * Math.Pow(volumeTotal, B2);
                }
                else
                {
                    temp= maxPowellSurfaceArea;
                }
            }
            else
            {
                temp = minPowellSurfaceArea;
            }
            // units in million acre-feet
            result = (temp * Constants.acreToSquareMeters * panEvaporation) * 
                Constants.cubicMetersToAcreFeet * (1 / Constants.oneMillion);
            return result;
        }
        // -------------------------------------------------------------------------------
        // Evaporation from Lake Powell
        // ------------------------------------------------------------------------------
        public static double EvaporationMead(double volumeTotal, double panEvaporation)
        {
            double B0 = 17136.2;
            double B1 = 7041.6;
            double B2 = 0.8978;
            double maxMeadSurfaceArea = 144893; // acres;
            double minMeadSurfaceArea = 28911;
            double result = 0;
            double temp = 0;
            // --------------------------------
            //
            if (Constants.meadDeadPool < volumeTotal)
            {
                if (volumeTotal < Constants.maxMeadTotalPool)
                {
                    temp = B0 + B1 * Math.Pow(volumeTotal, B2);
                }
                else
                {
                    temp = maxMeadSurfaceArea;

                }
            }
            else
            {
                temp = minMeadSurfaceArea;
            }
            // units in million acre-feet
            result = (temp * Constants.acreToSquareMeters * panEvaporation) *
                Constants.cubicMetersToAcreFeet * (1 / Constants.oneMillion);
            return result;
        }
        //
        public static double EvaporationReach(double panEvaporation)
        {
            double area = 38970000; // m2;
            double result = 0;
            result = area * panEvaporation * Constants.cubicMetersToAcreFeet * (1 / Constants.oneMillion);
            return result;
        }
        public static double UpperBasinActual(int year)
        {
            double result = 0;
            double temp = 0;
            // From Where?
            switch (year)
            {
                case 2000:
                    temp = 3.955722;
                    break;
                case 2001:
                    temp = 4.218954;
                    break;
                case 2002:
                    temp = 3.774367;
                    break;
                case 2003:
                    temp = 3.789496;
                    break;
                case 2004:
                    temp = 3.550691;
                    break;
                case 2005:
                    temp = 3.645886;
                     break;
                case 2006:
                    temp = 3.837139;
                     break;
                case 2007:
                    temp = 4.120767;
                    break;
                case 2008:
                    temp = 4.180609;
                    break;
                case 2009:
                    temp = 4.117443;
                    break;
                case 2010:
                    temp = 4.001442;
                    break;
                case 2011:
                    temp = 4.100000;
                    break;
                case 2012:
                    temp = 4.100000;
                    break;
            }
            result = temp;
            return result;
        }


        #endregion Evaporation
        // -------------------------------------------------------------------------------

        //
    }

    //
}
