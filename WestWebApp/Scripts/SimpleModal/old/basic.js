/*
 * SimpleModal Basic Modal Dialog
 * http://simplemodal.com
 *
 * Licensed under the MIT license:
 *   http://www.opensource.org/licenses/mit-license.php
 */
var myGlobal;
jQuery(function ($) {
	// Load dialog on page load
	//$('#basic-modal-content').modal();

	// Load dialog on click
	$('.help').click(function (e) {
	    //$('#basic-modal-content').modal();
	    // QUAY EDIT BEGIN 3/13/14
	    var test = $(this);
	    var test2 = $(this).closest('div[id*=ControlContainer]');
	    var url = "HELP_"+ $(this).closest('div[id*=ControlContainer]').attr('data-key') + ".html";
	    //var url = ($(this).closest('div[id*=ControlContainer]').attr('data-fld')).split(" ").join("");
	    var uri = $(this).find('input[id$=hvHelpURI]').val();
	    if (uri == undefined) { uri = "Content/HELPFILES/"; }
	    // QUAY EDIT BEGIN 3/13/14

	    // Display an external page using an iframe
	    var src = uri + url;
	    $.modal('<iframe src="' + src + '" height="470" width="770" style="border:0">', {
	        closeHTML: "",
	        containerCss: {
	            backgroundColor: "#fff",
	            borderColor: "#fff",
	            height: 470,
	            padding: 0,
	            width: 770,
	            maxWidth: 800,
	            maxHeight: 500,
	            provider: url.contains('LPROVIDERS')
	        },
	        overlayClose: true,
	        //STEPTOE EDIT BEGIN 11/10/15
	        //Check if it's the provider help, if it is then shift the dialog left to make it visible.
            //Smaller screens cover the text.
	        onOpen: function (dialog) {
	            myGlobal = dialog;
	            if (dialog.data.html().contains('LPROVIDERS'))
	                dialog.container.css('left', parseInt(dialog.container.css('left')) + $('#pageslide').width());
	            dialog.overlay.show();
	            dialog.container.show();
	            dialog.data.show();
	        },
	        onClose: function (dialog) {
	            if (dialog.data.html().contains('LPROVIDERS'))
	                dialog.container.css('left', parseInt(dialog.container.css('left')) - $('#pageslide').width());
	            $.modal.close(); // must call this!
	        }
	        //STEPTOE EDIT END 11/10/15
	    });

		return false;
	});
});