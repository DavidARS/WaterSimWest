function buildInactivityDialog(){
    var width = $('body').width() - (40 + 21) * 2;
    var dialog = $(
        '<div class="timer-details-panel" style="left: 0px; top: 0px; height: 400px; width: 0px;">' + 
        '<div class="timer-step-description" style="width: ' + width + 'px; left: 40px; top: 20px; background-color: yellow;">' + 
        '<h2 style="color: black;">No Activity</h2>' +
        '<div class="timer-step-text" style="color: black;"><p>' + 
        'WaterSim will restart unless the "Yes" button is pressed in the next 10 seconds.' + 
        '</p>' + 
        '</div>' + 
        generateButtonHTML(
            'yes-button',
            'Yes',
            'background-color: rgb(76, 175, 80); float: right; border-color: black;',
            'inactiveButtonClick()'
            ) +
        '</div>' + 
        '</div>'
    );
    
    return dialog;
}

function buildDialog(title, content, hideSeconds){
    var seconds = ' <span></span> seconds.';
    
    if(hideSeconds)
        seconds = '';

    var width = $('body').width() - (40 + 21) * 2;
    var dialog = $(
        '<div class="timer-details-panel" style="left: 0px; top: 0px; height: 400px; width: 0px;">' + 
        '<div class="timer-step-description" style="width: ' + width + 'px; left: 40px; top: 20px; background-color: yellow;">' + 
        '<h2 style="color: black;">' + title + '</h2>' +
        '<div class="timer-step-text" style="color: black;"><p>' + 
        content + seconds + 
        '</p>' + 
        '</div>' +
        '</div>' + 
        '</div>'
    );
    
    return dialog;
}

function getDialogSpan(timer){
    return timer.dialog.find('span');
}

function generateButtonHTML(id, value, style, callback){
    return '<input type="button" id="' + id + '" value="' + value + '"' + 
    'style="margin: 5px; height: 40px; width: 200px; text-align: center; padding: 1px;' + 
    'font-family: Arial; font-size: 30px; ' + style + '" onclick="' + callback + '"">';
}

function inactiveButtonClick(){
    inactivityTimer.currentTimeLimit++;
    closeDialog(inactivityTimer);

    setNextChallengeTimer();
    setNoRunTimer();
}

var inactivityTimer = {
    time: 0,
    interval: null,
    increment: 1000, // 1 second
    timeLimit: [60*4, 60*2], // in seconds
    // timeLimit: [10, 15, 20], // in seconds
    currentTimeLimit: 0,
    countdown: 10, // in seconds
    dialog: null,
    dialogOpen: false,
    paused: false,
    interaction: 'inactivityTimer dialog opened',
    name: 'inactivityTimer',
    // 06/23/17 disabled timers
    //enabled: true,
    enabled: false,
}

var noRunTimer = {
    time: 0,
    interval: null,
    increment: 1000, // 1 second
    timeLimit: 30, // in seconds
    countdown: 30, // in seconds
    // timeLimit: 10, // in seconds
    // countdown: 10, // in seconds
    dialog: null,
    dialogOpen: false,
    paused: false,
    interaction: 'noRunTimer dialog opened',
    name: 'noRunTimer',
    shown: false,
    // 06/23/17 disabled timers
    //enabled: true,
    enabled: false,
    ran: false,
}

var nextChallengeTimer = {
    time: 0,
    interval: null,
    increment: 1000, // 1 second
    timeLimit: 135, // in seconds
    countdown: 30, // in seconds
    // timeLimit: 5, // in seconds
    // countdown: 30, // in seconds
    dialog: null,
    dialogOpen: false,
    paused: false,
    interaction: 'nextChallengeTimer dialog opened',
    name: 'nextChallengeTimer',
    enabled: false,
}

var idleTime = 0;
var inactiveDialogOpen = false,
countdownDialogShown = false;
var idleInterval, timeLimitInterval;
var limitTime = 0;

