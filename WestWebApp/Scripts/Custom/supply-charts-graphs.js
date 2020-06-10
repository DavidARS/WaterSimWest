// Graph 1
// Point in time
// single column with drill down (real drill down)
// use separate graphs for each city (using multiple drill downs within one graph)
        // colorado
        // srp
        // effluent
        // reclaimed
        // ground water

// Graph 2
// Temporal
// stacked area
        // show/hide pseudo drill down
        // colorado
        // srp
        // effluent
        // reclaimed
        // ground water

// Inputs (used within the supply-charts-graphs.js file to adjust the graph dynamically
//$(function () {
//    var chart;
//    $(document).ready(function () {
//        chart = new Highcharts.Chart({
//            chart: {
//                renderTo: 'area-container',
//                backgroundColor: null,
//                type: 'area'
//            },
//            title: {
//                text: ''
//            },
//            subtitle: {
//                text: ''
//            },
//            xAxis: {
//                categories: ['1', '2', '3', '4', '5', '6', ],
//                tickmarkPlacement: 'on',
//                title: {
//                    text: 'months'
//                }
//            },
//            yAxis: {
//                title: {
//                    text: '$(thousands)'
//                },
//                labels: {
//                    formatter: function () {
//                        return this.value / 1000;
//                    }
//                }
//            },
//            tooltip: {
//                formatter: function () {
//                    return '' +
//                        this.x + ': ' + Highcharts.numberFormat(this.y, 0, ',') + ' hundred';
//                }
//            },
//            plotOptions: {
//                area: {
//                    stacking: 'normal',
//                    lineColor: '#666666',
//                    lineWidth: 1,
//                    marker: {
//                        lineWidth: 1,
//                        lineColor: '#666666'
//                    }
//                }
//            },
//            legend: {
//                align: 'bottom',
//                x: 275,
//                borderColor: null
//            },

//            credits: {
//                enabled: false
//            },
//            series: [{
//                name: 'Revenue',
//                data: [100, 130, 170, 220, 350, 580]
//            }, {
//                name: 'Cost',
//                data: [100, 120, 140, 160, 180, 200]
//            }]
//        });
//    });


//    var min_value = 0;
//    var max_value = 100;

//    $('#effluent-slider').slider({

//        min: min_value,
//        max: max_value,
//        step: 5,

//        slide: function (event, ui) {
//            $('#effluent-percentage').html('$' + ui.value);
//            var newdata = [];
//            for (var i = 0 ; i < 6 ; i++) {
//                newdata.push(ui.value * i);
//            }
//            chart.series[0].setData(newdata);
//        },
//        stop: function (event, ui) {

//        }
//    });


//});


$(function () {
    var chart,
        colors = Highcharts.getOptions().colors;

    function setChart(name, categories, data, color) {
        chart.xAxis[0].setCategories(categories);
        chart.series[0].remove();
        chart.addSeries({
            name: name,
            data: data,
            color: color || 'white'
        });
    }

    $(document).ready(function () {

        var categories = ['Scottsdale'],
            name = 'Cities',
            data = [{
                y: 55.11,
                color: colors[0],
                drilldown: {
                    name: 'Scottsdale',
                    categories: ['Colorado River', 'SRP', 'Effluent', 'Reclaimed', 'Groundwater'],
                    data: [10.85, 7.35, 33.06, 2.81, 5.81],
                    color: colors[0]
                }
            }];

        chart = new Highcharts.Chart({
            chart: {
                renderTo: 'time-point-scottsdale',
                type: 'column'
            },
            title: {
                text: 'Point in Time'
            },
            subtitle: {
                text: ''
            },
            xAxis: {
                categories: categories
            },
            yAxis: {
                title: {
                    text: 'Units'
                }
            },
            plotOptions: {
                column: {
                    cursor: 'pointer',
                    point: {
                        events: {
                            click: function () {
                                var drilldown = this.drilldown;
                                if (drilldown) { // drill down
                                    setChart(drilldown.name, drilldown.categories, drilldown.data, drilldown.color);
                                } else { // restore
                                    setChart(name, categories, data);
                                }
                            }
                        }
                    },
                    dataLabels: {
                        enabled: true,
                        color: colors[0],
                        style: {
                            fontWeight: 'bold'
                        },
                        formatter: function () {
                            return this.y + '%';
                        }
                    }
                }
            },
            tooltip: {
                formatter: function () {
                    var point = this.point,
                        s = this.x + ':<b>' + this.y + '% market share</b><br/>';
                    if (point.drilldown) {
                        s += 'Click to view ' + point.category + ' versions';
                    } else {
                        s += 'Click to return to Scottsdale';
                    }
                    return s;
                }
            },
            series: [{
                name: name,
                data: data,
                color: 'white'
            }],
            exporting: {
                enabled: false
            }
        });
    });

    $('.update').on('click', function () {
        var newData = [{
            name: 'MSIE versions',
            categories: ['MSIE 6.0', 'MSIE 7.0', 'MSIE 8.0', 'MSIE 9.0'],
            data: [1, 2, 3, 4],
            color: colors[0]
        }];
        jQuery.each(newData, function (i, serie) {
            setChart(serie.name, serie.categories, serie.data, serie.color);
        });
    });

});


