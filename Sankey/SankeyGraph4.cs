using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;


namespace ConsumerResourceModelFramework

{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Sankey graph. </summary>
    ///
    /// <seealso cref="System.Windows.Forms.UserControl"/>
    ///-------------------------------------------------------------------------------------------------

    public class SankeyGraph : UserControl
    {
        // The main panel for graphic drawing
        //Panel SKPanel;

        CRF_Network FCRFNetwork = null;

        // Ok create the Flowbar list
        public enum DrawOrder {doTopDown, doBottomUp};
        DrawOrder FDrawOrder = DrawOrder.doBottomUp;
        SankeyFlowBarList TheBarList = new SankeyFlowBarList(SankeyGraph.DrawOrder.doBottomUp);

        bool FSHowTool = false;

        // Used to convert from CRF values to Graphic Values
        double FScaleRatio = 1;

        // ------------------------------------
        // Display Parameters
        // ------------------------------------
        Color FNegativeColor = Color.Red;
        Color FPositiveColor = Color.YellowGreen;

        Color FLabelColor = Color.Black;
        Color FBorderLineColor = Color.Black;

        Color FNetworkBackground = Color.Black;

        Color FBoxLabelColor = Color.White;

        Color FFlowLabelBackground = Color.LightYellow;
        Color FFlowLableFontColor = Color.DarkGray;
        // CLEAN UP  Font FFlowLabelFont = null;
        
        int FBoxSpacer = 10;
        int FHeaderBoxWidth = 30;
        double FHeaderBoxWidthPCT = 0.2;

        const int MinToolWidth = 10;
        private ContextMenuStrip contextMenuStripPopUp;
        private IContainer components;
        private ToolStripMenuItem copyToolStripMenuItem;
        private SaveFileDialog saveFileDialogImage;
        int FToolOffset = 0;

        
        //--------------------------------------


        /// <summary>   Default constructor. 
        ///             For use when added with designer</summary>
        
        public SankeyGraph()
        {
            InitializeComponent();
            // setup tool bar
            ResetTool();
            //toolStripPopUp.BackColor = FNetworkBackground;
            // reset the grap
            ResetGraph();
            // set events
            Resize += _Resize;
            MouseClick += _MouseClick;
            
        }

         
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the CRF network. </summary>
        ///
        /// <value> The network. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Network Network
        {
            get 
            {
                return FCRFNetwork;
            }
            set
            {
                FCRFNetwork = value;
                if (value != null)
                {
                    FCRFNetwork.CallBackMethod = ResetGraph;
                    ResetGraph();
                }
            }
        }

