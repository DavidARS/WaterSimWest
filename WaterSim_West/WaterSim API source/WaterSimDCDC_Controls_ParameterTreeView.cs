using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WaterSimDCDC;
using System.Reflection;
using System.Runtime.InteropServices;



namespace WaterSimDCDC.WestVisual

{
//    public partial class ParameterTreeView : UserControl
    //public class ParameterTreeView : UserControl
     public partial class ParameterTreeView : UserControl
    {
        public delegate void ParmItemEventHandler(object sender, ParmTreeNode Node);

        public event ParmItemEventHandler ParmItemCheck;
        public event ParmItemEventHandler ParmItemSelect;
        
        ParameterManagerClass FParamManager;
        List<ModelParamItem> FParamItems = new List<ModelParamItem>();
        List<ModelParameterGroupClass> FAllGroups = new List<ModelParameterGroupClass>();
        List<ParmTreeNode> TheParmNodes = new List<ParmTreeNode>();
        eShowFieldName FShowFieldnames = eShowFieldName.sfHide;

        bool FAllowGroupCheck = true;

        // if you change the size of panelTreeViewKey then this factor miust match the design height.
        int FMinKeyHeight = 150;
        public ParameterTreeView()
        {
            InitializeComponent();
            treeViewParameters.ImageList = imageListTreeNodes;
            treeViewParameters.ShowNodeToolTips = true;
            //FMinKeyHeight = panelTreeViewKey.Height;
        }

        public ParameterTreeView(ParameterManagerClass ParamManager)
        {
            InitializeComponent();
            treeViewParameters.ImageList = imageListTreeNodes;
            treeViewParameters.ShowNodeToolTips = true;
            ///FMinKeyHeight = panelTreeViewKey.Height;

            FParamManager = ParamManager;
            InitializeComponent();
            BuildParmList();
            BuildTree();
        }

        public ParameterManagerClass ParameterManager
        {
            get { return FParamManager; }
            set
            {
                treeViewParameters.Nodes.Clear();
                FParamManager = value;
                if (FParamManager != null)
                {
                    BuildParmList();
                    BuildTree();
                }
            }
        }
        //-------------------------------------------------------------------------------------
        internal ParmTreeNode _FindFieldname(TreeNodeCollection theNodes, string Fieldname)
        {
            ParmTreeNode temp = null;
            foreach (TreeNode node in theNodes)
            {
                if ((node as ParmTreeNode).isParamItemNode)
                {
                    if ((node as ParmTreeNode).ParmItem.Fieldname.CompareTo(Fieldname) == 0)
                    {
                        temp = (node as ParmTreeNode);
                        break;
                    }
                }
                if (node.Nodes.Count > 0)
                {
                    temp = _FindFieldname(node.Nodes, Fieldname);
                    if (temp != null) { break; }
                }
            }
            
            return temp;
        }

        //-------------------------------------------------------------------------------------
        public ParmTreeNode FindFieldname(string Fieldname)
        {
            ParmTreeNode temp = null;
            temp = _FindFieldname(treeViewParameters.Nodes, Fieldname);
            return temp;
        }
        //-------------------------------------------------------------------------------------
        public void SetFieldsActive(List<string> theFields)
        {
            foreach (string fldstr in theFields)
            {
                ParmTreeNode PTN = FindFieldname(fldstr);
                if (PTN != null)
                {
                    PTN.Enabled = true; ;
                }
            }
        }

