var OutputTabCode = /#TAB#/g;
var OutputControlIndexCode = /#CID#/g;
var OutputFldNameCode = /#FLD#/g;
var OutputTitleCode = /#TTL#/g;
var OutputFilterCode = /#FLT#/g;
var OutputTypeCode = /#TYP#/g;
var OutputClassCode = /#CLS#/g;
var OutputSeriesColorCode = /#SSC#/g;

var divIsotope = '<div class="isotope" id="#TAB#">'
var divItem = '<div class="item transition #FLT#">';
var divItemv2 = '<div class="itemv2 transition #FLT#">';
var divChart = '<div class="chart">';
var divOutputControl = '<div data-series="#SSC#" data-title="#TTL#" data-fld="#FLD#" data-type="#TYP#" class="OutputControl" style="position:relative; top:0px;" id="GraphControls_OutputUserControl#CID#_OutputControl">';
var divChartContainer = '<div style="position: absolute; width: 500px; top: 30px; height: 255px;" id="GraphControls_OutputUserControl#CID#_ChartContainer"></div>'
var divClose = "</div>";

var selectType = '<select style="position: absolute; width: 100px; height: 30px; border: medium groove; top: 0px; left: 0px;" id="ddlTypes" class="ddlType">';
var selectFld = '<select style="position: absolute; width: 150px; height: 30px; border: medium groove; top: 0px; right: 30px;" id="ddlfld" class="ddlflds">';
var selectClose = '</select>';

var option = '<option value="#OPT#">';
var optionClose = '</option>';
var optionPie = '<option value="pie">Pie</option>';
var optionColumn = '<option value="column">Column</option>';
var optionReg = '<option value="reg">Regional</option>';
var optionBu = '<option value="bu">Buckeye</option>'
var optionCh = '<option value="ch">Chandler</option>';
var optionGi = '<option value="gi">Gilbert</option>';
var optionPh = '<option value="ph">Phoenix</option>';
var optionSc = '<option value="sc">Scottsdale</option>';

var spanLblChartOption = '<span id="GraphControls_OutputUserControl#CID#_lblChartOption" style="display: none;">';
var spanLblFldName = '<span id="GraphControls_OutputUserControl#CID#_lblFldName" style="display: none;">';
var spanLblTitle = '<span id="GraphControls_OutputUserControl#CID#_lblTitle" style="display: none;">';
var spanLblSeriesColor = '<span id="GraphControls_OutputUserControl#CID#_lblSeriesColors" style="display: none;">';
var spanClose = '</span>';

//Base Filter Container
var buttonDisplays = new Array();
//Supply Filters
buttonDisplays['Supply'] = '<a class="button display" data-filter=".Supply">Water Supply</a>';
buttonDisplays['Credits'] = '<a class="button display" data-filter=".Credits">Credits</a>';
buttonDisplays['Aquifer'] = '<a class="button display" data-filter=".Aquifer">Regional Aquifer</a>';
//Demand Filters
buttonDisplays['total'] = '<a class="button display" data-filter=".total">Water Demand</a>';
buttonDisplays['population'] = '<a class="button population-button" data-filter=".population">Population</a>';
buttonDisplays['gpcd'] = '<a class="button display" data-filter=".gpcd">GPCD</a>';
//Resevoirs/Rivers Filters
buttonDisplays['reservoirsCO'] = '<a class="button display" data-filter=".reservoirsCO">CO Reservoirs</a>';
buttonDisplays['reservoirsSV'] = '<a class="button display" data-filter=".reservoirsSV">SV Reservoirs</a>';
buttonDisplays['traces'] = '<a class="button display" data-filter=".traces">Traces</a>';
//Sustainability Filters
buttonDisplays['Indicators'] = '<a class="button display" data-filter=".Indicators">Indicators</a>';
buttonDisplays['GroundWater'] = '<a class="button display" data-filter=".GroundWater">GroundWater</a>';
buttonDisplays['all'] = '<a class="button display" data-filter="*">All</a>';
//Create custom filter
var buttonDisplay = '<a class="button #CLS#" data-filter=".#FLT#">';
var buttonDisplayClose = '</a>';

