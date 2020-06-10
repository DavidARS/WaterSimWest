// Xpan Div Library
// Quay 2/21/16
// Requires jQuery
// 
// Documentation
// This library enables expansion and contraction of div contents based on clicking the div
// 1) Div must have a class name begining with "XpanDiv"
// 2) 

var ExpandIconSrc = "images/blue cross.png";//"images/blue cross.jpg";//"images/blue expand.jpg";
var CollapseIconSrc = "images/blue mius.png";// "images/blue mius.jpg";// "images/blue collapse.jpg";
var EmptyIconSrc = "images/blue dot.png"; //"images/blue dot.jpg";
var XpanDiv = "XpanDiv";
var XpanState = "EXPANDED";
var XpanOnlyOne = "onlyone";
var XpanOnlyOneForce = "true";
var XpanAddIcon = true;
var XpanIMGplace = "left";
var XpanIMGHeight = "30";

//===============================================================
function CheckIfXpanChildren(ParentDiv) {
    test = false;
    if (ParentDiv.childNodes) {
        if (ParentDiv.childNodes.length > 0) {
            children = ParentDiv.childNodes;
            $(children).each(function () {
                if (this.className) {
                    classname = this.className;
                    if (classname.indexOf(XpanDiv) > -1) {
                        test = true;
                    }
                }
            });
            
        }
    }
    return test;

}

//====================================================================
// set up the Xpan system
function XpanDivSetup(aTargetDiv) {
    
    if (!aTargetDiv) {
        // find all divs of class XPanDiv
        
        $("[class^='XpanDiv']").each(function () {
            // If add icon, do so
            if (XpanAddIcon) {
                // make sure a Xpan IMG is not already added
                addIMG = true;
                theDiv = this;
                $(theDiv).find("*").each(function () {
                    if (this.id == theDiv.id + "IMG") {

                        addIMG = false;
                    }
                });
                // if addIMG, add the image
                objectforimg = theDiv;
                if (addIMG) {
                    // see if non div tag option is first child
                    firstObject = theDiv.firstChild;
                    // if it there test if uses tag
                    if (firstObject != null) {
                        if (firstObject.tagName) {
                            // now skip it if it is a DIV tag
                            if (firstObject.tagName != "DIV") {
                                objectforimg = firstObject;
                            }
                        }
                    }

                    // build new imag tag
                    useIcon = EmptyIconSrc;
                    if (CheckIfXpanChildren(this)) {
                        useIcon = ExpandIconSrc;
                    }
                    else {
                        useIcone = EmptyIconSrc;
                    }
                    newimgObject = "<img id='" + this.id + "IMG" + "' src='" + useIcon + "' height='" + XpanIMGHeight + "' style=' vertical-align: middle margin: 10px 0px 0px 0px';/>";
                    // place left or right
                    if (XpanIMGplace == "left") {
                        objectforimg.innerHTML = newimgObject + objectforimg.innerHTML;
                    }
                    else {
                        objectforimg.innerHTML = objectforimg.innerHTML + newimgObject;
                    }
                }

            }

            // trap the click event
            $(this).click(function (event) {
                if (event.stopPropagation) {
                    event.stopPropagation();
                };
                changeState(this)
            });
            // Create State attribute, set to true to allow callapse
            this.setAttribute(XpanState, true);
            // start with everything collapsed
        });

        // OK, collapse all of this
        $("[class^='XpanDiv']").each(function () {
            CollapseDiv(this);
        });
    } else {
        // find all divs of class XPanDiv
        $("#"+aTargetDiv).find("[class^='XpanDiv']").each(function () {
            // If add icon, do so
            if (XpanAddIcon) {
                // make sure a Xpan IMG is not already added
                addIMG = true;
                theDiv = this;
                $(theDiv).find("*").each(function () {
                    if (this.id == theDiv.id + "IMG") {
                        addIMG = false;
                    }
                });
                // if addIMG, add the image
                objectforimg = theDiv;
                if (addIMG) {
                    // see if non div tag option is first child
                    firstObject = theDiv.firstChild;
                    // if it there test if uses tag
                    if (firstObject != null) {
                        if (firstObject.tagName) {
                            // now skip it if it is a DIV tag
                            if (firstObject.tagName != "DIV") {
                                objectforimg = firstObject;
                            }
                        }
                    }
                }
                // build new imag tag
                useIcon = EmptyIconSrc;
                if (CheckIfXpanChildren(this)) {
                    useIcon = ExpandIconSrc;
                }
                else {
                    useIcone = EmptyIconSrc;
                }
                newimgObject = "<img id='" + this.id + "IMG" + "' src='" + useIcon + "' height='" + XpanIMGHeight + "' style=' vertical-align: middle margin: 10px 0px 0px 0px';/>";
                // place left or right
                if (XpanIMGplace == "left") {
                    objectforimg.innerHTML = newimgObject + objectforimg.innerHTML;
                }
                else {
                    objectforimg.innerHTML = objectforimg.innerHTML + newimgObject;
                }


            }

            // trap the click event
            $(this).click(function (event) {
                if (event.stopPropagation) {
                    event.stopPropagation();
                };
                changeState(this)
            });
            // Create State attribute, set to true to allow callapse
            this.setAttribute(XpanState, true);
            // start with everything collapsed
        });
        // OK, collapse all of this
        $("#" + aTargetDiv).find("[class^='XpanDiv']").each(function () {
            CollapseDiv(this);
        });

    }
    
}

