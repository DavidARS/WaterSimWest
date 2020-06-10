// Routines to Set sliders in new data load

// ADDED QUAY 6/13/14
// Set the sliders lable div with a value
function SetSliderValue(controlId, value) {
    // if the id is split by more than 1 "_" then it strips off the last item marked by "_"
    var target = GetRootId(controlId);
    target = '#' + target + 'lblSliderVal';
    $(target).html(value);

    //console.log('Highlight:', controlId, GetRootId(controlId));
    // console.log('Should use exact id later instead of find if possible to determine the id.');
    $('#' + controlId).find("div[id*=divslider]").attr("data-def", value);
}
// get the root control id by slicing off the last part marked by a '_'
//--------------------------------------
function GetRootId(ControlID)
{
    var id = ControlID.split('_');
    var idlen = id.length - 1;
    var target = "";
    for (var i = 0; i < idlen; i++) {
        target = target + id[i] + "_";
    }
    return target;
}
//=====================

function setSlider() {
    
    //function setInputControl(def, min, max) {
    $('.InputControl').each(function () {

        var divid = $(this).find("div[id*=divslider]").attr("id");
        var min = parseInt($(this).find("div[id*=divslider]").attr("data-min"));
        var max = parseInt($(this).find("div[id*=divslider]").attr("data-max"));
        var def = parseInt($(this).find("div[id*=divslider]").attr("data-def"));
        //console.log(divid, min, max, def);
     
        // DAS 03.18.14 based on Venu's suggestion

         if (max <  101) {
            var scaleVal = (max - min) / 4;
            $(this).find("span[id*=lblScalept1]").html(min);
            $(this).find("span[id*=lblScalept2]").html(min + scaleVal);
            $(this).find("span[id*=lblScalept3]").html(min + 2 * scaleVal);
            $(this).find("span[id*=lblScalept4]").html(min + 3 * scaleVal);
            $(this).find("span[id*=lblScalept5]").html(max);
        }
         else  if (max <  200) {
             var scaleVal = (max - min) / 6;
             $(this).find("span[id*=lblScalept1]").html(min);
             $(this).find("span[id*=lblScalept2]").html(min + scaleVal);
             $(this).find("span[id*=lblScalept3]").html(min + 2 * scaleVal);
             $(this).find("span[id*=lblScalept4]").html(min + 3 * scaleVal);
             $(this).find("span[id*=lblScalept5]").html(min + 4 * scaleVal);
             $(this).find("span[id*=lblScalept6]").html(min + 5 * scaleVal);
             $(this).find("span[id*=lblScalept7]").html(max);
         } 
         else
             {
             var scaleVal = (max - min) / 4;
             $(this).find("span[id*=lblScalept1]").html(min);
             $(this).find("span[id*=lblScalept2]").html(min + scaleVal);
             $(this).find("span[id*=lblScalept3]").html(min + 2 * scaleVal);
             $(this).find("span[id*=lblScalept4]").html(min + 3 * scaleVal);
             $(this).find("span[id*=lblScalept5]").html(max);

             }
         
       
         var TargetID = "POPInputUserControl";
         var BaseID = "_ControlContainer";
         SetScale(TargetID, BaseID);
                        
         TargetID = "GraphControls_InputUserControl1";
         BaseID = "_ControlContainer";
         SetScale(TargetID, BaseID);

         TargetID = "GraphControls_InputUserControl2";
         BaseID = "_ControlContainer";
         SetScale(TargetID, BaseID);
        //
        // NOTE : EVEN THOUGH THE SCALE HAS BEEN CHANGED
        // the values being passed have not been corrected
        // 02.17.15 DAS
        // ------------------------------------------------------------------------------------------------------------
         TargetID = "ENVIInputUserControl";
         BaseID = "_ControlContainer";
         SetEnvScale(TargetID, BaseID);

        // ===========================================================
        
        //Setting slider(Input Control) inputs
        $("#" + divid).slider({
            range: "min",
            value: def,
            min: min,
            max: max,
            slide: function (event, ui) {
                // Modified QUAY 6/13/14 to account for input controls on the default output tabs
                var thisid = $(this).attr("id");
                SetSliderValue(thisid, ui.value);                
   
                // Quay Modified ===================
            },
            // QUAY EDIT BEGIN 3/14/14
            stop: function (event, ui) {
                SetRunButtonState(true)
            }
            // QUAY EDIT END 3/14/14

        });
        // QUAY EDIT 6/12/14 
        var svalue = $("#" + divid).slider("value");
        SetSliderValue(divid, svalue);
        //       $('#' + id[0] + '_lblSliderVal').html($("#" + divid).slider("value"));
        //==================================

        // STEPTOE EDIT 06/27/17 update controls with run values
        if (inputControlsConverted != "undefined" && inputControlsConverted) {
            updateControlGroup(this, svalue);
        }
    });


    $("#point-in-time-slider").slider({
        range: "min",
        value: parseInt($("#point-in-time").html()),
        min: 2010,
        max: 2050,
        slide: function (event, ui) {
            $("#point-in-time").html(ui.value);
        }
    });
    $("#point-in-time").html($("#point-in-time-slider").slider("value"));

    $("#range-in-time-slider").slider({
        range: true,
        min: 2000,
        max: 2050,
        values: [parseInt($("#range-in-time-slider").attr("data-strtyr")), parseInt($("#range-in-time-slider").attr("data-endyr"))],
        slide: function (event, ui) {
            $("#range-in-time").html(ui.values[1] - ui.values[0]);
            // QUAY EDIT 3/13/14 begin
             $("#range-in-time-slider").attr("data-strtyr", ui.values[0]);
             $("#range-in-time-slider").attr("data-endyr", ui.values[1]);
            // QUAY EDIT 3/13/14 end;
        },
        // QUAY EDIT 3/13/14 BEGIN
        stop: function (event, ui) {
            //$("#range-in-time-slider").attr("data-strtyr", ui.values[0]);
            //$("#range-in-time-slider").attr("data-endyr", ui.values[1]);
            //ui.value = ui.values[1];
        },
        change: function () {
            //Time_Chart_Draw();
            // QUAY EDIT 3/13/14 End
        }
    });
    $("#range-in-time").html($("#range-in-time-slider").slider("values", 1) - $("#range-in-time-slider").slider("values", 0) + " years");
}

