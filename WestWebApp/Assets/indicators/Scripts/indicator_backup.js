// function Frame(left, top, width, height) {
//     this.left = left;
//     this.top = top;
//     this.width = width;
//     this.height = height;
//     this.reset = function () {
//     }
//     this.right = this.left + (this.width - 1);
//     this.bottom = this.top + (this.height - 1);

//     this.reset = function () {
//         this.right = this.left + (this.width - 1);
//         this.bottom = this.top + (this.height - 1);

//     }
// }

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

// QUAY EDIT 3/19/16 Begin
// DAS edit 03.28.16 (from zero to one index on the array)
var IndImageIndex = 0;
var IndImageIndex = 1;

//function IndSetupData(aTitle) {
//    this.filenames = new Array();
//    this.Title = aTitle;
//}
//var IndSetupData = new Array();
//IndSetupData["ECOR"] = new IndSetupData("");
//IndSetupData["ECOR"].filenames[0] = './Assets/indicators/New_Images/economy.jpg';
//IndSetupData["ECOR"].Title = "Economy";
//IndSetupData["ENVIND"] = new IndSetupData("");
//IndSetupData["ENVIND"].filenames[0] = './Assets/indicators/New_Images/environment.jpg';
//IndSetupData["ENVIND"].Title = "Environment";
//IndSetupData["SWI"] = new IndSetupData("");
//IndSetupData["SWI"].filenames[0] = './Assets/indicators/New_Images/surfacewater.jpg';
//IndSetupData["SWI"].Title = "Surface Water";
//IndSetupData["GWSYA"] = new IndSetupData("");
//IndSetupData["GWSYA"].filenames[0] = './Assets/indicators/New_Images/groundwater.jpg';
//IndSetupData["GWSYA"].Title = "Groundwater";
//IndSetupData["UEF"] = new IndSetupData("");
//IndSetupData["UEF"].filenames[0] = './Assets/indicators/New_Images/urbanefficiency.jpg';
//IndSetupData["UEF"].Title = "Urban Efficiency";
//IndSetupData["PEF"] = new IndSetupData("");
//IndSetupData["PEF"].filenames[0] = './Assets/indicators/New_Images/power.jpg';
//IndSetupData["PEF"].Title = "Power Efficiency";
//IndSetupData["AGIND"] = new IndSetupData("");
//IndSetupData["AGIND"].filenames[0] = './Assets/indicators/New_Images/agriculture.jpg';
//IndSetupData["AGIND"].Title = "Agriculture";

//var MeterFiles = {
//    rg_meter: {
//        fileName: './Assets/indicators/New_Images/rg_meter.jpg'
//    },
//    rgr_meter: {
//        fileName: './Assets/indicators/New_Images/rgr_meter.jpg'
//    }
//}

//function GetindicatorImages(indicatorType) {
//    var indicatorImages = {
//        ECOR: {
//            fileName: IndSetupData["ECOR"].filenames[IndImageIndex],
//            title: 'Economy'
//        },
//        ENVIND: {
//            fileName: IndSetupData["ENVIND"].filenames[IndImageIndex],
//            title: 'Environment'
//        },
//        SWI: {
//            fileName: IndSetupData["SWI"].filenames[IndImageIndex],
//            title: 'Surface Water'
//        },
//        GWSYA: {
//            fileName: IndSetupData["GWSYA"].filenames[IndImageIndex],
//            title: 'Groundwater'
//        },
//        UEF: {
//            fileName: IndSetupData["UEF"].filenames[IndImageIndex],
//            title: 'Urban Efficiency'
//        },
//        PEF: {
//            fileName: IndSetupData["PEF"].filenames[IndImageIndex],
//            title: 'Power Efficiency'
//        },
//        AGIND: {
//            fileName: IndSetupData["AGIND"].filenames[IndImageIndex],
//            title: 'Agriculture'
//        },


//        rg_meter: {
//            fileName: './Assets/indicators/New_Images/rg_meter.jpg'
//        },
//        rgr_meter: {
//            fileName: './Assets/indicators/New_Images/rgr_meter.jpg'
//        }

//    }
//    return indicatorImages[indicatorType];
//}



