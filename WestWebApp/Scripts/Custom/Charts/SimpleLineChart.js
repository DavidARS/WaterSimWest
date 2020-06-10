function drawSimpleLineChart($jsonObj, controlID, subcontrols, fldLabName, fldValue, Name, strtYr, endYr, index, Min, Max, Units) {
    var divID = $("#" + controlID).find("div[id*=ChartContainer]").attr('id');
    var title = $("#" + controlID).attr("data-title");

    //console.log($jsonObj, controlID, subcontrols, fldLabName, fldValue, Name, strtYr, endYr, index, Min, Max, Units);
    //return;

    //getting the chart type
    var chartType = 'column';
    var plotOptions = true;

    //console.log(chartType)

    if (chartType == 'pie')
        plotOptions = true;
    else
        plotOptions = false;
 
    //getting the provider
    var option = $("#" + controlID).find("select[id*=ddlfld]").find("option:selected").val();

    var fldnames = $("#" + controlID).attr("data-fld");
    //var $jsonObj = $.parseJSON(jsonData); //parsing the Input String as Json object
    CreateHighchartSimpleLine($jsonObj, controlID, subcontrols, fldLabName, fldValue, Name, strtYr, endYr, index);
    
}
    //function CreateHighchart(theUrbanData, theUrbanNetData, theAgData, theAgNetData, theIndData, theIndNetData, thePowData, thePowNetData) {
