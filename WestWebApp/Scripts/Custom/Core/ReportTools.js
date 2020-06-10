/* *************************************
   *  Print WaterSim Report
   *  Author: Quay
   *  Date: 6/20/14
   *
   *  Functions to print a report of inputs, indicators, and charts
   *  Dependencies
   *    Google canvg.js  http://code.google.com/p/canvg/
   *    rgbcolor.js   http://www.phpied.com/rgb-color-parser-in-javascript/
   *
   *******************************************/
// Class used to identify which charts to print
function UseChartClass(divid, useit, title, tab) {
    this.ID = divid;
    this.USE = useit;
    this.TITLE = title;
    this.TAB = tab;
    return this;
};

var TabNames = new Array();
var Tabhref = new Array();
var ChartSelect = new Array(); 
//--------------------------------------------------

function BuildSelectArray() {
    // Clear array
    ChartSelect.length = 0;
    ChartCnt = 0;
    for (var i = 0; i < TabNames.length; i++) {
        // if tabname defined
        if (TabNames[i]) {
            //console.log('BuildSelectArray: ' + TabNames[i]);
            // get this div
            var TabDiv = $("#" + Tabhref[i]);
            // ok no find all the frame class divs in this div
            //$(TabDiv).find(".frame").each(function () {
                // ok cycle through the output controls in this frame
                //$(this).find(".OutputControl").each(function () {
                $(TabDiv).find(".OutputControl").each(function () {
                    // get the control id
                    var controlID = $(this).attr('id');
                    var theTitle = $("#" + controlID).find("span[id*=_lblTitle]").html();
                    var TabName = TabNames[i];
                    var TempUseChart = new UseChartClass(controlID, true, theTitle, TabName);

                    //console.log('BuildSelectArray.OutputControl:', controlID, theTitle, TabName);

                    ChartSelect[ChartCnt] = TempUseChart;
                    ChartCnt++;
                    // get the container id
                });
            //});
        }

    }
}

//--------------------------------------------------

function BuildTabnames() {
    // Clear arrays
    TabNames.length = 0;
    Tabhref.length = 0;
    //$("#main-input-container").each(function () {
    //    // get all the anchor elements
    //    $(this).find("a").each(function () {
    //        var linkstr = $(this).attr("href");
    //        // wierd behavior, some elements returned do not have href, though a anchor tag matches
    //        if (linkstr) {
    //            var tabtitle = $(this).html();
    //            var linka = linkstr.split('-');
    //            var indexstr = linka[1];
    //            var index = Number(indexstr);
    //            Tabhref[index] = linkstr.slice(1);
    //            TabNames[index] = tabtitle;
    //        }
    //    });
    //});

    TabNames = ["Flow Chart", "Bar Charts", "Line Charts"];
    Tabhref = ["flowChart", "barCharts", "lineCharts"];
    //TabNames = ["Bar Charts", "Line Charts"];
    //Tabhref = ["barCharts", "lineCharts"];
};

//--------------------------------------------------

$("body").ready(function () {
    BuildTabnames();
    BuildSelectArray();
    // use the below to test select
    //ChartSelect[0].USE = false;
    //ChartSelect[1].USE = false;
    //-------------------
});


//--------------------------------------------------
// find chart by title and return if it should be printed
function PrintChart(charttitle) {
    var result = false;
    for (var i = 0; i < ChartSelect.length; i++) {
        if (ChartSelect[i].TITLE == charttitle) {
            result = ChartSelect[i].USE;
            break;
        }
    }
    return result;
}


var ProgressBarInc = 1;
var ChartDoneCnt = 0;
//===============================
// Creates the base browser window to copy report elements into

//var RTitle = "WHAT THE!";
//function GetReportTitle() {
//    return RTitle;
//}

//-------------------------------------------------------------

function TestIfWinReady(theWin) {
    return theWin.ReportReady
}

//------------------------------------------------------------------
function DelaySetTitle(thewin, Title ) {
    var Cnt = 0;
    var MaxCnt = 20;
    var result = false;
    while ((Cnt < MaxCnt) && (!result)) {

        if (thewin.document.getElementById) {
            var PTitle = thewin.document.getElementById("PageTitle");
            if (typeof (PTitle) != "undefined") {
                PTitle.innerHTML = Title;
                result = true;
            }
        }
        else {
                setTimeout(function () { result = TestIfWinReady(thewin); }, 300);
        }
        Cnt++;
    }
    return result;
}

