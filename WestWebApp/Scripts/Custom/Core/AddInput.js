/// <reference path="/Scripts/Custom/Core/Core.js" />
var InputControlDivCode = "#ICD#";
var InputFldNameCode = /#FLD#/g;
var InputUnitCode = "#UNIT#";
var InputLabelCode = "#LAB#";
var ScaleNumberCode = "#SN#";
var ScaleValueCode = "#SV#";
var ScalePctCode = "#SP#";
var MinValueCode = "#MIN#";
var MaxValueCode = /#MAX#/g;
var SubValueCode = "#SUB#";
var divCLose = "</div>";
var NewLineCode = "";//String.fromCharCode(13) + String.fromCharCode(10);
var divInputControl = "<div id='#FLD#InputUserControl' class='dynamicinputcontrol' data-key='#FLD#'>";
var divControlContainer = "<div id='#FLD#InputUserControl_ControlContainer' class='InputControl' data-key='#FLD#' data-fld='#FLD#' data-Subs='#SUB#'>";
var spanlblSliderfldName = "<span id='#FLD#InputUserControl_lblSliderfldName'>#LAB#</span> : <span id='#FLD#InputUserControl_lblSliderVal'></span> #UNIT# ";
var divHelpContainer = "<div id='#FLD#InputUserControl_containerHelp' class='help'>";
var inputHelpURI = "<input type='hidden' name='ctl00$#FLD#InputUserControl$hvHelpURI' id='#FLD#InputUserControl_hvHelpURI' value='Content/HELPFILES/' />";
var imgHelpIcon = "<img src='../Images/icon_help.png' />";
var spanKeyWord = "<span id='#FLD#InputUserControl_lblSliderKeyWord'>#FLD#</span><p><span class='icon-close-open'></span></p>";
var divSLiderContainer = "<div class='slider-container'>";
var divPopup = "<div id='#FLD#InputUserControl_PopupButton' class='ui-slider-popup-button'></div>";
var divSlider = "<div id='#FLD#InputUserControl_divslider' class='InputSliderControl' data-min='#MIN#' data-max='#MAX#' data-def='#MAX#'></div>";
var divScale = "<div class='scale'>";
var spanScaleTemplate = "<span id='#FLD#InputUserControl_lblScalept#SN#' style='left: #SP#%'>#SV#</span>";

///-------------------------------------------------------------------------------------------------
/// <summary>   Spaces. </summary>
/// <remarks> Create a string that contains N spaces </remarks>
/// <param name="parameter1">   The first parameter. Number of spaces</param>
///
/// <returns>  string . </returns>
///-------------------------------------------------------------------------------------------------

function Spaces(N) {
    var _space = "";
    for (var i=0;i<N;i++)
        _space += " ";
    return _space;
}

///-------------------------------------------------------------------------------------------------
/// <summary>   Builds the scales for InputControl. </summary>
///
/// <param name="parameter1">   The first parameter, required, the minumum value of range. </param>
/// <param name="parameter2">   The second parameter, required, the minumum value of range. </param>
/// <param name="parameter3">   The third parameter, required, the number of desried scales. </param>
/// <param name="parameter4">   The fourth parameter, optional, true if scales should be round numbers. </param>
///
/// <returns>   . </returns>
///-------------------------------------------------------------------------------------------------

function BuildScales(min, max, desiredNumber, round) {
    // setup scales
    var theScales = [];
    // set rounding
    var isround = true;
    if (round != undefined) { isround = round; }
    // make sure numbers are valid
    if ((desiredNumber > 1) && ((max - min) > 0)) {
        var aValue = 0;
        // set the first scale
        if (isround) { aValue = Math.round(min); } else { aValue = min; }
        theScales[0] = aValue;
        // estimate inc
        var inc = (max - min) / (desiredNumber - 1);
        // set inbetween scales
        for (var i = 1; i < (desiredNumber - 1) ; i++) {
            var temp = min + (inc * i);
            if (isround) { aValue = Math.round(temp); } else { aValue = temp; }
            theScales[i] = aValue;
        }
        // set last scale
        if (isround) { aValue = Math.round(max); } else { aValue = max; }
        theScales[desiredNumber - 1] = aValue;
    }
    return theScales;
}

var DynamicInputCount = 0;
///-------------------------------------------------------------------------------------------------
/// <summary>   Builds a dynamic input control html. </summary>
/// <remarks> Creates string of HTML that can be inserted to create an Input Control for Fieldname, with Unit using COntrolIndex as a unique id</remarks>
/// <param name="parameter1">   The first parameter, required string TheFieldname for control. </param>
/// <param name="parameter2">   The second parameter, required, string the units for the control (use short units) . </param>
/// <param name="parameter3">   The third parameter, required int the number of this control, each control has to have a unique number. </param>
/// <returns>  a string, the html for the input control. </returns>
///-------------------------------------------------------------------------------------------------

