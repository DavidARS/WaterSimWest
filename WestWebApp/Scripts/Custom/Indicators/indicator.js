function IndicatorControl(divId, anIndicatorType, ControlId, options) {
    var defaults = {};

    //fill defaults with user specified options
    if (typeof (options) != "undefined") {
        for (var option in options) {
            defaults[option] = options[option];
        }
    }

    this.parentRef = '.indicators-';
    this.id = ControlId;
    this.DivID = divId;
    this.IndicatorType = anIndicatorType;
    this.svgID = this.id + "_svg";
    this.Title = IndSetupData[anIndicatorType].title;
    this.showTitle = defaults.hideTitle ? false : true;
    this.Description = "";
    this.MinValue = 0; // default
    this.value = 0;  // 0 to 100 valid
    this.value_old = 0;
    this.drawOldValue = false;
    this.drawCurrentValue = false;

    if (defaults.meter && defaults.meter.style == 'rgr_meter') {
        this.parentRef += 'efficiency';
        this.meter = 'rgr';
        this.MaxValue = 100; // default

        this.scaleValue = d3.scale.linear()
            .domain([0, 50, 100])
            .range([0, 100, 0]);

        this.scaleTick = d3.scale.linear()
            .domain([0, 40, 50, 60, 100])
            .range([-17.5, 94, 108, 120, 238.5]);
    }
    else {
        this.parentRef += 'sustainability';
        this.meter = 'rg';
        this.MaxValue = 100; // default
        this.scale = d3.scale.linear()
            .domain([0, 100])
            .range([349.5, 251.5]);
    }

    //console.log('IndicatorControl:', anIndicatorType, ' divExists:', $('#' + divId).length);

    // Check if div exists, if not create the div
    if (!$('#' + divId).length) {
        $(this.parentRef + ' .indicators-content').append('<div id="' + divId + '" class="indicator-container info-item" data-fld="' + anIndicatorType + '"></div>');
    }

    // Get Div refence
    //this.divObj = $(this.parentRef + ' .indicators-content').find('#' + divId);
    this.divObj = $('#' + divId);

    // This is a function used to set the value of the control and redraw
    this.SetValue = function (value) {
        //console.log('SetValue (', this.IndicatorType, '):', value);

        // Check if valid range
        if ((value >= this.MinValue) && (value <= this.MaxValue)) {
            // Set old value
            this.value_old = this.value;

            // Set the value
            this.value = value;

            if (defaults.meter && defaults.meter.style == 'rgr_meter') {
                this.displayValue = Math.round(this.scaleValue(value));
            }
            else {
                this.displayValue = value;
            }

            // Paint the control
            this.drawCurrentValue = true;
            this.Paint();

            // A value has been set, allow old value to show on next draw
            this.drawOldValue = true;
        }
    };

    // Paint the object
    this.Paint = function () {
        DrawIndicator(this);
    }

    // COMMENT OUT LATER TESTING
    //this.value = Math.random() * 100;
    //this.value_old = Math.random() * 100;
    //this.drawCurrentValue = true;
    //this.drawOldValue = true;

    // draw the indicator
    PrepareIndicator(this);
    DrawIndicator(this);
}

if(typeof(LOAD_IPAD) == 'undefined'){
    LOAD_IPAD = false;
}

function PrepareIndicator(indControl) {
    // Get the meter type
    var meter = indControl.meter;

    // Start with base svg
    var htmlInd = IndSetupData.svg;

    // Add rg vs rgr specific g
    htmlInd += IndSetupData[meter + '_g'];

    // Add the indicator image
    var typeData = IndSetupData[indControl.IndicatorType];
    htmlInd += IndSetupData[indControl.IndicatorType][(LOAD_IPAD ? (typeData['svg_ipad'] ? 'svg_ipad' : 'svg') : 'svg')];

    // Add the indicator title
    htmlInd += IndSetupData[meter + '_title'].replace('#TITLE#', indControl.Title);

    // Add the meter    
    htmlInd += IndSetupData[meter + '_meter'];

    // Add the previous and current values    
    htmlInd += IndSetupData[meter + '_previousValue'];
    htmlInd += IndSetupData[meter + '_currentValue'];

    // Close the g & svg
    htmlInd += IndSetupData.g_close + IndSetupData.svg_close;

    // Append the html for the indicator to the indicator div
    indControl.divObj.append(htmlInd);

    // Get the indicators svg and give it an id
    indControl.svgObj = $('#' + indControl.DivID + ' svg');
    indControl.svgObj.attr('id', indControl.svgID);

    // Get the current and previous value elements
    indControl.cValObj = indControl.svgObj.find('.indicator-value-current-' + indControl.meter);
    indControl.pValObj = indControl.svgObj.find('.indicator-value-previous-' + indControl.meter);

    // Get the current and previous text elements
    indControl.cValTspanObj = indControl.cValObj.find('tspan');
    indControl.pValTspanObj = indControl.pValObj.find('tspan');

    if (indControl.meter == 'rgr') {
        indControl.cValTextObj = indControl.cValTspanObj.parent();
        indControl.pValTextObj = indControl.pValTspanObj.parent();

        indControl.cValTickObj = indControl.cValObj.find('.indicator-value-current-tick');
        indControl.pValTickObj = indControl.pValObj.find('.indicator-value-previous-tick');
    }
}

// Update the Red Green Red Meter text value and rotate the tick
function updateMeterValueRGR(value, textObj, tspanObj, tickObj, indControl) {
    tspanObj.text(Math.round(indControl.scaleValue(value)) + '%');
    tickObj.attr('transform', tickObj.attr('transform').split('rotate')[0] + 'rotate(' + indControl.scaleTick(value) + ' 58 85)');
}

