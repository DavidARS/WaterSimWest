using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WaterSimUI.UserControls
{
    public partial class OutputUserControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //Adding Options to select chart in properties
        public enum ChartOption
        {
            OFMP,
            OFMPR,
            OFOP,
            MFOP,
            BASEA,
            BASEL,
            // DAS edit 10.10.14
            BASEAL,
            // QUAY EDIT 3/13/14
            BASESL,
            BASESA,
            OFMPL,
            //DAS
            WSAPM,
            WSAPS,
            WSAPP,
            WSASR,
            WSASC,
            WSASF,
            WSASK,
            WSASL
            //--------------------
        }

        //setting and getting the type of chart to be displayed
        public ChartOption Type
        {
            get { return (ChartOption)Enum.Parse(typeof(ChartOption), lblChartOption.Text, true); }
            set { lblChartOption.Text = value.ToString(); }
        }

        public string FieldName
        {
            get { return lblFldName.Text; }
            set { lblFldName.Text = value; }
        }

        public string Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }
        public string SeriesColors
        {
            get { return lblSeriesColors.Text; }
            set { lblSeriesColors.Text = value; }
        }
    }
}