using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterSimDCDC.Generic
{
    class ReservoirBaseClass
    {


    }
}

namespace COReservoir_Base
{

    public abstract class COReservoirs : IDisposable
    {
        public COReservoirs() { }

        public abstract void Initialize();
        public abstract void ModifyFlows(int year);
        public abstract void ModifyFlows(int year, int choice);
        //
        public abstract void Flows(int year);
        //
        public abstract void FutureState(int year);
        //
        public abstract void Reservoirs(int year);
        //
        public abstract void UpStream(int year);
        //
        public abstract void DownStream(int year);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            //
            if (disposing)
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}