function WaitForIndicators(thewin) {
    var Cnt = 0;
    var MaxCnt = 20;
    var result = false;
    while ((Cnt < MaxCnt) && (!result)) {
        if (thewin.initializeIndicators) {
            result = true;
        }
        else
            setTimeout(function () { }, 300);
        Cnt++;
    }
    return result;
}
//---------------------------------------------------------------------
function CreatePrintWindow(width, Title, WinName) {


   var awindow = window.open("Report.html", WinName, "menubar=1,toolbar=yes,scrollbars=yes,resizable=yes,top=10,left=10,width=" + width.toString() + ",height=800,status=1");

//var awindow = window.open("", WinName, "toolbar=yes, scrollbars=yes, resizable=yes, menubar=yes, top=10, left=10, width="+width.toString()+", height=800");

//var wintxt = "<!DOCTYPE html>\n<html xmlns='http://www.w3.org/1999/xhtml'>"
//    wintxt += "<head><Title>WaterSim Screnario Report</title></head>";
//    wintxt += "<body><h1>WaterSim Scenario Report :</h1><h3 id='PageTitle'></h3>\n";
//    wintxt += "   <style> \n";
//    wintxt += "      .container{\n"; 
//    wintxt += "         width: 300px; \n";
//    wintxt += "         border: 1px solid #ddd; \n";
//    wintxt += "         border-radius: 5px; \n";
//    wintxt += "         overflow: hidden;\n";
//    wintxt += "         display:inline-block;\n";
//    wintxt += "         margin:0px 10px 5px 5px;\n";
//    wintxt += "         vertical-align:top;\n";
//    wintxt += "     }\n";
//    wintxt += "       .progressbar {\n";
//    wintxt += "           color: #fff;\n";
//    wintxt += "           text-align: right;\n";
//    wintxt += "            height: 25px;\n";
//    wintxt += "           width: 0;\n";
//    wintxt += "           background-color: #0ba1b5;\n"; 
//    wintxt += "            border-radius: 3px; \n";
//    wintxt += "       }\n";
//    wintxt += "   </style>\n";
//    wintxt += "  <div id='waitdiv' style='float:left; position:fixed; z-index:1; width:500px; height:300px;  border: 1px solid black; opacity: 0.9;"
//    wintxt +=     "text-align:center; vertical-align:middle; background-color: #3030ff;'> <h1 style='color:white'>Please wait while report is created</h1>\n";
//    wintxt += "  <div class='container'><div id='mypb' class='progressbar'></div></div></div> <!-- id='waitdiv' -->\n";
//    wintxt += "  <div id='inputoutput'>\n";
//    wintxt += "     <h2>Simulation Inputs</h2>\n";

//    wintxt += "     <div id='inputs'>\n";
//    wintxt += "     </div> <!-- id='inputs' -->\n";

//    wintxt += "  </div> <!-- inputoutput -->\n";

//    wintxt += "  <div id='indicatoroutput' style='height:850px'>\n";
//    wintxt += "  <h2>Indicator Results</h2>\n";

//    wintxt += "  <div id='indicators'>\n";
//    wintxt += "  </div> <!-- indicators -->\n";

//    wintxt += "  </div> <!-- indicatoroutput -->\n";

//    wintxt += "  <div id='chartoutput'>\n";
//    wintxt += "     <h2>Simulation Outputs</h2>\n";
//    wintxt += "    <div id='supply'>\n";
//    wintxt += "    </div> <!-- id supply -->\n";
//    wintxt += "    <div id='demand'>\n";
//    wintxt += "    </div> <!-- id demand -->\n";
//    wintxt += "    <div id='reservoirs'>\n";
//    wintxt += "    </div> <!-- id reservoirs -->\n";
//    wintxt += "    <div id='sustain'>\n";
//    wintxt += "    </div> <!-- id sustain -->\n";
//    wintxt += "  </div> <!-- ChartOuput -->\n";

//    wintxt += "  <div id='statusoutput'>\n";
//    wintxt += "  </div> <!-- statusoutput -->\n";
//    wintxt += "  <script src='Scripts/jquery-2.1.0.js'></script>\n";
//    wintxt += "  <script src='Scripts/jquery-ui-1.10.4.js'></script>\n";
//    wintxt += "  <script src='Assets/indicators/Scripts/IndicatorControl_v_4.js'></script>\n";
//    wintxt += "  <script src='Assets/indicators/Scripts/reportindicator.js'></script>\n";
//    wintxt += "  <script type='text/javascript'>\n";
//    wintxt += "      var ReportReady = false;\n";
//    wintxt += "      function HideWait() {$('#waitdiv').hide();}\n";
 
//    wintxt += "      function setProgress(progress) {\n";
//    wintxt += "          var progressBarWidth = progress * $('.container').width() / 100;\n";
//    wintxt += "          $('.progressbar').width(progressBarWidth).html(progress + '% '); }\n";

//    wintxt += "      $(document).ready(function () {\n";
//    wintxt += "         ReportReady = true;\n";
//    wintxt += "       });\n";
//    wintxt += "  </script>\n";
//    wintxt += "  <div style='font-size:xx-small'>Final Written</div>\n";
//    wintxt += "</body>\n";
//    wintxt += "</html>"

//    awindow.document.writeln(wintxt);

    return awindow;
}

