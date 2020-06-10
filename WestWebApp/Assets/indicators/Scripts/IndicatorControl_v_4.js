// <reference path="Assets/Scripts/IndicatorControl_v_4.js" />
// Indicator Control CLass
// Requires JQuery be load before this class is used
//
// Last write was 05.23.14,10.01.14,10.07.14 das

function Frame(left, top, width, height) {
    this.left = left;
    this.top = top;
    this.width = width;
    this.height = height;
    this.reset = function () {
    }
    this.right = this.left + (this.width - 1);
    this.bottom = this.top + (this.height - 1);

    this.reset = function () {
        this.right = this.left + (this.width - 1);
        this.bottom = this.top + (this.height - 1);

    }
}

function IndicatorColors(SkyColor, WaterColor, CloseColor, NearColor, MidColor, FarColor, SunColor, EarthColor, WallColor, PeopleColor, TreeColor, GrassColor, CactusColor, BuildingColor, TableTopColor) {
    this.Sky = SkyColor;
    this.Water = WaterColor;
    this.Near = NearColor;
    this.Mid = MidColor;
    this.Far = FarColor;
    this.Sun = SunColor;
    this.Earth = EarthColor;
    this.Wall = WallColor;
    this.People = PeopleColor;
    this.Tree = TreeColor;
    this.Grass = GrassColor;
    this.Cactus = CactusColor;
    this.Building = BuildingColor;
    this.TableTop = TableTopColor;
          
}
function getDomHeightWithID(anID) {
    var theheight = 0;
    var theelement = document.getElementById(anID);
    if (theelement != undefined) {
        if (theelement.clientHeight) {
            theheight = theelement.clientHeight;
        }
    }
    return theheight;
}
function getDomWidthWithID(anID) {
    var thewidth = 0;
    var theelement = document.getElementById(anID);
    if (theelement != undefined) {
        if (theelement.clientWidth) {
            thewidth = theelement.clientWidth;
        }
    }
    return thewidth;
}
///-------------------------------------------------------------------------------------------------
/// <summary>   Indicator control. </summary>
///
/// <param name="parameter1">   The id of the division to insert control into. </param>

/// <param name="parameter2">   The type of idnicator "GWP"= groundwater %, "ENV" = Environment "AGR" = Agriculture "PWC" = Personal / Comercial Water Consumption, "AWS" assuref water supply compliance. </param> <remark>Maychange these</remark>
/// <param name="parameter3">   The id for this control. </param>
/// <param name="parameter4">   The width of the Control. </param>
/// <param name="parameter5">   The Height of the Control. </param>///
/// <returns>   . </returns>
///-------------------------------------------------------------------------------------------------



function IndicatorControl(divId, anIndicatorType, ControlId, Width, Height) {
    console.log('IndicatorControlv_v4')
    //var myHeight = 100;
    //var myWidth = 100;
    this.id = ControlId
    if (Width == undefined) {
        var aWidth = getDomWidthWithID(divId);
        if (aWidth > 0) {
            this.width = aWidth;
        } else {
           this.width =myWidth;
        }
    } else {
        this.width = Width;
    }
    if (Height == undefined) {
            var aHeight = getDomHeightWithID(divId);
            if (aHeight > 0) {
                this.height = aHeight;
            } else {
            this.height = myHeight;
        }
    } else {
        this.height = Height;
    }

    if (this.height < 50) { this.height = 50; this.width = 50; }
    if (this.width > this.height) { this.width = this.height; }
    this.IndicatorType = anIndicatorType;
    this.DivID = divId;
    // QUAY ADDED FOr report support 1/2/14
    this.Description = "";

    // why is this here? QUAY 1/2/14
    this.DivID_Old = divId + "_OLD";
    this.MinValue = 0; // default
    this.MaxValue = 150; // default
    this.value = 0;  // 0 to 100 valid
    this.value_old = 0;
    //this.fudgecnt = 0;

    // This is a function used to set the value of the control and redraw
    this.SetValue = function (value) {
        // check if valid range
        if ((value >= this.MinValue) && (value <= this.MaxValue)) {
            ////set old value
            this.value_old = this.value;
                 // set the value
                this.value = value;
            // paint the control
            this.Paint();
        }
    };
    // The frame offset
    this.offsetleft = 4;
    this.offsetright = 7;
    this.offsettop = 4;
    this.offsetbottom = 4;

    // create Frame
    this.Frame = new Frame(this.offsetleft, this.offsettop, this.width - (this.offsetleft + this.offsetright), this.height - (this.offsettop + this.offsetbottom));
    // set background
    this.background = "white";
    ///-------------------------------------------------------------------------------------------------
    /// <summary> Resize the Control </summary>
    /// <param name="parameter1">   The first parameter. </param>
    /// <param name="parameter2">   The second parameter. </param>
    ///-------------------------------------------------------------------------------------------------
    this.resize = function (width, height) {
        // set the control
        this.width = width;
        this.height = height;
        // set the canvas
        this.canvas.width = width;
        this.canvas.height = height;
        // reset Frame
        this.Frame.width = this.width - (this.offsetleft + this.offsetright);
        this.Frame.height = this.height - (this.offsettop + this.offsetbottom);
        this.Frame.reset(); 
        // reinitialize
        InitializeIndicator(this);
        // draw it
        this.Paint();
    }
    // onclick event handler
    this.oncLick = function (event) {
    }
    // Paint the object
    this.Paint = function () {
        RaiseIndicator(this);
        DrawIndicator(this);
    }

    // constructor Code
//    this.canvas = canvas;
    this.canvas = document.createElement("Canvas");
    this.canvas.width = this.width;
    this.canvas.height = this.height
    this.canvas.id = this.DivID + this.id + "Canvas";
    this.canvas.style.position = "absolute";
    this.CT = this.canvas.getContext("2d");
    this.canvas.onclick = this.onclick;
    // Set defualt colors
    this.Colors = new IndicatorColors("lightskyblue", "royalblue", "darkgreen", "darkolivegreen", "darkkhaki", "khaki", "gold", "burlywood", "peachpuff", "mediumblue", "yellowgreen", "lawngreen", "seagreen", "peru","#777777");
    // call the indicators initialization code
    InitializeIndicator(this);
    // draw thw indicator
    DrawIndicator(this);
    // add it to the div element
    var TheDiv = document.getElementById(this.DivID);
    if (TheDiv != undefined)
    {
        if (TheDiv.appendChild) {
            TheDiv.appendChild(this.canvas);
        }
    }

}

///-------------------------------------------------------------------------------------------------
/// <summary>   Initializes the indicator. </summary>
///
/// <param name="parameter1">   an IndicatorControl Object. </param>
///
/// <returns>   . </returns>
///-------------------------------------------------------------------------------------------------

function InitializeIndicator(IndControl) {
    var ctType = IndControl.IndicatorType;
  
    //
    switch (ctType) {
        case "GWP":
            InitializeGWP(IndControl);
            break;
        case "ENV":
            InitializeENV(IndControl);
            break;
        case "AGR":
            InitializeAGR(IndControl);
            break;
        case "PWC":
            InitializePWC(IndControl);
            break;
        case "AWS":
            InitializeAWS(IndControl);
            break;
    }
}

