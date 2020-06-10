/// <reference path="/Scripts/Custom/Charts/ChartTools.js" />
// USED FOR CODE "BASEL"
function drawDrillDownChartBO(jsonData, controlID, subControls, fldLabName, fldValue, strtYr, endYr, index, Min, Max, Units, seriescolors, chartTypeCode) {
    var divID = $("#" + controlID).find("div[id*=ChartContainer]").attr('id');
    var fldnames = $("#" + controlID).attr("data-fld");
    var title = $("#" + controlID).attr("data-title");

    var $jsonObj = $.parseJSON(jsonData); //parsing the Input String as Json object

    // --------------------------------------------------------------------------------------------------
    var axisTitleStyle = {
        font: "12px 'Trebuchet MS', Verdana, sans-serif",
        fontcolor: '#000000'
    };
    var axisLabelFontStyle = {};
    var chartTitleFontStyle = {};
    var subTitleFontStyle = {};
    var legendFontStyle = {
        font: "12px 'Trebuchet MS', Verdana, sans-serif",
        fontcolor: '#000000'
    };
    var tooltipFontStyle = {};
    var chartFontStyle = {};
    if (getWindowType() == "Charts") {
        axisTitleStyle = {
            fontSize: "0.9em",
            fontcolor: '#000000'
        };
        axisLabelFontStyle = { fontSize: "0.8em" };
        chartTitleFontStyle = { fontSize: "1.6em" };
        subTitleFontStyle = { fontSize: "1.3em" };
        legendFontStyle = {
            fontSize: "1em",
            fontcolor: '#000000'
        };
        tooltipFontStyle = { fontSize: "1em" };
        chartFontStyle = { fontSize: "1em" };
    }

    // setup the chart
    var MyDataChartType = "";
    var MyDataChartType_2 = "";
    var MyXLabelRotation = 0;
    var MyXLabelEnabled = true;
    var MyStacking = "";
    var MyFillOpacity = 1;
    var isStacked = false;
    //
    var seriesColors = seriescolors;
   // --------------------------------------------------------------------------------------------------
    switch (chartTypeCode) {
        case ChartTypeLine:
            MyDataChartType = ChartTypeLine;
            MyDataChartType_2 = ChartTypeLine;
            MyStacking = "";
            MyXLabelRotation = 60;
            MyXLabelEnabled = true;
           // seriesColors = seriescolors_Line;
            break;
        case ChartTypeLineStacked:
            MyDataChartType = ChartTypeLine;
            MyDataChartType_2 = ChartTypeLine;
            MyStacking = "normal";
            MyXLabelRotation = 60;
            MyXLabelEnabled = true;
            isStacked = true;
           // seriesColors = seriescolors_Line;
            break;
        case ChartTypeColumn:
            MyDataChartType = ChartTypeColumn;
            MyDataChartType_2 = ChartTypeColumn;
            MyStacking = "";
            MyXLabelRotation = 0;
            MyXLabelEnabled = false;
            break;
        case ChartTypeColumnStacked:
            MyDataChartType = ChartTypeColumn;
            MyDataChartType_2 = ChartTypeColumn;
            MyStacking = "normal";
            MyXLabelRotation = 0;
            MyXLabelEnabled = false;
            isStacked = true;
            break;
        case ChartTypeArea:
            MyDataChartType = ChartTypeArea;
            MyDataChartType_2 = ChartTypeArea;
            MyStacking = "";
            MyFillOpacity = 0.6;
            MyXLabelRotation = 60;
            MyXLabelEnabled = true;
           // seriesColors = seriesColors_Area;
            break;
        case ChartTypeAreaStacked:
            MyDataChartType = ChartTypeArea;
            MyDataChartType_2 = ChartTypeArea;
            MyStacking = "normal";
            MyXLabelRotation = 60;
            MyXLabelEnabled = true;
            isStacked = true;
            //seriesColors = seriesColors_AreaStacked;

            break;
        case ChartTypeAreaLine:
            MyDataChartType_2 = ChartTypeLine;
            MyDataChartType = ChartTypeArea;
            MyStacking = "";
            MyXLabelRotation = 60;
            MyXLabelEnabled = true;
            //seriesColors =  seriescolors_AreaLine;
            break;
    }

   
    var drilldownData = [];
    var drilldownDataArray = [];  // used with line
    var drilldownSeries = [];
    var ctrls = [];
    var year = [];
    var chartsDataArray = [];
    var fldName = [];  // used with pie/column

    var flds = fldnames.split(',');
    // DAS --------------------------------------------------------------
    // This code is used to scale the y axis, and to assign units
    var Ymin = 0;
    var Ymax = 25000000;
    var Yunits = 'Acre Feet per Year';
    if (Units != undefined) {
        Yunits = Units[flds[0]];
    }
    
    if (Max != undefined) {
        Ymax = GetMax(flds, Max);
 //       Ymax = JudgeMax(flds,Max,false)
    }
    if (Min != undefined) {
        Ymin = GetMin(flds, Min);
        //       Ymax = JudgeMax(flds,Max,false)
    }
    // Calc Tic Sizes
    var xTicSize = YearTicSize(strtYr, endYr);
    var yTicSize = VertTicSize(Ymin, Ymax);
    var xStep = JudgeXStep(strtYr, endYr, xTicSize);
    // calc Scale
    var Yscale = 1;
    if (Ymax > 1000000) { Yscale = 1000000; Yunits += " (million)"; }
    else
        if (Ymax > 1000) { Yscale = 1000; Yunits += " (thousand)"; }
    // DAS ----------------------------------------------------------------------------
    // Fundging it hear, have to change this in the API
    if (Ymax == 1300) { Ymax = 1200; }
    //=============================
    for (yr = strtYr; yr <= endYr; yr++) {
        year.push(yr);
    }
    
    //Getting the chart data
    $.each(flds, function () {
        var chartsData = [];
        // get fldnames for column pie data
        fldName.push(fldLabName[this]);
        // build charts data
        if ((MyDataChartType_2 == ChartTypeLine) || (MyDataChartType_2 == ChartTypeArea)) {
            //adding data for multiple years
            for (i = index; i < index + year.length; i++) {
                if (fldValue[this]) {

                    chartsData.push({
                        name: year[i - index],
                        y: fldValue[this][i],
                        drilldown: year[i - index]
                    });
                }
            }
        }  else 
            if (MyDataChartType_2 == ChartTypeColumn) {
            // adding data for year (index)
            fldName.push(fldLabName[this]);
            title += " " + year[0].toString();
            if (fldValue[this])
                chartsData.push({
                    name: fldLabName[this],
                    y: fldValue[this][index],
                    drilldown: fldLabName[this]
                });
        }
           
        chartsDataArray.push(chartsData);
    });

    // OK check for values that exceed Ymax
    var maxVal = 0;
    if (isStacked) {
        for (var j = 0; j < chartsDataArray[0].length; j++) {
            var totVal = 0;
            for (var i = 0; i < chartsDataArray.length; i++) {
                totVal += chartsDataArray[i][j].y;
            }
            if (totVal > maxVal) { maxVal = totVal; }
        }

    } else {
        for (var i = 0; i < chartsDataArray.length; i++) {
            for (var j = 0; j < chartsDataArray[i].length; j++) {
                var theVal = chartsDataArray[i][j].y;
                if (theVal > maxVal) { maxVal = theVal; }
            }
        }
    }
    if (maxVal > Ymax) { Ymax = maxVal; }

    //-----------------------
    $.each(flds, function () {

        // retrieve the parent for this field
        var fldParent = fldLabName[this];

        //Getting the dependent field names and pushing into an array
        if (subControls[this] != "")
            ctrls = (subControls[this]).toString().split(',');

        if (ctrls.length > 0) {
            if ((chartTypeCode == ChartTypeLine) || (chartTypeCode == ChartTypeArea)) {
                //getting the drill down data
                for (i = index; i < index + year.length; i++) {

                    drilldownData = [];

                    $.each(ctrls, function () {
                        name = fldLabName[this].toString();
                        drilldownData.push([name, fldValue[this][i]]);
                    });
                    drilldownDataArray.push(drilldownData);
                }
                //pushing drill down data into drill down series
                $.each(ctrls, function () {
                    for (i = 0; i < year.length; i++) {
                        drilldownSeries.push({
                            name: year[i],
                            id: year[i],
                            data: drilldownDataArray[i]
                        });
                    }
                });
            } else 
                if (chartTypeCode == ChartTypeColumn) {
                    //forming data of drilldown and pushing into an array
                    $.each(ctrls, function () {
                        var fld = fldLabName[this].toString();
                        drilldownData.push([fld, fldValue[this][index]]);

                    });
                    drilldownSeries.push({
                        name: fldParent,
                        id: fldParent,
                        data: drilldownData
                    });
                }
                
        }

    });

    //--------------------------------------------------------------------------------------------------------------------------
    // DAS
    // Create the chart
    var $myChart = {  //$('#' + divID).highcharts({

        chart: {
           
            renderTo: divID,
            type: 'area',
            plotBackgroundColor: "WhiteSmoke",

            plotBorderWidth: 1,
            marginRight: 20,
            style: chartFontStyle
        },

        title: {
            text: title,
            style: chartTitleFontStyle
        },

        xAxis: {
            type: 'category',

            labels: {
                step: xStep,
                enabled: MyXLabelEnabled,
                rotation: MyXLabelRotation,
              //  format: '{value} ',
                formatter: function () { return this.value },
                style: axisLabelFontStyle
            },
            lineColor: '#000000',
            lineWidth: 1.5,
            tickLength: 5,
            tickWidth: 2,
            tickinterval:xTicSize
        },

        yAxis: {
            min: Ymin,
            max: Ymax,
            title: {
                text: Yunits,
                style: axisTitleStyle,
            },
            labels: {
                step: 1,
                enabled: true,
                formatter: function () { var scaleval = this.value / Yscale; return scaleval.toString(); },
                style: axisLabelFontStyle
            },
            lineColor: '#000000',
            lineWidth: 1.75,
            gridLineColor: '#4F5557',
            gridLineWidth: 0.5,
            gridLineDashStyle: 'longdash',
            showEmpty: false,
            tickinterval:yTicSize,
        },

        legend: {
            enabled: true,
            borderColor: '#FFFFFF',
            style: legendFontStyle,
            itemStyle: legendFontStyle
        },
        colors: seriesColors,
        plotOptions: {
            series: {
                fillOpacity: MyFillOpacity,
                borderWidth: 0,
                dataLabels: {
                    enabled: false,
                },
               
                marker: {
                    enabled: false
                }
            },
            area: {
                stacking: MyStacking,
                
                lineWidth: 2,
                marker: {
                    enabled: false,
                }
            },
        },
        series: [{
            name: fldLabName[flds[0]],
            type: MyDataChartType,
            data:  chartsDataArray[0]
        }],

        drilldown: {
            series: drilldownSeries
        }
    };
    //-----------------------------------------------------------------------------------------------------------------------------
    for (count = 1; count < chartsDataArray.length; count++) {
        
        $myChart.series.push({
            name: fldLabName[flds[count]],
            type: MyDataChartType_2,
            data: chartsDataArray[count]
        });
    }
   var chart = new Highcharts.Chart($myChart);
}