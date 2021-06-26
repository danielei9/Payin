// https://openlayers.org
'use strict';

angular
	.module('xpMap', [])
    .service('xpMap', ['$q', '$rootScope', // Line changed
        function ($q, $rootScope) { // Line changed
			var onClickedEvent;

            function getCurrentLocation() { // Added
                var deferred = $q.defer();

                if (navigator.geolocation) {
                    navigator.geolocation.watchPosition(
                        function (position) {
                            deferred.resolve(position.coords);
                        },
                        function (error) {
                            deferred.reject(error);
                        }, {
                            timeout: 10000 // Options: throw an error if no update is received every 10 seconds.
                        }
                    );
                }
                return deferred.promise;
            }

			function createView(mainLocation, zoom) {
				var view = new ol.View({
                    center: ol.proj.fromLonLat([
                        Number(mainLocation.longitude),
                        Number(mainLocation.latitude)
                    ]),
                    //center: ol.proj.fromLonLat([-0.173485, 38.536317]),
                    zoom: Number(zoom)
				});
				return view;
			}
			function createMap(view) {
				var layer = createTileLayer();
				var map = new ol.Map({
                    //interactions: ol.interaction.defaults({ mouseWheelZoom: false }),
                    target: 'map',
					layers: [layer],
					view: view
                });
                map.on('mousewheel', function (e) {
                    e.browserEvent.preventDefault();
                    var now = new Date();
                    if (lastScrollZoom === null || now > lastScrollZoom) {
                        var zoom_in = e.browserEvent.deltaY < 0;
                        _panAndZoom(e.map, zoom_in, e.coordinate);
                        lastScrollZoom = now.setMilliseconds(now.getMilliseconds() + scrollDelta)
                    }
                });
				return map;
			}
			function createPos(longitude, latitude) {
				var pos = ol.proj.fromLonLat([longitude, latitude]);
				return pos;
			}
			function createPoint(longitude, latitude) {
				var point = new ol.geom.Point(
					createPos(longitude, latitude)
				);
				return point;
			}
			function createFeature_LonLat(longitude, latitude, id, properties) {
				var feature = createFeature_Point(
					createPoint(longitude, latitude),
					id,
					properties
				);
				return feature;
			}
			function createFeature_Point(point, id, properties) {
				var feature = new ol.Feature({
					geometry: point
				});
				feature.setId(id);
				feature.setProperties(properties);
				return feature;
			}
			function createTileLayer() {
				var layer = new ol.layer.Tile({
					source: new ol.source.OSM({}) // OpenStreetMap
				});
				return layer;
			}
			function stringDivider(str, width, spaceReplacer) {
				if (str.length > width) {
					var p = width;
					while (p > 0 && (str[p] !== ' ' && str[p] !== '-')) {
						p--;
					}
					if (p > 0) {
						var left;
						if (str.substring(p, p + 1) === '-') {
							left = str.substring(0, p + 1);
						} else {
							left = str.substring(0, p);
						}
						var right = str.substring(p + 1);
						return left + spaceReplacer + stringDivider(right, width, spaceReplacer);
					}
				}
				
				return str;
			}

			function getText(feature, resolution, format) {
				var type = format.text;
				var maxResolution = format.maxreso;
                var text = feature.get('name');
                if (text === undefined || text === null) {
                    text = "";
                }
				
				if (resolution > maxResolution)
					text = '';
				else if (type === 'hide')
					text = '';
				else if (type === 'shorten')
					text = text.trunc(12);
				else if (type === 'wrap' && (!format.placement !== 'line'))
					text = stringDivider(text, 16, '\n');
				
				return text;
			}
			function createStyleType(feature, resolution, format) {
				var style = new ol.style.Text({
					textAlign: format.textAlign,
					textBaseline: format.textBaseline,
					font: format.font,
                    text: getText(feature, resolution, format),
                    weight: "Bold",
					fill: new ol.style.Fill({
                        color: format.textColor
					}),
					stroke: new ol.style.Stroke({
                        color: format.textStrokeOutlineColor,
                        width: format.textStrokeOutlineWidth
					}),
					offsetX: format.offsetX,
					offsetY: format.offsetY+1,
					placement: format.placement,
					maxAngle: format.maxAngle,
					overflow: format.overflow,
					rotation: format.rotation
				});
				return style;
			}
			function createVectorLayer(features, format) {
				var source = new ol.source.Vector({
					features: features
				});
                var style = function (feature, resolution) {
                    return new ol.style.Style({
                        image: new ol.style.Circle({
                            radius: format.pointRadius,
                            fill: new ol.style.Fill({
                                color: format.fill
                            }),
                            stroke: new ol.style.Stroke({
                                color: format.strokeOutlineColor,
                                width: format.strokeOutlineWidth
                            })
                        }),
                        text: createStyleType(feature, resolution, format)
                    });
                };
				
				var layer = new ol.layer.Vector({
					source: source,
					style: style
				});
				return layer;
			}
			
			return {
				xpMap: {
                    position: {},
                    getCurrentLocation: getCurrentLocation,
                    initialize: function (markersModel, mainLocation, zoom, target) { // Line changed
                        //if (mainLocation.longitude == undefined) {
                        //    mainLocation.longitude = -0.173485;
                        //}
                        //if (mainLocation.latitude == undefined) {
                        //    mainLocation.latitude = 38.536317;
                        //}
                        var view = createView(mainLocation, zoom);
                        var map = createMap(view, target); // Line changed
						
						return map;
					},
                    addCenter: function (map, poi) {
                        if (!poi.latitude || !poi.longitude) // Added
                            return;

                        var pos = createPos(
                            Number(poi.longitude),
                            Number(poi.latitude)
                        );
                        map.getView()
                            .setCenter(pos);

                        if (!$rootScope.$$phase) // Added
                            $rootScope.$apply();
                    },
					addLayer: function (map, pois, format) {
						// POIs
						var features = [];
                        angular.forEach(pois, function (item) {
                            if (!(item.longitude == null || item.latitude == null)) {
                                var feature = createFeature_LonLat(
                                    Number(item.longitude),
                                    Number(item.latitude),
                                    item.id || 0, {
                                        "name": item.code || "",
                                        "radius": item.radius || 0 // Added
                                    }
                                );
                                features.push(feature);
                            } else {
                                // console.log("El punto " + item.id + " no tiene coordenadas");
                            }
						});

						var layer = createVectorLayer(features, format);
						map.addLayer(layer);

      //                  var vectorLayer = new ol.layer.Vector("Overlay");

      //                  angular.forEach(pois, function (item) {
      //                      var lon = item.longitude;
      //                      var lat = item.latitude;
      //                      var feature = new ol.Feature.Vector(
      //                          new ol.geom.Point(lon, lat),
      //                          { description: "marker number " + item.id },
      //                          { externalGraphic: '/images/bus.png', graphicHeight: 24, graphicWidth: 24, graphicXOffset: -12, graphicYOffset: -24 }
      //                      );
      //                      vectorLayer.addFeatures(feature);
						//});
      //                  map.addLayer(vectorLayer);


                        if (!$rootScope.$$phase) // Added
                            $rootScope.$apply();

                        return layer;
                    },
					setLayer: function (layer, pois) {
						var source = layer.getSource();
						source.clear();

                        //features.length = 0;
                        angular.forEach(pois, function (item) {
                            var feature = createFeature_LonLat(
                                Number(item.longitude),
                                Number(item.latitude),
                                item.id || 0, {
                                    "name": item.code || "",
                                    "radius": item.radius || 0 // Added
                                }
							);

							source.addFeature(feature);
                        });
                    },
                    showPosition: function (map, position, format) {
                        if (!position.latitude || !position.longitude)
                            return;

                        return this
                            .addLayer(
                                map,
                                position ? [position] : [],
                                format
                            );
                    },
                    center: function (map, pois, poi) {
                        var all = _.union(
                            pois,
                            poi.longitude && poi.latitude ? [poi] : []
                        );
                        if (!all.length)
                            return;

                        // Max y min
                        var centerLatitudeMax = _.max(all, function (poi) { return poi.latitude; });
                        var centerLatitudeMin = _.min(all, function (poi) { return poi.latitude; });
                        var centerLongitudeMax = _.max(all, function (poi) { return poi.longitude; });
                        var centerLongitudeMin = _.max(all, function (poi) { return poi.longitude; });

                        // Center
                        var center = {
                            longitude: (centerLongitudeMax.longitude + centerLongitudeMin.longitude) / 2,
                            latitude: (centerLatitudeMax.latitude + centerLatitudeMin.latitude) / 2
                        };

                        this
                            .addCenter(
                                map,
                                center
                            );
                    }
				}
			};
		}
	])
	.directive('xpMap', ['$rootScope', '$http', 'xpMap', function ($rootScope, $http, xpMap) {
		// https://openlayers.org/en/latest/examples/vector-labels.html
		var format = {
			text: 'wrap',
			textAlign: 'center',
			textBaseline: 'middle',
			pointRadius: 10,
			font: 'arial',
            fill: 'rgba(7, 33, 152, 0.6)',
            textStrokeOutlineColor: 'rgba(7, 33, 152, 1)',
            textColor: 'white',
			strokeOutlineColor: 'rgba(7, 33, 152, 1)',
            strokeOutlineWidth: 1.5,
            textStrokeOutlineWidth: 3,
			offsetX: 0,
			offsetY: 0,
			placement: 'point',
			maxAngle: 0,
			overflow: true,
			rotation: 0
		};
		
		return {
			restrict: 'E',
			replace: true,
            template: '<div id="map" style="width:100%; height:100%"></div>',
			link: function ($scope, element, attrs) {
                var target = 'map' + $scope.$id; // Added
                //console.log("target: -", target, "-");
                // element.attr('id', target); // Added
				var markersModel = attrs["markers"];
				var routeModel = attrs["route"];

                var centerLongitude = attrs["centerLongitude"];
                var centerLatitude = attrs["centerLatitude"];
                var mainLongitude = attrs["mainLongitude"];
                var mainLatitude = attrs["mainLatitude"];
                var mainLongitudeTemplate = attrs["mainLongitudeTemplate"];
                var mainLatitudeTemplate = attrs["mainLatitudeTemplate"];
                var mainPlace = attrs["mainPlace"];

                var currentLocate = attrs["currentLocate"]; // Added
                var externalClick = attrs["externalClick"];
                var zoomTemplate = attrs["zoom"];
                var zoom = zoomTemplate && $scope.$eval(zoomTemplate);

                angular.extend($scope, xpMap);

                angular.element(document).ready(function () {
                    $scope.xpMap.position.longitude = mainLongitude || $scope.$eval(mainLongitudeTemplate);
                    $scope.xpMap.position.latitude = mainLatitude || $scope.$eval(mainLatitudeTemplate);
                    $scope.xpMap.position.name = mainPlace;

                    var map =
                        $scope.xpMap
                            .initialize(
                                markersModel,
                                $scope.xpMap.position.longitude && $scope.xpMap.position.latitude ?
                                    $scope.xpMap.position :
                                    {
                                        longitude: centerLongitude,
                                        latitude: centerLatitude
                                    }
                                ,
                                zoom,
                                target
                            );

                    // Added
                    console.log("EXPECT: XPMAP.POSITION LATITUDE AND LONGITUDE NOT EMPTY: " + $scope.xpMap.position.latitude + ' - ' + $scope.xpMap.position.longitude);

                    // POIs
                    var pois = [];
                    var poisLayer;
                    $scope.$watch(markersModel, function () {
                        pois = $scope.$eval(markersModel) || [];

                        // POIs
                        if (pois.length === 0) // Added
                            return;

                        poisLayer = $scope
                            .xpMap
                            .addLayer(
                                map,
                                pois,
                                format
                            );

                        $scope // Moved down
                            .xpMap
                            .center(map, pois, $scope.xpMap.position);
					});

					// Route
					var control;
					var options;
					var stopCounter = 0;
					var getWayPoints = function (route)
					{
						var waypoints = [];
						if (route.length > 0) {
							var stop = route[0];
							waypoints.push([stop.longitude, stop.latitude]);
						}

						for (var i = 1; i < route.length; i++) {
							var stop = route[i];
							waypoints.push([stop.longitude, stop.latitude]);

							stopCounter++;
							if (stopCounter == 2) {
								break;
							}
						}

						return waypoints;
					}
					$scope.$watch(routeModel, function () {
						var route = $scope.$eval(routeModel) || [];

						var waypoints = getWayPoints(route);

						//if (control) {
						//	map.removeControl(control);
						//	options.waypoints = waypoints;
						//	map.addControl(control);
						//} else {
							options = {
								map,
								waypoints
							};
							control = new olrm.Control(options);
							map.addControl(control);
						//}
					});

                    // Main
                    var mainFormat = angular.extend({}, format, { fill: 'green' });
                    var mainPois = [];
                    var mainLayer = $scope.xpMap.showPosition(map, $scope.xpMap.position, mainFormat);
                    if ($scope.xpMap.position.longitude && $scope.xpMap.position.latitude) {
                        mainPois.length = 0;
                        mainPois.push($scope.xpMap.position);

                        mainLayer = $scope
                            .xpMap
                            .addLayer(
                                map,
                                mainPois,
                                mainFormat
                            );

                        $scope
                            .xpMap
                            .addCenter(
                                map,
                                $scope.xpMap.position
                            );
                    }
                    $scope.$watch(mainLongitudeTemplate, function (newValue) {
                        $scope.xpMap.position.longitude = newValue;

                        if (!$scope.xpMap.position.longitude || !$scope.xpMap.position.latitude)
                            return;

                        // mainPois.length = 0;   <--   REMOVED
                        // mainPois.push($scope.xpMap.position);   <--   REMOVED
                        if (!mainLayer) {
                            mainLayer = $scope
                                .xpMap
                                .addLayer(
                                    map,
                                    [$scope.xpMap.position], // Line changed
                                    mainFormat
                                );
                        } else {
                            $scope
                                .xpMap
                                .setLayer(
                                    mainLayer,
                                    [$scope.xpMap.position]
                                );
                        }

                        $scope
                            .xpMap
                            .addCenter(
                                map,
                                $scope.xpMap.position
                            );
                    });
                    $scope.$watch(mainLatitudeTemplate, function (newValue) {
                        $scope.xpMap.position.latitude = newValue;

                        if (!$scope.xpMap.position.longitude || !$scope.xpMap.position.latitude)
                            return;

                        // mainPois.length = 0;   <--   REMOVED
                        // mainPois.push($scope.xpMap.position);   <--   REMOVED
                        if (!mainLayer) {
                            mainLayer = $scope
                                .xpMap
                                .addLayer(
                                    map,
                                [$scope.xpMap.position], // Line changed
                                    mainFormat
                                );
                        } else {
                            $scope
                                .xpMap
                                .setLayer(
                                    mainLayer,
                                    [$scope.xpMap.position]
                                );
                        }

                        $scope
                            .xpMap
                            .addCenter(
                                map,
                                $scope.xpMap.position
                            );
                    });

                    // ADDED
                    // on current Locate
                    if (currentLocate) {
                        $scope.xpMap
                            .getCurrentLocation()
                            .then(
                                function (position) {
                                    $scope.xpMap.position.longitude = position.longitude;
                                    $scope.xpMap.position.latitude = position.latitude;
                                    $scope.xpMap.position.name = "";

                                    $scope.$eval(currentLocate);

                                    if (!$scope.$$phase)
                                        $scope.$apply();
                                },
                                function (error) {
                                    alert(
                                        "Error obteniendo la posición actual"
                                    );
                                }
                            );
                    }

                    map.on('click', function (event) {
                        var pos = ol.proj.transform(event.coordinate, 'EPSG:3857', 'EPSG:4326');
                        $scope.xpMap.position.longitude = pos[0];
                        $scope.xpMap.position.latitude = pos[1];
                        $scope.xpMap.position.name = ""; // Value changed

                        if (externalClick) {
                            $scope.$eval(externalClick);

                            if (!$scope.$$phase)
                                $scope.$apply();
                        }
                    });
                });
            }
        };
    }])
    ;