using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemandModel_Base;

namespace WaterSim_Base
{
    /// <summary>
    /// 
    /// </summary>
    public class MyDemandModel : DemandModel
    {
        int Fyear;
        /// <summary>
        /// 
        /// </summary>
        public MyDemandModel(int year)
        {
            Fyear = year;
        }


        //public int geti_SurfaceLake(WaterSimCRFModel WSmith)
        //{
        //    int result = -1;
        //    double temp = WSmith.TheCRFNetwork.SurfaceLake.Limit;
        //    try
        //    {
        //        int tempint = Convert.ToInt32(Math.Round(temp));
        //        result = tempint;
        //    }
        //    catch (Exception ex)
        //    {
        //        // ouch
        //    }
        //    return result;
        //}

        /////-------------------------------------------------------------------------------------------------
        ///// <summary> Gets the geti surface lake From CRF_Network.</summary>
        ///// <remarks>2/18/18 Quay </remarks>
        ///// <returns> An int. -1 if conversion error.</returns>
        /////-------------------------------------------------------------------------------------------------

        //public int[] geti_SurfaceLake()
        //{
        //    int NumberOfModels = WestModel.ModelCount;
        //    int[] Values = new int[NumberOfModels];
        //    int cnt = 0;
        //    foreach (WaterSimCRFModel WSM in WestModel.WaterSimCRFModels)
        //    {
        //        Values[cnt] = geti_SurfaceLake(WSM);
        //    }
        //    return Values;
        //}





    }
}
