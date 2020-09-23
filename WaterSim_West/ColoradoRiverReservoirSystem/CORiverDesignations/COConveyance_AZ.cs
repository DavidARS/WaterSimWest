using System;
using WaterSimDCDC.Generic;
using System.Linq;
namespace CORiverDesignations 
{
    public class COConveyance_AZ : BaseAllottments
    {
        string FID = "RightsP1.txt";

        public COConveyance_AZ()
        {
            ReadIt(FID);          
            string[] array = data.ToArray();
            Priority_P1 = array.Select(x => float.Parse(x)).ToArray();
            Priority_P2 = array.Select(x => float.Parse(x)).ToArray();
            Priority_P3 = array.Select(x => float.Parse(x)).ToArray();
            Priority_P4 = array.Select(x => float.Parse(x)).ToArray();

        }
        public override void Initialize()
        {
            Priority_P3[(int)RightHoldersP3.AkChin] = Convert.ToSingle(Constants.p3_AkChin );
            Priority_P3[(int)RightHoldersP3.SevenCities] = Convert.ToSingle(Constants.p3_7cities );

        }
        #region properties
        //double _Conveyance = 0;
        //public double Conveyance
        //{
        //    set { _Conveyance = value; }
        //    get { return _Conveyance; }
        //}
        ////
        //bool _WaterForTheDelta = false;
        //public bool WaterForTheDelta
        //{
        //    set { _WaterForTheDelta = value; }
        //    get { return _WaterForTheDelta; }
        //}


        #region allocations
        double _P3AkChin = 0;
        public double AkChinP3
        {
            set { _P3AkChin = value; }
            get { return _P3AkChin; }
        }
        double _P4AkChin = 0;
        public double AkChinIndianPriority
        {
            set { _P4AkChin = value; }
            get { return _P4AkChin; }
        }
        // Seven Cities priority three
        double _P3cities = 0;
        public double CitiesP3
        {
            set { _P3cities = value; }
            get { return _P3cities; }
        }
        double _P4cities = 0;
        public double CitiesMandIPriority
        {
            set { _P4cities = value; }
            get { return _P4cities; }
        }
        //
        double _P5Indian = 0;
        public double TribalPriority_NIA
        {
            set { _P5Indian = value; }
            get { return _P5Indian; }
        }
        double _P5cities = 0;
        public double MandIPriority_NIA
        {
            set { _P5cities = value; }
            get { return _P5cities; }
        }
        double _AgPool = 0;
        public double AgPool
        {
            set { _AgPool = value; }
            get { return _AgPool; }
        }
        double _Excess = 0;
        public double Excess
        {
            set { _Excess = value; }
            get { return _Excess; }
        }
        double _Reductions = 1;
        public double ReduceP3Allocation
        {
            set { _Reductions = value; }
            get { return _Reductions; }
        }
        double _Remaining = 0;
        public double Remaining
        {
            set { _Remaining = value; }
            get { return _Remaining; }
        }
        double _Percent = 0;
        public double Percent
        {
            set { _Percent = value; }
            get { return _Percent; }
        }
        #endregion allocations
        // =====================
        // Priority Water
        // -------------------------
        #region Priority Water (rights)
        double _MandI = 0;
        public double CAPMandI
        {
            set { _MandI = value; }
            get { return _MandI; }
        }
        double _CAPNIA = 0;
        public double CAPNIA
        {
            set { _CAPNIA = value; }
            get { return _CAPNIA; }
        }
        double _AgPoolActual = 0;
        public double AgPoolActual
        {
            set { _AgPoolActual = value; }
            get { return _AgPoolActual; }
        }
        #endregion Priority Water (rights)
        // ===============================
        // CO RIver delta Water available
        #region CO delta properties
        //double _LossNIA = 0;
        //public double ProportionalLossNIA
        //{
        //    set { _LossNIA = value; }
        //    get { return _LossNIA; }
        //}
        //double _LossMandI = 0;
        //public double ProportionalLossMandI
        //{
        //    set { _LossMandI = value; }
        //    get { return _LossNIA; }
        //}
        //bool _AZalone = true;
        //public bool AZdeltaBurden
        //{
        //    set { _AZalone = value; }
        //    get { return _AZalone; }
        //}
        //double _COdeltaWater_AZ = 0;
        //public double COdeltaWaterAZ
        //{
        //    set { _COdeltaWater_AZ = value; }
        //    get { return _COdeltaWater_AZ; }
        //}
        //double _DeltaWaterReleased = 1;
        //public double ProportionalDeltaWater
        //{
        //    set { _DeltaWaterReleased = value; }
        //    get { return _DeltaWaterReleased; }
        //}
        #endregion CO delta properties
        // ===============================
        // CO water Designation Properties
        #region CO water designations
        //double _CreditsTransferedFromAg = 0;
        //public double CreditsTransferedFromAg
        //{
        //    set { _CreditsTransferedFromAg = value; }
        //    get { return _CreditsTransferedFromAg; }
        //}
        //double _AgCAPcredits = 0;
        //public double AgCAPcredits
        //{
        //    set { _AgCAPcredits = value; }
        //    get { return _AgCAPcredits; }
        //}
        //double _PropCreditsToTransferAg = 0;
        //public double PropCreditsToTransferAg
        //{
        //    set { _PropCreditsToTransferAg = value; }
        //    get { return _PropCreditsToTransferAg; }

