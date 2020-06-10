/// <reference path="/Scripts/Custom/Charts/ChartTools.js" />
function drawAreaStackedChart(jsonData, controlID, subcontrols, fldLabName, fldValue, providerName, strtYr, endYr, index, MINes, MAXes, Units) {

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

    var chartsDataArray = [];
    var drilldownDataArray = [];
    var drilldownSeries = [];
    var prvdrs = [];
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
    if (MAXes != undefined) {
        Ymax = JudgeMax(flds, MAXes, true)
    }

    var isStacked = true;
    // DAS -------------------------------------------------------------------------------
    //Creating an array of years
    var year = [];
    // getting TicSizes
    var xTicSize = YearTicSize(strtYr, endYr);
    var yTicSize = VertTicSize(Ymin, Ymax);
    var xStep = JudgeXStep(strtYr, endYr, xTicSize);

    for (yr = strtYr; yr <= endYr; yr++)
        year.push(yr);


    //getting chart data
    $.each(flds, function () {
 
        var fld = fldLabName[this];

        //Getting the dependent field names and pushing into an array
        if (subcontrols[this] != "")
            ctrls = (subcontrols[this]).toString().split(',');

        $.each(fldValue[this], function () {
            var chartsData = [];
            var pcode = this.PVC;
            if (pcode != RegionCode) {
                prvdrs.push(providerName[this.PVC]);

                for (i = index; i < index + year.length; i++) {
                    chartsData.push({
                        name: year[i - index],
                        y: this.VALS[i],
                        drilldown: providerName[this.PVC] + year[i - index]
                    });
                }
            }
            chartsDataArray.push(chartsData);
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

        $.each(prvdrs, function (i) {
            
            for (j = 0; j < year.length; j++) {
                
                drilldownSeries.push({
                    name: this.toString(),
                    id: this + year[j],
                    type: 'column',
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
            type: 'area',
            plotBackgroundColor: null,
            plotBorderWidth: 1
        },

        title: {
            text: title
        },
        // was false DAS , I added step and format
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
            gridLineDashStyle: 'longdash',
            labels: {
                step: 1,
                enabled: true,
                formatter: function () { return addCommas(this.value.toString()); },
                style: axisLabelFontStyle
            }
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
            area: {
                stacking: 'normal',
                lineColor: '#666666',
                lineWidth: 1,
                marker: {
                    enabled: false,
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
            name: prvdrs[0],
            data: chartsDataArray[0]
        }],

        drilldown: {
            series: drilldownSeries
        }
    };
    // -----------------------------------------------------------------------------------------------
    for (count = 1; count < chartsDataArray.length; count++) {
        
        $myChart.series.push({
            name: prvdrs[count],
            data: chartsDataArray[count]
        });
    }

    var chart = new Highcharts.Chart($myChart);
}