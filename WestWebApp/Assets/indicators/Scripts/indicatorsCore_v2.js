
//====================================================
// Indicator Control Functions
// ver 3.1

// Modified 6/23/14
// again, by DAS on 09.28.14

// Revisions
// 1 Changed canvas size and size of text font
// 2 Made size dynamic based on window size
// 2 Added an array of indicators for faster processing
//==========================================================
/// <reference path="/Assets/indicatos/Scripts/IndicatorControl_v_4.js" />
var GWPIndicatorControl;
var ENVIndicatorControl;
var AGRIndicatorControl;
var PWCIndicatorControl;
var AWSIndicatorControl;
var POWIndicatorControl;
var ECONIndicatorControl;

var IndicatorControlsArray = new Array();

var indMaxWidth = 160;
var indMaxHeight = 160;
var indMinWidth = 80;
var indMinHeight = 80;
var indWidth = indMaxWidth;
var indHeight = indMaxHeight;
var ScreenWidth = 0;

function findcssRule(asheet, ruleText) {
    for (var i = 0; i < asheet.cssRules.length; i++) {
        var rule = asheet.cssRules[i];
        if (rule.cssText.indexOf(ruleText) == 0) {
            return rule;
        }
    }
    return undefined;
}
function findStyleSheet(adoc,sheetName) {
  var sheets = adoc.styleSheets;
  for (var i = 0; i < sheets.length; i++) {
      if (sheets[i].href.indexOf(sheetName) > -1) {
          return sheets[i];
      }
    }
  return undefined;
}

//function SetLeftMargin(theValue) {
//    var theTarget = ".header article.accordion section:target {";
//    var SiteSheet = findStyleSheet(document, "Site.css");
//    if (SiteSheet) {
//        var targetRule = findcssRule(SiteSheet, theTarget);
//        if (targetRule) {
//            var thestyle = targetRule.style;
//            thestyle.marginLeft = theValue.toString() + "px";
//        }
//    }
//}

ResizeDashboard();

$(window).resize(function () {

    ResizeDashboard();

});

function ResizeDashboard() {
    ResizeIndicator();
    ResetIndicatorSections(indWidth,indHeight);
    if (IndicatorControlsArray.length > 0) {
        $(IndicatorControlsArray).each(function () {
            this.resize(indWidth, indHeight);
        });
    }

}

function SI_Target_Left_edge() {
    return 600;
}

function ResetIndicatorSections(indWidth, indHeight) {
    var WidthInc = 10;
    var HeightInc = 10;
    var TopInc = 8;
    var TopThree = -9;
    var LeftInc = 4;
    var step = (indWidth - 80) / 10;
    var inverseStep = 10 - step;

    var SectWidth = Math.floor(110 + (step * WidthInc));
    var SectHeight = Math.floor(120 + (step * HeightInc));
    var H1Top = Math.floor(130 + (step * TopInc));
    var H2Top = Math.floor(90 + (step * TopInc));
    //var H3Top = Math.floor(158 + (inverseStep * TopThree));
    var H3Top = Math.floor(190 + (inverseStep * TopThree));
   // var H3Margin = Math.floor(55 + (step * LeftInc));
    var H3Margin = Math.floor(57 + (step * LeftInc));
    //
    var FontSize = Math.floor(14 + step);

    var FontSize_SI = Math.floor(16 + step);
    var FontSize_SI = Math.floor(10 + (2 * step));

    var TargetLeft = (SectWidth * 4) + 10;
    var iFrameWidth = ScreenWidth - (220 + (4 * SectWidth) + indWidth);
    if (iFrameWidth > 500) { iFrameWidth = 500; }

    $(".accordion").each(function () {
        //var allelem = $(this).find("*");
        $(this).find("section").each(function () {
            this.style.width = SectWidth.toString()+"px";
            this.style.height = SectHeight.toString() + "px";

            var H2Elmt = $(this).find("h2");
            H2Elmt[0].style.top = H2Top.toString() + "px";
            H2Elmt[0].style.fontSize = FontSize.toString() + "px";
            //
            // 03.13.15. DAS
            // This code loads the place holders for old (previous run) of the indicators
            var H3Elmt = $(this).find("h3");
            H3Elmt[0].style.top = H3Top.toString() + "px";
            H3Elmt[0].style.marginLeft = H3Margin.toString() + "px";
            H3Elmt[0].style.fontSize = FontSize_SI.toString() + "px";
            
        });

  
       // $(this).find("iframe").each(function () {
        //   this.style.width = iFrameWidth.toString() + "px";
        //    this.style.height = (SectHeight-FontSize).toString() + "px";
       // });

      //  SetLeftMargin(TargetLeft);
    }
    );
   
   // DAS 09.26.14
    $(".dashboard-header").each(function () {
            // this.style.width = SectWidth.toString() + "px";
            //this.style.height = SectHeight.toString() + "px";

           var H1Elmt = $(this).find("h1");
            H1Elmt[0].style.top = H1Top.toString() + "px";
            H1Elmt[0].style.fontSize = FontSize.toString() + "px";
            //

      });
    }

