using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaterSimDCDC.Generic;
using ConsumerResourceModelFramework;

namespace WaterSim_West_v_1
{
    public partial class WaterSimSankeyForm : Form
    {
        string FName = "";
        CRF_Unit_Network FNetwork = null;
        bool Animate = false;
        int EndYear = 0;
        WaterSimCRFModel MyModel = null;
        public WaterSimSankeyForm()
        {
            InitializeComponent();
        }

        public WaterSimSankeyForm(WaterSimCRFModel TheModel)
        {
            InitializeComponent();
            MyModel = TheModel;
            FNetwork = MyModel.TheCRFNetwork;
            sankeyGraph1.Network = FNetwork;
            TheModel.OnRunHandler = RunModelCallBack;
            EndYear = MyModel.endYear;
            animateModelRunToolStripMenuItem.Checked = Animate;
            RedrawSankey();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the network. </summary>
        ///
        /// <value> The network. </value>

        public CRF_Unit_Network Network
        {
            get
            {
                return FNetwork;
            }
            set
            {
                FNetwork = value;
                SetNetwork();

            }
        }

        public void RunModelCallBack(int year)
        {
            // check if this is a rest
            // 
            if (year < 0)
            {
                RedrawSankey();

            }
            else
            {
                if (!Animate)
                {
                    if (year >= MyModel.endYear)
                    {
                        RedrawSankey();
                        //                    sankeyGraph1.Refresh();
                    }
                }
                else
                {
                    RedrawSankey();
                    //sankeyGraph1.Refresh();
                }
            }
        }
    
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the name of the network. </summary>
        ///
        /// <value> The name of the network. </value>

        public string NetworkName
        {
            get { return FName; }
        }

        public void RedrawSankey()
        {
            sankeyGraph1.ResetGraph();
            NetworkNameLabel.Text = FNetwork.Name;
            ShowNetworkData();
        }

        private void ShowNetworkData()
        {
            List<string> AllDataList = new List<string>();

            AllDataList.Add("Resources");
            foreach(CRF_Resource CRFR in FNetwork.Resources)
            {
                string limitstr = "  " + CRFR.Name + ":\t" + CRFR.Limit.ToString("N") + "\t" + CRFR.Allocated.ToString("N")+"\t"+CRFR.Net.ToString("N");
                AllDataList.Add(limitstr);
            }
            AllDataList.Add("Consumers");
            foreach (CRF_Consumer CRFC in FNetwork.Consumers)
            {
                string limitstr = "  " + CRFC.Name + ":\t" + CRFC.Demand.ToString("N") + "\t" + CRFC.ResourcesAllocated.ToString("N") + "\t" + CRFC.Net.ToString("N");
                AllDataList.Add(limitstr);
            }

            AllDataList.Add("  Fluxes");
            foreach (CRF_Resource CRFR in FNetwork.Resources)
            {
                string limitstr = "    " + CRFR.Name;
                AllDataList.Add(limitstr);
 
                foreach (CRF_Flux CRFF in CRFR.ToFluxs)
                {
                    limitstr = "    - " + CRFF.Target.Name + ":\t" + CRFF.Allocated().ToString("N");
                    AllDataList.Add(limitstr);
                }
            }

            listBoxData.Items.Clear();
            foreach (string str in AllDataList)
            {
                listBoxData.Items.Add(str);
            }
        }
        private void SetNetwork()
        {
           sankeyGraph1.Network = FNetwork;
           NetworkNameLabel.Text = FNetwork.Name;
           ShowNetworkData();
            
        }

        private void sankeyGraph1_Load(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void animateModelRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Animate = !Animate;
            animateModelRunToolStripMenuItem.Checked = Animate;
        }
    }
}
