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
using ConsumerResourceModelFramework;
using WaterSimDCDC.Generic;
using WaterSimDCDC.Controls;
using UniDB;

namespace WaterSim_West_v_1
{

    public partial class Form1 : Form
    {
        WaterSimManager_DB MyWSIM;
        //        WaterSimManager_SIO MyWSIM;

        //WaterSimManager MyWSIM;
       

        List<Form> SankeyForms = new List<Form>();

        WaterSimCRFModel TheCRFModel = null;
        string ModelUnitName = "Arizona Central North";

        ChartManager MyCM = null;

        StreamManager MyStream = null;

        ShowMultipleSankeyV1 MyMultiSankey = null;

        // The Assessment phrases and Colors
        SortedList<int, string> FBalancePhrase = new SortedList<int, string>();
        SortedList<int, string> FIndicatorsPhrase = new SortedList<int, string>();
        SortedList<int, string> FAssessmentPhrase = new SortedList<int, string>();

        SortedList<int, Color> FIndicatorColors = new SortedList<int, Color>();

        string TheFileNameForData = "";
        const string TheDefaultScenarioName = "Scenario_";
        string ActiveScenbarioName = TheDefaultScenarioName;
        //
        DateTime now = DateTime.Now;
        //
        string path = "C:\\Users\\dasamps1\\Source\\Repos\\WaterSimWest\\WaterSim West API 2020\\WaterSim_West\\WaterSim_West_v_2\bin\\Debug\\LCLU.txt";

        public Form1()
        {
            InitializeComponent();
           
             //Utilities_Stream.SimpleWrite SW = new Utilities_Stream.SimpleWrite();
            // MyWSIM = new WaterSimManager_DB(".", ".");
            SQLServer ServerType = SQLServer.stText;
            string TheDefaultDatabase = Path.GetDirectoryName(Application.ExecutablePath);
            //MyWSIM = new WaterSimManager_DB(ServerType, ".", ".", TheDefaultDatabase, "", "", "","");
            MyWSIM = new WaterSimManager_DB(ServerType, TheDefaultDatabase, TheDefaultDatabase, TheDefaultDatabase, "", "", "", "");
            MyWSIM.tempLCLU(MyWSIM);
            //     
            //MyWSIM = new WaterSimManager_SIO(".", ".");

            
            //sw.WriteLine(MyWSIM.WaterSimWestModel.TotalDemand);

            //UnitData TheData = null;
            //TheData = MyWSIM.TheCRFNetwork.CRFUnitData;

            // This add some groups for the west data
            WaterSimDCDC.WestTools.AddWestParameterGroups(MyWSIM);

            SetupPhrasesAndColors();

            foreach (string name in MyWSIM.WaterSimWestModel.UnitModelNames)
            {
                DisplaySankeyMenuItem.DropDownItems.Add(name);
                SanKeyGraphcomboBox.Items.Add(name);
            }

            parameterTreeView1.ParameterManager = MyWSIM.ParamManager;

            // Set al;l Industry to purple, default is red
            foreach (WaterSimCRFModel WSM in MyWSIM.WaterSimWestModel.WaterSimCRFModels)
            {
                WSM.TheCRFNetwork.Industrial.Color = Color.Purple;
            }
            // Reset Size of Sankey Graph
            sankeyGraphUnit.Width = SankeyGraphPanel.Width - 2;
            sankeyGraphUnit.Invalidate();
            Application.DoEvents();
            // Ok, reset the graph
            ResetSanKeyGraphUnit(ModelUnitName);
            // Create the chart manager
            MyCM = new ChartManager(chart1, "MyCHart");
            MyStream = new StreamManager("Demand");
            // Setup Input Tree
            treeViewInput.CheckBoxes = true;
            foreach (int emp in MyWSIM.ParamManager.eModelParameters())
            {
                ModelParameterClass MP = MyWSIM.ParamManager.Model_Parameter(emp);
                if (MP.isInputParam)
                {
                    treeViewInput.Nodes.Add(new ParameterTreeNode(MP));
                }
            }
            // Setup MultiSankey Window
            MyMultiSankey = new ShowMultipleSankeyV1(MyWSIM);
            MyMultiSankey.Hide();
            //regionTreeViewClass1.WaterSim = MyWSIM;
            regionTreeViewClass1.WaterSim = MyWSIM;
            regionTreeViewClass1.CallBackHandler = RegionTreeViewCallback;

            
           // LoadParameterDropDown();
        }
       
       

