function speechBubbleFadeOut(time) {
    $("#speechBubble").fadeOut(time, function () {
        // Animation complete.
    });
}

//$("#cactusjack").hover(function () {
//    $("#speechBubble").html(
//        '<p style="font-size: 50px;">HELLO!</p>' + 
//        '<p style="font-size: 30px;">Welcome to WaterSim Arizona.</p>' + 
//        '<p style="font-size: 18px; margin-bottom: 18px;">' + 
//            '<strong>YOU</strong> are now managing the water supplies and water use for your community!' + 
//            'Acting as a water manager, manipulate the management polices to achieve long-term water sustainability for your region.' + 
//            '<br />' + 
//            '<strong>REMEMBER:</strong> there are no “right” answers to this puzzle; we all have our own perspectives as a sustainable water future. Good luck!' + 
//        '</p>'
//    )
//    $("#speechBubble").fadeIn(500);
//}, function () {
//    $("#speechBubble").fadeOut(100);
//});

var HelpInfo = {};
$.get("./Content/HELPFILES/help.csv").then(function (data) {
    var dataObjects = $.csv.toObjects(data);
    for (var i = 0; i < dataObjects.length; i++) {
        var info = dataObjects[i]
        HelpInfo[info.field] = info;
    }
});

var Ass = {};
$.get("./Content/HELPFILES/RunAssessment.csv").then(function (data) {
    var dataObjects = $.csv.toObjects(data);
    for (var i = 0; i < dataObjects.length; i++) {
        var info = dataObjects[i]
        Ass[info.field] = info;
    }
});

var tempIndicators = {
    SFYG_P: 10,
    ENVIND_P: 45,
    ECOGPCD_P: 90,
    AGNET_P: 35
}
function runAssessment() {
    var htmlText = "";
    //Loop through indicators
    $.each(tempIndicators, function (field, value) {
        var ass = Ass[field];
        if (value <= ass.twentyFifth) {
            console.log(ass.ClassC)
            htmlText += ass.ClassC;
        }
        else if (value > ass.twentyFifth && value <= ass.seventyFifth) {
            console.log(ass.ClassB)
            htmlText += ass.ClassB;
        }
        else {
            console.log(ass.ClassA)
            htmlText += ass.ClassA;
        }

        htmlText += "<br> <br>";
    });


    //$("#speechBubble").html(htmlText);
    //$("#speechBubble").show();

    //clearTimeout(hoverTimer);
    //hoverTimer = setTimeout(function () {
    //    $("#speechBubble").fadeOut(5000);
    //}, 1000);

}