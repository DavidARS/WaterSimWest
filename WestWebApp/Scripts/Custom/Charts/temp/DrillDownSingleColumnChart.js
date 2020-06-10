function drawDrillDownSingleColumnChart(jsonData, controlID, subControls, fldLabName, fldValue, index,Min, Max, Unt) {
    // DAS --------------------------------------------------------------
    // This code is used to scale the y axis, and to assign units
    var Ymin = 0;
    var Ymax = 25000000;
    var Yunits = 'Unknown'

    if (Max == undefined)
        Ymax = 25000000;
    else
        Ymax = Max;
    // DAS -------------------------------------------------------------------------------
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
    var fldName = "";
    var flds = fldnames.split(',');

    var ctrls = [];

    //Getting the chart data
    $.each(flds, function () {

        fldName = fldLabName[this];

        if (subControls[this] != "")
            ctrls = (subControls[this]).toString().split(',');
        if (fldValue[this])
            chartsData.push({
                name: fldName,
                y: fldValue[this][0].VALS[index],
                drilldown: fldName
            });
    });

    if (ctrls.length > 0) {

        //getting the drill down data
        for (i = 0; i < ctrls.length; i++) {

            drilldownData = [];

            $.each(ctrls, function () {
                name = fldLabName[this].toString();
                drilldownData.push([name, fldValue[this][i].VALS[index]]);
            });
            drilldownDataArray.push(drilldownData);//pushing the drill down data into an array
        }

        //pushing drill down data into drill down series
        for (i = 0; i < ctrls.length; i++) {

            drilldownSeries.push({
                name: ctrls[i],
                id: fldName,
                data: drilldownDataArray[i]
            });
        }
    }

    // Create the chart
    var $myChart = $('#' + divID).highcharts({

        chart: {
            type: 'column',
            style: chartFontStyle
        },

        title: {
            text: title,
            style: chartTitleFontStyle
        },

        subtitle: {
            text: 'Click the columns to view sub-controls values.',
            style: subTitleFontStyle
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
                style: axisTitleStyle
            },
            lineColor: '#000000',
            lineWidth: 1.5,
            gridLineColor: '#4F5557',
            gridLineWidth: 0.5,
            gridLineDashStyle: 'longdash'
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
                }
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