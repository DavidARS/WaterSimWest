
/**********************************************************************************
	WaterSimDCDC Regional Water Demand and Supply Model
	Version 5.0

	WaterSim_Controls Version 3 7/25/11  Quay

	Copyright (C) 2011,2012 , The Arizona Board of Regents
	on behalf of Arizona State University
	
	All rights reserved.

	Developed by the Decision Center for a Desert City
	Lead Model Development - David Sampson <dasamps1@asu.edu>

	This program is free software: you can redistribute it and/or modify
	it under the terms of the GNU General Public License version 3 as published by
	the Free Software Foundation.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program.  If not, see <http://www.gnu.org/licenses/>.
  
	Three Control classes designed to be used with the WaterSim and WaterSimDB classes.
  
	WaterSim_Base_InputTextBox,  WaterSim_Provider_InputTextBox, WaterSim_InputPanel

 * Version 3

 * Keeper Quay
 * 

 *******************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace WaterSimDCDC.Controls
{
	//=================================================
	/// <summary>   Delegate for handling OnModelParameterSet events. </summary>
	/// <param name="source">  The WaterSim_InputTextBox issuing event. </param>
	/// <remarks> OnModelParameterSet events are issued when the textbox sets a model parameter.  </remarks>
	
	
	public delegate void OnValueChangeEventHandler (WaterSim_InputTextBox source);
   
	//class ValueChange : EventArgs
	//{
	//    public ValueChange() : base() {}
	//    public 
	//}
	//=================================================

	///-------------------------------------------------------------------------------------------------
	/// <summary> Water simulation input text box.  </summary>
	///
	/// <remarks> Base class for input textbox connect to a parameter manager </remarks>
	///-------------------------------------------------------------------------------------------------

	public abstract class  WaterSim_InputTextBox : TextBox
	{
		protected  bool FStoreLocal = true;
		protected bool FFetchLocal = true;
		protected  int FValue = 0;  // This is the TextBox maintained internal value, must call SetValue() to actually set Model Parameter Value
        protected  ModelParameterClass Fmp; // 7/29 BaseModelParameterClass Fmp;
		protected  bool _isMPSet = false;
		protected bool _MPSet = true;
		protected bool _MPGet = true;
		protected bool FHasChanged = false;


		protected OnValueChangeEventHandler _OnValueChange = null;

		///-------------------------------------------------------------------------------------------------
		/// <summary> Gets or sets the OnValueChange event handler </summary>
		///
		/// <value> The onValueChange event handler </value>
		///-------------------------------------------------------------------------------------------------

		public OnValueChangeEventHandler OnValueChange 
		{ get { return _OnValueChange; } set { _OnValueChange = value; }}
				//----------------------------------------------------------------------

		/// <summary>   Default constructor. </summary>
		/// <remarks> Values stored locally, no error checking, no events</remarks>
		public WaterSim_InputTextBox() : base ()  
		{
			this.KeyDown += new KeyEventHandler(InputTextBox_KeyDown);
			this.Leave += new EventHandler(InputTextBox_Leave);
		}
		//----------------------------------------------------------------------

		/// <summary>   Constructor. </summary>
		/// <param name="aModelParameter">  a ModelParameter. </param>
		/// <param name="SoreLocal">        true to store value local, false to store in Model Parameter. </param>
		/// <remarks> Storing local does not access model during user input, only on initialization or on a reload cal or event.  Code must fetch the data at set the model parameters.  
		/// 		           Storing in model access the model and sets the model parameter each time a value is changed.</remarks>
        public WaterSim_InputTextBox(ModelParameterClass aModelParameter, bool StoreValueLocal, bool GetValueLocal)   // BaseModelParameterClass aModelParameter, bool StoreValueLocal, bool GetValueLocal)
			: this()
		{
			FStoreLocal = StoreValueLocal;
			FFetchLocal = GetValueLocal;
			_setmp(aModelParameter);
			//this.KeyDown += new KeyEventHandler(InputTextBox_KeyDown);
			//this.Leave += new EventHandler(InputTextBox_Leave);
		   GetModelValue();
		}
	   //---------------------------------------------------------
        abstract protected bool _setmp(ModelParameterClass mp);  // 7/29 BaseModelParameterClass mp);
	   //---------------------------------------------------------

		/// <summary>   Gets or sets the ModelParameter. </summary>
		///
		/// <value> The model parameter. </value>
        public ModelParameterClass ModelParameter     // 7/29 BaseModelParameterClass ModelParameter
		{
			get { return Fmp;  }
			set { _setmp(value); }
		}
		//----------------------------------------------------------------
		/// <summary>
		/// Store Values Local 
		/// </summary>
		/// <value>True if values are stored local (default), false ModelParameters are set durectly by the control</value>
		/// <remarks>ModelPArameters can be set directly by this control, or indirectly outside of the control.  If they are not stored local (false) then
		/// the control sets them directly in the Model Parameter.  if the values are stored local (true default) the user must get the data from 
		/// the input panel and then set the ModelParameter.  In either case the control uses the model parameter to do range checking,
		/// Useful for creating and editing data that is not intended for imeadiate model use.</remarks>
		public bool StoreLocal
		{ get { return FStoreLocal; } set { FStoreLocal= value; } }
		//----------------------------------------------------------------
		/// <summary>
		/// Get Values from Local Storage
		/// </summary>
		/// <value>True if values are obtained from local data (default), false data is retrieved from the ModelParameters directly by the control</value>
		/// <remarks>Values for this control can be retrieved directly from ModelPArameters(false) or indirectly outside of the control (true default) by the user 
		/// by getting the data from ModelParameter and storing it in the control. 
		/// the input panel and then set the ModelParameter.  </remarks>
		public bool GetLocal
		{ get { return FFetchLocal; } set { FFetchLocal = value; } }

		public bool ValueHasChanged
		{
			get { return FHasChanged; }
		}

		///-------------------------------------------------------------------------------------------------
		/// <summary>   Event Handler for KeyDown. </summary>
		///
		/// <remarks>   Mcquay, 11/28/2011. </remarks>
		///
		/// <param name="sender">   Object triggering event. </param>
		/// <param name="e">        Event data. </param>
		///----------------------------------------------------------------------------------------------
		protected virtual void InputTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{ 
				int value = 0;
				string temp = "";
				temp = this.Text;
				try
				{
					value = Convert.ToInt16(temp);
				}
				catch
				{
					value = 0;
				}
				if (value != FValue)
				{
					SaveValue(value);
					FHasChanged = true;
				}
			}
		}
	 //------------------------------
		protected virtual void InputTextBox_Leave(object sender, EventArgs e)
		{
			int value = 0;
			string temp = "";
			temp = this.Text;
			try
			{
				value = Convert.ToInt16(temp);
			}
			catch 
			{
				value = 0;
			}
			if (value != FValue)
			{
				SaveValue(value);
				FHasChanged = true;

			}
		}
		//---------------------------------------
		abstract protected void SetValue(int value);
		//---------------------------------------
		abstract protected bool ValueInRange(int value);
		//------------------------
		abstract protected void SetModelValue();
		// Sets the actual model parameter value(s)
		//------------------------
		abstract protected void GetModelValue();
		// Gets the actual model value(s)
		//------------------------------------------------
		  protected void SaveValue(int value)
		  {
			  if (Fmp!=null)
			  {
				  if (Fmp.ShouldCheckRange())
				  {
					  if (ValueInRange(value))  // this will evoke the error dialog
					  {
						  SetValue(value);
						  if (_OnValueChange!=null)
						  {
							  _OnValueChange(this);
                              FHasChanged = false;
						  }
					  }
					  else
					  {
 	    			    this.Text = FValue.ToString();
    					this.Focus();
					  }
				  }
				  else
				  {
					  SetValue(value);
				  }
			  }
		  }
		//-----------------------------------------------

		  /// <summary> Reload event handler for ReloadEvents, reloads the parameter from the model. </summary>
		  ///
		  /// <remarks>  This is an event handler to respond to Reload events evoked by the owner of the control </remarks>
		  ///
		  /// <param name="source"> Source ModelParamter for the event. </param>
		  /// <param name="e">      Event information. </param>
		  public virtual void ReloadEventHandler(object source, System.EventArgs e)
		  {
			  if (e is ModelParameterEventArgs)
			  {
				  ReloadModelValue();
			  }
		  }
		//-----------------------------------------------------------------

		/// <summary>   Reloads the parameter value from the current model value. </summary>
		///
		/// <remarks>   </remarks>

          public virtual void ReloadModelValue()
          {
              if (!FHasChanged)
              {
                  GetModelValue();
              }
              else
              {
                  FHasChanged = false;
              }
          }
		//---------------------------------------------------------------------

		/// <summary>   Gets or sets the Parameter value saved by the text box. </summary>
		///
		/// <value> The current int value . </value>
		//-------------------------------------------------------
		public int Value
		{
			get { return FValue; }
			set { SetValue(value); }

		}
	
	}
	//=================================================

	///-------------------------------------------------------------------------------------------------
	/// <summary> Water simulation base input text box.  </summary>
	///
	/// <remarks> This is a text box input control that connects to a modelParamtype.mptInputBase ModelParameterClass parameter and manages the entry edit of the models value in real time. </remarks>
	///-------------------------------------------------------------------------------------------------

	public class WaterSim_Base_InputTextBox : WaterSim_InputTextBox    
	{

		//------------------------
		/// <summary>   Default constructor. </summary>
		/// <remarks> Values stored locally, no error checking, no events</remarks>
 
		public WaterSim_Base_InputTextBox() : base() { }
		//------------------------

		/// <summary>   Constructor. </summary>
		/// <param name="aModelParameter">  a model parameter. </param>
		/// <param name="StoreLocal">       true to store local. </param>
		/// <remarks> Storing local does not access model during user input, only on initialization or on a reload cal or event.  Code must fetch the data at set the model parameters.  
		/// 		           Storing in model access the model and sets the model parameter each time a value is changed.</remarks>
		/// <exception cref="WaterSim_Exception"> if aModelParameter is not modelParamtype.mptInputBase</exception>
        public WaterSim_Base_InputTextBox(ModelParameterClass aModelParameter, bool StoreDataLocal)    // 7/29 BaseModelParameterClass aModelParameter, bool StoreDataLocal)
			: base(aModelParameter, StoreDataLocal, StoreDataLocal)
		{
			
		}
		//------------------------
		// Protected routine that links the control to a model parameter.
        protected override bool _setmp(ModelParameterClass mp)   // 7/29 BaseModelParameterClass mp)
		{
			if (mp!=null)
			{
				if (mp.ParamType != modelParamtype.mptInputBase) throw new  Exception("Not InputBase");
				else
				{
					Fmp = mp;
					_isMPSet = true;
					GetModelValue();
					 Fmp.ReloadEvent += ReloadEventHandler;
				}
			}
			return _isMPSet;
		}
		//------------------------
		//  Sets the internal Value, the text box value, and the model value
		protected override void SetValue(int value)
		{
			FValue = value;
			Text = FValue.ToString();
			SetModelValue();
		}
		//------------------------
		// fetches the model value from the ModelParametreClass, sets the local field and sets the text box
		protected override void GetModelValue()
		{
			if ((Fmp != null) & (!FFetchLocal))
			{
               
					FValue = Fmp.Value;
					this.Text = FValue.ToString();
			}
		}
		//------------------------
		// Sets the model via the ModelParameterClass to the internal field value
		protected override void SetModelValue()
		{
			if ( (Fmp!=null)&(!FStoreLocal) )
			{
				Fmp.Value = FValue;
			}
		}
		//------------------------
		// Ivokes the ModelParameterClass range check, displays message if there is and error
		protected override bool ValueInRange(int value)
		{
			bool test = false;
			if (Fmp != null)
			{
				string errMessage = "";
				test = Fmp.isBaseValueInRange(value, ref errMessage);
				if (!test) MessageBox.Show(errMessage);
			}
			return test;
		}
		//-------------------------------------------------------
		//-------------------------------------------------------
	}
	//=================================================

	///-------------------------------------------------------------------------------------------------
	/// <summary> Water simulation provider input text box.  </summary>
	///
	/// <remarks> This is a text box input control that connects to a modelParamtype.mptInputProvider ModelParameterClass parameter and manages the entry edit of the models value in real time.  
	/// 		  works with a specific eProvider index into the ProviderPropertyArray, which can be changed.</remarks>
	///-------------------------------------------------------------------------------------------------

	public class WaterSim_Provider_InputTextBox : WaterSim_InputTextBox
	{
		protected ProviderIntArray FValues = new ProviderIntArray(0);
		protected eProvider Fprovider = (eProvider)0;
		protected bool FFillAll = false;
		//----------------------------------------------------------------------
	//------------------------
		/// <summary>   Default constructor. </summary>
		/// <remarks> Values stored locally, no error checking, no events</remarks>
 
		public WaterSim_Provider_InputTextBox() : base() { }
		//----------------------------------------------------------------------

		///-------------------------------------------------------------------------------------------------
		/// <summary> Constructor </summary>
		///
		/// <remarks> </remarks>
		///
		/// <param name="aModelParameter"> a model parameter. </param>
		/// <param name="StoreDataLocal">  true to store data local. </param>
		/// <param name="p">            The initial eProvider index for this control</param>
		///-------------------------------------------------------------------------------------------------

		public WaterSim_Provider_InputTextBox(ModelParameterClass aModelParameter, bool StoreDataLocal, eProvider p)
			: base(aModelParameter, StoreDataLocal, StoreDataLocal)
		{
			Fprovider = p;
			GetModelValues();
		}
		//------------------------
        protected override bool _setmp(ModelParameterClass mp)     /// 7/29 BaseModelParameterClass mp)
		{
			if (mp != null)
			{
				if (mp.ParamType != modelParamtype.mptInputProvider) throw new Exception("WS_Strings.wsModelParamMUstBeInputProvider");
				else
				{
					Fmp = mp;
					_isMPSet = true;
					GetModelValue();
					Fmp.ReloadEvent += ReloadEventHandler;
				}
			}
			return _isMPSet;
		}
		//------------------------
		protected override void SetValue(int value)
		{
			FValue = value;
			FValues[Fprovider] = value;
			Text = FValue.ToString();
			SetModelValue();
		}
		//------------------------
		protected override void GetModelValue()
		{
            //if ((Fmp != null)&(!FFetchLocal))
            //{
            //    ProviderIntArray temp = Fmp.ProviderProperty.getvalues();
            //    FValue = temp[Fprovider];
            //    this.Text = FValue.ToString();
            //}
            GetModelValues();
		}
		//------------------------
		protected void GetModelValues()
		{
			if ((Fmp != null) & (!FFetchLocal))
			{
				FValues = Fmp.ProviderProperty.getvalues();
				FValue = FValues[ProviderClass.index(Fprovider)];
				this.Text = FValue.ToString();
			}
		}
		//------------------------
		protected override void SetModelValue()
		{
			if ( (Fmp != null)&(!FStoreLocal))   Fmp.ProviderProperty[ Fprovider] = FValue;
		}
		//------------------------
		protected void SetModelValues()
		{
			if ((Fmp != null)&(!FStoreLocal)) Fmp.ProviderProperty.setvalues(FValues);
		}
		//--------------------------------------------------------------------
		public void checkBoxCheckedChanged(object sender, EventArgs e)
		{
			// TO DO Add avlue checlk
			if (sender is CheckBox)
			{
				FFillAll = (sender as CheckBox).Checked;
				if (FFillAll)
				{ 
					for(int i=0;i<FValues.Values.Length;i++) FValues.Values[i] = FValue;
				 }
				else
				{ GetModelValue(); }
			}
		}
		//--------------------------------------------------------------------

		///-------------------------------------------------------------------------------------------------
		/// <summary> Sets all button clicked. </summary>
		///
		/// <remarks> This is an event handler that responds when the all button is clicked.  Sets all the values of the local and model ProviderPropertyArray to teh value in the Text field</remarks>
		///
		/// <param name="sender"> Source of the event. </param>
		/// <param name="e">      Event information. </param>
		///-------------------------------------------------------------------------------------------------

		public void SetAllButtonClicked(object sender, EventArgs e)
		{
			// TO DO Add avlue checlk
			if (Fmp != null)
			{
				string errMessage = "";
				string temp = "";
				bool test = false;
				bool founderror = false;
			   
				for (int i = 0; i < FValues.Length; i++)
				{
					temp = "";
					test = Fmp.isProviderValueInRange(FValue, (eProvider)i, ref temp);
					if (test)
					{
						FValues[i] = FValue;
					}
					else
					{
						founderror = true;
						errMessage += "[" + temp +"]";
					}
				}
				if (founderror)
				{
					MessageBox.Show("Warning range errors, value=" +FValue.ToString()+" not set for "+  errMessage);
				}
				else
					SetModelValues();
			}
		}
		//-------------------------------------------------------------

		///-------------------------------------------------------------------------------------------------
		/// <summary> Gets or sets the model eProvider index into the ProviderPropertyArray. </summary>
		///
		/// <value> eProvider index </value>
		///-------------------------------------------------------------------------------------------------

		public eProvider ModelParameterProvider
		{
			get { return Fprovider; }
			set
			{
				Fprovider = value;
				FValue = FValues[ProviderClass.index(Fprovider)];
				this.Text = FValue.ToString();
			}
		}
		//------------------------
		protected override bool ValueInRange(int value)
		{
			bool test = true;
			string errMessage = "";
			if (Fmp!=null) test = Fmp.isProviderValueInRange(value, Fprovider, ref errMessage);
			if (!test) MessageBox.Show(errMessage);
			return test;
		}
		//------------------------

		///-------------------------------------------------------------------------------------------------
		/// <summary> Gets the local ProviderIntArray. </summary>
		///
		/// <remarks> </remarks>
		///
		/// <returns> The values. </returns>
		///-------------------------------------------------------------------------------------------------

		public ProviderIntArray GetValues()
		{ return FValues; }
		//------------------------

		///-------------------------------------------------------------------------------------------------
		/// <summary> Sets the values.of the local ProviderIntArray </summary>
		///
		/// <remarks> </remarks>
		///
		/// <param name="values"> The values. </param>
		///-------------------------------------------------------------------------------------------------

		public void SetValues(ProviderIntArray values)
		{
            // Set the values
            FValues = values;
            // Now update display and current value
            FValue = FValues[Fprovider];
            this.Text = FValue.ToString();
           // Ok set the model values first
            SetModelValues();
        }
	}
	//==================================================================
	/**************************************************************************************************
	 *  WaterSim Input Panel
	 *  Creates a Panel that connects to a WaterSim instance, can be used to input data before 
	 *  a Simulation is run.  Given WaterSim WS, and WaterSim_InputPanel WSP
	 *      WS.initializeSimulation();
	 *      WSP.SetAllModelValues();
	 *      WS.SimulationAllYears();
	 *      WS.stopSimulation()
	 * 
	 * 
	 * 
	 * *************************************************************************************************/

	///-------------------------------------------------------------------------------------------------
	/// <summary> Panel for editing the WaterSim model input values.  </summary>
	///
	/// <remarks> A Panel that will display an input textbxx all the base and property ModelParameters</remarks>
	///-------------------------------------------------------------------------------------------------

	public class WaterSim_InputPanel : Panel
	{
		const int inputTextBoxWidth = 50;
		const int inputTextBoxHeight = 20;

		bool FStoreLocal = true;
		bool FFetchLocal = true;


		/// <summary> The inputtable layout panel, were all the base input controls are placed</summary>
		public TableLayoutPanel InputtableLayoutPanel;

		/// <summary> The providertable layout panel, where all the provider input controls are placed </summary>
		public TableLayoutPanel ProvidertableLayoutPanel;
		
		ComboBox ProviderComboBox;
		
		List<WaterSim_Base_InputTextBox> BaseInputList = new List<WaterSim_Base_InputTextBox>();
		List<WaterSim_Provider_InputTextBox> ProviderInputList = new List<WaterSim_Provider_InputTextBox>();
		//WaterSim WSim;
		ParameterManagerClass FParamManager;

		//---------------------------------

		///-------------------------------------------------------------------------------------------------
		/// <summary> Default constructor. </summary>
		///
		///-------------------------------------------------------------------------------------------------

		public WaterSim_InputPanel() : base() 
		{  }
		//---------------------------------

		///-------------------------------------------------------------------------------------------------
		/// <summary> Constructor. </summary>
		///
		/// <remarks> Creates and input panel with defined size and placement and inserts into a specified control.  Links to a WaterSWimManager object </remarks>
		///
		/// <param name="WS">     The WaterSimManager. </param>
		/// <param name="width">  The width. </param>
		/// <param name="height"> The height. </param>
		/// <param name="top">    The top. </param>
		/// <param name="left">   The left. </param>
		/// <param name="Owner">  The owner. </param>
		///-------------------------------------------------------------------------------------------------

		public WaterSim_InputPanel(WaterSimManager WS,int width, int height, int top, int left, Control Owner)
			: base()
		{
			FParamManager = WS.ParamManager;
			LoadControls(width, height, top, left,Owner);
			SizeChanged += InputPanel_ClientSizeChanged;
			//ClientSizeChanged += InputPanel_ClientSizeChanged;
		}
		//---------------------------------

		///-------------------------------------------------------------------------------------------------
		/// <summary> Constructor. </summary>
		///
		/// <remarks> Creates and input panel with defined size and placement and inserts into a specified control.  Links to a ParameterManagerClass object  </remarks>
		///
		/// <param name="PM">     The ParameterManager. </param>
		/// <param name="width">  The width. </param>
		/// <param name="height"> The height. </param>
		/// <param name="top">    The top. </param>
		/// <param name="left">   The left. </param>
		/// <param name="Owner">  The owner. </param>
		///-------------------------------------------------------------------------------------------------

		public WaterSim_InputPanel(ParameterManagerClass PM, int width, int height, int top, int left, Control Owner)
			: base()
		{
			FParamManager = PM;
			LoadControls(width, height, top, left, Owner);
			//ClientSizeChanged += InputPanel_ClientSizeChanged;
			SizeChanged += InputPanel_ClientSizeChanged;
		}
		//---------------------------------

		///-------------------------------------------------------------------------------------------------
		/// <summary> Gets or sets the ParameterManagerClass object the control is linked to. </summary>
		///
		/// <value> The parameter manager. </value>
		///-------------------------------------------------------------------------------------------------

		public ParameterManagerClass ParamManager
		{
			get {return FParamManager; } 
			set 
			{
				FParamManager = value;
				
				LoadControls(this.Width, this.Height, this.Top, this.Left, this.Parent);
			}
			
		}
		//---------------------------------

        public void MyParameterManagerChangeEventHandler(object sender, ParameterManagerEventArgs e)
        {
            // if adding or deleting a process, update process list
            if ((e.TheEventType == ParameterManagerEventType.pmeAdd) || (e.TheEventType == ParameterManagerEventType.pmeDelete))
            {
                // What?
            }
        }
		///-------------------------------------------------------------------------------------------------
		/// <summary> Links the control to a WaterSimManager obejct. </summary>
		///
		/// <value> The WaterSimManager </value>
		///-------------------------------------------------------------------------------------------------

		protected WaterSimManager WaterSimModel 
		{
			set
			{
				if(value!=null)
				{
					FParamManager = value.ParamManager;
					
					LoadControls(this.Width, this.Height, this.Top, this.Left, this.Parent);
				}
			 }
		} 
		//------------------------------------------------------------

        public void OnValueChangeHandler(WaterSim_InputTextBox source)
        {
            this.Refresh_Inputs();
        }
		///-------------------------------------------------------------------------------------------------
		/// <summary> Gets or sets a value indicating whether all the input panels child inputTextBox controls should store values local. </summary>
		///
		/// <value> true if store local, false if not. </value>
		///-------------------------------------------------------------------------------------------------

		public bool StoreLocal
		{ get { return FStoreLocal; }
		  set
			{
				FStoreLocal = value;
			  // Now see if controls have already been created
				if (this.Controls.Count > 0)
				{
					foreach (Control CT in this.Controls)
					{
						if (CT is  WaterSim_InputTextBox)
						{
							WaterSim_InputTextBox temp = CT as WaterSim_InputTextBox;
							temp.StoreLocal = FStoreLocal;

						}

					}

				}
			}
		}
		//------------------------------------------------------------
        public bool FindModelParameter(int emp, ref WaterSim_Base_InputTextBox BIC)
        {
            bool found = false;
            foreach (Control ctrl in InputtableLayoutPanel.Controls)
                if (ctrl is WaterSim_Base_InputTextBox)
                    if ((ctrl as WaterSim_Base_InputTextBox).ModelParameter.ModelParam == emp)
                    {
                        found = true;
                        BIC = (ctrl as WaterSim_Base_InputTextBox);
                        break;
                    }
            return found;
        }
        //------------------------------------------------------------
        public bool FindModelParameter(int emp, ref WaterSim_Provider_InputTextBox BIC)
        {
            bool found = false;
            foreach (Control ctrl in ProvidertableLayoutPanel.Controls)
                if (ctrl is WaterSim_Provider_InputTextBox)
                    if ((ctrl as WaterSim_Provider_InputTextBox).ModelParameter.ModelParam == emp)
                    {
                        found = true;
                        BIC = (ctrl as WaterSim_Provider_InputTextBox);
                        break;
                    }
            return found;
        }
        //------------------------------------------------------------

		///-------------------------------------------------------------------------------------------------
		/// <summary>   Rturns a bool value indicating whether the value of any ofthe input textboxes has been changed. </summary>
		///
		/// <value> true if a value has changed, false if not. </value>
		///-------------------------------------------------------------------------------------------------

		public bool ValueHasChanged
		{ 
			get {
				bool found = false;
				if (this.Controls.Count > 0)
				{
					foreach (Control CT in this.Controls)
					{
						if (CT is WaterSim_InputTextBox)
						{
							if ((CT as WaterSim_InputTextBox).ValueHasChanged == true)
								found = true;
						 
						}
					}
				}
				return found;
			}
		}

		//--------------------------------------------------------------
		// BOTH OF THESE DO NOT SEEM NEEDED, ONE SHOULD GO, PERHAPS THIS ONE.

		public bool GetLocal
		{
			get { return FFetchLocal; }
			set
			{
				FFetchLocal = value;
				// Now see if controls have already been created
				if (this.Controls.Count > 0)
				{
					foreach (Control CT in this.Controls)
					{
						if (CT is WaterSim_InputTextBox)
						{
							WaterSim_InputTextBox temp = CT as WaterSim_InputTextBox;
							temp.GetLocal = FStoreLocal;
						}
					}
				}
			}
		}

		//--------------------------------------------------------------
		private void InputPanel_ClientSizeChanged(object sender, EventArgs e)
		{
			SuspendLayout();
			int InputPanelWidth = ((this.Width / 10) *4)-5;
			int ProviderPanelWidth =( (this.Width / 10) * 6)-5;
			int InputLabelWidth =(InputPanelWidth /10)*7;
			int ProviderLabelWidth = (ProviderPanelWidth / 10) * 7;
			ProvidertableLayoutPanel.Left = InputPanelWidth+5;
			InputtableLayoutPanel.Width = InputPanelWidth;
			InputtableLayoutPanel.ColumnStyles[0].Width = InputLabelWidth;
			ProvidertableLayoutPanel.Width = ProviderPanelWidth;
			ResumeLayout();
		}

 
		//--------------------------------------------------------------
		internal void LoadControls(int width, int height, int top, int left, Control Owner)
		{
			if (FParamManager != null)
			{
				
				Label InputLabel;
				ModelParameterBaseClass mp;
				WaterSim_Base_InputTextBox biTextBox;
				WaterSim_Provider_InputTextBox piTextBox;

				Button setallbutton;

				int Row = 0;
				int ipheight, ipwidth, tlpheight, tlpwidth;
				int ipTop = top;
				int ipLeft = left;
				int tlpTop = 40;
				int HeaderLabelTop, HeaderSize;



				ipheight = height;
				ipwidth = width;
				tlpheight = ipheight - tlpTop - 15;
				tlpwidth = (ipwidth / 2) - 25;

	  
				this.Top = ipTop;
				this.Left = ipLeft;
				this.Size = new Size(ipwidth, ipheight);
				this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
				if (this.Anchor==AnchorStyles.None)         this.Anchor = (AnchorStyles.Bottom | AnchorStyles.Top);

				// Owner.Controls.Add(this);

				HeaderSize = Convert.ToInt16(Owner.Font.SizeInPoints * 5) + 2;
				HeaderLabelTop = (HeaderSize / 2) + 2;
				tlpTop = HeaderSize;


				// Add providerbox
				InputLabel = new Label();
				InputLabel.AutoSize = true;
				InputLabel.Top = 4;
				InputLabel.Left = (ipwidth / 2) + 3;
				InputLabel.Text = "Provider ";
				InputLabel.Font = new Font(this.Font, System.Drawing.FontStyle.Bold);
				this.Controls.Add(InputLabel);

				ProviderComboBox = new ComboBox();
				ProviderComboBox.Top = 2;
				ProviderComboBox.Left = InputLabel.Right + 2;
				ProviderComboBox.Width = (ipwidth / 2) - (InputLabel.Width + 8);
				foreach (eProvider p in ProviderClass.providers())
					ProviderComboBox.Items.Add(ProviderClass.Label(p));
				ProviderComboBox.SelectedIndex = 0;
				ProviderComboBox.SelectedIndexChanged += ProviderComboBox_SelectedIndexChanged;

				this.Controls.Add(ProviderComboBox);

				// Create Base Input Table
				InputtableLayoutPanel = new TableLayoutPanel();
				InputtableLayoutPanel.Size = new Size(tlpwidth, tlpheight);
				InputtableLayoutPanel.Top = tlpTop;
				InputtableLayoutPanel.Left = 1;
				InputtableLayoutPanel.ColumnCount = 2;
			  
				InputtableLayoutPanel.RowCount = FParamManager.NumberOfParameters(modelParamtype.mptInputBase);
				InputtableLayoutPanel.AutoScroll = true;
				InputtableLayoutPanel.Anchor = (AnchorStyles.Bottom | AnchorStyles.Top); // | AnchorStyles.Left);
				ColumnStyle style = new ColumnStyle();
				style.SizeType = SizeType.Percent;
				style.Width = 0.80F;
				InputtableLayoutPanel.ColumnStyles.Add(style);
				style = new ColumnStyle();
				style.SizeType = SizeType.Percent;
				style.Width = 0.20F;
				InputtableLayoutPanel.ColumnStyles.Add(style);
				this.Controls.Add(InputtableLayoutPanel);

				Row = 0;
				// Add the Base Inputs to it
				foreach (int p in FParamManager.eModelParameters())
				{
					//InputtableLayoutPanel
					mp = FParamManager.Model_Parameter(p);
					if (mp.ParamType == modelParamtype.mptInputBase)
					{
						InputLabel = new Label();
						InputLabel.AutoSize = true;
						InputLabel.Text = mp.Label;
						InputtableLayoutPanel.Controls.Add(InputLabel);
						InputtableLayoutPanel.SetCellPosition(InputLabel, new TableLayoutPanelCellPosition(0, Row));

						biTextBox = new WaterSim_Base_InputTextBox((mp as ModelParameterClass), false);  // 7/29 (mp as BaseModelParameterClass), false);
						biTextBox.Size = new Size(inputTextBoxWidth, inputTextBoxHeight);
                        biTextBox.OnValueChange = OnValueChangeHandler; 
						InputtableLayoutPanel.Controls.Add(biTextBox);
						InputtableLayoutPanel.SetColumn(biTextBox, 1);
						InputtableLayoutPanel.SetRow(biTextBox, Row);
						BaseInputList.Add(biTextBox);

						Row++;
					}
				}
				//OK now put a label down for columns

				float ColWidth = InputtableLayoutPanel.ColumnStyles[0].Width * InputtableLayoutPanel.Width;

				InputLabel = new Label();
				InputLabel.AutoSize = true;
				InputLabel.Top = HeaderLabelTop;
				InputLabel.Left = 0;
				InputLabel.Text = "Base Simulation Parameters";
				InputLabel.Font = new Font(Font, System.Drawing.FontStyle.Bold);
				this.Controls.Add(InputLabel);

				InputLabel = new Label();
				InputLabel.AutoSize = true;
				InputLabel.Top = HeaderLabelTop;
				InputLabel.Left = Convert.ToInt16(ColWidth);
				InputLabel.Text = "Value";
				InputLabel.Font = new Font(Font, System.Drawing.FontStyle.Bold);
				this.Controls.Add(InputLabel);

				//-------------------------------------------
				// Create the Provider Table
				ProvidertableLayoutPanel = new TableLayoutPanel();
				ProvidertableLayoutPanel.Size = new Size(tlpwidth + 20, tlpheight);
				ProvidertableLayoutPanel.Top = tlpTop;
				ProvidertableLayoutPanel.Left = InputtableLayoutPanel.Left + InputtableLayoutPanel.Width + 4;
				ProvidertableLayoutPanel.ColumnCount = 3;
				ProvidertableLayoutPanel.AutoScroll = true;
				ProvidertableLayoutPanel.Anchor = (AnchorStyles.Bottom | AnchorStyles.Top );//| AnchorStyles.Right);
				ProvidertableLayoutPanel.RowCount = FParamManager.NumberOfParameters(modelParamtype.mptInputBase);

				style = new ColumnStyle();
				style.SizeType = SizeType.Percent;
				style.Width = 0.70F;
				ProvidertableLayoutPanel.ColumnStyles.Add(style);
				style = new ColumnStyle();
				style.SizeType = SizeType.Percent;
				style.Width = 0.20F;
				ProvidertableLayoutPanel.ColumnStyles.Add(style);
				style = new ColumnStyle();
				style.SizeType = SizeType.Percent;
				style.Width = 0.10F;
				ProvidertableLayoutPanel.ColumnStyles.Add(style);

				this.Controls.Add(ProvidertableLayoutPanel);

				// Add the Provider Inouts to it
				Row = 1;
				foreach (int p in FParamManager.eModelParameters())
				{
					InputLabel = new Label();
					InputLabel.AutoSize = true;


					//InputtableLayoutPanel
					mp = FParamManager.Model_Parameter(p);
					if ( (mp.ParamType == modelParamtype.mptInputProvider)) // && (mp.GetType()==typeof(ModelParameterClass)) )
					{
						InputLabel = new Label();
						InputLabel.AutoSize = true;
						InputLabel.Text = mp.Label;
						ProvidertableLayoutPanel.Controls.Add(InputLabel);
						ProvidertableLayoutPanel.SetColumn(InputLabel, 0);
						ProvidertableLayoutPanel.SetRow(InputLabel, Row);

						piTextBox = new WaterSim_Provider_InputTextBox((mp as ModelParameterClass), false, ProviderClass.provider(0));
						piTextBox.Size = new Size(inputTextBoxWidth, inputTextBoxHeight);
                        piTextBox.OnValueChange = OnValueChangeHandler;
						ProvidertableLayoutPanel.Controls.Add(piTextBox);
						ProvidertableLayoutPanel.SetColumn(piTextBox, 1);
						ProvidertableLayoutPanel.SetRow(piTextBox, Row);
						ProviderInputList.Add(piTextBox);

						//SetAllButtonClicked
						setallbutton = new Button();
						setallbutton.Font = new Font(Font.FontFamily, 8.0F, System.Drawing.FontStyle.Bold);
						setallbutton.Text = "All Set";
						setallbutton.AutoSize = true;
						setallbutton.Click += piTextBox.SetAllButtonClicked;
						ProvidertableLayoutPanel.Controls.Add(setallbutton);
						ProvidertableLayoutPanel.SetColumn(setallbutton, 2);
						ProvidertableLayoutPanel.SetRow(setallbutton, Row);

						//chckbox = new CheckBox();
						//chckbox.Text = "";
						//chckbox.CheckedChanged += piTextBox.checkBoxCheckedChanged;
						//ProvidertableLayoutPanel.Controls.Add(chckbox);
						//ProvidertableLayoutPanel.SetColumn(chckbox, 2);
						//ProvidertableLayoutPanel.SetRow(chckbox, Row);

						Row++;
					}


					InputLabel = new Label();
					InputLabel.AutoSize = true;
					InputLabel.Top = HeaderLabelTop;
					InputLabel.Left = ProvidertableLayoutPanel.Left;
					InputLabel.Text = "Provider Simulation Parameters";
					InputLabel.Font = new Font(Font, System.Drawing.FontStyle.Bold);
					this.Controls.Add(InputLabel);

					ColWidth = ProvidertableLayoutPanel.ColumnStyles[0].Width * ProvidertableLayoutPanel.Width;
					InputLabel = new Label();
					InputLabel.AutoSize = true;
					InputLabel.Top = HeaderLabelTop;
					InputLabel.Left = Convert.ToInt16(ColWidth) + ProvidertableLayoutPanel.Left - 8;
					InputLabel.Text = "Value";
					InputLabel.Font = new Font(Font, System.Drawing.FontStyle.Bold);
					this.Controls.Add(InputLabel);

					//                ColWidth += ProvidertableLayoutPanel.ColumnStyles[1].Width * ProvidertableLayoutPanel.Width;
					InputLabel = new Label();
					InputLabel.AutoSize = true;
					InputLabel.Top = HeaderLabelTop;
					InputLabel.Left = Convert.ToInt16(ColWidth) + ProvidertableLayoutPanel.Left + inputTextBoxWidth;
					InputLabel.Text = "Set  All";
					InputLabel.Font = new Font(Font, System.Drawing.FontStyle.Bold);
					this.Controls.Add(InputLabel);

					//InputLabel = new Label();
					//InputLabel.AutoSize = true;
					//InputLabel.Top = HeaderLabelTop;
					//InputLabel.Left = (ipwidth / 2) + 3;
					//InputLabel.Text = "Provider Simulation Parameters                      Value    SetAll";
					//InputLabel.Font = new Font(Font, System.Drawing.FontStyle.Bold);
					//this.Controls.Add(InputLabel);
				}
			}
		}

		//-------------------------------------------------------------------------------------------------------
		private void ProviderComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			eProvider pSelected = ProviderClass.provider(ProviderComboBox.SelectedIndex);
			foreach (WaterSim_Provider_InputTextBox piTextBox in ProviderInputList)
				piTextBox.ModelParameterProvider = pSelected;

		}
		//-------------------------------------------------------------------------------------------------------

		///-------------------------------------------------------------------------------------------------
		/// <summary> Gets all values </summary>
		///
		/// <remarks> Retreives the values from the input panel in a SimulationInputs struct. </remarks>
		///
		/// <returns> all values. </returns>
		///-------------------------------------------------------------------------------------------------

		public SimulationInputs GetAllValues()
		{
			SimulationInputs si = FParamManager.NewSimulationInputs();
           
            // Load the Base Input parameters
            WaterSim_Base_InputTextBox bic = new WaterSim_Base_InputTextBox();
            WaterSim_Provider_InputTextBox pic = new WaterSim_Provider_InputTextBox();
            int index = 0;
            foreach (ModelParameterClass MPC in FParamManager.BaseInputs())
            {
                // Ok See if you can find a Base_InputTextBox supporting this ModelParameterClass
                if (FindModelParameter(MPC.ModelParam, ref bic))
                {
                    // found it, use it to set the SI value
                    si.BaseInput[index] = bic.Value;
                    si.BaseInputModelParam[index] = bic.ModelParameter.ModelParam;
                    index++;
                }
                else
                {
                   // Not found, So this parameter is not in the InputPanel then use the ModelParametersClass current (or default) value to set value
                    si.BaseInput[index] = MPC.Value;
                    si.BaseInputModelParam[index] = MPC.ModelParam;
                    index++;
                }
            }
            index = 0;

            foreach (ModelParameterClass MPC in FParamManager.ProviderInputs())
            {
                // Ok See if you can find a Provider_InputTextBox supporting this ModelParameterClass
                if (FindModelParameter(MPC.ModelParam, ref pic))
                {
                    // found it, use it to set SI
                    si.ProviderInput.Values[index] = pic.GetValues();
                    si.ProviderInputModelParam[index] = pic.ModelParameter.ModelParam;
                    index++;
                }
                else
                {
                    // Not found, So this parameter is not in the InputPanel then use the ModelParametersClass current (or default) value to set value
                    si.ProviderInput.Values[index] = MPC.ProviderProperty.getvalues();
                    si.ProviderInputModelParam[index] = MPC.ModelParam;
                    index++;

                }
            }
               
           // int cnt = InputtableLayoutPanel.Controls.Count;
           //int index = 0;
           //for (int i = 0; i < cnt; i++)
           //{
           ////    if (InputtableLayoutPanel.Controls[i] is WaterSim_Base_InputTextBox)
           //  {
           //      WaterSim_Base_InputTextBox bic;
           //      bic = InputtableLayoutPanel.Controls[i] as WaterSim_Base_InputTextBox;
                 
           //      si.BaseInput[index] = bic.Value;
           //       si.BaseInputModelParam[index] = bic.ModelParameter.ModelParam;
           //        index++;
           //   }
           //}
           //cnt =ProvidertableLayoutPanel.Controls.Count;
		  
           //index = 0;
           //for (int i = 0; i < cnt; i++)
           //{
           //    if (ProvidertableLayoutPanel.Controls[i] is WaterSim_Provider_InputTextBox)
           //    {
           //        WaterSim_Provider_InputTextBox pic;
           //        pic = ProvidertableLayoutPanel.Controls[i] as WaterSim_Provider_InputTextBox;
           //        //pdata = si.ProviderInput[index];
           //        si.ProviderInput.Values[index] = pic.GetValues();
           //        si.ProviderInputModelParam[index] = pic.ModelParameter.ModelParam;
           //        //pdata.Values = pic.GetValues();
           //        index++;
           //    }
           //}
		   return si;
		}
		//-------------------------------------------------------------------------------------------------------

		///-------------------------------------------------------------------------------------------------
		/// <summary> Sets all value. </summary>
		///
		/// <remarks> Sets all values in the input panel from a SimulationInputs struct. </remarks>
		///
		/// <param name="si"> The Values </param>
		///-------------------------------------------------------------------------------------------------

		public void  SetAllValues(SimulationInputs si)
		{
            WaterSim_Base_InputTextBox bic = new WaterSim_Base_InputTextBox();
            WaterSim_Provider_InputTextBox pic = new WaterSim_Provider_InputTextBox();
            int index = 0;
            foreach (ModelParameterClass MPC in FParamManager.BaseInputs())
            {
                if (FindModelParameter(MPC.ModelParam, ref bic))
                {
                    index = si.BaseIndex(MPC.ModelParam);
                    if (index>=0)
                        bic.Value = si.BaseInput[index];
                }
            }
            index = 0;

            foreach (ModelParameterClass MPC in FParamManager.ProviderInputs())
            {
                if (FindModelParameter(MPC.ModelParam, ref pic))
                {
                    index = si.ProviderIndex(MPC.ModelParam);
                    if (index>=0)
                        pic.SetValues(si.ProviderInput[index]);
                }
            }
            
            
            //int cnt = InputtableLayoutPanel.Controls.Count;
            //int index = 0;
            //for (int i = 0; i < cnt; i++)
            //{
            //    if (InputtableLayoutPanel.Controls[i] is WaterSim_Base_InputTextBox)
            //    {
            //        WaterSim_Base_InputTextBox bic;
            //        bic = InputtableLayoutPanel.Controls[i] as WaterSim_Base_InputTextBox;
            //        bic.Value = si.BaseInput[index];
            //        index++;
            //    }
            //}
            //cnt = ProvidertableLayoutPanel.Controls.Count;
            //index = 0;
            //ProviderIntArray pdata;
            //for (int i = 0; i < cnt; i++)
            //{
            //    if (ProvidertableLayoutPanel.Controls[i] is WaterSim_Provider_InputTextBox)
            //    {
            //        WaterSim_Provider_InputTextBox pic;
            //        pic = ProvidertableLayoutPanel.Controls[i] as WaterSim_Provider_InputTextBox;
            //        pdata = si.ProviderInput[index];
            //        pic.SetValues(pdata);
            //    }
            //}
		}
		//-------------------------------------------------------------------------------------------------------

		///-------------------------------------------------------------------------------------------------
		/// <summary> Refresh inputs. </summary>
		///
		/// <remarks> Calls all the controls ReloadModelValue method, which reloads the controls value based on the StoreLocal value. </remarks>
		///-------------------------------------------------------------------------------------------------

		public void Refresh_Inputs()
		{
			int cnt = InputtableLayoutPanel.Controls.Count;
			for (int i = 0; i < cnt; i++)
			{
				if (InputtableLayoutPanel.Controls[i] is WaterSim_Base_InputTextBox)
				{
					WaterSim_Base_InputTextBox bic;
					bic = InputtableLayoutPanel.Controls[i] as WaterSim_Base_InputTextBox;
					bic.ReloadModelValue();
				}
			}
			cnt = ProvidertableLayoutPanel.Controls.Count;
			for (int i = 0; i < cnt; i++)
			{
				if (ProvidertableLayoutPanel.Controls[i] is WaterSim_Provider_InputTextBox)
				{
					WaterSim_Provider_InputTextBox pic;
					pic = ProvidertableLayoutPanel.Controls[i] as WaterSim_Provider_InputTextBox;
					pic.ReloadModelValue();
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------
	}


	internal class ModelParmeterClassConverter : System.ComponentModel.TypeConverter
	{
		public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
				return true;
			else
			   return base.CanConvertFrom(context, sourceType);
		}
		public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return true;
			}
			else
			   return base.CanConvertTo(context, destinationType);
		}
		public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if ((destinationType == typeof(string)) & (value is ModelParameterClass))
			{
				return value.ToString();
			}
			else
				return base.ConvertTo(context, culture, value, destinationType);
		}
		public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{   
			return base.ConvertFrom(context, culture, value);
		}

	}


}