function JustShownNonExpanChildren(childXpanDiv) {
    if (childXpanDiv.childNodes) {
        morechildren = childXpanDiv.childNodes;
        $(morechildren).each(function () {
            if (!isXpanDiv(this)) {
                $(this).show();
            }
        });
    }
    $(childXpanDiv).show();
}

// expand all DIVs within theDIV
function ExpandDiv(theDiv) {
        // check if this Xpan Div
        if (theDiv.hasAttribute(XpanState)) {
            // get the sate
            state = theDiv.getAttribute(XpanState);
            // do only if false
            if (state = "false") {
                // set the state
                theDiv.setAttribute(XpanState, "true");
                // get image
                $(theDiv).show();
            
            // show all child divs
                if (theDiv.childNodes) {
                    children = theDiv.childNodes;
                    $(children).each(function () {
                        if (isXpanDiv(this)) {
                            JustShownNonExpanChildren(this);
                        } else {
                            $(this).show();
                        }
                    });
                }
                // get img if there
                thesrc = "";
                if (CheckIfXpanChildren(theDiv)) {
                    thesrc = ExpandIconSrc;
                } else {
                    thesrc = EmptyIconSrc;
                }
                theIMG = document.getElementById(theDiv.id + "IMG");
                // if there, change the source
                if (theIMG) {
                    theIMG.src = CollapseIconSrc;
                }
                test = theIMG.src;
                test2 = theIMG.href;
                whatthehell = 1;

            }
        }
}

function isXpanDiv(checkDiv)
{
    result = false;
    // get the class name if there
    if (checkDiv.className) {
        // see if starts with "XpanDiv"
        if (checkDiv.className.indexOf(XpanDiv) == 0) {
                    result = true;
        }
    }
     return result;
}


