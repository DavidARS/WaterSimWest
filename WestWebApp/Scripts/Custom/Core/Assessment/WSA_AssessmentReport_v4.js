// DATA FOR WATERSIM AMERICA ASSESSMENT
// Ray Quay
// 3/20/16
// <reference path="scripts/jquery-2.2.0.js" /> 
var NumberOfSates = 5;
var NumberOfIndicators = 8;

var IndFields = ["GWSYA", "SWI", "TSW", "ENVIND", "UEF", "AGIND", "PEF", "ECOR"];
var IndFieldString = ["Groundwater", "Environment", "Environment", "Environment", "Cities and Towns", "Agriculture", "Power", "Economy"];

function GetLastData(ResultDataObject) {
    lastval = 0;
    // if this is an array get the last value
    if (ResultDataObject.length) {
        if (ResultDataObject.length > 0) {
            lastval = ResultDataObject[ResultDataObject.length - 1];
        }
    }
        // if not just get the valie
    else {
        lastval = ResultDataObject;
    }
    return lastval;
}

///-------------------------------------------------------------------------------------------------
/// <summary>   Evoke assessment report. </summary>
///
/// <remarks>  </remarks>
///
/// <param name="AssRepDivId">      Identifier for the ass rep div. </param>
/// <param name="jsonResultData">   Information describing the JSON result. </param>
///
/// <returns>   . </returns>
///-------------------------------------------------------------------------------------------------

function EvokeAssessmentReport(AssRepDivId, jsonResultData) {
    // check if data is there
    if ((AssRepDivId) && (jsonResultData)) {
        // build indicator array
        CurrentIndValues = new Array();
        // fill with avg values
        for (ci = 0; ci < IndFields.length; ci++) {
            CurrentIndValues[ci] = 50;
        }
        // go the results data
        if (jsonResultData.RESULTS) {
            ResultsData = jsonResultData.RESULTS;
            if (ResultsData.length) {
                CurrentState = -1;

                // loop through all the resulys
                for (ri = 0; ri < ResultsData.length; ri++) {
                    // for each one, see if you can find a match for the data FLD to indicator fields
                    if ((ResultsData[ri].FLD) && (ResultsData[ri].VALS)) {
                        fld = ResultsData[ri].FLD;
                        // get the state if there
                        if (fld == "ST") {
                            CurrentState = GetLastData(ResultsData[ri].VALS);
                        }
                        for (indi = 0; indi < IndFields.length; indi++) {
                            if (fld == IndFields[indi]) {
                                // OK a match, GOOD!
                                // get the results data
                                inddata = ResultsData[ri].VALS;
                                lastval = 0;
                                // if this is an array get the last value
                                lastval = GetLastData(ResultsData[ri].VALS);
                                // stick in the right array spot
                                CurrentIndValues[indi] = lastval;
                                break;
                            }
                        }
                    }
                }
                // OK doit
                WriteAllAssessments(AssRepDivId, CurrentState, CurrentIndValues);
                //$("#" + AssRepDivId).show();
            }
        }

    }
}

///-------------------------------------------------------------------------------------------------
/// <summary>   Indicator assessment. </summary>
///
/// <remarks>   Mcquay, 3/29/2016. </remarks>
///
/// <param name="aLow">         The low. </param>
/// <param name="aHigh">        The high. </param>
/// <param name="aLowMessage">  Message describing the low. </param>
/// <param name="aHighMessage"> Message describing the high. </param>
///
/// <returns>   . </returns>
///-------------------------------------------------------------------------------------------------

function IndicatorAssessment(aLow, aHigh, aLowMessage, aHighMessage) {
    this.Low = 0;
    if (aLow) {
        this.Low = aLow;
    }
    this.High = 100;
    if (aHigh) {
        this.High = aHigh;
    }
    
    this.LowMessage = "";
    if (aLowMessage) {
        this.LowMessage = "";
    }
    this.HighMessage = "";
    if (aHighMessage)
    {
        this.HighMessage = aHighMessage;
    }
    else
    {
        this.HighMessage = this.LowMessage;
    }

    this.IsSustainable = function (value) {
        test = ((value >= this.Low) && (value <= this.High));
        return test;
        }
    this.GetMessage = function (value) {
        susMsg = "";
        if (value<this.Low)
        {
            susMsg = this.LowMessage;
        }
        else
        {
            if(value>this.High)
            {
                susMsg = this.HighMessage;
            }
        }
        return susMsg;
    }
        
}

