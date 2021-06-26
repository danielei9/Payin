angular.module('xp.communication', [])
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
					.success(function (data, status, headers, config) {
						if (successFuncs)
							angular.forEach(successFuncs, function (func) {
								func.call(this, data, status, headers, config);
							});
					})
					.error(function (data, status, headers, config) {
						if (errorFuncs)
							angular.forEach(errorFuncs, function (func) {
								func.call(this, data, status, headers, config);
							});
					});
				return handler;
			}
		};
	})
	.factory('xpInitialScope', [function () {
		return {
			'initialize': function () {
				this.clearInbound();
			},
			'clearInbound': function () {
				this.id = "";
				this.arguments = {};
				this.autofile = {};
				this.files = {};
				this.temp = {};
				//this.$emit('clearInbound');
			},
			'clearOutbound': function () {
                this.data = [];
				this.count = 0;
				this.currentPage = 1;
				this.errors.length = 0;
				//this.$emit('clearOutbound');
			},
			'apiUrl': {},
			// Inbound arguments
			'id': "",
			'arguments': {},
			'argumentSelect': {},
			'temp': {},
			'autofile': {},
			'files': {},
			// Outbound arguments
			'data': [],
			'count': 0,
			'currentPage': 1,
			'errors': [],
			// State
			'windowState': function () {
				this._windowState = this._windowState || {
					'isReadOnly': false,
					'isBusy': 0,
				};
				return this._windowState;
			},
			'enable': function () {
				this.windowState().isReadOnly = false;
			},
			'disable': function () {
				this.windowState().isReadOnly = true;
			},
			// Connection methods
			'connect': function () {
				this.errors.length = 0;
				this.windowState().isBusy = this.windowState().isBusy + 1;
			},
			'success': function (result) {
				this.windowState().isBusy = this.windowState().isBusy - 1;
			},
			'error': function (result) {
				var that = this;

				var message = !(result || result.data) ?
					"Unknown" :
					result.data.error_description || // Login error
					result.data.exceptionMessage ||
					result.data.message;
				console.log(message);
				that.errors.push(message);

				if (result.modelState) {
					angular.forEach(result.modelState, function (values) {
						angular.forEach(values, function (value) {
							console.log(value);
							that.errors.push(value);
						});
					});
				}
				that.windowState().isBusy = that.windowState().isBusy - 1;
			},
			'clearErrors': function () {
				this.errors.length = 0;
			},
			// Path arguments
			'setValue': function (path, value) {
				var item = this;
				var roles = path.split('.');
				for (var i = 0; i < roles.length - 1; i++)
					item = item[roles[i]];
				item[roles[roles.length - 1]] = value;
			}
		};
	}])
	.factory('xpCommunication', ['$http', 'xpResponseHandler', function ($http, xpResponseHandler) {
		var locals = {
			'putParam': function (url, name, value) {
				if (!value)
					return url;

				var existInUrl = 0;

				var dirs = url.split("/");
				for (var i = 0; i < dirs.length; i++) {
					if (dirs[i] == ":" + name) {
						dirs[i] = value;
						existInUrl = 1;
					}
				}

				if ((name === "id") && (existInUrl === 0))
					return url + "/" + value;

				return dirs.join("/");
			}
		};
		var Url = function (url, id) {
			if (!id)
				return url;
			else if (url.indexOf(":id") == -1)
				return url + "/" + id;
			else {
				return url.replace(":id", id);
			}
		}
		var Data = function (data) {
			if (typeof data != 'string') {
				angular.forEach(data, function (value, key) {
					if (value === undefined) {
						delete data[key];
					}
				});
			}
			return data;
		}

		return {
			'retry': function (config, deferred) {
				$http(config).then(
					function (response) {
						deferred.resolve(response);
					},
					function (response) {
						deferred.reject(response);
					}
				);
			},
			'get': function (url, param, data) {
				var urlParams = "";
				angular.forEach(Data(data), function (value, name) {
					urlParams += (urlParams === "" ?
						"?" + encodeURIComponent(name) + "=" + encodeURIComponent(value) :
						"&" + encodeURIComponent(name) + "=" + encodeURIComponent(value)
					);
				})

				return xpResponseHandler.httpResponseHandler($http({
					'method': 'GET',
					'url': Url(url, param) + urlParams
				}));
			},
			'put': function (url, param, data, headers, files) {
				var url = locals.putParam(url, "id", param);

				var length = 0;
				if (files)
					length = Object.keys(files).length;
				if (length == 0) {
					return xpResponseHandler.httpResponseHandler($http({
						'method': 'PUT',
						'url': url,
						'headers': headers,
						'data': Data(data)
					}));
				} else {
					var formData = new FormData();
					if (data)
						formData.append("data", new Blob([angular.toJson(Data(data))], { type: "application/json" }));
					angular.forEach(files, function (value, key) {
						formData.append(key, value);
					});

					return xpResponseHandler.httpResponseHandler($http({
						'method': 'PUT',
						'url': url,
						'headers': { 'Content-Type': undefined },
						'transformRequest': angular.identity,
						'data': formData
					}));
				}
			},
			'putFile': function (url, param, data, headers, file) {
				var formData = new FormData();
				//if (data)
				//	formData.append("data", new Blob([angular.toJson(data)], { type: "application/json" }));
				formData.append("Content", file || "");

				return xpResponseHandler.httpResponseHandler($http({
					'method': 'PUT',
					'url': Url(url, param),
					'headers': { 'Content-Type': undefined },
					'transformRequest': angular.identity,
					'data': formData
				}));
			},
			'post': function (url, param, data, headers, files) {
				var length = 0;
				if (files)
					length = Object.keys(files).length;
				if (length == 0) {
					return xpResponseHandler.httpResponseHandler($http({
						'method': 'POST',
						'url': Url(url, param),
						'headers': headers,
						'data': Data(data)
					}));
				} else {
					var formData = new FormData();
					if (data)
						formData.append("data", new Blob([angular.toJson(data)], { type: "application/json" }));
					angular.forEach(files, function (value, key) {
						formData.append(key, value);
					});

					return xpResponseHandler.httpResponseHandler($http({
						'method': 'POST',
						'url': Url(url, param),
						'headers': { 'Content-Type': undefined },
						'transformRequest': angular.identity,
						'data': formData
					}));
				}
			},
			'delete': function (url, param, data, headers) {
				return xpResponseHandler.httpResponseHandler($http({
					'method': 'DELETE',
					'url': Url(url, data['detailId'] ? data['detailId'] : param),
					'data': data,
					'headers': headers
				}));
			}
		}
	}])
;