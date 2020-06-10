/// <reference path="/Scripts/Custom/Core/Core.js" />
/// <reference path="/Scripts/Custom/Slider/SettingSlider.js" />

var DroughtStart_ = 2013;
var DroughtStart_CommonCyclical = 2020;
var DroughtStop_CommonCyclical = 2030;
var DroughtStop_SevereLongTerm = 2030;
var DroughtStop_Extreme = 2040;
var DroughtStop_Mega = 2050;



//-----------------------------------------------
function SetFlowLabels()
{
    var COlab = document.getElementById("COFLOWYR");
    COlab.innerHTML = "Colorado River Start Year: " + CORiverFlowValue.toString();
    var SVlab = document.getElementById("SVFLOWYR");
    SVlab.innerHTML = "Salt-Verde Rivers Start Year: " + SVRiverFlowValue.toString();
}
//-----------------------------------------------
function SetFlowRadio(value) {
    switch (value) {

         case "cc":
            $('input[id="RBdry"]').prop('checked', true);
            SetFlowValues('dry');
            break;
        case "slt":
            $('input[id="RBmed"]').prop('checked', true);
            SetFlowValues('med');
            break;
        case "e":
            $('input[id="RBwet"]').prop('checked', true);
            SetFlowValues('wet');
            break;
        case "m":
            $('input[id="RBmix"]').prop('checked', true);
            SetFlowValues('mix');
            break;
        default:
            $('input[id="RBdry"]').prop('checked', true);
            SetFlowValues('dry');
            $('#img4').hide();
            $('#img1').hide();
            $('#img2').hide();
            $('#img3').hide();


    }
}
//--------------------------------------------------
// for the coorado, determine what condition matches this date
function GetCoFlow(val) {
    var rtnval = "";
    switch (val) {
        case COFlowDefault:
            rtnval = "cc";
            break;
        case COFlowMed:
            rtnval = "lt";
            break;
        case COFlowHigh:
            rtnval = "wet";
            break;
        case COFlowMixed:
            rtnval = "mix";
            break;
        default:
            rtnval = "dry";
    }

    return rtnval;
}
//---------------------------------------------
// set the Radio Button vased in this co flow
function SetCoFlow(val) {
    var rbval = GetCoFlow(val);
    SetFlowRadio(rbval);
}
//---------------------------------------------
// Get a label for the flow text value
function GetFlowLabel(val) {
    var aFlowLab = "";
    switch (val) {

        case "dry":
            aFlowLab = "Low flow years";
            break;
        case "med":
            aFlowLab = "Median flow years";
            break;
        case "wet":
            aFlowLab = "High flow years";
            break;
        case "mix":
            aFlowLab = "High inter-annual variability";
            break;
        default:
            aFlowLab = "Median flow years";
    }
    return aFlowLab;
}


//-----------------------------------------------
$('input[name="flowRecord"]').change(function () {
    var flowValue = this.value;
    SetFlowValues(flowValue);
    SetFlowLabels();
    SetRunButtonState(true);
})
// ------------------------------------------------------------------------------------------------------------------------
//


//----------------------------------------------

function setDefaultDrought() {
    // 03.05.15 DAS
   // Clear the drought text boxes for both riverine systems
    var COstart_array = $('input[id="COUSRSTR_v"]');
    COstart_array[0].value = "";
    var COstop_array = $('input[id="COUSRSTP_v"]');
    COstop_array[0].value = "";
    var STVstart_array = $('input[id="SVUSRSTR_v"]');
    STVstart = STVstart_array[0].value = "";
    var STVstop_array = $('input[id="SVUSRSTP_v"]');
    STVstop = STVstop_array[0].value="";

};


//-----------------------------------------------
