using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WaterSimDCDC;

namespace WaterSimDCDC.Controls
{
    public partial class ProcessSeclectControl : UserControl
    {
        WaterSimDCDC.WaterSimManager MyWSim;

        public ProcessSeclectControl()
        {
            InitializeComponent();
        }

        public WaterSimManager WaterSim
        {
            get { return MyWSim; }
            set
            {
                if (value != null)
                {
                    MyWSim = value;
                    UpdateProcessManager();
                    UpdateProcessRegistry();
                    MyWSim.ProcessManager.ProcessManagerChangeEvent += MyProcessManagerChangeEventHandler;
                    MyWSim.ProcessRegistry.ProcessRegistryChangeEvent += MyProcessRegistryChangeEventHandler;
                }
            }

        }
        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        public void MyProcessManagerChangeEventHandler(object sender, ProcessManagerEventArgs e)
        {
            // if adding or deleting a process, update process list
            if ((e.TheProcessEventType == ProcessManagerEventType.peAddProcess) || (e.TheProcessEventType == ProcessManagerEventType.peDeleteProcess))
            {
                UpdateProcessManager();
            }
        }
        public void MyProcessRegistryChangeEventHandler(object sender, ProcessRegistryEventArgs e)
        {
            // if adding or deleting a process, update process list
            if ((e.TheEventType == ProcessRegistryEventType.preAdd) || (e.TheEventType == ProcessRegistryEventType.preDelete))
            {
                UpdateProcessRegistry();
            }
        }
        // Update the Process Registry List box with current registered processes
        private void UpdateProcessRegistry()
        {
            if (MyWSim != null)
            {
                List<string> temp = MyWSim.ProcessRegistry.ClassNameList;
                ProcessRegistryListBox.Items.Clear();
                foreach (string str in temp)
                {
                    ProcessRegistryListBox.Items.Add(str);
                }
            }
        }
        // Update the process Registry List box with current Process Que items
        private void UpdateProcessManager()
        {
            if (MyWSim != null)
            {
                List<string> temp = MyWSim.ProcessManager.ActiveProcesses;
                ProcessManagerListBox.Items.Clear();
                foreach (string str in temp)
                {
                    ProcessManagerListBox.Items.Add(str);
                }
            }
        }
        private void ProcessRegistryListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MyWSim != null)
            {
                string SelectedName = ProcessRegistryListBox.SelectedItem.ToString();
                int index = ProcessRegistryListBox.SelectedIndex;
                CheckState ischecked = ProcessRegistryListBox.GetItemCheckState(index);
                AnnualFeedbackProcess AFP = MyWSim.ProcessRegistry.Construct(SelectedName, MyWSim);
                if (AFP != null)
                {
                    MyWSim.ProcessManager.AddProcess(AFP);
                    
                }
            }
        }

        private void ProcessRegistryListBox_DoubleClick(object sender, EventArgs e)
        {
            if (MyWSim != null)
            {
                string SelectedName = ProcessRegistryListBox.SelectedItem.ToString();
                int index = ProcessRegistryListBox.SelectedIndex;
                CheckState ischecked = ProcessRegistryListBox.GetItemCheckState(index);
                if (ischecked == CheckState.Unchecked)
                {
                    AnnualFeedbackProcess AFP = MyWSim.ProcessRegistry.Construct(SelectedName, MyWSim);
                    if (AFP != null)
                    {
                        MyWSim.ProcessManager.AddProcess(AFP);
                    }
                }
                else
                {
                    MyWSim.ProcessManager.Delete(SelectedName);
                }
            }

        }

        private void ProcessRegistryListBox_Click(object sender, EventArgs e)
        {
            if (MyWSim != null)
            {
                string SelectedName = ProcessRegistryListBox.SelectedItem.ToString();
                int index = ProcessRegistryListBox.SelectedIndex;
                CheckState ischecked = ProcessRegistryListBox.GetItemCheckState(index);
            }
        }

        private void ProcessRegistryListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (MyWSim != null)
            {
                string SelectedName = ProcessRegistryListBox.SelectedItem.ToString();
                int index = ProcessRegistryListBox.SelectedIndex;
                CheckState ischecked = ProcessRegistryListBox.GetItemCheckState(index);
                if (ischecked == CheckState.Unchecked)
                {
                    AnnualFeedbackProcess AFP = MyWSim.ProcessRegistry.Construct(SelectedName, MyWSim);
                    if (AFP != null)
                    {
                        MyWSim.ProcessManager.AddProcess(AFP);
                    }
                }
                else
                {
                    AnnualFeedbackProcess AFP = MyWSim.ProcessManager.FindByClassname(SelectedName);
                    if (AFP!=null)
                       MyWSim.ProcessManager.Delete(AFP);
                }
            }

        }

        private void ProcessRegistryListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string classname = ProcessRegistryListBox.SelectedItem.ToString();
            if (MyWSim != null)
            {
                System.Type theclass = MyWSim.ProcessRegistry.Find(classname);
                if (theclass != null)
                {
                    string classdescrip = MyWSim.ProcessRegistry.ClassDescription(classname);
                    StatusLabel1.Text = classname + " : " + classdescrip;
                }
                else
                {
                    StatusLabel1.Text = "";
                }
            }
            else
            {
                    StatusLabel1.Text = "";
            }
        }

        private void ProcessManagerListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SelectedName = ProcessManagerListBox.SelectedItem.ToString();
            int index = SelectedName.IndexOf(" : ");
            if (index>-1)
            {
                SelectedName = SelectedName.Substring(0, index).Trim();
                AnnualFeedbackProcess AFP = MyWSim.ProcessManager.Find(SelectedName);
                if (AFP != null)
                {
                    string temp = AFP.ProcessCode + "  " + AFP.Name + " : " + AFP.ProcessLongDescription;
                    StatusLabel1.Text = temp;
                }
        }
        }
    }
}
