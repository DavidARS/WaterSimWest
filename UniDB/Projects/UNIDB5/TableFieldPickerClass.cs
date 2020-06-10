using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UniDb2
{
    public partial class TableFieldPickerClass : Form
    {
        bool FMultiField = false;
        DataTable FData = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the UniDb2.TableFieldPickerClass form </summary>
        ///
        ///-------------------------------------------------------------------------------------------------

        public TableFieldPickerClass()
        {
            InitializeComponent();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the data table.</summary>
        ///
        /// <value> the data table.</value>
        ///-------------------------------------------------------------------------------------------------

        public DataTable TheDataTable
        {
            set { SetDataTable(value); }
            get { return FData;  }
        }

        public bool MultiField
        {
            set { SetFieldSelect(value); }
            get { return FMultiField; }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Sets field select.</summary>
        /// <param name="MultiField"> True to multi field.</param>
        ///-------------------------------------------------------------------------------------------------

        void SetFieldSelect(bool MultiField)
        {
            FMultiField = MultiField;
            if (FMultiField)
            {
                // hide combo box
                comboBoxSingleField.Hide();
                // show listbox
                listBoxMultiField.Show();
                // Chenge Label with plural
                labelPickField.Text = "Pick Fields";
                // expand window to include bottom of listbox
                this.Height = listBoxMultiField.Bottom + 150;
            }
            else
            {
                // hide listbox
                listBoxMultiField.Hide();
                // show combo box
                comboBoxSingleField.Show();
                // change label with single
                labelPickField.Text = "Pick Field";
                // shrink window to bottom of combo box
                this.Height = comboBoxSingleField.Bottom + 150;
            }
            Application.DoEvents();
        }

        void SetDataTable(DataTable aDataTable)
        {
            FData = aDataTable;
            listBoxMultiField.Items.Clear();
            comboBoxSingleField.Items.Clear();
            foreach(DataColumn COL in FData.Columns)
            {
                listBoxMultiField.Items.Add(COL.ColumnName);
                comboBoxSingleField.Items.Add(COL.ColumnName);
            }

        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the selected field.</summary>
        ///
        /// <value> The selected field.</value>
        ///-------------------------------------------------------------------------------------------------

        public string SelectedField
        {
            get
            {
                string result = "";
                if (!MultiField)
                {
                    result = comboBoxSingleField.Text;
                }
                else
                {
                    if (listBoxMultiField.SelectedItems.Count > 0)
                    {
                        result = listBoxMultiField.SelectedItems[0].ToString();
                    }
                }
                return result;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the selected fields.</summary>
        ///
        /// <value> The selected fields.</value>
        ///-------------------------------------------------------------------------------------------------

        public List<string> SelectedFields
        {
            get
            {
                List<string> result = new List<string>();
                if (MultiField)
                {
                    foreach (object lbo in listBoxMultiField.SelectedItems)
                    {
                        result.Add(lbo.ToString());
                    }
                }
                else
                {
                    result.Add(comboBoxSingleField.Text);
                }
                return result;
            }
        }
    }
}