        public bool AllowGroupCheck
        {
            get { return FAllowGroupCheck; }
            set { FAllowGroupCheck = value; }
        }
        // Invoke the ParmItemCheck

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the parm item check action. </summary>
        ///
        /// <param name="Node"> The node. </param>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void OnParmItemCheck(ParmTreeNode Node)
        {
         //public delegate void ParmItemCheckEventHandler(object sender, ParmTreeNode Node);
         //  ItemCheckEventArgs e = new ItemCheckEventArgs();

            if (Node != null)
            {
                if (ParmItemCheck != null)
                {
                    ParmItemCheck(this, Node);
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the parm item select action. </summary>
        ///
        /// <param name="Node"> The node. </param>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void OnParmItemSelect(ParmTreeNode Node)
        {
            if (Node != null)
            {
                if (ParmItemSelect != null)
                {
                    ParmItemSelect(this, Node);
                }
            }
        }

        //-------------------------------------------------------------------------------------
        internal void _Clear(TreeNodeCollection theNodes)
        {
            foreach (TreeNode node in theNodes)
            {
                (node as ParmTreeNode).Enabled = false;
                if (node.Nodes.Count > 0)
                {
                    _Clear(node.Nodes);
                }
            }
        }
        //----------------------------------------------------------------
        public void Clear()
        {
            treeViewParameters.BeginUpdate();
            _Clear(treeViewParameters.Nodes);
            treeViewParameters.EndUpdate();
        }
        //----------------------------------------------------------------
        internal void BuildParmList()
        {
            if (FParamManager != null)
            {
                foreach (ModelParameterClass MP in FParamManager.AllModelParameters())
                {
                    ModelParamItem MPI = new ModelParamItem(MP);
                    FParamItems.Add(MPI);
                }
            }
        }

        internal void BuildTree()
        {
            ModelParameterGroupClass AllParmsGroup = new ModelParameterGroupClass("All Parameters");
            ParmTreeNode AllParms = new ParmTreeNode(AllParmsGroup, FParamManager);

            foreach (ModelParamItem MPI in FParamItems)
            {

               // get groups 
               if (MPI.TopicsGroup.Count > 0)
                {

                    foreach (ModelParameterGroupClass MPG in MPI.TopicsGroup)
                    {
                        if (FAllGroups.Find(delegate(ModelParameterGroupClass item) { 
                            return item.ID == MPG.ID; 
                        }) == null)
                        {
                            FAllGroups.Add(MPG);
                            ParmTreeNode temp = new ParmTreeNode(MPG, FParamManager);
                            TheParmNodes.Add(temp);
                        }
                    }

                }

                AllParms.Nodes.Add(new ParmTreeNode(MPI,FParamManager));
            }

            foreach(ParmTreeNode Node in TheParmNodes)
            {
                treeViewParameters.Nodes.Add(Node);

            
            }

            treeViewParameters.Nodes.Add(AllParms);
        }

        public ParmTreeNode Selected()
        {
            return (treeViewParameters.SelectedNode as ParmTreeNode);
        }

        void _ResetFieldNames(TreeNode Node, eShowFieldName ShowMode)
        {
            (Node as ParmTreeNode).ShowFieldName = ShowMode;
            if (Node.Nodes.Count>0)
            {
                foreach(TreeNode NextNode in Node.Nodes)
                {
                    _ResetFieldNames(NextNode,ShowMode);
                }
            }
        }
        void ResetFieldNames()
        {
            if (treeViewParameters.Nodes.Count>0)
            {
                treeViewParameters.LabelEdit = true;
                treeViewParameters.BeginUpdate();
                foreach(TreeNode Node in treeViewParameters.Nodes)
                {
                    _ResetFieldNames(Node,FShowFieldnames);
                }
                treeViewParameters.EndUpdate();
                treeViewParameters.LabelEdit = false;

            }

        }

        public eShowFieldName ShowFieldNames
        {
            get {return FShowFieldnames; }
            set
            {
                if (FShowFieldnames!=value)
                {
                    FShowFieldnames = value;
                    ResetFieldNames();
                }
            }
        }
        //-----------------------------------------------------------------------------

        public void FieldSet(string fieldname)
        {

        }
        //-----------------------------------------------------------------------------
        public bool UseCheckBoxes
        {
            get { return treeViewParameters.CheckBoxes; }
            set { treeViewParameters.CheckBoxes = value; }
        }

        //-----------------------------------------------------------------------------
        internal void FindCheckedNodes(TreeNodeCollection Nodes, ref List<ParmTreeNode> theList)
        {
            foreach (TreeNode node in Nodes)
            {
                if ((node as ParmTreeNode).isParamItemNode)
                {
                    if (node.Checked) { theList.Add((node as ParmTreeNode)); }
                }
                if (node.Nodes.Count > 0) { FindCheckedNodes(node.Nodes, ref theList); }
             }
        }
        //-----------------------------------------------------------------------------
        public List<ParmTreeNode> CheckedItems
        {
            get 
            {
                List<ParmTreeNode> temp = new List<ParmTreeNode>();
                FindCheckedNodes(treeViewParameters.Nodes, ref temp);
                return temp;
            }
        }
        //-----------------------------------------------------------------------------
        internal void SwapParmNodes(TreeNodeCollection theNodes, int index1, int index2)
        {
            ParmTreeNode temp1 = (theNodes[index1] as ParmTreeNode);
            ParmTreeNode temp2 = (theNodes[index2] as ParmTreeNode);
            theNodes.Remove(temp1);
            theNodes.Remove(temp2);
            theNodes.Insert(index1, temp2);
            theNodes.Insert(index2, temp1);

            //theNodes[index1] = theNodes[index2];
            //theNodes[index2] = temp;
        }

        //-----------------------------------------------------------------------------
        internal void _SortNodes(TreeNodeCollection Nodes, eSortParmItem SortBy)
        {

            int N = Nodes.Count;
            if (N>0)
            {
                int index_of_min = 0;
                for (int iterator = 0; iterator < N - 1; iterator++)
                {
                        index_of_min = iterator;
                        for (int index = iterator + 1; index < N; index++)
                        {
                                int test = 0;
                                if (SortBy==eSortParmItem.spiFieldname)
                                {
                                    test = ParmTreeNode.CompareBasedOnField((Nodes[index] as ParmTreeNode),(Nodes[index_of_min] as ParmTreeNode));
                                }
                                else
                                {
                                    test = ParmTreeNode.CompareBasedOnLabel((Nodes[index] as ParmTreeNode), (Nodes[index_of_min] as ParmTreeNode));
                                }
                                if (test < 0)
                                {
                                    index_of_min = index;
                                }
                        }
                        if (iterator != index_of_min)
                        {
                            SwapParmNodes(Nodes, iterator, index_of_min);
                        }
                }

            }

            foreach (TreeNode node in Nodes)
            {
                
                if (node.Nodes.Count > 0)
                {
                    _SortNodes(node.Nodes,SortBy);
                }
            }
            
        }
        //-----------------------------------------------------------------------------
        internal void SortNodes(eSortParmItem SortBy)
        {
            treeViewParameters.BeginUpdate();
            _SortNodes(treeViewParameters.Nodes, SortBy);
            treeViewParameters.EndUpdate();
            treeViewParameters.Invalidate();
        }
        //-----------------------------------------------------------------------------
        internal void HideKey(bool HideTheKey)
        {
            if (HideTheKey)
            {
                panelTreeViewKey.Visible = false;
                treeViewParameters.Height = Height - (statusStripTreeView.Height);
            }
            else
            {
                //treeViewParameters.Height = Height - (statusStripTreeView.Height + FMinKeyHeight);
                treeViewParameters.Height = Height - (statusStripTreeView.Height + panelTreeViewKey.Height);
                panelTreeViewKey.Visible = true;
            }
        }
        //-----------------------------------------------------------------------------
        bool FHideKey = false;

        private void hideKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FHideKey = !FHideKey;
            if (FHideKey) { hideKeyToolStripMenuItem.Text = "Show Key"; }
            else { hideKeyToolStripMenuItem.Text = "Hide Key"; }
            HideKey(FHideKey);
        }
        //-----------------------------------------------------------------------------

        bool SettingShowField = false;
        //-----------------------------------------------------------------------------

        private void showFirstToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (!SettingShowField)
            {
                SettingShowField = true;
                showLastToolStripMenuItem.Checked = false;
                //showFirstToolStripMenuItem.Checked = false;
                hideKeyToolStripMenuItem.Checked = false;
                ShowFieldNames = eShowFieldName.sfFirst;
                SettingShowField = false;
            }
        }
        //-----------------------------------------------------------------------------

        private void showLastToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (!SettingShowField)
            {
                SettingShowField = true;
                //showLastToolStripMenuItem.Checked = false;
                showFirstToolStripMenuItem.Checked = false;
                hideKeyToolStripMenuItem.Checked = false;
                ShowFieldNames = eShowFieldName.sfLast;
                SettingShowField = false;
            }
        }
        //-----------------------------------------------------------------------------

