using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORiverModel;
// EDIT QUAY 9/10/20
// REMOVED REFERENCE TO WATERSIM NAMESPACE
// ADDED WaterSim Class Objects
//using WaterSimDCDC.Generic;
// END EDIT 9/10/20


namespace CORiverDesignations
{
    public class IndianMunicipalAg : BaseConveyance
    {
         COConveyance_AZ COV;
        double Fconveyance;
        public IndianMunicipalAg(string FileID)
        {
            COV = new COConveyance_AZ(FileID);
        }

        public IndianMunicipalAg(string FileID, double conveyance) {
            Fconveyance = conveyance;
            COV = new COConveyance_AZ(FileID);
          }
        //
        public override void Initialize()
        {

         }

        // Properties
        // =====================================================================
        #region properties
        double _Conveyance = 0;
        public double Conveyance
        {
            set { _Conveyance = value; }
            get { return _Conveyance; }
        }
        //
        bool _WaterForTheDelta = false;
        public bool WaterForTheDelta
        {
            set { _WaterForTheDelta = value; }
            get { return _WaterForTheDelta; }
        }
        // =====================
        // allocations
        // ------------------------
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
            set { _Excess = value;}
            get { return _Excess; }
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
        double _LossNIA = 0;
        public double ProportionalLossNIA
        {
            set {_LossNIA =  value; }
            get { return _LossNIA; }
        }
        double _LossMandI = 0;
        public double ProportionalLossMandI
        {
            set { _LossMandI = value ; }
            get { return _LossNIA; }
        }
        bool _AZalone = true;
        public bool AZdeltaBurden
        {
            set { _AZalone = value; }
            get { return _AZalone; }
        }
        double _COdeltaWater_AZ = 0;
        public double COdeltaWaterAZ
        {
            set { _COdeltaWater_AZ = value; }
            get { return _COdeltaWater_AZ; }
        }
        double _DeltaWaterReleased = 1;
        public double ProportionalDeltaWater
        {
            set { _DeltaWaterReleased = value; }
            get { return _DeltaWaterReleased; }
        }
        #endregion CO delta properties
        // ===============================
        // CO water Designation Properties
        #region CO water designations
        double _CreditsTransferedFromAg = 0;
        public double CreditsTransferedFromAg
        {
            set { _CreditsTransferedFromAg = value; }
            get { return _CreditsTransferedFromAg; }
        }
        double _AgCAPcredits= 0;
        public double AgCAPcredits
        {
            set { _AgCAPcredits = value; }
            get { return _AgCAPcredits; }
        }
        double _PropCreditsToTransferAg = 0;
        public double PropCreditsToTransferAg
        {
            set { _PropCreditsToTransferAg = value; }
            get { return _PropCreditsToTransferAg; }

        }

        double _FallowAg = 0;
        public double FallowAgProportion
        {
            set { _FallowAg = value; }
            get { return _FallowAg; }

        }
        //
        double _CAPneeded = 0;
        public double CAPneeded
        {
            set { _CAPneeded = value; }
            get { return _CAPneeded; }
        }
        double _CAPdesiredFour = 0;
        public double CAPdesired_IV
        {
            set { _CAPdesiredFour = value; }
            get { return _CAPdesiredFour; }
        }
        double _CAPdesiredFive = 0;
        public double CAPdesired_V
        {
            set { _CAPdesiredFive = value; }
            get { return _CAPdesiredFive; }
        }
        double _CAPdesiredSix = 0;
        public double CAPdesired_VI
        {
            set { _CAPdesiredSix = value; }
            get { return _CAPdesiredSix; }
        }
        double _RealizedCAP = 0;
        public double RealizedCAP
        {
            set { _RealizedCAP = value; }
            get { return _RealizedCAP; }
        }
        double _WaterBankCAP = 0;
        public double WaterBankCAP
        {
            set { _WaterBankCAP = value; }
            get { return _WaterBankCAP; }
        }

        #endregion CO water designations
        #endregion properties
      
