using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaterSimDCDC;

namespace WaterSim_West_v_1
{
    public partial class ShowMultipleSankeyV1 : Form
    {

        WaterSimManager WSIM = null;

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
        List<StateClass> TheStates = new List<StateClass>();

        public ShowMultipleSankeyV1()
        {
            InitializeComponent();
        }

        public ShowMultipleSankeyV1(WaterSimManager WaterSim)
        {
            InitializeComponent();
            WSIM = WaterSim;
            SetupControls();

        }

        private void SetupControls()
        {
            if (WSIM != null)
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
                    treeViewRegions.Nodes.Add(TempStateNode);
                }


            }
        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddSankey(string UnitName)
        {
           ConsumerResourceModelFramework.SankeyGraph sankeyGraphUnit = new ConsumerResourceModelFramework.SankeyGraph();
           WaterSimDCDC.Generic.CRF_Unit_Network Temp = WSIM.WaterSimWestModel.GetUnitModel(UnitName).TheCRFNetwork;
            //  sankeyGraphUnit.Network = WSIM.WaterSimWestModel.GetUnitModel(UnitName).TheCRFNetwork;
            sankeyGraphUnit.Network = Temp; // WSIM.WaterSimWestModel.GetUnitModel(UnitName).TheCRFNetwork;

            flowLayoutPanelSankey.Controls.Add(sankeyGraphUnit);

        }

        private void DeleteSankey(string UnitName)
        {
            List<Control> TheSankeys = new List<Control>();
            foreach (Control Ctrl in flowLayoutPanelSankey.Controls)
            {
                if (Ctrl.Name == "SankeyGraph")
                {
                    if ((Ctrl as ConsumerResourceModelFramework.SankeyGraph).Network.Name == UnitName)
                    {
                        TheSankeys.Add(Ctrl);
                    }
                }
            }
            
            if (TheSankeys.Count>0)
            {
                foreach(Control CTRL in TheSankeys) flowLayoutPanelSankey.Controls.Remove(CTRL);
            }
        }
        private void treeViewRegions_AfterCheck(object sender, TreeViewEventArgs e)
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
                            AddSankey(TN.Text);
                        }
                    }
                    else
                    {
                        foreach (TreeNode TN in e.Node.Nodes)
                        {
                            TN.Checked = false;
                            DeleteSankey(TN.Text);
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
                        AddSankey(e.Node.Text);
                    }
                    else
                    {
                        // see if in the display list, if it is delete
                        // Also, Uncheck the owner, not all are checked
                        e.Node.Parent.Checked = false;
                        DeleteSankey(e.Node.Text);

                    }
                }
            }
        }
    }
}