function CreateHighchartSimpleLine($jsonObj, controlID, subcontrols, fldLabName, fldValue, Name, strtYr, endYr, index) {
    // get Column Labels

    var series = [];
    ResourceNames = new Array();
    if (Name == 'Resources') {
        ResourceNames[0] = "Surface Water";
        ResourceNames[1] = "Lake";
        ResourceNames[2] = "Surface Water";
        ResourceNames[3] = "Ground Water";
        ResourceNames[4] = "Reclaimed";

    }
    else {
        ResourceNames[0] = "Cities and Towns";
        ResourceNames[1] = "Agriculture";
        ResourceNames[2] = "Industry";
        ResourceNames[3] = "Electricity Production";
    }
        // Get Data
        DData = new Array();
        NDData = new Array();
    //
        var divID = $("#" + controlID).find("div[id*=ChartContainer]").attr('id');
        var Title = $("#" + controlID).attr("data-title");
        var fldnames = $("#" + controlID).attr("data-fld");
        var flds = fldnames.split(',');
        var yUnits = 'Million Gallons per Day (MGD)';
         // =================
        var legendText = 'Empty';
    // ==================

        var year = [];
    // getting TicSizes
        var xTicSize = YearTicSize(strtYr, endYr);
        var xStep = JudgeXStep(strtYr, endYr, xTicSize);

        for (yr = strtYr; yr <= endYr; yr++)
            year.push(yr);

        $.each(flds, function () {
            //looping through the controls to grt the parent controls field names
            if (fldValue[this] != undefined) {

                var fld = String(this);
                if (Name == 'Resources') {
                    legendText = 'Supply';
                    if (fld == 'SUR_P') { DData[0] = fldValue[this][0].VALS.map(function (d, i) { return { name: year[i], y: d } }) }
                    if (fld == 'SURL_P') { DData[1] = fldValue[this][0].VALS.map(function (d, i) { return { name: year[i], y: d } }) }
                    if (fld == 'SAL_P') { DData[2] = fldValue[this][0].VALS.map(function (d, i) { return { name: year[i], y: d } }) }
                    if (fld == 'GW_P') { DData[3] = fldValue[this][0].VALS.map(function (d, i) { return { name: year[i], y: d } }) }
                    if (fld == 'REC_P') { DData[4] = fldValue[this][0].VALS.map(function (d, i) { return { name: year[i], y: d } }) }
                }
                else if (Name == 'Consumers') {
                    legendText = 'Demand Met';
                    if (fld == 'UD_P') { DData[0] = fldValue[this][0].VALS.map(function (d, i) { return {name: year[i], y: d} }) }
                    if (fld == 'AD_P') { DData[1] = fldValue[this][0].VALS.map(function (d, i) { return { name: year[i], y: d } }) }
                    if (fld == 'ID_P') { DData[2] = fldValue[this][0].VALS.map(function (d, i) { return { name: year[i], y: d } }) }
                    if (fld == 'PD_P') { DData[3] = fldValue[this][0].VALS.map(function (d, i) { return { name: year[i], y: d } }) }

                    // STEPTOE EDIT 12/6/17
                    //if (fld == 'UDN_P') { NDData[0] = fldValue[this][0].VALS }
                    //if (fld == 'ADN_P') { NDData[1] = fldValue[this][0].VALS }
                    //if (fld == 'IDN_P') { NDData[2] = fldValue[this][0].VALS }
                    //if (fld == 'PDN_P') { NDData[3] = fldValue[this][0].VALS }
                }
            }
        });

        $.each(DData, function (index, data) {
            series.push({
                name: ResourceNames[index], //'Demand',
                data: data,// [5, 3, 4, 7, 2]
                style: legendFontStyle
            })
        });
            
    // QUAY EDIT 3/19/16 BEGIN
            if (Name == 'Consumers') {
                // STEPTOE EDIT 12/6/17
                //for (var di = 0; di < DData.length; di++) {
                //    DData[di] = DData[di] - NDData[di];
                //}
            }
            else {
                // OK, let's decide if we need SAL
                // first sum up all the SAL values  SAL is 4;  could get this dynamically, but in a hury
                var SALTotal = 0;
                var SALIndex = 2;
                SALTotal += DData[SALIndex];

                var SALNeeded = true;
                // ok here is the test
                if (SALTotal == 0) {
                    SALNeeded = false;
                }

                // OK, let's decide if we need Lake

                var LAKTotal = 0;
                var LAKIndex = 1;
                LAKTotal += DData[LAKIndex];

                // ok here is the test
                var LAKNeeded = true;
                if (LAKTotal == 0) {
                    LAKNeeded = false;
                }
                // OK rebuild the Resource Array and Data Arrays

                var resIndex = 0;
                ResourceNames = new Array();

                ResourceNames[resIndex] = "Surface";
                resIndex++;
                if (LAKNeeded) {
                    ResourceNames[resIndex] = "Lake";
                    resIndex++;
                }
                if (SALNeeded) {
                    ResourceNames[resIndex] = "Saline";
                    resIndex++;
                }
                ResourceNames[resIndex] = "Groundwater";
                resIndex++;
                ResourceNames[resIndex] = "Reclaimed";
                resIndex++;

                // ok REBUILD dATA aRRAYS
                var tmpDData = new Array();

                dataIndex = 0;
                for (di = 0; di < DData.length; di++) {
                    if (((di != LAKIndex) || (LAKNeeded)) && ((di != SALIndex) || (SALNeeded))) {
                        tmpDData[dataIndex] = DData[di];
                        dataIndex++;
                    }
                }

                // reassign
                DData = tmpDData;
            }
    // QUAY EDIT 3/19/16 BEGIN

            // --------------------------------------------------
            //var axisLabelFontStyle = { fontSize: "1.0em" };
            //var yaxisLabelFontStyle = { fontSize: "1.5em" };
            //var columnLabelFontStyle = { fontSize: "1.5em" };
            //var chartTitleFontStyle = { fontSize: "2.25em" };
            //var subTitleFontStyle = { fontSize: "2.0em" };
            //var legendFontStyle = { fontSize: "1.5em" };
            //var tooltipFontStyle = { fontSize: "1em" };
            //var chartFontStyle = { fontSize: "1em" };
            var fontFamily = '\'Lato\', sans-serif';
            var axisLabelFontStyle = { fontFamily: fontFamily, fontSize: '14px' };
            var axisTitleFontStyle = { fontFamily: fontFamily, fontSize: '18px' };
            var columnLabelFontStyle = { fontFamily: fontFamily, fontSize: '18px' };
            var chartTitleFontStyle = { fontFamily: fontFamily, fontSize: '24px' };
            var subTitleFontStyle = { fontFamily: fontFamily, fontSize: '20px' };
            var legendFontStyle = { fontFamily: fontFamily, fontSize: '14px' };
            var tooltipFontStyle = { fontFamily: fontFamily, fontSize: '16px' };
            var chartFontStyle = { fontFamily: fontFamily, fontSize: '16.5px' };
            // ===================================================

            //console.log(series,JSON.stringify(series))
        var $stackedColumnChart = $('#' + divID).highcharts({
       // $('#HC1').highcharts({
            chart: {
                type: 'line'
            },
            title: {
                text: Title,
                style: chartTitleFontStyle
            },
            xAxis: {
                type: 'category',
                //tickColor: '#AAA',
                labels: {
                    step: xStep,
                    enabled: true,
                    rotation: 60,
                    format: '{value}',
                    style: axisLabelFontStyle
                },
                title: {
                    style: axisTitleFontStyle
                },
                //lineColor: '#000000',
                lineWidth: 1.5,
                tickLength: 5,
                tickWidth: 2,

                minorTickLength: 3,
                minorTickWidth: 1
            },
            yAxis: {
                min: 0,
                title: {
                    text: yUnits, //Name,
                    style: axisTitleFontStyle
                    //itemStyle: yaxisLabelFontStyle
                },
                labels: {
                    style: axisLabelFontStyle
                }
            },
            legend: {
                itemStyle: legendFontStyle
            },
            //xAxis: {
            //    categories: ResourceNames, // ['Apples', 'Oranges', 'Pears', 'Grapes', 'Bananas']
            //    style: axisLabelFontStyle
            //},
            //yAxis: {
            //    min: 0,
            //    title: {
            //        text: yUnits, //Name,
            //        style: yaxisLabelFontStyle
            //        //itemStyle: yaxisLabelFontStyle
            //    }
            //},
            tooltip: {
                style: tooltipFontStyle
                //headerFormat: '<b>{point.x}</b><br/>',
                //pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
            },
            plotOptions: {
                //column: {
                //    stacking: 'normal',
                //    dataLabels: {
                //        style: {
                //            'fontWeight': 'bold',
                //            'fontSize': columnLabelFontStyle
                //        }
                //    }
                //}
            },
            colors: ['#FFA080', '#20D020', '#808080', '#0020D0'],
            series: series
            //series: [{
            //    name: legendText, //'Demand',
            //    data: DData,// [5, 3, 4, 7, 2]
            //    style: legendFontStyle
            //}, {
            //    name: 'Demand Deficit',
            //    data: NDData, //[2, 2, 3, 2, 1]
            //    style: legendFontStyle
            //}]
        });
    }