//Used to generate html for an output control chart, return type is a string.
//fiter: Isotope filter
//controlIndex: output control number, should be unique
//type: valid chart type
//fieldName: field name for the control
//title: title to be displayed
//seriesColor: determines colors used for chart
//Example Usage -> var htmlString = buildOutputControl("gpcd", "8C", "OFMPL", "GPCDUSED", "Total GPCD By Provider", "2");
function buildOutputControl(filter, controlIndex, type, fieldName, title, seriesColor) {
    var html = "";
    html += divItemv2;
    html += divChart;
    html += divOutputControl;
    html += selectType + optionPie + optionColumn + selectClose;
    //Using dynamic list from Default.aspx
    //html += selectFld + optionReg + optionBu + optionCh + optionGi + optionPh + optionSc + selectClose;
    html += selectFld + selectClose;
    html += divChartContainer + divClose;
    html += spanLblChartOption + type + spanClose;
    html += spanLblFldName + fieldName + spanClose;
    html += spanLblTitle + title + spanClose;
    html += spanLblSeriesColor + seriesColor + spanClose;
    html += divClose + divClose + divClose;

    html = html.replace(OutputFilterCode, filter);
    html = html.replace(OutputControlIndexCode, controlIndex);
    html = html.replace(OutputTypeCode, type);
    html = html.replace(OutputFldNameCode, fieldName);
    html = html.replace(OutputTitleCode, title);
    html = html.replace(OutputSeriesColorCode, seriesColor);

    return html;
}

//Used to generate html for a button to filter charts in Isotope
//className: button class
//filter: item class to filter by
//name: text to be shown on button
function buildButtonFilter(className, filter, name) {
    var html = "";
    html += buttonDisplay + name + buttonDisplayClose;
    html = html.replace(OutputClassCode, className);
    html = html.replace(OutputFilterCode, filter);

    return html;
}

//Used to generate html for an Array of filters that are in buttonDisplays
//filters: Array of strings, can only contain strings that are in buttonDisplays
function getButtonFilters(filters) {
    var html = "";

    for (var i = 0; i < filters.length; i++) {
        html += buttonDisplays[filters[i]];
    }

    return html;
}

//Used to activate change functions for newly added select controls
function loadChangeFunctions() {
    //Getting the div ID drop down list selected item changed
    $(".ddlType").change(function () {
        drawChart($(this).closest('div[id*=OutputControl]').attr('id'));
    });

    //Getting the div ID drop down list selected item changed
    $(".ddlflds").change(function () {
        drawChart($(this).closest('div[id*=OutputControl]').attr('id'));
    });
}

//Used to load charts and resize them for isotope and activate selectors
//callWebService -> gets necessary data from Default.aspx
//loadIsotope -> setups up Isotope container and resizes elements
//loadChangeFunctions -> sets up selectors
function loadCharts() {
    callWebService(getJSONData('empty'));
    loadIsotope();
    loadChangeFunctions();
}

/*
<div class="item transition Supply"><div class="chart" ><Wsmo:OutputUserControl runat="server"  ID="OutputControl1" Type="MFOP" FieldName="WATAUGUSED,BNKUSED,RECTOT,MGWPUMP,SRPSURF,SRPGW,COLDELIV" Title="Water Source"  SeriesColors="8" /></div></div>
<div class="item transition Supply"><div class="chart" ><Wsmo:OutputUserControl runat="server"  ID="OutputControl1a" Type="MFOP" FieldName="MGWPUMP,AGPUMPED,SRPGW" Title="Groundwater Pumped"  SeriesColors="7"/></div></div>

<div class="item transition Credits"><div class="chart"><Wsmo:OutputUserControl runat="server" ID="OutputControl2" Type="OFMP" FieldName="GWAVAIL" Title="Groundwater Credits Available" SeriesColors="5"/></div></div> 
<div class="item transition Credits"><div class="chart"><Wsmo:OutputUserControl runat="server" ID="OutputControl2a" Type="OFMPR" FieldName="AGTOMUN1" Title="Agriculture Credit Transfer to Muni" SeriesColors="5" /></div></div>
<div class="item transition Aquifer"><div class="chart"><Wsmo:OutputUserControl runat="server" ID="OutputControl3" Type="BASEA" FieldName="REGAQBAL" Title="Regional Aquifer Level" SeriesColors="1" /></div></div>
*/