function BuildDynamicInputControlHTML(Fieldname, ControlIndex, NumberOfScales) {
    // Get thus fields info 
    var FullHTML = "";
    theFieldInfo = FieldInfoArray[Fieldname];
    // ok this will be undefined if field is not in info list
    if (theFieldInfo != undefined) {
        // OK, Get the field info
        var Label = theFieldInfo.LAB;
        var Unit = theFieldInfo.UNT;
        var MinValue = theFieldInfo.MIN;
        var MaxValue = theFieldInfo.MAX;
        var UseFieldname = theFieldInfo.FLD;
        var DependFields = theFieldInfo.DEP;
        // OK, now build the scales
        var MyScales = BuildScales(MinValue, MaxValue, NumberOfScales, true);
        // now build the base string
        FullHTML = divInputControl + NewLineCode + Spaces(4) + divControlContainer + NewLineCode + Spaces(8) + spanlblSliderfldName + NewLineCode;
        FullHTML +=  Spaces(8) + divHelpContainer + NewLineCode + Spaces(12) + inputHelpURI + NewLineCode;
        FullHTML += Spaces(12) + imgHelpIcon + NewLineCode + Spaces(8) + divCLose + NewLineCode + Spaces(8) + spanKeyWord + NewLineCode;
        FullHTML += Spaces(8) + divSLiderContainer + NewLineCode + Spaces(12) + divPopup + NewLineCode + Spaces(12) + divSlider + NewLineCode;
        FullHTML += Spaces(12) + divScale + NewLineCode;
        // add the scales
        var pctval = 100 / (NumberOfScales - 1);
        for (var i = 0; i < MyScales.length; i++) {
            var scalestr = spanScaleTemplate.replace(ScaleNumberCode, (i+1).toString());
            scalestr = scalestr.replace(ScaleValueCode, "");// MyScales[i].toString());
            scalestr = scalestr.replace(ScalePctCode, (i * pctval).toString());
            FullHTML += Spaces(16) + scalestr + NewLineCode;
        }
        // close all the divisions
        FullHTML += Spaces(12) + divCLose + NewLineCode + Spaces(8) + divCLose + NewLineCode + Spaces(4) + divCLose + NewLineCode + divCLose + NewLineCode ;
        // build with replacements
        FullHTML = FullHTML.replace(InputFldNameCode, UseFieldname);
        FullHTML = FullHTML.replace(InputUnitCode, Unit);
        FullHTML = FullHTML.replace(InputLabelCode, Label);
        FullHTML = FullHTML.replace(MaxValueCode, MaxValue.toString());
        FullHTML = FullHTML.replace(MinValueCode, MinValue.toString());
        FullHTML = FullHTML.replace(SubValueCode, DependFields);

        FullHTML = FullHTML.replace(InputControlDivCode, ControlIndex.toString());
    }
    return FullHTML;
}

var whatthe = "<div id='COEXTSTYRInputUserControl' class='dynamicinputcontrol' data-key='COEXTSTYR'>    <div id='COEXTSTYRInputUserControl_ControlContainer' class='InputControl' data-key='COEXTSTYR' data-fld='COEXTSTYR' data-Subs=''>        <span id='COEXTSTYRInputUserControl_lblSliderfldName'></span> : <span id='COEXTSTYRInputUserControl_lblSliderVal'></span> Year        <div id='COEXTSTYRInputUserControl_containerHelp' class='help'>            <input type='hidden' name='ctl00$COEXTSTYRInputUserControl$hvHelpURI' id='COEXTSTYRInputUserControl_hvHelpURI' value='Content/HELPFILES/' />            <img src='../Images/icon_help.png' />        </div>        <span id='COEXTSTYRInputUserControl_lblSliderKeyWord'>COEXTSTYR</span><p><span class='icon-close-open'></span></p>        <div class='slider-container'>            <div id='COEXTSTYRInputUserControl_PopupButton' class='ui-slider-popup-button'></div>            <div id='COEXTSTYRInputUserControl_divslider' class='InputSliderControl' data-min='1950' data-max='1970' data-def='1970'></div>            <div class='scale'>                <span id='COEXTSTYRInputUserControl_lblScalept0' style='left: 0%'>762</span>                <span id='COEXTSTYRInputUserControl_lblScalept1' style='left: 0%'>1066</span>                <span id='COEXTSTYRInputUserControl_lblScalept2' style='left: 0%'>1371</span>                <span id='COEXTSTYRInputUserControl_lblScalept3' style='left: 0%'>1675</span>                <span id='COEXTSTYRInputUserControl_lblScalept4' style='left: 0%'>1979</span>            </div>        </div>    </div></div>"
///-------------------------------------------------------------------------------------------------
/// <summary>   Adds a dynamic input control to 'parameter2'. </summary>
/// <remarks> Adds an input control for the Fieldname to the Target Control </remarks>
/// <param name="parameter1">   The first parameter, required, fieldname to add control for. </param>
/// <param name="parameter2">   The second parameter, required, the ID of the DOM object to put html into</param>
///
/// <returns>  Nothing </returns>
///-------------------------------------------------------------------------------------------------

function AddDynamicInputControl(Fieldname, targetControlId) {
    var theControl = $("#" + targetControlId);
    if (theControl != undefined) {
        var thisIDN = DynamicInputCount + 1;
        var theHTML = BuildDynamicInputControlHTML(Fieldname, thisIDN, 6);
        if ((theHTML != undefined) && (theHTML != "")) {
            DynamicInputCount++;
            var oldHTML = theControl[0].innerHTML;
            theControl.html(oldHTML+theHTML);  //whatthe);             //theHTML);
            thesliderControl = $("#" + Fieldname + "InputUserControl");
            thesliderControl.find("span[id*=lblSliderKeyWord]").hide();
            _setSlider(thesliderControl);
           // callWebService(getJSONData('parent'));
        }
    }
}