///-------------------------------------------------------------------------------------------------
/// <summary>   Draw indicator. </summary>
///
/// <param name="parameter1">   n IndicatorControl object. </param>
///
/// <returns>   . </returns>
///-------------------------------------------------------------------------------------------------

function DrawIndicator(IndControl) {
    var ctType = IndControl.IndicatorType;
    switch (ctType) {
        case "GWP":
            DrawGWP(IndControl);
            break;
        case "ENV":
            DrawENV(IndControl);
            break;
        case "AGR":
            DrawAGR(IndControl);
            break;
        case "PWC":
            DrawPWC(IndControl);
            break;
        case "AWS":
            DrawAWS(IndControl);
            break;
        //case "POW":
        //    DrawAWS(IndControl);
        //    break;
    }
    // Find the div's (h3) that contain the old values (for each indicator) and write to each section
    // 09.29.14 DAS (with much help from RAY)
    var targetdiv = document.getElementById(IndControl.DivID_Old);
    if(targetdiv != undefined)
    {
        if (0 < IndControl.value_old.toString()) {
            targetdiv.innerHTML = "(" + IndControl.value_old.toString() + ")";
        }
        else {
            
                targetdiv.innerHTML = " ... "
            
        }
    }
}


///-------------------------------------------------------------------------------------------------
/// <summary>   If there is an offset, this draws a frame with a shadow </summary>
///
/// <param name="parameter1">   The first parameter. </param>
///
/// <returns>   . </returns>
///-------------------------------------------------------------------------------------------------

function RaiseIndicator(TheControl) {
    // get variables
    var IC = TheControl.CT;
    indWidth = TheControl.width;
    indHeight = TheControl.height;
    frameHeight = TheControl.Frame.height;
    frameWidth = TheControl.Frame.width;
    frameTop = TheControl.Frame.top;
    frameLeft = TheControl.Frame.left;
    // draw background across entire control
    IC.fillStyle = "transparent"; //TheControl.background;
    IC.fillRect(0, 0, indWidth, indHeight)
    // Now draw a reectangle around frame
    IC.lineWidth = 1;
    IC.strokeStyle = "black";
    IC.fillStyle = TheControl.background;
    IC.rect(frameLeft,frameTop,frameWidth,frameHeight);
    // now fill it with a shadow
    IC.shadowColor = '#555555';
    IC.shadowBlur = 5;
    IC.shadowOffsetX = TheControl.offsetright - 1;
    IC.shadowOffsetY = TheControl.offsetbottom - 1;;
    // okk fill with a shadow
    IC.fill();
    // reset the shadow parametes
    IC.shadowColor = '';
    IC.shadowBlur = 0;
    IC.shadowOffsetX = 0//IndOffset-1;
    IC.shadowOffsetY = 0//IndOffset-1;

}
//==================================================
// Base Background
// ===================================================

function drawBaseBackground(IndControl) {
    var CT = IndControl.CT;
    var TheValue = IndControl.value;
    var MyWidth = IndControl.Frame.width;
    var MyHeight = IndControl.Frame.height;
    var MyLeft = IndControl.Frame.left;
    var MyTop = IndControl.Frame.top;
    SkyHeight = (MyHeight * 0.375) + MyTop;
    Horizon = (MyHeight * 0.375) + 5 + MyTop;
    // draw sky
    drawBaseSky(CT, MyLeft, MyTop, MyWidth, MyHeight, IndControl.Colors.Sky, SkyHeight);

    // draw Far Mtn
    drawBaseFarMountain(CT, MyLeft, MyTop, MyWidth, MyHeight, IndControl.Colors.Far, Horizon);

    // draw Mid Mtn
    drawBaseMidMountain(CT, MyLeft, MyTop, MyWidth, MyHeight, IndControl.Colors.Mid, Horizon);

    // draw near mtn
    drawBaseNearMountain(CT, MyLeft, MyTop, MyWidth, MyHeight, IndControl.Colors.Near, Horizon);

    // draw sun
    drawBaseSun(CT,MyLeft, MyTop, MyWidth, MyHeight, IndControl.Colors.Sun, Horizon);
    
}
function drawBaseSun(CT, MyLeft, MyTop, MyWidth, MyHeight, MyColor, SkyHeight, Horizon) {

    CT.strokeStyle = MyColor;
    CT.fillStyle = MyColor;
    CT.beginPath();
    var radius = 7;
    CT.arc(15 * MXinc+2, 35 * MYinc, radius, 0, 2 * Math.PI, false);
    CT.stroke();
    CT.fill();
    CT.closePath();
}
function drawBaseSky(CT, MyLeft, MyTop, MyWidth, MyHeight, MyColor, SkyHeight, Horizon) {
    CT.strokeStyle = MyColor;
    CT.fillStyle = MyColor;
    CT.fillRect(MyLeft, MyTop, MyWidth, SkyHeight);
}
function drawBaseFarMountain(CT, MyLeft, MyTop, MyWidth, MyHeight, MyColor, Horizon) {
    CT.strokeStyle = MyColor;
    CT.fillStyle = MyColor;
 //   TBaseX = IndWidth / 2 + IndOffset;
    CT.beginPath();
    MXinc = MyWidth / 100;
    MYinc = Horizon / 100;
    CT.moveTo(MyLeft, Horizon);
    CT.lineTo(15 * MXinc, 75 * MYinc);
    CT.lineTo(18 * MXinc, 79 * MYinc);
    CT.lineTo(25 * MXinc, 82 * MYinc);
    CT.lineTo(30 * MXinc, 100 * MYinc);
    CT.lineTo(5 * MXinc, Horizon);
    CT.stroke();
    CT.fill();
    CT.closePath();
}
function drawBaseMidMountain(CT, MyLeft, MyTop, MyWidth, MyHeight, MyColor) {
    CT.strokeStyle = MyColor;
    CT.fillStyle = MyColor;
    MXinc = MyWidth / 100;
    MYinc = Horizon / 100;
    CT.beginPath();
    CT.moveTo(17 * MXinc, Horizon);
    CT.lineTo(25 * MXinc, 85 * MYinc);
    CT.lineTo(40 * MXinc, 45 * MYinc);
    CT.lineTo(45 * MXinc, 50 * MYinc);
    CT.lineTo(50 * MXinc, 47 * MYinc);
    CT.lineTo(55 * MXinc, 38 * MYinc);
    CT.lineTo(85 * MXinc, Horizon);
    CT.lineTo(25 * MXinc, Horizon);
    CT.stroke();
    CT.fill();
    CT.closePath();
}
function drawBaseNearMountain(CT, MyLeft, MyTop, MyWidth, MyHeight, MyColor, Horizon) {
    CT.strokeStyle = MyColor;
    CT.fillStyle = MyColor;
    MXinc = MyWidth / 100;
    MYinc = Horizon / 100;
    CT.beginPath();
    CT.moveTo(40 * MXinc, Horizon);
    CT.lineTo(60 * MXinc, 75 * MYinc);
    CT.lineTo(80 * MXinc, 35 * MYinc);
    CT.lineTo(85 * MXinc, 35 * MYinc);
    CT.lineTo(88 * MXinc, 30 * MYinc);
    CT.lineTo(92 * MXinc, 33 * MYinc);
    CT.lineTo(96 * MXinc, 37 * MYinc);


    CT.lineTo((MyLeft+MyWidth), 50 * MYinc);

    CT.lineTo((MyLeft + MyWidth), Horizon);
    CT.lineTo(40 * MXinc, Horizon);
    CT.stroke();
    CT.fill();
    CT.closePath();
    // shoulder of near mountain
    //CT.strokeStyle = "#000000";
    //CT.fillStyle = "#000000";
    //CT.beginPath();
    //CT.moveTo(80 * MXinc, Horizon);
    //CT.lineTo(75 * MXinc, 73 * MYinc);
    //CT.lineTo(87 * MXinc, Horizon);
    //CT.lineTo(80 * MXinc, Horizon);
    //CT.stroke();
    //CT.fill();
    //CT.closePath();
    
}
//
//=====================================================
// Assured Water Supply
//=====================================================
// 
function DrawAWS(IndControl) {
    var CT = IndControl.CT;
    var TheValue = IndControl.value;
    var MyWidth = IndControl.Frame.width;
    var MyHeight = IndControl.Frame.height;
    var MyLeft = IndControl.Frame.left;
    var MyTop = IndControl.Frame.top;
    var Horizon = (MyHeight * 0.375) + 5 + MyTop;
    drawBaseBackground(IndControl);
    drawAWSBackground(CT, MyLeft, MyTop, MyWidth, MyHeight, Horizon, IndControl.Colors.Earth, IndControl.Colors.Water);
    drawPeople(CT, MyLeft, MyTop, MyWidth, MyHeight, Horizon, IndControl.Colors.People, TheValue);
}

