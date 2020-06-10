function drawDrillDownPieColumnChartMF(jsonData, controlID, subcontrols, fldLabName, fldValue, providerName, index, Min, Max, Units) {
    var divID = $("#" + controlID).find("div[id*=ChartContainer]").attr('id');    
    var title = $("#" + controlID).attr("data-title");
    
    //getting the chart type
    var chartType = $("#" + controlID).find("select[id*=ddlTypes]").find("option:selected").val();
    var plotOptions = false;

    if (chartType == 'pie')
        plotOptions = true;
    else
        plotOptions = false;
    
    //getting the provider
    var option = $("#" + controlID).find("select[id*=ddlfld]").find("option:selected").val();
    
    var fldnames = $("#" + controlID).attr("data-fld");

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
    var drilldownSeries = [];
    var drilldownData = [];
    var ctrls = Array();    
    
    var flds = fldnames.split(',');
    // DAS --------------------------------------------------------------
    // This code is used to scale the y axis, and to assign units
    var Ymin = 0;
    var Ymax = 150000;
    var Yunits = 'People'
    //if (Units != undefined) {
    //    Yunits = Units[flds[0]];
    //}
    //if (Max != undefined) {
    //    Ymax = JudgeMax(flds, Max, false)
    //}
    // DAS -------------------------------------------------------------------------------

    //forming the series of chart data
    $.each(flds, function () {
        
        var fld = fldLabName[this];

        //looping through the controls to grt the parent controls field names
        if (fldValue[this])
            $.each(fldValue[this], function () {
                if (this.PVC == option) {
  
                        chartsData.push({
                            name: fld,
                            y: this.VALS[index],
                            drilldown: fld
                        });
                    
                }
            });
    });
    
    $.each(flds, function () {
        
        ctrls = [];
        drilldownData = [];
        
        var fldParent = fldLabName[this];

        if (subcontrols[this])
            if (subcontrols[this] != "")
                ctrls = subcontrols[this].toString().split(',');
        
        if (ctrls.length > 0) {
            
            //forming data of drilldown and pushing into an array
            $.each(ctrls, function () {

                var fld = fldLabName[this].toString();
                
                $.each(fldValue[this], function () {

                        drilldownData.push([fld, this.VALS[index]]);
                    
                });
            });
            
            drilldownSeries.push({
                name: fldParent,
                id: fldParent,
                data: drilldownData
            });
        }
    });
    
    //Declaring a chart Object
    var $pieChart = $('#' + divID).highcharts({

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
                style: axisLabelFontStyle,
            },
            lineColor: '#000000',
            lineWidth: 1.5,
            gridLineColor: '#4F5557',
            gridLineWidth: 0.5,
            gridLineDashStyle: 'longdash',
            showEmpty: false
        },

        plotOptions: {
            series: {
                dataLabels: {
                    enabled: false,
                    format: '{point.name}: {point.y:.1f}'
                },
                showInLegend: true
            }
        },

        legend: {
            enabled: true,
            borderColor: '#FFFFFF',
            itemStyle: legendFontStyle
        },

        tooltip: {
            headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
            pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b> of total<br/>'
        },

        series: [{
            name: providerName[option],
            colorByPoint: true,
            data: chartsData
        }],

        drilldown: {
            series: drilldownSeries
        }
    })    
}