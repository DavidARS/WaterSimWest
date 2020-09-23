// /////////////////////////////////////////////////////////////////////////
// Colroado Basin Specific River Models
//
// This uses classes from "WaterSimDCDC_API_CRF_Models" 
// It builds upon this base classes to create classes that are specfic to
// the Colorado River Basin
// These classes use  thge WaterSimDCDC.Generic namespace so they are aware of all the public classes 
// used in the WestModel.
//
// Version 1.0
// Author Ray Quay
//
// /////////////////////////////////////////////////////////////////////////
namespace WaterSimDCDC.Generic
{
    public interface ISurfaceWaterInput
    {
        double ClimateChangeTarget { get; set; }
        int DroughtActive { get; set; }
        BasinWater InitialWater { get; set; }
        double SurfaceGoal { get; set; }
        int YearsOfScenario { get; set; }
    }
}