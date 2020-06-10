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



// Intial code 
//Sampson edit 08.13.14,08.28.14,10.01.14,03.09.15
// Moved to 16.1.1. on 1 October, 2014
// 16a.1.2 on 10 October, 2014
// to 19.3.2 on 5 March, 2015
// 06.01.15 - version 20.0.0
// ----------------------------------------------------------
// UI Version
var WaterSimVersion = "UI: 22.5.0  ";
var UI = WaterSimVersion.fontsize(1);
function SetVersion(theVersion) {
    $("#VersionInfo").html(theVersion);

}

function API(theVersion) {
    $("#Version").html(theVersion);
}

function Model(theVersion) {
    $("#Model").html(theVersion);
}
// ---------------------------------------------------------
//getting inforequest json data

var infoRequestJSON = $('input[id$=JSONData]').val();
localStorage.clear();
//setting a default name to active scenario
if (!localStorage.actvScenario) {
    localStorage.BaseScenario = "Base";
    localStorage.actvScenario = "Base";
    localStorage.scenarioList = "";
}


// Set up Scenario Buttons

// Save Screnario
$(document).ready(function () {

    //dialog box
    $("#dialog").dialog({
        autoOpen: false,
        height: 175,
        width: 350,
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
            $('.ui-dialog-buttonpane').find('button:contains("Save Scenario")').addClass('button');
        }
    });

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
                    callWebService(getJSONData('parent'));

                    alert("You loaded a scenario successfully!!!");
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
            callWebService(getJSONData('empty'));
            //
            setDefaultDrought();


            //====================================
            alert("You loaded a scenario successfully!!!");
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

//setting the controls with the data from the local storage
function setInputControlsFromScenario(JSONData) {

    var jsonstr = JSON.parse(JSONData);
    var jsonInputs = JSON.parse(jsonstr.inputJsonArray);
    // QUAY EDIT BEGIN 3/12/14 3:20
    //var strtYr = 0;
    //var endYr = 0;
    var strtYr = 2000;
    var endYr = 2085;
    // QUAY EDIT BEGIN 3/12/14 3:20

    $.each(jsonInputs.Inputs, function () {

        var fld = this.FLD;
        var val = this.VAL;

        if (fld == 'STARTYR') {
            strtYr = val;
        }
        else if (fld == 'STOPYR')
            endYr = val;

        else {
            // Quay Edit 6/4/14
            setSpecialInputControls(fld, val);
            //=================================
            //setting input controls
            $('.InputControl').each(function () {

                if ($(this).attr("data-key") == fld) {
                    $(this).find("div[id*=divslider]").attr("data-def", val);
                }
            });
        }
        //DASS


    });

    //setting temporal controls    
    if (strtYr == endYr) {

        $("#point-in-time").html(strtYr);
        $("#point").prop("checked", true)
    }

    else {
        $("#range-in-time-slider").attr("data-strtyr", strtYr);
        $("#range-in-time-slider").attr("data-endyr", endYr);
        $("#range").prop("checked", true)
    }

    setSlider();
}

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

// QUAY EDIT BEGIN 3/13/14 evening
function Time_Chart_Draw() {
    if (jsondata != undefined) {
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
    }
}
// QUAY EDIT END 3/13/14 evening

$(document).on('change', 'input[name="geography"]', function () {

    var provider = $(this).val();

    //looping through the output controls and getting the required data and populating the charts
    $('.OutputControl').each(function () {

        var controlID = $(this).attr('id');
        var type = $("#" + controlID).attr("data-Type");

        if (type == "MFOP") {
            $("#" + controlID).find("select[id*=ddlfld]").val(provider);
            drawChart(controlID);
        }
    });
    //STEPTOE EDIT 07/15/15 BEGIN
    //If Core.js is included and Window is Default.aspx send geography data to tabs
    if (getWindowType() == 'Default')
        sendGeographyRadioChange(provider);
    //STEPTOE EDIT 07/15/15 END
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

//Raising "Run Model" button click event and calling web service
$(document).on('click', '.run-model', function () {

    var subControls;
    var sum = 0;// to hold the sum if the sub controls value
    var subControlsValid;

    if ($(this).attr('id') == 'run-model-Pop') {

        //getting the sub contrls values and adding them
        /*$('.InputControl').each(function () {
    
            if ($(this).attr("data-Subs") != "") {
    
                subControls = $(this).attr("data-Subs").split(',');
    
                $.each(subControls, function () {
                    sum = sum + parseInt($('#' + this + '_lblSliderVal').html());
                });
    
                if (sum != 100) {
                    subControlsValid = false;
                }
                else
                    subControlsValid = true;
            }
        });*/
        subControlsValid = true;

        //if sum of sub controls is not equal to 100
        if (!subControlsValid) {
            alert("Please Ensure that the sum of sub controls is 100");
        }

        else {
            $('div[id*=panelDependents]').dialog("close");

            //hiding all sub controls
            jsonStr = JSON.parse(infoRequestJSON);

            $.each(jsonStr.FieldInfo, function () {
                if (this.DEP != "") {
                    $.each(this.DEP, function () {
                        $('#' + this + "_ControlContainer").attr("style", "display:none");
                    });
                }
            });
            callWebService(getJSONData('DEP'));
        }
    }
    else
        callWebService(getJSONData('parent'));

});

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
//Toggling the sub controls
$(document).on('click', '.trace-CO', function () {

    var subControls;
    var sum = 0;// to hold the sum if the sub controls value
    var subControlsValid;
    var inputData = {};

    var DefaultInputs = [];
    eyr = {};

    if ($(this).attr('id') == 'trace-CO-one') {
        eyr = {};
        eyr["FLD"] = "COEXTSTYR";
        // eyr["VAL"] = 1939;
        eyr["VAL"] = 1938;
        DefaultInputs.push(eyr);

        inputData["Inputs"] = DefaultInputs;


    };


});
////Self invoking function to set the input values and calling web service to populate the charts
//(function () {

//    //setting the slider values
//    setSliderValues();

//    //checking the provider list check boxes
//    setProviderCheckBoxes();

//    initializeIndicators();

//    //calling the webservice
//    if (localStorage.actvScenario == "Base") {
//        callWebService(getJSONData('empty'));
//    }

//    else {
//        callWebService(getJSONData('parent'));
//    }


//    //setting load scenarios list from local storage
//    setLoadScenarios();
//})();

//setting Provider check boxes
function setProviderCheckBoxes() {

    var jsonstr = JSON.parse(localStorage[localStorage.actvScenario]);
    var jsonOutputs = JSON.parse(jsonstr.outputJsonArray);
    var prvdrs = {};

    $.each(jsonOutputs.Providers, function () {
        prvdrs[this] = true;
    });

    $('input[name="geography"]').each(function () {

        if (prvdrs[$(this).attr("id")]) {
            $(this).prop("checked", true);
        }
        else {
            $(this).prop("checked", false);
        }
    });
}


//reading and setting the scenario names as optons of dropdown list
function setLoadScenarios() {

    if (localStorage.scenarioList != "") {

        var name = localStorage.actvScenario;

        var scenarios = (localStorage.scenarioList).split(',');

        //setting scenarios
        $('#adj-scenarios-list').empty();
        $.each(scenarios, function (count) {
            if (scenarios[count] != "")
                $('#adj-scenarios-list').append('<li><input type="radio" class="saved-scenario" name="saved-scenario" value="' + count + '" /><label id="' + count + '">' + scenarios[count] + '</label></li>');
        });

        if (name != 'Base') {
            $('.saved-scenario').each(function (count) {
                if ($(this).siblings('label').html() == name)
                    $(this).attr('checked', true);
            });
        }
    }
}

//Setting the slider value by reading the property value property value
function setSliderValues() {

    var valCount = 0;
    var jsonStr;

    //looping through the input controls and setting the slider values by calling the meta data service
    $('.InputControl').each(function () {
        var controlID = $(this).attr('id');
        var fldName = "";
        var min = "";
        var max = "";
        var def = "";
        var subs = "";
        // ======BEGIN QUAY EDIT 6/17/14
        var tooltipstr = "";
        // ======END QUAY EDIT 

        //parsing the json
        jsonStr = JSON.parse(infoRequestJSON);

        //Getting the key word to compare and get the respective values of the slider
        var keyWord = $(this).attr("data-key");

        //setting the values of the sliders from the json
        $.each(jsonStr.FieldInfo, function () {

            //comparing the JSON to get its details
            if (this.FLD == keyWord) {

                fldName = this.LAB; //Getting Field Name
                min = this.MIN; //Getting Min Value
                max = this.MAX; //Getting Max Value
                subs = this.DEP; //Getting Sub control details
                def = this.DEFAULT;
                // ======BEGIN QUAY EDIT 6/17/14
                tooltipstr = this.LNG;
                // ======END QUAY EDIT 


                if (this.DEP != "") {

                    //var subid = ($(this).closest("div[id*=ControlContainer]").attr("data-Subs")).split(',');
                    $.each(this.DEP, function () {
                        $('#' + this + "_ControlContainer").attr("style", "display:none");
                    });
                }
                else
                    $("#" + controlID).find('div[id*=PopupButton]').hide();
            }
        });

        $('div[id*=panelDependents]').attr("style", "display:none");

        //Assigning the values obtained from the service
        $(this).find("span[id*=lblSliderfldName]").html(fldName);
        $(this).attr("data-fld", fldName);
        $(this).find("span[id*=lblSliderVal]").html(def);
        $(this).attr("data-Subs", subs);
        $(this).find("div[id*=divslider]").attr("data-min", min);
        $(this).find("div[id*=divslider]").attr("data-max", max);
        $(this).find("div[id*=divslider]").attr("data-def", def);
        // ======BEGIN QUAY EDIT 6/17/14
        $(this).find("div[id*=SliderTitle]").attr("title", tooltipstr);
        // ======END QUAY EDIT 


    });

    //saving the baase scenario
    localStorage[localStorage.BaseScenario] = getJSONData('ALL');

    //if ther is any data in local storage
    if (localStorage.actvScenario == "Base") {
        setSlider();
    }

    else {
        setInputControlsFromScenario(localStorage[localStorage.actvScenario]);
    }
}


//calling webservice and getting the json string
function callWebService(jsonData) {

    //STEPTOE ADD 07/15/15 BEGIN
    //If CoreAdd.js is included and window is Default.aspx send loading to all windows
    if (getWindowType() == 'Default')
        sendLoading()
    //STEPTOE ADD 07/15/15 END

    //auto-saving the scenario in local storage
    //localStorage[localStorage.actvScenario] = jsonData;

    //calling webservice
    $.ajax({

        type: "POST",
        url: "WaterSimService.asmx/GetData",
        data: jsonData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function ($res) {
            setInputControls($res.d);
            drawOutputCharts($res.d);

            //STEPTOE ADD 07/15/15 BEGIN
            //If window is Default.aspx send webService data to windows
            if(getWindowType() == 'Default')
                sendWebServData($res.d);
            //STEPTOE ADD 07/15/15 END

            // QUAY EDIT 3/14/14 noon BEGIN
            SetRunButtonState();
            // QUAY EDIT 3/14/14 noon END;
        },
        error: function (xhr, status, error) {
            alert("Ajax : " + status + "\n" + error + "\n" + "status : " + xhr.status);
            alert(xhr.responseText);
        }
    });
}
// QUAY EDIT 3/19/14 Begin
////Sampson edit 08.13.14,08.28.14,10.01.14
//// Moved to 16.1.1. on 1 October, 2014
//// ----------------------------------------------------------
//// UI Version
//var WaterSimVersion = "UI Version: 16a.1.1";
//var UI = WaterSimVersion.fontsize(1);
//function SetVersion(theVersion) {
//    $("#VersionInfo").html(theVersion);

//}
//function APIandModel(theVersion) {
//    $("#VersionModel").html(theVersion);
//}
//// ---------------------------------------------------------


// QUAY EDIT 3/19/14 Begin
// DAS edits 08.15.14 I added the parsing of the string for Web Service, API, and Model versions
function callWebServiceVersion() {

    //auto-saving the scenario in local storage
    //localStorage[localStorage.actvScenario] = jsonData;

    //calling webservice
    $.ajax({

        type: "POST",
        url: "WaterSimService.asmx/GetVersion",
        contentType: "application/json; charset=utf-8",
        //data:{},
        //dataType: "text",
        dataType: "json",
        //data: jsonData,
        //contentType: "application/json; charset=utf-8",
        async: false,
        success: function ($res) {
            var verdata = $.parseJSON($res.d);

            if (verdata.VERSION) {
                var MyVersion = verdata.VERSION.substring(10, verdata.length).fontsize(1);
                //
                var index_2 = MyVersion.indexOf("API");
                var Web = MyVersion.substring(0, index_2 - 1);
                SetVersion(UI + " " + Web);

                var index_3 = MyVersion.indexOf("Model");
                var APIv = MyVersion.substring(index_2, index_3 - 1).fontsize(1);
                API(APIv);

                var Modelv = MyVersion.substring(index_3 - 2, verdata.length).fontsize(1);

                Model(Modelv);

            }
        },
        error: function (xhr, status, error) {
            alert("Ajax : " + status + "\n" + error + "\n" + "status : " + xhr.status);
            alert(xhr.responseText);
        }
    });
}
function getSustainabilityIndicatorValues() {



}





//  Gets the Input and Output JSON to pass to the Web Service
//  inputType can be three values 
//  ''  or anything
//  'ALL'
//  'empty'
//  'DEP'
//  'parent'

//  For inputType = 'ALL"    
//      This will set STOPYR and STARTYR based on temporal controls
//      pushes all input fields as input and for output

//  For inout Type = 'parent'
//      This will end up setting STOPYR to 2050 and STARTYR is not specified
//      Only pushes inputfields that are parents, no dependent fields

//  For inout Type = 'DEP'
//      This will end up setting STOPYR to 2050 and STARTYR is not specified
//      Only pushes inputfields that are dependents no parent fields


// for all inoutType = all values
//      loops through and gathers all input control values



function getJSONData(inputType) {

    var inputData = {};
    var outputData = {};
    var eyr = {};
    var inputFields = [];
    var duplicate = {};
    var jsonData = {};
    var input = {};
    var output = [];
    var outputFields = [];
    var dependent = {};

    //start year and end year
    /*$('input[name="temporal"]:checked').each(function () {        

        if (this.value == "point-in-time") {
            eyr["FLD"] = "STOPYR";
            eyr["VAL"] = parseInt($("#point-in-time").html());
            inputFields.push(eyr);
        }
        else {
            eyr["FLD"] = "STOPYR";
              eyr["VAL"] = 2040;//parseInt($("#range-in-time-slider").attr("data-endyr"));
            inputFields.push(eyr);
        }
    });*/

    if (inputType == 'ALL') {

        $('input[name="temporal"]:checked').each(function () {

            if (this.value == "point-in-time") {
                eyr["FLD"] = "STARTYR";
                eyr["VAL"] = parseInt($("#point-in-time").html());
                inputFields.push(eyr);

                eyr = {};

                eyr["FLD"] = "STOPYR";
                eyr["VAL"] = parseInt($("#point-in-time").html());
                inputFields.push(eyr);
            }
            else {
                eyr["FLD"] = "STARTYR";
                eyr["VAL"] = parseInt($("#range-in-time-slider").attr("data-strtyr"));
                inputFields.push(eyr);

                eyr = {};

                eyr["FLD"] = "STOPYR";
                eyr["VAL"] = parseInt($("#range-in-time-slider").attr("data-endyr"));
                inputFields.push(eyr);
            }
        });
    }
    else {
        eyr["FLD"] = "STOPYR";
        eyr["VAL"] = 2050;//parseInt($("#range-in-time-slider").attr("data-endyr"));
        inputFields.push(eyr);

        //das
        eyr = {};
        eyr["FLD"] = "DECSNGAME";
        eyr["VAL"] = 0;
        inputFields.push(eyr);
        // QUAY EDIT

    }

    //getting input controls
    $('.InputControl').each(function () {
        input = {};

        input["FLD"] = $(this).attr("data-key");
        var FooTest = $(this).find("span[id*=lblSliderVal]");
        var FooStr = $(this).find("span[id*=lblSliderVal]").html();
        input["VAL"] = parseInt($(this).find("span[id*=lblSliderVal]").html());


        var dep = ($(this).attr("data-Subs")).toString().split(',');

        $.each(dep, function () {
            dependent[this] = true;
        });

        //checking for duplicate       
        if (!duplicate[$(this).attr("data-key")]) {

            if (inputType == 'parent' && !dependent[input["FLD"]]) {
                inputFields.push(input);
            } else if (inputType == 'DEP' && $(this).attr("data-Subs") == "") {
                inputFields.push(input);
            } else if (inputType == 'ALL') {
                inputFields.push(input);
            }

            outputFields.push(input["FLD"]);

        }
        duplicate[$(this).attr("data-key")] = true;

    });

    if (inputType == 'empty') {
        var DefaultInputs = [];
        eyr = {};

        // DAS edit, 04.20.15
        eyr = {};
        eyr["FLD"] = "DECSNGAME";
        eyr["VAL"] = 0;
        inputFields.push(eyr);
        // QUAY EDIT
        // Forcing this to true
        eyr = {};
        eyr["FLD"] = "AWSLIMIT";
        eyr["VAL"] = 1;
        DefaultInputs.push(eyr);

        //// get flow years
        // create object
        eyr = {};
        // assign field and value
        eyr["FLD"] = "COEXTSTYR";
        eyr["VAL"] = CORiverFlowValue;
        // push this as an input value
        DefaultInputs.push(eyr);
        // request this as a data field
        outputFields.push(eyr["FLD"]);

        eyr = {};
        eyr["FLD"] = "SVEXTSTYR";
        eyr["VAL"] = SVRiverFlowValue;
        DefaultInputs.push(eyr);
        outputFields.push(eyr["FLD"]);

        // DAS edits 10.15.14
        eyr = {};
        eyr["FLD"] = "COUSRSTR";
        eyr["VAL"] = CODroughtStartValue;
        DefaultInputs.push(eyr);
        outputFields.push(eyr["FLD"]);

        eyr = {};
        eyr["FLD"] = "COUSRSTP";
        eyr["VAL"] = CODroughtStopValue;
        DefaultInputs.push(eyr);
        outputFields.push(eyr["FLD"]);

        eyr = {};
        eyr["FLD"] = "SVUSRSTR";
        eyr["VAL"] = STVDroughtStartValue;
        DefaultInputs.push(eyr);
        outputFields.push(eyr["FLD"]);

        eyr = {};
        eyr["FLD"] = "SVUSRSTP";
        eyr["VAL"] = STVDroughtStopValue;
        DefaultInputs.push(eyr);
        outputFields.push(eyr["FLD"]);

        inputData["Inputs"] = DefaultInputs;
        // QUAY EDIT

        //inputData["Inputs"] = [];

    } else {

        // DAS edit. 04.20.15
        eyr = {};
        eyr["FLD"] = "DECSNGAME";
        eyr["VAL"] = 0;
        inputFields.push(eyr);
        // QUAY EDIT

        eyr = {};
        eyr["FLD"] = "AWSLIMIT";
        eyr["VAL"] = 1;
        inputFields.push(eyr);

        // get flow years
        eyr = {};
        eyr["FLD"] = "COEXTSTYR";
        eyr["VAL"] = CORiverFlowValue;
        inputFields.push(eyr);
        // request this as a data field
        outputFields.push(eyr["FLD"]);

        eyr = {};
        eyr["FLD"] = "SVEXTSTYR";
        eyr["VAL"] = SVRiverFlowValue;
        inputFields.push(eyr);
        // request this as a data field
        outputFields.push(eyr["FLD"]);
        //  QUAY EDIT
        // DAS edits
        // 02.27.15
        // This is the bool for the deltat burden to Arizona; false we share the burden, true AZ covers all
        // CO delta water
        eyr = {};
        eyr["FLD"] = "CODELTAB";
        eyr["VAL"] = 0;
        inputFields.push(eyr);
        // request this as a data field
        outputFields.push(eyr["FLD"]);
        // eyr = {};
        //
        var COstart_array = $('input[id="COUSRSTR_v"]');
        var COstart = undefined;
        if ($('input[id="COUSRSTR_v"]').length > 0)
            COstart = COstart_array[0].value;
        var MinDifference = "10";

        if (COstart != undefined) {
        }
        else {
            var COstart = "2010";
        }
        if (COstart != "") {
            eyr = {};
            eyr["FLD"] = "COUSRSTR";
            eyr["VAL"] = COstart;
            inputFields.push(eyr);
        }

        var COstop_array = $('input[id="COUSRSTP_v"]');
        var COstop = undefined;
        if ($('input[id="COUSRSTP_v"]').length > 0)
            COstop = COstop_array[0].value;

        if (COstop != undefined) {
            if (COstart != "") {

                if (COstop == "") {
                    alert("No drought stop year: Colorado River- was entered!");
                    COstop = COstart;
                }
            }

        }
        else {
            COstop = "2015";
        }

        if (COstop != "") {
            eyr = {};
            eyr["FLD"] = "COUSRSTP";
            eyr["VAL"] = COstop;
            inputFields.push(eyr);
        }

        var STVstart_array = $('input[id="SVUSRSTR_v"]');
        var STVstart = undefined;
        if ($('input[id="SVUSRSTR_v"]').length > 0)
            STVstart = STVstart_array[0].value;

        if (STVstart != undefined) {

            if (STVstart != "") {
                if (STVstop == "") {
                    alert("No drought stop year: Salt-Verde Rivers- was entered!");
                    STVstop = STVstart;
                }
            }
        }
        else {
            STVstart = "2010";
        }
        if (STVstart != "") {
            eyr = {};
            eyr["FLD"] = "SVUSRSTR";
            eyr["VAL"] = STVstart;
            inputFields.push(eyr);
        }

        var STVstop_array = $('input[id="SVUSRSTP_v"]');
        var STVstop = undefined;
        if ($('input[id="SVUSRSTP_v"]').length > 0)
            STVstop = STVstop_array[0].value;


        if (STVstop != undefined) {
        }
        else {
            STVstop = "2015";
        }
        if (STVstop != "") {
            eyr = {};
            eyr["FLD"] = "SVUSRSTP";
            eyr["VAL"] = STVstop;
            inputFields.push(eyr);
        }


        inputData["Inputs"] = inputFields;
    }

    //getting output controls
    $('.OutputControl').each(function () {
        output = [];
        output = ($(this).attr("data-fld")).split(',');

        //checking for duplicate
        $.each(output, function () {

            if (!duplicate[this]) {
                outputFields.push(this);
            }
            duplicate[this] = true;
        });
    });

    //Skip if window is Charts
    if ( getWindowType() != 'Charts') {
        //getting dependent fields
        var infoRequest = JSON.parse(infoRequestJSON);

        $.each(infoRequest.FieldInfo, function () {

            if (this.DEP)
                $.each(this.DEP, function () {

                    if (!duplicate[this]) {
                        outputFields.push(this);
                    }
                    duplicate[this] = true;
                });
        });
    }
    $('.IndicatorControl').each(function () {
        output = [];
        output = ($(this).attr("data-fld")).split(',');

        //checking for duplicate
        $.each(output, function () {

            if (!duplicate[this]) {
                outputFields.push(this);
            }
            duplicate[this] = true;
        });

        if (outputFields["name"] == "SINPCTGW ") {
            var val = this.VALS[0];
            var GW = val;
            One(GW);

        };
    });

    outputData["Outputs"] = outputFields;//['all'];

    //getting providers
    var providers = [];

    $('input[name="geography"]').each(function () {
        providers.push(this.value);
    });

    outputData["Providers"] = providers;

    jsonData['inputJsonArray'] = JSON.stringify(inputData);
    jsonData['outputJsonArray'] = JSON.stringify(outputData);

    return JSON.stringify(jsonData);
}
// End of function getJSONData(inputType) {

//setTimeout(function () { alert("Drought Stop year not entered") }, 2000);

// Special code to handle customized input controls
// Quay 6/4/14

function setSpecialInputControls(aFLD, aval) {
    switch (aFLD) {
        case "COEXTSTYR":
            SetCoFlow(aval);
            break;
        case "SVEXTSTYR":
            SetSVFlow(aval)
            break;
    }
}


//Setting the slider with values obtained from the service
function setInputControls(outputJSON) {

    var jsonstr = $.parseJSON(outputJSON);

    $.each(jsonstr.RESULTS, function () {

        var fld = this.FLD;
        var val = this.VALS[0];
        var found = false;

        $('.InputControl').each(function () {

            if ($(this).attr("data-key") == fld) {
                //var fooid = $(this).find("div[id*=divslider]");
                $(this).find("div[id*=divslider]").attr("data-def", val);
                found = true;
            }
        });


        // QUAY EDIT 6/4/14
        // If an input control for this field was not found, then pass to special routine;
        if (!found) {
            setSpecialInputControls(fld, val);
        }
        //=========================
    });
    setSlider();
}

var jsondata;

var subControls;
var fldNames;
// QUAY EDIT BEGIN 3/12/14 4:00
var fldMAXes;
var fldMINes;
var fldUnits;
var fldLongunits;
// QUAY EDIT END 3/12/14 4:00
var fldValues;
var providerName;
var inputJSONStr;
var strtYr;
var endYr;
var $jsonObj;


function setStrtandEndYr() {
    //STEPTOE EDIT 07/15/15 Begin
    //If page is Default.aspx perform normal operations otherwise skip
    //If CoreAdd.js is included send data to tabs
    if (getWindowType() != "Charts") {
        $('input[name="temporal"]:checked').each(function () {

            if (this.value == "point-in-time") {
                strtYr = parseInt($("#point-in-time").html());
                endYr = parseInt($("#point-in-time").html());
            }
            else {
                strtYr = parseInt($("#range-in-time-slider").attr("data-strtyr"));
                endYr = parseInt($("#range-in-time-slider").attr("data-endyr"));
            }
        });
        if(getWindowType() == "Default")
            sendTemporalRadioChange();
    }
    //STEPTOE EDIT 07/15/15 END
}

//draw output charts depending on the output controls used
function drawOutputCharts(outputJSON) {

    jsondata = outputJSON;

    subControls = {};
    fldNames = {};
    fldValues = {};
    providerName = {};
    // QUAY EDIT BEGIN 3/12/14 4:00
    fldMAXes = {};
    fldMINes = {};
    fldUnits = {};
    fldLongunits = {};

    // QUAY EDIT END 3/12/14 4:00

    setStrtandEndYr();

    inputJSONStr = JSON.parse(infoRequestJSON);

    //getting the fld names and sub controls
    $.each(inputJSONStr.FieldInfo, function () {

        fldNames[this.FLD] = this.LAB;
        // QUAY EDIT BEGIN 3/12/14 4:00
        fldMAXes[this.FLD] = this.MAX;
        fldMINes[this.FLD] = this.MIN;
        fldUnits[this.FLD] = this.UNT;
        fldLongunits[this.FLD] = this.UNTL;
        // QUAY EDIT END 3/12/14 4:00

        if (this.DEP)
            subControls[this.FLD] = [this.DEP];
        else
            subControls[this.FLD] = [""];

    });

    //getting provider names w.r.to their code
    $('input[name="geography"]').each(function () {
        providerName[this.value] = $(this).next('label').html();
    });


    $jsonObj = JSON.parse(jsondata);

    $.each($jsonObj.RESULTS, function () {
        fldValues[this.FLD] = this.VALS;

        //
    });

    //looping through the output controls and getting the required data and populating the charts
    $('.OutputControl').each(function () {
        var controlID = $(this).attr('id');
        drawChart(controlID);
    });

    drawAllIndicators();
}

//===============================================
// Draw Chart Function
// Version 2
// Substantial Changes here  3/15/14,06.23.15
// ===============================================
//                    green       orange    dk green
// colors to try #82C341, #FAA31B,#009F75
var ColorSeriesArray = new Array();
var seriesDBlue = ["#0066CC", "#E60000"];
var seriesNew = ["#7cb5ec", "#f7a35c", "#7798BF", " #90ee7e", "#aaeeee", "#ff0066", "#eeaaee"];
var seriesArea = ["#336699", "#D63333"];
var seriesAreaStacked = seriesNew;
var seriesAreaLine = ["#336699", "#FF0000", "#00FF00", "#FF6600", "#33CCFF"];
var seriesLine = ["#000000", "#f7a35c", "#0066CC", "#FF0000", " #009933", "#B200B2", "#E6E6E6"];
// var seriesColumn3 = ["#FF9900", "#336699", "#FF0000"];
var seriesColumn3 = ["#3399FF", "#000000", "#009900"];

var seriesColumn7 = ["#E68AE6", "#FF6600", "#000000", "#88C6ED", "#009900", "#3399FF", "#FF0000"];
//                              black           orange       
//
ColorSeriesArray[0] = seriesLine;
ColorSeriesArray[1] = seriesDBlue;
ColorSeriesArray[2] = seriesLine;
ColorSeriesArray[3] = seriesAreaLine;
ColorSeriesArray[4] = seriesArea
ColorSeriesArray[5] = seriesAreaStacked;
ColorSeriesArray[6] = seriesNew;
ColorSeriesArray[7] = seriesColumn3;
ColorSeriesArray[8] = seriesColumn7;

var seriesColors = seriesLine;

//
function drawChart(controlID) {
    var MyChartType;

    var type = $("#" + controlID).attr("data-Type");

    var mySeriesString = $("#" + controlID).attr("data-series");
    var color = ColorSeriesArray[mySeriesString];
    if (color != undefined) {
        seriesColors = color;
    }
    var MainType = type.substr(0, 4);
    var SubType = "";
    if (type.length > 4)
        SubType = type.substr(4, 3);
    var TimeMode = "";
    // ================================================================================================

    // ================================================================================================
    switch (MainType) {
        case "OFMP":
            $('#' + controlID).find("select[id*=ddlfld]").hide();

            if (strtYr == endYr) {
                // ok point in time show the graph types drop down
                $('#' + controlID).find("select[id*=ddlTypes]").show();
                drawDrillDownPieColumnChartMP(jsondata, controlID, subControls, fldNames, fldValues, providerName, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits);
            } else {
                $('#' + controlID).find("select[id*=ddlTypes]").hide();
                switch (SubType) {
                    case "":
                        var CurrentProvider = providerName;
                        CurrentProvider["doreg"] = false;
                        //drawAreaStackedChart(jsondata, controlID, subControls, fldNames, fldValues, providerName, strtYr, endYr, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits);
                        drawProvidersChart(jsondata, controlID, subControls, fldNames, fldValues, CurrentProvider, strtYr, endYr, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits, seriesColors, ChartTypeAreaStacked);
                        break;
                    case "L":
                        //drawLineChartMP(jsondata, controlID, subControls, fldNames, fldValues, providerName, strtYr, endYr, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits);
                        drawProvidersChart(jsondata, controlID, subControls, fldNames, fldValues, providerName, strtYr, endYr, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits, seriesColors, ChartTypeLine);
                        break;
                    case "R":
                        var CurrentProvider = providerName;
                        //var pcode = $("#" + controlID).find("select[id*=ddlfld]").find("option:selected").val();
                        //CurrentProvider[pcode] = providerName[pcode];
                        CurrentProvider["doreg"] = true;

                        //drawLineChartMP(jsondata, controlID, subControls, fldNames, fldValues, providerName, strtYr, endYr, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits);
                        drawProvidersChart(jsondata, controlID, subControls, fldNames, fldValues, CurrentProvider, strtYr, endYr, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits, seriesColors, ChartTypeLine);
                        break;

                }
            }
            break;
        case "MFOP":
            // Add providers to drop down
            if ($("#" + controlID).find("select[id*=ddlfld]").find("option").length == 0) {
                $('input[name="geography"]').each(function () {
                    $("#" + controlID).find("select[id*=ddlfld]").append(new Option($(this).next('label').html(), this.value));
                });
            }
            if (strtYr == endYr) {
                // ok point in time show the graph types drop down
                $('#' + controlID).find("select[id*=ddlTypes]").show();
                // multi fields so show the provider drop down
                $('#' + controlID).find("select[id*=ddlfld]").show();
                drawDrillDownPieColumnChartMF(jsondata, controlID, subControls, fldNames, fldValues, providerName, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits);
            } else {
                // range so hyde the chart types
                $('#' + controlID).find("select[id*=ddlTypes]").hide();
                // this is going to use the provider drop down, so get the cuurent provider code in the drop down and use this
                var CurrentProvider = {};
                var pcode = $("#" + controlID).find("select[id*=ddlfld]").find("option:selected").val();
                CurrentProvider[pcode] = providerName[pcode];
                CurrentProvider["doreg"] = true;
                // ok make standard provider chart call with only one provider code and ChartTypeColumnStacked
                drawProvidersChart(jsondata, controlID, subControls, fldNames, fldValues, CurrentProvider, strtYr, endYr, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits, seriesColors, ChartTypeColumnStacked);
                //drawColumnStackedChart(jsondata, controlID, subControls, fldNames, fldValues,     strtYr, endYr, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits);
            };
            break;
        case "OFOP":
            // only one provider so hide the provider drop down
            $('#' + controlID).find("select[id*=ddlfld]").hide();
            if (strtYr == endYr) {
                // ok point in time show the graph types drop down
                $('#' + controlID).find("select[id*=ddlTypes]").show();
                drawDrillDownSingleColumnChart(jsondata, controlID, subControls, fldNames, fldValues, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR));
            } else {
                // ok a range show hide the graph type
                $('#' + controlID).find("select[id*=ddlTypes]").hide();
                drawDrillDownLineChartTEMP(jsondata, controlID, subControls, fldNames, fldValues, providerName, strtYr, endYr, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR));
            };
            break;
        case "BASE":
            $('#' + controlID).find("select[id*=ddlTypes]").hide();
            $('#' + controlID).find("select[id*=ddlfld]").hide();
            if (strtYr == endYr) {
                switch (SubType) {
                    case "L", "A":
                        MyChartType = ChartTypeColumn;
                        break;
                    case "SL", "SA":
                        MyChartType = ChartTypeColumnStacked;
                        break;
                }
            } else {
                switch (SubType) {
                    case "L":
                        MyChartType = ChartTypeLine;
                        break;
                    case "LS":
                        MyChartType = ChartTypeLineStacked;
                        break;
                    case "A":
                        MyChartType = ChartTypeArea;
                        break;
                    case "SA":
                        MyChartType = ChartTypeAreaStacked;
                        break;
                    case "AL":
                        MyChartType = ChartTypeAreaLine;
                        break;

                }
            }

            drawDrillDownChartBO(jsondata, controlID, subControls, fldNames, fldValues, strtYr, endYr, (strtYr - $jsonObj.MODELSTATUS.STARTYEAR), fldMINes, fldMAXes, fldLongunits, seriesColors, MyChartType);
            break;

    }

}

//STEPTOE ADD 07/15/15 BEGIN
//Added to determine if Com is defined and if so return the window type
//if it is not defined assume window is Default.aspx without CoreAdd.js included
function getWindowType() {
    if (typeof (Com) != "undefined" && Com != null)
        return Com.windowType;
    else
        return null;
}
//STEPTOE ADD 07/15/15 END

////Self invoking function to set the input values and calling web service to populate the charts
(
    function () {
        //STEPTOE 07/11/14
        //If Com is defined and Window is a Charts tab overwrite Default functions
        if (getWindowType() == 'Charts') {
            setSliderValues = function () { };
            setProviderCheckBoxes = setSliderValues;
            initializeIndicators = setSliderValues;
            drawAllIndicators = setSliderValues;
            callWebService = callWebServiceCharts;
            var type = $.urlParam('type');
            if (type != null) {
                checkBoxValues[type] = true;
                $('#checkbox'+type).prop('checked', true);
            }
            callWebServiceVersion();
            return;
        }
        //STEPTOE EDIT 07/11/15 END

        //$(document).ready(function () {
        //$("#MainForm").show();
        //setting the slider values
        setSliderValues();

        //checking the provider list check boxes
        setProviderCheckBoxes();

        initializeIndicators();

        //calling the webservice
        if (localStorage.actvScenario == "Base") {
            callWebService(getJSONData('empty'));
        }

        else {
            callWebService(getJSONData('parent'));


        }

        // QUAY EDIT 3/18/14 Begin
        callWebServiceVersion();
        //SetVersion(WaterSimVersion);
        // QUAY EDIT 3/18/14 End;Begin

        //setting load scenarios list from local storage
        setLoadScenarios();
        // DAS ADDED 6/25/14
        // OK turn the walkthrough wizard off
        // 03.09.15 DAS
        // $("#wizard").fadeOut();
        //-------------------------
        $("#WSLoading").hide();
       // $("#WSLoadingCharts").hide();
        // DAS - this is a "cludge" as part of another funciton to stop the splash page from loading text
        // September, 2014
        $("#dashboard-header-h1").show();
        //});

        // 

    })();
// E.O.F.
