'use strict';
angular.module('xp.authentication', ['xp.communication'])
	.run(['$rootScope', 'xpAuthentication', function ($rootScope, xpAuthentication) {
		if (
			sessionStorage["accessToken"] ||
			window.location.pathname == '#/Account/Login' ||
			window.location.pathname == '/Account/Register' ||
			window.location.pathname == '/Account/ForgotPassword' ||
			window.location.pathname == '/Account/ConfirmEmail' ||
			window.location.pathname == '/Account/ConfirmForgotPassword' ||			
			window.location.pathname == '/ServiceWorker/AcceptAssignment'
		) {
			xpAuthentication.load();
		} else {
			xpAuthentication.logout();
		}
	}])
	.config(function ($httpProvider) {
		$httpProvider.interceptors.push('xpInterceptorService');
	})
	.factory('xpAuthentication', ['xpCommunication', '$http', '$rootScope', '$q', '$ionicHistory',
		function (xpCommunication, $http, $rootScope, $q, $ionicHistory) {
			function getURLParameter(name) {
				return decodeURIComponent((new RegExp('[?|&]' + name + '=([^&;]+?)(&|#|;|$)').exec(location.search) || [, ""])[1]
					//.replace(/\+/g, '%20')
					) || null
			}
	
			return {
				'retry': function (config, deferred) {
					xpCommunication.retry(config, deferred);
				},
				'load': function () {
					$rootScope.authentication = $rootScope.authentication || {};
	
					var roles = [];
					if (sessionStorage.roles) {
						roles = sessionStorage.roles.split(',');
					}
	
					$rootScope.authentication.name   = sessionStorage.userName || '';
					$rootScope.authentication.roles = roles;
					$rootScope.authentication.hasRole = function (role) {
						var roles = role.split(',');
						return _.intersection(this.roles, roles).length > 0;
					}
				},
				'save': function (accessToken, refreshToken, name, roles, clientId) {
					sessionStorage.accessToken  = accessToken  || '';
					sessionStorage.refreshToken = refreshToken || '';
					sessionStorage.userName     = name         || '';
					sessionStorage.roles        = roles        || '';
					sessionStorage.clientId     = clientId     || '';
				},
				'checkRole': function (role) {
	
				},
				'login': function () {
					var that = this;	
	
					var userName = that.arguments.tenant ?
						that.arguments.tenant + ":" + that.arguments.user :
						that.arguments.user;
					var data = 
						'grant_type=password&username=' + userName + 
						'&password=' + that.arguments.password +
						(that.app.clientId ? '&client_id=' + that.app.clientId : '') +
						(that.app.clientSecret ? '&client_secret=' + that.app.clientSecret : '');
					var headers = { 'Content-Type': 'application/x-www-form-urlencoded' };
	
					that.connect();
					xpCommunication
						.post(that.apiUrl, that.id, data, headers)
						.success(angular.bind(this, this.success), function (data) {
							that.save(data.access_token, data.refresh_token, data["as:name"], data["as:roles"], data["as:client_id"]);
							that.clearInbound();
	
							window.location.href = "";
						})
						.error(angular.bind(this, this.error));
				},
				'refreshToken': function () {
					var deferred = $q.defer();
					var that = this; // this is not $scope
	
					if (sessionStorage.accessToken && sessionStorage.refreshToken) {
						var data = "grant_type=refresh_token&refresh_token=" + sessionStorage.refreshToken + "&client_id=" + sessionStorage.clientId;
						var headers = { 'Content-Type': 'application/x-www-form-urlencoded' };
	
						xpCommunication
							.post($rootScope.app.apiBaseUrl + "token", "", data, headers)
							.success(function (data) {
								that.save(data.access_token, data.refresh_token, data["as:name"], data["as:roles"], data["as:client_id"]);
								that.clearInbound();
	
								deferred.resolve(data);
							})
							.error(function (data) {
								deferred.reject(data);
							});
					}
					return deferred.promise;
				},
				'register': function () {
					var deferred = $q.defer();
					var that = this;	
		
					var params = angular.extend({}, that.arguments);
	
					that.connect();
					xpCommunication
						.post(that.apiUrl, that.id, params, null, that.files)
						.success(that.success.bind(that), function (data) {
							that.clearInbound();
							
							deferred.resolve(data);
						})
						.error(function(data, status, headers, config) {
							that.error(data, status, headers, config);
							deferred.reject();
						});
					return deferred.promise;
				},
				'forgotPassword': function () {
					var that = this;	
		
					var params = angular.extend({}, that.arguments);
	
					that.connect();
					xpCommunication
						.post(that.apiUrl, that.id, params, null, that.files)
						.success(that.success.bind(that), function (data) {
						
							that.clearInbound();
							window.location.href = '/Account/ConfirmForgotPassword';
							
						})
						.error(that.error.bind(that));
				},
				'confirmForgotPassword': function () {
					var that = this;	
	
					//that.arguments.userId = getURLParameter("userid");
					//that.arguments.code = getURLParameter("code");
					//that.arguments.email = getURLParameter("email");
	
					var params = angular.extend({}, that.arguments);
	
					that.connect();
					xpCommunication
						.post(that.apiUrl, that.id, params, null, that.files)
						.success(that.success.bind(that), function (data) {
							that.clearInbound();
	
							window.location.href = "/";
						})
						.error(that.error.bind(that));
				},
				'changePassword': function () {
					var that = this;
	
					var params = angular.extend({}, that.arguments);				
	
					that.connect();
					xpCommunication
					.post(that.apiUrl, that.id, params, null, that.files)
					.success(that.success.bind(that), function (data) {
						that.clearInbound();
	
						window.location.href = "/";
					})
					.error(that.error.bind(that));
				},
				'updatePassword': function () {
					var that = this;
	
					var params = angular.extend({}, that.arguments);
									
					that.connect();
					xpCommunication
					.post(that.apiUrl, that.id, params, null, that.files)
					.success(that.success.bind(that), function (data) {
						that.clearInbound();					
						window.location.href = "#/Account/Profile";
					})
					.error(that.error.bind(that));
				},
				'updatePin': function () {
					var that = this;
	
					var params = angular.extend({}, that.arguments);
	
					that.connect();
					xpCommunication
					.post(that.apiUrl, that.id, params, null, that.files)
					.success(that.success.bind(that), function (data) {
						that.clearInbound();								
						window.location.href = "/";
					})
					.error(that.error.bind(that));
				},
				'userSetting': function () {
					var that = this;
	
					var params = angular.extend({}, that.arguments);
	
					that.connect();
					xpCommunication
					.post(that.apiUrl, that.id, params, null, that.files)
					.success(that.success.bind(that), function (data) {
						that.clearInbound();
	
						window.location.href = "/Account/UserSetting";
					})
					.error(that.error.bind(that));
				},
				'checkPin': function () {
					var that = this;
	
					var params = angular.extend({}, that.arguments);
	
					that.connect();
					xpCommunication
					.get(that.apiUrl, that.id, params, null, that.files)
					.success(that.success.bind(that), function (data) {
						that.clearInbound();
	
						/*Enviar al banco*/
						window.location.href = "/";
					})
					.error(that.error.bind(that));
				},
				'logout': function () {
					this.save();
					this.load();
	
					window.location.href = '#/Account/Login';
				},
	
				'confirmEmail': function () {
					var that = this;
	
					//that.arguments = {
						//userId: getURLParameter("userid"),
						//code: getURLParameter("code"),
					//that.arguments.email = getURLParameter("email");
					//};
				
					var params = angular.extend({}, that.arguments);
	
					that.connect();
					xpCommunication
					.post(that.apiUrl, that.id, params, null, that.files)
					.success(that.success.bind(that), function (data) {
						that.clearInbound();
						window.location.href = "/";
					})
					.error(that.error.bind(that));
				},
				'acceptassignment': function () {
					var params = angular.extend({}, that.arguments);
	
					that.connect();
					xpCommunication
					.post(that.apiUrl, that.id, params, null, that.files)
					.success(that.success.bind(that), function (data) {
						that.clearInbound();
						window.location.href = "/";
					})
					.error(that.error.bind(that));
			}
			};
		}
	])
	.factory('xpInterceptorService', ['$rootScope', '$injector', 'xpInitialScope', '$q', function ($rootScope, $injector, xpInitialScope, $q) {
		return {
			'request' : function (config) {
				config.headers = config.headers || {};

				if (sessionStorage.accessToken) {
					config.headers.Authorization = 'Bearer ' + sessionStorage.accessToken;
				}

				return config;
			},
			'responseError' : function (rejection) {
				var deferred = $q.defer();
				if (rejection.status === 401) {
					var xpAuthentication = angular.extend({}, xpInitialScope, $injector.get('xpAuthentication'));

					xpAuthentication.refreshToken().then(
						function () {
							xpAuthentication.retry(rejection.config, deferred);
						},
						function () {
							window.location.href = "#/Account/Login";
							deferred.reject(rejection);
						}
					);
				} else {
					deferred.reject(rejection);
				}
				return deferred.promise;
			}
		};
	}])
	.directive('xpLogin', function () {
		return {
			restrict: 'AE',
			priority: 100,
			scope: true,
			controller: ['$scope', '$attrs', 'xpInitialScope', 'xpAuthentication', function ($scope, $attrs, xpInitialScope, xpAuthentication) {
				// Initialize scope
				angular.extend($scope, xpInitialScope, xpAuthentication);
				$scope.initialize();
				$scope.apiUrl = $scope.app.apiBaseUrl + $attrs.api
					.replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
					.replace("//", "/");
			}]
		};
	})
	.directive('xpRegister', function () {
		return {
			restrict: 'AE',
			priority: 100,
			scope: true,
			controller: ['$scope', '$attrs', 'xpInitialScope', 'xpAuthentication', function ($scope, $attrs, xpInitialScope, xpAuthentication) {
				// Initialize scope
				angular.extend($scope, xpInitialScope, xpAuthentication);
				$scope.initialize();
				$scope.apiUrl = $scope.app.apiBaseUrl + $attrs.api
					.replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
					.replace("//", "/");
			}]
		};
	})
	.directive('xpForgotPassword', function () {
		return {
			restrict: 'AE',
			priority: 100,
			scope: true,
			controller: ['$scope', '$attrs', 'xpInitialScope', 'xpAuthentication', function ($scope, $attrs, xpInitialScope, xpAuthentication) {
				// Initialize scope
				angular.extend($scope, xpInitialScope, xpAuthentication);
				$scope.initialize();
				$scope.apiUrl = $scope.app.apiBaseUrl + $attrs.api
					.replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
					.replace("//", "/");
			}]
		};
	})
	.directive('xpConfirmForgotPassword', function () {
		function getURLParameter(name) {
			return decodeURIComponent((new RegExp('[?|&]' + name + '=([^&;]+?)(&|#|;|$)').exec(location.search) || [, ""])[1]
				//.replace(/\+/g, '%20')
				) || null
		}

				return {
			restrict: 'AE',
			priority: 100,
			scope: true,
			controller: ['$scope', '$attrs', 'xpInitialScope', 'xpAuthentication', function ($scope, $attrs, xpInitialScope, xpAuthentication) {
				// Initialize scope
				angular.extend($scope, xpInitialScope, xpAuthentication);
				$scope.initialize();
				$scope.arguments.userId = getURLParameter("userid");
				$scope.arguments.code = getURLParameter("code");
				$scope.arguments.email = getURLParameter("email");
				$scope.apiUrl = $scope.app.apiBaseUrl + $attrs.api
					.replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
					.replace("//", "/");
			}]
		};
	})
	.directive('xpUpdatePassword', function () {
		return {
			restrict: 'AE',
			priority: 100,
			scope: true,
			controller: ['$scope', '$attrs', 'xpInitialScope', 'xpAuthentication', function ($scope, $attrs, xpInitialScope, xpAuthentication) {
				// Initialize scope
				angular.extend($scope, xpInitialScope, xpAuthentication);
				$scope.initialize();
				$scope.apiUrl = $scope.app.apiBaseUrl + $attrs.api
					.replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
					.replace("//", "/");
			}]
		};
	})
	.directive('xpUpdatePin', function () {
		return {
			restrict: 'AE',
			priority: 100,
			scope: true,
			controller: ['$scope', '$attrs', 'xpInitialScope', 'xpAuthentication', function ($scope, $attrs, xpInitialScope, xpAuthentication) {
				// Initialize scope
				angular.extend($scope, xpInitialScope, xpAuthentication);
				$scope.initialize();
				$scope.apiUrl = $scope.app.apiBaseUrl + $attrs.api
					.replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
					.replace("//", "/");


			
            
			}]
		};
	})
	.directive('xpChangePassword', function () {
		return {
			restrict: 'AE',
			priority: 100,
			scope: true,
			controller: ['$scope', '$attrs', 'xpInitialScope', 'xpAuthentication', function ($scope, $attrs, xpInitialScope, xpAuthentication) {
				// Initialize scope
				angular.extend($scope, xpInitialScope, xpAuthentication);
				$scope.initialize();
				$scope.apiUrl = $scope.app.apiBaseUrl + $attrs.api
					.replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
					.replace("//", "/");
			}]
		};
	})
	.directive('xpLogout', function () {
		return {
			restrict: 'AE',
			priority: 100,
			scope: true,
			controller: ['$scope', 'xpInitialScope', 'xpAuthentication', function ($scope, xpInitialScope, xpAuthentication) {
				// Initialize scope
				angular.extend($scope, xpInitialScope, xpAuthentication);
				$scope.initialize();
			}]
		};
	})
	.directive('xpConfirmEmail', function () {
		function getURLParameter(name) {
			return decodeURIComponent((new RegExp('[?|&]' + name + '=([^&;]+?)(&|#|;|$)').exec(location.search) || [, ""])[1]
				//.replace(/\+/g, '%20')
				) || null
		}
		return {
			restrict: 'AE',
			priority: 100,
			scope: true,
			controller: ['$scope', '$attrs', 'xpInitialScope', 'xpAuthentication', function ($scope, $attrs, xpInitialScope, xpAuthentication) {
				// Initialize scope
				angular.extend($scope, xpInitialScope, xpAuthentication);
				$scope.initialize();
				$scope.arguments.userId = getURLParameter("userid");
				$scope.arguments.code = getURLParameter("code");
				$scope.arguments.email = getURLParameter("email");
				$scope.apiUrl = $scope.app.apiBaseUrl + $attrs.api
					.replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
					.replace("//", "/");
			}]
		};
	})
	.directive('xpCheckPin', function () {
	function getURLParameter(name) {
			return decodeURIComponent((new RegExp('[?|&]' + name + '=([^&;]+?)(&|#|;|$)').exec(location.search) || [, ""])[1]
				//.replace(/\+/g, '%20')
				) || null
		}		
		return {
			restrict: 'AE',
			priority: 100,
			scope: true,
			controller: ['$scope', '$attrs', 'xpInitialScope', 'xpAuthentication', function ($scope, $attrs, xpInitialScope, xpAuthentication) {
				// Initialize scope
				angular.extend($scope, xpInitialScope, xpAuthentication);
				$scope.initialize();
				$scope.arguments.pin = getURLParameter("pin");				
				$scope.apiUrl = $scope.app.apiBaseUrl + $attrs.api
					.replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
					.replace("//", "/");
			}]
		};
	})
	.directive('xpAcceptAssignment', function () {
		return {
			restrict: 'AE',
			priority: 100,
			scope: true,
			controller: ['$scope', 'xpInitialScope', 'xpAuthentication', function ($scope, xpInitialScope, xpAuthentication) {
				// Initialize scope
				angular.extend($scope, xpInitialScope, xpAuthentication);
				$scope.initialize();
			}]
		};
	})
;