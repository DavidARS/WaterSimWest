//Source: http://stackoverflow.com/questions/19491336/get-url-parameter-jquery
//Get URL parameter 'name'
$.urlParam = function (name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href.toLowerCase());
    if (results == null) {
        return null;
    }
    else {
        return results[1] || 0;
    }
}

function getUserParameter(){
    console.log($.urlParam('user'));
}

function getPageType() {
    if (window.location.href.contains('Ipad.aspx'))
        return 'Ipad';
    else
        return 'Default';
}