        // ====================================================================== 
        //
        // Methods
        //
        #region allottments
        //public override void Allottments(int year, double cap)
        public override void Allottments(int year, double cap)
        {
           // double cap = Conveyance;
            double remaining = 0;
            //Excess = 0;
            //AgPool = 0;
            double CAP = cap * (1 / Constants.acreFeetToMAF);
            //
            if (year <= 2043)
            {
                // current configuration
                // ====================
                if (CAP < Constants.p3_threshold)
                {
                    AkChinP3 = (Constants.p3_AkChin / Constants.p3_threshold) * cap;
                    CitiesP3 = cap - AkChinP3;
                }
                else
                {
                    AkChinP3 = Constants.p3_AkChin;
                    CitiesP3 = Constants.p3_7cities;
                    remaining = Math.Max(0, CAP - (AkChinP3 + CitiesP3));
                    //  CAP water > 68400 and <=853,079 AcF  - 
                    if (CAP < Constants.p4_threshold)
                    {
                        AkChinIndianPriority = (1 - (Constants.p4_indianPercent * 0.01)) * remaining;
                        CitiesMandIPriority = remaining - AkChinIndianPriority;
                    }
                    //CAP water is between 853,079 and 1,050,302 AcF
                    else
                    {
                        AkChinIndianPriority = Constants.pIndian_total;
                        CitiesMandIPriority = Constants.pMandI_total;
                        //
                        remaining -= Math.Max(0, AkChinIndianPriority + CitiesMandIPriority);
                        //pNIA_threshold = 1050302;
                        if (CAP < Constants.pNIA_threshold)
                        {
                            TribalPriority_NIA = (Constants.pNIA_indian + ((Constants.pNIA_capMinIPW * 0.01) * remaining));
                            MandIPriority_NIA = remaining - TribalPriority_NIA;
                        }
                        // up to this point, 1415000 acre-feet
                        else
                        {
                            TribalPriority_NIA = Constants.tribalNIA_total;
                            MandIPriority_NIA = Constants.mandINIA_total;
                            remaining -= Math.Max(0, TribalPriority_NIA + MandIPriority_NIA);
                            if (remaining < Constants.pAgPool_total)
                            {
                                AgPool = remaining;
                            }
                            else
                            {
                                AgPool = Constants.pAgPool_total;
                                Excess = Math.Max(0, remaining - AgPool);
                            }
                            AgPoolActual = 185000;
                        }
                    }

                }
            }
            else
            {

            }
            //
            CAPNIA = MandIPriority_NIA + AgPool + Excess;
            CAPMandI = CitiesP3 + CitiesMandIPriority;
            //
        }
        #endregion allotments
        //
        #region Delta
        double proportionalShareAZ = 0;
        internal void COdeltawater()
        {
             ProportionalLossNIA = Math.Min(1, CAPNIA / (Constants.mandINIA_total + AgPoolActual));
            ProportionalLossMandI = Math.Min(1, CAPMandI / Constants.pMandI_total);

            double dropInBurdan = ProportionalLossNIA * ProportionalLossMandI;
            //double az = 2.8; 
            // NOTE: using constants here. Actual upper Basin deliveries not met at 7.5 MAF... more like 5.0 MAF
            proportionalShareAZ = Constants.allocation_az  / (Constants.allocation_LowerBasin+ Constants.allocation_UpperBasin );
            double AZnormalShare = proportionalShareAZ * Constants.deltaBurden; // 158088 acre-feet year-1 * az proportional share of CO river water
            if (AZdeltaBurden)
            {
                COdeltaWaterAZ = dropInBurdan * ProportionalDeltaWater *  Constants.deltaBurden;
            }
            else
            {
                COdeltaWaterAZ = dropInBurdan * proportionalShareAZ * Constants.deltaBurden;
            }
        }
        internal void CAPwaterDelta()
        {
            double NIAdifference = 0;
            if (WaterForTheDelta)
            {
                // Perhaps this should change....? i.e., take also from P4?
                if (COdeltaWaterAZ < CAPNIA)
                {
                    CAPNIA = Math.Max(0, CAPNIA - COdeltaWaterAZ);
                }
                else
                {
                    NIAdifference = COdeltaWaterAZ - CAPNIA;
                }
            }
            CAPMandI = Math.Max(0, CAPMandI - NIAdifference);
        }