var IndSetupData = {
    ECOR: {
        filenames:['./Assets/indicators/New_Images/economy_button_grey.jpg','./Assets/indicators/New_Images/economy_flat_grey.jpg','./Assets/indicators/New_Images/economy_flat_grey_color.jpg','./Assets/indicators/New_Images/economy.jpg'],
        title: 'Economy'
    },
    ENVIND: {
        filenames: ['./Assets/indicators/New_Images/environment_button_grey.jpg', './Assets/indicators/New_Images/environment_flat_grey.jpg', './Assets/indicators/New_Images/environment_flat_grey_color.jpg', './Assets/indicators/New_Images/environment.jpg'],
        title: 'Environment'
    },
    SWI: {
        filenames: ['./Assets/indicators/New_Images/surfacewater_button_grey.jpg', './Assets/indicators/New_Images/surfacewater_flat_grey.jpg', './Assets/indicators/New_Images/surfacewater_flat_grey_color.jpg', './Assets/indicators/New_Images/surfacewater.jpg'],
        title: 'Surface Water'
    },
    GWSYA: {
        filenames: ['./Assets/indicators/New_Images/groundwater_button_grey.jpg', './Assets/indicators/New_Images/groundwater_flat_grey.jpg', './Assets/indicators/New_Images/groundwater_flat_grey_color.jpg', './Assets/indicators/New_Images/groundwater.jpg'],
        title: 'Groundwater'
    },
    UEF: {
        filenames: ['./Assets/indicators/New_Images/urbanefficiency_button_grey.jpg', './Assets/indicators/New_Images/urbanefficiency_flat_grey.jpg', './Assets/indicators/New_Images/urbanefficiency_flat_grey_color.jpg', './Assets/indicators/New_Images/urbanefficiency.jpg'],
        title: 'Urban Efficiency'
    },
    PEF: {
        filenames: ['./Assets/indicators/New_Images/power_button_grey.jpg', './Assets/indicators/New_Images/power_flat_grey.jpg', './Assets/indicators/New_Images/power_flat_grey_color.jpg', './Assets/indicators/New_Images/power.jpg'],
        title: 'Power Efficiency'
    },
    AGIND: {
        filenames: ['./Assets/indicators/New_Images/agriculture_button_grey.jpg', './Assets/indicators/New_Images/agriculture_flat_grey.jpg', './Assets/indicators/New_Images/agriculture_flat_grey_color.jpg', './Assets/indicators/New_Images/agriculture.jpg'],
        title: 'Agriculture'
    },

    // STEPTOE EDIT 05_11_16 BEGIN
    MINFO: {
        upperTitle: 'Current',
        lowerTitle: 'Previous'
    },
    // STEPTOE EDIT 05_11_16 BEGIN

    rg_meter: {
        filename:'./Assets/indicators/New_Images/rg_meter.jpg'
    },
    rgr_meter: {
        filename:'./Assets/indicators/New_Images/rgr_meter.jpg'
    },
    fillstyle: { color:"#E6E6E6"},
}
//var indicatorImages = {
//    ECOR: {
//        fileName: './Assets/indicators/New_Images/economy.jpg',
//        title: 'Economy'
//    },
//    ENVIND: {
//        fileName: './Assets/indicators/New_Images/environment.jpg',
//        title: 'Environment'
//    },
//    SWI: {
//        fileName: './Assets/indicators/New_Images/surfacewater.jpg',
//        title: 'Surface Water'
//    },
//    GWSYA: {
//        fileName: './Assets/indicators/New_Images/groundwater.jpg',
//        title: 'Groundwater'
//    },
//    UEF: {
//        fileName: './Assets/indicators/New_Images/urbanefficiency.jpg',
//        title: 'Urban Efficiency'
//    },
//    PEF: {
//        fileName: './Assets/indicators/New_Images/power.jpg',
//        title: 'Power Efficiency'
//    },
//    AGIND: {
//        fileName: './Assets/indicators/New_Images/agriculture.jpg',
//        title: 'Agriculture'
//    },


//    rg_meter: {
//        fileName: './Assets/indicators/New_Images/rg_meter.jpg'
//    },
//    rgr_meter: {
//        fileName: './Assets/indicators/New_Images/rgr_meter.jpg'
//    }

//}

// QUAY EDIT 3/19/16 end;