//==================================================================================
// Adds charts from an array of Highcharts to a div with the specified id, 
// the target width and height of each chart canvas can be defined or default to 250 by 250 

function AddCharts(targetWindow, aChartArray, divid, groupTitle, width, height) {
    var result = "";
    try{
        if (targetWindow.document.write) {
            if (aChartArray.length) {
                if (!width) { width = 250; }
                if (!height) { height = 250; }
                var ChartN = aChartArray.length;
                if (ChartN > 0) {
                    BuildChartTable(targetWindow, divid, ChartN, groupTitle, width, height);
                    result = AddChartArrayToTable(targetWindow, aChartArray, divid);
                }
            }
            else {
                result = "Not an Array";
            }
        } else {
            result = "Not a document";
        }
    }
    catch (err) {
        result = "Exception: " + err.message;
    }
    return result;
}

//===============================================
// Writes heml to create a chart section, includes a title, a table with each cell containing a
// a canvas with an id to match the section name and a chart number
// Width and height of chart canvas must be defined

function BuildChartTable(targetWindow, divId, chartNumber, groupTitle, width, height) {
    var outputDiv = targetWindow.document.getElementById(divId);
    var rowcnt = (Math.floor( chartNumber / 2)) + (chartNumber % 2);
    var groupHTML = "<h3>"+groupTitle+"</h3> <Table >";
    for (var i = 0; i < rowcnt; i++) {
        groupHTML += "<tr><td><canvas id='"+divId+"chart"+(i*2).toString() +
                      "' width='"+width.toString()+"' height='"+height.toString()+"' /><td>"+
                      "<td><canvas id='"+divId+"chart"+((i*2)+1).toString() +
                    "' width='"+width.toString()+"' height='"+height.toString()+"' /><td><tr>";
    }
    groupHTML += "</table>";
    outputDiv.innerHTML = groupHTML;
    
}

//==================================================
// Adds images from an array of highchart charts to a div that has been predefined using BuildChartTable

function AddChartArrayToTable(targetWindow, aChartArray, divid) {
    // now set graphics;
    var result = "";
    try {
        var PbVal = 0;
        for (var i = 0; i < aChartArray.length; i++) {
            //console.log('AddChartArrayToTable:', aChartArray[i]);
            if (aChartArray[i].getSVG()) {
                var TheCanvas = targetWindow.document.getElementById(divid+"chart" + i.toString());
                result = CopyChartImage(aChartArray[i], TheCanvas, targetWindow.document);
                if (targetWindow.setProgress) {
                    ChartDoneCnt++;
                    PbVal = ChartDoneCnt * ProgressBarInc;
                    targetWindow.setProgress(PbVal)
                }
            }
        }
    }
    catch (err) {
        result = "Exception: " + err.message;
    }
    return result;
}

