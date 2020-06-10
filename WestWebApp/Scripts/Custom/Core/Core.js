/// <reference path="/Scripts/Custom/Charts/ChartTools.js" />
/// <reference path="/Scripts/Custom/Charts/AreaChart.js" />
/// <reference path="/Scripts/Custom/Charts/DrillDownChartBO.js" />
/// <reference path="/Scripts/Custom/Charts/DrillDownColumnChartBO.js" />
/// <reference path="/Scripts/Custom/Charts/DrillDownLineChartTEMP.js" />
/// <reference path="/Scripts/Custom/Charts/DrillDownPieColumnChartMF.js" />
/// <reference path="/Scripts/Custom/Charts/DrillDownPieColumnChartMP.js" />
/// <reference path="/Scripts/Custom/Charts/DrillDownSingleColumnChart.js" />
/// <reference path="/Scripts/Custom/Charts/HighStocks.js" />
/// <reference path="/Scripts/Custom/Charts/LineChartMP.js" />
/// <reference path="/Scripts/Custom/Charts/ProviderChart.js" />
/// <reference path="/Scripts/Custom/Charts/StackedAreaChart.js" />
/// <reference path="/Scripts/Custom/Charts/StackedColumnChart.js" />
/// <reference path="/Assets/indicators/Scripts/IndicatorControl_v_4.js" />
/// <reference path="/Assets/indicators/Scripts/reportindicator.js" />
/// <reference path="/Assets/indicators/Scripts/indicatorsCore_v2.js" />

// QUAY EDIT 1 29 15
// THis is the Root object for storing the json output after a Web Service request
var WS_RETURN_DATA = null;
// This is the Root object for storing the Inforequest json after parsing
var INFO_REQUEST = null;
// need to figure this one out
var BaseProviders = ["st"]
//
var colorBrewer = {
    "SUR_P": '#225ea8',
    //"SUR": '#a1dab4',
    "SURL_P": '#25aae1',
    "GW_P": '#41b6c4',
    //"GW": '#fcb040',
    "REC_P": '#a1dab4',
    //"REC": '#9260a9',
    "SAL_P": '#25aae1',
    "UD_P": '#60BF7E',
    "ID_P": '#60BF7E',
    "AD_P": '#60BF7E',
    "PD_P": '#60BF7E'
    //"SUR": '#8dd3c7',
    //"SURL": '#ffffb3',
    //"GW": '#bebada',
    //"REC": '#80b1d3',
    //"SAL": '#fdb462',
    //"UD": '#Eb8383', // apa 8b2323 QUAY EDIT 4/11/16 '#b3de69',
    //"ID": '#C590FB',  //apa 55008b QUAY EDIT 4/11/16'#fccde5',
    //"AD": '#52BB52', // apa 228b22 QUAY EDIT 4/11/16 '#d9d9d9',
    //"PD": '#B5B5B5' // apa 858585 QUAY EDIT 4/11/16'#bc80bd',

};

function GetCRColor(theFld) {
    aColor = "#e6e6e6";
    switch (theFld) {
        case "SUR_P", "SWM_P":
            aColor = colorBrewer.SUR;
            break;
        case "SURL_P":
            aColor = colorBrewer.SURL;
            break;
        case "GW_P", "GWM_P":
            aColor = colorBrewer.GW;
            break;
        case "REC_P", "RECM_P":
            aColor = colorBrewer.REC;
            break;
        case "SAL_P":
            aColor = colorBrewer.SAL;
            break;
        case "UD_P", "UCON_P":
            aColor = colorBrewer.UD;
            break;
        case "ID_P":
            aColor = colorBrewer.ID;
            break;
        case "AD_P", "AGCON_P":
            aColor = colorBrewer.AD;
            break;
        case "PD_P", "PCON_P":
            aColor = colorBrewer.PD;
            break;
    }
    return aColor;
}
// END QUAY EDIT
//If the page is not a charts page setup the base scenario and
//get list of available providers and their names
if (getWindowType() != 'Charts') {
    var infoRequestJSON = $('input[id$=JSONData]').val();
    INFO_REQUEST = JSON.parse(infoRequestJSON);

    localStorage.clear();
    //setting a default name to active scenario
    if (!localStorage.actvScenario) {
        localStorage.BaseScenario = "Base";
        localStorage.actvScenario = "Base";
        localStorage.scenarioList = "";
        //console.log('localStorage:', localStorage);
    }

    //STEPTOE EDIT BEGIN 11/08/15
    //Remove initial json information from html page 
    remove_hvJSONData();

    //var providerInfo = {};
    //var providersAdded = false;
    ////store all available providers and their codes 
    //function setupProviderInfo() {
    //    var options = $('.chosen-select option');

    //    for (var i in options) {
    //        providerInfo[options[i].value] = options[i].text;
    //    }
    //}
    //setupProviderInfo();
    //STEPTOE EDIT END 11/08/15
}


