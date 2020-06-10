//A Sideshow Tutorial Example
//This tutorial introduces the Sideshow basics to the newcomer

Sideshow.config.language = "en";
Sideshow.config.autoSkipIntro = true;
Sideshow.init();
Sideshow.actionComplete = false;
Sideshow.active = false;

$(document).on('click', '.sideshow-close-button', function (e) {
    Sideshow.active = false;
    console.log("close clicked");
});
//$('.sideshow-close-button').hide();

applicationState = 1;
stepsCalled = [0, 0];

Sideshow.registerWizard({
    name: "introducing_sideshow",
    title: "WaterSim Guided Tour",
    description: "Introducing the main features and concepts of WaterSim. ",
    estimatedTime: "3 Minutes",
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
    steps: [
    	{
    	    title: "Task #1",
    	    text: "It is 2065 and the state has new demands, but the same supply. " + 
    	    "The system is out of balance! You need to balance the system. " +
    	    "Select Policy Options to balance the system. " +
    	    "<h1>Press 'Next' to begin. </H1>", 
    	    //+
    	    //"Press 'Done' when finished.",
    	    subject: ".wrapper",
    	    //targets: "#tabs li",
    	    format: "markdown"
            //,
 	
    	    //listeners: {
            //    beforeStep: function(){
            //        console.log("Task #1(bs):", applicationState);
            //        // if(applicationState > 1)
            //    	   // Sideshow.gotoStep(applicationState);
            //    },
            //    afterStep: function(){
            //        console.log("Task #2(as):", applicationState);
            //        if(applicationState == 1){
            //            // Sideshow.close();
            //            // applicationState++;
            //            Sideshow.close();
            //        }
            //    	//make done button active
            //    }
            //}
    	    //Can use auto-continue feature to make a go button if not satisfied with next button
    	},
		{
		    title: "Add Supplies or Implement Conservation",
		    text: "Consider Increasing Power Efficiency",
		    subject: "#left-sidebar",
		    format: "markdown",
		    listeners: {
		        beforeStep: function () {
		            console.log("Task #2(bs):", applicationState);
		            // if(applicationState > 1)
		            // Sideshow.gotoStep(applicationState);
		        },
		        afterStep: function () {
		            console.log("Task #3(as):", applicationState);
		            if (applicationState == 1) {
		                // Sideshow.close();
		                // applicationState++;
		                Sideshow.close();
		            }
		            //make done button active
		        }
		    }



		},




        {
        	title: "Task #3",
        	text: "It is 2065 and a long lasting drought has occured! Surface supplies have been reduced. " + 
        	"The system is out of balance! " +
        	"Select Policy Options to balance the system. " + 
        	"Press 'Next' to begin. " +
    	    "Press 'Done' when finished.",
    	    listeners: {
                afterStep: function(){
                    console.log("Task #3:(as)", applicationState);
                	//make done button active
                    if(applicationState == 2 && stepsCalled[0]){
                        Sideshow.close();
                    }
                    else{
                        stepsCalled[0]++;
                    }
                }
            }
        },
        {
            title: "Great Job!",
            text: "Please press next to view your report. " + 
            "Press 'Restart' to start the game over.",
            listeners: {
		        afterStep: function () {
                    if(applicationState == 3 && stepsCalled[1]){
    		            Sideshow.active = false;
    		            //show report
                    }
                    else{
                        stepsCalled[1]++;
                    }
		        }
		    }
        }
    ]
});

$('#Finish_button').click(function(){
    //change application state
    applicationState++;

	//make changes to model
	//applicationState

	//show Sideshow
    Sideshow.start();
	Sideshow.gotoStep(applicationState);
});