        #endregion Delta
        //
        #region partition
        double CAP_five = 0;
        double usedCAP_four = 0;
        double usedCAP_five = 0;
        double usedCAP_six = 0;
        double usedCAP_ag = 0;
        internal void DesignationsPartition()
        {
            double ratioCAPfour = Constants.CAP_one / (Constants.CAP_one + Constants.CAP_two);
            
             double CAP_six = AgPool;
             CAP_five = MandIPriority_NIA;
            //
            if(0 < CreditsTransferedFromAg)
            {
                  double max4plus5 = Constants.CAP_one + Constants.CAP_two;
                CAPdesired_IV = Math.Min(Constants.CAP_one, CAPneeded * ratioCAPfour);
                CAPdesired_V = Math.Min(Constants.CAP_two, CAPneeded * (1 - ratioCAPfour));
                if(max4plus5 < CAPneeded) { CAPdesired_VI = Math.Min(CreditsTransferedFromAg, CAPneeded-(max4plus5)); }
            }
            else
            {
                // This does not seem correct.....
                CAPdesired_IV = Math.Min(Constants.CAP_one, CAPneeded * ratioCAPfour);
                CAPdesired_V = Math.Min(Constants.CAP_two, CAPneeded * (1 - ratioCAPfour));
                CAPdesired_VI = 0;
            }
            if (0 < CreditsTransferedFromAg)
            {
                if(0 < CAPdesired_VI)
                {
                    usedCAP_six = Math.Min(CAPdesired_VI, AgPool);
                    CAP_six = Math.Max(0, CAP_six - usedCAP_six); 
                }
                if(0 < CAP_six)
                {
                    usedCAP_ag = usedCAP_six;
                }
                else
                {

                }
            }
            double demandMinus = 0;
            double available_IV = CAPMandI; // Adding priority three into CAP M&I here
            double lossPotential_IV = 0;
            double lossPotential_V = 0;
            double unused_IV = 0;
            double unused_V = 0;
            double potentialWaterBank = 0;
            if(CAPdesired_IV <= Constants.CAP_one)
            {
                demandMinus = CAPdesired_IV - available_IV;
                if(0 < demandMinus)
                {
                    lossPotential_IV = (CAPdesired_IV - available_IV);
                    usedCAP_four = available_IV;           
                }
                else
                {
                    potentialWaterBank = available_IV - CAPdesired_IV;
                    usedCAP_four = CAPdesired_IV;
                    unused_IV = potentialWaterBank;
                }

            }
            double available_V = 0;
            available_V = CAP_five * (Constants.CAP_two / (Constants.CAP_one + Constants.CAP_two));

            if (CAP_five <= Constants. CAP_two)
            {
                if(CAPdesired_V <= Constants.CAP_two)
                {
                    demandMinus = CAPdesired_V - available_V;
                    if(0 < demandMinus)
                    {
                        lossPotential_V = CAPdesired_V - available_V;
                        usedCAP_five = available_V;
                    }
                    else
                    {
                        potentialWaterBank = potentialWaterBank + (available_V - CAPdesired_V);
                        usedCAP_five = CAPdesired_V;
                        unused_V = potentialWaterBank;
                    }
                }

            } // start here
            else
            {
                if(0 < Constants.CAP_two)
                {

                }
                if(CAPdesired_V <= Constants.CAP_two)
                {
                    demandMinus = CAPdesired_V - available_V;
                    if(0 < demandMinus)
                    {
                        lossPotential_V = CAPdesired_V - available_V;
                        usedCAP_five = available_V;
                    }
                    else
                    {
                        potentialWaterBank = potentialWaterBank + (available_V - CAPdesired_V);
                        usedCAP_five = CAPdesired_V;
                        unused_V = potentialWaterBank;
                    }
                }
            }
            RealizedCAP = usedCAP_four + usedCAP_five + usedCAP_six;
            WaterBankCAP = unused_IV+ unused_V;

         }
        //
        #endregion partition
        //
        // allocate
       public override void Allocate(int year, double cap)
        {
            //Allottments(year, cap);
            COV.Allottments(year, cap);
            COdeltawater();
            CAPwaterDelta();
            DesignationsPartition();
        }

        // =====================================================================
        //
        // Functions
        //
        internal double CalculateAgTransferCredits()
        {
            double result = 0;
            AgCAPcredits = Math.Max(0, Math.Min(Constants.CAP_agSurface, Constants.CAP_agSurface * PropCreditsToTransferAg));
            CreditsTransferedFromAg = Math.Max(0, Math.Min(AgCAPcredits, AgCAPcredits * FallowAgProportion));
            result = CreditsTransferedFromAg;
            return result;
        }

    }
}
