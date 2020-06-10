using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaterSimDCDC;

namespace WaterSimDCDC.WestVisual
{

    public delegate void AfterCheckCallBack();
    ///-------------------------------------------------------------------------------------------------
    /// <summary> A region class.</summary>
    ///
    /// <remarks> Quay, 3/11/2018.</remarks>
    ///-------------------------------------------------------------------------------------------------

    public class RegionClass
    {
        string Fname;
        eProvider FeP;
        int FIndex;
        string FFldName;
        StateClass FOwner;

        public RegionClass(string aName, eProvider AnEProvider, int aIndex, string aFieldName, StateClass owner)
        {
            Fname = aName;
            FeP = AnEProvider;
            FIndex = aIndex;
            FOwner = owner;
            FFldName = aFieldName;
        }

        public string Name
        { get { return Fname; } }

        public eProvider Provider
        { get { return FeP; } }

        public int Index
        { get { return FIndex; } }

        public string Fieldname
        { get { return FFldName; } }

        public StateClass Owner
        { get { return FOwner; } }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> A state class.</summary>
    ///
    /// <remarks> Quay, 3/11/2018.</remarks>
    ///-------------------------------------------------------------------------------------------------

    public class StateClass
    {
        string FName;
        List<RegionClass> FRegions;

        public StateClass(string aName)
        {
            FName = aName;
            FRegions = new List<RegionClass>();
        }

        public void AddRegion(string aName, eProvider aProvider, int anIndex, string aField)
        {
            RegionClass Temp = FRegions.Find(delegate (RegionClass Reg) { return Reg.Name == aName; });
            if (Temp == null)
            {
                FRegions.Add(new RegionClass(aName, aProvider, anIndex, aField, this));
            }
        }
        public string Name
        { get { return FName; } }

        public List<RegionClass> Regions
        { get { return FRegions; } }

    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> A region tree view class.</summary>
    ///
    /// <remarks> Quay, 4/1/2018.</remarks>
    ///
    /// <seealso cref="T:System.Windows.Forms.TreeView"/>
    ///-------------------------------------------------------------------------------------------------

    public class RegionTreeViewClass : TreeView
    {
        WaterSimManager FWSim;
        List<StateClass> TheStates = new List<StateClass>();
        List<string> TheSelectedRegions = new List<string>();
        AfterCheckCallBack TheCallback = null;

        public RegionTreeViewClass()
        {

        }

        public WaterSimManagerClass WaterSim
        {
            set
            {
                FWSim = (WaterSimManager)value;
                SetupControls();
                
            }
        }
        private void SetupControls()
        {
            if (FWSim != null)
            {
                foreach (string StateName in ProviderClass.StateNames)
                {
                    StateClass TempStateClass = new StateClass(StateName);
                    TheStates.Add(TempStateClass);
                }
                for (int i = 0; i < ProviderClass.ProviderNameList.Length; i++)
                {
                    string MySTate = ProviderClass.RegionStateName[i];
                    StateClass MyStateClass = TheStates.Find(
                        delegate (StateClass SC) { return SC.Name == MySTate; });
                    if (MyStateClass != null)
                    {
                        string RName = ProviderClass.ProviderNameList[i];
                        eProvider Ep = (eProvider)i;
                        string RFld = ProviderClass.FieldNameList[i];
                        MyStateClass.AddRegion(RName, Ep, i, RFld);
                    }

                }

                foreach (StateClass SC in TheStates)
                {
                    TreeNode TempStateNode = new TreeNode(SC.Name);
                    foreach (RegionClass RC in SC.Regions)
                    {
                        TempStateNode.Nodes.Add(new TreeNode(RC.Name));
                    }
                    this.Nodes.Add(TempStateNode);
                }

                
            }
        }

        public AfterCheckCallBack CallBackHandler
        {
            get { return TheCallback; }
            set { TheCallback = value; }
        }
        protected void AddName(string aName)
        {
            TheSelectedRegions.Add(aName);
        }

        protected void DeleteName(string aName)
        {
            TheSelectedRegions.Remove(aName);
        }

        protected override void OnAfterCheck(TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    if (e.Node.Checked)
                    {
                        foreach (TreeNode TN in e.Node.Nodes)
                        {
                            TN.Checked = true;
                            AddName(TN.Text);
                        }
                    }
                    else
                    {
                        foreach (TreeNode TN in e.Node.Nodes)
                        {
                            TN.Checked = false;
                            DeleteName(TN.Text);
                        }
                    }
                }
                else
                {
                    // Where we draw the graph
                    string UnitName = e.Node.Text;
                    if (e.Node.Checked)
                    {
                        // see if in the display list, if not, add
                        AddName(e.Node.Text);
                    }
                    else
                    {
                        // see if in the display list, if it is delete
                        // Also, Uncheck the owner, not all are checked
                        e.Node.Parent.Checked = false;
                        DeleteName(e.Node.Text);

                    }
                }
                if (TheCallback != null)
                {
                    TheCallback();
                }

            }
        }

        public string[] SelectedRegions
        {
            get { return TheSelectedRegions.ToArray(); }
        }
    }
}