// Set up Scenario Buttons

// Save Screnario
$(document).ready(function () {

    //dialog box
    $("#dialog").dialog({
        autoOpen: false,
        height: 250,
        width: 400,
        modal: true,
        buttons: {

            "Save Scenario": function () {

                if (typeof (Storage) !== "undefined") {

                    var name = $("#tbdialogScenarioName").val();
                    $("#tbdialogScenarioName").val("");

                    localStorage.actvScenario = name;
                    localStorage[localStorage.actvScenario] = getJSONData('ALL');

                    localStorage.scenarioList += "," + name;
                    setLoadScenarios();

                    alert("You saved a scenario successfully!!!");
                }

                else {
                    alert("Sorry, your browser does not support web storage...");
                }

                $(this).dialog("close");
            }
        },
        open: function (event) {
            $('.ui-dialog-buttonpane').find('button:contains("Save Scenario")').attr('class', 'btn btn-sec btn-force-margins');
        }
    });

    $("#loading-dialog").dialog({
        autoOpen: false,
        height: 125,
        width: 250,
        modal: true
    });
    $('#loading-dialog').parent().find('[title="close"]').hide();

    //saving the data in local storage
    $('#savebutton').click(function () {

        if (localStorage.actvScenario == "Base")

            $("#dialog").dialog("open");

        else {

            if (typeof (Storage) !== "undefined") {

                var name = localStorage.actvScenario;

                localStorage[localStorage.actvScenario] = getJSONData('ALL');

                alert("You saved the scenario to " + name + " successfully!!!");
            }

            else {
                alert("Sorry, your browser does not support web storage...");
            }
        }
    });

    //Creating new copy
    $("#createbutton").click(function () {
        $("#dialog").dialog("open");
    });

    //loading Adjusted Scenario from local storage
    $("#loadASbutton").click(function () {

        if ($('input[name="saved-scenario"]:checked').val()) {

            var scenarioName = $('input[name="saved-scenario"]:checked').siblings("label").html();

            if (typeof (Storage) !== "undefined") {

                if (localStorage[scenarioName]) {

                    //loading the scenario and calling the web service
                    setInputControlsFromScenario(localStorage[scenarioName]);
                    localStorage.actvScenario = scenarioName;
                    setProviderCheckBoxes();
                    callWebService(getJSONData('parent'), true);
                    $("#loading-dialog").dialog("open");
                    //alert("You loaded a scenario successfully!!!");
                }
                else {
                    alert("Sorry, you don't have any saved scenarios...");
                }
            }
            else {
                alert("Sorry, your browser does not support web storage...");
            }
        }
        else {

            if ($('#adj-scenarios-list').find("input[type=radio]"))
                alert("Sorry, you don't have any saved scenarios...");

            else
                alert("Please select any of the saved scenarios...");
        }

    });


    //loading Base Scenario from local storage
    $("#loadBSbutton").click(function () {

        if (typeof (Storage) !== "undefined") {

            var scenarioName = localStorage.BaseScenario;

            //loading the Base scenario and calling the web service
            setInputControlsFromScenario(localStorage[scenarioName]);
            localStorage.actvScenario = scenarioName;
            setLoadScenarios();
            setProviderCheckBoxes();
            SetFlowRadio("default")
            // QUAY EDIT 6/13/14
            //callWebService(getJSONData('parent'));
            callWebService(getJSONData('empty'), true);
            $("#loading-dialog").dialog("open");

            //
            // 06/23/17 Drought does not exist
            //setDefaultDrought();
            // STEPTOE EDIT 08/04/15 BEGIN
            //Reset all previous indicator values to '...'
            //$('.IndicatorControl_OLD').each(function () {
            //    this.innerHTML = " ... "
            //});
            // STEPTOE EDIT 08/04/15 BEGIN

            //====================================
            //alert("You loaded a scenario successfully!!!");
        }

        else {
            alert("Sorry, your browser does not support web storage...");
        }

    });

});

$(document).ready(function () {
    $('div[id*=panelDependents]').dialog({
        modal: true,
        autoOpen: false,
        height: 275,
        width: 360
    });
});

