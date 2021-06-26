app.controller('xpGraph', ['$scope', function ($scope) {

}]);

app.directive('xpGraph', function ($window) {
	return {
		restrict: 'EA',
		template: "<svg width='850' height='200'></svg>",
		link: function ($scope, $element, $attrs) {

			var salesData = [
	{ hour: "2015-07-06T17:05:47Z", sales: 54, count: 10,id:1 },
	{ hour: "2015-07-07T17:05:47Z", sales: 66, count: 10,id:2 },
	{ hour: "2015-07-08T17:05:47Z", sales: 77, count: 9, id:3 },
	{ hour: "2015-07-09T17:05:47Z", sales: 70, count: 6, id:4 },
	{ hour: "2015-07-10T17:05:47Z", sales: 60, count: 18,id:5 },
	{ hour: "2015-07-11T17:05:47Z", sales: 63, count: 10,id:6 },
	{ hour: "2015-07-12T17:05:47Z", sales: 55, count: 20,id:7 },
	{ hour: "2015-07-13T17:05:47Z", sales: 47, count: 40,id:8 },
	{ hour: "2015-07-14T17:05:47Z", sales: 55, count: 30,id:9 },
	{ hour: "2015-07-15T17:05:47Z", sales: 30, count: 15,id:10}
			];
			
			var data = salesData;			
			var padding = 20;
			var count = -1;
			var pathClass = "path";
			var xScale, yScale, y1Scale, xAxisGen, yAxisGen, y1AxisGen, lineFun, lineFun2;
			var d3 = $window.d3;
			var rawSvg = $element.find('svg');
			var svg = d3.select(rawSvg[0]);
			var tooltip = d3.select("body").append("div")
			.attr("class", "tooltipD3")
			.style("opacity", 0);
	

			function setChartParameters() {			

				xScale = d3.time.scale()
					.domain([data[0].id, data[data.length - 1].id])
					.range([padding + 5, rawSvg.attr("width") - padding]);

				yScale = d3.scale.linear()
					.domain([0, d3.max(data, function (d) {
						return d.sales;
					})])
					.rangeRound([rawSvg.attr("height") - padding, 0]);

				y1Scale = d3.scale.linear()
				.domain([0, d3.max(data, function (d) {
					return d.count;
				})])
				.range([rawSvg.attr("height") - padding, 0]);

				xAxisGen = d3.svg.axis()
					.scale(xScale)
					.orient("bottom")
					.tickFormat(function () {
						count++;						
						//return parseDate.parse(data[count].hour);	
						return new Date(data[count].hour).toDateString();
					})
					.ticks(10)
				    .tickSize(10)
				    .tickPadding(0);
				

				yAxisGen = d3.svg.axis()
					.scale(yScale)
					.orient("left")
					.ticks(5);

				y1AxisGen = d3.svg.axis()
					.scale(y1Scale)
					.orient("right")
					.ticks(5);

				lineFun = d3.svg.line()
					.x(function (d) {
						return xScale(d.id);
					})
					.y(function (d) {
						return yScale(d.sales);
					})
					.interpolate("basis");

				lineFun2 = d3.svg.line()
					.x(function (d) {
						return xScale(d.id);
					})
					.y(function (d) {
						return yScale(d.count);
					})
					.interpolate("basis");
			}

			function drawLineChart() {

				setChartParameters();

				// Add the X Axis
				svg.append("svg:g")
					.attr("class", "x axis")
					.attr("transform", "translate(0,180)")
					.call(xAxisGen)
					.selectAll("text")
						.style("text-anchor", "end")
						.attr("dx", "-.8em")
						.attr("dy", ".15em")
						.attr("transform", function (d) {
							return "rotate(-90)"
						});


				svg.append("svg:g")
					.attr("class", "y axis")
					.attr("transform", "translate(20,0)")
					.call(yAxisGen);

				svg.append("svg:g")
				   .attr("class", "y axis")
				   .attr("transform", "translate(835 ,0)")				  
				   .call(y1AxisGen);

				svg.append("svg:path")
					.attr({
						d: lineFun(data),
						"stroke": "orange",
						"stroke-width": 2,
						"fill": "none",
						"class": pathClass
					});

				svg.append("svg:path")
					.attr({
						d: lineFun2(data),
						"stroke": "red",
						"stroke-width": 2,
						"fill": "none",
						"class": pathClass
					});

				//Tooltip

				svg.selectAll("dot")
				 .data(data)
				 .enter().append("circle")
				 .attr({ "fill": "#FFCC00" })
				 .attr("r", 7)
				 .attr("cx", function (d) { return xScale(d.id); })
                 .attr("cy", function (d) { return yScale(d.sales); })
                 .on("mouseover", function (d) {
        	tooltip.transition()
                .duration(200)
                .style("opacity", .9);
                 	tooltip.html(moment(d.hour, 'yyyy-MM-ddTHH:mm:ssZ').format('HH:mm:ss') + "<br/>" + d.sales)
                .style("left", (d3.event.pageX) + "px")
                .style("top", (d3.event.pageY - 28) + "px");
        })
        .on("mouseout", function (d) {
        	tooltip.transition()
                .duration(500)
                .style("opacity", 0);
        });

				svg.selectAll("dot")
				 .data(data)
				 .enter().append("circle")
				 .attr("r", 7)
				 .attr({ "fill": "#CC0000" })
				 .attr("cx", function (d) { return xScale(d.id); })
                 .attr("cy", function (d) { return yScale(d.count); })
                 .on("mouseover", function (d) {
                 	tooltip.transition()
						.duration(200)
						.style("opacity", .9);
                 	tooltip.html(moment(d.hour, 'yyyy-MM-ddTHH:mm:ssZ').format('HH:mm:ss') + "<br/>" + d.count)
                .style("left", (d3.event.pageX) + "px")
                .style("top", (d3.event.pageY - 28) + "px");
                 })
        .on("mouseout", function (d) {
        	tooltip.transition()
                .duration(500)
                .style("opacity", 0);
        });
							
			}

			drawLineChart();
		}
	};
});