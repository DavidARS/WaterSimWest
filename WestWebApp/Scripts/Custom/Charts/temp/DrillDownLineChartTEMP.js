function drawDrillDownLineChartTEMP(jsonData, controlID, subControls, fldLabName, fldValue, providerName, strtYr, endYr, index, Min, Max, Unt) {

    // DAS --------------------------------------------------------------
    // This code is used to scale the y axis, and to assign units
    var Ymin = 0;
    var Ymax = 25000000;
    var Yunits = 'Acre Feet'

    if (Max == undefined)
        Ymax = 25000000;
    else
        Ymax = Max;
    // DAS -----------------------------------------------------------------------------
    var divID = $("#" + controlID).find("div[id*=ChartContainer]").attr('id');
    var fldnames = $("#" + controlID).attr("data-fld");
    var title = $("#" + controlID).attr("data-title");

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
    var drilldownDataArray = [];
    var drilldownSeries = [];
    var pcd;
    var flds = fldnames.split(',');

    var ctrls = [];

    var year = [];

    for (yr = strtYr; yr <= endYr; yr++)
        year.push(yr);
    
    //Getting the chart data
    $.each(flds, function () {

        pcd = providerName[fldValue[this][0].PVC];
        
        if (subControls[this.FLD] != "")
            ctrls = (subControls[this]).toString().split(',');

        //adding data for multiple providers
        for (i = index; i < index + year.length; i++) {

            if (fldValue[this])
                chartsData.push({
                    name: year[i - index],
                    y: fldValue[this][0].VALS[i],
                    drilldown: year[i - index]
                });
        }
    });

    if (ctrls.length > 0) {

        //getting the drill down data
        for (i = index; i < index + year.length; i++) {

            drilldownData = [];

            $.each(ctrls, function () {
                name = fldLabName[this].toString();
                drilldownData.push([name, fldValue[this][0].VALS[i]]);
            });

            //pushing the drill down data into an array
            drilldownSeries.push({
                name: year[i-index],
                id: year[i - index],
                data: drilldownData
            });
        }
    }

    // Create the chart
    var $myChart = $('#' + divID).highcharts({

        chart: {
            type: 'column',
            plotBackgroundColor: null,
            plotBorderWidth: 1,
            style:chartFontStyle
        },

        title: {
            text: title,
            style: chartTitleFontStyle
        },

        xAxis: {
            type: 'category',
            tickColor: '#AAA',
            labels: {
                step: 5,
                enabled: true,
                rotation: 60,
                format: '{value}',
                style: axisLabelFontStyle
           
            },
            lineColor: '#000000',
            lineWidth: 1.5,
            tickLength: 5,
            tickWidth: 2,
            
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
            showEmpty: false
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
                    enabled: false,
                    lineWidth: 1,
                    lineColor: '#666666'
                },
            }
        },

        series: [{
            name: pcd,
            type: 'line',
            data: chartsData
        }],

        drilldown: {
            series: drilldownSeries
        }
    });
}