        void SetupPhrasesAndColors()
        {
            FBalancePhrase.Add(98, " well balanced.");
            FBalancePhrase.Add(90, " almost balanced.");
            FBalancePhrase.Add(30, " not balanced");
            FBalancePhrase.Add(0,  " crazy!");


            FIndicatorsPhrase.Add(81," very high.");
            FIndicatorsPhrase.Add(71, " high.");
            FIndicatorsPhrase.Add(61, " challenged.");
            FIndicatorsPhrase.Add(51, " low.");
            FIndicatorsPhrase.Add(0, " threatened.");

            FAssessmentPhrase.Add(69, " Can you make this better?");
            FAssessmentPhrase.Add(61, " Try some more options.");
            FAssessmentPhrase.Add(51, " Interesting results !.");
            FAssessmentPhrase.Add(0, " You can fix this !");


            FIndicatorColors.Add(81,Color.DarkGreen);
            FIndicatorColors.Add(61, Color.GreenYellow);
            FIndicatorColors.Add(41, Color.Yellow);
            FIndicatorColors.Add(21, Color.Orange);
            FIndicatorColors.Add(0, Color.Red);
        }


        string GetValuePhrase(SortedList<int, string> TheList, int TheValue)
        {
            string result = "?";
            if (TheValue < 0) TheValue = 0;
            if ((TheList != null) && (TheList.Count > 0))
            {
                foreach (KeyValuePair<int, string> kvp in TheList)
                {
                    if (kvp.Key <= TheValue)
                    {
                        result = kvp.Value;
                    }
                }
            }
            return result;
        }

        Color GetValueColor(SortedList<int, Color> TheList, int TheValue)
        {
            Color result = Color.Black;
            if (TheValue < 0) TheValue = 0;
            if ((TheList != null) && (TheList.Count > 0))
            {
                foreach (KeyValuePair<int, Color> kvp in TheList)
                {
                    if (kvp.Key <= TheValue)
                    {
                        result = kvp.Value;
                    }
                }
            }
            return result;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();

        }

        int unitIndex = 0;
        private void ResetSanKeyGraphUnit(String aUnitName)
        {
            TheCRFModel = MyWSIM.WaterSimWestModel.GetUnitModel(aUnitName);
            if (TheCRFModel != null)
            {
                ModelUnitName = aUnitName;
                sankeyGraphUnit.Network = TheCRFModel.TheCRFNetwork;
                SankeyGraphUnitNameLabel.Text = ModelUnitName;
                unitIndex = MyWSIM.WaterSimWestModel.GetIndex(TheCRFModel);
                // This is a pain it has to be done every time model is reset
                TheCRFModel.TheCRFNetwork.Industrial.Color = Color.Purple;
                ResetIndicators(unitIndex);
            }
        }


