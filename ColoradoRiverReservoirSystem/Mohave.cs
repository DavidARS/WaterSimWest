using System;
using System.IO;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using COReservoir_Base;

// EDIT QUAY 9/10/20
// Restored CORiverModel namespace
namespace CORiverModel
//namespace WaterSimDCDC.Generic
// END EDit

{
    public class Mohave : COReservoirs
    {
        #region constructors
        public Mohave(string DataDirectoryName)
        {

        }
        //
        #endregion constructors
        //
        #region methods
        public override void Initialize()
        {
            throw new NotImplementedException();
        }
        //
        public override void UpStream(int year)
        {
            throw new NotImplementedException();
        }
        // 
        public override void DownStream(int year)
        {
            throw new NotImplementedException();
        }
        //
        public override void Flows(int year)
        {
            throw new NotImplementedException();
        }
        //
        public override void ModifyFlows(int year)
        {
            throw new NotImplementedException();
        }
        // 
        public override void SelectUBuse(int year)
        {
            throw new NotImplementedException();
        }
        //
        public override void Reservoirs(int year)
        {
            throw new NotImplementedException();
        }
        //
        public override void FutureState(int year)
        {
            throw new NotImplementedException();
        }
        #endregion methods
    }
}
