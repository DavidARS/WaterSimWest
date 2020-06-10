// ===========================================================
//     WaterSimDCDC Regional Water Demand and Supply Model Version 5.0

//       A Class the adds Feedback Process support to WaterSimDCDC.WaterSim Class

//       WaterSimDCDC_API_Process 
//       Version 4.0
//       Keeper Ray Quay  ray.quay@asu.edu
//       Copyright (C) 2011,2012 , The Arizona Board of Regents
//              on behalf of Arizona State University

//       All rights reserved.

//       Developed by the Decision Center for a Desert City
//       Lead Model Development - David A. Sampson <david.a.sampson@asu.edu>

//       This program is free software: you can redistribute it and/or modify
//       it under the terms of the GNU General Public License version 3 as published by
//       the Free Software Foundation.

//       This program is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU General Public License for more details.

//       You should have received a copy of the GNU General Public License
//       along with this program.  If not, please see <http://www.gnu.org/licenses/>.
//
//====================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using WaterSimDCDC.Documentation;



namespace WaterSimDCDC
{

    #region ProcessManager

    ///-------------------------------------------------------------------------------------------------
    /// <summary> Annual feedback Process Class </summary>
    ///-------------------------------------------------------------------------------------------------

    public class AnnualFeedbackProcess // IDisposable
    {
        internal const string ShortDescrip =  "Base: Does nothing!";
        internal const string LongDescrip = "Base: Does nothing for ProcessInitialized(), ProcessedStarted(), PreProcess() or PostProcess()";
        internal const string CodeDescrip = "BASE";
        /// <summary> Name of Process. </summary>
        protected string Fname = "";
        /// <summary> Message describing the error. </summary>
        protected string FErrorMessage = "";
        /// <summary> Information describing the process. </summary>
        protected string FProcessDescription = "";
        /// <summary> More Information describing the process. </summary>
        protected string FProcessLongDescription = "";
        /// <summary> The process code. </summary>
        protected string FProcessCode = "";
        /// <summary>   The WaterSimManager Object Process is Attached to . </summary>
        protected WaterSimManager FWsim;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor </summary>
        ///
        /// <param name="aName"> The name of the process </param>
        /// <param name="WSim"> The WaterSimManager Using the Process. </param>
        ///  <remarks> It should not be assumed that the WaterSimManager value being passed is the WaterSimManager that will make'
        /// 		   pre and post process calls</remarks>
        ///-------------------------------------------------------------------------------------------------

