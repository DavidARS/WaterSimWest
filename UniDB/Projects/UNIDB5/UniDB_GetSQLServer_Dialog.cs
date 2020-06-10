using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using UniDB;

namespace UniDB.Dialogs
{

    /// <content> Dialog for setting the SQL server.</content>
    public partial class SqlServerDialog : Form
    {
        List<string> FLocations = new List<string>();
        List<string> FDatabasenames = new List<string>();
        List<string> FPorts = new List<string>();
        List<string> FUsers = new List<string>();
        List<string> FPasswords = new List<string>();
        List<string> FOptions = new List<string>();
        List<string> FServers = new List<string>();

        string FLocation = "";
        string FDatabasename = "";
        string FPort = "";
        string FUserID = "";
        string FPassword = "";
        string FOptionStr = "";
        string FLastDirectory = "";

        SQLServer FServerType = SQLServer.stAccess;

        XmlDocument FXMLDoc;
        bool fXMLLoaded = false;

        const string FXMLFILENAME = "SQLSERVERDIALOG.XML";
        const string FSQLSERVERID = "SQLSERVERDIALOG";
        const string XMLID = "ID";
        const string XMLSERVER = "SQLSERVER";
        const string XMLSERVERLIST = "SQLSERVERS";
        const string XMLLOCATION = "LOCATION";
        const string XMLLOCATIONLIST = "SERVERLOCATIONS";
        const string XMLDATABASE = "DATABASE";
        const string XMLDATABASELIST = "DATABASES";
        const string XMLPORT = "PORT";
        const string XMLPORTLIST = "PORTS";
        const string XMLUSER = "USERID";
        const string XMLUSERLIST = "USERIDS";
        const string XMLPASSWORD = "PASSWORD";
        const string XMLPASWORDLIST = "PASSWORDS";
        const string XMLOPTION = "OPTION";
        const string XMLOPTIONLIST = "OPTIONS";
        const string XMLLastDirectory = "LASTDIRECTORY";


        const string CBLAccess = "Access - SQLServer.stAccess";
        const string SSAcces =   "Access";
        const string CBLMySQL = "MySQL - SQLServer.stMySQL";
        const string SSMySQL =  "MySQL";
        const string SSText = "Text";

        const string CBLPostGreSQL = "PostGreSQL - SQLServer.stPostGreSQL";
        const string SSPostGreSQL =  "PostGreSQL";

        //------------------------------------------------------------------------
        public SqlServerDialog()
        {
            InitializeComponent();
            //comboBoxServerType.Items.Add(CBLAccess);
            //comboBoxServerType.Items.Add(CBLMySQL);
            //comboBoxServerType.Items.Add(CBLPostGreSQL);
            LoadLists();
            comboBoxServerType.SelectedIndex = 0;
            SetUpServer(FServerType);


        }

        //------------------------------------------------------------------------
        public string ServerLocation
        { get { return FLocation; } }

        //------------------------------------------------------------------------
        public string Port
        { get { return FPort; } }

        //------------------------------------------------------------------------
        public string Datbasename
        { get { return FDatabasename; } }

        //------------------------------------------------------------------------
        public string User
        { get { return FUserID; } }

        //------------------------------------------------------------------------
        public string Password
        { get { return FPassword; } }

        //------------------------------------------------------------------------
        public string Options
        { get { return FOptionStr; } }

        //------------------------------------------------------------------------
        public SQLServer ServerType
        { get { return FServerType; } }

        //------------------------------------------------------------------------
        private void SqlServerDialog_Load(object sender, EventArgs e)
        {

        }

        //------------------------------------------------------------------------
        string ExtractServer(string Source)
        {
            string temp = "";
            int index = Source.IndexOf("-");
            if (index > -1)
                temp = Source.Substring(0, index - 1);
            return temp;
        }

        //------------------------------------------------------------------------
        protected string Encrypt(string Source)
        {
            DateTime DT = new DateTime();
            DT = DateTime.Now;
            int imod = (DT.Second % 256);
            byte bmod = Convert.ToByte(imod);
            string temp = bmod.ToString()+" ";
            foreach (char ch in Source)
            {
                temp += ((byte) ch ^ bmod).ToString() +" ";
            }
            return temp;
        }