        //}

        //double _FallowAg = 0;
        //public double FallowAgProportion
        //{
        //    set { _FallowAg = value; }
        //    get { return _FallowAg; }

        //}
        ////
        //double _CAPneeded = 0;
        //public double CAPneeded
        //{
        //    set { _CAPneeded = value; }
        //    get { return _CAPneeded; }
        //}
        //double _CAPdesiredFour = 0;
        //public double CAPdesired_IV
        //{
        //    set { _CAPdesiredFour = value; }
        //    get { return _CAPdesiredFour; }
        //}
        //double _CAPdesiredFive = 0;
        //public double CAPdesired_V
        //{
        //    set { _CAPdesiredFive = value; }
        //    get { return _CAPdesiredFive; }
        //}
        //double _CAPdesiredSix = 0;
        //public double CAPdesired_VI
        //{
        //    set { _CAPdesiredSix = value; }
        //    get { return _CAPdesiredSix; }
        //}
        //double _RealizedCAP = 0;
        //public double RealizedCAP
        //{
        //    set { _RealizedCAP = value; }
        //    get { return _RealizedCAP; }
        //}
        //double _WaterBankCAP = 0;
        //public double WaterBankCAP
        //{
        //    set { _WaterBankCAP = value; }
        //    get { return _WaterBankCAP; }
        //}

        #endregion CO water designations

       
        #endregion properties


        public override int Elevation()
        {
            throw new NotImplementedException();
        }
        public override double LivePoolVolume()
        {
            throw new NotImplementedException();
        }

        //public enum rightHolders
        //{
        //    AkChin=0,
        //    CitiesP3,
        //    IndianP4,
        //    MandIP4