var PeopleColors = new Array();
PeopleColors[0] = "lightpink";
PeopleColors[1] = "sienna";
PeopleColors[2] = "mistyrose";
PeopleColors[3] = "peru";
PeopleColors[4] = "sandybrown";
PeopleColors[5] = "#80220d";

var assetsPath = "Assets/indicators/";


function point(X, Y) {
    this.X = X;
    this.Y = Y;
}

var PeopleXY = new Array();
var PeopleCreated = false;
var PeopleN = 50;
var PeopleSpaceWidth = 20;
var PeopleSpaceHeight = 10;
var PeopleHeadColors = new Array();

function InitializeAWS(IndControl) {
    if (!PeopleCreated) {
        for (i = 0; i < PeopleN; i++) {
            RelX = Math.round(PeopleSpaceWidth * Math.random());
            RelY = Math.round(PeopleSpaceHeight * Math.random());
            PeopleXY[i] = new point(RelX, RelY);
            HeadColorindex = Math.round(((PeopleColors.length) * Math.random()));
            if (HeadColorindex == PeopleColors.length) HeadColorindex = 0;
            HeadColor = PeopleColors[HeadColorindex];
            PeopleHeadColors[i] = HeadColor;
        }
        PeopleXY.sort(function (a, b) { return a.Y - b.Y; });
    }
}


function drawPerson(CT,X,Y,Size,TheColor,TheHeadColor,Male) {
    Sizeinc = Size / 15;
    HeadX = X + (Sizeinc*6);
    HeadY = Y + (Sizeinc*6);
    ArcSize = Sizeinc * 6

    // draw body
    CT.beginPath();
    CT.strokeStyle = TheColor;
    CT.fillStyle = TheColor;
    CT.moveTo(HeadX, HeadY);
    // if male little husky
    if (Male) {
        CT.lineTo(HeadX - (Sizeinc * 3), HeadY);
    }
    CT.lineTo(HeadX - (Sizeinc * 5), HeadY + (Sizeinc * 12));
    CT.lineTo(HeadX + (Sizeinc * 5), HeadY + (Sizeinc * 12));
    if (Male) {
        CT.lineTo(HeadX + (Sizeinc * 3), HeadY);
    }
    CT.lineTo(HeadX, HeadY);
    CT.closePath();
    CT.stroke();
    CT.fill();
    // draw their cute little heads!
    CT.strokeStyle = TheHeadColor;
    CT.fillStyle = TheHeadColor;
    CT.beginPath();
    CT.arc(HeadX, HeadY, ArcSize, 0, 2 * Math.PI);
    CT.closePath();
    CT.stroke();
    CT.fill();
    // now some eyes
    CT.strokeStyle = "black";
    CT.lineWidth = Sizeinc + 1;
    CT.beginPath();
    CT.moveTo(HeadX - (3*Sizeinc), HeadY - (2*Sizeinc));
    CT.lineTo(HeadX - Sizeinc, HeadY - (2 * Sizeinc));
    CT.moveTo(HeadX + Sizeinc, HeadY - (2 * Sizeinc));
    CT.lineTo(HeadX + (3 * Sizeinc), HeadY - (2 * Sizeinc));
    CT.closePath();
    CT.stroke();



    }
    
function drawPeople(CT, MyLeft, MyTop, MyWidth, MyHeight, Horizon, BodyColor, TheValue){
    // get ground height
    GroundHeight = (MyHeight + MyTop) - Horizon;
    // set size of people
    YSize = GroundHeight / 6;
    // where they start
    BaseY = Horizon - ((YSize / 5) * 3);
    // how many to draw
    PN = (TheValue / 2);
    // OK setup to use PeopleXY space
    XspaceSize = ((MyWidth-YSize) / PeopleSpaceWidth);
    YspaceSize = (GroundHeight - (GroundHeight / 3)) / PeopleSpaceHeight;
    // set up the index and flip
    pindex = 0;
    flip = true
    // lop through people, get rando location and color, draw thwm
    for (i = 1; i < PN  ; i++) {

            TheX = (PeopleXY[pindex].X *XspaceSize)+MyLeft;
            TheY = (PeopleXY[pindex].Y * YspaceSize) + BaseY;
            MyHeadColor = PeopleHeadColors[pindex];
            drawPerson(CT, TheX, TheY, YSize, BodyColor, MyHeadColor, flip);
            pindex++;
            if (pindex == PeopleN) pindex = 0;
            flip = !flip;
        }
    // ok now put down the text
    
    var PCTStr = TheValue.toString();

    
    fontsize = Math.round(MyHeight / 5);
    fontstr = fontsize.toString() + "px Arial"
    CT.font = fontstr;
    //CT.fillStyle = BodyColor;
    CT.fillStyle = "#990033";
    CT.textAlign = "right";
    CT.fillText(PCTStr, MyWidth+MyLeft - fontsize , MyHeight+MyTop-2);
    PCTStr = "Yrs";
    fontsize = Math.round(MyHeight / 8);
    fontstr = fontsize.toString() + "px Arial"
    CT.font = fontstr;
    CT.fillStyle = BodyColor;
    CT.textAlign = "right";
    CT.fillText(PCTStr, MyWidth + MyLeft, MyHeight + MyTop - 2);
    
    // 
}
///-------------------------------------------------------------------------------------------------
/// <summary>   Draw the ws background. (void)</summary>
///
/// <param name="parameter1">   The canvas context. </param>
/// <param name="parameter2">   left side of frame. </param>
/// <param name="parameter3">   top of frame. </param>
/// <param name="parameter4">   width of frame. </param>
/// <param name="parameter5">   Height of frame. </param>
///-------------------------------------------------------------------------------------------------

