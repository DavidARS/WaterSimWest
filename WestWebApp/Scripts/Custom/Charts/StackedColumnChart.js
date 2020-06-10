// <reference path="/Scripts/Custom/Charts/StackedColumnChart.js" />

function drawColumnStackedChart(jsonData, controlID, subcontrols, fldLabName, fldValue, strtYr, endYr, index, MINes, MAXes, Units) {


    var divID = $("#" + controlID).find("div[id*=ChartContainer]").attr('id');
    var fldnames = $("#" + controlID).attr("data-fld");
    var title = $("#" + controlID).attr("data-title");
    
    var option = $("#" + controlID).find("select[id*=ddlfld]").find("option:selected").val();
    
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
    var flds = fldnames.split(',');
    // DAS --------------------------------------------------------------
    // This code is used to scale the y axis, and to assign units
    var Ymin = 0;
    var Ymax = 2000000;
    var Yunits = 'Unknown';
    // of if Unt is is defined and is array  grab the unit of the first fld
    if (Units != undefined) {
        Yunits = Units[flds[0]];
    }
    if (MAXes != undefined) {
        Ymax = JudgeMax(flds, MAXes, true)
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

    // QUAY EDIT BEGIN 3/13/14 3:30 am
    //testMax = 0;
    // QUAY EDIT END 3/13/14 3:30 am


    //getting charts data
    $.each(flds, function () {

        var fld = fldLabName[this];

        if (fldValue[this])
            $.each(fldValue[this], function () {

                if (this.PVC== option) {
                    //// QUAY EDIT BEGIN 3/13/14 3:30 am
                    //biggest = maxval = Math.max.apply(Math, this.VALS);
                    //testMax += biggest;
                    //// QUAY EDIT END 3/13/14 3:30 am
                    var val = [];

                    for (i = index; i < index + year.length; i++) {
                        val.push(this.VALS[i]);
                    }

                    chartsData.push([fld, val]);
                }
            });
    });

    var Testmax = GetChartMax(chartsData, true);
    if (Testmax > Ymax) { Ymax = Testmax; }
    // QUAY EDIT BEGIN 3/13/14 3:30 am
    //if (testMax > Ymax) Ymax = testMax + (testMax / 10);
    // QUAY EDIT END 3/13/14 3:30 am


    // Water Supply By Catagory
    // DAS
    // Create the chart
    var $myChart = {

        chart: {
            renderTo: divID,
            type: 'column',
            plotBackgroundColor: null,
            plotBorderWidth: 1,
            style: chartFontStyle
        },

        title: {
            text: title,
            style: chartTitleFontStyle
        },

        xAxis: {
            categories: year,
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
            
            minorTickLength: 3,
            minorTickWidth: 1,
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
            gridLineDashStyle: 'longdash'
           
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

        plotOptions: {
            column: {
                stacking: 'normal',
                lineColor: '#666666',
                lineWidth: 1,
                marker: {
                    lineWidth: 1,
                    lineColor: '#666666'
                }
            },
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: false,
                }
            }
        },

        series: [{
            name: chartsData[0][0],
            data: chartsData[0][1]
        }]
    };

    for (count = 1; count < chartsData.length; count++) {
        $myChart.series.push({
            name: chartsData[count][0],
            data: chartsData[count][1]
        });
    }    
    var chart = new Highcharts.Chart($myChart);    
}