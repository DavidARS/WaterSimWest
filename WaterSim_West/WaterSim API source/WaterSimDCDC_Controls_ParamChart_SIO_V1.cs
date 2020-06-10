// ===========================================================
//     WaterSimDCDC Regional Water Demand and Supply Model Version 5.0

//       A generic parameter chart that charts a provider's parameter values over time

//       WaterSimDCDC.Controls 
//       Version 1
//       Keeper Ray Quay  ray.quay@asu.edu
//       Copyright (C) 2011,2012 , The Arizona Board of Regents
//              on behalf of Arizona State University

//       All rights reserved.

//       Developed by the Decision Center for a Desert City
//       Lead Model Development - David A. Sampson <david.a.sampson@asu.edu>

//       This program is free software: you can redistribute it and/or modify
//       it under the terms of the GNU General Public License version 3 as published by
//       the Free Software Foundation.

//       This program is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU General Public License for more details.

//       You should have received a copy of the GNU General Public License
//       along with this program.  If not, please see <http://www.gnu.org/licenses/>.
//
//====================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.DataVisualization.Charting;

using System.IO;
using UniDB;

namespace WaterSimDCDC.Controls
{
    public partial class WaterSimChartControl : UserControl
    {
        const int MAXLINESIZE = 10;

        //DataTable FChartDBTable = null;
        /// <summary> The manager. </summary>
        public ChartManager Manager;
        
        internal int FLineWidth = 2;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        ///
        /// <remarks> Ray, 1/23/2013. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public WaterSimChartControl()
        {
            InitializeComponent();
            Manager = new ChartManager(WaterSimChart,"");
            LineChartMenuItem.Enabled = false;
            Manager.ChartType = SeriesChartType.Line;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the information describing the chart. </summary>
        ///
        /// <value> Information describing the chart. </value>
        ///-------------------------------------------------------------------------------------------------

        //public DataTable ChartData
        //{
        //    get { return FChartDBTable; }
        //    set { FChartDBTable = value; }

        //}

        public ChartManager ChartManager
        {
            get{ return Manager; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the chart title. </summary>
        ///
        /// <value> The chart title. </value>
        ///-------------------------------------------------------------------------------------------------

        public string ChartTitle
        {
            get { return Manager.ChartTitle; }
            set { Manager.ChartTitle = value; }
        }

        //-------------------------------------------------------------------------------------
        private void increaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Series dbS in WaterSimChart.Series)
            {
                if (dbS.BorderWidth < MAXLINESIZE) dbS.BorderWidth++;
            }
        }

        //-------------------------------------------------------------------------------------
        private void decreaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Series dbS in WaterSimChart.Series)
            {
                if (dbS.BorderWidth > 1) dbS.BorderWidth--;
            }
        }

        //-------------------------------------------------------------------------------------
        private void WaterSimChart_Click(object sender, EventArgs e)
        {
           

        }

        //-------------------------------------------------------------------------------------
        private void WaterSimChart_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.Series != null)
            {
                e.Text = e.HitTestResult.Series.LabelToolTip;
            } 
        }

        //-------------------------------------------------------------------------------------
        private void StackedChartMenuItem_Click(object sender, EventArgs e)
        {
            int scount = WaterSimChart.Series.Count;
            foreach (Series S in WaterSimChart.Series)
            {
                S.ChartType = SeriesChartType.StackedArea;
            }
            Manager.ChartType = SeriesChartType.StackedArea;
            StackedChartMenuItem.Enabled = false;
            LineChartMenuItem.Enabled = true;
            lineWidthToolStripMenuItem.Enabled = false;
        }

        //-------------------------------------------------------------------------------------
        private void LineChartMenuItem_Click(object sender, EventArgs e)
        {
            int scount = WaterSimChart.Series.Count;
            foreach (Series S in WaterSimChart.Series)
            {
                S.ChartType = SeriesChartType.Line;
            }
            Manager.ChartType = SeriesChartType.Line;
            StackedChartMenuItem.Enabled = true;
            LineChartMenuItem.Enabled = false;
            lineWidthToolStripMenuItem.Enabled = true;
        }

        //-------------------------------------------------------------------------------------
        private void CopyChartMenuItem_Click(object sender, EventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                WaterSimChart.SaveImage(ms, ChartImageFormat.Bmp);
                Bitmap bm = new Bitmap(ms);
                Clipboard.SetImage(bm);
            } 
        }

        public void SaveChart(string filename)
        {
            string ext = Path.GetExtension(filename).ToUpper();
            if (ext == ".BMP")
            { WaterSimChart.SaveImage(filename, ChartImageFormat.Bmp); }
            else
                if ((ext == ".JPG") || (ext == ".JPEG"))
                { WaterSimChart.SaveImage(filename, ChartImageFormat.Jpeg); }
                else
                    if (ext == ".EMF")
                    { WaterSimChart.SaveImage(filename, ChartImageFormat.EmfPlus); }
                    else
                        if (ext == ".PNG")
                        { WaterSimChart.SaveImage(filename, ChartImageFormat.Png); }
                        else
                            if ((ext == ".TIF") || (ext == ".TIFF"))
                            { WaterSimChart.SaveImage(filename, ChartImageFormat.Tiff); }
        }
        //-------------------------------------------------------------------------------------
        private void saveAsMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveChartToFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = SaveChartToFileDialog.FileName;
                SaveChart(filename);
            }
        }
    }
    }


