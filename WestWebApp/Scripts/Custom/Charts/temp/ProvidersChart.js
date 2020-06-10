/// <reference path="/Scripts/Custom/Charts/ChartTools.js" />
function drawProvidersChart(jsonData, controlID, subcontrols, fldLabName, fldValue, providerName, strtYr, endYr, index, Min, Max, Units, seriescolors,chartTypeCode) {

    var divID = $("#" + controlID).find("div[id*=ChartContainer]").attr('id');
    var fldnames = $("#" + controlID).attr("data-fld");
    var title = $("#" + controlID).attr("data-title");

    var $jsonObj = $.parseJSON(jsonData); //parsing the Input String as Json object
    //
    var seriesColors = seriescolors;
    // --------------------------------------------------------------------------------------------------
    var axisTitleStyle = {
        font: "12px 'Trebuchet MS', Verdana, sans-serif",
        fontcolor: '#000000'
    };
    var axisLabelFontStyle = {};
    var chartTitleFontStyle = {};
    var subTitleFontStyle = {};
    var legendFontStyle = {};
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
        legendFontStyle = { fontSize: "1em" };
        tooltipFontStyle = { fontSize: "1em" };
        chartFontStyle = { fontSize: "1em" };
    }
    // --------------------------------------------------------------------------------------------------
    // setup the chart
    var MyDataChartType = "";
    var MyXLabelRotation = 0;
    var MyXLabelEnabled = true;
    var MyStacking = "";
    var MyFillOpacity = 1;
    var isStacked = false;
    switch (chartTypeCode) {
        case ChartTypeLine:
            MyDataChartType = ChartTypeLine;
            MyStacking = "";
            MyXLabelRotation = 60;
            MyXLabelEnabled = true;
            break;
        case ChartTypeLineStacked:
            MyDataChartType = ChartTypeLine;
            MyStacking = "normal";
            MyXLabelRotation = 60;
            MyXLabelEnabled = true;
            isStacked = true;
            break;
        case ChartTypeColumn:
            MyDataChartType = ChartTypeColumn;
            MyStacking = "";
            MyXLabelRotation = 0;
            MyXLabelEnabled = false;
            break;
        case ChartTypeColumnStacked:
            MyDataChartType = ChartTypeColumn;
            MyStacking = "normal";
            MyXLabelRotation = 60;
            MyXLabelEnabled = true;
            isStacked = true;
            break;
        case ChartTypeArea:
            MyDataChartType = ChartTypeArea;
            MyStacking = "";
            MyFillOpacity = 0.3;
            MyXLabelRotation = 60;
            MyXLabelEnabled = true;
            break;
        case ChartTypeAreaStacked:
            MyDataChartType = ChartTypeArea;
            MyStacking = "normal";
            MyXLabelRotation = 60;
            MyXLabelEnabled = true;
            isStacked = true;
            break;
        case Polar:

            break;
    }
    var chartsDataArray = [];
    var drilldownDataArray = [];
    var drilldownSeries = [];
    var prvdrs = [];
    var fldValueNames = [];
    var ctrls = [];
    var flds = fldnames.split(',');
    // DAS --------------------------------------------------------------
    // This code is used to scale the y axis, and to assign units
    var Ymin = 0;
    var Ymax = 25000000;
    var Yunits = 'Acre Feet per Year';
    // of if Unt is is defined and is array  grab the unit of the first fld
    if (Units != undefined) {
        Yunits = Units[flds[0]];
    }
    if (Max != undefined) {
        Ymax = GetMax(flds, Max)
    }
    if (Min != undefined) {
        Ymin = GetMin(flds, Min)
    }
    // 06.26.15 DAS- this code makes it easier to see the individual water provider-level responses
    if (isStacked) {
        var pcode = $("#" + controlID).find("select[id*=ddlfld]").find("option:selected").val();
        if (pcode != "reg") {
            Ymax = Ymax * 0.3;
        }
    }
    
    //
    // DAS -------------------------------------------------------------------------------
    //Creating an array of years
    var year = [];
    // getting TicSizes
    var xTicSize = YearTicSize(strtYr, endYr);
    var yTicSize = VertTicSize(Ymin, Ymax);
    var xStep = JudgeXStep(strtYr, endYr, xTicSize);

    // calc Scale
    var Yscale = 1;
    if (Ymax > 1000000) { Yscale = 1000000; Yunits += " (million)"; }
    else
        if (Ymax > 1000) { Yscale = 1000; Yunits += " (thousand)"; }

    // Fill Year Array
    for (yr = strtYr; yr <= endYr; yr++)
        year.push(yr);


    //getting chart data
    $.each(flds, function () {
 
        fldValueNames.push(fldLabName[this]);

        //Getting the dependent field names and pushing into an array
        if (subcontrols[this] != "")
            ctrls = (subcontrols[this]).toString().split(',');

        $.each(fldValue[this], function () {
            var chartsData = [];
            // make sure this is provider data
            if (this.PVC) {
                // get the provder code
                var pcode = this.PVC;
                // do not process if this is a region code and there are other codes in the list
                if ((pcode != RegionCode)||(providerName.doreg)) {
                    // ok now get the name of this provider
                    var pname = providerName[this.PVC];
                    // if this pcode was not in provider code list, skip this
                    if (pname != undefined) {
                        // push thsi into the list of prvdrs so we can add to series later
                        prvdrs.push(pname);
                        // ok get each year value and put inot chart data array
                        for (i = index; i < index + year.length; i++) {
                            var dataval = this.VALS[i];
                            if (dataval < -2000000000) dataval = 0;
                            chartsData.push({
                                name: year[i - index],
                                y:dataval ,
                                // add the name of this drill down
                                drilldown: pname + year[i - index]
                            });
                        }
                        // add this flds provider data
                        chartsDataArray.push(chartsData);
                    }
                }
            }
            
        });
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


    var YValueNames = [];
    // OK decide if this is a MFOP or OFMP, if OF then series is providers, if MP series is fields 
    
    if (flds.length >1) {
        YValueNames = fldValueNames;
    } else {
        YValueNames = prvdrs;
    }

    if (ctrls.length > 0) {
        
        for (i = 0; i < prvdrs.length; i++) {

            //getting the drill down data
            for (j = 0; j < year.length; j++) {

                var drilldownData = [];

                //looping through the controls to get the dependent field data
                $.each(ctrls, function () {
                    name = fldLabName[this].toString();
                    drilldownData.push([name, fldValue[this][i].VALS[j]]);
                });
                drilldownDataArray.push(drilldownData);//pushing the drill down data into an array
            }
        }
        
        //pushing drill down data into drill down series
        var count = 0;

 

        $.each(YValueNames, function (i) {
            
            for (j = 0; j < year.length; j++) {
                
                drilldownSeries.push({
                    name: this.toString(),
                    id: this + year[j],
                    type: MyDataChartType,
                    data: drilldownDataArray[count]
                });
                count++;
            }
        });        
    }

    // Demand TAB DAS --------------------------------------------------------------------
    // Create the chart
    var $myChart = {

        chart: {
            renderTo: divID,
            type: MyDataChartType,
            plotBackgroundColor: null,
            plotBorderWidth: 1,
            marginRight: 20,
            style: chartFontStyle
        },

        title: {
            text: title,
            style: chartTitleFontStyle
        },
        // was false DAS , I added step and format
        xAxis: {
            type: 'category',
            tickColor: '#AAA',
            labels: {
                step: xStep,
                enabled: MyXLabelEnabled,
                rotation: MyXLabelRotation,
                format: '{value}',
                style: axisLabelFontStyle
           
            },
            lineColor: '#000000',
            lineWidth: 1.5,
            tickLength: 5,
            tickWidth: 2,
            minorTickLength: 3,
            minorTickWidth: 1,
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
            lineWidth: 1.5,
            gridLineColor: '#4F5557',
            gridLineWidth: 0.5,
            gridLineDashStyle: 'longdash',
            showEmpty: false,
            tickinterval:yTicSize,
        },

        legend: {
            enabled: true,
            borderColor: '#FFFFFF',
            itemStyle: legendFontStyle
        },

        tooltip: {
            formatter: function () {
                return '<b>' + this.x + '</b><br/>' +
                    this.series.name + ': ' + this.y + '<br/>' +
                    'Total: ' + this.point.stackTotal;
            }
        },
        colors: seriesColors,
        plotOptions: {
            series: {
                fillOpacity: MyFillOpacity,
                borderWidth: 0,
                stacking: MyStacking,

                dataLabels: {
                    enabled: false,
                },
                marker: {
                    enabled: false
                }
            },
            area: {
                stacking: MyStacking,
                lineColor: '#666666',
                lineWidth: 1,
                marker: {
                    enabled: false,
                }
            },
        },

        series: [{
            name: YValueNames[0],
            data: chartsDataArray[0]
        }],

        drilldown: {
            series: drilldownSeries
        }
    };
    // -----------------------------------------------------------------------------------------------
    for (count = 1; count < chartsDataArray.length; count++) {
        
        $myChart.series.push({
            name: YValueNames[count],
            data: chartsDataArray[count]
        });
    }

    var chart = new Highcharts.Chart($myChart);
}