/// <summary>   List of assessments. </summary>
var AssessmentList = new Array();
// Assign the Assessment Values
AssessmentList[0] = new IndicatorAssessment(30, 100, "Groundwater safe yield goal is not likely to be achieved.", "What the fuck");
AssessmentList[1] = new IndicatorAssessment(30, 100, "Community withdrawals from surface water are reaching unsuatianble levels and are susceptible to impact from drought as the amount of water withdrawn bwecomes a larger percentage of surface water flow.");
AssessmentList[2] = new IndicatorAssessment(30, 100, "Community withdrawals from surface water are reaching unsuatianble levels and are susceptible to impact from drought as the amount of water withdrawn bwecomes a larger percentage of surface water flow.");
AssessmentList[3] = new IndicatorAssessment(30, 100, "Reductions in surface flows due to high withdrawals or declines in effluent discharge are impacting natural areas dependent on surface water flows.");
AssessmentList[4] = new IndicatorAssessment(20, 75, "Very high levels of water efficiency can impact the sustainable quality of life and/or economy of the community", "Low levels efficiency can waste community water resource and strain the ability to grow or respond to long term drought.");
AssessmentList[5] = new IndicatorAssessment(30, 75, "Very high levels of water efficiency can strain the economic sustainablity of agricultural production", "Low levels efficiency can waste water resources and strain the ability to respond to economic changes or long term drought.");
AssessmentList[6] = new IndicatorAssessment(30, 75, "Very high levels of water efficiency can strain the economic sustainablity of power production", "Low levels efficiency can waste water resources and strain the ability to respond to changes in the economics of power production.");
AssessmentList[7] = new IndicatorAssessment(30, 100, "Increasing water efficiency to very high levels can have a negative impact on the portion of the economy that relies on water as a basic resources for production or service delivery");

/// <summary>   List of indicator items. </summary>
var IndicatorItemList = new Array();
// Sampler = new AssessmentItem(201,"A Label","Short description",[1,1,0,1,1],
//                              [{Low:0,High:100},{Low:0,High:100},{Low:0,High:100},{Low:0,High:100},{Low:0,High:100},{Low:0,High:100},{Low:0,High:100},{Low:0,High:100}]);
//                              

///-------------------------------------------------------------------------------------------------
/// <summary>   Assessment item. </summary>
///
/// <param name="aCodeNum">             The code number. </param>
/// <param name="aLabelTxt">            The label text. </param>
/// <param name="theText">              the text. </param>
/// <param name="StateDataArray">       Array of state data. </param>
/// <param name="IndicatorDataArray">   Array of indicator data. </param>
///
/// <returns>   . </returns>
///-------------------------------------------------------------------------------------------------

function AssessmentItem(aCodeNum, aLabelTxt, theText, StateDataArray, IndicatorDataArray) {
    this.Code = aCodeNum;
    this.Label = aLabelTxt;
    this.Text = theText;
    this.StateData = [];
    if (StateDataArray.length) {
        if (StateDataArray.length == NumberOfSates) {
            this.StateData = StateDataArray;
        }
    }
    this.IndData = [];
    if (IndicatorDataArray.length) {
        if (IndicatorDataArray.length == NumberOfIndicators) {
            //if (IndicatorDataArray[0].Low) {
                this.IndData = IndicatorDataArray;
            //}
        }
    }
    this.ValidForState = function (aStateIndex) {
        if ((this.StateData.length > 0) && (aStateIndex > -1) && (aStateIndex < NumberOfSates)) {
            return (this.StateData[aStateIndex] == 1);
        }
    };
    this.ValidForIndicator = function (IndicatorValueArray) {// GWind, USWind, TSWind, ENVind, UEFind, AGEind, PWEind, ECOind) {
        test = false;
        if(IndicatorValueArray.length){
            if (IndicatorValueArray.length == this.IndData.length)
            {
                test = true;
                for(i=0;i<this.IndData.length;i++)
                {
                    indvalue = IndicatorValueArray[i];
                    if((indvalue<this.IndData[i].Low)||(indvalue>this.IndData[i].High)) {
                        test = false;
                        break;
                    }
                }
            }
        }
        return test;
    }
}

