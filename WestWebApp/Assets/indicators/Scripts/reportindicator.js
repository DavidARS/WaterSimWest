/// <reference path="/Assets/indicators/Scripts/indicatorControl_v_4.js" />
///                                                          
var GWPIndicatorControl;
var ENVIndicatorControl;
var AGRIndicatorControl;
var PWCIndicatorControl;
var AWSIndicatorControl;

var IndicatorControlsArray = new Array();

var indicatorParameters = {

    "GWSYA_P": {
        divId: "idGWSYADiv", anIndicatorType: "GWSYA_P", ControlId: "idGWSYAIndicator", options: { }
    },
    "ECOR_P": {
        divId: "idECORDiv", anIndicatorType: "ECOR_P", ControlId: "idECORIndicator", options: {  }
    },
    "EVIND_P": {
        divId: "idENVINDDiv", anIndicatorType: "EVIND_P", ControlId: "idENVINDIndicator", options: {  }
    },
    "PEF_P": {
        divId: "idPEFDiv", anIndicatorType: "PEF_P", ControlId: "idPEFIndicator", options: {
            meter: {
                style: 'rgr_meter'
            }
        }
    },
    "AGIND_P": {
        divId: "idAGINDDiv", anIndicatorType: "AGIND_P", ControlId: "idAGINDIndicator", options: {
            meter: {
                style: 'rgr_meter'
            }
        }
    },
    "IEF_P": {
        divId: "idIEFDiv", anIndicatorType: "IEF_P", ControlId: "idIEFIndicator", options: {
            meter: {
                style: 'rgr_meter'
            }
        }
    },
    "SWI": {
        divId: "idSWIDiv", anIndicatorType: "SWI", ControlId: "idSWIIndicator", options: {  }
    },
    "UEF_P": {
        divId: "idUEFDiv", anIndicatorType: "UEF_P", ControlId: "idUEFIndicator",
        options: {
            meter: {
                style: 'rgr_meter'
            }
        }
    }
}


document.writeln("<!--  WHAT THE? -->");

function initializeIndicators(indicatorTypes) {
    for (var index = 0; index < indicatorTypes.length; index++) {
        var params = indicatorParameters[indicatorTypes[index]];
        IndicatorControlsArray.push(new IndicatorControl("indR" + index, params.anIndicatorType, params.ControlId, params.options));
    }
}


function SetIndicatorValues(arrayOfValues) {
    for (var i = 0; i < arrayOfValues.length; i++) {
        if (i < IndicatorControlsArray.length) {
            IndicatorControlsArray[i].SetValue(arrayOfValues[i]);
        }
    }

}