function ResizeIndicator() {
    ScreenWidth = window.innerWidth;
    //var indSize = Math.floor((ScreenWidth - 800) / 4); DAS 09.28.14
    var indSize = Math.floor((ScreenWidth - 800) / 2);
    if (indSize < indMaxWidth) {
        if (indSize > indMinWidth) {
            indWidth = indSize;
            indHeight = indSize;
        }
        else {
            indWidth = indMinWidth;
            indHeight = indMinHeight;
        }
    }


}
// QUAY ADDED 1/2/14 to provide support to the report function
// This is a hack and we need a more elegant indicator system
function IndicatorAddDescript(TheIndicator, TheLabel, TheDescription) {
    if (TheIndicator.Description != undefined) {
        if (TheDescription != undefined) {

            TheIndicator.Description = TheDescription;
        }
        if (TheLabel != undefined) {
            TheIndicator.Label = TheLabel;
        }
    }
}
// QUAY ADDED 1/2/14 END
function initializeIndicators() {

    GWPIndicatorControl = new IndicatorControl("idGWPDiv", "GWP", "idGWPIndicator", indWidth, indHeight);//, document.getElementById("idhelp"));
    ENVIndicatorControl = new IndicatorControl("idENVDiv", "ENV", "idENVIndicator", indWidth, indHeight);//, document.getElementById("idhelp"));
    AGRIndicatorControl = new IndicatorControl("idAGRDiv", "AGR", "idAGRIndicator",indWidth, indHeight);//, document.getElementById("idhelp"));
    PWCIndicatorControl = new IndicatorControl("idPWCDiv", "PWC", "idPWCIndicator", indWidth, indHeight);//, document.getElementById("idhelp"));
    AWSIndicatorControl = new IndicatorControl("idAWSDiv", "AWS", "idAWSIndicator", indWidth, indHeight);//, document.getElementById("idhelp"));
    POWIndicatorControl = new IndicatorControl("idPOWDiv", "POW", "idPOWIndicator", indWidth, indHeight);//, document.getElementById("idhelp"));
    ECONIndicatorControl = new IndicatorControl("idECONDiv", "ECON", "idECONIndicator", indWidth, indHeight);//, document.getElementById("idhelp"));




    PWCIndicatorControl.MaxValue = 300;

    IndicatorControlsArray[0] = GWPIndicatorControl;
    IndicatorControlsArray[1] = ENVIndicatorControl;
    IndicatorControlsArray[2] = AGRIndicatorControl;
    IndicatorControlsArray[3] = PWCIndicatorControl;
    IndicatorControlsArray[4] = AWSIndicatorControl;
    IndicatorControlsArray[5] = POWIndicatorControl;
    IndicatorControlsArray[6] = ECONIndicatorControl;



    // QUAY ADDED 1/2/14
    // The indicator structire is just all messed up now, add this hardcoded label becuase it is two difficult to extract it out of the heml now.
    // ALL NEEDS TO BE REDESIGNED!!!
    IndicatorAddDescript(GWPIndicatorControl,"Groundwater", "The percent of groundwater that is used to meet total demand is one indicator of a sustainable water supply.  The lower the percent groundwater used, the more replensishable surface water is used, leaving more groundwater available for use during surface water curtailes cause by droughts or emergencies.");
    IndicatorAddDescript(ENVIndicatorControl, "Environment","We all benefit from the environmental features we find along and in our rivers.  Adequate water flows in these rivers and streams is essential for these environments to thrive. Yet we withdraw millions of gallons a day from rivers to irrigate frame lands and provide water for you to drink.  This indicator measures how much of water is left in the river to meet these environmental needs.");
    IndicatorAddDescript(AGRIndicatorControl, "Agriculture","Agrigulture provides the food we all need to survive.  The closer that food is produced to you, the lower the cost and the fresher it is.  Today close to  50% of the water in Maricopa county is used for agricultural irrigation.  But this water could be used to help meet the water needs of growing communities.  This idicator measures how much water is being used for irrigation or for urban use.");
    IndicatorAddDescript(PWCIndicatorControl, "Personal", "Water is essential for you to be healthy and comfortable.  It is also needed to support the communities industrial and commercial businesses.  One way provide more water for other personal values and the economy is to personally use less.  But as you use less water, you also give up some comfort and at very low consumption some health.  This indicator measures how much water is used by each person in the community.");
    IndicatorAddDescript(AWSIndicatorControl, "Population", "Most communities require some population growth in order to maintain a healthy economy and improve the community's quality of life.  But more people requires more water and water supplies are limited.  This indicator measures how many years you can support the current population with water before you will experience water deicits or have to find more water at a higher cost.");
    IndicatorAddDescript(POWIndicatorControl, "Power", "Most communities require some population growth in order to maintain a healthy economy and improve the community's quality of life.  But more people requires more water and water supplies are limited.  This indicator measures how many years you can support the current population with water before you will experience water deicits or have to find more water at a higher cost.");
    IndicatorAddDescript(ECONIndicatorControl, "Economy", "Most communities require some population growth in order to maintain a healthy economy and improve the community's quality of life.  But more people requires more water and water supplies are limited.  This indicator measures how many years you can support the current population with water before you will experience water deicits or have to find more water at a higher cost.");



    // QUAY ADDED 1/2/14 END
}

   
function drawAllIndicators() {
    //looping through indicator controls and getting the required data and populating the charts
    //$('.IndicatorControl').each(function () {
    //    var controlID = $(this).attr('id');
    //    drawIndicator(controlID, (endYr - $jsonObj.MODELSTATUS.STARTYEAR));
        //
        drawIndicatorSimple( (endYr - $jsonObj.MODELSTATUS.STARTYEAR));
        //
    //});

}


