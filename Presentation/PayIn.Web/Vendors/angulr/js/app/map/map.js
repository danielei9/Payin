/* global console:false, google:false */
/*jshint unused:false */
'use strict';
app.directive('xpMap', function () {
	return {
		restrict: 'EA',
		priority: 100,
		scope: true,
		link: function ($scope, $element, $attrs)
		{
			var latitude = $attrs.xpLatitude;
			var longitude = $attrs.xpLongitude;
			var track = $attrs.xpTrack;
			var mode = $attrs.xpMode || 'direct';
			var polyline = null;
			var elevations = null;

			var SAMPLES = 256;
			var travelMode = null;
			var count = 0;
			
			google.maps.event.addListener($scope.myMap, 'idle', function (e) {
				if ($scope.myMarkers[0]) {
					var lat = $scope.myMarkers[0].position.A || $scope.myMarkers[0].position.G;
					var lng = $scope.myMarkers[0].position.F || $scope.myMarkers[0].position.K;
				}
				else {
					var lat = 39.4707265;
					var lng = -0.3763669;
				}
				
				if (count == 0) {
					$scope.myMap.setCenter(new google.maps.LatLng(lat, lng), 5);
					$scope.myMap.setZoom(13);
					count++;
				}
			});

			var directionsService = new google.maps.DirectionsService();
			var elevationService = new google.maps.ElevationService();
			var geocoderService = new google.maps.Geocoder();			

			var paintMarker = function (position) {
				if ($scope.myMarkers.length)
					$scope.deleteMarkers();
				$scope.myMarkers.push(new google.maps.Marker({
					map: $scope.myMap,
					draggable: true,
					position: position
				}));
				google.maps.event.addListener($scope.myMarkers[0], "drag", function () {
					eval('$scope.' + latitude + '=$scope.myMarkers[0].getPosition().k');
					eval('$scope.' + longitude + '=$scope.myMarkers[0].getPosition().D');					
				});					
				google.maps.event.addListener($scope.myMarkers[0], "dragend", function () {
					eval('$scope.' + latitude + '=($scope.myMarkers[0].getPosition().lat()).toFixed(6)');
					eval('$scope.' + longitude + '=($scope.myMarkers[0].getPosition().lng()).toFixed(6)');
				});	
				
			};
			var addMarkerTrack = function addMarkerTrack(position) {				

				$scope.myMarkers.push(new google.maps.Marker({
					map: $scope.myMap,
					draggable: false,					
					icon: '/Images/marker.png',
					position: position
				}));
			};

			var markerInitial = new google.maps.MarkerImage(
				"/Images/markerInit.png",
				null, /* size is determined at runtime */
				null, /* origin is 0,0 */
				null, /* anchor is bottom center of the scaled image */
				new google.maps.Size(20, 20)
			);

			var markerFinish = new google.maps.MarkerImage(
				"/Images/markerEnd.png",
				null, /* size is determined at runtime */
				null, /* origin is 0,0 */
				null, /* anchor is bottom center of the scaled image */
				new google.maps.Size(20,20)
			);
						
			var addMarkerTrackIni = function addMarkerTrackIni(position) {

				$scope.myMarkers.push(new google.maps.Marker({
					map: $scope.myMap,
					draggable: false,
					icon: markerInitial,
					position: position
				}));
			};

			var addMarkerTrackEnd = function addMarkerTrackEnd(position) {

				$scope.myMarkers.push(new google.maps.Marker({
					map: $scope.myMap,
					draggable: false,
					icon: markerFinish,
					position: position
				}));
			};
			var calcRoute = function (travelMode) {
				var origin = $scope.myMarkers[0].getPosition();
				var destination = $scope.myMarkers[$scope.myMarkers.length - 1].getPosition();

				var waypoints = [];
				for (var i = 1; i < $scope.myMarkers.length - 1; i++) {
					waypoints.push({
						location: $scope.myMarkers[i].getPosition(),
						stopover: true
					});
				}

				var request = {
					origin: origin,
					destination: destination,
					waypoints: waypoints
				};

				switch (travelMode) {
					case "driving":
						request.travelMode = google.maps.DirectionsTravelMode.DRIVING;
						break;
					case "walking":
						request.travelMode = google.maps.DirectionsTravelMode.WALKING;
						break;
				}

				directionsService.route(request, function (response, status) {
					if (status == google.maps.DirectionsStatus.OK) {
						elevationService.getElevationAlongPath({
							path: response.routes[0].overview_path,
							samples: SAMPLES
						}, plotElevation);
					}
				});
			};

			var plotElevation=function (results) {
				elevations = results;

				var path = [];
				if (results) {
					for (var i = 0; i < results.length; i++) {
						path.push(elevations[i].location);
					}
				}

				if (polyline) {
					polyline.setMap(null);
				}

				polyline = new google.maps.Polyline({
					path: path,
					strokeColor: "#E9AF30",
					map: $scope.myMap
				});

				document.getElementById("distance").innerHTML = (polyline.Distance() / 1000).toFixed(2) + " km";
				//var data = new google.visualization.DataTable();
				//data.addColumn('string', 'Sample');
				//data.addColumn('number', 'Elevation');
				//for (var i = 0; i < results.length; i++) {
				//	data.addRow(['', elevations[i].elevation]);
				//}

				/*document.getElementById('chart_div').style.display = 'block';
				chart.draw(data, {
					width: 512,
					height: 200,
					legend: 'none',
					titleY: 'Elevation (m)',
					focusBorderColor: '#00ff00'
				});*/
			};


			var updateElevation = function () {
				if ($scope.myMarkers.length > 1) {
					var travelMode = eval('$scope.' + mode) || 'direct';
					
					if (travelMode != 'direct') {
						calcRoute(travelMode);
					} else {
						var latlngs = [];
						for (var i in $scope.myMarkers) {
							latlngs.push($scope.myMarkers[i].getPosition())
						}
						elevationService.getElevationAlongPath({
							path: latlngs,
							samples: SAMPLES
						}, plotElevation);
					}
				}
			};

			var reset = function () {
				if (polyline) {
					polyline.setMap(null);
				}

				for (var i in $scope.myMarkers) {
					$scope.myMarkers[i].setMap(null);
				}

				$scope.myMarkers = [];

				document.getElementById('chart_div').style.display = 'none';
			};

			var paintTrack = function () {
				if (polyline) {					
					polyline.setMap(null);

				}
					
				var list = eval('$scope.myMarkers');
				$scope.myMap.setMapTypeId('roadmap');
				document.getElementById('mode').value = eval('$scope.' + mode) || 'direct';
				var bounds = new google.maps.LatLngBounds();

			
				angular.forEach(list, function (trackObj) {
					angular.forEach(trackObj.items, function (itemObj) {
						
						var lat = itemObj.latitude;
						var lon = itemObj.longitude;

						var latlng = new google.maps.LatLng(lat, lon);
						addMarkerTrack(latlng);						
						bounds.extend(latlng);
					});
				});

				$scope.myMap.fitBounds(bounds);
				var center = new google.maps.LatLng($scope.myMarkers[0].position.k, $scope.myMarkers[0].position.D);
				$scope.myMap.setCenter(center);
				$scope.myMap.setZoom(13);
				updateElevation();
			}

			$scope.addMarker = function ($event, $params) {
				eval('$scope.' + latitude + '=$params[0].latLng.k');
				eval('$scope.' + longitude + '=$params[0].latLng.D');
			};
			
			$scope.$watch(latitude, function () {
				var lat = eval('$scope.' + latitude);
				var lon = eval('$scope.' + longitude);

				if (lat || lon) {
					paintMarker(new google.maps.LatLng(lat, lon));
				}
				
			});

			$scope.$watch(mode, function () {
				if (polyline) {
					polyline.setMap(null);
				}

				for (var i in $scope.myMarkers) {
					$scope.myMarkers[i].setMap(null);
				}

				$scope.myMarkers = [];
				
				var list = $scope.$eval(track);
				//travelMode = document.getElementById("mode").value;
				travelMode = 'direct';

				$scope.myMap.setMapTypeId('roadmap');
				var bounds = new google.maps.LatLngBounds();
				var i = 0;

				angular.forEach(list, function (trackObj) {					
					angular.forEach(trackObj.items, function (itemObj) {
						
						var lat = itemObj.latitude;
						var lon = itemObj.longitude;

						var latlng = new google.maps.LatLng(lat, lon);

						  if(i==0) {
							addMarkerTrackIni(latlng);
						} else if (i == trackObj.items.length - 1) {
							addMarkerTrackEnd(latlng);
						} else {
							addMarkerTrack(latlng);
						}
						bounds.extend(latlng);
						i++;

					});
				});

				$scope.myMap.fitBounds(bounds);
				if ($scope.myMarkers.length)
					paintTrack();

			})

			$scope.$watch(longitude, function () {
				var lat = eval('$scope.' + latitude);
				var lon = eval('$scope.' + longitude);

				if (lat || lon) {
					paintMarker(new google.maps.LatLng(lat, lon));
				}
			});
			$scope.$watch(track, function () {

				if (polyline) {
					polyline.setMap(null);
				}

				for (var i in $scope.myMarkers) {
					$scope.myMarkers[i].setMap(null);
				}

				$scope.myMarkers = [];

				var list = $scope.$eval(track);
			  travelMode = eval('$scope.' + mode) || 'direct';

				$scope.myMap.setMapTypeId('roadmap');
				var bounds = new google.maps.LatLngBounds();
				var i = 0;
				angular.forEach(list, function (trackObj) {
					angular.forEach(trackObj.items, function (itemObj) {
						var lat = itemObj.latitude;
						var lon = itemObj.longitude;

						var latlng = new google.maps.LatLng(lat, lon);
						if (i == 0) {
							addMarkerTrackIni(latlng);
						} else if (i == trackObj.items.length - 1) {
							addMarkerTrackEnd(latlng);
						} else {
							addMarkerTrack(latlng);
						}
						bounds.extend(latlng);
						i++;
					});
				});
								
				$scope.myMap.fitBounds(bounds);
				if ($scope.myMarkers.length)
					paintTrack();
			})
		},
		controller: ['$scope', '$attrs', 
			function ($scope, $attrs) {
				var latitude = $attrs.xpLatitude;
				var longitude = $attrs.xpLongitude;
				var geocoder = new google.maps.Geocoder();
				var address;				


				var map = null;
				var chart = null;

				var geocoderService = null;
				var elevationService = null;
				var directionsService = null;

				var mousemarker = null;
				var markers = [];
				var polyline = null;
				var elevations = null;

				var SAMPLES = 256;

				var examples = [{
					// Plaza ayuntamiento, Valencia
					latlngs: [
					  [39.4707265, -0.37636699999995926],
					  [39.46913631492731, -0.37529411639400223],
					  [39.46847372708768, -0.37392082537837723]
					],
					mapType: google.maps.MapTypeId.ROADMAP,
					travelMode: 'driving'
				}];
				
				$scope.myMarkers = [];

				//Start Map
				$scope.options = {
					center: new google.maps.LatLng(39.4707265, -0.3763669),
					zoom: 11,
					mapTypeId: google.maps.MapTypeId.SATELLITE,
					disableDefaultUI: true
				};			

				// Load the Visualization API and the piechart package.
				//google.load("visualization", "1", { packages: ["columnchart"] });
								
				// Trigger the elevation query for point to point
				// or submit a directions request for the path between points
				$scope.updateElevation=function () {
					if (markers.length > 1) {
						var travelMode = document.getElementById("mode").value;
						if (travelMode != 'direct') {
							calcRoute(travelMode);
						} else {
							var latlngs = [];
							for (var i in markers) {
								latlngs.push(markers[i].getPosition())
							}
							elevationService.getElevationAlongPath({
								path: latlngs,
								samples: SAMPLES
							}, plotElevation);
						}
					}
				};
			
				$scope.codeAddress = function codeAddress() {
					if ($scope.myMarkers.length)
						$scope.deleteMarkers();


					 address = document.getElementById('address').value;
					
					geocoder.geocode({ 'address': address }, function (results, status) {
						if (status == google.maps.GeocoderStatus.OK) {
							$scope.myMap.setCenter(results[0].geometry.location);
							$scope.myMarkers.push(new google.maps.Marker({
								map: $scope.myMap,
								position: results[0].geometry.location,
								draggable: true
							}));

							google.maps.event.addListener($scope.myMarkers[0], "drag", function () {
								eval('$scope.' + latitude + '=($scope.myMarkers[0].getPosition().lat()).toFixed(6)');
								eval('$scope.' + longitude + '=($scope.myMarkers[0].getPosition().lng()).toFixed(6)');
							});
							
							eval('$scope.' + latitude + '=($scope.myMarkers[0].getPosition().lat()).toFixed(6)');
							eval('$scope.' + longitude + '=($scope.myMarkers[0].getPosition().lng()).toFixed(6)');

						} else {
							alert('Geocode was not successful for the following reason: ' + status);
						}						
					});
				};


				// Sets the map on all markers in the array.
				function setAllMap(map) {
					for (var i = 0; i < $scope.myMarkers.length; i++) {
						$scope.myMarkers[i].setMap(map);
					}
				}

				// Removes the markers from the map, but keeps them in the array.
				function clearMarkers() {
					setAllMap(null);
				}
				// Deletes all markers in the array by removing references to them.
				$scope.deleteMarkers= function deleteMarkers() {
					clearMarkers();
					$scope.myMarkers = [];
					var markerButton = document.getElementById("marker");
					if (markerButton) {
						var parentMarkerButton = markerButton.parentNode;
						parentMarkerButton.removeChild(markerButton);
					}
				}

				$scope.setZoomMessage = function (zoom) {
					$scope.zoomMessage = 'You just zoomed to ' + zoom + '!';
					console.log(zoom, 'zoomed');
				};
				
				$scope.addMark = function ($event, $params) {
					if ($scope.myMarkers.length)
						$scope.deleteMarkers();
					$scope.myMarkers.push(new google.maps.Marker({
						map: $scope.myMap,
						draggable: true,
						position: $params[0].latLng
					}));

					google.maps.event.addListener($scope.myMarkers[0], "drag", function () {
						eval('$scope.' + latitude + '=($scope.myMarkers[0].getPosition().lat()).toFixed(6)');
						eval('$scope.' + longitude + '=($scope.myMarkers[0].getPosition().lng()).toFixed(6)');
					});					
					eval('$scope.' + latitude + '=($scope.myMarkers[0].getPosition().lat()).toFixed(6)');
					eval('$scope.' + longitude + '=($scope.myMarkers[0].getPosition().lng()).toFixed(6)');					
				};

				$scope.openMarkerInfo = function (marker) {
					$scope.currentMarker = marker;
					$scope.currentMarkerLat = marker.getPosition().lat();
					$scope.currentMarkerLng = marker.getPosition().lng();
					$scope.name = marker.name;
					$scope.myInfoWindow.open($scope.myMap, marker);
				};
						
				$scope.setMarkerPosition = function (marker, lat, lng) {
					marker.setPosition(new google.maps.LatLng(lat, lng));
				};
			}
		]
	};
});

app.directive('uiEvent', ['$parse',function ($parse) {
	return function ($scope, elm, attrs) {
		var events = $scope.$eval(attrs.uiEvent);
		angular.forEach(events, function (uiEvent, eventName) {
			var fn = $parse(uiEvent);
			elm.bind(eventName, function (evt) {
				var params = Array.prototype.slice.call(arguments);
				//Take out first paramater (event object);
				params = params.splice(1);
				fn($scope, {$event: evt, $params: params});
				if (!$scope.$$phase) {
					$scope.$apply();
				}
			});
		});
	};
}]);