$(function () {
    var chart,
        colors = Highcharts.getOptions().colors;

    function setChart(name, categories, data, color) {
        chart.xAxis[0].setCategories(categories);
        chart.series[0].remove();
        chart.addSeries({
            name: name,
            data: data,
            color: color || 'white'
        });
    }

    $(document).ready(function () {

        var categories = ['Tempe'],
            name = 'Cities',
            data = [{
                y: 21.63,
                color: colors[1],
                drilldown: {
                    name: 'Tempe',
                    categories: ['Colorado River', 'SRP', 'Effluent', 'Reclaimed', 'Groundwater'],
                    data: [0.20, 0.83, 1.58, 13.12, 5.43, 3.37],
                    color: colors[1]
                }
            }];

        chart = new Highcharts.Chart({
            chart: {
                renderTo: 'time-point-tempe',
                type: 'column'
            },
            title: {
                text: 'Point in Time'
            },
            subtitle: {
                text: ''
            },
            xAxis: {
                categories: categories
            },
            yAxis: {
                title: {
                    text: 'Units'
                }
            },
            plotOptions: {
                column: {
                    cursor: 'pointer',
                    point: {
                        events: {
                            click: function () {
                                var drilldown = this.drilldown;
                                if (drilldown) { // drill down
                                    setChart(drilldown.name, drilldown.categories, drilldown.data, drilldown.color);
                                } else { // restore
                                    setChart(name, categories, data);
                                }
                            }
                        }
                    },
                    dataLabels: {
                        enabled: true,
                        color: colors[0],
                        style: {
                            fontWeight: 'bold'
                        },
                        formatter: function () {
                            return this.y + '%';
                        }
                    }
                }
            },
            tooltip: {
                formatter: function () {
                    var point = this.point,
                        s = this.x + ':<b>' + this.y + '% market share</b><br/>';
                    if (point.drilldown) {
                        s += 'Click to view ' + point.category + ' versions';
                    } else {
                        s += 'Click to return to Tempe';
                    }
                    return s;
                }
            },
            series: [{
                name: name,
                data: data,
                color: 'white'
            }],
            exporting: {
                enabled: false
            }
        });
    });

    $('.update').on('click', function () {
        var newData = [{
            name: 'MSIE versions',
            categories: ['MSIE 6.0', 'MSIE 7.0', 'MSIE 8.0', 'MSIE 9.0'],
            data: [1, 2, 3, 4],
            color: colors[0]
        }];
        jQuery.each(newData, function (i, serie) {
            setChart(serie.name, serie.categories, serie.data, serie.color);
        });
    });

});


$(function () {
    $('#time-range-scottsdale').highcharts({
        chart: {
            type: 'areaspline'
        },
        title: {
            text: 'Temporal'
        },
        legend: {
            layout: 'vertical',
            align: 'left',
            verticalAlign: 'top',
            x: 150,
            y: 100,
            floating: true,
            borderWidth: 1,
            backgroundColor: '#FFFFFF'
        },
        xAxis: {
            categories: [
                '2010',
                '2015',
                '2020',
                '2025',
                '2030',
                '2035',
                '2040',
                '2045',
                '2050'
            ],
            plotBands: [{ // visualize the weekend
                from: 4.5,
                to: 6.5,
                color: 'rgba(68, 170, 213, .2)'
            }]
        },
        yAxis: {
            title: {
                text: 'Units'
            }
        },
        tooltip: {
            shared: true,
            valueSuffix: ' units'
        },
        credits: {
            enabled: false
        },
        plotOptions: {
            areaspline: {
                fillOpacity: 0.5
            }
        },
        series: [{
            name: 'Scottsdale',
            data: [3, 4, 3, 5, 4, 10, 12]
        }, {
            name: 'Tempe',
            data: [1, 3, 4, 3, 3, 5, 4]
        }]
    });
});