//hiding labels of the user controls used as properties and assigning to their respective div properties
(function () {

    //hiding the keyword property label in input
    $('.InputControl').each(function () {

        //assigning        
        $(this).attr("data-key", $(this).find("span[id*=lblSliderKeyWord]").html());

        //hiding labels
        $(this).find("span[id*=lblSliderKeyWord]").hide();
    });

    //hiding the type property label in output
    $('.OutputControl').each(function () {

        //assigning
        $(this).attr("data-Type", $(this).find("span[id*=lblChartOption]").html());
        $(this).attr("data-fld", $(this).find("span[id*=lblFldName]").html());
        $(this).attr("data-title", $(this).find("span[id*=lblTitle]").html());
        $(this).attr("data-series", $(this).find("span[id*=lblSeries]").html());
        //hiding labels
        $(this).find("span[id*=lblChartOption]").hide();
        $(this).find("span[id*=lblFldName]").hide();
        $(this).find("span[id*=lblTitle]").hide();
        $(this).find("span[id*=lblSeriesColors]").hide();
    });
})();

//Getting the div ID drop down list selected item changed
$(".ddlType").change(function () {
    drawChart($(this).closest('div[id*=OutputControl]').attr('id'));
});

//Getting the div ID drop down list selected item changed
$(".ddlflds").change(function () {
    drawChart($(this).closest('div[id*=OutputControl]').attr('id'));
});


//drwaing charts on temporal slider(years) change
$(document).on('mouseup', '.temporal', function (event) {

    var refresh = false;
    $('input[name="temporal"]:checked').each(function () {

        if (this.value == "point-in-time" && event.currentTarget.id == "point-in-time-slider") {
            refresh = true;
        } else if (this.value == "range-in-time" && event.currentTarget.id == "range-in-time-slider") {
            refresh = true;
        }
    });

    if (!refresh) {
        return;
    }
    setStrtandEndYr();

    //looping through the output controls and getting the required data and populating the charts
    $('.OutputControl').each(function () {
        var controlID = $(this).attr('id');
        drawChart(controlID);
    });

    drawAllIndicators();
});

$('#map-button').click(function () {
    showDialog("Images/AZ_regions.png");
})

function getProviderRegionText() {
    return $('#providersList option:selected').text();
}

// Grab the name of the region from the url (if not null)
var ourState = "AZN10";
function getRegion() {
    var temp = $.urlParam('region');
    if (temp && temp != null) {
        var found = false;
        temp = temp.toLowerCase();
        $.each($('#providersList').children('option'), function (index, option) {
            var regionText = option.text.toLowerCase().replace(/\s/g, '');
            if (regionText == temp) {
                temp = option.value;
                found = true;
                return false;
            }
        });

        if (!found) {
            temp = null;
        }
    }
    else {
        temp = null;
    }
    return temp;
}

var providerRegion = null;

function setRegion(region){
    if (region == null) {
        providerRegion = $('#providersList').val();
    }
    else {
        providerRegion = region;
        $('#providersList').val(providerRegion);
    }

    // Set the regions name on the page
    $('.title-region').text(getProviderRegionText());
}

// Get the provider region if set in the URL else use the default
setRegion(getRegion());

// When user changes the provider region, update provider region
// Rerun the model for the selected region
$(document).on('change', '#providersList', function () {
    providerRegion = $(this).val();
    $('.title-region').text(getProviderRegionText());
    //console.log('getProviderRegionText:', $(this).text());

    if (LOAD_IPAD) {
        setStateInformation();
    }

    runModel();

    //    //STEPTOE EDIT 07/15/15 BEGIN
    //    //If Core.js is included and Window is Default.aspx send geography data to tabs
    //    if (getWindowType() == 'Default')
    //        sendGeographyRadioChange(provider);
    //    //STEPTOE EDIT 07/15/15 END
});

//drwaing charts on temporal radio change
$(document).on('change', 'input[name="temporal"]', function () {

    setStrtandEndYr();

    //looping through the output controls and getting the required data and populating the charts
    $('.OutputControl').each(function () {
        var controlID = $(this).attr('id');
        drawChart(controlID);
    });

    drawAllIndicators();
});

//STEPTOE BEGIN EDIT 11/09/15
//Raising "Run Model" button click event and calling web service
$(document).on('click', '.run-model', runModel);

//Toggling the sub controls
$(document).on('click', '.ui-slider-popup-button', function () {

    if ($(this).closest("div[id*=ControlContainer]").attr("data-Subs") != "") {

        var subid = ($(this).closest("div[id*=ControlContainer]").attr("data-Subs")).split(',');

        $.each(subid, function () {
            $('#' + this + "_ControlContainer").attr("style", "display:inline");
        });

        $('div[id*=panelDependents]').dialog("open");
    }
});

