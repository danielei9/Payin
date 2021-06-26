// https://openlayers.org
'use strict';

angular
	.module('xpGraph', [])
	.service('xpGraph', [
		function () {
			return {
			};
		}
	])
	.directive('xpGraph', ["$interpolate", "$parse", function ($interpolate, $parse) {
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

		return {
			restrict: 'E',
			replace: true,
			template: '<div style="minheight:1000px"></div>',
			link: function ($scope, $element, $attrs) {
				var model = $attrs['xpModel'];
				var nodes = $attrs['xpNodes'];
				var links = $attrs['xpLinks'];
				
				// create an array with nodes
				//var nodes = new vis.DataSet([
				//	{ id: 1, value: '1', label: 'Node 1' },
				//	{ id: 2, label: 'Node 2' },
				//	{ id: 3, label: 'Node 3' },
				//	{ id: 4, label: 'Node 4' },
				//	{ id: 5, label: 'Node 5' }
				//]);

				// create an array with edges
				//var edges = new vis.DataSet([
				//	{ from: 1, to: 3, value: 3, arrows: 'to' },
				//	{ from: 1, to: 2, value: 5 },
				//	{ from: 2, to: 4, value: 1 },
				//	{ from: 2, to: 5 },
				//	{ from: 3, to: 3 }
				//]);

				// create a network
				var container = $element[0];
				var data = {
					nodes: nodes,
					edges: edges
				};
				var options = {
					layout: {
						hierarchical: {
							direction: "UD"
						}
					},
					nodes: {
						shape: 'dot',
						size: 10
					}
				};
				var network = new vis.Network(container, data, options);

				$scope.$watchCollection(model, function (values) {
					angular.forEach(values, function (value) {
						var valueValue = $parse(itemValue)(value);
					});
				});
			}
		}
	}]);