function IndicatorControlNew(divId, anIndicatorType, ControlId, options, showMeterInfo) {
    //divId, anIndicatorType, ControlId, Width, Height
    //var myHeight = 100;
    //var myWidth = 100;
   //  defWidth: 225,
    // defHeight: 186,
    // new=176       specWidth: 196,
    //    specHeight: 162
    var defaults = {
        defWidth: 225,
        defHeight: 186,
        specWidth: 225,
        specHeight: 172
    };


    //fill defaults with user specified options
    if(typeof(options) != "undefined"){
        for(var option in options){
            defaults[option] = options[option];
        }
    }

    // console.log(defaults)
    if (defaults.Width == undefined && defaults.Height != undefined){
        defaults.Width = defaults.specWidth * (defaults.Height / defaults.specHeight);
    }
    else if(defaults.Width != undefined && defaults.Height == undefined){
        defaults.Height = defaults.specHeight * (defaults.Width / defaults.specWidth);
    }

    this.id = ControlId
    if (defaults.Width == undefined) {
        var aWidth = getDomWidthWithID(divId);
        // console.log("aWidth: " + aWidth)
        if (aWidth > 0) {
            this.width = aWidth;
            defaults.width = aWidth;
        } else {
           this.width = defaults.defWidth;
        }
    } else {
        this.width = defaults.Width;
    }
    if (defaults.Height == undefined) {
            var aHeight = getDomHeightWithID(divId);
            if (aHeight > 0) {
                this.height = aHeight;
                defaults.height = aHeight;
            } else {
            this.height = defaults.defHeight;
        }
    } else {
        this.height = defaults.Height;
    }

    //Check if div exists, if not create the div
    if (!$('#'+divId).length){
        $('.accordion').append('<div id="' + divId + '" class="IndicatorControl" data-fld="' + anIndicatorType + '" style="position:relative;"></div>')
    }
    this.textWidth = 0;

    // STEPTOE EDIT 05_11_16 BEGIN
    if(showMeterInfo){
        this.UpperTitle = IndSetupData["MINFO"].upperTitle;
        this.LowerTitle = IndSetupData["MINFO"].lowerTitle;
        this.textWidth = 90;
    }
    // STEPTOE EDIT 05_11_16 BEGIN

    // Setup Div Size so that no outer container is required
    this.divObj = $('.accordion').find('#' + divId);
    this.divObj.css('width', this.width + this.textWidth);
    this.divObj.css('height', this.height);
    this.divObj.css('float', 'left');

    // console.log(this.width, this.height);

    this.IndicatorType = anIndicatorType;
    // QUAY EDIT 3/19/16 begin
    // adding file name here, so doe snot need to be extreacted each call to draw
    this.Filename = IndSetupData[anIndicatorType].filenames[IndImageIndex];
    this.Title = IndSetupData[anIndicatorType].title;
    // QUAY EDIT 3/19/16 end

    this.DivID = divId;
    // QUAY ADDED FOr report support 1/2/14
    this.Description = "";

    this.MinValue = 0; // default
    this.MaxValue = 150; // default
    this.value = 0;  // 0 to 100 valid
    this.value_old = 0;
    this.drawOldValue = false;
    this.drawCurrentValue = false;
    //this.fudgecnt = 0;

    // This is a function used to set the value of the control and redraw
    this.SetValue = function (value) {
        // STEPTOE EDIT 05_11_16 BEGIN
        console.log('SetValue ', anIndicatorType, ': ', value);
        // STEPTOE EDIT 05_11_16 END

        // check if valid range
        if ((value >= this.MinValue) && (value <= this.MaxValue)) {
            ////set old value
            this.value_old = this.value;
                 // set the value
                this.value = value;
            // paint the control
            this.drawCurrentValue = true;
            this.Paint();

            this.drawOldValue = true;
        }
    };
    // The frame offset
    this.offsetleft = 4;
    this.offsetright = 7;
    this.offsettop = 4;
    this.offsetbottom = 4;

    // create Frame
    // this.Frame = new Frame(this.offsetleft, this.offsettop, this.width - (this.offsetleft + this.offsetright), this.height - (this.offsettop + this.offsetbottom));
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
        // this.Frame.width = this.width - (this.offsetleft + this.offsetright);
        // this.Frame.height = this.height - (this.offsettop + this.offsetbottom);
        // this.Frame.reset();
        // reinitialize
        // InitializeIndicator(this);
        this.divObj.css('width', this.width);
        this.divObj.css('height', this.height);
        this.dims = calculateDims(this.canvas, defaults);

        // draw it
        this.Paint();
    }
    // onclick event handler
    this.oncLick = function (event) {
    }
    // Paint the object
    this.Paint = function () {
        DrawIndicatorNew(this);
    }

    // constructor Code