function drawIndicator(controlID, yearIndex) {

    var $jsonObj = $.parseJSON(jsondata); //parsing the Input String as Json object
    var fieldID = $("#" + controlID).attr('data-fld');
    //Getting the chart data
    $.each($jsonObj.RESULTS, function () {
         if (this.FLD == "PCTGWDEM") {
                GWPIndicatorControl.SetValue(this.VALS[0].VALS[yearIndex]);
        } else if (this.FLD == "SINDENV") {
                ENVIndicatorControl.SetValue(this.VALS[yearIndex]);
         } else if (this.FLD == "SINDAG") {
                AGRIndicatorControl.SetValue(this.VALS[yearIndex]);
         } else if (this.FLD == "SINDPC") {
                PWCIndicatorControl.SetValue(this.VALS[yearIndex]);
         } else if (this.FLD == "SINYRGWR") {
                AWSIndicatorControl.SetValue(this.VALS[0].VALS[yearIndex]);
         }
   });
}

function drawIndicatorSimple(yearIndex) {

    var $jsonObj = $.parseJSON(jsondata); //parsing the Input String as Json object
    var done = new Array();
    for (di = 0; di < 5; di++) { done[di] = 0; }
    //Getting the chart data
    $.each($jsonObj.RESULTS, function () {
        //for (i = 0; i < $jsonObj.RESULTS.length; i++) {
        if (this.FLD == "PCTGWDEM") {
            if (done[0] == 0) {
                GWPIndicatorControl.SetValue(this.VALS[0].VALS[yearIndex]);
                done[0] = 1;
            }
            
        } else if (this.FLD == "SINDENV") {
            if (done[1] == 0) {
                ENVIndicatorControl.SetValue(this.VALS[yearIndex]);
                done[1] = 1;
            }
         
        } else if (this.FLD == "SINDAG") {
            if (done[2] == 0) {
                AGRIndicatorControl.SetValue(this.VALS[yearIndex]);
                done[2] = 1;
            }
         
        } else if (this.FLD == "SINDPC") {
            if (done[3] == 0) {
                PWCIndicatorControl.SetValue(this.VALS[yearIndex]);
                done[3] = 1;
            }
        
        } else if (this.FLD == "SINYRGWR") {
            if (done[4] == 0) {
                AWSIndicatorControl.SetValue(this.VALS[yearIndex]);
                done[4] = 1;
            }
        }
     
    });



}
