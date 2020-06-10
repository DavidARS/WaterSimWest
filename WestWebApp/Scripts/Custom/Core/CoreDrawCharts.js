//draw output charts depending on the output controls used

// QUAY EDIT 1 29 16
// Modofied to receive jason already parsed no need to do this more than once
//function drawOutputCharts(outputJSON) {
function drawOutputCharts($jsonObj) {

    //jsondata = outputJSON;

    // END QUAY EDIT
     
    subControls = {};
    fldNames = {};
    fldValues = {};
    providerName = {};
    // QUAY EDIT BEGIN 3/12/14 4:00
    fldMAXes = {};
    fldMINes = {};
    fldUnits = {};
    fldLongunits = {};

    // QUAY EDIT END 3/12/14 4:00

    // QUAY EDIT 1 29 16
    // Passing Parsed JSON returned by web service to extract years
    setStrtandEndYr($jsonObj);
    //setStrtandEndYr();
    // QUAY EDIT 4/11/16 Begin
    // Added preparsed InfoRequest
//    inputJSONStr = JSON.parse(infoRequestJSON);

    //getting the fld names and sub controls
    $.each(INFO_REQUEST.FieldInfo, function () {
//        $.each(inputJSONStr.FieldInfo, function () {

        fldNames[this.FLD] = this.LAB;
        // QUAY EDIT BEGIN 3/12/14 4:00
        fldMAXes[this.FLD] = this.MAX;
        fldMINes[this.FLD] = this.MIN;
        fldUnits[this.FLD] = this.UNT;
        fldLongunits[this.FLD] = this.UNTL;
        // QUAY EDIT END 3/12/14 4:00

        if (this.DEP)
            subControls[this.FLD] = [this.DEP];
        else
            subControls[this.FLD] = [""];

    });

    //STEPTOE EDIT BEGIN 11/08/15
    //getting provider names w.r.to their code
    //$('input[name="geography"]').each(function () {
    //    providerName[this.value] = $(this).next('label').html();
    //});

    //If the window is not Charts.aspx then get provider names w.r. to their code
    //from the Chosen Selector
    if (getWindowType() != "Charts") {
        // QUAY EDIT 1 29 16
        // OK there is no provider select control if ipad

        // STEPTOE EDIT BEGIN 12/27/17
        //providerName["st"] = "State";
        //if (window.location.href.indexOf('Ipad') > -1) {
        //    // THIS IS A KLUDGE!!!!! Need to figure out this provider stuff
        //    providerName["st"] = "State";
        //}
        //else {
        //    var selectedProviders = $('.chosen-select').val();
        //    for (var i in selectedProviders)
        //        providerName[selectedProviders[i]] = providerInfo[selectedProviders[i]];
        //}
        // STEPTOE EDIT END 12/27/17
    }
        //Otherwise page is Charts.aspx, get provider info from providerInfo
    else {
        for (var i in providerInfo)
            providerName[i] = providerInfo[i];
    }
    //STEPTOE EDIT END 11/08/15

    // QUAY EDIT 1 29 16
    // NO NEED FOR THIS, Info come into function parsed, see above
    //$jsonObj = JSON.parse(jsondata);
    // END QUAY EDIT
    $.each($jsonObj.RESULTS, function () {
        fldValues[this.FLD] = this.VALS;

        //
    });

    //looping through the output controls and getting the required data and populating the charts
    $('.OutputControl').each(function () {
        var controlID = $(this).attr('id');
        // QUAT EDIT 1 30 16
        // modified to pass parsed json object
        drawChart(controlID, $jsonObj);
        // END Quay Edit

    });

    //STEPTOE EDIT BEGIN 03/22/16
    //Make all charts show
    $('#isotope-demand-container').isotope();
    // drawAllIndicators();
}

// QUAY EDIT BEGIN 3/13/14 evening
function Time_Chart_Draw() {
    if (jsondata != undefined) {
        var refresh = false;
        $('input[name="temporal"]:checked').each(function () {

            if (this.value == "point-in-time" && event.currentTarget.id == "point-in-time-slider") {
                refresh = true;
            } else if (this.value == "range-in-time" && event.currentTarget.id == "range-in-time-slider") {
                refresh = true;
            }
        });

        if (!refresh) {
            return;
        }
        setStrtandEndYr();
        //looping through the output controls and getting the required data and populating the charts
        $('.OutputControl').each(function () {
            var controlID = $(this).attr('id');
            drawChart(controlID);
        });

        drawAllIndicators();
    }
}
// QUAY EDIT END 3/13/14 evening

//===============================================
// Draw Chart Function
// Version 2
// Substantial Changes here  3/15/14,06.23.15
// ===============================================
//                    green       orange    dk green
// colors to try #82C341, #FAA31B,#009F75
var ColorSeriesArray = new Array();
var seriesDBlue = ["#0066CC", "#E60000"];
var seriesNew = ["#7cb5ec", "#f7a35c", "#7798BF", " #90ee7e", "#aaeeee", "#ff0066", "#eeaaee"];
var seriesArea = ["#336699", "#D63333"];
var seriesAreaStacked = seriesNew;
var seriesAreaLine = ["#336699", "#FF0000", "#00FF00", "#FF6600", "#33CCFF"];
var seriesLine = ["#000000", "#f7a35c", "#0066CC", "#FF0000", " #009933", "#B200B2", "#E6E6E6"];
// var seriesColumn3 = ["#FF9900", "#336699", "#FF0000"];
var seriesColumn3 = ["#3399FF", "#000000", "#009900"];

