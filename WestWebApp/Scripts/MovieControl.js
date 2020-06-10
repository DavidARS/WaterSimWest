var IntroVideoID = "idVidFrame";
var IntroVideoDiv = "idIntroVidDiv";
var TutorVideoID = "idVidFrame2";
var TutorVideoDiv = "idTutorVidDiv";
var MainID = "idMain";
var StartID = "idStartButton";
var StartScrnId = "idStartScreen";
var StartHref = "isAnchorButton";
var WSAWindow = null;
var ScrnWidth = 0;
var ScrnHeight = 0;
var MaxWidth = 0;
var MaxHeight = 0;
var ProWidth = 2732;
var ProHeight = 2048;
var ProRatio = ProWidth / ProHeight;
function OnStartClick() {
    $("#" + MainID).hide();
    $("#" + IntroVideoDiv).show();
    $("#" + StartID).hide();
    $("#" + StartHref).hide();

    var VidCtrl = document.getElementById(IntroVideoID);
    if (VidCtrl.play) {
        VidCtrl.play();
    }
}

function TutorVideoEnded() {
    $("#" + TutorVideoDiv).hide();
    $("#" + MainID).show();
    $("#" + StartHref).show();
    window.blur();
    // does not work on Safari
    WSAWindow = window.open("http://watersimamerica6.quayapps.com/Ipad", "", ""); //toolbar=no,scrollbars=yes,menubar=no


}
function IntroVideoEnded() {

    var TutorVidDiv = document.getElementById(TutorVideoDiv);
    $("#" + IntroVideoDiv).hide();
    $("#" + TutorVideoDiv).show();
    var VidCtrl = document.getElementById(TutorVideoID);
    if (VidCtrl.play) {
        VidCtrl.play();
    }


}
function AllIsLoaded() {

    window.focus();
    ScrnWidth = window.screen.width;
    ScrnHeight = window.screen.height;
    if ((ScrnWidth < (ProWidth - 5)) || (ScrnHeight < (ProHeight - 5))) {
        MaxWidth = ScrnWidth - 10;
        MaxHeight = ScrnHeight - 10;
    }
    else {
        MaxWidth = ProWidth - 10;
        MaxHeight = ProHeight - 10;
    }

    var TestRatio = MaxWidth / MaxHeight;
    if (TestRatio < ProRatio) {
        MaxHeight = MaxWidth / ProRatio;
    }
    else {
        MaxWidth = MaxHeight * ProRatio;
    }
    //alert(["ScrnW=" + ScrnWidth.toString() + "  ScrnH=" + ScrnHeight.toString()]);
    window.moveTo(0, 0);
    window.resizeTo(MaxWidth, MaxHeight);
    var StartImg = document.getElementById(StartScrnId);
    if (StartImg) {
        StartImg.width = (MaxWidth).toString();
        StartImg.height = (MaxHeight*.9).toString();
    }
    var StartButton = document.getElementById(StartID);
    if (StartButton) {
        var left = (MaxWidth / 3)+40;
        StartButton.style.left = left.toString() + "px";
        var top = ((MaxHeight / 5) * 2) - 40;
        StartButton.style.top = top.toString() + "px";

    }
    var StartHref = document.getElementById(StartHref);
    if (StartHref) {
        var left = (MaxWidth / 3) + 40;
        StartHref.style.left = left.toString() + "px";
        var top = ((MaxHeight / 5) * 2) - 40;
        StartHref.style.top = top.toString() + "px";

    }


}

function DocReady() {
    var VidCtrl = document.getElementById(IntroVideoID);
    VidCtrl.onended = function () { IntroVideoEnded() };
    VidCtrl = document.getElementById(TutorVideoID);
    VidCtrl.onended = function () { TutorVideoEnded() };

    $("#" + IntroVideoDiv).hide();
    $("#" + TutorVideoDiv).hide();
    var ST = document.getElementById(StartID);
    $(ST).click(function () { OnStartClick(); });
}

$(document).ready(function () { DocReady(); })
window.onload = function () { AllIsLoaded() };


