using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaterSimDCDC;

namespace WaterSim_West_v_1
{
    public partial class EditParam : UserControl
    {
        double FinitialValue = 0;
        double FCurrentValue = 0;
        ModelParameterClass FMP;
        int FregionIdex = 0;
        public EditParam()
        {
            InitializeComponent();
            numericUpDownParAM.Minimum = 0;
            numericUpDownParAM.Maximum = int.MaxValue;
            

        }

        public EditParam(string LabelStr, double initialValue)
        {
            InitializeComponent();

            numericUpDownParAM.Minimum = 0;
            numericUpDownParAM.Maximum = int.MaxValue;
            
            FMP = null;
            FregionIdex = 0;
            labelParam.Text = LabelStr;
            FinitialValue = initialValue;
            numericUpDownParAM.Value = (decimal)FinitialValue;
            FCurrentValue = FinitialValue;
            int LeftText = numericUpDownParAM.Left;
            Size TextSize = TextRenderer.MeasureText(labelParam.Text, labelParam.Font);
            double Ratio = (double)TextSize.Width / (double)LeftText;
            if (Ratio > 1)
            {
                float NewFontSize = labelParam.Font.Size / (float)Ratio;
                Font NewFont = new Font(labelParam.Font.FontFamily, NewFontSize, labelParam.Font.Style);
                labelParam.Font = NewFont;
            }

            buttonDoall.Image = imageList1.Images[1];
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.buttonDoall, this.buttonDoall.Text);
            System.Windows.Forms.ToolTip ToolTip2 = new System.Windows.Forms.ToolTip();
            ToolTip2.SetToolTip(this.button1, this.button1.Text);



        }

        public EditParam(ModelParameterClass MP, int RegionIndex, int widthRequest)
        {
            InitializeComponent();
            this.Width = widthRequest;
            numericUpDownParAM.Minimum = 0;
            numericUpDownParAM.Maximum = int.MaxValue;
           
            FMP = MP;
            FregionIdex = RegionIndex;
            if (MP != null)
            {
                labelParam.Text = MP.Label;
                SetValue(false);
            }
            else
            {
                labelParam.Text = "Null Parameter!";
                FinitialValue = 0;
                numericUpDownParAM.Value = 0;
                FCurrentValue = FinitialValue;
            }
            int LeftText = numericUpDownParAM.Left;
            Size TextSize = TextRenderer.MeasureText(labelParam.Text, labelParam.Font);
            double Ratio = (double)TextSize.Width / (double)LeftText;
            if (Ratio > 1)
            {
                float NewFontSize = labelParam.Font.Size / (float)Ratio;
                Font NewFont = new Font(labelParam.Font.FontFamily, NewFontSize, labelParam.Font.Style);
                labelParam.Font = NewFont;
            }
        }

        public void SetValue(bool SetColor)
        {
            if (FMP != null)
            {
                if (!FMP.isInputParam)
                {
                    numericUpDownParAM.Enabled = false;
                }
                if (FMP.isBaseParam)
                {
                    FinitialValue = FMP.Value;
                }
                else
                {
                    if ((FregionIdex < 0) || (FregionIdex >= FMP.ProviderProperty.getvalues().Values.Length))
                    {
                        FregionIdex = 0;
                    }
                    FinitialValue = FMP.ProviderProperty.getvalues().Values[FregionIdex];
                }
                numericUpDownParAM.Value = (decimal)FinitialValue;
                if (SetColor)
                {
                    if (FCurrentValue != FinitialValue)
                    {
                        BackColor = Color.SkyBlue;
                    }
                }
                FCurrentValue = FinitialValue;
            }
        }
        private void numericUpDownParAM_ValueChanged(object sender, EventArgs e)
        {
            FCurrentValue = (double)numericUpDownParAM.Value;
            numericUpDownParAM.BackColor = SystemColors.Window;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Cancel Edit?", "Cancel", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                numericUpDownParAM.Value = (decimal)FinitialValue;
                FCurrentValue = FinitialValue;
            }
        }

        public double Value
        {
            get { return FCurrentValue; }
            set
            {
                FinitialValue = value;
                FCurrentValue = FinitialValue;
                numericUpDownParAM.Value = (decimal)FinitialValue;
            }
        }

        bool FChangeAllValues = false;

        public bool AllValues
        {
            get { return FChangeAllValues; }
        }


        public int RegionIndex
        {  get { return FregionIdex;  }
           set
            {
                FregionIdex = value;
                if ((FregionIdex < 0) || (FregionIdex >= FMP.ProviderProperty.getvalues().Values.Length))
                {
                    FregionIdex = 0;
                }
                if (FMP.isProviderParam)
                {
                    FinitialValue = FMP.ProviderProperty.getvalues().Values[FregionIdex];
                    numericUpDownParAM.Value = (decimal)FinitialValue;
                    FCurrentValue = FinitialValue;
                }

            }
}
        public ModelParameterClass Model_Parameter
        {
            get { return FMP;  }
        }

        public void SetModelParameterValue()
        {
            if (FMP != null)
            {
                double HoldCurrentValue = FCurrentValue; 
                try
                {
                    if (FMP.isBaseParam)
                    {
                        // The event of change value is unreliable, only occurs when focus is lost force change
                        FCurrentValue = (double)numericUpDownParAM.Value;
                        FMP.Value = (int)FCurrentValue;
                    }
                    else
                    if (FMP.isProviderParam)
                    {
                        if (!FChangeAllValues)
                        {
                            // The event of change value is unreliable, only occurs when focus is lost force change
                            FCurrentValue = (double)numericUpDownParAM.Value;
                            int[] CurrentValues = FMP.ProviderProperty.getvalues().Values;
                            CurrentValues[FregionIdex] = (int)FCurrentValue;
                            ProviderIntArray Temp = new ProviderIntArray();
                            Temp.Values = CurrentValues;
                            FMP.ProviderProperty.setvalues(Temp);
                        }
                        else
                        {
                            FCurrentValue = (double)numericUpDownParAM.Value;
                            int[] CurrentValues = FMP.ProviderProperty.getvalues().Values;
                            for (int i = 0; i < CurrentValues.Length; i++)
                            {
                                CurrentValues[i] = (int)FCurrentValue;
                            }
                            ProviderIntArray Temp = new ProviderIntArray();
                            Temp.Values = CurrentValues;
                            FMP.ProviderProperty.setvalues(Temp);


                        }
                    }
                }
                catch 
                {
                    //
                    numericUpDownParAM.Value  = (decimal)HoldCurrentValue;
                    numericUpDownParAM.BackColor = Color.LightCoral;
                }
            }
        }

        private void EditParam_Resize(object sender, EventArgs e)
        {
            float Fwidth = Width;
        }

        private void buttonDoall_Click(object sender, EventArgs e)
        {
            FChangeAllValues = !FChangeAllValues;
            if (FChangeAllValues)
            {
                buttonDoall.Image = imageList1.Images[0];
                
            }
            else
            {
                buttonDoall.Image = imageList1.Images[1];
            }
        }
    }
}