///-------------------------------------------------------------------------------------------------
/// <summary>   Gets a level. </summary>
/// <remarks> get the level represented by code 1000 = 1, 1100 = 2; 1101 = 3;
/// <param name="parameter1">   The first parameter. </param>
///
/// <returns>   The level. </returns>
///-------------------------------------------------------------------------------------------------

function GetLevel(sCode) {
    mylevel = 0;
    if ((sCode % 1000) == 0) {
        mylevel = 1;
    } else
        if ((sCode % 100) == 0) {
            mylevel = 2;
        } else {
            mylevel = 3;
        }
    return mylevel;
}


// WriteAssessment
/// <summary>   Size of the ass font. </summary>
// Need to get ridf of these
var AssFontSize = new Array();
AssFontSize[0] = "12px";
AssFontSize[1] = "24px";
AssFontSize[2] = "20px";
AssFontSize[3] = "18px";
AssFontSize[4] = "16px";

///-------------------------------------------------------------------------------------------------
/// <summary>   Creates multi string. </summary>
///
/// <param name="NumberOf"> Number of ofs. </param>
/// <param name="aString">  The string. </param>
///
/// <returns>   The new multi string. </returns>
///-------------------------------------------------------------------------------------------------

function CreateMultiString(NumberOf, aString) {
    multistring = "";
    for (si = 0; si < NumberOf; si++) {
        multistring += aString;

    }
    return multistring;
}

///-------------------------------------------------------------------------------------------------
/// <summary>   Writes all assessments. </summary>
///
/// <remarks>   Mcquay, 3/29/2016. </remarks>
///
/// <param name="theDivid">         the divid. </param>
/// <param name="theState">         State of the. </param>
/// <param name="IndValueArray">    Array of ind values. </param>
///
/// <returns>   . </returns>
///-------------------------------------------------------------------------------------------------

