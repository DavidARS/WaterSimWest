using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WaterSimDCDC
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Water simulation manager class. </summary>
    ///
    /// <seealso cref="System.IDisposable"/>
    ///-------------------------------------------------------------------------------------------------

    public abstract class WaterSimManagerClass : IDisposable
    {
        static protected bool _isWaterSimInstatiated = false;  // used to keep track if a WAterSimManager object has been constructed
        
        // Version Strings
        const string _APIVersion = "11.0.0";  // latest version of API
        string FAPIVersion = _APIVersion;

        //====================================================================
        // The API Managers
        //====================================================================

        internal ParameterManagerClass _pm;
        internal ProcessManager _ProcessManager;
        internal ProcessRegistry _ProcessRegistry;

        //====================================================================
        //  Simulation Control Variables
        //====================================================================

        internal const int FDefaultStartYear = 2015;
        //internal const int FDefaultStopYear = 2050;
        internal const int FDefaultStopYear = 2064;

        ///-----------------------------------------------------------
        /// <summary>
        /// FLag used to lock access to the model parameters once model is running
        /// </summary>
        protected bool FModelLocked = false;

        ///-----------------------------------------------------------
        /// <summary>
        ///  used to keep track if Simulation has been initialized  False means not yet or stopped
        /// </summary>
        
        protected bool _simulationStarted = false;    

        ///-----------------------------------------------------------
        /// <summary>
        /// The First year of the simulation, such as 2010
        /// </summary>
        
        protected int _StartYear = 0;

        ///-----------------------------------------------------------
        /// <summary>
        /// The last year of the simulation, such as 2050
        /// </summary>

        protected int _EndYear = 0;                   
        
        ///-----------------------------------------------------------
        /// <summary>
        /// Used to keep track of the Current Year that will run in the simulation
        /// </summary>

        protected int _CurrentYear = 0;               // used to keep track of years inside run routines

        ///-----------------------------------------------------------
        /// <summary>
        /// Used to keep track of the total years for simulation
        /// </summary>

        protected int _NumberOfYears = 0;             

        ///-----------------------------------------------------------
        /// <summary>
        /// used to keep track if model runs have started
        /// </summary>
       
        protected bool _inRun = false;

        // private fields
        protected string _TempDirectoryName;
        protected string _DataDirectoryName;

        //============================================================================================
        // METHODS
        // ===============================================================================================
        
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <exception cref="WaterSim_Exception">   Thrown when watersim_. </exception>
        ///-------------------------------------------------------------------------------------------------

        public WaterSimManagerClass(string DataDirectoryName, string TempDirectoryName)
        {
            // Some Basic Tests
            if (_isWaterSimInstatiated) throw new WaterSim_Exception(WS_Strings.wsOnlyOneObject);
            
            // Good to go, set initiated to true;
            _isWaterSimInstatiated = true;
            
            // Set the Version string
            Assembly asm = Assembly.GetExecutingAssembly();
            FileInfo fi = new FileInfo(asm.Location);
            DateTime LastWrite = fi.LastWriteTime;
            FAPIVersion = _APIVersion + ":" + LastWrite.ToString("M/d/y H:mm");

            _StartYear = DefaultStartYear();
            _EndYear = DefaultStopYear();

            // Setup Parameter Manager and Model Paramters
           
            // Create a ParameterManager
            _pm = new ParameterManagerClass(API_Version, Model_Version);
            // Ok create Process Manager
            _ProcessManager = new ProcessManager();
            // OK create a Process Registry
            _ProcessRegistry = new ProcessRegistry();

            _TempDirectoryName = TempDirectoryName;
            _DataDirectoryName = DataDirectoryName;
            //initialize_ModelParameters();
                        
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Dispose of this object, cleaning up any resources it uses. </summary>
        ///
        /// <param name="disposing">    true if resources should be disposed, false if not. </param>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void Dispose(bool disposing)
        {
            WaterSimManager._isWaterSimInstatiated = false;
            if (disposing)
            {
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Performs application-defined tasks associated with freeing, releasing, or
        ///             resetting unmanaged resources. </summary>
        ///
        /// <seealso cref="System.IDisposable.Dispose()"/>
        ///-------------------------------------------------------------------------------------------------

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Tests if a WaterSimManager Object can be instantiated
        /// </summary>
        /// <returns> True if new object can be instantiated, false of an object already exists</returns>
        
        static public bool isReadyToInstantiate
        {
            get { return (!_isWaterSimInstatiated); }
        }

        //Error CHecking

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether model has been setup Correctly. </summary>
        ///
        /// <value> true if valid model, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool ValidModel
        {
            get { return ValidModelCheck(); }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Check if Model Setup Correct. </summary>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected virtual bool ValidModelCheck()
        {
            return true;
        }
        //************************************************************
        // 
        //  MANAGERS
        //  
        //************************************************************
         
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Provides access to the Parameter Manager
        /// </summary>
        ///<remarks> This object is used to manage the model parameters, <see cref="ParameterManagerClass"/></remarks>
        /// <seealso cref="ParameterManagerClass"/>
        /// <seealso cref="ModelParameterClass"/>
        public ParameterManagerClass ParamManager { get { return _pm; } }

        // ------------------------------------------- 
        /// <summary>
        /// Provides access to the Process Manager
        /// </summary>
        /// <remarks>The process manager manages the pre and post processes of yearly Simulations <see cref="ProcessManager"/></remarks>
        /// <seealso cref="ProcessManager"/>
        /// <seealso cref="AnnualFeedbackProcess"/>

        public ProcessManager ProcessManager { get { return _ProcessManager; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Provides access to a registry of available AnnualFeedbackProcess classes. </summary>
        ///
        /// <value> The process registry. </value>
        /// <seealso cref="ProcessRegistry"/>
        /// <seealso cref="ProcessManager"/>
        /// <seealso cref="AnnualFeedbackProcess"/>
        ///-------------------------------------------------------------------------------------------------

        public ProcessRegistry ProcessRegistry { get { return _ProcessRegistry; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the pathname of the data directory. </summary>
        ///
        /// <value> The pathname of the data directory. </value>
        ///-------------------------------------------------------------------------------------------------

        public string DataDirectory
        {
            get { return _DataDirectoryName; }
            set { _DataDirectoryName = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the pathname of the temporary directory. </summary>
        ///
        /// <value> The pathname of the temporary directory. </value>
        ///-------------------------------------------------------------------------------------------------

        public string TempDirectory
        {
            get { return _TempDirectoryName; }
            set { _TempDirectoryName = value; }
        }
        //********************************************************
        // 
        // VERSIONS
        // 
        //********************************************************
         
        // ------------------------------------------- 
        /// <summary>
        /// Version of the API interface
        /// </summary>
        public string API_Version 
        { 
            get 
            {
                return FAPIVersion;
            } 
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the model version. </summary>
        ///
        /// <value> The model version. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Model_Version
        {
            get { return GetModelVersion(); }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the model version. </summary>
        ///
        /// <returns>   The model version. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected virtual string GetModelVersion()
        {
            return "0.0.0";
        }

        //******************************************************************
        // 
        // SIMULATION CONTROL
        // 
        //***************************************************************

        #region Simulation Control

        //---------------------------------------------------------
        /// <summary>
        /// IEnumerable for simulationYears()
        /// </summary>
        /// <returns>a year from range of Simulation_Start_Year to Simulation_End_Year</returns>
        public IEnumerable<int> simulationYears()
        {
            for (int i = Simulation_Start_Year; i <= Simulation_End_Year; i++)
            {
                yield return i;
            }
        }
          
 
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the default start year. </summary>
        ///
        /// <returns>  An Int </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual int DefaultStartYear()
        {
            return FDefaultStartYear;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default maximum start year. </summary>
        ///
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual int DefaultMaxStartYear()
        {
            return FDefaultStopYear-1;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the default stop year. </summary>
        ///
        /// <returns>  An Int </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual int DefaultStopYear()
        {
            return FDefaultStopYear;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default minimum stop year. </summary>
        ///
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual int DefaultMinStopYear()
        {
            return FDefaultStartYear+1;
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exposes to inherited classes the StartYear field (int) that tracks the Start Year during Simulation Runs
        /// </summary>

        protected int Sim_StartYear
        { 
            get { return _StartYear; }
            set { _StartYear = value; }
        }

        // ------------------------------------------- 
        /// <summary>
        /// Exposes to inherited class for EndYear field (int) that tracks the End Year during Simulation Runs
        /// </summary>
        protected int Sim_EndYear
        { 
            get { return _EndYear; }
            set { _EndYear = value; }
        }

        // ------------------------------------------- 
        /// <summary>
        /// Exposes to inherited class for read only the private CurrentYear field (int) that tracks the Current Year during Simulation Runs
        /// </summary>
        protected int Sim_CurrentYear
        { get { return _CurrentYear; } }
               
        //----------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Property Locks and unlocks (true false) the simulation so no parameters can be set until unlocked.  
        /// </summary>
        /// <remarks>I started with this and then moved to LockSimulation() and UnlockSimulation(), may remove this at some point.</remarks>
        /// <seealso cref="UnLockSimulation"/>
        /// <seealso cref="LockSimulation"/>

        public bool SimulationLock
        // Sets Model Locl
        {
            get { return FModelLocked; }
            set { FModelLocked = value; }
        }

        //----------------------------------------------------------------------------------------------------
        /// <summary>
        /// Locks the simulation so Base Input parameters can be set until unlocked. 
        /// </summary>
        /// <seealso cref="UnLockSimulation"/>
        public void LockSimulation() { FModelLocked = true; }

        //----------------------------------------------------------------------------------------------------

        /// <summary>   Unlock simulation so Base Input parameters can be set . </summary>
        /// <seealso cref="LockSimulation"/>

        public void UnLockSimulation() { FModelLocked = false; }

        //----------------------------------------------------------------------------------------------------
        
        internal bool isLocked() { return FModelLocked; }
        
        //----------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Must be called to setup a Simulation
        ///             All simulations should be stopped with StopSimulation(), which will make sure
        ///             all files are closed
        ///             </summary>
        /// <remarks>   Sets SimulationLock to false, _inRun to false, runs ProcessInitializeAll(), and 
        ///             _simulationStarted to true
        ///             override methods should use following structure
        ///                protected override Simulation_Initialize()
        ///                {
        ///                   callmodelinitilaization();
        ///                   base.Simulation_Initialize()
        ///                   checkformodelerror()
        ///                }             
        /// </remarks>
        ///-------------------------------------------------------------------------------------------------
        
        public virtual void Simulation_Initialize()
        /* -------------------------------------------
        * Initializes a Simulation.   
        * All simulations should be stopped with StopSimulation(), which will make sure all files are closed
        * --------------------------------------------*/
        {

            // check to see if this was initialized before model stopped running
            if (_simulationStarted) { Simulation_Stop(); }

            // first unlock the simulation so parameters can be setup
            SimulationLock = false;
            
            // Not yet running
            _inRun = false;
            
            // reset Incase there was an error last run
            _inSimulationAllYears = false;
            _instopSimulation = false;
            _simulationStarted = false;

            // set the current year to 0 becuase nothing is running
            _CurrentYear = 0;
            
            // Initialize all Processes 
            _ProcessManager.ProcessInitializeAll(this);

            // tell API we are ready to start
            _simulationStarted = true;
            
            // Ready but not yet running
        }

        //----------------------------------------------------------------------------------
        bool _instopSimulation = false;


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Stops simulation. </summary>
        ///
        /// <remarks>   Unlocks simulation, unlocks base input parameters. </remarks>
        ///-------------------------------------------------------------------------------------------------
        
        public virtual void Simulation_Stop()
        {
            if (!_instopSimulation)
            {
                _instopSimulation = true;
                _ProcessManager.StopProcessAll(this);

                 // clean up after simulation run
                Simulation_Cleanup(); 
                                 
                                                               
                // Unset lock, recurrence, and in simulation flags
                FModelLocked = false;
                _inRun = false;
                _simulationStarted = false;
                _instopSimulation = false;
            }
        }

        /// <summary>   Simulation cleanup. </summary>
        protected abstract void Simulation_Cleanup();

        
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the year operation. </summary>
        ///
        /// <remarks>   This is a protected routine, not meant for public use, all routines use this to
        ///             run one year of the model
        ///             This does no error checking, model should be locked before it is called, errors checked after it is called.
        ///             This is the heart if the model.  This is how the model is actually executed, so all derived classes (ie models) must
        ///             override this to run the model.
        ///              </remarks>
        ///
        /// <param name="year"> The year. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected virtual bool runYear(int year)
        {
            bool testrunbool = false;

            int testrun = RunModelYear(year);
            testrunbool = (testrun==0);
            if (!testrunbool)
            {
                // set error
            }
            return testrunbool;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Posts the data. </summary>
        ///
        /// <remarks>A routime, used in the Simulation_AllYears to initiate any post model run actions after a year has been run
        /// </remarks>
        /// <returns>   An int. 0 if Posting Was Error free, > 0 if there was an error, should be error code</returns>
        ///-------------------------------------------------------------------------------------------------

        protected virtual int PostData(int year, WaterSimManagerClass TheWSManager)
        {
            return 0;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the model year operation. </summary>
        ///<remarks>  This is an abstract class that must be defined in WaterSimManagerClass.  This runs the model for the designated year and returns an error
        ///           error code as an int, 0 means no error, oitherwise and error occurred and the value return is a code for that error.  
        ///           NO EXCEPTIONS should be thrown</remarks>
        /// <param name="year"> The year. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected abstract int RunModelYear(int year);


        //----------------------------------------------------------------------------------------------------
        bool _inSimulationNextYear = false;

        /// <summary>
        /// Runs the next year in a series of years in a simulation, no pre or post process is evoked 
        /// </summary>
        /// <returns>The year of the simulation run</returns>

        public virtual int Simulation_NextYear()
        /* --------------------------------------------------------
        * Returns the year run, if has already run last year, returns 0;
        * Calls close files afer running last year
        ----------------------------------------------------------- */
        {
            //
            int runyear = 0;
            if ((_simulationStarted) & (!_inSimulationNextYear))
            {
                // Keep from rentry
                _inSimulationNextYear = true;
                // Save Lock State and lock this down
                bool locked = isLocked();
                LockSimulation();
                // Check if this is the first year
                if (!_inRun)
                {
                    // OK We are starting, set the STart and End years
                    _StartYear = _CurrentYear = Simulation_Start_Year;
                    _EndYear = Simulation_End_Year;
                    _NumberOfYears = (_EndYear - _StartYear) + 1;

                    _inRun = true;
                }
                else
                // OK, Not the first yeat, inc the year;
                { _CurrentYear++; }
                // OK we should never get to this code, but to be safe, check of we have already called the last year, if so, lets shut it down
                if (_CurrentYear > _EndYear)
                {

                    _simulationStarted = false;
                    runyear = 0;
                }
                else
                {
                    //OK run a year
                    // do Process Start for first year and PreProcess for all other years
                    // Unlock to allo parmeters to be set
                    UnLockSimulation();
                    if (_StartYear != _CurrentYear)
                    {
                        _ProcessManager.PreProcessAll(_CurrentYear, this);
                    }
                    else
                    {
                        _ProcessManager.ProcessStartedAll(_CurrentYear, this);
                    }
                    // Lock it back up
                    LockSimulation();
                    // OK run Model
                    runYear(_CurrentYear);
                    //// Set STartSimulatio to false
                    //_ws.StartSimulation = false;
                    // do PostProcess
                    // Simulation is locked for this, ie can not change parameters
                    _ProcessManager.PostProcessAll(_CurrentYear, this);
                    // Return the year just done
                    runyear = _CurrentYear;
                }
                SimulationLock = locked;
            }
            _inSimulationNextYear = false;
            return runyear;
        }

        //----------------------------------------------------------------------------------
        // das august 2021
        bool _inSimulationAllYears = false;
        int FDataPostingResult = 0;
        /// <summary>
        /// Runs all years of the specified simulation
        /// </summary>
        /// <remarks>Runs each year.  Before each year, except the first, it calls a preprocess processes.  After each year, including the last it calls all postprocess processes.  No data is retained.  If a post process does not collect data fater each year, the only output data available will be that for the last year run.</remarks>
        public virtual void Simulation_AllYears()
        {

            if ((_simulationStarted) && (!_inSimulationAllYears))
            {
                _inSimulationAllYears = true;
                _StartYear = Sim_StartYear;
                _CurrentYear = Sim_StartYear;// Simulation_Start_Year;
                _EndYear = Sim_EndYear;// Simulation_End_Year;
                _inRun = true;
                for (int year = _StartYear; year <= _EndYear; year++)
                //foreach (int year in simulationYears())
                {    // do PreProcess, unless first year then do ProcessStarted
                    // Unlock things first
                    UnLockSimulation();
                    if (_StartYear != _CurrentYear)
                    {
                        _ProcessManager.PreProcessAll(_CurrentYear, this);
                    }
                    else
                    {
                        _ProcessManager.ProcessStartedAll(_CurrentYear, this);
                    }
                    // lock it back up
                    LockSimulation();
                    runYear(year);

                    // do PostProcess
                    _ProcessManager.PostProcessAll(_CurrentYear, this);

                    // Ok, final actions, like posting data if needed
                    FDataPostingResult = PostData(_CurrentYear, this);
                    // OK, inc the year;
                    _CurrentYear++;
                }
                Simulation_Stop();
                _inSimulationAllYears = false;
            }
        }

        #endregion Simulation Control



        //=============================================================
        //
        // PARAMETERS
        //
        //=============================================================
         
         
        //---------------------------------------
        /// <summary>
        /// Simulation_Start_Year
        /// </summary>
        /// <value>The first year of the Simulation.</value>
        /// <remarks> The first year of the Simulation. Cannot be set after Simulation starts.</remarks>
        /// <exception cref="WaterSim_Exception">if setting a value that does not pass the range check</exception>

        public virtual int Simulation_Start_Year
        {
            set
            {
                if ((!_inRun) & (!FModelLocked))
                {
                    _pm.CheckBaseValueRange(eModelParam.epSimulation_Start_Year, value);
                    _StartYear = value;
                }
            }
            get { return Sim_StartYear; }// Sim_Simulation_Start_Year; }    // 
        }

        //---------------------------------------
        //End year of simulation	SimulationEnd
        // Cannot be set while simulation in progress
        // Using shadow value _Simulation_End_Year;  no get in WaterSimU

        /// <summary>   Gets or sets the simulation end year. </summary>
        /// <value> The simulation end year. </value>
        ///<remarks> The last year of the Simulation. Range = 2012 to 2085  Cannot be set after Simulation starts.</remarks>
        /// <exception cref="WaterSim_Exception">if setting a value that does not pass the range check</exception>

        public virtual int Simulation_End_Year
        {
            set
            {
                if ((!_inRun) & (!FModelLocked))
                {
                    _pm.CheckBaseValueRange(eModelParam.epSimulation_End_Year, value);
                    _EndYear = value;
                }
                // ELSE do we throw an exception? No Just document that it is ignored
            }
            get { return Sim_EndYear; }// _Simulation_End_Year; }
        }


        private int geti_Simulation_Start_Year() { return Simulation_Start_Year; }
        private void seti_Simulation_Start_Year(int value) { Simulation_Start_Year = value; }

        private int geti_Simulation_End_Year() { return Simulation_End_Year; }
        private void seti_Simulation_End_Year(int value) { Simulation_End_Year = value; }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes the model parameters. </summary>
        protected virtual void initialize_ModelParameters()
        {
            _pm.AddParameter(new ModelParameterClass(eModelParam.epSimulation_Start_Year, "Simulation Start Year", "STARTYR", rangeChecktype.rctCheckRange, DefaultStartYear(), DefaultMaxStartYear(), geti_Simulation_Start_Year, seti_Simulation_Start_Year, RangeCheck.NoSpecialBase));
            _pm.AddParameter(new ModelParameterClass(eModelParam.epSimulation_End_Year, "Simulation End Year", "STOPYR", rangeChecktype.rctCheckRange, DefaultMinStopYear() + 1, DefaultMaxStartYear() + 1, geti_Simulation_End_Year, seti_Simulation_End_Year, RangeCheck.NoSpecialBase));

        }

        protected abstract void initialize_ExtendedDocumentation();
    }
}
