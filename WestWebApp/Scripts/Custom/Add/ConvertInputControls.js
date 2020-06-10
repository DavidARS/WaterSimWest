var DivIDCode = /#DID#/g;
var InputIDCode = /#IID#/g;
var ValueCode = /#VAL#/g;
var divControlGroup = '<div id="#DID#controlgroup" class="realclearfix controlgroup">'
var divRadio = '<div id="#DID#buttonset" class="radio-container" style="float:left;">';
var inputRadio = '<input type="radio" id="#DID#radio_#IID#" name="#DID#radio" value="#VAL#">';
var inputRadioChecked = '<input type="radio" id="#DID#radio_#IID#" name="#DID#radio" value="#VAL#" checked="checked">';
var inputNumber = '<input id="#DID#number" name="#DID#number" class="input-number" type="number" step="any">';
var labelOpen = '<label for="#DID#radio_#IID#">';
var labelClose = '</label>';
var divClose = '</div>';
var inputControlsConverted = false;

var inputControlRadioValues = {};
// QUAY EDIT 2 8 16
if (window.location.href.toLowerCase().indexOf('ipad') > -1)
{
    // inputControlRadioValues['UCON'] = { checked: "High", values: [0, 50, 75, 100], labels: ["None", "Low", "Med", "High"] };
    // inputControlRadioValues['ACON'] = { checked: "High", values: [0, 50, 75, 100], labels: ["None", "Low", "Med", "High"] };
    // inputControlRadioValues['PCON'] = { checked: "High", values: [0, 50, 75, 100], labels: ["None", "Low", "Med", "High"] };
}
else {
    //inputControlRadioValues['REGRECEFF'] = { checked: 17, values: [17, 58, 100], labels: ["Low", "Medium", "High"], inputRange: [0, 100], labelRanges: [[0, 34], [34, 82], [82, 100]] };
    //inputControlRadioValues['WEBAGTR1'] = { checked: 31, values: [31, 65, 100], labels: ["Low", "Moderate", "High"], inputRange: [0, 100], labelRanges: [[0, 34], [34, 82], [82, 100]] };
    //inputControlRadioValues['ENFLOAMT'] = { checked: 0, values: [0, 79044, 158088], labels: ["None", "Some", "Most"], inputRange: [0, 158088], labelRanges: [[0, 39522], [39522, 79044], [79044, 158088]] };
    //inputControlRadioValues['ENFLOPCT'] = { checked: 0, values: [0, 50, 100], labels: ["None", "Some", "Most"], inputRange: [0, 100], labelRanges: [[0, 33], [33, 66], [66, 100]] };
    //inputControlRadioValues['WEBPRPCT'] = { checked: 100, values: [0, 50, 100], labels: ["Low", "Moderate", "High"], inputRange: [0, 100], labelRanges: [[0, 34], [34, 82], [82, 100]] };
    //inputControlRadioValues['WEBPOPGR'] = { checked: 100, values: [50, 100, 150], labels: ["Low", "Medium", "High"], inputRange: [0, 150], labelRanges: [[0, 65], [65, 115], [115, 150]] };

    //inputControlRadioValues['COCLMADJ'] = { checked: 100, values: [50, 100, 150], labels: ["Reduced", "Normal", "Increased"], inputRange: [0, 150], labelRanges: [[0, 65], [65, 115], [115, 150]] };
    //inputControlRadioValues['SVCLMADJ'] = { checked: 100, values: [50, 100, 150], labels: ["Reduced", "Normal", "Increased"], inputRange: [0, 150], labelRanges: [[0, 65], [65, 115], [115, 150]] };

    //inputControlRadioValues['DROUSCEN'] = { checked: 0, values: [0, 1, 2, 3], labels: ["None", "Low", "Med", "High"], inputRange: [0, 3], labelRanges: [[0, 1], [1, 2], [2, 3]] };

    inputControlRadioValues['REGRECEFF'] = { checked: 17, values: [17, 58, 100], labels: ["Labels", "Not", "Provided"], inputRange: [0, 100], labelRanges: [[0, 34], [34, 82], [82, 100]] };
    inputControlRadioValues['WEBAGTR1'] = { checked: 31, values: [31, 65, 100], labels: ["Labels", "Not", "Provided"], inputRange: [0, 100], labelRanges: [[0, 34], [34, 82], [82, 100]] };
    inputControlRadioValues['ENFLOAMT'] = { checked: 0, values: [0, 79044, 158088], labels: ["Labels", "Not", "Provided"], inputRange: [0, 158088], labelRanges: [[0, 39522], [39522, 79044], [79044, 158088]] };
    inputControlRadioValues['ENFLOPCT'] = { checked: 0, values: [0, 50, 100], labels: ["Labels", "Not", "Provided"], inputRange: [0, 100], labelRanges: [[0, 33], [33, 66], [66, 100]] };
    inputControlRadioValues['WEBPRPCT'] = { checked: 100, values: [0, 50, 100], labels: ["Labels", "Not", "Provided"], inputRange: [0, 100], labelRanges: [[0, 34], [34, 82], [82, 100]] };
    inputControlRadioValues['WEBPOPGR'] = { checked: 100, values: [50, 100, 150], labels: ["Labels", "Not", "Provided"], inputRange: [0, 150], labelRanges: [[0, 65], [65, 115], [115, 150]] };

    inputControlRadioValues['COCLMADJ'] = { checked: 100, values: [50, 100, 150], labels: ["Labels", "Not", "Provided"], inputRange: [0, 150], labelRanges: [[0, 65], [65, 115], [115, 150]] };
    inputControlRadioValues['SVCLMADJ'] = { checked: 100, values: [50, 100, 150], labels: ["Labels", "Not", "Provided"], inputRange: [0, 150], labelRanges: [[0, 65], [65, 115], [115, 150]] };
    inputControlRadioValues['DROUSCEN'] = { checked: 0, values: [0, 1, 2, 3], labels: ["Labels", "Info", "Not", "Provided"], inputRange: [0, 3], labelRanges: [[0, 1], [1, 2], [2, 3]] };
}
var inputControlRadioLabels = ["None", "Low", "Med", "High"];

