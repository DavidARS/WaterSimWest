$(function () {
$.getJSON('http://www.highcharts.com/samples/data/jsonp.php?filename=aapl-c.json&callback=?', function (data) {
    // Create the chart
    $('#HighTrace').highcharts('StockChart', {


        rangeSelector : {
            selected : 2
        },

        title : {
            text : 'AAPL Stock Price'
        },

        series : [{
            name : 'AAPL',
            data : data,
            tooltip: {
                valueDecimals: 2
            }
        }]
    });
});

});
