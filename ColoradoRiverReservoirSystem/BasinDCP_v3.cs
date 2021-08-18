using System;
using CORiverModel;

namespace CORiverDesignations
{
    public class BasinDCP : BaseConveyance
    {
        public UnitData_ICS FICS;
        //
        COConveyance_AZ COV;
        //double Fconveyance;
        public BasinDCP(string FileID)
        {
            COV = new COConveyance_AZ(FileID);
        }

        public BasinDCP(string FileID,string DataDirectoryName, UnitData_ICS FICS)
        {
           // 
            COV = new COConveyance_AZ(FileID);
            Initialize();
        }
        //
        public override void Initialize()
        {

        }
        #region allocate
        public override void Allocate(int year, int elevation, int state, double dcp)
        {
            throw new NotImplementedException();
        }
        public override void Allocate(int year, double cap)
        {
            throw new NotImplementedException();
        }
        #endregion allocate
        #region allottments
        //public override void Allottments(int year, double cap)
        public override void Allottments(int year, int elevation, int state, out double dcp)
        {
            dcp = 0;
            if(year < Constants.DCPstartYear)
            { }
            else {
                // 1 January elevations
                if (elevation <= Constants.meadTierZero)
                {
                    if (Constants.mead1045 < elevation)
                    {
                        if (state == Constants.AZ)
                        {
                            dcp = Constants.azMead192k;
                        }
                        if (state == Constants.NV)
                        {
                            dcp = Constants.nvMead8k;
                        }
                    }
                    else if (elevation <= Constants.mead1045)
                    {
                        if (state == Constants.AZ)
                        {
                            dcp = Constants.azMead240k;
                        }
                        if (state == Constants.NV)
                        {
                            dcp = Constants.nvMead10k;
                        }
                        if (state == Constants.CA)
                        {
                            if (Constants.mead1030 < elevation)
                            {
                                if (Constants.mead1035 < elevation)
                                {
                                    if (Constants.mead1040 < elevation)
                                    {
                                        dcp = Constants.caMead200k;
                                    }
                                    else
                                    {
                                        dcp = Constants.caMead250k;
                                    }
                                }
                                else
                                {
                                    dcp = Constants.caMead300k;
                                }
                            }
                            else
                            {
                                dcp = Constants.caMead350k;
                            }
                        }
                    }
                }
            }

        }
        // ---------------------------------------------------------------------------------------------
        // added on 04.29.21 after seeing the joint DCP shortage sharing presentation from Tom and Ted
        // A copy of the presentaion is available on-line
        // Data came from cap-dcp-mead-contributions-map-2019.pdf
        // ======================================================
        public override void Allottments(int year, int elevation, int state, bool update, out double dcp)
        {
            dcp = 0;
            if (year < Constants.DCPstartYear)
            { }
            else
            {
                //
                //
                // 1 January elevations
                if (elevation <= Constants.meadTierZero)
                {
                    if (Constants.mead1075 <= elevation)
                    {
                        if (state == Constants.AZ)
                        {
                            dcp = Constants.azMead192k;
                        }
                        if (state == Constants.NV)
                        {
                            dcp = Constants.nvMead8k;
                        }
                        if (state == Constants.MX)
                        {
                            dcp = Constants.mxMead41k;
                        }
                        if (state == Constants.CA)
                        {
                            dcp = 0;
                        }
                    }
                    else
                    {
                        if (Constants.mead1050 <= elevation)
                        {
                            if (state == Constants.CA)
                            {
                                dcp = 0;
                            }
                            if (state == Constants.AZ)
                            {
                                dcp = Constants.azMead512k;
                            }
                            if (state == Constants.NV)
                            {
                                dcp = Constants.nvMead21k;
                            }
                            if (state == Constants.MX)
                            {
                                dcp = Constants.mxMead80k;
                            }
                        }
                        else
                        {
                            if (Constants.mead1045 <= elevation)
                            {
                                if (state == Constants.CA)
                                {
                                    dcp = 0;
                                }
                                if (state == Constants.AZ)
                                {
                                    dcp = Constants.azMead592k;
                                }
                                if (state == Constants.NV)
                                {
                                    dcp = Constants.nvMead25k;
                                }
                                if (state == Constants.MX)
                                {
                                    dcp = Constants.mxMead104k;
                                }
                            }
                            else
                            {

                                if (Constants.mead1025 <= elevation)
                                {
                                    if (state == Constants.AZ)
                                    {
                                        dcp = Constants.azMead640k;
                                    }
                                    if (state == Constants.NV)
                                    {
                                        dcp = Constants.nvMead27k;
                                    }
                                    if (state == Constants.MX)
                                    {
                                        dcp = Constants.mxMead146k;
                                    }
                                    if (state == Constants.CA)
                                    {
                                        dcp = Constants.caMead200k;
                                    }
                                }
                                else
                                {
                                    if (state == Constants.AZ)
                                    {
                                        dcp = Constants.azMead720k;
                                    }
                                    if (state == Constants.NV)
                                    {
                                        dcp = Constants.nvMead30k;
                                    }
                                    if (state == Constants.MX)
                                    {
                                        dcp = Constants.mxMead275k;
                                    }
                                    if (state == Constants.CA)
                                    {
                                        dcp = Constants.caMead350k;
                                    }


                                }

                            }
                        }
                    }
                }
            }
        }

        // ----------------------------------------------------------------------------------------
        public override void Allottments(int year, double convey)
        {
            throw new NotImplementedException();
        }
        // ========================================================
        // 04.29.21 das
        //
        public override void Mitigation(int year, int elevation)
        {
            throw new NotImplementedException();
        }
        public override void Mitigation(int year, int elevation, int state, out double mit)
        {
            throw new NotImplementedException();
        }
        // Mitigation to agriculture and NIA pools for elevations below 1075 on Mead
        // cut of 512k to priority 5 and 6 for CAP
        public override void Mitigation(int year, int elevation, int state, int priority, string pool, out double mitigation)
        {
            mitigation = 0;
            if (year < Constants.MitigationstartYear)
            { }
            else
            {
                // 1 January elevations
                if (elevation <= Constants.meadTierZero)
                {
                    if (Constants.mead1075 <= elevation)
                    {
                    }
                    else if (elevation < Constants.mead1075)
                    {
                        if (Constants.mead1050 <= elevation)
                        {
                            if (state == Constants.AZ)
                            {
                                switch (priority)
                                {
                                    case 0:
                                        if(pool == "NIA")
                                        {
                                            mitigation = Constants.azMitigation_Tier1_NIA;
                                        }
                                        if(pool == "Ag Pool")
                                        {
                                            mitigation = Constants.azMitigation_Tier1_AgPool;
                                        }
                                        break;
                                    case 4:
                                        break;
                                    case 5:
                                        break;
                                    case 6:
                                        break;
                                }
                             }
                        }
                    }
                }
            }
        }

               
    #endregion
    }
}