//    this.canvas = canvas;
    this.canvas = document.createElement("Canvas");
    this.canvas.width = this.width;
    this.canvas.height = this.height;
    this.canvas.id = this.DivID + this.id + "Canvas";
    this.canvas.style.position = "absolute";
    this.CT = this.canvas.getContext("2d");
    this.canvas.onclick = this.onclick;
    // Set defualt colors
    // this.Colors = new IndicatorColors("lightskyblue", "royalblue", "darkgreen", "darkolivegreen", "darkkhaki", "khaki", "gold", "burlywood", "peachpuff", "mediumblue", "yellowgreen", "lawngreen", "seagreen", "peru","#777777");

    // call the indicators initialization code
    // store all images in object no need to init
    // InitializeIndicator(this);
    this.dims = calculateDims(this.canvas, defaults);

    // STEPTOE EDIT 05_11_16 BEGIN
    if(showMeterInfo){
        this.textCanvas = document.createElement("Canvas");
        this.textCanvas.width = this.textWidth;
        this.textCanvas.height = this.height;
        this.textCanvas.id = this.DivID + this.id + "TextCanvas";
        this.textCanvas.style.position = "absolute";
        var meterDims = this.dims.meter;
        console.log('left should be: ', meterDims.width + meterDims.padding.left + this.dims.indicator.x * 2 - 5);
        this.textCanvas.style.left = meterDims.width + meterDims.padding.left + this.dims.indicator.x * 2 - 5;
    }
    // STEPTOE EDIT 05_11_16 BEGIN

    // draw thw indicator
    DrawIndicatorNew(this);
    // add it to the div element
    var TheDiv = document.getElementById(this.DivID);
    if (TheDiv != undefined)
    {
        if (TheDiv.appendChild) {
            TheDiv.appendChild(this.canvas);

            // STEPTOE EDIT 05_11_16 BEGIN
            if(showMeterInfo){
                TheDiv.appendChild(this.textCanvas);
            }
            // STEPTOE EDIT 05_11_16 BEGIN
        }
    }

}

