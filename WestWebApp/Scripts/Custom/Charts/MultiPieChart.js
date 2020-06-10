function drawMultiPieChart($jsonObj, controlID, subcontrols, fldLabName, fldValue, providerName, index, Min, Max, Units) {
    var divID = $("#" + controlID).find("div[id*=ChartContainer]").attr('id');    
    var title = $("#" + controlID).attr("data-title");

    console.log($jsonObj, controlID, subcontrols, fldLabName, fldValue, providerName, index, Min, Max, Units);
    //return;
    
    //getting the chart type
    var chartType = $("#" + controlID).find("select[id*=ddlTypes]").find("option:selected").val();
    var plotOptions = true;
    
    console.log(chartType)

    if (chartType == 'pie')
        plotOptions = true;
    else
        plotOptions = false;
    
    //getting the provider
    var option = $("#" + controlID).find("select[id*=ddlfld]").find("option:selected").val();
    
    var fldnames = $("#" + controlID).attr("data-fld");

    //var $jsonObj = $.parseJSON(jsonData); //parsing the Input String as Json object
    
    // --------------------------------------------------------------------------------------------------
    var axisTitleStyle = {
        font: "18px 'Trebuchet MS', Verdana, sans-serif",
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
            fontSize: "1.5em",
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

    var chartsData = [[],[]];
    
    var flds = fldnames.split(',');

    var piecolors = {
        SUR: '#e41a1c',
        SAL: '#377eb8',
        GW: '#4daf4a',
        REC: '#984ea3',
        SURL: '#ff7f00',
        UD: '#ffff33',
        UDN: '#ffff33',
        AD: '#a65628',
        ID:'#f781bf',
        PD:'#999999'
    }

    //forming the series of chart data
    $.each(flds, function () {
        //looping through the controls to grt the parent controls field names
        if (fldValue[this] != undefined) {

            var fld = String(this);

            switch (fld) {

                case "SUR":
                case "SAL":
                case "GW":
                case "REC":
                case "SURL":
                    chartsData[0].push({
                        name: fldLabName[this],
                        y: fldValue[this][index],
                        color: piecolors[this]
                    });
                    console.log(this)
                    break;
                case "UD":
                case "AD":
                case "ID":
                case "PD":
                    chartsData[1].push({
                        name: fldLabName[this],
                        y: fldValue[this][index],
                        color: piecolors[this]
                    });
                    break;
             

            }
        }
    });
    console.log(chartsData)

    var $pieChart = $('#' + divID).highcharts({

        chart: {
            type: chartType,
            style: chartFontStyle
        },

        title: {
            text: title,
            style: chartTitleFontStyle
        },

        plotOptions: {
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
            type: 'pie',
            name: 'Resources',
            center: [125, null],
            size: 250,
            dataLabels: {
                enabled: false
            },
            showInLegend: true,
            data: chartsData[0]
        },
        {
            type: 'pie',
            name: 'Consumers',
            center: [425, null],
            size: 250,
            dataLabels: {
                enabled: false,
                style: {
                    color: 'black'
                }
            },
            showInLegend: true,
            data: chartsData[1]
        }]
    })    
}