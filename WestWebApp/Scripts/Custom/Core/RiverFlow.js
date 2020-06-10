/// <reference path="/Scripts/Custom/Core/Core.js" />
/// <reference path="/Scripts/Custom/Slider/SettingSlider.js" />

// low med high flow years for Saltverde
var SVFlowLow = 1946;
var SVFlowMed = 1955;
var SVFlowDefault = 1946;
var SVFlowHigh = 1965;
var SVFlowMixed = 1974;

// low med high flow years for Colorado
var COFlowLow = 1938;
var COFlowMed = 1922;
//var COFlowDefault = 1922; 03.05.15 DAS
var COFlowDefault = 1938;
var COFlowHigh = 1906;
var COFlowMixed = 1975;

var SVRiverFlowValue = SVFlowDefault;
var CORiverFlowValue = COFlowDefault;

var CODroughtStartValue = 2012;
var CODroughtStopValue = 2012;
var STVDroughtStartValue = 2012;
var STVDroughtStopValue = 2012;


//-----------------------------------------------
function SetFlowValues(value) {
    switch (value) {
        case "dry":
            SVRiverFlowValue = SVFlowDefault;
            CORiverFlowValue = COFlowDefault;
            // Load the image for dry flow traces
            $('#img1').show();
            $('#img2').hide();
            $('#img3').hide();
            $('#img4').hide();
            break;
        case "med":
            SVRiverFlowValue = SVFlowMed;
            CORiverFlowValue = COFlowMed;
            $('#img2').show();
            $('#img1').hide();
            $('#img3').hide();
            $('#img4').hide();
            break;
        case "wet":
            SVRiverFlowValue = SVFlowHigh;
            CORiverFlowValue = COFlowHigh;
            $('#img3').show();
            $('#img1').hide();
            $('#img2').hide();
            $('#img4').hide();
            break;
        case "mix":
            SVRiverFlowValue = SVFlowMixed;
            CORiverFlowValue = COFlowMixed;
            $('#img4').show();
            $('#img1').hide();
            $('#img2').hide();
            $('#img3').hide();
            break;
        default:
            SVRiverFlowValue = SVFlowDefault;
            CORiverFlowValue = COFlowDefault;
    }
}


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

         case "dry":
            $('input[id="RBdry"]').prop('checked', true);
            SetFlowValues('dry');
            break;
        case "med":
            $('input[id="RBmed"]').prop('checked', true);
            SetFlowValues('med');
            break;
        case "wet":
            $('input[id="RBwet"]').prop('checked', true);
            SetFlowValues('wet');
            break;
        case "mix":
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
            rtnval = "dry";
            break;
        case COFlowMed:
            rtnval = "med";
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


//---------------------------------------------
function SetSVFlow(val) {
    //switch (val) {
    //    case SVFlowLow:
    //        SetFlowRadio("dry");
    //        break;
    //    case SVFlowDefault:
    //        SetFlowRadio("med");
    //        break;
    //    case SVFlowHigh:
    //        SetFlowRadio("wet");
    //        break;
    //    case SVFlowMixed:
    //        SetFlowRadio("mix");
    //        break;
    //    default:
    //        SetFlowRadio("med");
    //}
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