//=======================================
// Copiesthe image of a highchart to a canvas (using SVG to extract image from Highhcart)
// uses Google canvg function

function CopyChartImage(aChart, aCanvas, targetDocument) {
    //console.log('CopyChartImage: ', aChart);
    error = "";
    try{
        if (aChart.getSVG) {
            if (aCanvas.getContext) {
                if (canvg) {
                    var ChartSVG = aChart.getSVG();
                    var TempCanvas = targetDocument.createElement('canvas');
                    targetWidth = aCanvas.width;
                    targetHeight = aCanvas.height;
                    TempCanvas.width = targetWidth;
                    TempCanvas.height = targetHeight;
                    canvg(TempCanvas, ChartSVG, { ignoreDimensions: true, scaleWidth: targetWidth, scaleHeight: targetHeight });
                    var SourceCTX = TempCanvas.getContext('2d');
                    var chartImage = SourceCTX.getImageData(0, 0,targetWidth,targetHeight);
                    var TargetCTX = aCanvas.getContext('2d');
                    TargetCTX.putImageData(chartImage, 0, 0 );//, 0, 0, targetWidth, targetHeight);
                }
                else {
                    error = "CANVG is not available";
                }
            }
            else {
                error = "Not a Canvas";
            }
        }
        else {
            error = "Not a Highchart";
        }
    }
    catch (err) {
        error = "Exception: " + err.message;
    }
    return error;
}

//=====================================================
function SetBodyCursor(awindow, value) {
    var OldCursor = "default";
    try {
        if (awindow.document.body) {
            var TempCursor = awindow.document.body.style.cursor;
            if (TempCursor !== "") { OldCursor = TempCursor; }
            awindow.document.body.style.cursor = value;
            }
    }
    finally
    {
        return OldCursor;
    }
}
//=================================================
var ReportCnt = 0;
var QuickTitle = "";
function CreateReport() {
    var result = "";
    try{
        // increment count
        ReportCnt++;
        // get date and time for title
        var today = new Date();
        var dateStr = today.toDateString();
        var timeStr = today.toTimeString();
        // set up window name and report tite
        var WinName = "Report"+ReportCnt.toString();
        var Title = "#" + ReportCnt.toString() + "  " + dateStr + "  " + timeStr;
        // create a new browser window with need divs
        // var ReportWindow = CreatePrintWindow(700, Title + ReportCnt.toString(), WinName);
        QuickTitle = Title;
        var ReportWindow = CreatePrintWindow(800, Title, WinName);
        //result = ReportEventHandler(ReportWindow, Title);
    }
    catch (err) {
        alert("Error Creating Report: " + err.message);
    }
    return result;
}

function CreatePrintableSVG(obj) {
    var svg = obj.html();
    return {
        getSVG: function () {
            return svg;
        }
    };
}

