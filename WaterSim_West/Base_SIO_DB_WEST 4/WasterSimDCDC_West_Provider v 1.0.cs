using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterSimDCDC.Generic;
namespace WaterSimDCDC.West
{

   

    /// <summary>
    /// Provider class is one provider = State
    /// </summary>
    public class ProviderClassDynamic
    {
        UnitData FUnitData;
        int FUnitCount = 0;

        public ProviderClassDynamic(UnitData TheUnitData)
        {
            FUnitData = TheUnitData;
            FUnitCount = FUnitData.UnitCount;
            FProviderNameList = new string[FUnitCount];
            for (int i=0;i<FUnitCount;i++)
            {
                FProviderNameList[i] = FUnitData.UnitNames[i];
            }

        }
        // Provider Routines, Constants and enums
        /// <summary>
        /// The last valid provider enum value
        /// </summary>
        /// <value>eProvider enum</value>
        public const eProvider LastProvider = eProvider.eState;

        /// <summary>
        /// The first valid enum value
        /// </summary>
        /// <value>eProvider enum</value>
        public const eProvider FirstProvider = eProvider.eState;

        /// <summary>
        /// The Last valid Aggregator value
        /// </summary>
        /// <value>eProvider enum</value>
        public const eProvider LastAggregate = eProvider.eState;

        /// <summary>
        /// The number of valid Provider (eProvider) enum values for use with WaterSimModel and ProviderIntArray.
        /// </summary>
        /// <value>count of valid eProvider enums</value>
        /// <remarks>all providers after LastProvider are not considered one of the valid eProvider enum value</remarks>
  
        ///public const int NumberOfProviders = (int)LastProvider + 1;
        
        public int NumberOfProviders
        { get { return FUnitCount; } }

        /// <summary>
        /// The number of valid Provide Aggregate (eProvider) enum values.
        /// </summary>
        /// <value>count of valid eProvider enums</value>
        /// <remarks>all providers after LastProvider are not considered one of the valid eProvider enum value</remarks>
        public static int NumberOfAggregates = ((int)LastAggregate - (int)LastProvider);

        internal const int TotalNumberOfProviderEnums = ((int)LastAggregate) + 1;

        private string[] FProviderNameList; 

        private static string[] FieldNameList = new string[TotalNumberOfProviderEnums]  {      
 
            "st"
           };

        private static eProvider[] RegionProviders = new eProvider[(int)LastProvider + 1] {
            eProvider.eState
        };

        public static eProvider[] GetRegion(eProvider ep)
        {
            switch (ep)
            {
                case eProvider.eState:
                    return RegionProviders;
                default:
                    return null;
            }
        }

    }

}