function WriteAllAssessments(theDivid, theState, IndValueArray) {
    theDiv = document.getElementById(theDivid);
    if (theDiv) {
        if (theDiv.innerHTML) {
            // clear it frst
            theDiv.innerHTML = "";
            theHTML = "";

            // do Introduction Assessment
            theHTML += "<div>  </div>";
            theHTML += "<div class='WSA-header'> Assessment Report </div>";
            tempSusAss = "";
            SusAsscnt = 0;
            lastitem = "";
            for (assi = 0; assi < IndFields.length; assi++) {
                assitem = AssessmentList[assi];
                if (!assitem.IsSustainable(IndValueArray[assi]))
                {
                    SusAsscnt++;
                    tempSusAss += lastitem;
                    lastitem = IndFieldString[assi];
                    if (SusAsscnt > 1) {
                        tempSusAss += ", ";
                    }
                }
            }
            if (SusAsscnt > 1) {
                tempSusAss += " and " + lastitem;
            }
            else {
                tempSusAss += lastitem;
            }

            if (SusAsscnt > 0) {
                theHTML += "<div class='WSA-msg'> Your State still faces some " + tempSusAss + " sustainability challenges! ";
                theHTML += "There are actions that could make your State more sustainable.</div>";

            } else {
                theHTML += "<div class='WSA-msg'> Well Done, Sustainability is well balanced! ";
                theHTML += "But there are still actions that could make your State more sustainable.</div>";
            }
            theHTML += "<div class='WSA-msg'> Click or press on items with a <img src='" +ExpandIconSrc+ "' height='" + XpanIMGHeight / 2 + "'/> to expand the list of actions.</div>";
            // now do assistance  report
            lastlevel = 0;

            iconsrc = "";
            for (alli = 1; alli < IndicatorItemList.length; alli++) {
                anItem = IndicatorItemList[alli];
                if (anItem.ValidForState(theState)) {
                    if (anItem.ValidForIndicator(IndValueArray)) {
                        temp = "";
                        major = ~~(anItem.Code / 1000);
                        level = GetLevel(anItem.Code);

                        // if this is the same as last, close the div
                        if (level == lastlevel) {
                            temp += "</div>";
                        }
                        if (level < lastlevel) {
                            if (level == 1) {
                                temp += CreateMultiString((lastlevel - level), "</div>");
                                temp += "<br></div>";
                            }
                            else {
                                temp += CreateMultiString((lastlevel - level) + 1, "</div>");
                            }
                        }
                        //theXpanId = id" + anItem.Code.toString() + "' class='XpanDiv-L" + level.toString() + "-C" + major.toString() + level.toString()
                        temp += "<div id='idWSA" + anItem.Code.toString() + "' class='XpanDiv-L" + level.toString() + "-C" + major.toString() + level.toString() + "' data-onlyone='true' >";
                        //                temp += "<div class='Info-C" + major.toString() + level.toString() + "'>";
                        //temp += "<div id='id" + anItem.Code.toString() + "' class='XpanDiv-L" + level.toString() + "' data-onlyone='true' >";
                        //temp += "<div class='Info-C" + major.toString() + level.toString() + "'>";

                        //temp += "<p style='font-size:" + AssFontSize[level] + "; '>";
                        if (i < (IndicatorItemList.length - 1)) {
                            nextitem = IndicatorItemList[i + 1];
                            nextlevel = GetLevel(nextitem.Code);
                            if (nextlevel > level) {
                                iconsrc = ExpandIconSrc;
                            }
                            else {
                                iconsrc = EmptyIconSrc;
                            }
                        }
                        else {
                            iconsrc = EmptyIconSrc;
                        }
                        temp += "<Table cellpadding='10'><tr>";
                        if (level > 2) {
                            temp += "<td style='width:8%; vertical-align: text-top;'>";
                            temp += "<img id='idWSA" + anItem.Code + "IMG" + "' src='" + iconsrc + "' height='" + XpanIMGHeight / 2 + "' style='display: block; margin-left: auto; margin-right: auto; '/>";
                            temp += "</td><td style='width:30%; vertical-align: text-top; text-align: left;'>";
                        } else {
                            temp += "<td style='width:10%; vertical-align: middle;'>";
                            temp += "<img id='idWSA" + anItem.Code + "IMG" + "' src='" + iconsrc + "' height='" + XpanIMGHeight + "' style=' padding: 0 10px 0 20px; '/>";
                            temp += "</td><td style='width:90%; vertical-align: middle; text-align: left;'> &nbsp;&nbsp; ";
                        }

                        temp += anItem.Label;
                        temp += "</td>";
                        if (level > 2) {
                            temp += "<td style='width:65%'>" + anItem.Text + "</td>";
                        }
                        temp += "</tr></table>";

                        //               temp += "<img id='id" + anItem.Code + "IMG" + "' src='" + iconsrc + "' height='" + XpanIMGHeight + "' style=' vertical-align: middle; margin: 10px 0px 0px 0px;'/>";
                        //if (level > 2) {
                        //    temp += "<Table><tr><td style='width:5%; vertical-align: text-top;'>";
                        //    temp += "<img id='idWSA" + anItem.Code + "IMG" + "' src='" + iconsrc + "' height='" + XpanIMGHeight/2 + "' style='padding-left: 20px;'/>";
                        //    temp += "</td><td style='width:30%; vertical-align: text-top;'>";
                        //} else {
                        //    temp += "<img id='idWSA" + anItem.Code + "IMG" + "' src='" + iconsrc + "' height='" + XpanIMGHeight + "' style='padding-top: 10px; padding-left: 20px;'/>";
                        //}
                        //temp += anItem.Label;
                        //if (level > 2) {
                        //    temp += "</td>";
                        //    temp += "<td style='width:65%'>" + anItem.Text + "</td></tr></table>";
                        //}

                        theHTML += temp;

                        lastlevel = level;
                    }
                }
            }
            // close all the open divs
            if (lastlevel > 1) {
                theHTML += CreateMultiString(lastlevel - 1, "</div>");
            }
            theHTML += "<br></div>";

            theDiv.innerHTML = theHTML;
            $("#" + theDivid).find("[class^='XpanDiv']").each(function () {
                if (CheckIfXpanChildren(this)) {
                    $(this).click(function (event) {
                        if (event.stopPropagation) {
                            event.stopPropagation();
                        };
                        changeState(this)
                    });
                }
                else {
                    $(this).click(function (event) {
                        if (event.stopPropagation) {
                            event.stopPropagation();
                        };
                    });
                }
                // Create State attribute, set to true to allow callapse
                this.setAttribute(XpanState, true);
                // start with everything collapsed
                CollapseDiv(this);
            });
        }


    }
}