$(function () {
    $('#time-range-tempe').highcharts({
        chart: {
            type: 'areaspline'
        },
        title: {
            text: 'Temporal'
        },
        legend: {
            layout: 'vertical',
            align: 'left',
            verticalAlign: 'top',
            x: 150,
            y: 100,
            floating: true,
            borderWidth: 1,
            backgroundColor: '#FFFFFF'
        },
        xAxis: {
            categories: [
                '2010',
                '2015',
                '2020',
                '2025',
                '2030',
                '2035',
                '2040',
                '2045',
                '2050'
            ],
            plotBands: [{ // visualize the weekend
                from: 4.5,
                to: 6.5,
                color: 'rgba(68, 170, 213, .2)'
            }]
        },
        yAxis: {
            title: {
                text: 'Units'
            }
        },
        tooltip: {
            shared: true,
            valueSuffix: ' units'
        },
        credits: {
            enabled: false
        },
        plotOptions: {
            areaspline: {
                fillOpacity: 0.5
            }
        },
        series: [{
            name: 'Scottsdale',
            data: [3, 4, 3, 5, 4, 10, 12]
        }, {
            name: 'Tempe',
            data: [1, 3, 4, 3, 3, 5, 4]
        }]
    });
});



// Supply - Pie Chart
/*
$(function () {
    $('#supply-pie-base').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
        title: {
            text: 'Water Percentage'
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    color: '#000000',
                    connectorColor: '#000000',
                    format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                }
            }
        },
        series: [{
            type: 'pie',
            name: 'Water Percentage',
            data: [
                ['Groundwater', 45.0],
                ['Effluent', 26.8],
                {
                    name: 'Runoff',
                    y: 12.8,
                    sliced: true,
                    selected: true
                },
                ['Salt-Tonto-Verde River', 8.5],
                ['Colorado River', 6.2]
            ]
        }]
    });
});
*/

//var chart;
//$(document).ready(function () { /*begin chart render*/
//    var colors = Highcharts.getOptions().colors,
//        categories = ['The Americas', 'Asia Pacific', 'Europe & Africa'],
//        //name = 'Sectors',
//        data = [{
//            name: 'A-1',
//            y: 55,
//            color: colors[0],
//            drilldown: {
//                //begin alcohol
//                name: 'A-1',
//                color: colors[0],
//                data: [{
//                    y: 33.06,
//                    name: 'A',
//                    drilldown: {
//                        name: 'Budweiser',
//                        data: [
//                            { name: 'A', y: 10838 }
//                            , { name: 'B', y: 11349 }
//                            , { name: 'C', y: 11894 }
//                            , { name: 'D', y: 11846 }
//                            , { name: 'E', y: 11878 }
//                            , { name: 'F', y: 11662 }
//                            , { name: 'G', y: 11652 }
//                        ],
//                        color: colors[0]
//                    }
//                },
//                {
//                    y: 10.85,
//                    name: 'B',
//                    drilldown: {
//                        name: 'Heinekein',
//                        categories: ['A', 'B', 'C', 'D', 'E', 'F', 'G'],
//                        data: [2266, 2396, 2431, 2380, 2357, 3516],
//                        color: colors[0]
//                    }
//                },
//                {
//                    y: 7.35,
//                    name: 'C',
//                    drilldown: {
//                        name: 'Jack Daniels',
//                        categories: ['A', 'B', 'C', 'D', 'E', 'F', 'G'],
//                        data: [1583, 1580, 1612, 4036],
//                        color: colors[0]
//                    }
//                },
//                {
//                    y: 2.41,
//                    name: 'D',
//                    drilldown: {
//                        name: 'Johnnie Walker',
//                        categories: ['A', 'B', 'C', 'D', 'E', 'F', 'G'],
//                        data: [1649, 1654, 1724, 3557],
//                        color: colors[0]
//                    }
//                },
//                {
//                    y: 2.41,
//                    name: 'E',
//                    drilldown: {
//                        name: 'Moet & Chandon',
//                        categories: ['A', 'B', 'C', 'D', 'E', 'F', 'G'],
//                        data: [2470, 2445, 2524, 2861, 2991, 3257, 3739, 3951, 3754, 4021],
//                        color: colors[0]
//                    }
//                },
//                {
//                    y: 2.41,
//                    name: 'F',
//                    drilldown: {
//                        name: 'Smirnoff',
//                        categories: ['A', 'B', 'C', 'D', 'E', 'F', 'G'],
//                        data: [2594, 2723, 5600, 2975, 3097, 3032, 3379, 3590, 7350, 3624],
//                        color: colors[0]
//                    }
//                },
//                {
//                    y: 2.41,
//                    name: 'G',
//                    drilldown: {
//                        name: 'Corona',
//                        categories: ['A', 'B', 'C', 'D', 'E', 'F', 'G'],
//                        data: [3847],
//                        color: colors[0]
//                    }
//                }],
//            }
//        },
//        {
//            name: 'B-1',
//            y: 11.94,
//            color: colors[2],
//            drilldown: {
//                name: 'B',
//                categories: ['A-2', 'B-2', 'C-2'],
//                color: colors[2],
//                data: [{
//                    y: 33.06,
//                    name: 'A',
//                    drilldown: {
//                        name: 'A',
//                        categories: ['A', 'B'],
//                        data: [4444, 6666],
//                        color: colors[3]
//                    },
//                },
//                {
//                    name: 'B',
//                    y: 10.85,
//                    drilldown: {
//                        name: 'B',
//                        categories: ['A', 'B'],
//                        data: [22222, 6005],
//                        color: colors[3]
//                    },
//                },
//                {
//                    name: 'C',
//                    y: 7.35,
//                    drilldown: {
//                        name: 'C',
//                        categories: ['2011'],
//                        data: [3605],
//                        color: colors[3]
//                    }
//                }],
//            }
//        },
//        ];

