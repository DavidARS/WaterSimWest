
// FluxData List Class   Stores FluxData

function FluxDataList() {

    // the list of Flux Data

    this.List = new Array();

    // add a fluxdata item

    this.AddFlux = function (FluxDataItem) { this.List.push(FluxDataItem); };

    // finds a fluxdata item based on field name  Returns a FluxData object or null if not found

    this.FindByField = function (Fieldname) {

        founditem = null;

        for (FDLi = 0; FDLi < List.length; FDLi++) {

            if (List[FDLi].FieldName == Fieldname) {

                founditem = List[FDLi];

                break;

            }

        };

    };

    // Will process a function over all items in the list

    // do this must have form DoThis(FluxData)

    this.ForEach = function (DoThis) {

        if (DoThis != undefined) {

            for (FDLi = 0; FDLi < this.List.length; FDLi++) {

                DoThis(this.List[FDLi]);

            }

        }

    };

    // returns an array of FluxData items that match the ResourceFieldname

    this.GetResourceFluxes = function (ResourceFieldname) {

        results = new FluxDataList();

        for (FDLi = 0; FDLi < this.List.length; FDLi++) {

            if (this.List[FDLi].Resource == ResourceFieldname) {

                results.AddFlux(this.List[FDLi]);

            }

        };

        return results;

    };

    // returns an array of FluxData items that match the ConsumerFieldname

    this.GetConsumerFluxes = function (ConsumerFieldname) {

        results = new FluxDataList();

        for (FDLi = 0; FDLi < this.List.length; FDLi++) {

            if (this.List[FDLi].Consumer == ConsumerFieldname) {

                results.AddFlux(this.List[FDLi]);

            }

        };

        return results;

    };

}



//===========================================================================

// Flux Data Class



// Constructor

// Fieldname : String, the name of the fieldname for this fluxdata

// values: [Array].  array of values, one for each year

// Source: string, this is the fieldname of the Fluc Source (Resource)

// Target: string, this is the fieldname of the Flux Target (consumer)



function FluxData(FieldName, values, Source, Target) {

    // The fieldname coming from web service foir this flux data

    this.Fieldname = FieldName;

    // The array of Values (should be an array) may be just a single value

    this.Values = values;

    // the Resource Fieldname

    this.Resource = Source;

    // the Consumer Fieldname

    this.Consumer = Target;

    // gets the Lastvalue of teh array

    this.LastValue = function () {

        if (this.Values.length != undefined) {

            return this.Values[this.Values.length - 1];

        }

        else {

            return this.Values;

        }

    };

}

//-------------------------------------------------------------------

// This is where the flux data is stired

var TheFluxList = new FluxDataList();



function ProcessFluxData(TheOutput) {

    // STEPTOE COMMENTED THIS OUT
    //console.log("TheOutput", TheOutput)

    if (TheOutput.RESULTS != undefined) {

        TheFluxList.List = [];

        var TheResultsArray = TheOutput.RESULTS;

        for (ori = 0; ori < TheResultsArray.length; ori++) {

            theitem = TheResultsArray[ori];

            theFLD = theitem.FLD;

            theVALS = theitem.VALS[0].VALS;

            switch (theFLD) {

                case "SUR_UD_P":

                    FluxDataItem = new FluxData(theFLD, theVALS, "SUR_P", "UD_P");

                    TheFluxList.AddFlux(FluxDataItem);

                    break;

                case "SUR_AD_P":

                    FluxDataItem = new FluxData(theFLD, theVALS, "SUR_P", "AD_P");

                    TheFluxList.AddFlux(FluxDataItem);

                    break;

                case "SUR_ID_P":

                    FluxDataItem = new FluxData(theFLD, theVALS, "SUR_P", "ID_P");

                    TheFluxList.AddFlux(FluxDataItem);

                    break;

                case "SUR_PD_P":

                    FluxDataItem = new FluxData(theFLD, theVALS, "SUR_P", "PD_P");

                    TheFluxList.AddFlux(FluxDataItem);

                    break;

                case "SURL_UD_P":

                    FluxDataItem = new FluxData(theFLD, theVALS, "SURL_P", "UD_P");

                    TheFluxList.AddFlux(FluxDataItem);

                    break;

                case "SURL_AD_P":

                    FluxDataItem = new FluxData(theFLD, theVALS, "SURL_P", "AD_P");

                    TheFluxList.AddFlux(FluxDataItem);

                    break;

                case "SURL_ID_P":

                    FluxDataItem = new FluxData(theFLD, theVALS, "SURL_P", "ID_P");

                    TheFluxList.AddFlux(FluxDataItem);

                    break;

                case "SURL_PD_P":

                    FluxDataItem = new FluxData(theFLD, theVALS, "SURL_P", "PD_P");

                    TheFluxList.AddFlux(FluxDataItem);

                    break;

                case "GW_UD_P":

                    FluxDataItem = new FluxData(theFLD, theVALS, "GW_P", "UD_P");

                    TheFluxList.AddFlux(FluxDataItem);

                    break;

                case "GW_AD_P":

                    FluxDataItem = new FluxData(theFLD, theVALS, "GW_P", "AD_P");

                    TheFluxList.AddFlux(FluxDataItem);

                    break;

                case "GW_ID_P":

                    FluxDataItem = new FluxData(theFLD, theVALS, "GW_P", "ID_P");

                    TheFluxList.AddFlux(FluxDataItem);

                    break;

                case "GW_PD_P":

                    FluxDataItem = new FluxData(theFLD, theVALS, "GW_P", "PD_P");

                    TheFluxList.AddFlux(FluxDataItem);

                    break;

                case "REC_UD_P":

                    FluxDataItem = new FluxData(theFLD, theVALS, "REC_P", "UD_P");

                    TheFluxList.AddFlux(FluxDataItem);

                    break;

                case "REC_AD_P":

                    FluxDataItem = new FluxData(theFLD, theVALS, "REC_P", "AD_P");

                    TheFluxList.AddFlux(FluxDataItem);

                    break;

                case "REC_ID_P":

                    FluxDataItem = new FluxData(theFLD, theVALS, "REC_P", "ID_P");

                    TheFluxList.AddFlux(FluxDataItem);

                    break;

                case "REC_PD_P":

                    FluxDataItem = new FluxData(theFLD, theVALS, "REC_P", "PD_P");

                    TheFluxList.AddFlux(FluxDataItem);

                    break;

                case "SAL_UD_P":

                    FluxDataItem = new FluxData(theFLD, theVALS, "SAL_P", "UD_P");

                    TheFluxList.AddFlux(FluxDataItem);

                    break;

                case "SAL_AD_P":

                    FluxDataItem = new FluxData(theFLD, theVALS, "SAL_P", "AD_P");

                    TheFluxList.AddFlux(FluxDataItem);

                    break;

                case "SAL_ID_P":

                    FluxDataItem = new FluxData(theFLD, theVALS, "SAL_P", "ID_P");

                    TheFluxList.AddFlux(FluxDataItem);

                    break;

                case "SAL_PD_P":

                    FluxDataItem = new FluxData(theFLD, theVALS, "SAL_P", "PD_P");

                    TheFluxList.AddFlux(FluxDataItem);

                    break;



            }

        }

    }

}



//total = 0;

//function SumAll(FluxItem) {

//    total = total + FluxItem.LastValue();

//}

////=========================================================

//function ReportFluxData() {

//    mySurfaceList = TheFluxList.GetResourceFluxes("SUR");

//    mySurfaceList.ForEach(SumAll);

//    SurfaceValue = total;





//}

