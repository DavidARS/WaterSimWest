//A Sideshow Tutorial Example
//This tutorial introduces the Sideshow basics to the newcomer

Sideshow.config.language = "en";
Sideshow.config.autoSkipIntro = true;
Sideshow.init();
Sideshow.actionComplete = false;
Sideshow.active = false;

//$(document).on('click', '.sideshow-close-button', function (e) {
//    Sideshow.active = false;
//    console.log("close clicked");
//});
//$('.sideshow-close-button').hide();

// Modal One; Invoke First
// --------------------------------------------------------------------------
//applicationState = 1;
//stepsCalled = [0, 0];
Sideshow.registerWizard({
    name: "WaterSim Basic Side Show",
    title: "WaterSim Guided Tour",
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
            Sideshow.active = true;
        },
        afterWizardEnds: function () {
            //What goes here will be executed after the last step of this tutorial
            Sideshow.active = false;
            console.log("WizardEnds");
        }
    }
}).storyLine({
    showStepPosition: true,
    steps:[
    	
    	{
    	    title: "Basic Side SHow for WaterSim",
    	    text: "Add Content", 
    	    
    	    // + "Press 'Done' when finished.",
    	    subject: ".wrapper",
    	    format: "markdown"
    	},
        {
            title: "The Second Slide",
		    text: "Add Content ",
		    listeners: {
		        afterStep: function () {
		            Sideshow.active = false;
		        }
		    }
		}
    ]
});

//$('#Finish_button').click(function(){
//    //change application state
//    applicationState++;

//	//make changes to model
//	//applicationState
//    config = {};
//    config.wizardName = "Modal 3";
//    Sideshow.start(config);
 
//});
