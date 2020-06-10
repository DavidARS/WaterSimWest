// Generic Area Chart modified by DAS
function drawAreaChart(jsonData, controlID, subcontrols, fldLabName, fldValue, strtYr, endYr, index, Min, Max, Unt) {

  

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
    var subTitleFontStyle = {
        color: '#000000',
        font: '13px "Trebuchet MS", Verdana, sans-serif'
    };
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
        subTitleFontStyle = {
            fontSize: "1.3em",
            color: '#000000'
        };
        legendFontStyle = { fontSize: "1em" };
        tooltipFontStyle = { fontSize: "1em" };
        chartFontStyle = { fontSize: "1em" };
    }

    var chartsDataArray = [];
    var drilldownSeries = [];
    var ctrls = [];
    var drilldownDataArray = [];
    var flds = fldnames.split(',');
    // DAS --------------------------------------------------------------
    // This code is used to scale the y axis, and to assign units
    var Ymin = 0;
    var Ymax = 125000000;
    var Yunits = 'Acre Feet'

    if (Max != undefined) {
        Ymax = JudgeMax(flds, Max, true)
    }

    // DAS -------------------------------------------------------------------------------
    //Creating an array of years
    var year = [];
    // getting TicSizes
    var xTicSize = YearTicSize(strtYr, endYr);
    var yTicSize = VertTicSize(Ymin, Ymax);
    var xStep = JudgeXStep(strtYr, endYr, xTicSize);

    for (yr = strtYr; yr <= endYr; yr++)
        year.push(yr);    
    
    $.each(flds, function () {

            var chartsData = [];            
        
            for (i = index; i < index + year.length; i++) {
                if (fldValue[this])
                    chartsData.push({
                        name: year[i - index],
                        y: fldValue[this][i],
                        drilldown: year[i - index]
                    });
            }
            chartsDataArray.push(chartsData);        
    });

    $.each(flds, function () {

        if (subcontrols[this]) {

            //Getting the dependent field names and pushing into an array
            if (subcontrols[this] != "")
                ctrls = (subcontrols[this]).toString().split(',');

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
        }
    });

    var Whatthe = chartsDataArray[0];

    // Create the chart
    var $myChart = {
        colors: ["#2b908f", "#90ee7e", "#f45b5b", "#7798BF", "#aaeeee", "#ff0066", "#eeaaee",
            "#55BF3B", "#DF5353", "#7798BF", "#aaeeee"],

        chart: {
           
            renderTo: divID,
            type: 'area',
            backgroundColor: {
                linearGradient: { x1: 0, y1: 0, x2: 1, y2: 1 },
                stops: [
                   [0, '#2a2a2b'],
                   [1, '#3e3e40']
                ]

            },
            plotBackgroundColor: "white",
            plotBorderWidth: 1,
            style: chartFontStyle
          
        },

        title: {
           
            text: title,
            align: 'center',
            style: chartTitleFontStyle
        },

        xAxis: {
            type: 'category',
            tickColor: '#AAA',
            labels: {
                step: xStep,
                enabled: true,
                rotation: 60,
                format: '{value}',
                style: axisLabelFontStyle
            },
            lineColor: '#000000',
            lineWidth: 1.5,
            tickLength: 5,
            tickWidth: 2,
            tickinterval: xTicSize,
            
        },
        subtitle: {
            text: '(Acre-feet)',
            style: subTitleFontStyle,
            align: 'center'
        },
        yAxis: {
           // min: Ymin,
            //max: Ymax,
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
            tickinterval: yTicSize,
            
    },

        legend: {
            enabled: true,
            borderColor: '#FFFFFF',
            itemStyle: legendFontStyle
        },

        tooltip: {
            formatter: function () {
                return '<b>' + this.point.name + '</b><br/>' +
                    this.series.name + ': ' + this.y + '<br/>';
            }
        },

        plotOptions: {
            series: {
                //fillOpacity: 0.5,
                borderWidth: 0,
                dataLabels: {
                    color: '#B0B0B3'
                    //enabled: false,
                },
                marker: {
                    enabled: false,
                    lineWidth: 1,
                    lineColor: '#666666'
                },
            },
            area: {
        stacking: 'normal',
        lineColor: '#666666',
        lineWidth: 1,
        marker: {
            lineWidth: 1,
            lineColor: '#666666'
        }
    }
        },
       
        series: [{
            name: fldLabName[flds[0]],
            type: 'area',
            data: Whatthe
            //data: chartsDataArray[0]
        }],

        drilldown: {
            series: drilldownSeries
        },
        background2: '#F0F0EA'
};
   // maskColor: 'rgba(255,255,255,0.3)'
    //-----------------------------------------------------------------------------------------------------------------------------
    for (count = 1; count < chartsDataArray.length; count++) {
        
        $myChart.series.push({
            name: fldLabName[flds[count]],
            type: 'area',
            data: chartsDataArray[count]
        });
    }
    var chart = new Highcharts.Chart($myChart);
}