        public Color NetworkBackground
        {
            get { return FNetworkBackground; }
            set 
            { 
                FNetworkBackground = value;
                ResetTool();
                ResetGraph();
            }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the flow bar draw order. </summary>
        ///
        /// <value> The flow bar draw order. </value>
        ///-------------------------------------------------------------------------------------------------

        public DrawOrder FlowBarDrawOrder
        {
            get { return FDrawOrder; }
            set { FDrawOrder = value; }
        }

        public Color PositiveColor
        {
            get { return FPositiveColor; }
            set { FPositiveColor = value; }
        }

        public Color NegativeColor
        {
            get { return FNegativeColor; }
            set { FNegativeColor = value; }
        }
        /// <summary>   Resets the graph. </summary>
        
        public void ResetGraph()
        {
            SetRatio();
            //FHeaderBoxWidth = Convert.ToInt32(Convert.ToDouble(SKPanel.Width) * FHeaderBoxWidthPCT);
            FHeaderBoxWidth = Convert.ToInt32(Convert.ToDouble(this.Width) * FHeaderBoxWidthPCT);
            //BuildResourceConsumerBoxes();
            BuildResourceConsumerBoxesExt();
            TheBarList = BuildTheFlowBars();
            this.Invalidate();
            
        }
        /// <summary>   Initializes the component. </summary>

        void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStripPopUp = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialogImage = new System.Windows.Forms.SaveFileDialog();
            this.contextMenuStripPopUp.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStripPopUp
            // 
            this.contextMenuStripPopUp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.contextMenuStripPopUp.Name = "contextMenuStripPopUp";
            this.contextMenuStripPopUp.Size = new System.Drawing.Size(103, 26);
            this.contextMenuStripPopUp.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // saveFileDialogImage
            // 
            this.saveFileDialogImage.Filter = "jpg|*.jpg";
            // 
            // SankeyGraph
            // 
            this.ContextMenuStrip = this.contextMenuStripPopUp;
            this.Name = "SankeyGraph";
            this.Size = new System.Drawing.Size(289, 180);
            this.contextMenuStripPopUp.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        // Build the Header Boxes
        int[] ResourcePointY = null;
        int[] ConsumerPointY = null;
        int[] LimitHeight = null;
        int[] DemandHeight = null;
        int[] ResourceNetHeight = null;
        int[] ConsumerNetHeight = null;

        /// <summary>   Builds the resource consumer boxes. </summary>
        
        SankeyBoxList TheBoxes = new SankeyBoxList();

        protected void BuildResourceConsumerBoxesExt()
        {
            SetRatio();
            if (FCRFNetwork != null)
            {
                TheBoxes.Clear();

                CRF_ResourceList SKResources = FCRFNetwork.Resources;
                int Columns = SKResources.MaxDepth();

                int BoxUnitCnt = (Columns * 3) + 1;
                int UnitSize = this.Width / BoxUnitCnt;
                FHeaderBoxWidth = UnitSize;
                

                CRF_ConsumerList SKConsumers = FCRFNetwork.Consumers;


                // build Resource Nodes
                int FResourceCnt = SKResources.Count;
                int FConsumerCnt = SKConsumers.Count;

                // Ok, need a top Y for each Resource
                // make sure there are Resources
                if (FResourceCnt > 0)
                {
                    // Create this Y point array
                    ResourcePointY = new int[FResourceCnt];
                    LimitHeight = new int[FResourceCnt];
                    ResourceNetHeight = new int[FResourceCnt];
                    // Use the bottom of last, the first is 0;
                    int NextTop = FBoxSpacer;
                    for (int i = 0; i < FResourceCnt; i++)
                    {
                        CRF_Resource CRFResource = (SKResources[i] as CRF_Resource);
                        // Assign the top o f this based on the botton of the last
                        ResourcePointY[i] = NextTop;
                        // ok this will now be the last, figure out bottom and pass it on
                        double theLimit = CRFResource.Limit;
                        double theNet = CRFResource.Net;
                        double Size = theLimit;  // use the limit as the initial size of the box
                        // if net is negative, then leave more room for next box
                        if (theNet < 0)
                        {
                            Size += theNet * -1;
                        }
                        ResourceNetHeight[i] = Convert.ToInt32(theNet * FScaleRatio);
                        LimitHeight[i] = Convert.ToInt32(theLimit * FScaleRatio);
                        SankeyBox SB = new SankeyBox(CRFResource, this, FToolOffset, ResourcePointY[i], FHeaderBoxWidth, LimitHeight[i], ResourceNetHeight[i]);
                        TheBoxes.Add(SB);
                        NextTop += Convert.ToInt32(Size * FScaleRatio) + FBoxSpacer;
                    }
                }

                // Ok, need a top Y for each Consumers
                // make sure there are Resources
                int MaxConsDepth = SKConsumers.MaxDepth();
                if (FConsumerCnt > 0)
                {
                    // Create this Y point array
                    ConsumerPointY = new int[FConsumerCnt];
                    DemandHeight = new int[FConsumerCnt];
                    ConsumerNetHeight = new int[FConsumerCnt];

                    // Use the bottom of last, the first is 0;
                    int NextTop = FBoxSpacer;
                    for (int i = 0; i < FConsumerCnt; i++)
                    {
                        CRF_Consumer CRFConsumer = (SKConsumers[i] as CRF_Consumer);
                        int mydepth = CRFConsumer.Depth();
                        int boxstep = MaxConsDepth - mydepth;

                        // Assign the top o f this based on the botton of the last
                        ConsumerPointY[i] = NextTop;
                        // ok this will now be the last, figure out bottom and pass it on
                        double theDemand = CRFConsumer.Demand;
                        double theNet = CRFConsumer.Net;
                        double Size = theDemand; // This is the Resource.Demand, which is the initial size of the box
                        // if net is negative, the leave more room for next box
                        if (theNet < 0)
                        {
                            Size += theNet * -1;
                        }
                        ConsumerNetHeight[i] = Convert.ToInt32(theNet * FScaleRatio);
                        DemandHeight[i] = Convert.ToInt32(theDemand * FScaleRatio);
                        SankeyBox SB = new SankeyBox(CRFConsumer, this, (((boxstep+1) * 3)+1)*UnitSize /* this.Width */ - (FHeaderBoxWidth + FToolOffset), ConsumerPointY[i], FHeaderBoxWidth, DemandHeight[i], ConsumerNetHeight[i]);
                        TheBoxes.Add(SB);
                        NextTop += Convert.ToInt32(Size * FScaleRatio) + FBoxSpacer;

                    }

                }
            }
        }

        protected void BuildResourceConsumerBoxes()
        {
            SetRatio();
            if (FCRFNetwork != null)
            {
                TheBoxes.Clear();

                CRF_ResourceList SKResources = FCRFNetwork.Resources;
                CRF_ConsumerList SKConsumers = FCRFNetwork.Consumers;

               
                // build Resource Nodes
                int FResourceCnt = SKResources.Count;
                int FConsumerCnt = SKConsumers.Count;

                // Ok, need a top Y for each Resource
                // make sure there are Resources
                if (FResourceCnt > 0)
                {
                    // Create this Y point array
                    ResourcePointY = new int[FResourceCnt];
                    LimitHeight = new int[FResourceCnt];
                    ResourceNetHeight = new int[FResourceCnt];
                    // Use the bottom of last, the first is 0;
                    int NextTop = FBoxSpacer;
                    for (int i = 0; i < FResourceCnt; i++)
                    {
                        CRF_Resource CRFResource = (SKResources[i] as CRF_Resource);
                        if (CRFResource.Value > 0)
                        {
                            // Assign the top o f this based on the botton of the last
                            ResourcePointY[i] = NextTop;
                            // ok this will now be the last, figure out bottom and pass it on
                            double theLimit = CRFResource.Limit;
                            double theNet = CRFResource.Net;
                            double Size = theLimit;  // use the limit as the initial size of the box
                                                     // if net is negative, the leave more room for next box
                            if (theNet < 0)
                            {
                                Size += theNet * -1;
                            }
                            ResourceNetHeight[i] = Convert.ToInt32(theNet * FScaleRatio);
                            LimitHeight[i] = Convert.ToInt32(theLimit * FScaleRatio);
                            SankeyBox SB = new SankeyBox(CRFResource, this, FToolOffset, ResourcePointY[i], FHeaderBoxWidth, LimitHeight[i], ResourceNetHeight[i]);
                            TheBoxes.Add(SB);
                            NextTop += Convert.ToInt32(Size * FScaleRatio) + FBoxSpacer;
                        }
                    }
                }

                // Ok, need a top Y for each Consumers
                // make sure there are Resources
                if (FConsumerCnt > 0)
                {
                    // Create this Y point array
                    ConsumerPointY = new int[FConsumerCnt];
                    DemandHeight = new int[FConsumerCnt];
                    ConsumerNetHeight = new int[FConsumerCnt];

                    // Use the bottom of last, the first is 0;
                    int NextTop = FBoxSpacer;
                    for (int i = 0; i < FConsumerCnt; i++)
                    {
                        CRF_Consumer CRFConsumer = (SKConsumers[i] as CRF_Consumer);
                        // Assign the top o f this based on the botton of the last
                        ConsumerPointY[i] = NextTop;
                        // ok this will now be the last, figure out bottom and pass it on
                        double theDemand = CRFConsumer.Demand;
                        double theNet = CRFConsumer.Net;
                        double Size = theDemand; // This is the Resource.Demand, which is the initial size of the box
                        // if net is negative, the leave more room for next box
                        if (theNet < 0)
                        {
                            Size += theNet * -1;
                        }
                        ConsumerNetHeight[i] = Convert.ToInt32(theNet * FScaleRatio);
                        DemandHeight[i] = Convert.ToInt32(theDemand * FScaleRatio);
                        SankeyBox SB = new SankeyBox(CRFConsumer, this, this.Width -(FHeaderBoxWidth+FToolOffset), ConsumerPointY[i], FHeaderBoxWidth, DemandHeight[i], ConsumerNetHeight[i]);
                        TheBoxes.Add(SB);
                        NextTop += Convert.ToInt32(Size * FScaleRatio) + FBoxSpacer;
                    }
                }
            }
        }
        protected void DrawName(Graphics Gr)
        {
            string Temp = "Region Name";
            if (FCRFNetwork != null) Temp = FCRFNetwork.Name;
            Color TextColor = ColorTools.ContrastColor(FNetworkBackground);
            Brush TextBrush = new SolidBrush(TextColor);
            Font TextFont = Font;
            int TextHeight = (int)TextFont.GetHeight(Gr);
            int Toolwidth = MinToolWidth * 2;
            int Offset = Toolwidth / 2; 
            Point TextPoint = new Point(Offset + Toolwidth + 2, this.Height - (TextHeight));
            Gr.DrawString(Temp, TextFont, TextBrush, TextPoint);

        }

        Rectangle GearRect = new Rectangle(0, 0, 0, 0);
        protected void DrawGear(Graphics Gr)
        {
            if (!FSHowTool)
            {
                int Toolwidth = MinToolWidth * 2;
                int Offset = Toolwidth / 2; // FToolOffset / 2;
                Point Center = new Point(Offset, this.Height - Offset);
                Point[] TheGear = Geometry.CirclePoints(Center, Offset - 1, 16);
                GearRect = new Rectangle(0, this.Height - Toolwidth, Toolwidth, Toolwidth);
                Brush FillGear = new SolidBrush(FNetworkBackground);
                Gr.FillRectangle(FillGear, GearRect);
                Color GearColor = ColorTools.ContrastColor(FNetworkBackground);
                Pen MyPen = new Pen(GearColor);
                MyPen.Width = 1;
                foreach (Point Pt in TheGear)
                {
                    Gr.DrawLine(MyPen, Center, Pt);
                }

                Gr.DrawRectangle(MyPen, GearRect);
            }
            else
            {
            }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Draw the Header boxes. </summary>
        ///
        /// <param name="Gr">   The gr. </param>
        ///-------------------------------------------------------------------------------------------------

        protected void DrawBoxes(Graphics Gr)
        {
            if (FCRFNetwork != null)
            {
                TheBoxes.DrawAll(Gr);
            }
        }

        protected SankeyFlowBarList BuildTheFlowBars()
        {
            
            if (FCRFNetwork != null)
            {
                TheBarList.Clear();
                // Setup the CRF list counts
                int FResourceCnt = FCRFNetwork.Resources.Count;
                int FConsumerCnt = FCRFNetwork.Consumers.Count;
                // Make sure there are resources to draw
                if (FResourceCnt > 0)
                {
                    // Setup Values for Offsets
                    float[] ResourceYOffset = new float[FResourceCnt];
                    float[] ConsumerYOffset = new float[FConsumerCnt];

                    for (int i = 0; i < FResourceCnt; i++)
                    {
                        ResourceYOffset[i] = 0;
                    }
                    for (int i = 0; i < FConsumerCnt; i++)
                    {
                        ConsumerYOffset[i] = 0;
                    }

                    // this is the index to current Resource
                    int ResIndex = 0;

                    float ResourcePointLeft = FHeaderBoxWidth+FToolOffset;
                   // float ConsumerPointRight = SKPanel.Width - FHeaderBoxWidth;
                    float ConsumerPointRight = this.Width -( FHeaderBoxWidth + FToolOffset);
                    // Keep track of the initial order
                    
                    foreach (CRF_Resource CRFResource in FCRFNetwork.Resources)
                    {
                        if (CRFResource.ToFluxs.Count > 0)
                        {
                            foreach (CRF_Flux Flux in CRFResource.ToFluxs)
                            {
                                // get the allocat Target
                                CRF_DataItem ConsSDI = Flux.Target;
                                //CRF_Consumer ConsSDI = Flux.Target;
                                if (ConsSDI != null)
                                {
                                    // get its index
                                    int ConsIndex = FCRFNetwork.Consumers.FindIndex(ConsSDI);
                                    if (ConsIndex > -1)
                                    {
                                        // Set up the Sankey Graph
                                        // 
                                        SankeyBox DB = TheBoxes.FindDataItem(ConsSDI);
                                        int ConsLeft = DB.Left();
                                        float ConsY = ConsumerPointY[ConsIndex] + ConsumerYOffset[ConsIndex];
                                        float ResY = ResourcePointY[ResIndex] + ResourceYOffset[ResIndex];

                                        float FlowSize = Convert.ToSingle(Flux.Allocated() * FScaleRatio);
                                        float TopLeftY = ResY;
                                        float TopRightY = ConsY;
                                        float BottomLeftY = ResY + FlowSize;
                                        float BottomRightY = ConsY + FlowSize;

                                        PointF ResourceTop = new PointF(ResourcePointLeft, TopLeftY);
                                        PointF ResourceBottom = new PointF(ResourcePointLeft, BottomLeftY);
                                        PointF ConsumerTop = new PointF(ConsLeft, TopRightY);//ConsumerPointRight, TopRightY);
                                        PointF ConsumerBottom = new PointF(ConsLeft, BottomRightY);//ConsumerPointRight, BottomRightY);

                                        SanKeyFlowBar Temp = new SanKeyFlowBar(Flux, ResourceTop, ResourceBottom, ConsumerTop, ConsumerBottom, CRFResource.Color, 0, CRFResource.Color);

                                        TheBarList.Add(Temp);
                                        ConsumerYOffset[ConsIndex] += FlowSize;
                                        ResourceYOffset[ResIndex] += FlowSize;

                                    } // if (ConsIndex > -1)
                                } // foreach (CRF_Flux Flux in CRFResource.Fluxs)
                            } // if (CRFResource.Fluxs.Count > 0)
                            ResIndex++;
                        } //   foreach (CRF_Resource CRFResource in FCRFNetwork.Resources)
                    } // foreach (CRF_Resource CRFResource in FCRFNetwork.Resources)
                } // if (FResourceCnt > 0)
            }
            return TheBarList;
        }
        
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Paints this window. </summary>
        ///
        /// <param name="e">    Event information to send to registered event handlers. </param>
        ///-------------------------------------------------------------------------------------------------

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //Graphics SKGraphic = SKPanel.CreateGraphics(); // e.Graphics;
           // Graphics SKGraphic = this.CreateGraphics(); // e.Graphics;
            // clear the panel
            //Brush ClearBrush = new SolidBrush(SKPanel.BackColor);
            Brush ClearBrush = new SolidBrush(FNetworkBackground);
            //SKGraphic.FillRectangle(ClearBrush,new Rectangle(0,0,SKPanel.Width,SKPanel.Height));
            e.Graphics.FillRectangle(ClearBrush, new Rectangle(0, 0, this.Width, this.Height));
            DrawBoxes(e.Graphics);
            //SankeyFlowBarList TheBars = BuildTheFlowBars();
            TheBarList.DrawAll(e.Graphics);
            DrawGear(e.Graphics);
            DrawName(e.Graphics);

            
        }

        /// <summary>   Sets the CRF Data to Graphic Ratio. </summary>
        protected void SetRatio()
        {
            if (FCRFNetwork != null)
            {
                // Get Resource Total Material
                double ResTotal = FCRFNetwork.Resources.Limit;
                int XtraResSpace =((FCRFNetwork.Resources.Count+1) * FBoxSpacer);
                // Now account for net
                double TotalNegNet = FCRFNetwork.Resources.NegNet;
                if (TotalNegNet < 0)
                {
                    ResTotal += -1 * TotalNegNet;
                }
                // Get Consumer Total Material
                double ConsTotal = FCRFNetwork.Consumers.Demand;
                int XtraConsSpace = ((FCRFNetwork.Consumers.Count+1) * FBoxSpacer);
                // Now account for net
                TotalNegNet = FCRFNetwork.Consumers.NegNet; 
                if (TotalNegNet < 0)
                {
                    ConsTotal += -1 * TotalNegNet;
                }
                float Largest = Convert.ToSingle(Math.Max(ResTotal, ConsTotal));
                int MaxXtra = Math.Max(XtraConsSpace, XtraResSpace);
                float PanelHeight = this.Height;
                FScaleRatio = (PanelHeight-(MaxXtra))  / Largest;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Event handler. Called by  for resize events. </summary>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information to send to registered event handlers. </param>
        ///-------------------------------------------------------------------------------------------------

        private void _Resize(object sender, EventArgs e)
        {
            ResetGraph();
        }

        private void ResetTool()
        {

            if (FSHowTool)
            {
               // FToolOffset = toolStripPopUp.Width;
            }
            else
            {
                FToolOffset = MinToolWidth;
            }
           // toolStripPopUp.BackColor = FNetworkBackground;
            //toolStripButtonCloseTool.ForeColor = ColorTools.ContrastColor(FNetworkBackground);
           // toolStripPopUp.Visible = FSHowTool;
            ResetGraph();

        }
        private void _MouseHover(object sender, EventArgs e)
        {
            // is this flow bar
            Point WhereP = this.PointToClient(Cursor.Position);

            if ((WhereP.X > FHeaderBoxWidth) && (WhereP.X < Width - FHeaderBoxWidth))
            {
                int TargetIndex = TheBarList.FindFlowBarIndex(WhereP.X, WhereP.Y, FDrawOrder);
                if (TargetIndex > -1)
                {
                    SanKeyFlowBar TargetFlowBar = TheBarList[TargetIndex];
                    CRF_Flux TheFlux = TargetFlowBar.Owner;
                    string Label = TheFlux.Label;
                }
            }
            else
            {
                // Not flowbar must be box
            }

        }

        private void _MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // is this flow bar
            if ((e.X > FHeaderBoxWidth) && (e.X < Width - FHeaderBoxWidth))
            {

            }
            else
            {
                // Not flowbar must be box
            }
          
    

        }

        private void _MouseClick(object sender, MouseEventArgs e)
        {
            // is this flow bar
            if ((e.X > FHeaderBoxWidth) && (e.X < Width - FHeaderBoxWidth))
            {
                int TargetIndex = TheBarList.FindFlowBarIndex(e.X, e.Y, FDrawOrder);
                if (TargetIndex > -1)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        Point Mpoint = new Point(e.X, e.Y);

                        if (TheBarList[TargetIndex].ShowLabel)
                        {
                            if (Geometry.IsPointInRectangle(TheBarList[TargetIndex].LabelRectangle, Mpoint))
                            {
                                TheBarList[TargetIndex].ShowLabel = false;
                            }
                            else
                            {
                                TheBarList[TargetIndex].LabelPoint = new Point(e.X, e.Y);
                            }
                        }
                        else
                        {
                            TheBarList[TargetIndex].LabelPoint = new Point(e.X, e.Y);
                            TheBarList[TargetIndex].ShowLabel = true;
                        }
                        Invalidate();
                    }

                    else
                    {
                        if (e.Button == System.Windows.Forms.MouseButtons.Right)
                        {
                            SanKeyFlowBar ToMove = TheBarList[TargetIndex];
                            TheBarList.Remove(ToMove);
                            ToMove.ShowLabel = false;
                            if (FDrawOrder == SankeyGraph.DrawOrder.doTopDown)
                            {
                                TheBarList.Add(ToMove);
                            }
                            else
                            {
                                TheBarList.Insert(0, ToMove);
                            }

                            Invalidate();
                        }
                    }
                }
            }
            else
            {
                // Not flowbar must be box

                if (Geometry.IsPointInRectangle(GearRect,new Point(e.X,e.Y)))
                {
                    FSHowTool = !FSHowTool;
                    ResetTool();
                }
            }

        }

