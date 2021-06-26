'use strict';
app
.directive('xpMap2', function () {
	return {
		restrict: 'EA',
		priority: 100,
		scope: true,
		link: function ($scope, $element, $attrs) {
			var markers = [];
			var polylines = [];
			var mousemarker = null;			
			
			var center = undefined;
			function AddPositionCenter(position) {				
				if (!center) {
					center = {
						minLatitude: position.latitude,
						maxLatitude: position.latitude,
						minLongitude: position.longitude,
						maxLongitude: position.longitude
					}
				} else {
					if (!position.latitude) {
					} else if (position.latitude >= 0) {
						if (center.minLatitude > position.latitude)
							center.minLatitude = position.latitude;
						if (center.maxLatitude < position.latitude)
							center.maxLatitude = position.latitude;
					} else {
						if (center.minLatitude < position.latitude)
							center.minLatitude = position.latitude;
						if (center.maxLatitude > position.latitude)
							center.maxLatitude = position.latitude;
					}

					if (!position.longitude) {
					} else if (position.longitude >= 0) {
						if (center.minLongitude > position.longitude)
							center.minLongitude = position.longitude;
						if (center.maxLongitude < position.longitude)
							center.maxLongitude = position.longitude;
					} else {
						if (center.minLongitude < position.longitude)
							center.minLongitude = position.longitude;
						if (center.maxLongitude > position.longitude)
							center.maxLongitude = position.longitude;
					}
				}
			};

			var addMarker = function (map, position, icon) {				
				var marker = new google.maps.Marker({
					position: new google.maps.LatLng(position.latitude, position.longitude),
					title: position.title,
					icon: icon
				})
				marker.setMap(map);
				markers.push(marker);
				AddPositionCenter(position);
			};
			var addMarkers = function (map, positions, icon) {				
				var polyline = new google.maps.Polyline({
					strokeColor: '#E9AF30',
					strokeOpacity: 1.0,
					strokeWeight: 3
				});
				polyline.setMap(map);
				polylines.push(polyline);				
				var path = polyline.getPath();
				angular.forEach(positions, function (position) {
					path.push(new google.maps.LatLng(position.latitude, position.longitude));
					addMarker(map, position, icon);
				});
			};
			var addPaths = function (map, paths, icon) {
				
				center = undefined;
				for (var i = 0; i < markers.length; i++) {
					markers[i].setMap(null);
				}
				for (var i = 0; i < polylines.length; i++) {
					polylines[i].setMap(null);
				}

				angular.forEach(paths, function (path) {					
					addMarkers(map, path.positions, icon);
					var tam = path.positions.length - 1;

					if (path.positions[0])
							addMarker(map, path.positions[0], new google.maps.MarkerImage(
								'/Images/markerSectionInit.png',
								null, /* size is determined at runtime */
								null, /* origin is 0,0 */
								null, /* anchor is bottom center of the scaled image */
								new google.maps.Size(24, 41)
							));

						if (path.positions[tam])
							addMarker(map, path.positions[tam], new google.maps.MarkerImage(
								'/Images/markerSectionEnd.png',
								null, /* size is determined at runtime */
								null, /* origin is 0,0 */
								null, /* anchor is bottom center of the scaled image */
								new google.maps.Size(24, 41)
							));

						if (path.since)
							addMarker(map, path.since, new google.maps.MarkerImage(
								"/Images/markerInit.png",
								null, /* size is determined at runtime */
								null, /* origin is 0,0 */
								null, /* anchor is bottom center of the scaled image */
								new google.maps.Size(24, 41)
							));
						if (path.until)
							addMarker(map, path.until, new google.maps.MarkerImage(
								'/Images/markerEnd.png',
								null, /* size is determined at runtime */
								null, /* origin is 0,0 */
								null, /* anchor is bottom center of the scaled image */
								new google.maps.Size(24, 41)
							));
						
					
				});
				if (center) {					
					var bounds = new google.maps.LatLngBounds();
					bounds.extend(new google.maps.LatLng(center.maxLatitude || 39.469749, center.maxLongitude || -0.377952));
					bounds.extend(new google.maps.LatLng(center.minLatitude || 39.469749, center.minLongitude || -0.377952));

					map.fitBounds(bounds);
					map.panToBounds(bounds);
					map.setZoom(10);
				}				
			};
			var path = $attrs.xpPaths;

			// Start map
			$scope.$parent.map = new google.maps.Map(
				$element[0],
				{
					zoom: 12,
					center: new google.maps.LatLng(39.469749, -0.377952)
				}
			);

			$scope.$watch(path, function (data) {			  
					addPaths(
						$scope.map,
						data,
						{ // Icon
							path: google.maps.SymbolPath.CIRCLE,
							strokeColor: '#E9AF30',
							strokeWeight: 2,
							fillColor: '#FFB265',
							scale: 3
						}
					);
			});

			$scope.$watch('data', function (data) {
				var tracks = [];
				var trackEnum = [];

				angular.forEach(data, function (item) {
					var positions = [];

					angular.forEach(item.items, function (pos) {
						var position = {
							latitude: pos.latitude,
							longitude: pos.longitude,
							title: moment(pos.date, moment.ISO_8601).format("HH:mm:ss"),
							date: new Date(pos.date),
							velocity: pos.velocity * 3600 / 1000
						};

						positions.push(position);
					});

					tracks.push({
						name: item.since ? moment(item.since.date || item.until.date).format("HH:mm:ss") : "",
						since: item.since,
						until: item.until,
						positions: positions
					});
				});

				$scope.temp.tracks = tracks;

			});
		
		}
	};
})
.directive('xpMapGraphics', function () {
	var loaded = 0;
	return {
		restrict: 'EA',
		controller: ['$scope', '$attrs', function ($scope, $attrs) {			
			function addGraphics(data) {				
				var cols = [{ "label": "Date", "type": "datetime" }];
				var rows = [];
				var index = 0;				

				angular.forEach(data, function (list) {
					{
						var value = {};
						value.type = "number";
						value.label = list.name;
						cols.push(value);
					}

					angular.forEach(list.positions, function (item) {
						var values = [];
						{
							var value = {};
							value.v = item.date;
							values.push(value);
						}
						for (var i = 0; i < index; i++) {
							var value = {};
							//value.v = 0;
							values.push(value);
						}
						{
							var value = {};
							value.v = item.velocity;
							values.push(value);
						}
						for (var i = index + 1; i < data.length; i++) {
							var value = {};
							//value.v = 0;
							values.push(value);
						}

						{
							var value = {};
							value.c = values;
							rows.push(value);
						}

					});

					index++;


				});

				var data = {
					"cols": cols,
					"rows": rows
				};

				$scope.chart = {
					"type": "AreaChart",
					"displayed": true,
					"cssStyle": "height:200px; width:100%;",
					"formatters": {},
					"options": {
						"title": "Velocity",
						"fill": 20,
						"displayExactValues": true,
						"vAxis": {
							"title": "Velocity km/h",
							"gridlines": {
								"count": 6
							}
						},
						"hAxis": {
							"title": "Date"
						}
					},
					"data": data
				};
			};

			var path = $attrs.xpPaths;
			$scope.$watch(path, function (data) {				
					addGraphics(data);					
				

			});
		}]
	};
});
