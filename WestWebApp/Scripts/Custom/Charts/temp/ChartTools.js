/// <reference path="/Scripts/Custom/Charts/ChartTools.js" />
//------------------------------------------------------------

var ChartTypeLine = "line";
var ChartTypeColumn = "column";
var ChartTypeColumnStacked = "columnstack";
var ChartTypePie = "pie";
var ChartTypeArea = "area";
var ChartTypeAreaLine = "arealine";
var ChartTypeAreaStacked = "areastacked";
var ChartTypeLineStacked = "linestacked";

var RegionCode = "reg";

function GetMax(fldnames, fldmax) {
    // setup vars
    var maxval = 0;
    // check if array or single string
    if (fldnames.length) {
        if (fldnames.length>0) {
            if (fldmax[fldnames[0]] != undefined) {
                maxval = fldmax[fldnames[0]]; 
                for (var i = 0; i < fldnames.length; i++) {
                    if (fldmax[fldnames[i]] != undefined) {
                        var aMax = fldmax[fldnames[i]];
                        if (aMax > maxval) { maxval = aMax; }
                    }
                }
            }
        }
    }
    return maxval;
}
function GetMin(fldnames, fldmax) {
    // setup vars
    var minval = 0;
    // check if array or single string
    if (fldnames.length) {
        if (fldnames.length>0) {
            if (fldmax[fldnames[0]] != undefined) {
                minval = fldmax[fldnames[0]]; 
                for (var i = 0; i < fldnames.length; i++) {
                    if (fldmax[fldnames[i]] != undefined) {
                        var aMin = fldmax[fldnames[i]];
                        if (aMin < minval) { minval = aMin; }
                    }
                }
            }
        }
    }
    return minval;
}

        //// check if single dell
        //if (fldnames.length == 1) {
        //    maxval = fldmax[fldnames[0]];
        //} else {
        //    // ok build an array of vals
        //    var maxvalues = [];
        //    $.each(fldnames, function () {
        //        if (fldmax[this] != undefined) {
        //            maxvalues.push(fldmax[this]);
        //        }
        //    });
        //    // find the max in this array
        //    maxval = Math.max.apply(Math, maxvalues);
        //}
//    } else {
//        // Ok something else, if number will assign, otherwise will be wierd.
//        maxval = fldmax;
//    }
//    return maxval;
//}
//-----------------------------------------------------------
function VertTicSize(low, high) {
    var range = high - low;
    var ticbase = 0;
    if (range > 1000000) {
        ticbase = 1000000;
    } else
        if (range > 100000) {
            ticbase = 100000;
        } else
            if (range > 10000) {
                ticbase = 10000;
            } else
                if (range > 1000) {
                    ticbase = 1000;
                } else
                    if (range > 100) {
                        ticbase = 100;
                    } else {
                        ticbase = 10;
                    }
    var ticsize = ticbase / 2
    return ticsize;
}
//--------------------------------------------------

function YearTicSize(thestartyr, theendyr) {
    range = theendyr - thestartyr;
    ticsize = 1;
    if (range > 50) { ticsize = 5; }
    else if (range > 25) { ticsize = 2; }
    return ticsize;
}
//--------------------------------------------------

function JudgeMax(flds, maxes, isStacked) {
    var maxval =100;
    if (flds.length) {
        if (flds.length == 1) {
            maxval = maxes[flds[0]];
        } else {
            for (i = 0; i < flds.length; i++) {
                if (maxes[flds[i]] != undefined) {
                    if (maxes[flds[i]] > maxval) { maxval = maxes[flds[i]]; }
                }
            }
        }
    }
     
    if (isStacked) { maxval = maxval * 1; }
    return maxval;
}

function JudgeXStep(firstyear, lastyear, ticsize) {
    var range = lastyear - firstyear;
    var ticN = range / ticsize;
    var stepInt = 1;
    if (ticN > 19) { stepInt = 5; }
    else if (ticN > 9) { stepInt = 2; }
    return stepInt;
}
//--------------------------------------------------------------------
function addCommas(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

function GetChartMax(TheCHartData, isStacked) {
    var maxval = 0;
    var bigvals = new Array();
    var SerN = TheCHartData.length;
    var XN = TheCHartData[0][1].length;
    if (isStacked) {
        var theTotal = 0;
        for (i = 0; i < XN; i++) {
            theTotal = 0;
            for (j = 0; j < SerN; j++) {
                theTotal += TheCHartData[j][1][i];
            }
            bigvals[i] = theTotal;
        }
        maxval = Math.max.apply(Math, bigvals);
    } else {
        for (i = 0; i < XN; i++) {
            theTotal = 0;
            for (j = 0; j < SerN; j++) {
                if (TheCHartData[j][1][i] > maxval) { maxval = TheCHartData[j][1][i]; }
            }
        }

    }
    return maxval;
}
//--------------------------------------------------
// Get provider name from array of object code:value 
//---------------------------
function GetProviderName(pcode,Names) {
    var pname = "";
    if (Names.Length) {
        var PObject = Names[pcode];
        if (PObject!=undefined) {
            pname = Names[pcode];
        }
    }
    return pname;
}


