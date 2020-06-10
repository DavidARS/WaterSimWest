using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterSimDCDC;


namespace WaterSimDCDC
{
    public static class WaterSimManagerInfo
    {
        //public static string GetCurrentDirectory = Environment.CurrentDirectory;

        public static string GetCurrentDirectory
        {
            get { return Environment.CurrentDirectory; }
         }
    
    }
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Water simulation manager sio. </summary>
    ///
    /// <remarks>   Ray Quay, 1/7/2014. 
    ///             Runs extended watersim but only uses system Input and Output data types </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class WaterSimManager_SIO : WaterSimManager
    {

        protected SimulationResults FAnnualResults = null;
        internal bool FIncludeAggregates = false;
        //int FSimulationYears = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="DataDirectoryName">    Pathname of the data directory. </param>
        /// <param name="TempDirectoryName">    Pathname of the temporary directory. </param>
        ///-------------------------------------------------------------------------------------------------

        public WaterSimManager_SIO(string DataDirectoryName, string TempDirectoryName) : base (DataDirectoryName,TempDirectoryName)
        {

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the aggregates should be included in the Simulation results for Provider Outputs. </summary>
        ///
        /// <value> true if include aggregates, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IncludeAggregates
        {
            get { return FIncludeAggregates; }
            set { FIncludeAggregates = value; }
        }
        /// <summary>   Simulation initialize. </summary>
        public override void Simulation_Initialize()
        {
            base.Simulation_Initialize();
            FAnnualResults = null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the year operation. </summary>
        ///
        /// <param name="year"> The year. </param>
        ///-------------------------------------------------------------------------------------------------

        protected override bool runYear(int year)
        {

            bool testrun = base.runYear(year);
            if (testrun)
            {
               // EDIT QUAY 8 16 17
               // NOT SURE WHY THIS WAS COMMENTED OUT
//                fetchData(year, ref FAnnualResults);
                 fetchData(year, ref FAnnualResults);
                //                END EDIT 8 16 17
            }
            return testrun;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Posts the data. </summary>
        ///
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected override int PostData(int year, WaterSimManagerClass TheWSManager)
        {
            base.PostData(year, TheWSManager);
            fetchData(year, ref FAnnualResults);
            return 0;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the simulation next year. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public override int Simulation_NextYear()
        {
            if (Sim_CurrentYear == 0)
            {
                int NumberOfYears = (Simulation_End_Year - Simulation_Start_Year) + 1;
                initializeData(NumberOfYears, Sim_StartYear, ref FAnnualResults, FIncludeAggregates);
            }
            return base.Simulation_NextYear();

            }
        /// <summary>   Simulation all years. </summary>
        public override void Simulation_AllYears()
        {
            int NumberOfYears = (Simulation_End_Year - Simulation_Start_Year) + 1;
            initializeData(NumberOfYears, Sim_StartYear, ref FAnnualResults, FIncludeAggregates);
            base.Simulation_AllYears();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the simulation results. </summary>
        ///
        /// <value> The simulation results. </value>
        ///-------------------------------------------------------------------------------------------------

        public SimulationResults SimulationRunResults
        {
            get { return FAnnualResults; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes the data. </summary>
        ///
        /// <remarks>   Ray Quay, 1/27/2014. </remarks>
        ///
        /// <param name="Years">        The years. </param>
        /// <param name="StartYear">    the AnnualSimulationResults . </param>
        /// <param name="SimResults">   [in,out] The simulation results. </param>
        ///-------------------------------------------------------------------------------------------------

        internal void initializeData(int Years, int StartYear, ref SimulationResults SimResults)
        {
            initializeData(Years, StartYear, ref SimResults, false);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes the data. </summary>
        /// <param name="Years">                The years. </param>
        /// <param name="StartYear">            the AnnualSimulationResults . </param>
        /// <param name="SimResults">           The simulation results. </param>
        /// <param name="IncludeAggregates">    true to include, false to exclude the aggregates. </param>
        ///-------------------------------------------------------------------------------------------------

        internal void initializeData(int Years, int StartYear, ref SimulationResults SimResults, bool IncludeAggregates)
        {
            if ((SimResults != null)&&(Years == (SimResults.LastYear-SimResults.StartYear)+1))
            // if not new then zero it out from last use  Added 5 22 12
            {

                // Only supporting Aggregates in Output right now
                ProviderIntArray OutBlank = new ProviderIntArray(0,IncludeAggregates);
                ProviderIntArray InBlank = new ProviderIntArray(0, false);

                for (int yr = 0; yr < Years; yr++)
                {
                    AnnualSimulationResults TheASR = SimResults[yr];

                    int cnt = 0;
                    cnt = ParamManager.NumberOfParameters(modelParamtype.mptOutputBase);
                    for (int parmi = 0; parmi < cnt; parmi++)
                    {
                        TheASR.Outputs.BaseOutput[parmi] = 0;
                        TheASR.Outputs.BaseOutputModelParam[parmi] = -1;
                    }
                    cnt = 0;
                    cnt = ParamManager.NumberOfParameters(modelParamtype.mptOutputProvider);
                    for (int parmi = 0; parmi < cnt; parmi++)
                    {
                        TheASR.Outputs.ProviderOutput[parmi] = OutBlank;
                        TheASR.Outputs.ProviderOutputModelParam[parmi] = -1;
                    }
                    cnt = 0;
                    cnt = ParamManager.NumberOfParameters(modelParamtype.mptInputBase);
                    for (int parmi = 0; parmi < cnt; parmi++)
                    {
                        TheASR.Inputs.BaseInput[parmi] = 0;
                        TheASR.Inputs.BaseInputModelParam[parmi] = -1;
                    }
                    cnt = 0;
                    cnt = ParamManager.NumberOfParameters(modelParamtype.mptInputProvider);
                    for (int parmi = 0; parmi < cnt; parmi++)
                    {
                        TheASR.Inputs.ProviderInput[parmi] = InBlank;
                        TheASR.Inputs.ProviderInputModelParam[parmi] = -1;
                    }
                }
            }
            else
            {
                SimResults = new SimulationResults(Years, StartYear,
                                                    ParamManager.NumberOfParameters(modelParamtype.mptOutputBase),
                                                    ParamManager.NumberOfParameters(modelParamtype.mptOutputProvider),
                                                    ParamManager.NumberOfParameters(modelParamtype.mptInputBase),
                                                    ParamManager.NumberOfParameters(modelParamtype.mptInputProvider) ,
                                                    IncludeAggregates);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Fetches a data.</summary>
        /// <param name="year">       The year.</param>
        /// <param name="SimResults"> [in,out] The simulation results.</param>
        /// <see cref="SimulationResults"/>
        ///-------------------------------------------------------------------------------------------------

        internal void fetchData(int year, ref SimulationResults SimResults)
        {
            int datai = 0;
            
            int index = year - Sim_StartYear; // changed 7 26 11 _SimulationDB_Start_Year; 

            if ((index >= 0) & (index < SimResults.Length))
            {
                int BaseOutputSize = ParamManager.NumberOfParameters(modelParamtype.mptOutputBase);
                int ProviderOutputSize = ParamManager.NumberOfParameters(modelParamtype.mptOutputProvider);
                int BaseInputSize = ParamManager.NumberOfParameters(modelParamtype.mptInputBase);
                int ProviderInputSize = ParamManager.NumberOfParameters(modelParamtype.mptInputProvider);

                datai = 0;
                AnnualSimulationResults TheASR = new AnnualSimulationResults(BaseOutputSize,ProviderOutputSize,BaseInputSize,ProviderInputSize, FIncludeAggregates);
    
                foreach (ModelParameterClass MP in ParamManager.BaseOutputs())
                {
                    TheASR.Outputs.BaseOutput[datai] = MP.Value;
                    TheASR.Outputs.BaseOutputModelParam[datai] = MP.ModelParam;
                    datai++;
                }
                datai = 0;
                foreach (ModelParameterClass MP in ParamManager.BaseInputs())
                {
                    TheASR.Inputs.BaseInput[datai] = MP.Value;
                    TheASR.Inputs.BaseInputModelParam[datai] = MP.ModelParam;
                    datai++;
                }
                datai = 0;
                foreach (ModelParameterClass MP in ParamManager.ProviderOutputs())
                {
                    // OK this is a bit complicated becuase of Aggregates
                    if ((!TheASR.Outputs.AggregatesIncluded)||(MP.ProviderProperty.AggregateMode==eProviderAggregateMode.agNone))
                    {
                        TheASR.Outputs.ProviderOutput[datai] = MP.ProviderProperty.getvalues();
                    }
                    else
                    {
                        for (int i = 0; i < TheASR.Outputs.ProviderOutput[datai].Length; i++)
                        {
                            int tempVal = MP.ProviderProperty[i];
                            TheASR.Outputs.ProviderOutput[datai].Values[i] = tempVal;
                        }
                    }

                    TheASR.Outputs.ProviderOutputModelParam[datai] = MP.ModelParam;
                    datai++;
                }
                datai = 0;
                foreach (ModelParameterClass MP in ParamManager.ProviderInputs())
                {
                    // OK this is a bit complicated becuase of Aggregates
                    if ((!TheASR.Inputs.AggregatesIncluded) || (MP.ProviderProperty.AggregateMode == eProviderAggregateMode.agNone))
                    {
                        TheASR.Inputs.ProviderInput[datai] = MP.ProviderProperty.getvalues();
                    }
                    else
                    {
                        TheASR.Inputs.ProviderInput.Values[datai] = MP.ProviderProperty.getvalues();

                        //for (int i = 0; i < TheASR.Inputs.ProviderInput[datai].Length; i++)
                        //{
                        //    int tempVal = MP.ProviderProperty[i];
                        //    TheASR.Inputs.ProviderInput[datai].Values[i] = tempVal;
                        //}
                    }
                    //TheASR.Inputs.ProviderInput[datai] = MP.ProviderProperty.getvalues();
                    TheASR.Inputs.ProviderInputModelParam[datai] = MP.ModelParam;
                    datai++;
                }
                TheASR.year = year;
                SimResults[index] = TheASR;
            }
        }

    }

   
}
