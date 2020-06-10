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
        public static double initialPowellStorage = 23.435;
        public static double initialMeadStorage = 27.012167;
        public static double powellDeadPool = 1.89;
        public static double meadDeadPool = 2.0;
        public static double powellMaxPool = 26.322;
        public static double meadMaxPool = 27.877;
        //
        public static double powellTargetRelease = 8.23;
        public static double powellNineFive = 9.5;
        public static double powellNineZero = 9.0;
        public static double powellSevenEight = 7.8;
        public static double powellSevenFourEight = 7.48;
        public static double powellSeven = 7.0;
        //
        public static double panEvap_mead = 2.28092;
        public static double panReach = 4.5;
        public static double pan_powell = 1.763776;
        public static double bankStorage = 1.5;
        public static double bankStorage_powell = 0.08;
        public static double bankStorage_mead = 0.065;
        //
        public static double allocation_nv = 0.3;
        public static double allocation_ca = 4.4;
        public static double allocation_az = 2.8;
        public static double allocation_mx = 1.5;
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
        public static double meadStepOne = 1075;
        public static double meadStepTwo = 1050;
        public static double meadStepThree = 1025;
        //
        public const double sevenStatesLCshortage_3 = 0.625;
        public const double sevenStatesLCshortage_2 = 0.487;
        public const double sevenStatesLCshortage_1 = 0.383;



    }
}
