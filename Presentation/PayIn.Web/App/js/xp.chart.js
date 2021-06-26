// https://openlayers.org
'use strict';

angular
	.module('xpChart', [])
	.service('xpChart', [
		function () {
			return {
			};
		}
	])
	.directive('xpChart', ["$interpolate", "$parse", function ($interpolate, $parse) {
		function getOptions(json) {
			var options = angular.extend(
				{
					//type: "line", // line, bar
					aspectRatio: 3,
					legend: {
						display: true,
						position: "top"
					}
				},
				JSON.parse(json || "{}")
			);

			return options;
		}
		function getData(json) {
			var data = angular.extend(
				{ },
				JSON.parse(json || "{}")
			);
			return data;
		}
		function getXAxis(json) {
			var xAxis = angular.extend(
				{
					isTime: false,
					isDate: false,
					isDateTime: false,
					unity: "",
					beginAtZero: false,
					displayLabel: true
				},
				JSON.parse(json || "{}")
			);
			return xAxis;
		}
		function getYAxis(json) {
			var yAxis = angular.extend(
				{
					isTime: false,
					isDateTime: false,
					unity: "",
					beginAtZero: false,
					displayLabel: true
				},
				JSON.parse(json || "{}")
			);
			return yAxis;
		}

		function transparentize(color, opacity) {
			var alpha = 1 - opacity || 0;
			return Color(color).alpha(alpha).rgbaString();
		}
		function convertToTime(value) {
			if (value) {
				var result = (moment(value, ["YYYY-MM-DDTmm:ssZ", "YYYY-MM-DD HH:mm:ssZ", moment.ISO_8601]));
				return result.format("HH:mm");
			};
			return value;
		}
		function convertToDate(value) {
			if (value) {
				var result = (moment(value, ["YYYY-MM-DDTmm:ssZ", "YYYY-MM-DD HH:mm:ssZ", moment.ISO_8601]));
				return result.format("YYYY-MM-DD");
			};
		}
		function convertToDateTime(value) {
			if (value) {
				var result = (moment(value, ["YYYY-MM-DDTmm:ssZ", "YYYY-MM-DD HH:mm:ssZ", moment.ISO_8601]));
				return result.format("YYYY-MM-DD HH:mm:ss");
			};
		}

		return {
			restrict: 'E',
			replace: true,
			template: '<canvas style="min-height:150px"></canvas>',
			link: function ($scope, $element, $attrs) {
				var model = $attrs['xpModel'];
				var options = getOptions($attrs['xpOptions']);
				var data = getData($attrs['xpData']);
				var xAxis = getXAxis($attrs['xpXAxis']);
				var yAxis = getYAxis($attrs['xpYAxis']);
				
				// Create datasets
				var dataset = [];
				angular.forEach(data, function (item) {
					dataset.push({
						type: item.type || options.type || "bar",
						label: item.name || '',
						backgroundColor: [],
						borderColor: [],
						data: [],
						fill: item.fill || false,
						steppedLine: item.steppedLine || false,
						pointRadius: 0,
						pointHoverRadius: 0,
						borderWidth: item.borderWidth,
						stack: item.stack || false
					});
				});

				// Create chart
				var context = $element[0].getContext('2d');
				var chartOptions = {
					legend: {
						display: options.legend.display,
						position: options.legend.position
					},
					tooltips: {
						enabled: options.tooltips.enabled
                    },
					aspectRatio: options.aspectRatio,
					scales:
						{
							xAxes: [{
								display: true,
								//barPercentage: 1.0,
								//categoryPercentage: 1.0,
								//scaleLabel: {
								//	display: true,
								//	labelString: 'Timestamp'
								//},
								ticks: {
									callback: function (value, index, values) {
										var result =
											xAxis.isTime ? convertToTime(value) :
											xAxis.isDate ? convertToDate(value) :
											xAxis.isDateTime ? convertToDateTime(value) :
											value;
										return result;

									},
									beginAtZero: xAxis.beginAtZero,
									display: xAxis.displayLabel
								},
								stacked: xAxis.stacked || false,
							}],
							yAxes: [{
								type: 'linear',
								display: true,
								//scaleLabel: {
								//	display: true,
								//	labelString: 'Value'
								//},
								ticks: {
									callback: function (value, index, values) {
										return value + yAxis.unity;
									},
									beginAtZero: yAxis.beginAtZero,
									display: yAxis.displayLabel,
									min: yAxis.min,
									max: yAxis.max
								},
								stacked: yAxis.stacked || false,
							}]
						}
				};
				var myChart =
					new Chart(
						context,
						options.type != "doughnut"  ?
						{
							responsive: true,
							maintainAspectRatio: false,
							//aspectRatio: 3,
							type: options.type,
							data: {
								labels: [],
								datasets: dataset
							},
							options: chartOptions
						}
						:
						{
							responsive: true,
							maintainAspectRatio: false,
							type: 'doughnut',
							data: {
								datasets: [{
									type: options.type,
									data: [],
									backgroundColor: [ ],
									borderColor: [ ]
								}],
								labels: [ ]
							},
							options: {
								responsive: true,
								legend: {
									display: false,
									position: 'top',
								}
							}
						}
					);

				$scope.$watchCollection(model, function (values) {
					// Clear data
					myChart.data.labels.length = 0;
					angular.forEach(myChart.data.datasets, function (item) {
						item.data.length = 0;
						item.backgroundColor.length = 0;
						item.borderColor.length = 0;
					});

					// Create Values
					if (Array.isArray(values)) {
						angular.forEach(values, function (value) {
							var itemXAxisDatas = xAxis.data && xAxis.data.split(",");
							for (var i = 0; i < itemXAxisDatas.length; i++) {
								// xAxisDatas
								var itemXAxisData = itemXAxisDatas.length <= i ? itemXAxisDatas[0] : itemXAxisDatas[i];
								var xAxisData = itemXAxisData ? $parse(itemXAxisData)(value) : '';
								myChart.data.labels.push(xAxisData);
							}
							
							var i;
							for (i = 0; i < dataset.length; i++) {
								var item = data[i];

								var itemValues = item.value && item.value.split(",");
								var itemBackgroundColors = item.backgroundColor && item.backgroundColor.split(",");
								var itemFillTransparencies = (item.fillTransparency && item.fillTransparency.toString().split(",")) || "0";
								var itemBorderColors = item.borderColor && item.borderColor.split(",");
								for (var j = 0; j < itemValues.length; j++) {
									// Value
									var itemValue = itemValues[j];
									var valueValue = $parse(itemValue)(value);
									myChart.data.datasets[i].data.push(valueValue);

									// FillTransparency
									var fillTransparency = 0;
									if (itemFillTransparencies && itemFillTransparencies.length > 0) {
										var itemFillTransparency = itemFillTransparencies.length <= j ? itemFillTransparencies[0] : itemFillTransparencies[j];
										fillTransparency = itemFillTransparency ? $parse(itemFillTransparency)(value) : 0;
									}

									// BackgroundColor
									var backgroundColor = '#FFB835';
									if (itemBackgroundColors && itemBackgroundColors.length > 0) {
										var itemBackgroundColor = itemBackgroundColors.length <= j ? itemBackgroundColors[0] : itemBackgroundColors[j];
										backgroundColor = itemBackgroundColor ?
											(
												$parse(itemBackgroundColor)(value) ||
												$parse(itemBackgroundColor)($scope.arguments)
											) :
											'#FFB835';
									}
									myChart.data.datasets[i].backgroundColor.push(transparentize(backgroundColor, fillTransparency || 0));

									// BorderColor
									var borderColor = '#FFB835';
									if (itemBorderColors && itemBorderColors.length > 0) {
										var itemBorderColor = itemBorderColors.length <= j ? itemBorderColors[0] : itemBorderColors[j];
										borderColor = itemBorderColor ?
											(
												$parse(itemBorderColor)(value) ||
												$parse(itemBorderColor)($scope.arguments)
											) :
											'#FFB835';
									}
									myChart.data.datasets[i].borderColor.push(transparentize(borderColor, 0));
								}
							}
						});
					} else {
						var value = values;
						var itemXAxisDatas = xAxis.data && xAxis.data.split(",");
						for (var i = 0; i < itemXAxisDatas.length; i++) {
							// xAxisDatas
							var itemXAxisData = itemXAxisDatas.length <= i ? itemXAxisDatas[0] : itemXAxisDatas[i];
							var xAxisData = itemXAxisData ? $parse(itemXAxisData)(value) : '';
							myChart.data.labels.push(xAxisData);
						}

						var i;
						for (i = 0; i < dataset.length; i++) {
							var item = data[i];

							var itemValues = item.value && item.value.split(",");
							var itemBackgroundColors = item.backgroundColor && item.backgroundColor.split(",");
							var itemFillTransparencies = (item.fillTransparency && item.fillTransparency.toString().split(",")) || "0";
							var itemBorderColors = item.borderColor && item.borderColor.split(",");
							for (var j = 0; j < itemValues.length; j++) {
								// Value
								var itemValue = itemValues[j];
								var valueValue = $parse(itemValue)(value);
								myChart.data.datasets[i].data.push(valueValue);

								// BackgroundColor
								if (itemBackgroundColors) {
									var itemFillTransparency = itemFillTransparencies.length <= j ? itemFillTransparencies[0] : itemFillTransparencies[j];
									var fillTransparency = itemFillTransparency ? $parse(itemFillTransparency)(value) : 0;

									var itemBackgroundColor = itemBackgroundColors.length <= j ? itemBackgroundColors[0] : itemBackgroundColors[j];
									var backgroundColor = itemBackgroundColor ? $parse(itemBackgroundColor)(value) : '#FFB835';
									myChart.data.datasets[i].backgroundColor.push(transparentize(backgroundColor, fillTransparency || 0));
								}

								// BorderColor
								if (itemBorderColors) {
									var itemBorderColor = itemBorderColors.length <= j ? itemBorderColors[0] : itemBorderColors[j];
									var borderColor = itemBorderColor ? $parse(itemBorderColor)(value) : '#FFB835';
									myChart.data.datasets[i].borderColor.push(transparentize(borderColor, 0));
								}
							}
						}
					}

					// Update canvas
					myChart.update();
				});
			}
		}
	}]);