//=========================================================================
//  STEP 1
//   
//    
//============================================================================
//Sankey class: draws a sankey inside an SVG in the specified DIV
//use the update function to redraw with new data but same options as declaration
//linkColorScheme: (0 (Source), 1 (Target), 2 (Gradient))
function Sankey(fluxList, modelOutput, divID, options) {// width, height, linkColorScheme){

    var defaults = {
        width: 700, //SVG width
        svgHeight: 500, //SVG height
        linkColorScheme: 0, //0 (Source), 1 (Target), 2 (Gradient)
        units: "MGD", //units displayed with values
        nodeWidth: 20, //width of rects
        imgWidth: 40, //width of image
        imgHeight: 60, //height of image
        nodePadding: 40, //vertical space between nodes
        titlefontsize: "23px",
        titleOffset: 20,
        bucketfontsize: "16px",
        fontFamily: "'Lato', sans-serif",
        fontColor: "#4e4d4d",
        // QUAY EDIT 4/5/16 END
        autoScaleImgHeight: false, //scale image based on rect height
        showText: true, //show text label beside Resource/Consumer
        imgPath: "/Images/Sankey/White/" //User defined path for Resource/Consumer images
    };

    //fill defaults with user specified options
    if (typeof (options) != "undefined") {
        for (var option in options) {
            defaults[option] = options[option];
        }
    }

    //setup colors
    var red = '#E6585D';

    //Draw sandkey with specified parameters
    function drawSankey(fluxList, modelOutput, divID, defaults) {

        //console.log('Sankey drawSankey:', modelOutput)

        // QUAY EDIT 4/11/16
        // Loading of labels from INFO_REQUEST
        var GWLabel = "";
        var RECLabel = "";
        var SURLabel = "";
        var SURLLabel = "";
        var SALLable = "";
        var UDLabel = "";
        var ADLabel = "";
        var IDLabel = "";
        var PDLabel = "";

        $.each(INFO_REQUEST.FieldInfo, function () {
            switch (this.FLD) {
                case "GW_P":
                    GWLabel = this.LAB;
                    break;
                case "REC_P":
                    RECLabel = this.LAB;
                    break;
                case "SUR_P":
                    SURLabel = this.LAB;
                    break;
                case "SURL_P":
                    SURLLabel = this.LAB;
                    break;
                case "SAL_P":
                    SALLabel = this.LAB;
                    break;
                case "UD_P":
                    UDLabel = this.LAB;
                    break;
                case "AD_P":
                    ADLabel = this.LAB;
                    break;
                case "ID_P":
                    IDLabel = this.LAB;
                    break;
                case "PD_P":
                    PDLabel = this.LAB;
                    break;
            }
        });

        var fields = {
            "GW_P": { name: GWLabel },
            "REC_P": { name: RECLabel },
            "SUR_P": { name: SURLabel },
            "SURL_P": { name: SURLLabel },
            "SAL_P": { name: SALLabel }
        };

        //Loop through model results to get the Demand/Deficit for consumers
        for (var index = 0; index < modelOutput.RESULTS.length; index++) {
            switch (modelOutput.RESULTS[index].FLD) {
                case "UD_P":
                    var vals = modelOutput.RESULTS[index].VALS[0].VALS;
                    var field = {};

                    field.val = vals[vals.length - 1];
                    field.name = UDLabel; //"Urban/Rural";

                    fields[modelOutput.RESULTS[index].FLD] = field;
                    break;
                case "UDN_P":
                    var vals = modelOutput.RESULTS[index].VALS[0].VALS;
                    var field = {};
                    field.val = vals[vals.length - 1];

                    fields[modelOutput.RESULTS[index].FLD] = field;
                    break;
                case "ID_P":
                    var vals = modelOutput.RESULTS[index].VALS[0].VALS;
                    var field = {};
                    field.val = vals[vals.length - 1];
                    field.name = IDLabel;// "Industry";
                    fields[modelOutput.RESULTS[index].FLD] = field;
                    break;
                case "IDN_P":
                    var vals = modelOutput.RESULTS[index].VALS[0].VALS;
                    var field = {};
                    field.val = vals[vals.length - 1];
                    fields[modelOutput.RESULTS[index].FLD] = field;
                    break;
                case "AD_P":
                    var vals = modelOutput.RESULTS[index].VALS[0].VALS;
                    var field = {};

                    field.val = vals[vals.length - 1];
                    field.name = ADLabel; //"Agriculture";

                    fields[modelOutput.RESULTS[index].FLD] = field;
                    break;
                case "ADN_P":
                    var vals = modelOutput.RESULTS[index].VALS[0].VALS;
                    var field = {};
                    field.val = vals[vals.length - 1];
                    fields[modelOutput.RESULTS[index].FLD] = field;
                    break;
                case "PD_P":
                    var vals = modelOutput.RESULTS[index].VALS[0].VALS;
                    var field = {};

                    field.val = vals[vals.length - 1];
                    field.name = PDLabel;// "Power";

                    fields[modelOutput.RESULTS[index].FLD] = field;
                    break;
                case "PDN_P":
                    var vals = modelOutput.RESULTS[index].VALS[0].VALS;
                    var field = {};
                    field.val = vals[vals.length - 1];

                    fields[modelOutput.RESULTS[index].FLD] = field;
                    break;
                case "GW_P":
                case "REC_P":
                case "SUR_P":
                case "SURL_P":
                case "SAL_P":
                    var vals = modelOutput.RESULTS[index].VALS[0].VALS;
                    fields[modelOutput.RESULTS[index].FLD].val = vals[vals.length - 1];
                    break;
            }
        }

        //console.log('Sankey fields:', JSON.stringify(fields));

        var units = defaults.units,
        nodeWidth = defaults.nodeWidth,
        imgWidth = defaults.imgWidth,
        imgHeight = defaults.imgHeight,
        nodePadding = defaults.nodePadding;

        var divHeight = defaults.divHeight ? defaults.divHeight : 550;
        var margin = { top: 40, right: 5, bottom: 10, left: 5 },
          width = defaults.width - margin.left - margin.right,
          height = defaults.svgHeight - margin.top - margin.bottom;

        var formatNumber = d3.format(",.0f"),    // zero decimal places
            format = function (d) { return formatNumber(d) + " " + units; },
            color = d3.scale.category20();

        // Delete old Sankey
        d3.select(divID).select("svg").remove();

        // Setup to show all consumers unless they that are identified in STC.js
        var sources = ["SUR_P", "SURL_P", "SAL_P", "GW_P", "REC_P"];
        var demands = ["UD_P", "AD_P", "ID_P", "PD_P"];
        var deficits = ["UDN_P", "ADN_P", "IDN_P", "PDN_P"];
        var consumers = ["UD_P", "UDN_P", "AD_P", "ADN_P", "ID_P", "IDN_P", "PD_P", "PDN_P"];

        // Return true for consumers that are identified in the STC.js file
        function filterConsumers(item, index) {
            if (item.indexOf('N_') == -1) {
                return STC[providerRegion].SKCFLDS.indexOf(item) > -1;
            }
            else {
                var prefix = item.split('N_')[0] + '_P';
                return STC[providerRegion].SKCFLDS.indexOf(prefix) > -1;
            }
        }

        // If specified consumers have been identified then filter out others
        if (STC[providerRegion].SKCFLDS && STC[providerRegion].SKCFLDS.length) {
            demands = demands.filter(filterConsumers);
            deficits = deficits.filter(filterConsumers);
            consumers = consumers.filter(filterConsumers);
        }

        var fluxValues = [];
        fluxList.ForEach(function (d, i) {
            fluxValues.push(d.LastValue());
        });

        //set up graph in same style as original example but empty
        var graph = { "nodes": [], "links": [] };

        //console.log('Sankey fluxList:', fluxList);
        fluxList.ForEach(function (d) {
            if (d.LastValue() && consumers.indexOf(d.Consumer) > -1) {
                graph.nodes.push({ "name": d.Resource });
                graph.nodes.push({ "name": d.Consumer });
                graph.links.push({
                    "source": d.Resource,
                    "target": d.Consumer,
                    "value": +d.LastValue()
                });
            }
        });

        // return only the distinct / unique nodes
        graph.nodes = d3.keys(d3.nest()
          .key(function (d) { return d.name; })
          .map(graph.nodes));

        // loop through each link replacing the text with its index from node
        graph.links.forEach(function (d, i) {
            graph.links[i].source = graph.nodes.indexOf(graph.links[i].source);
            graph.links[i].target = graph.nodes.indexOf(graph.links[i].target);
        });

        //now loop through each nodes to make nodes an array of objects
        // rather than an array of strings
        graph.nodes.forEach(function (d, i) {
            graph.nodes[i] = { "name": d };
        });

        //console.log('Sankey graph:', JSON.stringify(graph));

        // define utility functions
        function getLinkID(d) {
            return "link-" + d.source.name + "-" + d.target.name;
        }
        function nodeColor(d) {
            return d.color = colorBrewer[d.name.replace(/ .*/, "")];
        }

        //create gradients if gradient is the current color scheme
        if (defaults.linkColorScheme == 2) {
            var defs = svg.append("defs");

            // create gradients for the links
            var grads = defs.selectAll("linearGradient")
                    .data(graph.links, getLinkID);

            grads.enter().append("linearGradient")
                    .attr("id", getLinkID)
                    .attr("gradientUnits", "objectBoundingBox");
            //stretch to fit

            grads.html("") //erase any existing <stop> elements on update
                .append("stop")
                .attr("offset", "0%")
                .attr("stop-color", function (d) {
                    return nodeColor((+d.source.x <= +d.target.x) ?
                                     d.source : d.target);
                });

            grads.append("stop")
                .attr("offset", "100%")
                .attr("stop-color", function (d) {
                    return nodeColor((+d.source.x > +d.target.x) ?
                                     d.source : d.target)
                });
        }

        //console.log('Sankey graph.links:', graph.links);

        // Append the svg to the page
        var svg = d3.select(divID).append("svg")
            .attr("width", width + margin.left + margin.right)
            .attr("height", height + margin.top + margin.bottom)
          .append("g")
            .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

        // Append Sources title to svg
        svg.append("text")
            .attr("x", 0)
            .attr("y", 0 - (margin.top / 2))
            .attr("text-anchor", "start")
            .style("font-family", defaults.fontFamily)
            .style("font-size", defaults.titlefontsize)
            .style("fill", defaults.fontColor)
            .text("Sources of Water");

        // Append Consumers title to svg
        svg.append("text")
            .attr("x", width)
            .attr("y", 0 - (margin.top / 2))
            .attr("text-anchor", "end")
            .style("font-family", defaults.fontFamily)
            .style("font-size", defaults.titlefontsize)
            .style("fill", defaults.fontColor)
            .text("Consumers of Water");

        // Filter out sources that are not specified in the sources array
        sources = sources.filter(function (name) {
            for (var i = 0; i < graph.nodes.length; i++) {
                if (graph.nodes[i].name == name) {
                    return true;
                }
            }

            return false;
        })

        function getDemandDeficitName(name) {
            return (name.split('_')[0]) + "N_P";
        }

        function getDemandMet(d, name) {
            var value = d.val - fields[getDemandDeficitName(name)].val;
            return value;
        }

        // Calculate the total value of consumer demand
        var yMax = 0;
        demands.forEach(function (name) {
            var obj = fields[name];

            yMax += Math.max(obj.val, 1);
        });

        // Create linear scales to map value to pixel
        var x = d3.scale.linear().range([0, width]);
        var y = d3.scale.linear().range([0, height - demands.length * nodePadding]);

        // Scale the range of the data
        x.domain([0, 1]);
        y.domain([0, yMax]);

        // Compute positions for nodes and links
        function computeLinkDepths() {
            var nodes = graph.nodes;
            var links = graph.links;

            // Calculate positions for sources
            var position = 0;
            sources.forEach(function (name) {
                var obj = fields[name];
                obj.x = x(0);
                obj.y = position;

                obj.dx = nodeWidth;
                obj.dy = y(Math.max(obj.val, 1));

                position += obj.dy + nodePadding;
            });

            // Calculate positions for consumers
            position = 0;
            consumers.forEach(function (name) {
                var condition = name.indexOf('N_P') > -1;
                var obj = fields[name];
                obj.x = x(1) - nodeWidth;
                obj.y = position;

                if (condition) {
                    position += nodePadding;
                    obj.dy = y(obj.val);
                }
                else {
                    obj.dy = y(Math.max(getDemandMet(obj, name), 1));
                }

                obj.dx = nodeWidth;
                position += obj.dy;
            });

            // Transmit calculated postions to graph nodes
            graph.nodes.forEach(function (d) {
                var obj = fields[d.name];
                d.x = obj.x;
                d.y = obj.y;

                d.dy = obj.dy;
                d.dx = obj.dx;
            });

            // Give each node source and target arrays
            nodes.forEach(function (node) {
                node.sourceLinks = [];
                node.targetLinks = [];
            });

            // Calculate height of all links
            links.forEach(function (link) {
                link.dy = y(link.value);
            });

            // Add link references to their respective source and target nodes
            links.forEach(function (link) {
                var source = link.source,
                    target = link.target;
                if (typeof source === "number") source = link.source = nodes[link.source];
                if (typeof target === "number") target = link.target = nodes[link.target];
                source.sourceLinks.push(link);
                target.targetLinks.push(link);
            });

            // Source the links based on their numerical ordering
            nodes.forEach(function (node) {
                node.sourceLinks.sort(ascendingTargetDepth);
                node.targetLinks.sort(ascendingSourceDepth);
            });

            // Compute each nodes value from the total of their source & target links
            nodes.forEach(function (node) {
                node.value = Math.max(
                  d3.sum(node.sourceLinks, value),
                  d3.sum(node.targetLinks, value)
                );
            });

            // Calculate offsets for the node links
            nodes.forEach(function (node) {
                var sy = 0, ty = 0;
                node.sourceLinks.forEach(function (link) {
                    link.sy = sy;
                    sy += link.dy;
                });

                node.targetLinks.forEach(function (link) {
                    link.ty = ty;
                    ty += link.dy;
                });
            });

            function value(link) {
                return link.value;
            }

            function ascendingSourceDepth(a, b) {
                return a.source.y - b.source.y;
            }

            function ascendingTargetDepth(a, b) {
                return a.target.y - b.target.y;
            }
        }

        computeLinkDepths();

        var curvature = .5;
        function path(d, offset) {
            var x0 = d.source.x + d.source.dx,
                x1 = d.target.x,
                xi = d3.interpolateNumber(x0, x1),
                x2 = xi(curvature),
                x3 = xi(1 - curvature),
                y0 = d.source.y + d.sy + d.dy / 2,
                y1 = d.target.y + d.ty + d.dy / 2;
            return "M" + x0 + "," + y0
                 + "C" + x2 + "," + y0
                 + " " + x3 + "," + y1
                 + " " + x1 + "," + y1;
        }

        // add in the links
        var link = svg.append("g").selectAll(".link")
            .data(graph.links)
          .enter().append("path")
            .attr("class", "link")
            .attr("d", path)
            .style("stroke", function (d) {
                switch (defaults.linkColorScheme) {
                        //Source
                    case 0:
                        d.color = colorBrewer[d.source.name.replace(/ .*/, "")];
                        break;
                        //Target
                    case 1:
                        d.color = colorBrewer[d.target.name.replace(/ .*/, "")];
                        break;
                        //Gradient
                    case 2:
                        d.color = "url(#" + getLinkID(d) + ")";
                        break;
                }
                return d.color;
            })
            .style("stroke-width", function (d) { return Math.max(1, d.dy); })
            .style("fill", "none")
            //.style("stroke-opacity", .5)
            .sort(function (a, b) { return b.dy - a.dy; });

        // add the link titles
        link.append("title")
              .text(function (d) {
                  return fields[d.source.name].name + " → " +
                          fields[d.target.name].name + "\n" + format(d.value);
              });

        var node = svg.append("g").selectAll(".node")
            .data(graph.nodes)
            .enter().append("g")
            .attr("class", "node")
            .attr("id", function (d) { return d.name + "_node"; })
            .attr("transform", function (d) {
                return "translate(" + d.x + "," + d.y + ")";
            });

        // add the rectangles for the nodes (Consumers/Resources)
        node.append("rect")
            .attr("height", function (d) {
                return d.dy;
            })
            .attr("width", nodeWidth)
            .style("fill", function (d) {
                return d.color = colorBrewer[d.name.replace(/ .*/, "")];
            })
            .style("stroke", function (d) {
                return d3.rgb(d.color).darker(2);
            })
          .append("title")
            .text(function (d) {
                if (fields[getDemandDeficitName(d.name)])
                    return fields[d.name].name + "\nDemand: " + format(d.value);
                else
                    return fields[d.name].name + "\nAvailable: " + format(d.value);
            });

        // add the Deficit rectangles for the Consumers
        node.filter(function (d) {
            switch (d.name) {
                case "UD_P":
                case "ID_P":
                case "AD_P":
                case "PD_P":
                    condition = true;
                    break;
                default:
                    condition = false;
                    break;
            }
            return condition;
        })
          .append("rect")
            .attr("height", function (d) {
                var name = d.name.split('_')[0];
                return fields[name + "N_P"].dy;
            })
            .attr("width", nodeWidth)
            .attr("transform", function (d) {
                return "translate(0," + (d.dy + 1) + ")";
            })
            .style("fill", function (d) {
                return d.color = red;
            })
            .style("stroke", function (d) {
                return d3.rgb(d.color).darker(2);
            })
          .append("title")
            .text(function (d) {
                var name = d.name.split('_')[0];
                return fields[d.name].name + "\nDeficit: " + format(fields[name + "N_P"].val);
            });

        if (defaults.showText) {
            // add in the text that displays beside the rectangles
            // for the nodes (Consumers/Resources)
            node.append("text")
                .style("font-size", defaults.bucketfontsize)
                .style("font-weight", "bold")
                .style("font-family", defaults.fontFamily)
                .style("fill", defaults.fontColor)
                .attr("x", -6)
                .attr("y", function (d) {
                    var ratio = 1;
                    if (fields[d.name + "N"])
                        ratio += fields[d.name + "N"].val / fields[d.name].val;

                    return (ratio) * d.dy / 2;
                })
                .attr("dy", ".35em")
                .attr("text-anchor", "end")
                .attr("transform", null)
                .text(function (d) { return fields[d.name].name; })
              .filter(function (d) { return d.x < width / 2; })
                .attr("x", 6 + nodeWidth)
                .attr("text-anchor", "start");
        }
    }
    //===========================================================================
    this.update = function (fluxList, modelOutput) {
        //this.sankey = drawSankey(fluxList, modelOutput, this.divID, this.defaults);
        drawSankey(fluxList, modelOutput, this.divID, this.defaults);
    }

    this.divID = divID;
    this.defaults = defaults;
    //this.sankey = drawSankey(fluxList, modelOutput, divID, defaults);
    drawSankey(fluxList, modelOutput, divID, defaults);
}