function drawAWSBackground(CT, MyLeft, MyTop, MyWidth, MyHeight, Horizon, EarthColor, WaterColor) {
    // Draw Earth
    CT.strokeStyle = EarthColor;
    CT.fillStyle = EarthColor;
    MyBottom = MyHeight + MyTop;
    MyRight = MyWidth + MyLeft;
    GroundHeight =  (MyHeight+MyTop)-Horizon;
    CT.fillRect(MyLeft, Horizon, MyWidth,GroundHeight);
    // draw water
   
    CT.strokeStyle = WaterColor;
    CT.fillStyle = WaterColor;
    //CT.globalAlpha = 0.5;
    WaterY = Horizon + ((GroundHeight / 6) * 3);
    WaterX = (MyWidth / 3) + MyLeft;
    CT.beginPath();
    CT.moveTo(MyLeft, MyBottom);
    AngleX = (MyWidth/4)+MyLeft;
    AngleY = WaterY;
    CT.quadraticCurveTo(AngleX, AngleY, MyRight, WaterY);
    //WaterY += GroundHeight / 20;
    WaterY += GroundHeight / 35;
    CT.lineTo(MyRight, WaterY);
    AngleX = (MyWidth / 4)+WaterX;
    AngleY = WaterY;
    CT.quadraticCurveTo(AngleX, AngleY, WaterX, MyBottom);
    CT.lineTo(MyLeft, MyBottom);
    CT.closePath();
    CT.stroke();
    CT.fill();




}


//
//=====================================================
// Personal Water Consumption
// =================================================
function DrawPWC(IndControl) {
    var CT = IndControl.CT;
    // QUAY EDIT TO DRAW GPCD SHOWER
    //var TheValue = IndControl.value;
    var TheValue = (IndControl.value / 240) * 100;
    if (TheValue > 100) { TheValue = 99; }
    else { 
        if (TheValue == 0) { TheValue = 5; }
    }
    // END QUAY EDIT 1/2/14
    var MyWidth = IndControl.Frame.width;
    var MyHeight = IndControl.Frame.height;
    var MyLeft = IndControl.Frame.left;
    var MyTop = IndControl.Frame.top;
    drawShowerBackground(CT, MyLeft, MyTop, MyWidth, MyHeight);
    drawShowerWater(CT, MyLeft, MyTop, MyWidth, MyHeight, TheValue);
    // QUAY EDIT TO FOR GPCD VALUE
    // Do not draw a phrase 
//    drawValueText(CT, MyLeft, MyTop, MyWidth, MyHeight, TheValue);
     drawPerValue(CT, MyLeft, MyTop, MyWidth, MyHeight,IndControl.value);
    // drawPerValue(CT, MyLeft, MyTop, MyWidth, MyHeight, TheValue);

    drawShowerHead(CT, MyLeft, MyTop, MyWidth, MyHeight);
 
}

var UsePhrase = true;
function ValuePhrase(value) {
    var phrase = "";
    if (value > 75) { phrase = "Very High"; }
    else if (value > 60) { phrase = "High"; }
    else if (value > 40) { phrase = "Moderate"; }
    else if (value > 25) { phrase = "Low"; }
    else {phrase = "Very Low" }
    return phrase;
}

function RelPoint(X, Y) {
    this.X = X;
    this.Y = Y;
}


function DotDashPattern(aDotDashArray) {
    this.DotDashs = aDotDashArray;
}

var DotDashPatterns = new Array();
var MaxDotDashPatterns = 10;

function InitializePWC(IndControl) {
    for (var i = 0; i < MaxDotDashPatterns; i++) {
        var Dot1 = Math.random() * (IndControl.Frame.width / 10);
        var Dash1 = Math.random() * (IndControl.Frame.width / 25);
        var Dot2 = Math.random() * (IndControl.Frame.width / 5);
        var Dash2 = Math.random() * (IndControl.Frame.width / 10);
        var Dot3 = Math.random() * (IndControl.Frame.width / 20);
        var Dash3 = Math.random() * (IndControl.Frame.width / 30);
        DotDashPatterns[i] = new DotDashPattern([Dot1, Dash1, Dot2, Dash2, Dot3, Dash3]);
    }
}

///-------------------------------------------------------------------------------------------------
/// <summary>   Draw background. </summary>
///
/// <param name="parameter1">   Context for a canvas. </param>
///
/// <returns>   . </returns>
///-------------------------------------------------------------------------------------------------

