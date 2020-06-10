// WaterSim America Control ScripsT
// this script brings in ontrols for the behavior of the app
// Some controls are for debugging
// Some controls affect the behavior of the game
// QUAY 3/19/16
var DebugMode = false;

function ChangeState(value) {
    if (ourState) {
        ourState = value;
        setStateInformation();
        callWebService(getJSONData('empty'));
    }
}

function ChangeImages() {
    displayIndicators(ourState);
//        callWebService(getJSONData('empty'));
}

function OhDearRun2015() {
    var tempjsonstr = getJSONData('empty');
    DoPostProcess = true;
    callWebService(tempjsonstr);
    //}
}

var SelectStateCtrl = $("#idSelectState");
if (SelectStateCtrl) {
    $(SelectStateCtrl).change(function () {
        statevalue = this.value;
        ChangeState(statevalue);
    });
}

var SelectImageCtrl = $("#idSetIndImage");
if (SelectImageCtrl) {
    $(SelectImageCtrl).change(function () {
        IndImageIndex = this.value;
        ChangeImages();
    });
}

var Button2015Ctrl = $("#id2015Button");
if (Button2015Ctrl) {
    $(Button2015Ctrl).click(function () {
        OhDearRun2015();
    });
}


var DoPreProcess = false;
function PreProcessWebCall(thejsonData) {
    if (DoPreProcess) {
    }
}

var TimesThrough = 0;
var DoPostProcess = false;
function PostProcessWebCall(Return_Data) {
    if (DoPostProcess) {
        if (TimesThrough < 1) {
            if (WS_RETURN_DATA.RESULTS) {
                $(WS_RETURN_DATA.RESULTS).each(function () {
                    if (this.VALS) {
                        tempVal = this.VALS[0];
                        this.VALS = new Array();
                        this.VALS[0] = tempVal;
                    }

                });
                if (WS_RETURN_DATA.MODELSTATUS)
                    WS_RETURN_DATA.MODELSTATUS.ENDYEAR = WS_RETURN_DATA.MODELSTATUS.STARTYEAR;
            }

            $('#dashboard-header-h0').text("Your State");
            DoPostProcess = false;
            //TimesThrough++;

        }
        //else {
        //    $('#dashboard-header-h0').text("Your State");
        //    DoPostProcess = false;
        //    TimesThrough = 0;
        //}
    }
}
var SelectDivCtrl = $("#idSetDiv");
if (SelectDivCtrl) {
    $(SelectDivCtrl).change(function () {
        Index = this.value;
        SetSanKeyColors(Index);

    });
}
function showMarquee(index) {
    var value = Number(index);
    switch (value) {

        case 0:
            hideMarquee();
            break;
        case 1:
            $('.marquee-a').show();
            break;
        case 2:
            $('.marquee-b').show();
            break;
        case 3:
            $('.marquee-c').show();
            break;
        case 4:
            $('.marquee-d').show();
            break;
        case 5:
            //$('.marquee-e').show();
            //marquee.marquee();

            break;

        default:
            $('.marquee-c').show();
    }


}

function hideMarquee() {
    $("[class^='marquee']").each(function () {
        $(this).hide();
    });
}


function SetSanKeyColors(index) {
    var value = Number(index);
    switch (value) {
        case 0:
            colorBrewer = {
                "SUR": '#8dd3c7',
                "SURL": '#ffffb3',
                "GW": '#bebada',
                "REC": '#80b1d3',
                "SAL": '#fdb462',
                "UD": '#Eb8383', // apa 8b2323 QUAY EDIT 4/11/16 '#b3de69',
                "ID": '#C590FB',  //apa 55008b QUAY EDIT 4/11/16'#fccde5',
                "AD": '#52BB52', // apa 228b22 QUAY EDIT 4/11/16 '#d9d9d9',
                "PD": '#B5B5B5' // apa 858585 QUAY EDIT 4/11/16'#bc80bd',
            };
            break;
        case 1:
            colorBrewer = {
                "SUR": '#8dd3c7',
                "SURL": '#ffffb3',
                "GW": '#bebada',
                "REC": '#80b1d3',
                "SAL": '#fdb462',
                "UD": '#00DD00', 
                "ID": '#00DD00', 
                "AD": '#00DD00', 
                "PD": '#00DD00' 
            };
            break;
        case 2:
            colorBrewer = {
                "SUR": '#6797d0',
                "SURL": '#f47521',
                "GW": '#fcb040',
                "REC": '#7295b1',
                "SAL": '#d9bd9e',
                "UD": '#00AA00', 
                "ID": '#00AA00', 
                "AD": '#00AA00', 
                "PD": '#00AA00' 
            };
            break;
        case 3:
            colorBrewer = {
                "SUR": '#62b7e6',
                "SURL": '#8ecad8',
                "GW": '#7489a5',
                "REC": '#96ae97',
                "SAL": '#92cd8b',
                "UD": '#00FF00', 
                "ID": '#00FF00', 
                "AD": '#00FF00', 
                "PD": '#00FF00'
            };
            break;
        case 4:
            colorBrewer = {
                "SUR": '#9a9bcd',
                "SURL": '#6797d0',
                "GW": '#99dbf8',
                "REC": '#9260a9',
                "SAL": '#25aae1',
                "UD": '#60FF60',
                "ID": '#60FF60',
                "AD": '#60FF60',
                "PD": '#60FF60'
            };
            break;
        case 5:
            colorBrewer = {
                "SUR": '#a1dab4',
                "SURL": '#ffffcc',
                "GW": '#41b6c4',
                "REC": '#225ea8',
                "SAL": '#ffffcc',
                "UD": '#10AA10',
                "ID": '#10AA10',
                "AD": '#10AA10',
                "PD": '#10AA10'
            };
            break;
        case 6:
            colorBrewer = {
                "SUR": '#6797d0',
                "SURL": '#f47521',
                "GW": '#fcb040',
                "REC": '#d9bd9e',
                "SAL": '#7295b1',
                "UD": '#00CC00',
                "ID": '#00CC00',
                "AD": '#00CC00',
                "PD": '#00CC00'
            };
            break;
        case 7:
            colorBrewer = {
                "SUR": '#62b7e6',
                "SURL": '#8ecad8',
                "GW": '#96ae97',
                "REC": '#7489a5',
                "SAL": '#92cd8b',
                "UD": '#00FF00',
                "ID": '#00FF00',
                "AD": '#00FF00',
                "PD": '#00FF00'
            };
            break;
        case 8:
            colorBrewer = {
                "SUR": '#9a9bcd',
                "SURL": '#6797d0',
                "GW": '#fcb040',
                "REC": '#9260a9',
                "SAL": '#25aae1',
                "UD": '#60FF60',
                "ID": '#60FF60',
                "AD": '#60FF60',
                "PD": '#60FF60'
            };
            break;

    }
    callWebService(getJSONData('empty'));

}