        //------------------------------------------------------------------------
        protected string Decrypt(string Source)
        {
            string temp = "";
            string Original="";
            string grab = "";
            if (Source.Length > 3)
            {   
                temp = Source;
                int index = temp.IndexOf(" ");
                grab = temp.Substring(0, index);
                temp = temp.Substring(index + 1, temp.Length -( index+1));
                byte bmod = Convert.ToByte(grab);
                index = temp.IndexOf(" ");
                while ((index > 0)&&(temp.Length>1))
                {
                    grab = temp.Substring(0, index);
                    temp = temp.Substring(index + 1, temp.Length -( index+1));
                    byte Code = Convert.ToByte(grab);
                    Original += (Char)(Code ^ bmod);
                    index = temp.IndexOf(" ");
                }
            }
            return Original;
        }

        //------------------------------------------------------------------------
        protected void CopyToComboBox(List<string> list, ComboBox CB)
        {
            foreach (string str in list)
                CB.Items.Add(str);
        }

        //------------------------------------------------------------------------
        protected void LoadXMLList(string Tag, List<string> TheList, ComboBox CB)
        {
            XmlNodeList Nodes = FXMLDoc.GetElementsByTagName(Tag);
            if (Nodes.Count > 0)
            {
                foreach (XmlNode aNode in Nodes)
                {
                    TheList.Add(aNode.InnerText);
                }
                if (TheList.Count > 0)
                {
                    CopyToComboBox(TheList, CB);
                }
            }
        }
        // Load the string lists from XML file if there
        private void LoadLists()
        {
            string input = "";
            if (File.Exists(FXMLFILENAME))
                // if file is their, load the XMLDOC from the file
            {
                try
                {
                    FXMLDoc = new XmlDocument();
                    using (StreamReader SR = new StreamReader(FXMLFILENAME))
                    {
                        input = SR.ReadToEnd();
                    }
                    FXMLDoc.Load(FXMLFILENAME);
                    fXMLLoaded = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("XML List Error : " + ex.Message + "  Is the " + FXMLFILENAME + " in the program root directoyr?");
                }
            }
            else
                // if not there create one
            {
                FXMLDoc = CreateServerXMLDoc();
                fXMLLoaded = true;
                XMLChanged = true;
            }
            // ok if there was no error, setup the lists
            if (fXMLLoaded)
            {
                    XmlNodeList Nodes;
                    Nodes = FXMLDoc.GetElementsByTagName(XMLID);
                    // check if SQLSERVER XML File
                    if (Nodes.Count > 0)
                    {
                        if (Nodes[0].InnerText == FSQLSERVERID)
                        {

                            // Get Servers
                            LoadXMLList(XMLSERVER, FServers, comboBoxServerType);
                            // Get Locations
                            LoadXMLList(XMLLOCATION, FLocations, comboBoxLocation);
                            // Get Databases
                            LoadXMLList(XMLDATABASE, FDatabasenames, comboBoxDatabase);
                            // Get Ports
                            LoadXMLList(XMLPORT, FPorts, comboBoxPort);
                            // Get UserIDs
                            LoadXMLList(XMLUSER, FUsers, comboBoxUser);
                            // Get Passwords
                            Nodes = FXMLDoc.GetElementsByTagName(XMLPASSWORD);
                            if (Nodes.Count > 0)
                            {
                                foreach (XmlNode aNode in Nodes)
                                {
                                    string temp = Decrypt(aNode.InnerText);
                                    FPasswords.Add(temp);
                                }
                                CopyToComboBox(FPasswords, comboBoxPassword);
                            }
                            // Get Options
                            LoadXMLList(XMLOPTION, FOptions, comboBoxOptions);
                            // Get Last Directory
                            Nodes = FXMLDoc.GetElementsByTagName(XMLLastDirectory);
                            if (Nodes.Count > 0)
                            {
                                FLastDirectory = Nodes[0].InnerText;
                            }

                            // 
                        }
                    }
                }
        }

