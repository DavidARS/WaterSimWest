// September, 2014: DAS added to kludge the splash page - stop it from writing header text
$("#dashboard-header-h1").hide();

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

// STEPTOE 04_2018 Commented out new styling for tabs
//// Settings - Scenarios tabs
//$(function () {
//    var tabs = $("#settings-tabs-scenarios").tabs();
//});

// Settings - Climate tabs
$(function () {
    var tabs = $("#settings-tabs-climate").tabs();
});

$(function () {
    var tabs = $("#settings-tabs-drought").tabs();
});
$(function () {
    var tabs = $("#settings-tabs-climateDrought").tabs();
});

// Settings - Trace tabs
//$(function () {
//    var tabs = $("#settings-tabs-trace").tabs();
//});
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
      if ($(".wrapper").hasClass("open")) {
          $("#pageslide").fadeToggle();
          $(".wrapper").toggleClass("open");
          $(".wrapper-dt").toggleClass("open");
      }
  });
//
  $("#settings-button-container-ipad").click(function () {
      if ($(".wrapper").hasClass("open")) {
          $("#pageslide").fadeToggle();
          $(".wrapper").toggleClass("open");
          $(".wrapper-dt").toggleClass("open");
      }
      else {
          alert("Please Click 'Load Selected Providers' under the 'Geography' tab!");
      }
  });


  //$(".decision-theater-mode-button").click(function () {
  //    $("#pageslide").fadeToggle();
 // });

 // $(".decision-theater-mode-button-off").click(function () {
 //     $("#pageslide").fadeToggle();
 // });


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
 // DAS added on 06.21.14 from UI_11_10
  //jQuery tool tip for settings button
  $(function () {
      $(document).tooltip();
  });

  // --------------------------------------------------------------------------
  // DAS September 2014. 
  // open a new tab for the troubleshoot html file
  // found on the status bar (icon)
  var targetUrl = 'Content/HELPFILES/HELP_TROUBLESHOOT.html';
  $("#WindowObject_TS").click(function () {

      window.open(targetUrl);

     // $(this).slideUp();

  });
  $(window).load(function () {
      function resizeImage() {
          img.css('height', el.height());
      }

      $(window).resize(function () {
          resizeImage();
      });

      var el = $("#WindowObject_TS");
      var img = $("#TShootImg");
      resizeImage();
  });
//
// DAS November 2014. 
// open a new tab for the document html file
// found on the status bar (icon)
  var targetUrl_2 = 'Content/HELPFILES/HELP_DOCUMENT.html';
  $("#WindowObject_DOC").click(function () {

      window.open(targetUrl_2);

      //$(this).slideUp();

  });
  $(window).load(function () {
      function resizeImage() {
          img.css('height', el.height());
      }

      $(window).resize(function () {
          resizeImage();
      });

      var el = $("#WindowObject_DOC");
      var img = $("#DOCImg");
      resizeImage();
  });


  // end of DAS September-November, 2014
// -------------------------------------------------------------------------  
// DAS 10.06.14
  //$(function () {
  //    $('.openFS').click(function () {
  //        $('body').fullscreen();
  //        return false;
  //    });
  //    $('.closeFS').click(function () {
  //        $.fullscreen.exit();
  //        return false;
  //    });
  //});