//    function setChart(name, categories, data, color) {
//        //chart.xAxis[0].setCategories(categories);
//        chart.series[0].remove();
//        chart.addSeries({
//            name: name,
//            data: data,
//            pointPadding: -0.3,
//            borderWidth: 0,
//            pointWidth: 15,
//            shadow: false,
//            color: color || 'white'
//        });
//    }

//    chart = new Highcharts.Chart({
//        chart: {
//            renderTo: 'supply-pie-base',
//            type: 'pie',
//            /* changes bar size */
//            pointPadding: -0.3,
//            borderWidth: 0,
//            pointWidth: 10,
//            shadow: false,
//            backgroundColor: '#ffffff'
//        },
//        title: {
//            text: 'Pie Test'
//        },
//        subtitle: {
//            text: 'Pie Chart Triple Breakdown'
//        },
//        xAxis: {
//            categories: categories
//        },
//        yAxis: {
//            title: {
//                text: 'Total Brand Value',
//                categories: categories
//            }
//        },
//        //drilldown plot
//        plotOptions: {
//            pie: {
//                cursor: 'pointer',
//                allowPointSelect: true,
//                point: {
//                    events: {
//                        click: function () {
//                            var drilldown = this.drilldown;
//                            if (drilldown) { // drill down
//                                setChart(drilldown.name, drilldown.categories, drilldown.data, drilldown.color);
//                            } else { // restore
//                                setChart(name, categories, data);
//                            }
//                        }
//                    }
//                },
//                dataLabels: {
//                    enabled: true,
//                    color: '#000',
//                    //label colors
//                    connectorColor: '#000',
//                    // connector label colors
//                    formatter: function () {
//                        return '<b>' + this.point.name + '</b>: ' + this.y;

//                    }
//                }
//            }
//        },
//        //formatting over hover tooltip
//        tooltip: {
//            formatter: function () {
//                var point = this.point,
//                    s = point.name + ':<b>' + this.y + '% market share</b><br/>';
//                if (point.drilldown) {
//                    s = point.name + ':<b>' + this.y + '222</b><br/>';
//                    s += 'Click to view ' + point.name + ' versions';
//                } else {
//                    s = point.name + ':<b>' + this.y + '333</b><br/>';
//                    s += 'Click to return to browser brands';
//                }
//                return s;
//            }
//        },
//        credits: {
//            enabled: false
//        },
//        series: [{
//            name: name,
//            data: data,
//            /* changes bar size */
//            pointPadding: -0.3,
//            borderWidth: 0,
//            pointWidth: 15,
//            shadow: false,
//            color: 'black' //Sectors icon
//        }],
//        exporting: {
//            enabled: false
//        }
//    });


