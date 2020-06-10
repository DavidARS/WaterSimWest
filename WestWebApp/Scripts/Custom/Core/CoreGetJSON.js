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

var DroughtFlag = false;

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

    //STEPTOE EDIT 12/6/17
    //eyr = {};
    //eyr["FLD"] = "ST";
    //eyr["VAL"] = getStateInt();// 0;
    //inputFields.push(eyr);

    // QUAY EDIT 3/29/16 BEGIN
    eyr = {};
    eyr["FLD"] = "STOPYR";
    //eyr["VAL"] = "2060";
    //eyr["VAL"] = "2064";
    eyr["VAL"] = "2050";
    inputFields.push(eyr);
    // QUAY EDIT 3/29/16 END

    //
    //  QUAY EDIT 4/21/16 BEGIN
    //  THis is not doing anything so getting rid of it
    // --------------------------------------------------------
    //if (inputType == 'ALL') {

    //    $('input[name="temporal"]:checked').each(function () {
    //        //if (this.value == "point-in-time") {

    //        //    eyr = {};
    //        //    eyr["FLD"] = "STARTYR";
    //        //    eyr["VAL"] = parseInt($("#point-in-time").html());
    //        //    inputFields.push(eyr);

    //    });
    //}
    //else {

    //}
    //  QUAY EDIT 4/21/16 END

    //getting input controls
    $('#PanelUserInputs .InputControl').each(function () {
        input = {};

        input["FLD"] = $(this).attr("data-key");
        var FooTest = $(this).find("span[id*=lblSliderVal]");
        var FooStr = $(this).find("span[id*=lblSliderVal]").html();
        input["VAL"] = parseInt($(this).find("span[id*=lblSliderVal]").html());
        input["PVC"] = providerRegion;
        input["TYP"] = "PI";


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
            //else if (inputType == 'drought') {
            //    inputFields.push(input);
            //}
            outputFields.push(input["FLD"]);

        }
        duplicate[$(this).attr("data-key")] = true;

    });

    var DefaultInputs = [];

    //
    // QUAY EDIT 4/21/16 BEGIN
   //if (inputType == 'drought') {
    if (DroughtFlag) {
   // QUAY EDIT 4/21/16 END
        //
        //setDroughtControls(inputFields);
        //
        var VAL = 65;
        eyr = {};
        eyr["FLD"] = "DC";
        eyr["VAL"] = VAL;// 0;
        inputFields.push(eyr);
        //
    }
    // QUAY EDIT 4/21/16 BEGIN
    // This will now always include ST and STOPYR in all cases

        // STEPTOE EDIT 12/6/17
        // Set the state for the default call to the Model
        //eyr = {};
        //eyr["FLD"] = "ST";
        //eyr["VAL"] = getStateInt();
        //inputFields.push(eyr);
        //DefaultInputs.push(eyr);

        outputFields.push(eyr["FLD"]);

        // QUAY EDIT 3/29/16 BEGIN
        eyr = {};
        eyr["FLD"] = "STOPYR";
        //eyr["VAL"] = "2060";
        //eyr["VAL"] = "2064";
        eyr["VAL"] = "2050";
        inputFields.push(eyr);

       
        outputFields.push(eyr["FLD"]);
       
        inputData["Inputs"] = inputFields;
        //inputData["Inputs"] = DefaultInputs;
    //} else
    //{
    //    inputData["Inputs"] = inputFields;
    //}
    // QUAY EDIT 4/21/16 END
    //
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

    //console.log('outputFields:', JSON.stringify(outputFields));

    //Skip if window is Charts
    if (getWindowType() != 'Charts') {
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
    $('.indicator-container').each(function () {
        output = [];
        output = ($(this).attr("data-fld")).split(',');

        //checking for duplicate
        $.each(output, function () {

            if (!duplicate[this]) {
                outputFields.push(this);
            }
            duplicate[this] = true;
        });

        // QUAY 1 29 16
        //if (outputFields["name"] == "SINPCTGW ") {
        //    var val = this.VALS[0];
        //    var GW = val;
        //    One(GW);

       // };
    });

    $.each(AssessmentFields, function (aIndex, aField) {
        outputFields.push(aField);
    });
    //outputFields.push("SAI_P");
    outputData["Outputs"] = outputFields;//['all'];
    //
   
    //getting providers
    var providers = [];

    //STEPTOE EDIT BEGIN 11/08/15
    //If chosen is not empty then use the selected providers as input
    //otherwise use the default values
    if ($('.chosen-select').val() != null) {
        providers = $('.chosen-select').val();
    }
    else {
        // QUAY 2 8 16
        //providers = ["st"]
        providers = [providerRegion];
        outputData["Providers"] = providers;
    }
    //console.log('checking it out:', outputData);

    // QUAY EDIT 6 8 16 BEGIN
    // Adding Username to Input Field to mark Ver in Log
    inputData["User"] = "WSA_"+WaterSimVersion;
    jsonData['inputJsonArray'] = JSON.stringify(inputData);
    //console.log('inputJsonArray', jsonData['inputJsonArray'])
    // QUAY EDIT 6 8 16 END

    // not sure when and why this was channged QUAY 6 8 16 see original in comment below
    jsonData['outputJsonArray'] = JSON.stringify(outputData); 

    // QUAY EDIT 6 8 16
    // There was an error here, Providers was an array in an array, should just be an array
    //    jsonData['outputJsonArray'] = JSON.stringify({ Outputs: ["all"], Providers: [providers] });
    //jsonData['outputJsonArray'] = JSON.stringify({ Outputs: ["all"], Providers: providers });
    // QUAY EDIT 6 8 16 END
    
    return JSON.stringify(jsonData);
}

function setDroughtControls(inputFields) {
    var VAL = 65;
    eyr = {};
    eyr["FLD"] = "DC";
    eyr["VAL"] = VAL;// 0;
    inputFields.push(eyr);
    // Sampson 03.21.16
}
//function setDrought(truth) {
//    var value = 100;
//    if (truth) value = 65;
//    return value;
//}

// End of function getJSONData(inputType) {

// 03.16.16 DAS
// For second button - 
// ------------------------------------------------
// 03.16.16 DAS [function found at the bottom of this file]
$('#Finish_button').on('click', continueButtonClicked);
// set at 60% of historical flows
// ==============================
function continueButtonClicked() {
    console.log("woot got a click");
            //eyr = {};
            //eyr["FLD"] = "DC";
            //eyr["VAL"] = 60;
            //DefaultInputs.push(eyr);
    //        inputData["Inputs"] = DefaultInputs;
}
// EOF
