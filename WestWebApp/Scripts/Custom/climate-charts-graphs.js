// Climate - Reservoirs SRP
$(function () {
    $('#climate-reservoirs-colorado').highcharts({
        chart: {
            type: 'area'
        },
        title: {
            text: 'Colorado River Reservoir Volumes'
        },
        subtitle: {
            text: '(acre-feet)'
        },
        xAxis: {
            categories: ['2000', '2005', '2010', '2015', '2020', '2025', '2030', '2035', '2040', '2045', '2050'],
            tickmarkPlacement: 'on',
            title: {
                enabled: false
            }
        },
        yAxis: {
            title: {
                text: 'Acre-Feet'
            }
        },
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.percentage:.1f}%</b> ({point.y:,.0f} millions)<br/>',
            shared: true
        },
        plotOptions: {
            area: {
                stacking: 'normal',
                lineColor: '#ffffff',
                lineWidth: 1,
                marker: {
                    lineWidth: 1,
                    lineColor: '#ffffff'
                }
            }
        },
        series: [{
            name: 'Lake Mead',
            data: [502, 635, 809, 947, 1402, 3634, 5268]
        }, {
            name: 'Lake Powell',
            data: [106, 107, 111, 133, 221, 767, 1766]
        }]
    });
});

// Climate - Reservoirs Colorado
$(function () {
    $('#climate-reservoirs-srp').highcharts({
        chart: {
            type: 'area'
        },
        title: {
            text: 'Salt River Reservoir Volumes'
        },
        subtitle: {
            text: '(acre-feet)'
        },
        xAxis: {
            categories: ['2000', '2005', '2010', '2015', '2020', '2025', '2030', '2035', '2040', '2045', '2050'],
            tickmarkPlacement: 'on',
            title: {
                enabled: false
            }
        },
        yAxis: {
            title: {
                text: 'Acre-Feet'
            }
        },
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.percentage:.1f}%</b> ({point.y:,.0f} millions)<br/>',
            shared: true
        },
        plotOptions: {
            area: {
                stacking: 'normal',
                lineColor: '#ffffff',
                lineWidth: 1,
                marker: {
                    lineWidth: 1,
                    lineColor: '#ffffff'
                }
            }
        },
        series: [{
            name: 'Roosevelt Lake',
            data: [502, 635, 809, 947, 1402, 3634, 5268]
        }, {
            name: 'Verde Canyon',
            data: [106, 107, 111, 133, 221, 767, 1766]
        }, {
            name: 'Salt-Tonto-Other',
            data: [163, 203, 276, 408, 547, 729, 628]
        }]
    });
});

// Climate - Traces Colorado
$(function () {
    $('#climate-traces-colorado').highcharts({
        title: {
            text: 'Colorado River Flow',
            x: -20 //center
        },
        subtitle: {
            text: '(acre-feet per annum)',
            x: -20
        },
        xAxis: {
            categories: ['2000', '2005', '2010', '2015', '2020', '2025',
                '2030', '2035', '2040', '2045', '2050']
        },
        yAxis: {
            title: {
                text: 'Acre-Feet Annum -1'
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        },
        tooltip: {
            valueSuffix: 'acre-feet'
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
        series: [{
            name: 'River Flow',
            data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9]
        }]
    });
});

// Climate - Traces SRP
/* 
$(function () {
    $('#climate-traces-srp').highcharts({
        title: {
            text: 'River Flow',
            x: -20 //center
        },
        subtitle: {
            text: '(acre-feet per annum)',
            x: -20
        },
        xAxis: {
            categories: ['2000', '2005', '2010', '2015', '2020', '2025',
                '2030', '2035', '2040', '2045', '2050']
        },
        yAxis: {
            title: {
                text: 'Acre-Feet'
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        },
        tooltip: {
            valueSuffix: 'acre-feet'
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
        series: [{
            name: 'Groundwater',
            data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9]
        }, {
            name: 'Effluent',
            data: [-0.2, 0.8, 5.7, 11.3, 17.0, 22.0, 24.8, 24.1, 20.1, 14.1, 8.6]
        }, {
            name: 'Salt-Tonto-Verde',
            data: [-0.9, 0.6, 3.5, 8.4, 13.5, 17.0, 18.6, 17.9, 14.3, 9.0, 3.9]
        }, {
            name: 'Colorado River',
            data: [3.9, 4.2, 5.7, 8.5, 11.9, 15.2, 17.0, 16.6, 14.2, 10.3, 6.6]
        }]
    });
}); */

$(function () {
    $('#climate-traces-srp').highcharts({
        chart: {
            type: 'area'
        },
        title: {
            text: 'Salt-Verde-Tonto River Flow'
        },
        subtitle: {
            text: '(acre-feet per annum)'
        },


        subtitle: {
            text: '(acre-feet per annum)',
            style: {
                color: '#666666'
                //font: 'bold 24px "Trebuchet MS", Verdana, sans-serif'
            }
        },

        xAxis: {
            labels: {
                formatter: function () {
                    return this.value; // clean, unformatted number for year
                }
            }
        },
        yAxis: {
            title: {
                text: 'Acre-Feet Annum -1'
            },
            labels: {
                formatter: function () {
                    return this.value / 1000 + 'k';
                }
            }
        },
        tooltip: {
            pointFormat: '{series.name} people <b>{point.y:,.0f}</b><br/> in {point.x}'
        },
        plotOptions: {
            area: {
                pointStart: 2000,
                marker: {
                    enabled: false,
                    symbol: 'circle',
                    radius: 2,
                    states: {
                        hover: {
                            enabled: true
                        }
                    }
                }
            }
        },
        series: [{
            name: 'Salt River',
            data: [null, null, null, null, null, 6, 11, 32, 110, 235, 369, 640,
                1005, 1436, 2063, 3057, 4618, 6444, 9822, 15468, 20434, 24126,
                27387, 29459, 31056, 31982, 32040, 31233, 29224, 27342, 26662,
                26956, 27912, 28999, 28965, 27826, 25579, 25722, 24826, 24605,
                24304, 23464, 23708, 24099, 24357, 24237, 24401, 24344, 23586,
                22380, 21004, 17287, 14747, 13076, 12555, 12144, 11009, 10950,
                10871, 10824, 10577, 10527, 10475, 10421, 10358, 10295, 10104]
        }, {
            name: 'Verde River',
            data: [null, null, null, null, null, null, null, null, null, null,
            5, 25, 50, 120, 150, 200, 426, 660, 869, 1060, 1605, 2471, 3322,
            4238, 5221, 6129, 7089, 8339, 9399, 10538, 11643, 13092, 14478,
            15915, 17385, 19055, 21205, 23044, 25393, 27935, 30062, 32049,
            33952, 35804, 37431, 39197, 45000, 43000, 41000, 39000, 37000,
            35000, 33000, 31000, 29000, 27000, 25000, 24000, 23000, 22000,
            21000, 20000, 19000, 18000, 18000, 17000, 16000]
        }, {
            name: 'SRP Total',
            data: [null, null, null, null, null, null, null, null, null, null,
            500, 2500, 500, 1420, 1050, 2200, 4026, 6660, 1869, 10760, 1605, 22471, 31322,
            4238, 5221, 6129, 7089, 8339, 9399, 10538, 11643, 13092, 14478,
            15915, 17385, 19055, 21205, 23044, 25393, 27935, 30062, 32049,
            33952, 35804, 37431, 39197, 45000, 43000, 41000, 39000, 37000,
            35000, 33000, 31000, 29000, 27000, 25000, 24000, 23000, 22000,
            21000, 20000, 19000, 38000, 28000, 27000, 36000]
        }]
    });
});