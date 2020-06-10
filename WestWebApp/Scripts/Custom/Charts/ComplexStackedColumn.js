


function drawComplexStackedColumnChart($jsonObj, controlID, subcontrols, AfluxList) {

    //console.log($jsonObj, controlID, subcontrols, AfluxList);
    //return;

    //getting the chart type
    var chartType = 'column';
    var plotOptions = true;

    //console.log(chartType)

     CreateHighchart($jsonObj, controlID, subcontrols, AfluxList)
}
function CreateHighchart($jsonObj, controlID, subcontrols, AFluxList) {

      var divID = $("#" + controlID).find("div[id*=ChartContainer]").attr('id');

    // get Column Labels
      //fldValues = {};
      //$.each($jsonObj.RESULTS, function () {
      //    fldValues[this.FLD] = this.VALS;

      //    //
      //});
      //var fldnames = $("#" + controlID).attr("data-fld");
      //var flds = fldnames.split(',');
      //var fldList = ["SUR", "SURL", "GW", "REC", "SAL"];
      //var fldListFlux = ["SUR", "SURL_UD", "SURL_AD", "SURL_PD", "GW", "REC", "SAL"];

      //ResFields = new Array();
      //var count = 0;
      //var all=0;
      //for (all = 0; all < fldList.length; all++) {
      //    for (count = 0; count < flds.length; count++) {
      //        if (flds[count] == fldListFlux[all]) {

      //            ResFields[all] = fldListflux[all];
      //        }
      //    }
         
      //}
      ResFields = new Array();
      ResFields[0] = "SUR_P";
      ResFields[1] = "SURL_P";
      ResFields[2] = "GW_P";
      ResFields[3] = "REC_P";
      ResFields[4] = "SAL_P";

        ResourceNames = new Array();
        ResourceNames[0] = "Surface";
        ResourceNames[1] = "Lake";
        ResourceNames[2] = "Groundwater";
        ResourceNames[3] = "Reclaimed";
        ResourceNames[4] = "Saline";

        // Get Data
        UData = new Array();
        AData = new Array();
        IData = new Array();
        PData = new Array();
    //
    


    for (ri = 0; ri < ResFields.length; ri++) {
        // get the field name
        ResFld = ResFields[ri];
        // get the list of the fluxes
        ResFluxList = AFluxList.GetResourceFluxes(ResFld);
        // get the Consumers
        // get the UD list, should only be one
        ConsFluxList = ResFluxList.GetConsumerFluxes("UD_P");
        Allocated = ConsFluxList.List.length ? ConsFluxList.List[0].LastValue() : 0;
        UData[ri] = Allocated;
        // get the AD list, should only be one
        ConsFluxList = ResFluxList.GetConsumerFluxes("AD_P");
        Allocated = ConsFluxList.List.length ? ConsFluxList.List[0].LastValue() : 0;
        AData[ri] = Allocated;
        // get the ID list, should only be one
        ConsFluxList = ResFluxList.GetConsumerFluxes("ID_P");
        Allocated = ConsFluxList.List.length ? ConsFluxList.List[0].LastValue() : 0;
        IData[ri] = Allocated;
        // get the PD list, should only be one
        ConsFluxList = ResFluxList.GetConsumerFluxes("PD_P");
        Allocated = ConsFluxList.List.length ? ConsFluxList.List[0].LastValue() : 0;
        PData[ri] = Allocated;
    }

    // OK, let's decide if we need SAL
    // first sum up all the SAL values  SAL is 4;  could get this dynamically, but in a hury
    var SALTotal = 0;
    var SALIndex = 4;
    SALTotal += UData[SALIndex];
    SALTotal += AData[SALIndex];
    SALTotal += IData[SALIndex];
    SALTotal += PData[SALIndex];

    var SALNeeded = true;
    // ok here is the test
    if (SALTotal == 0) {
        SALNeeded = false;
    }

    // OK, let's decide if we need Lake

    var LAKTotal = 0;
    var LAKIndex = 1;
    LAKTotal += UData[LAKIndex];
    LAKTotal += AData[LAKIndex];
    LAKTotal += IData[LAKIndex];
    LAKTotal += PData[LAKIndex];

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
    ResourceNames[resIndex] = "Groundwater";
    resIndex++;
    ResourceNames[resIndex] = "Reclaimed";
    resIndex++;
    if (SALNeeded) {
        ResourceNames[resIndex] = "Saline";
    }

    // ok REBUILD dATA aRRAYS
    var tmpUData = new Array();
    var tmpAData = new Array();
    var tmpIData = new Array();
    var tmpPData = new Array();

    dataIndex = 0;
    for (di = 0; di < UData.length; di++) {
        if (((di != LAKIndex) || (LAKNeeded)) && ((di != SALIndex) || (SALNeeded))) {
            tmpUData[dataIndex] = UData[di];
            tmpAData[dataIndex] = AData[di];
            tmpIData[dataIndex] = IData[di];
            tmpPData[dataIndex] = PData[di];
            dataIndex++;
        }
    }

    // reassign
    UData = tmpUData;
    AData = tmpAData;
    IData = tmpIData;
    PData = tmpPData;

    Index = 0;
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

    // -----------------------------------------------------------------------------------------
    var $complexColumnChart = $('#' + divID).highcharts({
    //$('#FLUX1').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: 'Resource By Consumers',
            style: chartTitleFontStyle
        },
        xAxis: {
            categories: ResourceNames,
            title: {
                style: axisTitleFontStyle
            },
            labels: {
                style: axisLabelFontStyle
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Resource by Consumers (MGD)',
                style: axisTitleFontStyle
            },
            labels: {
                style: axisLabelFontStyle
            }
        },
        legend: {
            itemStyle: legendFontStyle
        },
        tooltip: {
            headerFormat: '<b>{point.x}</b><br/>',
            pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}',
            style: tooltipFontStyle
        },
        plotOptions: {
            column: {
                stacking: 'normal',
                dataLabels: { style: { 'fontWeight': 'bold' } }
            }
        },
        colors: ['#FFA080', '#20D020', '#808080', '#0020D0'],
        series: [{
            name: 'Cities and Towns',
            data: UData// [5, 3, 4, 7, 2]
        }, {
            name: 'Agriculture',
            data: AData //[2, 2, 3, 2, 1]
        }, {
            name: 'Industrial',
            data: IData //[2, 2, 3, 2, 1]
        }, {
            name: 'Electricity Production',
            data: PData //[2, 2, 3, 2, 1]
        }]
    });
}