        //}
        #region allottments
        public override void Allottments(int year, double cap)
        {
            //Excess = 0;
            //AgPool = 0;
            int p = 0;
            double CAP = cap * (1 / Constants.acreFeetToMAF);
            //
            //Priority_P1[(int)RightHolders.one] = 1;
            //float me = Priority_P2[(int)RightHolders.one] * 0.9  ;
            double est = Priority_P2[(int)RightHoldersP2.two] * 0.9; 
            float Test = Priority_P3[(int)RightHoldersP1.one];
            float TesT = Priority_P4[(int)RightHoldersP1.one];
            if (year <= 2043)
            {
                p = 3;
                //Priority_P3[(int)RightHoldersP3.AkChin]= Convert.ToSingle(Constants.p3_AkChin * ReduceLegalRightThreeOne);
                //Priority_P3[(int)RightHoldersP3.SevenCities] = Convert.ToSingle(Constants.p3_7cities * ReduceLegalRightThreeOne);
                InitialRight = LegalRightOne = Priority_P3[(int)RightHoldersP3.AkChin] ; //LegalRightOne = Constants.p3_AkChin * ReduceLegalRightThreeOne;
                DependentRight = LegalRightTwo = Priority_P3[(int)RightHoldersP3.SevenCities]; //LegalRightTwo = Constants.p3_7cities * ReduceLegalRightThreeTwo;
                //
                Threshold = Constants.p3_threshold;
                VolumeAllottments(p, CAP);
                RealizedRight_P3[(int)RightHoldersP3.AkChin] = Convert.ToSingle(InitialRight);
                RealizedRight_P3[(int)RightHoldersP3.SevenCities] = Convert.ToSingle(DependentRight);
                //
                //AkChinP3 = InitialRight; CitiesP3 = DependentRight;

                p = 4;
                InitialRight = LegalRightOne = Constants.pIndian_total;
                DependentRight = LegalRightTwo = Constants.pMandI_total;
                Percent = Constants.p4_indianPercent;
                //InitialRight = LegalRightOne; DependentRight = LegalRightTwo;

                Threshold = Constants.p4_threshold;
                VolumeAllottments(p, CAP);
                AkChinIndianPriority = InitialRight; CitiesMandIPriority = DependentRight;

                p = 5;
                InitialRight = Constants.tribalNIA_total;
                LegalRightOne = Constants.pNIA_indian;
                DependentRight = LegalRightTwo = Constants.mandINIA_total;
                //InitialRight = LegalRightOne; DependentRight = LegalRightTwo;
                Percent = Constants.pNIA_capMinIPW;

                Threshold = Constants.pNIA_threshold;
                VolumeAllottments(p, CAP);
                TribalPriority_NIA = InitialRight; MandIPriority_NIA = DependentRight;

                p = 6;
                Threshold = Constants.pAgPool_total;
                VolumeAllottments(p, CAP);
                CAPNIA = MandIPriority_NIA + AgPool + Excess;
                CAPMandI = CitiesP3 + CitiesMandIPriority;

            }
            else
            { }
        }


        public override bool VolumeAllottments(int priority, double volume)
        {
            bool result=false;
             switch (priority)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    // CAP < 68400 acre-feet;
                     if (volume < Threshold)
                    {
                        InitialRight = (LegalRightOne / Threshold) * Remaining;
                        DependentRight = Remaining - InitialRight;
                        Remaining = 0;
                    }
                    else { Remaining -= Math.Max(0, InitialRight + DependentRight); }
                    break;
                case 4:
                    //  CAP water > 68400 and <=853,079 AcF  - 
                    if (volume < Threshold)
                    {
                        InitialRight = (1 - (Percent * 0.01)) * Remaining;
                        DependentRight = Remaining - InitialRight;
                        Remaining = 0;
                    }
                    else { Remaining -= Math.Max(0, InitialRight + DependentRight); }

                    break;
                case 5:
                    //CAP water is between 853,079 and 1,050,302 AcF
                    if (volume < Threshold)
                    {
                        InitialRight =  (LegalRightOne + ((Percent * 0.01) * Remaining));
                        DependentRight = Remaining - InitialRight;
                        Remaining = 0;
                    }
                    else { Remaining -= Math.Max(0, InitialRight + DependentRight); }
                    break;
                case 6:
                    if (Remaining < Threshold)
                    {
                        AgPool = Remaining;
                        Remaining = 0;
                    }
                    else
                    {
                        AgPool = Constants.pAgPool_total;
                        Excess = Math.Max(0, Remaining - AgPool);
                    }
                    AgPoolActual = 185000;

                    break;
                default:
  
                    break;

            }
            
            return result;
        }
        #endregion Allottments 
    }

}