function SetScale(targetid, Baseid) {
    //var targetid = "POPInputUserControl";
    //var Baseid = "_ControlContainer";
    var ControlDiv = document.getElementById(targetid + Baseid);
    if (ControlDiv != undefined) {
        var ScaleDivs = $(ControlDiv).find(".scale");
        $(ScaleDivs).each(function () {
            oldhtml = this.innerHTML;
            this.innerHTML =
                "<span id='" + targetid + "_lblScalept1' style='left: 0%'>0</span>" +
                " <span id='" + targetid + "_lblScalept2' style='left: 16%'>25</span>" +
                " <span id='" + targetid + "_lblScalept3' style='left: 32%'>50</span>" +
                " <span id='" + targetid + "_lblScalept4' style='left: 48%'>75</span>" +
                " <span id='" + targetid + "_lblScalept5' style='left: 65%'>100</span>" +
                "<span id='" + targetid + "_lblScalept6' style='left: 82%'>125</span>" +
                "<span id='" + targetid + "_lblScalept7' style='left: 100%'>150</span>";
        });
    }
}
function SetEnvScale(targetid, Baseid) {
    //var targetid = "AGRIInputUserControl";
    //var Baseid = "_ControlContainer";
    //$("AGRIInputUserControl"+"_divslider").slider({min:50, max:150});
    var sliderid = "_divslider";
    var ControlDiv = document.getElementById(targetid + Baseid);
    if (ControlDiv != undefined) {
        var ScaleDivs = $(ControlDiv).find(".scale");
        $(ScaleDivs).each(function () {
            oldhtml = this.innerHTML;
            this.innerHTML =
                "<span id='" + targetid + "_lblScalept1' style='left:  0%'>0</span>" +
                 "<span id='" + targetid + "_lblScalept2' style='left: 25%'>39522</span>" +
                " <span id='" + targetid + "_lblScalept3' style='left: 50%'>79044</span>" +
                " <span id='" + targetid + "_lblScalept4' style='left: 75%'>118566</span>" +
                " <span id='" + targetid + "_lblScalept5' style='left: 100%'>158088</span>";

        });
        //$(targetid + sliderid).slider({min:50, max:150});
    }
}