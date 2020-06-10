// Generic Area Chart modified by DAS
function drawDoublePieChart(jsonData, controlID, subcontrols, fldLabName, fldValue, strtYr, endYr, index, Min, Max, Unt) {

  

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
    var chart = new Highcharts.Chart({

        chart: {
            renderTo: 'container',
            type: 'pie'
        },
        title: {
            text: 'Group Name'
        },
        series: [{
            data: [29.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4],
            center: ['20%'],
            name: 'foo'
        }, {
            data: [29.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4],
            center: ['80%'],
            name: 'bar'
        }],
        plotOptions: {
            pie: {
                dataLabels: {
                    enabled: false
                }
            }
        }
    });
   // maskColor: 'rgba(255,255,255,0.3)'
    //-----------------------------------------------------------------------------------------------------------------------------
    //for (count = 1; count < chartsDataArray.length; count++) {
        
    //    $myChart.series.push({
    //        name: fldLabName[flds[count]],
    //        type: 'area',
    //        data: chartsDataArray[count]
    //    });
    //}
    //var chart = new Highcharts.Chart($myChart);
}