var seriesColumn7 = ["#E68AE6", "#FF6600", "#000000", "#88C6ED", "#009900", "#3399FF", "#FF0000"];
//                              black           orange       
//
ColorSeriesArray[0] = seriesLine;
ColorSeriesArray[1] = seriesDBlue;
ColorSeriesArray[2] = seriesLine;
ColorSeriesArray[3] = seriesAreaLine;
ColorSeriesArray[4] = seriesArea
ColorSeriesArray[5] = seriesAreaStacked;
ColorSeriesArray[6] = seriesNew;
ColorSeriesArray[7] = seriesColumn3;
ColorSeriesArray[8] = seriesColumn7;

var seriesColors = seriesLine;

// QUAY EDIT 1 30 16
// modified to pass parsed json object
function drawChart(controlID, jsondata) {
    var MyChartType;

    var type = $("#" + controlID).attr("data-Type");

    var mySeriesString = $("#" + controlID).attr("data-series");
    var color = ColorSeriesArray[mySeriesString];
    if (color != undefined) {
        seriesColors = color;
    }
    var MainType = type.substr(0, 4);
    var SubType = "";
    if (type.length > 4)
        SubType = type.substr(4, 3);

    //console.log("MainType:", MainType, ", Subtype:", SubType);

    var TimeMode = "";
    // ================================================================================================

   

    // ================================================================================================
    switch (MainType) {
        case "OFMP":
            $('#' + controlID).find("select[id*=ddlfld]").hide();

            if (strtYr == endYr) {
                // ok point in time show the graph types drop down
                $('#' + controlID).find("select[id*=ddlTypes]").show();
                drawDrillDownPieColumnChartMP(jsondata, controlID, subControls, fldNames, fldValues, providerName, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits);
            } else {
                $('#' + controlID).find("select[id*=ddlTypes]").hide();
                switch (SubType) {
                    case "":
                        var CurrentProvider = providerName;
                        CurrentProvider["doreg"] = false;
                        //drawAreaStackedChart(jsondata, controlID, subControls, fldNames, fldValues, providerName, strtYr, endYr, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits);
                        //QUAY EDIT 1 29 16
                        // modified drawproviderschart to pass parsed json object
                        //drawProvidersChart(jsondata, controlID, subControls, fldNames, fldValues, CurrentProvider, strtYr, endYr, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits, seriesColors, ChartTypeAreaStacked);
                        drawProvidersChart(jsondata, controlID, subControls, fldNames, fldValues, CurrentProvider, strtYr, endYr, (strtYr - jsondata.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits, seriesColors, ChartTypeAreaStacked);
                        break;
                    case "L":
                        //drawLineChartMP(jsondata, controlID, subControls, fldNames, fldValues, providerName, strtYr, endYr, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits);
                        drawProvidersChart(jsondata, controlID, subControls, fldNames, fldValues, BaseProviders, strtYr, endYr, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits, seriesColors, ChartTypeLine);
                        break;
                    case "R":
                        var CurrentProvider = providerName;
                        //var pcode = $("#" + controlID).find("select[id*=ddlfld]").find("option:selected").val();
                        //CurrentProvider[pcode] = providerName[pcode];
                        CurrentProvider["doreg"] = true;

                        //drawLineChartMP(jsondata, controlID, subControls, fldNames, fldValues, providerName, strtYr, endYr, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits);
                        drawProvidersChart(jsondata, controlID, subControls, fldNames, fldValues, CurrentProvider, strtYr, endYr, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits, seriesColors, ChartTypeLine);
                        break;

                }
            }
            break;
        case "MFOP":
            // Add providers to drop down
            if ($("#" + controlID).find("select[id*=ddlfld]").find("option").length == 0) {
                //STEPTOE EDIT BEGIN 11/08/15
                //$('input[name="geography"]').each(function () {
                //    $("#" + controlID).find("select[id*=ddlfld]").append(new Option($(this).next('label').html(), this.value));
                //});

                //Get available providers from Chosen Selector instead of geography control
                var selectedProviders = $('.chosen-select').val();
                for (var i in selectedProviders)
                    $("#" + controlID).find("select[id*=ddlfld]").append(new Option(providerInfo[selectedProviders[i]], selectedProviders[i]));
                //STEPTOE EDIT END 11/08/15
            }
            if (strtYr == endYr) {
                // ok point in time show the graph types drop down
                $('#' + controlID).find("select[id*=ddlTypes]").show();
                // multi fields so show the provider drop down
                $('#' + controlID).find("select[id*=ddlfld]").show();
                drawDrillDownPieColumnChartMF(jsondata, controlID, subControls, fldNames, fldValues, providerName, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits);
            } else {
                // range so hyde the chart types
                $('#' + controlID).find("select[id*=ddlTypes]").hide();
                // this is going to use the provider drop down, so get the cuurent provider code in the drop down and use this
                var CurrentProvider = {};
                var pcode = $("#" + controlID).find("select[id*=ddlfld]").find("option:selected").val();
                CurrentProvider[pcode] = providerName[pcode];
                CurrentProvider["doreg"] = true;
                // ok make standard provider chart call with only one provider code and ChartTypeColumnStacked
                drawProvidersChart(jsondata, controlID, subControls, fldNames, fldValues, CurrentProvider, strtYr, endYr, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits, seriesColors, ChartTypeColumnStacked);
                //drawColumnStackedChart(jsondata, controlID, subControls, fldNames, fldValues,     strtYr, endYr, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits);
            };
            break;
        case "OFOP":
            // only one provider so hide the provider drop down
            $('#' + controlID).find("select[id*=ddlfld]").hide();
            if (strtYr == endYr) {
             //    ok point in time show the graph types drop down
                $('#' + controlID).find("select[id*=ddlTypes]").show();
                drawDrillDownSingleColumnChart(jsondata, controlID, subControls, fldNames, fldValues, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR));
            } else {
                // ok a range show hide the graph type
                $('#' + controlID).find("select[id*=ddlTypes]").hide();
                drawDrillDownLineChartTEMP(jsondata, controlID, subControls, fldNames, fldValues, providerName, strtYr, endYr, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR));
            };
            break;
        case "BASE":
            $('#' + controlID).find("select[id*=ddlTypes]").hide();
            $('#' + controlID).find("select[id*=ddlfld]").hide();
            if (strtYr == endYr) {
                switch (SubType) {
                    case "L", "A":
                        MyChartType = ChartTypeColumn;
                        break;
                    case "SL", "SA":
                        MyChartType = ChartTypeColumnStacked;
                        break;
                }
            } else {
                switch (SubType) {
                    case "L":
                        MyChartType = ChartTypeLine;
                        break;
                    case "LS":
                        MyChartType = ChartTypeLineStacked;
                        break;
                    case "A":
                        MyChartType = ChartTypeArea;
                        break;
                    case "SA":
                        MyChartType = ChartTypeAreaStacked;
                        break;
                    case "AL":
                        MyChartType = ChartTypeAreaLine;
                        break;

                }
            }

            drawDrillDownChartBO(jsondata, controlID, subControls, fldNames, fldValues, strtYr, endYr, (strtYr - jsondata.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits, seriesColors, MyChartType);
            break;

        case "WSAP":
            $('#' + controlID).find("select[id*=ddlTypes]").hide();
            $('#' + controlID).find("select[id*=ddlfld]").hide();
            var index = endYr- strtYr;
            switch (SubType) {
                case "M":
                    drawMultiPieChart(jsondata, controlID, subControls, fldNames, fldValues, providerName,index, fldMINes, fldMAXes, fldLongunits);
                    break;
                case "S":
                    drawPieChart(jsondata, controlID, subControls, fldNames, fldValues, providerName, 1, fldMINes, fldMAXes, fldLongunits);
                    break;

                case "P":
                    drawPieChart(jsondata, controlID, subControls, fldNames, fldValues, providerName, endYr - strtYr, fldMINes, fldMAXes, fldLongunits);
                    break;
            }
            break;
         case "WSAS":
            $('#' + controlID).find("select[id*=ddlTypes]").hide();
            $('#' + controlID).find("select[id*=ddlfld]").hide();
            var index = 1;
            switch (SubType) {

                case "R":
                    Name = "Resources";
                    drawSimpleStackedColumnChart(jsondata, controlID, subControls, fldNames, fldValues, Name, endYr - strtYr, fldMINes, fldMAXes, fldLongunits);
                    break;
                case "C":
                    Name = "Consumers";
                    drawSimpleStackedColumnChart(jsondata, controlID, subControls, fldNames, fldValues, Name, endYr - strtYr, fldMINes, fldMAXes, fldLongunits);
                    break;
                case "F":
                    //DrawCorFlow(jsondata);
                    //var TheFluxList = new FluxDataList();
                    //console.log(TheFluxList)
                    //console.log('Need to fix drawComplexStackedColumnChart');
                    drawComplexStackedColumnChart(jsondata, controlID, subControls, TheFluxList);
                    break;
                case "K":
                    $('#' + controlID).find("select[id*=ddlTypes]").hide();
                    $('#' + controlID).find("select[id*=ddlfld]").hide();
                    //
                    //console.log("WSASK")
                    //console.log(TheFluxList)
                    //console.log('Need to fix drawMySankey');
                    drawMySankey(TheFluxList, jsondata, controlID);
                    //
                    break;
                case "L":
                    drawSimpleLineChart(jsondata, controlID, subControls, fldNames, fldValues, Name, strtYr, endYr, (strtYr - jsondata.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits);
                    break;
             }
            break;
    }

}