using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaterSimDCDC;
using System.Drawing;

namespace WaterSim_West_v_1
{
    public class ParameterTreeNode : TreeNode
    {
        ModelParameterClass FMP = null;
        public ParameterTreeNode(ModelParameterClass MP) : base(MP.Label)
        {
            FMP = MP;
            if (FMP.isInputParam)
            {
                if (FMP.isProviderParam)
                {
                    this.ForeColor = Color.DarkBlue;
                }
                else
                {
                    this.ForeColor = Color.Green;
                }
            }
            else
            {
                this.ForeColor = Color.LightGray;
            }
        }

        public ModelParameterClass Model_Parameter
        { get { return FMP; } }

    }

}
