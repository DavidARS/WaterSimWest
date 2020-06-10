function drawDrillDownLineChartBO($jsonObj, controlID, subControls, fldLabName, fldValue, strtYr, endYr, index, Min, Max, Unt) {

    var divID = $("#" + controlID).find("div[id*=ChartContainer]").attr('id');
    var fldnames = $("#" + controlID).attr("data-fld");
    var title = $("#" + controlID).attr("data-title");

    // DO NOT NEEED TO DO THIS MULTIPLE TIMEs
    // QUAY EDIT 2 8 16
    //var $jsonObj = $.parseJSON(jsonData); //parsing the Input String as Json object
    // --------------------------------------------------------------------------------------------------

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
    var drilldownDataArray = [];
    var drilldownSeries = [];

    var flds = fldnames.split(',');
    // DAS --------------------------------------------------------------
    // This code is used to scale the y axis, and to assign units
    var Ymin = 0;
    var Ymax = 25000000;
    var Yunits = 'Acre Feet per Year'

    if (Max != undefined) {
        Ymax = JudgeMax(flds,Max,false)
    }
    // DAS ----------------------------------------------------------------------------

    var ctrls = [];

    var year = [];

    // Calc Tic Sizes
    var xTicSize = YearTicSize(strtYr, endYr);
    var yTicSize = VertTicSize(Ymin, Ymax);
    var xStep = JudgeXStep(strtYr, endYr, xTicSize);

    for (yr = strtYr; yr <= endYr; yr++) {
        year.push(yr);
    }
    
    //Getting the chart data
    $.each(flds, function () {
        
        //adding data for multiple providers
        for (i = index; i < index + year.length; i++) {

            if (fldValue[this])
                chartsData.push({
                    name: year[i - index],
                    y: fldValue[this][i],
                    drilldown: year[i - index]
                });
        }
    });
    
    $.each(flds, function () {

        //Getting the dependent field names and pushing into an array
        if (subControls[this] != "")
            ctrls = (subControls[this]).toString().split(',');

        if (ctrls.length > 0) {

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
        }

    });
 
    //--------------------------------------------------------------------------------------------------------------------------
    // DAS
    // Line chart, flows on the Colorado River and the Salt Verde Rivers
    // Create the chart
    var $myChart = $('#' + divID).highcharts({

        chart: {
            type: 'column',
            plotBackgroundColor: null,
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
                enabled: true,
                rotation: 60,
                format: '{value} ',
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
                formatter: function () { return addCommas(this.value.toString()); },
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

        plotOptions: {
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: false,
                },
                marker: {
                    enabled: false
                }
            }
        },

        series: [{
            name: fldLabName[flds[0]],
            type: 'line',
            data: chartsData
        }],

        drilldown: {
            series: drilldownSeries
        }
    });
    //-----------------------------------------------------------------------------------------------------------------------------
}