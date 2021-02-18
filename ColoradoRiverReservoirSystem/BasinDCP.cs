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

        public override void Allottments(int year, double convey)
        {
            throw new NotImplementedException();
        }
         #endregion
    }
}