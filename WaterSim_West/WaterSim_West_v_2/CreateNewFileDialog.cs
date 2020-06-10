using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using WaterSimDCDC;
using WaterSimDCDC.WestVisual;

namespace WaterSim_West_v_1
{
    public partial class CreateNewFileDialog : Form
    {
        WaterSimManagerClass FWaterSim = null;
        string FDatabase = "";
        public CreateNewFileDialog()
        {
            InitializeComponent();

        }

        public CreateNewFileDialog(WaterSimManagerClass WSim, string Database)
        {
            InitializeComponent();
            MyWaterSimManager = WSim;

        }
        public WaterSimManagerClass MyWaterSimManager
        {
            get { return FWaterSim; }
            set
            {
                FWaterSim = value;
                parameterTreeView1.ParameterManager = FWaterSim.ParamManager;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a new File by looking for checked parameters
        ///             Event handler. Called by buttonBuildFile for click events. </summary>
        ///
        /// <remarks>   8/14/2018. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ///-------------------------------------------------------------------------------------------------

        private void buttonBuildFile_Click(object sender, EventArgs e)
        {
            // set up list of fieldnames
            List<string> FieldNames = new List<string>();
            // get the items  checked on the ParameterTreeView
            List<ParmTreeNode> TheNodes = parameterTreeView1.CheckedItems;
            // create a list of these
            foreach (ParmTreeNode PTN in TheNodes)
            {
                string Fieldname = PTN.ParmItem.Fieldname;
                FieldNames.Add(Fieldname);
            }
            if (FieldNames.Count > 0)
            {
                // get the required fields
                string fldlist = WaterSimManager_DB.RequiredFields[0];
                for (int i = 1; i < WaterSimManager_DB.RequiredFields.Length; i++)
                {
                    fldlist += " , " + WaterSimManager_DB.RequiredFields[i];
                }
                // add the field names
                foreach (string str in FieldNames)
                {
                    fldlist += " , " + str;
                }
                // now getr the file name
                saveFileDialog1.Filter = "csv files|*.csv|text files|*.txt";
                saveFileDialog1.CheckFileExists = false;
                saveFileDialog1.CheckPathExists = true;
                saveFileDialog1.DefaultExt = "csv";
                saveFileDialog1.InitialDirectory = FDatabase;
                saveFileDialog1.OverwritePrompt = true;

                saveFileDialog1.ShowDialog();
                string TargetFilename = saveFileDialog1.FileName;
                using (StreamWriter SW = new StreamWriter(TargetFilename))
                {
                    SW.WriteLine(fldlist);

                }

            }

        }

    }
}