        private void toolStripButtonCloseTool_Click(object sender, EventArgs e)
        {
            FSHowTool = false;
            ResetTool();
        }

        private void resetGraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetGraph();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialogImage.ShowDialog() == DialogResult.OK)
            {
                Bitmap MyBMP = new Bitmap(this.Width, this.Height);
                this.DrawToBitmap(MyBMP, new Rectangle(0, 0, this.Width, this.Height));
                string filename = saveFileDialogImage.FileName;
                MyBMP.Save(filename);
            }
        }

    }
    //================================================================================
    // TOOLS!
    //===================================================================================
    static public class ColorTools
    {
        static public Color ContrastColor(Color color)
        {
            int d = 0;

            // Counting the perceptive luminance - human eye favors green color... 
            double a = 1 - (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

            if (a < 0.5)
                d = 0; // bright colors - black font
            else
                d = 255; // dark colors - white font

            return Color.FromArgb(d, d, d);
        }
    }

    //===================================================================================
    static public class Geometry
    {
        static public bool IsPointInPolygon(PointF[] polygon, PointF point) 
        { 
            bool isInside = false; 
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            { 
                if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) && (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                { 
                    isInside = !isInside; 
                } 
            } 
            return isInside; 
        }

        static public bool IsPointInRectangle(Rectangle TheRect, Point point)
        {
            bool isInside = false;
            if (point.X >= TheRect.Left)
            {
                if (point.X <= TheRect.Right)
                {
                    if (point.Y >= TheRect.Top)
                    {
                        if (point.Y <= TheRect.Bottom)
                        {
                            isInside = true;
                        }
                    }
                }
            }
            return isInside;
        }

        static private int CircleY(Point Center, int Radius, int X, bool Pos)
        {
            int temp = 0;
            int Xdif = X - Center.X;
            double Q = Math.Abs((Radius * Radius) - (Xdif * Xdif));
            if (Pos)
            {
                temp = Convert.ToInt32(Math.Sqrt(Q)) + Center.Y;
            }
            else
            // Y<=Center.Y
            {
                temp = Center.Y - Convert.ToInt32(Math.Sqrt(Q));
            }

            return temp;
        }
        static private int CircleX(Point Center, int Radius, int Y, bool Pos)
        {
            int temp = 0;
            int Ydif = Y - Center.Y;
            double Q = Math.Abs((Radius*Radius)-(Ydif*Ydif));
            if (Pos)
            {
                temp = Convert.ToInt32(Math.Sqrt(Q)) + Center.X;
            }
            else
                // Y<=Center.Y
            {
                temp = Center.X - Convert.ToInt32(Math.Sqrt(Q));
            }

            return temp;
        }



        static public Point[] CirclePoints(Point Center, int Radius, int NumberOfPoints) // Number of points will be adjusted to be divisible by four
        {
            int PointN = NumberOfPoints / 4;
            int PointTotal = PointN * 4;


            
            Point[] thePoints = new Point[PointTotal];
           
            int index = 0;


            // Top
            int PtShift = Convert.ToInt32(Math.Sqrt((Radius*Radius)/2));
            int StartX = Center.X - PtShift;
            int EndX = Center.X + PtShift;
            int RangeX = EndX - StartX;
            int Xinc = RangeX / (PointN-1);
            int theX = StartX; 
            for(int i=0; i<PointN; i++)
            {
                thePoints[index] = new Point(theX,CircleY(Center,Radius,theX,false));
                theX += Xinc;
                index++;
            }
            // right
            int StartY = Center.Y - PtShift;
            int EndY = Center.Y + PtShift;
            int RangeY = EndY - StartY;
            int Yinc = RangeY / (PointN - 1);
            int theY = StartY;
            for (int i = 0; i < PointN; i++)
            {
                thePoints[index] = new Point(CircleX(Center, Radius, theY, true), theY);
                theY += Yinc;
                index++;
            }
            // Bottom
            theX = EndX;
            for (int i = 0; i < PointN; i++)
            {
                thePoints[index] = new Point(theX, CircleY(Center, Radius, theX, true));
                theX -= Xinc;
                index++;
            }
            // Left
            theY = EndY;
            for (int i = 0; i < PointN; i++)
            {
                thePoints[index] = new Point(CircleX(Center, Radius, theY, false), theY);
                theY -= Yinc;
                index++;
            }

            // bottom
            
            return thePoints;
        }
    }

     
    /// <summary>
    /// Spline interpolation class.
    /// </summary>
    public class SplineInterpolator
    {
        private readonly double[] _keys;

        private readonly double[] _values;
        
        private readonly double[] _h;
        
        private readonly double[] _a;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="nodes">Collection of known points for further interpolation.
        /// Should contain at least two items.</param>
        public SplineInterpolator(Dictionary<double, double> nodes)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException("nodes");
            }

            var n = nodes.Count;

            if (n < 2)
            {
                throw new ArgumentException("At least two point required for interpolation.");
            }

            Dictionary<double,double> Fuckyou = new Dictionary<double,double>();

            _keys = new double[nodes.Count];
            _values = new double[nodes.Count];
            int Nodeindex = 0;
            foreach (var keyAndVal in nodes)
            {
                _keys[Nodeindex] = keyAndVal.Key;
                _values[Nodeindex] = keyAndVal.Value;
                Nodeindex++;
            }
            
            //_keys = nodes.Keys.ToArray();
            //_values = nodes.Values.ToArray();
            _a = new double[n];
            _h = new double[n];

            for (int i = 1; i < n; i++)
            {
                _h[i] = _keys[i] - _keys[i - 1];
            }

            if (n > 2)
            {
                var sub = new double[n - 1];
                var diag = new double[n - 1];
                var sup = new double[n - 1];

                for (int i = 1; i <= n - 2; i++)
                {
                    diag[i] = (_h[i] + _h[i + 1]) / 3;
                    sup[i] = _h[i + 1] / 6;
                    sub[i] = _h[i] / 6;
                    _a[i] = (_values[i + 1] - _values[i]) / _h[i + 1] - (_values[i] - _values[i - 1]) / _h[i];
                }

                SolveTridiag(sub, diag, sup, ref _a, n - 2);
            }
        }

        /// <summary>
        /// Gets interpolated value for specified argument.
        /// </summary>
        /// <param name="key">Argument value for interpolation. Must be within 
        /// the interval bounded by lowest ang highest <see cref="_keys"/> values.</param>
        public double GetValue(double key)
        {
            int gap = 0;
            var previous = double.MinValue;

            // At the end of this iteration, "gap" will contain the index of the interval
            // between two known values, which contains the unknown z, and "previous" will
            // contain the biggest z value among the known samples, left of the unknown z
            for (int i = 0; i < _keys.Length; i++)
            {
                if (_keys[i] < key && _keys[i] > previous)
                {
                    previous = _keys[i];
                    gap = i + 1;
                }
            }
            double temp = 0;
            if (gap == 0)
            {
                temp = _keys[gap];
            }
            else
            {

                var x1 = key - previous;
                var x2 = _h[gap] - x1;

                temp = ((-_a[gap - 1] / 6 * (x2 + _h[gap]) * x1 + _values[gap - 1]) * x2 +
                    (-_a[gap] / 6 * (x1 + _h[gap]) * x2 + _values[gap]) * x1) / _h[gap];
            }
            return temp;
            }


        /// <summary>
        /// Solve linear system with tridiagonal n*n matrix "a"
        /// using Gaussian elimination without pivoting.
        /// </summary>
        private static void SolveTridiag(double[] sub, double[] diag, double[] sup, ref double[] b, int n)
        {
            int i;

            for (i = 2; i <= n; i++)
            {
                sub[i] = sub[i] / diag[i - 1];
                diag[i] = diag[i] - sub[i] * sup[i - 1];
                b[i] = b[i] - sub[i] * b[i - 1];
            }

            b[n] = b[n] / diag[n];
            
            for (i = n - 1; i >= 1; i--)
            {
                b[i] = (b[i] - sup[i] * b[i + 1]) / diag[i];
            }
        }
        /// <summary>
        /// Returns a Spline line
        /// </summary>
        /// <remarks> Line is NumberofPoints long, with the first and last X leys used as the first and last X of the line</remarks>
        /// <param name="NumberOfPoints"></param>
        /// <returns></returns>

        public PointF[] GetSpline(int NumberOfPoints)
        {
            PointF[] SplinePts = new PointF[NumberOfPoints];

            double start = _keys[0];
            double end = _keys[_keys.Length-1];
            double step = (end - start) / (NumberOfPoints);

            SplinePts[0] = new PointF(Convert.ToSingle(start),Convert.ToSingle(_values[0]));
            SplinePts[SplinePts.Length - 1] = new PointF(Convert.ToSingle(_keys[_keys.Length-1]), Convert.ToSingle(_values[_values.Length-1]));

            double TempX = start;
            for (int i = 1; i < SplinePts.Length - 1; i++)
            {
                TempX += step;
                double TempY = GetValue(TempX);
                SplinePts[i] = new PointF(Convert.ToSingle(TempX), Convert.ToSingle(TempY));
            }
            return SplinePts;
        }

        /// <summary>
        /// Returns a Spline line
        /// </summary>
        /// <remarks> Line is based on the XValues passed, for value and length of line</remarks>
        /// <param name="XValues"></param>
        /// <returns></returns>
        
        public PointF[] GetSpline(double[] XValues)
        {
            PointF[] SplinePts = new PointF[XValues.Length];

            SplinePts[0] = new PointF(Convert.ToSingle(_keys[0]), Convert.ToSingle(_values[0]));
            SplinePts[SplinePts.Length - 1] = new PointF(Convert.ToSingle(_keys[_keys.Length-1]), Convert.ToSingle(_values[_values.Length-1]));

            for (int i = 0; i < SplinePts.Length - 1; i++)
            {
                double TempY = GetValue(XValues[i]);
                SplinePts[i] = new PointF(Convert.ToSingle(XValues[i]), Convert.ToSingle(TempY));
            }
            return SplinePts;
        }
    }

    // ==================================================================================================
    public class SankeyBox
    {
        // CLEAN UPint FTopY = 0;
        // CLEAN UP int FWidth = 0;
        // CLEAN UP int FValueHeight = 0;
        // CLEAN UP int NetHeight = 0;
        Rectangle FBaseRect = new Rectangle(0,0,0,0);
        bool FDrawPos = false;
        Rectangle FPosRect = new Rectangle(0,0,0,0);
        bool FDrawNeg = false;
        Rectangle FNegRect = new Rectangle(0,0,0,0);

        SankeyGraph FTheGraph = null;
        CRF_DataItem FOwner = null;
        
        // Display Parms
        Font FTextFont = new Font("Arial", 9);

        
        public SankeyBox()
        {

        }
        public SankeyBox(CRF_DataItem theOwner,SankeyGraph theGraph, int theLeft, int theTop, int theWidth, int theValueHeight, int theNetHeight) 
        {
            FTheGraph = theGraph;
            FOwner = theOwner;
            FBaseRect = new Rectangle(theLeft, theTop, theWidth, theValueHeight);
            if(theNetHeight > 0)
                    {
                        FDrawPos = true;
                        FPosRect = new Rectangle(theLeft, theTop + (theValueHeight - theNetHeight), theWidth, theNetHeight);
                    }
                    else
                    {
                        if (theNetHeight < 0)
                        {
                            FDrawNeg = true;
                            FNegRect = new Rectangle(theLeft, theTop + theValueHeight, theWidth, -1 * theNetHeight);
                        }
                    }
        }

        public CRF_DataItem Owner
        {
            get { return FOwner; }
            set { FOwner = value; }
        }

        public Font Font
        {
            get { return FTextFont; }
            set { FTextFont = value; }
        }
        public Rectangle Box
        {
            get {
                    if (FDrawPos)
                    {
                        return new Rectangle(FBaseRect.X, FBaseRect.Y, FBaseRect.Width, FBaseRect.Height + FPosRect.Height);
                    }
                    else
                    {
                        return FBaseRect;
                    }
                }
        }

        public bool InBox(int X, int Y)
        {
            return Geometry.IsPointInRectangle(Box, new Point(X, Y));
        }

        public int Left()
        {
            return FBaseRect.X;
        }
       

        public void Draw(Graphics Gr)
        {
            if (FOwner != null)
            {
                Color UsePos = FTheGraph.PositiveColor;
                Color UseNeg = FTheGraph.NegativeColor;

                if (FOwner is CRF_Consumer)
                {
                    UsePos = FTheGraph.NegativeColor;
                    UseNeg = FTheGraph.PositiveColor;
                }
                Brush BoxBrush = new SolidBrush(FOwner.Color);
                Gr.FillRectangle(BoxBrush, FBaseRect);
                if (FDrawPos)
                {
                    BoxBrush = new SolidBrush(UsePos);
                    Gr.FillRectangle(BoxBrush, FPosRect);
                }
                else
                {
                    if (FDrawNeg)
                    {
                        BoxBrush = new SolidBrush(UseNeg);
                        Gr.FillRectangle(BoxBrush, FNegRect);
                    }
                }
                Color LabelColor = ColorTools.ContrastColor(FOwner.Color);
                Brush LabelBrush = new SolidBrush(LabelColor);
                Gr.DrawString(FOwner.Label, FTextFont, LabelBrush, FBaseRect.Location);
               
            }
        }
    }
    // ==================================================================================================
    public class SankeyResourceBox: SankeyBox
    {
        public SankeyResourceBox()
            : base()
        { 
        }
    }

    // ==================================================================================================
    public class SankeyConsumerBox : SankeyBox
    {
        public SankeyConsumerBox()
            : base()
        {
        }
    }
    // ==================================================================================================
    
    public class SankeyBoxList : List<SankeyBox>
    {
        public SankeyBoxList() : base ()
        {

        }

        public void DrawAll(Graphics Gr)
        {
             foreach (SankeyBox SKB in this)
            {
                 SKB.Draw(Gr);
            }
        }

        SankeyBox FindBox(int X, int Y)
        {
            foreach (SankeyBox SB in this)
            {
                if (SB.InBox(X, Y))
                {
                    return SB;
                }
            }
            return null;
        }

        public SankeyBox FindDataItem(CRF_DataItem DI)
        {
            SankeyBox item = null;
            foreach (SankeyBox SB in this)
            {
                if (SB.Owner == DI)
                {
                    item = SB;
                    break;
                }
            }
            return item;
        }
    }
    // ==================================================================================================
    public class SankeyResourceBoxList : SankeyBoxList
    {
        public SankeyResourceBoxList()
        {
        }
    }
 
    // ==================================================================================================
    public class SankeyConsumerBoxList : SankeyBoxList
    {
        public SankeyConsumerBoxList()
        {
        }
    }
    // ==================================================================================================
    public class SanKeyFlowBar
    {
        // The Owner
        CRF_Flux FOwner;

        int FOrderToBeDrawn = 0;  // 0 based order, with 0 first

        // The POlygonPoints
        PointF[] FPoints;

        // Display Parameters
        Color FLineColor = Color.Black;
        int FLineSize = 2; // 0 means no line
        Color FFlowColor = Color.Gray;

        // Mouse Parameters
        bool FShowLabel = false;
        Point FLabelPoint = new Point(0,0);
        Color FLabelColor = Color.LightYellow;
        Color FTextColor = Color.DarkGray;
        Font  FTextFont  = new Font("Arial", 9);
        Rectangle FlabelRect = new Rectangle();

        public SanKeyFlowBar():base()
        {
            FOrderToBeDrawn = 0;
        }

        public SanKeyFlowBar(CRF_Flux theOwner, PointF[] thePoints)
            : base()
        {
            FPoints = thePoints;
            FOwner = theOwner;

            
        }

        public SanKeyFlowBar(CRF_Flux theOwner, PointF[] thePoints, Color aLineColor, int aLineSize, Color aFlowColor)
            : base()
        {
            FPoints = thePoints;
            FOwner = theOwner;
            FLineColor = aLineColor;
            FLineSize = aLineSize;
            FFlowColor = aFlowColor;
        }

        public SanKeyFlowBar(CRF_Flux theOwner, Point ResourceTop, Point ResourceBottom, Point ConsumerTop, Point ConsumerBottom)
            : base()
        {
            FPoints = BuildPolygon(ResourceTop,ResourceBottom, ConsumerTop, ConsumerBottom);
            FOwner = theOwner;
        }

        public SanKeyFlowBar(CRF_Flux theOwner, PointF ResourceTop, PointF ResourceBottom, PointF ConsumerTop, PointF ConsumerBottom, Color aLineColor, int aLineSize, Color aFlowColor)
            : base()
        {
            FPoints = BuildPolygon(ResourceTop, ResourceBottom, ConsumerTop, ConsumerBottom);
            FOwner = theOwner;
            FLineColor = aLineColor;
            FLineSize = aLineSize;
            FFlowColor = aFlowColor;
        }

        public bool ShowLabel
        {
            get { return FShowLabel; }
            set { FShowLabel = value; }
        }

        public Rectangle LabelRectangle
        {
            get { return FlabelRect; }
        }

        protected PointF[] BuildPolygon(PointF ResourceTop, PointF ResourceBottom, PointF ConsumerTop, PointF ConsumerBottom)
        {
            float FlowWidth = ConsumerTop.X - ResourceTop.X;
            float Slide = 2;// FlowWidth / 10;

            float ResourceRightX = ResourceTop.X + (Slide / 2);
            float ConsumerLeftX = ConsumerTop.X - (Slide / 2);
            float ResourceStartX = ResourceTop.X + Slide;
            float ConsumerStartX = ConsumerTop.X - Slide;
            float FlowMidX = (ResourceStartX + ConsumerStartX) / 2;
            float ResourceQtrX = FlowMidX - ((FlowMidX - ResourceStartX) / 4);
            float ConsumerQtrX = FlowMidX + ((ConsumerStartX - FlowMidX) / 4);
            //float ResourceQtrX = ResourceStartX + ((FlowMidX - ResourceStartX) / 4);  
            //float ConsumerQtrX = ConsumerStartX - ((ConsumerStartX - FlowMidX) / 4);
            //float ResourceQtrX = ResourceStartX + ((FlowMidX - ResourceStartX) / 2);
            //float ConsumerQtrX = ConsumerStartX - ((ConsumerStartX - FlowMidX) / 2);
            

            float ResourceRightY = ResourceTop.Y;
            float ResourceStartY = ResourceTop.Y;
            float FlowMidTopY = (ResourceTop.Y + ConsumerTop.Y) / 2;
            float ConsumerStartY = ConsumerTop.Y;
            float ConsumerLeftY = ConsumerTop.Y;
            //float ResourceQtrY = FlowMidTopY - ((FlowMidTopY - ResourceTop.Y) / 4);
            //float ConsumerQtrY = FlowMidTopY + ((ConsumerTop.Y - FlowMidTopY) / 4);
            //float ResourceQtrY = ResourceStartY + ((FlowMidTopY - ResourceTop.Y)/4);
            //float ConsumerQtrY = ConsumerStartY - ((ConsumerTop.Y - FlowMidTopY) / 4);
            float ResourceQtrY = ResourceStartY + ((FlowMidTopY - ResourceTop.Y) / 2);
            float ConsumerQtrY = ConsumerStartY - ((ConsumerTop.Y - FlowMidTopY) / 2);

            float FlowMidBottomY = (ResourceBottom.Y + ConsumerBottom.Y) / 2;

            //float[] KnotX = new float[7];
            //KnotX[0] = ResourceRightX;
            //KnotX[1] = ResourceStartX;
            //KnotX[2] = ResourceQtrX;
            //KnotX[3] = FlowMidX;
            //KnotX[4] = ConsumerQtrX;
            //KnotX[5] = ConsumerStartX;
            //KnotX[6] = ConsumerLeftX;

            float[] KnotX = new float[6];
            KnotX[0] = ResourceRightX;
            KnotX[1] = ResourceStartX;
            KnotX[2] = ResourceQtrX;
            KnotX[3] = ConsumerQtrX;
            KnotX[4] = ConsumerStartX;
            KnotX[5] = ConsumerLeftX;

            //float[] KnotX = new float[5];
            //KnotX[0] = ResourceRightX;
            //KnotX[1] = ResourceStartX;
            //KnotX[2] = FlowMidX;
            //KnotX[3] = ConsumerStartX;
            //KnotX[4] = ConsumerLeftX;

            //float[] KnotX = new float[4];
            //KnotX[0] = ResourceRightX;
            //KnotX[1] = ResourceStartX;
            //KnotX[2] = ConsumerStartX;
            //KnotX[3] = ConsumerLeftX;
            
            //float[] KnotY = new float[7];
            //KnotY[0] = ResourceRightY;
            //KnotY[1] = ResourceStartY;
            //KnotY[2] = ResourceQtrY;
            //KnotY[3] = FlowMidTopY;
            //KnotY[4] = ConsumerQtrY;
            //KnotY[5] = ConsumerStartY;
            //KnotY[6] = ConsumerLeftY;

            float[] KnotY = new float[6];
            KnotY[0] = ResourceRightY;
            KnotY[1] = ResourceStartY;
            KnotY[2] = ResourceQtrY;
            KnotY[3] = ConsumerQtrY;
            KnotY[4] = ConsumerStartY;
            KnotY[5] = ConsumerLeftY;
            
            //float[] KnotY = new float[5];
            //KnotY[0] = ResourceRightY;
            //KnotY[1] = ResourceStartY;
            //KnotY[2] = FlowMidTopY;
            //KnotY[3] = ConsumerStartY;
            //KnotY[4] = ConsumerLeftY;

            //fzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzloat[] KnotY = new float[4];
            //KnotY[0] = ResourceRightY;
            //KnotY[1] = ResourceStartY;
            //KnotY[2] = ConsumerStartY;
            //KnotY[3] = ConsumerLeftY;

            Dictionary<double,double> MyTopSet = new Dictionary<double,double>();
            for (int i = 0; i < KnotX.Length; i++)
            {
                MyTopSet.Add(KnotX[i],KnotY[i]);
            }

            SplineInterpolator MySpline = new SplineInterpolator(MyTopSet);

            int NumberOfPoints = 20;

            double[] Seed = new double[NumberOfPoints];

            Seed[0] = ResourceStartX;
            Seed[NumberOfPoints - 1] = ConsumerStartX;

            double range = (ConsumerStartX - ResourceStartX);
            double seedinc = range / (NumberOfPoints - 1);
            for (int i = 1; i < NumberOfPoints - 1; i++)
            {
                Seed[i] = Seed[i - 1] + seedinc;
            }
            PointF[] TheLine = MySpline.GetSpline(Seed);
                
//            PointF[] TheLine = MySpline.GetSpline(NumberOfPoints);

            //PointF[] FancyTemp = new PointF[(NumberOfPoints * 2) + 5];
            
            //FancyTemp[0] = ResourceTop;
            //FancyTemp[NumberOfPoints+1] = ConsumerTop;
            //FancyTemp[NumberOfPoints+ 2] = ConsumerBottom;
            //FancyTemp[(NumberOfPoints * 2) + 3] = ResourceBottom;
            //FancyTemp[(NumberOfPoints * 2) + 4] = ResourceTop;

            //for (int i = 0; i < TheLine.Length; i++)
            //{
            //    FancyTemp[i + 1] = TheLine[i];
            //}
            PointF[] FancyTemp = new PointF[(NumberOfPoints * 2)];

            //FancyTemp[0] = ResourceTop;
            //FancyTemp[NumberOfPoints] = ConsumerTop;
            //FancyTemp[NumberOfPoints + 1] = ConsumerBottom;
            //FancyTemp[(NumberOfPoints * 2) + 1] = ResourceBottom;
            //FancyTemp[(NumberOfPoints * 2) + 2] = ResourceTop;

            for (int i = 0; i < TheLine.Length; i++)
            {
                FancyTemp[i] = TheLine[i];
            }

            const float AdjustForSlopeReduction = 0.6f;
            float Basedrop = ConsumerBottom.Y - ConsumerTop.Y;
            int index = (NumberOfPoints * 2)-1;
            for (int i = 0; i < TheLine.Length; i++)
            {
                float drop = Basedrop;

                if (i > 0)
                // so this is the first line segment
                {
                    // As the slopr the flow line gets larger, visually the line appears thinner
                    // The best way to adjust for this is to adjustr the width using a measurement that is right angle to the slope
                    // That is a lot of work ebcuase then the points do not align with the X value.
                    // Here is a simpler method trhat approximates this.
                    // adjust the drop for slope of line, if slope is high, drop should be higher

                    float Yrise = (TheLine[i - 1].Y - TheLine[i].Y);
                    float Xrun = (TheLine[i - 1].X - TheLine[i].X);
                    if ((Xrun * Yrise) != 0)
                    {
                        float slope = Math.Abs(Yrise / Xrun);
                        float add = ( Basedrop * slope) *AdjustForSlopeReduction;
                        drop += add;
                    }
                }
                // OK assign this point
                FancyTemp[index] = new PointF(TheLine[i].X,TheLine[i].Y+drop);
                index--;
            }

//            return Temp;
            return FancyTemp;
        }

        public Point LabelPoint
        {
            get { return FLabelPoint; }
            set { FLabelPoint = value; }
        }
        public CRF_Flux Owner
        {
            get { return FOwner; }
            set { FOwner = value; }
        }

        public PointF[] Points
        {
            get { return FPoints; }
            set { FPoints = value; }
        }

        public Color FlowColor
        {
            get { return FFlowColor; }
            set { FFlowColor = value; }
        }

        public Color LineColor
        {
            get { return FLineColor; }
            set { FLineColor = value; }
        }

        public int LineSize
        {
            get { return FLineSize; }
            set { FLineSize = value; }
        }

        public void DrawLabel(Graphics Gr)
        {
            if (FLabelPoint.X==0)
            {
                FLabelPoint.X = Convert.ToInt32(FPoints[0].X)+3;
                FLabelPoint.Y = Convert.ToInt32(FPoints[0].Y)+3;
            }
                Brush LabelBrush = new SolidBrush(FLabelColor);
                SizeF strsize = Gr.MeasureString(FOwner.Label, FTextFont);
                int LabelWidth = Convert.ToInt32(strsize.Width);
                int LabelHeight = Convert.ToInt32(strsize.Height);
                int LabelLeft = FLabelPoint.X - (LabelWidth/2);
                int LabelTop = FLabelPoint.Y;// -(LabelHeight / 2);
                Point DrawPoint = new Point(LabelLeft,LabelTop);
                FlabelRect = new Rectangle(LabelLeft-3,LabelTop-3,LabelWidth+6,LabelHeight+6);
                Gr.FillRectangle(LabelBrush,FlabelRect);
                LabelBrush = new SolidBrush(FTextColor);
                Gr.DrawString(FOwner.Label, FTextFont, LabelBrush, DrawPoint);
 
        }
       public void Draw(Graphics Gr)
        {
            Brush FillBrush = new SolidBrush(FFlowColor);
            
            Gr.FillPolygon(FillBrush, FPoints,System.Drawing.Drawing2D.FillMode.Alternate);
            if (FLineSize > 0)
            {
                Pen LinePen = new Pen(FLineColor, FLineSize);
                Gr.DrawPolygon(LinePen, FPoints);
            }
        }

        public bool InFlow(PointF ThePoint)
        {
            bool result = false;
            result = Geometry.IsPointInPolygon(FPoints, ThePoint);
            return result;
        }
    }

    //================================================================================
    public class SankeyFlowBarList : List<SanKeyFlowBar>
     {
        SankeyGraph.DrawOrder FDrawOrder = SankeyGraph.DrawOrder.doTopDown;
        public SankeyFlowBarList()
            : base()
        {
        }

        public SankeyFlowBarList(SankeyGraph.DrawOrder theDrawOrder): base()
        {
            FDrawOrder = theDrawOrder;
        }

        public SanKeyFlowBar FindFLowBar(PointF ThePoint)
        {
            SanKeyFlowBar Temp = null;
            foreach (SanKeyFlowBar SKFB in this)
            {
                if (SKFB.InFlow(ThePoint))
                {
                    Temp = SKFB;
                    break;
                }
            }
            return Temp;
        }

        public int FindFlowBarIndex(int X, int Y, SankeyGraph.DrawOrder theDrawOrder)
        {
            int tempindex = 0;
            int found = -1;

            PointF target = new PointF(X,Y);
            if (theDrawOrder == SankeyGraph.DrawOrder.doBottomUp)
            {
                foreach (SanKeyFlowBar SKFB in this)
                {
                    if (SKFB.InFlow(target))
                    {
                        found = tempindex;
                        break;
                    }
                    tempindex++;
                }
            }
            else
            {
                int cnt = this.Count;
                for(int i=cnt-1;i>-1;i--)
                {
                    SanKeyFlowBar SKFB = this[i];
                    if (SKFB.InFlow(target))
                    {
                        found = i;
                        break;
                    }
                }

            }
            return found;
        }


        public void DrawAll(Graphics Gr)
        {
            foreach (SanKeyFlowBar SKFB in this)
            {
                SKFB.Draw(Gr);
            }
            foreach (SanKeyFlowBar SKFB in this)
            {
               if(SKFB.ShowLabel)
                   SKFB.DrawLabel(Gr);
            }
        }
    }
}
                    