// -------------------------------------------------------------------------  

  //<!-- 03.09.15 DAS removed this code from loading-->
  // Wizard walk through
  //$("#wizard").steps();

  //$(".open-wizard").click(function () {
  //    $("#wizard").fadeIn();
  //});
  //$(".close-wizard").click(function () {
  //    $("#wizard").fadeOut();
  //});
  //$('a[href="#finish"]').click(function () {
  //    // $("#wizard").fadeOut();
  //    $("#pageslide").fadeToggle();
  //    $(".wrapper").toggleClass("open");
  //    $("#wizard-t-0").toggleClass("open");
  //    $("#wizard-t-1").toggleClass("open");
  //    $("#wizard-t-2").toggleClass("open");
  //    $("#wizard-t-3").toggleClass("open");
  //    $("#wizard-t-4").toggleClass("open");
  //});

  //$('#settings-button-container').click(function () {
  //    $("#pageslide").fadeOut();
  //    $(".wrapper").removeClass("open");
  //    $("#wizard-t-0").removeClass("open");
  //    $("#wizard-t-1").removeClass("open");
  //    $("#wizard-t-2").removeClass("open");
  //    $("#wizard-t-3").removeClass("open");
  //    $("#wizard-t-4").removeClass("open");
  //});

  //$('a[href="#previous"]').click(function () {
  //    $("#pageslide").fadeOut();
  //    $(".wrapper").removeClass("open");
  //    $("#wizard-t-0").removeClass("open");
  //    $("#wizard-t-1").removeClass("open");
  //    $("#wizard-t-2").removeClass("open");
  //    $("#wizard-t-3").removeClass("open");
  //    $("#wizard-t-4").removeClass("open");
  //});

  //$('#wizard-t-0').click(function () {
  //    $("#pageslide").fadeOut();
  //    $(".wrapper").removeClass("open");
  //    $("#wizard-t-0").removeClass("open");
  //    $("#wizard-t-1").removeClass("open");
  //    $("#wizard-t-2").removeClass("open");
  //    $("#wizard-t-3").removeClass("open");
  //    $("#wizard-t-4").removeClass("open");
  //});

  //$('#wizard-t-1').click(function () {
  //    // $("#wizard").fadeOut();
  //    $("#pageslide").fadeOut();
  //    $(".wrapper").removeClass("open");
  //    $("#wizard-t-0").removeClass("open");
  //    $("#wizard-t-1").removeClass("open");
  //    $("#wizard-t-2").removeClass("open");
  //    $("#wizard-t-3").removeClass("open");
  //    $("#wizard-t-4").removeClass("open");
  //});

  //$('#wizard-t-2').click(function () {
  //    // $("#wizard").fadeOut();
  //    $("#pageslide").fadeOut();
  //    $(".wrapper").removeClass("open");
  //    $("#wizard-t-0").removeClass("open");
  //    $("#wizard-t-1").removeClass("open");
  //    $("#wizard-t-2").removeClass("open");
  //    $("#wizard-t-3").removeClass("open");
  //    $("#wizard-t-4").removeClass("open");
  //});

  //$('#wizard-t-3').click(function () {
  //    // $("#wizard").fadeOut();
  //    $("#pageslide").fadeOut();
  //    $(".wrapper").removeClass("open");
  //    $("#wizard-t-0").removeClass("open");
  //    $("#wizard-t-1").removeClass("open");
  //    $("#wizard-t-2").removeClass("open");
  //    $("#wizard-t-3").removeClass("open");
  //    $("#wizard-t-4").removeClass("open");
  //});

  //$('#wizard-t-4').click(function () {
  //    // $("#wizard").fadeOut();
  //    $("#pageslide").fadeOut();
  //    $(".wrapper").removeClass("open");
  //    $("#wizard-t-0").removeClass("open");
  //    $("#wizard-t-1").removeClass("open");
  //    $("#wizard-t-2").removeClass("open");
  //    $("#wizard-t-3").removeClass("open");
  //    $("#wizard-t-4").removeClass("open");
  //});

  //     // Decision Theater Mode

  //    $('.decision-theater-mode-button').show();
  //    $('.decision-theater-mode-button-off').hide();

  //    $(".decision-theater-mode-button").click(function () {
  //        $('#dragResize-dashboard-off').attr('id', 'dragResize-dashboard');
  //        $('#dragResize-policies-off').attr('id', 'dragResize-policies');
  //        $('#left-sidebar').attr('id', 'left-sidebar-dt');
  //        $('#dragResize-tab-1-off').attr('id', 'dragResize-tab-1');
  //        $('#dragResize-tab-2-off').attr('id', 'dragResize-tab-2');
  //        $('#dragResize-tab-3-off').attr('id', 'dragResize-tab-3');
  //        $('#dragResize-tab-4-off').attr('id', 'dragResize-tab-4');
  //        $('#dragResize-tab-5-off').attr('id', 'dragResize-tab-5');
  //        $('#dragResize-tab-6-off').attr('id', 'dragResize-tab-6');
  //        $('.wrapper').attr('class', 'wrapper-dt');
  //        $('.scrollbar').attr('class', 'scrollbar-off');
  //        $('.footer').attr('class', 'footer-dt');
  //        $('#tabs').attr('id', 'tabs-off');
  //        $("#tabs-off").css({ "position": "absolute" });
  //        $("#tabs-1").css({ "display": "block" });
  //        $("#tabs-2").css({ "display": "block" });
  //        $("#tabs-3").css({ "display": "block" });
  //        $("#tabs-4").css({ "display": "block" });
  //        $("#tabs-5").css({ "display": "block" });
  //        $("#tabs-6").css({ "display": "block" });
  //        $('.decision-theater-mode-button').hide();
  //        $('.decision-theater-mode-button-off').show();

  //        // Draggable objects
  //        $(function () {
  //            $("#dragResize-dashboard").draggable().resizable();
  //            $("#dragResize-policies").draggable();
  //            $("#dragResize-tab-1").draggable().resizable({
  //                width: 'auto',
  //                cursor: 'move'
  //            });
  //            $("#dragResize-tab-2").draggable().resizable({
  //                width: 'auto',
  //                cursor: 'move'
  //            });
  //            $("#dragResize-tab-3").draggable().resizable({
  //                width: 'auto',
  //                cursor: 'move'
  //            });
  //            $("#dragResize-tab-4").draggable().resizable({
  //                width: 'auto',
  //                cursor: 'move'
  //            });
  //            $("#dragResize-tab-5").draggable().resizable({
  //                width: 'auto',
  //                cursor: 'move'
  //            });
  //            $("#dragResize-tab-6").draggable().resizable({
  //                width: 'auto',
  //                cursor: 'move'
  //            });


  //        });



  //        $('.click-to-top').click(function () {
  //            var topZ = 0;
  //            $('.click-to-top').each(function () {
  //                var thisZ = parseInt($(this).css('z-index'), 10);
  //                if (thisZ > topZ) {
  //                    topZ = thisZ;
  //                }
  //            });
  //            $(this).css('z-index', topZ + 1);
  //        });
  
  //    });

  //    $(".decision-theater-mode-button-off").click(function () {
  //        $('#dragResize-dashboard').attr('id', 'dragResize-dashboard-off');
  //        $('#dragResize-policies').attr('id', 'dragResize-policies-off');
  //        $('#left-sidebar-dt').attr('id', 'left-sidebar');
  //        $('#dragResize-tab-1').attr('id', 'dragResize-tab-1-off');
  //        $('#dragResize-tab-2').attr('id', 'dragResize-tab-2-off');
  //        $('#dragResize-tab-3').attr('id', 'dragResize-tab-3-off');
  //        $('#dragResize-tab-4').attr('id', 'dragResize-tab-4-off');
  //        $('#dragResize-tab-5').attr('id', 'dragResize-tab-5-off');
  //        $('#dragResize-tab-6').attr('id', 'dragResize-tab-6-off');
  //        $('.wrapper-dt').attr('class', 'wrapper-dt');
  //        $('.scrollbar-off').attr('class', 'scrollbar');
  //        $('.footer-dt').attr('class', 'footer');
  //        $('#tabs-off').attr('id', 'tabs');
  //        $("#tabs").css({ "position": "relative" });
  //        $("#tabs-1").css({ "display": "block" });
  //        $("#tabs-2").css({ "display": "none" });
  //        $("#tabs-3").css({ "display": "none" });
  //        $("#tabs-4").css({ "display": "none" });
  //        $("#tabs-5").css({ "display": "none" });
  //        $("#tabs-6").css({ "display": "none" });
  //        $('.decision-theater-mode-button').show();
  //        $('.decision-theater-mode-button-off').hide();
  //        $('#dragResize-dashboard-off').removeAttr('class');
  //        $('#dragResize-policies-off').removeAttr('class');
  //        $('#dragResize-tab-1-off').removeAttr('style');
  //        $('#dragResize-tab-2-off').removeAttr('style');
  //        $('#dragResize-tab-3-off').removeAttr('style');
  //        $('#dragResize-tab-4-off').removeAttr('style');
  //        $('#dragResize-tab-5-off').removeAttr('style');
  //        $('#dragResize-tab-6-off').removeAttr('style');
  //        $('#dragResize-tab-1-off').removeAttr('class');
  //        $('#dragResize-tab-2-off').removeAttr('class');
  //        $('#dragResize-tab-3-off').removeAttr('class');
  //        $('#dragResize-tab-4-off').removeAttr('class');
  //        $('#dragResize-tab-5-off').removeAttr('class');
  //        $('#dragResize-tab-6-off').removeAttr('class');
  //    });


      $("#OpenNew").click(function () {
          myurl = document.location;
          myNewHome = window.open(myurl, "WaterSim Web", "menubar=1,resizable=1");
          myNewHome.window.moveTo(0, 0);
          myNewHome.window.resizeTo(screen.width, screen.height);
          window.open('', '_self', '');
          window.close();
      });
 