        //------------------------------------------------------------------------
        private void SetUpServer(SQLServer ServerType)
        {
            switch (ServerType)
            {
                case SQLServer.stAccess:
                    {
                        comboBoxPort.Text = "";
                        comboBoxPort.Enabled = false;
                        comboBoxLocation.Text = "";
                        comboBoxLocation.Enabled = false;
                        buttonGetFileName.Visible = true;
                        buttonGetFileName.Enabled = true;
                        break;
                    }
                case SQLServer.stMySQL:
                    {
                        comboBoxPort.Text = "";
                        comboBoxPort.Enabled = false;
                        comboBoxLocation.Text = "LocalHost";
                        comboBoxLocation.Enabled = true;
                        buttonGetFileName.Visible = false;
                        buttonGetFileName.Enabled = false;
                        break;
                    }
                case SQLServer.stPostgreSQL:
                    {
                        comboBoxPort.Text = UniDbConnection.PostGreSQLPort;
                        comboBoxPort.Enabled = true;
                        comboBoxLocation.Text = "LocalHost";
                        comboBoxLocation.Enabled = true;
                        buttonGetFileName.Visible = false;
                        buttonGetFileName.Enabled = false;
                        break;
                    }
            }
        }

        private XmlDocument CreateServerXMLDoc()
        {
            XmlDocument NewXML = new XmlDocument();
            // First Add a root
            XmlNode RootNode = NewXML.CreateNode(XmlNodeType.Element, "ROOT", null);           
            //Add ID
            XmlNode Temp = NewXML.CreateNode(XmlNodeType.Element, "ID", null);
            Temp.InnerText = "SQLSERVERDIALOG";
           RootNode.AppendChild(Temp);

            // now add standard SQLServers
            Temp = NewXML.CreateNode(XmlNodeType.Element, XMLSERVERLIST, null);
            
            XmlNode TempSub = NewXML.CreateNode(XmlNodeType.Element, XMLSERVER, null);
            TempSub.InnerText = "Access - SQLServer.stAccess";
            Temp.AppendChild(TempSub);
            TempSub = NewXML.CreateNode(XmlNodeType.Element, XMLSERVER, null);
            TempSub.InnerText = "MySQL - SQLServer.stMySQL";
            Temp.AppendChild(TempSub);
            TempSub = NewXML.CreateNode(XmlNodeType.Element, XMLSERVER, null);
            TempSub.InnerText = "PostGreSQL - SQLServer.stPostGreSQL";
            Temp.AppendChild(TempSub);
            TempSub = NewXML.CreateNode(XmlNodeType.Element, XMLSERVER, null);
            TempSub.InnerText = "Text - SQLServer.stText";
            Temp.AppendChild(TempSub);
            
           RootNode.AppendChild(Temp);
            // Now Add Other Tags
            Temp = NewXML.CreateNode(XmlNodeType.Element, XMLDATABASELIST, null);
           RootNode.AppendChild(Temp);
            Temp = NewXML.CreateNode(XmlNodeType.Element, XMLLOCATIONLIST, null);
           RootNode.AppendChild(Temp);
            Temp = NewXML.CreateNode(XmlNodeType.Element, XMLPORTLIST, null);
           RootNode.AppendChild(Temp);
            Temp = NewXML.CreateNode(XmlNodeType.Element, XMLUSERLIST, null);
           RootNode.AppendChild(Temp);
            Temp = NewXML.CreateNode(XmlNodeType.Element, XMLPASWORDLIST, null);
           RootNode.AppendChild(Temp);
            Temp = NewXML.CreateNode(XmlNodeType.Element, XMLOPTIONLIST, null);
           RootNode.AppendChild(Temp);
           Temp = NewXML.CreateNode(XmlNodeType.Element, XMLLastDirectory, null);
           RootNode.AppendChild(Temp);

            NewXML.AppendChild(RootNode);
            return NewXML;
        }
        //------------------------------------------------------------------------
        private bool TestInList(string target, List<string> TheList)
        {
            string found = TheList.Find(delegate(string str) { return (str == target); });
            return (found != null);
        }
        
        private void AddToXML(string target, List<string> TheList, string XMLList, string XMLTag)
        {
            if (target != "")
            {
                bool testit = TestInList(target, TheList);
                if (!testit)
                {
                    XmlNodeList Nodes = FXMLDoc.GetElementsByTagName(XMLList);
                    if (Nodes.Count > 0)
                    {
                        XmlNode NewNode = FXMLDoc.CreateNode(XmlNodeType.Element, XMLTag, null);
                        NewNode.InnerText = target;
                        Nodes[Nodes.Count - 1].AppendChild(NewNode);
                        XMLChanged = true;
                    }
                }
            }
        }

