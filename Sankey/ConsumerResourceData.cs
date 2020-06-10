using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ConsumerResourceModelFramework
{
    #region Foundation Consumer Resource Model Framework Classes
    //===========================================================================
    /// <summary>   Key identifier. 
    ///             Every CRF_DataItem needs a unique key so every object will have a uniqe identifier, for equal and find purposes
    ///             Essentially the binary datetime of now stored in an long (int64).  The only reason it is a long rather than Int64 is 
    ///             that the DateTime.ToBinary() returns a long.  
    ///             </summary>
    /// 
    public class KeyID
    {
        DateTime FDateTime;
        long  FKey;
        //-----------------------
        /// <summary>   Default constructor. </summary>
        public KeyID()
        {
            FDateTime = DateTime.Now;
            
            FKey = FDateTime.ToBinary();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Query if 'aKey' is equal to this keyID. </summary>
        ///
        /// <param name="aKey"> The key. </param>
        ///
        /// <returns>   true if equal, false if not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool isEqual(KeyID aKey)
        {
            return (aKey.FKey == FKey);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculates the hash code for this object. 
        ///             This is just the base Object hash code</summary>
        ///
        /// <returns>   The hash code for this object. </returns>
        ///
        /// <seealso cref="System.Object.GetHashCode()"/>
        ///-------------------------------------------------------------------------------------------------

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Convert this object into a string representation.
        ///             Which is the Long int value of the KEYID, its uniqe code </summary>
        ///
        /// <returns>   A string representation of this object. </returns>
        ///
        /// <seealso cref="System.Object.ToString()"/>
        ///-------------------------------------------------------------------------------------------------

        public override string ToString()
        {
            return FKey.ToString();
        }
    }

    static class CRF_Utility
    {
        static public double BalanceTolerance = 0.01; 
    }
    //===========================================================================
    /// <summary>   CRF_ data item. 
    ///             This is the heart of it all.  All Resources and Consumers are derived from a CRF_DataItem
    ///             This base class provides qa unique IDkey, Name, Label, LabelColor
    ///             </summary>
    /// <seealso cref="CRF_Consumer"/>
    /// <seealso cref="CRF_Resource"/>
    ///             ==============================================================
                 
    public class CRF_DataItem
    {
        protected string FName = "";
        protected string FLabel = "";
        protected KeyID FKey = new KeyID();
        protected double FValue = 0;
        protected Color FColor = Color.Gray;


        /// <summary>   The List of Fluxs for this data item </summary>
        protected CRF_FluxList FFluxs;

        /// <summary>   Default constructor. </summary>
        public CRF_DataItem()
        {
            FFluxs = new CRF_FluxList(this);
            FName = FKey.ToString();
            FLabel = FName;

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. This is the local named used for object identification, best if unique, but not neccesary, best if short </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_DataItem(string aName)
            : base()
        {
            FFluxs = new CRF_FluxList(this);
            FName = aName;
            FLabel = aName;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. This is the string that will be used to label this item in visualizations</param>
        /// <param name="aColor">   The color. This will be the color of the label</param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_DataItem(string aName, string aLabel, Color aColor)
            : base()
        {
            FFluxs = new CRF_FluxList(this);
            FName = aName;
            FLabel = aLabel;
            FColor = aColor;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">  The name. This is the local named used for object identification, best if
        ///     unique, but not neccesary, best if short. </param>
        /// <param name="aLabel"> The label. This is the string that will be used to label this item in
        ///     visualizations. </param>
        /// <param name="aColor">   The color. This will be the color of this data item when visualized. </param>
        /// <param name="aValue">   The value. Value in this case is used differently by each derived class, see direved class for units</param>
        /// <seealso cref="CRF_Consumer"/>
        /// <seealso cref="CRF_Resource"/>
        ///-------------------------------------------------------------------------------------------------

        public CRF_DataItem(string aName, string aLabel, Color aColor, double aValue)
            : base()
        {
            FFluxs = new CRF_FluxList(this);
            FName = aName;
            FLabel = aLabel;
            FValue = aValue;
            FColor = aColor;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Name
        {
            get { return FName; }
            set { FName = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the label. </summary>
        ///
        /// <value> The label. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Label
        {
            get { return FLabel; }
            set {FLabel = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the value. </summary>
        ///
        /// <value> The value. Value in this case is used differently by each derived class, see direved class for units</value>
        /// <seealso cref="CRF_Consumer"/>
        /// <seealso cref="CRF_Resource"/>
        ///-------------------------------------------------------------------------------------------------

        public double Value
        {
            get { return FValue; }
            set { FValue = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the Unique key. </summary>
        ///
        /// <value> The key. </value>
        ///-------------------------------------------------------------------------------------------------

        public KeyID Key
        {
            get { return FKey; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color of this item when visualized. </summary>
        ///
        /// <value> The color. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color Color
        {
            get { return FColor; }
            set { FColor = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Tests if this object is considered equal to another.  This is based on the KeyID of the object</summary>
        ///
        /// <param name="obj">  The object to compare to this object. </param>
        ///
        /// <returns>   true if the objects are considered equal, false if they are not. </returns>
        ///
        /// <seealso cref="System.Object.Equals(object)"/>
        ///-------------------------------------------------------------------------------------------------

        public override bool Equals(object obj)
        {
 	        bool result = false;
            if (obj is CRF_DataItem)
            {
                if ((obj as CRF_DataItem).FKey == FKey)
                {
                    result = true;
                }
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculates the hash code for this object. </summary>
        ///
        /// <returns>   The hash code for this object. </returns>
        ///
        /// <seealso cref="System.Object.GetHashCode()"/>
        ///-------------------------------------------------------------------------------------------------

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the Fluxs. </summary>
        ///
        /// <value> The Fluxs. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_FluxList Fluxs
        {
            get { return FFluxs; }
        }

    }

    //===========================================================================

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   List of CRF_ data items. </summary>
    ///
    /// <seealso cref="System.Collections.Generic.List<QCRF_.CRF_DataItem>"/>
    ///-------------------------------------------------------------------------------------------------

    public class CRF_DataItemList : List<CRF_DataItem>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Searches for the first index of this item based on IDKey values (which is unique so this should be only one). </summary>
        ///
        /// <param name="anItem">   an item. </param>
        ///
        /// <returns>   The found index. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int FindIndex(CRF_DataItem anItem)
        {
            return this.FindIndex(
                delegate(CRF_DataItem item)
                {
                    return item.Key == anItem.Key;
                }
            );
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds anItem.. </summary>
        ///
        /// <param name="anItem">   The CRF_DataItem to add. </param>
        ///
        /// <seealso cref="System.Collections.Generic.List<QCRF_.CRF_DataItem>.Add(CRF_DataItem)"/>
        ///-------------------------------------------------------------------------------------------------

        public void Add(CRF_DataItem anItem)
        {
            int index = FindIndex(anItem);
            if (index < 0)
            {
                base.Add(anItem);
            }
            else
            {
                // Some error?
            }
        }
    }
    #endregion

    #region CRF_ Flux (Flux) Classes
    //===========================================================================
     

    /// <summary>   CRF_ Flux. 
    ///             An Flux is movement of material from a Resource to a Consumer </summary>
    ///             <remarks> Material (resources) flow from a Resource to one or more Consumers.  Resources allocate a portion of their
    ///                       available value to a consumer, the flux moves this value to the consumer, the consumer then adds the flux value 
    ///                       to their value.  
    ///                       Here is an example. A Resource RES1 has an available supply of resources RES.Limit. A Consumer CONS1 has a demand  
    ///                       for resources CONS1.Demand.  These values are managed by each RES1 and CONS1.  
    ///                       RES1 can allocate a portion of its supply to CONS1.  This is done with a CRF_Flux. 
    ///                       
    ///                       The Flux can be done as an absolute value or as a % of the CRF_DataItems Value (Limit or demand); 
    ///                      </remarks>
    /// 
    /// <seealso cref="CRF_Consumer"/>
    /// <seealso cref="CRF_Resource"/>
    ///                      
    public class CRF_Flux 
    {
        KeyID FKey = new KeyID();

        double FAllocate = 0;
        /// <summary>   Values that represent the Method of the Flux (flux). </summary>
        public enum Method {
            /// <summary>   Unknown method </summary>
            amUnknown,
            /// <summary>   The allocated value is an absolute amount to be allocated </summary>
            amAbsolute,
            /// <summary>   The Flux is a percent of the total DataItems Value (Limit or Demand) </summary>
            amPercent
        };

 
        Method FMethod = Method.amUnknown;
        CRF_Resource FromDataItem;
        CRF_Consumer ToDataItem;

        public CRF_Flux(CRF_Consumer DataItem, double Amount, Method aMethod)
        {
            FAllocate = Amount;
            ToDataItem = DataItem;
            FMethod = aMethod;
            FromDataItem = null;
        }

        public CRF_Flux(CRF_Resource DataItem, double Amount, Method aMethod)
        {
            FAllocate = Amount;
            FromDataItem = DataItem;
            FMethod = aMethod;
            ToDataItem = null;
        }

        public double Allocated()
        {
            double amount = 0;
            if (FromDataItem!=null)
            switch (FMethod)
            {
                case Method.amAbsolute:
                    amount = FAllocate;
                    break;
                case Method.amPercent:
                    amount = FromDataItem.Limit * FAllocate;
                    break;
                case Method.amUnknown:
                    break;
            }
            return amount;
        }

        public CRF_Resource Source
        {
            get { return FromDataItem; }
            set { FromDataItem = value;}
        }

        public CRF_Consumer Target
        {
            get { return ToDataItem; }
            set { ToDataItem = value; }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the Unique key. </summary>
        ///
        /// <value> The key. </value>
        ///-------------------------------------------------------------------------------------------------

        public KeyID Key
        {
            get { return FKey; }
        }

        public string Label
        {
            get
            {

                string temp = Allocated().ToString("00.0") + " ";

                if (FromDataItem != null)
                {
                    temp += "From:"+FromDataItem.Label + " ";
                }
                if (ToDataItem != null)
                {
                    temp += "To:" + ToDataItem.Label;
                }
                return temp;
            }

        }
        /// <summary>   Tests if this object is considered equal to another.  This is based on the KeyID of the object</summary>
        ///
        /// <param name="obj">  The object to compare to this object. </param>
        ///
        /// <returns>   true if the objects are considered equal, false if they are not. </returns>
        ///
        /// <seealso cref="System.Object.Equals(object)"/>
        ///-------------------------------------------------------------------------------------------------

        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj is CRF_Flux)
            {
                if ((obj as CRF_Flux).FKey == FKey)
                {
                    result = true;
                }
            }
            return result;
        }
    }


    //===========================================================================

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   List of CRF_ Fluxs.
    ///             </summary>
    ///             <remarks>
    ///             CRF_DataItems (Resources or Consumers) use CRF_FluxLists to keep track of a ist Flux (flux - movement from a Resource to a Consumer)  
    ///             For a Resource, an Flux (flux) is a movement of some portion of its value to a Consumer
    ///             For a Consumer, an Flux (flux) is movement from a Resource to be added to its value.
    ///             The Resource knows where it is sending Fluxs(flux) based on its Flux list
    ///             The Consumers knows from whom it is receiving an al;location (flux) based on its Flux list.
    ///             An Flux FLUX represents this connection, and there is only one unique Flux object for each connection, and this uniuqe FLUX should be 
    ///             both in the Resource's and Cosnumer's Flux list.  Thus they share the same object.
    ///             This creates a delima becuse both Resources and Cosumers can add a flux to their own Flux list, however, allocating a flux
    ///             means that the other object (Resource or Consumer) needs to have it added to their list as well. 
    ///</remarks>
    /// <seealso cref="System.Collections.Generic.List<QCRF_.CRF_Flux>"/>
    ///-------------------------------------------------------------------------------------------------

    public class CRF_FluxList : List<CRF_Flux>
    {
        CRF_DataItem FOwner = null;

        public CRF_FluxList(CRF_DataItem DataItemOwner) : base()
        {
            FOwner = DataItemOwner;
        }

        public int FindIndex(CRF_Flux anItem)
        {
            int index = this.FindIndex(
                delegate(CRF_Flux item)
                {
                    return item.Key == anItem.Key;
                }
            );
            return index;
        }
        public int FindIndex(CRF_DataItem anItem)
        {
            return this.FindIndex(
                delegate(CRF_Flux item)
                {
                    return item.Source.Key == anItem.Key;
                }
            );
        }
        //-----------------------------------
        public CRF_Flux Find(CRF_DataItem anItem)
        {
            int index = FindIndex(anItem);
            if (index > -1)
            {
                return this[index];
            }
            else
            {
                return null;
            }
        }
        public CRF_Flux Find(CRF_Flux anItem)
        {
            int index = FindIndex(anItem);
            if (index > -1)
            {
                return this[index];
            }
            else
            {
                return null;
            }
        }
        //-----------------------------------
        public void Remove(CRF_DataItem anItem)
        {
            int index = FindIndex(anItem);
            if (index > -1)
            {
                this.RemoveAt(index);
            }

        }
        //-----------------------------------
        // Must be Unique
        public void Add(CRF_Flux anItem)
        {
            // Ok, first lets set From and Tos
            if (anItem.Source == null)
            {
                if (FOwner is CRF_Resource)
                {
                    anItem.Source = (FOwner as CRF_Resource);
                }
            }
            if (anItem.Target == null)
            {
                if (FOwner is CRF_Consumer)
                {
                    anItem.Target = (FOwner as CRF_Consumer);
                }

            }
            // look and see if it is in the list already
            int index = FindIndex(anItem);
            if (index < 0)
            {
                // if not add it
                base.Add(anItem);
            }
            else
            {
                // Ahh!   Being added again, what does that mean?
                // let's generate and exception for now, I need to think about this a bit more
                throw new Exception("This Flux is a;lready in the list!");
            }
            
            // OK,  Does Flux list have an owner, ie A Resource or a consumer, if not then we are done
            if (FOwner != null)
            {
                // OK this list is owned by someone,  
                // see if the owner of the Flux (flux) is resource or consumer
                if (FOwner is CRF_Resource)
                { 
                    
                    // OK, this is a bit complicated.  This Fluxlist is owned by a Resource, and an Flux is being added to list
                    // Let's make sure that the Target of this Flux, has this Flux in its list
                    CRF_DataItem SDI = anItem.Target;
                    // see if owner of this list is in the SDI list
                    CRF_Flux Target = SDI.Fluxs.Find(anItem);
                    if (Target != null)
                    {
                        // ok, it is there, late make sure that this Fowner is the source, if not change it
                        //if (!FOwner.Equals(Target.Source))
                        //{
                        //    Target.Source = (FOwner as CRF_Resource);
                        //}
                        // Well maybe not, need to decide if we want to do this
                    }
                    else
                    {
                        // ok it is not there, let's modofy it
                        // create an Flux object
                        //CRF_Flux NewSA = new CRF_Flux(FOwner, anItem.Allocated(), CRF_Flux.Method.amAbsolute);
                        SDI.Fluxs.Add(anItem);
                    }

                }
                else
                {
                    if (FOwner is CRF_Consumer)
                    {
                        
                        // OK, this is a bit complicated.  This Fluxlist is owned by a consumer, and an Flux is being added to list
                        // Let's make sure that the Source of this Flux, has this Flux in its list
                        CRF_DataItem SDI = anItem.Source;
                        // see if owener of this list is in the SDI list
                        CRF_Flux Target = SDI.Fluxs.Find(anItem);
                        if (Target != null)
                        {
                            // ok, it is there, late make sure that this consumer is the target, if not change it
                            //if (!FOwner.Equals(Target.Target))
                            //{
                            //    Target.Target = (FOwner as CRF_Consumer);
                            //}

                            // Well maybe not, need to decide if we want to do this
                        }
                        else
                        {
                            // ok it is not there, add it, of course this is going to cause it to go through this routine again 
                            SDI.Fluxs.Add(anItem);
                        }
                    }
                }
              }
         }
    

        //-----------------------------------
        public CRF_DataItem Owner
        {
            get { return FOwner; }
        }

        //-----------------------------------
        public double TotalAllocated
        {
            get
            {
                double temp = 0;
                foreach (CRF_Flux SA in this)
                {
                    temp += SA.Allocated();
                }
                return temp;
            }
        }

    }

    #endregion
    //===========================================================================

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   CRF_ resource. </summary>
    ///
    /// <seealso cref="QCRF_.CRF_DataItem"/>
    ///-------------------------------------------------------------------------------------------------

    public class CRF_Resource : CRF_DataItem
    {
        double FTotalDemand = 0;
        // OK Setup Consumers using Flux List to avoid confusion between Resources and Consumers
        CRF_FluxList FConsumerList;
        //-----------------------
        public CRF_Resource(string aName)
            : base(aName)
        {
            // OK Setup Consumers using Flux List to avoid confusion between Resources and Consumers
            FConsumerList = FFluxs;
        }

        //-----------------------
        public CRF_Resource(string aName, string aLabel, Color aColor)
            : base(aName, aLabel, aColor) 
        {
            // OK Setup Consumers using Flux List to avoid confusion between Resources and Consumers
            FConsumerList = FFluxs;
        }

        //-----------------------
        public CRF_Resource(string aName, string aLabel, Color aColor, double AvailableSupply)
            : base(aName, aLabel, aColor)
        {
            FValue = AvailableSupply;
            // OK Setup Consumers using Flux List to avoid confusion between Resources and Consumers
            FConsumerList = FFluxs;
        }
        //-----------------------
        public double Limit
        {
            get {return FValue;}
            set {FValue = value; }
        }
        //-----------------------
        public double Allocated
        {
            get { return FConsumerList.TotalAllocated; }
        }
        //-----------------------
        public double Net
        {
            get { return FValue - Allocated; }
        }
        public void AddConsumer(CRF_Consumer aConsumer, double amount, CRF_Flux.Method aMethod)
        {
            FConsumerList.Add(new CRF_Flux(aConsumer, amount, aMethod));
        }

        public void DeleteConsumer(CRF_Consumer aConsumer)
        {
            FConsumerList.Remove(aConsumer);
        }
    }

    //===========================================================================
    public class CRF_ResourceList : CRF_DataItemList
    {
        //-----------------------
        public CRF_ResourceList()
            : base()
        {
        }
        //-----------------------
        public double Allocated
        {
            get 
            {
                double temp = 0;
                foreach(CRF_Resource skRes in this)
                {
                    temp += skRes.Allocated;
                }
                return temp;
            }
        }
        //-----------------------
        public double Net
        {
            get
            {
                double temp = 0;
                foreach (CRF_Resource skRes in this)
                {
                    temp += skRes.Net;
                }
                return temp;
            }
        }
        //-----------------------
        public double NegNet
        {
            get
            {
                double temp = 0;
                foreach (CRF_Resource skRes in this)
                {
                    if (skRes.Net < 0)
                    {
                        temp += skRes.Net;
                    }
                }
                return temp;
            }
        }
        //-----------------------
        public double Limit
        {
            get
            {
                double temp = 0;
                foreach (CRF_Resource skRes in this)
                {
                    temp += skRes.Limit;
                }
                return temp;
            }
        }
        //-----------------------

        public bool Balanced()
        {
            bool result = true;
            
            foreach (CRF_Resource skRes in this)
            {
                double Test = skRes.Limit * CRF_Utility.BalanceTolerance;
                if ((skRes.Net > Test) || (skRes.Net < (-1 * Test)))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        //-----------------------

        public bool Balanced(out bool Canbe)
        {
            bool result = true;
            double totalNet = 0;
            double Test = 0;
            double TotTest = 0;
            foreach (CRF_Resource skRes in this)
            {
                totalNet += skRes.Net;
                Test = skRes.Limit * CRF_Utility.BalanceTolerance;
                TotTest += Test;
                if ((skRes.Net > Test) || (skRes.Net < (-1 * Test)))
                {
                    result = false;
                }
            }
            Canbe = ((totalNet > TotTest) || (totalNet < (-1 * TotTest)));
            return result;
        }

        //-----------------------
        public CRF_Resource AddResource(string aName, string aLabel, Color aColor, double aLimit)
        {
            CRF_Resource Temp = new CRF_Resource(aName, aLabel, aColor, aLimit );
            this.Add(Temp);
            return Temp;
        }
    }

    //===========================================================================
    public class CRF_Consumer : CRF_DataItem
    {
        double FTotalResources = 0;
        // OK Setup Resources using Flux List to avoid confusion between Resources and Consumers
        CRF_FluxList FResources;

        //-----------------------
        public CRF_Consumer()
            : base()
        {
            // OK Setup Resources using Flux List to avoid confusion between Resources and Consumers
            FResources = FFluxs;
        }

        //-----------------------
        public CRF_Consumer(string aName)
            : base(aName)
        {
            // OK Setup Resources using Flux List to avoid confusion between Resources and Consumers
            FResources = FFluxs;
        }

        //-----------------------
        public CRF_Consumer(string aName, string aLabel, Color aColor)
            : base(aName, aLabel, aColor) 
        {
            // OK Setup Resources using Flux List to avoid confusion between Resources and Consumers
            FResources = FFluxs;
        }

        //-----------------------
        public CRF_Consumer(string aName, string aLabel, Color aColor, double Demand)
            : base(aName, aLabel, aColor)
        {
            FValue = Demand;
            // OK Setup Resources using Flux List to avoid confusion between Resources and Consumers
            FResources = FFluxs;
        }

        //-----------------------
        public double Demand
        {
            get { return FValue; }
            set { FValue = value; }
        }

        //-----------------------
        public double Resources
        {
            get { return FResources.TotalAllocated; }
        }

        //-----------------------
        public double Net
        {
            get {return FValue - Resources; }
        }

        public void AddResource(CRF_Resource aResource, double amount, CRF_Flux.Method aMethod)
        {
            FResources.Add(new CRF_Flux(aResource, amount, aMethod));
        }

        public void RemoveResource(CRF_Resource aResource)
        {
            FResources.Remove(aResource);
        }


    }

    //internal delegate bool CompareCRF_DataItem ( );

    //static class CompareCRF_DataItem : IEqualityComparer<CRF_DataItem>
    //{
    //    public bool Equals(CRF_DataItem T1, CRF_DataItem T2)
    //    {
    //        return T1.Key == T2.Key;
    //    }
    //    public int GetHashCode(CRF_DataItem SDI)
    //    {
    //        return SDI.Key.GetHashCode();
    //    }
    //}

    //===========================================================================
    public class CRF_ConsumerList : CRF_DataItemList
    {
        //-----------------------
        public CRF_ConsumerList()
            : base()
        {
        }

        //-----------------------
        public double Demand
        {
            get
            {
                double temp = 0;
                foreach (CRF_Consumer skCons in this)
                {
                    temp += skCons.Demand;
                }
                return temp;
            }
        }

        //-----------------------
        public double Net
        {
            get
            {
                double temp = 0;
                foreach (CRF_Consumer skCons in this)
                {
                    temp += skCons.Net;
                }
                return temp;
            }
        }

        //-----------------------
        public double NegNet
        {
            get
            {
                double temp = 0;
                foreach (CRF_Consumer skCons in this)
                {
                    if (skCons.Net < 0)
                    {
                        temp += skCons.Net;
                    }
                }
                return temp;
            }
        }
        //-----------------------

        public bool Balanced()
        {
            bool result = true;
            foreach (CRF_Consumer skCons in this)
            {
                double Test = skCons.Demand * CRF_Utility.BalanceTolerance;
                if ((skCons.Net > Test) || (skCons.Net < (-1 * Test)))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        //-----------------------

        public bool Balanced(out bool Canbe)
        {
            bool result = true;
            double totalNet = 0;
            double Test = 0;
            double TotTest = 0;
            foreach (CRF_Consumer skCons in this)
            {
                totalNet += skCons.Net;
                Test = skCons.Demand * CRF_Utility.BalanceTolerance;
                TotTest += Test;
                if ((skCons.Net>Test)||(skCons.Net<(-1*Test)))
                {
                    result = false;
                }
            }
            Canbe = ((totalNet > TotTest) || (totalNet < (-1 * TotTest)));
            return result;
        }
        //-----------------------
        public double Resources
        {
            get
            {
                double temp = 0;
                foreach (CRF_Consumer skCons in this)
                {
                    temp += skCons.Resources;
                }
                return temp;
            }
        }

        //-----------------------
        public CRF_Consumer AddConsumer(string aName, string aLabel,  Color aColor, double aDemand)
        {
            CRF_Consumer Temp = new CRF_Consumer(aName, aLabel, aColor, aDemand);
            this.Add(Temp);
            return Temp;
        }

    }

    public class CRF_Network
    {
        CRF_ResourceList FResources;
        CRF_ConsumerList FConsumers;
  
        public delegate void NetworkResetDelegate();

        NetworkResetDelegate FCallback = null;

        public CRF_Network()
            : base()
        {
            FResources = new CRF_ResourceList();
            FConsumers = new CRF_ConsumerList();
        }

        public CRF_Network(string Name, CRF_ResourceList aResourceList, CRF_ConsumerList aConsumerList, NetworkResetDelegate Callback)
            : base()
        {
            FResources = aResourceList;
            FConsumers = aConsumerList;
            FCallback = Callback;

        }
        //-----------------------------------------------------------

        public CRF_ResourceList Resources
        {
            get { return FResources; }
        }
        //-----------------------------------------------------------

        public CRF_ConsumerList Consumers
        {
            get { return FConsumers; }
        }
        //-----------------------------------------------------------

        public bool Balanced()
        {
            return (Resources.Balanced() && Consumers.Balanced());
        }
        //-----------------------------------------------------------

        public bool Balanced(out bool ResourcesBalanced, out bool ConsumersBalanced)
        {
            ResourcesBalanced = Resources.Balanced();
            ConsumersBalanced = Consumers.Balanced();
            return (ResourcesBalanced && ConsumersBalanced);
        }
        //-----------------------------------------------------------
        public NetworkResetDelegate CallBackMethod
        {
            get { return FCallback; }
            set { FCallback = value; }
        }
        //-----------------------------------------------------------

        public void SetNetwork(CRF_ResourceList aResourceList, CRF_ConsumerList aConsumerList)
        {
            FResources = aResourceList;
            FConsumers = aConsumerList;
            if (FCallback != null)
            {
                FCallback();
            }
        }

        public void ResetNetwork()
        {
            if (FCallback != null)
            {
                FCallback();
            }

        }
    }
}