function DrawIndicator(indControl) {
    // If only drawing indicator but no value is provided do not draw value
    if (!indControl.drawCurrentValue)
        return;

    if (indControl.meter == 'rg') {
        var currentValue = Math.round(indControl.value);
        indControl.cValObj.attr('transform', 'translate(763 ' + indControl.scale(indControl.value) + ')');
        indControl.cValTspanObj.text(currentValue + '%');

        if (indControl.drawOldValue) {
            var oldValue = Math.round(indControl.value_old);
            indControl.pValObj.attr('transform', 'translate(' + (currentValue != oldValue ? 763 : 760) + ' ' + indControl.scale(indControl.value_old) + ')');
            indControl.pValTspanObj.text(oldValue + '%');
            $('.indicator-value-previous-rg').show();
        }
    }
    else {
        updateMeterValueRGR(indControl.value, indControl.cValTextObj, indControl.cValTspanObj, indControl.cValTickObj, indControl);

        if (indControl.drawOldValue) {
            updateMeterValueRGR(indControl.value_old, indControl.pValTextObj, indControl.pValTspanObj, indControl.pValTickObj, indControl);
            $('.indicator-value-previous-rgr').show();
        }        
    }
}

var IndSetupData = {
    ECOR_P: {
        svg: '<g transform="translate(741 147)"> \
                    <g transform="translate(28.9 20.136)"> \
                        <path class="economy-dollar" d="M41.062,43.726a2.908,2.908,0,0,0-.7-2.193,10.893,10.893,0,0,0-2.392-1.595,19.063,19.063,0,0,1-6.18-3.489A6.547,6.547,0,0,1,29.9,31.365a6.922,6.922,0,0,1,2.093-5.084,8.9,8.9,0,0,1,5.483-2.293V20.2h2.891v3.888a7.978,7.978,0,0,1,5.184,2.691,7.714,7.714,0,0,1,1.794,5.582v.1H41.361a4.548,4.548,0,0,0-.8-2.891,2.6,2.6,0,0,0-1.994-1,2.261,2.261,0,0,0-1.894.8,3.163,3.163,0,0,0-.6,1.994,2.715,2.715,0,0,0,.7,1.994,8.22,8.22,0,0,0,2.492,1.595,22.4,22.4,0,0,1,6.18,3.589,6.6,6.6,0,0,1,1.994,5.084,6.91,6.91,0,0,1-1.994,5.084A8.908,8.908,0,0,1,40.165,51v3.589h-3.19V50.9a9.052,9.052,0,0,1-5.782-2.492A7.725,7.725,0,0,1,29,42.23l.1-.1h5.981a4.086,4.086,0,0,0,1,3.19,3.306,3.306,0,0,0,2.492,1,2.621,2.621,0,0,0,1.994-.8A3.209,3.209,0,0,0,41.062,43.726Z" transform="translate(-28.992 -20.2)" /> \
                    </g> \
                    <g> \
                        <path class="economy-outline" d="M38.183,0a37.889,37.889,0,0,0-33,19.239L1.2,17.345,2.2,30.5l10.965-7.476L9.175,21.133A33.147,33.147,0,0,1,38.183,4.386,33.609,33.609,0,0,1,71.777,37.98h4.386A38.034,38.034,0,0,0,38.183,0Z" transform="translate(-0.004)" /> \
                        <path class="economy-outline" d="M37.88,74.98a37.889,37.889,0,0,0,33-19.239l3.987,1.894-1-13.158L62.9,51.953l3.987,1.894A33.5,33.5,0,0,1,4.386,37H0A37.949,37.949,0,0,0,37.88,74.98Z" transform="translate(0 -0.117)" /> \
                    </g> \
                </g>',
        title: 'Economy'
    },
    EVIND_P: {
        svg: '<g transform="translate(741 147)"> \
                <path class="environment-stem" d="M42.683,18.218l.1,1.047S34.975,26.5,31.169,40.1c0,0-1.142,10.181-.761,16.651,0,0,12.084-5.614,16.271-10.657l1.047.571S37.449,57.324,31.074,60.559c0,0,2.95,10.657,6.185,15.509l-7.041-.381a62.942,62.942,0,0,1-3.33-29.782S24.128,37.914,18.8,33.441l.571-.666s6.28,5.328,8.468,9.42c0,0,2.188-8.659,4.757-12.369,0,0-3.806-8.278-4.377-13.226h.571a37.473,37.473,0,0,0,4.853,11.608C33.738,28.3,37.925,22.119,42.683,18.218Z" transform="translate(-0.912 -0.805)"/> \
                <path class="environment-leaf-big" d="M0,14.4S3.14,44.182,19.886,39.8c0,0-5.138-3.045-6.09-11.7,0,0,10.276,1.427,11.7,6.28C25.5,34.381,32.351,18.967,0,14.4Z" transform="translate(0 -0.698)"/> \
                <path class="environment-leaf-small" d="M25.243,0s-7.8,14.653,1.427,17.983c0,0-1.047-4.757.856-7.231,0,0,3.616,2.379,3.711,6.09C31.333,16.841,39.23,11.323,25.243,0Z" transform="translate(-1.075)"/> \
                <path class="environment-leaf-small" d="M38.782,18.677S34.025,6.213,59.715,4.5c0,0-4.662,23.692-16.841,18.554,0,0,3.425-.856,5.328-8.563C48.107,14.491,40.21,15.062,38.782,18.677Z" transform="translate(-1.864 -0.218)"/> \
                <path class="environment-leaf-big" d="M37.482,49.547S31.2,29.947,71.45,28.9c0,0-8.563,36.537-27.879,28.069,0,0,4.472.856,8.944-13.226C52.611,43.648,40.527,43.648,37.482,49.547Z" transform="translate(-1.801 -1.402)"/> \
              </g>',
        title: 'Environment'
    },
    SWI: {
        filenames: ['./Assets/indicators/New_Images/surfacewater_button_grey.jpg', './Assets/indicators/New_Images/surfacewater_flat_white.jpg', './Assets/indicators/New_Images/surfacewater_flat_grey_color.jpg', './Assets/indicators/New_Images/surfacewater.jpg'],
        title: 'Surface Water'
    },
    GWSYA_P: {
        svg: '<g transform="translate(741 153)"> \
                <g transform="translate(0 31.328)"> \
                  <path class="groundwater-ground" d="M1.1,29.3l.321,12.3s7.164,1.711,9.3.855c6.308-2.566,12.937-.535,16.466,1.176C30.824,45.338,41.623,52.5,50.07,53.678c8.126,1.176,8.34,1.925,18.711,0,10.478-1.925,6.308-24.271,6.308-24.271H1.1Z" transform="translate(0.076 -29.3)"/> \
                  <path class="groundwater-water" d="M0,47.058a56.3,56.3,0,0,1,30.686,5.988c12.082,6.095,25.447,11.975,47.473.214a93.61,93.61,0,0,1-22.347,2.78c-10.371-.107-23.095-7.484-33.039-10.585C14.434,42.888,0,47.058,0,47.058Z" transform="translate(0 -28.24)"/> \
                  <path class="groundwater-water" d="M0,53.458a56.3,56.3,0,0,1,30.686,5.988c12.082,6.094,25.447,11.975,47.473.214a93.609,93.609,0,0,1-22.347,2.78c-10.371-.107-23.095-7.484-33.039-10.585C14.434,49.288,0,53.458,0,53.458Z" transform="translate(0 -27.797)"/> \
                </g> \
                <g transform="translate(8.447 8.238)"> \
                  <rect class="groundwater-house" width="23.105" height="9.935" transform="translate(3.764 12.3)"/> \
                  <path class="groundwater-house" d="M23.216,9.7,7.9,20.935H38.533Z" transform="translate(-7.9 -9.7)"/> \
                </g> \
                <g transform="translate(39.525)"> \
                  <path class="groundwater-ground" d="M46.9,17.3s-.321,9.041-4.17,9.283,15.5,0,15.5,0S54.28,26.1,54.708,17.3Z" transform="translate(-36.603 4.245)"/> \
                  <path class="groundwater-tree" d="M59.566,7.284V6.667a7.048,7.048,0,0,0-14.075,0v.617A6.912,6.912,0,0,0,38.7,13.952a6.883,6.883,0,0,0,7.038,6.667H58.825a6.806,6.806,0,0,0,7.038-6.667A6.82,6.82,0,0,0,59.566,7.284Z" transform="translate(-38.7)"/> \
                </g> \
              </g>',
        title: 'Groundwater'
    },
    UEF_P: {
        svg: '<g transform="translate(1514 156)"> \
                <path class="urban-body" d="M49.7,20.451a.96.96,0,0,0-1.057-.94s-13.158,0-13.393-.352c-.47-.822-.7-6.7-.822-7.754a.88.88,0,0,0-.822-.7H23.732a.99.99,0,0,0-.822.822c-.117,1.057-.822,7.871-1.41,7.989-.352,0-11.4.117-11.865.117H8.342a21.334,21.334,0,0,0-4.934.47A4.388,4.388,0,0,0,.236,23.153c-.47,1.527-.117,15.86-.117,15.86a1.19,1.19,0,0,0,.7,1.057,20.78,20.78,0,0,0,6.344.822,19.973,19.973,0,0,0,5.639-.7,1.059,1.059,0,0,0,.7-.7c.235-.94-.117-5.991.235-6.579,0,0,6.109-.587,7.4-.47a17,17,0,0,1,3.054.822,12.443,12.443,0,0,0,4.229,1.057h0a11.679,11.679,0,0,0,3.994-.94,10.854,10.854,0,0,1,3.172-.94c.94-.117,12.453,0,13.158,0a1.053,1.053,0,0,0,1.057-1.057C49.813,29.967,49.93,22.565,49.7,20.451Z" transform="translate(0 1.87)"/> \
                <path class="urban-handle" d="M30.042,10.926V8.106c.235-.235.47-.352.7-.587.235-.117.352-.352.587-.47.235,0,.47.117.7.117a22.886,22.886,0,0,1,2.7.7c.235.117.47.117.7.235a12.8,12.8,0,0,0,3.054.587h.7a3.963,3.963,0,0,0,3.524-4.229A4.156,4.156,0,0,0,39.088.47H38.5a12.336,12.336,0,0,0-4.112.94,10.911,10.911,0,0,1-1.527.47c-.117,0-.352.117-.47.117a4.36,4.36,0,0,1-1.057.235c-.117,0-.352-.235-.47-.47A4.958,4.958,0,0,0,29.454.587,6.849,6.849,0,0,0,26.517,0,7.7,7.7,0,0,0,23.58.587a4.439,4.439,0,0,0-1.41,1.057L21.818,2c-.94-.117-1.88-.47-2.937-.7a12.365,12.365,0,0,0-4.229-.94h-.7A4.459,4.459,0,0,0,10.657,2.82a4.223,4.223,0,0,0,.117,3.759,4.47,4.47,0,0,0,3.054,2.115h.822a11.707,11.707,0,0,0,4.112-.94,6.623,6.623,0,0,1,1.41-.47c.117,0,.352-.117.47-.117A4.36,4.36,0,0,1,21.7,6.931h0c.117,0,.235.235.47.47a6.965,6.965,0,0,0,.94.94v2.7Z" transform="translate(1.797 0)"/> \
                <path class="urban-water" d="M9.01,43.924c-.7-.94-2.7-3.524-2.82-3.524s-1.645,2.585-2.35,3.524a2.063,2.063,0,0,1-.47.587c-.822,1.175-3.289,4.7-.587,7.284A4.9,4.9,0,0,0,6.19,53.088a5.054,5.054,0,0,0,3.642-1.41,4.092,4.092,0,0,0,1.175-3.289C11.007,46.509,9.949,45.217,9.01,43.924Z" transform="translate(0.273 7.062)"/> \
              </g>',
        title: 'Cities & Towns'
    },
    PEF_P: {
        svg: '<g transform="translate(1524 156)"> \
              <rect class="power-base" width="16.664" height="2.997" transform="translate(10.045 45.284)"/> \
              <rect class="power-base" width="16.664" height="2.997" transform="translate(10.045 49.73)"/> \
              <path class="power-base" d="M48.087,181.3c2.108,0,3.82-1.12,3.82-2.5H44.3C44.3,180.183,45.98,181.3,48.087,181.3Z" transform="translate(-29.71 -119.915)"/> \
              <path class="power-base" d="M38.832,164.5H30.5c0,1.647,2.075,3,3.886,3h8.892c1.811,0,3.886-1.35,3.886-3Z" transform="translate(-20.455 -110.324)"/> \
              <path class="power-bulb" d="M18.377,0A18.384,18.384,0,0,0,0,18.377c0,9.781,5.961,13.6,8.826,18.9.988,1.811,1.219,6.488,1.219,6.488H26.709s.231-4.677,1.219-6.488c2.865-5.3,8.826-9.123,8.826-18.9A18.342,18.342,0,0,0,18.377,0ZM11.362,32.868,17.488,21.11l-7.674.066L25.425,5.862,19.3,17.587l7.674-.066Z"/> \
            </g>',
        title: 'Electricity'
    },
    IEF_P: {
        svg: '<g transform="translate(1514 124)"> \
                <path class="industry-building" d="M35.894,46.627V58L17.947,46.627V58l-5.069-3.151-.959-30.688s-.685-.959-2.877-.959-2.877.959-2.877.959L5.206,49.915,0,46.627V83.754H53.841V58Z" transform="translate(0 8.584)" /> \
                <path class="industry-smoke" d="M38.383,0A10.759,10.759,0,0,0,27.56,10.823h0a8.121,8.121,0,0,0-9.453,2.329,8.3,8.3,0,0,0-1.918,4.521,7.052,7.052,0,1,0,3.7,11.371,8.3,8.3,0,0,0,1.507-2.877,8.229,8.229,0,0,0,9.453-2.192A7.98,7.98,0,0,0,32.629,20a10.448,10.448,0,0,0,5.754,1.644A10.759,10.759,0,0,0,49.206,10.823,10.935,10.935,0,0,0,38.383,0Z" transform="translate(2.717)" /> \
                <g transform="translate(0 73.021)"> \
                    <rect class="industry-windows" width="11.919" height="5.754" transform="translate(6.165 0)" /> \
                    <rect class="industry-windows" width="11.919" height="5.754" transform="translate(20.961 0)" /> \
                    <rect class="industry-windows" width="11.919" height="5.754" transform="translate(35.757 0)" /> \
                </g> \
            </g>',
        title: 'Industry'
    },
    AGIND_P: {
        svg: '<g transform="translate(1514 128)"> \
                <g transform="translate(18.931)"> \
                  <rect class="agriculture-stem" width="1.185" height="12.88" transform="translate(9.43 2.924)"/> \
                  <rect class="agriculture-stem" width="1.185" height="12.88" transform="translate(12.196 7.349)"/> \
                  <path class="agriculture-leaf" d="M33.9,34.432c6.953.79,6.163-7.032,6.163-7.032l-2.845.869C31.843,30.4,33.9,34.432,33.9,34.432Z" transform="translate(-25.969 -5.749)"/> \
                  <path class="agriculture-leaf" d="M40.068,35.3l-2.845.869c-5.294,2.054-3.319,6.163-3.319,6.163C40.779,43.123,40.068,35.3,40.068,35.3Z" transform="translate(-25.975 -7.407)"/> \
                  <path class="agriculture-leaf" d="M40.068,43.6l-2.845.869c-5.294,2.054-3.319,6.163-3.319,6.163C40.779,51.423,40.068,43.6,40.068,43.6Z" transform="translate(-25.975 -9.149)"/> \
                  <path class="agriculture-leaf" d="M40.068,51.7l-2.845.869c-5.294,2.054-3.319,6.163-3.319,6.163C40.779,59.523,40.068,51.7,40.068,51.7Z" transform="translate(-25.975 -10.848)"/> \
                  <path class="agriculture-leaf" d="M30.156,34.432s1.975-4.109-3.319-6.163L23.993,27.4S23.2,35.223,30.156,34.432Z" transform="translate(-23.966 -5.749)"/> \
                  <path class="agriculture-leaf" d="M30.156,42.412s1.975-4.109-3.319-6.163L23.993,35.3S23.2,43.123,30.156,42.412Z" transform="translate(-23.966 -7.407)"/> \
                  <path class="agriculture-leaf" d="M30.156,50.632s1.975-4.109-3.319-6.163L23.993,43.6S23.2,51.423,30.156,50.632Z" transform="translate(-23.966 -9.149)"/> \
                  <path class="agriculture-leaf" d="M30.156,58.732s1.975-4.109-3.319-6.163L23.993,51.7S23.2,59.523,30.156,58.732Z" transform="translate(-23.966 -10.848)"/> \
                  <path class="agriculture-leaf" d="M32.2,27.276s3.793-1.5,1.5-6.084L32.278,18.9S27.063,23.641,32.2,27.276Z" transform="translate(-25.219 -3.966)"/> \
                  <rect class="agriculture-stem" width="1.185" height="38.955" transform="translate(6.348 48.753)"/> \
                  <rect class="agriculture-stem" width="1.185" height="12.88" transform="translate(6.348)"/> \
                  <rect class="agriculture-stem" width="1.185" height="12.88" transform="translate(3.267 2.924)"/> \
                  <rect class="agriculture-stem" width="1.185" height="12.88" transform="translate(0.58 7.349)"/> \
                </g> \
                <g transform="translate(37.895 23.705)"> \
                  <path class="agriculture-leaf" d="M57.8,64.532c6.953.79,6.163-7.032,6.163-7.032l-2.845.869C55.829,60.424,57.8,64.532,57.8,64.532Z" transform="translate(-49.954 -35.77)"/> \
                  <path class="agriculture-leaf" d="M63.967,65.4l-2.845.869c-5.294,2.054-3.319,6.163-3.319,6.163C64.758,73.223,63.967,65.4,63.967,65.4Z" transform="translate(-49.954 -37.428)"/> \
                  <path class="agriculture-leaf" d="M63.967,73.7l-2.845.869c-5.294,2.054-3.319,6.163-3.319,6.163C64.758,81.523,63.967,73.7,63.967,73.7Z" transform="translate(-49.954 -39.17)"/> \
                  <path class="agriculture-leaf" d="M63.967,81.7l-2.845.869c-5.294,2.054-3.319,6.163-3.319,6.163C64.758,89.523,63.967,81.7,63.967,81.7Z" transform="translate(-49.954 -40.848)"/> \
                  <path class="agriculture-leaf" d="M54.156,64.511s1.975-4.109-3.319-6.163L47.993,57.4S47.2,65.223,54.156,64.511Z" transform="translate(-47.966 -35.749)"/> \
                  <path class="agriculture-leaf" d="M54.156,72.432s1.975-4.109-3.319-6.163L47.993,65.4S47.2,73.223,54.156,72.432Z" transform="translate(-47.966 -37.428)"/> \
                  <path class="agriculture-leaf" d="M54.156,80.732s1.975-4.109-3.319-6.163L47.993,73.7S47.2,81.523,54.156,80.732Z" transform="translate(-47.966 -39.17)"/> \
                  <path class="agriculture-leaf" d="M54.156,88.732s1.975-4.109-3.319-6.163L47.993,81.7S47.2,89.523,54.156,88.732Z" transform="translate(-47.966 -40.848)"/> \
                  <path class="agriculture-leaf" d="M56.113,57.376s3.793-1.5,1.5-6.084L56.192,49C56.271,48.921,51.056,53.741,56.113,57.376Z" transform="translate(-49.212 -33.987)"/> \
                  <rect class="agriculture-stem" width="1.185" height="12.88" transform="translate(9.588 2.924)"/> \
                  <rect class="agriculture-stem" width="1.185" height="12.88" transform="translate(12.354 7.428)"/> \
                  <rect class="agriculture-stem" width="1.185" height="15.171" transform="translate(6.348 48.753)"/> \
                  <rect class="agriculture-stem" width="1.185" height="12.88" transform="translate(6.348)"/> \
                  <rect class="agriculture-stem" width="1.185" height="12.88" transform="translate(3.267 2.924)"/> \
                  <rect class="agriculture-stem" width="1.185" height="12.88" transform="translate(0.58 7.428)"/> \
                </g> \
                <g transform="translate(-0.026 23.705)"> \
                  <path class="agriculture-leaf" d="M9.9,64.532c6.953.79,6.163-7.032,6.163-7.032l-2.845.869C7.929,60.424,9.9,64.532,9.9,64.532Z" transform="translate(-1.982 -35.77)"/> \
                  <path class="agriculture-leaf" d="M16.068,65.4l-2.845.869C7.929,68.324,9.9,72.432,9.9,72.432,16.858,73.223,16.068,65.4,16.068,65.4Z" transform="translate(-1.982 -37.428)"/> \
                  <path class="agriculture-leaf" d="M16.068,73.7l-2.845.869C7.929,76.624,9.9,80.732,9.9,80.732,16.858,81.523,16.068,73.7,16.068,73.7Z" transform="translate(-1.982 -39.17)"/> \
                  <path class="agriculture-leaf" d="M16.068,81.7l-2.845.869C7.929,84.624,9.9,88.732,9.9,88.732,16.858,89.523,16.068,81.7,16.068,81.7Z" transform="translate(-1.982 -40.848)"/> \
                  <path class="agriculture-leaf" d="M6.158,64.511S8.133,60.4,2.839,58.348L-.006,57.4S-.717,65.223,6.158,64.511Z" transform="translate(0.026 -35.749)"/> \
                  <path class="agriculture-leaf" d="M6.158,72.432s1.975-4.109-3.319-6.163L-.006,65.4S-.717,73.223,6.158,72.432Z" transform="translate(0.026 -37.428)"/> \
                  <path class="agriculture-leaf" d="M6.158,80.732s1.975-4.109-3.319-6.163L-.006,73.7S-.717,81.523,6.158,80.732Z" transform="translate(0.026 -39.17)"/> \
                  <path class="agriculture-leaf" d="M6.158,88.732s1.975-4.109-3.319-6.163L-.006,81.7S-.717,89.523,6.158,88.732Z" transform="translate(0.026 -40.848)"/> \
                  <path class="agriculture-leaf" d="M8.2,57.376s3.793-1.5,1.5-6.084L8.278,49C8.278,48.921,3.063,53.741,8.2,57.376Z" transform="translate(-1.225 -33.987)"/> \
                  <rect class="agriculture-stem" width="1.185" height="12.88" transform="translate(9.661 2.924)"/> \
                  <rect class="agriculture-stem" width="1.185" height="12.88" transform="translate(12.348 7.428)"/> \
                  <rect class="agriculture-stem" width="1.185" height="15.171" transform="translate(6.342 48.753)"/> \
                  <rect class="agriculture-stem" width="1.185" height="12.88" transform="translate(6.342)"/> \
                  <rect class="agriculture-stem" width="1.185" height="12.88" transform="translate(3.34 2.924)"/> \
                  <rect class="agriculture-stem" width="1.185" height="12.88" transform="translate(0.653 7.428)"/> \
                </g> \
              </g>',
        svg_ipad: '<g transform="translate(1518 155)">\
                      <g transform="translate(8.874)">\
                        <path class="agriculture-corn-yellow-dark" d="M44.624,62.513l7.537-1.82a6.4,6.4,0,0,0,4.863-5.375,111.719,111.719,0,0,0,1-15.273c0-22.1-6-40.045-13.4-40.045S31.2,17.946,31.2,40.073A110.477,110.477,0,0,0,32.252,55.6a6.239,6.239,0,0,0,4.835,5.233Z" transform="translate(-31.2)"/>\
                        <path class="agriculture-corn-yellow-light" d="M78.7,0V62.513l8.845-2.133a4.248,4.248,0,0,0,3.242-3.527A110.328,110.328,0,0,0,92.01,40.045C92.01,18.031,86.066.142,78.7,0Z" transform="translate(-65.191)"/>\
                      </g>\
                      <path class="agriculture-corn-green" d="M22.383,122.366c0-1.308-5.2-37.542-22.383-38.566,0,3.242,3.47,25.142,8.532,32.565,5.176,7.565,13.851,6,13.851,6s8.674,1.564,13.851-6c5.062-7.395,9.9-28.412,8.532-32.565C25.568,88.635,22.383,121.057,22.383,122.366Z" transform="translate(0 -59.967)"/>\
                      <path class="agriculture-corn-white" d="M86.1,163.938s8.418-10.779,10.409-22.838C96.509,141.1,98.7,152.675,86.1,163.938Z" transform="translate(-61.612 -100.97)"/>\
                      <path class="agriculture-corn-white" d="M41.8,163.938S33.383,153.159,31.392,141.1C31.364,141.1,29.174,152.675,41.8,163.938Z" transform="translate(-22.348 -100.97)"/>\
                    </g>',
        title: 'Agriculture'
    },

    // STEPTOE EDIT 05_11_16 BEGIN
    MINFO: {
        upperTitle: 'Current',
        lowerTitle: 'Previous'
    },
    // STEPTOE EDIT 05_11_16 BEGIN

    rg_meter: '<g class="meter-rg" transform="translate(171 26)" style="border: 1px solid black;"> \
                <path class="indicator-meter-fill-color-0" d="M58.5,20H0V2.217C0,.988,2.109,0,4.734,0H53.766C56.391,0,58.5.988,58.5,2.217Z" transform="translate(575 235)" /> \
                <rect class="indicator-meter-fill-color-1" width="58.5" height="20" transform="translate(575 255)" /> \
                <rect class="indicator-meter-fill-color-2" width="58.5" height="20" transform="translate(575 275)" /> \
                <rect class="indicator-meter-fill-color-3" width="58.5" height="20" transform="translate(575 295)" /> \
                <path class="indicator-meter-fill-color-4" d="M53.766,351.5H4.734C2.109,351.5,0,350.512,0,349.283V331.5H58.5v17.783C58.5,350.512,56.391,351.5,53.766,351.5Z" transform="translate(575 -16.5)" /> \
              </g>',
    rgr_meter: '<g class="meter-rgr" transform="translate(155 0)" style="border: 1px solid black;"> \
                  <path class="indicator-meter-stroke-color-2" d="M16.234,48.523a37.586,37.586,0,0,1,8.5-12.8L18.118,29.2A46.974,46.974,0,0,0,7.2,46.156Z" transform="translate(1334.338 241.21)"/> \
                  <path class="indicator-meter-stroke-color-3" d="M10.583,85.914a37.377,37.377,0,0,1,1.5-12.995L3.1,70.6A46.784,46.784,0,0,0,1.308,86.493a43.205,43.205,0,0,0,.58,5.024l8.985-2.464C10.777,87.991,10.632,86.976,10.583,85.914Z" transform="translate(1337.428 219.81)"/> \
                  <path class="indicator-meter-stroke-color-4" d="M165.124,114.747a36.561,36.561,0,0,1-7.324,15.384l5.652,6.116a42.294,42.294,0,0,0,10.531-19Z" transform="translate(1256.49 198.065)"/> \
                  <path class="indicator-meter-stroke-color-2" d="M157.522,28.1,151,34.718a37.229,37.229,0,0,1,8.792,12.753l8.985-2.464A46.385,46.385,0,0,0,157.522,28.1Z" transform="translate(1260.005 241.779)"/> \
                  <path class="indicator-meter-stroke-color-3" d="M173.142,80.9a39.574,39.574,0,0,1-.242,7.2l9.034,2.367a46.59,46.59,0,0,0,.483-10.1A47.826,47.826,0,0,0,180.437,69.4L171.5,71.864A39.305,39.305,0,0,1,173.142,80.9Z" transform="translate(1249.408 220.43)"/> \
                  <path class="indicator-meter-stroke-color-0" d="M79.212,11.318a34.473,34.473,0,0,1,5.459-.725,36.123,36.123,0,0,1,9.179.58l2.319-8.985a47.51,47.51,0,0,0-12.077-.87A46.156,46.156,0,0,0,76.7,2.332Z" transform="translate(1298.412 255.663)"/> \
                  <path class="indicator-meter-stroke-color-1" d="M118.6,13.685A37.115,37.115,0,0,1,131.836,20.5l6.522-6.618A46.556,46.556,0,0,0,120.919,4.7Z" transform="translate(1276.753 253.875)"/> \
                  <path class="indicator-meter-stroke-color-4" d="M20.425,130.417A36.884,36.884,0,0,1,12.937,115.2L4,117.664a46.541,46.541,0,0,0,9.9,19.371Z" transform="translate(1335.992 196.755)"/> \
                  <path class="indicator-meter-stroke-color-1" d="M34.7,14.62l6.618,6.522a37.043,37.043,0,0,1,13.043-7L51.9,5.2A47.067,47.067,0,0,0,34.7,14.62Z" transform="translate(1320.123 253.616)"/> \
                  <path class="indicator-meter-fill-color-2" d="M15.793,48.317,5.6,45.66l.193-.628A47.527,47.527,0,0,1,16.9,27.835l.435-.435,7.488,7.391-.435.435a36.459,36.459,0,0,0-8.357,12.56Zm-8.6-3.478,7.874,2.029a38.641,38.641,0,0,1,7.971-12.077l-5.7-5.652A46.7,46.7,0,0,0,7.194,44.839Z" transform="translate(1335.165 242.141)"/> \
                  <path class="indicator-meter-fill-color-4" d="M163.588,136.574l-7.488-7.391.338-.435a36.419,36.419,0,0,0,7.294-15.12l.145-.628,10.193,2.657-.145.58a48.414,48.414,0,0,1-9.9,19.855Zm-5.845-7.488,5.749,5.7a45.891,45.891,0,0,0,9.082-18.212l-7.778-2.029A37.736,37.736,0,0,1,157.742,129.087Z" transform="translate(1257.369 198.892)"/> \
                  <path class="indicator-meter-fill-color-2" d="M158.575,47.266l-.193-.483a36.337,36.337,0,0,0-8.647-12.56l-.435-.435,7.391-7.488.435.435a47.9,47.9,0,0,1,11.4,17.1l.242.628Zm-7.584-13.526a37.938,37.938,0,0,1,8.261,12.029l7.826-2.174a46.812,46.812,0,0,0-10.435-15.652Z" transform="translate(1260.884 242.709)"/> \
                  <path class="indicator-meter-fill-color-3" d="M181.539,90.36,171.346,87.7l.048-.531a38.141,38.141,0,0,0,.242-7.053,36.567,36.567,0,0,0-1.642-8.889L169.8,70.6l10.145-2.8.193.58a48.408,48.408,0,0,1,2.029,11.111,47.79,47.79,0,0,1-.483,10.193Zm-8.84-3.575,7.874,2.029a45.976,45.976,0,0,0-1.4-19.516l-7.778,2.126a37.316,37.316,0,0,1,1.5,8.6h0A39.429,39.429,0,0,1,172.7,86.785Z" transform="translate(1250.287 221.257)"/> \
                  <path class="indicator-meter-fill-color-0" d="M77.9,11.416,75.1,1.271l.628-.145A50.392,50.392,0,0,1,83.264.112a47.1,47.1,0,0,1,12.222.87l.628.145L93.457,11.319l-.58-.1a35.275,35.275,0,0,0-9.034-.58,34.628,34.628,0,0,0-5.41.725ZM76.646,2.189l2.126,7.778A34.829,34.829,0,0,1,83.8,9.339a37.684,37.684,0,0,1,8.744.483L94.568,2a48.858,48.858,0,0,0-11.256-.725A63.128,63.128,0,0,0,76.646,2.189Z" transform="translate(1299.24 256.289)"/> \
                  <path class="indicator-meter-fill-color-1" d="M131.058,20.543l-.435-.338A36.852,36.852,0,0,0,117.58,13.49l-.58-.145L119.657,3.2l.58.145a46.786,46.786,0,0,1,17.681,9.275l.531.435ZM118.5,12.475A37.336,37.336,0,0,1,130.961,18.9l5.7-5.749A45.355,45.355,0,0,0,120.526,4.7Z" transform="translate(1277.58 254.65)"/> \
                  <path class="indicator-meter-fill-color-4" d="M13.079,137.178l-.435-.483a46.82,46.82,0,0,1-10-19.613L2.5,116.5l10.145-2.8.145.628a36.666,36.666,0,0,0,7.343,14.927l.338.435Zm-9.13-19.806a46.278,46.278,0,0,0,9.179,18.019l5.7-5.749a37.587,37.587,0,0,1-7.1-14.4Z" transform="translate(1336.768 197.53)"/> \
                  <path class="indicator-meter-fill-color-1" d="M40.336,21.184,32.8,13.793l.483-.435A47.33,47.33,0,0,1,50.771,3.793l.58-.193,2.8,10.145-.58.193a35.692,35.692,0,0,0-12.85,6.908Zm-5.7-7.343,5.749,5.7a38.253,38.253,0,0,1,12.27-6.618L50.529,5.146A45.64,45.64,0,0,0,34.636,13.841Z" transform="translate(1321.105 254.443)"/> \
                  <path class="indicator-meter-fill-color-3" d="M.781,91.515l-.1-.676A44.872,44.872,0,0,1,.1,85.718,47.105,47.105,0,0,1,1.892,69.68l.145-.58L12.23,71.757l-.145.58a36.686,36.686,0,0,0-1.5,12.753q.072,1.522.29,3.043l.1.531ZM2.906,70.549A46.471,46.471,0,0,0,1.312,85.621c.1,1.449.242,2.85.435,4.3l7.826-2.174c-.1-.87-.193-1.739-.242-2.609h0A38.184,38.184,0,0,1,10.684,72.53Z" transform="translate(1338.052 220.585)"/> \
                  <ellipse class="indicator-meter-elipse" cx="11.304" cy="11.304" rx="11.304" ry="11.304" transform="translate(1374 295.53)"/> \
                </g>',
    fillstyle: { color: "white" },
    svg: '<svg xmlns="http://www.w3.org/2000/svg" viewBox="3655 99 154.5 243">',
    svg_close: '</svg>',
    rg_g: '<g class="main-g" transform="translate(2954.5 -31)">',
    rgr_g: '<g class="main-g" transform="translate(2193 -24.615)">',
    g_close: '</g>',
    rg_title: '<text class="indicator-title" transform="translate(775.5 245)" text-anchor="middle"> \
                <tspan x="0" y="0">#TITLE#</tspan> \
            </text>',
    rgr_title: '<text class="indicator-title" transform="translate(1541 238.586)" text-anchor="middle"> \
                <tspan x="0" y="0">#TITLE#</tspan> \
            </text>',
    rg_currentValue: '<g class="indicator-value-current-rg" transform="translate(763 349.5)"> \
                        <path class="indicator-value-current-tick" d="M44.763,0H14.23a2.732,2.732,0,0,0-.865.138V.115l-.137.069a3.29,3.29,0,0,0-.774.459L0,10.511l12.454,9.846a2.047,2.047,0,0,0,.774.459l.137.069v-.023A2.732,2.732,0,0,0,14.23,21H44.763a2.186,2.186,0,0,0,2.163-2.18V2.18A2.186,2.186,0,0,0,44.763,0Z" /> \
                        <path class="indicator-value-background" d="M90.9,24.5H60.751A.762.762,0,0,1,60,23.74V7.26a.762.762,0,0,1,.751-.76H90.9a.762.762,0,0,1,.751.76V23.717A.768.768,0,0,1,90.9,24.5Z" transform="translate(-46.5 -5)" /> \
                        <text data-name="80" class="indicator-value-current-text" text-anchor="middle" transform="translate(29 15)"> \
                            <tspan x="0" y="0">...</tspan> \
                        </text> \
                    </g>',
    rg_previousValue: '<g class="indicator-value-previous-rg" transform="translate(763 349.5)"> \
                        <path class="indicator-value-previous-tick" d="M44.763,0H14.23a2.732,2.732,0,0,0-.865.138V.115l-.137.069a3.29,3.29,0,0,0-.774.459L0,10.511l12.454,9.846a2.047,2.047,0,0,0,.774.459l.137.069v-.023A2.732,2.732,0,0,0,14.23,21H44.763a2.186,2.186,0,0,0,2.163-2.18V2.18A2.186,2.186,0,0,0,44.763,0Z" /> \
                        <path class="indicator-value-background" d="M90.9,24.5H60.751A.762.762,0,0,1,60,23.74V7.26a.762.762,0,0,1,.751-.76H90.9a.762.762,0,0,1,.751.76V23.717A.768.768,0,0,1,90.9,24.5Z" transform="translate(-46.5 -5)" /> \
                        <text data-name="80" class="indicator-value-previous-text" text-anchor="middle" transform="translate(29 15)"> \
                            <tspan x="0" y="0">...</tspan> \
                        </text> \
                    </g>',
    /*rg_previousValue: '<g class="indicator-value-previous-rg" transform="translate(806 320)"> \
                        <path class="indicator-value-previous-tick" d="M13.817,0a2.583,2.583,0,0,0-.84.138V.115l-.133.069a3.176,3.176,0,0,0-.752.459L0,10.511l12.093,9.846a1.976,1.976,0,0,0,.752.459l.133.069v-.023a2.583,2.583,0,0,0,.84.138H62.895a2.309,2.309,0,0,0,2.255-2.341V2.341A2.309,2.309,0,0,0,62.895,0Z" /> \
                        <path class="indicator-value-background" d="M113.2,24.7H65.23a.752.752,0,0,1-.73-.76V7.46a.752.752,0,0,1,.73-.76H113.2a.752.752,0,0,1,.73.76V23.917A.73.73,0,0,1,113.2,24.7Z" transform="translate(-50.6 -5.2)" /> \
                        <text data-name="30" class="indicator-value-previous-text" transform="translate(17 14)"> \
                            <tspan x="0" y="0">Previous</tspan> \
                        </text> \
                    </g>',*/
    rgr_currentValue: '<g class="indicator-value-current-rgr" transform="translate(1309 -20.5)"> \
                        <text class="indicator-value-current-text" transform="translate(232 354)" text-anchor="middle"><tspan x="0" y="0">...</tspan></text> \
                        <path class="indicator-value-current-tick" d="M57.3,82.4l1.151,3.137-8.258,3.017-37.757,12.7L12,100.147l37.043-14.73Z" transform="translate(180, 242) scale(0.9 1) rotate(-17.5 58 85)"/> \
                      </g>',
    rgr_previousValue: '<g class="indicator-value-previous-rgr" transform="translate(1309 -20.5)"> \
                        <text class="indicator-value-previous-text" transform="translate(232 369)" text-anchor="middle"><tspan x="0" y="0">...</tspan></text> \
                        <path class="indicator-value-previous-tick" d="M57.3,82.4l1.151,3.137-8.258,3.017-37.757,12.7L12,100.147l37.043-14.73Z" transform="translate(180, 242) scale(0.9 1) rotate(238.5 58 85)"/> \
                      </g>'
}