$(document).ready(function () {

    // $('#Finish_button').css('background-color', 'purple')

    // Build each timer's dialog
    inactivityTimer.dialog = buildInactivityDialog();

    noRunTimer.dialog = buildDialog(
        'Run The Model',
        'To implement your policy choices, you have to press the "Run Model" Button.',
        true
        );
    noRunTimer.span = getDialogSpan(noRunTimer);

    // Change 'Next Challenge Text'
    nextChallengeTimer.dialog = buildDialog(
        'Time\'s Up!',
        'Try and finish up your choices. WaterSim will move on to the next challenge in'
        );
    nextChallengeTimer.span = getDialogSpan(nextChallengeTimer);

    // Increment the idle time counter every second.
    // inactivityTimer.interval = setInterval(inactivityCheck, inactivityTimer.increment);
    // nextChallengeTimer.interval = setInterval(nextChallengeCheck, nextChallengeTimer.increment);

    if(!startTimersMessage()){
        console.log("iPad App not detected!");
    }
    else{
        sendMessage('playsound');
    }

    //Zero the idle timer on mouse movement.
    $(this).mousemove(function (e) {
        if(!inactivityTimer.dialogOpen){
            inactivityTimer.time = 0;
            inactivityTimer.currentTimeLimit = 0;
        }
    });
    $(this).keypress(function (e) {
        if(!inactivityTimer.dialogOpen){
            inactivityTimer.time = 0;
            inactivityTimer.currentTimeLimit = 0;
        }
    });

    captureUserInteractions();
    $($('.ui-tabs-anchor')[1]).click(function(){
        $('#isotope-demand-container').isotope('layout');
    });
});

function startTimers(){
    // inactivityTimer.interval = setInterval(inactivityCheck, inactivityTimer.increment);
    // nextChallengeTimer.interval = setInterval(nextChallengeCheck, nextChallengeTimer.increment);
    
    if(inactivityTimer.enabled)
        setInactivityTimer();

    if(nextChallengeTimer.enabled)
        setNextChallengeTimer();

    sendMessage('Timers Started!');
}

function setNextChallengeTimer(){
    nextChallengeTimer.paused = false;
    if(nextChallengeTimer.enabled)
        nextChallengeTimer.interval = setInterval(nextChallengeCheck, nextChallengeTimer.increment);
}

function setInactivityTimer(){
    inactivityTimer.paused = false;
    if(inactivityTimer.enabled)
        inactivityTimer.interval = setInterval(inactivityCheck, inactivityTimer.increment);
}

function setNoRunTimer(status){
    noRunTimer.paused = false;
    if(noRunTimer.started || status)
        noRunTimer.interval = setInterval(noRunCheck, noRunTimer.increment);
}

function openDialog(timer){
    $("body").append(timer.dialog);
    timer.dialogOpen = true;
    storeUserInteraction(timer.interaction);
}

function closeDialog(timer){
    timer.time = 0;
    timer.dialog.remove();
    timer.dialogOpen = false;
    console.log('timer.time:', timer.time);
}

function clearTimerInterval(timer){
    clearInterval(timer.interval);
    closeDialog(timer);
    timer.paused = false;

    if(timer.interaction.indexOf('noRun') > -1){
        noRunTimer.started = false;
    }
}

function pauseTimerInterval(timer){
    console.log(timer.name, ' paused');
    clearInterval(timer.interval);
    timer.paused = true;
}

function inactivityCheck() {
    inactivityTimer.time++;

    if(inactivityTimer.time > inactivityTimer.timeLimit[inactivityTimer.currentTimeLimit] && !inactivityTimer.dialogOpen){

        if(inactivityTimer.currentTimeLimit > 0){
            inactivityTimer.dialog.find('input').remove();
            // inactivityTimer.dialog.find('p').text('Thank you for playing.');
            inactivityTimer.dialog.find('p').remove();
            inactivityTimer.dialog.find('h2').text('Thank you for playing.');
        }

        openDialog(inactivityTimer);

        if(noRunTimer.dialogOpen)
            clearTimerInterval(noRunTimer);
        // pauseTimerInterval(nextChallengeTimer);
        // pauseTimerInterval(noRunTimer);
    }
    else if(inactivityTimer.time > inactivityTimer.timeLimit[inactivityTimer.currentTimeLimit] + inactivityTimer.countdown && inactivityTimer.dialogOpen){
        inactivityTimerCallback();
    }
    if(!(inactivityTimer.time % 5))
        console.log('inactivityCheck: ', inactivityTimer.time);

}

function inactivityTimerCallback(){
    clearTimerInterval(inactivityTimer);
    clearTimerInterval(noRunTimer);
    clearTimerInterval(nextChallengeTimer);

    storeUserInteraction('inactivityTimer Timeout');

    sendMessage("should call native");
    saveUserSession();

    console.log("stop everything!");
}