//// 02.29.16 DAS
//// For second button
//// --------------------------------------
//function continueButtonClicked() {
//    console.log("woot got a click");
//   // eyr = {};
//    //        eyr["FLD"] = "COEXTSTYR";
//    //        // eyr["VAL"] = 1939;
//    //        eyr["VAL"] = 1938;
//    //        DefaultInputs.push(eyr);

//    //        inputData["Inputs"] = DefaultInputs;
//}

//$('#Finish_button').on('click', continueButtonClicked);
// -------------------------------------------------------
// Pull in State Level Data of input controls and
// indicator selections from Scripts/Ipad/STC.js
// 03.08.16 DAS
// =============
var CT = STC;
// 03.18.16 DAS
// Groundwater, Economy, environemt, Power, Agriculture, surface water, and Urban efficiencies
// Three places these MUST be defined; 1) Here, 2_ Indicator.js (~line 39), and Below
// For five indicators: var height = 220;var width = 235;
//var height = 230;

var height = 220;
var width = 235;
//var height = 110;
//var width = 118;

// For six indicators
var indicatorParameters = {
    "GWSYA_P": {
        divId: "idGWSYADiv", anIndicatorType: "GWSYA_P", ControlId: "idGWSYAIndicator", options: { Height: height, Width: width }
    },
    "ECOR_P": {
        divId: "idECORDiv", anIndicatorType: "ECOR_P", ControlId: "idECORIndicator", options: { Height: height, Width: width }
    },
    "EVIND_P": {
        divId: "idENVINDDiv", anIndicatorType: "EVIND_P", ControlId: "idENVINDIndicator", options: { Height: height, Width: width }
    },
    "PEF_P": {
        divId: "idPEFDiv", anIndicatorType: "PEF_P", ControlId: "idPEFIndicator", options: {
            Height: height,
            Width: width,
            meter: {
                style: 'rgr_meter'
            }
        }
    },
    "AGIND_P": {
        divId: "idAGINDDiv", anIndicatorType: "AGIND_P", ControlId: "idAGINDIndicator", options: {
            Height: height,
            Width: width,
            meter: {
                style: 'rgr_meter'
            }
        }
    },
    "IEF_P": {
        divId: "idIEFDiv", anIndicatorType: "IEF_P", ControlId: "idIEFIndicator", options: {
            Height: height,
            Width: width,
            meter: {
                style: 'rgr_meter'
            }
        }
    },
    "SWI": {
        divId: "idSWIDiv", anIndicatorType: "SWI", ControlId: "idSWIIndicator", options: { Height: height, Width: width }
    },
    "UEF_P": {
        divId: "idUEFDiv", anIndicatorType: "UEF_P", ControlId: "idUEFIndicator",
        options: {
            Height: height,
            Width: width,
            meter: {
                style: 'rgr_meter'
            }
        }
    }
}


var IndicatorControlsArray = [];
function setStateInformation() {
    //var userState = $.urlParam('state');
    //var userState = getRegion();
    
    if (providerRegion != null) {
        //Perform state specific information here such as choosing which
        //input controls should be visible/available
        // Display the name of the State above the Policy DIV, i.e.,

        $('#dashboard-header-h0').text(providerRegion);

        hideInputControls(providerRegion);
        displayIndicators(providerRegion);
        displayAssessment(providerRegion);
      }
    else {

    }
}
setStateInformation();

// Hide input controls that should not be shown
function hideInputControls(region) {
    //hide all input controls
    //$('.controlgroup:not(#climate-controls .controlgroup)').hide();
    $('#policyPane .controlgroup').hide();

    //Show controls in array
    $.each(STC[region].IFLDS, function (index, fld) {
        var field = fld.split('_')[0];

        $('#policyPane').find('#' + field + 'InputUserControl_controlgroup').show();
        //console.log('hideInputControls:', field);
    });
}

// This will set the indicator Titles to the web label
function ResetIndicatorTitles() {
    var fieldInfo = INFO_REQUEST.FieldInfo;
    for (var i = 0; i < fieldInfo.length; i++) {
        if (fieldInfo[i].WEBL) {
            if (fieldInfo[i].WEBL != "") {
                switch (fieldInfo[i].FLD) {
                    case "ENVIND":
                        IndSetupData.ENVIND.title = fieldInfo[i].WEBL;
                        break;
                    case "SWI":
                        IndSetupData.SWI.title = fieldInfo[i].WEBL;
                        break;
                    case "UEF":
                        IndSetupData.UEF.title = fieldInfo[i].WEBL;
                        break;
                }
            }
        }
    }
}

