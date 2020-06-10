var pbarCnt = 1;
function add_qPbar(thedoc,divid, startvalue, maxvalue, width, height) {
    var result = "";
    var targetdiv = thedoc.getElementById(divid);
    if (maxvalue > 0) {
        if (targetdiv) {
            var newqPbar = document.createElement("progress");
            var newSpan = document.createElement("div");
            if (newqPbar && newSpan) {
                newqPbar.value = startvalue;
                newqPbar.max = maxvalue;
                var anid = "qPbar" + pbarCnt.toString();
                newqPbar.id = anid;
                newqPbar.classList.add("qPbar");
                var thewidth = "";
                var theheight = "";
                if (width) {
                    if (typeof (width) == "number") {
                        thewidth += width.toString() + "px";
                    }
                    else {
                        thewidth += width;
                    }
                }
                newqPbar.style.width = thewidth;

                if (height) {
                    if (typeof (height) == "number") {
                        theheight += height.toString() + "px";
                    }
                    else {
                        theheight += height;
                    }
                }
                newqPbar.style.height = theheight;
                newSpan.id = anid + "Div";
                newSpan.classList.add("qPbarSpan");
                newSpan.style.textAlign = "center";
                newSpan.style.width = thewidth;
                var pctval = (startvalue / maxvalue) * 100;
                newSpan.innerHTML = " " + pctval.toString() + " %";
                targetdiv.appendChild(newqPbar);
                targetdiv.appendChild(newSpan);
                result = newqPbar.id;
            }
        }
    }
    return result;
}

function set_qPbar(qPbarId, value) {
    var theqPbar = thedoc.getElementById(qPbarId);
    var theqPbarSpan = thedoc.getElementById(qPbarId + "Div");
    if (theqPbar && theqPbarSpan) {
        if (theqPbar.tagName == "PROGRESS") {
            theqPbar.value = value;
            var maxval = theqPbar.max;
            var pct = 100;
            if (maxval > 0) {
                pct = (value / maxval) * 100;
                theqPbarSpan.innerHTML = " " + pct.toString() + " %";
            }
            else {
                theqPbarSpan.innerHTML = " Error Max<=0";
            }
        }
    }
}