        void ResetIndicators(int Index)
        {
            if (TheCRFModel != null)
            {
                listBoxIndicators.Items.Clear();
                listBoxIndicators2.Items.Clear();
                // Economic
                int[] ECOIndicator = MyWSIM.ParamManager.Model_Parameter(eModelParam.epP_ECORatio).ProviderProperty.getvalues().Values;
                int LastECOIndicator = ECOIndicator[Index];
                listBoxIndicators.Items.Add("Economic = " + LastECOIndicator.ToString());
                listBoxIndicators2.Items.Add("Economic = " + LastECOIndicator.ToString());

                // Envirionment
                int[] ENVIndicator = MyWSIM.ParamManager.Model_Parameter(eModelParam.epP_ENVIndicator).ProviderProperty.getvalues().Values;
                int LastENVIndicator = ENVIndicator[Index];
                listBoxIndicators.Items.Add("Environment = " + LastENVIndicator.ToString());
                listBoxIndicators2.Items.Add("Environment = " + LastENVIndicator.ToString());
                // groundwater
                int[] GWIndicator = MyWSIM.ParamManager.Model_Parameter(eModelParam.epP_GW_Annual_Indicator).ProviderProperty.getvalues().Values;
                int LastGwIndicator = GWIndicator[Index];
                listBoxIndicators.Items.Add("Groundwater = " + LastGwIndicator.ToString());
                listBoxIndicators2.Items.Add("Groundwater = " + LastGwIndicator.ToString());
                // Average
                int[] AVGIndicator = MyWSIM.ParamManager.Model_Parameter(eModelParam.epP_AverageSustainability).ProviderProperty.getvalues().Values;
                int LastAVGIndicator = AVGIndicator[Index];
                listBoxIndicators.Items.Add("Average = " + LastAVGIndicator.ToString());
                listBoxIndicators2.Items.Add("Average = " + LastAVGIndicator.ToString());
                // Balance
                int[] BalIndicator = MyWSIM.ParamManager.Model_Parameter(eModelParam.epP_NetDemandRatio).ProviderProperty.getvalues().Values;
                int LastBalIndicator = BalIndicator[Index];
                listBoxIndicators.Items.Add("Balance = " + LastBalIndicator.ToString());
                listBoxIndicators2.Items.Add("Balance = " + LastBalIndicator.ToString());
                // Assessment
                int[] ASSIndicator = MyWSIM.ParamManager.Model_Parameter(eModelParam.epP_Assessment).ProviderProperty.getvalues().Values;
                int LastASSIndicator = ASSIndicator[Index];
                listBoxIndicators.Items.Add("Assessment = " + LastASSIndicator.ToString());
                listBoxIndicators2.Items.Add("Assessment = " + LastASSIndicator.ToString());

                //// SurfaceWater
                //int[] SWIndicator = MyWSIM.ParamManager.Model_Parameter(eModelParam.epP_SWIndicator).ProviderProperty.getvalues().Values;
                //int LastSwIndicator = SWIndicator[Index];
                //listBoxIndicators.Items.Add("Surface Water = " + LastSwIndicator.ToString());

                // Urban
                int[] UrbanIndicator = MyWSIM.ParamManager.Model_Parameter(eModelParam.epP_UrbanEfficiencyIndicator).ProviderProperty.getvalues().Values;
                int LastUrbanIndicator = UrbanIndicator[Index];
                listBoxIndicators.Items.Add("Urban = " + LastUrbanIndicator.ToString());
                // Agriculure
                int[] AGIndicator = MyWSIM.ParamManager.Model_Parameter(eModelParam.epP_AgricultureIndicator).ProviderProperty.getvalues().Values;
                int LastAgIndicator = AGIndicator[Index];
                listBoxIndicators.Items.Add("Agriculture = " + LastAgIndicator.ToString());
                // Industry
                int[] INDIndicator = MyWSIM.ParamManager.Model_Parameter(eModelParam.epP_IndustryEfficiency).ProviderProperty.getvalues().Values;
                int LastINDIndicator = INDIndicator[Index];
                listBoxIndicators.Items.Add("industry = " + LastINDIndicator.ToString());
                // Power
                int[] PWIndicator = MyWSIM.ParamManager.Model_Parameter(eModelParam.epP_PowerEfficiency).ProviderProperty.getvalues().Values;
                int LastPwIndicator = PWIndicator[Index];
                listBoxIndicators.Items.Add("Power = " + LastPwIndicator.ToString());

                //Balance
                string ThePhrase = GetValuePhrase(FBalancePhrase, 100 -LastBalIndicator);
                Color TheColor = GetValueColor(FIndicatorColors, 100 -LastBalIndicator);
                labelBalanceValue.Text = (100- LastBalIndicator).ToString();
                labelBalancePhrase.Text = ThePhrase;
                panelBalanceColor.BackColor = TheColor;
                // average Indicator
                ThePhrase = GetValuePhrase(FIndicatorsPhrase, LastAVGIndicator);
                TheColor = GetValueColor(FIndicatorColors,LastAVGIndicator );
                labelIndicatorValue.Text = LastAVGIndicator.ToString();
                labelIndicatorPhrase.Text = ThePhrase;
                panelIndicatorColor.BackColor = TheColor;

                ThePhrase = GetValuePhrase(FAssessmentPhrase, LastASSIndicator);
                TheColor = GetValueColor(FIndicatorColors, LastASSIndicator);
                labelAssessmentValue.Text = LastASSIndicator.ToString();
                labelAssessmentPhrase.Text = ThePhrase;
                panelAssessmentColor.BackColor = TheColor;

                TheColor = GetValueColor(FIndicatorColors, LastECOIndicator);
                labelEconomicValue.Text = LastECOIndicator.ToString();
                panelEcoCOlor.BackColor = TheColor;

                TheColor = GetValueColor(FIndicatorColors, LastENVIndicator);
                labelEnvValue.Text = LastENVIndicator.ToString();
                panelEnvColor.BackColor = TheColor;

                TheColor = GetValueColor(FIndicatorColors, LastGwIndicator);
                labelGWValue.Text = LastGwIndicator.ToString();
                panelGWColor.BackColor = TheColor;

            }
        }