//});

//// Supply Bar Grouped

//$(function () {
//    $('#supply-bar-grouped-base').highcharts({
//        chart: {
//            type: 'column'
//        },
//        title: {
//            text: 'Absolute Value of Water Supply'
//        },
//        subtitle: {
//            text: '(in acre-feet)'
//        },
//        xAxis: {
//            categories: [
//                '2000',
//                '2005',
//                '2010',
//                '2015',
//                '2020',
//                '2025',
//                '2030',
//                '2035',
//                '2040',
//                '2045',
//                '2050'
//            ]
//        },
//        yAxis: {
//            min: 0,
//            title: {
//                text: 'Water (acre-feet)'
//            }
//        },
//        tooltip: {
//            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
//            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
//                '<td style="padding:0"><b>{point.y:.1f} mm</b></td></tr>',
//            footerFormat: '</table>',
//            shared: true,
//            useHTML: true
//        },
//        plotOptions: {
//            column: {
//                pointPadding: 0.2,
//                borderWidth: 0
//            }
//        },
//        series: [{
//            name: 'Groundwater',
//            data: [49.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6]

//        }, {
//            name: 'Effluent',
//            data: [83.6, 78.8, 98.5, 93.4, 106.0, 84.5, 105.0, 104.3, 91.2, 83.5, 106.6]

//        }, {
//            name: 'Salt-Tonto-Verde',
//            data: [48.9, 38.8, 39.3, 41.4, 47.0, 48.3, 59.0, 59.6, 52.4, 65.2, 59.3]

//        }, {
//            name: 'Colorado River',
//            data: [42.4, 33.2, 34.5, 39.7, 52.6, 75.5, 57.4, 60.4, 47.6, 39.1, 46.8]

//        }]
//    });
//});

///* Supply - Stacked Area */
//$(function () {
//    $('#stacked-area-base').highcharts({
//        chart: {
//            type: 'area'
//        },
//        title: {
//            text: 'Water Percentage'
//        },
//        subtitle: {
//            text: '(in acre-feet)'
//        },
//        xAxis: {
//            categories: ['2000', '2005', '2010', '2015', '2020', '2025', '2030', '2035', '2040', '2045', '2050'],
//            tickmarkPlacement: 'on',
//            title: {
//                enabled: false
//            }
//        },
//        yAxis: {
//            title: {
//                text: 'Percent'
//            }
//        },
//        tooltip: {
//            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.percentage:.1f}%</b> ({point.y:,.0f} millions)<br/>',
//            shared: true
//        },
//        plotOptions: {
//            area: {
//                stacking: 'percent',
//                lineColor: '#ffffff',
//                lineWidth: 1,
//                marker: {
//                    lineWidth: 1,
//                    lineColor: '#ffffff'
//                }
//            }
//        },
//        series: [{
//            name: 'Groundwater',
//            data: [502, 635, 809, 947, 1402, 3634, 5268]
//        }, {
//            name: 'Effluent',
//            data: [106, 107, 111, 133, 221, 767, 1766]
//        }, {
//            name: 'Salt-Tonto-Verde',
//            data: [163, 203, 276, 408, 547, 729, 628]
//        }, {
//            name: 'Colorado River',
//            data: [18, 31, 54, 156, 339, 818, 1201]
//        }]
//    });
//});

///* Supply - Line */
//$(function () {
//    $('#line-base').highcharts({
//            title: {
//                text: 'Absolute Value of Water Supply',
//                x: -20 //center
//            },
//            subtitle: {
//                text: '(in acre-feet)',
//                x: -20
//            },
//            xAxis: {
//                categories: ['2000', '2005', '2010', '2015', '2020', '2025',
//                    '2030', '2035', '2040', '2045', '2050']
//            },
//            yAxis: {
//                title: {
//                    text: 'Acre-Feet'
//                },
//                plotLines: [{
//                    value: 0,
//                    width: 1,
//                    color: '#808080'
//                }]
//            },
//            tooltip: {
//                valueSuffix: 'acre-feet'
//            },
//            legend: {
//                layout: 'vertical',
//                align: 'right',
//                verticalAlign: 'middle',
//                borderWidth: 0
//            },
//            series: [{
//                name: 'Groundwater',
//                data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9]
//            }, {
//                name: 'Effluent',
//                data: [-0.2, 0.8, 5.7, 11.3, 17.0, 22.0, 24.8, 24.1, 20.1, 14.1, 8.6]
//            }, {
//                name: 'Salt-Tonto-Verde',
//                data: [-0.9, 0.6, 3.5, 8.4, 13.5, 17.0, 18.6, 17.9, 14.3, 9.0, 3.9]
//            }, {
//                name: 'Colorado River',
//                data: [3.9, 4.2, 5.7, 8.5, 11.9, 15.2, 17.0, 16.6, 14.2, 10.3, 6.6]
//            }]
//        });
//});