function drawShowerBackground(CT, left, top, width, height) {
    // draw back
    CT.strokeStyle = "peachpuff";//"cornsilk";
    CT.fillStyle = "peachpuff";//"cornsilk";
    CT.fillRect(left, top, width, height);
    tileSize = width / 10;
    CT.lineWidth = 1;
    CT.strokeStyle = "BurlyWood ";//"cornsilk";
    var row = tileSize;
    var index = 4;
    while (row < height) {
        CT.beginPath();
        CT.setLineDash(DotDashPatterns[index].DotDashs);
        CT.moveTo(left, row);
        CT.lineTo(left + width, row);
        CT.stroke();
        CT.closePath;
        index++;
        if (index = MaxDotDashPatterns) index = 0;
        row += tileSize;
    }
    var col = tileSize;
    index = 8;
    while (col < width) {
        CT.beginPath();
        CT.setLineDash(DotDashPatterns[index].DotDashs);
        CT.moveTo(col, top);
        CT.lineTo(col, top + height);
        CT.stroke();
        CT.closePath;
        index++;
        if (index = MaxDotDashPatterns) index = 0;
        col += tileSize;
    }
}
//DAS
// text
function drawValueText(CT, left, top, width, height, value) {
    var PCTStr = " ";
    if (UsePhrase) { PCTStr = ValuePhrase(value); }
    else { PCTStr = value.toString() + "%"; }
    fontsize = Math.round(height / 6.5);
    fontstr = fontsize.toString() + "px Arial"
    CT.font = fontstr;
    CT.fillStyle = "royalblue";
    CT.fillText(PCTStr,left+2,top+fontsize );
  
}
// assigned number
function drawPerValue(CT, left, top, width, height, value) {
    // draw text
    var divisor = 6;
    var multiple = 5.45;
    //var PERStr = value.toString() + "%";
    // QUAY EDIT for GPCD 1/3/14
    var PERStr = value.toString()+" gpcd";
    fontsize = Math.round(height/divisor);
    fontStr = fontsize.toString() + "px Arial"
    CT.font = fontStr;
    CT.fillStyle = "#990033";
    //CT.textAlign = "right";
    CT.textAlign = "left";
    //CT.fillText(PERStr, left + (multiple * fontsize), top + (multiple * fontsize));
    CT.fillText(PERStr, left , top + fontsize);
}
function drawShowerWater(CT, left, top, width, height, value) {
    

    if (value > 0) {
        linedash1 = 2;
        linedash2 = 4;
        unitsX = width / 20;
        unitsY = height / 14;
        headdrop = width / 8;
        right = left + width - 1;
        headX = (right - headdrop);
        headY = top + headdrop;
        applycurve = false;
        usedash = false;
        if (value > 50) {
            incY = ((13 - (value / 10))*(unitsY/10));
            incX = ( 13 - (value / 10))*(unitsX/7);
            startY = (height / 2) + ((11 - (value / 10)) * unitsY);
            startX = left;
            endY = height + top;
            endX = (width / 2) +( (8-(value / 10))*unitsX);
        }
        else {
            applycurve = true;
            incY = ((13 - (value / 10)) * (unitsY / 10));
            incX = (13 - (value / 12)) * (unitsX / 7);
            startY = height + top;
            startX = left+((width / 50) * (50 - value));
            endY = height + top;
            endX = (width / 2) + ((9 - (value / 7)) * unitsX);
        }

        // if (value < 60) usedash = true;
        usedash = true;

        //        incY = 13 - (value / 10);
        //        incX = 13 -(value / 10);
        posY = startY;
        CT.strokeStyle = "royalblue";
        CT.lineWidth = (width / 100)+1;

        if (startY < height) {
            while (posY < endY) {
                CT.beginPath();
                if (usedash) {
                    dash1 = Math.round(Math.random() * (20 - incX)) * 2;
                    dash2 = Math.round(Math.random() * 5) + 1;
                    dash3 = Math.round(Math.random() * (20 - incX));
                    //dash1 = Math.round(Math.random() * (30 - incX)) * 2;
                    //dash2 = Math.round(Math.random() * 5) + 1;
                    //dash3 = Math.round(Math.random() * (30 - incX));
                    CT.setLineDash([dash1, dash2, dash3, dash2, dash1 / dash2, linedash2]);
                }
                CT.moveTo(headX, headY);
                if (applycurve) {
                    AngleX = ((headX + startX) / 2) - incX;
                    AngleY = (headY + posY) / 2;
                    CT.quadraticCurveTo(AngleX, AngleY, startX, posY);
                } else {
                    CT.lineTo(startX, posY);
                }
                CT.moveTo(headX, headY);
                CT.closePath();
                CT.stroke();
                posY = posY + incY;
            }
        }
        posX = startX;
        while (posX < endX) {
            CT.beginPath();
            if (usedash) {
                dash1 = Math.round(Math.random() * (20-incX))*2;
                dash2 = Math.round(Math.random() * 5)+1;
                dash3 = Math.round(Math.random() * (20 - incX));
                CT.setLineDash([dash1, dash2,dash3,dash2,dash1/dash2,linedash2]);
            }
            CT.moveTo(headX, headY);
            if (applycurve) {
                
                AngleX = ((headX + posX) / 2) - (incX+1);
                AngleY = (headY + endY) / 2;
                CT.quadraticCurveTo(AngleX, AngleY, posX, endY);
            } else {
                CT.lineTo(posX, endY);
            }
            CT.moveTo(headX, headY);
            CT.closePath();
            CT.stroke();
            posX = posX + incX;
        }
       
    }

        CT.setLineDash([]);
}
function drawShowerHead(CT, left, top, width, height) {
    right = left + width - 1;
    headdrop = width / 8;
    headX = (right - headdrop);
    headY = top + headdrop;
    CT.strokeStyle = "LightSlateGray ";
    CT.fillStyle = "";
    CT.lineWidth = width / 40;
    CT.beginPath();
    //CT.moveTo(right - (headdrop / 2), top + height);
    //CT.lineTo(right - (headdrop / 2), headY);
    //CT.lineTo(headX, headY);
    //CT.stroke();
    //CT.moveTo(right - (headdrop / 2) - 2, height / 2);
    //CT.lineTo(right - (headdrop / 2) - 2, (height / 2) + headdrop);
    //CT.moveTo(right - (headdrop / 2) - 4, height / 2);
    //CT.lineTo(right - (headdrop / 2) - 4, (height / 2) + headdrop);
    CT.moveTo(headX, headY);
    AngleX = ((headX + right) / 2) - (headdrop/4);
    AngleY = (headY + headY - (headdrop / 3)) / 2;
    CT.quadraticCurveTo(AngleX, AngleY, right, headY - (headdrop / 3));
    //CT.lineTo(right, top + (headdrop/2));
    CT.stroke();
    CT.closePath();
   //CT.stroke();
    CT.beginPath()
    CT.fillStyle = "LightSlateGray ";
    CT.moveTo(headX, headY);
    CT.lineTo(headX - headdrop, headY);
    CT.lineTo(headX, headY + headdrop);
    CT.lineTo(headX, headY);
    CT.closePath();
    CT.stroke();
    CT.fill();
  
}

//===========================================================
// The Groundwater Percent Indicator Support Functions
// ===========================================================


///-------------------------------------------------------------------------------------------------
/// <summary>   Draw gwp.   </summary>
///
/// <param name="parameter1">   The Control being drawn. </param>
///
/// <returns>   . </returns>
///-------------------------------------------------------------------------------------------------

function DrawGWP(IndControl) {
    var CT = IndControl.CT;
    var TheValue = IndControl.value;
    var MyWidth = IndControl.Frame.width;
    var MyHeight = IndControl.Frame.height;
    var MyLeft = IndControl.Frame.left;
    var MyTop = IndControl.Frame.top;
    SkyHeight = (MyHeight * 0.375) + MyTop;
    Horizon = (MyHeight * 0.375) +  MyTop;

    //RaiseIndicator(CT);
    drawBaseBackground(IndControl);
    drawGWPBackground(CT, MyLeft, MyTop, MyWidth, MyHeight, IndControl.Colors.Earth, Horizon);
    drawTower(CT, MyLeft, MyTop, MyWidth, MyHeight, "black", Horizon, TheValue);
    drawWater(CT, MyLeft, MyTop, MyWidth, MyHeight, IndControl.Colors.Water, Horizon, TheValue);
}

var GrndPattern;
var PatternCreated = false;

function InitializeGWP(IndControl) {
    if (!PatternCreated) {
        CreateGrndPattern(IndControl.Colors.Earth);
    }
}


function CreateGrndPattern(EarthColor) {
    GrndPattern = document.createElement("canvas");
    GrndPattern.width = 10;
    GrndPattern.height = 10;
    var tempContext = GrndPattern.getContext('2d');
    var LB = tempContext.createImageData(1, 1);
    LB.data[0] = 212;
    LB.data[1] = 136;
    LB.data[2] = 116;
    LB.data[3] = 255;
    var MB = tempContext.createImageData(1, 1);
    MB.data[0] = 175;
    MB.data[1] = 95;
    MB.data[2] = 71;
    MB.data[3] = 255;
    var DB = tempContext.createImageData(1, 1);
    DB.data[0] = 163;
    DB.data[1] = 83;
    DB.data[2] = 56;
    DB.data[3] = 255;
    var DDB = tempContext.createImageData(1, 1);
    DDB.data[0] = 145;
    DDB.data[1] = 77;
    DDB.data[2] = 62;
    DDB.data[3] = 255;
    tempContext.fillStyle = EarthColor;
    tempContext.fillRect(0, 0, 10, 10);
    tempContext.putImageData(LB, 2, 6);
    tempContext.putImageData(LB, 9, 2);
    tempContext.putImageData(MB, 3, 7);
    tempContext.putImageData(MB, 4, 9);
    tempContext.putImageData(DB, 7, 0);
    tempContext.putImageData(DB, 0, 2);
    tempContext.putImageData(DB, 1, 2);
    tempContext.putImageData(DB, 0, 3);
    tempContext.putImageData(DB, 1, 3);
    tempContext.putImageData(DB, 5, 3);
    tempContext.putImageData(DB, 6, 4);
    tempContext.putImageData(DB, 7, 6);
    tempContext.putImageData(DB, 4, 8);
    tempContext.putImageData(DDB, 1, 2);
    tempContext.putImageData(DDB, 8, 7);
}
///-------------------------------------------------------------------------------------------------
/// <summary>   Initializes the gwp. </summary>
///
/// <param name="parameter1">   The first parameter. </param>
/// <param name="parameter2">   The second parameter. </param>
///
/// <returns>   . </returns>
///-------------------------------------------------------------------------------------------------





