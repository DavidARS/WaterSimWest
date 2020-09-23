using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterSimDCDC.Generic
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
        // Million Acre-feet
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


        // conversions
        public const double acreToSquareMeters = 4046.8564224;
        public const double cubicMetersToAcreFeet = 0.00081071318212;
        public const double oneMillion = 1000000;
        public const double oneThousandth = 1000;
        public const double acreFeetToMAF = 0.0000001;
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
