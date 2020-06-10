
/// <reference path="/Scripts/Custom/Charts/ChartTools.js" />
function drawDrillDownPieColumnChartMP(jsonData, controlID, subControls, fldLabName, fldValue, providerName, index, Min, Max, Units) {
    var divID = $("#" + controlID).find("div[id*=ChartContainer]").attr('id');
    var fldnames = $("#" + controlID).attr("data-fld");
    var title = $("#" + controlID).attr("data-title");
    
    //getting the chart type    
    var chartType = $("#" + controlID).find("select[id*=ddlTypes]").find("option:selected").val();
    var plotOptions=false;
   
    if (chartType == 'pie')
        plotOptions = true;
    else
        plotOptions = false;

    
    var $jsonObj = $.parseJSON(jsonData); //parsing the Input String as Json object

    // --------------------------------------------------------------------------------------------------
    var axisTitleStyle = {
        font: "14px 'Trebuchet MS', Verdana, sans-serif",
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
        legendFontStyle = {
            fontSize: "1em"
        };
        tooltipFontStyle = { fontSize: "1em" };
        chartFontStyle = { fontSize: "1em" };
    }
    
    var chartsData = [];
    var drilldownData = [];
    var drilldownSeries = [];
   
    var flds = fldnames.split(',');
    var fld;
    // DAS --------------------------------------------------------------
    // This code is used to scale the y axis, and to assign units
    var Ymin = 0;
    var Ymax = 5000000;
    var Yunits = 'Unknown'

    if (Units != undefined) {
        Yunits = Units[flds[0]];
    }

    if (Max != undefined) {
        Ymax = JudgeMax(flds, Max, false)
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
    // DAS -------------------------------------------------------------------------------

    var ctrls = [];
    
    //Getting the chart data
    $.each(flds, function () {

        fld = fldLabName[this];

        //Getting the dependent field names and pushing into an array
        if (subControls[this] != "")
            ctrls = subControls[this].toString().split(',');

        //adding data for multiple providers
        $.each(fldValue[this], function () {
            if (this.PVC) {
                var pCode = this.PVC;
                if (pCode != RegionCode) {
                    chartsData.push({
                        name: providerName[this.PVC],
                        y: this.VALS[index],
                        drilldown: this.PVC
                    });
                }
            }
        });
    });
    
    //if having dependent fields
    if (ctrls.length > 0) {
        
        //getting the drill down data
        for (i = 0; i < chartsData.length; i++) {

            drilldownData = [];

            $.each(ctrls, function () {
                name = fldLabName[this].toString();
                drilldownData.push([name, fldValue[this][i].VALS[index]]);
            });
            
            //pushing drill down data into drill down series    
            $.each(ctrls, function (j) {
                if (fldValue[this][i].PVC) {
                    var pCode = fldValue[this][i].PVC;
                    if (pCode != RegionCode) {
                        pname = pCode; //providerName[fldValue[this][i].PVC];
                        drilldownSeries.push({
                            name: pname,
                            id: fldValue[this][i].PVC,
                            data: drilldownData
                        });
                    }
                }
            });           
        }
    }
    // Population by Provider
    // Object Two
    // DAS
    // Create the chart
    var $myChart = $('#' + divID).highcharts({

        chart: {
            type: chartType,
            style: chartFontStyle
        },

        title: {
            text: title,
            style: chartTitleFontStyle
        },

        xAxis: {
            type: 'category',
            lineColor: '#000000',
            lineWidth: 1.5,
            tickLength: 5,
            tickWidth: 2,
            style: axisLabelFontStyle
        },

        yAxis: {
            min: Ymin,
            max: Ymax,
            title: {
                text: Yunits,
                style: axisTitleStyle
            },
            lineColor: '#000000',
            lineWidth: 1.5,
            gridLineColor: '#4F5557',
            gridLineWidth: 0.5,
            gridLineDashStyle: 'longdash',
            showEmpty: false,
            labels: {
                step: 1,
                enabled: true,
                formatter: function () { var scaleval = this.value / Yscale; return scaleval.toString(); },
                style: axisLabelFontStyle
            },
        },

        legend: {
            enabled: true,
            borderColor: '#FFFFFF',
            itemStyle: legendFontStyle
        },

        plotOptions: {
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: false,
                    format: '{point.y:.1f}'
                },
                showInLegend: true
            }
        },

        tooltip: {
            headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
            pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b> of total<br/>'
        },

        series: [{
            name: fld,
            colorByPoint: true,
            data: chartsData
        }],

        drilldown: {
            series: drilldownSeries
        }
    })
    var chart = new Highcharts.Chart($myChart);
}