        bool XMLChanged = false;
        //------------------------------------------------------------------------
        private void SqlServerDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            FLocation = comboBoxLocation.Text;
            FDatabasename = comboBoxDatabase.Text;
            FPort = comboBoxPort.Text;
            FUserID = comboBoxUser.Text;
            FPassword = comboBoxPassword.Text;
            FOptionStr = comboBoxOptions.Text;
            
            if (fXMLLoaded)
            {
            // OK ADD Database
                AddToXML(FDatabasename, FDatabasenames, XMLDATABASELIST, XMLDATABASE);
                AddToXML(FLocation, FLocations, XMLLOCATIONLIST, XMLLOCATION);
                AddToXML(FUserID, FUsers, XMLUSERLIST, XMLUSER);
                AddToXML(FOptionStr, FOptions, XMLOPTIONLIST, XMLOPTION);
                // Add Pasword by had so it can be encrypted
                if (FPassword != "")
                {
                    bool testit = TestInList(FPassword, FPasswords);
                    if (!testit)
                    {
                        XmlNodeList Nodes = FXMLDoc.GetElementsByTagName(XMLPASWORDLIST);
                        if (Nodes.Count > 0)
                        {
                            XmlNode NewNode = FXMLDoc.CreateNode(XmlNodeType.Element, XMLPASSWORD, null);
                            NewNode.InnerText = Encrypt(FPassword);
                            Nodes[Nodes.Count - 1].AppendChild(NewNode);
                            XMLChanged = true;
                        }
                    }
                }
                // add last directory if Text or Access
                if ((FServerType == SQLServer.stAccess) || (FServerType == SQLServer.stText))
                {
                    XmlNodeList Nodes = FXMLDoc.GetElementsByTagName(XMLLastDirectory);
                    if (Nodes.Count > 0)
                    {
                        XmlNode TheNode = Nodes[0];
                        string UseThis = comboBoxDatabase.Text;
                        if (UseThis == "")
                        {
                            UseThis = FLastDirectory;
                        }
                        TheNode.InnerText = UseThis;
                        XMLChanged = true;
                    }
                }
                // Done
                if (XMLChanged)
                    FXMLDoc.Save(FXMLFILENAME);
            }
        }

        bool IndexChanged = false;
        private void comboBoxServerType_TextChanged(object sender, EventArgs e)
        {
            if (!IndexChanged)
                comboBoxServerType.Text = "Select From Dropdown";
            IndexChanged = false;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Event handler. Called by comboBoxServerType for selected index changed
        ///             events. </summary>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ///-------------------------------------------------------------------------------------------------

        private void comboBoxServerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            IndexChanged = true;
                      
            string temp = comboBoxServerType.SelectedItem.ToString();
            string ServerStr = ExtractServer(temp);

            if (ServerStr != "")
            {
                switch (ServerStr)
                {
                    case SSAcces:
                        FServerType = SQLServer.stAccess;
                        break;
                    case SSMySQL:
                        FServerType = SQLServer.stMySQL;
                        break;
                    case SSPostGreSQL:
                        FServerType = SQLServer.stPostgreSQL;
                        break;
                    case SSText:
                        FServerType = SQLServer.stText;
                        break;
                }
                SetUpServer(FServerType);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Event handler. Called by buttonGetFileName for click events. </summary>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ///-------------------------------------------------------------------------------------------------

        private void buttonGetFileName_Click(object sender, EventArgs e)
        {

            // check to see of FServerType is Text, if so open a folder browser dialog
            if (FServerType == SQLServer.stText)
            {
                //openFileDialog1.ValidateNames = false;
                //openFileDialog1.CheckFileExists = false;
                //openFileDialog1.CheckPathExists = true;
                //openFileDialog1.InitialDirectory = FLastDirectory;
                //openFileDialog1.FileName = "Folder Selection.";
                //if (openFileDialog1.ShowDialog() == DialogResult.OK)
                //{
                //    comboBoxDatabase.Text = folderBrowserDialog1.SelectedPath;
                //}
                folderBrowserDialog1.SelectedPath = FLastDirectory;
                //folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Recent;
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    comboBoxDatabase.Text = folderBrowserDialog1.SelectedPath;
                    FLastDirectory = folderBrowserDialog1.SelectedPath;
                    
                }

            }
            else
            {
                // otherwise open a FileDialog 
                openFileDialog1.InitialDirectory = FLastDirectory;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    comboBoxDatabase.Text = openFileDialog1.FileName;
                }
            }
        }

    }
}