function draw(canvas, dims){
  if (canvas.getContext){
    var canvasDims = dims.canvas;

    var ctx = canvas.getContext('2d');
    ctx.clearRect(0, 0, canvasDims.width, canvasDims.height);
    ctx.beginPath();
    ctx.rect(0, 0, canvasDims.width, canvasDims.height);
      //ctx.fillStyle = 'rgb(80, 85, 88)';
    // QUAY EDIT 3/19/16 begin
    ctx.fillStyle = IndSetupData["fillstyle"].color;// '#e6e6e6';
    // QUAY EDIT 3/19/16 end;
    ctx.fill();
  }
}
function getTextHeight(font) {

    var text = $('<span>Hg</span>').css({ fontFamily: font });
    var block = $('<div style="display: inline-block; width: 1px; height: 0px;"></div>');

    var div = $('<div></div>');
    div.append(text, block);

    var body = $('body');
    body.append(div);

    try {

        var result = {};

        block.css({ verticalAlign: 'baseline' });
        result.ascent = block.offset().top - text.offset().top;

        block.css({ verticalAlign: 'bottom' });
        result.height = block.offset().top - text.offset().top;

        result.descent = result.height - result.ascent;

    } finally {
        div.remove();
    }

    return result;
}
function drawImage(IndControl,canvas,dims, src, values) {
    if (canvas.getContext) {
        var ctx = canvas.getContext('2d');
        var img = new Image();
        var drawOldValue = IndControl.drawOldValue;
        var drawCurrentValue = IndControl.drawCurrentValue;
        var indicatorDims = dims.indicator;
        // QUAY EDIT 3/19/16 begin
        //var indicatorFile = IndicatorImages[IndControl.IndicatorType];
        src = IndControl.Filename;
        // QUAY EDIT 3/19/16 end
        // var canvas = IndControl.canvas,
        dims = IndControl.dims;

        var x = indicatorDims.x + indicatorDims.border;
        var y = indicatorDims.border;

        img.onload = function () {
            ctx.drawImage(img, x, y, indicatorDims.width, indicatorDims.height);
            // QUAY EDIT 3/19/16 begin
            //drawText(canvas, dims, indicatorFile.title);
            drawText(canvas, dims, IndControl.Title);
            // QUAY EDIT 3/19/16 end;

            if(drawCurrentValue)
                drawMeter(canvas, dims, 'big', values.current);

            if (drawOldValue)
                drawMeter(canvas, dims, 'small', values.old);
        };
        // img.src = 'https://mdn.mozillademos.org/files/5397/rhino.jpg';
        img.src = src;
    }
}
function drawText(canvas, dims, title) {
  if (canvas.getContext){
    var canvasDims = dims.canvas;
    var indicatorDims = dims.indicator;
    //var fontInt = indicatorDims.x - indicatorDims.border;
    var fontInt = 22;
    //var x = indicatorDims.x - indicatorDims.border;
    //var y = canvasDims.height - indicatorDims.border;
    //var x = indicatorDims.x - indicatorDims.border;
   // var y = canvasHeight - border - textHeight;
    var ctx = canvas.getContext('2d');
    ctx.save();
    ctx.translate(x, y);
    //ctx.rotate(-Math.PI/4);
    ctx.font = fontInt + "px sans-serif";

    // STEPTOE EDIT 05_11_16 BEGIN
    // ctx.fillStyle = "#0066cc";
    ctx.fillStyle = "#000000";
    // STEPTOE EDIT 05_11_16 END

    var text = ctx.measureText(title); // TextMetrics object
    var Awidth = text.width/2;
    //while (text.width > y - indicatorDims.border) {
      while(text.width > indicatorDims.width - indicatorDims.border * 2){
      fontInt--;
      ctx.font = fontInt + "px sans-serif";
      text = ctx.measureText(title); // TextMetrics object
      // console.log("looping")
      }
      var adjRight = indicatorDims.width / 2 - Awidth;
      var x = adjRight + indicatorDims.x + indicatorDims.border;
      var y = canvasDims.height - indicatorDims.border - getTextHeight(ctx.font).height;

      console.log("text.height: ", getTextHeight(ctx.font));
      console.log("y: " + y);

      ctx.save();
      ctx.translate(x, y);
    ctx.fillText(title, 0, 0);
    ctx.restore();
  }
}

function drawTextAt(canvas, title, y, fontSize) {
  if (canvas.getContext){

    var fontInt = !fontSize ? 22 : fontSize;
    var ctx = canvas.getContext('2d');

    ctx.save();
    // ctx.translate(0, y);
    ctx.font = fontInt + "px sans-serif";
    ctx.textBaseline = 'middle';

    ctx.fillStyle = "#000000";

    var text = ctx.measureText(title); // TextMetrics object
    var Awidth = text.width/2;
    while(text.width > canvas.width){
        fontInt--;
        ctx.font = fontInt + "px sans-serif";
        text = ctx.measureText(title); // TextMetrics object
    }
    // Awidth = text.width/2;
    // var adjRight = canvas.width / 2 - Awidth;
    // var x = adjRight;
    ctx.translate(0, y);
    ctx.fillText(title, 0, 0);
    ctx.restore();
  }
}

