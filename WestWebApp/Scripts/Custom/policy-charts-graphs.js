// Policy - Pie Chart
$(function () {
    $('#policy-pie-relative').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
        title: {
            text: 'Relative Water'
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
            name: 'Relative Water',
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

$(function () {
    $('#policy-pie-absolute').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
        title: {
            text: 'Absolute Water'
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
            name: 'Absolute Water',
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