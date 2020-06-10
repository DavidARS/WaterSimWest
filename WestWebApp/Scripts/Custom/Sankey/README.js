//To use sankey.js include in load-files.js both it and its css file
//FluxData structure is required for sankey.js to work,
//include flux.js if not already defined

//function Sankey(fluxList, modelOutput, divID, options)

//DO NOT SCALE WxH below 400x400
//options = {
// width: 700, //SVG width
// height: 500, //SVG height
// linkColorScheme: 0, //0 (Source), 1 (Target), 2 (Gradient)
// units: "MGD", //units displayed with values
// nodeWidth: 20, //width of rects
// imgWidth: 40, //width of image
// imgHeight: 60, //height of image
// nodePadding: 50, //vertical space between nodes
// useQS: true, //use custom scaling to make rects match links
// autoScaleImgHeight: false //scale image based on rect height
// };

//Create a new Sankey
mySankey = new Sankey(TheFluxList, TheModelOutput, "#SANKEY1", {width: 400, height: 400});

//Update a current Sankey with new data
mySankey.update(TheFluxList, TheModelOutput);