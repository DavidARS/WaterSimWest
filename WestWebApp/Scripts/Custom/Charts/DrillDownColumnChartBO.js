function drawDrillDownColumnChartBO($jsonObj, controlID, subcontrols, fldLabName, fldValue, index, Min, Max, Unt) {



    var divID = $("#" + controlID).find("div[id*=ChartContainer]").attr('id');    
    var fldnames = $("#" + controlID).attr("data-fld");
    var title = $("#" + controlID).attr("data-title");

    // DO NOT NEEED TO DO THIS MULTIPLE TIMEs
    // QUAY EDIT 2 8 16
    //var $jsonObj = $.parseJSON(jsonData); //parsing the Input String as Json object
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
    var fldName = [];
    var flds = fldnames.split(',');
    // DAS --------------------------------------------------------------
    // This code is used to scale the y axis, and to assign units
    var Ymin = 0;
    var Ymax = 25000000;
    var Yunits = 'Acre Feet'

    if (Max != undefined) {
        Ymax = JudgeMax(flds, Max, true)
    }
    // getting TicSizes
    var xTicSize = YearTicSize(strtYr, endYr);
    var yTicSize = VertTicSize(Ymin, Ymax);
    var xStep = JudgeXStep(strtYr, endYr, xTicSize);
    // DAS -------------------------------------------------------------------------------
    var ctrls = [];
        
    //Getting the chart data
    $.each(flds, function () {

        fldName.push(fldLabName[this]);

        if (fldValue[this])
            chartsData.push({
                name: fldLabName[this],
                y: fldValue[this][index],
                drilldown: fldLabName[this]
            });
    });

    $.each(flds, function () {

        ctrls = [];
        drilldownData = [];

        var fldParent = fldLabName[this];
        
        if (subcontrols[this] != "")
            ctrls = subcontrols[this].toString().split(',');
        
        if (ctrls.length > 0) {

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
    });


    // Create the chart
    var $myChart = $('#' + divID).highcharts({

        chart: {
            type: 'column',
            plotBackgroundColor: null,
            plotBorderWidth: 1
        },

        title: {
            text: title,
            style: chartTitleFontStyle
        },

        xAxis: {
            type: 'category',
            tickColor: '#AAA',
            labels: {
                step:xStep,
                enabled: false,
                rotation: 60,
                format: '{value}',
                style: axisLabelFontStyle
           
            },
            lineColor: '#000000',
            lineWidth: 1.5,
            tickLength: 5,
            tickWidth: 2,
            tickinterval:xTicSize,
            minorTickLength: 3,
            minorTickWidth: 1
          
        },

        yAxis: {
            min: Ymin,
            max: Ymax,
            title: {
                text: Yunits,
                style: axisTitleStyle,
            },
            lineColor: '#000000',
            lineWidth: 1.5,
            gridLineColor: '#4F5557',
            gridLineWidth: 0.5,
            gridLineDashStyle: 'longdash',
            labels: {
                step: 1,
                enabled: true,
                formatter: function () { return addCommas(this.value.toString()); },
                style: axisLabelFontStyle
            },
            tickinterval:yTicSize,
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
                    enabled: false
                },
                showInLegend: true
            }
        },

        tooltip: {
            headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
            pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b> of total<br/>'
        },

        series: [{
            name: fldName,
            data: chartsData
        }],

        drilldown: {
            series: drilldownSeries
        }
    })
}