function buildRadioInputControl(id, fieldName, controlOptions) {
    var html = "";
    html += divControlGroup;
    html += divRadio;

    var radioLabels = controlOptions.labels;
    var foundChecked = false;
    var length = radioLabels.length;

    for (var i = 0; i < length; i++) {
        var radio = "";
        var label = radioLabels[i];
        var labelRanges = controlOptions.labelRanges;

        if (controlOptions.values[i] == controlOptions.checked) {
            radio += inputRadioChecked;
            foundChecked = true;
        }
        else {
            var radioDefined = false;
            if (!foundChecked) {
                if (i < (length - 1)) {
                    if (controlOptions.checked >= labelRanges[i][0] && controlOptions.checked < labelRanges[i][1]) {
                        radio += inputRadioChecked;
                        foundChecked = true;
                        radioDefined = true;
                    }
                }
                else {
                    if (controlOptions.checked >= labelRanges[i][0] && controlOptions.checked <= labelRanges[i][1]) {
                        radio += inputRadioChecked;
                        foundChecked = true;
                        radioDefined = true;
                    }
                }
            }

            if (!radioDefined) {
                radio += inputRadio;
            }
        }
            

        radio += labelOpen + label + labelClose;

        radio = radio.replace(InputIDCode, label);
        radio = radio.replace(ValueCode, controlOptions.values[i]);
        html += radio;
    }
    html += divClose;

    html += inputNumber + divClose;

    html = html.replace(DivIDCode, id);

    return html;
}

function inputControl2Radio(inputControl, options) {
    //Hide the slider container
    var sliderContainer = $(inputControl).find("div[class=slider-container]");
    sliderContainer.hide();

    var id = GetRootId($(inputControl).attr('id'));    
    var fieldName = $(inputControl).attr("data-key");

    //Get the html to add the radio input control to the page and insert it after the slider
    var html = buildRadioInputControl(id, fieldName, inputControlRadioValues[fieldName]);
    $(html).insertAfter(sliderContainer);

    //Convert the designated control to a buttonset
    var buttonset = $(inputControl).find("div[class=radio-container]").buttonset();
    var inputRange = inputControlRadioValues[fieldName].inputRange,
        labelRanges = inputControlRadioValues[fieldName].labelRanges,
        labels = inputControlRadioValues[fieldName].labels,
        inputs = [];
    for (var i = inputRange[0]; i <= inputRange[1]; i++) {
        inputs.push(i + "");
    }
    var autoComp = $("#" + id + "number");
    autoComp.val(inputControlRadioValues[fieldName].checked);
    if (options) {
        if(options.height)
            autoComp.height(options.height);
    }

    autoComp.change(function () {
        //console.log('autoComp.change: ' + this.value)
        if (this.value > inputRange[1]) {
            $('#' + id + 'radio_' + labels[labels.length - 1]).prop('checked', true);
            this.value = inputRange[1];
        }
        else if(this.value < inputRange[0]){
            $('#' + id + 'radio_' + labels[0]).prop('checked', true);
            this.value = inputRange[0];
        }
        else {
            var length = labelRanges.length;
            for (var i = 0; i < length; i++) {
                if (i < length - 1) {
                    if (this.value >= labelRanges[i][0] && this.value < labelRanges[i][1]) {
                        $('#' + id + 'radio_' + labels[i]).prop('checked', true);
                        break;
                    }
                }
                else {
                    if (this.value >= labelRanges[i][0] && this.value <= labelRanges[i][1]) {
                        $('#' + id + 'radio_' + labels[i]).prop('checked', true);
                        break;
                    }
                }
            }
        }
        
        buttonset.buttonset('refresh');
        SetSliderValue($(this).parent().parent().attr('id'), this.value);
        SetRunButtonState(true);
    });

    //var spinner = $(inputControl).find("input[id*=spinner]").spinner();
    //var controlGroup = $(inputControl).find("div[class=controlgroup]").controlgroup();

    //When the value of the control changes update the parent's value and set the run model button
    buttonset.find("input[type=radio]").change(function () {
        SetSliderValue($(this).parent().parent().attr('id'), this.value);
        $("#" + id + "number").val(this.value);
        SetRunButtonState(true);
    });
}