function addSupplyCharts(targetID) {
    var html = "";
    html += buildOutputControl("Supply", "1", "MFOP", "WATAUGUSED,BNKUSED,RECTOT,MGWPUMP,SRPSURF,SRPGW,COLDELIV", "Water Source", "8");
    html += buildOutputControl("Supply", "1a", "MFOP", "MGWPUMP,AGPUMPED,SRPGW", "Groundwater Pumped", "7");

    html += buildOutputControl("Credits", "2", "OFMP", "GWAVAIL", "Groundwater Credits Available", "5");
    html += buildOutputControl("Credits", "2a", "OFMPR", "AGTOMUN1", "Agriculture Credit Transfer to Muni", "5");
    html += buildOutputControl("Aquifer", "3", "BASEA", "REGAQBAL", "Regional Aquifer Level", "1");
    if (typeof (targetID) != "undefinded" && targetID != null)
        $('#' + targetID).append(html);
    else
        $('#myCharts').append(html);
}

/*
<a class="button" data-filter=".Supply">Water Supply</a>
<a class="button" data-filter=".Credits">Credits</a>
<a class="button" data-filter=".Aquifer">Regional Aquifer</a>
<a class="button" data-filter="*">All</a>
*/
function addSupplyFilters(targetID) {
    var html = "";
    html += getButtonFilters(['Supply', 'Credits', 'Aquifer'])
    if (typeof (targetID) != "undefinded" && targetID != null)
        $('#' + targetID).append(html);
    else
        $('#isotope-filters').append(html);
}

//Adds html for the supply charts and filters
//targetID: tag to add the html to, if not given assigns to default location
function addSupply(targetID) {
    addSupplyCharts(targetID);
    addSupplyFilters(targetID);
}

/*
<div class="item transition total"><div class="chart"><Wsmo:OutputUserControl runat="server" ID="OutputControl7" Type="OFMP" FieldName="TOTDEM" Title="Total Water Use"  SeriesColors="5"/></div></div>
<div class="item transition population"><div class="chart"><Wsmo:OutputUserControl runat="server" ID="OutputControl5" Type="OFMP" FieldName="POPUSED" Title="Population"  SeriesColors="5"/></div></div>                                                                 
<div class="item transition gpcd"><div class="chart"><Wsmo:OutputUserControl runat="server" ID="OutputControl8B" Type="MFOP" FieldName="GPCDCOMIND,GPCDRES" Title="Gallons per Person per Day"  SeriesColors="7"/></div></div>
<div class="item transition gpcd"><div class="chart"><Wsmo:OutputUserControl runat="server" ID="OutputControl8C" Type="OFMPL" FieldName="GPCDUSED" Title="Total GPCD By Provider"  SeriesColors="2"/></div></div>
*/
function addDemandCharts(targetID) {
    var html = "";
    html += buildOutputControl("total", "7", "OFMP", "TOTDEM", "Total Water Use", "5");
    html += buildOutputControl("population", "5", "OFMP", "POPUSED", "Population", "5");
    html += buildOutputControl("gpcd", "8B", "MFOP", "GPCDCOMIND,GPCDRES", "Gallons per Person per Day", "7");
    html += buildOutputControl("gpcd", "8C", "OFMPL", "GPCDUSED", "Total GPCD By Provider", "2");
    if (typeof (targetID) != "undefinded" && targetID != null)
        $('#' + targetID).append(html);
    else
        $('#myCharts').append(html);
}

/*
<a class="button display" data-filter=".total">Water Demand</a>
<a class="button population-button" data-filter=".population">Population</a>
<a class="button display" data-filter=".gpcd">GPCD</a>
<a class="button display" data-filter="*">All</a>
*/
function addDemandFilters(targetID) {
    var html = "";
    html += getButtonFilters(['total', 'population', 'gpcd'])
    if (typeof (targetID) != "undefinded" && targetID != null)
        $('#' + targetID).append(html);
    else
        $('#isotope-filters').append(html);
}

//Adds html for the demand charts and filters
//targetID: tag to add the html to, if not given assigns to default location
function addDemand(targetID) {
    addDemandCharts(targetID);
    addDemandFilters(targetID);
}

