//A Sideshow For the WaterSim AMerica Game
//This activates the 3 Modals and support calls for the game

Sideshow.config.language = "en";
Sideshow.config.autoSkipIntro = true;
Sideshow.init();
Sideshow.actionComplete = false;
Sideshow.active = false;

$(document).on('click', '.sideshow-close-button', function (e) {
    Sideshow.active = false;
    console.log("close clicked");
});
$('.sideshow-close-button').hide();

// Modal One; Invoke First
// --------------------------------------------------------------------------
//  var userState = getState();
var CurrentModal = 0;
var RestartModalValue = 3;

var ModalWizards = new Array();
ModalWizards[0] = "Modal 1";
ModalWizards[1] = "Modal 2";
ModalWizards[2] = "Modal 3";


Sideshow.registerWizard({
    name: ModalWizards[0],
    //name: "Modal 1",
    title: "Its 2065!",
    description: "Depict 2065 and provide some guidance on what to do to balance the system. ",
    estimatedTime: "1 Minutes",
    affects: [
		function () {
		    //Here we could do any checking to infer if this tutorial is eligible the current screen/context. 
		    //After this checking, just return a boolean indicating if this tutorial will be available. 
		    //As a simple example we're returning a true, so this tutorial would be available in any context
		    return true;
		}
    ],
    listeners: {
        beforeWizardStarts: function () {
            //What goes here will be executed before the first step of this tutorial
            SetFinishButtonWaiting()
            // QUAY EDIT 4/12/16 BEGIN
            HideHighestInputControlValue();
            // QUAY EDIT 4/12/16 END;
            // QUAY EDIT 4/19/16 BEGIN
            playsound(1);
            // QUAY EDIT 4/19/16 END;

            Sideshow.active = true;
        },
        afterWizardEnds: function () {
            //What goes here will be executed after the last step of this tutorial
            Sideshow.active = false;
            // QUAY EDIT 4/19/16 BEGIN
            stopsound(1);
            // QUAY EDIT 4/19/16 END;
            // var mydiv = document.getElementsByClassName("marquee");

          //  $('.marquee').show();
            CurrentModal++;
          //  mydiv[0].style.visibility = "visible";

            console.log("WizardEnds");
        }
    }
}).storyLine({
    showStepPosition: false,
    steps:[
    	
    	{
    	    title: "Water Challenge: Population Growth",
    	    text: "The Year is 2065. Population and water demands have grown in your state. <BR>" +
                  " <BR>The chart’s buckets show how water is being used in your state. Red levels in the buckets mean more water is being used than supplied. <BR>" +
                    " <BR>Reduce red levels by selecting policies that are connected to the buckets, then press the “RUN MODEL” button.  Watch for changes in your buckets and indicators.<BR> " +
                    "<BR>When you are finished with this challenge, press the “NEXT CHALLENGE” button.<BR>" +
                    " <BR>Now press “CONTINUE” to begin balancing your water system. Can you keep your Sustainability Indicators out of the red and still balance your system?"
                    , 
    	    
    	    // + "Press 'Done' when finished.",
    	    subject: ".wrapper",
    	    format: "markdown",
    	    listeners: {
                        afterStep: function () {
                            Sideshow.active = false;
                            }
                         }
        }
    ]
});
// Modal Two - Invoke second
// -------------------------------------------------------------------------
Sideshow.registerWizard({
    name: ModalWizards[1],
    //name: "Modal 2",
    title: "WaterSim Guided Tour",
    description: "Initiate the Drought and then provide access to the model again. ",
    estimatedTime: "1 Minutes",
    affects: [
		function () {
		    //Here we could do any checking to infer if this tutorial is eligible the current screen/context. 
		    //After this checking, just return a boolean indicating if this tutorial will be available. 
		    //As a simple example we're returning a true, so this tutorial would be available in any context
		    return true;
		}
    ],
    listeners: {
        beforeWizardStarts: function () {
            //What goes here will be executed before the first step of this tutorial
       //     var mydiv = document.getElementsByClassName("marquee");
      //      mydiv[0].style.visibility = "hidden";
            // QUAY EDIT 4/12/16 BEGIN
            ShowHighestInputControlValue();
            // QUAY EDIT 4/12/16 END;

            // QUAY EDIT 4/21/16 BEGIN
            //callWebService(getJSONData('drought'));
            DroughtFlag = true;
            callWebService(getJSONData('ALL'));
            // QUAY EDIT 4/21/16 END
            SetFinishButtonWaiting();
            // QUAY EDIT 4/19/16 BEGIN
            playsound(2);
            // QUAY EDIT 4/19/16 END;
            Sideshow.active = true;
            //$('.marquee').hide();
        },
        afterWizardEnds: function () {
            //What goes here will be executed after the last step of this tutorial
            Sideshow.active = false;
            // QUAY EDIT 4/19/16 BEGIN
            stopsound(2);
            // QUAY EDIT 4/19/16 END;
            //   var mydiv = document.getElementsByClassName("marquee");
            //mydiv[0].innerHTML = "Drought has REDUCED surface water supplies; NOW manage for drought!";

            CurrentModal++;
            console.log("WizardEnds");
       //     mydiv[0].style.visibility = "visible";
        }
    }
}).storyLine({
    showStepPosition: false,
    steps: [
    	{
    	    title: "Water Challenge: Long Term Drought",
    	    text: "The year is 2065. <BR>"+
            
            "<BR> You did a good job of planning for growth, but now a long-term drought has reduced your water supplies. <BR>" +
                  "<BR>Surface water supplies have been reduced and demand is a little higher because of the heat associated with the drought.<BR>  "+
                  "<BR>Reduce red levels by selecting policies that are connected to the buckets, then press the “RUN MODEL” button.  Watch for changes in your buckets and indicators. <BR>" +
                   "<BR>When you are finished with this challenge, press the “NEXT CHALLENGE” button.<BR>" +
                " <BR>Now press “CONTINUE” to begin balancing your water system. Can you still keep your Sustainability Indicators out of the red and balance your system?"
                ,
    	    subject: ".wrapper",
    	    format: "markdown",
    	    listeners: {
    	        afterStep: function () {
    	            Sideshow.active = false;
    	        }
    	    }
    	}
    ]
});
// Modal Three - Invoke LAST
// ---------------------------------------------------------------------------
Sideshow.registerWizard({
    name: ModalWizards[2],
    //name: "Modal 3",
    title: "WaterSim Guided Tour",
    description: "Bring up the Assessment Tool. ",
    estimatedTime: "1 Minutes",
    affects: [
		function () {
		    //Here we could do any checking to infer if this tutorial is eligible the current screen/context. 
		    //After this checking, just return a boolean indicating if this tutorial will be available. 
		    //As a simple example we're returning a true, so this tutorial would be available in any context
		    return true;
		}
    ],
    listeners: {
        beforeWizardStarts: function () {
            //What goes here will be executed before the first step of this tutorial
            SetFinishButtonWaiting()
       //     var mydiv = document.getElementsByClassName("marquee");
       //     mydiv[0].style.visibility = "hidden";
            // QUAY EDIT 4/19/16 BEGIN
            playsound(3);
            // QUAY EDIT 4/19/16 END;
            Sideshow.active = true;
        },
        afterWizardEnds: function () {
            //What goes here will be executed after the last step of this tutorial
            Sideshow.active = false;
            // QUAY EDIT 4/19/16 BEGIN
            stopsound(3);
            // QUAY EDIT 4/19/16 END;

            CurrentModal++;
            SetFinishButtonAssessment();
            EvokeAssessmentReport("idAssessment", WS_RETURN_DATA);
            $("#idAssessment").show();
            //$("#Notabs").hide();
            $("#tabs").hide();
            console.log("WizardEnds");
        }
    }
}).storyLine({
    showStepPosition: false,
    steps: [
    	
        {
            title: "Water Challenges: More To Do!",
            text: "Good Job!<BR>" +
                    "<BR>Even when your system is balanced, there are challenges to keep your system sustainable. Are all of your Sustainability Indicators in the solid green zones? If not, here is a report of things you can do to improve the sustainability of your state’s water system.<BR>"+
                    "<BR>Press “CONTINUE” to see your sustainability assessment report.<BR>" +
                    "<BR>When you are finished, press the “End Game” button on the lower right side of the display."
            ,
            subject: ".wrapper",
            format: "markdown",
            listeners: {
                afterStep: function () {
                    Sideshow.active = false;
                }
            }
        }
    ]
});

var tempVal = 0;
$('#Finish_button').click(function(){
    // Check if this is a restart request
    if (CurrentModal == RestartModalValue) {
        // do the magic here
        // FOr now just closing down

        // STEPTOE EDIT 05/03/16
        // Commented out for iPad to process data and restart app instead of restarting page.
        // window.location.reload();
    }
    else {
        // evoke the currentModal sideshow
        // STEPTOE EDIT 07/05/17 Disable Sideshow
        //config = {};
        //config.wizardName = ModalWizards[CurrentModal];
        //Sideshow.start(config);

        if (tempVal == 0) {
            ShowHighestInputControlValue();
            // QUAY EDIT 4/12/16 END;

            // QUAY EDIT 4/21/16 BEGIN
            //callWebService(getJSONData('drought'));
            DroughtFlag = true;
            callWebService(getJSONData('ALL'));
            // QUAY EDIT 4/21/16 END
            SetFinishButtonWaiting();
            tempVal++;
        }
    }

 
});