function CollapseAllDescendentDivs(theChildDiv) {
    // check if their is a childnodes field
    if (theChildDiv.childNodes) {
        // get the children field
        children = theChildDiv.childNodes;
        // if their are some children
        if (children.length) {
            if (children.length > 0) {
                // for each child
                $(children).each(function () {
                    // check if this is an XpanDiv
                    if (isXpanDiv(this)) {
                        // if so collapse this
                        CollapseDiv(this);
                    }
                    // hide this child
                    $(this).hide();
                });
            }
        }
    }
    $(theChildDiv).hide();

}
// collapse all the DIVs within in theDIV
function CollapseDiv(theDiv) {
    if (theDiv.className) {
        if (theDiv.className.indexOf(XpanDiv) == 0) {
            if (theDiv.hasAttribute) {
                // check of Xpan Div
                if (theDiv.hasAttribute(XpanState)) {
                    // get the sate
                    state = theDiv.getAttribute(XpanState);
                    // do only if true
                    if (state = "true") {
                        // set the state
                        theDiv.setAttribute(XpanState, "false");
                        // get img if there
                        thesrc = "";
                        theIMG = document.getElementById(theDiv.id + "IMG");
                        //// set to source
                        if (theIMG) {
                            if (CheckIfXpanChildren(theDiv)) {
                                theIMG.src = ExpandIconSrc;
                            } else {
                                theIMG.src = EmptyIconSrc;
                            }
                        }
                        // hide all child Xpan divs
                        if (theDiv.childNodes) {
                            children = theDiv.childNodes;
                            $(children).each(function () {
                                if (this.className) {
                                    if (this.className.indexOf(XpanDiv) == 0) {
                                        CollapseDiv(this);
                                        CollapseAllDescendentDivs(this);
                                    }
                                }
                            });
                        }
                    }
                }
            }
        }
    }
}

function GetBaseXpanClassName(theClassName) {
    index1 = theClassName.indexOf('-');
    baseClass = theClassName.substring(0, index1);
    tempSub = theClassName.substring(index1 + 1, theClassName.length);
    index2 = tempSub.indexOf("-");
    if (index2 == -1) {
        return theClassName;
    }
    else {
        temp = tempSub.substring(0, index2)
        return baseClass + "-"+temp;
    }

}
function CollapseAllOthers(theDivCollapsing) {
    // dothis unless there is a reason not to
    dothis = true;
    // first test to see of this DIV has OnlyOne set to false
    test = theDivCollapsing.dataset[XpanOnlyOne];
    // if defined, then see if it is defined false
    if (test) {
        if (test == XpanOnlyOneForce) {
            if (theDivCollapsing.className) {
                tempClassName = GetBaseXpanClassName(theDivCollapsing.className);
                $("[class^='" + tempClassName+"']").each(function () {
                    // if not the passed theDiv
                    if (this.id != theDivCollapsing.id) {
                        // test if you can close, and if so, close it
                        test = this.dataset[XpanOnlyOne];
                        if (test) {
                            if (test == XpanOnlyOneForce) {
                                CollapseDiv(this);
                            }
                        }
                        else {
                            CollapseDiv(this);
                        }
                    }
                });
            }
//            dothis = false;
        }
    }
    // OK, do this?
    //if (dothis) {
    //    // for each object of the same class name as this DIv
    //    if (theDiv.className) {
    //        tempClassName = GetBaseXpanClassName(theDiv.className);
    //        $("[class^='" + theDiv.className + "']").each(function () {
    //            // if not the passed theDiv
    //            if (this.id != theDiv.id) {
    //                // test if you can close, and if so, close it
    //                test = this.dataset[XpanOnlyOne];
    //                if (test) {
    //                    if (test == XpanOnlyOneForce) {
    //                        CollapseDiv(this);
    //                    }
    //                }
    //            }
    //        });
    //    }
    //}
}

// change the state of the DIV (object), if EXPANDED true, then collapse, if EXPANDED false the expand
function changeState(object) {
    if (object.hasAttribute(XpanState))
    {
        Xstate = object.getAttribute(XpanState);
        if (Xstate=="false") {
            ExpandDiv(object);
            // check if OneOnly collapses need to occur
            CollapseAllOthers(object);
        }
        else {
            CollapseDiv(object);
            //nothing special here
        }
    }
}

var FUCKTHIS = 0;
// Setup document after load.
///$(document).ready(function () { XpanDivSetup(); });