function drawTower(CT, MyLeft, MyTop, MyWidth, MyHeight, TowerColor, Horizon, value) {
    if (value > 0) {
        // set color
        CT.strokeStyle = TowerColor;
        CT.lineWidth = TLineWidth;
        // set absolute height
        HeightInc = ((Horizon - (MyTop + 3)) / 100);
        WidthInc = (MyWidth / 2000);
        // put the base just off center on horizon
        BaseX = ((MyWidth / 5) * 2) + MyLeft;
        BaseY = Horizon;
        // get the value
        IndValue = parseInt(value);
        // calcuate top of tower
        TopY = Horizon - (IndValue * HeightInc);

        THeight = BaseY - TopY;
        TCrossY = BaseY - (THeight / 2);
        if (TopY < 1) { TopY = 1; }
        WX = value * WidthInc;
        if (WX > 20) { WX = 20; }
        CT.beginPath();
        // draw outside of tower
        CT.moveTo(BaseX - (WX + 5), BaseY);
        CT.lineTo(BaseX - 5, TopY);
        CT.lineTo(BaseX + 5, TopY);
        CT.lineTo(BaseX + WX + 5, BaseY);
        CT.lineTo(BaseX - (WX + 5), BaseY);
        // draw cross beams
        // if  Large enough else just fill
        if ((BaseY - TopY) > 10) {
            CT.moveTo(BaseX - (WX + 3), BaseY)
            CT.lineTo(BaseX + (3 + (WX / 2)), TCrossY);
            CT.lineTo(BaseX - (3 + (WX / 2)), TCrossY);
            //CT.moveTo(BX -(4+(WX/2)), TCrossY);
            CT.lineTo(BaseX + (WX + 3), BaseY)
        }
        else {
            CT.fillStyle = "#606060";
            CT.fill();
        }
        // now draw well line
        WaterHeight = (MyHeight - Horizon) * 0.8;
        GBaseY = Horizon + WaterHeight + MyTop;
        CT.moveTo(BaseX, BaseY);
        CT.lineTo(BaseX, GBaseY);
        CT.closePath();
        CT.stroke();
    }
}
    

function drawWater(CT, MyLeft, MyTop, MyWidth, MyHeight, WaterColor, Horizon, value) {
    // lightskyblue
    // royalblue
    // tan or burleywood
    // 
    var divisor = 2.5;
    var Oldvalue = 0;
    MyRight = MyWidth + MyLeft;
    CT.strokeStyle = WaterColor;
    CT.fillStyle = WaterColor;
    CT.globalAlpha = 0.7;
    CT.lineWidth = 1;
    WaterHeight = (MyHeight - Horizon) * 0.8;
    GBaseY = Horizon + ((MyHeight - (Horizon + WaterHeight)) / 2) + MyTop;
    WaterInc = (WaterHeight / 2) / 110;
    ArcSize = parseInt(value) * WaterInc;
    Base1X = ((MyWidth / 5)*2)+MyLeft;
    Base1Y = GBaseY + ArcSize;
    CT.beginPath();
    if (value > 0) {
      CT.arc(Base1X, Base1Y, ArcSize, Math.PI, 0, true);
      CT.arc(Base1X + (ArcSize * 2), Base1Y, ArcSize, Math.PI, 1.5 * Math.PI);
      CT.lineTo(MyRight, GBaseY);
      CT.lineTo(MyRight, GBaseY + WaterHeight);
      CT.lineTo(MyLeft+1, GBaseY + WaterHeight);
      CT.lineTo(MyLeft+1, GBaseY);
      
      CT.arc(Base1X - (ArcSize * 2), Base1Y, ArcSize, 1.5 * Math.PI, 0);
      } else {
         CT.moveTo(MyLeft+1, GBaseY);
         CT.lineTo(MyRight, GBaseY);
         CT.lineTo(MyRight, GBaseY + WaterHeight-3);
         CT.lineTo(MyLeft+1, GBaseY + WaterHeight-3);
         CT.lineTo(MyLeft+1, GBaseY);
        }
      CT.stroke();
      CT.fill()
      CT.closePath();
        // draw text
      var PCTStr = value.toString() + "%";
      fontsize = Math.round(WaterHeight / divisor);
      fontstr = fontsize.toString() + "px Arial"
      CT.font = fontstr;
      CT.fillStyle = "#FFFFFF";
      CT.textAlign = "right";
      CT.fillText(PCTStr, MyRight - 5, (GBaseY + WaterHeight - 5));
    //
      //var oldPCTStr = Oldvalue.toString() + "%";
      //fontsize = Math.round(WaterHeight / divisor);
      //fontstr = fontsize.toString() + "px Arial"
      //CT.font = fontstr;
      //CT.fillStyle = "#FFB310";
      //CT.textAlign = "left";
      //CT.fillText(oldPCTStr, MyRight - 85, (GBaseY + WaterHeight -20));
      

    }


function drawGWPBackground(CT, MyLeft, MyTop, MyWidth, MyHeight, EarthColor, Horizon) {

        
        // Draw Earth
        CT.strokeStyle = EarthColor;
        //IC.fillStyle = "#A08010";
        var GrnPattern = CT.createPattern(GrndPattern, "repeat");
        CT.fillStyle = GrnPattern;
        CT.fillRect(MyLeft, Horizon, MyWidth, (MyHeight-Horizon)+MyTop);
    }


//================================================================
// ENVIRONMENTAL
// ==============================================================
function DrawENV(IndControl) {
    var CT = IndControl.CT;
    var TheValue = IndControl.value;
    var MyWidth = IndControl.Frame.width;
    var MyHeight = IndControl.Frame.height;
    var MyLeft = IndControl.Frame.left;
    var MyTop = IndControl.Frame.top;
    Horizon = (MyHeight * 0.375) + MyTop;
    //RaiseIndicator(TheContext);
    drawBaseBackground(IndControl);
    //drawBackground(TheContext);
    ENVforeground(CT, MyLeft, MyTop, MyWidth, MyHeight, Horizon, IndControl.Colors.Earth, TheValue);
    river(CT, MyLeft, MyTop, MyWidth, MyHeight, Horizon, IndControl.Colors.Water, TheValue);
//    horizon(TheContext);
    //  offsetMountain(TheContext);
    
}


// Initialize Variables
//var CHeight;
//var CWidth;
//var IndOffset;
//var IndLeft;
//var IndRight;
//var IndTop;
//var IndBottom;
//var IndWidth;
//var IndHeight;
//var TBaseX;
//var TBaseY;
//var SkyHeight;
//var WaterHeight;
//var GroundHeight;
//var GBaseY;
//var WaterInc;
//var TLineWidth;
//var WidthInc;
//var HeightInc;

var OakPattern;