//=============================================================
function ReportEventHandler(ReportWindow,Title) {
    var TheTitle = QuickTitle;
    setTimeout(function () { BuildReport(ReportWindow, TheTitle); }, 500);
    return "Scheduled";
}
function BuildReport(ReportWindow, Title)
{
    ReportWindow.focus();
    var result = "";
    // setup a wait cursor
    var MyOldCursor = "default";
    MyOldCursor = SetBodyCursor(window, "wait");
    try {
        var WinReady = DelaySetTitle(ReportWindow, Title);
        if (WinReady) {

            var groupcnt = 0;

            SetBodyCursor(ReportWindow, "wait");

            //====================================================
            // Copy the Input Values
            //==================================================
            var inputOutput = "<table><tr><th><b>Input Description</b></th><th><b>Input Value</b><//th></tr>";
            inputOutput += "<tr><td><b>Policy</b></td><td></td></tr>"
            // get inputs from the inoout panel
            $("#PanelUserInputs").find('.InputControl').each(function () {
                var divid = $(this).find("div[id*=divslider]").attr("id");
                var svalue = $("#" + divid).slider("value");
                var label = $(this).find("span[id*=lblSliderfldName]").html();
                inputOutput += "<tr><td>&nbsp;&nbsp; " + label + "</td><td> = " + svalue.toString() + "</td></tr>";
            });

            ////////////////////////////////////////////////////////////
            // DO NOT NEED CLIMATE
            //==========================================================
            //// SPECIAL CODE FOR THE CLIMATE VARIABLES
            //inputOutput += "<tr><td><b>Climate</b></td><td></td></tr>"
            //// get inputs from the climate panel
            //$("#climateTab").find('.InputControl').each(function () {
            //    var divid = $(this).find("div[id*=divslider]").attr("id");
            //    var svalue = $("#" + divid).slider("value");
            //    var label = $(this).find("span[id*=lblSliderfldName]").html();
            //    inputOutput += "<tr><td>&nbsp;&nbsp; " + label + "</td><td> = " + svalue.toString() + "</td></tr>";
            //});

            //var flowState = GetCoFlow(CORiverFlowValue);
            //var flowLabel = GetFlowLabel(flowState);
            //inputOutput += "<tr><td>&nbsp;&nbsp River Flow</td><td> = " + flowLabel + "</tr>";
            //inputOutput += "<tr><td>&nbsp;&nbsp Colorado River Trace State Year</td><td> = " + CORiverFlowValue.toString() + "</tr>";
            //inputOutput += "<tr><td>&nbsp;&nbsp Salt/Verde Rivers Trace State Year</td><td> = " + SVRiverFlowValue.toString() + "</tr>";
            ////////////////////////////////////////////////////////////


            inputOutput += "</table>";
            ReportWindow.document.getElementById("inputs").innerHTML = inputOutput;
            //============================================
            // Copy Indicator Controls
            //=======================================
            var IndOutput = "<table><tr style='border-collapse: collapse'>";
            var cnt = 0;
            var IndLength = d3.keys(IndicatorControlsArray).length;
            var IndicatorValues = new Array();
            var IndicatorTypes = new Array();
            for (var element in IndicatorControlsArray) {
                IndicatorTypes.push(element);
                var indicator = IndicatorControlsArray[element];
                //IndOutput += "<tr><td><div id='indR" + cnt.toString() +
                //            "' width='" + this.canvas.width.toString() +
                //            "' height='" + this.canvas.height.toString() + "' /></td>"+
                //            "<td></td></tr>";

                // QUAY EDIT 1/2/14 to support report creation
                //var section = document.getElementById("acc" + (cnt + 1).toString());
                var si_description = "";
                var sectlab = "";
                //var sectsrc = "";
                // This is now hard coded in indicatorcore.js VERY BAD THIS IS ALL VERY CUMBERSOME AND NEEDS ReDESIGNED
                if (indicator.Description != undefined) {
                    si_description = indicator.Description;
                }

                IndicatorValues[cnt] = indicator.value;
                if (indicator.Title != undefined) {
                    sectlab = indicator.Title;
                }
                //if ((section !== undefined) && (section !== null)) {
                //    //$(section).find("iframe").each(function () {
                //    //    sectsrc = this.src;
                //    //});
                //    $(section).find("a").each(function () {
                //        sectlab = this.innerHTML;
                //    });
                //}
                IndOutput += "<td id='indR" + cnt.toString() + "' style='height:110px;width:110px; vertical-align:top; border:1px solid black'></td>" +
                "<td style='height:110px; border:1px solid black'><p style='font-size:90%'><b>" + sectlab + "</b><br>&nbsp;&nbsp;&nbsp;Value = " + indicator.displayValue.toString() + "</p></td><td><p>" + si_description + "<p></td>";

                //IndOutput += "<tr style='border-collapse: collapse'><td id='indR" + cnt.toString() + "' style='height:110px;width:110px; vertical-align:top; border:1px solid black'></td>" +
                //                "<td style='height:110px; border:1px solid black'><p style='font-size:90%'><b>" + sectlab + "</b>&nbsp;&nbsp;&nbsp;Value = " + this.value.toString() + "</p><iframe width='600' height='100' src='" + sectsrc + "'></iframe></td></tr>";
                // QUAY edit End 1/2/14

                cnt++;
                
                if (cnt % 3 == 0) {
                    IndOutput += "</tr><tr style='border-collapse: collapse'>"
                }
                else if (IndLength == cnt) {
                    IndOutput += "</tr>";
                }
            }
            IndOutput += "</table>";
            ReportWindow.document.getElementById("indicators").innerHTML = IndOutput;
            cnt = 0;

            var TestIndicators = WaitForIndicators(ReportWindow);
            if (TestIndicators) {
                ReportWindow.initializeIndicators(IndicatorTypes);

                ReportWindow.SetIndicatorValues(IndicatorValues);
            }
            //=================================================
            // Copy the charts to the Report Window
            //==========================================



            //============================================
            var OutCnt = 0;
            $(".OutputControl").each(function () {
                OutCnt++;
            });
            ProgressBarInc = Math.floor(100 / OutCnt);
            ChartDoneCnt = 0;
            // ok cycle through this list of tabs, fetch the tab, find the frame
            try {
                if (ReportWindow.document.body.style.cursor) {
                    ReportWindow.document.body.style.cursor = "wait";
                }
                // create a new array of these chart references
                var chartarray = new Array();
                // loop through the different tabs
                for (var i = 0; i < TabNames.length; i++) {
                    // if tabname defined
                    if (TabNames[i]) {
                        // get this div
                        var TabDiv = $("#" + Tabhref[i]);
                        // ok no find all the frame class divs in this div
                        //$(TabDiv).find(".frame").each(function () {
                        //    //groupcnt++;
                        //    // get teh frames id and grab the lower part to use as ther base id for the chart canvas
                        //    var frameId = $(this).attr('id');
                        //    var splitid = frameId.split('-');
                        //    var groupname = splitid[1];

                            var groupname = Tabhref[i].split('Chart')[0];
                            // ok its title
                            //var grouptitle = TabNames[i] + " Charts";
                            var grouptitle = TabNames[i];
                            // clear the chart array
                            var chartcnt = 0;
                            chartarray.length = 0;
                            // ok cycle through the output controls in this frame
                            $(TabDiv).find(".OutputControl").each(function () {

                                // get the control id
                                var controlID = $(this).attr('id');
                                // ghet the title
                                var theTitle = $("#" + controlID).find("span[id*=_lblTitle]").html();
                                
                                // check if this shouldf be printed
                                var PrintThis = PrintChart(theTitle);
                                //console.log('theTitle:', theTitle, PrintThis);
                                if (PrintThis) {
                                    // get the container id
                                    var chartdivID = $("#" + controlID).find("div[id*=ChartContainer]").attr('id');
                                    //  just in case make sure it was found
                                    if (chartdivID !== undefined) {
                                        // ok get the container div
                                        var chartdiv = $("#" + chartdivID);
                                        // make sure it contains a highchart
                                        if (chartdiv.highcharts) {
                                            // fetch the highchart
                                            var chart = chartdiv.highcharts();
                                            // if found stick it in the array
                                            if (chart) {
                                                chartarray[chartcnt] = chart;
                                                chartcnt++;
                                            }
                                            else {
                                                //console.log('chartdiv:', "#" + chartdivID, chartdiv, CreatePrintableSVG(chartdiv));
                                                chartarray[chartcnt] = CreatePrintableSVG(chartdiv);
                                                chartcnt++;
                                            }
                                        }
                                    }
                                }
                            });
                            // ok, we have everything, copy the charts
                            if (chartcnt > 0) {
                                //result = AddCharts(ReportWindow, chartarray, groupname, grouptitle, 300, 300);
                                result = AddCharts(ReportWindow, chartarray, groupname, grouptitle, 425, 500);
                            }
                        //});
                    }
                }
            }
            catch (err) {
                result = "Exception: " + err.message;
            }
            if (result != "") {
                alert("Error creating report: " + result + "\n\nDid you close the Report Browser Window?");
            }
        }
    }
    catch (err) {
        alert("Error creating report: " + result+ " - "+err.message);
    }
    if (result=="") {
        ReportWindow.HideWait();
        ReportWindow.ShowPrint();
        SetBodyCursor(ReportWindow, "default");
    }
    
    if (MyOldCursor) {
        SetBodyCursor(window,MyOldCursor)
    }
    return result;
}

//=================================================

$("#ReportButton").click(function () {
    CreateReport();
});