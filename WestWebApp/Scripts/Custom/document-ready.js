$(document).ready(function () {
    $("#slider").zAccordion({
        tabWidth: "15%",
        speed: 650,
        startingSlide: 0,
        auto: false,
        trigger: "click",
        slideClass: 'slider',
        animationStart: function () {
            $('#slider').find('li.slider-open div').css('display', 'none');
            $('#slider').find('li.slider-previous div').css('display', 'none');
        },
        animationComplete: function () {
            $('#slider').find('li div').fadeIn(600);
        },
        width: "100%",
        height: "200px"
    });
});