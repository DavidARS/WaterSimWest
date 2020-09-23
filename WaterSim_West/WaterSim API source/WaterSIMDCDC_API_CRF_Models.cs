///-------------------------------------------------------------------------------------------------
// <copyright file="WaterSIMDCDC_API_CRF_Models.cs" company="Decision Center for a Desert City, Arizona State UniversityMyCompany.com">
// Copyright (c) 2020 Arizona State university All rights reserved.
// </copyright>
// <author>Ray Quay</author>
// <date>6/18/2020</date>
// <version>1.0.0</version>
// <summary>Implements the Resource model prototype class</summary>
// <remarks> A set of abstract classes to implement a Resource model and resource allocation scheme </remarks>
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterSimDCDC.Generic
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary> A class that holds available Resource.</summary>
    /// <remarks>Likely an array of values, one for each reagion</remarks>
    ///-------------------------------------------------------------------------------------------------

    public abstract class AvailableResource
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the AvailableResource Class
        ///     class.</summary>
        ///-------------------------------------------------------------------------------------------------

        public AvailableResource()
        {
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> A Resource model output class.</summary>
    ///
    /// <remarks>abstract class to hold the data that is output from a Resource Mdel</remarks>
    ///-------------------------------------------------------------------------------------------------

    public abstract class ResourceModelOutput
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the ResourceModelClass_Prototype.ResourceModelOutput
        ///     class.</summary>
        ///-------------------------------------------------------------------------------------------------

        public ResourceModelOutput()
        {

        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> A Resource model input.</summary>
    /// <remarks>abstract class to hold the data that is passed to the Run method of a Resource Model</remarks>
    ///-------------------------------------------------------------------------------------------------

    public abstract class ResourceModelInput
    {
        public ResourceModelInput()
        {

        }
    }

    //===========================================================================================================

    ///-------------------------------------------------------------------------------------------------
    /// <summary> A data Model for a Resource.</summary>
    /// <remarks> Abstract class for a Resource model that uses a Run() method to run a model
    ///           Input to the model is a derived ResourceModelInput class and the Run() method
    ///           returns a derived class of ResourceModelOutput  </remarks>
    ///-------------------------------------------------------------------------------------------------

    public abstract class ResourceModel
    {

        public ResourceModel()
        {
 
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Runs the Resource Model with the given input data</summary>
        /// <param name="InputData"> Information describing the input.</param>
        /// <param name="isErr">     [out] The error code 0=no error.</param>
        /// <param name="errStr">    [out] The error string.</param>
        ///
        /// <returns> A ResourceModelOutput.</returns>
        ///-------------------------------------------------------------------------------------------------

        public abstract ResourceModelOutput Run(ResourceModelInput InputData, out int isErr, out string errStr);

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Executes a model run for one year as specified.</summary>
        ///
        /// <param name="Input">  The ResourceModel input data.</param>
        /// <param name="year">   The year as 0 being the first year.</param>
        /// <param name="error">  [out] The error code, 0 = no error.</param>
        /// <param name="ErrStr"> [out] The error string.</param>
        ///
        /// <returns> A ResourceModelOutput object with model results</returns>
        ///-------------------------------------------------------------------------------------------------

        public abstract ResourceModelOutput RunYear(ResourceModelInput Input, int year, int endyear, out int error, out string ErrStr);

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Executes a model run for one year as specified.</summary>
        ///
        /// <remarks> Intended for use on models that keep the ResourceModelInput locally in the model</remarks>
        ///
        /// <param name="year">    The year as 0 being the first year.</param>
        /// <param name="endyear"> The endyear.</param>
        /// <param name="error">   [out] The error code, 0 = no error.</param>
        /// <param name="ErrStr">  [out] The error string.</param>
        ///
        /// <returns> A ResourceModelOutput object with model results.</returns>
        ///-------------------------------------------------------------------------------------------------

        public abstract ResourceModelOutput RunYear(int year, int endyear, out int error, out string ErrStr);

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Executes a model run for one year as specified.</summary>
        ///
        /// <remarks> Intended for use on models that keep the ResourceModelInput and ResourceModelOutput locally in the model.
        ///           and the current year and endyear are in the ResourceModelInput</remarks>
        ///
        /// <param name="error">   [out] The error code, 0 = no error.</param>
        /// <param name="ErrStr">  [out] The error string.</param>
        ///
        /// <returns> A ResourceModelOutput object with model results.</returns>
        ///-------------------------------------------------------------------------------------------------

        public abstract bool RunYear(out int error, out string ErrStr);

        /// <summary>
        /// 09.15.20 das
        /// </summary>
        /// <param name="year"></param>
        /// <param name="error"></param>
        /// <param name="ErrStr"></param>
        /// <returns></returns>
        public abstract bool RunYear(int year, out int error, out string ErrStr);
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Resets the model.</summary>
        ///-------------------------------------------------------------------------------------------------

        public abstract void ResetModel();

      
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> A data Model for allocating a resource using a Resource Model Output</summary>
    ///
    ///-------------------------------------------------------------------------------------------------

    public abstract class ResourceAllocationModel
    {
        public ResourceAllocationModel()
        {

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Allocate Resource.</summary>
        ///
        /// <remarks> Uses the data output from a Resource Model to allocate available resource to various regions for use.</remarks>
        ///
        /// <param name="OutputData"> Information describing the output.</param>
        ///
        /// <returns> A AvailableResource.</returns>
        ///-------------------------------------------------------------------------------------------------

        public abstract AvailableResource AllocateResource(ResourceModelOutput OutputData);
    }


    //======================================================================================================
    
    ///-------------------------------------------------------------------------------------------------
    /// <summary> List of resource models.</summary>
    ///
    /// <remarks> Adds tools to help Manage a list of Resource Models</remarks>
    ///
    /// <seealso cref="System.Collections.Generic.List{WaterSimDCDC.Generic.ResourceModel}"/>
    ///-------------------------------------------------------------------------------------------------

    public class ResourceModelList: List<ResourceModel>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Query if 'TheType' is contained in List.</summary>
        ///
        /// <param name="TheType"> Type of the.</param>
        ///
        /// <returns> True if it succeeds, false if it fails.</returns>
        ///-------------------------------------------------------------------------------------------------

        public bool ContainsType (Type TheType)
        {
            bool result = false;
            foreach(ResourceModel RM in this)
            {
                if (RM.GetType() == TheType)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Searches for the first Resource model of type.</summary>
        /// <param name="TheType"> Type of the.</param>
        ///
        /// <returns> The found type.</returns>
        ///-------------------------------------------------------------------------------------------------

        public ResourceModel FindType(Type TheType)
        {
            ResourceModel result = null;
            foreach (ResourceModel RM in this)
            {
                if (RM.GetType() == TheType)
                {
                    result = RM;
                    break;
                }
            }
            return result;
        }

    }
    

}
