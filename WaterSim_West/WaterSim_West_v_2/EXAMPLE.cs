using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterSimDCDC.Generic;
using WaterSimDCDC;

namespace WaterSim_West_v_1
{
    // Big model that manages submodels and Stream model
    // Similar to WaterSimModel class
    public class Models
    {
        // StreamModel
        StreamModel FStreamModel;
        // Submodels
        List<Submodel> FSubModelList = new List<Submodel>();
        // The WaterSim Manager
        WaterSimDCDC.WaterSimManager FWSIM;
        // Parameters
        public const int epP_StreamAllocation = 5000;
        public const int ep_StreamWithdraw = 50001;
        // Construictor
        public Models(int RegionN, WaterSimDCDC.WaterSimManager TheWaterSimManager)
        {
            FWSIM = TheWaterSimManager;
            // Construct the stream model
            FStreamModel = new StreamModel(RegionN);
            // create submodels
            for (int i=0;i<RegionN;i++)
            {
                FSubModelList.Add(new Submodel());
            }

            // Create WaterSim Parameters

            // This parameter accesses the array of regional stream allocation
            StreamAllocation = new providerArrayProperty(FWSIM.ParamManager, epP_StreamAllocation, geti_StreamAllocation, eProviderAggregateMode.agSum);
            FWSIM.ParamManager.AddParameter(new ModelParameterClass(epP_StreamAllocation, "Regional Stream Allocation", "RSTALLOC", rangeChecktype.rctNoRangeCheck, 0, 0, null, StreamAllocation));

            // This parameter sets or get value from the stream model
            FWSIM.ParamManager.AddParameter(new ModelParameterClass(ep_StreamWithdraw, "Stream Withdrawal", "STRWD", rangeChecktype.rctCheckRange, 0, 100000000, geti_StremWithdraw, seti_StreamWithdraw, null));
        }

        // Allocation Parameter, read only
        public providerArrayProperty StreamAllocation;
        int[] geti_StreamAllocation()
        {
            return FStreamModel.RegionAllocation();
        }

        // Withdraw parameter
        void seti_StreamWithdraw(int value)
        {
            FStreamModel.seti_Withdraw(value);
        }
        int geti_StremWithdraw()
        {
            return FStreamModel.geti_Withdraw();
        }


        // Run all the models stream and sub
        public void RunModels ()
        {
            // run the stream model
            FStreamModel.RunModel();
            // get the stream allocation for all regions
            int[] ModelAllocation = FStreamModel.RegionAllocation();
            int index = 0;
            foreach(Submodel SM in FSubModelList)
            {
                // Sets each modesl allocation
                SM.Set_Allocation(ModelAllocation[index]);
                index++;
                // now run the submodels
                SM.RunSubModel();
            }
        }
    }

    // Sub MOdel
    // Similar to WaterSimCRFModel class
    public class Submodel
    {
        int FMyAllocation = 0;
        // Constructor
        public Submodel()
        {

        }

        //Run the sub model
        public void RunSubModel()
        {
            // dp somethings with allocation
        }

        // Set my allocation
        public void Set_Allocation(int value)
        {
            FMyAllocation = value;
        }

    }

    // stream model
    public class StreamModel
    {
        // Variables

        int FStreamFlow = 0;
        int FWithdraw = 0;
        int[] FRegionAllocation;
        // constructor
        public StreamModel(int RegionN)
        {
            FRegionAllocation = new int[RegionN];
            
        }

        // Accessors for Streamflows
        public int geti_StreamFLow()
        {
            return FStreamFlow;
        }

        public void seti_StreamFlow(int value)
        {
            FStreamFlow = value;
        }

        // Accessors for Withdraw
        public int geti_Withdraw()
        {
            return FWithdraw;
        }

        public void seti_Withdraw(int value)
        {
            FWithdraw = value;
        }

        // Access the current allocation by region
        public int[] RegionAllocation()
        {
            return FRegionAllocation;
        }

        public void RunModel()
        {
            // do something
            
            // set values for allocation of each region
        }
    }


}
