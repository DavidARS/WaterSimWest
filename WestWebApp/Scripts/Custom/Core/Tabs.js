var lastActiveInputTab = '#policyPane';

$('#main-input-container .nav-link').click(function () {
    $(lastActiveInputTab).removeClass('active');
    $(lastActiveInputTab + '-tab').removeClass('active');

    $(this.name).addClass('active');
    $(this.name + '-tab').addClass('active');

    lastActiveInputTab = this.name;
});

var lastActiveOutputTab = '#flowChart';

$('#main-output-container .nav-link').click(function () {
    $(lastActiveOutputTab).removeClass('active');
    $(lastActiveOutputTab + '-tab').removeClass('active');

    $(this.name).addClass('active');
    $(this.name + '-tab').addClass('active');

    lastActiveOutputTab = this.name;
});

var lastActiveScenariosTab = '#baseScenario';

$('#settings-scenarios-container .nav-link').click(function () {
    $(lastActiveScenariosTab).removeClass('active');
    $(lastActiveScenariosTab + '-tab').removeClass('active');

    $(this.name).addClass('active');
    $(this.name + '-tab').addClass('active');

    lastActiveScenariosTab = this.name;
});