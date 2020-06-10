using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniDB;
using UniDB.Dialogs;

namespace TestUniDb2
{
    public partial class Form1 : Form
    {
        SqlServerDialog ServerDlg = new SqlServerDialog();
        TablenameDialog TableDlg = new TablenameDialog();

        UniDbConnection MyUniDbConnection;
        public Form1()
        {
            InitializeComponent();
            toolStripTextBox1.Text = UniDB.UniDbVersion.Version();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            bool IsError = false;
            string ErrorMessage = "";
            //MyUniDbConnection = new UniDbConnection(SQLServer.stAccess, "Database=TestuniDb2.mdb");
//            MyUniDbConnection = new UniDbConnection(SQLServer.stAccess, "", "G:\\DCDC\\Visual Studio Projects\\Db Stuff\\UniDb2\\Projects\\UniDb2\\bin\\Debug\\TestUniDb2.mdb", "", "", "");
//            MyUniDbConnection = new UniDbConnection(SQLServer.stText, "", "G:\\DCDC\\Visual Studio Projects\\Db Stuff\\UniDb2\\Projects\\UniDb2\\bin\\Debug", "", "", "");
            MyUniDbConnection = new UniDbConnection(SQLServer.stText, "", "G:\\DCDC\\Visual Studio Projects\\Db Stuff\\UniDb2\\Source", "", "", "");
            //            MyUniDbConnection = new UniDbConnection(SQLServer.stText, "", "I:\\Visual Studio Projects\\Db Stuff\\UniDb2\\Projects\\UniDb2\\bin\\Debug", "", "", "");
            MyUniDbConnection.Open();
//            DataTable DT = Tools.LoadTable(MyUniDbConnection, "TestTable", ref IsError, ref ErrorMessage);
            DataTable DT = Tools.LoadTable(MyUniDbConnection, "TxtDbTestFile1.cdf.txt", ref IsError, ref ErrorMessage);
            listBox1.Items.Clear();
            foreach (DataRow DR in DT.Rows)
            {
                string line = "";
                foreach (DataColumn DC in DT.Columns)
                {
                    string colval = DR[DC].ToString();
                    line += colval + " ";
                }
                listBox1.Items.Add(line);
            }
            MyUniDbConnection.Close();
        }

        private void toolStripMenuItemSelectTable_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItemOpenDB_Click(object sender, EventArgs e)
        {
            if (ServerDlg.ShowDialog() == DialogResult.OK)
            {
                MyUniDbConnection = new UniDbConnection(ServerDlg.ServerType, ServerDlg.ServerLocation, ServerDlg.Datbasename, ServerDlg.User, ServerDlg.Password, ServerDlg.Options);
                MyUniDbConnection.Open();
            }
            
        }

        private void ToolStripMenuItemSchema_Click(object sender, EventArgs e)
        {
            if (MyUniDbConnection != null)
            {
                if (MyUniDbConnection.State == ConnectionState.Open)
                {
                    DataTable SchemaTable = MyUniDbConnection.GetSchema();
                    dataGridViewTest.DataSource = SchemaTable;
                }
            }
            
        }

        private void ToolStripMenuItemGetTables_Click(object sender, EventArgs e)
        {
            if (MyUniDbConnection != null)
            {
                if (MyUniDbConnection.State == ConnectionState.Open)
                {
                    bool isError = false;
                    string errMessage = "";
                    List<string> TheNames = MyUniDbConnection.GetTableNames(ref isError, ref errMessage);
                    listBox1.Items.Clear();
                    foreach (string table in TheNames)
                    {
                        listBox1.Items.Add(table);
                    }
                }
            }

        }

        private void testReadTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MyUniDbConnection != null)
            {
                TableDlg.dbConnect = MyUniDbConnection;
                if (TableDlg.ShowDialog() == DialogResult.OK)
                {
                    string TableName = TableDlg.Tablename;
                    UniDataAdapter TheAdapter = new UniDataAdapter("SELECT * FROM " + TableName + ";",MyUniDbConnection);
                    DataTable TheTable = new DataTable();
                    TheAdapter.Fill(TheTable);
                    dataGridViewTest.DataSource = TheTable;
                }

            }
        }
    }
}