///-------------------------------------------------------------------------------------------------
/// <summary>   Writes an assessment list. </summary>
/// <remarks>   Writes and assessment if criteria met
/// <param name="parameter1">   The first parameter. </param>
/// <param name="parameter2">   The second parameter. </param>
///
/// <returns>   . </returns>
///-------------------------------------------------------------------------------------------------

function WriteAssessmentList(IndicatorValueArray, TargetDivID) {
    if (IndicatorValueArray.length) {
        if (IndicatorValueArray.length == IndFields.length) {
            $(IndicatorItemList).each(function () {
                mycode = this.Code;
                thelevel = GetLevel(mycode);
                if (this.ValidForIndicator(IndicatorValueArray)) {
                    text = AssessmentHTML(this);
                }
            });
        }
    }
}

//################################################
// 
// THE ASSESSMENT DATA
//
//################################################

IndicatorItemList[1] = new AssessmentItem(1000, 'Decrease Water Demand: ', 'Decrease Water Demand ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[2] = new AssessmentItem(1100, 'Increase Urban Efficiency: ', 'Increase Urban Efficiency ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[3] = new AssessmentItem(1101, 'Efficient water fixtures ', 'Install more efficient water fixtures such as toilets and faucets to decrease the amount of water that is flushed away. ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 30, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[4] = new AssessmentItem(1102, 'Water Efficient Landscaping ', 'Not all plants need the same amount of water to live. You can choose plants that are more water efficient for your landscaping and plants are often over watered. Install drip irrigation so that the plant receives the exact amount of water it needs.  When the sun is up, water is lost to evaporation. Save water by irrigating at night when evaporation is reduced. ', [1, 1, 1, 1, 1], [{ Low: 0, High: 60 }, { Low: 0, High: 60 }, { Low: 0, High: 60 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[5] = new AssessmentItem(1103, 'Mandate Water Conservation ', 'Cities can play a role in how much water their residents use my passing laws that limit water use. ', [1, 1, 1, 1, 1], [{ Low: 0, High: 60 }, { Low: 0, High: 60 }, { Low: 0, High: 60 }, { Low: 30, High: 100 }, { Low: 0, High: 70 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 30, High: 100 }]);
IndicatorItemList[6] = new AssessmentItem(1104, 'Increase the Price of Water ', 'By increasing the price of water for residential use, some users will use less water in order to save money. ', [1, 1, 1, 1, 1], [{ Low: 0, High: 60 }, { Low: 0, High: 60 }, { Low: 0, High: 60 }, { Low: 30, High: 100 }, { Low: 0, High: 70 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 30, High: 100 }]);
IndicatorItemList[7] = new AssessmentItem(1105, 'Community Parks and Pools ', 'Increase community parks and pools and decrease the amount of personal lawns and pools, which use a lot of water. ', [1, 1, 1, 1, 1], [{ Low: 0, High: 60 }, { Low: 0, High: 60 }, { Low: 0, High: 60 }, { Low: 0, High: 100 }, { Low: 0, High: 70 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 30, High: 100 }]);
IndicatorItemList[8] = new AssessmentItem(1200, 'Improve Urban Water Infrastructure: ', 'Improve Urban Water Infrastructure ', [0, 0, 1, 1, 0], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 30, High: 100 }]);
IndicatorItemList[9] = new AssessmentItem(1201, 'Replace Old Pipes ', 'A substantial amount of water can be saved by replacing old pipes with new pipes before they leak. ', [0, 0, 1, 1, 0], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 30, High: 100 }]);
IndicatorItemList[10] = new AssessmentItem(1202, 'Increase Water Efficiency of Water Treatment Plants ', 'Installing new, efficient pumping systems and controls improve water system automation. ', [0, 0, 1, 1, 0], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 30, High: 100 }]);
IndicatorItemList[11] = new AssessmentItem(1300, 'Increase Agricultural Efficiency: ', 'Increase Agricultural Efficiency ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[12] = new AssessmentItem(1301, 'Grow Less Water Intensive Crops ', 'Some crops such as almonds and cotton need a lot of water to grow. Save water by switching to crops which use less water. ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 30, High: 100 }]);
IndicatorItemList[13] = new AssessmentItem(1302, 'Reduce Animal Agriculture ', 'Animal agriculture requires a lot of water to raise animals. Switch to plants to save water. ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 30, High: 100 }]);
IndicatorItemList[14] = new AssessmentItem(1303, 'Upgrade Water Infrastructure ', 'Farmers can improve water pump efficiency and powering irrigation pumps with renewable energy. ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 50, High: 100 }]);
IndicatorItemList[15] = new AssessmentItem(1304, 'Improve Irrigation Efficiency ', 'New water technology is constantly emerging. Improve irrigation efficiency by replacing less efficient irrigation systems or replace flood irrigation. Also, level fields to reduce runoff and use drip irrigations systems. ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[16] = new AssessmentItem(1400, 'Attract Less Water Intensive Industry: ', 'Attract Less Water Intensive Industry ', [1, 1, 1, 1, 1], [{ Low: 0, High: 60 }, { Low: 0, High: 60 }, { Low: 0, High: 60 }, { Low: 0, High: 60 }, { Low: 0, High: 50 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 30, High: 100 }]);
IndicatorItemList[17] = new AssessmentItem(1401, 'Water Rates Based on Efficiency ', 'Base rates on efficiency of industrial/commercial water use. ', [1, 1, 1, 1, 1], [{ Low: 0, High: 60 }, { Low: 0, High: 60 }, { Low: 0, High: 60 }, { Low: 0, High: 60 }, { Low: 0, High: 50 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 30, High: 100 }]);
IndicatorItemList[18] = new AssessmentItem(1402, 'Require Industry to Provide Water Rights ', 'Require less efficient industry to provide their own water rights. ', [1, 1, 1, 1, 1], [{ Low: 0, High: 60 }, { Low: 0, High: 60 }, { Low: 0, High: 60 }, { Low: 0, High: 60 }, { Low: 0, High: 50 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 30, High: 100 }]);
IndicatorItemList[19] = new AssessmentItem(1500, 'Increase Power Generation Efficiency: ', 'Increase power efficiency ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[20] = new AssessmentItem(1501, 'Air Cooled Power Plants. ', 'Traditional power plants require water to cool them. Save water by switching to air-cooled power plants. ', [0, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 70 }, { Low: 30, High: 100 }]);
IndicatorItemList[21] = new AssessmentItem(1502, 'Use More Water from Large Lakes for Cooling ', 'Instead of using groundwater, use more water from large lakes for cooling. ', [0, 0, 1, 1, 0], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[22] = new AssessmentItem(1503, 'Use More Water from the Ocean for Cooling ', 'Instead of using groundwater or drinking water, states that are close to the ocean can use more water from the ocean for cooling. ', [1, 0, 0, 0, 0], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[23] = new AssessmentItem(1504, 'Switch to Less Water Intensive Power Plants ', 'Coal and nuclear powered power plants use water to cool their towers. Save water by switching to a less water intensive energy such as solar. ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 30, High: 100 }]);
IndicatorItemList[24] = new AssessmentItem(2000, 'Increase Water Supply: ', 'Increase Supply ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[25] = new AssessmentItem(2100, 'Use More Recycled Water: ', 'Most areas underutilize their recycled water. Using 90% of recycled resources increases water supply. ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 25, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[26] = new AssessmentItem(2102, 'Use Recycled Water for Industry ', 'Find industries that can use recycled water for cooling or production water instead of using surface water or groundwater. ', [1, 0, 1, 1, 0], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 25, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[27] = new AssessmentItem(2104, 'Use Recycled Water for Irrigation ', 'Use recycled water for large turf irrigation such as golf courses. ', [1, 0, 1, 1, 0], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 25, High: 100 }, { Low: 0, High: 60 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 30, High: 100 }]);
IndicatorItemList[28] = new AssessmentItem(2105, 'Adopt Toilet to Tap Legislation ', 'Recycled water can be cleaned to drinkable standards. Change laws to allow recycled water for drinking use. ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 25, High: 100 }, { Low: 0, High: 50 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 30, High: 100 }]);
IndicatorItemList[29] = new AssessmentItem(2200, 'Increase Surface Water Supply: ', 'Increase surface water supply ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[30] = new AssessmentItem(2201, 'Add Reservoirs ', 'By building reservoirs, water can be saved for times of drought. ', [0, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 20, High: 100 }, { Low: 20, High: 100 }, { Low: 20, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 20, High: 100 }]);
IndicatorItemList[31] = new AssessmentItem(2203, 'Watershed Management  ', 'Watershed management reduces pollution and sustains natural areas such as forests, to enhance and sustain quality and quantity of water. ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[32] = new AssessmentItem(2204, 'Cooperative Agreements to Share Resources and Risk ', 'Water is a shared resource vital for economic growth, food production, and human survival. Cooperative water agreements are important to reduce conflicts over water use. ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[33] = new AssessmentItem(2205, 'Climate Change Strategies ', 'Climate change creates a hotter world leading to less snow and decreasing water supply. Individuals can reduce water use in times of drought. Cities and towns can improve their water management and remove invasive non-native vegetation from riparian areas. ', [1, 0, 0, 0, 0], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[34] = new AssessmentItem(2300, 'Coastal Desalination: ', 'Coastal desalination ', [1, 0, 0, 0, 0], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 70 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 20, High: 100 }]);
IndicatorItemList[35] = new AssessmentItem(2301, 'Use Desalinated Ocean Water ', 'Salt water is desalinated to produce fresh water suitable for human consumption or irrigation. ', [1, 0, 0, 0, 0], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 50 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 20, High: 100 }]);
IndicatorItemList[36] = new AssessmentItem(2302, 'Use Desalinated Groundwater ', 'Industry and public drinking-water suppliers use desalinated groundwater to supplement or replace the use of freshwater. ', [1, 0, 0, 0, 0], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 70 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 20, High: 100 }]);
IndicatorItemList[37] = new AssessmentItem(2400, 'Increase Groundwater Use: ', 'Increase groundwater ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[38] = new AssessmentItem(2401, 'Drill More Wells and Drill Deeper ', 'More water can sometimes be found if more wells are drilled and drilled deeper. ', [0, 1, 1, 1, 1], [{ Low: 30, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 70 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 20, High: 100 }]);
IndicatorItemList[39] = new AssessmentItem(2403, 'Pass Groundwater Management Legislation ', 'Groundwater is a precious resource. States can pass groundwater management legislation to make sure groundwater does not run out. ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[40] = new AssessmentItem(2404, 'Recharge Groundwater ', 'Use water from rivers and effluent not needed to meet demand to move more water into groundwater aquifers. ', [1, 1, 1, 1, 1], [{ Low: 0, High: 60 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 20, High: 100 }]);
IndicatorItemList[41] = new AssessmentItem(2405, 'Use Permeable Surfaces for Parking Lots and Driveways ', 'By using concrete that absorbs water instead of washing it away, water can trickle down for collection. ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[42] = new AssessmentItem(2406, 'Cooperative Agreements to Share Resources and Risk: ', 'Water is a shared resource vital for economic growth, food production, and human survival. Cooperative water agreements are important to reduce conflicts over water use. ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[43] = new AssessmentItem(2500, 'Use More Large Lake Water: ', 'Use more lake water ', [0, 0, 1, 1, 0], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[44] = new AssessmentItem(2501, 'Use Lake Water to Meet Public Water System Demand ', 'Lake water can be used for community water supplies and water treatment systems. ', [0, 0, 1, 1, 0], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
IndicatorItemList[45] = new AssessmentItem(2502, 'Use Lake Water to Cool Power Plants ', 'Lake water can be used to cool thermoelectric power plants. ', [0, 0, 1, 1, 0], [{ Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
//IndicatorItemList[46] = new AssessmentItem(2600, 'Environment: ', 'Environment ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 20, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
//IndicatorItemList[47] = new AssessmentItem(2601, 'Dedicated Environmental Flows for Critical Habitat ', 'When water is left in the rivers and lakes for environmental use, this can help threatened or endangered species. ', [1, 1, 1, 1, 1], [{ Low: 20, High: 100 }, { Low: 20, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 70 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
//IndicatorItemList[48] = new AssessmentItem(2602, 'Enhanced Wetlands ', 'More fresh water can be naturally generated in wetlands which helps both the environment and people. ', [1, 1, 1, 1, 1], [{ Low: 0, High: 100 }, { Low: 20, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
//IndicatorItemList[49] = new AssessmentItem(2603, 'Manage Flood Flows for Critical Environmental Needs ', Good flood control management sustains freshwater ecosystems and the livelihoods that depend on them. ', [1, 1, 1, 1, 1], [{ Low: 20, High: 100 }, { Low: 20, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 70 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }, { Low: 0, High: 100 }]);
