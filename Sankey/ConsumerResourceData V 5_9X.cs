using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


//=================================================================
// LOG
// 2/9/17 QUAY
// Added support for comments and and error log for Consumers and Respurces
//
// ===================================================================

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
        static int count = 0;
        DateTime FDateTime;
        long FKey;
        //-----------------------
        /// <summary>   Default constructor. </summary>
        public KeyID()
        {
            FDateTime = DateTime.Now;

            FKey = FDateTime.ToBinary() + count;
            count++;
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

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A crf utility. </summary>
    ///
    /// <remarks>   Mcquay, 1/25/2016. </remarks>
    ///-------------------------------------------------------------------------------------------------

    static public class CRF_Utility
    {
        /// <summary>   The balance tolerance. </summary>
        static public double BalanceTolerance = 0.01;
        /// <summary>   The flux size tolerance. </summary>
        //static public double FluxSizeTolerance = .001;
        static public double FluxSizeTolerance = .001;

        static public bool AllowFluxChange(CRF_DataItem FromItem, CRF_DataItem ToItem)
        {
            return (ToItem.AllowFluxChangeFrom(FromItem) && FromItem.AllowFluxChangeTo(ToItem));
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Values that represent management styles. </summary>
        ///
        /// <remarks>   Mcquay, 1/27/2016. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public enum ManagementStyle
        {
            /// <summary>   Cooperative </summary>
            /// <remarks>   Willing to share, help as needed</remarks>
            msCooperate,
            /// <summary>   Protect </summary>
            /// <remarks>   Try to keep what is already acquired, Opposite of Cooperative</remarks>
            msProtect,
            /// <summary>   Expansive </summary>
            /// <remarks>   Similar to Protect, but desire to acquire even more</remarks>
            msExpand,
            // edits 01.07.22 das
            msSeekNew
            // end edits 01.07.22 das
        };

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

        protected CRF_DataItemList FOwner;
        protected bool FValueChange = false;

        protected string FName = "";
        protected string FLabel = "";
        protected KeyID FKey = new KeyID();
        protected double FValue = 0;
        protected Color FColor = Color.Gray;

        // Added by Quay 2/9/17
        // this is used to leave comments aboput resource.
        string FComment = "";
        // this is used to leave behind notes if errors occur during automatic creation.
        string FError = "";

        protected CRF_Utility.ManagementStyle FMStyle = CRF_Utility.ManagementStyle.msCooperate;

        /// <summary>   The List of Fluxs for this data item </summary>
        protected CRF_FluxList FToFluxs;
        protected CRF_FluxList FFromFluxs;

        /// <summary>   Default constructor. </summary>
        public CRF_DataItem()
        {
            FToFluxs = new CRF_FluxList(this);
            FFromFluxs = new CRF_FluxList(this);
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
            FToFluxs = new CRF_FluxList(this);
            FFromFluxs = new CRF_FluxList(this);
            FName = aName;
            FLabel = aName;
        }
        /// <summary>
        /// 09.08.16 DAS to accomodate population
        /// </summary>
        /// <param name="aName"></param>
        /// <param name="aValue"></param>
        public CRF_DataItem(string aName, double aValue)
            : base()
        {
            FToFluxs = new CRF_FluxList(this);
            FFromFluxs = new CRF_FluxList(this);
            FName = aName;
         
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
            FToFluxs = new CRF_FluxList(this);
            FFromFluxs = new CRF_FluxList(this);
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
            FToFluxs = new CRF_FluxList(this);
            FFromFluxs = new CRF_FluxList(this);
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
            set { FLabel = value; }
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
        /// <summary>   Gets or sets the management style. </summary>
        ///
        /// <value> The management style. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Utility.ManagementStyle ManagementStyle
        {
            get { return FMStyle; }
            set { FMStyle = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds to the flux. </summary>
        ///
        /// <param name="Flux"> The flux. </param>
        ///-------------------------------------------------------------------------------------------------

        public void AddToFlux(CRF_Flux Flux)
        {
            FToFluxs.Add(Flux);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a comment. </summary>
        ///
        /// <param name="newComment">   The new comment. </param>
        ///-------------------------------------------------------------------------------------------------

        public void AddComment(string newComment)
        {
            FComment += ";" + newComment;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds an error to log. </summary>
        ///
        /// <param name="newerror"> The newerror. </param>
        ///-------------------------------------------------------------------------------------------------

        public void AddError(string newerror)
        {
            FError += ";" + newerror;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the comment about this resource. </summary>
        ///
        /// <value> The comment. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Comment
        {
            get {return FComment;}
            set {FComment = value;}
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the error log for this . </summary>
        ///
        /// <value> The error. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Error
        {
            get {return FError; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds from flux. </summary>
        ///
        /// <param name="Flux"> The flux. </param>
        ///-------------------------------------------------------------------------------------------------

        public void AddFromFlux(CRF_Flux Flux)
        {

        }

        //List<string> shitlist = new List<string>();
        private int _seek(ref CRF_DataItem anItem, int level)
        {
            //shitlist.Add(anItem.Key.ToString());
            if (anItem.ToFluxs.Count > 0)
            {
                int depth = level;
                foreach (CRF_Flux flux in anItem.ToFluxs)
                {
                    CRF_DataItem tempdi = flux.Target;
                    int temp = _seek(ref tempdi, level + 1);
                    if (temp > depth)
                    {
                        depth = temp;
                    }
                }
                return depth;
            }
            else
            {
                return level;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the depth. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int Depth()
        {
            int index = 0;
            CRF_DataItem Temp = this;
            index = _seek(ref Temp, 0);
            return index;
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
        /// <summary>   Gets the Fluxs That send Resource On. </summary>
        ///
        /// <value> The ToFluxs. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_FluxList ToFluxs
        {
            get { return FToFluxs; }
        }

        public CRF_FluxList FromFluxs
        {
            get { return FFromFluxs; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the owner. </summary>
        ///
        /// <value> The owner. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_DataItemList Owner
        {
            get { return FOwner; }
            set { FOwner = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sum to on item. </summary>
        /// <param name="theItem">  the item. </param>
        ///
        /// <returns>   The total number of to on item. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double SumToOnItem(CRF_DataItem theItem)
        {
            double result = 0;
            result = FToFluxs.SumToOnItem(theItem);
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sum from on item. </summary>
        /// <param name="theItem">  the item. </param>
        ///
        /// <returns>   The total number of from on item. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double SumFromOnItem(CRF_DataItem theItem)
        {
            double result = 0;
            result = FFromFluxs.SumFromOnItem(theItem);
            return result;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the value change. </summary>
        ///
        /// <value> true if value change, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool ValueChange
        {
            get { return FValueChange; }
            set
            {
                // Set Value Change field
                FValueChange = true;
                // if there is an Owner, which would be a list, call the Owners value change method
                if (FOwner != null)
                {
                    FOwner.ItemValueChanged(this);
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Change flux. </summary>
        /// <remarks>   This is enforce the DataItems rule about changing fluxes.  A class could deny altering a flux TO a certiain class of dataitem</remarks>
        /// <param name="FluxItem"> The flux item. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual bool AllowFluxChangeTo(CRF_DataItem FluxToItem)
        {
            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Determine if we allow flux change from. </summary>
        /// <remarks>   This is enforce the DataItems rule about changing fluxes.  A class could deny altering a flux FROM a certiain class of dataitem</remarks>
        /// <param name="FluxFromItem"> The flux from item. </param>
        ///
        /// <returns>   true if we allow flux change from, false if not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual bool AllowFluxChangeFrom(CRF_DataItem FluxFromItem)
        {
            return true;
        }
        /// <summary>
        /// Testing..... DAS
        /// </summary>
        /// <param name="FluxFromItem"></param>
        /// <returns></returns>
        public virtual bool AllowUnlimitedFluxChangeFrom(CRF_DataItem FluxFromItem)
        {
            return true;
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
        protected bool FDataItemChange = false;
        protected CRF_Network FOwnerNetwork;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the owner. </summary>
        ///
        /// <value> The owner. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Network Owner
        {
            get { return FOwnerNetwork; }
            set
            {
                FOwnerNetwork = value;
            }
        }

        public int MaxDepth()
        {
            int temp = 0;
            foreach (CRF_DataItem DI in this)
            {
                int TempDepth = DI.Depth();
                if (TempDepth > temp)
                {
                    temp = TempDepth;
                }
            }
            return temp;
        }
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
        /// <summary>   Searches for the first name. </summary>
        ///
        /// <remarks>   Mcquay, 3/15/2016. </remarks>
        ///
        /// <param name="aName">    The name. </param>
        ///
        /// <returns>   The found name. </returns>
        ///-------------------------------------------------------------------------------------------------

        public CRF_DataItem FindByName(string aName)
        {
            aName = aName.ToUpper();
            return this.Find(
                delegate(CRF_DataItem item)
                {
                    return aName == item.Name.ToUpper();
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

        public new void Add(CRF_DataItem anItem)
        {
            int index = FindIndex(anItem);
            if (index < 0)
            {
                base.Add(anItem);
                anItem.Owner = this;
            }
            else
            {
                // Some error?
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Decimals. </summary>
        ///
        /// <param name="a">    a. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual double Decimal(double a)
        {
            double temp = 0;
            temp = Convert.ToDouble(a.ToString("00.00"));
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Item value changed. </summary>
        ///
        /// <param name="ItemChanged">  true if item changed, false if not. </param>
        ///-------------------------------------------------------------------------------------------------

        public virtual void ItemValueChanged(CRF_DataItem Item)
        {
            ItemChanged = true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the item was changed. </summary>
        ///
        /// <value> true if item changed, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool ItemChanged
        {
            get { return FDataItemChange; }
            set
            {
                FDataItemChange = value;
                if (FOwnerNetwork != null)
                {
                    FOwnerNetwork.Ichanged(this);
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets List of the items changed. </summary>
        ///
        /// <value> The items changed. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<CRF_DataItem> ItemsChanged
        {
            get
            {
                List<CRF_DataItem> TheList = new List<CRF_DataItem>();
                foreach (CRF_DataItem DI in this)
                {
                    if (DI.ValueChange)
                    {
                        TheList.Add(DI);
                    }
                }
                return TheList;
            }
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////
    // ===========================================================================


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
        public double FAllocateMemory = 0;
        // double SAllocate = 0;

        /// <summary>   Values that represent the Method of the Flux (flux). </summary>
        public enum Method
        {
            /// <summary>   Unknown method </summary>
            amUnknown,
            /// <summary>   The allocated value is an absolute amount to be allocated </summary>
            amAbsolute,
            /// <summary>   The Flux is a percent of the total DataItems Value (Limit or Demand) </summary>
            amPercent
        };


        Method FMethod = Method.amUnknown;

        //CRF_Resource FromDataItem;
        //CRF_Consumer ToDataItem;
        CRF_DataItem FromDataItem;
        CRF_DataItem ToDataItem;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Mcquay, 1/25/2016. </remarks>
        ///
        /// <param name="DataItem"> The data item. </param>
        /// <param name="Amount">   The amount. </param>
        /// <param name="aMethod">  The method. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Flux(CRF_Consumer DataItem, double Amount, Method aMethod)
        {
            FAllocate = Amount;
            ToDataItem = DataItem;
            FMethod = aMethod;
            FromDataItem = null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Mcquay, 1/25/2016. </remarks>
        ///
        /// <param name="DataItem"> The data item. </param>
        /// <param name="Amount">   The amount. </param>
        /// <param name="aMethod">  The method. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Flux(CRF_Resource DataItem, double Amount, Method aMethod)
        {
            FAllocate = Amount;
            FromDataItem = DataItem;
            FMethod = aMethod;
            ToDataItem = null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="ToItem">   to item. </param>
        /// <param name="FromItem"> from item. </param>
        /// <param name="Amount">   The amount. </param>
        /// <param name="aMethod">  The method. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Flux(CRF_DataItem ToItem, CRF_DataItem FromItem, double Amount, Method aMethod)
        {
            FAllocate = Amount;
            FromDataItem = FromItem;
            FMethod = aMethod;
            ToDataItem = ToItem;
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the allocated. </summary>
        ///
        /// <remarks>   Mcquay, 1/25/2016. </remarks>
        ///
        /// <returns>   A double. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double Allocated()
        {
            double amount = 0;
            if (FromDataItem != null)
                switch (FMethod)
                {
                    case Method.amAbsolute:
                        amount = FAllocate;
                        break;
                    case Method.amPercent:
                        amount = FromDataItem.Value * FAllocate;
                        break;
                    case Method.amUnknown:
                        break;
                }
            return amount;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets an allocation. </summary>
        /// <remarks>   Resets the Allocation amount based on new Amount, based on methods of allocation.</remarks>
        /// <param name="NewAmount">    The new amount. </param>
        ///-------------------------------------------------------------------------------------------------

        public void SetAllocation(double NewAmount)
        {
            if (FromDataItem != null)
            {
                switch (FMethod)
                {
                    case Method.amAbsolute:
                        FAllocate = NewAmount;
                        break;
                    case Method.amPercent:
                        FAllocate = (NewAmount / FromDataItem.Value);
                        break;
                    case Method.amUnknown:
                        break;
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets source for the. </summary>
        ///
        /// <value> The source. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_DataItem Source
        //public CRF_Resource Source
        {
            get { return FromDataItem; }
            set { FromDataItem = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets target for the. </summary>
        ///
        /// <value> The target. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_DataItem Target
        //public CRF_Consumer Target
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the label. </summary>
        ///
        /// <value> The label. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Label
        {
            get
            {

                string temp = Allocated().ToString("00.0") + " ";

                if (FromDataItem != null)
                {
                    temp += "From:" + FromDataItem.Label + " ";
                }
                if (ToDataItem != null)
                {
                    temp += "To:" + ToDataItem.Label;
                }
                return temp;
            }

        }


        // ---------------------------------------------------------------------------------------
        // 

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
    ///             CRF_DataItems (Resources or Consumers) use CRF_FluxLists to keep track of a list Flux (flux - movement from a Resource to a Consumer)  
    ///             For a Resource, an Flux (flux) is a movement of some portion of its value to a Consumer
    ///             For a Consumer, an Flux (flux) is movement from a Resource to be added to its value.
    ///             The Resource knows where it is sending Fluxs(flux) based on its Flux list
    ///             The Consumers knows from whom it is receiving an allocation (flux) based on its Flux list.
    ///             An Flux FLUX represents this connection, and there is only one unique Flux object for each connection, and this uniuqe FLUX should be 
    ///             both in the Resource's and Consumer's Flux list.  Thus they share the same object.
    ///             This creates a delima because both Resources and Consumers can add a flux to their own Flux list, however, allocating a flux
    ///             means that the other object (Resource or Consumer) needs to have it added to their list as well. 
    ///</remarks>
    /// <seealso cref="System.Collections.Generic.List<QCRF_.CRF_Flux>"/>
    ///-------------------------------------------------------------------------------------------------

    public class CRF_FluxList : List<CRF_Flux>
    {
        CRF_DataItem FOwner = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="DataItemOwner">    The owner of the data item. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_FluxList(CRF_DataItem DataItemOwner)
            : base()
        {
            FOwner = DataItemOwner;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Searches for the first index. </summary>
        ///
        /// <remarks>   Mcquay, 1/25/2016. </remarks>
        ///
        /// <param name="anItem">   an item. </param>
        ///
        /// <returns>   The found index. </returns>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Searches for the first index. </summary>
        ///
        /// <param name="anItem">   an item. </param>
        ///
        /// <returns>   The found index. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int FindIndex(CRF_DataItem anItem)
        {
            return this.FindIndex(
                delegate(CRF_Flux item)
                {
                    return item.Source.Key == anItem.Key;
                }
            );
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Searches for the first match for the given crf data item. </summary>
        ///
        /// <param name="anItem">   an item. </param>
        ///
        /// <returns>   A CRF_Flux. </returns>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Searches for the first match for the given crf flux. </summary>
        ///
        /// <param name="anItem">   an item. </param>
        ///
        /// <returns>   A CRF_Flux. </returns>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Searches for the first target. </summary>
        /// <remarks>  Return null if not found</remarks>
        /// <param name="TargetName">   Name of the target. </param>
        ///
        /// <returns>   The found target. </returns>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Flux FindTarget(string TargetName)
        {
            CRF_Flux Result = null;
            TargetName = TargetName.ToUpper();
            foreach (CRF_Flux Flux in this)
            {
                if (Flux.Target.Name.ToUpper() == TargetName)
                {
                    Result = Flux;
                    break;
                }
            }
            return Result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Searches for the first source. </summary>
        /// <remarks>  Return null if not found</remarks>
        /// <param name="SourceName">   Name of the source. </param>
        ///
        /// <returns>   The found source. </returns>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Flux FindSource(string SourceName)
        {
            CRF_Flux Result = null;
            SourceName = SourceName.ToUpper();
            foreach (CRF_Flux Flux in this)
            {
                if (Flux.Source.Name.ToUpper() == SourceName)
                {
                    Result = Flux;
                    break;
                }
            }
            return Result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the given anItem. </summary>
        ///
        /// <param name="anItem">   an item. </param>
        ///-------------------------------------------------------------------------------------------------

        public void Remove(CRF_DataItem anItem)
        {
            int index = FindIndex(anItem);
            if (index > -1)
            {
                this.RemoveAt(index);
            }

        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds anItem. </summary>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="anItem">   an item to add. </param>
        ///-------------------------------------------------------------------------------------------------

        public void Add(CRF_Flux anItem)
        {
            // Ok, first lets set From and Tos
            if (anItem.Source == null)
            {
                anItem.Source = FOwner;

                //if (FOwner is CRF_Resource)
                //{
                //    anItem.Source = (FOwner as CRF_Resource);
                //}
            }
            if (anItem.Target == null)
            {
                anItem.Target = FOwner;
                //if (FOwner is CRF_Consumer)
                //{
                //    anItem.Target = (FOwner as CRF_Consumer);
                //}

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
                throw new Exception("This Flux is already in the list!");
            }

            // OK,  Does Flux list have an owner, ie A Resource or a consumer, if not then we are done
            if (FOwner != null)
            {
                // OK, see if we need to make sure target has this in their list
                if (anItem.Source == FOwner)
                {
                    CRF_DataItem SDI = anItem.Target;
                    // see if owner of this list is in the SDI list
                    CRF_Flux Target = SDI.FromFluxs.Find(anItem);
                    if (Target != null)
                    {
                        //OK it is there just ignore
                    }
                    else
                    {
                        // ok it is not there, let's modofy it
                        // create an Flux object
                        SDI.FromFluxs.Add(anItem);
                    }
                }
                else
                {
                    // ok see if we need to see if this is in the Source has this in its list
                    if (anItem.Target == FOwner)
                    {
                        CRF_DataItem SDI = anItem.Source;
                        // see if owener of this list is in the SDI list
                        CRF_Flux Target = SDI.ToFluxs.Find(anItem);
                        if (Target != null)
                        {
                            // ok, it is there, ignore
                        }
                        else
                        {
                            // ok it is not there, add it, of course this is going to cause it to go through this routine again 
                            SDI.ToFluxs.Add(anItem);
                        }
                    }
                }
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the owner. </summary>
        ///
        /// <value> The owner. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_DataItem Owner
        {
            get { return FOwner; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the total number of allocated. </summary>
        ///
        /// <value> The total number of allocated. </value>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sum to on item. </summary>
        ///
        /// <param name="TheItem">  the item. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double SumToOnItem(CRF_DataItem TheItem)
        {
            double result = 0;
            foreach (CRF_Flux Flux in this)
            {
                if (Flux.Target.Equals(TheItem))
                {
                    result += Flux.Allocated();
                }
            }

            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sum from on item. </summary>
        ///
        /// <param name="TheItem">  the item. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double SumFromOnItem(CRF_DataItem TheItem)
        {
            double result = 0;
            foreach (CRF_Flux Flux in this)
            {
                if (Flux.Target.Equals(TheItem))
                {
                    result += Flux.Allocated();
                }
            }

            return result;
        }
    }


    #endregion
    //===========================================================================

    #region Resource and Consumer Classes

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   CRF_ resource. </summary>
    /// <remarks>   This is a provider of resources, Limit is the total amount of resource available</remarks>
    /// <seealso cref="QCRF_.CRF_DataItem"/>
    ///-------------------------------------------------------------------------------------------------

    public class CRF_Resource : CRF_DataItem
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the ConsumerResourceModelFramework.CRF_Resource
        ///     class.</summary>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource()
            : base()
        {
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Mcquay, 1/25/2016. </remarks>
        ///
        /// <param name="aName">    The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource(string aName)
            : base(aName)
        {
            //FFluxList = FFluxs;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Mcquay, 1/25/2016. </remarks>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource(string aName, string aLabel, Color aColor)
            : base(aName, aLabel, aColor)
        {
            //FFluxList = FFluxs;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Mcquay, 1/25/2016. </remarks>
        ///
        /// <param name="aName">            The name. </param>
        /// <param name="aLabel">           The label. </param>
        /// <param name="aColor">           The color. </param>
        /// <param name="AvailableSupply">  The available supply. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource(string aName, string aLabel, Color aColor, double AvailableSupply)
            : base(aName, aLabel, aColor)
        {
            FValue = AvailableSupply;

            // OK Setup Consumers using Flux List to avoid confusion between Resources and Consumers
            // FFluxList = FFluxs;
        }
        //-----------------------



        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Resets the limit described by NewLimit. </summary>
        ///
        /// <remarks>   Mcquay, 1/25/2016. </remarks>
        /// <remarks>EDIT QUAY 3/23/18
        ///          Added a constraint factor  See Edit marks
        /// </remarks>
        /// <param name="NewLimit"> The new limit. </param>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void ResetLimit(double NewLimit)
        {
            // check of NewLimit is larger or smaller
            if (NewLimit > FValue)
            {
                // OK, need to adjust each flux so original new allocated values stays the same as old
                // Set Allocation will adjust the Allocated value of flux based on method of allocation being used 
                // get the oldvalues
                List<double> OldValues = new List<double>();
                foreach (CRF_Flux Flux in ToFluxs) //FFluxList)
                {
                    OldValues.Add(Flux.Allocated());
                }
                // set the new limit
                FValue = NewLimit;

                // loop through the fluxes and set values
                int index = 0;
                foreach (CRF_Flux Flux in ToFluxs) //FFluxList)
                {
                    // Check if a flux transfer is allowed, base CRF Resource should say no unless their is a need
                    // if not, then reset value to old value
                    if (!CRF_Utility.AllowFluxChange(this, Flux.Target))
                    {
                        Flux.SetAllocation(OldValues[index]);
                        index++;
                    }
                }

            }
            // ok just do it if less everyone lives with consequences, if ratio not reset, they get alot more
            FValue = NewLimit;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the limit. </summary>
        ///
        /// <value> The limit. </value>
        ///-------------------------------------------------------------------------------------------------

        public double Limit
        {
            get { return FValue; }
            set
            {
                // A force limit chaneg, need to chnage Unconstrained Value
                // Need to reallocate fluxes
                ResetLimit(value);
                // need to set value change to trigger 
                ValueChange = true;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the allocated. </summary>
        ///
        /// <value> The allocated. </value>
        ///-------------------------------------------------------------------------------------------------

        public double Allocated
        {
            get { return FToFluxs.TotalAllocated; } //FFluxList.TotalAllocated; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the net.</summary>
        ///
        /// <value> The net.</value>
        ///-------------------------------------------------------------------------------------------------

        public double Net
        {
            get { return FValue - Allocated; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a consumer. </summary>
        ///
        /// <param name="aConsumer">    The consumer. </param>
        /// <param name="amount">       The amount. </param>
        /// <param name="aMethod">      The method. </param>
        ///-------------------------------------------------------------------------------------------------

        public void AddConsumer(CRF_Consumer aConsumer, double amount, CRF_Flux.Method aMethod)
        {
            FToFluxs.Add(new CRF_Flux(aConsumer, amount, aMethod)); // FFluxList.Add(new CRF_Flux(aConsumer, amount, aMethod));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Deletes the consumer described by aConsumer. </summary>
        ///
        /// <param name="aConsumer">    The consumer. </param>
        ///-------------------------------------------------------------------------------------------------

        public void DeleteConsumer(CRF_Consumer aConsumer)
        {
            FToFluxs.Remove(aConsumer); //FFluxList.Remove(aConsumer);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Just take resources.</summary>
        /// <param name="Resource">  The resource.</param>
        /// <param name="GiveRatio"> The give ratio.</param>
        ///-------------------------------------------------------------------------------------------------

        public virtual void JustTakeResources(CRF_Resource Resource, double GiveRatio)
        {
            // get the nets resources
            double MyNet = Resource.Net;
            double AvailByAll = 0.0;
            if (MyNet < 0)
            {
                // OK find out how much is available to return
                foreach (CRF_Flux Flux in Resource.ToFluxs)
                {
                    if (CRF_Utility.AllowFluxChange(Flux.Target, Resource))
                    {
                        if (Flux.Target is CRF_Consumer)
                        {
                            double Avail = Flux.Allocated();
                            AvailByAll += Math.Abs(Avail);
                        }
                    }
                }
            }
            // if none avail dont do this
            if (AvailByAll > 0)
            {
                // ok go through each flux and see if transfer is warranted
                foreach (CRF_Flux Flux in Resource.ToFluxs)
                {
                    if (CRF_Utility.AllowFluxChange(Resource, Flux.Target))
                    {
                        if (Flux.Target is CRF_Consumer)
                        {

                            double Avail = Flux.Allocated(); //(Flux.Target as CRF_Consumer).Net;
                            // Remeber Consumer numbers are negative, if they have more resources allocated than demand
                            double TakeBack = ((MyNet * (Avail / AvailByAll)) * GiveRatio);
                            double original = Flux.Allocated();
                            double GiveNew = Flux.Allocated() + TakeBack; //((MyNet * (Avail / AvailByAll)) * GiveRatio);
                            Flux.SetAllocation(GiveNew);
                        }
                    }
                }
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Give less resources.</summary>
        /// <param name="Resource">  The resource.</param>
        /// <param name="GiveRatio"> The give ratio.</param>
        ///-------------------------------------------------------------------------------------------------

        public virtual void GiveLessResources(CRF_Resource Resource, double GiveRatio)
        {
            // get the nets resources
            double MyNet = Resource.Net;
            if (MyNet < 0)
            {
                // OK find out how much is available to return
                double AvailByAll = 0.0;
                foreach (CRF_Flux Flux in Resource.ToFluxs)
                {
                    if (CRF_Utility.AllowFluxChange(Flux.Target, Resource))
                    {
                        if (Flux.Target is CRF_Consumer)
                        {
                            double Avail = (Flux.Target as CRF_Consumer).Net;
                            // Remeber Consumer numbers are negative, if they have more resources allocated than demand
                            if (Avail < 0)
                            {
                                AvailByAll += Math.Abs(Avail);
                            }
                        }
                    }
                }
                // if none avail dont do this
                if (AvailByAll > 0)
                {
                    // ok go through each flux and see if transfer is warranted
                    foreach (CRF_Flux Flux in Resource.ToFluxs)
                    {
                        if (CRF_Utility.AllowFluxChange(Resource, Flux.Target))
                        {
                            if (Flux.Target is CRF_Consumer)
                            {

                                double Avail = (Flux.Target as CRF_Consumer).Net;
                                // Remeber Consumer numbers are negative, if they have more resources allocated than demand
                                if (Avail < 0)
                                {
                                    double GiveNew = +Flux.Allocated() - ((MyNet * (Avail / AvailByAll)) * GiveRatio);
                                    Flux.SetAllocation(GiveNew);
                                }
                            }
                        }
                    }
                }

            }

        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Give more resources. </summary>
        ///  <remarks>  GiveNewRatio is used to decide how much of the available resources should be allocated among the consumers</remarks>
        /// <param name="Resource">     The resource. </param>
        /// <param name="GiveNewRatio"> The give new ratio. </param>
        ///-------------------------------------------------------------------------------------------------

        public virtual void GiveMoreResources(CRF_Resource Resource, double GiveNewRatio)
        {
            // get the nets resources
            double MyNet = Resource.Net;
            // only do this if the resources are available
            if (MyNet > 0)
            {
                double NeededByAll = 0.0;
                // OK find out how much is needed
                foreach (CRF_Flux Flux in Resource.ToFluxs)
                {
                    if (CRF_Utility.AllowFluxChange(Resource, Flux.Target))
                    {
                        if (Flux.Target is CRF_Consumer)
                        {
                            double Needed = (Flux.Target as CRF_Consumer).Net;
                            NeededByAll += Needed;
                        }
                    }
                }
                // if none needed dont do this
                if (NeededByAll > 0)
                {
                    // ok go through each flux and see if transfer is warranted
                    foreach (CRF_Flux Flux in Resource.ToFluxs)
                    {
                        if (CRF_Utility.AllowFluxChange(Resource, Flux.Target))
                        {
                            if (Flux.Target is CRF_Consumer)
                            {
                                double Needed = (Flux.Target as CRF_Consumer).Net;
                                // Remember Consumer net is negative if more allocated than demand
                                if (Needed > 0)
                                {
                                    double GiveNew = ((MyNet * (Needed / NeededByAll)) * GiveNewRatio) + Flux.Allocated();
                                    Flux.SetAllocation(GiveNew);
                                }
                            }
                        }
                    }
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Adjusts Limit to Not Exceed amount Allocated to Consumers</summary>
        /// <remarks> Quay, 3/31/2018.</remarks>
        /// <remarks> This is a low level serious method that should be used with caution.  
        ///           It adjusts Limit to not exceed amount allocated to consumers.  </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void CapToDemand()
        {
            double currentDemand = Allocated;
           
            if ((int)Limit > (int)currentDemand)
            {
                Limit = currentDemand;
            }


        }
        // =================================================================================================
        /// <summary>
        ///  Sampson attempt at adding cost to the CRF resource list
        ///  12.17.21
        /// </summary>
        /// <param name="Resource"></param>
        /// <param name="GiveNewRatio"></param>
        public virtual void ResourceCost(CRF_Resource Resource, double GiveNewRatio)
        {
            double MyNet = Resource.Net;
            // only do this if the resources are available
            if (MyNet > 0)
            {
                double NeededByAll = 0.0;
                // OK find out how much is needed
                foreach (CRF_Flux Flux in Resource.ToFluxs)
                {
                    if (CRF_Utility.AllowFluxChange(Resource, Flux.Target))
                    {
                        if (Flux.Target is CRF_Consumer)
                        {
                            double Needed = (Flux.Target as CRF_Consumer).Net;
                            NeededByAll += Needed;
                        }
                    }
                }
            }



        }
            // ==================================================================================================
    }

        //===========================================================================

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   List of crf resources. </summary>
        ///
        /// <remarks>   Mcquay, 1/25/2016. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public class CRF_ResourceList : CRF_DataItemList
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Mcquay, 1/25/2016. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public CRF_ResourceList()
            : base()
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the allocated. </summary>
        ///
        /// <value> The allocated. </value>
        ///-------------------------------------------------------------------------------------------------

        public double Allocated
        {
            get
            {
                double temp = 0;
                foreach (CRF_Resource skRes in this)
                {
                    temp += skRes.Allocated;
                }
                return temp;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the total net resource in list. </summary>
        ///<remarks>    Sums negative and positive net valurs, so could be 0 with some positive and some negative</remarks>
        /// <value> The net. </value>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the neg net. </summary>
        /// <remarks>   This totals only resources that have a negative net value</remarks>
        /// <value> The neg net. </value>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the position net. </summary>
        /// <remarks>   This totals only resources that have a positive net value</remarks>
        /// <value> The position net. </value>
        ///-------------------------------------------------------------------------------------------------

        public double PosNet
        {
            get
            {
                double temp = 0;
                foreach (CRF_Resource skRes in this)
                {
                    if (skRes.Net > 0)
                    {
                        temp += skRes.Net;
                    }
                }
                return temp;
            }
        }

        //--------------------------------
        // Where resource flux(s) changed
        // DAS note (original code)
        // ------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the limit. </summary>
        /// <remarks>   This is the total amount of this resource that can be allocated</remarks>
        /// <value> The limit. </value>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   List of Resources is Balanced </summary>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Balanceds. </summary>
        ///
        /// <param name="Canbe">    [out] The canbe. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a resource. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        /// <param name="aLimit">   The limit. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource AddResource(string aName, string aLabel, Color aColor, double aLimit)
        {
            CRF_Resource Temp = new CRF_Resource(aName, aLabel, aColor, aLimit);
            this.Add(Temp);
            return Temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds to resource. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        /// <param name="aLimit">   The limit. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource AddToResource(string aName, string aLabel, Color aColor, double aLimit)
        {
            CRF_Resource Temp = new CRF_Resource(aName, aLabel, aColor, aLimit);
            this.Add(Temp);
            return Temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Adjusts Limitsd to not exceed amount allocated to Consumers.</summary>
        /// <remarks> Quay, 3/31/2018.</remarks>
        ///-------------------------------------------------------------------------------------------------

        public void CapLimitToDemand()
        {
            foreach (CRF_Resource skRes in this)
            {
                skRes.CapToDemand();
            }
        }
    }
    //===========================================================================

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Crf consumer. </summary>
    /// <remarks>   This is a consumer of resources.  Demand is the amount of resources desired </remarks>
    /// <seealso cref="ConsumerResourceModelFramework.CRF_DataItem"/>
    ///-------------------------------------------------------------------------------------------------

    public class CRF_Consumer : CRF_DataItem
    {

        // CLEAN UP double FTotalResources = 0;
        // OK Setup Resources using Flux List to avoid confusion between Resources and Consumers
        //CRF_FluxList FResources;
        // CLEAN UP CRF_FluxList FConsumers;

        /// <summary>   Default constructor. </summary>
        public CRF_Consumer()
            : base()
        {
            // OK Setup Resources using Flux List to avoid confusion between Resources and Consumers
            //FResources = FFluxs;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Consumer(string aName)
            : base(aName)
        {
            // OK Setup Resources using Flux List to avoid confusion between Resources and Consumers
            //FResources = FFluxs;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Consumer(string aName, string aLabel, Color aColor)
            : base(aName, aLabel, aColor)
        {
            // OK Setup Resources using Flux List to avoid confusion between Resources and Consumers
            //FResources = FFluxs;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        /// <param name="Demand">   The Demand of this resource. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Consumer(string aName, string aLabel, Color aColor, double Demand)
            : base(aName, aLabel, aColor)
        {
            FValue = Demand;
            // OK Setup Resources using Flux List to avoid confusion between Resources and Consumers
            //FResources = FFluxs;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a consumer. </summary>
        ///
        /// <param name="aConsumer">    The consumer. </param>
        /// <param name="amount">       The amount. </param>
        /// <param name="aMethod">      The method. </param>
        ///-------------------------------------------------------------------------------------------------

        public void AddConsumer(CRF_Consumer aConsumer, double amount, CRF_Flux.Method aMethod)
        {
            FToFluxs.Add(new CRF_Flux(aConsumer, amount, aMethod));// FFluxList.Add(new CRF_Flux(aConsumer, amount, aMethod));
        }

        public void AddToItem(CRF_DataItem Item, double amount, CRF_Flux.Method aMethod)
        {
            FToFluxs.Add(new CRF_Flux(Item, this, amount, aMethod));// FFluxList.Add(new CRF_Flux(aConsumer, amount, aMethod));
        }

        protected virtual double GetDemand()
        {
            return FValue;
        }

        protected virtual void SetDemand(double value)
        {
            FValue = value;
        }
        ////-----------------------
        /// <summary>
        ///  The Demand of this resource
        /// </summary>
        public double Demand
        {
            get { return GetDemand(); }
            set
            {
                SetDemand(value);
                ValueChange = true;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the total resources Allocated. </summary>
        ///
        /// <value> The resources. </value>
        ///-------------------------------------------------------------------------------------------------

        public double ResourcesAllocated
        {
            get { return FFromFluxs.TotalAllocated; } //FResources.TotalAllocated; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the net Allocated across all resources. </summary>
        ///
        /// <value> The net. </value>
        ///-------------------------------------------------------------------------------------------------

        public double Net
        {
            get { return FValue - ResourcesAllocated; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a resource. </summary>
        ///
        /// <param name="aResource">    The resource. </param>
        /// <param name="amount">       The amount. </param>
        /// <param name="aMethod">      The method. </param>
        ///-------------------------------------------------------------------------------------------------

        public void AddResource(CRF_Resource aResource, double amount, CRF_Flux.Method aMethod)
        {
            FFromFluxs.Add(new CRF_Flux(aResource, amount, aMethod));
            // FResources.Add(new CRF_Flux(aResource, amount, aMethod));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the resource described by aResource. </summary>
        ///
        /// <param name="aResource">    The resource. </param>
        ///-------------------------------------------------------------------------------------------------

        public void RemoveResource(CRF_Resource aResource)
        {
            FFromFluxs.Remove(aResource);//FResources.Remove(aResource);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Give back extra. </summary>
        ///
        /// <param name="Consumer"> The consumer. </param>
        ///-------------------------------------------------------------------------------------------------

        public virtual void GiveBackExtra(CRF_Consumer Consumer)
        {
            // how many Resources can be allocated Total
            double ResAllocCnt = 0.0;
            double ResAllocTotal = 0.0;
            double TheNetValue = Consumer.Net;
            double ToleranceRatio = (TheNetValue / Consumer.Demand) * -1;

            if (ToleranceRatio > CRF_Utility.FluxSizeTolerance)
            {
                foreach (CRF_Flux Flux in Consumer.FFromFluxs)
                {
                    if (CRF_Utility.AllowFluxChange(Consumer, Flux.Source))
                    {
                        if (Flux.Source is CRF_Resource)
                        {
                            ResAllocCnt++;
                            ResAllocTotal += (Flux.Source as CRF_Resource).Allocated;
                        }
                    }
                }

                foreach (CRF_Flux Flux in Consumer.FFromFluxs)
                {
                    if (CRF_Utility.AllowFluxChange(Consumer, Flux.Source))
                    {
                        // excess to allocate 
                        if (Flux.Source is CRF_Resource)
                        {

                            double ReAllocate = Flux.Allocated() + (((Flux.Source as CRF_Resource).Allocated / ResAllocTotal) * TheNetValue);
                            Flux.SetAllocation(ReAllocate);
                        }
                    }
                }

            }
        }

        // ============================================================================================================================



        // =============================================================================================================================
    }
    public class CRF_Other : CRF_DataItem
    {
        // QUAY 
        // WHAT IS THIS FOR?
        // 
        //double FTotalResources = 0;
        // OK Setup Resources using Flux List to avoid confusion between Resources and Consumers
        //CRF_FluxList FResources;
        //CRF_FluxList FOther;

        /// <summary>   Default constructor. </summary>
        public CRF_Other()
            : base()
        {
            // OK Setup Resources using Flux List to avoid confusion between Resources and Consumers
            //FResources = FFluxs;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Other(string aName)
            : base(aName)
        {
            // OK Setup Resources using Flux List to avoid confusion between Resources and Consumers
            //FResources = FFluxs;
        }
        public CRF_Other(string aName, double myValue)
            : base(aName, myValue)
        {
            FValue = myValue;
            // OK Setup Resources using Flux List to avoid confusion between Resources and Consumers
            //FResources = FFluxs;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Other(string aName, string aLabel, Color aColor)
            : base(aName, aLabel, aColor)
        {
            // OK Setup Resources using Flux List to avoid confusion between Resources and Consumers
            //FResources = FFluxs;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        /// <param name="Demand">   The Demand of this resource. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Other(string aName, string aLabel, Color aColor, double Demand)
            : base(aName, aLabel, aColor)
        {
            FValue = Demand;
            // OK Setup Resources using Flux List to avoid confusion between Resources and Consumers
            //FResources = FFluxs;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a consumer. </summary>
        ///
        /// <param name="aConsumer">    The consumer. </param>
        /// <param name="amount">       The amount. </param>
        /// <param name="aMethod">      The method. </param>
        ///-------------------------------------------------------------------------------------------------

        public void AddOther(CRF_Other aOther, double amount, CRF_Flux.Method aMethod)
        {
       //     FToFluxs.Add(new CRF_Flux(aOther, amount, aMethod));// FFluxList.Add(new CRF_Flux(aConsumer, amount, aMethod));
        }

        public void AddToItem(CRF_DataItem Item, double amount, CRF_Flux.Method aMethod)
        {
            FToFluxs.Add(new CRF_Flux(Item, this, amount, aMethod));// FFluxList.Add(new CRF_Flux(aConsumer, amount, aMethod));
        }

        protected virtual double GetOther()
        {
            return FValue;
        }

        protected virtual void SetOther(double value)
        {
            FValue = value;
        }
        ////-----------------------
        /// <summary>
        ///  The Demand of this resource
        /// </summary>
        public double CurrentState
        {
            get { return GetOther(); }
            set
            {
                SetOther(value);
                ValueChange = true;
            }
        }

        
    }
    //=========================================================================================================

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   List of crf consumers. /</summary>
    ///
    /// <seealso cref="ConsumerResourceModelFramework.CRF_DataItemList"/>
    ///-------------------------------------------------------------------------------------------------

    public class CRF_ConsumerList : CRF_DataItemList
    {

        //-----------------------
        /// <summary>   Default constructor. </summary>
        public CRF_ConsumerList()
            : base()
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the demand. </summary>
        ///
        /// <value> The demand. </value>
        ///-------------------------------------------------------------------------------------------------

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
            set
            {

                double temp = 0;
                foreach (CRF_Consumer skCons in this)
                {
                    temp = value;//+= skCons.Demand;
                    // MyNet.Ichanged(this);
                }


            }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the net Allocated across the list. </summary>
        ///
        /// <value> The net. </value>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the neg net. </summary>
        /// <remarks>   This totals only Resources that have a positive net value</remarks>
        ///
        /// <value> The neg net. </value>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the position net. </summary>
        /// <remarks>   This totals only Resources that have a positive net value</remarks>
        /// <value> The position net. </value>
        ///-------------------------------------------------------------------------------------------------

        public double PosNet
        {
            get
            {
                double temp = 0;
                foreach (CRF_Consumer skCos in this)
                {
                    if (skCos.Net > 0)
                    {
                        temp += skCos.Net;
                    }
                }
                return temp;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Balanceds this object. </summary>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Balanceds this object. </summary>
        ///
        /// <param name="Canbe">    [out] The canbe. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

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
                if ((skCons.Net > Test) || (skCons.Net < (-1 * Test)))
                {
                    result = false;
                }
            }
            Canbe = ((totalNet > TotTest) || (totalNet < (-1 * TotTest)));
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the resources. </summary>
        ///
        /// <value> The resources. </value>
        ///-------------------------------------------------------------------------------------------------

        public double Resources
        {
            get
            {
                double temp = 0;
                foreach (CRF_Consumer skCons in this)
                {
                    temp += skCons.ResourcesAllocated;
                }
                return temp;
            }
        }

        //------------------------------------------------------------------------

        public CRF_Consumer AddConsumer(string aName, string aLabel, Color aColor, double aDemand)
        {
            CRF_Consumer Temp = new CRF_Consumer(aName, aLabel, aColor, aDemand);
            this.Add(Temp);

            return Temp;
        }

    }

    public class CRF_Sink : CRF_Consumer
    {
        /// <summary>   Default constructor. </summary>
        public CRF_Sink()
            : base()
        {
            // OK Setup Resources using Flux List to avoid confusion between Resources and Consumers
            //FResources = FFluxs;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Sink(string aName)
            : base(aName)
        {
            // OK Setup Resources using Flux List to avoid confusion between Resources and Consumers
            //FResources = FFluxs;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Sink(string aName, string aLabel, Color aColor)
            : base(aName, aLabel, aColor)
        {
            // OK Setup Resources using Flux List to avoid confusion between Resources and Consumers
            //FResources = FFluxs;
        }

        protected override double GetDemand()
        {
            double temp = FFromFluxs.TotalAllocated;
            FValue = temp;
            return temp;
        }

        protected override void SetDemand(double value)
        {
            // Do nothing, can not set demand
        }


    }
    #endregion Resource and Consumer Classes

    //===========================================================================================================
    public class CRF_OtherList : CRF_DataItemList
    {

        //-----------------------
        /// <summary>   Default constructor. </summary>
        public CRF_OtherList()
            : base()
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state. </summary>
        ///
        /// <value> The demand. </value>
        ///-------------------------------------------------------------------------------------------------

        //public double Population
        //{
        //    get
        //    {
        //        double temp = 0;
        //        foreach (CRF_Other skCons in this)
        //        {
        //            temp += skCons.CurrentState;

        //        }
        //        return temp;
        //    }
        //    set
        //    {

        //        double temp = 0;
        //        foreach (CRF_Other skCons in this)
        //        {
        //            temp = value;//+= skCons.Demand;
        //            // MyNet.Ichanged(this);
        //        }


        //    }
        //}
       
  
        //------------------------------------------------------------------------

        public CRF_Other AddOther(string aName, string aLabel, Color aColor, double aDemand)
        {
            CRF_Other Temp = new CRF_Other(aName, aDemand);
            this.Add(Temp);

            return Temp;
        }

    }

    //*****************************************************
    // CRF Network
    // 
    // This is the class that manages the list of resources and classes
    // 
    // **************************************************
    /// <summary>   Crf network. </summary>
    public class CRF_Network
    {
        protected CRF_ResourceList FResources;
        protected CRF_ConsumerList FConsumers;
        protected CRF_OtherList FOther;

        protected string FName = "";
        // Add this to give a way for each CRF_Network to have a unique ID to be used to identify it.

        protected Guid FUniqueKey = Guid.NewGuid();

        bool FResourceChange = false;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Network reset delegate. </summary>
        ///
        /// <remarks>   Mcquay, 1/25/2016. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public delegate void NetworkResetDelegate();


        /// <summary> The callback, called if not null when network is reset.</summary>
        protected NetworkResetDelegate FCallback = null;

        //--------------------------------------
        // CLEAN UP double FMaxTolerance = .01;

        /// <summary>   Default constructor. </summary>
        public CRF_Network()
            : base()
        {
            FResources = new CRF_ResourceList();
            FConsumers = new CRF_ConsumerList();
            FOther = new CRF_OtherList();
            FName = FUniqueKey.ToString("X");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="Name">             The name. </param>
        /// <param name="aResourceList">    List of resources. </param>
        /// <param name="aConsumerList">    List of consumers. </param>
        /// <param name="Callback">         The callback. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Network(string aName, CRF_ResourceList aResourceList, CRF_ConsumerList aConsumerList, NetworkResetDelegate Callback)
            : base()
        {
            // Assign the name unless it is null then us unique ID
            if (aName == "")
            {
                FName = FUniqueKey.ToString("X");
            }
            else
            {
                FName = aName;
            }
            FResources = aResourceList;
            FResources.Owner = this;
            FConsumers = aConsumerList;
            FConsumers.Owner = this;

            FCallback = Callback;
            if (FCallback != null)
            {
                FCallback();
            }

        }
        //-----------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the name of the Network </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Name
        {
            get { return FName; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets an unique identifier for network. </summary>
        ///
        /// <value> The identifier of the unique network. </value>
        ///-------------------------------------------------------------------------------------------------

        public Guid UniqueID
        {
            get { return FUniqueKey; }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the resources. </summary>
        ///
        /// <value> The resources. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_ResourceList Resources
        {
            get { return FResources; }
        }
        //-----------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the consumers. </summary>
        ///
        /// <value> The consumers. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_ConsumerList Consumers
        {
            get { return FConsumers; }
        }
        //-----------------------------------------------------------

        public CRF_OtherList Other
        {
            get { return FOther; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Balanceds this object. </summary>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool Balanced()
        {
            return (Resources.Balanced() && Consumers.Balanced());
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ichangeds. </summary>
        ///
        /// <param name="me">   me. </param>
        ///-------------------------------------------------------------------------------------------------

        public void Ichanged(CRF_DataItemList me)
        {
            FResourceChange = true;
            ResetNetwork();
        }
        //-----------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Are Network Resources and Consumers Balanced. </summary>
        ///
        /// <param name="ResourcesBalanced">    [out] The resources balanced. </param>
        /// <param name="ConsumersBalanced">    [out] The consumers balanced. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool Balanced(out bool ResourcesBalanced, out bool ConsumersBalanced)
        {
            ResourcesBalanced = Resources.Balanced();
            ConsumersBalanced = Consumers.Balanced();
            return (ResourcesBalanced && ConsumersBalanced);
        }
        // -----------------------------------------------------------. 

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the call back method. </summary>
        ///
        /// <value> The call back method. </value>
        ///-------------------------------------------------------------------------------------------------

        public NetworkResetDelegate CallBackMethod
        {
            get { return FCallback; }
            set { FCallback = value; }
        }
        //-----------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets a network up with Resources and Consumers. </summary>
        ///
        /// <param name="aResourceList">    List of resources. </param>
        /// <param name="aConsumerList">    List of consumers. </param>
        ///-------------------------------------------------------------------------------------------------

        public void SetNetwork(CRF_ResourceList aResourceList, CRF_ConsumerList aConsumerList)
        {
            FResources = aResourceList;
            FResources.Owner = this;
            FConsumers = aConsumerList;
            FConsumers.Owner = this;

            if (FCallback != null)
            {
                FCallback();
            }
        }

        /// <summary>   Resets the network. </summary>
        public void ResetNetwork()
        {
            if (FResourceChange)
            {
                FResourceChange = false;
                optimize();
            }

            if (FCallback != null)
            {
                FCallback();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Optimize phase 3. </summary>
        ///
        ///-------------------------------------------------------------------------------------------------
        /// <remarks>   If Resources is over allocated, reduces allocations to consumers to reach balance</remarks>

        protected void OptimizePhase3()
        {

            // See how much the Total Resources Over Allocated is
            double OverResources = Math.Abs(FResources.NegNet);

            if (OverResources > 0)
            {
                double NetRatio = OverResources / FResources.Allocated;
                if (NetRatio > CRF_Utility.FluxSizeTolerance)
                {
                    foreach (CRF_Resource Res in FResources)
                    {
                        Res.JustTakeResources(Res, 1);
                    }
                }
            }
        }

        /// <summary>   Optimize phase 2. </summary>
        /// <remarks>   Rebalances the Resources, ie reditricute fluxes</remarks>
        protected virtual void OptimizePhase2()
        {
            // ----------- see what resources need to be rebalanced -------------------

            //--------------------------------------------
            // Resources that are over allocated, IE are allocated to deliver more resources than they have

            // See how much the Total Resources Over Allocated is
            double OverResources = Math.Abs(FResources.NegNet);
            // see how much extra resources the consumers have
            double AvailableResources = Math.Abs(FConsumers.NegNet);
            // Take the least of the two
            double XtraAvailable = Math.Min(OverResources, AvailableResources);
            //if (OverResources > AvailabledResources)
            //{
            //    XtraAvailable = AvailableResources;
            //}
            //else
            //{
            //    XtraNeeded = XtraResources;
            //}

            double TakeNewRatio = XtraAvailable / OverResources;
            double NetRatio = XtraAvailable / FResources.Allocated;
            if (NetRatio > CRF_Utility.FluxSizeTolerance)
            {
                foreach (CRF_Resource Res in FResources)
                {
                    Res.GiveLessResources(Res, TakeNewRatio);
                }
            }

            //-----------------------------------------------------------
            // REsources that are under allocated, IE have more resources than allocated

            // see what total extra positive resources are available
            double XtraResources = FResources.PosNet;
            // see what needs are
            double NeededResources = FConsumers.PosNet;
            // Available Needed XtraResources
            double XtraNeeded = 0.0;
            if (XtraResources > NeededResources)
            {
                XtraNeeded = NeededResources;
            }
            else
            {
                XtraNeeded = XtraResources;
            }

            double GiveNewRatio = XtraNeeded / XtraResources;
            NetRatio = XtraNeeded / FResources.Allocated;
            if (NetRatio > CRF_Utility.FluxSizeTolerance)
            {
                foreach (CRF_Resource Res in FResources)
                {
                    Res.GiveMoreResources(Res, GiveNewRatio);
                }
            }
        }

        /// <summary>   Optimize phase 1. </summary>
        /// <remarks>  This phase reallocated extra resources not needed to meet demand from consumers back to resources</remarks>
        protected virtual void OptimizePhase1()
        {
            // first, go through Consumers and give back extra resources if allowed
            foreach (CRF_Consumer TheCons in FConsumers)
            {
                TheCons.GiveBackExtra(TheCons);
            }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Optimizes the Network. </summary>
        ///<remarks>    Calls the optimization Phases and then returns the net balance of network</remarks>
        /// <returns>  The Net between Resources (supply) and Consumers (demand). </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual double optimize()
        {
            double NetValue = 0.0;

            // Each Consumer is quearied to give back resources not need to meet demand
            OptimizePhase1();
            // Each Resource is queried to allocated more resources if available and needed
            OptimizePhase2();
            // Each Resource reduces its allocations if over allocated.
            OptimizePhase3();

            NetValue = FResources.Net - FConsumers.Net;
            if (FCallback != null)
            {
                FCallback();
            }
            return NetValue;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets flux allocated. </summary>
        ///
        /// <remarks>   Mcquay, 3/15/2016. </remarks>
        ///
        /// <param name="ResField">     The resource field. </param>
        /// <param name="ConsField">    The cons field. </param>
        ///
        /// <returns>   The flux , null if not found </returns>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Flux FindFlux(string ResField, string ConsField)
        {
            CRF_DataItem Res = Resources.FindByName(ResField);
            CRF_Flux theFlux = Res.ToFluxs.FindTarget(ConsField);
            return theFlux;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Adjusts Resources Limits to Not Exceed Amount Allocated to Consumers.</summary>
        /// <remarks> Quay, 3/31/2018.</remarks>
        ///-------------------------------------------------------------------------------------------------

        public void CapLimitToDemand()
        {
            FResources.CapLimitToDemand();
        }

    }
    

}
    // -------------------------------------------------------------------------------------------------
    