        int ParamCount = 0;
        int[] ParamCodes = null;
        string[] ParamFields = null;
        string[] ParamLabels = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads parameter drop down. Should only need to do this once</summary>
        ///
        /// <remarks>   8/15/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        void LoadParameterDropDown()
        {

            //ParamCount = MyWSIM.ParamManager.NumberOfParameters();
            //ParamFields = new string[ParamCount];
            //ParamLabels = new string[ParamCount];
            //comboBoxParameters.Items.Clear();
            //comboBoxParameters.Sorted = false;
            //int index = 0;
            //foreach(ModelParameterBaseClass MP in MyWSIM.ParamManager.AllModelParameters())
            //{
            //        ParamFields[index] = MP.Fieldname;
            //        ParamLabels[index] = MP.Label;
            //        comboBoxParameters.Items.Add(MP.Label);
            //    index++;
            //}
            ParamCount = MyWSIM.SimulationRunResults.ParameterCount();
            ParamCodes = MyWSIM.SimulationRunResults.ModelParam();
            ParamFields = new string[ParamCount];
            ParamLabels = new string[ParamCount];
            comboBoxParameters.Items.Clear();
            comboBoxParameters.Sorted = false;
            for (int i = 0; i < ParamCount; i++)
            {
                ModelParameterClass MP = MyWSIM.ParamManager.Model_Parameter(ParamCodes[i]);
                ParamFields[i] = MP.Fieldname;
                ParamLabels[i] = MP.Label;
                comboBoxParameters.Items.Add(MP.Label);
            }
            comboBoxParameters.Sorted = true;
        }
        int AdjustSanKeyGraphWidth()
        {
            return (tabPage1.Width / 2) - 5;
        }

        void AdjustInputPanelWidthAndLeft(out int NewWidth, out int NewRight)
        {
            double _newWidth = (double)tabPage1.Width * 0.55;
            NewWidth = (int)_newWidth - 10;
            NewRight = tabPage1.Width - (NewWidth + 5);
        }
        