function InitializeENV(IndControl) {
// will have to change this maybe these would be better as objects?
    CHeight = IndControl.Frame.height;
    CWidth = IndControl.Frame.width;
    IndOffset = IndControl.offsetright;
    IndLeft = IndControl.Frame.left;
    IndRight = IndControl.Frame.right;
    IndTop = IndControl.Frame.top;
    IndBottom = IndControl.Frame.bottom;
    IndWidth = IndControl.Frame.width;
    IndHeight = IndControl.Frame.height;
    TBaseX = IndWidth / 2 + IndOffset;
    TBaseY = (IndHeight * 0.375) + 5 + IndOffset;
    SkyHeight = (IndHeight * 0.375) + 5;
    WaterHeight = (IndHeight - TBaseY) * 0.8;
    GroundHeight = IndHeight - SkyHeight;
    GBaseY = TBaseY + ((IndHeight - (TBaseY + WaterHeight)) / 2)
    WaterInc = (WaterHeight / 2) / 100;
    TLineWidth = IndWidth / 100;
    if (TLineWidth < 1) TLineWidth = 1;
    WidthInc = (IndWidth / 30) / 100;
    HeightInc = (TBaseY - (IndTop + IndOffset)) / 100;
    //Oakimage2.onload = function () {
    var OakImage = new Image();
    OakImage.onload = function () {
        OakPattern = IndControl.CT.createPattern(OakImage, "repeat");
        IndControl.Paint();
    }
    OakImage.src = assetsPath + "Images/oak_sm2.png";
    
   
  
}

function river(CT, MyLeft, MyTop, MyWidth, MyHeight, Horizon, WaterColor, TheValue) {
    groundHeight = (MyHeight+MyTop) - Horizon;
    MyBottom = MyHeight + MyTop;
    MYinc = groundHeight / 100;
    MXinc = MyWidth / 100;
    var rest = (( (TheValue*MXinc) /3)*2);
    var Rleft = (35 * MXinc) + MyLeft;
    var RBLeft = Rleft + (45*MXinc);
    var reSize = 1.0;
    CT.fillStyle = WaterColor;
    CT.strokeStyle = WaterColor;
    CT.beginPath();
    CT.moveTo(Rleft, Horizon);
    CT.quadraticCurveTo(RBLeft,( MyBottom+Horizon)/2, RBLeft, MyBottom);
    // Move left to accomodate changes in "value"
    var left = Math.max(MyLeft, RBLeft - rest);
    CT.lineTo(left, MyBottom);
    //
    CT.quadraticCurveTo(left, (MyBottom + Horizon) / 2, Rleft, Horizon);
    CT.stroke();
    CT.fill();
    CT.closePath();
}
function ENVforeground(CT, MyLeft, MyTop, MyWidth, MyHeight, Horizon, EarthColor, TheValue) {
    MyBottom = MyHeight + MyTop;
    groundHeight =( MyHeight+MyTop) - Horizon;
    CT.fillStyle = EarthColor;
    CT.fillRect(MyLeft, Horizon, MyWidth, groundHeight);
    var divisor = 2.6;
    var adj = 8;
    if (OakPattern != null) {
        MyBottom = MyHeight + MyTop;
        MyRight = MyLeft+MyWidth;
        MYinc = groundHeight / 100;
        MXinc = MyWidth / 100;
        var rest = (((TheValue * MXinc) / 3) * 2);
        var Rleft = (35 * MXinc) + MyLeft ;
        var RBLeft = Rleft + (45 * MXinc);
        var reSize = 1.0;
        buffer = rest / 4;
        //buffer = rest / 2.5;
        CT.fillStyle = OakPattern;
        CT.beginPath();
        CT.moveTo(Rleft, Horizon);
        CT.quadraticCurveTo(RBLeft+buffer, (MyBottom + Horizon) / 2, RBLeft+buffer, MyBottom);
        // Move left to accomodate changes in "value"
        var left = Math.max(MyLeft, (RBLeft - rest)-buffer);
        CT.lineTo(left, MyBottom);
        //
        CT.quadraticCurveTo(left, (MyBottom + Horizon) / 2, Rleft, Horizon);
        CT.stroke();
        CT.fill();
        CT.closePath();
        // draw text
        var PCTStr = TheValue.toString() + "%";
        fontsize = Math.round(WaterHeight / divisor);
        fontstr = fontsize.toString() + "px Arial"
        CT.font = fontstr;
        //CT.fillStyle = "#990033";
        //CT.textAlign = "right";
        //CT.fillText(PCTStr, MyRight - 2, Horizon + (25 * MYinc));

        CT.fillStyle = "#FFFFFF";
        CT.textAlign = "right";
        CT.fillText(PCTStr, MyRight - 3, Horizon - 3);

    //}
    //groundHeight = MyHeight - Horizon;
    //if (TheValue > 0) {
    //    Yinc = groundHeight / 100;
    //    var mod = TheValue * Yinc;
    //    if (OakPattern != null) {
    //        CT.fillStyle = OakPattern;
    //        //IC.fillRect(IndLeft - 5 + (IndWidth / 2), BY, IndWidth / 2, GroundHeight - mod);
    //        CT.fillRect(IndLeft, Horizon, MyWidth,  mod);
    //    }
    }


}
function horizon(TheContext) {
    BY = TBaseY;
    MYinc = TBaseY / 100;
    MXinc = IndWidth / 100;
    TheContext.strokeStyle = "#663300";
    TheContext.beginPath();
    TheContext.moveTo(IndLeft, BY);
    TheContext.lineTo((107 * MXinc) - IndOffset, BY);
    TheContext.lineWidth = 2;
    TheContext.stroke();
    TheContext.closePath();
}
function drawBackground(IC) {
    // draw sky
    sky(IC);

    // draw Far Mtn
    farMountain(IC);

    // draw Mid Mtn
    midMountain(IC);

    // draw near mtn
    nearMountain(IC);

    // draw shoulder
    shoulder(IC);
    // draw shoulder shadow/
   highlights(IC);
    // draw sun
   sun(IC);
    // Draw Earth
    var GrnPattern = IC.createPattern(ENVGrndPattern, "repeat");
    IC.fillStyle = GrnPattern;
    IC.fillRect(IndLeft, BY, IndWidth, GroundHeight);
    //
}
function sky(IC) {
    IC.strokeStyle = "#10B0FF";
    IC.fillStyle = "#10B0FF";
    BY = TBaseY;
    SkyHeight = TBaseY;
    MYinc = TBaseY / 100;
    MXinc = IndWidth / 100;
    IC.fillRect(IndLeft, IndTop, IndWidth, BY);
}
function farMountain(IC) {
    IC.strokeStyle = "#C2C2AD";
    IC.fillStyle = "#C2C2AD";
    IC.beginPath();
    IC.moveTo(IndLeft * MXinc, BY);
    IC.lineTo(15 * MXinc, 75 * MYinc);
    IC.lineTo(18 * MXinc, 79 * MYinc);
    IC.lineTo(25 * MXinc, 82 * MYinc);
    IC.lineTo(30 * MXinc, 100 * MYinc);
    IC.lineTo(5 * MXinc, BY);
    IC.stroke();
    IC.fill();
    IC.closePath();
    //
    IC.strokeStyle = "#3D4C3D";
    IC.fillStyle = "#3D4C3D";
    IC.beginPath();
    IC.moveTo(21 * MXinc, BY);
    IC.lineTo(17 * MXinc, 85 * MYinc);
    IC.lineTo(23 * MXinc, BY);
    IC.lineTo(21 * MXinc, BY);
    IC.stroke();
    IC.fill();
    IC.closePath();
    //
    IC.strokeStyle = "#EBEBE0";
    IC.fillStyle = "#EBEBE0";
    IC.beginPath();
    IC.moveTo(IndLeft * MXinc + 2, BY);
    IC.lineTo(15 * MXinc, 88 * MYinc);
    IC.lineTo(23 * MXinc, BY);
    IC.lineTo(IndLeft * MXinc + 5, BY);
    IC.stroke();
    IC.fill();
    IC.closePath();
}
function midMountain(IC) {
    IC.strokeStyle = "#949470";
    IC.fillStyle = "#949470";
    IC.beginPath();
    IC.moveTo(17 * MXinc, BY);
    IC.lineTo(25 * MXinc, 85 * MYinc);
    IC.lineTo(40 * MXinc, 45 * MYinc);
    IC.lineTo(45 * MXinc, 50 * MYinc);
    IC.lineTo(50 * MXinc, 47 * MYinc);
    IC.lineTo(55 * MXinc, 38 * MYinc);
    IC.lineTo(85 * MXinc, BY);
    IC.lineTo(25 * MXinc, BY);
    IC.stroke();
    IC.fill();
    IC.closePath();
}
function nearMountain(IC) {
    IC.beginPath();
    IC.strokeStyle = "#5C5C3D";
    IC.fillStyle = "#5C5C3D";
    IC.moveTo(40 * MXinc, BY);
    IC.lineTo(60 * MXinc, 75 * MYinc);
    IC.lineTo(80 * MXinc, 35 * MYinc);
    IC.lineTo(85 * MXinc, 35 * MYinc);
    IC.lineTo(88 * MXinc, 30 * MYinc);
    IC.lineTo(92 * MXinc, 33 * MYinc);
    IC.lineTo(96 * MXinc, 37 * MYinc);


    IC.lineTo(IndRight, 50 * MYinc);

    IC.lineTo(IndRight, BY);
    IC.lineTo(40 * MXinc, BY);
    IC.stroke();
    IC.fill();
    IC.closePath();
}
function shoulder(IC) {
    IC.strokeStyle = "#3D3D1F";
    IC.fillStyle = "#3D3D1F";
    IC.beginPath();
    IC.moveTo(50 * MXinc, BY);
    IC.lineTo(75 * MXinc, 70 * MYinc);
    IC.lineTo(80 * MXinc, 100 * MYinc);
    IC.lineTo(70 * MXinc, BY);
    IC.stroke();
    IC.fill();
    IC.closePath();
}
function highlights(IC) {
    // shoulder of near mountain
    IC.strokeStyle = "#000000";
    IC.fillStyle = "#000000";
    IC.beginPath();
    IC.moveTo(80 * MXinc, BY);
    IC.lineTo(75 * MXinc, 73 * MYinc);
    IC.lineTo(87 * MXinc, 100 * MYinc);
    IC.lineTo(80 * MXinc, BY);
    IC.stroke();
    IC.fill();
    IC.closePath();
}
function offsetMountain(IC) {
    var shift = 41;
    var down = 2;
    IC.strokeStyle = "#3D3D1F";
    IC.fillStyle = "#3D3D1F";
    IC.beginPath();
    IC.moveTo((55 - shift) * MXinc, BY + down);
    IC.lineTo((70 - shift) * MXinc, 87 * MYinc);
    IC.lineTo((75 - shift) * MXinc, 87 * MYinc);

    IC.lineTo((80 - shift) * MXinc, 100 * MYinc + down);
    IC.lineTo((70 - shift) * MXinc, BY + down * 1.5);
    IC.stroke();
    IC.fill();
    IC.closePath();


}

