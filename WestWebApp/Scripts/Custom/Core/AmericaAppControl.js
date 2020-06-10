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

function OhDearRun2016() {
    var tempjsonstr = getJSONData('empty');
    var newJsonStr = "";
    // find stopyr
    var stopyearIndex = tempjsonstr.indexOf("STOPYR");
    if (stopyearIndex > -1) {
        // found it!
        // ok move to the VAL
        ValIndex = stopyearIndex + 11;
        testVal = tempjsonstr.substring(ValIndex, ValIndex + 3);
        if (testVal == "VAL") {
            // so far so good
            // get the year value
            testyrstr = tempjsonstr.substring(ValIndex + 8, ValIndex + 12);
            testyr = Number(testyrstr);
            if ((testyr > 2016) && (testyr < 2066)) {
                // OK good to go
                newJsonStr = tempjsonstr.replace(testyrstr, "2017");
            }
        }
    } else {
        // did not find it, assume it is not specified, add it
        // find start of inputs
        var inputsindex = tempjsonstr.indexOf("Inputs");
        if (inputsindex > -1) {
            // move over to first object in input array
            var baseLen = inputsindex + 10;
            // garb the first part of imputs
            var BaseStr = tempjsonstr.substring(0, baseLen);
            // grab the last part
            var restOfStr = tempjsonstr.substring(baseLen, tempjsonstr.length);
            // now insert the stop year
            // now insert the stop year
            newJsonStr = BaseStr + "{\\\"FLD\\\":\\\"STOPYR\\\",\\\"VAL\\\":2017}," + restOfStr;
        }
    }
    // Reset Drought Flag
    DroughtFlag = false;
    // Reset Modals
    var CurrentModal = 0;
    // call the web service
      config = {};
    config.wizardName = ModalWizards[CurrentModal];
    Sideshow.start(config);
    if (newJsonStr != "") {
        callWebService(newJsonStr);
    // start model
  
    }
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

var Button2016Ctrl = $("#id2016Button");
if (Button2016Ctrl) {
    $(Button2016Ctrl).click(function () {
        OhDearRun2016();
    });
}

var SelectDivCtrl = $("#idSetDiv");
if (SelectDivCtrl) {
    $(SelectDivCtrl).change(function () {
        Index = this.value;
        hideMarquee();
        showMarquee(Index);
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
//hideMarquee();
$(document).ready(function () {
    marquee();
    var timeout = 1;
    function marquee() {
        var margin = -($('.marquee-e > div:eq(0)').outerHeight() + 20);
        $('.marquee-e > div:eq(0)').animate({ 'margin-top': margin + 'px' }, 4000, function () {
            $('.marquee-e > div:eq(2)').html($(this).html());
            $(this).html('').css({ 'margin-top': '0px' }).appendTo($('.marquee-e'));
            loop();
        });
    }
    function loop() {
        if (timeout)
            setTimeout(marquee, 0);
    }
    $('.marquee-e').hover(function () {
        timeout = 0;
        $('.marquee-e > div:eq(0)').stop(true);
    }, function () {
        timeout = 1;
        marquee();
    });
});
(function ($) {

    var methods = {
        init: function (options) {
            this.children(':first').stop();
            this.marquee('play');
        },
        play: function () {
            var marquee = this,
                pixelsPerSecond = 100,
                firstChild = this.children(':first'),
                totalHeight = 0,
                difference,
                duration;

            // Find the total height of the children by adding each child's height:
            this.children().each(function (index, element) {
                totalHeight += $(element).innerHeight();
            });

            // The distance the divs have to travel to reach -1 * totalHeight:
            difference = totalHeight + parseInt(firstChild.css('margin-top'), 10);

            // The duration of the animation needed to get the correct speed:
            duration = (difference / pixelsPerSecond) * 1000;

            // Animate the first child's margin-top to -1 * totalHeight:
            firstChild.animate(
                { 'margin-top': -1 * totalHeight },
                duration,
                'linear',
                function () {
                    // Move the first child back down (below the container):
                    firstChild.css('margin-top', marquee.innerHeight());
                    // Restart whole process... :)
                    marquee.marquee('play');
                }
            );
        },
        pause: function () {
            this.children(':first').stop();
        }
    };

    $.fn.marquee = function (method) {

        // Method calling logic
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.marquee');
        }

    };

})(jQuery);

var marquee = $('.marquee-e');

//marquee.marquee();

marquee.hover(function () {
    marquee.marquee('pause');
}, function () {
    marquee.marquee('play');
});