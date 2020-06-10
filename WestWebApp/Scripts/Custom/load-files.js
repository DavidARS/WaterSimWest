var LOAD_IPAD = window.location.href.toLowerCase().indexOf('ipad') > -1;

function loadCSSFiles() {
    var cssFiles = {
        scripts: ["Scripts/Chosen/chosen.css"],
        themes: ["Content/themes/smoothness/jquery-ui.css", "Content/themes/base/Gear.tooltip.css"],
        content: ["Content/Title.css", "Content/Tabs.css", "Content/InputControls.css", "Content/Indicators.css", "Content/Assessment.css",
            "Content/accordion.css", "Content/collapsible_panels.css", "Content/pageslide.css", "Content/climateTab.css",
            /*This reference causing NetworkError: 404 Not found
            "Content/validationTab.css",*/
            "Content/wizard.css", "Content/basic.css", "Content/basic_ie.css", "Content/horizontal.css", "Content/isotope.css"],
        sideshow: ["Scripts/Sideshow/distr/fonts/sideshow-fontface.min.css", "Scripts/Sideshow/distr/stylesheets/sideshow.css"],
        sankey: ["Scripts/Custom/Sankey/sankey.css"],
        timer: ["Content/timer.css"]
    };
    if (LOAD_IPAD) {
        cssFiles['ipad'] = ['Content/ipad.css'];
    }

    for (var files in cssFiles) {
        for (var i = 0; i < cssFiles[files].length; i++) {
            var link = document.createElement("link")
            link.setAttribute("rel", "stylesheet")
            link.setAttribute("type", "text/css")
            link.setAttribute("href", cssFiles[files][i])
            document.getElementsByTagName('head')[0].appendChild(link);
        }
    }
}

loadCSSFiles();


var Utilities = {};

//---------------------------------------------------------------------------------------
//Source - http://stackoverflow.com/questions/2879509/dynamically-loading-javascript-synchronously
//Asynchronously load each file in order
Utilities.require = function (file, callback) {
    callback = callback ||
    function () { };
    var filenode;
    var jsfile_extension = /(.js)$/i;
    var cssfile_extension = /(.css)$/i;

    if (jsfile_extension.test(file)) {
        filenode = document.createElement('script');
        filenode.src = file;
        // IE
        filenode.onreadystatechange = function () {
            if (filenode.readyState === 'loaded' || filenode.readyState === 'complete') {
                filenode.onreadystatechange = null;
                callback();
            }
        };
        // others
        filenode.onload = function () {
            callback();
        };
        document.head.appendChild(filenode);
    } else if (cssfile_extension.test(file)) {
        filenode = document.createElement('link');
        filenode.rel = 'stylesheet';
        filenode.type = 'text/css';
        filenode.href = file;
        document.head.appendChild(filenode);
        callback();
    } else {
        console.log("Unknown file type to load.")
    }
};

Utilities.requireFiles = function () {
    var index = 0;
    return function (files, callback) {
        index += 1;
        Utilities.require(files[index - 1], callBackCounter);

        function callBackCounter() {
            if (index === files.length) {
                index = 0;
                callback();
            } else {
                Utilities.requireFiles(files, callback);
            }
        };
    };
}();

