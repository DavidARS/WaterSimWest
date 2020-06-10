function drawMySankey(AfluxList, $jsonObj, controlID) {
    var divID = $("#" + controlID).find("div[id*=ChartContainer]").attr('id');

    //Strip standard chart style from Sankey div
    // $("#" + divID).attr("style", "position:relative; top:25px;");
    $("#" + divID).attr("style", "");

    //console.log('drawMySankey:', controlID, divID);

    // get the parent height if it is their
    var defaultHeight = 480;
    var defaultWidth = 450;
    var thetargetdiv = $('#' + controlID);
    if (thetargetdiv.parent().length) {
        var height = thetargetdiv.parent().height();
        var width = thetargetdiv.parent().width();
        if (height) {
            defaultHeight = height;
        }
        if (width) {
            defaultWidth = width;
        }
    }

    //console.log('drawMySankey:', defaultHeight, defaultWidth);

    var SankeyOptions = {
        width: defaultWidth, //SVG width
        //width: 500, //SVG width
        svgHeight: defaultHeight,//SVG height
        linkColorScheme: 0, //0 (Source), 1 (Target), 2 (Gradient)
        units: "MGD", //units displayed with values
        nodeWidth: 50, //width of rects
        imgWidth: 60,// QUAY EDIT 4/5/16  80, //width of image
        imgHeight: 60, // QUAY EDIT 4/5/16 80, //height of image
        nodePadding: 20, // QUAY EDIT 4/5/16 80, 50, //vertical space between nodes
        // QUAY EDIT 4/5/16 END
        autoScaleImgHeight: false, //scale image based on rect height
        showText: true, //show text label beside Resource/Consumer
        imgPath: "/Images/Sankey/White/", //User defined path for Resource/Consumer images
        titlefontsize: "23px",
        titleOffset: 20,
        bucketfontsize: "16px"
    };

    //console.log($jsonObj, controlID, divID);
    CreateSankey(AfluxList, $jsonObj, "#" + divID, SankeyOptions);
}
function CreateSankey(TheFluxList, TheModelOutput, divID, options){

    //var SandKeyCreated;
    if (typeof (mySankey) == "undefined") {
        //Create a new Sankey
        mySankey = new Sankey(TheFluxList, TheModelOutput, divID, options);
        //drawSandkey(TheFluxList, TheModelOutput, divID, options);
    }
    else {
        //Update a current Sankey with new data
        mySankey.update(TheFluxList, TheModelOutput);
    }
}