function drawMeter(canvas, dims, size, value){
  if (canvas.getContext){
    var ctx = canvas.getContext('2d');
    var img = new Image();

    var indicatorDims = dims.indicator;
    var meterDims = dims.meter;
    var height = meterDims.height;
    var shuttleDims = dims.shuttle;
    //var adjHeight = -15;
    var x = indicatorDims.x + indicatorDims.border + meterDims.padding.left;
  //  var y = indicatorDims.border + indicatorDims.height + adjHeight- meterDims.padding.bottom;
    var y = indicatorDims.border + indicatorDims.height + meterDims.addHeight - meterDims.padding.bottom;
    if(size == 'small'){
        height *= 0.5;
     // y += shuttleDims.lineWidth * 3.5 + height;//15 + height;
      //  y += shuttleDims.lineWidth * 3.5 + height + meterDims.adjSeparation;//15 + height;
      //  y += shuttleDims.lineWidth * 3.5 + height + adjHeight + meterDims.adjSeparation;//15 + height;
        y += shuttleDims.lineWidth * 3.5 + height + meterDims.adjSeparation;// - meterDims.addHeight;//15 + height;
    }

    img.onload = function(){
      ctx.drawImage(img, x, y, meterDims.width, height);
      drawShuttle(canvas, dims, size, value);
    };
    // QUAY EDIT 3/19/16 BEGIN
    //
    img.src = IndSetupData[meterDims.style].filename;
    test = img.src;
    //img.src = indicatorImages[meterDims.style].fileName;
    // QUAY EDIT 3/19/16 END
  }
}

function drawMeterWithCustomShuttle(canvas, dims, size, value){
  if (canvas.getContext){
    var ctx = canvas.getContext('2d');
    var img = new Image();

    var indicatorDims = dims.indicator;
    var meterDims = dims.meter;
    var height = meterDims.height;
    var shuttleDims = dims.shuttle;

    var x = indicatorDims.x + indicatorDims.border + meterDims.padding.left;
    var y = indicatorDims.border + indicatorDims.height - meterDims.padding.bottom;

    if(size == 'small'){
      height *= 0.5;
      y += shuttleDims.lineWidth * 3.5 + height + meterDims.adjSeparation;  //15 + height;
    }

    img.onload = function(){
      ctx.drawImage(img, x, y, meterDims.width, height);
      drawShuttle(canvas, dims, size, value);
    };
    img.src = indicatorImages[meterDims.style].fileName;
  }
}

function drawShuttle(canvas, dims, size, value) {
  if (canvas.getContext){
    var ctx = canvas.getContext('2d');
    var img = new Image();

    var indicatorDims = dims.indicator;
    var meterDims = dims.meter;
    var shuttleDims = dims.shuttle;
    //  var height = meterDims.height;
    var height = meterDims.height + shuttleDims.addHeight;

    var percentage = value / 100 * Math.max(0, meterDims.width - shuttleDims.width - shuttleDims.lineWidth/2);
    // console.log("percentage: " + percentage);
    var x = percentage + indicatorDims.x + indicatorDims.border + meterDims.padding.left;
      // Add height to the shuttle (defined in the setting of the object below) 02.29.16 DAS
    if (size == 'big') {
        var y = meterDims.addHeight + indicatorDims.border + indicatorDims.height - meterDims.padding.bottom - shuttleDims.addHeight / 2;
    }
    else {
        height += 5;
        var y = indicatorDims.border + indicatorDims.height - (meterDims.padding.bottom+5  )- (shuttleDims.addHeight) / 2;
        //if(size == 'small'){
        height *= 0.5;
        // y += shuttleDims.lineWidth * 3.5 + height;//15 + height;
        y += shuttleDims.lineWidth * 3.5 + height - meterDims.adjSeparation*0.2;// + adjHeight*2;//15 + height;
    }

    ctx.beginPath();
    ctx.lineWidth = shuttleDims.lineWidth;
    ctx.strokeStyle = shuttleDims.color;
    ctx.rect(x, y, shuttleDims.width, height);
    ctx.stroke();
  }
}
function drawShuttleImage(canvas, dims, size, value){
  if (canvas.getContext){
    var ctx = canvas.getContext('2d');
    var img = new Image();

    var indicatorDims = dims.indicator;
    var meterDims = dims.meter;
    var shuttleDims = dims.shuttle;
   // var height = meterDims.height;
    var height = meterDims.height + shuttle.addHeight;


    var percentage = value / 100 * Math.max(0, meterDims.width - shuttleDims.width - shuttleDims.lineWidth/2);
    // console.log("percentage: " + percentage);
    var x = percentage + indicatorDims.x + indicatorDims.border + meterDims.padding.left;
    var y = indicatorDims.border + indicatorDims.height - meterDims.padding.bottom;

    if(size == 'small'){
        height *= 0.5;
       // y += shuttleDims.lineWidth * 3.5 + height;
      y += shuttleDims.lineWidth * 3.5 + height + meterDims.adjSeparation;//15 + height;
    }

    ctx.beginPath();
    ctx.lineWidth = shuttleDims.lineWidth;
    ctx.strokeStyle = '#33ccff';
    ctx.rect(x, y, shuttleDims.width, height);
    ctx.stroke();
  }
}

