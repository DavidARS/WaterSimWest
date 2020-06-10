using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace WaterSimUI.UserControls
{
    public partial class InputUserControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hvHelpURI.Value = ConfigurationManager.AppSettings["URIHelp"];
        }

        //setting and getting name(Name Property)
        public string FieldKeyWord
        {
            set { 
                  lblSliderKeyWord.Text = value;
                  
                }
            get { return lblSliderKeyWord.Text; }
        }

        // MODIFIED BY QUAY 5/7/15
        // Sets the small units for the control
        public string FieldUnit
        {
            set
            {
               this.lblunits.Text = value;
                
            }
            get
            {
               return lblunits.Text;
            }
        }
        //--------------------------- 5/7/15
    }
}