        public AnnualFeedbackProcess(string aName, WaterSimManager WSim)
        {
            BuildDescStrings();
            Fname = aName;
            this.Name = this.GetType().Name;
            FWsim = WSim;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor </summary>
        /// <remarks>   This is intended to be an interactive constructor where at least a prompt for a name is used 
        ///             It should not be assumed that the WaterSimManager value being passed is the WaterSimManager that will make'
        /// 		    pre and post process calls</remarks>
        /// <param name="WSim"> The WaterSimManager Using the Process. </param>
        ///-------------------------------------------------------------------------------------------------

        public AnnualFeedbackProcess(WaterSimManager WSim)
        {
            BuildDescStrings();
            this.Name = this.GetType().Name;
            FWsim = WSim;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        /// <remarks>   This is intended to be an interactive constructor where normally a dialog prompt for a name or initial values is used
        ///             to set initial values, however, if quite is true, then this prompt is not used and default values are used. 
        ///             It should not be assumed that the WaterSimManager value being passed is the WaterSimManager that will make'
        /// 		    pre and post process calls</remarks>       ///
        /// <param name="WSim">     The WaterSimManager Using the Process. </param>
        /// <param name="quiet">    true to quiet. </param>
        ///-------------------------------------------------------------------------------------------------

        public AnnualFeedbackProcess(WaterSimManager WSim, bool quiet)
        {
            BuildDescStrings();
            this.Name = this.GetType().Name;
            FWsim = WSim;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor </summary>
        ///
        /// <param name="aName"> The name of the process </param>
        /// <remarks>WaterSimManager is not known till Pre and Post Porcesses are called</remarks>
        ///-------------------------------------------------------------------------------------------------

        public AnnualFeedbackProcess(string aName)
        {
            BuildDescStrings();
            Fname = aName;
            this.Name = this.GetType().Name;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks>Uses InstanceData string to setup Object field values usinge SetupProcess()</remarks>
        /// <param name="aName">        The name of the process. </param>
        /// <param name="InstanceData"> Information describing the instance. </param>
        /// <param name="WSim">         The WaterSimManager Using the Process. </param>
        ///-------------------------------------------------------------------------------------------------

        public AnnualFeedbackProcess(string aName, string InstanceData, WaterSimManager WSim)
            {
                Fname = aName;
                SetupProcessData(InstanceData);
                FWsim = WSim;
            }       

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks>Uses InstanceData string to setup Object field values usinge SetupProcess()</remarks>
        /// <param name="aName">        The name of the process. </param>
        /// <param name="InstanceData"> Information describing the instance. </param>
        ///-------------------------------------------------------------------------------------------------

        public AnnualFeedbackProcess(string aName, string InstanceData)
            {
                Fname = aName;
                SetupProcessData(InstanceData);  
            }       
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor </summary>
        /// <remarks>   This is intended to be an interactive constructor where at least a prompt for a name is used </remarks>
        ///-------------------------------------------------------------------------------------------------

        public AnnualFeedbackProcess()
        {
            BuildDescStrings();
            Fname = Name;
            this.Name = this.GetType().Name;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Sets up the process data using InstanceData string. </summary>
        ///
        /// <remarks> THis method parce the InstanceData string to set the objects fields after a basic create</remarks>
        ///
        /// <param name="InstanceData"> Information describing the instance. </param>
        ///-------------------------------------------------------------------------------------------------

        public virtual void SetupProcessData(string InstanceData)
        {
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Performs application-defined tasks associated with freeing, releasing, or
        ///             resetting unmanaged resources. 
        ///             Best not redefine this with a custon dispose, use Clenup for that.</summary>
        ///-------------------------------------------------------------------------------------------------

        public void Dispose()
        {
            CleanUp();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Clean up. </summary>
        /// <remarks> </remarks>
        ///-------------------------------------------------------------------------------------------------

        public virtual void CleanUp()
        {

            // Do clean up here
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a description of the process class. </summary>
        /// <remarks>A Static property that returns a description of the class.  For each new derived class, this should change</remarks>
        /// <value> The description of the process class. </value>
        ///-------------------------------------------------------------------------------------------------

        static public string ProcessClassDescription
        {
            get { return ClassDescription(); }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a description of what the process will do (or is doing). </summary>
        ///
        /// <value> Information describing the process. </value>
        ///-------------------------------------------------------------------------------------------------

        public string ProcessDescription
        {
            get {
                BuildDescStrings();
                return FProcessDescription;
                }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a long description of what the process will do (or is doing). </summary>
        ///
        /// <value> Extended information describing the process. </value>
        ///-------------------------------------------------------------------------------------------------

        public string ProcessLongDescription
        {
            get {
                BuildDescStrings();
                return FProcessLongDescription;
                }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the process code. </summary>
        /// <remarks> This is a short string used to uniquely identify this process and its current parameters</remarks>
        /// <value> The process code. </value>
        ///-------------------------------------------------------------------------------------------------

        public string ProcessCode
        {
            get
            {
                BuildDescStrings();
                return FProcessCode;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Documentation </summary>
        /// <remarks> Returns a string that documents the instance of this class.  Must override for all classes</remarks>
        /// <returns> Documentation in a string </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual DocTreeNode Documentation()
        {
            DocTreeNode temp = new DocTreeNode("ANNUALFEEDBACKPROCESS", this.GetType().Name, Fname, ProcessCode, "");
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Saves info from this instance in a string. </summary>
        ///
        /// <remarks>  This instance string can be used to recreate this object using the static CreateProcess method.</remarks>
        ///
        /// <returns> . </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual string SaveInstance()
        {

            return this.GetType().Name;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds the description strings. </summary>
        /// <remarks> Override this method to build description strings for new classes and objects</remarks>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void BuildDescStrings()
        {
            FProcessDescription = ShortDescrip;
            FProcessLongDescription = LongDescrip;
            FProcessCode = CodeDescrip;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   method that is called when a Simulation is initialized. </summary>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        /// <remarks> Override this method to do some activity before the simulation starts
        /// 		  AT this point, the WSim ParamManager methods are available and set to their default values but   
        /// 		  note that none of the values of parameters from WaterSim are reliable at this point since they can 
        /// 		  all be set before the simulation actually starts, to do some activity before the first year is run, 
        /// 		  but after all values have been set, refer to ProcessStarte. </remarks>
        /// <seealso cref="ProcessStarted"/>
        ///-------------------------------------------------------------------------------------------------


        public virtual bool ProcessInitialized(WaterSimManagerClass WSim)
        {
            return true;
   
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   method that is called right before the first year of a simulation is called. </summary>
        /// <param name="year"> The year about to be run </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
       /// <returns>   true if it succeeds, false if it fails. </returns>
       /// <remarks> Year is the first year fo the simulation.  Input parameters available at this point have been set for the Simulation, 
       /// 			 however ther is output data since the model has not yet been run (ie Output values are not meaningful)</remarks>
        ///-------------------------------------------------------------------------------------------------

        public virtual bool ProcessStarted(int year, WaterSimManagerClass WSim)
        {
            return true;

        }
 
 
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method that is called before each annual run </summary>
        /// <param name="year"> The year about to be run </param>
        /// <param name="WSim"> The WaterSimManager that is making call</param>
        /// <returns>   true if it succeeds, false if it fails. Error should be placed in FErrorMessage</returns>
        /// <remarks> Override this method to do some activity after each annual run.  The first year does not
        /// 		  call this Preprocess</remarks>
        ///-------------------------------------------------------------------------------------------------
        public virtual bool PreProcess(int year,WaterSimManagerClass WSim)
        {
            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method that is called before after each annual run </summary>
        /// <param name="year"> The year just run </param>
        /// <param name="WSim"> The WaterSimManager that is making call</param>
        /// <returns>   true if it succeeds, false if it fails. Error should be placed in FErrorMessage</returns>
        /// <remarks> Override this method to do some activity after each annual run
        /// 		  To test if this is the first year use (year == Wsim.Simulation_Start_Year) 
        /// 		  To test if this is the last year use  (year == Wsim.Simulation_End_Year) </remarks>
        ///-------------------------------------------------------------------------------------------------
        public virtual bool PostProcess(int year,WaterSimManagerClass WSim)
        {
            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method called when Sumulation is stopped </summary>
        ///-------------------------------------------------------------------------------------------------

        virtual public bool ProcessStop(WaterSimManagerClass WSim)
        {
            return true;
 
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name of this instance of a process. </summary>
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Name 
        {
        get { return Fname;}
        set { Fname = value;}
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a message describing the error. </summary>
        /// <value> A message describing the error. </value>
        ///-------------------------------------------------------------------------------------------------

        public string ErrorMessage
            { get {return FErrorMessage;}     }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the class description. </summary>
        /// <returns>  Class Description </returns>
        ///-------------------------------------------------------------------------------------------------

        static public string ClassDescription()
        {
            return "Base abstract class for AnnualFeedbackProcess";
        }
    }
    //==============================================================
    //  WaterSIm Process Manager
    // 
    // 
    // 
    //==============================================================

    ///-------------------------------------------------------------------------------------------------
    /// <summary> ProcessManagerEventType. </summary>
    /// <remarks> indicates the type of event</remarks>
    ///-------------------------------------------------------------------------------------------------

    public enum ProcessManagerEventType { 
        /// <summary> Event at Process Added to Que. </summary>
        peAddProcess,
        /// <summary> Event at Process Deleted from Que. </summary>
        peDeleteProcess, 
        /// <summary> Event with a PreProcess call. </summary>
        pePreProcess,
        /// <summary> Event with a PostProcess call. </summary>
        pePostProcess,
        /// <summary> Event with a StartProcess call. </summary>
        peStartProcess,
        /// <summary> Event with an Initialize Process call. </summary>
        peInitializeProcess, 

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Event at stop process. </summary>
        ///-------------------------------------------------------------------------------------------------
        peStopProcess
    };

    ///-------------------------------------------------------------------------------------------------
    /// <summary> Information passed for process manager events. </summary>
    ///-------------------------------------------------------------------------------------------------

    public class ProcessManagerEventArgs : EventArgs
    {
        /// <summary> Type of the process event. </summary>
        public ProcessManagerEventType TheProcessEventType;
        /// <summary> the annual feedback process. </summary>
        public AnnualFeedbackProcess TheAnnualFeedbackProcess;
        /// <summary> the result. </summary>
        public bool TheResult;
        /// <summary> the year. </summary>
        public int TheYear;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <param name="PET">    The ProcessMangerEvent Type. </param>
        ///-------------------------------------------------------------------------------------------------
        public ProcessManagerEventArgs(ProcessManagerEventType PET)
        {
            TheProcessEventType = PET;
            TheAnnualFeedbackProcess = null;
            TheResult = true;
            TheYear = 0;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <param name="PET">    The ProcessMangerEvent Type. </param>
        /// <param name="AFP">    The Process afp. </param>
        /// <param name="Result"> true to result. </param>
        ///-------------------------------------------------------------------------------------------------
        public ProcessManagerEventArgs(ProcessManagerEventType PET, AnnualFeedbackProcess AFP, bool Result)
        {
            TheProcessEventType = PET;
            TheAnnualFeedbackProcess = AFP;
            TheResult = Result;
            TheYear = 0;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <param name="PET">    The ProcessMangerEvent Type. </param>
        /// <param name="AFP">    The Process for which the event was triggered. </param>
        /// <param name="Result"> The result of the process result. </param>
        /// <param name="Year">   The year Event occurred. </param>
        ///-------------------------------------------------------------------------------------------------

        public ProcessManagerEventArgs(ProcessManagerEventType PET, AnnualFeedbackProcess AFP, bool Result, int Year)
        {
            TheProcessEventType = PET;
            TheAnnualFeedbackProcess = AFP;
            TheResult = Result;
            TheYear = Year;
        }
    }
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Class for Managing AnnualFeedbackProcess objects. </summary>
    /// <remarks>   </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class ProcessManager
    {
        List<string> FErrorMessages = new List<string>();
        bool FIsError = false;
        /// <summary> List of processes </summary>
        protected List<AnnualFeedbackProcess> FProcessList = new List<AnnualFeedbackProcess>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///-------------------------------------------------------------------------------------------------

        public ProcessManager()
        {

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating how many errors occurred during a Simulation. </summary>
        /// <remarks>  If there is an error, ErrorMessages contains a list of the errors ("Stage:ClassName:ErrorMessage")
        /// 		   The error flag is reset with a ProcessInitialize call, errors from a previous siumulation runs
        /// 		   are cleared at the start of a new simulation.</remarks>
        /// <value> Number of errors that occurred during process. </value>
        ///-------------------------------------------------------------------------------------------------

        public int IsError
        { get { if (FIsError) return FErrorMessages.Count; else return 0 ;} }
  
        internal void ResetError()
        {
            FIsError = false;
            FErrorMessages.Clear();
        }
        /// <summary> Event queue for all listeners interested in ProcessManager Process events. </summary>
        /// <remarks> ProcessManagerProcessEvents are called after InitializeProcess,StartProcess,PreProcess,PostProcess, StopProcess methods
        ///           his should be used primarily for debugging purposes.  This event gets called five times during each annual
        ///           run of the WaterSim simulation, which can amount to hudreds of calls, thus slowing down the simulation</remarks>
        ///           <seealso cref="ProcessManagerEventArgs"/>
        ///           <seealso cref="ProcessManagerEventType"/>
        public event EventHandler<ProcessManagerEventArgs> ProcessManagerProcessEvent;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Raises the process manager process event. </summary>
        /// <remarks> This should be used primarily for debugging purposes.  This event gets called five times during each annual
        ///           run of the WaterSim simulation, which can amount to hudreds of calls, thus slowing down the simulation</remarks>
        /// <param name="e"> Event information to send to registered event handlers. </param>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void OnProcessManagerProcessEvent(ProcessManagerEventArgs e)
        {
            if (ProcessManagerProcessEvent != null) ProcessManagerProcessEvent(this, e);
        }

        /// <summary> Event queue for all listeners interested in ProcessManagerChange events. </summary>
        /// <remarks>Event evoked after the que list of AnnualFeedbackProcess objects is changed, (add or delete)</remarks>

        public event EventHandler<ProcessManagerEventArgs> ProcessManagerChangeEvent;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Raises the process manager eChange vent. </summary>
        /// <param name="e"> Event information to send to registered event handlers. </param>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void OnProcessManagerChangeEvent(ProcessManagerEventArgs e)
        {
            if (ProcessManagerChangeEvent != null) ProcessManagerChangeEvent(this, e);
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a process to the ProcessManager list </summary>
        /// <remarks> if aProcess is null, or a process of the same class type is already in the Process que, 
        ///           the process is not added and this routinne returns false.
        ///           Once an AnnualFeedbackProcess objet has been added to the ProcessManager, the ProcessManager
        ///           is the owner of the process object.  This means when it is deleted, its dispose method is called. </remarks>
        /// <param name="aProcess"> a process. </param>
        /// <returns>   true if it succeeds, false if it fails (only failure is if aProcess is null. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool AddProcess(AnnualFeedbackProcess aProcess)
        {
            bool Success = false;
            // Test for null objects
            if (aProcess != null)
            {
                //int index = Findindex(aProcess.Name);
                int index = FindindexByClassname(aProcess.GetType().Name);
                if (index == -1)
                {
                    FProcessList.Add(aProcess);
                    Success = true;
                    OnProcessManagerChangeEvent(new ProcessManagerEventArgs(ProcessManagerEventType.peAddProcess, aProcess, Success));
                }
            }
            return Success;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Enumerates all processes in this collection. </summary>
        /// <returns> An enumerator that allows foreach to be used to process all processes in this
        ///     collection.
        ///     </returns>
        ///-------------------------------------------------------------------------------------------------

        public IEnumerable<AnnualFeedbackProcess> AllProcesses()
        {
            int length = FProcessList.Count;
            for (int i = 0; i < length; i++)
            {
                yield return FProcessList[i];
            }
        }      //---------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
         /// <summary> Gets a list of process. </summary>
         ///
         /// <value> A List of process. </value>
         ///-------------------------------------------------------------------------------------------------

         public List<AnnualFeedbackProcess>   ProcessList
         {
             get { return FProcessList; } 
         }
        //-----------------------------------------------------------------
        internal int Findindex(string ProcessName)
        {
            int index = -1;
            ProcessName = ProcessName.ToUpper();
            for (int i = 0; i < FProcessList.Count; i++)
            {
                if (FProcessList[i].Name.ToUpper() == ProcessName)
                {
                    index = i;
                    break;
                }
            }
                return index;
        }
        //-----------------------------------------------------------------
        internal int FindindexByClassname(string ClassName)
        {
            int index = -1;
            //ClassName = ClassName.ToUpper();
            for (int i = 0; i < FProcessList.Count; i++)
            {
                AnnualFeedbackProcess AFP = FProcessList[i];
                if (AFP.GetType().Name == ClassName)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Searches for the first Process for the given string in the list of processes being managed. </summary>
        /// <param name="ProcessName">  Name of the process. </param>
        /// <returns>  AnnualFeedbackProcess found, null if not found</returns>
        ///-------------------------------------------------------------------------------------------------

        public AnnualFeedbackProcess Find(string ProcessName)
        {
            AnnualFeedbackProcess AFP = null;
            int index = Findindex(ProcessName);
            if (index > -1)
            { AFP = FProcessList[index]; }
            return AFP;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Searches for the first Process of the classname typein the list of processes being managed. </summary>
        /// <param name="ClassName">  Name of the class. </param>
        /// <returns>  AnnualFeedbackProcess found, null if not found</returns>
        ///-------------------------------------------------------------------------------------------------

        public AnnualFeedbackProcess FindByClassname(string ClassName)
        {
            AnnualFeedbackProcess AFP = null;
            int index = FindindexByClassname(ClassName);
            if (index > -1)
            { AFP = FProcessList[index]; }
            return AFP;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Deletes the given ProcessName from list of processes being managed</summary>
        /// <remarks> Also calls the Process' Dispose method, since the Manager Owns it and no longer
        ///           will have access to it.
        ///           ProcessManagerEvent is called with ProcessEventType.peDisposeProcess after it is removed from the list but before it is disposed.</remarks>
        /// <param name="ProcessName">  Name of the process. </param>
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool Delete (string ProcessName)
        {
            bool done = false;
            AnnualFeedbackProcess AFP = Find(ProcessName);
            if (AFP != null)
              { 
                FProcessList.Remove(AFP);
                // Manager Owns it, so dispose of it.
                 ProcessManagerEventArgs PMEA = new ProcessManagerEventArgs(ProcessManagerEventType.peDeleteProcess, AFP, done);
                 OnProcessManagerChangeEvent(PMEA);
                 if (PMEA.TheResult == true)
                 {
                     AFP.Dispose();
                     done = true;
                 }
              }
            return done;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Deletes the given ProcessName from list of processes being managed</summary>
        /// <remarks> Also calls the Process' Dispose method, since the Manager Owns it and no longer
        ///           will have access to it.
        ///           ProcessManagerEvent is called with ProcessEventType.peDisposeProcess after it is removed from the list but before it is disposed.</remarks>
        /// <param name="AFP">  Feedback Process </param>
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool Delete(AnnualFeedbackProcess AFP)
        {
            bool done = false;
            
            if (AFP!=null)
                done = Delete(AFP.Name);
            return done;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Call the PreProcess method of all Processes being managed </summary>
        /// <param name="WSim"> The WaterSimManger simulation. </param>
        /// <returns>   true if it succeeds, false if it fails. </returns>
        /// <seealso cref="AnnualFeedbackProcess.ProcessInitialized"/>
        ///-------------------------------------------------------------------------------------------------

        public bool ProcessInitializeAll(WaterSimManagerClass WSim)
        {
            bool test = true;
            foreach (AnnualFeedbackProcess PC in FProcessList)
            {
                bool pretest = PC.ProcessInitialized(WSim);
                if (! pretest)
                {
                    FIsError = true;
                    FErrorMessages.Add("Init:"+PC.Name + ":" + PC.ErrorMessage);
                    test = false; 
                }
               OnProcessManagerProcessEvent( new ProcessManagerEventArgs(ProcessManagerEventType.peInitializeProcess,PC,pretest ));
            }
            return test;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Call the ProcessStarted method of all Processes being managed </summary> <summary> Process the started all. </summary>
        /// <param name="year"> The year. </param>
        /// <param name="WSim"> The WaterSimManger simulation. </param>
        /// <seealso cref="AnnualFeedbackProcess.ProcessStarted"/>
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool ProcessStartedAll(int year, WaterSimManagerClass WSim)
        {
            bool test = true;
            foreach (AnnualFeedbackProcess PC in FProcessList)
            {
                bool pretest = PC.ProcessStarted(year, WSim);
                if (! pretest)
                {
                    FIsError = true;
                    FErrorMessages.Add("Strt:" + PC.Name + ":" + PC.ErrorMessage);
                    test = false;
                }
                OnProcessManagerProcessEvent(new ProcessManagerEventArgs(ProcessManagerEventType.peStartProcess, PC, pretest));
            }
            return test;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Call the PreProcess method of all Processes being managed </summary>
        /// <param name="year"> The year. </param>
        /// <param name="WSim"> The WaterSimManger simulation. </param>
        /// <returns>   true if it succeeds, false if it fails. </returns>
        /// <seealso cref="AnnualFeedbackProcess.PreProcess"/>
        ///-------------------------------------------------------------------------------------------------

        public bool PreProcessAll(int year,WaterSimManagerClass WSim)
        {
            bool test = true;
            ResetError();
            foreach (AnnualFeedbackProcess PC in FProcessList)
            {
                bool pretest = PC.PreProcess(year,WSim);
                if (! pretest)
                {
                    FIsError = true;
                    FErrorMessages.Add(" Pre:"+PC.Name+":"+PC.ErrorMessage);
                    test = false;
                }
                OnProcessManagerProcessEvent(new ProcessManagerEventArgs(ProcessManagerEventType.pePreProcess, PC, pretest));
            }
            return test;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Call the PostProcess method of all Processes being managed </summary>
        /// <param name="year"> The year. </param>
        /// <param name="WSim"> The WaterAimManager simulation. </param>
        /// <returns>   true if it succeeds, false if it fails. </returns>
        /// <seealso cref="AnnualFeedbackProcess.PostProcess"/>
        ///-------------------------------------------------------------------------------------------------

        public bool PostProcessAll(int year,WaterSimManagerClass WSim)
        {
            bool test = true;
            ResetError();
            foreach (AnnualFeedbackProcess PC in FProcessList)
            {
                bool pretest = PC.PostProcess(year,WSim);
                if (! pretest)
                {
                    FIsError = true;
                    FErrorMessages.Add("Post:"+PC.Name + ":" + PC.ErrorMessage);
                    test = false;
                }
                OnProcessManagerProcessEvent(new ProcessManagerEventArgs(ProcessManagerEventType.pePostProcess, PC, pretest));
            }
            return test;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Call the PostProcess method of all Processes being managed </summary>
        /// <param name="WSim"> The WaterAimManager simulation. </param>
        /// <returns>   true if it succeeds, false if it fails. </returns>
        /// <seealso cref="AnnualFeedbackProcess.PostProcess"/>
        ///-------------------------------------------------------------------------------------------------
        public bool StopProcessAll(WaterSimManagerClass WSim)
        {
            bool test = true;
            ResetError();
            foreach (AnnualFeedbackProcess PC in FProcessList)
            {
                bool pretest = PC.ProcessStop(WSim);
                if (!pretest)
                {
                    FIsError = true;
                    FErrorMessages.Add("Stop:" + PC.Name + ":" + PC.ErrorMessage);
                    test = false;
                }
                OnProcessManagerProcessEvent(new ProcessManagerEventArgs(ProcessManagerEventType.peStopProcess, PC, pretest));
            }
            return test;
        }

 
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets list of names (string) for active processes being managed. </summary>
        /// <value> string list of the active processes. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<string> ActiveProcesses
        {
            get
            {
                List<string> names = new List<string>();
                foreach (AnnualFeedbackProcess AFP in FProcessList)
                {
                    names.Add(AFP.Name+" : "+AFP.ProcessDescription);
                }
                return names;
            }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets list of ProcessCodes (string) for active processes being managed. </summary>
        /// <value> string list of the active process Codes. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<string> ActiveProcessCodes
        {
            get
            {
                List<string> names = new List<string>();
                foreach (AnnualFeedbackProcess AFP in FProcessList)
                {
                    names.Add(AFP.ProcessCode);
                }
                return names;
            }
        }
    }

    #endregion

    //*********************************************************'
    // PROCESS REGISTRY CLASS
    // 
    //*********************************************************
    // 

    #region
    ///-------------------------------------------------------------------------------------------------
    /// <summary> Values that represent ProcessRegistryEventType. </summary>
    ///
    /// <remarks> Ray, 1/24/2013. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public enum ProcessRegistryEventType  {
        /// <summary> A process class was added to the process Registry. </summary>
        preAdd,
        /// <summary> A process class was Deleted from the process Registry. </summary>
        preDelete, 
        /// <summary> A process object was instantiated usinge a process class in the registry. </summary>
        preConstruct
    };
    ///-------------------------------------------------------------------------------------------------
    /// <summary> Information for process regsitry events. </summary>
    ///-------------------------------------------------------------------------------------------------

    public class ProcessRegistryEventArgs : EventArgs
    {
        /// <summary> Type of the event. </summary>
        public ProcessRegistryEventType TheEventType;
        /// <summary> Name of the class. </summary>
        /// <remarks>Classname, added, deleted, or Constructed</remarks>
        public string TheClassName; // The Classname event is evoked on
        /// <summary> The result of the event. </summary>
        public bool TheResult;
        /// <summary> the process Constructed otherwise null. </summary>
        public AnnualFeedbackProcess TheProcess;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <param name="anEventType"> Type of an event. </param>
        /// <param name="aClassName">  Name of the class. </param>
        ///-------------------------------------------------------------------------------------------------

        public ProcessRegistryEventArgs(ProcessRegistryEventType anEventType, string aClassName)
        {
            TheEventType = anEventType;
            TheClassName = aClassName;
            TheResult = true;
            TheProcess = null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <param name="anEventType"> Type of an event. </param>
        /// <param name="aClassName">  Name of the class. </param>
        /// <param name="EventResult"> event result. </param>
        ///-------------------------------------------------------------------------------------------------
        
        public ProcessRegistryEventArgs(ProcessRegistryEventType anEventType, string aClassName, bool EventResult)
        {
            TheEventType = anEventType;
            TheClassName = aClassName;
            TheResult = EventResult;
            TheProcess = null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <param name="anEventType"> Type of an event. </param>
        /// <param name="aClassName">  Name of the class. </param>
        /// <param name="EventResult"> event result. </param>
        /// <param name="aProcess">    The process. </param>
        ///-------------------------------------------------------------------------------------------------

        public ProcessRegistryEventArgs(ProcessRegistryEventType anEventType, string aClassName, bool EventResult, AnnualFeedbackProcess aProcess)
        {
            TheEventType = anEventType;
            TheClassName = aClassName;
            TheResult = EventResult;
            TheProcess = aProcess;
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Process registry. </summary>
    /// <remarks>   Class to dynamiclly manage and invoke available AnnualFeedbackProcess classes </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class ProcessRegistry
    {
        
        List<System.Type> FAnnualFeedbackProcesses = new List<System.Type>();

        string FErrMessage = "";

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        ///-------------------------------------------------------------------------------------------------

        public ProcessRegistry()
        {

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Adds an annual feedback process. </summary>
        /// <param name="PCT"> The type of the AnnualFeedbackProcess. </param>
        ///<remarks>Only will add objects that a descendants of the AnnualFeedbackProcess class</remarks>
        /// <returns> true if it succeeds, false if it fails. </returns>
        /// <seealso cref="AnnualFeedbackProcess"/>
        ///-------------------------------------------------------------------------------------------------

        public bool addAnnualFeedbackProcess(System.Type PCT)
        {
            bool test = false;

            if (typeof(AnnualFeedbackProcess).IsAssignableFrom(PCT))
            {
                FAnnualFeedbackProcesses.Add(PCT);
                test = true;
            }
            OnProcessManagerConstructEvent(new ProcessRegistryEventArgs(ProcessRegistryEventType.preAdd, PCT.Name,test));
            return test;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Searches for the first classname in the registry that matches the given string. </summary>
        /// <param name="PName"> The class name. </param>
        ///
        /// <returns> the type of the class found. </returns>
        ///-------------------------------------------------------------------------------------------------

        public System.Type Find(string PName)
        {
            System.Type target = null;
            foreach (System.Type temp in FAnnualFeedbackProcesses)
            {
                if (temp.Name == PName)
                {
                    target = temp;
                    break;
                }
            }
            return target;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets a value indicating whether adding, deleteing, or evoking caused an error. </summary>
        /// <value> true if this object is error, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IsError
            { get { return FErrMessage != ""; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets a message describing the error. </summary>
        ///
        /// <value> A message describing the error. </value>
        ///-------------------------------------------------------------------------------------------------

        public string ErrMessage
        { 
            get 
            { 
                string temp = FErrMessage;
                FErrMessage = "";
                return temp;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Evokes the ClassName Constructor. </summary>
        /// <param name="ClassName"> Name of the class. </param>
        /// <param name="WSim">      The WaterSim simulation owning ProcessRegistry. </param>
        /// <returns> a new AnnualFeedbackProcess </returns>
        ///-------------------------------------------------------------------------------------------------

        public AnnualFeedbackProcess Construct(string ClassName, WaterSimManager WSim)
        {
            AnnualFeedbackProcess NewAFP = null;
            System.Type target = Find(ClassName);
            if (target != null)
            {
                ConstructorInfo ctor = target.GetConstructor(new[] { typeof(WaterSimManager) });
                if (ctor != null)
                {
                    try
                    {
                        object instance = ctor.Invoke(new object[] { WSim });
                        NewAFP = (instance as AnnualFeedbackProcess);
                    }
                    catch (Exception e)
                    {
                        FErrMessage = e.Message;
                    }
                }
                else
                    FErrMessage = "Class Not Found: " + ClassName;
            }
            OnProcessManagerConstructEvent(new ProcessRegistryEventArgs(ProcessRegistryEventType.preConstruct, ClassName, (FErrMessage == ""), NewAFP));
            return NewAFP;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Evokes the ClassName Constructor. </summary>
        ///
        /// <param name="ClassName">    Name of the class. </param>
        /// <param name="WSim">         The WaterSim simulation owning ProcessRegistry. </param>
        /// <param name="Quiet">        true to quiet. </param>
        ///
        /// <returns>   a new AnnualFeedbackProcess. </returns>
        ///-------------------------------------------------------------------------------------------------

        public AnnualFeedbackProcess Construct(string ClassName, WaterSimManager WSim, bool Quiet)
        {
            AnnualFeedbackProcess NewAFP = null;
            System.Type target = Find(ClassName);
            if (target != null)
            {
                ConstructorInfo ctor = target.GetConstructor(new[] { typeof(WaterSimManager) , typeof(bool)});
                if (ctor != null)
                {
                    try
                    {
                        object instance = ctor.Invoke(new object[] { WSim, Quiet });
                        NewAFP = (instance as AnnualFeedbackProcess);
                    }
                    catch (Exception e)
                    {
                        FErrMessage = e.Message;
                    }
                }
                else
                    FErrMessage = "Class Not Found: " + ClassName;
            }
            OnProcessManagerConstructEvent(new ProcessRegistryEventArgs(ProcessRegistryEventType.preConstruct, ClassName, (FErrMessage == ""), NewAFP));
            return NewAFP;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Evokes the ClassName Constructor. </summary>
        /// <param name="ClassName"> Name of the class. </param>
        /// <returns> a new AnnualFeedbackProcess. </returns>
        ///-------------------------------------------------------------------------------------------------

        public AnnualFeedbackProcess Construct(string ClassName)
        {
            AnnualFeedbackProcess NewAFP = null;
            System.Type target = Find(ClassName);
            if (target != null)
            {
                ConstructorInfo ctor = target.GetConstructor(new Type[] {});
                if (ctor != null)
                {
                    try
                    {
                        object instance = ctor.Invoke(new object[] { });
                        NewAFP = (instance as AnnualFeedbackProcess);
                    }
                    catch (Exception e)
                    {
                        FErrMessage = e.Message;
                    }
                }
                else
                    FErrMessage = "Class Not Found: " + ClassName;
            }
            OnProcessManagerConstructEvent(new ProcessRegistryEventArgs(ProcessRegistryEventType.preConstruct, ClassName, (FErrMessage==""),NewAFP));
            return NewAFP;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Evokes the ClassName Constructor. </summary>
        /// <param name="ClassName">   Name of the class. </param>
        /// <param name="ProcessName"> Name of the process. </param>
        /// <param name="WSim">        The WaterSim simulation owning ProcessRegistry. </param>
        /// <returns> a new AnnualFeedbackProcess. </returns>
        ///-------------------------------------------------------------------------------------------------

        public AnnualFeedbackProcess Construct(string ClassName, string ProcessName, WaterSimManager WSim)
        {
            AnnualFeedbackProcess NewAFP = null;
            System.Type target = Find(ClassName);
            if (target != null)
            {
                ConstructorInfo ctor = target.GetConstructor(new Type[] { typeof(string),typeof(WaterSimManager) });
                if (ctor != null)
                {
                    try
                    {
                        object instance = ctor.Invoke(new object[] { ProcessName, WSim });
                        NewAFP = (instance as AnnualFeedbackProcess);
                    }
                    catch (Exception e)
                    {
                        FErrMessage = e.Message;
                    }
                }
                else
                    FErrMessage = "Class Not Found: " + ClassName;
            }
            OnProcessManagerConstructEvent(new ProcessRegistryEventArgs(ProcessRegistryEventType.preConstruct, ClassName, (FErrMessage==""),NewAFP));
            return NewAFP;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Evokes the ClassName Constructor. </summary>
        /// <param name="ClassName">   Name of the class. </param>
        /// <param name="ProcessName"> Name of the process. </param>
        /// <returns> a new AnnualFeedbackProcess. </returns>
        ///-------------------------------------------------------------------------------------------------

        public AnnualFeedbackProcess Construct(string ClassName, string ProcessName)
        {
            AnnualFeedbackProcess NewAFP = null;
            System.Type target = Find(ClassName);
            if (target != null)
            {
                ConstructorInfo ctor = target.GetConstructor(new Type[] { typeof(string) });
                if (ctor != null)
                {
                    try
                    {
                        object instance = ctor.Invoke(new object[] { ProcessName });
                        NewAFP = (instance as AnnualFeedbackProcess);
                    }
                    catch (Exception e)
                    {
                        FErrMessage = e.Message;
                    }
                }
                else
                    FErrMessage = "Class Not Found: " + ClassName;
            }
            OnProcessManagerConstructEvent(new ProcessRegistryEventArgs(ProcessRegistryEventType.preConstruct, ClassName, (FErrMessage==""),NewAFP));
            return NewAFP;
        }

        /// <summary> Event queue for all listeners interested in ProcessRegistry Change events. </summary>
        /// <remarks>Event Evoked after a class has been added to or deleted from the registry. e.TheClassname is the class involved.</remarks>
        public event EventHandler<ProcessRegistryEventArgs> ProcessRegistryChangeEvent;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Raises the process registry change event. </summary>
        /// <param name="e"> Event information to send to registered event handlers. </param>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void OnProcessManagerChangeEvent(ProcessRegistryEventArgs e)
        {
            if (ProcessRegistryChangeEvent != null)  ProcessRegistryChangeEvent(this, e);
        }

        /// <summary> Event queue for all listeners interested in ProcessRegistry Construct events. </summary>
        /// <remarks> Event evoked after a class has been deleted from registry. e.TheClassname is the class involved.
        ///                   e.TheProcess is the process that was constructed, and e.TheResult is the result if evoking the class constructor</remarks>
        public event EventHandler<ProcessRegistryEventArgs> ProcessRegistryConstructEvent;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Raises the process registry Construct event. </summary>
        /// <param name="e"> Event information to send to registered event handlers. </param>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void OnProcessManagerConstructEvent(ProcessRegistryEventArgs e)
        {
            if (ProcessRegistryConstructEvent != null) ProcessRegistryConstructEvent(this, e);
        }

       ///-------------------------------------------------------------------------------------------------
       /// <summary> Gets a list of class names in the Registry. </summary>
       ///
       /// <value> The names of the classes in Registry </value>
       ///-------------------------------------------------------------------------------------------------

       public List<string> ClassNameList
        {
            get
            {
                List<string> Names = new List<string>();
                foreach (System.Type temp in FAnnualFeedbackProcesses)
                {
                    Names.Add(temp.Name);
                }
                return Names;
            }
        }

       ///-------------------------------------------------------------------------------------------------
       /// <summary> Gets a Class description for Classname. </summary>
       /// <param name="ClassName"> Name of the class. </param>
       /// <returns> class description </returns>
       ///-------------------------------------------------------------------------------------------------

       public string ClassDescription(string ClassName)
       {
           string descrip = "";
           System.Type temp = Find(ClassName);
           if (temp != null)
           {
               MethodInfo minfo = temp.GetMethod("ClassDescription");
               if (minfo != null)
               {
                   descrip = (minfo.Invoke(null, new object[] { }) as string);
               }
           }
           return descrip;
       }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets a list of class descriptions In registry. </summary>
        ///
        /// <value> A List of class descriptions. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<string> ClassDescriptionList
        {
            get
            {
                List<string> Names = new List<string>();
                foreach (System.Type temp in FAnnualFeedbackProcesses)
                {
                    MethodInfo minfo = temp.GetMethod("ClassDescription");
                    if (minfo != null)
                    {
                        string Descrip = (minfo.Invoke(null, new object[] { }) as string);
                        Names.Add(temp.Name + " : " + Descrip);
                    }
                }
                return Names;
            }
        }
    }

#endregion

}