function noRunCheck(){
    noRunTimer.time++;

    var totalTime = noRunTimer.timeLimit + noRunTimer.countdown;

    if(noRunTimer.time > noRunTimer.timeLimit && !noRunTimer.dialogOpen){
        openDialog(noRunTimer);
        // noRunTimer.span.text(totalTime - noRunTimer.time + 1);
        noRunTimer.shown = true;

        // pauseTimerInterval(inactivityTimer);
        // pauseTimerInterval(nextChallengeTimer);
    }
    else if(noRunTimer.time > totalTime && noRunTimer.dialogOpen){
        // noRunTimerCallback();
    }
    else if(noRunTimer.time > noRunTimer.timeLimit && noRunTimer.dialogOpen){
        // noRunTimer.span.text(totalTime - noRunTimer.time + 1);
    }
    if(!(noRunTimer.time % 5))
        console.log('noRunCheck: ', noRunTimer.time);
}

function noRunTimerCallback(){
    storeUserInteraction('noRunTimer Timeout');

    // Double check on if timer restarts on run or clears
    clearTimerInterval(noRunTimer);
    $('#run-model-Main').click();
}

function startNoRunTimer(){
    if(userSession.continueClicked && CurrentModal < RestartModalValue &&
        userSession.actionsSinceContinue != userSession.userInteraction.length && noRunTimer.enabled){
        userSession.continueClicked = false;
        setNoRunTimer(true);
        noRunTimer.started = true;
        noRunTimer.enabled = false;
    }
}

function nextChallengeCheck(){
    nextChallengeTimer.time++;

    var totalTime = nextChallengeTimer.timeLimit + nextChallengeTimer.countdown;

    if(nextChallengeTimer.time  > nextChallengeTimer.timeLimit && !nextChallengeTimer.dialogOpen){
        if(CurrentModal == RestartModalValue ||
            (CurrentModal == RestartModalValue - 1 && userSession.continueClicked)){
            nextChallengeTimer.dialog = buildDialog('Game Over',
                'Thank you for playing Watersim! The App will be restarting in the next');
            nextChallengeTimer.span = getDialogSpan(nextChallengeTimer);
        }
        openDialog(nextChallengeTimer);
        nextChallengeTimer.span.text(totalTime - nextChallengeTimer.time + 1);

        pauseTimerInterval(inactivityTimer);
        pauseTimerInterval(noRunTimer);
    }
    else if(nextChallengeTimer.time > totalTime && nextChallengeTimer.dialogOpen){
        nextChallengeTimerCallback();
    }
    else if(nextChallengeTimer.time > nextChallengeTimer.timeLimit && nextChallengeTimer.dialogOpen){
        nextChallengeTimer.span.text(totalTime - nextChallengeTimer.time + 1);
    }

    if(!(nextChallengeTimer.time % 5))
        console.log('nextChallengeCheck: ', nextChallengeTimer.time);
}

function nextChallengeTimerCallback(){
    storeUserInteraction('nextChallengeTimer Timeout');
    nextChallenge();
}

//-----------------------------------------------------------
// User Session Logging
var userSession = {
    key: 'test',
    startTime: (new Date()).getTime(),
    endTime: (new Date()).getTime(),
    inactiveCount: 0,
    progressionCount: 0,
    appState: 0,
    state: '',
    smithValue: '',
    SNValue: '',
    userInteraction: [],
    logging: false,
    actionsSinceContinue: 0,
    continueClicked: false,
}
var saveCount = 0;

function storeUserInteraction(name) {
    // userSession.userInteraction.push((new Date()).getTime() + ',' + name);
    // QUAY EDIT 5 18 16 BEGIN
    if (userSession) {
        userSession.userInteraction.push((new Date()).getTime() + ':' + name);
        console.log(userSession.userInteraction[userSession.userInteraction.length - 1]);
    }
    // QUAY EDIT 5 18 16 END
}
function captureUserInteractions() {
    $('span.ui-button-text').click(function (e) {
        storeUserInteraction(this.parentElement.getAttribute('for'));
        startNoRunTimer();
    });

    $('.ui-tabs-anchor').click(function (e) {
        storeUserInteraction('TAB_' + this.innerText);
        startNoRunTimer();
    });

    $('#run-model-Main').click(function (e) {
        storeUserInteraction('Run Model');
        setTimeout(addLegendItemListener, 1000); // 1 second
    });

    addLegendItemListener();
    addNextButtonListener();

    // QUAY EDIT 5 18 16 BEGIN
    if (userSession) {
        userSession.logging = true;
    }
    // QUAY EDIT 5 18 16 END
}

