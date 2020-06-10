using System;
using WaterSimDCDC.Generic;
using WaterSimDCDC;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemandModel_Base
{
    /// <summary>
    /// This is the generi water demand model- first order - base class
    /// </summary>
    public abstract class DemandModel : IDisposable
    {

        CRF_Unit_Network UnitNetwork;
        int Fyear;
        double FChangeCoeff;
        double _changeCoeff = 0;
        double FGallonsPerUnit;
        double _gallonsPerUnit = 0;
        double FDriver;
        double _driver = 0;
        double FAdjustConservation;
        double _conservation = 0;
        double FminimumValue;
        double _minimumValue = 0;
        //
        // =================================================================================
        //
        // Constructors
        // ============
        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public DemandModel()
        { }
        /// <summary>
        /// Simple constructor using year
        /// </summary>
        /// <param name="year"></param>
        public DemandModel(int year)
        {
            Fyear = year;
        }
        #endregion Constructors
        /// <summary>
        /// 

        // =============================================================================
        // Properties
        // ==========
        #region Properties
        public virtual double minimumValue
        {
            set { _minimumValue = value; }
            get { return _minimumValue; }

        }
        public virtual double conservation
        {
            set { _conservation = value; }
            get { return _conservation; }

        }
        public virtual double gallonsPerUnit
        {
            set { _gallonsPerUnit = value; }
            get { return _gallonsPerUnit; }

        }
        public virtual double driver
        {
            set { _driver = value; }
            get { return _driver; }

        }
        public virtual double changeCoefficient
        {
            set { _changeCoeff = value; }
            get { return _changeCoeff; }

        }
        #endregion Properties
        // =====================================================================================
        //
        // Methods
        // =======
        //
        #region Methods
            /// <summary>
            /// 
            /// </summary>
            /// <param name="yr"></param>
        //public abstract void preProcess();

        public abstract void preProcessDemand();

        public abstract void preProcessDemand(int year);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public abstract double GetDemand(int year);
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="WSM"></param>
        //public abstract void Initialize_Variables(CRF_Unit_Network UnitNetwork);
        public abstract void Initialize_Variables();
        /// <summary>
        /// 
        /// </summary>
        public abstract void SetDemandFactors();
        /// <summary>
        /// 
        /// </summary>
        public abstract void SetBaseValues();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ConsumerUnits"></param>
        /// <param name="GallonsPerUnit"></param>
        /// <param name="AdjustEfficiency"></param>
        /// <param name="AdjustDamper"></param>
        /// <param name="MinValue"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public virtual double EstimateConsumerDemand(double ConsumerUnits, double GallonsPerUnit, double AdjustEfficiency, double AdjustDamper, double MinValue, double period)
        {
            double result = 0;
            try
            {

                // Get the modify factor to apply for this period using the coefficient AdjustDamper
                //double ModifyRate = ChangeIncrement(1, period, AdjustDamper, MinValue);
                double ModifyRate = utilities.AnnualExponentialChange(1, period, AdjustDamper, MinValue);
                // only use the Modifyrate if your target change is not 1.
                if (AdjustEfficiency != 1)
                {
                    // demand = units * a modified resource per unit value
                    result = ConsumerUnits * (GallonsPerUnit * ModifyRate);
                }
                else
                {
                    // staright forward resources = units * resources per unit
                    result = ConsumerUnits * GallonsPerUnit;
                }
            }
            // Why is this here?  Good question, ChangeIncrement uses the Math.POW() method which can
            // throw an exception.  Hopefully that does not happen and if it does we will just use a 
            // zero as the default value
            catch (Exception ex)
            {
                // Ouch Only thing going here is the Change Increment Function
            }
            return result;


        }
        // ==============================================================
       
            /// <summary>
            /// 
            /// </summary>
            /// <param name="ConsumerUnits"></param>
            /// <param name="ConsumerRate"></param>
            /// <param name="AdjustEfficiency"></param>
            /// <param name="AdjustDamper"></param>
            /// <param name="MinValue"></param>
            /// <param name="period"></param>
            /// <returns></returns>
        public virtual double EstimateLCLUDemand(double ConsumerUnits, double ConsumerRate, double AdjustEfficiency, double AdjustDamper, double MinValue, double period)
        {
            double result = 0;
            try
            {
                // So, we need a minimum value, but what would it be?
                // 07.08.2018 Sampson edits
                 MinValue = 0.22;
                // ENd Sampson edits
                // Get the modify factor to apply for this period using the coefficient AdjustDamper
                //double ModifyRate = ChangeIncrement(1, period, AdjustDamper, MinValue);
                double ModifyRate = utilities.AnnualExponentialChange(1, period, AdjustDamper, MinValue);
                //ModifyRate = 1;
                // only use the Modifyrate if your target change is not 1.
                if (AdjustEfficiency != 1)
                {
                    // demand = units * a modified resource per unit value
                    //result = ConsumerUnits * (ConsumerRate * ModifyRate);
                    // Sampson edits (07.08.2018)
                    result = Math.Max(0,ConsumerUnits * (ConsumerRate * ModifyRate));
                    // ENd Sampson edits
                }
                else
                {
                    // staright forward resources = units * resources per unit
                    //result = ConsumerUnits * ConsumerRate;
                    // Sampson edits 07.08.2018
                    result = Math.Max(0, ConsumerUnits * ConsumerRate);
                    // End Sampson edits
                }
            }
            // Why is this here?  Good question, ChangeIncrement uses the Math.POW() method which can
            // throw an exception.  Hopefully that does not happen and if it does we will just use a 
            // zero as the default value
            catch (Exception ex)
            {
                // Ouch Only thing going here is the Change Increment Function
            }
            return result;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            //
            if (disposing)
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Methods
    }
}
