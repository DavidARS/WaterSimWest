/// <reference path="/Scripts/Custom/Core.js" />

//Current state of chart type checkbox
var checkBoxValues = {
    "Supply": false,
    "Demand": false,
    "Reservoirs": false,
    "Sustainability": false,
    "All": false
};

//Loop through all checkboxes and setup on click to enable or disable the chart type
$(":checkbox").click(function () {
    checkBoxValues[this.name] = !checkBoxValues[this.name];
    if (this.name == "All") {
        var value = checkBoxValues["All"];
        for (var i in checkBoxValues) {
            checkBoxValues[i] = value;
            $('#checkbox' + i).prop('checked', value);
        }
    }
});

//Setup dialog to select type of charts to be created
$(function () {
    $("#dialog-charts").dialog({
        autoOpen:false,
        resizable: false,
        height: 300,
        width: 300,
        modal: true,
        buttons: {
            "Done": {
                click: function () {
                    if (checkBoxValues["Supply"])
                        addSupply();
                    if (checkBoxValues["Demand"])
                        addDemand();
                    if (checkBoxValues["Reservoirs"])
                        addResevoirs();
                    if (checkBoxValues["Sustainability"])
                        addSustainability();

                    loadCharts();

                    $(this).dialog("close");
                },
                'class': 'button',
                text: 'Done'
            }
        }
    });
});

//Source: http://stackoverflow.com/questions/19491336/get-url-parameter-jquery
//Get URL parameter 'name'
$.urlParam = function (name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results == null) {
        return null;
    }
    else {
        return results[1] || 0;
    }
}
//********************************************************
// Begin Communication Code
//********************************************************

//Standard Communication Strings
var dataRequestString = 'requesting data';
var dataSentString = 'Sending requested data';
var callWebServSuccessString = 'callWebService Success';
var callWebServFailString = 'callWebService Failure';
var temporalRadioChangeString = 'temporal radio change';
var geographyRadioChangeString = 'geography radio change';
var loadingString = 'loading';
var selectedProvidersChangeString = 'selected providers change';

//Communication data container
var Com = {
    connection: false, //Is the window connected to the master?
    windowType: 'Charts', //The type of window this is
    masterWindow: '', //The type of the Master window
    masterWindowPath: '', //The Master window's path
    id: -1 //Unique ID assigned to the window by the Master for communication
}
//Adds window event handler to recieve messages from other windows
//Once the connection is established it'll remove the loading screen
//and display the menu to select charts.
function communication() {
    window.addEventListener('message', function (event) {
        //console.log("receive something ", event);
        try {
            var data = JSON.parse(event.data);

            if (data.message === "FirstConnection") {
                Com.connection = true;
                Com.masterWindow = event.source;
                Com.masterWindowPath = event.origin;
                Com.id = data.id;

                data.message = "FirstConnectionReceive";
                Com.masterWindow.postMessage(JSON.stringify(data), event.origin);
                //console.log("Charts is ready to communicate.");
                $("#WSLoading").hide();
                $("#dialog-charts").dialog("open");
            }
            else if (Com.connection) {
                //Connection has been established process message
                //console.log(data.message);
                //If message has a windowType check for a message
                if (data.hasOwnProperty('source')) {
                    if (data.message == dataSentString) {
                        recieveRequestedData(data);
                    }
                    else if (data.message == temporalRadioChangeString) {
                        recieveTemporalRadioChange(data);
                    }
                    else if (data.message == geographyRadioChangeString) {
                        recieveGeographyRadioChange(data);
                    }
                    else if (data.message == callWebServSuccessString) {
                        recieveWebServSuccess(data);
                    }
                    else if (data.message == loadingString) {
                        recieveLoading();
                    }
                    else if (data.message == selectedProvidersChangeString) {
                        recieveSelectedProvidersChange(data);
                    }
                }
            }
        } catch (e) {
            console.log('invalid json: ', data);
            //console.log(event.data);
        }
        //console.log(event);
    });
}
//Requests webService data from Default.aspx
function callWebServiceCharts(testData) {
    if (Com.connection) {
        var data = {
            source: Com.windowType,
            id: Com.id,
            message: dataRequestString
        }

        Com.masterWindow.postMessage(JSON.stringify(data), Com.masterWindowPath);
    }
}
//Updating the selector with new providers for each chart
function updateProviders(data) {
    //If providerInfo is different than repopulate selector for each chart
    if (JSON.stringify(providerInfo) !== data.providerInfo) {
        try {
            //Parse the provider information and get the current selected provider
            //for each chart
            providerInfo = JSON.parse(data.providerInfo);
            var selectedOptions = $(".ddlflds option:selected"),
            valArray = [];
            for (var i = 0; i < selectedOptions.length; i++) {
                valArray.push(selectedOptions[i].value);
            }

            //Remove all providers from each chart and set them to the proivders
            //recieved
            $(".ddlflds").html('');
            for (var i in providerInfo) {
                $('.ddlflds')
                    .append($("<option></option>")
                    .attr("value", i)
                    .text(providerInfo[i]));
            }

            //Set the selected proivder to the previous selection if available
            //for each chart
            for (var i = 0; i < valArray.length; i++) {
                $('.ddlflds')[i].value = valArray[i];
            }
        } catch (e) {
            console.log("updateProviders", providerInfo)
        }        
    }
}
//Updating the strtYr and endYr to match Default.aspx
function updateStrtYrEndYr(data) {
    strtYr = parseInt(data.strtYr);
    endYr = parseInt(data.endYr);
    //console.log("strtYr & endYr: "+strtYr+", "+endYr);
}
//Recieved initial data to setup page (strYr,endYr,provider,webserviceData)
function recieveRequestedData(data) {
    infoRequestJSON = data.infoRequestJSON;

    updateProviders(data);

    //Updating the strtYr and endYr to match Default.aspx
    updateStrtYrEndYr(data);

    //looping through the output controls to set the provider
    /*if ((typeof (data.provider) != "undefined")) {
        $('.OutputControl').each(function () {

            var controlID = $(this).attr('id');
            var type = $("#" + controlID).attr("data-Type");

            if (type == "MFOP") {
                $("#" + controlID).find("select[id*=ddlfld]").val(data.provider);
            }
        });
    }*/

    drawOutputCharts(data.testData);
}
//Temporal Radio Changed update charts
function recieveTemporalRadioChange(data) {
    updateStrtYrEndYr(data);

    //looping through the output controls to populate the charts
    $('.OutputControl').each(function () {
        var controlID = $(this).attr('id');
        drawChart(controlID);
    });
}
//Geography Radio Changed update charts
function recieveGeographyRadioChange(data) {
    //looping through the output controls to set the provider and populate the charts
    $('.OutputControl').each(function () {

        var controlID = $(this).attr('id');
        var type = $("#" + controlID).attr("data-Type");

        if (type == "MFOP") {
            $("#" + controlID).find("select[id*=ddlfld]").val(data.provider);
            drawChart(controlID);
        }
    });
}
//Selected Providers Changed update charts
function recieveSelectedProvidersChange(data) {
    updateProviders(data);

    //looping through the output controls to populate the charts
    $('.OutputControl').each(function () {
        var controlID = $(this).attr('id');
        drawChart(controlID);
    });
}
//Model Successful ran and returned new data
function recieveWebServSuccess(data) {
    //$("#WSLoading").hide();
    updateProviders(data);
    drawOutputCharts(data.testData);
}
//Model is about to run show loading screen
function recieveLoading() {
    //$("#WSLoading").show();
}
//Enable Communication
communication();