function displayIndicators(region) {
    //Reset indicator titles, remove previous indicators if any, and setup the array
    ResetIndicatorTitles();
    $('.indicator-container').remove();
    IndicatorControlsArray = new Array();

    //$('#indicators-empty').append(IndSetupData['ECOR'].svg)
    //var params = indicatorParameters['ECOR'];
    //IndicatorControl(params.divId, params.anIndicatorType, params.ControlId, params.options);

    //console.log('region:', region, STC[region])
    for (var index = 0; index < STC[region].INDFLDS.length; index++) {
        var params = indicatorParameters[STC[region].INDFLDS[index]];
        IndicatorControlsArray[params.anIndicatorType] = new IndicatorControl(params.divId, params.anIndicatorType, params.ControlId, params.options);
    }
}

function displayAssessment(region) {
    // Remove all assessment indicators
    clearAssessment();

    $.each(STC[region].INDFLDS, function (index, field) {
        appendAssessmentField(field);
    });
} 

// New drawAllInndicators function for WaterSimAmerica
// 03.02.2016 DAS
function drawAllIndicators(jsondata) {
    var stateString = providerRegion;
    var indicatorDisplayed = STC[stateString].INDFLDS;

    $.each(jsondata.RESULTS, function () {
        if (indicatorDisplayed.indexOf(this.FLD) > -1) {
            var values = this.VALS[0].VALS;
            var lastIndex = values.length - 1;
            //console.log('drawAllIndicators:', this.FLD, values[lastIndex], this);
            IndicatorControlsArray[this.FLD].SetValue(values[lastIndex]);
        }
    });
}

// STEPTOE EDIT 05/07/16
// Call to start Ipad timers
function loadingScreenHidden(){
    try {
        webkit.messageHandlers.sendMessage.postMessage('loadingScreenHidden');
        return true;
    } catch(err) {
        return false;
    }
}

////Self invoking function to set the input values and calling web service to populate the charts
(
    function () {

        setSliderValues();

        //checking the provider list check boxes
        setProviderCheckBoxes();

        //Adjust the line charts size
        $('#GraphControls_OutputUserControl7_ChartContainer').width(705);

        //Inidicators not on iPad page
        //initializeIndicators();

        //calling the webservice
        if (localStorage.actvScenario == "Base") {
            callWebService(getJSONData('empty'));
        }
        else {
            callWebService(getJSONData('parent'));
        }
        // QUAY EDIT 3/18/14 Begin
        callWebServiceVersion();
        //setting load scenarios list from local storage
        setLoadScenarios();

        //STEPTOE EDIT 12/08/2015
        //Hide input sliders and show buttons to select values
        inputControls2Radios();
        hideInputControls(providerRegion);
        if (LOAD_IPAD) {
            hideLastInputButton();
        }

        //$("#WSLoading").hide();

        //Hide all Values
        // 03.22.16 DAS =======================
        $("span[id*=lblSliderVal]").hide()

        //Hide all Units
        $("span[id*=lblunits]").hide()
        // DAS ================================
        //
        $("#dashboard-header-h1").show();
        // QUAY EDIT 3/19/16 BEGIN
        // Hide the GOOD JOB DIV
        $("#idGoodJob").hide();
        $("#idAssessment").hide();
        if (!DebugMode) {

            //STEPTOE 12/08/2015  Modified Quay 3/19/16
            // ======================
            //comment out / in to start/stop Sideshow

            config = {};
            config.wizardName = ModalWizards[CurrentModal];
            //===========================
        }
        //function hideMarquee() {
        //    var mydiv = document.getElementsByClassName("marquee");
        //    mydiv[0].style.visibility = "hidden";
        //}
        hideMarquee();
       // $('.marquee').hide();
        // QUAY EDIT 3/19/16 END

        // QUAY EDIT 4/19/16
        // Hide audio elements
        hideaudio();

        // STEPTOE EDIT 05/07/16
        // Send loadingScreenHidden message!
        loadingScreenHidden();

        return;
    })();

    $(document).ready(function () {
        // STEPTOE EDIT 07/05/17 Disable Sideshow
        //Sideshow.start(config);
        SetFinishButtonWaiting()
        //HideHighestInputControlValue();
    })
// E.O.F.
