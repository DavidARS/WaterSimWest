/// <reference path="/Scripts/jquery-ui-1.8.24.min.js" />

// Topics - Supply, Demand, and Climate tabs
$(function() {
    var tabs = $( "#tabs" ).tabs();
});

// Settings - Time tabs
$(function () {
    var tabs = $("#settings-tabs-time").tabs();
});

// Settings - Geography tabs
$(function () {
    var tabs = $("#settings-tabs-geography").tabs();
});

// Settings - Scenarios tabs
$(function () {
    var tabs = $("#settings-tabs-scenarios").tabs();
});

// Settings - Climate tabs
$(function () {
    var tabs = $("#settings-tabs-climate").tabs();
});

// Checkbox show hide for city inputs in settings
//function evaluate() {
//    var item = $(this);
//    var relatedItem = $("#" + item.attr("data-related-item")).parent();

//    if (item.is(":checked")) {
//        relatedItem.fadeIn();
//    } else {
//        relatedItem.fadeOut();
//    }
//}

//$('input[type="checkbox"]').click(evaluate).each(evaluate);

// Exclusive checkboxes for Settings
$("input:checkbox").click(function () {
    if ($(this).is(":checked")) {
        var group = "input:checkbox[name='" + $(this).attr("name") + "']";
        $(group).prop("checked", false);
        $(this).prop("checked", true);
    } else {
        $(this).prop("checked", false);
    }
});

// jQuery vertical accordion
  $(function () {
      $("#accordion").accordion({
          heightStyle: "content",
          active: false,
          collapsible: true
      });
  });

// jQuery horizontal accordion
  $(document).ready(function () {
      $("#accordion").msAccordion({ 
          defaultid: 5 
      });
  });

  $(".on-off-project-button").hide();
  $("#pageslide").hide();

  $(".display").click(function () {
      $(".on-off-project-button").hide();
  });

  $(".population-button").click(function () {
      $(".on-off-project-button").show();
  });

  $("#pageslide-button-container").click(function () {
      $("#pageslide").fadeToggle();
      $(".wrapper").toggleClass("open");
      $(".wrapper-dt").toggleClass("open");
  });
// DAS
  $("#settings-button-container").click(function () {
      $("#pageslide").fadeToggle();
      $(".wrapper").toggleClass("open");
      $(".wrapper-dt").toggleClass("open");
  });

//
  $(".decision-theater-mode-button").click(function () {
      $("#pageslide").fadeToggle();
  });

  $(".decision-theater-mode-button-off").click(function () {
      $("#pageslide").fadeToggle();
  });


// jQuery model popups for user inputs
  $("#effluent-popup-subcontrols").hide();

  $(function () {
      $('#effluent-popup-button').click(function () {
          $("#effluent-popup-subcontrols").dialog({
              modal: true,
              minWidth: 300,
              buttons: {
                  Ok: function () {
                      $(this).dialog("close");
                  }
              }
          });
      });
  });


