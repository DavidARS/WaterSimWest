/*
 * SimpleModal Basic Modal Dialog
 * http://simplemodal.com
 *
 * Licensed under the MIT license:
 *   http://www.opensource.org/licenses/mit-license.php
 */
var myGlobal;

function showDialog(src, provider) {
    $.modal('<iframe src="' + src + '" height="500" width="770" style="border:0">', {
        closeHTML: "",
        containerCss: {
            backgroundColor: "#fff",
            borderColor: "#fff",
            height: 520,
            padding: 0,
            width: 800,
            maxWidth: 850,
            maxHeight: 850,
            provider: provider
        },
        overlayClose: true,
        //STEPTOE EDIT BEGIN 11/10/15
        //Check if it's the provider help, if it is then shift the dialog left to make it visible.
        //Smaller screens cover the text.
        onOpen: function (dialog) {
            myGlobal = dialog;
            if (dialog.data.html().indexOf('LPROVIDERS') > -1)
                dialog.container.css('left', parseInt(dialog.container.css('left')) + $('#pageslide').width());
            dialog.overlay.show();
            dialog.container.show();
            dialog.data.show();
        },
        onClose: function (dialog) {
            if (dialog.data.html().indexOf('LPROVIDERS') > -1)
                dialog.container.css('left', parseInt(dialog.container.css('left')) - $('#pageslide').width());
            $.modal.close(); // must call this!
        }
        //STEPTOE EDIT END 11/10/15
    });
}

jQuery(function ($) {
    //var itemClass = '.help';
    var itemClass = '.info-item';

	// Load dialog on page load
	//$('#basic-modal-content').modal();

	// Load dialog on click
    $(itemClass).click(function (e) {
	    //$('#basic-modal-content').modal();
	    // QUAY EDIT BEGIN 3/13/14
        var url = "HELP_" + $(this).attr('data-fld') + ".html";
	    //var url = ($(this).closest('div[id*=ControlContainer]').attr('data-fld')).split(" ").join("");
	    var uri = $(this).find('input[id$=hvHelpURI]').val();
	    if (uri == undefined) { uri = "Content/HELPFILES/"; }
	    // QUAY EDIT BEGIN 3/13/14

	    var provider = false;
	    if (url.indexOf('LPROVIDERS') > -1)
	        provider = true;
	    // Display an external page using an iframe
	    var src = uri + url;

	    showDialog(src, provider);

		return false;
	});
});