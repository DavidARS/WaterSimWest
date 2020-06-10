//      WaterSimDCDC Regional Water Demand and Supply Model Version 5.0

//       This is a support classes for the C# WaterSim API, that allows Parameter support 
//       without being connected to the Forran Model//

//       WaterSimDCDC_API_ShadowPM 
//       Version 4.0
//       Keeper: Ray Quay ray.quay@asu.edu
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
//====================================================================================using System;
using System.Collections.Generic;
using System.Text;

//========================================
// ShadowPM
// A classes that facility parameters and aparameter managers that are not connected to the WaterSim Fortran Model
// Keeper Ray Quay
// 12/7/2011
// ==============================================
namespace WaterSimDCDC
{
    /// <summary>   Shadow provider array property.  </summary>
    /// <remarks>this class mimics the ProviderArrayProperty calls to the WaterSim Model, but values are stored locally.  Intended for use with the 
    /// ShadowPamerterClass</remarks>
    /// <seealso cref="WaterSimDCDC.ShadowParameterManager"/>
    class ShadowProviderArrayProperty : providerArrayProperty
    {
        //protected eModelParam Femp;
        //protected ParameterManagerClass Fpm;
        /// <summary>
        /// To sumulate model values reset
        /// </summary>
        protected ProviderIntArray Defaults;
        /// <summary>
        /// Shdow ProviderIntARray used to hold values
        /// </summary>
        protected ProviderIntArray Values =  new ProviderIntArray(0);

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <param name="modelparam"> The emodelparam value. </param>
        ///-------------------------------------------------------------------------------------------------

