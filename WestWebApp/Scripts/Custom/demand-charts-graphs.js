/* Demand - Cities */
$(function () {
    $('#demand-total').highcharts({
        chart: {
            type: 'area'
        },
        title: {
            text: 'Total Demand'
        },
        subtitle: {
            text: 'Combination of Population and GPCD'
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
                text: 'Millions of Gallons'
            },
            labels: {
                formatter: function () {
                    return this.value / 1000 + 'M';
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
            name: 'Scottsdale',
            data: [null, null, null, null, null, 6, 11, 32, 110, 235, 369, 640,
                1005, 1436, 2063, 3057, 4618, 6444, 9822, 15468, 20434, 24126,
                27387, 29459, 31056, 31982, 32040, 31233, 29224, 27342, 26662,
                26956, 27912, 28999, 28965, 27826, 25579, 25722, 24826, 24605,
                24304, 23464, 23708, 24099, 24357, 24237, 24401, 24344, 23586,
                22380, 21004, 17287, 14747, 13076, 12555, 12144, 11009, 10950,
                10871, 10824, 10577, 10527, 10475, 10421, 10358, 10295, 10104]
        }, {
            name: 'Tempe',
            data: [null, null, null, null, null, null, null, null, null, null,
            5, 25, 50, 120, 150, 200, 426, 660, 869, 1060, 1605, 2471, 3322,
            4238, 5221, 6129, 7089, 8339, 9399, 10538, 11643, 13092, 14478,
            15915, 17385, 19055, 21205, 23044, 25393, 27935, 30062, 32049,
            33952, 35804, 37431, 39197, 45000, 43000, 41000, 39000, 37000,
            35000, 33000, 31000, 29000, 27000, 25000, 24000, 23000, 22000,
            21000, 20000, 19000, 18000, 18000, 17000, 16000]
        }]
    });
});

// Demand - Population by City
$(function () {
    $('#demand-population').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: 'Population'
        },
        subtitle: {
            text: '(in thousands of gallons per people)'
        },
        xAxis: {
            categories: [
                '2000',
                '2005',
                '2010',
                '2015',
                '2020',
                '2025',
                '2030',
                '2035',
                '2040',
                '2045',
                '2050'
            ]
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Thousands of People'
            }
        },
        tooltip: {
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                '<td style="padding:0"><b>{point.y:.1f} mm</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        series: [{
            name: 'Scottsdale',
            data: [49.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6]

        }, {
            name: 'Tempe',
            data: [83.6, 78.8, 98.5, 93.4, 106.0, 84.5, 105.0, 104.3, 91.2, 83.5, 106.6]

        }]
    });
});

// Demand - GPCD by City
$(function () {
    $('#demand-gpcd').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: 'GPCD'
        },
        subtitle: {
            text: '(in millions of gallons per day)'
        },
        xAxis: {
            categories: [
                '2000',
                '2005',
                '2010',
                '2015',
                '2020',
                '2025',
                '2030',
                '2035',
                '2040',
                '2045',
                '2050'
            ]
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Millions of Gallons per Day'
            }
        },
        tooltip: {
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                '<td style="padding:0"><b>{point.y:.1f} mm</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        series: [{
            name: 'Scottsdale',
            data: [83.6, 78.8, 98.5, 93.4, 106.0, 84.5, 105.0, 104.3, 91.2, 83.5, 106.6]

        }, {
            name: 'Tempe',
            data: [49.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6]

        }]
    });
});

// Demand - On/Off Project
$(function () {
    $('#demand-population-on-off-project').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: 'On/Off Project'
        },
        subtitle: {
            text: '(in thousands of people)'
        },
        xAxis: {
            categories: [
                '2000',
                '2005',
                '2010',
                '2015',
                '2020',
                '2025',
                '2030',
                '2035',
                '2040',
                '2045',
                '2050'
            ]
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Thousands of People'
            }
        },
        tooltip: {
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                '<td style="padding:0"><b>{point.y:.1f} mm</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        series: [{
            name: 'On Project - Scottsdale',
            data: [83.6, 78.8, 98.5, 93.4, 106.0, 84.5, 105.0, 104.3, 91.2, 83.5, 106.6]

        }, {
            name: 'On Project - Tempe',
            data: [49.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6]

        }, {
            name: 'Off Project - Scottsdale',
            data: [49.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6]

        }, {
            name: 'Off Project - Tempe',
            data: [49.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6]

        }]
    });
});