///*
//$(function () {
//    $('#supply-pie-projected').highcharts({
//        chart: {
//            plotBackgroundColor: null,
//            plotBorderWidth: null,
//            plotShadow: false
//        },
//        title: {
//            text: 'Water Percentage'
//        },
//        tooltip: {
//            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
//        },
//        plotOptions: {
//            pie: {
//                allowPointSelect: true,
//                cursor: 'pointer',
//                dataLabels: {
//                    enabled: true,
//                    color: '#000000',
//                    connectorColor: '#000000',
//                    format: '<b>{point.name}</b>: {point.percentage:.1f} %'
//                }
//            }
//        },
//        series: [{
//            type: 'pie',
//            name: 'Water Percentage',
//            data: [
//                ['Groundwater', 25.0],
//                ['Effluent', 66.8],
//                {
//                    name: 'Runoff',
//                    y: 12.8,
//                    sliced: true,
//                    selected: true
//                },
//                ['Salt-Tonto-Verde River', 2.5],
//                ['Colorado River', .2]
//            ]
//        }]
//    });
//});
//*/

//var chart;
//$(document).ready(function () { /*begin chart render*/
//    var colors = Highcharts.getOptions().colors,
//        categories = ['The Americas', 'Asia Pacific', 'Europe & Africa'],
//        //name = 'Sectors',
//        data = [{
//            name: 'A-1',
//            y: 55,
//            color: colors[0],
//            drilldown: {
//                //begin alcohol
//                name: 'A-1',
//                color: colors[0],
//                data: [{
//                    y: 33.06,
//                    name: 'A',
//                    drilldown: {
//                        name: 'Budweiser',
//                        data: [
//                            { name: 'A', y: 10838 }
//                            , { name: 'B', y: 11349 }
//                            , { name: 'C', y: 11894 }
//                            , { name: 'D', y: 11846 }
//                            , { name: 'E', y: 11878 }
//                            , { name: 'F', y: 11662 }
//                            , { name: 'G', y: 11652 }
//                        ],
//                        color: colors[0]
//                    }
//                },
//                {
//                    y: 10.85,
//                    name: 'B',
//                    drilldown: {
//                        name: 'Heinekein',
//                        categories: ['A', 'B', 'C', 'D', 'E', 'F', 'G'],
//                        data: [2266, 2396, 2431, 2380, 2357, 3516],
//                        color: colors[0]
//                    }
//                },
//                {
//                    y: 7.35,
//                    name: 'C',
//                    drilldown: {
//                        name: 'Jack Daniels',
//                        categories: ['A', 'B', 'C', 'D', 'E', 'F', 'G'],
//                        data: [1583, 1580, 1612, 4036],
//                        color: colors[0]
//                    }
//                },
//                {
//                    y: 2.41,
//                    name: 'D',
//                    drilldown: {
//                        name: 'Johnnie Walker',
//                        categories: ['A', 'B', 'C', 'D', 'E', 'F', 'G'],
//                        data: [1649, 1654, 1724, 3557],
//                        color: colors[0]
//                    }
//                },
//                {
//                    y: 2.41,
//                    name: 'E',
//                    drilldown: {
//                        name: 'Moet & Chandon',
//                        categories: ['A', 'B', 'C', 'D', 'E', 'F', 'G'],
//                        data: [2470, 2445, 2524, 2861, 2991, 3257, 3739, 3951, 3754, 4021],
//                        color: colors[0]
//                    }
//                },
//                {
//                    y: 2.41,
//                    name: 'F',
//                    drilldown: {
//                        name: 'Smirnoff',
//                        categories: ['A', 'B', 'C', 'D', 'E', 'F', 'G'],
//                        data: [2594, 2723, 5600, 2975, 3097, 3032, 3379, 3590, 7350, 3624],
//                        color: colors[0]
//                    }
//                },
//                {
//                    y: 2.41,
//                    name: 'G',
//                    drilldown: {
//                        name: 'Corona',
//                        categories: ['A', 'B', 'C', 'D', 'E', 'F', 'G'],
//                        data: [3847],
//                        color: colors[0]
//                    }
//                }],
//            }
//        },
//        {
//            name: 'B-1',
//            y: 11.94,
//            color: colors[2],
//            drilldown: {
//                name: 'B',
//                categories: ['A-2', 'B-2', 'C-2'],
//                color: colors[2],
//                data: [{
//                    y: 33.06,
//                    name: 'A',
//                    drilldown: {
//                        name: 'A',
//                        categories: ['A', 'B'],
//                        data: [4444, 6666],
//                        color: colors[3]
//                    },
//                },
//                {
//                    name: 'B',
//                    y: 10.85,
//                    drilldown: {
//                        name: 'B',
//                        categories: ['A', 'B'],
//                        data: [22222, 6005],
//                        color: colors[3]
//                    },
//                },
//                {
//                    name: 'C',
//                    y: 7.35,
//                    drilldown: {
//                        name: 'C',
//                        categories: ['2011'],
//                        data: [3605],
//                        color: colors[3]
//                    }
//                }],
//            }
//        },
//        ];

