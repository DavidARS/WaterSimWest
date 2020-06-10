
//-- QUAY EDIT FOR NEW RUN BUTTON 12 18 14 ------------------------->           

//function refreshBackgrounds(selector) {
//    // Chrome shim to fix http://groups.google.com/a/chromium.org/group/chromium-bugs/browse_thread/thread/1b6a86d6d4cb8b04/739e937fa945a921
//    // Remove this once Chrome fixes its bug.
//    if (/chrome/.test(navigator.userAgent.toLowerCase())) {
//        $(selector).each(function() {
//            var $this = $(this);
//            if ($this.css("background-image")) {
//                var oldBackgroundImage = $this.css("background-image");
//                var oldBackground = $this.css("background");
//                setTimeout(function() {
//                    $this.css("background-image", oldBackgroundImage);
//                    $this.css("background", oldBackground);
//                }, 1);
//            }
//        });
//    }
//}
 


var ModelRunningImage = new Image(180, 30);
ModelRunningImageSrc = "Images/Model Running.png";
ModelRunningImage.src = ModelRunningImageSrc;
var ModelWaitingImage = new Image(180, 30);
ModelWaitingImageSrc = "Images/RunModelGrey.png";
ModelWaitingImage.src = ModelWaitingImageSrc;
var ModelReadyImage = new Image(180, 30);
ModelReadyImageSrc = "Images/RunModelGreenGold.png";
ModelReadyImage.src = ModelReadyImageSrc;

var SliderDirty = false;
var OldRunButtonImage;

//var Greens = new Array();
////Greens[0] = "#00AF00";
////Greens[1] = "#30FF1E";
////Greens[2] = "#60FF2E";
////Greens[3] = "#9AFE2E";
////Greens[4] = "#60FF2E";
////Greens[5] = "#30FF1E";
////Greens[6] = "#00AF00";

//Greens[0] = 0x00AF00;
//Greens[1] = 0x30FF1E;
//Greens[2] = 0x60FF2E;
//Greens[3] = 0x9AFE2E;
//Greens[4] = 0x60FF2E;
//Greens[5] = 0x30FF1E;
//Greens[6] = 0x00AF00;

//var isDoBlink = false;
//var BlinkIndex = 0;
//var BlinkPause = 40;
//var BlinkCnt = 30;
//var BlinkTimer;

//function BlinkRun() {
//    if (BlinkCnt < BlinkPause) {
//        BlinkCnt++;
//    } else {
//        var test = $("*.run-model");
//        test[0].style.backgroundColor = Greens[BlinkIndex];
//        test[1].style.backgroundColor = Greens[BlinkIndex];
//        //// for chrome and firefox
//        test[0].style.background = Greens[BlinkIndex];
//        test[1].style.background = Greens[BlinkIndex];
//        BlinkIndex++;
//        if (BlinkIndex == Greens.length) { BlinkCnt = 0; BlinkIndex = 0; }
 
//    }
//}
function SetRunButtonWaiting() {
    $("*.run-model").each(function () {
  //      $(this).find('img').attr('src', ModelReadyImageSrc);

        this.src = ModelReadyImageSrc;
        //var mss = this.style;
        //this.style.backgroundColor = Greens[0];
        //this.style.backgroundImage = "none !important";
        ////// for chrome and firefox
        //this.style['background-color'] = Greens[0];
        //if (isDoBlink) {
        //    BlinkTimer = setInterval(function () { BlinkRun(); }, 50);
        //}
        //var spinner = document.getElementById('spinnerImg').style.display = "";

     });

  //   refreshBackgrounds("*.run-model");
}


function SetRunButtonDone() {
    //if (isDoBlink) {
    //    clearInterval(BlinkTimer);
    //}
    $("*.run-model").each(function () {

        this.src = ModelWaitingImageSrc;

        //this.style.backgroundImage = "";
        //this.style.backgroundColor = 0x990000;
        //this.style.background = 0x990000;
    });
    //var spinner = document.getElementById('spinnerImg').style.display = "none";
}

function SetRunButtonRunning() {
    $("*.run-model").each(function () {
        $(this).find('img').attr('src', ModelRunningImageSrc);
        
                this.src = ModelRunningImageSrc;

    });
}

//=============================================================================
function SetRunButtonState(isSliderDirty) {
    // see if isSLiderDirty is undefined, if so then reset to done state
    if (isSliderDirty == undefined)
    { SetRunButtonDone(); SliderDirty = false; }
    else
    {

    if (isSliderDirty) {
        if (!SliderDirty) { SetRunButtonWaiting(); SliderDirty = true; }
        // else do nothing, already dirty
    } else 
            SetRunButtonRunning(); SliderDirty = false;
            // already off do nothing
    }
}
   

var TraceDirty = false;


//function SetCOButtonWaiting() {
//    var test = $("*.trace-CO");
//    //    var mss = test[0].style;
//    test[0].style.backgroundColor = Greens[0];
//    test[0].style.backgroundImage = " none !important";
//    test[1].style.backgroundColor = Greens[0];
//    test[1].style.backgroundImage = " none !important";
//    if (isDoBlink) {
//        BlinkTimer = setInterval(function () { BlinkCO(); }, 50);
//    }

//}
//function SetCOButtonDone() {
//    if (isDoBlink) {
//        clearInterval(BlinkTimer);
//    }
//    var test = $("*.trace-CO");

//    test[0].style.backgroundImage = "";
//    test[0].style.backgroundColor = "";
//    test[1].style.backgroundImage = "";
//    test[1].style.backgroundColor = "";
//}
////=============================================================================
//function SetCOButtonState(isTraceDirty) {
//    // see if isSLiderDirty is undefined, if so then reset to done state
//    if (isTraceDirty == undefined) { SetCOButtonDone(); TraceDirty = false; }
//    if (isTraceDirty) {
//        if (!TraceDirty) { SetCOButtonWaiting(); TraceDirty = true; }
//        // else do nothing, already dirty
//    } else
//        if (TraceDirty) {
//            SetCOButtonDone(); TraceDirty = false;
//            // already off do nothing
//        }
//}
