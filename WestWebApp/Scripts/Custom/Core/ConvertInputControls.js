var DivIDCode = /#DID#/g;
var ParentIDCode = /#PID#/g;
var InputIDCode = /#IID#/g;
var ClassCode = /#CLS#/g;
var ValueCode = /#VAL#/g;
var FieldCode = /#FLD#/g;
var divControlGroup = '<div id="#DID#controlgroup" class="realclearfix controlgroup">'
var divRadio = '<div id="#DID#buttonset-#PID#" class="radio-container" style="float:left;">';
var inputRadio = '<input type="radio" id="#DID#radio_#IID#-#PID#" name="#DID#radio-#PID#" value="#VAL#">';
var inputRadioChecked = '<input type="radio" id="#DID#radio_#IID#-#PID#" name="#DID#radio-#PID#" value="#VAL#" checked="checked">';
var inputNumber = '<input id="#DID#number" name="#DID#number-#PID#" class="input-number" data-fld="#FLD#" type="text">';
var inputNumberHidden = '<input id="#DID#number" name="#DID#number-#PID#" class="input-number" data-fld="#FLD#" type="text" style="visibility: hidden;">';
var labelOpen = '<label for="#DID#radio_#IID#-#PID#">';
var labelClose = '</label>';
var divClose = '</div>';
var inputControlsConverted = false;

var policyControlCategories = {
    source: ['SWM_P', 'GWM_P', 'RECM_P', ],
    consumer: ['PCON_P', 'ICON_P', 'UCON_P', 'ACON_P'],
    climate: ['DSCN_P'],
    pop: ['POPGRM_P']
};

var inputControlRadioValues = {};
// QUAY EDIT 2 8 16
// STEPTOE NOTE: ipad check below
if (window.location.href.toLowerCase().indexOf('ipad') > -1) {
    // inputControlRadioValues['UCON'] = { checked: "High", values: [0, 50, 75, 100], labels: ["None", "Low", "Med", "High"] };
    // inputControlRadioValues['ACON'] = { checked: "High", values: [0, 50, 75, 100], labels: ["None", "Low", "Med", "High"] };
    // inputControlRadioValues['PCON'] = { checked: "High", values: [0, 50, 75, 100], labels: ["None", "Low", "Med", "High"] };
}
else {
    inputControlRadioValues['REGRECEFF'] = { checked: 16, values: [16, 58, 100], labels: ["Low", "Med", "High"] };
    inputControlRadioValues['WEBAGTR1'] = { checked: 31, values: [31, 65, 100], labels: ["Low", "Mod", "High"] };
    inputControlRadioValues['ENFLOAMT'] = { checked: 0, values: [0, 50, 100], labels: ["Limited", "Fair", "Immense"] };
    inputControlRadioValues['WEBPRPCT'] = { checked: 100, values: [0, 50, 100], labels: ["Low", "Moderate", "High"] };
    inputControlRadioValues['WEBPOPGR'] = { checked: 100, values: [50, 100, 150], labels: ["Low", "Med", "High"] };
}
var inputControlRadioLabels = ["None", "Low", "Med", "High"];
var inputControlRadioClasses = ["none", "low", "med", "high"];

