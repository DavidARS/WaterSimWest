<%@ Page Title="Charts" Language="C#" MasterPageFile="~/Charts.Master" AutoEventWireup="true" CodeBehind="Charts.aspx.cs" Inherits="WaterSimUI._Charts" %>

<%@ Register src="~/UserControls/OutputUserControl.ascx" tagname="OutputUserControl" tagprefix="Wsmo" %>

<%@ Register src="~/UserControls/InputUserControl.ascx" tagname="InputUserControl" tagprefix="Wsmi" %>




<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="GraphControls">
 
                    <link href="Content/horizontal.css" rel="stylesheet" />
                    <link href="Content/isotope.css" rel="stylesheet" />
                    <link href="Content/Add/isotopeV2.css" rel="stylesheet" />
                    
 
                    
                    <div style="background: #4f5557;">
                            
			                <div class="isotope" id="myCharts">
			                </div>

                            <div id="isotope-filters" class="button-group">
                                <a class="button" data-filter="*">All</a>
                            </div>
                            <div id="empty-space" style="height:10px;"></div>
                        <div id="dialog-charts" title="Charts">
                            <p>Please select the charts to be displayed:</p>
                                <div id="checkboxChartTypes">
								<input type="checkbox" id="checkboxSupply" name="Supply">
									<label for="checkboxSupply">Supply</label>
                                <br>
								<input type="checkbox" id="checkboxDemand" name="Demand">
									<label for="checkboxDemand">Demand</label>
								<br>
								<input type="checkbox" id="checkboxReservoirs" name="Reservoirs">
									<label for="checkboxReservoirs">Reservoirs/Rivers</label>
                                <br>
								<%--<input type="checkbox" id="checkboxSustainability" name="Sustainability">
									<label for="checkboxSustainability">Sustainability</label>
                                <br>--%>
                                <input type="checkbox" id="checkboxAll" name="All">
									<label for="checkboxAll">All</label>
							</div>
                        </div>
                    </div>



                    <script src="Scripts/Sly/sly.js"></script>
                    <script src="Scripts/Sly/horizontal-supply.js"></script>
                    <script src="Scripts/Sly/horizontal-demand.js"></script>
                    <script src="Scripts/Sly/horizontal-reservoirs.js"></script>
                    <script src="Scripts/Sly/horizontal-climate.js"></script>

                    <script src="Scripts/Isotope/isotope.pkgd.js"></script>
<!-- QUAY EDIT 3/13/14 -->

                    <script src="Scripts/Custom/Charts/ChartTools.js"></script>
                    <script src="Scripts/Custom/Charts/DrillDownChartBO.js"></script>
                    <script src="Scripts/Custom/Charts/ProvidersChart.js"></script>
<!-- QUAY EDIT 3/13/14 -->
    
                    <script src="Scripts/Custom/Charts/AreaChart.js"></script>
                    <script src="Scripts/Custom/Charts/DrillDownColumnChartBO.js"></script>
                    <script src="Scripts/Custom/Charts/DrillDownLineChartBO.js"></script>
                    <script src="Scripts/Custom/Charts/DrillDownLineChartTEMP.js"></script>
                    <script src="Scripts/Custom/Charts/DrillDownPieColumnChartMF.js"></script>
                    <script src="Scripts/Custom/Charts/DrillDownPieColumnChartMP.js"></script>
                    <script src="Scripts/Custom/Charts/DrillDownSingleColumnChart.js"></script>
                    <script src="Scripts/Custom/Charts/StackedAreaChart.js"></script>
                    <script src="Scripts/Custom/Charts/StackedColumnChart.js"></script>
<!-- QUAY EDIT 3/13/14 -->
                    <script src="Scripts/Custom/Charts/LineChartMP.js"></script>
<!-- QUAY EDIT 3/13/14 -->

                    <script src="Scripts/Highcharts/highcharts.js"></script>
                    <script src="Scripts/HighCharts/modules/drilldown.js"></script>
                    <script src="Scripts/Custom/Charts/HighChartsUnderscoreFix.js"></script>

  <!-- QUAY EDIT 6/30/14 
    Used to support Report Generation
    -->
    <script src="Scripts/rgbcolor.js"></script>
    <script src="Scripts/canvg.js"></script>
    <script src="Scripts/Highcharts/modules/exporting.js"></script>
      <script src='Scripts/Custom/qPbar.js'></script>

    <!-- ------------------------------------- -->

</asp:Content>


