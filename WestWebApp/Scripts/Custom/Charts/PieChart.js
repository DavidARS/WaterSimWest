function drawPieChart($jsonObj, controlID, subcontrols, fldLabName, fldValue, providerName, index, Min, Max, Units) {
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
    var axisLabelFontStyle = { fontSize: "0.8em" };
    var chartTitleFontStyle = { fontSize: "2.25em" };
    var subTitleFontStyle = { fontSize: "2.0em" };
    var legendFontStyle = { fontSize: "1em" };
    var tooltipFontStyle = { fontSize: "1em" };
    var chartFontStyle = { fontSize: "1em" };
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
        SUR: '#66ccff',
        SURN: '#79B3DF',
        SAL: '#66ff33',
        SALN: '#85CC29',
        GW: '#3366ff',
        GWN: '#5C52CC',
        REC: '#669999',
        RECN: '#857A7A',
        SURL: '#0066ff',
        SURLN: '#3352CC',

        UD: '#ffff00',
        UDN:'#FFAA00',
        AD: '#33cc33',
        ADN:'#778822',
        ID: '#999966',
        IDN:'#BB6644',
        PD: '#66ccff',
        PDN: '#80AAD5'
    }
    $.each(flds, function () {
        //looping through the controls to grt the parent controls field names
        if (fldValue[this] != undefined) {

            var fld = String(this);

            chartsData[0].push({
                name: fldLabName[this],
                y: fldValue[this][index],
                color: piecolors[this]
            });
        }});

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
            dataLabels: {
                enabled: false
            },
            showInLegend: true,
            data: chartsData[0]
        }]
    })    
}