/*
<div class="item transition reservoirsCO"><div class="chart"><Wsmo:OutputUserControl runat="server" ID="OutputControl9" Type="BASEA" FieldName="POWSTORE,MEDSTORE" Title="Water Storage: Lake Powell and Lake Mead"  SeriesColors="4"/></div></div>
<div class="item transition reservoirsCO"><div class="chart"><Wsmo:OutputUserControl runat="server" ID="OutputControl13" Type="BASEAL" FieldName="MEADELEV,MDDPL,MDSSL,MDSSH,MDMPL" Title="Lake Mead Elevation(s)"  SeriesColors="3"/></div></div>
<div class="item transition traces"><div class="chart"><Wsmo:OutputUserControl runat="server" ID="OutputControl10" Type="BASEL" FieldName="CORFLOW" Title="Flow on the Colorado River"  SeriesColors="1"/></div></div>
<div class="item transition reservoirsSV"><div class="chart"><Wsmo:OutputUserControl runat="server" ID="OutputControl11" Type="BASEA" FieldName="SRSTORE,VRSTORE" Title="Water Storage: Salt-Verde"  SeriesColors="4"/></div></div>--%>
<div class="item transition traces"><div class="chart"><Wsmo:OutputUserControl runat="server" ID="OutputControl18" Type="BASEL" FieldName="STRFLOW,VRFLOW" Title="Salt-Verde River Flows"  SeriesColors="1" /></div></div>

*/
function addResevoirsCharts(targetID) {
    var html = "";
    html += buildOutputControl("reservoirsCO", "9", "BASEA", "POWSTORE,MEDSTORE", "Water Storage: Lake Powell and Lake Mead", "4");
    html += buildOutputControl("reservoirsCO", "13", "BASEAL", "MEADELEV,MDDPL", "Lake Mead Elevation(s)", "3");
    html += buildOutputControl("traces", "10", "BASEL", "CORFLOW", "Flow on the Colorado River", "1");
    html += buildOutputControl("reservoirsSV", "11", "BASEA", "SRSTORE,VRSTORE", "Water Storage: Salt-Verde", "4");
    html += buildOutputControl("traces", "18", "BASEL", "STRFLOW,VRFLOW", "Salt-Verde River Flows", "1");
    if (typeof (targetID) != "undefinded" && targetID != null)
        $('#' + targetID).append(html);
    else
        $('#myCharts').append(html);
}

/*
<a class="button" data-filter=".reservoirsCO">CO Reservoirs</a>
<a class="button" data-filter=".reservoirsSV">SV Reservoirs</a>

<a class="button" data-filter=".traces">Traces</a>
<a class="button" data-filter="*">All</a>
*/
function addResevoirsFilters(targetID) {
    var html = "";
    html += getButtonFilters(['reservoirsCO', 'reservoirsSV', 'traces'])
    if (typeof (targetID) != "undefinded" && targetID != null)
        $('#' + targetID).append(html);
    else
        $('#isotope-filters').append(html);
}

//Adds html for the resevoirs charts and filters
//targetID: tag to add the html to, if not given assigns to default location
function addResevoirs(targetID) {
    addResevoirsCharts(targetID);
    addResevoirsFilters(targetID);
}

/*
<div class="item transition Indicators"><div class="chart" ><Wsmo:OutputUserControl runat="server"  ID="OutputControl14" Type="BASEL" FieldName="SINPCTGW,SINDENV,SINDAG,SINDPC,SINYRGWR" Title="Temporal Sustainability Indicators"  SeriesColors="2"/></div></div>
<div class="item transition GroundWater"><div class="chart" ><Wsmo:OutputUserControl runat="server"  ID="OutputContro15" Type="OFMPL" FieldName="PCTGWAVL"  Title="Percent of Original Groundwater Credits Available"  SeriesColors="2"/></div></div>
                                     
*/
function addSustainabilityCharts(targetID) {
    var html = "";
    html += buildOutputControl("Indicators", "14", "BASEL", "SINPCTGW,SINDENV,SINDAG,SINDPC,SINYRGWR", "Temporal Sustainability Indicators", "2");
    html += buildOutputControl("GroundWater", "15", "OFMPL", "PCTGWAVL", "Percent of Original Groundwater Credits Available", "2");
    if (typeof (targetID) != "undefinded" && targetID != null)
        $('#' + targetID).append(html);
    else
        $('#myCharts').append(html);
}

/*
<a class="button" data-filter="*">All</a>
<a class="button" data-filter=".Indicators">Indicators</a>
<a class="button" data-filter=".GroundWater">Groundwater</a>
*/
function addSustainabilityFilters(targetID) {
    var html = "";
    html += getButtonFilters(['Indicators', 'GroundWater'])
    if (typeof (targetID) != "undefinded" && targetID != null)
        $('#' + targetID).append(html);
    else
        $('#isotope-filters').append(html);
}

//Adds html for the sustainability charts and filters
//targetID: tag to add the html to, if not given assigns to default location
function addSustainability(targetID) {
    addSustainabilityCharts(targetID);
    addSustainabilityFilters(targetID);
}
