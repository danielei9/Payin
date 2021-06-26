app
	.directive('xpGraphBar', function ($window) {
		return {
			restrict: 'EA',
			scope: true,
			priority: 100,
			template: "<svg width='100%' height='500'></svg>",
			link: function ($scope, $element, $attrs) {

				var padding = 20;
				var pathClass = "path";
				var xScale, yScale, y1Scale, xAxisGen, yAxisGen, y1AxisGen;
				var d3 = $window.d3;
				var rawSvg = $element.find('svg');
				var svg = d3.select(rawSvg[0]);
				var tooltip = d3.select("body").append("div")
							.attr("class", "tooltipD3")
							.style("opacity", 0);



				$scope.$watch('data', function () {
					//Remove chart
					svg.selectAll("svg").remove();
					svg.selectAll("g").remove();
					svg.selectAll("rect").remove();

					var data = $scope.data;
					$scope.totalHoursWorked = 0;
					$scope.totalHoursPlanned = 0;
					var hoursPlanned = 0;
					var minutesPlanned = 0;

					var hoursWorked = svg.append("g")
						.attr("class", "hoursWorkedD3");

					var legend = svg.append("g")
						.attr("class", "legendD3")
						.attr("x", 900 - 65)
						.attr("y", 25)
						.attr("height", 100)
						.attr("width", 100);

					if (!data.length)
						return;


					function setChartParameters() {
						xScale = d3.scale.ordinal()
							.rangeRoundBands([25, 900 - 20], 0.1)
							.domain(data.map(function (d) {
								return d.day;
							}));

						yScale = d3.scale.linear().range([420 - 20, 20]).domain([0,
						  d3.max(data, function (d) {
						  	if (d.type == 1)
						  		return d.duration;
						  })]);

						y1Scale = d3.scale.linear().range([420 - 20, 20]).domain([0,
							  d3.max(data, function (d) {
							  	if (d.type == 2)
							  		return d.duration;
							  })]);

						xAxisGen = d3.svg.axis()
						  .scale(xScale)
						  .tickSize(1)
						  .tickSubdivide(true);

						yAxisGen = d3.svg.axis()
							.scale(yScale)
							.orient("left")
							.tickSize(5);

						y1AxisGen = d3.svg.axis()
							.scale(y1Scale)
							.orient("right")
							.tickSize(5);

					}

					function drawLineChart() {

						setChartParameters();

						// Add the X Axis
						svg.append("svg:g")
							.attr("class", "x axis")
							.attr("transform", "translate(0,400)")
							.call(xAxisGen)
							.selectAll("text")
								.style("text-anchor", "end")
								.attr("dx", "-0.5em")
								.attr("dy", "-1.3em")
								.attr("transform", function (d) {
									return "rotate(-90)"
								});


						svg.append("svg:g")
							.attr("class", "y axis")
							.attr("transform", "translate(20,0)")
							.call(yAxisGen);

						svg.append("svg:g")
						   .attr("class", "y axis")
						   .attr("transform", "translate(885 ,0)")
						   .call(y1AxisGen);

						svg.selectAll('rect')
							.data(data)
							.enter()
							.append('rect')
							.attr('x', function (d) {
								if (d.type == 1)
									return xScale(d.day);
							})
							.attr('y', function (d) {
								if (d.type == 1) {
									$scope.totalHoursPlanned += d.duration;
									return yScale(d.duration);
								}
							})
							.attr('width', xScale.rangeBand() / 2)
							.attr('height', function (d) {
								if (d.type == 1)
									return (400 - yScale(d.duration));
							})
							.attr('fill', '#FFCC00')
							 .on("mouseover", function (d) {
							 	var hours = Math.floor(d.duration);
							 	var minutes = Math.trunc((d.duration - hours) * 60);
							 	if (minutes < 10)
							 		minutes = "0" + minutes;

							 	tooltip.transition()
									.duration(200)
									.style("opacity", .9);
							 	tooltip.html("Date: " + d.day + "<br/> Duration: " + hours + ":" + minutes)
								.style("left", (d3.event.pageX - 10) + "px")
								.style("top", (d3.event.pageY - 60) + "px");
							 })
							.on("mouseout", function (d) {
								tooltip.transition()
									.duration(500)
									.style("opacity", 0);
							});

						svg.append("g").selectAll("g")
						  .data(data)
							.enter()
							.append('rect')
							.attr('x', function (d) {
								if (d.type == 2) {
									$scope.totalHoursWorked += d.duration;
									return xScale(d.day);
								}
							})
							.attr('y', function (d) {
								if (d.type == 2)
									return yScale(d.duration);
							})
							.attr('width', xScale.rangeBand() / 2)
							.attr('height', function (d) {
								if (d.type == 2)
									return (400 - yScale(d.duration));
							})
						.attr("fill", "#0957FF").on("mouseover", function (d) {
							var hours = Math.floor(d.duration);
							var minutes = Math.trunc((d.duration - hours) * 60);
							if (minutes < 10)
								minutes = "0" + minutes;
							tooltip.transition()
								.duration(200)
								.style("opacity", .9);
							tooltip.html("Date: " + d.day + "<br/> Duration: " + hours + ":" + minutes)
						.style("left", (d3.event.pageX - 10) + "px")
							.style("top", (d3.event.pageY - 60) + "px");
						})
							.on("mouseout", function (d) {
								tooltip.transition()
									.duration(500)
									.style("opacity", 0);
							});
						var hoursWork = Math.floor($scope.totalHoursWorked);
						var minutesWork = Math.trunc(($scope.totalHoursWorked - hoursWork) * 60);
						if (minutesWork < 10)
							minutesWork = "0" + minutesWork;

						hoursPlanned = Math.floor($scope.totalHoursPlanned);
						minutesPlanned = Math.trunc(($scope.totalHoursPlanned - hoursPlanned) * 60);
						if (minutesPlanned < 10)
							minutesPlanned = "0" + minutesPlanned;

						var hoursDifference = Math.floor($scope.totalHoursPlanned) - Math.floor($scope.totalHoursWorked);
						var minutesDifference = Math.trunc(($scope.totalHoursPlanned - $scope.totalHoursWorked - hoursDifference) * 60);
						if (minutesDifference < 10)
							minutesDifference = "0" + minutesDifference;

						hoursWorked.append("text")
						  .attr("x", 980 - 65)
						  .attr("y", 235)
						  .text("Horas trabajadas:" + hoursWork + ":" + minutesWork)
						  .append("br");

						hoursWorked.append("text")
						.attr("x", 980 - 65)
						.attr("y", 250)
						.text(" Horas planificadas: " + hoursPlanned + ":" + minutesPlanned)
						.append("br");

						hoursWorked.append("text")
						.attr("x", 980 - 65)
						.attr("y", 265)
						.text(hoursDifference + ":" + minutesDifference + "h. de diferencia");

						legend.append("rect")
						  .attr("x", 980 - 65)
						  .attr("y", 25)
						  .attr("width", 10)
						  .attr("height", 10)
						  .style("fill", "#FFCC00");

						legend.append("text")
						  .attr("x", 980 - 45)
						  .attr("y", 35)
						  .text("H. planificadas");

						legend.append("rect")
						  .attr("x", 980 - 65)
						  .attr("y", 40)
						  .attr("width", 10)
						  .attr("height", 10)
						  .style("fill", "#0957FF");

						legend.append("text")
						  .attr("x", 980 - 45)
						  .attr("y", 50)
						  .text("H. trabajadas");

					}

					drawLineChart();


				});
			}
		};
	})
	.directive('xpGraph', function ($window) {
		return {
			restrict: 'E',
			template: "<svg width='100%' height='500'></svg>",
			replace: true,
			link: function ($scope, $element, $attrs) {
				var xScale, yScale, xAxisGen, yAxisGen, lineFun;
				var padding = 20;
				var pathClass = "path";

				var xpModel = $attrs.xpModel;
				var xAxis = $attrs.xpXAxis;
				var yData = $attrs.xpYAxis.split(',');
				var color = ["#0957FF", "#FFCC00"]; //blue, orange

				var d3 = $window.d3;
				var svg = d3.select($element[0]);
				var tooltip = d3
					.select("body")
					.append("div")
					.attr("class", "tooltipD3")
					.style("opacity", 0);


				$scope.$watch(xpModel, function (model) {
					//Si no hay datos eliminamos el contenido de la gráfica
					if (!model.length) {
						svg.selectAll("text").remove();
						svg.selectAll("#line").remove();
						svg.selectAll("circle").remove();
						return;
					}

					//Reiniciamos la gráfica antes de pintar la nueva
					svg.selectAll("svg").remove();
					svg.selectAll("g").remove();
					svg.selectAll("path").remove();
					svg.selectAll("circle").remove();

					var xFirst = $scope.$eval(xpModel + "[0]." + xAxis);
					var xLast = $scope.$eval(xpModel + "[" + model.length + " - 1]." + xAxis);
					var count = 0;

					xScale = d3.scale.ordinal()
							.rangePoints([padding + 40, $element[0].scrollWidth - 35], 0.1)
							.domain(model.map(function (x) {
								return eval("x." + xAxis);
							}));

					xAxisGen = d3.svg.axis()
						.scale(xScale)
						.tickSize(1)
										.orient("bottom");


					for (i = 0; i < yData.length; i++) {

						yScale = d3.scale.linear()
						.range([$element[0].scrollHeight - 100, 50]).domain([0,
							d3.max(model, function (y) {
								return eval("y." + yData[i]);
							})]);

						yAxisGen = d3.svg.axis()
							.scale(yScale)
							.orient("left")
							.tickSize(5);

						lineFun = d3.svg.line()
							.x(function (x) {
								return xScale(eval("x." + xAxis));
							})
							.y(function (y) {
								return yScale(eval("y." + yData[i]));
							})
							.interpolate("monotone");

						svg.append("svg:path")
							.attr({
								d: lineFun(model),
								"stroke": color[i],
								"stroke-width": 2,
								"fill": "none",
								"id": "line",
								"class": pathClass
							});

						//Tooltip
						svg.selectAll("dot")
						 .data(model)
						 .enter().append("circle")
						 .attr({ "fill": color[1] })
						 .attr("r", 4)
						 .attr("cx", function (x) { return xScale(eval("x." + xAxis)); })
						 .attr("cy", function (y) { return yScale(eval("y." + yData[i])); })
						 .on("mouseover", function (data) {
						 	tooltip.transition()
								.duration(200)
								.style("opacity", .9);
						 	tooltip.html((eval("data." + xAxis)) + "<br/>" + (eval("data." + yData[count + 1])))
							.style("left", (d3.event.pageX) + "px")
							.style("top", (d3.event.pageY - 28) + "px");

						 }
							)
					.on("mouseout", function (d) {
						tooltip.transition()
							.duration(500)
							.style("opacity", 0);
					});
						svg.selectAll("dot")
						 .data(model)
						 .enter().append("circle")
						 .attr({ "fill": color[0] })
						 .attr("r", 4)
						 .attr("cx", function (x) { return xScale(eval("x." + xAxis)); })
						 .attr("cy", function (y) { return yScale(eval("y." + yData[0])); })
						 .on("mouseover", function (data) {
						 	tooltip.transition()
								.duration(200)
								.style("opacity", .9);
						 	tooltip.html((eval("data." + xAxis)) + "<br/>" + (eval("data." + yData[count])))
							.style("left", (d3.event.pageX) + "px")
							.style("top", (d3.event.pageY - 28) + "px");

						 }
							)
					.on("mouseout", function (d) {
						tooltip.transition()
							.duration(500)
							.style("opacity", 0);
					});
					}

					svg.append("svg:g")
							.attr("class", "x axis")
							.attr("transform", "translate(0," + [$element[0].scrollHeight - 100] + ")")
							.call(xAxisGen)
							.selectAll("text")
								.style("text-anchor", "end")
								.attr("dx", "-0.8em")
								.attr("dy", "0px")
								.attr("transform", function (d) {
									return "rotate(-90)"
								});

					svg.append("svg:g")
							.attr("class", "y axis")
							.attr("transform", "translate(60,0)")
							.call(yAxisGen);
				});
			}
		};
	})
	.directive('xpGraph2', ['$window','$filter',function ($window, $filter) {
		return {
			restrict: 'E',
			template: "<svg width='100%' height='450'></svg>",
			replace: true,
			link: function ($scope, $element, $attrs) {
				var xScale, yScale, xAxisGen, yAxisGen, lineFun;
				var padding = 20;
				var pathClass = "path";

				var xpModel = $attrs.xpModel;
				var xAxis = angular.fromJson($attrs.xpXAxis);
				var yAxis = angular.fromJson($attrs.xpYAxis);
				var color=[];

				(yAxis.length > 1) ? color = ["#228B22", "#FFCC00", "#FF0000"] /*green, orange, red*/ : color = ["#0957FF"]; //blue


				var d3 = $window.d3;
				var svg = d3.select($element[0]);
				var tooltip = d3
					.select("body")
					.append("div")
					.attr("class", "tooltipD3")
					.style("opacity", 0);
				var option = $attrs.ngOptions;
				

				$scope.$watch(xpModel, function (model) {
					//Si no hay datos eliminamos el contenido de la gráfica
					if (!model || !model.length) {
						svg.selectAll("text").remove();
						svg.selectAll("#line").remove();
						svg.selectAll("circle").remove();
						return;
					}

					//Reiniciamos la gráfica antes de pintar la nueva
					svg.selectAll("svg").remove();
					svg.selectAll("g").remove();
					svg.selectAll("path").remove();
					svg.selectAll("circle").remove();

					var xFirst = $scope.$eval(xpModel + "[0]." + xAxis.data);
					var xLast = $scope.$eval(xpModel + "[" + model.length + " - 1]." + xAxis.data);
					
					var maximo = 0;
					var aux = 0;
					var tamanyoX = model.length;				

					for (j = 0; j < yAxis.length; j++) {
						model.map(function (y) {
							aux = eval("y." + yAxis[j].data);
							(aux > maximo) ? maximo = aux : maximo = maximo; 					
						})
						};		
				
						xScale = d3.scale.ordinal()
								.rangePoints([padding + 40, $element[0].scrollWidth - 35], 0.1)
								.domain(model.map(function (x) {
									return eval("x." + xAxis.data);
								}));


						xAxisGen = d3.svg.axis()
							.scale(xScale)
							.tickSize(1)
							.orient("bottom");
					
						
					for (i = 0; i < yAxis.length; i++) {
						yScale = d3.scale.linear()
						.range([$element[0].scrollHeight - 100, 50]).domain([0, maximo+10]);

						yAxisGen = d3.svg.axis()
							.scale(yScale)
							.orient("left")
							.tickSize(5);

						lineFun = d3.svg.line()
							.x(function (x) {
								return xScale(eval("x." + xAxis.data));
							})
							.y(function (y) {
								return yScale(eval("y." + yAxis[i].data));
							})
							.interpolate("monotone");

						svg.append("svg:path")
							.attr({
								d: lineFun(model),
								"stroke": color[i],
								"stroke-width": 2,
								"fill": "none",
								"id": "line",
								"class": pathClass
							});
					}

					if (option == "hour") {
						//Tooltip hour
						svg.selectAll("dot")
						 .data(model)
						 .enter().append("circle")
						 .attr({ "fill": color[0] })
						 .attr("r", 4)
							 .attr("cx", function (x) { return xScale(eval("x." + xAxis.data))+2; })
							 .attr("cy", function (y) { return yScale(eval("y." + yAxis[0].data)); })
							 .on("mouseover", function (d) {
							 	var hours = Math.floor(eval("d." + yAxis[0].data));
							 	var minutes = Math.trunc((eval("d." + yAxis[0].data) - hours) * 60);
						 	if (minutes < 10)
						 		minutes = "0" + minutes;
						 	tooltip.transition()
								.duration(200)
								.style("opacity", .9);
						 	tooltip.html((eval("d." + xAxis.data)) + "<br/>" + hours + ":" + minutes)
							.style("left", (d3.event.pageX) + "px")
							.style("top", (d3.event.pageY - 28) + "px");

						 })
						.on("mouseout", function (d) {
							tooltip.transition()
								.duration(500)
								.style("opacity", 0);
						});

						if (yAxis.length == 2) {

							//Tooltip hour
						svg.selectAll("dot")
						 .data(model)
						 .enter().append("circle")
						 .attr({ "fill": color[1] })
						 .attr("r", 4)
						 .attr("cx", function (x) { return xScale(eval("x." + xAxis.data)); })
						 .attr("cy", function (y) { return yScale(eval("y." + yAxis[1].data)); })
							 .on("mouseover", function (d) {
							 	var hours = Math.floor(eval("d." + yAxis[1].data));
							 	var minutes = Math.trunc((eval("d." + yAxis[1].data) - hours) * 60);
						 	if (minutes < 10)
						 		minutes = "0" + minutes;
						 	tooltip.transition()
								.duration(200)
								.style("opacity", .9);
							 	tooltip.html((eval("d." + xAxis.data)) + "<br/>" + hours + ":" + minutes)
							.style("left", (d3.event.pageX) + "px")
							.style("top", (d3.event.pageY - 28) + "px");

						 })
						.on("mouseout", function (d) {
							tooltip.transition()
								.duration(500)
								.style("opacity", 0);
						});
					}



					}
					else {
						//Tooltip
						svg.selectAll("dot")
						 .data(model)
						 .enter().append("circle")
						 .attr({ "fill": color[0] })
						 .attr("r", 2)
						 .attr("cx", function (x) { return xScale(eval("x." + xAxis.data))+2; })
						 .attr("cy", function (y) { return yScale(eval("y." + yAxis[0].data)); })
						 .on("mouseover", function (d) {
						 	tooltip.transition()
								.duration(200)
								.style("opacity", .9);
						 	tooltip.html((eval("d." + xAxis.data)) + "<br/>" + eval("d." + yAxis[0].data).toFixed(2))
							.style("left", (d3.event.pageX) + "px")
							.style("top", (d3.event.pageY - 28) + "px");

						 })
						.on("mouseout", function (d) {
							tooltip.transition()
								.duration(500)
								.style("opacity", 0);

						});

						if (yAxis.length > 1 ) {

							//Tooltip
						svg.selectAll("dot")
						 .data(model)
						 .enter().append("circle")
						 .attr({ "fill": color[1] })
						 .attr("r", 2)
						 .attr("cx", function (x) { return xScale(eval("x." + xAxis.data)); })
						 .attr("cy", function (y) { return yScale(eval("y." + yAxis[1].data)); })
							 .on("mouseover", function (d) {
						 	tooltip.transition()
								.duration(200)
								.style("opacity", .9);
						 	tooltip.html((eval("d." + xAxis.data)) + "<br/>" + eval("d." + yAxis[1].data).toFixed(2))
							.style("left", (d3.event.pageX) + "px")
							.style("top", (d3.event.pageY - 28) + "px");

						 })
						.on("mouseout", function (d) {
							tooltip.transition()
								.duration(500)
								.style("opacity", 0);

						});

						}
						if (yAxis.length > 2) {

							//Tooltip
							svg.selectAll("dot")
							 .data(model)
							 .enter().append("circle")
							 .attr({ "fill": color[2] })
							 .attr("r", 2)
							 .attr("cx", function (x) { return xScale(eval("x." + xAxis.data))-2; })
							 .attr("cy", function (y) { return yScale(eval("y." + yAxis[2].data)); })
								 .on("mouseover", function (d) {
								 	tooltip.transition()
										.duration(200)
										.style("opacity", .9);
								 	tooltip.html((eval("d." + xAxis.data)) + "<br/>" + eval("d." + yAxis[2].data).toFixed(2))
								.style("left", (d3.event.pageX) + "px")
								.style("top", (d3.event.pageY - 28) + "px");

								 })
							.on("mouseout", function (d) {
								tooltip.transition()
									.duration(500)
									.style("opacity", 0);

							});

						}

					}

					if ((tamanyoX >= 100) && (tamanyoX < 1000)) {
						var contador = -1;
						svg.append("svg:g")
								.attr("class", "x axis")
								.attr("transform", "translate(0," + [$element[0].scrollHeight - 100] + ")")
								.call(xAxisGen)
								.selectAll("text")
									.style("text-anchor", "end")
							        .style("display", function (d) {
							         	contador++;
							         	if ((contador % 15 == 0) || (contador == 0) || (contador == tamanyoX)) return "inherit"
							         	else return "none"
							         })
									.attr("dx", "-0.8em")
									.attr("dy", "0px")
									.attr("transform", function (d) {									
											return "rotate(-90)"									
									});
					} else if (tamanyoX >= 1000) {
						var contador = -1;
						svg.append("svg:g")
								.attr("class", "x axis")
								.attr("transform", "translate(0," + [$element[0].scrollHeight - 100] + ")")
								.call(xAxisGen)
								.selectAll("text")
									.style("text-anchor", "end")
							        .style("display", function (d) {
							        	contador++;
							        	if ((contador % 100 == 0) || (contador == 0) || (contador == tamanyoX)) return "inherit"
							        	else return "none"
							        })
									.attr("dx", "-0.8em")
									.attr("dy", "0px")
									.attr("transform", function (d) {
										return "rotate(-90)"
									});

					}else {
						svg.append("svg:g")
								.attr("class", "x axis")
								.attr("transform", "translate(0," + [$element[0].scrollHeight - 100] + ")")
								.call(xAxisGen)
								.selectAll("text")
									.style("text-anchor", "end")
							       .style("display", function (d) {
									return "inherit"
							       })
									.attr("dx", "-0.8em")
									.attr("dy", "0px")
									.attr("transform", function (d) {
										return "rotate(-90)"
									});

					}


						svg.append("svg:g")
								.attr("class", "y axis")
								.attr("transform", "translate(60,0)")
								.call(yAxisGen);
					
				});
			}
		};
	}]);