// ==================================================
// AG
// =====================================================
function DrawAGR(TheControl) {
    var TheContext = TheControl.CT;
    var value = TheControl.value;
    var MyWidth = TheControl.Frame.width;
    var MyHeight = TheControl.Frame.height;
    var MyLeft = TheControl.Frame.left;
    var MyTop = TheControl.Frame.top;
    //RaiseIndicator(TheContext);
    drawBaseBackground(TheControl);
    //drawBackground(TheContext);
    //horizon(TheContext);
    Agforeground(TheContext, MyLeft, MyTop, MyWidth, MyHeight, Horizon, value)
}

var AgGrnPattern;
var HousePattern;
function InitializeAGR(TheControl) {
    var drawing1 = document.createElement("canvas");
    var con1 = drawing1.getContext("2d");
    var image2 = new Image();
    image2.onload = function () {
        HousePattern = con1.createPattern(image2, "repeat");
        TheControl.Paint();
    }
    image2.src = assetsPath + "Images/house3.png";
    var drawing2 = document.createElement("canvas");
    //var drawing = document.getElementById("idIndicatorCanvasAgToMuni");
    var con2 = drawing2.getContext("2d");
    var image1 = new Image();
    image1.onload = function () {
        AgGrnPattern = con2.createPattern(image1, "repeat");
        TheControl.Paint();
    }
    image1.src = assetsPath + "Images/rowcropsquare1.jpg";
}

function Agforeground(CT, MyLeft, MyTop, MyWidth, MyHeight, Horizon, value) {
    //
    var divisor = 2.5;
    var Oldvalue = 0;
    var AdjHorizon = 2;
    var ZeroStr = value;
    Horizon = Horizon + AdjHorizon
    MyRight = MyLeft + MyWidth;
    MyTop = MyTop + AdjHorizon;
    groundHeight = (MyHeight + MyTop) - Horizon;
    CT.strokeStyle = "#5C5C3D";
    CT.fillStyle = HousePattern;

    //var top = Math.min(GroundHeight, GroundHeight - mod);
    CT.fillRect(MyLeft, Horizon, MyWidth, groundHeight);

    //var image2 = new Image();
    //image2.src = assetsPath + "Images/house3.png";
    //var AgGrnPattern = IC.createPattern(image2, "repeat");
    if (value < 1) value = 1;
    if (value > 0) {
        Yinc = groundHeight / 100;
        //ValueHeight = value * Yinc;
        ValueHeight = (100-value) * Yinc;
        if (AgGrnPattern != null) {
            CT.fillStyle = AgGrnPattern;
            CT.fillRect(MyLeft, Horizon + (groundHeight - ValueHeight), MyWidth, ValueHeight);
        }
        if (ZeroStr < 1) {
            var PCTStr = "0" + "%";
        }
        else {
            var PCTStr = value.toString() + "%";
        }
        fontsize = Math.round(WaterHeight / divisor);
        fontstr = fontsize.toString() + "px Arial"
        CT.font = fontstr;
        CT.fillStyle = "#FFFFFF";
        CT.textAlign = "right";
        CT.fillText(PCTStr, MyRight - 2, Horizon - 5);
    }
}
function horizon(TheContext) {
    BY = TBaseY;
    MYinc = TBaseY / 100;
    MXinc = IndWidth / 100;
    TheContext.strokeStyle = "#663300";
    TheContext.beginPath();
    TheContext.moveTo(IndLeft, BY);
    TheContext.lineTo((107 * MXinc) - IndOffset, BY);
    TheContext.lineWidth = 2;
    TheContext.stroke();
    TheContext.closePath();
}
function drawBackground(IC) {
    // draw sky
    sky(IC);

    // draw Far Mtn
    farMountain(IC);

    // draw sun
    sun(IC);
}



