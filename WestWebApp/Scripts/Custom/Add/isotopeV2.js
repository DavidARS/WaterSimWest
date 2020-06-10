// external js:
// http://isotope.metafizzy.co/beta/isotope.pkgd.js

function loadIsotope() {
    $(function () {
        // initialize Isotope after all images have loaded
        var $container = $('.isotope').imagesLoaded(function () {
            $container.isotope({
                // options
                itemSelector: '.itemv2',
                layoutMode: 'masonry',
                masonry: {
                    columnwidth: 1
                }
            });
            $container.isotope('on', 'layoutComplete', function (isoInstance, laidOutItems) {
                //console.log('Isotope layout completed with ' + laidOutItems.length + ' items');
            });
        });


        // filter items on button click
        $('#isotope-filters').on('click', '.button', function (event) {
            var filtr = $(this).attr('data-filter');
            $container.isotope({ filter: filtr });
        });


        //Use .OutputControl instead of .itemv2 if button should be stationary
        $('.itemv2').each(function () {
            //Add resize button to each item   
            $(this).append('<img class="button-resizeChart" src="Images/maximizeWhite.jpg">');

            //Backup of old resize button
            //$(this).append('<button class="button-customChart" role="button" type="button" style="width:100px; margin-left: 10px">' + 'Resize<span class="ui-con ui-icon-arrowthick-2-se-nw"></span>' + '</button');
        });

        //Realigning the Region and Chart Type dropdowns
        $('.ddlflds').css('top', 0);
        $('.ddlflds').css('right', 30);
        $('.ddlType').css('top', 0);
        $('.ddlType').css('left', 0);

        //Realigning the Chart to leave trim empty space and resize to be smaller
        $('div[id*="ChartContainer"]').each(function () {
            $(this).css('top', 30);
            $(this).css('height', 255);
            if (typeof($(this).highcharts()) != "undefined" && ($(this).highcharts() != null))
                $(this).highcharts().reflow();
        });

        //Commented out due to warning
        //$container.isotope('layout');


        //When resize button is clicked change font size, toggle item class, and change chartContainer size
        $('.button-resizeChart').click(function () {
            var chartHeight = 256;
            var chartWidth = 500;
            var fontSize = "12px";

            var item = $(this).parent();
            item.toggleClass('gigante');

            var chartEdit = item.find('.highcharts-container').parent();

            //If chart is small make it bigger
            if (chartEdit.height() < 500) {
                chartHeight = 540;
                chartWidth = 745;
                fontSize = "20px";
            }

            chartEdit.height(chartHeight);
            chartEdit.width(chartWidth);

            chartEdit.highcharts().container.parentNode.style.fontSize = fontSize;
            chartEdit.highcharts(chartEdit.highcharts().options);

            chartEdit.highcharts().reflow();
            $container.isotope('layout');
        });
    });
}