        private void hideToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
                        if (!SettingShowField)
            {
                SettingShowField = true;
                showLastToolStripMenuItem.Checked = false;
                showFirstToolStripMenuItem.Checked = false;
                //hideKeyToolStripMenuItem.Checked = false;
                ShowFieldNames = eShowFieldName.sfHide;
                SettingShowField = false;
            }

        }
        //-----------------------------------------------------------------------------

        private void byLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SortNodes(eSortParmItem.spiLabel);
        }
        //-----------------------------------------------------------------------------

        private void byFieldnameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SortNodes(eSortParmItem.spiFieldname);
        }


        List<string> Shit = new List<string>();
        internal void AddNodeStatus(string Stage, TreeNode Node, string evt)
        {
            string state = "";
            string able = "";
            if (Node != null)
            {
                if (Node.Checked) { state = "Ccecked"; } else { state = "not CHecked"; }
                if ((Node as ParmTreeNode).Enabled) { able = " Enabled"; } else { able = " Disabled"; }
            }
            Shit.Add(Stage + ": " + state + " , " + able+ " action:"+evt);
        }
        //--------------------------------------------------------------------
        private void treeViewParameters_AfterCheck(object sender, TreeViewEventArgs e)
        {
           // AddNodeStatus("> AfterCheck", e.Node, e.Action.ToString());
            if ((e.Node as ParmTreeNode).isGroupNode)
            {
                if (e.Action != TreeViewAction.Unknown)
                {
                    if (e.Node.Nodes.Count > 0)
                    {
                        foreach (TreeNode node in e.Node.Nodes)
                        {
                            if ((node as ParmTreeNode).isParamItemNode)
                            {
                                node.Checked = e.Node.Checked;
                            }
                        }
                    }
                }

            }
            if (e.Node.Checked)
            {
            OnParmItemCheck((e.Node as ParmTreeNode));
                }
           
         //   AddNodeStatus("< AfterCheck", e.Node, e.Action.ToString());
        }

               //--------------------------------------------------------------------
      
        private void treeViewParameters_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if ((e.Node as ParmTreeNode).isParamItemNode)
            {
                string temp = "";
                if ((e.Node as ParmTreeNode).Enabled)
                {
                    string minstr = "0";
                    if ((e.Node as ParmTreeNode).ParmItem.Min > 0) { minstr = (e.Node as ParmTreeNode).ParmItem.Min.ToString("##,#"); }
                    temp = (e.Node as ParmTreeNode).ParmItem.UnitsLong + "  Min: " + minstr + "  Max: " + (e.Node as ParmTreeNode).ParmItem.Max.ToString("##,#");
                }
                else
                {
                    temp = "Not Available";
                }
                ItemStatusLabel.Text = temp;
            }

            else
            {
                ItemStatusLabel.Text = "";
            }
        }

        private void treeViewParameters_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
  //          AddNodeStatus("<> NodeDoubleClick", e.Node, e.ToString());

        }

        private void treeViewParameters_MouseDoubleClick(object sender, MouseEventArgs e)
        {
 //           AddNodeStatus("<> MouseDoubleClick", null, e.ToString());

        }

        private void treeViewParameters_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            // cancel event if node disabled
            if (!(e.Node as ParmTreeNode).Enabled)
            {
                e.Cancel = true;
            }
            else
            {
               // cancel event if group and allowGroupCheck is false;
                if (((e.Node as ParmTreeNode).isGroupNode) && (!FAllowGroupCheck))
                {
                    e.Cancel = true;
                }
            }
           
        }

        private void treeViewParameters_MouseClick(object sender, MouseEventArgs e)
        {
 //           AddNodeStatus("<> MouseClick", null, e.ToString());
        }

        private void treeViewParameters_DoubleClick(object sender, EventArgs e)
        {
 //           AddNodeStatus("<> DoubleClick", null, e.ToString());

        }

        private void treeViewParameters_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
 //           AddNodeStatus("<> NodeClick", e.Node, e.ToString());

        }

        private void treeViewParameters_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (ParmItemSelect != null)
            {
                OnParmItemSelect((e.Node as ParmTreeNode));
            }
        }

        private void ParameterTreeView_Resize(object sender, EventArgs e)
        {
            if (FHideKey)
            {
                treeViewParameters.Height = Height - statusStripTreeView.Height; 
            }
            else
            {
                //treeViewParameters.Height = Height - (FMinKeyHeight + statusStripTreeView.Height);
                //panelTreeViewKey.Top =( Height - FMinKeyHeight)+2;
                treeViewParameters.Height = Height - (panelTreeViewKey.Height + statusStripTreeView.Height);
                panelTreeViewKey.Top = (Height - (panelTreeViewKey.Height+ statusStripTreeView.Height)) + 2;


            }
            //int FullHeight = Height;

            ////// Do Tree View Parameters
            ////treeViewParameters.Height = Height - FMinKeySize;
            ////treeViewParameters.Top = 0;
            ////panelTreeViewKey.Height = FMinKeySize;
            //panelTreeViewKey.Top = Height - FMinKeyHeight;
        }
    }

    //------------------------------------------------------------

    public delegate int CompareParmNodes(ParmTreeNode Node1, ParmTreeNode Node2);
    //------------------------------------------------------------
    public enum eSortParmItem { spiLabel, spiFieldname };
    public enum eShowFieldName  {sfHide, sfFirst, sfLast};
    //------------------------------------------------------------

    public class ParmTreeNode : TreeNode
    {
        bool FEnabled = true;
        ParameterManagerClass FPM;
        ModelParameterGroupClass ThisGroup = null;
        ModelParamItem ThisParamItem = null;
        eShowFieldName FShowField = eShowFieldName.sfHide;
        
//      List<ParmNode> Children = new List<ParmNode>();
//      List<ModelParamItem> ParmList = new List<ModelParamItem>();

        public ParmTreeNode(string aName, ParameterManagerClass aPM) : base (aName)
        {
            FPM = aPM;
        }

        //------------------------------------------------------------

        public ParmTreeNode(ModelParameterGroupClass aGroup,ParameterManagerClass aPM ) : base(aGroup.Name)
        {
            FPM = aPM;
            ThisGroup = aGroup;
            
            SelectedImageIndex = ImageIndex = 4;
            foreach (ModelParameterGroupClass groupitem in aGroup.Groups())
            {
                Add(groupitem);
            }
            foreach (int parmcode in aGroup.ModelParameters())
            {
                    Add(parmcode);

            }

        }
        //------------------------------------------------------------

        public ParmTreeNode(int ParmCode, ParameterManagerClass aPM)
        {
            FPM = aPM;
            try
                {
                    ModelParameterClass MP = aPM.Model_Parameter(ParmCode);
                    ThisParamItem = new ModelParamItem(MP);
                    SetUpItem(ThisParamItem);
                }
             catch
                {
                    ModelParamItem temp = new ModelParamItem();
                    temp.SetUndefined(ParmCode);
                    ThisParamItem = temp;
                    Name = ThisParamItem.Fieldname;
                    Text = ThisParamItem.Label;
                    ImageIndex = SelectedImageIndex = 6;
                }
        }

        public ParmTreeNode(ModelParamItem MPI, ParameterManagerClass aPM)
        {
            FPM = aPM;
            ThisParamItem = MPI;
            SetUpItem(MPI);
        }

        internal void SetUpItem (ModelParamItem MPI)
        {
                ThisParamItem = MPI;
                Name = ThisParamItem.Fieldname;
                Text = ThisParamItem.Label;
                ToolTipText = ThisParamItem.Description;
                switch (ThisParamItem.ParameterType)
                {
                    case modelParamtype.mptOutputProvider:
                        ImageIndex = SelectedImageIndex = 3;
                        break;
                    case modelParamtype.mptOutputBase:
                        ImageIndex = SelectedImageIndex = 2;
                        break;
                    case modelParamtype.mptInputProvider:
                        ImageIndex = SelectedImageIndex = 1;
                        break;
                    case modelParamtype.mptInputBase:
                        ImageIndex = SelectedImageIndex = 0;
                        break;
                    default:
                        break;
                }
        }
        //------------------------------------------------------------
        public bool Enabled
        {
            get { return FEnabled; }
            set
            {
                if (FEnabled != value)
                {
                    if (value)
                    {
                        SetNodeAsActve();
                    }
                    else
                    {
                        SetNodeAsInActve();
                    }
                }
                FEnabled = value;
            }
        }
        //------------------------------------------------------------
        public bool anyNodesEnabled
        {
            get
            {
                bool temp = false;
                foreach (TreeNode node in Nodes)
                {
                    if ((node as ParmTreeNode).Enabled)
                    {
                        temp = true;
                        break;
                    }
                }
                return temp;
            }
        }
        //----------------------------------------------------------------
        internal void SetNodeAsInActve()
        {
            if (isParamItemNode)
            {
                ForeColor = Color.LightGray;
                switch (ParmItem.ParameterType)
                {
                    case modelParamtype.mptInputProvider:
                        ImageIndex = SelectedImageIndex = 8;
                        break;
                    case modelParamtype.mptInputBase:
                        ImageIndex = SelectedImageIndex = 7;
                        break;
                    case modelParamtype.mptOutputProvider:
                        ImageIndex = SelectedImageIndex = 10;
                        break;
                    case modelParamtype.mptOutputBase:
                        ImageIndex = SelectedImageIndex = 9;
                        break;
                    default:
                        ImageIndex = SelectedImageIndex = 13;
                        break;
                }
                // of now do a check for parent, if group then check of any nodes are enabled, if not set parent enable to false;
                // essentially we are the straw that broke the camels back
                if (Parent != null)
                {
                    if (Parent is ParmTreeNode)
                    {
                        if ((Parent as ParmTreeNode).isGroupNode)
                        {
                            if ((Parent as ParmTreeNode).anyNodesEnabled == false)
                            {
                                (Parent as ParmTreeNode).Enabled = false;
                            }
                        }
                    }
                }
            }
            else
            {
                // OK this is group node, check if any are enabled, if not, set enable to false;
                if (!anyNodesEnabled)
                {
                    FEnabled = false;
                    ForeColor = Color.LightGray;
                    ImageIndex = SelectedImageIndex = 11;
                }
            }
        }
        //----------------------------------------------------------------
        internal void SetNodeAsActve()
        {
            if (isParamItemNode)
            {
                ForeColor = Color.Black;
                switch (ParmItem.ParameterType)
                {
                    case modelParamtype.mptInputProvider:
                        ImageIndex = SelectedImageIndex = 1;
                        break;
                    case modelParamtype.mptInputBase:
                        ImageIndex = SelectedImageIndex = 0;
                        break;
                    case modelParamtype.mptOutputProvider:
                        ImageIndex = SelectedImageIndex = 3;
                        break;
                    case modelParamtype.mptOutputBase:
                        ImageIndex = SelectedImageIndex = 2;
                        break;
                    default:
                        ImageIndex = SelectedImageIndex = 6;
                        break;
                }
                // ok this nodes is enabled so if parent is group, it shlod be enabled.
                if (Parent != null)
                {
                    if (Parent is ParmTreeNode)
                        if ((Parent as ParmTreeNode).isGroupNode)
                        {
                            {
                                (Parent as ParmTreeNode).Enabled = true;
                            }
                        }
                }
            }
            else
                // This is a group node
            {
                // check to see if any nodes are enabled, if 
                if (anyNodesEnabled)
                {
                    ForeColor = Color.DarkBlue;
                    ImageIndex = SelectedImageIndex = 4;
                    FEnabled = true;
                }
            }
        }
        //------------------------------------------------------------
        public ModelParamItem ParmItem 
        {
            get {return ThisParamItem; }
        }
        //------------------------------------------------------------

        public ModelParameterGroupClass ParmGroup
        {
            get { return ThisGroup; }
        }
        //------------------------------------------------------------

        public eShowFieldName ShowFieldName
        {
            get { return FShowField; }
            set {
                    if (FShowField!=value)
                    {
                        FShowField = value;
                        ResetText();
                    }
                }
        }
        //------------------------------------------------------------

        public static int CompareBasedOnLabel(ParmTreeNode Node1, ParmTreeNode Node2)
        {
            // OK, Groupnames are never sorted, always equal
            if ((Node1.isGroupNode) || (Node2.isGroupNode))
            {
                return 0;
            }
            else
            {
                return Node1.ParmItem.Label.CompareTo(Node2.ParmItem.Label);
            }
        }
        //------------------------------------------------------------
        public static int CompareBasedOnField(ParmTreeNode Node1, ParmTreeNode Node2)
        {
            // OK, Groupnames are never sorted, always equal
            if ((Node1.isGroupNode) || (Node2.isGroupNode))
            {
                return 0;
            }
            else
            {
                return Node1.ParmItem.Fieldname.CompareTo(Node2.ParmItem.Fieldname);
            }
        }
        //------------------------------------------------------------

        string BuildText()
        {
            string temp = "";
            
            if (isParamItemNode)
            {
                string Base = ThisParamItem.Label;
                switch (FShowField)
                {
                    case eShowFieldName.sfHide:
                        temp =Base;
                        break;
                    case eShowFieldName.sfFirst:
                        temp = ThisParamItem.Fieldname+" : "+Base;
                        break;
                    case eShowFieldName.sfLast:
                        temp = Base + " : " + ThisParamItem.Fieldname;
                        break;
                }
            }
            else
            {
                temp = ThisGroup.Name;
            }
            
            return temp;
        }

        void ResetText()
        {
            BeginEdit();
            Text = BuildText();
            EndEdit(false);
        }
        void Add(ModelParameterGroupClass aGroup)
        {
            // Create new node
            ParmTreeNode Temp = new ParmTreeNode(aGroup, FPM);
            Nodes.Add(Temp);
        }

        void Add(int ParamCode)
        {
            // Create new node
            ParmTreeNode Temp = new ParmTreeNode(ParamCode, FPM);
            Nodes.Add(Temp);
        }

        public int Levels()
        {
            return LastNode.Index;
        }

        public bool isGroupNode
        {
            get {return (ThisGroup!=null);}
        }

        public bool isParamItemNode
        {
            get { return (ThisParamItem != null); }
        }


        public bool isInTree(ModelParameterGroupClass Target)
        {
            bool found = false;
            if (isGroupNode)
            {
                if (ThisGroup.ID == Target.ID) { found = true; }
            }
            else
            {
                foreach (TreeNode node in Nodes)
                {
                    if (node is ParmTreeNode)
                    {
                        if ((node as ParmTreeNode).isInTree(Target))
                        {
                            found = true;
                            break;
                        }
                    }
                }
            }
            return found;
        }

        public bool isInTree(int paramcode)
        {
            bool found = false;
            if (isParamItemNode)
            {
                if ( ThisParamItem.ModelParam==paramcode) { found = true; }
            }
            else
            {
                foreach (TreeNode node in Nodes)
                {
                    if (node is ParmTreeNode)
                    {
                        if ((node as ParmTreeNode).isInTree(paramcode))
                        {
                            found = true;
                            break;
                        }
                    }
                }
            }
            return found;
        }

        private const int TVIF_STATE = 0x8;
        private const int TVIS_STATEIMAGEMASK = 0xF000;
        private const int TV_FIRST = 0x1100;
        private const int TVM_SETITEM = TV_FIRST + 63;

        [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]
        private struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam,
                                                 ref TVITEM lParam);

        /// <summary>
        /// Hides the checkbox for the specified node on a TreeView control.
        /// </summary>
        private void HideCheckBox(TreeView tvw, TreeNode node)
        {
            TVITEM tvi = new TVITEM();
            tvi.hItem = node.Handle;
            tvi.mask = TVIF_STATE;
            tvi.stateMask = TVIS_STATEIMAGEMASK;
            tvi.state = 0;
            SendMessage(tvw.Handle, TVM_SETITEM, IntPtr.Zero, ref tvi);
        }
    }

    //--------------------------------------------------------------
    public class ModelParamItem
    {
        int FModelParam = -1;
        string FModelParamName = "";
        string FFieldname = "";
        string FLabel = "";
        string FDescrip = "";
        string FUnitsShort = "";
        string FUnitsLong = "";
        string FWebLabel = "";
        int FMax = 0;
        int FMin = 0;
        rangeChecktype FRangeType = rangeChecktype.rctUnknown;
        ModelParameterGroupClass FDepends = null;
        List<ModelParameterGroupClass> FTopics = new List<ModelParameterGroupClass>();
        int Default = 0;
        modelParamtype FParamType = modelParamtype.mptUnknown;
        eProviderAggregateMode FProvAgMode = eProviderAggregateMode.agNone;
        string FProviderPropertyName = "";
        string FgetintName = "";
        string FgetintarrayName = "";
        string FsetintName = "";
        string FsetintarrayName = "";
        string FspecialBaseRangeCheckName = "";
        string FspecialProviderRangeCheckName = "";
        string FreloadEventName = "";

        /// <summary>   Default constructor. </summary>
        public ModelParamItem() 
        { 
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="TheModelParam">    the model parameter to create item from. </param>
        ///-------------------------------------------------------------------------------------------------

        public ModelParamItem(ModelParameterClass TheModelParam)
        {
            FModelParam = TheModelParam.ModelParam;
            FModelParamName = eModelParam.Names(FModelParam);
            FFieldname = TheModelParam.Fieldname; ;
            FLabel = TheModelParam.Label;

            if (TheModelParam.isExtended)
            {
                FDescrip = TheModelParam.Description;
                FUnitsShort = TheModelParam.Units;
                FUnitsLong = TheModelParam.UnitsLong;
                FWebLabel = TheModelParam.WebLabel;
                if (TheModelParam.TopicGroups != null)
                {
                    if (TheModelParam.TopicGroups.Count > 0)
                    {
                        foreach (ModelParameterGroupClass MPG in TheModelParam.TopicGroups)
                        {
                            FTopics.Add(MPG);
                        }
                    }
                }
            }
            FMax = TheModelParam.HighRange;
            FMin = TheModelParam.LowRange;
            FRangeType = TheModelParam.RangeCheckType;
            FDepends = TheModelParam.DerivedFrom;
            //int Default = 0;
            FParamType = TheModelParam.ParamType;
            if ((TheModelParam.isProviderParam)&&(TheModelParam.ProviderProperty != null))
            {
                FProvAgMode = TheModelParam.ProviderProperty.AggregateMode;
                FProviderPropertyName = TheModelParam.ProviderProperty.GetType().FullName;
            }
        }

        public void SetUndefined(int code)
        {
            // see of this is in documentation
            string test = eModelParam.Names(code);
            if (test != "")
            {
                FLabel = test;
            }
            else
            {
                FLabel = "UnDefined #" + code.ToString();
            }
            FModelParam = code;
            FFieldname = "??#"+code.ToString();
        }

        public int ModelParam
        {
            get { return FModelParam; }
            set { FModelParam = value; }
        }
 
        public string ModelParamName
        {
            get { return FModelParamName; }
            set { FModelParamName = value; }
        }


        public string Fieldname
        {
            get { return FFieldname; }
            set { FFieldname = value; }
        }

        public string Label
        {
            get { return FLabel; }
            set { FLabel = value; }
        }

        public string Description
        {
            get { return FDescrip; }
            set { FDescrip = value; }
        }

        public string Units
        {
            get { return FUnitsShort; }
            set { FUnitsShort = value; }
        }

        public string UnitsLong
        {
            get { return FUnitsLong; }
            set { FUnitsLong = value; }
        }

        public string WebLabel
        {
            get { return FWebLabel; }
            set { FWebLabel = value; }
        }
        public int Max
        {
            get { return FMax; }
            set { FMax = value; }
        }

        public int Min
        {
            get { return FMin; }
            set { FMin = value; }
        }

        public rangeChecktype RangeCheckType
        {
            get { return FRangeType; }
            set { FRangeType = value; }
        }

        public ModelParameterGroupClass DependencyGroup
        {
            get { return FDepends; }
            set { FDepends = value; }
        }

        public List<ModelParameterGroupClass> TopicsGroup
        {
            get { return FTopics; }
            set { FTopics = value; }
        }

        public modelParamtype ParameterType
        {
            get { return FParamType; }
            set { FParamType = value; }
        }

        public eProviderAggregateMode ProviderAggregateMode
        {
            get { return FProvAgMode; }
            set { FProvAgMode = value; }
        }

        public string ProviderPropertyName
        {
            get { return FProviderPropertyName; }
            set { FProviderPropertyName = value; }
        }
    }
}
