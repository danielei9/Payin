'use strict';
angular.module('xp', ['xp.communication', 'xp.authentication'])
	.filter('xpTime', function () {
		return function (input) {
			var time = new Date(input);
			var result =
				("00" + time.getHours()).slice(-2) + ':' +
				("00" + time.getMinutes()).slice(-2);
			return result;
		}
	})
	.filter('xpAdd', function () {
		return function (input, value) {
			return input + value;
		};
	})
	.filter('xpEquals', function () {
		return function (input, value) {
			return input == value;
		};
	})
	.filter('xpDistinct', function () {
		return function (input, value) {
			return input != value;
		};
	})
	.filter('xpMore', function () {
		return function (input, value) {
			return input > value;
		};
	})
	.filter('xpMoreOrEquals', function () {
		return function (input, value) {
			return input >= value;
		};
	})
	.filter('xpLess', function () {
		return function (input, value) {
			return input < value;
		};
	})
	.filter('xpLessOrEquals', function () {
		return function (input, value) {
			return input <= value;
		};
	})
	.service('xpStack', ['$rootScope', '$cacheFactory', '$location', '$window', function ($rootScope, $cacheFactory, $location, $window) {
		var cachefilter = $cacheFactory('filters');

		return {
			'get': function (key) {
				var filter = cachefilter.get(key);
				return filter;
			},
			'pop': function (key) {
				var filter = cachefilter.get(key);
				if (filter) {
					cachefilter.remove(key);
				}
				return filter;
			},
			'push': function (param) {
				if (param) {
					//this.search = param.search = param.search.bind(param.scope);
					cachefilter.put(param.key, param);
				}
				//$rootScope.$broadcast('advancedSearch', param);
			},
			// Navigations
			'navigations': function () {
				var navigation = this.get('navigationStack');
				if (!navigation) {
					navigation = {
						'key': 'navigationStack',
						'list': []
					};
					this.push(navigation);
				}
				return navigation.list;
			},
			'navigationClear': function (url) {
				this.navigations().length = 0; // Empty the array
			},
			'navigationAddPopup': function (url, title, target, previousScope) {
				this.navigations().push({
					'type': 'popup',
					'url': url || $location.path(),
					'target': target,
					'title': title,
					'arguments': function () { return scope.arguments },
					'data': function () { return scope.data },
					'returnData': function (item) {
						if (returnvalues)
							scope.$eval(returnvalues, { item: item });
					},
					'previousScope': previousScope
				});
			},
			'navigationAddPanel': function (url, title, target, previousScope) {
				this.navigations().push({
					'type': 'panel',
					'url': url || $location.path(),
					'target': target,
					'title': title,
					'arguments': function () { return scope.arguments },
					'data': function () { return scope.data },
					'returnData': function (item) {
						if (returnvalues)
							scope.$eval(returnvalues, { item: item });
					},
					'previousScope': previousScope
				});
			},
			'navigationSearch': function (url) {
				var navigations = this.navigations();
				do {
					var item = navigations.pop();
				} while ((item) && (item.url != url))
			},
			'navigate': function (scope, redirectUrl, result) {
				var navigations = this.navigations();
				var item = navigations.pop();

                if (redirectUrl) {
                    if ((item) && (item.type === 'popup')) {
                        // Close popup
                        scope.close();
                    }

                    this.navigationAddPanel(item ? item.url : "", "", redirectUrl, scope);
					$location.url(redirectUrl);
				} else if (!item) {
					//$window.history.back();
					return false;
				} else if (item.type === 'popup') {
					// Close popup
					scope.close(result);

					// Restore previous view
					if (item.previousScope)
						if (item.previousScope.search)
							item.previousScope.search();
				} else if (item.url) {
					$location.url(item.url);
				} else {
					$window.history.back();
				}
				return true;
			}
		};
	}])
	.service('xpResponseHandler', function () {
		function paramError(where) {
			throw {
				'name': 'defaultHttpResponseHandler',
				'level': where + ': Error de parámetros',
				'message': 'Todos los parámetros de ' + where + ' deben ser funciones válidas'
			};
		}

		return {
			'httpResponseHandler': function (http) {
				var successFuncs = [];
				var errorFuncs = [];
				var handler = {
					'success': function () {
						var that = this;
						angular.forEach(arguments, function (obj) {
							if (angular.isFunction(obj))
								successFuncs.push(obj);
							else
								paramError('"success"');
						});
						return handler;
					},
					'error': function () {
						var that = this;
						angular.forEach(arguments, function (obj) {
							if (angular.isFunction(obj))
								errorFuncs.push(obj);
							else
								paramError('"error"');
						});
						return handler;
					}
				};
				http
					.then(
						function (data) {
							if (successFuncs)
								angular.forEach(successFuncs, function (func) {
									func.call(this, data.data, data.status, data.headers, data.config);
								});
						},
						function (data, status, headers, config) {
							if (errorFuncs)
								angular.forEach(errorFuncs, function (func) {
									func.call(this, data, status, headers, config);
								});
						}
					);
				return handler;
			}
		};
	})
	.factory('xpQuery', ['xpCommunication', 'xpStack', '$interpolate', function (xpCommunication, xpStack, $interpolate) {
		return {
			'initializeQuery': function () {
				var that = this;
				this.$on('search', function (event) {
					var params = {};
					angular.extend(params, that.arguments);

					that.count = 0;

					that.connect();
					if (that.onPrevious) {
						var action = new Function('scope', that.onPrevious);
						action(that);
					}
					xpCommunication
						.get(that.apiUrl, that.id, params)
						.success(that.success.bind(that), function (data) {
                            that.count = data.length;

                            that.data.length = 0;
                            angular.forEach(data.data, function (item, key) {
                                that.data[key] = item;
                            });
							angular.forEach(data, function (item, key) {
								if (key != 'data' && key != 'count')
									that.arguments[key] = item;
							});

							if (that.onSuccess) {
								var action = new Function('scope', 'xpStack', 'data', that.onSuccess);
								action(that, xpStack, data);
							}

							var successUrl =
								that.onSuccessUrl ?
									$interpolate(that.onSuccessUrl)(data)
									.replace("//", "/") :
								"";
						})
						.error(that.error.bind(that));
				});
				this.$on('clean', function (event) {
					that.arguments.filter = '';
					that.search();
				});
			},
			'search': function () {
				this.$emit('search');
			},
			'cancel': function () {
				var that = this;

				// Refrescar
				if (xpStack.navigate(that))
					that.clearInbound();
			},
			'clean': function () {
				this.$emit('clean');
			},
			'count': function () { return this.count; }
		};
	}])
	.factory('xpQueryPaginable', ['xpCommunication', function (xpCommunication) {
		return {
			'top': this.pageSize,
			'skip': 0,
			'currentPage': function () { return this.currentPage; },
			'numPages': function () {
				var result = Math.ceil(this.count / this.top);
				return result;
			},
			'loadPage': function (page) {
				this.currentPage = page;
				this.search();
			},
			'search': function () {
				var that = this;

				var params = {};
				angular.extend(params, that.arguments);
				params.$skip = that.skip;
				params.$top = that.top;
				params.$inlinecount = "allpages";

				that.data = {};
				that.count = 0;

				that.connect();
				xpCommunication
					.get(that.apiUrl, that.id, params)
					.success(that.success.bind(that), function (data) {
						that.data = data.items;
						that.count = data.count;
						that.currentPage = that.currentPage() || 1;
					})
					.error(that.error.bind(that));
			}
		};
	}])
	.factory('xpGet', ['xpCommunication', 'xpStack', '$interpolate', function (xpCommunication, xpStack, $interpolate) {
		return {
			'search': function () {
				var that = this;

				var params = {};
				angular.extend(params, that.arguments);

				that.connect();
				if (that.onPrevious) {
					var action = new Function('scope', that.onPrevious);
					action(that);
				}
				xpCommunication
					.get(that.apiUrl, that.id, params)
					.success(that.success.bind(that), function (data) {
						if (data.data && (data.data.length > 0)) {
							angular.forEach(data.data[0], function (item, key) {
								that.arguments[key] = item;
							});
						}
						angular.forEach(data, function (item, key) {
							if (key != 'data')
								that.temp[key] = item;
						});

						if (that.onSuccess) {
							var action = new Function('scope', 'xpStack', 'data', that.onSuccess);
							action(that, xpStack, data);
						}

						var successUrl =
							that.onSuccessUrl ?
								$interpolate(that.onSuccessUrl)(data)
								.replace("//", "/") :
							"";

						// Refrescar
						if (that.xpGoBack)
							if (xpStack.navigate(that, successUrl))
								that.clearInbound();
					})
					.error(that.error.bind(that));
			}
		};
	}])
	.factory('xpPut', ['xpCommunication', 'xpStack', '$window', '$interpolate', function (xpCommunication, xpStack, $window, $interpolate) {
		return {
			'accept': function () {
				var that = this;

				that.connect();
				if (that.onPrevious) {
					var action = new Function('scope', that.onPrevious);
					action(that);
				}
				var params = angular.extend({}, that.arguments);

				xpCommunication
					.put(that.apiUrl, that.id, params, null, that.files)
					.success(that.success.bind(that), function (data) {
						if (that.onSuccess) {
							var action = new Function('scope', 'xpStack', 'data', that.onSuccess);
							action(that, xpStack, data);
						}

						var successUrl =
							that.onSuccessUrl ?
								$interpolate(that.onSuccessUrl)(data)
								.replace("//", "/") :
							"";

						// Refrescar
						if (that.xpGoBack)
							if (xpStack.navigate(that, successUrl))
								that.clearInbound();
						
						if (that.onModal) {
							xpStack.navigationAddPopup(that.panelUrl, that.panelTitle, "", that);
							that.open();
						}
					})
					.error(that.error.bind(that));
			}
		};
	}])
	.factory('xpPost', ['xpCommunication', 'xpStack', '$window', '$interpolate', function (xpCommunication, xpStack, $window, $interpolate) {
		return {
			'accept': function () {
				var that = this;

				var params = angular.extend({}, that.arguments);

				that.connect();
				if (that.onPrevious) {
					var action = new Function('scope', that.onPrevious);
					action(that);
				}
				xpCommunication
					.post(that.apiUrl, that.id, params, null, that.files)
					.success(that.success.bind(that), function (data) {
						if (that.onSuccess) {
							var action = new Function('scope', 'xpStack', 'data', that.onSuccess);
							action(that, xpStack, data);
						}

						var successUrl =
							that.onSuccessUrl ?
								$interpolate(that.onSuccessUrl)(data)
								.replace("//", "/") :
							"";

						// Refrescar
						if (that.xpGoBack)
							if (xpStack.navigate(that, successUrl) || that.clearOnExecute)
								that.clearInbound();

						if (that.onModal) {
							xpStack.navigationAddPopup(that.panelUrl, that.panelTitle, "", that);
							that.open();
						}
					})
					.error(that.error.bind(that));
			}
		};
	}])
	.factory('xpDelete', ['xpCommunication', 'xpStack', '$window', '$interpolate', function (xpCommunication, xpStack, $window, $interpolate) {
		return {
			'accept': function () {
				var that = this;

				var params = angular.extend({}, that.arguments);

				that.connect();
				if (that.onPrevious) {
					var action = new Function('scope', that.onPrevious);
					action(that);
				}
				xpCommunication
					.delete(that.apiUrl, that.id, params)
					.success(that.success.bind(that), function (data) {
						if (that.onSuccess) {
							var action = new Function('scope', 'xpStack', 'data', that.onSuccess);
							action(that, xpStack, data);
						}

						var successUrl =
							that.onSuccessUrl ?
								$interpolate(that.onSuccessUrl)(data)
								.replace("//", "/") :
							"";

						// Refrescar
						if (that.xpGoBack)
							if (xpStack.navigate(that, successUrl) || that.clearOnExecute)
								that.clearInbound();

						if (that.onModal) {
							xpStack.navigationAddPopup(that.panelUrl, that.panelTitle, "", that);
							that.open();
						}
					})
					.error(that.error.bind(that));
			}
		};
	}])
	.factory('xpMasterDetail', ['xpCommunication', function (xpCommunication) {
		return {
			'searchDetail': function (id) {
				var that = this;

				var param = id || that.temp[that.masterDetailName + 'Id'];
				if (!param)
					return;

				var params = {};
				that.temp[that.masterDetailName + 'Id'] = param;

				that.connect();
				xpCommunication
					.get(that.apiDetailUrl, param, params)
					.success(that.success.bind(that), function (data) {
						that.temp[that.masterDetailName] = data;
					})
					.error(that.error.bind(that));
			}
		};
	}])
	.factory('navigationService', ['$location', function ($location) {
		var routes = [];
		var navigationService = {};
		var canAssociate = false;

		navigationService.setData = function (data) {
			routes.push(data);
			canAssociate = true;
		}

		navigationService.getData = function (scope) {
			var route, index;
			var path = angular.isString(scope) ? scope : $location.path();
			angular.forEach(routes, function (value, i) {
				if (path === value.path) {
					route = value;
					index = i;
				}
			});
			if (route) {
				routes.splice(index, 1);
				return route.data;
			} else {
				if (canAssociate && (routes.length > 0)) {
					route = routes[routes.length - 1];
					if (scope)
						scope.returData = route.returData;
				}
				canAssociate = false;
			}
		}

		return navigationService;
	}])
	// Verbs
	.directive('xpList', ['xpQuery', 'xpQueryPaginable', 'xpStack', function (xpQuery, xpQueryPaginable, xpStack) {
		return {
			restrict: 'E',
			priority: 100,
			scope: true,
			controller: ['$scope', '$attrs', '$stateParams', //'xpAnalitics',
				function ($scope, $attrs, $stateParams/*,   xpAnalitics*/) {
					var isPaginable = $attrs.isPaginable;
					var initialSearch = $attrs.$attr['initialSearch'];

					// Initialize scope
					angular.extend($scope, xpQuery);
					$scope.initializeQuery();
					if (isPaginable)
						angular.extend($scope, xpQueryPaginable);
					$scope.apiUrl = $scope.app.apiBaseUrl + $attrs.api
						.replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
						.replace("//", "/");
					$scope.onPrevious = $attrs.xpPrevious;
					$scope.onSuccess = $attrs.xpSuccess;

					xpStack.push({
						"key": "list",
						"scope": $scope
					});

					var init = $attrs["xpInit"];
					if (init) {
						var action = new Function('scope', 'params', init);
						action($scope, $stateParams);
					}

					if (initialSearch)
						$scope.search();
				}
			]
		};
	}])
	.directive('xpGet', ['xpGet', function (xpGet) {
		return {
			restrict: 'E',
			priority: 100,
			scope: true,
			controller: ['$rootScope', '$scope', '$attrs', '$stateParams',
				function ($rootScope, $scope, $attrs, $stateParams) {
					var noInitialSearch = $attrs.$attr['noInitialSearch'];

					// Initialize scope
					angular.extend($scope, xpGet);
					$scope.apiUrl = $scope.app.apiBaseUrl + $attrs.api
						.replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
						.replace("//", "/");
					if ($stateParams.id)
						$scope.id = $scope.id || $stateParams.id;
					$scope.onPrevious = $attrs.xpPrevious;
					$scope.onSuccess = $attrs.xpSuccess;
					$scope.onSuccessUrl = $attrs.xpSuccessUrl;
					$scope.xpGoBack = $attrs.xpGoBack ? $scope.$eval($attrs.xpGoBack) : false;

					var init = $attrs["xpInit"];
					if (init) {
						var action = new Function('scope', 'params', init);
						action($scope, $stateParams);
					}

					if (!noInitialSearch)
						$scope.search();
				}
			]
		};
	}])
	.directive('xpPut', ['xpPut', function (xpPut) {
		return {
			restrict: 'E',
			priority: 100,
			scope: true,
			controller: ['$scope', '$attrs', '$stateParams',
				function ($scope, $attrs, $stateParams) {
					// Initialize scope
					angular.extend($scope, xpPut);
					$scope.apiUrl = $scope.app.apiBaseUrl + $attrs.api
						.replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
						.replace("//", "/");
					if ($stateParams.id)
						$scope.id = $scope.id || $stateParams.id;
					$scope.modalId = $attrs.id;
					$scope.onPrevious = $attrs.xpPrevious;
					$scope.onSuccess = $attrs.xpSuccess;
					$scope.onModal = $attrs.xpModal;
					$scope.xpGoBack = $attrs.xpGoBack ? $scope.$eval($attrs.xpGoBack) : true;
					$scope.clearOnExecute = $attrs.xpClearOnExecute;

					var init = $attrs["xpInit"];
					if (init) {
						var action = new Function('scope', 'params', init);
						action($scope, $stateParams);
					}
				}
			]
		};
	}])
	.directive('xpPost', ['xpPost', function (xpPost) {
		return {
			restrict: 'E',
			priority: 100,
			scope: true,
			controller: ['$scope', '$attrs', '$stateParams',
				function ($scope, $attrs, $stateParams) {
					// Initialize scope
					angular.extend($scope, xpPost);
					$scope.apiUrl = $scope.app.apiBaseUrl + $attrs.api
						.replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
						.replace("//", "/");
					if ($stateParams.id)
						$scope.id = $scope.id || $stateParams.id;
					$scope.modalId = $attrs.id;
					$scope.onPrevious = $attrs.xpPrevious;
					$scope.onSuccess = $attrs.xpSuccess;
					$scope.onSuccessUrl = $attrs.xpSuccessUrl;
					$scope.onModal = $attrs.xpModal;
					$scope.xpGoBack = $attrs.xpGoBack ? $scope.$eval($attrs.xpGoBack) : true;
					$scope.clearOnExecute = $attrs.xpClearOnExecute;

					var init = $attrs["xpInit"];
					if (init) {
						var action = new Function('scope', 'params', init);
						action($scope, $stateParams);
					}
				}
			]
		};
	}])
	.directive('xpDelete', ['xpDelete', function (xpDelete) {
		return {
			restrict: 'E',
			priority: 100,
			scope: true,
			controller: ['$scope', '$attrs', '$stateParams', //'xpAnalitics',
				function ($scope, $attrs, $stateParams/*, xpAnalitics*/) {
					// Initialize scope
					angular.extend($scope, xpDelete);
					$scope.apiUrl = $scope.app.apiBaseUrl + $attrs.api
						.replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
						.replace("//", "/");
					if ($stateParams.id)
						$scope.id = $scope.id || $stateParams.id;
					$scope.modalId = $attrs.id;
					$scope.onPrevious = $attrs.xpPrevious;
					$scope.onSuccess = $attrs.xpSuccess;
					$scope.onSuccessUrl = $attrs.xpSuccessUrl;
					$scope.onModal = $attrs.xpModal;
					$scope.xpGoBack = $attrs.xpGoBack ? $scope.$eval($attrs.xpGoBack) : true;
					$scope.clearOnExecute = $attrs.xpClearOnExecute;

					var init = $attrs["xpInit"];
					if (init) {
						var action = new Function('scope', 'params', init);
						action($scope, $stateParams);
					}
				}
			]
		};
	}])
	.directive('xpMasterDetail', ['xpMasterDetail', function (xpMasterDetail) {
		return {
			restrict: 'E',
			controller: ['$scope', '$attrs', '$stateParams',
				function ($scope, $attrs, $stateParams) {
					angular.extend($scope, xpMasterDetail);
					$scope.apiDetailUrl = $scope.app.apiBaseUrl + $attrs.xpApi
						.replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
						.replace("//", "/")
					$scope.masterDetailName = $attrs.xpName;

					var init = $attrs["xpInit"];
					if (init) {
						var action = new Function('scope', 'params', init);
						action($scope, $stateParams);
					}

					$scope.searchDetail();
				}
			]
		};
	}])
	.directive('xpRealTime', [function () {
		return {
			restrict: 'EA',
			scope: true,
			controller: ['$scope', '$element', '$attrs', function ($scope, $element, $attrs) {
				var root = $scope.$root;
				var result = $attrs.result;

				root.StartNotification = function () {
					root.notificaciones = [];

					// Register
					var chat = $.connection.chatHub;

					chat.client.sendMessage = function (notification) {
						root.notificaciones.push(notification);
						titlenotifier.add();
						root.$apply();
					};
					$.connection.hub.start();
				}

				root.StartNotification();
				//},
				//controller: function ($scope) {
				//	$scope.deleteByUser = function (id) {
				//		for (var i = $scope.notificaciones.length; i--;) {
				//			if ($scope.notificaciones[i].origenId === id) {
				//				$scope.notificaciones.splice(i, 1);
				//			}
				//		}
				//	}
				//}
				//controller: function ($scope) {
				$scope.deleteAll = function (id) {
					for (var i = $scope.notificaciones.length; i--;) {
						$scope.notificaciones.splice(i, 1);
						titlenotifier.reset();
						//$scope.notificaciones = []; // LO BORRA PARA SIEMPRE
						setTimeout(function () { scrollToBottom() }, 1000);
					}
				}
			}]
		};
	}])
	// Window
	.directive('xpPopup', [function () {
		return {
			restrict: 'A',
			priority: 100,
			scope: true,
			controller: ['$scope', '$element', '$attrs', 'xpInitialScope', 'xpStack', function ($scope, $element, $attrs, xpInitialScope, xpStack) {
				angular.extend($scope, xpInitialScope);
				$scope.initialize();
				$scope.windowState();
				$scope.panelUrl = $attrs['xpPanel'];
				$scope.panelTitle = $attrs['xpTitle'];
				$scope.cancel = function () {
					// Refrescar
					if (xpStack.navigate($scope))
						$scope.clearInbound();
				};
				$scope.accept = function (result) {
					// Refrescar
					if (xpStack.navigate($scope, "", result))
						$scope.clearInbound();
				};

				$element.on('click.xp-popup', '[data-dismiss="modal"]', function () {
					$scope.dismiss('cancel');
				});

				angular.extend($scope, $scope.panelInfo);
			}]
		};
	}])
	.directive('xpPanel', ['xpInitialScope', function (xpInitialScope) {
		return {
			restrict: 'AE',
			priority: 100,
			scope: true,
			controller: ['$scope', '$attrs', '$location', 'xpStack', function ($scope, $attrs, $location, xpStack) {
				angular.extend($scope, xpInitialScope);
				$scope.initialize();
				$scope.windowState();
				//$scope.panelUrl = $attrs['xpPanel'];
				$scope.panelUrl = $location.url();
				$scope.panelTitle = $attrs['xpTitle'];
				$scope.cancel = function () {
					// Refrescar
					if (xpStack.navigate($scope))
						$scope.clearInbound();
				};
			}]
		};
	}])
	// Navigations
	.directive('xpNavigate', ['xpStack', '$location', function (xpStack, $location) {
		return {
			restrict: 'AE',
			link: function ($scope, elemen, $attrs) {
				elemen.bind('click', function (ev) {
					var href = $attrs.href;
					var returnvalues = $attrs.returnvalues;

					if ($attrs.xpModal) {
						// Popup
						xpStack.navigationAddPopup($scope.panelUrl, $scope.panelTitle, href, $scope);
					} else {
						// Panel
						xpStack.navigationAddPanel($scope.panelUrl, $scope.panelTitle, href, $scope);
					}
				});
			}
		};
	}])
	.directive('xpNavigateBack', ['xpStack', '$location', function (xpStack, $location) {
		return {
			restrict: 'A',
			link: function ($scope, elemen, $attrs) {
				elemen.bind('click', function (ev) {
					var href = $attrs.href;
					var returnvalues = $attrs.returnvalues;
					if (href && (href.substr(0, 1) === '#'))
						href = href.substr(1);

					// Add navigation
					var item = xpStack.navigationSearch(href);
					if (!href && item)
						$location.path(item.url);
				});
			}
		};
	}])
	.directive('xpBreadcrum', [function () {
		return {
			restrict: 'A',
			controller: ['$scope', 'xpStack', function ($scope, xpStack) {
				$scope.breadcrums = xpStack.navigations();
			}]
		};
	}])
	// Data types and validators
	.directive('xpInit', [function () {
		return {
			restrict: 'A',
			priority: 50,
			scope: true,
			controller: ['$scope', '$stateParams', '$attrs', '$element',
				function ($scope, $stateParams, $attrs, $element) {
					if (
						($element[0].localName === "xp-get") ||
						($element[0].localName === "xp-list") ||
						($element[0].localName === "xp-master-detail")
					)
						return;

					var init = $attrs["xpInit"];
					var action = new Function('scope', 'params', init);
					action($scope, $stateParams);
				}
			]
		};
	}])
	.directive('xpTypeahead', ['xpCommunication', 'xpInitialScope', '$parse', '$interpolate', function (xpCommunication, xpInitialScope, $parse, $interpolate) {
		var getLocals = function (url, scope, param, name, params, relation) {
			var local = {
				'relation': relation,
				'shown': false,
				'show': function () {
					var pos = $.extend({}, this.elemen.position(), {
						height: this.elemen[0].offsetHeight
					});
					this.$menu
						.css({
							top: pos.top + pos.height,
							left: pos.left,
							width: this.elemen.width()
						})
									.show();
					this.shown = true;
				},
				'hide': function () {
					this.$menu.hide();
					this.shown = false;
				},
				'prev': function () {
					var active = this.$menu.find('.active').removeClass('active');
					var prev = active.prev();
					if (!prev.length) {
						prev = this.$menu.find('li').last();
					}
					prev.addClass('active');
				},
				'next': function () {
					var active = this.$menu.find('.active').removeClass('active');
					var next = active.next();
					if (!next.length) {
						next = $(this.$menu.find('li')[0]);
					}
					next.addClass('active');
				},
				'lookup': function () {
					var filter = this.elemen.val();
					var argumentValues = this.arguments ?
						// scope.$eval(this.arguments, { 'params': params });
						angular.fromJson($interpolate(this.arguments)(scope)) :
						"";

					xpCommunication
						.get(url, filter, argumentValues)
						.success(angular.bind(this, this.render));
				},
				'render': function (data) {
					this.scope.parent.argumentSelect[name].items = data.data;
					this.show();
					this.canSearch = true;
					this.shown = true;
				},
				'canSearch': true,
				'handle': 0,
				'move': function (e) {
					if (!this.shown) return

					switch (e.keyCode) {
						case 13: // enter
						case 9: // tab
						case 27: // escape
							e.preventDefault();
							break;
						case 38: // up arrow
							e.preventDefault();
							this.prev();
							break;

						case 40: // down arrow
							e.preventDefault();
							this.next();
							break;
					}

					e.stopPropagation()
				},
				'keyup': function (e) {
					var handle;
					switch (e.keyCode) {
						case 40: // down arrow
						case 38: // up arrow
						case 16: // shift
						case 17: // ctrl
						case 18: // alt
						case 8: // delete
							break;

						case 9: // tab
						case 13: // enter
							if (!this.shown)
								return;
							this.select();
							break
						case 27: // escape
							if (!this.shown)
								return;
							this.hide();
							break
						default:
							if (this.canSearch) {
								this.canSearch = false;
								if (this.handle) {
									window.clearTimeout(this.handle);
									this.handle = 0;
								}
								this.handle = window.setTimeout(
										angular.bind(this, this.lookup),
										1000);
							}
					};
					e.stopPropagation();
					e.preventDefault();
				},
				'keydown': function (e) {
					this.suppressKeyPressRepeat = ~$.inArray(e.keyCode, [40, 38, 9, 13, 27])
					this.move(e)
				},
				'keypress': function (e) {
					if (this.suppressKeyPressRepeat)
						return
					this.move(e)
				},
				'selectItem': function (item) {
					if (relation)
						scope.$eval(relation, { item: item });
					if (item)
						this.elemen.focus();
					this.hide();
				},
				'select': function () {
					var scope = this.$menu.find('.active').scope();
					if (!scope) {
						return;
					}
					var item = scope.item;

					this.selectItem(item);
					scope.$apply();
				},
				'mouseenter': function (e) {
					this.mousedover = true;
					this.$menu.find('.active').removeClass('active');
					$(e.currentTarget).addClass('active');
				},
				'mouseleave': function (e) {
					this.mousedover = false
					if (!this.focused && this.shown) this.hide()
				},
				'focus': function (e) {
					this.focused = true
				},
				'blur': function (e) {
					this.focused = false
					if (!this.mousedover && this.shown) {
						this.hide();
						this.elemen.val("");
					}
					if (this.elemen && !this.elemen[0].value) {
						this.selectItem(null);
						scope.$apply();
					}
				}
			};
			return local;
		}

		return {
			restrict: 'A',
			replace: false,
			link: function ($scope, $element, $attrs) {
				var api = $scope.app.apiBaseUrl + $attrs.api;
				var name = $attrs.xpTypeahead;
				var urlParam = "";
				var relation = $attrs.relation;

				$scope.argumentSelect = $scope.argumentSelect || {};

				var locals = getLocals(api, $scope, urlParam, name, null, relation);
				locals.elemen = $element;
				locals.scope = $scope.argumentSelect[name] = {
					'itemClick': function (item) {
						locals.selectItem(item);
					},
					'parent': $scope
				};
				if ($attrs.arguments) {
					//locals.arguments = $attrs.arguments;
					locals.arguments = $element.attr("arguments") || $element.attr("data-arguments");
				}
				$element.bind('keyup', angular.bind(locals, locals.keyup));
				$element.bind('keypress', angular.bind(locals, locals.keypress));
				$element.bind('keydown', angular.bind(locals, locals.keydown));
				$element.bind('focus', angular.bind(locals, locals.focus));
				$element.bind('blur', angular.bind(locals, locals.blur));

				locals.$menu = $($element.siblings('ul')[0]);
				locals.$menu.on('mouseenter', angular.bind(locals, locals.mouseenter));
				locals.$menu.on('mouseleave', angular.bind(locals, locals.mouseleave));
			}
		}
	}])
	.directive('xpMatch', [function () {
		return {
			require: 'ngModel',
			link: function (scope, elem, attrs, ctrl) {
				scope.$watch('[' + attrs.ngModel + ', ' + attrs.xpMatch + ']', function (value) {
					ctrl.$setValidity('match', value[0] === value[1]);
				}, true);
			}
		};
	}])
	.directive('xpColor', [function () {
		return {
			restrict: 'A',
			link: function (scope, element, attrs, ngModel) {
				element
					.on("changeColor", function (ev) {
						scope.$apply();
					});
			},
			controller: ['$element', '$attrs', '$scope', function ($element, $attrs, $scope) {
				var model = $attrs.ngModel;

				// http://www.eyecon.ro/bootstrap-colorpicker/
				$element.colorpicker();

				$scope.$watch(model, function () {
					$scope.t = 3;
				})
			}]
		};
	}])
	.controller('xpModalCtrl', ['$scope', '$modalInstance', 'id', 'args', 'file', function ($scope, $modalInstance, id, args, file) {
		$scope.panelInfo = {};

		if (id) {
			var data = new Function('scope', 'return ' + id);
			$scope.panelInfo.id = data($scope.parent);
		}
		if (args) {
			var data = new Function('scope', 'return ' + args);
			$scope.panelInfo.arguments = data($scope.parent);
		}
		if (file) {
			$scope.myImage = file;
		}

		$scope.close = function (result) {
			$modalInstance.close(result);
		};

		$scope.dismiss = function (result) {
			$modalInstance.dismiss(result);
		};
	}])
	.directive('xpModal', [function () {
		return {
			restrict: 'A',
			scope: true,
			link: function ($scope, $element, $attrs) {
				if ($element[0].localName === 'input' && $attrs['type'] === 'file') {
					$element.bind("change", function (evt) {
						var file = evt.currentTarget.files[0];
						var reader = new FileReader();
						reader.onload = function (evt) {
							$scope.$apply(function ($scope) {
								$scope.open(evt.target.result);
							});
						};
						reader.readAsDataURL(file);
					});
				} else if ($element[0].localName === 'a') {
					$element.bind("click", function () {
						$scope.open();
					});
				}
			},
			controller: ['$scope', '$attrs', '$modal', '$log', '$state', function ($scope, $attrs, $modal, $log, $state) {
				$scope.open = function (file) {
					var args = $attrs.xpArguments
					var id = $attrs.xpId;
					var templateUrl = $attrs.xpModal;
					var onSuccess = $attrs.xpSuccess;

					var state = $state.get(templateUrl);

					var modalInstance = $modal.open({
						templateUrl: state.templateUrl,
						controller: 'xpModalCtrl',
						size: 'lg',
						resolve: {
							id: function () {
								return id;
							},
							args: function () {
								return args;
							},
							file: function () {
								return file;
							}
						}
					});
					modalInstance.result.then(
						function (selectedItem) {
							$scope.selected = selectedItem;

							if (onSuccess) {
								var action = new Function('scope', 'data', onSuccess);
								action($scope, selectedItem);
							}
						}, function () {
							$log.info('Modal dismissed at: ' + new Date());
						}
					);
				};
			}]
		};
	}])
	.directive('xpTime', [function () {
		// http://www.malot.fr/bootstrap-datetimepicker/
		return {
			restrict: 'A',
			scope: true,
			link: function ($scope, $element, $attrs) {
				var model = $attrs.xpTime;
				var input = $element.find('input');
				var inputModel = input[0].dataset.ngModel;

				$element.on('changeDate', function (event) {
					if (event.date) {
						var value = inputModel ? eval("$scope." + inputModel) : "";
						var newValue = moment(value, 'HH:mm');

						eval("$scope." + model + " = newValue.format('HH:mm:ssZ');");
					}
					else
						eval("delete $scope." + model);

					if (!$scope.$$phase)
						$scope.$apply();
				});
				$scope.$watch(model, function () {
					var value = model ? eval("$scope." + model) : "";

					if (value) {
						var newValue = moment(value, ['HH:mm:ssZ', moment.ISO_8601])
						var text = newValue.format('HH:mm');

						if (eval("$scope." + inputModel) != text) {
							eval("$scope." + inputModel + "='" + text + "'");
							$scope.picker.datetimepicker('setDate', newValue.toDate());
						}
					}
					else
						$scope.value = "";
				});
				input.on('focusout', function () {
					if (eval("$scope." + inputModel)) {
						var value = moment(eval("$scope." + model), [moment.ISO_8601])
						var text = value.format('HH:mm');

						if (eval("$scope." + inputModel) != text) {
							var newValue = moment(eval("$scope." + inputModel), ['HH:mm'])

							eval("$scope." + model + "=newValue.format('HH:mm:ssZ')");
							$scope.picker.datetimepicker('setDate', newValue.toDate());
						}
					}
					else
						eval("delete $scope." + model);

					if (!$scope.$$phase)
						$scope.$apply();
				});
			},
			controller: ['$scope', '$attrs', '$element', function ($scope, $attrs, $element) {
				$scope.picker = $element.datetimepicker({
					format: 'hh:ii',
					language: 'es-ES',
					weekStart: 1,
					todayBtn: 0,
					autoclose: 1,
					todayHighlight: 1,
					startView: 1,
					maxView: 1,
					forceParse: 0,
					minuteStep: 5,
					showMeridian: 0,
					pickerPosition: "bottom-left"
				});
			}]
		};
	}])
	.filter('xpTime', function () {
		return function (value, format) {
			if (value) {
				var result = moment(value, ["HH:mm:ssZ", "HH:mm:ss", moment.ISO_8601]);
				return result.format(format || "HH:mm:ss");
			};
		}
	})
	.filter('xpTimeUtc', function () {
		return function (value, format) {
			if (value) {
				var result = moment(value, ["HH:mm:ssZ", "HH:mm:ss", moment.ISO_8601]).utc();
				return result.format(format || "HH:mm:ss");
			};
		}
	})
	.directive('xpDuration', [function () {
		// http://www.malot.fr/bootstrap-datetimepicker/
		return {
			restrict: 'A',
			scope: true,
			link: function ($scope, $element, $attrs) {
				var model = $attrs.xpDuration;
				var input = $element.find('input');
				var inputModel = input[0].dataset.ngModel;

				$element.on('changeDate', function (event) {
					if (event.date) {
						var value = inputModel ? eval("$scope." + inputModel) : "";
						var newValue = moment(value, 'HH:mm');

						eval("$scope." + model + " = newValue.format('HH:mm:ss');");
					}
					else
						eval("delete $scope." + model);

					if (!$scope.$$phase)
						$scope.$apply();
				});
				$scope.$watch(model, function () {
					var value = model ? eval("$scope." + model) : "";

					if (value) {
						var newValue = moment(value, ['HH:mm:ss', moment.ISO_8601])
						var text = newValue.format('HH:mm');

						if (eval("$scope." + inputModel) != text) {
							eval("$scope." + inputModel + "='" + text + "'");
							$scope.picker.datetimepicker('setDate', newValue.toDate());
						}
					}
					else
						$scope.value = "";
				});
				input.on('focusout', function () {
					if (eval("$scope." + inputModel)) {
						var value = moment(eval("$scope." + model), [moment.ISO_8601])
						var text = value.format('HH:mm');

						if (eval("$scope." + inputModel) != text) {
							var newValue = moment(eval("$scope." + inputModel), ['HH:mm'])

							eval("$scope." + model + "=newValue.format('HH:mm:ss')");
							$scope.picker.datetimepicker('setDate', newValue.toDate());
						}
					}
					else
						eval("delete $scope." + model);

					if (!$scope.$$phase)
						$scope.$apply();
				});
			},
			controller: ['$scope', '$attrs', '$element', function ($scope, $attrs, $element) {
				$scope.picker = $element.datetimepicker({
					format: 'hh:ii',
					language: 'es-ES',
					weekStart: 1,
					todayBtn: 0,
					autoclose: 1,
					todayHighlight: 1,
					startView: 1,
					maxView: 1,
					forceParse: 0,
					minuteStep: 5,
					showMeridian: 0,
					pickerPosition: "bottom-left"
				});
			}]
		};
	}])
	.filter('xpDuration', function () {
		return function (value, format) {
			if (value) {
				var result = moment(value, ["HH:mm:ss", moment.ISO_8601]);
				return result.format(format || "HH:mm:ss");
			};
		}
	})
	.directive('xpDate', [function () {
		// http://www.malot.fr/bootstrap-datetimepicker/
		return {
			restrict: 'A',
			scope: true,
			link: function ($scope, $element, $attrs) {
				var model = $attrs.xpDate;
				var input = $element.find('input');
				var inputModel = input[0].dataset.ngModel;

				$element.on('changeDate', function (event) {
					if (event.date) {
						var value = inputModel ? eval("$scope." + inputModel) : "";
						var newValue = moment(value, 'YYYY-MM-DD');

						eval("$scope." + model + " = newValue.format('YYYY-MM-DD');");
					}
					else
						eval("delete $scope." + model);

					if (!$scope.$$phase)
						$scope.$apply();
				});
				$scope.$watch(model, function () {
					var value = model ? eval("$scope." + model) : "";

					if (value) {
						var newValue = moment(value, ['YYYY-MM-DD', moment.ISO_8601])
						var text = newValue.format('YYYY-MM-DD');

						if (eval("$scope." + inputModel) != text) {
							eval("$scope." + inputModel + "='" + text + "'");
							$scope.picker.datetimepicker('setDate', newValue.toDate());
						}
					}
					else
						$scope.value = "";
				});
				input.on('focusout', function () {
					if (eval("$scope." + inputModel)) {
						var value = moment(eval("$scope." + model), [moment.ISO_8601])
						var text = value.format('YYYY-MM-DD');

						if (eval("$scope." + inputModel) != text) {
							var newValue = moment(eval("$scope." + inputModel), ['YYYY-MM-DD'])

							eval("$scope." + model + "=newValue.format('YYYY-MM-DD')");
							$scope.picker.datetimepicker('setDate', newValue.toDate());
						}
					}
					else
						eval("delete $scope." + model);

					if (!$scope.$$phase)
						$scope.$apply();
				});
			},
			controller: ['$scope', '$attrs', '$element', function ($scope, $attrs, $element) {
				$scope.picker = $element.datetimepicker({
					format: 'yyyy-mm-dd hh:ii',
					language: 'es-ES',
					weekStart: 1,
					todayBtn: 1,
					autoclose: 1,
					todayHighlight: 1,
					startView: 2,
					minView: 2,
					forceParse: 0,
					minuteStep: 5,
					showMeridian: 0,
					pickerPosition: "bottom-left"
				});
			}]
		};
	}])
	.filter('xpDate', function () {
		return function (value, format) {
			if (value) {
				var result = (moment(value, ["YYYY-MM-DD", moment.ISO_8601]));
				return result.format(format || "YYYY-MM-DD");
			};
		}
	})
	.directive('xpDateTime', [function () {
		// http://www.malot.fr/bootstrap-datetimepicker/
		return {
			restrict: 'A',
			scope: true,
			link: function ($scope, $element, $attrs) {
				var model = $attrs.xpDateTime;
				var input = $element.find('input');
				var inputModel = input[0].dataset.ngModel;

				$element.on('changeDate', function (event) {
					if (event.date) {
						var value = inputModel ? eval("$scope." + inputModel) : "";
						var newValue = moment(value, 'YYYY-MM-DD HH:mm');

						eval("$scope." + model + " = newValue.format('YYYY-MM-DDTHH:mm:ssZ');");
					}
					else
						eval("delete $scope." + model);

					if (!$scope.$$phase)
						$scope.$apply();
				});
				$scope.$watch(model, function () {
					var value = model ? eval("$scope." + model) : "";

					if (value) {
						var newValue = moment(value, [moment.ISO_8601])
						var text = newValue.format('YYYY-MM-DD HH:mm');

						if (eval("$scope." + inputModel) != text) {
							eval("$scope." + inputModel + "='" + text + "'");
							$scope.picker.datetimepicker('setDate', newValue.toDate());
						}
					}
					else
						$scope.value = "";
				});
				input.on('focusout', function () {
					if (eval("$scope." + inputModel)) {
						var value = moment(eval("$scope." + model), [moment.ISO_8601])
						var text = value.format('YYYY-MM-DD HH:mm');

						if (eval("$scope." + inputModel) != text) {
							var newValue = moment(eval("$scope." + inputModel), ['YYYY-MM-DD HH:mm'])

							eval("$scope." + model + "=newValue.format('YYYY-MM-DDTHH:mm:ssZ')");
							$scope.picker.datetimepicker('setDate', newValue.toDate());
						}
					}
					else
						eval("delete $scope." + model);

					if (!$scope.$$phase)
						$scope.$apply();
				});
			},
			controller: ['$scope', '$attrs', '$element', function ($scope, $attrs, $element) {
				$scope.picker = $element.datetimepicker({
					format: 'yyyy-mm-dd hh:ii',
					language: 'es-ES',
					weekStart: 1,
					todayBtn: 1,
					autoclose: 1,
					todayHighlight: 1,
					startView: 2,
					forceParse: 0,
					minuteStep: 5,
					showMeridian: 0,
					pickerPosition: "bottom-left"
				});
			}]
		};
	}])
	.filter('xpDateTime', function () {
		return function (value, format) {
			if (value) {
				var result = (moment(value, ["YYYY-MM-DDTHH:mm:ssZ", "YYYY-MM-DD HH:mm:ssZ", moment.ISO_8601]));
				return result.format(format || "YYYY-MM-DD HH:mm:ss");
			};
		}
	})
	.filter('xpHourHHMM', function () {
		return function (value, format) {
			if (value) {
				var result = (moment(value, ["YYYY-MM-DDTmm:ssZ", "YYYY-MM-DD HH:mm:ssZ", moment.ISO_8601]));
				return result.format(format || "HH:mm");
			};
		}
	})
	.filter('xpHour', function () {
		return function (value, format) {
			if (value) {
				var result = (moment(value, ["YYYY-MM-DDTmm:ssZ", "YYYY-MM-DD HH:mm:ssZ", moment.ISO_8601]));
				return result.format(format || "HH:mm:ss");
			};
		}
	})
	.directive('xpImage', [function () {
		return {
			scope: true,
			link: function ($scope, $element, $attrs) {
				$scope.myImage = $scope.myImage || '';
				$scope.temp.avatar = $scope.temp.avatar || {};
				$scope.temp.avatar.image = '';
				$scope.cropType = "square";
				$scope.temp.size = 160;
				$scope.temp.avatar.loaded = true;
			}
		};
	}])
    .directive('xpImagePurse', [function () {
        return {
            scope: true,
            link: function ($scope, $element, $attrs) {
                $scope.myImage = $scope.myImage || '';
                $scope.temp.avatar = $scope.temp.avatar || {};
                $scope.temp.avatar.image = '';
                $scope.cropType = "rectangle";
                $scope.temp.size = { w: 231, h: 143 };
                $scope.temp.avatar.loaded = true;
            }
        };
    }])
	.controller("OnChangedAvatar", ['$rootScope', '$scope', function ($rootScope, $scope) {
		$rootScope.$on('changedAvatar', function () {			
			$scope.search();
		});
	}])
	.controller("xpCopyClipboard", ['$scope','$timeout','$parse', function ($scope,$timeout,$parse) {
	
		$scope.init = function() {
			var hexa = $scope.arguments.scriptRequest;
			var res;
			var i = 0;
			var j = 0;

			for (i = 0; i < 16; i++) {

				var cabecera = "-----------Sector " + i + "-----------";

				res = hexa.split(cabecera)
				res = hexa.split('\n');
			}

			/*for (j = 3; j < 65; j = j + 3) {
				res.splice(j, 1);				
			}*/
			$scope.arguments.modifyBlock = res;
			$scope.data.hexadecimal = angular.copy($scope.arguments.modifyBlock);
			$scope.arguments.block = angular.copy($scope.data.hexadecimal);
		};
		 $timeout($scope.init, 10000);

		$scope.copyText = function (text) {
			var textArea = document.createElement("textarea");			
			textArea.value = text;
			document.body.appendChild(textArea);
			textArea.select();			
			document.execCommand('copy');
			textArea.style.cssText = 'top:0;position:absolute;';
		}

		var copyBeforeBtn = document.querySelector('.before-button'),
            copyAfterBtn = document.querySelector('.after-button');

		copyBeforeBtn.addEventListener('click', function (event) {
			var i = 0;
			var hexa = $scope.arguments.scriptRequest;
			var res;
			for (i = 0; i < 16; i++) {

				var eliminar = "-----------Sector " + i + "-----------";	

				if (i == 0) res = hexa.replace(eliminar, "");					
				else res = res.replace(eliminar, "");				
			}
			$scope.copyText(res);
		});		
	}])
	.controller('GreyListController',['$scope', function ($scope) {

		$scope.items = [];

		$scope.greyList = [];

		$scope.actions = [];

		$scope.add = function (List) {
			if (List.quantity !== undefined || List.action != 1 || List.bonus != 1 || List.type != 1) {
				$scope.greyList = [{
					action: List.action,
					bonus: List.bonus,
					type: List.type,
					quantity: List.quantity
				}]

				$scope.items.push(List);

				$('.btn-success').removeAttr('disabled');

				$scope.arguments.items = $scope.items;
		}

		}

		$scope.add2 = function (List) {
			if (List.quantity !== undefined || List.sector != 1 || List.block != 1) {
				List.splice(0, 1);

				$scope.greyList = [{				
					"sector": List.sector,
					"block": List.block,
					"quantity": List.quantity
				},]

				$scope.items.push(List);
			
				$scope.actions.push({
					sector: List.sector,
					block: List.block,
					quantity: List.quantity
				});
			

				$('.btn-success').removeAttr('disabled');

				$scope.arguments.items = $scope.actions;

				console.log($scope.arguments.items);

			}

		}

		$scope.remove = function (index) {

			$scope.items.splice(index, 1);

			if (!$scope.items.length) {	$('.btn-success').attr('disabled', true);}
		}

	}])
	.controller('PromotionController', ['$scope','$filter', function ($scope,$filter) {

		$scope.promoConditions = [];
	
		$scope.promoActions = [];
	
		//$scope.promoLaunchers = [];

		$scope.titles = [];

		$scope.arguments.promoConditions = [];

		$scope.arguments.promoActions = [];

		//$scope.arguments.promoLaunchers = [];

		$scope.arguments.promoPrices = []

		$scope.add3 = function (List) {
			if (List.quantity !== undefined || List.condition > -1 ) {
				$scope.promoConditions = [{
					quantity: List.quantity,				
					condition: List.condition
				}]
				$scope.arguments.promoConditions.push($scope.promoConditions[0]);
			}
		}
		$scope.add6 = function (id,titleName) {
			if (id) {

				$scope.titles = [{
					id: id,
					name: titleName
				}, ]
				var found = $filter('filter')($scope.arguments.promoPrices, { id: $scope.titles[0].id }, true);
				if (!found.length)			
					$scope.arguments.promoPrices.push($scope.titles[0]);
						
				$scope.arguments.title = "";
				$scope.arguments.TitleName = "";
			}
		}

		$scope.removeTitle = function (index) {		
				$scope.arguments.promoPrices.splice(index, 1);
		}

		$scope.removeCondition = function (index, type) {
			$scope.arguments.promoConditions.splice(index, 1);
		}

		$scope.add3Update = function (List) {
			if (List.quantity !== undefined || List.condition > -1) {
				$scope.promoConditions = [{
					quantity: List.quantity,
					type: List.condition
				}]
				$scope.arguments.promoConditions.push($scope.promoConditions[0]);
			}
		}

		//$scope.add4 = function (List) {
		//	if (List.quantity !== undefined || List.launcher > -1) {

		//		$scope.promoLaunchers = [{
		//			quantity: List.quantity,
		//			launcher: List.launcher
		//		}, ]
		//		$scope.arguments.promoLaunchers = $scope.promoLaunchers;
		//	}
		//}

		//	$scope.add4Update = function (List) {
		//		if (List.quantity !== undefined || List.launcher > -1) {

		//			$scope.promoLaunchers = [{
		//				quantity: List.quantity,
		//				type: List.launcher
		//			}, ]
		//			$scope.arguments.promoLaunchers.push($scope.promoLaunchers[0]);
		//		}
		//	}
		

			$scope.add5 = function (List) {
				if (List.quantity !== undefined || List.actions > -1) {
					$scope.promoActions = [{
						quantity: List.quantity,
						action: List.action
					}]
					$scope.arguments.promoActions = $scope.promoActions;
				}
			}
			$scope.add5Update = function (List) {
				if (List.quantity !== undefined || List.actions > -1) {
					$scope.promoActions = [{
						quantity: List.quantity,
						type: List.action
					}]
					$scope.arguments.promoActions.push($scope.promoActions[0]);
				}

			}

			$scope.remove = function (index, type) {

				if(type==3)
					$scope.arguments.promoConditions.splice(index, 1);

				else if (type == 4)
					$scope.arguments.promoLaunchers.splice(index, 1);

				else if (type == 5)
					$scope.arguments.promoActions.splice(index, 1);

			}

	

	}])
	.controller('CreateNotificationController', ['$scope', function ($scope) {

		$scope.users = [];
		$scope.arguments.users = [];
	

		$scope.addUser = function (users) {
			if (users !== undefined) {
			
				$scope.arguments.users.push(users);
			}

		}

		$scope.remove = function (index) {
			$scope.arguments.users.splice(index, 1);		
		}

	}])
;