        private void runModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TheFileNameForData == "")
            {
                MessageBox.Show("A filename mus be selected or created, use Open File or Create New");
            }
            else
            {
                // Get the scernarioName Index Tag
                int TagIndex = ActiveScenbarioName.IndexOf("_");
                // if not found
                if(TagIndex<0)
                {
                    ActiveScenbarioName += "_0";
                }
                else
                // Found but nothing after
                if (TagIndex == (ActiveScenbarioName.Length-1))
                {
                    ActiveScenbarioName += "0";
                }
                else
                {
                    // something there, see if number
                    string Temp = ActiveScenbarioName.Substring(TagIndex + 1);
                    try
                    {
                        int testme = Convert.ToInt32(Temp);
                        // ok, worked
                        // Increment and reattache
                        testme++;
                        ActiveScenbarioName = ActiveScenbarioName.Substring(0, TagIndex) + "_" + testme.ToString();
                    }
                    catch
                    {
                        // OK, not a number, create one
                        ActiveScenbarioName = ActiveScenbarioName.Substring(0, TagIndex) + "_0";
                    }
                }
                // Setup Surface Water External Model
                ProviderIntArray UseSurface = new  ProviderIntArray(1);
                MyWSIM.ParamManager.Model_Parameter(eModelParam.epP_SURFACE_USE_EXT_MODEL).ProviderProperty.setvalues(UseSurface);
                //MyWSIM.Simulation_Initialize("TestFile1.csv", "FirstRun");
                MyWSIM.Simulation_Initialize(TheFileNameForData, ActiveScenbarioName);
                //MyWSIM.Simulation_Initialize();
                SetEditParameters();

                MyWSIM.Simulation_AllYears();
                MyWSIM.Simulation_Stop();
                sankeyGraphUnit.ResetGraph();
                foreach (WaterSimSankeyForm TF in SankeyForms)
                {
                    TF.RedrawSankey();
                }
                int unitIndex = MyWSIM.WaterSimWestModel.GetIndex(MyWSIM.WaterSimWestModel.GetUnitModel(ModelUnitName));
                ResetIndicators(unitIndex);
                LoadParameterDropDown();
                ResetParmChart();
                ResetEditParametersValues();

                // EXAMPLE CODE FOR ACCESSING PROCESS
                // Get the Process Manager
                ProcessManager PM = MyWSIM.ProcessManager;
                // Here is a list of the names of all active processed
                List<string> MyProcessNames = PM.ActiveProcesses;
                // Find one process named GWIndicatorProcess:
                // Loop using the AllProcesses enumerator
                foreach (AnnualFeedbackProcess AFP in PM.AllProcesses())
                {
                    // AFP is now a reference to a process
                    // Get its name
                    string AFPName = AFP.Name;
                    // Is it the one we atre looking for
                    if (AFPName == "GWIndicatorProcess: ")
                    {
                        // Cast it as the process we are looking for, you can check first just to be safe
                        if (AFP is GWIndicatorProcess)
                        {
                            int[] TheSafeYield = (AFP as GWIndicatorProcess).SafeYield;
                            break;
                        }
                    }
                    // Alternately we can just look for the class
                    if (AFP is GWIndicatorProcess)
                    {
                        int[] TheSafeYield = (AFP as GWIndicatorProcess).SafeYield;
                        break;
                    }
                }
                // if you want to save it for future reference
                GWIndicatorProcess MyGwProcess;
                foreach (AnnualFeedbackProcess AFP in PM.AllProcesses())
                {
                    if (AFP is GWIndicatorProcess)
                    {
                        MyGwProcess = (AFP as GWIndicatorProcess);
                    }
                }
            }
        }
                private void sankeyGraph2_Load(object sender, EventArgs e)
        {

        }

        private void DisplaySankeyMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string Test = e.ClickedItem.ToString();

            Form SankeyForm = SankeyForms.Find(delegate (Form TF) { return TF.Name == Test; });
            if (SankeyForm != null)
            {
                SankeyForm.Show();
            }
            else
            {
                WaterSimCRFModel WSCRF = MyWSIM.WaterSimWestModel.GetUnitModel(Test);
                SankeyForm = new WaterSimSankeyForm(WSCRF);
                SankeyForm.Show();
                SankeyForms.Add(SankeyForm);
            }


        }

        void ResetEditParametersValues()
        {
            foreach (Control Ctrl in flowLayoutPanelMPs.Controls)
            {
                if (Ctrl.Name == "EditParam")
                {
                    (Ctrl as EditParam).SetValue(true);
                }
            }

        }

        void ResetEditParameters(int newIndex)
        {
            foreach (Control Ctrl in flowLayoutPanelMPs.Controls)
            {
                if (Ctrl.Name == "EditParam")
                {
                    (Ctrl as EditParam).RegionIndex = newIndex;
                }
            }

        }
        private void SanKeyGraphcomboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string thename = SanKeyGraphcomboBox.SelectedItem.ToString();
            ResetSanKeyGraphUnit(thename);
            int unitIndex = MyWSIM.WaterSimWestModel.GetIndex(MyWSIM.WaterSimWestModel.GetUnitModel(ModelUnitName));
            ResetIndicators(unitIndex);
            ResetEditParameters(unitIndex);
        }

        private void resetModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // EDITED QUAY 9/8 2020
            // This is wrong, should reset network
            // MyWSIM.WaterSimWestModel.ResetVariables();
            MyWSIM.WaterSimWestModel.ResetNetwork();
            // END EDIT
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (MyMultiSankey.IsDisposed)
            {
                MyMultiSankey = new ShowMultipleSankeyV1(MyWSIM);
                MyMultiSankey.Show();
            }
            else
            {
                MyMultiSankey.Show();
            }
            //MyWSIM.Simulation_Initialize();

            //WaterSimModel WSM = MyWSIM.WaterSimWestModel;
            //WSM.SetUnitValue(4, ref WSM.UrbanWaterConservation, 50);
            //MyWSIM.Simulation_AllYears();
            //MyWSIM.Simulation_Stop();
            //sankeyGraphUnit.ResetGraph();
            //foreach (WaterSimSankeyForm TF in SankeyForms)
            //{
            //    TF.RedrawSankey();
            //}


        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MyCM.BuildAnnualParameterGraph(MyWSIM, MyWSIM.SimulationRunResults, "POP_P", "Population");
            MyCM.BuildAnnualParameterGraph(MyWSIM, MyWSIM.SimulationRunResults, "UD_P", "Population");



            //waterSimChartControl1.ChartManager.BuildAnnualParameterGraph(MyWSIM, MyWSIM.SimulationRunResults, "POP_P", "Population");
        }

        int CurrentParmChartIndex = 0;

       

        private void DrawParameterGraph()
        {
            
            int index = comboBoxParameters.SelectedIndex;
            if (index >= 0)
            {
                CurrentParmChartIndex = -1;
                string ParmLabel = comboBoxParameters.Items[index].ToString();
                for (int i = 0; i < ParamLabels.Length; i++)
                {
                    if (ParamLabels[i] == ParmLabel)
                    {
                        CurrentParmChartIndex = i;
                        break;
                    }
                }
                if (CurrentParmChartIndex > -1)
                {
                    MyCM.BuildAnnualParameterGraph(MyWSIM, MyWSIM.SimulationRunResults, ParamFields[CurrentParmChartIndex], ParamLabels[CurrentParmChartIndex], regionTreeViewClass1.SelectedRegions);
                }
            }
        }

        private void DrawExampleGraph()
        {
            List<ParmTreeNode> ParmNodes = parameterTreeView1.CheckedItems;

            int index = comboBoxParameters.SelectedIndex;
            if (index >= 0)
            {
                CurrentParmChartIndex = -1;
                string ParmLabel = comboBoxParameters.Items[index].ToString();
                for (int i = 0; i < ParamLabels.Length; i++)
                {
                    if (ParamLabels[i] == ParmLabel)
                    {
                        CurrentParmChartIndex = i;
                        break;
                    }
                }
                if (CurrentParmChartIndex > -1)
                {
                    MyCM.BuildAnnualParameterGraph(MyWSIM, MyWSIM.SimulationRunResults, ParamFields[CurrentParmChartIndex], ParamLabels[CurrentParmChartIndex], regionTreeViewClass1.SelectedRegions);
                }
            }

        }
        private void comboBoxParameters_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawParameterGraph();
            //int index = comboBoxParameters.SelectedIndex;
            //if (index >= 0)
            //{
            //    CurrentParmChartIndex = -1;
            //    string ParmLabel = comboBoxParameters.Items[index].ToString();
            //    for (int i = 0; i < ParamLabels.Length; i++)
            //    {
            //        if (ParamLabels[i] == ParmLabel)
            //        {
            //            CurrentParmChartIndex = i;
            //            break;
            //        }
            //    }
            //    if (CurrentParmChartIndex > -1)
            //    {
            //        MyCM.BuildAnnualParameterGraph(MyWSIM, MyWSIM.SimulationRunResults, ParamFields[CurrentParmChartIndex], ParamLabels[CurrentParmChartIndex], regionTreeViewClass1.SelectedRegions);
            //    }
            //}
        }

        public void RegionTreeViewCallback()
        {
            DrawParameterGraph();
        }
        private void ResetParmChart()
        {
            MyCM.BuildAnnualParameterGraph(MyWSIM, MyWSIM.SimulationRunResults, ParamFields[CurrentParmChartIndex], ParamLabels[CurrentParmChartIndex], regionTreeViewClass1.SelectedRegions);
        }

        private void treeViewInput_Resize(object sender, EventArgs e)
        {
            treeViewInput.Width = panelInput.Width / 2;
        }

        private void SankeyGraphPanel_Resize(object sender, EventArgs e)
        {
            SankeyGraphPanel.Width = AdjustSanKeyGraphWidth();
           // sankeyGraphUnit.Width = SankeyGraphPanel.Width - 5;
            
        }

        private void sankeyGraphUnit_Resize(object sender, EventArgs e)
        {
            //sankeyGraphUnit.Width = SankeyGraphPanel.Width - 2;
        }

        private void SanKeyGarphControlPanel_Resize(object sender, EventArgs e)
        {

            //SanKeyGarphControlPanel.Width = (tabPage1.Width / 2) - 10;
            //SanKeyGarphControlPanel.Left = (SanKeyGarphControlPanel.Right - SanKeyGarphControlPanel.Left) + 5;
           // SanKeyGraphcomboBox.Location = new Point(SanKeyGarphControlPanel.Right - SanKeyGraphcomboBox.Width, SanKeyGraphcomboBox.Top);
        }

        private void SanKeyGarphControlPanel_Move(object sender, EventArgs e)
        {
            //SanKeyGarphControlPanel.Width = (tabPage1.Width / 2) - 10;
            //SanKeyGarphControlPanel.Left = (SanKeyGarphControlPanel.Right - SanKeyGarphControlPanel.Left)+5;
          //  SanKeyGraphcomboBox.Location = new Point(SanKeyGarphControlPanel.Right - SanKeyGraphcomboBox.Width, SanKeyGraphcomboBox.Top);
        }

        private void panelUserControls_Resize(object sender, EventArgs e)
        {
            int NewWidth = 0;
            int NewLeft = 0;
            AdjustInputPanelWidthAndLeft(out NewWidth, out NewLeft);
            panelUserControls.Left = NewLeft;
            panelUserControls.Width = NewWidth;
        }

        private void treeViewInput_Resize_1(object sender, EventArgs e)
        {
            treeViewInput.Width = panelInput.Width - flowLayoutPanelMPs.Width;
            //treeViewInput.Width = ((panelInput.Width / 5)*2)-2;
            //flowLayoutPanelMPs.Width = (panelInput.Width / 5)*3;
            //flowLayoutPanelMPs.Left = (panelInput.Width - (flowLayoutPanelMPs.Width))+2;
        }

        private void panelInput_Resize(object sender, EventArgs e)
        {

        }

        void SetEditParameters()
        {
            foreach (Control Ctrl in flowLayoutPanelMPs.Controls)
            {
                if (Ctrl.Name == "EditParam")
                {
                    //Ctrl.Foc.Focus = false;
                    
                    (Ctrl as EditParam).SetModelParameterValue();
                }
            }
        }
        void AddEditParameter(ModelParameterClass MP, int index)
        {
            int fuck = treeViewInput.Width;
            int shit = panelInput.Width;
            EditParam EP = new EditParam(MP, index, flowLayoutPanelMPs.Width-100);
            //EP.Size = new Size(flowLayoutPanelMPs.Width-30, EP.Height);
            flowLayoutPanelMPs.Controls.Add(EP);
        }

        void DeleteEditParameter(ModelParameterClass MP)
        {
            foreach (Control Ctrl in flowLayoutPanelMPs.Controls)
            {
                if (Ctrl.Name == "EditParam")
                {

                    if ((Ctrl as EditParam).Model_Parameter.ModelParam == MP.ModelParam)
                    {
                        flowLayoutPanelMPs.Controls.Remove(Ctrl);
                        break;
                    }
                }
            }
        }

        private void treeViewInput_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node is ParameterTreeNode)
                {
                    ParameterTreeNode PMTN = (e.Node as ParameterTreeNode);
                    // Where we draw the graph
                    string UnitName = e.Node.Text;
                    if (e.Node.Checked)
                    {
                        // see if in the display list, if not, add
                        int unitIndex = MyWSIM.WaterSimWestModel.GetIndex(MyWSIM.WaterSimWestModel.GetUnitModel(ModelUnitName));
                        AddEditParameter(PMTN.Model_Parameter, unitIndex);
                    }
                    else
                    {
                        // see if in the display list, if it is delete
                        // Also, Uncheck the owner, not all are checked
                        DeleteEditParameter(PMTN.Model_Parameter);
                    }
                }
            }
        }

        //private void button1_Click_1(object sender, EventArgs e)
        //{
        //    MyWSIM.Simulation_Initialize();
        //    SetEditParameters();

        //    MyWSIM.seti_SimpleAllRegionsDroughtScenario(3);
        //    // HACING
        //    //int ModelCount = MyWSIM.WaterSimWestModel.ModelCount;
        //    //int[] DDepth = new int[ModelCount];
        //    //int[] DLength = new int[ModelCount];
        //    //int[] DStartYear = new int[ModelCount];
        //    //int[] DActive = new int[ModelCount];

        //    //for (int i = 0; i < ModelCount; i++)
        //    //{
        //    //    DStartYear[i] = 2020;
        //    //    DLength[i] = 40;
        //    //    DDepth[i] = 60;
        //    //    DActive[i] = 1;
        //    //}

        //    //MyWSIM.WaterSimWestModel.seti_DroughtStartYear(DStartYear);
        //    //MyWSIM.WaterSimWestModel.seti_DroughtLength(DLength);
        //    //MyWSIM.WaterSimWestModel.seti_DroughtDepth(DDepth);
        //    //MyWSIM.WaterSimWestModel.seti_DroughtActive(DActive);
           
        //    MyWSIM.Simulation_AllYears();
        //    MyWSIM.Simulation_Stop();
        //    sankeyGraphUnit.ResetGraph();
        //    foreach (WaterSimSankeyForm TF in SankeyForms)
        //    {
        //        TF.RedrawSankey();
        //    }
        //    int unitIndex = MyWSIM.WaterSimWestModel.GetIndex(MyWSIM.WaterSimWestModel.GetUnitModel(ModelUnitName));
        //    ResetIndicators(unitIndex);
        //    LoadParameterDropDown();
        //    ResetParmChart();
        //}

        private void flowLayoutPanelMPs_Resize(object sender, EventArgs e)
        {
            int thewidth = flowLayoutPanelMPs.Width;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>  This sets the filename to a new file for data output. </summary>
        ///
        /// <remarks>   8/14/2018. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ///-------------------------------------------------------------------------------------------------

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = "csv";
            openFileDialog1.Filter = "csv files|*.csv";
            openFileDialog1.InitialDirectory = MyWSIM.DbConnection.Database;
            bool OkToUse = false;
            string errMsg = "";
            openFileDialog1.ShowDialog();
            string TempFilename = openFileDialog1.FileName;
            try
            {
                // THis is tedious, UNIDB Text server expects files to be in the Database ie the directory of the database, 
                // and it expects the filename to be like a tablename, ie no path, just relative to the database/directory,
                //  but text files can be open anywhere and have a path associated with them.
                // So we need to check for this, strip off the directory of the file and change the database if different
                string TempDir = Path.GetDirectoryName(TempFilename);
                string TempTableName = Path.GetFileName(TempFilename);
                if (MyWSIM.DbConnection.Database.ToUpper()!= TempDir.ToUpper())
                {
                    MyWSIM.DbConnection.ChangeDatabase(TempDir);
                }
                // OK, now database is the directory and TempTableName is the file/table name
                bool isErr = false;
                // OK, load this table and check that it is viable
                DataTable Temp = MyWSIM.LoadWaterSimTable(TempTableName);
                OkToUse = WaterSimManager_DB.Valid_DataTable(Temp);
                if (OkToUse)
                {
                    // it is good set the table name and reset the scenario name
                    TheFileNameForData = TempTableName;
                    ActiveScenbarioName = TheDefaultScenarioName;
                }
                else
                // Opps!
                {
                    errMsg = "File does not contain the required fieldnames : ";
                    foreach (string str in WaterSimManager_DB.RequiredFields)
                    {
                        errMsg += str + "  ";
                    }
                }

            }
            // OUCH!! Double Opps
            catch (Exception ex)
            {
                errMsg = ex.Message;
                OkToUse = false;
            }
            // Tel; the user to get their act together!  If needed of course.
            if (!OkToUse)
            {
                MessageBox.Show("Unable to Use " + TempFilename + " - " + errMsg);
            }
            else
            {
                string JustFIle = Path.GetFileName(TheFileNameForData);
                toolStripStatusLabel1.Text = JustFIle;
            }
        }

        private void toolStripMenuItemNewFile_Click(object sender, EventArgs e)
        {
            CreateNewFileDialog CNFD = new CreateNewFileDialog(MyWSIM,MyWSIM.DbConnection.Database);
            CNFD.Show();
        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            bool start = true;
            for (int i = 0; i < ParamFields.Length; i++)
            {             
                CurrentParmChartIndex = i;
                MyStream.BuildAnnualParameterStream(MyWSIM, MyWSIM.SimulationRunResults, ParamFields[CurrentParmChartIndex], ParamLabels[CurrentParmChartIndex],start);
                start = false;
                
            }
        }

    }

    
}