// Position the meters within the canvas, and other things
// 02.28.16 DAS
//-----------------------------------------------------------
function calculateDims(canvas, options){

  var scale = {
    width: canvas.width / 196,
    height: canvas.height / 162
  }
  // console.log(scale.width, scale.height)

    var dims = {
        canvas: {
          width: canvas.width,
          height: canvas.height
        },
        indicator: {
          width: 154,
          height: 154,
          border: 3,
          x: 15
        },
        meter: {
            style: 'rg_meter',
            width: 124,
            height: 22,
            adjSeparation: 10,
            addHeight: -12,
            padding: {
                left: 15 * scale.width,
                //bottom: 46 * scale.height = Value * scale.height determines meter placement on the indicator
                bottom: 62 * scale.height
            }
        },
        shuttle: {
            image: false,
            width: 10,
            lineWidth: 4,
            addHeight: 3,
            padding: {
                left: 15 * scale.width,
                bottom: 62 * scale.height
            },
            // color: '#377eb8'

            // STEPTOE EDIT 05_12_16 BEGIN
            // color: '#33ccff'
            color: '#03fc41'
            // STEPTOE EDIT 05_12_16 END
        },
        textCanvas: {
            width: 90,
            height: canvas.height
        }
    }

    for(var option in options){
        if(typeof(options[option]) != 'object')
            dims[option] = options[option];
        else{
            var object = options[option];
            for(var subOption in object){
                dims[option][subOption] = object[subOption];
            }
        }
    }

    for(var element in dims){
        if(element != 'canvas'){
          for(var dim in dims[element]){
            if(dim == 'width' || dim == 'height')
              dims[element][dim] = dims[element][dim] * scale[dim];
            else if(typeof(dims[element][dim]) == 'number'){
              dims[element][dim] = dims[element][dim] * scale.width;
            }
            // console.log(element, dim)
          }
        }
    }
  return dims;
}

function DrawIndicatorNew(IndControl) {
    // QUAY EDIT 3/19/16 Begin
    //var indicatorFile = indicatorImages[IndControl.IndicatorType];
    var indicatorFileName =  IndSetupData[IndControl.IndicatorType].filenames[IndImageIndex];
    // QUAY EDIT 3/19/16 Begin
    var canvas = IndControl.canvas,
    dims = IndControl.dims;
    draw(canvas, dims);
    //drawText(canvas, dims, indicatorFile.title);
    // QUAY EDIT 3/19/16 Begin
    //drawImage(IndControl, canvas, dims, indicatorFile.fileName, { current: IndControl.value, old: IndControl.value_old });
    drawImage(IndControl, canvas, dims, indicatorFileName, { current: IndControl.value, old: IndControl.value_old });
    // QUAY EDIT 3/19/16 End
    // console.log(dims)


    // STEPTOE EDIT 5/11/16 Begin
    if(IndControl.UpperTitle && !IndControl.TextDone){
        var indicatorDims = dims.indicator;
        var meterDims = dims.meter;
        var canvasDims = dims.canvas;

        // var y = indicatorDims.border + indicatorDims.height + meterDims.addHeight - meterDims.padding.bottom;

        var y = meterDims.height / 2 + indicatorDims.border + indicatorDims.height + meterDims.addHeight - meterDims.padding.bottom;

        if(IndControl.drawOldValue){
            var shuttleDims = dims.shuttle;
            y += shuttleDims.lineWidth * 3.5 + meterDims.height * (1 / 4) + meterDims.adjSeparation;
            console.log(IndControl.LowerTitle, y);
            drawTextAt(IndControl.textCanvas, IndControl.LowerTitle, y, meterDims.height / 2);
            IndControl.TextDone = true;
        }
        else{
            drawTextAt(IndControl.textCanvas, IndControl.UpperTitle, y, meterDims.height);
        }
    }
    // STEPTOE EDIT 5/11/16 End
}
