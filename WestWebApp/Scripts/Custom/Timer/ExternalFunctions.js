// Disable Right Click / contextmenu
//$(document).on("contextmenu", function (event) { event.preventDefault(); });

// $('.footer').append(window.location.href);

//var primaryURL = "http://America.watersimDCDC.org/",
var primaryURL = "https://america.watersimdcdc.org/";
var secondaryURL = "https://america.quaytest.net/";

$('#idDebugControls').append('<div> <p style="font-size:12px;">' + primaryURL + '<br>' + secondaryURL);

function callNativeApp(message){
    try {
        webkit.messageHandlers.callbackHandler.postMessage('Hello from JavaScript');
    } catch(err) {
        console.log('The native context does not exist yet');
    }
}

function sendMessage(input) {
    try {
        webkit.messageHandlers.sendMessage.postMessage(input);
    } catch(err) {
        console.log('The native context does not exist yet');
    }
}

function startTimersMessage(){
    try {
        webkit.messageHandlers.sendMessage.postMessage('startTimers');
        return true;
    } catch(err) {
        return false;
    }
}

function isIpadApp(){
    try {
        webkit.messageHandlers.sendMessage.postMessage('testing');
        return true;
    } catch(err) {
        return false;
    }
}

function connectionLost(){
    try {
        webkit.messageHandlers.sendMessage.postMessage('connectionLost');
        return true;
    } catch(err) {
        return false;
    }
}

function callNativeSessionStore(session){
    try {
        webkit.messageHandlers.storeSessionHandler.postMessage(session);
    } catch(err) {
        console.log('The native context does not exist yet');
    }
}

function ajaxError(where, jqXHR, textStatus, errorThrown, saveUserSession){
    userSession.state = getState();
    sendMessage('ajaxError');
    $.ajax({
        type: 'POST',
        url: './services/servererror.php',
        data: {
            key: userSession.key, state: userSession.state, statusCode: jqXHR.status,
            statusText: textStatus, details: errorThrown, where: where, date: new Date().toGMTString()
        },
        dataType: 'json',
        error: function (jqXHR, textStatus, errorThrown) {
            // do something when error...
            console.log('ajaxError error: ', jqXHR, textStatus, errorThrown);
            callNativeSessionStore(JSON.stringify(userSession));
            connectionLost();
        },
        success: function (data, textStatus, jqXHR) {
            console.log('ajaxError success: ', data);
            callNativeSessionStore(JSON.stringify(userSession));
            if(!saveUserSession){
                saveUserSession();
            }
            connectionLost();
        }
    });
}

function testAjaxError(){
    $.ajax({
        type: 'POST',
        url: 'boom.html',
        data: {},
        dataType: 'json',
        error: function (jqXHR, textStatus, errorThrown) {
            // do something when error...
            console.log('testAjaxError error: ', jqXHR, textStatus, errorThrown)
            ajaxError('testAjaxError', jqXHR, textStatus, errorThrown);
        },
        success: function (data, textStatus, jqXHR) {
            console.log('ajaxError success: ', data);
            connectionLost();
        }
    });
}

function getPrimaryURL(){
    return primaryURL;
}

function getSecondaryURL(){
    return secondaryURL;
}

function setSessionKey(key){
    userSession.key = key;
}