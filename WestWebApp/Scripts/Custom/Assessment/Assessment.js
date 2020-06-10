function clearAssessment() {
    $('.assessment-indicator').remove();
}

function appendAssessmentField(field) {
    $('.assessment').append(AssessmentSetupData[field]);
}

function drawAssessment(jsondata) {
    var indicatorDisplayed = STC[providerRegion].INDFLDS;

    var scaleValue = d3.scale.linear()
            .domain([0, 50, 100])
            .range([0, 100, 0]);

    var rgrScale = d3.scale.linear()
            .domain([0, 100])
            .range([4, 0]);

    $.each(jsondata.RESULTS, function () {
        if (indicatorDisplayed.indexOf(this.FLD) > -1) {
            var values = this.VALS[0].VALS;
            var lastIndex = values.length - 1;
            var value = values[lastIndex];
            var textIndex = 0;
            var container = $('.assessment-indicator[data-fld=' + this.FLD + ']');

            if (value < 20) {
                textIndex = 1;
            }
            else if (value < 40) {
                textIndex = 2;
            }
            else if (value < 60) {
                textIndex = 3;
            }
            else if (value < 80) {
                textIndex = 4;
            }
            else {
                textIndex = 5;
            }

            container.children('.assessment-indicator-prefix').text(AssessmentText[0][this.FLD]);
            container.children('.assessment-indicator-info').text(AssessmentText[textIndex][this.FLD]);


            if (indicatorParameters[this.FLD].options && indicatorParameters[this.FLD].options.meter && indicatorParameters[this.FLD].options.meter.style == 'rgr_meter') {
                //console.log('drawAssessment:', this.FLD, value, Math.round(rgrScale(scaleValue(value))), AssessmentText[textIndex][this.FLD]);
                //rgrScale(scaleValue(value))
                var scaledValue = scaleValue(value);
                var colorIndex = 0;

                if (scaledValue < 20) {
                    colorIndex = 4;
                }
                else if (scaledValue < 40) {
                    colorIndex = 3;
                }
                else if (scaledValue < 60) {
                    colorIndex = 2;
                }
                else if (scaledValue < 80) {
                    colorIndex = 1;
                }
                else {
                    colorIndex = 0;
                }

                container.find('[class*=indicator-meter-fill-color-]').each(function () {
                    var item = $(this);
                    item.attr('class', 'indicator-meter-fill-color-' + colorIndex);
                });
            }
            else {
                //console.log('drawAssessment:', this.FLD, value, (5 - textIndex), AssessmentText[textIndex][this.FLD]);
                container.find('[class*=indicator-meter-fill-color-]').each(function () {
                    var item = $(this);
                    item.attr('class', 'indicator-meter-fill-color-' + (5 - textIndex));
                });
            }
        }
        if (AssessmentFields.indexOf(this.FLD) > -1) {
            var values = this.VALS[0].VALS;
            var lastIndex = values.length - 1;
            var value = values[lastIndex];

            var textIndex = 0;
            var colorIndex = 0;
            var statusText = '';
            var container = $('span[data-fld=' + this.FLD + ']');
            var ranges = AssessmentFieldRanges[this.FLD];
            var phrases = AssessmentFieldPhrases[this.FLD];

            if (value >= ranges[0]) {
                textIndex = 0;
                colorIndex = 0;
            }
            else if (value >= ranges[1]) {
                textIndex = 1;
                colorIndex = 1;
            }
            else if (value >= ranges[2]) {
                textIndex = 2;
                colorIndex = 2;
            }
            else {
                textIndex = 3;
                colorIndex = 4;
            }

            statusText = phrases[textIndex];
            
            if ('SAI_P' == this.FLD) {
                //statusText = '<span style="font-size:20px;color:#4e4d4d;">(' + value + ')</span> ' + statusText;

                //container.attr('class', 'assessment-status indicator-meter-color-color-' + colorIndex);
            }

            //console.log('drawAssessment:', this.FLD, this);
            container.html(statusText);
        }
    });
}