// Decision Theater Mode

  $('.decision-theater-mode-button').show();
  $('.decision-theater-mode-button-off').hide();

  $(".decision-theater-mode-button").click(function () {
      $('#dragResize-dashboard-off').attr('id', 'dragResize-dashboard');
      $('#dragResize-policies-off').attr('id', 'dragResize-policies');
      $('#left-sidebar').attr('id', 'left-sidebar-dt');
      $('#dragResize-tab-1-off').attr('id', 'dragResize-tab-1');
      $('#dragResize-tab-2-off').attr('id', 'dragResize-tab-2');
      $('#dragResize-tab-3-off').attr('id', 'dragResize-tab-3');
      $('#dragResize-tab-4-off').attr('id', 'dragResize-tab-4');
      $('.wrapper').attr('class', 'wrapper-dt');
      $('.scrollbar').attr('class', 'scrollbar-off');
      $('.footer').attr('class', 'footer-dt');
      $('#tabs').attr('id', 'tabs-off');
      $("#tabs-off").css({ "position": "absolute" });
      $("#tabs-1").css({ "display": "block" });
      $("#tabs-2").css({ "display": "block" });
      $("#tabs-3").css({ "display": "block" });
      $("#tabs-4").css({ "display": "block" });
      $('.decision-theater-mode-button').hide();
      $('.decision-theater-mode-button-off').show();

      // Draggable objects
      $(function () {
          $("#dragResize-dashboard").draggable().resizable({
              handles: 'e, w'
          });
          $("#dragResize-policies").draggable();
          $("#dragResize-tab-1").draggable().resizable({
              handles: 'e, w',
              width: 'auto',
              cursor: 'move'
          });
          $("#dragResize-tab-2").draggable().resizable({
              handles: 'e, w',
              width: 'auto',
              cursor: 'move'
          });
          $("#dragResize-tab-3").draggable().resizable({
              handles: 'e, w',
              width: 'auto',
              cursor: 'move'
          });
          $("#dragResize-tab-4").draggable().resizable({
              handles: 'e, w',
              width: 'auto',
              cursor: 'move'
          });
      });



      $('.click-to-top').click(function () {
          var topZ = 0;
          $('.click-to-top').each(function () {
              var thisZ = parseInt($(this).css('z-index'), 10);
              if (thisZ > topZ) {
                  topZ = thisZ;
              }
          });
          $(this).css('z-index', topZ + 1);
      });
  
  });

  $(".decision-theater-mode-button-off").click(function () {
      $('#dragResize-dashboard').attr('id', 'dragResize-dashboard-off');
      $('#dragResize-policies').attr('id', 'dragResize-policies-off');
      $('#left-sidebar-dt').attr('id', 'left-sidebar');
      $('#dragResize-tab-1').attr('id', 'dragResize-tab-1-off');
      $('#dragResize-tab-2').attr('id', 'dragResize-tab-2-off');
      $('#dragResize-tab-3').attr('id', 'dragResize-tab-3-off');
      $('#dragResize-tab-4').attr('id', 'dragResize-tab-4-off');
      $('.wrapper-dt').attr('class', 'wrapper-dt');
      $('.scrollbar-off').attr('class', 'scrollbar');
      $('.footer-dt').attr('class', 'footer');
      $('#tabs-off').attr('id', 'tabs');
      $("#tabs").css({ "position": "relative" });
      $("#tabs-1").css({ "display": "block" });
      $("#tabs-2").css({ "display": "none" });
      $("#tabs-3").css({ "display": "none" });
      $("#tabs-4").css({ "display": "none" });
      $('.decision-theater-mode-button').show();
      $('.decision-theater-mode-button-off').hide();
      $('#dragResize-dashboard-off').removeAttr('class');
      $('#dragResize-policies-off').removeAttr('class');
      $('#dragResize-tab-1-off').removeAttr('style');
      $('#dragResize-tab-2-off').removeAttr('style');
      $('#dragResize-tab-3-off').removeAttr('style');
      $('#dragResize-tab-4-off').removeAttr('style');
      $('#dragResize-tab-1-off').removeAttr('class');
      $('#dragResize-tab-2-off').removeAttr('class');
      $('#dragResize-tab-3-off').removeAttr('class');
      $('#dragResize-tab-4-off').removeAttr('class');
  });