//    function setChart(name, categories, data, color) {
//        //chart.xAxis[0].setCategories(categories);
//        chart.series[0].remove();
//        chart.addSeries({
//            name: name,
//            data: data,
//            pointPadding: -0.3,
//            borderWidth: 0,
//            pointWidth: 15,
//            shadow: false,
//            color: color || 'white'
//        });
//    }

//    chart = new Highcharts.Chart({
//        chart: {
//            renderTo: 'supply-pie-projected',
//            type: 'pie',
//            /* changes bar size */
//            pointPadding: -0.3,
//            borderWidth: 0,
//            pointWidth: 10,
//            shadow: false,
//            backgroundColor: '#ffffff'
//        },
//        title: {
//            text: 'Pie Test'
//        },
//        subtitle: {
//            text: 'Pie Chart Triple Breakdown'
//        },
//        xAxis: {
//            categories: categories
//        },
//        yAxis: {
//            title: {
//                text: 'Total Brand Value',
//                categories: categories
//            }
//        },
//        //drilldown plot
//        plotOptions: {
//            pie: {
//                cursor: 'pointer',
//                allowPointSelect: true,
//                point: {
//                    events: {
//                        click: function () {
//                            var drilldown = this.drilldown;
//                            if (drilldown) { // drill down
//                                setChart(drilldown.name, drilldown.categories, drilldown.data, drilldown.color);
//                            } else { // restore
//                                setChart(name, categories, data);
//                            }
//                        }
//                    }
//                },
//                dataLabels: {
//                    enabled: true,
//                    color: '#000',
//                    //label colors
//                    connectorColor: '#000',
//                    // connector label colors
//                    formatter: function () {
//                        return '<b>' + this.point.name + '</b>: ' + this.y;

//                    }
//                }
//            }
//        },
//        //formatting over hover tooltip
//        tooltip: {
//            formatter: function () {
//                var point = this.point,
//                    s = point.name + ':<b>' + this.y + '% market share</b><br/>';
//                if (point.drilldown) {
//                    s = point.name + ':<b>' + this.y + '222</b><br/>';
//                    s += 'Click to view ' + point.name + ' versions';
//                } else {
//                    s = point.name + ':<b>' + this.y + '333</b><br/>';
//                    s += 'Click to return to browser brands';
//                }
//                return s;
//            }
//        },
//        credits: {
//            enabled: false
//        },
//        series: [{
//            name: name,
//            data: data,
//            /* changes bar size */
//            pointPadding: -0.3,
//            borderWidth: 0,
//            pointWidth: 15,
//            shadow: false,
//            color: 'black' //Sectors icon
//        }],
//        exporting: {
//            enabled: false
//        }
//    });


//});


