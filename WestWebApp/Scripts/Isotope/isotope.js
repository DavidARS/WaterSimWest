// init Supply Isotope
var $isotope_supply_container = $('#isotope-supply-container').isotope({
    // options
});

// filter items on button click
$('#isotope-supply-filters').on('click', '.button', function (event) {
    var filtr = $(this).attr('data-filter');
    $isotope_supply_container.isotope({ filter: filtr });
});



// init Demand Isotope
var $isotope_demand_container = $('#isotope-demand-container').isotope({
    // options
});

// filter items on button click
$('#isotope-demand-filters').on('click', '.button', function (event) {
    var filtr = $(this).attr('data-filter');
    $isotope_demand_container.isotope({ filter: filtr });
});
// init Demand Isotope - DAS TEMP
var $isotope_demand_contain = $('#isotope-demand-contain').isotope({
    // options
});

// filter items on button click
$('#isotope-demand-filters').on('click', '.button', function (event) {
    var filtr = $(this).attr('data-filter');
    $isotope_demand_contain.isotope({ filter: filtr });
});



// init Climate Isotope
var $isotope_reservoir_container = $('#isotope-reservoir-container').isotope({
    // options
});

// filter items on button click
$('#isotope-reservoir-filters').on('click', '.button', function (event) {
    var filtr = $(this).attr('data-filter');
    $isotope_reservoir_container.isotope({ filter: filtr });
});


// init Sustainability Isotope
var $isotope_sustainability_container = $('#isotope-sustainability-container').isotope({
});

// filter items on button click
$('#isotope-sustainability-filters').on('click', '.button', function (event) {
    var filtr = $(this).attr('data-filter');
    $isotope_sustainability_container.isotope({ filter: filtr });
});

// init Climate Isotope
var $isotope_climate_container = $('#isotope-climate-container').isotope({
});

// filter items on button click
$('#isotope-climate-filters').on('click', '.button', function (event) {
    var filtr = $(this).attr('data-filter');
    $isotope_sustainability_container.isotope({ filter: filtr });
});

// init Validation Isotope
var $isotope_validation_container = $('#isotope-validation-container').isotope({
});

// filter items on button click
$('#isotope-validation-filters').on('click', '.button', function (event) {
    var filtr = $(this).attr('data-filter');
    $isotope_validation_container.isotope({ filter: filtr });
});