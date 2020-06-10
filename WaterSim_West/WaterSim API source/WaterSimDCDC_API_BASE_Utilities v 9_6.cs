//      WaterSimDCDC Regional Water Demand and Supply Model Version 5.0

//       This is support classes for the C# WaterSim API

//       Version 4.1
//       Keeper Ray Quay ray.quay@asu.edu
//       
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

/***************************************************************
 * Water Sim API Utilities 
 * Version 9.7
 * 
 *   
 * 
 * Keeper Ray Quay
 * Version 4.2  7/24/12 Original Version
 * Version 9.0  1/28/2014  Number of methods added to provide additional provideer and SimulationResults support, search 1/28/2014
 * Version 9.6  2/27/2018. added SpecialValues.LogMod10 reoutines to provide fast and debugable Log10 support
 * Version 9.7  3/2/2018   Added methods in SimulationResults and AnnualSimulationResults to assist with extracting data quikcly from these structures
 * 
 * ************************************************************/
namespace WaterSimDCDC
{
    //********************************************
    // Enums and Constants
    // 
    // *******************************************
    // 
    #region Enums_Constants

    ///-------------------------------------------------------------------------------------------------
    /// <summary> Special values Used to represent Missing and Infinite values </summary>
    ///
    /// <remarks> Ray, 1/24/2013. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public static class SpecialValues
    {
        /// <summary> The missing int value. </summary>
        public const int MissingIntValue = -9999;// David asked to change this 4/24/14 -2147483648 ;  // Smallest int can get
        /// <summary> The infinite int value. </summary>
        public const int InfiniteIntValue = MissingIntValue + 1 ; // 

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Estimate of Integer part of LOG10()</summary>
        /// <remarks> Quay, 2/27/2018.</remarks>
        /// <remarks> Returnd an estimate of the integer part of LOG10() 
        ///           This is designed to be fast.  It is faster than Math.Log10() under relaease, twice as fast with large numbers and 5 times faster with smaller numbers
        ///           Sometimes faster than Math.Log() in debug mode.  Math>log() speed seems less predictable in debug mode
        ///           Has limitations, if a negative value is passed, returns a 0, technically this should be NAN</remarks>
        /// <param name="v"> An int value.</param>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static int FastLog10(int v)
        {
            return (v >= 100000) ? (
                 (v >= 1000000000) ? 9 : (v >= 100000000) ? 8 :
                    (v >= 10000000) ? 7 : (v >= 1000000) ? 6 : 5)
                : (
                (v >= 10000) ? 4 :
                (v >= 1000) ? 3 : (v >= 100) ? 2 : (v >= 10) ? 1 : 0);
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary> Logs a 10.</summary>
        /// <remarks> Quay, 2/27/2018.</remarks>
        /// <remarks> Returnd an estimate of the integer part of LOG10() 
        ///           This is designed to be fast.  It is faster than Math.Log10() under relaease, twice as fast with large numbers and 5 times faster with smaller numbers
        ///           Sometimes faster than Math.Log() in debug mode.  Math>log() speed seems less predictable in debug mode
        ///           Has limitations, if a negative value is passed, returns a 0, technically this should be NAN
        ///           if a number bigger than 9,999,999,999 is passed returns 9 which is incorrect </remarks>
        /// <param name="v"> a double value.</param>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static int FastLog10(double v)
        {
            return (v >= 100000) ? (
                 (v >= 1000000000) ? 9 : (v >= 100000000) ? 8 :
                    (v >= 10000000) ? 7 : (v >= 1000000) ? 6 : 5)
                : (
                (v >= 10000) ? 4 :
                (v >= 1000) ? 3 : (v >= 100) ? 2 : (v >= 10) ? 1 : 0);
        }


    }


    //---------------------------------------------------------
    /// <summary>   Values that represent modelParamtype.  </summary>
    public enum modelParamtype
    {

        /// <summary> Unknown so ignore.  </summary>
        mptUnknown,

        /// <summary> a base output paramter.  </summary>
        /// <remarks> Single integer value that represents some output</remarks>
        mptOutputBase,

        /// <summary> a provider output paramter.  </summary>
        /// <remarks> An array of integer values that represents some output for each provider</remarks>
        mptOutputProvider,

        /// <summary> a base input parameter.  </summary>
        /// <remarks> Single integer value that represents a model Input</remarks>
        mptInputBase,

        /// <summary> a provider input parameter. </summary>
        /// <remarks> An array of integer values that represents some input for each provider</remarks>
        mptInputProvider,
        /// <summary> Input to Non-Model Function</summary>
        mptInputOther,
        /// <summary> Output from Non-Model Fucntion. </summary>
        mptOutputOther,

        
        //mptOutput,

        //mptInput,

        /// <summary>  Grid2D Output Parameter    </summary>
        mptOutput2DGrid,
        /// <summary>  A Grid2D input Parameter</summary>
        mptInput2DGrid,
        /// <summary>   A Grid3D Output Parameter. </summary>
        mptOutput3DGrid,
        /// <summary>  A Grid3D Input Parameter . </summary>
        mptInput3DGrid,
    };
    /// <summary>   Values that represent rangeChecktype.  </summary>
    public enum rangeChecktype
    {

        /// <summary>  Unknown, no range check done.  </summary>
        rctUnknown,

        /// <summary> parameter needs no range check.  </summary>
        rctNoRangeCheck,

        /// <summary> parameter has a valid low and high range.  </summary>
        rctCheckRange,

        /// <summary> parameter has a valid low and high range. which is affected by or affects the range of other model parameters.  </summary>
        rctCheckRangeSpecial,

        /// <summary> parameter value must be positive.  </summary>
        rctCheckPositive
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Range check Static Class </summary>
    ///-------------------------------------------------------------------------------------------------

    static class RangeCheck
    {
        /// <summary>   Const (null) for  no special provider Range Check Routine. </summary>
        static public DcheckProvider NoSpecialProvider = null;
        /// <summary>   Const (null) for  no special provider Range Check Routine. </summary>
        static public DcheckBase NoSpecialBase = null;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Default Grid values Used for Grid2D and Grid3D structures for use with Maricopa ModFlow Model</summary>
    ///-------------------------------------------------------------------------------------------------

    static public class GridValues
    {
        /// <summary>   The row number. </summary>
        public static int RowNumber = 125; 
        /// <summary>   The column number. </summary>
        public static int ColumnNumber = 222;
        /// <summary>   The layer number. </summary>
        public static int LayerNumber = 3;
    }
    //=========================================
   
    // Provider AGGREGATE MODE

    ///-------------------------------------------------------------------------------------------------
    /// <summary> Values that represent eProviderAggregateMode. </summary>
    ///
    /// <remarks> These values indicate how a parameter should be summarized at a regional level based on individual provider values. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public enum eProviderAggregateMode
    {
        /// <summary> No Aggregation, each provider has same value, use first provider value. </summary>
        agNone,  // No aggregation
        /// <summary> Sum all the providers. </summary>
        agSum,  // Add all provider values
        /// <summary> Average all the providers. </summary>
        agAverage, // Average all provider values
        /// <summary> Average all the providers by weighting each based on demand. </summary>
        agWeighted,  // Weighted Average  // 
        ///
        agMovingAvg  // Weighted Average  // 
       
    };


#endregion

    /*********************************************************
     *   Provider Classes
     * 
     * 
     * **************************************************************/
    #region provider
    //===========================================
    // ProviderClass

    /// <summary>
    /// This a static class that provides support methods and constants for the eProvider enum
    /// </summary>
    public static partial class ProviderClass
    {
        //// Provider Routines, Constants and enums
        ///// <summary>
        ///// The last valid provider enum value
        ///// </summary>
        ///// <value>eProvider enum</value>
        //public const eProvider LastProvider = (eProvider)((int)eProviderCounts.epclastProvider); 

        ///// <summary>
        ///// The first valid enum value
        ///// </summary>
        ///// <value>eProvider enum</value>
        //public const eProvider FirstProvider = (eProvider)0;
        
        ///// <summary>
        ///// The Last valid Aggregator value
        ///// </summary>
        ///// <value>eProvider enum</value>
        //public const eProvider LastAggregate = (eProvider)((int)eProviderCounts.epcTotalProviderCount-1);

        ///// <summary>
        ///// The number of valid Provider (eProvider) enum values for use with WaterSimModel and ProviderIntArray.
        ///// </summary>
        ///// <value>count of valid eProvider enums</value>
        ///// <remarks>all providers after LastProvider are not considered one of the valid eProvider enum value</remarks>
        //public const int NumberOfProviders = (int)LastProvider + 1;

        ///// <summary>
        ///// The number of valid Provide Aggregate (eProvider) enum values.
        ///// </summary>
        ///// <value>count of valid eProvider enums</value>
        ///// <remarks>all providers after LastProvider are not considered one of the valid eProvider enum value</remarks>
        //public static int NumberOfAggregates = ((int)LastAggregate - (int)LastProvider );

        //internal const int TotalNumberOfProviderEnums = (int)eProviderCounts.epcTotalProviderCount;// NumberOfProviders + NumberOfAggregates;
        //---------------------------------------------------------

        //---------------------------------------------------------
        /// <summary>
        /// Enumerable Collection of eProviders
        /// </summary>
        /// <returns>eProvider</returns>
        public static IEnumerable<eProvider> providers()
        {
            for (eProvider i = FirstProvider; i < (LastProvider+1); i++)
            {
                yield return i;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IncludeAggregates"></param>
        /// <returns></returns>
        public static IEnumerable<eProvider> providers(bool IncludeAggregates)
        {
            if (IncludeAggregates)
            {
                for (eProvider i = FirstProvider; i <= LastAggregate; i++)
                {
                    yield return i;
                }
            }
            else
            {
                for (eProvider i = FirstProvider; i <= LastAggregate; i++)
                {
                    yield return i;
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Enumerates providers all in this collection. </summary>
        ///
        /// <remarks>   Ray Quay, 1/28/2014. </remarks>
        ///
        /// <returns>   An enumerator that allows foreach to be used to process providers all in this
        ///             collection. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static IEnumerable<eProvider> providersAll()
        {
            for (eProvider i = FirstProvider; i <=LastAggregate; i++)
            {
                yield return i;
            }
        }
        //-------------------------------------------------------------
        /// <summary>
        /// Determines of index is a valid eProvider array index
        /// </summary>
        /// <param name="index">an index to check</param>
        /// <returns>true if valid, otherwise false</returns>
        public static bool valid(int index)
        {
            return ((index >= 0) & (index <= (int)LastProvider));
        }
        //-----------------------------------------------------------------
        /// <summary>
        /// Determines if valid eProvider value
        /// </summary>
        /// <param name="p">an eProvider value to check</param>
        /// <returns>true if valid</returns>
        public static bool valid(eProvider p)
        {
            return (((int)p >= 0) & (p <= LastProvider));
        }
        //-----------------------------------------------------------------
        /// <summary>
        /// Determines if valid eProvider fieldname
        /// </summary>
        /// <param name="fieldname">a fieldname to check</param>
        /// <returns>true if valid</returns>
        public static bool valid(string fieldname)
        {
            bool found = false;
            int pindex = -1;
            fieldname = fieldname.ToUpper().Trim();
            for (int i = 0; i < FieldNameList.Length; i++)
            {
                if (FieldNameList[i].ToUpper() == fieldname)
                {
                    pindex = i;
                    found = true;
                    break;
                }
            }
            return found;
        }
        //-------------------------------------------------------------
        /// <summary>
        /// Determines if index is a valid eProvider Aggregate index
        /// </summary>
        /// <param name="index">an index to check</param>
        /// <returns>true if valid, otherwise false</returns>
        public static bool validAggregate(int index)
        {
            return ((index > (int)LastProvider) & (index <= (int)LastAggregate));
        }
        //-----------------------------------------------------------------
        /// <summary>
        /// Determines if valid eProvider Aggregate value
        /// </summary>
        /// <param name="p">an eProvider value to check</param>
        /// <returns>true if valid</returns>
        public static bool validAggregate(eProvider p)
        {
            return (((int)p > (int)LastProvider) & (p <= LastAggregate));
        }
        //-----------------------------------------------------------------
        //-----------------------------------------------------------------
        internal static void TestAndThrowInvalidProviderException(eProvider p, Boolean IncludeAggregate)
        {
            bool test = true;
            if (!valid(p))
                if (!IncludeAggregate)
                    test = false;
                else
                    if (!validAggregate(p))
                        test = false;
            if (!test)
               { throw new WaterSim_Exception(WS_Strings.Get(WS_Strings.wsInvalidProvider) + p.ToString()); }
        }

        //-----------------------------------------------------------------
        /// <summary>
        /// Returns an eProvider based on its fieldname
        /// </summary>
        /// <param name="fieldname">a fieldname to look up</param>
        /// <returns>a valid eProvider value</returns>
        /// <exception cref="WaterSim_Exception"> if not a valid fieldname</exception>
        public static eProvider provider(string fieldname)
        {
            if (valid(fieldname))
            {
                int pindex = -1;
                FastFindFieldname(fieldname, ref pindex);
                return (eProvider)pindex;
            }
            else
            {
                throw new WaterSim_Exception("Invlaid Provider Fieldname: " + fieldname);

            }

        }
        //-----------------------------------------------------------------
        /// <summary>
        /// Returns the eProvider value that corresponds to the eProvider array index
        /// </summary>
        /// <param name="index">index for eProvider</param>
        /// <returns>a valid eProvider value</returns>
        /// <exception cref="WaterSim_Exception"> if not a valid index</exception>
        public static eProvider provider(int index)
        {
            if (valid(index))
            {
                return (eProvider)index;
            }
            else
            {
                throw new WaterSim_Exception("Provider index -" + index.ToString() + " is out of range of " + FirstProvider.ToString() + " to " + LastProvider.ToString());

            }
        }

        public static eProvider providerAll(int index)
        {
            if (valid(index))
            {
                return (eProvider)index;
            }
            else
            {
                if (validAggregate(index))
                {
                    return (eProvider)index;
                }
                else
                   throw new WaterSim_Exception("Provider index -" + index.ToString() + " is out of range of " + FirstProvider.ToString() + " to " + LastAggregate.ToString());

            }
        }

        //-----------------------------------------------------------------
        /// <summary>
        /// Provider Array index for eProvider value
        /// </summary>
        /// <param name="p">provider</param>
        /// <returns>index of provider</returns>
        public static int index(eProvider p)
        {
            TestAndThrowInvalidProviderException(p,false);
            return (int)p;
        }
        //-----------------------------------------------------------------
        /// <summary>
        /// Provider Array index for eProvider value
        /// </summary>
        /// <param name="p">provider</param>
        /// <param name="includeAggregate"> true to include, false to exclude the aggregate. </param>
        /// <returns>index of provider</returns>
        ///-------------------------------------------------------------------------------------------------

        public static int index(eProvider p, bool includeAggregate)
        {
            TestAndThrowInvalidProviderException(p, includeAggregate);
            return (int)p;
        } 
        
        //-----------------------------------------------------------------
        /// <summary>
        /// Returns the index of the eProvider based on is fieldname.  Very slow, First checks if valid and thorws exception of not
        /// </summary>
        /// <param name="fieldname">an eProvider fieldname</param>
        /// <returns>index of eProvider</returns>
        /// <exception cref="WaterSim_Exception"> if no valid fieldname</exception>
        public static int index(string fieldname)
        {
            if (!valid(fieldname)) throw new WaterSim_Exception("Invlaid provider fieldname: fieldname");
            int pindex = -1;
            FastFindFieldname(fieldname, ref pindex);
            return pindex;
        }
        //-----------------------------------------------------------------
        /// <summary>
        /// Returns if fieldname is found.  Much faster, one search no exceptions, does not check for valid eProvider values, will find index for Aggregates, Use with Caution.  Should not be used for finding
        /// an index for a ProviderIntArray
        /// </summary>
        /// <param name="fieldname">an eProvider fieldname</param>
        /// <param name="index">ref for index, returns -1 if returns false</param>
        /// <returns>true of valid eProvider fieldname, false otherwise</returns>
        public static bool FastFindFieldname(string fieldname, ref int index)
        {
            bool found = false;
            int pindex = -1;
            fieldname = fieldname.ToUpper().Trim();
            for (int i = 0; i < FieldNameList.Length; i++)
            {
                if (FieldNameList[i].ToUpper() == fieldname)
                {
                    pindex = i;
                    found = true;
                    break;
                }
            }
            index = pindex;
            return found;
        }
        //-------------------------------------------------------------
        /// <summary>
        /// Full text label for eProvider and Aggregate
        /// </summary>
        /// <param name="p">provider</param>
        /// <returns>label</returns>
        public static string Label(eProvider p)
        {
            TestAndThrowInvalidProviderException(p,true);
            return ProviderNameList[(int)p];
        }
        //-------------------------------------------------------------
        /// <summary>
        /// Fieldname for Provider and Aggregate
        /// </summary>
        /// <param name="p">provider</param>
        /// <returns>fieldname</returns>
        public static string FieldName(eProvider p)
        {
            TestAndThrowInvalidProviderException(p,true);
            return FieldNameList[(int)p];
        }
        //-------------------------------------------------------------
        //-------------------------------------------------------------

      }  // end or ProviderClass
    // Speical Range Check
#endregion

    //**************************************************
    // Data classes and Structures 
    // 
    // *************************************************
    #region DataClasses
    /// <summary>   Dcheck base. </summary>
    /// <remarks>   Delegate for special range check routines. </remarks>
    /// <param name="value">            The value to check. </param>
    /// <param name="errorstr">         [in,out] if error return a string. </param>
    /// <param name="aModelParameter">  a model parameter. </param>
    /// <returns>  True if valeu passes range check, else false </returns>

    public delegate bool DcheckBase(int value, ref string errorstr, ModelParameterBaseClass aModelParameter);

    //--------------------------------------------------------------------------------------------
    /// <summary>   Dcheck provider. </summary>
    /// <remarks>  Delegate for special range checks of provider input paramteres. </remarks>
    /// <param name="value"> The value to check. </param>
    /// <param name="ep">  the provider </param>
    /// <param name="errorstr">         [in,out] if error return a string. </param>
    /// <param name="aModelParameter">  a model parameter. </param>
    /// <returns>  True if valeu passes range check, else false </returns>

    public delegate bool DcheckProvider(int value, eProvider ep, ref string errorstr, ModelParameterBaseClass aModelParameter);

    //================================================================
    // GET AND SET MOCDEL VALUE DELEGATES
    // 
    // =================================================================
    //--------------------------------------------------------------------------------------------
    // INT DELEGATES
    // get int delegate
    /// <summary>   Delagate for "Gets an int" method </summary>
    /// <returns>  just an int </returns>
    public delegate int Dget();
    // Set int delegate
    /// <summary>   Delagate for "Sets an int" method </summary>
    public delegate void Dset(int value);

    // DOUBLE DELEGATES
    // get double delegate
    /// <summary>   Delagate for "Gets a double" method </summary>
    /// <returns>  just a double </returns>
    public delegate int DgetDouble();
    // Set double delegate
    /// <summary>   Delagate for "Sets a double" method </summary>
    public delegate void DsetSouble(double value);
    
    // INT ARRAY DELGATES
    // Getarray delegate
    /// <summary>   Delagate for "Gets an int[]" method </summary>
    /// <returns>  just an int[] </returns>
    public delegate int[] Dgetarray();
    // Setarayy delegate
    /// <summary>   Delagate for "Sets an int[]" method </summary>
    public delegate void Dsetarray(int[] value);

    // DOPUBLE ARRAY DELGATES
    // GetDoublearray delegate
    /// <summary>   Delagate for "Gets a double[]" method </summary>
    /// <returns>  just an int[] </returns>
    public delegate double[] DgetDoublearray();
    // SetDoublearayy delegate
    /// <summary>   Delagate for "Sets a double[]" method </summary>
    public delegate void DsetDoublearray(double[] value);

    // GRID DELEGATES
    // GetGrid2D delegate
    /// <summary>   Delagate for "Gets a double[,]" method </summary>
    /// <returns>  just an double[,] </returns>
    public delegate double[,] DGetGrid2D();
    // SetGrid2D delegate
    /// <summary>   Delagate for "Sets a double[,]" method </summary>
    public delegate void DSetGrid2D(double[,] value);
    // GetGrid3D delegate
    /// <summary>   Delagate for "Gets a double[,,]" method </summary>
    /// <returns>  just an double[,,] </returns>
    public delegate double[,,] DGetGrid3D();
    // SetGrid3D delegate
    /// <summary>   Delagate for "Sets a double[,,]" method </summary>
    public delegate void DSetGrid3D(double[,,] value);
 

    //================================================================
    //  providerArrayPropertyBaseClassBaseClass  Initial Structure for providing access get/set to
    //  provider data
    // 
    // =================================================================

    /// <summary>
    /// Base class for All Provider Model Parameters
    /// </summary>
    /// <remark>This is the abstract base class that sets up a structure used to provde an indexed property of provider data </remark>
   
    public abstract class providerArrayPropertyBaseClass
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public providerArrayPropertyBaseClass() : base() { }

        ////------------------
        /// <summary>
        /// ABSTRACT indexer for indexed array gets and sets based on eProvider enum values
        /// </summary>
        /// <param name="index">eProvider index</param>
        /// <value>indexed Model Parameter value</value>
        /// <returns>value in provider array for eProvider index</returns>
        /// <exception cref="WaterSim_Exception">Should throw exception if set and value viloates a range rule</exception>
        public abstract int this[eProvider index] { get; set; }
   
        
        /// <summary>
        /// indexer for indexed array gets and sets
        /// </summary>
        /// <param name="index">index to array</param>
        /// <value>indexed Model Parameter value</value>
        /// <returns>value in provider array for index</returns>
        /// <exception cref="WaterSim_Exception">if set and value viloates a range rule</exception>
        public virtual int this[int index]
        {
            get
            {
                return this[(eProvider)index];
            }
            set
            {
                this[(eProvider)index] = value;
            }
        }        //----------------------
        /// <summary>
        /// ABSTRACT Gets Model Parameter Array values 
        /// </summary>
        /// <returns>an Array of Model Values</returns>
        public abstract ProviderIntArray getvalues(); 

        //----------------------
        /// <summary>
        /// ABSTRACT Set Model Parameter array using value array
        /// </summary>
        /// <param name="value">values to set Model Parameter</param>
        /// <exception cref="WaterSim_Exception">if any numbers in value violate a range rule</exception>
        public abstract void setvalues(ProviderIntArray value);
    }
    //-------------------------------------------------------------------------------------
    // This is the class used to provde and indexed property to wrap around the provider input array parammeters
    /// <summary>
    /// Class for All Provider Model Parameters that access the WaterSimManager model directly
    /// </summary>
     /// <remarks>This is the class used to provde an indexed property to wrap around the provider input array parammeters that need direct access to the 
     /// 		  Fortan model Parameters, with a layer of rangechacking that throws exception when values violate range check rules.  
     /// 		  The standard constructor for this class is hidden.  Parameters of this class are created by the WaterSimManager class and managed by the 
    /// 		  WaterSimManager.ParamManager, thus the constructor is hidden.  However default cconstructor is exposed so new classes can be derived.</remarks>
   public class providerArrayProperty : providerArrayPropertyBaseClass
   {
       // get data references
       /// <summary> A function used to get an array of values. </summary>
       protected Dgetarray Fget;
       /// <summary> A function used to set and array of values. </summary>
       protected Dsetarray Fset;
       /// <summary> The ParameterManager. </summary>
       protected ParameterManagerClass Fpm;
       /// <summary> The code for this parameter. </summary>
       protected int Femp;
       /// <summary> The aggregate mode. </summary>
       protected eProviderAggregateMode FAggregateMode;
       //------------------
       
       ///-------------------------------------------------------------------------------------------------
       /// <summary> Default constructor. </summary>
       ///
       /// <remarks> This base constructor has been exposed.  Does not implement anything, thus to create a new providerArrayProperty, a new class will
       /// 		  need to be created, with a new constructor to overload this base constructor.</remarks>
       ///-------------------------------------------------------------------------------------------------

       public providerArrayProperty() : base() { }
       
       //------------------
       // 
       ///-------------------------------------------------------------------------------------------------
       /// <summary>    Constructor. </summary>
       /// <remarks>readonly constructor, no need for Parameter Manager or AggregateMode becuse there is no range checking or Aggregation for read only
       /// </remarks>
       /// <param name="modelparam">    The modelparam. </param>
       /// <param name="getcall">       The getcall. </param>
       ///-------------------------------------------------------------------------------------------------

       public providerArrayProperty(int modelparam, Dgetarray getcall)
       {
           SetFields(modelparam, getcall, null, eProviderAggregateMode.agNone, null);
       }

       ///-------------------------------------------------------------------------------------------------
       /// <summary>    Constructor. </summary>
       /// <remarks>        Ok, this parameter has aggregation so we need the parameter manager but this is still readonly
       /// </remarks>
       /// <param name="PM">            The ParameterManager. </param>
       /// <param name="modelparam">    The modelparam. </param>
       /// <param name="getcall">       The getcall. </param>
       /// <param name="AggregateMode"> The aggregate mode. </param>
       ///-------------------------------------------------------------------------------------------------

       public providerArrayProperty(ParameterManagerClass PM, int modelparam, Dgetarray getcall, eProviderAggregateMode AggregateMode)
       {
           SetFields(modelparam, getcall, null, AggregateMode, PM);
       }

       internal void SetFields(int modelparam, Dgetarray getcall, Dsetarray setcall, eProviderAggregateMode AggregateMode, ParameterManagerClass PM)
       {
           if (getcall == null) throw new WaterSim_Exception(WS_Strings.wsdbNullParameter);
           if (!ModelParamClass.valid(modelparam)) throw new WaterSim_Exception(WS_Strings.wsInvalid_EModelPAram);
          
           Fget = getcall;
           Fset = setcall;
           Fpm = PM;
           Femp = modelparam;
           FAggregateMode = AggregateMode;
       }
       // Read Write Constructor, Need it all baby!

       ///-------------------------------------------------------------------------------------------------
       /// <summary> Constructor. </summary>
       ///
       /// <remarks>        OK, This is the full thing, read write and all the works
       /// </remarks>
       ///
       /// <exception cref="WaterSim_Exception"> Thrown when watersim_. </exception>
       ///
       /// <param name="pm">            The pm. </param>
       /// <param name="modelparam">    The modelparam. </param>
       /// <param name="getcall">       The getcall. </param>
       /// <param name="setcall">       The setcall. </param>
       /// <param name="AggregateMode"> The aggregate mode. </param>
       ///-------------------------------------------------------------------------------------------------

       internal providerArrayProperty(ParameterManagerClass pm, int modelparam, Dgetarray getcall, Dsetarray setcall, eProviderAggregateMode AggregateMode)
           : base()
       {
           SetFields(modelparam, getcall, setcall, AggregateMode, pm);
       }
       //------------------------------------
       /// <summary>
       /// the eModelParam value
       /// <value> the eModelParam for the ModelParameter this property is supporting</value>
       /// </summary>
       public int ModelParam
       { get { return Femp; } set { Femp = value; } }

       //----------------------
       /// <summary>
       /// indexer for indexed array gets and sets based on eProvider enum values
       /// </summary>
       /// <param name="index">eProvider index</param>
       /// <value>indexed Model Parameter value</value>
       /// <returns>value in provider array for eProvider index</returns>
       /// <exception cref="WaterSim_Exception">if set and value viloates a range rule</exception>
       public override int this[eProvider index]
       {
           get
           {
               if (ProviderClass.valid(index))
               {
                   int[] temp;
                   temp = Fget();
                   return temp[(int)index];
               }
               else
               {
                  if (!ProviderClass.validAggregate(index))
                   {
                       throw new WaterSim_Exception(WS_Strings.wsInvalidProvider);
                   }
                   else
                   {
                      //  insert aggregation code
                    return RegionalValue(index);
                   }
               
              }
           }
           set
           {
               if (Fset == null) throw new WaterSim_Exception(WS_Strings.wsReadOnly);
               if (!ProviderClass.valid(index)) throw new WaterSim_Exception(WS_Strings.wsInvalidProvider);
               string errMessage = "";
               if (Fpm.CheckProviderValueRange(Femp, value, (eProvider)index, ref errMessage))
               {
                   int[] temp = Fget();
                   temp[(int)index] = value;
                   Fset(temp);
               }
               else throw new WaterSim_Exception(errMessage);
           }
       }
       //------------------------------------------------------------
       /// <summary>
       /// Gets Model Parameter Array values 
       /// </summary>
       /// <returns>an Array of Model Values</returns>
       public override ProviderIntArray getvalues()
       // public int[] getvalues()
       {
           ProviderIntArray pia = new ProviderIntArray(0);
           pia.Values = Fget();
           return pia;
           //return Fget();
       }
       //----------------------
       /// <summary>
       /// Set Model Parameter array using value array
       /// </summary>
       /// <param name="value">values to set Model Parameter</param>
       /// <exception cref="WaterSim_Exception">if any numbers in value violate a range rule</exception>
       public override void setvalues(ProviderIntArray value)
       {
           if (Fset == null) throw new WaterSim_Exception(WS_Strings.wsReadOnly);
           if (!Fpm.CheckProviderValuesRange(Femp, value))
           {
               throw new WaterSim_Exception("Invalid Value For Set Found");
           }
           Fset(value.Values);
       }

     
       ///-------------------------------------------------------------------------------------------------
       /// <summary> Gets the aggregate mode. </summary>
       ///
       /// <value> The aggregate mode. </value>
       ///-------------------------------------------------------------------------------------------------

       public eProviderAggregateMode AggregateMode
       { get { return FAggregateMode; } }

       ///-------------------------------------------------------------------------------------------------
       /// <summary>    Regional value. </summary>
       ///
       /// <remarks>    Static version of the. </remarks>
       ///
       /// <param name="agmode">    The agmode. </param>
       /// <param name="Data">      The data. </param>
       /// <param name="Weight">    The POP. </param>
       ///
       /// <returns>    The value of all the providers in the regiona aggregated. </returns>
       ///-------------------------------------------------------------------------------------------------

       public static int RegionalValue(eProviderAggregateMode agmode, int[] Data, int[] Weight)
       {

           int temp = SpecialValues.MissingIntValue;

           switch (agmode)
           {
               case eProviderAggregateMode.agNone:
                   {
                       break;  // no need to do anything altready set at missing
                   }
               case eProviderAggregateMode.agAverage:
                   {
                       int total = 0;
                       int cnt = 0;
                       foreach (int value in Data)
                       {
                           cnt++;
                           total += value;
                       }
                       if (cnt > 0)
                           temp = total / cnt;
                       break;
                   } //agAverage
               case eProviderAggregateMode.agSum:
                   {
                       int total = 0;
                       foreach (int value in Data)
                       {
                           total += value;
                       }
                       temp = total;
                       break;
                   } // ag sum
                   
               case eProviderAggregateMode.agWeighted:
                   {
                       Int64 totalweight = 0;
                       Int64 total = 0;
                         for (int i = 0; i < Data.Length; i++)
                               {  Int64 Pop64 = Convert.ToInt64(Weight[i]);
                                   totalweight += Pop64;
                                   total += Convert.ToInt64(Data[i]) * Pop64;
                               }
                               if (totalweight > 0)
                               {  Int64 result64 = total / totalweight;
                                   if (result64<2147483647)
                                       temp =  Convert.ToInt32(result64);
                               }
                       }
                       break; // agweighted
               case eProviderAggregateMode.agMovingAvg:
                       {
                       //    Int64 totalweight = 0;
                       //    Int64 total = 0;
                       //    for (int i = 0; i < Data.Length; i++)
                       //    {
                       //        Int64 Pop64 = Convert.ToInt64(Weight[i]);
                       //        totalweight += Pop64;
                       //        total += Convert.ToInt64(Data[i]) * Pop64;
                       //    }
                       //    if (totalweight > 0)
                       //    {
                       //        Int64 result64 = total / totalweight;
                       //        if (result64 < 2147483647)
                       //            temp = Convert.ToInt32(result64);
                       //    }
                       }
                       break; // agweighted

               default:
                   {
                       break;
                   }
           }  // switch
           return temp;
       }

       ///-------------------------------------------------------------------------------------------------
       /// <summary> Regional value. </summary>
       ///
       /// <remarks> Ray, 1/24/2013. </remarks>
       ///
       /// <returns> The value of all the providers in the regiona aggregated. </returns>
       ///-------------------------------------------------------------------------------------------------

       public int RegionalValue(eProvider ep)
       {
           int result = SpecialValues.MissingIntValue;
           ProviderIntArray pia = new ProviderIntArray(0);
           ProviderIntArray regwt; 
           // get the regional weight parameter
           ModelParameterClass MP = Fpm.Model_Parameter(eModelParam.RegionalWeight);
           // if MP != nukk then use these values for weights
           if (MP != null)
           {
               regwt = MP.ProviderProperty.getvalues();
           }
               // else create an array with all values equal;
           else
           {
               regwt = new ProviderIntArray(1);
           }
           // get values to be averaged
           pia.Values = Fget();
           // get array of providers for this region
           eProvider[] RegionProviders = ProviderClass.GetRegion(ep);
           // loop through regional values, getting values and weights            
           if (RegionProviders != null)
           {
               int[] Values = new int[RegionProviders.Length];
               int[] Weights = new int[RegionProviders.Length];

               for (int i = 0; i < RegionProviders.Length; i++)
               {
                   Values[i] = pia[ProviderClass.index(RegionProviders[i])];
                   Weights[i] = regwt[ProviderClass.index(RegionProviders[i])];
               }
               // weighted average
               result = RegionalValue(FAggregateMode, Values, Weights);
           }
           return result;
       }  // region value
       

   }
    //---------------------------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   DoubleGrid2D. </summary>
        ///
        /// <remarks>   This is a prototype object that represents a 2D grid of double values used with the Mode FLow Model </remarks>
        ///-------------------------------------------------------------------------------------------------

     public struct DoubleGrid2D
     {
            double[,] FGridData;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        /// <param name="defaultValue"> The default value. </param>
        ///-------------------------------------------------------------------------------------------------

        public DoubleGrid2D(double defaultValue)
        {
            FGridData = new double[GridValues.RowNumber, GridValues.ColumnNumber];
            for (int row = 0; row<GridValues.RowNumber; row++)
                for (int col = 0; col<GridValues.ColumnNumber; col++)
                    FGridData[row,col] = defaultValue;
        }

        internal bool validIndex(int row, int column)
        {
            return ((row > -1) && (column > -1) && (row < RowLength) && (column < ColumnLength));
        }

         ///-------------------------------------------------------------------------------------------------
         /// <summary>
         ///    Indexer to get or set items within this collection using array index syntax.
         /// </summary>
         ///
         /// <param name="row">     The row. </param>
         /// <param name="column">  The column. </param>
         ///
         /// <returns>  The indexed item. </returns>
         ///-------------------------------------------------------------------------------------------------
         public double this[int row, int column]
         {
            get
            {
                if (validIndex( row, column))
                    return FGridData[ row, column];
                else
                    throw new WaterSim_Exception(WS_Strings.wsInvalidGridIndex);
            }
            set
            {
                if (validIndex( row, column))
                    FGridData[row, column] = value;
                else
                    throw new WaterSim_Exception(WS_Strings.wsInvalidGridIndex);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the information describing the grid. </summary>
        ///
        /// <value> Information describing the grid. </value>
        ///-------------------------------------------------------------------------------------------------

        public double[,] GridData
        {
            get { return FGridData;}
            set { FGridData = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the Number of the rows. </summary>
        ///
        /// <value> The Number of rows. </value>
        ///-------------------------------------------------------------------------------------------------

        public int RowLength
        { get { return GridValues.RowNumber; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the number of columns. </summary>
        ///
        /// <value> The number of columns. </value>
        ///-------------------------------------------------------------------------------------------------

        public int ColumnLength
        { get { return GridValues.ColumnNumber; } }

    }
    //--------------------------------------------------------------------------

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   DoubleGrid3D. </summary>
     /// <remarks>   This is a prototype object that represents a 3D grid of double values used with the Mode FLow Model </remarks>
     ///
    ///-------------------------------------------------------------------------------------------------

    public struct DoubleGrid3D
    {
        double[,,] FGridData;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="defaultValue"> The default value. </param>
        ///-------------------------------------------------------------------------------------------------

        public DoubleGrid3D(double defaultValue)
        {
            FGridData = new double[GridValues.LayerNumber, GridValues.RowNumber, GridValues.ColumnNumber];
            for (int lyr = 0; lyr<GridValues.LayerNumber; lyr++)
              for (int row = 0; row < GridValues.RowNumber; row++)
                for (int col = 0; col < GridValues.ColumnNumber; col++)
                    FGridData[lyr,row, col] = defaultValue;

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Valid index. </summary>
        /// <remarks> Returns a true valeu of the Layer, Row, and Col indexes are valid for this 3D array</remarks>
        /// <param name="layer">    The layer. </param>
        /// <param name="row">      The row. </param>
        /// <param name="column">   The column. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        internal bool validIndex(int layer, int row, int column)
        {
            return ((layer > -1) && (row > -1) && (column > -1) && (layer < LayerLength) && (row < RowLength) && (column < ColumnLength));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Indexer to get or set items within this collection using array index syntax.
        /// </summary>
        ///
        /// <param name="layer">    The layer. </param>
        /// <param name="row">      The row. </param>
        /// <param name="column">   The column. </param>
        ///
        /// <returns>   The indexed item. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double this[int layer, int row, int column]
        {
            get
            {
                if (validIndex(layer, row, column))
                    return FGridData[layer, row, column];
                else
                    throw new WaterSim_Exception(WS_Strings.wsInvalidGridIndex);
            }
            set
            {
                if (validIndex(layer, row, column))
                    FGridData[layer, row, column] = value;
                else
                    throw new WaterSim_Exception(WS_Strings.wsInvalidGridIndex);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the information describing the grid. </summary>
        ///
        /// <value> Information describing the grid. </value>
        ///-------------------------------------------------------------------------------------------------

        public double[,,] GridData
        {
            get { return FGridData; }
            set { FGridData = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the number of the layers. </summary>
        ///
        /// <value> The number of layers. </value>
        ///-------------------------------------------------------------------------------------------------

        public int LayerLength
        { get { return GridValues.LayerNumber; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the number of rows. </summary>
        ///
        /// <value> The number of rows. </value>
        ///-------------------------------------------------------------------------------------------------

        public int RowLength
        { get { return GridValues.RowNumber; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the number of columns. </summary>
        ///
        /// <value> The number of columns. </value>
        ///-------------------------------------------------------------------------------------------------

        public int ColumnLength
        { get { return GridValues.ColumnNumber; } }
    }
    //----------------------------------------------------------------------------

   ///-------------------------------------------------------------------------------------------------
   /// <summary>    Grid2d property. </summary>
   ///-------------------------------------------------------------------------------------------------

   public class Grid2DProperty
   {
       /// <summary> The ParameterManager. </summary>
       protected ParameterManagerClass Fpm;
       /// <summary> The code for this parameter. </summary>
       protected int Femp;

       // Get Grid method
       DGetGrid2D FGet;
       // Set Grid Method
       DSetGrid2D FSet;

       ///-------------------------------------------------------------------------------------------------
       /// <summary>    Constructor. </summary>
       /// <exception cref="WaterSim_Exception">    Thrown when invlaid modelparam or get or set = null
       ///                                          </exception>
       ///
       /// <param name="pm">            The ParameterManager. </param>
       /// <param name="modelparam">    the eModelParam for the ModelParameter this property is
       ///                              supporting. </param>
       /// <param name="getcall">       The getcall. </param>
       /// <param name="setcall">       The setcall. </param>
       ///-------------------------------------------------------------------------------------------------

       public Grid2DProperty(ParameterManagerClass pm, int modelparam, DGetGrid2D getcall, DSetGrid2D setcall)
       {
           if ((pm == null) | (getcall == null)) throw new WaterSim_Exception(WS_Strings.wsdbNullParameter);
           if (!ModelParamClass.valid(modelparam)) throw new WaterSim_Exception(WS_Strings.wsInvalid_EModelPAram);
           Fpm = pm;
           Femp = modelparam;
           FGet = getcall;
           FSet = setcall;
       }

       ///-------------------------------------------------------------------------------------------------
       /// <summary>    Constructor. </summary>
       /// <exception cref="WaterSim_Exception">    Thrown when a water simulation error condition
       ///                                          occurs. </exception>
       ///
       /// <param name="pm">            The ParameterManager. </param>
       /// <param name="modelparam">    the eModelParam for the ModelParameter this property is
       ///                              supporting. </param>
       /// <param name="getcall">       The getcall. </param>
       ///-------------------------------------------------------------------------------------------------

       public Grid2DProperty(ParameterManagerClass pm, int modelparam, DGetGrid2D getcall)
       {
           if ((pm == null) | (getcall == null)) throw new WaterSim_Exception(WS_Strings.wsdbNullParameter);
           if (!ModelParamClass.valid(modelparam)) throw new WaterSim_Exception(WS_Strings.wsInvalid_EModelPAram);
           Fpm = pm;
           Femp = modelparam;
           FGet = getcall;
           FSet = null;
       }

       ///-------------------------------------------------------------------------------------------------
       /// <summary>    Gets a value indicating whether this object is read only. </summary>
       ///
       /// <value>  true if this object is read only, false if not. </value>
       ///-------------------------------------------------------------------------------------------------

       public bool IsReadOnly
       { get { return (FSet == null); } }

       ///-------------------------------------------------------------------------------------------------
       /// <summary>    Gets the values in a DoubleGrid2D struct. </summary>
       ///
       /// <exception cref="WaterSim_Exception">    Thrown when Fget is not set. </exception>
       ///
       /// <returns>    DoubleGrid2D struct. </returns>
       ///-------------------------------------------------------------------------------------------------

       public DoubleGrid2D GetValues()
       {
           if (FGet != null)
           {
               DoubleGrid2D temp = new DoubleGrid2D();
               temp.GridData = FGet();
               return temp;
           }
           else
               throw new WaterSim_Exception(WS_Strings.wsGetIsNull);
       }

       ///-------------------------------------------------------------------------------------------------
       /// <summary>    Sets the values using a DoubleGrid2D struct. </summary>
       ///
       /// <exception cref="WaterSim_Exception">    Thrown when a property is read only </exception>
       ///
       /// <param name="value"> The value. </param>
       ///-------------------------------------------------------------------------------------------------

       public void SetValues(DoubleGrid2D value)
       {
           if (FSet != null)
           {
               FSet(value.GridData);
           }
           else
               throw new WaterSim_Exception(WS_Strings.wsSetIsNull);
       }

       //------------------------------------
       /// <summary>
       /// the eModelParam value
       /// <value> the eModelParam for the ModelParameter this property is supporting</value>
       /// </summary>
       public int ModelParam
       { get { return Femp; } set { Femp = value; } }
   }
    #endregion
    //----------------------------------------------------------------
    //BASIC DATA / PROVIDER ARRAYS
    //--------------------------------------------------------------------------------- 
    #region DataArrays
    /// <summary>
   /// An array structure for provider integer data.  Each cell corresponds to a different provider.  Used to set and retrieve ModelParameter data.  See ProviderClass for indexes to array 
   /// <see cref="ProviderClass"/> 
   /// </summary>
    public struct ProviderIntArray
    {
        internal bool FIncludesAggregates;
        /// <summary> The int values array</summary>
        internal int[] FValues;  // change to internal 1/20/14
        /// <summary>   The length of the Values array </summary>
        /// <value> The length zero based. </value>
        public int Length
        { get 
            {
                TestValues();
                return FValues.Length;
            }
        }

        // This is a dumb feature of structs that you can not override the parameterless constructor!
        //  Default is not to create an array for aggregates with paramterless constructor
        internal void TestValues()
        {
            if (FValues == null)
            {
                FValues = new int[ProviderClass.NumberOfProviders];
                FIncludesAggregates = false;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the values. </summary>
        /// <remarks> This only gets an array ProviderClass.NumberOfProviders in length.  
        ///           If set passes an array of only ProviderClass.NumberOfProviders in length, resets 
        ///           IncludeAggregates to false;   Use ValuesAll to get aggregate values if there</remarks>
        /// <seealso cref="IncludesAggregates"/>
        /// <value> The values. </value>
        ///------------------------------------------------------------------------------------------------
        public int[] Values
        {
            get
            {
                TestValues();
                // OK, this get complicate  To Avoid crashing the model, this only returns provider values if aggregates is true
                if (FIncludesAggregates)
                    return FValues;
                else
                {
                    int[] shortValues = new int[ProviderClass.NumberOfProviders];
                    for (int i = 0; i < shortValues.Length; i++)
                        shortValues[i] = FValues[i];
                    return shortValues;
                }
            }
            set
            {
                FValues = value;
                // ok, passing a value with only providers changes this to a provider only if it did IncludeAggregates before;
                if (value.Length == ProviderClass.NumberOfProviders)
                    FIncludesAggregates = false;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Indexer to get or set items within this array using index syntax based on an
        ///             eProvider vakue. </summary>
        ///
        /// <param name="ep">   The ep. </param>
        ///
        /// <value> The indexed item. </value>
        ///-------------------------------------------------------------------------------------------------

        public int this[eProvider ep]
        {
            get
            {
                TestValues();
                int index = (int)ep;
                if ((index < 0) || (index >= FValues.Length))
                { throw new WaterSim_Exception(WS_Strings.Get(WS_Strings.wseProviderOutOfRange)); }
                return FValues[index];
            }
            set {
                TestValues();
                int index = (int)ep;
                if ((index < 0) || (index >= FValues.Length))
                { throw new WaterSim_Exception (WS_Strings.Get(WS_Strings.wseProviderOutOfRange)); }
                FValues[index] = value;
                }
        }

        /// <summary>
        ///     Indexer to get or set items within this array using an int index (overloaded)
        /// </summary>
        ///
        /// <value> The indexed item. </value>
        public int this[int index]
        {
            get
            {
                TestValues();
                if ((index < 0) || (index >= FValues.Length))
                { throw new WaterSim_Exception(WS_Strings.Get(WS_Strings.wseProviderOutOfRange)); }
                return FValues[index];
            }
            set {
                TestValues();
                if ((index < 0) || (index >= FValues.Length))
                { throw new WaterSim_Exception (WS_Strings.Get(WS_Strings.wseProviderOutOfRange)); }
                FValues[index] = value;
                }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the array includes aggregates. </summary>
        ///
        /// <value> true if includes aggregates, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IncludesAggregates
        { get { return FIncludesAggregates; } }

        /// <summary>   Constructor. </summary>
        /// <param name="Default">  The default int value to set each cell at instantiation. </param>
        public ProviderIntArray(int Default)
        {
            FIncludesAggregates = false;
            FValues = new int[ProviderClass.NumberOfProviders];
            if (Default != 0) for (int i = 0; i < ProviderClass.NumberOfProviders; i++) FValues[i] = Default;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        /// <remarks> Used to create a ProviderIntArray that include Aggregate results</remarks>
        /// <param name="Default">      The default int value to set each cell at instantiation. </param>
        /// <param name="IncludeAll">   true to include, false to exclude all. </param>
        ///-------------------------------------------------------------------------------------------------

        public ProviderIntArray(int Default, bool IncludeAll)
        {
            if (IncludeAll)
            {
                FIncludesAggregates = true;
                int allep = ProviderClass.NumberOfProviders + ProviderClass.NumberOfAggregates;
                FValues = new int[allep];
            }
            else
            {
                FIncludesAggregates = false;
                FValues = new int[ProviderClass.NumberOfProviders];

            }
            if (Default != 0) for (int i = 0; i < FValues.Length; i++) FValues[i] = Default;
        }
  
    }
    //-------------------------------------------------
    /// <summary>
    /// An array structure for provider double data.  Each cell corresponds to a different provider.  Used to set and retrieve Derived ModelParameter data.  See ProviderClass for indexes to array 
    /// <see cref="ProviderClass"/> 
    /// </summary>
    public struct ProviderDoubleArray
    {
        internal bool FIncludesAggregates;
        /// <summary> The Double values array</summary>
        public double[] FValues;
        /// <summary>   The length of the Values array </summary>
        /// <value> The length zero based. </value>
        public int Length
        { get 
            {
                TestValues();
                return FValues.Length; 
            }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the values. </summary>
        ///
        /// <value> The values. </value>
        ///------------------------------------------------------------------------------------------------
        public double[] Values
        {
            get
            {
                TestValues();
                return FValues;
            }
            set
            {
                FValues = value;
            }
        }
        // This is a dumb feature of structs that you can not override the parameterless constructor!
        internal void TestValues()
        {
            if (FValues == null)
                FValues = new double[ProviderClass.NumberOfProviders];
        }

        /// <summary>
        ///     Indexer to get or set items within this array using index syntax based on an eProvider vakue.
        /// </summary>
        ///
        /// <value> The indexed item. </value>

        public double this[eProvider ep]
        { 
            get
            {
                TestValues();
                int index = (int)ep;
                if ((index < 0) || (index >= FValues.Length))
                { throw new WaterSim_Exception(WS_Strings.Get(WS_Strings.wseProviderOutOfRange)); }
                return FValues[index];
            }
            set {
                TestValues();
                int index = (int)ep;
                if ((index < 0) || (index >= FValues.Length))
                { throw new WaterSim_Exception (WS_Strings.Get(WS_Strings.wseProviderOutOfRange)); }
                FValues[index] = value;
                }
        }
        /// <summary>
        ///     Indexer to get or set items within this array using an int index (overloaded)
        /// </summary>
        ///
        /// <value> The indexed item. </value>
        public double this[int index]
        {
            get
            {
                TestValues();
                if ((index < 0) || (index >= FValues.Length))
                { throw new WaterSim_Exception(WS_Strings.Get(WS_Strings.wseProviderOutOfRange)); }
                return FValues[index];
            }
            set
            {
                TestValues();
                if ((index < 0) || (index >= FValues.Length))
                { throw new WaterSim_Exception(WS_Strings.Get(WS_Strings.wseProviderOutOfRange)); }
                FValues[index] = value;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the array includes aggregates. </summary>
        ///
        /// <value> true if includes aggregates, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IncludesAggregates
        { get { return FIncludesAggregates; } }

        /// <summary>   Constructor. </summary>
        /// <param name="Default">  The default int value to set each cell at instantiation. </param>
        public ProviderDoubleArray(double Default)
        {
            FIncludesAggregates = true;
            FValues = new double[ProviderClass.NumberOfProviders];
            if (Default != 0.0) for (int i = 0; i < ProviderClass.NumberOfProviders; i++) FValues[i] = Default;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        /// <param name="Default">      The default int value to set each cell at instantiation. </param>
        /// <param name="IncludeAll">   true to include, false to exclude all. </param>
        ///-------------------------------------------------------------------------------------------------

        public ProviderDoubleArray(double Default, bool IncludeAll)
        {
            if (IncludeAll)
            {
                FIncludesAggregates = true;
                int allep = ProviderClass.NumberOfProviders + ProviderClass.NumberOfAggregates;
                FValues = new double[allep];
            }
            else
            {
                FIncludesAggregates = true;
                FValues = new double[ProviderClass.NumberOfProviders];
            }
            if (Default != 0.0) for (int i = 0; i < FValues.Length; i++) FValues[i] = Default;
        }

    }
    //--------------------------------------------------------------------------------- 
   /// <summary>
   /// An array structure for provider strings.  Each cell corresponds to a different provider.  Used primarily for labeling
   /// </summary>
    public struct ProviderStringArray
    {

        bool FIncludesAggregates;        
        /// <summary> The string values </summary>
        public string[] Values;

        /// <summary>   Constructor. </summary>
        /// <param name="Default">  The default string value to set each string to on instantiation.. </param>

        public ProviderStringArray(string Default)
        {
            FIncludesAggregates = false;
            Values = new string[ProviderClass.NumberOfProviders];
            if (Default != "") for (int i = 0; i < ProviderClass.NumberOfProviders; i++) Values[i] = Default;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        /// <param name="Default">    The default string value to set each string to on instantiation.. </param>
        /// <param name="IncludeAll">   true to include, false to exclude all. </param>
        ///-------------------------------------------------------------------------------------------------

        public ProviderStringArray(string Default, bool IncludeAll)
        {
            if (IncludeAll)
            {
                FIncludesAggregates = true;
                int allep = ProviderClass.NumberOfProviders + ProviderClass.NumberOfAggregates;
                Values = new string[allep];
            }
            else
            {
                FIncludesAggregates = false;
                Values = new string[ProviderClass.NumberOfProviders];
            }
            if (Default != "") for (int i = 0; i < Values.Length; i++) Values[i] = Default;
        }

        /// <summary>   The length of the Values array </summary>
        /// <value> The length zero based. </value>

        public int Length
        { get { return Values.Length; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the array includes aggregates. </summary>
        ///
        /// <value> true if includes aggregates, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IncludesAggregates
        { get { return FIncludesAggregates; } }

        // This is a dumb feature of structs that you can not override the parameterless constructor!
        internal void TestValues()
        {
            if (Values == null)
                Values = new string[ProviderClass.NumberOfProviders];
        }

        /// <summary>
        ///     Indexer to get or set items within this array using index syntax based on an eProvider vakue.
        /// </summary>
        ///
        /// <value> The indexed item. </value>

        public string this[eProvider ep]
        {
            get
            {
                TestValues();
                int index = (int)ep;
                if ((index < 0) || (index >= Values.Length))
                { throw new WaterSim_Exception(WS_Strings.Get(WS_Strings.wseProviderOutOfRange)); }
                return Values[index];
            }
            set
            {
                TestValues();
                int index = (int)ep;
                if ((index < 0) || (index >= Values.Length))
                { throw new WaterSim_Exception(WS_Strings.Get(WS_Strings.wseProviderOutOfRange)); }
                Values[index] = value;
            }
        }
        /// <summary>
        ///     Indexer to get or set items within this array using an int index (overloaded)
        /// </summary>
        ///
        /// <value> The indexed item. </value>
        public string this[int index]
        {
            get
            {
                TestValues();
                if ((index < 0) || (index >= Values.Length))
                { throw new WaterSim_Exception(WS_Strings.Get(WS_Strings.wseProviderOutOfRange)); }
                return Values[index];
            }
            set
            {
                TestValues();
                if ((index < 0) || (index >= Values.Length))
                { throw new WaterSim_Exception(WS_Strings.Get(WS_Strings.wseProviderOutOfRange)); }
                Values[index] = value;
            }
        }

    }
    //----------------------------------------------------------------
   /// <summary>
   /// An array structure for ModelParamter integer data for Base parameters.
    /// <remarks>Each cell corresponds to a different ModelParmeter value. See ParameterManager for indexes to array.</remarks>  
   /// </summary>
    public struct ModelParameterBaseArray
    {
        /// <summary> The values </summary>
        public int[] Values;

        /// <summary>   The length of the Values array </summary>
        /// <value> The length zero based. </value>

        public int Length
        { get { return Values.Length; } }

        /// <summary>
        ///     Indexer to get or set items within this array using an int index (overloaded)
        /// </summary>
        ///
        /// <value> The indexed item. </value>

        public int this[int index]
        {
            get 
            {
               return Values[index];
            }
            set { Values[index] = value; }
        }

        /// <summary>   Constructor. </summary>
        /// <param name="size"> The size of the BaseArray. </param>

        public ModelParameterBaseArray(int size)
        {
            Values = new int[size];
        }
    }
    //----------------------------------------------------------------
   /// <summary>
    /// An Array structure for ModelParameters for Provider Data.  
    /// <remarks>Each cell corresponds to a different ModelParmeter value. See ParameterManager for indexes to array.</remarks> 
   /// </summary>
    public struct ModelParameterProviderArray
    {

        /// <summary> The values </summary>
        
        public ProviderIntArray[] Values;

        /// <summary>   Constructor. </summary>
        /// <param name="size"> The size of the array</param>

        public ModelParameterProviderArray(int size)
        {
            Values = new ProviderIntArray[size];
            for (int i = 0; i < size; i++) Values[i] = new ProviderIntArray(0);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        /// <param name="size">                 The size of the array. </param>
        /// <param name="IncludeAggregates">    true to include, false to exclude the aggregates. </param>
        ///-------------------------------------------------------------------------------------------------

        public ModelParameterProviderArray(int size, bool IncludeAggregates)
        {
            Values = new ProviderIntArray[size];
            for (int i = 0; i < size; i++) Values[i] = new ProviderIntArray(0,IncludeAggregates);
        }

        /// <summary>   The length of the Values array </summary>
        /// <value> The length zero based. </value>

        public int Length
        { get { return Values.Length; } }


        /// <summary>
        ///     Indexer to get or set items within this array using an int index (overloaded)
        /// </summary>
        ///
        /// <value> The indexed item. </value>
        public ProviderIntArray this[int index]
        {
            get { return Values[index]; }
            set { Values[index] = value; }
        }
    }
    //--------------------------------------------------------------------------
    // DAS
    public struct ProviderDoubleDoubleArray
    {
        internal bool FIncludesAggregates;
        /// <summary> The Double values array</summary>
        public double[,] FValues;
        public int year;

            //public ProviderDoubleDoubleArray()
            //{
            //    FValues = new double[100, ProviderClass.NumberOfProviders];
            //    FIncludesAggregates = false;
            //    year = 2000;
            //}
        
        /// <summary>   The length of the Values array </summary>
        /// <value> The length zero based. </value>
        public int Length
        {
            get
            {
                TestValues();
                return FValues.Length;
            }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the values. </summary>
        ///
        /// <value> The values. </value>
        ///------------------------------------------------------------------------------------------------
        public double[,] Values
        {
            get
            {
                TestValues();
                return FValues;
            }
            set
            {
                FValues = value;
            }
        }
        // This is a dumb feature of structs that you can not override the parameterless constructor!
        internal void TestValues()
        {
            if (FValues == null)
                FValues = new double[100, ProviderClass.NumberOfProviders];
        }

        /// <summary>
        ///     Indexer to get or set items within this array using index syntax based on an eProvider vakue.
        /// </summary>
        ///
        /// <value> The indexed item. </value>

        public double this[int year, eProvider ep]
        {
            get
            {
                TestValues();
                int index = (int)ep;
                if ((index < 0) || (index >= FValues.Length))
                { throw new WaterSim_Exception(WS_Strings.Get(WS_Strings.wseProviderOutOfRange)); }
                return FValues[year, index];
            }
            set
            {
                TestValues();
                int index = (int)ep;
                if ((index < 0) || (index >= FValues.Length))
                { throw new WaterSim_Exception(WS_Strings.Get(WS_Strings.wseProviderOutOfRange)); }
                FValues[year, index] = value;
            }
        }
        /// <summary>
        ///     Indexer to get or set items within this array using an int index (overloaded)
        /// </summary>
        ///
        /// <value> The indexed item. </value>
        public double this[int year, int index]
        {
            get
            {
                TestValues();
                if ((index < 0) || (index >= FValues.Length))
                { throw new WaterSim_Exception(WS_Strings.Get(WS_Strings.wseProviderOutOfRange)); }
                return FValues[year, index];
            }
            set
            {
                TestValues();
                if ((index < 0) || (index >= FValues.Length))
                { throw new WaterSim_Exception(WS_Strings.Get(WS_Strings.wseProviderOutOfRange)); }
                FValues[year, index] = value;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the array includes aggregates. </summary>
        ///
        /// <value> true if includes aggregates, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IncludesAggregates
        { get { return FIncludesAggregates; } }

        /// <summary>   Constructor. </summary>
        /// <param name="Default">  The default int value to set each cell at instantiation. </param>
        public ProviderDoubleDoubleArray(int year, double Default)
        {
            this.year = 0;
            FIncludesAggregates = true;
            FValues = new double[100, ProviderClass.NumberOfProviders];
            if (Default != 0.0)
                for (int j = 0; j < 100; j++)
                {
                    for (int i = 0; i < ProviderClass.NumberOfProviders; i++) FValues[j, i] = Default;
                }
        }

           

    }
    // -------------------------------------------------------------------------
  /// <summary>
  /// Struct to hold base and Provider Inputs
  /// </summary>
  /// <remarks>The ParameterManagerClass provides a method to create this struct without passing sizes to the constructor.
  ///          This is a dynamic class and the contents and structure are not static.  Even when constructed with the ParamManager method
  ///          the contents can be dynamic over time.  For example, if a SimulationInputs object is created, and then an input parameter is added (or deleted)
  ///          using the ParamManger, the next object created with NewSimulationInputs will be different.</remarks>
  /// <seealso cref="ParameterManagerClass.NewSimulationInputs"/>
  /// <example><code>
  ///          WaterSimManager WSim = new WaterSimManager("Temp","Data");
  /// 		   SimulationInputs MySI = WSim.ParamManager.NewSimulationInputs();
  ///		   
  /// </code></example>
    public struct SimulationInputs
  {
      // for aggregates
      internal bool FIncludeAggregates;
      /// <summary> Name of the scenario for these inputs</summary>
      public string ScenarioName;

      /// <summary> The version of API and Model used to create these Inputs</summary>
      public string Version;

      /// <summary> Date/Time of when simulation inputs were set</summary>
      public DateTime When; 
        
      /// <summary>
      /// Base Input Values
      /// </summary>
      public ModelParameterBaseArray BaseInput;
      /// <summary>
      /// the eModelParam for each parameter in BaseInput
      /// </summary>
      public int[] BaseInputModelParam;

      /// <summary>
      /// Provider Input Values
      /// </summary>
      public ModelParameterProviderArray ProviderInput;

      /// <summary>
      /// the eModelParam for each parameter in ProviderInput
      /// </summary>
      public int[] ProviderInputModelParam;

      ///-------------------------------------------------------------------------------------------------
      /// <summary> Index For eModelParam in BaseInputs. </summary>
      /// <param name="emp"> The eModelParam to find an index  </param>
      /// <returns> index into Baseinputs if found otherwise -1 if not</returns>
      ///-------------------------------------------------------------------------------------------------
      public int BaseIndex(int emp)
      {
          int index = -1;
          int cnt = 0;
          if (ModelParamClass.valid(emp))
          {
              foreach (int _emp in BaseInputModelParam)
              {
                  if (_emp == emp)
                  {
                      index = cnt;
                      break;
                  }
                  else
                      cnt++;
              }
          }
          return index;
      }

      ///-------------------------------------------------------------------------------------------------
      /// <summary> Index For eModelParam in ProviderInputs. </summary>
      /// <param name="emp"> The eModelParam to find an index  </param>
      /// <returns> index into ProviderInputs if found otherwise -1 if not</returns>
      ///-------------------------------------------------------------------------------------------------
      public int ProviderIndex(int emp)
      {
          int index = -1;
          int cnt = 0;
          if (ModelParamClass.valid(emp))
          {
              foreach (int _emp in ProviderInputModelParam)
              {
                  if (_emp == emp)
                  {
                      index = cnt;
                      break;
                  }
                  else
                      cnt++;
              }
          }
          return index;
      }

      ///-------------------------------------------------------------------------------------------------
      /// <summary>   Gets a value indicating whether the aggregates are included. </summary>
      ///
      /// <value> true if aggregates included, false if not. </value>
      ///-------------------------------------------------------------------------------------------------

      public bool AggregatesIncluded
      { get { return FIncludeAggregates; } }

        /// <summary>   Constructor. </summary>
      /// <param name="BaseInputSize">        Size of the base input array. </param>
      /// <param name="ProviderInputSize">    Size of the provider input array. </param>
      
      public SimulationInputs(int BaseInputSize, int ProviderInputSize)
      {
          When = DateTime.Now;
          Version = "";
          ScenarioName = "";
          BaseInput = new ModelParameterBaseArray(BaseInputSize);
          BaseInputModelParam = new int[BaseInputSize];
          ProviderInput = new ModelParameterProviderArray(ProviderInputSize);
          ProviderInputModelParam = new int[ProviderInputSize];
          FIncludeAggregates = false;
      }

      ///-------------------------------------------------------------------------------------------------
      /// <summary> Constructor. </summary>
      ///
      /// <param name="BaseInputSize">      Size of the base input array. </param>
      /// <param name="ProviderInputSize">  Size of the provider input array. </param>
      /// <param name="IncludeRegions">     true to include, false to exclude the regions. </param>
      ///-------------------------------------------------------------------------------------------------

      public SimulationInputs(int BaseInputSize, int ProviderInputSize, bool IncludeRegions)
      {
          When = DateTime.Now;
          Version = "";
          ScenarioName = "";
          BaseInput = new ModelParameterBaseArray(BaseInputSize);
          BaseInputModelParam = new int[BaseInputSize];
          ProviderInput = new ModelParameterProviderArray(ProviderInputSize,IncludeRegions);
          ProviderInputModelParam = new int[ProviderInputSize];
          FIncludeAggregates = IncludeRegions;
      }
  }
    //----------------------------------------------------------------------
    /// <summary>
    /// Struct to hold base and Provider Outputs
    /// <remarks>The ParameterManagerClass provides a method to create this struct without passing sizes to the constructor</remarks>
    /// <seealso cref="ParameterManagerClass.NewSimulationOutputs"/>
    /// <example><code>
    ///          WaterSimManager WSim = new WaterSimManager("Temp","Data");
    /// 		   SimulationOutputs MySO = WSim.ParamManager.NewSimulationOutputs();
    ///		   
    /// </code></example>
    /// </summary>
    public struct SimulationOutputs
    {
        internal bool FIncludeAggregates;
        /// <summary> Name of the scenario for these outputs</summary>
        public string ScenarioName;
        /// <summary> The version of API and Model used to create these outputs</summary>
        public string Version;
        /// <summary> Date/Time of when simulation outputs were set</summary>
        public DateTime When;
        /// <summary>
        /// Base Output Values
        /// </summary>
        public ModelParameterBaseArray BaseOutput;
        /// <summary>
        /// Provider Output Values
        /// </summary>
        public ModelParameterProviderArray ProviderOutput;

        /// <summary>
        /// the eModelParam for each parameter in BaseInput
        /// </summary>
        public int[] BaseOutputModelParam;

        /// <summary>
        /// the eModelParam for each parameter in ProviderInput
        /// </summary>
        public int[] ProviderOutputModelParam;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the aggregates are included. </summary>
        ///
        /// <value> true if aggregates included, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool AggregatesIncluded
        { get { return FIncludeAggregates; } }
       
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Index For eModelParam in BaseOutputs. </summary>
        /// <param name="emp"> The eModelParam to find an index  </param>
        /// <returns> index into BaseOutputs if found otherwise -1 if not</returns>
        ///-------------------------------------------------------------------------------------------------
        public int BaseIndex(int emp)
        {
            int index = -1;
            int cnt = 0;
            if (ModelParamClass.valid(emp))
            {
                foreach (int _emp in BaseOutputModelParam)
                {
                    if (_emp == emp)
                    {
                        index = cnt;
                        break;
                    }
                    else
                        cnt++;
                }
            }
            return index;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Index For eModelParam in ProviderOutputs. </summary>
        /// <param name="emp"> The eModelParam to find an index  </param>
        /// <returns> index into ProviderOutputs if found otherwise -1 if not</returns>
        ///-------------------------------------------------------------------------------------------------
        public int ProviderIndex(int emp)
        {
            int index = -1;
            int cnt = 0;
            if (ModelParamClass.valid(emp))
            {
                foreach (int _emp in ProviderOutputModelParam)
                {
                    if (_emp == emp)
                    {
                        index = cnt;
                        break;
                    }
                    else
                        cnt++;
                }
            }
            return index;
        }


        /// <summary>   Constructor. </summary>
        /// <param name="BaseOutputSize">        Size of the base output array. </param>
        /// <param name="ProviderOutputSize">    Size of the provider output array. </param>

        public SimulationOutputs(int BaseOutputSize, int ProviderOutputSize)
        {
            FIncludeAggregates = false;
            When = DateTime.Now;
            Version = "";
            ScenarioName = "";
            BaseOutput = new ModelParameterBaseArray(BaseOutputSize);
            BaseOutputModelParam = new int[BaseOutputSize];
            ProviderOutput = new ModelParameterProviderArray(ProviderOutputSize);
            ProviderOutputModelParam = new int[ProviderOutputSize];
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>

        /// <param name="BaseOutputSize">       Size of the base output array. </param>
        /// <param name="ProviderOutputSize">   Size of the provider output array. </param>
        /// <param name="IncludeAggregates">    true to include, false to exclude the aggregates. </param>
        ///-------------------------------------------------------------------------------------------------

        public SimulationOutputs(int BaseOutputSize, int ProviderOutputSize, bool IncludeAggregates)
        {
            FIncludeAggregates = IncludeAggregates;
            When = DateTime.Now;
            Version = "";
            ScenarioName = "";
            BaseOutput = new ModelParameterBaseArray(BaseOutputSize);
            BaseOutputModelParam = new int[BaseOutputSize];
            ProviderOutput = new ModelParameterProviderArray(ProviderOutputSize,IncludeAggregates);
            ProviderOutputModelParam = new int[ProviderOutputSize];
        }

    }
    //----------------------------------------------------------------------
    
    /// <summary>
    /// A structure for one years worth of Simulation Results
    /// </summary>
    public struct AnnualSimulationResults
    {
        /// <summary>
        ///  Year of Simulation
        /// </summary>
        public int year;
        /// <summary> The outputs. </summary>
        public SimulationOutputs Outputs;
        /// <summary> The inputs. </summary>
        public SimulationInputs Inputs;

        /// <summary>   Constructor. </summary>
        /// <param name="BaseOutputSize">       Size of the base output array. </param>
        /// <param name="ProviderOutputSize">   Size of the provider output array. </param>
        /// <param name="BaseInputSize">        Size of the base input array. </param>
        /// <param name="ProviderInputSize">    Size of the provider input array. </param>

        public AnnualSimulationResults(int BaseOutputSize, int ProviderOutputSize, int BaseInputSize, int ProviderInputSize)
        {
            year = 0;
            Outputs = new SimulationOutputs(BaseOutputSize, ProviderOutputSize);
            Inputs = new SimulationInputs(BaseInputSize, ProviderInputSize);

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        /// <param name="BaseOutputSize">       Size of the base output array. </param>
        /// <param name="ProviderOutputSize">   Size of the provider output array. </param>
        /// <param name="BaseInputSize">        Size of the base input array. </param>
        /// <param name="ProviderInputSize">    Size of the provider input array. </param>
        /// <param name="IncludeAggregates">    true to include, false to exclude the aggregates. </param>
        ///-------------------------------------------------------------------------------------------------

        public AnnualSimulationResults(int BaseOutputSize, int ProviderOutputSize, int BaseInputSize, int ProviderInputSize, bool IncludeAggregates)
        {
            year = 0;
            Outputs = new SimulationOutputs(BaseOutputSize, ProviderOutputSize, IncludeAggregates);
            Inputs = new SimulationInputs(BaseInputSize, ProviderInputSize, IncludeAggregates);

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets parameter index.</summary>
        ///
        /// <remarks> Quay, 3/2/2018.</remarks>
        ///
        /// <param name="MP"> The mp.</param>
        ///
        /// <returns> The parameter index.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int GetParmIndex(ModelParameterClass MP)
        {
            int ArrayIndex = -1;
            if (MP.isProviderParam)
            {
                if (MP.isInputParam)
                {
                    ArrayIndex = Inputs.ProviderIndex(MP.ModelParam);
                }
                else
                {
                    ArrayIndex = Outputs.ProviderIndex(MP.ModelParam);
                }
            }
            else
            if (MP.isBaseParam)
            {
                if (MP.isInputParam)
                {
                    ArrayIndex = Inputs.BaseIndex(MP.ModelParam);
                }
                else
                {
                    ArrayIndex = Inputs.ProviderIndex(MP.ModelParam);
                }
            }
            else
            {
                ArrayIndex = -1;
            }
            return ArrayIndex;
        }

        public int[] ModelParam()
        {
            int ParmCnt = Outputs.ProviderOutput.Values.Length + Outputs.BaseOutput.Values.Length+Inputs.BaseInput.Values.Length + Inputs.ProviderInput.Values.Length;
            int[] TheParams = new int[ParmCnt];
            int cnt = 0;
            foreach (int parmcode in Outputs.ProviderOutputModelParam)
            {
                TheParams[cnt] = parmcode;
                cnt++;
            }
            foreach (int parmcode in Outputs.BaseOutputModelParam)
            {
                TheParams[cnt] = parmcode;
                cnt++;
            }
            foreach (int parmcode in Inputs.BaseInputModelParam)
            {
                TheParams[cnt] = parmcode;
                cnt++;
            }
            foreach (int parmcode in Inputs.ProviderInputModelParam)
            {
                TheParams[cnt] = parmcode;
                cnt++;
            }
            return TheParams;
        }

    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Simulation results. </summary>
    ///-------------------------------------------------------------------------------------------------

    public class SimulationResults
    {
        AnnualSimulationResults[] FSimResults;
        int FStartYear = 0;
        int FStopYear = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="years">                The years. </param>
        /// <param name="StartYear">            The start year. </param>
        /// <param name="BaseOutputSize">       Size of the base output. </param>
        /// <param name="ProviderOutputSize">   Size of the provider output. </param>
        /// <param name="BaseInputSize">        Size of the base input. </param>
        /// <param name="ProviderInputSize">    Size of the provider input. </param>
        ///-------------------------------------------------------------------------------------------------

        public SimulationResults(int years, int StartYear, int BaseOutputSize, int ProviderOutputSize, int BaseInputSize, int ProviderInputSize)
        {
            SetUpFields(years, StartYear, BaseOutputSize, ProviderOutputSize, BaseInputSize, ProviderInputSize, false);
        }

        public SimulationResults(int years, int StartYear, int BaseOutputSize, int ProviderOutputSize, int BaseInputSize, int ProviderInputSize, bool IncludeAggregates)
        {
            SetUpFields(years, StartYear, BaseOutputSize, ProviderOutputSize, BaseInputSize, ProviderInputSize, IncludeAggregates);
        }

        
        internal void SetUpFields(int years, int StartYear, int BaseOutputSize, int ProviderOutputSize, int BaseInputSize, int ProviderInputSize, bool IncludeAggregates)
        {

            ProviderIntArray Outblank = new ProviderIntArray(0, IncludeAggregates);
            ProviderIntArray Inblank = new ProviderIntArray(0, false);

            FSimResults = new AnnualSimulationResults[years];
            for (int yr = 0; yr < years; yr++)
            {
                AnnualSimulationResults TheASR = new AnnualSimulationResults(BaseOutputSize, ProviderOutputSize, BaseInputSize, ProviderInputSize, IncludeAggregates );

                int cnt = BaseOutputSize;
                for (int i = 0; i < cnt; i++)
                {
                    TheASR.Outputs.BaseOutput[i] = 0;
                    TheASR.Outputs.BaseOutputModelParam[i] = -1;
                }
                cnt = ProviderOutputSize;
                for (int parmi = 0; parmi < cnt; parmi++)
                {
                    TheASR.Outputs.ProviderOutput[parmi] = Outblank;
                    TheASR.Outputs.ProviderOutputModelParam[parmi] = -1;
                }
                cnt = BaseInputSize;
                for (int parmi = 0; parmi < cnt; parmi++)
                {
                    TheASR.Inputs.BaseInput[parmi] = 0;
                    TheASR.Inputs.BaseInputModelParam[parmi] = -1;
                }
                cnt = ProviderInputSize;
                for (int parmi = 0; parmi < cnt; parmi++)
                {
                    TheASR.Inputs.ProviderInput[parmi] = Inblank;
                    TheASR.Inputs.ProviderInputModelParam[parmi] = -1;
                }
                FSimResults[yr] = TheASR;
            }
            FStartYear = StartYear;
            FStopYear = (StartYear + years) - 1;
         }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Indexer to get items within this collection using array index syntax. </summary>
        ///
        /// <param name="index">    Zero-based index of the entry to access. </param>
        ///
        /// <returns>   The indexed item. </returns>
        ///-------------------------------------------------------------------------------------------------

        public AnnualSimulationResults this[int index]
        {
            get { return FSimResults[index]; }
            set { FSimResults[index] = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the last year. </summary>
        ///
        /// <value> The last year. </value>
        ///-------------------------------------------------------------------------------------------------

        public int LastYear
        {
            get { return FStopYear; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the start year. </summary>
        ///
        /// <value> The start year. </value>
        ///-------------------------------------------------------------------------------------------------

        public int StartYear
        { 
            get { return FStartYear; }
            set
            { 
                FStartYear = value;
                FStopYear = (FStartYear + Length) - 1;
            }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the number of years in the array of AnnualResults</summary>
        /// <value> The length. </value>
        ///-------------------------------------------------------------------------------------------------

        public int Length
        {
            get { return FSimResults.Length; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets AnnualResults By year. </summary>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="year"> The year. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public AnnualSimulationResults ByYear(int year)
        {
            if ((year >= FStartYear) & (year <= FStopYear))
            {
                //das int index = year - FStartYear ;
                int index = year - FStartYear ;
                return FSimResults[index];
            }
            else
            {
                throw new Exception("Year [" + year.ToString() + "] is out of range : " + FStartYear.ToString() + " to " + FStopYear.ToString());
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the array of AnnualResults. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public AnnualSimulationResults[] GetAllYears()
        {
            return FSimResults;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the enumerator. </summary>
        /// <returns>   The enumerator. </returns>
        ///-------------------------------------------------------------------------------------------------

        public IEnumerator<AnnualSimulationResults> GetEnumerator()
        {
            foreach (AnnualSimulationResults ARS in FSimResults)
            {
                yield return ARS;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Output provider parameter count.</summary>
        ///
        /// <remarks> Quay, 3/2/2018.</remarks>
        ///
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int OutputProviderParameterCount()
        {
            int cnt = 0;
            if (FSimResults != null)
            {
                cnt = FSimResults[0].Outputs.ProviderOutput.Values.Length;
            }
            return cnt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Output base parameter count.</summary>
        ///
        /// <remarks> Quay, 3/2/2018.</remarks>
        ///
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int OutputBaseParameterCount()
        {
            int cnt = 0;
            if (FSimResults != null)
            {
                cnt = FSimResults[0].Outputs.BaseOutput.Values.Length;
            }
            return cnt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Input provider parameter count.</summary>
        ///
        /// <remarks> Quay, 3/2/2018.</remarks>
        ///
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int InputProviderParameterCount()
        {
            int cnt = 0;
            if (FSimResults != null)
            {
                cnt = FSimResults[0].Inputs.ProviderInput.Values.Length;
            }
            return cnt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Input base parameter count.</summary>
        ///
        /// <remarks> Quay, 3/2/2018.</remarks>
        ///
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int InputBaseParameterCount()
        {
            int cnt = 0;
            if (FSimResults != null)
            {
                cnt = FSimResults[0].Inputs.BaseInput.Values.Length;
            }
            return cnt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Input parameter count.</summary>
        ///
        /// <remarks> Quay, 3/2/2018.</remarks>
        ///
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int InputParameterCount()
        {
            int cnt = 0;
            if (FSimResults != null)
            {
                cnt = FSimResults[0].Inputs.BaseInput.Values.Length;
                cnt += FSimResults[0].Inputs.ProviderInput.Values.Length;

            }
            return cnt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Output parameter count.</summary>
        ///
        /// <remarks> Quay, 3/2/2018.</remarks>
        ///
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int OutputParameterCount()
        {
            int cnt = 0;
            if (FSimResults != null)
            {
                cnt = FSimResults[0].Outputs.BaseOutput.Values.Length;
                cnt += FSimResults[0].Outputs.ProviderOutput.Values.Length;
            }
            return cnt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Parameter count.</summary>
        ///
        /// <remarks> Quay, 3/2/2018.</remarks>
        ///
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int ParameterCount()
        {
            return InputParameterCount() + OutputParameterCount();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets parameter index.</summary>
        ///
        /// <remarks> Quay, 3/2/2018.</remarks>
        ///
        /// <param name="MP"> The mp.</param>
        ///
        /// <returns> The parameter index.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int GetParmIndex(ModelParameterClass MP)
        {
            int ArrayIndex = FSimResults[0].GetParmIndex(MP);
            return ArrayIndex; 
        }

        public int[] ModelParam()
        {
            return FSimResults[0].ModelParam();
        }
    }
    //----------------------------------------------------------------------------

    /// <summary>   Simulation strings for Labels.  </summary>
    public struct SimulationStrings
    {

        /// <summary> The base output </summary>
        public string[] BaseOutput;

        /// <summary> The base input </summary>
        public string[] BaseInput;

        /// <summary> The provider output </summary>
        public string[] ProviderOutput;

        /// <summary> The provider input </summary>
        public string[] ProviderInput;

        /// <summary>   Constructor. </summary>
        /// <param name="BaseOutputSize">       Size of the base output. </param>
        /// <param name="ProviderOutputSize">   Size of the provider output. </param>
        /// <param name="BaseInputSize">        Size of the base input. </param>
        /// <param name="ProviderInputSize">    Size of the provider input. </param>

        public SimulationStrings(int BaseOutputSize, int ProviderOutputSize, int BaseInputSize, int ProviderInputSize)
        {
            BaseOutput = new string[BaseOutputSize];
            BaseInput = new string[BaseInputSize];
            ProviderOutput = new string[ProviderOutputSize];
            ProviderInput = new string[ProviderInputSize];
        }
    }
        //----------------------------------------------------------------------------
    #endregion


        /****************************************************************
     * Parameter Classes, Routines, Contants and enums
     * ************************************************************/

    //====================================================================================
    #region Report
    //----------------------------------------------------

    /// <summary>   Report header.  </summary>
    
    public class ReportHeader
    {

        /// <summary> The lines of the header, 1, 2 or 3</summary>
        public string[] Lines;

        /// <summary>   Default constructor. </summary>
  
        public ReportHeader()
        {
            Lines = new string[3];
        }
        // indexer

        /// <summary>
        ///     Indexer to get or set items within this collection using array index syntax.
        /// </summary>
        ///
        /// <value> The indexed item. </value>

        public string this[int index]
        {
            get { return Lines[index]; }
            set { Lines[index] = value; }
        }

        /// <summary>   Gets the length of the lines array 1, 2 or 3. </summary>
        ///
        /// <value> The length. </value>

        public int Length
        { get { return Lines.Length; } }
    }
    //---------------------------------------------------------

    /// <summary>   Report class.  </summary>
    /// <remarks> static class with methods for creating and processing reports and report headers</remarks>
    public static class ReportClass
    {
        const int top = 0;
        const int bottom = 1;
        //---------------------------------------------------------
        static string[] ParseLabel(string value, int size)
        {
            string[] templines = new string[2];
            if (value.Length > size)
            {
                int Len = value.Length;
                int start = (Len - size);
                int pos = 0;
                int i = start;
                do
                {
                    if (value[i] == ' ')
                        pos = i;
                    i++;
                } while ((i < Len) & (pos == 0));
                if (pos == 0)
                {
                    templines[top] = value.Substring(0, size);
                    templines[bottom] = value.Substring(size, start);
                }
                else
                {
                    templines[top] = value.Substring(0, pos);
                    pos++;
                    templines[bottom] = value.Substring(pos, (Len - pos));
                }
            }
            else
            {
                templines[bottom] = value;
                templines[top] = "";
            }

            return templines;
        }

        /// <summary> The parameter input label used for the report header, labels each paramter as input</summary>
        public static string ParamInputLabel = "Input";

        /// <summary> The parameter output label used for the report header, labels each paramter as output</summary>
        public static string ParamOutputLabel = "Output";
        //---------------------------------------------------------

        /// <summary>   Parameter header. </summary>
        ///
        /// <remarks>   Ray, 8/5/2011. </remarks>
        ///
        /// <param name="_header">      [in,out] The header. </param>
        /// <param name="ClearHeader">  true to clear header otherwise adds to what ever is passed with _header. </param>
        /// <param name="columnSize">   width of each column. </param>
        /// <param name="UseTabs">      true to use tabs otherwise pads with spaces. </param>
        /// <param name="Use2ndLine">   true to use 2nd line for label wrapping into column size, otherwise truncates label to fit column size. </param>
        /// <param name="Parms">        The parms enumerator to use. </param>

        public static void ParamHeader(ref ReportHeader _header, bool ClearHeader, int columnSize, bool UseTabs, bool Use2ndLine, IEnumerable<ModelParameterBaseClass> Parms)
        {
            string[] tempLines = new string[_header.Length];
            string[] parsedLines = new string[2];
            string temp = "";
            int LineNum = _header.Length;
            if (ClearHeader)   // set header to ""
                for (int i = 0; i < _header.Length; i++)
                    _header[i] = "";
            // get the input Base values
            foreach (ModelParameterClass Mp in Parms) //WSim.ParamManager.BaseInputs())
            {
                temp = Mp.Label;
                if (Mp.isInputParam) tempLines[2] = ParamInputLabel;
                else tempLines[2] = ParamOutputLabel; ;


                if ((columnSize > 0) & (temp.Length > (columnSize - 1)))
                {
                    if (Use2ndLine)
                    {
                        parsedLines = ParseLabel(temp, columnSize - 1);
                        tempLines[0] = parsedLines[0];
                        tempLines[1] = parsedLines[1];

                    }
                    else
                    {
                        tempLines[bottom] = temp.Substring(0, columnSize - 1);
                    }
                }
                else
                {
                    if (Use2ndLine)
                    {
                        tempLines[bottom] = temp;
                        tempLines[top] = "";
                    }
                    else
                    {
                        tempLines[0] = temp;
                        tempLines[1] = "";
                    }
                }

                if (!UseTabs)
                {
                    if (columnSize > 0)
                    {
                        for (int i = 0; i < LineNum; i++)
                        {
                            if (tempLines[i].Length > columnSize)
                                tempLines[i] = tempLines[i].Substring(0, columnSize - 1);
                            _header.Lines[i] += tempLines[i].PadLeft(columnSize, ' ');
                        }
                    }
                    else
                    {
                        for (int i = 0; i < LineNum; i++)
                            _header.Lines[i] += " " + tempLines[i];
                    }
                }
                else
                {
                    for (int i = 0; i < LineNum; i++)
                        _header.Lines[i] += " \t" + tempLines[i];
                }

            }
        }
        ////---------------------------------------------------------
        
        //-------------------------------------------------------------------------------------

        /// <summary>   Full header. </summary>
        ///
        /// <remarks>   Creates a full reportheader, all parameters plus year, scenario name, and provider label and proivider fieldname </remarks>
        ///
        /// <param name="WSim">         The WaterSimManager class object being used for Simulation. </param>
        /// <param name="columnSize">   width of each column. </param>
        /// <param name="UseTabs">      true to use tabs otherwise pads with spaces. </param>
        /// <param name="Use2ndLine">   true to use 2nd line for label wrapping into column size,
        ///                             otherwise truncates label to fit column size. </param>
        ///
        /// <returns> a ReportHeader <see cref="ReportHeader"/> </returns>

        public static ReportHeader FullHeader(WaterSimManager WSim, int columnSize, bool UseTabs, bool Use2ndLine)
        {
            ReportHeader temp = new ReportHeader();
            ReportHeader header = new ReportHeader();

            string pTemp = "Provider";
            string yTemp = "Year";
            string sTemp = "Scenario";
            string blank = "";

            if (columnSize != 0)
            {
                if (pTemp.Length > columnSize) pTemp = pTemp.Substring(0, columnSize - 1);
                if (yTemp.Length > columnSize) yTemp = yTemp.Substring(0, columnSize - 1);
                if (sTemp.Length > columnSize) sTemp = sTemp.Substring(0, columnSize - 1);
            }
            if (UseTabs)
            {
                pTemp = " \t" + pTemp;
                yTemp = " \t" + yTemp;
                sTemp = " \t" + sTemp;
                blank = " \t" + blank; ;
            }
            else
            {
                if (columnSize == 0)
                {
                    pTemp = " " + pTemp;
                    yTemp = " " + yTemp;
                    sTemp = " " + sTemp;
                    blank = " " + blank;
                }
                else
                {
                    pTemp = pTemp.PadLeft(columnSize, ' ');
                    yTemp = yTemp.PadLeft(columnSize, ' ');
                    sTemp = sTemp.PadLeft(columnSize, ' ');
                    blank = blank.PadLeft(columnSize, ' ');
                }

            }
            if (Use2ndLine)
            {

                header[1] = sTemp + yTemp + pTemp;
                header[0] = blank + blank + blank;
            }
            else
                header[0] = sTemp + yTemp + pTemp;


            header[2] = blank + blank + blank;
            ParamHeader(ref header, false, columnSize, UseTabs, Use2ndLine, WSim.ParamManager.BaseOutputs());
            //header[0] += temp[0];
            //header[1] += temp[1];
            ParamHeader(ref header, false, columnSize, UseTabs, Use2ndLine, WSim.ParamManager.ProviderOutputs());
            //header[0] += temp[0];
            //header[1] += temp[1];
            ParamHeader(ref header, false, columnSize, UseTabs, Use2ndLine, WSim.ParamManager.BaseInputs());
            //header[0] += temp[0];
            //header[1] += temp[1];
            ParamHeader(ref header, false, columnSize, UseTabs, Use2ndLine, WSim.ParamManager.ProviderInputs());
            //header[0] += temp[0];
            //header[1] += temp[1];

            return header;
        }

        //--------------------------------------------------------------------------------------

        /// <summary>   Annual full data. </summary>
        ///
        /// <remarks>   returns an array of strings, one string for each provider with the value of all parameters in order to match report header. Essentiall the report header strings plus these strings is all the input and output data for one Simulation year.  Reads the current data from the paramters for the WaterSimManager class object passed.</remarks>
        ///
        /// <param name="WSim">         The WaterSimManager class object being used for Simulation. </param>
        /// <param name="ScenarioName"> Name of the scenario. </param>
        /// <param name="year">         The year. </param>
        /// <param name="columnSize">   width of each column. </param>
        /// <param name="UseTabs">      true to use tabs otherwise pads with spaces. </param>
        ///
        /// <returns>   . </returns>

        public static string[] AnnualFullData(WaterSimManager WSim, string ScenarioName, int year, int columnSize, bool UseTabs)
        {
            ModelParameterBaseClass Mp;
            int parmvalue = 0;
            int index = 0;
            string temp = "";
            string close = "";
            string InputBaseLine = "";
            string OutputBaseLine = "";
            string OutputProviderLine = "";
            string InputProviderLine = "";
            int ProviderSize = ProviderClass.NumberOfProviders;
            ProviderStringArray AllStrings = new ProviderStringArray("");


            if (UseTabs)
            {
                close = System.Environment.NewLine;
            }

            // get the output values

            foreach (int emp in WSim.ParamManager.eModelParameters())
            {
                Mp = WSim.ParamManager.Model_Parameter(emp);

                if (Mp.ParamType == modelParamtype.mptOutputBase)
                {
                    parmvalue = (Mp as ModelParameterClass).Value;  // 7/29 (Mp as BaseModelParameterClass).Value;
                    if (!UseTabs) temp = parmvalue.ToString().PadLeft(columnSize, ' ');
                    else temp = " \t" + parmvalue.ToString();
                    OutputBaseLine += temp;
                }
            }
            // get the input values
            foreach (int emp in WSim.ParamManager.eModelParameters())
            {
                Mp = WSim.ParamManager.Model_Parameter(emp);
                if (Mp.ParamType == modelParamtype.mptInputBase)
                {
                    parmvalue = (Mp as ModelParameterClass).Value;  // 7/29 (Mp as BaseModelParameterClass).Value;
                    if (!UseTabs) temp = parmvalue.ToString().PadLeft(columnSize, ' ');
                    else temp = " \t" + parmvalue.ToString();
                    InputBaseLine += temp;
                }
            }
            // Now Get providers Data
            ProviderIntArray ProviderArray = new ProviderIntArray(0);

            //Provider Output Data
            int OutputProviderCnt = WSim.ParamManager.NumberOfParameters(modelParamtype.mptOutputProvider);
            int[,] ProviderOutputData = new int[OutputProviderCnt, ProviderSize];//ProviderClass.NumberOfProviders);

            index = 0;
            foreach (int emp in WSim.ParamManager.eModelParameters())
            {
                Mp = WSim.ParamManager.Model_Parameter(emp);
                if (Mp.ParamType == modelParamtype.mptOutputProvider)
                {
                    ProviderArray = Mp.ProviderProperty.getvalues();
                    foreach (eProvider p in ProviderClass.providers())
                        ProviderOutputData[index, ProviderClass.index(p)] = ProviderArray.Values[ProviderClass.index(p)];
                    index++;
                }
            }
            //Provider Input Data
            int InputProviderCnt = WSim.ParamManager.NumberOfParameters(modelParamtype.mptInputProvider);
            int[,] ProviderInputData = new int[InputProviderCnt, ProviderSize];//ProviderClass.NumberOfProviders);

            index = 0;
            foreach (int emp in WSim.ParamManager.eModelParameters())
            {
                Mp = WSim.ParamManager.Model_Parameter(emp);
                if (Mp.ParamType == modelParamtype.mptInputProvider)
                {
                    ProviderArray = Mp.ProviderProperty.getvalues();
                    foreach (eProvider p in ProviderClass.providers())
                        ProviderInputData[index, ProviderClass.index(p)] = ProviderArray.Values[ProviderClass.index(p)];
                    index++;
                }
            }

            foreach (eProvider p in ProviderClass.providers())
            {
                // Output Line
                OutputProviderLine = "";
                for (int i = 0; i < OutputProviderCnt; i++)
                {
                    if (UseTabs) OutputProviderLine += " \t" + ProviderOutputData[i, ProviderClass.index(p)].ToString();
                    else OutputProviderLine += ProviderOutputData[i, ProviderClass.index(p)].ToString().PadLeft(columnSize, ' ');
                }
                // Input Line
                InputProviderLine = "";
                for (int i = 0; i < InputProviderCnt; i++)
                {
                    if (UseTabs) InputProviderLine += " \t" + ProviderInputData[i, ProviderClass.index(p)].ToString();
                    else InputProviderLine += ProviderInputData[i, ProviderClass.index(p)].ToString().PadLeft(columnSize, ' ');
                }
                // Get the year and provider 
                temp = ProviderClass.Label(p);
                if (temp.Length > (columnSize - 1)) temp = temp.Substring(0, columnSize - 1);
                if (UseTabs) { temp = "\t" + ScenarioName + "\t" + year.ToString() + " \t" + temp; }
                else
                {
                    temp = ScenarioName.PadLeft(columnSize, ' ') + year.ToString().PadLeft(columnSize, ' ') + temp.PadLeft(columnSize, ' ');
                }

                AllStrings.Values[ProviderClass.index(p)] = temp + OutputBaseLine + OutputProviderLine + InputBaseLine + InputProviderLine + close;
            }
            return AllStrings.Values;
        }  // FullAnnualData

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Annual full data. </summary>
        ///
        /// <remarks>   Ray Quay, 1/28/2014. </remarks>
        ///
        /// <param name="SimResults">   The simulation results. </param>
        /// <param name="ScenarioName"> Name of the scenario. </param>
        /// <param name="year">         The year. </param>
        /// <param name="columnSize">   width of each column. </param>
        /// <param name="UseTabs">      true to use tabs otherwise pads with spaces. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public static string[] AnnualFullData(SimulationResults SimResults,string ScenarioName, int year, int columnSize, bool UseTabs)
        {
           
            int parmvalue = 0;
            string temp = "";
            string close = "";
            string InputBaseLine = "";
            string OutputBaseLine = "";
            string OutputProviderLine = "";
            string InputProviderLine = "";
            int ProviderInSize = ProviderClass.NumberOfProviders;
            int ProviderOutSize = ProviderInSize;
            bool useAggregates = false;
            AnnualSimulationResults ASR = SimResults.ByYear(year);

            if ((ASR.Outputs.ProviderOutput.Length > 0) && (ASR.Outputs.AggregatesIncluded))
            {
                useAggregates = true;
                ProviderOutSize = ASR.Outputs.ProviderOutput[0].Length;
            }
            ProviderStringArray AllStrings = new ProviderStringArray("",useAggregates);

            if (UseTabs)
            {
                close = System.Environment.NewLine;
            }

            // get the output values
            for (int i = 0; i < ASR.Outputs.BaseOutput.Length; i++)
            {
                parmvalue = ASR.Outputs.BaseOutput[i];  
                if (!UseTabs) temp = parmvalue.ToString().PadLeft(columnSize, ' ');
                else temp = " \t" + parmvalue.ToString();
                OutputBaseLine += temp;
            }

            for (int i = 0; i < ASR.Inputs.BaseInput.Length; i++)
            {
                parmvalue = ASR.Inputs.BaseInput[i];
                if (!UseTabs) temp = parmvalue.ToString().PadLeft(columnSize, ' ');
                else temp = " \t" + parmvalue.ToString();
                InputBaseLine += temp;
            }

            // Now Get providers Data
            //Provider Output Data
            int[,] ProviderOutputData = new int[ASR.Outputs.ProviderOutput.Length, ProviderOutSize];//ProviderClass.NumberOfProviders);
            for (int i = 0; i < ASR.Outputs.ProviderOutput.Length; i++)
            {
                ProviderIntArray ProviderArray = new ProviderIntArray(0,ASR.Outputs.AggregatesIncluded);
                for (int j = 0; j < ProviderArray.Length; j++)
                {
                    ProviderArray.Values[j] = ASR.Outputs.ProviderOutput[i].Values[j];
                    foreach (eProvider p in ProviderClass.providers(ASR.Outputs.AggregatesIncluded))
                    {
                            ProviderOutputData[i, ProviderClass.index(p, ASR.Outputs.AggregatesIncluded)] = ProviderArray.Values[ProviderClass.index(p, ASR.Outputs.AggregatesIncluded)];
                    }
                }
            }

            int[,] ProviderInputData = new int[ASR.Inputs.ProviderInput.Length, ProviderInSize];//ProviderClass.NumberOfProviders);
            for (int i = 0; i < ASR.Inputs.ProviderInput.Length; i++)
            {
                ProviderIntArray ProviderArray = new ProviderIntArray();
                ProviderArray.Values = ASR.Inputs.ProviderInput[i].Values;
                foreach (eProvider p in ProviderClass.providers())
                    ProviderInputData[i, ProviderClass.index(p)] = ProviderArray.Values[ProviderClass.index(p)];
            }

            //for (int pindex=0;pindex<ProviderOutSize;pindex++)
            foreach (eProvider p in ProviderClass.providers(ASR.Outputs.AggregatesIncluded))
            {
                // Output Line
                OutputProviderLine = "";
                for (int i = 0; i <  ASR.Outputs.ProviderOutput.Length; i++)
                {
                    if (UseTabs) OutputProviderLine += " \t" + ProviderOutputData[i, ProviderClass.index(p, useAggregates)].ToString();
                    else OutputProviderLine += ProviderOutputData[i, ProviderClass.index(p,useAggregates)].ToString().PadLeft(columnSize, ' ');
                }
                // Input Line
                InputProviderLine = "";
                if (p > ProviderClass.LastProvider)
                {
                    for (int i = 0; i < ASR.Inputs.ProviderInput.Length; i++)
                    {
                        if (UseTabs) InputProviderLine += " \t" + "na";
                        else InputProviderLine += "na".PadLeft(columnSize, ' ');
                    }
                }
                else
                {
                    for (int i = 0; i < ASR.Inputs.ProviderInput.Length; i++)
                    {
                        if (UseTabs) InputProviderLine += " \t" + ProviderInputData[i, ProviderClass.index(p,useAggregates)].ToString();
                        else InputProviderLine += ProviderInputData[i, ProviderClass.index(p,useAggregates)].ToString().PadLeft(columnSize, ' ');
                    }
                }
                // Get the year and provider 
                temp = ProviderClass.Label(p);
                if (temp.Length > (columnSize - 1)) temp = temp.Substring(0, columnSize - 1);
                if (UseTabs) { temp = "\t" + ScenarioName + "\t" + year.ToString() + " \t" + temp; }
                else
                {
                    temp = ScenarioName.PadLeft(columnSize, ' ') + year.ToString().PadLeft(columnSize, ' ') + temp.PadLeft(columnSize, ' ');
                }

                AllStrings.Values[ProviderClass.index(p,useAggregates)] = temp + OutputBaseLine + OutputProviderLine + InputBaseLine + InputProviderLine + close;
            }
            return AllStrings.Values;
        }  // FullAnnualData

    }  // Report Class
    #endregion
    //=================================================================
    #region WaterSimException
    /// <summary>   Exception for throwing water simulation errors.  </summary>

    public class WaterSim_Exception : Exception
    {

        /// <summary> Identifier string for this exception</summary>
        protected const string Pre = "WaterSim Error: ";

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        ///
        /// <remarks> Ray, 1/24/2013. </remarks>
        ///
        /// <param name="message"> The message. </param>
        ///-------------------------------------------------------------------------------------------------

        public WaterSim_Exception(string message) : base(Pre + message) { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        ///
        /// <remarks> Ray, 1/24/2013. </remarks>
        ///
        /// <param name="index"> Zero-based index of the. </param>
        ///-------------------------------------------------------------------------------------------------

        public WaterSim_Exception(int index) : base(Pre + WS_Strings.Get(index)) { }
    }

     #endregion


}


//================================================================================================
       
