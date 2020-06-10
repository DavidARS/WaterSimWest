using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


// WEST TOOLS
// Version 1.0
// 
namespace WaterSimDCDC
{
    public static class WestTools
    {
        static void AddGroupToParameterTopics(ParameterManagerClass PM, ModelParameterGroupClass TheGroup)
        {
            foreach (int eModCode in TheGroup.ModelParameters())
            {
                try
                {
                    ModelParameterClass MP = PM.Model_Parameter(eModCode);
                    if (MP != null)
                    {
                        MP.TopicGroups.Add(TheGroup);
                    }
                }
                finally
                {

                }
            }
        }
        public static void AddWestParameterGroups(WaterSimManager WSim)
        {   
            // Create a supply group
            // epP_SurfaceFresh = 1031; epP_SurfaceLake = 1032; epP_SurfaceSaline = 1033; epP_Groundwater = 1034; 
            //  epP_Effluent = 1035;  epP_Augmented = 1036; epP_TotalSupplies = 1037;  epP_UrbanWasteWater = 1038;
            
            int[] TempSupplies = new int[8] { eModelParam.epP_SurfaceFresh, eModelParam.epP_SurfaceLake, eModelParam.epP_SurfaceSaline, eModelParam.epP_Groundwater,
              eModelParam.epP_Effluent, eModelParam.epP_Augmented, eModelParam.epP_TotalSupplies, eModelParam.epP_UrbanWasteWater };
            ModelParameterGroupClass MPGC = new ModelParameterGroupClass("Supplies",TempSupplies);
            WSim.ParamManager.GroupManager.Add(MPGC);
            AddGroupToParameterTopics(WSim.ParamManager, MPGC);

            // Create Consumer Group
            // epUrban = 1051;epAgriculture = 1052;epIndustrial = 1053;epPower = 1054; epP_TotalDemand = 1111;
            int[] TempConsumer = new int[5] { eModelParam.epP_Urban, eModelParam.epP_Agriculture, eModelParam.epP_Industrial, eModelParam.epP_Power, eModelParam.epP_TotalDemand};
            MPGC = new ModelParameterGroupClass("Consumers", TempConsumer);
            WSim.ParamManager.GroupManager.Add(MPGC);
            AddGroupToParameterTopics(WSim.ParamManager, MPGC);


        }

    }
}
