///-------------------------------------------------------------------------------------------------
// <copyright file="RiverModelClass_Prototype.cs" company="Decision Center for a Desert City, Arizona State UniversityMyCompany.com">
// Copyright (c) 2020 Arizona State university All rights reserved.
// </copyright>
// <author>Ray Quay</author>
// <date>6/18/2020</date>
// <version>1.0.0</version>
// <summary>Implements the river model prototype class</summary>
// <remarks> A set of abstract classes to implement a river model and water allocation scheme </remarks>
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiverModelClass_Prototype
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary> A class that holds available water by region.</summary>
    /// <remarks>Likely an array of values, one for each reagion</remarks>
    ///-------------------------------------------------------------------------------------------------

    public abstract class RegionAvailableWater
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the RiverModelClass_Prototype.RegionAvailableWater
        ///     class.</summary>
        ///-------------------------------------------------------------------------------------------------

        public RegionAvailableWater()
        {
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> A river model output class.</summary>
    ///
    /// <remarks>abstract class to hold the data that is output from a River Mdel</remarks>
    ///-------------------------------------------------------------------------------------------------

    public abstract class RiverModelOutput
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the RiverModelClass_Prototype.RiverModelOutput
        ///     class.</summary>
        ///-------------------------------------------------------------------------------------------------

        public RiverModelOutput()
        {

        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> A river model input.</summary>
    /// <remarks>abstract class to hold the data that is passed to the Run method of a River Model</remarks>
    ///-------------------------------------------------------------------------------------------------

    public abstract class RiverModelInput
    {
        public RiverModelInput()
        {

        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> A data Model for a river.</summary>
    /// <remarks> Abstract class for a river model that uses a Run() method to run a model
    ///           Input to the model is a derived RiverModelInput class and the Run() method
    ///           returns a derived class of RiverModelOutput  </remarks>
    ///-------------------------------------------------------------------------------------------------

    public abstract class RiverModel
    {
        public RiverModel()
        {
 
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Runs the River Model with the given input data.</summary>
        /// <param name="InputData"> Information describing the input.</param>
        /// <returns> A RiverModelOutput.</returns>
        ///-------------------------------------------------------------------------------------------------

        public abstract RiverModelOutput Run(RiverModelInput InputData);
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> A data Model for allocating water using a River Model Output</summary>
    ///
    ///-------------------------------------------------------------------------------------------------

    public abstract class RiverAllocationModel
    {
        public RiverAllocationModel()
        {

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Allocate water.</summary>
        ///
        /// <remarks> Uses the data output from a River Model to allocate available water to various regions for use.</remarks>
        ///
        /// <param name="OutputData"> Information describing the output.</param>
        ///
        /// <returns> A RegionAvailableWater.</returns>
        ///-------------------------------------------------------------------------------------------------

        public abstract RegionAvailableWater AllocateWater(RiverModelOutput OutputData);
    }
    ///-------------------------------------------------------------------------------------------------
    /// <summary> Manager for river models.</summary>
    ///
    ///-------------------------------------------------------------------------------------------------

    public abstract class RiverModelManager
    {
        RiverModel FRiverModel;
        RiverAllocationModel FAllocationModel;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the RiverModelClass_Prototype.RiverModelManager
        ///     class.</summary>
        ///-------------------------------------------------------------------------------------------------

        public RiverModelManager()
        {

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the RiverModelClass_Prototype.RiverModelManager
        ///     class.</summary>
        /// <param name="aRiverModel">     The river model.</param>
        /// <param name="AllocationModel"> The allocation model.</param>
        ///-------------------------------------------------------------------------------------------------

        public RiverModelManager(RiverModel aRiverModel, RiverAllocationModel AllocationModel)
        {
            FRiverModel = aRiverModel;
            FAllocationModel = AllocationModel;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Runs the model and allocated water to regions.
        ///           </summary>
        ///
        ///
        /// <param name="InputData">      Information describing the input.</param>
        /// <param name="OutputData">     [out] Information describing the output.</param>
        /// <param name="AllocatedWater"> [out] The allocated water.</param>
        ///
        /// <returns> True if it succeeds, false if it fails.</returns>
        ///-------------------------------------------------------------------------------------------------

        public bool RunAndAllocate(RiverModelInput InputData, out RiverModelOutput OutputData,out RegionAvailableWater AllocatedWater)
        {
            bool result = false;
            AllocatedWater = null;
            OutputData = FRiverModel.Run(InputData);
            if (OutputData != null)
            {
                AllocatedWater = FAllocationModel.AllocateWater(OutputData);
                if (AllocatedWater!=null)
                {
                    result = true;
                }
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the river model.</summary>
        ///
        /// <value> the river model.</value>
        ///-------------------------------------------------------------------------------------------------

        public RiverModel TheRiverModel
        {
            get { return FRiverModel; }
            set { FRiverModel = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the allocation model.</summary>
        ///
        /// <value> the allocation model.</value>
        ///-------------------------------------------------------------------------------------------------

        public RiverAllocationModel TheAllocationModel
        {
            get { return FAllocationModel; }
            set { FAllocationModel = value;  }
        }
    }


}