/*

  // Exit full screen and decision theater mode
  $(document).keydown(function (e) {

      if (e.keyCode == 27) {
          $('#decision-theater-left').attr('id', 'decision-theater-left-no-dt');
          $('#decision-theater-right').attr('id', 'decision-theater-right-no-dt');
          $('#watersim').attr('id', 'watersim-no-dt');
          $('#drop').attr('id', 'drop-no-dt');
          $('.draggable').attr('class', 'draggable-off');
          $('#pageslide').fadeOut();
          $(".wrapper").removeClass("open");
      }   // esc
      //if (e.keyCode == 13) { alert('ENTER Pressed'); }     // enter
      //if (e.keyCode == 9) { alert('TAB Pressed'); }     // tab
      var tagName = jQuery(this).find(':focus').get(0).tagName;
      //if (tagName == 'INPUT') return false;

  });
  */

  /* 
  Native FullScreen JavaScript API
  -------------
  Assumes Mozilla naming conventions instead of W3C for now
  

  (function() {
      var 
          fullScreenApi = { 
              supportsFullScreen: false,
              isFullScreen: function() { return false; }, 
              requestFullScreen: function() {}, 
              cancelFullScreen: function() {},
              fullScreenEventName: '',
              prefix: ''
          },
          browserPrefixes = 'webkit moz o ms khtml'.split(' ');
	
      // check for native support
      if (typeof document.cancelFullScreen != 'undefined') {
          fullScreenApi.supportsFullScreen = true;
      } else {	 
          // check for fullscreen support by vendor prefix
          for (var i = 0, il = browserPrefixes.length; i < il; i++ ) {
              fullScreenApi.prefix = browserPrefixes[i];
			
              if (typeof document[fullScreenApi.prefix + 'CancelFullScreen' ] != 'undefined' ) {
                  fullScreenApi.supportsFullScreen = true;
				
                  break;
              }
          }
      }
	
      // update methods to do something useful
      if (fullScreenApi.supportsFullScreen) {
          fullScreenApi.fullScreenEventName = fullScreenApi.prefix + 'fullscreenchange';
		
          fullScreenApi.isFullScreen = function() {
              switch (this.prefix) {	
                  case '':
                      return document.fullScreen;
                  case 'webkit':
                      return document.webkitIsFullScreen;
                  default:
                      return document[this.prefix + 'FullScreen'];
              }
          }
          fullScreenApi.requestFullScreen = function(el) {
              return (this.prefix === '') ? el.requestFullScreen() : el[this.prefix + 'RequestFullScreen']();
          }
          fullScreenApi.cancelFullScreen = function(el) {
              return (this.prefix === '') ? document.cancelFullScreen() : document[this.prefix + 'CancelFullScreen']();
          }		
      }

      // jQuery plugin
      if (typeof jQuery != 'undefined') {
          jQuery.fn.requestFullScreen = function() {
	
              return this.each(function() {
                  var el = jQuery(this);
                  if (fullScreenApi.supportsFullScreen) {
                      fullScreenApi.requestFullScreen(el);
                  }
              });
          };
      }

      // export api
      window.fullScreenApi = fullScreenApi;	
  })();


  // do something interesting with fullscreen support
  var fsButton = document.getElementById('fsbutton'),
      fsElement = document.getElementById('specialstuff'),
      fsStatus = document.getElementById('fsstatus');


  if (window.fullScreenApi.supportsFullScreen) {
      fsStatus.innerHTML = 'YES: Your browser supports FullScreen';
      fsStatus.className = 'fullScreenSupported';
	
      // handle button click
      fsButton.addEventListener('click', function() {
          window.fullScreenApi.requestFullScreen(fsElement);
      }, true);
	
      fsElement.addEventListener(fullScreenApi.fullScreenEventName, function() {
          if (fullScreenApi.isFullScreen()) {
              fsStatus.innerHTML = 'Whoa, you went fullscreen';
          } else {
              fsStatus.innerHTML = 'Back to normal';
          }
      }, true);
	
  } else {
      fsStatus.innerHTML = 'SORRY: Your browser does not support FullScreen';
  }

//====================================================

  $(document).ready(function () {

      // QUAY EDIT 3/16/14 Begin
      
      $("#closeGear").tooltip(
          {
              track: false, 
              delay: 2, 
              showURL: false, 
              opacity: 1, 
              showBody: " - ", 
              top: 2, 
              left: 1,
              position: { my: "left+40 bottom", at:"right"}, 
              content: "Click gear to close the Options/Settings Panel"
          });
      var tooltest = $("#openGear").tooltip(
          {
              track: false,
              delay: 2,
              showURL: false,
              opacity: 1,
              showBody: " - ",
              content: "Click gear icon to open options/settings panel to set options (Time Scale, Geography, Climate Forcing, Scenarios, Browser Mode)"
          });
       
  });

  */

  $("#OpenNew").click(function () {
     myurl = document.location;
     myNewHome = window.open(myurl, "WaterSim Web", "menubar=1,resizable=1");
     myNewHome.window.moveTo(0, 0);
     myNewHome.window.resizeTo(screen.width, screen.height);
     window.open('', '_self', '');
     window.close();


                   })