function addLegendItemListener(){
    $('.highcharts-legend-item').click(function(e){
        storeUserInteraction('highcharts-legend-item_' + d3.select(this).select('tspan').text());
        startNoRunTimer();
    });
}

function addNextButtonListener(){
    $('.sideshow-next-step-button').click(function(e){
        storeUserInteraction('SIDESHOW_' + this.innerText);
        // QUAY EDIT 5 18 16 BEGIN
        if (userSession) {
            userSession.actionsSinceContinue = userSession.userInteraction.length;
            userSession.continueClicked = true;
        }
        // QUAY EDIT 5 18 16 END

        if (CurrentModal == 2)
            setTimeout(addAssessmentListener, 1000);
    });
}

function addAssessmentListener(){
    $("img[id*='WSA']").click(function(e){
        storeUserInteraction('ASSESSMENT_' + this.parentNode.nextSibling.innerText.split(':')[0].trim())
    });
}

//Have to save on ipad
function saveUserSession(){
    if(saveCount < 2){
        // QUAY EDIT 5 18 16 BEGIN
        if (userSession) {
            userSession.appState = CurrentModal;
            userSession.endTime = (new Date()).getTime();
            userSession.state = getState();

            // userSession.userInteraction = userSession.userInteraction.join('\n');
            userSession.userInteraction = userSession.userInteraction.join(',');
            sendUserSession();
        }
        // QUAY EDIT 5 18 16 END
    }
}

function sendUserSession(){
    $.ajax({
        type: 'GET',
        url: 'http://vader.lab.asu.edu/GCloudTomcat/WaterSim/services/storesessionget?callback=?',
        data: userSession,
        dataType: 'jsonp',
        error: function (jqXHR, textStatus, errorThrown) {
            // do something when error...
            console.log('error: ', jqXHR, textStatus, errorThrown)
            if(saveCount < 2){
                saveCount++;
                sendUserSession();
            }
            else{
                ajaxError('saveUserSession', jqXHR, textStatus, errorThrown, true);
                // callNativeApp();
            }
        },
        success: function (data, textStatus, jqXHR) {
            console.log('success: ', data);
            callNativeSessionStore(JSON.stringify(userSession));
            callNativeApp(JSON.stringify(userSession));
            userSession.sentInteractions = userSession.userInteraction;
            userSession.userInteraction = [];
        }
    });
}

function nextChallenge(){
    clearTimerInterval(inactivityTimer);
    clearTimerInterval(noRunTimer);
    clearTimerInterval(nextChallengeTimer);

    var waitTime = 0;
    if(Sideshow.active){
        $('.sideshow-next-step-button').click();
        waitTime = 1000
    }

    // evoke the currentModal sideshow
    setTimeout(function(){
        if(CurrentModal < 3){
            config = {};
            config.wizardName = ModalWizards[CurrentModal];
            Sideshow.start(config);
        }
        else{
            saveUserSession();
            return;
        }

        addLegendItemListener();
        addNextButtonListener();

        startTimers();
    },
    waitTime);
}

$('#Finish_button').click(function(e){

    if(userSession.logging){
        storeUserInteraction(this.value)
    }

    if($(this).val() != "Next Challenge"){
        // QUAY EDIT 5 18 16 BEGIN
        if (userSession) {
            saveUserSession();
        }
        // QUAY EDIT 5 18 16 END
        return;
    }

    sendMessage('callWebService');
    
    if(userSession.continueClicked || noRunTimer.started){
        console.log('finish clicked!');

        clearTimerInterval(inactivityTimer);
        clearTimerInterval(noRunTimer);
        clearTimerInterval(nextChallengeTimer);

        addLegendItemListener();
        addNextButtonListener();
        noRunTimer.shown = false;

        startTimers();
    }
});

// If model ran within a challenge do we not clear timer?
$('#run-model-Main').click(function(e){
    clearTimerInterval(noRunTimer);

    if(inactivityTimer.paused)
        setInactivityTimer();
    if(nextChallengeTimer.paused)
        setNextChallengeTimer();

    sendMessage('callWebService');
});