//$(function () {
//    $('#supply-bar-grouped-projected').highcharts({
//        chart: {
//            type: 'column'
//        },
//        title: {
//            text: 'Absolute Value of Water Supply'
//        },
//        subtitle: {
//            text: '(in acre-feet)'
//        },
//        xAxis: {
//            categories: [
//                '2000',
//                '2005',
//                '2010',
//                '2015',
//                '2020',
//                '2025',
//                '2030',
//                '2035',
//                '2040',
//                '2045',
//                '2050'
//            ]
//        },
//        yAxis: {
//            min: 0,
//            title: {
//                text: 'Water (acre-feet)'
//            }
//        },
//        tooltip: {
//            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
//            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
//                '<td style="padding:0"><b>{point.y:.1f} mm</b></td></tr>',
//            footerFormat: '</table>',
//            shared: true,
//            useHTML: true
//        },
//        plotOptions: {
//            column: {
//                pointPadding: 0.2,
//                borderWidth: 0
//            }
//        },
//        series: [{
//            name: 'Groundwater',
//            data: [49.9, 71.5, 106.4, 129.2, 114.0, 176.0, 135.6, 118.5, 216.4, 94.1, 95.6]

//        }, {
//            name: 'Effluent',
//            data: [83.6, 30.8, 98.5, 93.4, 56.0, 84.5, 105.0, 104.3, 61.2, 83.5, 106.6]

//        }, {
//            name: 'Salt-Tonto-Verde',
//            data: [48.9, 38.8, 39.3, 41.4, 47.0, 48.3, 59.0, 59.6, 52.4, 65.2, 59.3]

//        }, {
//            name: 'Colorado River',
//            data: [42.4, 23.2, 34.5, 19.7, 52.6, 75.5, 37.4, 60.4, 47.6, 59.1, 46.8]

//        }]
//    });
//});

//$(function () {
//    $('#stacked-area-projected').highcharts({
//        chart: {
//            type: 'area'
//        },
//        title: {
//            text: 'Water Percentage'
//        },
//        subtitle: {
//            text: '(in acre-feet)'
//        },
//        xAxis: {
//            categories: ['2000', '2005', '2010', '2015', '2020', '2025', '2030', '2035', '2040', '2045', '2050'],
//            tickmarkPlacement: 'on',
//            title: {
//                enabled: false
//            }
//        },
//        yAxis: {
//            title: {
//                text: 'Percent'
//            }
//        },
//        tooltip: {
//            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.percentage:.1f}%</b> ({point.y:,.0f} millions)<br/>',
//            shared: true
//        },
//        plotOptions: {
//            area: {
//                stacking: 'percent',
//                lineColor: '#ffffff',
//                lineWidth: 1,
//                marker: {
//                    lineWidth: 1,
//                    lineColor: '#ffffff'
//                }
//            }
//        },
//        series: [{
//            name: 'Groundwater',
//            data: [502, 835, 309, 647, 1402, 2634, 3268]
//        }, {
//            name: 'Effluent',
//            data: [106, 107, 111, 133, 221, 767, 1766]
//        }, {
//            name: 'Salt-Tonto-Verde',
//            data: [163, 703, 76, 308, 247, 429, 628]
//        }, {
//            name: 'Colorado River',
//            data: [108, 71, 84, 156, 319, 418, 1001]
//        }]
//    });
//});

//$(function () {
//    $('#line-projected').highcharts({
//        title: {
//            text: 'Absolute Value of Water Supply',
//            x: -20 //center
//        },
//        subtitle: {
//            text: '(in acre-feet)',
//            x: -20
//        },
//        xAxis: {
//            categories: ['2000', '2005', '2010', '2015', '2020', '2025',
//                '2030', '2035', '2040', '2045', '2050']
//        },
//        yAxis: {
//            title: {
//                text: 'Acre-Feet'
//            },
//            plotLines: [{
//                value: 0,
//                width: 1,
//                color: '#808080'
//            }]
//        },
//        tooltip: {
//            valueSuffix: 'acre-feet'
//        },
//        legend: {
//            layout: 'vertical',
//            align: 'right',
//            verticalAlign: 'middle',
//            borderWidth: 0
//        },
//        series: [{
//            name: 'Groundwater',
//            data: [17.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 3.3, 18.3, 3.9]
//        }, {
//            name: 'Effluent',
//            data: [-0.2, 3.8, 5.7, 11.3, 17.0, 22.0, 24.8, 4.1, 20.1, 14.1, 8.6]
//        }, {
//            name: 'Salt-Tonto-Verde',
//            data: [-0.9, 0.6, 3.5, 1.4, 13.5, 17.0, 18.6, 17.9, 4.3, 9.0, 3.9]
//        }, {
//            name: 'Colorado River',
//            data: [3.9, 4.2, 5.7, 8.5, 11.9, 5.2, 17.0, 16.6, 14.2, 10.3, 6.6]
//        }]
//    });
//});