function buildRadioInputControl(id, fieldName, controlOptions, parentID, hideInputNumber) {
    var html = "";
    html += divControlGroup;
    html += divRadio;

    var radioLabels = controlOptions.labels;
    var radioValues = controlOptions.values;
    var foundChecked = false;
    var length = radioValues.length;

    for (var i = 0; i < length; i++) {
        var radio = "";
        var label = radioLabels[i];
        var labelRanges = controlOptions.labelRanges;

        if (radioValues[i] == controlOptions.checked) {
            radio += inputRadioChecked;
            foundChecked = true;
        }
        else {
            var radioDefined = false;
            if (!foundChecked) {
                if (controlOptions.right2Left) {
                    if (i < (length - 1)) {
                        if (controlOptions.checked <= labelRanges[i][0] && controlOptions.checked > labelRanges[i][1]) {
                            radio += inputRadioChecked;
                            foundChecked = true;
                            radioDefined = true;
                        }
                    }
                    else {
                        if (controlOptions.checked <= labelRanges[i][0] && controlOptions.checked >= labelRanges[i][1]) {
                            radio += inputRadioChecked;
                            foundChecked = true;
                            radioDefined = true;
                        }
                    }
                }
                else {
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
            }

            if (!radioDefined) {
                radio += inputRadio;
            }
        }


        //radio += labelOpen + "&nbsp;&nbsp;&nbsp;" + labelClose;
        // Use below to edit what is shown on the input labels
        radio += labelOpen + (radioLabels[i] != "" ? radioLabels[i] : "&nbsp;") + labelClose;

        //radio = radio.replace(InputIDCode, label);
        radio = radio.replace(InputIDCode, radioValues[i]);
        radio = radio.replace(ValueCode, radioValues[i]);
        html += radio;
    }
    html += divClose;

    html += (hideInputNumber == true ? inputNumberHidden : inputNumber) + divClose;

    html = html.replace(FieldCode, fieldName);
    html = html.replace(DivIDCode, id);
    html = html.replace(ParentIDCode, parentID);

    return html;
}

var hoverTimer;
var hoverVisible = false;

function inputControl2Radio(inputControl, options, mode) {
    //Hide the slider container
    var sliderContainer = $(inputControl).find("div[class=slider-container]");
    sliderContainer.hide();

    var id = GetRootId($(inputControl).attr('id'));

    var fieldName = $(inputControl).attr("data-key");
    
    var side = "left";
    switch (fieldName) {
        case "UCON_P":
        case "ACON_P":
        case "ICON_P":
            side = "right";
            break;
            //case "GWM":
            //case "SWM":
            //default:
            //    break;
    }

    var parents = [];
    var buttonsets = [];

    // If not a climate control then build and append controls for specified tabs
    // otherwise build and append climate control
    if (mode == undefined || mode == 0) {
        // Get the html to add the radio input control to the page and insert it after the slider
        var htmlFlow = buildRadioInputControl(id, fieldName, inputControlRadioValues[fieldName], 'flowChart');
        var htmlChart = buildRadioInputControl(id, fieldName, inputControlRadioValues[fieldName], 'barCharts');
        var htmlChartLine = buildRadioInputControl(id, fieldName, inputControlRadioValues[fieldName], 'lineCharts');

        //console.log('inputControl2Radio fieldName:', fieldName);
        var controlCategory = '';

        $.each(policyControlCategories, function (type, controls) {
            if (controls.indexOf(fieldName) > -1) {
                controlCategory = type;
                return false;
            }
        });

        //console.log('controlCategory:', controlCategory);

        // Append the generated controls to the respective tabs
        //$('#' + side + '-controls').append(htmlFlow);
        $('#flow-controls #policy-' + controlCategory + '-controls').append(htmlFlow);
        $('#chart-controls').append(htmlChart);
        $('#line-chart-controls').append(htmlChartLine);

        // Store the parents for the buttonsets
        parents = ['-flowChart', '-barCharts', '-lineCharts'];

        // Store a reference to each buttonset that was added to the page
        $.each(parents, function (index, parent) {
            buttonsets.push($('#' + id + 'buttonset' + parent))
        });
    }
    else {
        // Get the html to add the radio input control to the page and insert it after the slider
        var htmlClimate = buildRadioInputControl(id, fieldName, inputControlRadioValues[fieldName], 'climateTab', true);

        // Append the generated control to the climate tab
        $('#climate-controls').append(htmlClimate);

        // Store the parents for the buttonsets
        parents = ['-climateTab'];

        // Store a reference to each buttonset that was added to the page
        buttonsets = [$('#' + id + 'buttonset' + parents[0])];
    }

    //Convert the designated control to a buttonset
    buttonsets.forEach(function (bsObject, bIndex) {
        var buttonset = bsObject.buttonset();
        var currentIndex = bIndex;
        var controlDescription = $('#' + id + 'lblSliderfldName');

        // Make a clone of the control description and insert it before the buttonset
        controlDescription.clone().insertBefore(buttonset);
        $('<img class="info-icon info-item" src="Images/info_icon.png" data-fld="' + fieldName + '"/>').insertBefore(buttonset);
        $('<br>').insertBefore(buttonset);

        //When the value of the control changes update the parent's value and set the run model button
        buttonset.find("input[type=radio]").change(function () {
            // Get the input-number that corresponds to the buttonset that was changed
            var inputNumber = $(this).parent().siblings('.input-number');
            var value = this.value;

            // Set all input-numbers for this field to the specified value
            $("[id=" + inputNumber.attr('id') + "]").val(this.value);

            inputNumber.css('background-color', '');

            // For all buttonsets other than the one clicked update the checked label and refresh
            buttonsets.forEach(function (bsObject, bIndex) {
                if (currentIndex != bIndex) {
                    var buttonset = bsObject.buttonset();
                    buttonset.children('#' + id + 'radio_' + value + parents[bIndex]).prop('checked', true);
                    buttonset.buttonset('refresh');
                }
            });

            // Set the hidden slider to the specified value and set the run button to active
            SetSliderValue(id + 'ControlContainer', this.value);
            SetRunButtonState(true);
        });

        // STEPTOE EDIT HIDE NUMBER
        //if (mode == 1) {
        //    return;
        //}

        // Extract relevant data from the specified input control
        var inputRange = inputControlRadioValues[fieldName].inputRange,
        labelRanges = inputControlRadioValues[fieldName].labelRanges,
        labels = inputControlRadioValues[fieldName].labels,
        values = inputControlRadioValues[fieldName].values;

        // Get the input-number element and set its value to the current value
        var autoComp = buttonset.siblings('.input-number');
        autoComp.val(inputControlRadioValues[fieldName].checked);

        // If a height is specified set the height of the input-number element
        if (options) {
            if (options.height)
                autoComp.height(options.height);
        }

        // Depending on if the control is left2Right or right2Left the comparisons for the change event differ.
        // left2Right = 0 -> 100. right2Left = 100 -> 0.
        if (inputControlRadioValues[fieldName].right2Left) {
            autoComp.change(function () {
                // Id of the element that will be checked after comparisons
                var toCheckId = '';
                var thisValue = +this.value;

                // If the value is above the max then cap it at the max
                // Else if the value is below the min then cap it at the min
                // Otherwise iterate over the label ranges until the appropriate label is found
                if (this.value >= inputRange[1]) {
                    toCheckId = '#' + id + 'radio_' + values[0];
                    this.value = inputRange[1];
                }
                else if (this.value <= inputRange[0]) {
                    toCheckId = '#' + id + 'radio_' + values[values.length - 1];
                    this.value = inputRange[0];
                }
                //else if (values.indexOf(thisValue) > -1) {
                //    toCheckId = '#' + id + 'radio_' + values.indexOf(thisValue);
                //}
                else {
                    var length = labelRanges.length;
                    for (var i = 0; i < length; i++) {
                        if (i < length - 1) {
                            if (this.value <= labelRanges[i][0] && this.value > labelRanges[i][1]) {
                                toCheckId = '#' + id + 'radio_' + values[i];
                                break;
                            }
                        }
                        else {
                            if (this.value <= labelRanges[i][0] && this.value >= labelRanges[i][1]) {
                                toCheckId = '#' + id + 'radio_' + values[i];
                                break;
                            }
                        }
                    }
                }

                // For each buttonset set the appropriate label to checked and refresh
                buttonsets.forEach(function (bsObject, bIndex) {
                    var buttonset = bsObject.buttonset();
                    buttonset.children(toCheckId + parents[bIndex]).prop('checked', true);
                    buttonset.buttonset('refresh');
                });

                // Set all input-number controls for this field to the specified value
                $("[id=" + this.id + "]").val(this.value);

                // Set the hidden slider to the specified value and set the run button to active
                SetSliderValue(id + 'ControlContainer', this.value);
                SetRunButtonState(true);
                
                if (!waitingForData) {
                    $(this).css('background-color', '');
                }
            });
        }
        else {
            autoComp.change(function () {
                // Id of the element that will be checked after comparisons
                var toCheckId = '';
                var thisValue = +this.value;

                // If the value is above the max then cap it at the max
                // Else if the value is below the min then cap it at the min
                // Otherwise iterate over the label ranges until the appropriate label is found
                if (this.value >= inputRange[1]) {
                    toCheckId = '#' + id + 'radio_' + values[values.length - 1];
                    this.value = inputRange[1];
                }
                else if (this.value <= inputRange[0]) {
                    toCheckId = '#' + id + 'radio_' + values[0];
                    this.value = inputRange[0];
                }
                //else if (values.indexOf(thisValue) > -1) {
                //    toCheckId = '#' + id + 'radio_' + values.indexOf(thisValue);
                //}
                else {
                    var length = labelRanges.length;
                    for (var i = 0; i < length; i++) {
                        if (i < length - 1) {
                            if (this.value >= labelRanges[i][0] && this.value < labelRanges[i][1]) {
                                toCheckId = '#' + id + 'radio_' + values[i];
                                break;
                            }
                        }
                        else {
                            if (this.value >= labelRanges[i][0] && this.value <= labelRanges[i][1]) {
                                toCheckId = '#' + id + 'radio_' + values[i];
                                break;
                            }
                        }
                    }
                }

                // For each buttonset set the appropriate label to checked and refresh
                buttonsets.forEach(function (bsObject, bIndex) {
                    var buttonset = bsObject.buttonset();
                    buttonset.children(toCheckId + parents[bIndex]).prop('checked', true);
                    buttonset.buttonset('refresh');
                });

                // Set all input-number controls for this field to the specified value
                $("[id=" + this.id + "]").val(this.value);
                
                // Set the hidden slider to the specified value and set the run button to active
                SetSliderValue(id + 'ControlContainer', this.value);
                SetRunButtonState(true);

                if (!waitingForData) {
                    $(this).css('background-color', '');
                }
            });
        }
    });

    return buttonsets;
}

// Hide highest value radio buttons
function HideHighestInputControlValue() {

    $('.InputControl').each(function () {
        var fieldName = $(this).attr("data-key");
        var values = inputControlRadioValues[fieldName].values
        if (values.length) {
            var lastindex = values.length - 1;
            var LastValue = values[lastindex];
            $(this).find("Label[for*='" + LastValue + "']").hide();
        }
    });
}

// Show highest value radio buttons
function ShowHighestInputControlValue() {

    $('.InputControl').each(function () {
        var fieldName = $(this).attr("data-key");
        var values = inputControlRadioValues[fieldName].values
        if (values.length) {
            var lastindex = values.length - 1;
            var LastValue = values[lastindex];
            $(this).find("Label[for*='" + LastValue + "']").show();
        }
    });
}

// Compute the label ranges for the provided min, max and values
function getLabelRanges(min, max, values) {
    var result = [],
        length = values.length,
        lastValue = 0,
        pushFirst = min,
        pushLast = max;

    if (values[0] > values[1]) {
        pushFirst = max,
        pushLast = min;
    }

    // For the number of values generated equally spaced ranges
    for (var i = 0; i < length; i++) {
        var tempArray = [];
        if (i == 0) {
            tempArray.push(pushFirst);
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
            tempArray.push(pushLast);
        }
        result.push(tempArray)
    }
    return result;
}

// STEPTOE EDIT 06/21/17 END
//['#00B2EB', '#51C1E1', '#A2D1D7', '#F3E1CE'],
var gradientColors = [
    ['#00B2EB', '#51C1E1', '#A2D1D7', '#F3E1CE'],
    ['#34B6ED', '#65C3E8', '#96D0E4', '#C6DCDF', '#F7E9DA'],
    ['#34B6ED', '#5BC0E9', '#82CAE5', '#A9D5E2', '#D0DFDE', '#F7E9DA'],
];

// Generate the gradients for the buttonsets, dependent on if they are left2Right or right2Left
function generateButtonGradients() {
    $.each(gradientColors, function (gIndex, colors) {
        var buttonCount = colors.length - 1;
        for (var cIndex = 0; cIndex < buttonCount; cIndex++) {
            // Left to right
            var style = document.createElement('style');
            style.type = 'text/css';
            style.innerHTML = ".radio-container label.ltr" + buttonCount + "-" + cIndex + "{"
                + "background: " + colors[cIndex] + ";"
                + "background: -moz-linear-gradient(left, " + colors[cIndex] + " 0%, " + colors[cIndex + 1] + " 100%) !important;"
                + "background: -webkit-linear-gradient(left, " + colors[cIndex] + " 0%, " + colors[cIndex + 1] + " 100%) !important;"
                + "background: linear-gradient(to right, " + colors[cIndex] + " 0%, " + colors[cIndex + 1] + " 100%) !important;"
                + "}";
            document.getElementsByTagName('head')[0].appendChild(style);

            // Right to left
            style = document.createElement('style');
            style.type = 'text/css';
            style.innerHTML = ".radio-container label.rtl" + buttonCount + "-" + (buttonCount - cIndex - 1) + "{"
                + "background: " + colors[cIndex + 1] + ";"
                + "background: -moz-linear-gradient(left, " + colors[cIndex + 1] + " 0%, " + colors[cIndex] + " 100%) !important;"
                + "background: -webkit-linear-gradient(left, " + colors[cIndex + 1] + " 0%, " + colors[cIndex] + " 100%) !important;"
                + "background: linear-gradient(to right, " + colors[cIndex + 1] + " 0%, " + colors[cIndex] + " 100%) !important;"
                + "}";
            document.getElementsByTagName('head')[0].appendChild(style);
        };
    });
}


function generateButtonColors() {
    var buttonColors = ['#C4ECF5', '#8BE6F7', '#55CDE4', '#37B6D7', '#32A8C9'];

    $.each(gradientColors, function (gIndex, colors) {
        var buttonCount = colors.length - 1;
        for (var cIndex = 0; cIndex < buttonCount; cIndex++) {
            // Left to right
            var style = document.createElement('style');
            style.type = 'text/css';
            style.innerHTML = ".radio-container label.ltr" + buttonCount + "-" + cIndex + "{"
                + "background: " + colors[cIndex] + ";"
                + "background: -moz-linear-gradient(left, " + colors[cIndex] + " 0%, " + colors[cIndex + 1] + " 100%) !important;"
                + "background: -webkit-linear-gradient(left, " + colors[cIndex] + " 0%, " + colors[cIndex + 1] + " 100%) !important;"
                + "background: linear-gradient(to right, " + colors[cIndex] + " 0%, " + colors[cIndex + 1] + " 100%) !important;"
                + "}";
            document.getElementsByTagName('head')[0].appendChild(style);

            // Right to left
            style = document.createElement('style');
            style.type = 'text/css';
            style.innerHTML = ".radio-container label.rtl" + buttonCount + "-" + (buttonCount - cIndex - 1) + "{"
                + "background: " + colors[cIndex + 1] + ";"
                + "background: -moz-linear-gradient(left, " + colors[cIndex + 1] + " 0%, " + colors[cIndex] + " 100%) !important;"
                + "background: -webkit-linear-gradient(left, " + colors[cIndex + 1] + " 0%, " + colors[cIndex] + " 100%) !important;"
                + "background: linear-gradient(to right, " + colors[cIndex + 1] + " 0%, " + colors[cIndex] + " 100%) !important;"
                + "}";
            document.getElementsByTagName('head')[0].appendChild(style);
        };
    });
}

function initClimateEffectButtonSet() {
    var buttonsetDC = $("#DSCNInputUserControl_buttonset").buttonset();
    //var buttonsetCLIM = $("#CLIMInputUserControl_buttonset").buttonset();
    //var buttonsetDC = $("#DCInputUserControl_buttonset").buttonset();
    //buttonset.find("input[type=radio]").change(function () {
    //    var coFlow = $('.InputControl[data-key="COCLMADJ"]').find(".input-number"),
    //        svFlow = $('.InputControl[data-key="SVCLMADJ"]').find(".input-number"),
    //        flowRecordList = $('#flowRecordList');

    //    switch (this.value) {
    //        case "0":
    //            flowRecordList.val('med').change();
    //            coFlow.val(100).change();
    //            svFlow.val(100).change();
    //            break;

    //        case "1":
    //            flowRecordList.val('med').change();
    //            coFlow.val(89).change();
    //            svFlow.val(80).change();
    //            break;
    //        case "2":
    //            // flowRecordList.val('dry').change();
    //            //coFlow.val(50).change();
    //            //svFlow.val(40).change();
    //            // coFlow.val(60).change();
    //            // svFlow.val(55).change();

    //            break;
    //        case "3":
    //            flowRecordList.val('dry').change();
    //            coFlow.val(70).change();
    //            svFlow.val(65).change();
    //            break;
    //        case "4":
    //            flowRecordList.val('dry').change();
    //            coFlow.val(60).change();
    //            svFlow.val(55).change();
    //            break;
    //        case "5":
    //            flowRecordList.val('wet').change();
    //            coFlow.val(120).change();
    //            svFlow.val(120).change();
    //            break;

    //    }
    //});
}

function inputControls2Radios() {
    // Generate button gradients
    //generateButtonGradients();
    generateButtonColors();

    //Parse input control field information
    // QUAY EDIT 4/12/16
    // ADD preparsed INFO_REQUEST
    //var fieldInfo = JSON.parse(infoRequestJSON).FieldInfo;
    var fieldInfo = INFO_REQUEST.FieldInfo;

    // INFO_REQUEST['FieldInfo'].forEach((d)=>{if(['COCLMADJ', 'SVCLMADJ'].indexOf(d.FLD) > -1)console.log(JSON.stringify(d))});
    //fieldInfo.push({ "FLD": "COCLMADJ", "LAB": "Alter Historical Flows- Colorado", "MIN": 0, "MAX": 125, "TYP": "IB", "RCT": "Range", "LNG": "The percent (Value=50 is 50%) which is used to modify the Colorado river flow record, simulating impacts of climate change.  Change starts (or impacts the flow record) in any year that the value differs from 100%.", "UNT": "%", "UNTL": "% of that forecasted", "DEP": [], "values": [0, 25, 50, 75, 100, 125], "labels": ["0", "25", "50", "75", "100", "125"] });
    //fieldInfo.push({ "FLD": "SVCLMADJ", "LAB": "Alter Historical Flows- Salt-Verde", "MIN": 0, "MAX": 125, "TYP": "IB", "RCT": "Range", "LNG": "The percent (Value=50 is 50%) which is used to modify the Salt Verde River flow record, simulating impacts of climate change.  Change starts at beginning of Simulation (or in any year where the value departs from 100%).", "UNT": "%", "UNTL": "% of that forecasted", "DEP": [], "values": [0, 25, 50, 75, 100, 125], "labels": ["0", "25", "50", "75", "100", "125"] });

    //-------------------------------------------------
    //Loop through all fields and find those which are input controls
    //Must have labels and their associated values specified from server to be found
    for (var i = 0; i < fieldInfo.length; i++) {
        if (fieldInfo[i].labels.length) {
            //store the field and its information for when the radio control is created
            var values = fieldInfo[i].values,
                right2Left = values[0] > values[1];
            inputControlRadioValues[fieldInfo[i].FLD] = {
                checked: -1,
                values: values,
                labels: fieldInfo[i].labels,
                inputRange: [fieldInfo[i].MIN, fieldInfo[i].MAX],
                labelRanges: getLabelRanges(fieldInfo[i].MIN, fieldInfo[i].MAX, values),
                right2Left: right2Left,
            }
        }
    }

    //getting input controls
    $('#PanelUserInputs .InputControl').each(function () {
        //Get the default value for the control from the slider
        var id = $(this).attr("data-key");
        var radioValues = inputControlRadioValues[id];
        radioValues.checked = $(this).find('.InputSliderControl').slider("value");
        //console.log(id, ',', radioValues);
        //Convert from input control to a radio control
        //var buttonsets = inputControl2Radio(this, { height: 39 });
        var buttonsets = inputControl2Radio(this, {}, (policyControlCategories.climate.indexOf(id) > -1));
        buttonsets.forEach(function (buttonset, bIndex) {
            var labels = buttonset.find('label');
            for (var i = 0; i < labels.length; i++) {
                var label = $(labels[i]);
                var text = $(labels[i]).attr('for').split("_")[2];
                var classToAdd = inputControlRadioClasses[radioValues.labels.indexOf(text)];
                //label.addClass((radioValues.right2Left ? 'rtl' : 'ltr') + labels.length + '-' + i);
                //label.addClass('rtl' + labels.length + '-' + i);
                label.addClass('input-button-' + i);
            }
        });
    });

    //$('#settings-tabs-climateDrought .InputControl').each(function () {
    //    //Get the default value for the control from the slider
    //    var id = $(this).attr("data-key");
    //    console.log(id);
    //    var radioValues = inputControlRadioValues[id];
    //    radioValues.checked = $(this).find('.InputSliderControl').slider("option", "value");
    //    //Convert from input control to a radio control
    //    var buttonsets = inputControl2Radio(this, { height: 39 }, 1);
    //});

    initClimateEffectButtonSet();

    $('#CLIMInputUserControl_radio_0').prop('checked', true);
    $('#CLIMInputUserControl_buttonset').buttonset('refresh');

    $('#DCInputUserControl_radio_0').prop('checked', true);
    $('#DCInputUserControl_buttonset').buttonset('refresh');

    //setFontSize('left-controls');
    //setFontSize('right-controls');
    setFontSize('flow-controls')
    setFontSize('chart-controls');
    setFontSize('line-chart-controls');
    setFontSize('input-controls');

    inputControlsConverted = true;
}

function updateControlGroup(inputControl, value, noChange) {
    var id = GetRootId($(inputControl).attr('id'));
    var number = $("[id=" + id + "number]");
    //console.log('updateControlGroup:', id, inputControl, value, noChange, number);
    if (number.length) {
        if (noChange) {
            number.val(value);

        }
        else {
            number.val(value).trigger('change');
        }
    }
    else {
        console.log('ERROR ucp:', id, $(inputControl).attr('id'), number);
    }
}

function setFontSize(id) {
    $("#" + id).css({ 'font-size': '18px' });
}


function increaseFontSize(id) {
    var fontSize = parseInt($("#" + id).css("font-size"));
    fontSize = fontSize + 1 + "px";
    $("#" + id).css({ 'font-size': fontSize });
}

// Resize Flow Diagram control divs for an exact fit
function resizeFlowControlDivs() {    
    var maxWidth = 0;
    $("#left-controls").find(".controlgroup").each(function (i, d) {
        var width = $(d).width();
        if ($(d).width() > maxWidth){// && (CT[getState()].IFLDS.indexOf(d.id.substring(0, 3)) > -1) || CT[getState()].IFLDS.indexOf(d.id.substring(0, 4)) > -1) {
            maxWidth = $(d).width();
        }
    });
    $("#left-controls").width(maxWidth);

    maxWidth = 0;
    $("#right-controls").find(".controlgroup").each(function (i, d) {
        var width = $(d).width();
        if ($(d).width() > maxWidth){// && (CT[getState()].IFLDS.indexOf(d.id.substring(0, 3)) > -1) || CT[getState()].IFLDS.indexOf(d.id.substring(0, 4)) > -1) {
            maxWidth = $(d).width();
        }
    });
    $("#right-controls").width(maxWidth);
}

function hideLastInputButton() {
    $('.input-button-4').hide();
}

function showLastInputButton() {
    $('.input-button-4').show();
}

$("#hideLastInputButton").click(function () {
    hideLastInputButton();
});

$("#showLastInputButton").click(function () {
    showLastInputButton();
});

$(document).ready(function () {
    //resizeFlowControlDivs();
})