// QUAY EDIT 4/12/16 BEGIN
// Add hide andf show functions for highest value radio buttons
function HideHighestInputControlValue() {

    $('.InputControl').each(function () {
        var fieldName = $(this).attr("data-key");
        var values = inputControlRadioValues[fieldName].labels
        if (values.length) {
            var lastindex = values.length - 1;
            var LastValue = values[lastindex];
            $(this).find("Label[for*='" + LastValue + "']").hide();
        }
    });
}

function ShowHighestInputControlValue() {

    $('.InputControl').each(function () {
        var fieldName = $(this).attr("data-key");
        var values = inputControlRadioValues[fieldName].labels
        if (values.length) {
            var lastindex = values.length - 1;
            var LastValue = values[lastindex];
            $(this).find("Label[for*='" + LastValue + "']").show();
        }
    });
}
// QUAY EDIT 4/12/16 END

function getLabelRanges(min, max, values) {
    var result = [],
        length = values.length,
        lastValue = 0;
    for (var i = 0; i < length; i++) {
        var tempArray = [];
        if (i == 0) {
            tempArray.push(min);
            lastValue = Math.round(values[i] + (values[i + 1] - values[i]) / 2);
            tempArray.push(lastValue);
        }
        else if (i < (length - 1)) {
            tempArray.push(lastValue);
            lastValue = Math.round(values[i] + (values[i + 1] - values[i]) / 2);
            tempArray.push(lastValue);
        }
        else {
            tempArray.push(lastValue);
            tempArray.push(max);
        }
        result.push(tempArray)
    }
    return result;
}

function inputControls2Radios() {
    //Parse input control field information
    // QUAY EDIT 4/12/16
    // ADD preparsed INFO_REQUEST
    //var fieldInfo = JSON.parse(infoRequestJSON).FieldInfo;
    var fieldInfo = INFO_REQUEST.FieldInfo;
    //-------------------------------------------------
    //Loop through all fields and find those which are input controls
    //Must have labels and their associated values specified from server to be found
    for(var i = 0; i < fieldInfo.length; i++){
        if (fieldInfo[i].labels && fieldInfo[i].labels.length) {
            //store the field and its information for when the radio control is created
            var values = fieldInfo[i].values;
            inputControlRadioValues[fieldInfo[i].FLD] = {
                checked: -1,
                values: values,
                labels: fieldInfo[i].labels,
                inputRange: [fieldInfo[i].MIN, fieldInfo[i].MAX],
                labelRanges: getLabelRanges(fieldInfo[i].MIN, fieldInfo[i].MAX, values),
            }
        }
    }

    //getting input controls
    $('.InputControl').each(function () {
        //Get the default value for the control from the slider
        inputControlRadioValues[$(this).attr("data-key")].checked = $(this).find('.InputSliderControl').slider("option", "value");
        //Convert from input control to a radio control
        inputControl2Radio(this, { height: 39 });
        console.error('.InputControl', this)
    });
 
    setFontSize('PanelUserInputs');

    $('#settings-tabs-climateDrought .InputControl').each(function () {
        //Get the default value for the control from the slider
        inputControlRadioValues[$(this).attr("data-key")].checked = $(this).find('.InputSliderControl').slider("option", "value");
        //Convert from input control to a radio control
        inputControl2Radio(this, {height: 29});
    });
    inputControlsConverted = true;
}

function updateControlGroup(inputControl, value) {
    var id = GetRootId($(inputControl).attr('id'));
    var inputNumber = $("#" + id + "number");
    inputNumber.val(value).trigger('change');
}

function setFontSize(id) {
    //$("#" + id).css({ 'font-size': '18px' });
    $("#" + id).css({ 'font-size': '16px' });
 }


function increaseFontSize(id) {
    var fontSize = parseInt($("#" + id).css("font-size"));
    fontSize = fontSize + 1 + "px";
    $("#" + id).css({ 'font-size': fontSize });
}