        public ShadowProviderArrayProperty(int modelparam)
        {
            Femp = modelparam;
            Defaults = new ProviderIntArray(0);
            for (int i = 0; i < Defaults.Length; i++)
                Defaults[i] = 0;
            Values = Defaults;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        ///
        /// <remarks> Ray, 12/7/2011. </remarks>
        ///
        /// <param name="modelparam">    The emodelparam value </param>
        /// <param name="DefaultValues"> The default values. </param>
        ///-------------------------------------------------------------------------------------------------

        public ShadowProviderArrayProperty(int modelparam, ProviderIntArray DefaultValues)
        {
            Femp = modelparam;
            Defaults = DefaultValues;
            Values = Defaults;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        ///
        /// <remarks> Ray, 12/7/2011. </remarks>
        ///
        /// <param name="pm">  The Paramerter Manager  </param>
        /// <param name="modelparam">    The emodelparam value </param>
        /// <param name="DefaultValues"> The default values. </param>
        ///-------------------------------------------------------------------------------------------------

        public ShadowProviderArrayProperty(ParameterManagerClass pm, int modelparam, ProviderIntArray DefaultValues)
        {
            Fpm = pm;
            Femp = modelparam;
            Defaults = DefaultValues;
            Values = Defaults;
            
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        ///
        /// <remarks> Ray, 12/7/2011. </remarks>
        ///
        /// <param name="pm">  The Paramerter Manager  </param>
        /// <param name="pap"> The ProviderArrayProperty to Mimick. </param>
        ///-------------------------------------------------------------------------------------------------

        public ShadowProviderArrayProperty(ParameterManagerClass pm, providerArrayProperty pap) : this(pm, pap.ModelParam, pap.getvalues()) 
        {
            FAggregateMode = pap.AggregateMode;
        }

        ////------------------
        /// <summary>
        /// indexer for indexed array gets and sets based on eProvider enum values
        /// </summary>
        /// <param name="index">eProvider index</param>
        /// <value>indexed Model Parameter value</value>
        /// <returns>value in provider array for eProvider index</returns>
        /// <exception cref="WaterSim_Exception">Should throw exception if set and value viloates a range rule</exception>
        public override int this[eProvider index] 
        {
             get
            {
                if (!ProviderClass.valid(index)) throw new WaterSim_Exception(WS_Strings.wsInvalidProvider);
                return Values[index];
            }
            set
            {
                if (!ProviderClass.valid(index)) throw new WaterSim_Exception(WS_Strings.wsInvalidProvider);
                string errMessage = "";
                if (Fpm.CheckProviderValueRange(Femp,value, (eProvider)index,ref errMessage))
                {
                    Values[index] = value;
                }
                else throw new WaterSim_Exception(errMessage);
            }
        }
        //----------------------
        /// <summary>
        /// Gets Model Parameter Array values 
        /// </summary>
        /// <returns>an Array of Model Values</returns>
        public override ProviderIntArray getvalues()
        { 
            return Values;
        }
        //----------------------
        /// <summary>
        /// Set Model Parameter array using value array
        /// </summary>
        /// <param name="value">values to set Model Parameter</param>
        /// <exception cref="WaterSim_Exception">if any numbers in value violate a range rule</exception>
        public override void setvalues(ProviderIntArray value)
        { 
            Values = value;
        }
    }
    //-------------------------------------------------------------
    class ShadowModelParameter : ModelParameterClass
    {

        /// <summary>
        /// Shadow Int Value
        /// </summary>
        protected int FValue = 0;
        protected int FDefault = 0;
        protected providerArrayProperty FDefaultValues;

       /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   For now this constructor is hidden </remarks>
        ///
        /// <param name="aModelParam">                  a model parameter. </param>
        /// <param name="aLabel">                       a label. </param>
        /// <param name="aFieldname">                   a fieldname. </param>
        /// <param name="aParamType">                   Type of a parameter. </param>
        /// <param name="aRangeCheckType">              Type of a range check. </param>
        /// <param name="aLowRange">                    a low range. </param>
        /// <param name="aHighRange">                   a high range. </param>
        /// <param name="specialBaseRangeCheck">        The special base range check. </param>
        /// <param name="specialProviderRangeCheck">    The special provider range check. </param>
        /// <param name="Providerproperty">             The providerproperty. </param>
        /// <param name="DefaultValue">                 The default value for simulating a model reset</param>
        internal ShadowModelParameter(int aModelParam, string aLabel, string aFieldname, modelParamtype aParamType,
            rangeChecktype aRangeCheckType, int aLowRange, int aHighRange, 
            DcheckBase specialBaseRangeCheck, DcheckProvider specialProviderRangeCheck, providerArrayProperty Providerproperty, int DefaultValue): base()
         
        {
            FModelParam = aModelParam;
            FLabel = aLabel;
            FFieldname = aFieldname;
            Ftype = aParamType;
            FRangeCheckType = aRangeCheckType;
            FLowRange = aLowRange;
            FHighRange = aHighRange;
            FSpecialBaseCheck = specialBaseRangeCheck;
            FSpecialProviderCheck = specialProviderRangeCheck;
            FProviderProperty = FDefaultValues = Providerproperty;
            FValue = FDefault = DefaultValue;
            // FReload = ReloadValuesOnSet;
        }

        public ShadowModelParameter(ModelParameterClass mp)
        {
            FModelParam = mp.ModelParam;
            FLabel = mp.Label;
            FFieldname = mp.Fieldname;
            Ftype = mp.ParamType;
            FRangeCheckType = mp.RangeCheckType;
            FLowRange = mp.LowRange;
            FHighRange = mp.HighRange;
            FSpecialBaseCheck = mp.SpecialBaseCheck;
            FSpecialProviderCheck = mp.FSpecialProviderCheck;
            if (mp.isProviderParam)
            {
                ShadowProviderArrayProperty spap = new ShadowProviderArrayProperty(mp.ParameterManager, mp.ProviderProperty);
                FProviderProperty = FDefaultValues = spap;
            }
            else
            {
                if (mp.isBaseParam)
                {
                    FValue = FDefault = (mp as ModelParameterClass).Value;
                }
            }
#if ExtendedParameter
            setupExtended();
#endif
        }
        //------------------------------------------
        /// <summary>   Gets or sets the ModelParameter value. </summary>
        /// <remarks> This is the basic access to base model parameter</remarks>
        /// <value> The value. </value>
        /// <exception cref="WaterSim_Exception">if value or values are out of range for the parameter</exception>

        //        public override int Value
        public override int Value
        { 
            get
            {
                 if (isProviderParam) throw new WaterSim_Exception(WS_Strings.wsMustBeBase);
                  return FValue;
            } 
            set
            {
                if (isProviderParam) throw new WaterSim_Exception(WS_Strings.wsMustBeBase);
                if (isOutputParam) throw new WaterSim_Exception(WS_Strings.wsModelParamMustBeInputBase);
                FValue = value;
            }
        }

    }
    //====================================================================
    class ShadowBaseModelParameter : ModelParameterClass
    {
        /// <summary>
        /// Shadow Int Value
        /// </summary>
        protected int FValue = 0;
        protected int FDefault = 0;
       // protected providerArrayProperty FDefaultValues;

        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   For now this constructor is hidden </remarks>
        ///
        /// <param name="aModelParam">                  a model parameter. </param>
        /// <param name="aLabel">                       a label. </param>
        /// <param name="aFieldname">                   a fieldname. </param>
        /// <param name="aParamType">                   Type of a parameter. </param>
        /// <param name="aRangeCheckType">              Type of a range check. </param>
        /// <param name="aLowRange">                    a low range. </param>
        /// <param name="aHighRange">                   a high range. </param>
        /// <param name="specialBaseRangeCheck">        The special base range check. </param>
        /// <param name="specialProviderRangeCheck">    The special provider range check. </param>
        /// <param name="Providerproperty">             The providerproperty. </param>
        /// <param name="DefaultValue">                 The default value for simulating a model reset</param>
        internal ShadowBaseModelParameter(int aModelParam, string aLabel, string aFieldname, modelParamtype aParamType,
            rangeChecktype aRangeCheckType, int aLowRange, int aHighRange,
            DcheckBase specialBaseRangeCheck, int DefaultValue)
            : base()
        {
            FModelParam = aModelParam;
            FLabel = aLabel;
            FFieldname = aFieldname;
            Ftype = aParamType;
            FRangeCheckType = aRangeCheckType;
            FLowRange = aLowRange;
            FHighRange = aHighRange;
            FSpecialBaseCheck = specialBaseRangeCheck;
            FSpecialProviderCheck = null;
            FProviderProperty = null;
            FValue = FDefault = DefaultValue;
            // FReload = ReloadValuesOnSet;
        }

        public ShadowBaseModelParameter(ModelParameterBaseClass mp)
        {
            if (mp.isBaseParam)
            {
                FModelParam = mp.ModelParam;
                FLabel = mp.Label;
                FFieldname = mp.Fieldname;
                Ftype = mp.ParamType;
                FRangeCheckType = mp.RangeCheckType;
                FLowRange = mp.LowRange;
                FHighRange = mp.HighRange;
                FSpecialBaseCheck = mp.SpecialBaseCheck;
                FSpecialProviderCheck = mp.FSpecialProviderCheck;

                FValue = FDefault = (mp as ModelParameterClass).Value;   // 7/29  (mp as BaseModelParameterClass).Value;
            }
        }
        //------------------------------------------
        /// <summary>   Gets or sets the ModelParameter value. </summary>
        /// <remarks> This is the basic access to base model parameter</remarks>
        /// <value> The value. </value>
        /// <exception cref="WaterSim_Exception">if value or values are out of range for the parameter</exception>

        //        public override int Value
        public override int Value
        {
            get
            {
                if (isProviderParam) throw new WaterSim_Exception(WS_Strings.wsMustBeBase);
                return FValue;
            }
            set
            {
                if (isProviderParam) throw new WaterSim_Exception(WS_Strings.wsMustBeBase);
                if (isOutputParam) throw new WaterSim_Exception(WS_Strings.wsModelParamMustBeInputBase);
                FValue = value;
            }
        }

    }

    //----------------------------------------------------

    ///-------------------------------------------------------------------------------------------------
    /// <summary> Manager for shadow parameters.  </summary>
    ///
    /// <remarks> Ray, 8/25/2011. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class ShadowParameterManager : ParameterManagerClass
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        ///
        /// <remarks> Ray, 8/25/2011. </remarks>
        ///
        /// <param name="PM"> The pm. </param>
        ///-------------------------------------------------------------------------------------------------

        public ShadowParameterManager(ParameterManagerClass PM) : base()
       {
          //foreach (ModelParameterClass mp in PM.AllModelParameters())
          //{
          //    ShadowModelParameter smp = new ShadowModelParameter(mp, mp.Value);
          //    smp.ParameterManagerForFriends = this;
          foreach (int emp in PM.eModelParameters())
          {
              ModelParameterClass mp = PM.Model_Parameter(emp);
              ShadowModelParameter smp = new ShadowModelParameter(mp);
              smp.ParameterManagerForFriends = this;
              this.AddParameter(smp);
              //FModelParameters.Add(smp);


          }
          FAPIVersion = PM.APIVersion;
          FModelVersion = PM.ModelVersion;
     }
    }
  
}