function loadJSFiles() {
    var jsFiles = {
        jquery: ["Scripts/jquery-2.1.0.js", "Scripts/jquery-ui-1.10.4.js", "Scripts/jquery.csv-0.71.min.js"],
        d3: ["Scripts/d3.min.js"],
        accordion: ["Scripts/msAccordion.js", "Scripts/Custom/jquery.zaccordion.js", "Scripts/Custom/collapsible-panels.js"],
        init: ["Scripts/Custom/document-ready.js", "Scripts/Custom/jquery-functions.js"],
        chosen: ["Scripts/Chosen/chosen.jquery.js", "Scripts/Chosen/docsupport/prism.js"],
        modal: ["Scripts/SimpleModal/jquery.simplemodal.js"],
        isotope: ["Scripts/Isotope/isotope.pkgd.js", "Scripts/Isotope/isotope.js"],
        slider: ["Scripts/Custom/Core/SettingRunButton.js", "Scripts/Custom/Slider/SettingSlider.js"],
        //indicators: ["Assets/indicators/Scripts/IndicatorControl_v_4.js", "Assets/indicators/Scripts/indicatorsCore_v2.js"],
        sly: ["Scripts/Sly/sly.js", "Scripts/Sly/horizontal-supply.js", "Scripts/Sly/horizontal-demand.js", "Scripts/Sly/horizontal-reservoirs.js",
            "Scripts/Sly/horizontal-climate.js"],
        charts: [
            "Scripts/Highcharts/highcharts.js", "Scripts/Custom/Charts/ChartTools.js", "Scripts/Custom/Charts/DrillDownChartBO.js", "Scripts/Custom/Charts/ProvidersChart.js",
            "Scripts/Custom/Charts/AreaChart.js", "Scripts/Custom/Charts/DrillDownColumnChartBO.js", "Scripts/Custom/Charts/DrillDownLineChartBO.js",
            "Scripts/Custom/Charts/DrillDownLineChartTEMP.js", "Scripts/Custom/Charts/DrillDownPieColumnChartMF.js", "Scripts/Custom/Charts/DrillDownPieColumnChartMP.js",
            "Scripts/Custom/Charts/DrillDownSingleColumnChart.js", "Scripts/Custom/Charts/StackedAreaChart.js", "Scripts/Custom/Charts/StackedColumnChart.js",
            "Scripts/Custom/Charts/LineChartMP.js", "Scripts/HighCharts/modules/drilldown.js",
            "Scripts/Custom/Charts/HighChartsUnderscoreFix.js", "Scripts/Highcharts/modules/exporting.js", "Scripts/Custom/Charts/SankeyChart.js",
            "Scripts/Custom/Charts/MultiPieChart.js", "Scripts/Custom/Charts/PieChart.js", "Scripts/Custom/Charts/SimpleStackedColumn.js",
            "Scripts/Custom/Charts/ComplexStackedColumn.js", "Scripts/Custom/Charts/SimpleLineChart.js"
        ],
        sankey: ["Scripts/Custom/Sankey/sankey.js"],
        other: ["Scripts/rgbcolor.js", "Scripts/canvg.js", "Scripts/Custom/qPbar.js"],
        sideshow: [
            "Scripts/Sideshow/distr/dependencies/jazz.min.js", "Scripts/Sideshow/distr/dependencies/pagedown.min.js",
        "Scripts/Sideshow/distr/sideshow.js", "Scripts/Custom/Core/GuidedInteractionWSAmerica.js"
        ],
        utilities: ["Scripts/Custom/Core/ConvertInputControls.js", "Scripts/Custom/Core/UrlHandler.js", "Scripts/Custom/Core/ReportTools.js"],
        assessment: ["Scripts/Custom/Assessment/Assessment_Base.js"],
        core: [
        "Scripts/Custom/Core/Tabs.js", "Scripts/Custom/Core/AmericaAppControl_v2.js", , "Scripts/Custom/Core/Assessment/Xpan_V2.js",
        "Scripts/Custom/Core/Assessment/WSA_AssessmentReport_v4.js", "Scripts/Custom/Core/Feedback.js",
        "Scripts/Custom/STC" + (LOAD_IPAD ? "_Ipad" : "") + ".js", "Scripts/Custom/Core/RiverFlow.js", "Scripts/Custom/Indicators/indicator.js",
        "Scripts/Custom/Core/CoreGenFunctions.js", "Scripts/Custom/Core/CoreGetJSON.js",
        "Scripts/Custom/Core/CoreDrawCharts.js", "Scripts/Custom/Core/CoreQuayChart.js",
        "Scripts/Custom/Core/soundsupport.js", "Scripts/Custom/Core/Core.js"
        ]
    };

    if (LOAD_IPAD) {
        jsFiles['ipad'] = ["Scripts/Custom/Ipad/Main.js"];
        jsFiles['assessment'].push("Scripts/Custom/Assessment/Assessment_Ipad.js");
    }
    else {
        jsFiles['modal'].push("Scripts/SimpleModal/basic.js");
        jsFiles['assessment'].push("Scripts/Custom/Assessment/Assessment.js");
    }

    var fileArray = [];
    for (var fileType in jsFiles) {
        for (var file in jsFiles[fileType]) {
            fileArray.push(jsFiles[fileType][file]);
        }
    }

    Utilities.requireFiles(fileArray, function () {
        //Call the init function in the loaded file.
    })
}

loadJSFiles();