var AssessmentSetupData = {
    ECOR_P: '<div class="assessment-indicator" data-fld="ECOR_P">\
                <svg viewBox="400 400 40 40">\
                    <g transform="translate(415 412)">\
                        <circle class="indicator-meter-fill-color-white" cx="18.5" cy="18.5" r="18.5" transform="translate(-14.406 -10.689)" />\
                        <path class="assessment-white" d="M44.612,37.316a1.363,1.363,0,0,0-.335-1,3.033,3.033,0,0,0-1.115-.706,9.4,9.4,0,0,1-2.862-1.6,3.414,3.414,0,0,1,.074-4.683A4.264,4.264,0,0,1,42.9,28.247V26.5h1.3v1.784a3.792,3.792,0,0,1,2.379,1.227,3.729,3.729,0,0,1,.855,2.6v.037h-2.75a2.218,2.218,0,0,0-.335-1.338,1.128,1.128,0,0,0-.929-.446,1.055,1.055,0,0,0-.892.372,1.541,1.541,0,0,0-.3.929,1.463,1.463,0,0,0,.3.929,3.076,3.076,0,0,0,1.152.706,10.872,10.872,0,0,1,2.825,1.635,3.466,3.466,0,0,1-.037,4.683,4.019,4.019,0,0,1-2.453,1.041v1.673h-1.3V40.661a4.606,4.606,0,0,1-2.676-1.115A3.655,3.655,0,0,1,39,36.684l.037-.037h2.75a2.031,2.031,0,0,0,.446,1.487,1.6,1.6,0,0,0,1.152.446,1.1,1.1,0,0,0,.892-.335A1.3,1.3,0,0,0,44.612,37.316Z" transform="translate(-39 -26.5)" />\
                    </g>\
                </svg>\
                <span class="assessment-indicator-prefix">...</span>\
                <span class="assessment-indicator-info">...</span>\
            </div>',
    EVIND_P: '<div class="assessment-indicator" data-fld="EVIND_P">\
                <svg viewBox="400 400 40 40">\
                    <g transform="translate(403 407)">\
                        <circle class="indicator-meter-fill-color-white" cx="18.5" cy="18.5" r="18.5" transform="translate(-2.31 -6.042)" />\
                        <path class="assessment-white" d="M42.159,188.454l.038.423a19.287,19.287,0,0,0-4.695,8.428,50.833,50.833,0,0,0-.308,6.734s4.887-2.27,6.58-4.31l.423.231s-4.156,4.31-6.734,5.618a29.185,29.185,0,0,0,2.5,6.273l-2.848-.154a25.456,25.456,0,0,1-1.347-12.045,13.166,13.166,0,0,0-3.271-5.041l.231-.269a18.231,18.231,0,0,1,3.425,3.81,19.325,19.325,0,0,1,1.924-5,24.148,24.148,0,0,1-1.77-5.349h.231a15.156,15.156,0,0,0,1.963,4.695A30.844,30.844,0,0,1,42.159,188.454Z" transform="translate(-25.265 -181.412)" />\
                        <path class="assessment-white" d="M13.7,185.6s1.231,12.045,8.043,10.275a6.189,6.189,0,0,1-2.463-4.695s4.156.577,4.733,2.54C24.013,193.681,26.822,187.447,13.7,185.6Z" transform="translate(-13.7 -180.059)" />\
                        <path class="assessment-white" d="M37.208,171.2s-3.156,5.926.577,7.273c0,0-.423-1.924.346-2.925a3.457,3.457,0,0,1,1.5,2.463S42.827,175.779,37.208,171.2Z" transform="translate(-27.395 -171.2)" />\
                        <path class="assessment-white" d="M52.275,181.434s-1.924-5.041,8.466-5.734c0,0-1.886,9.582-6.811,7.5,0,0,1.385-.346,2.155-3.463C56.047,179.741,52.853,179.971,52.275,181.434Z" transform="translate(-37.344 -173.968)" />\
                        <path class="assessment-white" d="M50.97,208.451s-2.54-7.927,13.738-8.351c0,0-3.463,14.777-11.237,11.352,0,0,1.809.346,3.617-5.349C57.089,206.065,52.2,206.065,50.97,208.451Z" transform="translate(-36.54 -188.979)" />\
                    </g>\
                </svg>\
                <span class="assessment-indicator-prefix">...</span>\
                <span class="assessment-indicator-info">...</span>\
            </div>',
    GWSYA_P: '<div class="assessment-indicator" data-fld="GWSYA_P">\
                <svg viewBox="400 400 40 40">\
                    <g transform="translate(405 408)">\
                        <circle class="indicator-meter-fill-color-white" cx="18.5" cy="18.5" r="18.5" transform="translate(-3.849 -6.142)" />\
                        <g transform="translate(0 11.352)">\
                            <path class="assessment-white" d="M11,365.8l.117,4.471s2.6.622,3.382.311a7.617,7.617,0,0,1,5.987.428c1.322.622,5.248,3.227,8.319,3.654,2.955.428,3.032.7,6.8,0,3.81-.7,2.294-8.825,2.294-8.825H11Z" transform="translate(-10.572 -365.8)" />\
                            <path class="assessment-white" d="M9.9,382.106a20.469,20.469,0,0,1,11.157,2.177c4.393,2.216,9.252,4.354,17.261.078a34.034,34.034,0,0,1-8.125,1.011c-3.771-.039-8.4-2.721-12.013-3.849C15.148,380.59,9.9,382.106,9.9,382.106Z" transform="translate(-9.9 -375.225)" />\
                            <path class="assessment-white" d="M9.9,388.506a20.469,20.469,0,0,1,11.157,2.177c4.393,2.216,9.252,4.354,17.261.078a34.037,34.037,0,0,1-8.125,1.011c-3.771-.039-8.4-2.721-12.013-3.849C15.148,386.99,9.9,388.506,9.9,388.506Z" transform="translate(-9.9 -379.137)" />\
                        </g>\
                        <g transform="translate(3.071 3.732)">\
                            <rect class="assessment-white" width="6.92" height="3.266" transform="translate(1.127 4.082)" />\
                            <path class="assessment-white" d="M22.387,346.2,17.8,349.932h9.214Z" transform="translate(-17.8 -346.2)" />\
                        </g>\
                        <g transform="translate(15.006 0)">\
                            <path class="assessment-white" d="M53.827,353.8s-.117,4.354-1.516,4.471,5.637,0,5.637,0-1.438-.233-1.283-4.471Z" transform="translate(-50.717 -347.113)" />\
                            <path class="assessment-white" d="M55.109,338.894V338.7a2.167,2.167,0,0,0-2.216-2.1,2.2,2.2,0,0,0-2.255,2.06v.194a2.1,2.1,0,1,0,.078,4.2h4.121a2.143,2.143,0,0,0,2.216-2.1A2.048,2.048,0,0,0,55.109,338.894Z" transform="translate(-48.5 -336.6)" />\
                        </g>\
                    </g>\
                </svg>\
                <span class="assessment-indicator-prefix">...</span>\
                <span class="assessment-indicator-info">...</span>\
            </div>',
    UEF_P: '<div class="assessment-indicator" data-fld="UEF_P">\
                <svg viewBox="400 400 40 40">\
                    <g transform="translate(382 376)">\
                        <circle class="indicator-meter-fill-color-white" cx="18.5" cy="18.5" r="18.5" transform="translate(18.247 25.508)" />\
                        <path class="assessment-white" d="M22.069,15.031a.426.426,0,0,0-.47-.417s-5.844,0-5.949-.157a24.009,24.009,0,0,1-.365-3.444.391.391,0,0,0-.365-.313H10.537a.44.44,0,0,0-.365.365c-.052.47-.365,3.5-.626,3.548-.157,0-5.062.052-5.27.052H3.7a9.476,9.476,0,0,0-2.192.209A1.949,1.949,0,0,0,.1,16.231c-.209.678-.052,7.044-.052,7.044a.528.528,0,0,0,.313.47,9.23,9.23,0,0,0,2.818.365,8.872,8.872,0,0,0,2.5-.313A.47.47,0,0,0,6,23.484c.1-.417-.052-2.661.1-2.922,0,0,2.713-.261,3.287-.209a7.549,7.549,0,0,1,1.357.365,5.527,5.527,0,0,0,1.879.47h0a5.188,5.188,0,0,0,1.774-.417,4.821,4.821,0,0,1,1.409-.417c.417-.052,5.531,0,5.844,0a.468.468,0,0,0,.47-.47C22.121,19.258,22.173,15.97,22.069,15.031Z" transform="translate(25.487 25.636)" />\
                        <path class="assessment-white" d="M19.053,4.853V3.6c.1-.1.209-.157.313-.261.1-.052.157-.157.261-.209.1,0,.209.052.313.052a10.165,10.165,0,0,1,1.2.313c.1.052.209.052.313.1a5.683,5.683,0,0,0,1.357.261h.313a1.76,1.76,0,0,0,1.565-1.879A1.846,1.846,0,0,0,23.071.209H22.81a5.479,5.479,0,0,0-1.826.417,4.847,4.847,0,0,1-.678.209c-.052,0-.157.052-.209.052a1.936,1.936,0,0,1-.47.1c-.052,0-.157-.1-.209-.209a2.2,2.2,0,0,0-.626-.522A3.042,3.042,0,0,0,17.487,0a3.418,3.418,0,0,0-1.3.261,1.972,1.972,0,0,0-.626.47L15.4.887c-.417-.052-.835-.209-1.3-.313A5.492,5.492,0,0,0,12.217.157H11.9a1.981,1.981,0,0,0-1.461,1.1,1.876,1.876,0,0,0,.052,1.67,1.986,1.986,0,0,0,1.357.939h.365a5.2,5.2,0,0,0,1.826-.417,2.942,2.942,0,0,1,.626-.209c.052,0,.157-.052.209-.052a1.936,1.936,0,0,1,.47-.1h0c.052,0,.1.1.209.209a3.094,3.094,0,0,0,.417.417v1.2Z" transform="translate(20.571 30.752)" />\
                        <path class="assessment-white" d="M4.864,41.965c-.313-.417-1.2-1.565-1.252-1.565s-.731,1.148-1.044,1.565a.916.916,0,0,1-.209.261C1.994,42.748.9,44.314,2.1,45.462a2.177,2.177,0,0,0,1.513.574,2.245,2.245,0,0,0,1.618-.626,1.817,1.817,0,0,0,.522-1.461A3.316,3.316,0,0,0,4.864,41.965Z" transform="translate(24.741 11.434)" />\
                    </g>\
                </svg>\
                <span class="assessment-indicator-prefix">...</span>\
                <span class="assessment-indicator-info">...</span>\
            </div>',
    AGIND_P: '<div class="assessment-indicator" data-fld="AGIND_P">\
                <svg viewBox="400 400 40 40">\
                    <g transform="translate(415 405)">\
                        <circle class="indicator-meter-fill-color-white" cx="18.5" cy="18.5" r="18.5" transform="translate(-14.144 -3.407)" />\
                        <g>\
                            <path class="assessment-white" d="M57.658,60.984A3.039,3.039,0,0,0,61.32,57.5l-1.69.431C56.484,58.948,57.658,60.984,57.658,60.984Z" transform="translate(-52.994 -46.736)" />\
                            <path class="assessment-white" d="M61.32,65.4l-1.69.431c-3.145,1.018-1.972,3.053-1.972,3.053A3.039,3.039,0,0,0,61.32,65.4Z" transform="translate(-52.994 -51.544)" />\
                            <path class="assessment-white" d="M61.32,73.7l-1.69.431c-3.145,1.018-1.972,3.053-1.972,3.053A3.039,3.039,0,0,0,61.32,73.7Z" transform="translate(-52.994 -56.595)" />\
                            <path class="assessment-white" d="M61.32,81.7l-1.69.431c-3.145,1.018-1.972,3.053-1.972,3.053A3.039,3.039,0,0,0,61.32,81.7Z" transform="translate(-52.994 -61.464)" />\
                            <path class="assessment-white" d="M51.644,60.923s1.174-2.035-1.972-3.053l-1.69-.47A3.072,3.072,0,0,0,51.644,60.923Z" transform="translate(-47.966 -46.675)" />\
                            <path class="assessment-white" d="M51.644,68.884s1.174-2.035-1.972-3.053l-1.69-.431A3.039,3.039,0,0,0,51.644,68.884Z" transform="translate(-47.966 -51.544)" />\
                            <path class="assessment-white" d="M51.644,77.184s1.174-2.035-1.972-3.053l-1.69-.431A3.039,3.039,0,0,0,51.644,77.184Z" transform="translate(-47.966 -56.595)" />\
                            <path class="assessment-white" d="M51.644,85.184s1.174-2.035-1.972-3.053l-1.69-.431A3.039,3.039,0,0,0,51.644,85.184Z" transform="translate(-47.966 -61.464)" />\
                            <path class="assessment-white" d="M55.216,53.148a1.871,1.871,0,0,0,.892-3.014L55.263,49C55.31,48.96,52.212,51.348,55.216,53.148Z" transform="translate(-51.116 -41.562)" />\
                            <rect class="assessment-white" width="0.704" height="6.38" transform="translate(5.697 1.448)" />\
                            <rect class="assessment-white" width="0.704" height="6.38" transform="translate(7.34 3.679)" />\
                            <rect class="assessment-white" width="0.704" height="7.515" transform="translate(3.772 24.151)" />\
                            <rect class="assessment-white" width="0.704" height="6.38" transform="translate(3.772)" />\
                            <rect class="assessment-white" width="0.704" height="6.38" transform="translate(1.941 1.448)" />\
                            <rect class="assessment-white" width="0.704" height="6.38" transform="translate(0.345 3.679)" />\
                        </g>\
                    </g>\
                </svg>\
                <span class="assessment-indicator-prefix">...</span>\
                <span class="assessment-indicator-info">...</span>\
            </div>',
    AGIND_P_IPAD: '<div class="assessment-indicator" data-fld="AGIND_P">\
                <svg viewBox="400 400 40 40">\
                    <g transform="translate(415 405)">\
                        <circle class="indicator-meter-fill-color-white" cx="18.5" cy="18.5" r="18.5" transform="translate(-14.144 -3.407)" />\
                        <g transform="translate(-5 1)">\
                          <path  class="assessment-white" d="M9.953,100.949c0-.582-2.314-16.694-9.953-17.149,0,1.442,1.543,11.18,3.794,14.481a6.427,6.427,0,0,0,6.159,2.668,6.427,6.427,0,0,0,6.159-2.668c2.251-3.288,4.4-12.634,3.794-14.481C11.369,85.95,9.953,100.367,9.953,100.949Z" transform="translate(0 -73.202)"/>\
                          <path  class="assessment-white" d="M37.169,27.8l3.351-.809a2.845,2.845,0,0,0,2.163-2.39,49.678,49.678,0,0,0,.443-6.791C43.126,7.98,40.457,0,37.169,0S31.2,7.98,31.2,17.819a49.126,49.126,0,0,0,.468,6.905,2.774,2.774,0,0,0,2.15,2.327Z" transform="translate(-27.254)"/>\
                          <path  class="assessment-white" d="M78.7,0V27.8l3.933-.949a1.889,1.889,0,0,0,1.442-1.568,49.06,49.06,0,0,0,.544-7.474C84.619,8.018,81.976.063,78.7,0Z" transform="translate(-68.747)"/>\
                          <path  class="indicator-meter-fill-color-white" style="fill-rule: evenodd;"  d="M86.1,151.255A25.973,25.973,0,0,0,90.729,141.1S91.7,146.247,86.1,151.255Z" transform="translate(-75.211 -123.255)"/>\
                          <path  class="indicator-meter-fill-color-white" style="fill-rule: evenodd;" d="M35.931,151.255A25.974,25.974,0,0,1,31.3,141.1C31.289,141.1,30.316,146.247,35.931,151.255Z" transform="translate(-27.28 -123.255)"/>\
                        </g>\
                    </g>\
                </svg>\
                <span class="assessment-indicator-prefix">...</span>\
                <span class="assessment-indicator-info">...</span>\
            </div>',
    IEF_P: '<div class="assessment-indicator" data-fld="IEF_P">\
                <svg viewBox="400 400 40 40">\
                    <g transform="translate(407 406)">\
                        <circle class="indicator-meter-fill-color-white" cx="18.5" cy="18.5" r="18.5" transform="translate(-5.707 -5.281)" />\
                        <path class="assessment-white" d="M17.235,29.375v3l-8.618-3v3l-2.434-.831-.46-8.088A2.476,2.476,0,0,0,4.342,23.2a2.476,2.476,0,0,0-1.381.253L2.5,30.241,0,29.375V39.16H25.853V32.372Z" transform="translate(0 -14.823)" />\
                        <path class="assessment-white" d="M22.248,0c-2.894,0-5.2,1.264-5.2,2.853h0a6.226,6.226,0,0,0-4.539.614,1.906,1.906,0,0,0-.921,1.192,5.287,5.287,0,0,0-3.421.578c-1.25.794-1.053,1.986.395,2.636a5.442,5.442,0,0,0,4.8-.217,2.2,2.2,0,0,0,.724-.758,6.358,6.358,0,0,0,4.539-.578,1.89,1.89,0,0,0,.855-1.047,8.3,8.3,0,0,0,2.763.433c2.894,0,5.2-1.264,5.2-2.853C27.379,1.264,25.076,0,22.248,0Z" transform="translate(-2.513)" />\
                        <g>\
                            <rect class="indicator-meter-fill-color-white" width="5.723" height="1.517" transform="translate(2.96 19.246)" />\
                            <rect class="indicator-meter-fill-color-white" width="5.723" height="1.517" transform="translate(10.065 19.246)" />\
                            <rect class="indicator-meter-fill-color-white" width="5.723" height="1.517" transform="translate(17.169 19.246)" />\
                        </g>\
                    </g>\
                </svg>\
                <span class="assessment-indicator-prefix">...</span>\
                <span class="assessment-indicator-info">...</span>\
            </div>',
    PEF_P: '<div class="assessment-indicator" data-fld="PEF_P">\
                <svg viewBox="400 400 40 40">\
                    <g transform="translate(405 403)">\
                        <circle class="indicator-meter-fill-color-white" cx="18.5" cy="18.5" r="18.5" transform="translate(-4.111 -0.979)" />\
                        <g transform="translate(6 3)">\
                          <path class="assessment-white" d="M0,0H7.753V1.394H0Z" transform="translate(4.673 21.068)"/>\
                          <path class="assessment-white" d="M0,0H7.753V1.394H0Z" transform="translate(4.673 23.136)"/>\
                          <path class="assessment-white" d="M46.062,179.964c.981,0,1.777-.521,1.777-1.164H44.3C44.3,179.444,45.081,179.964,46.062,179.964Z" transform="translate(-37.512 -151.404)"/>\
                          <path class="assessment-white" d="M34.376,164.5H30.5c0,.766.965,1.394,1.808,1.394h4.137c.843,0,1.808-.628,1.808-1.394Z" transform="translate(-25.827 -139.295)"/>\
                          <path class="assessment-white" d="M8.55,0A8.553,8.553,0,0,0,0,8.55c0,4.551,2.773,6.328,4.106,8.795a10.148,10.148,0,0,1,.567,3.018h7.753a10.148,10.148,0,0,1,.567-3.018C14.326,14.878,17.1,13.1,17.1,8.55A8.533,8.533,0,0,0,8.55,0ZM5.286,15.291l2.85-5.47-3.57.031,7.263-7.125L8.979,8.182l3.57-.031Z"/>\
                        </g>\
                    </g>\
                </svg>\
                <span class="assessment-indicator-prefix">...</span>\
                <span class="assessment-indicator-info">...</span>\
            </div>'
};