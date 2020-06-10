var DivIDCode = /#DID#/g;
var InputIDCode = /#IID#/g;
var ValueCode = /#VAL#/g
var divRadio = '<div id="#DID#buttonset" class="radio-container">';
var inputRadio = '<input type="radio" id="#DID#radio_#IID#" name="#DID#radio" value="#VAL#">';
var inputRadioChecked = '<input type="radio" id="#DID#radio_#IID#" name="#DID#radio" value="#VAL#" checked="checked">';
var labelOpen = '<label for="#DID#radio_#IID#">';
var labelClose = '</label>';
var divClose = '</div>';

var inputControlRadioValues = {};
// QUAY EDIT 2 8 16
if (window.location.href.toLowerCase().indexOf('ipad') > -1)
{
}
else {
    inputControlRadioValues['REGRECEFF'] = { checked: 16, values: [16, 58, 100], labels: ["Low", "Med", "High"] };
    inputControlRadioValues['WEBAGTR1'] = { checked: 31, values: [31, 65, 100], labels: ["Low", "Mod", "High"] };
    inputControlRadioValues['ENFLOAMT'] = { checked: 0, values: [0, 50, 100], labels: ["Limited", "Fair", "Immense"] };
    inputControlRadioValues['WEBPRPCT'] = { checked: 100, values: [0, 50, 100], labels: ["Low", "Moderate", "High"] };
    inputControlRadioValues['WEBPOPGR'] = { checked: 100, values: [50, 100, 150], labels: ["Low", "Med", "High"] };
}
var inputControlRadioLabels = ["None", "Low", "Med", "High"];

function buildRadioInputControl(id, fieldName, controlOptions) {
    var html = "";
    html += divRadio;

    var radioLabels = controlOptions.labels;

    for (var i = 0; i < radioLabels.length; i++) {
        var radio = "";
        var label = radioLabels[i];

        if (controlOptions.values[i] == controlOptions.checked)
            radio += inputRadioChecked;
        else
            radio += inputRadio;

        radio += labelOpen + label + labelClose;

        radio = radio.replace(InputIDCode, label);
        radio = radio.replace(ValueCode, controlOptions.values[i]);
        html += radio;
    }
    html += divClose;

    html = html.replace(DivIDCode, id);

    return html;
}

function inputControl2Radio(inputControl) {
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

    //When the value of the control changes update the parent's value and set the run model button
    buttonset.find("input[type=radio]").change(function () {
        SetSliderValue($(this).parent().parent().attr('id'), this.value);
        SetRunButtonState(true);
      // 02.29.16 DAS
    });
   
}


function inputControls2Radios() {
    //Parse input control field information
    var fieldInfo = JSON.parse(infoRequestJSON).FieldInfo;

    //Loop through all fields and find those which are input controls
    //Must have labels and their associated values specified from server to be found
    for(var i = 0; i < fieldInfo.length; i++){
        if(fieldInfo[i].WEBSCL.length){
            //store the field and its information for when the radio control is created
            inputControlRadioValues[fieldInfo[i].FLD] = {
                checked: -1,
                values: fieldInfo[i].WEBSCVAL,
                labels: fieldInfo[i].WEBSCL
            }
        }
    }

    //getting input controls
    $('#PanelUserInputs .InputControl').each(function () {
        //Get the default value for the control from the slider
        inputControlRadioValues[$(this).attr("data-key")].checked = $(this).find('.InputSliderControl').slider("option", "value");
        //Convert from input control to a radio control
        inputControl2Radio(this);
    });
    setFontSize('PanelUserInputs');
}

function setFontSize(id) {
    $("#" + id).css({ 'font-size': '18px' });
}

function increaseFontSize(id) {
    var fontSize = parseInt($("#" + id).css("font-size"));
    fontSize = fontSize + 1 + "px";
    $("#" + id).css({ 'font-size': fontSize });
}