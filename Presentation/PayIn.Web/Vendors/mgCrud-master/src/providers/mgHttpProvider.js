﻿(function (module, undefined) {
    
    function mgHttpProvider() {
        var defaultConfig;
        this.setDefaultConfig = function (config) {
            defaultConfig = config;
        }
        this.$get = ['$http', function (http) {                               
            return mgHttp(http);
        }]

        function mgHttp(http) {
            var forEach = angular.forEach, service = {}, extend = angular.extend, isFunction = angular.isFunction;
            
            function resolve(action,self, response) {
                forEach(action, function (value) {
                    if (isFunction(value)) {
                        (response) ? value.call(self,response) : value.call(self);
                    }
                });
            }
            function resolveMethod(name) {
                return (name === 'query') ? 'get' : name;
            }
            function resolveData(name, data) {
                return (name === 'query' || name==='jsonp') ? { params: data } : { data: data };
            }
            function resolveUrl(config, path) {
                var url = resolveConfig(config).url;
                return (url) ? url + path || '' : path || '';
            }
            function resolveConfig(config) {
                return config || defaultConfig || {};
            }
            function resolveAdditionalConfig(config) {
                return resolveConfig(config).additionalConfig || {};
            }
            function createResponse(data, status, headers, config,statusText) {
                return { data: data, status: status, headers: headers, config: config,statusText:statusText };
            }
            function runService(config, before, success, error,transform,self) {
                resolve(before,self);
                http(config).
                   success(function (data, status, headers, config, statusText) {
                       var newdata;
                       if (transform) {
                           newdata = transform.fn.call(self, data, transform.expression);
                       }
                       resolve(success, self, createResponse(newdata || data, status, headers, config, statusText));
                   }).
                   error(function (data, status, headers, config, statusText) {
                       resolve(error, self,createResponse(data, status, headers, config, statusText));
                   });
            }
            function createShortMethod() {
                forEach(arguments, function (name) {
                    service[name] = function (config, before, sucess, error,transform) {
                        return function (path, self) {
                            runService(extend({ method: name, url: resolveUrl(config, path) }, resolveAdditionalConfig(config)), before, sucess, error,transform, self);
                        };
                    };
                });
            }
            function createShortMethodWithData() {
                forEach(arguments, function (name) {
                    service[name] = function (config, before, sucess, error,transform) {
                        return function (path, data, self) {
                            runService(extend(extend({ method: resolveMethod(name), url: resolveUrl(config, path) }, resolveData(name, data)), resolveAdditionalConfig(config)), before, sucess, error,transform, self);
                        };
                    };
                });
            }
            createShortMethod('get', 'delete');
            createShortMethodWithData('post', 'put', 'query', 'patch','jsonp');

            return service;
        }
    };

    module.provider('mgHttp', mgHttpProvider);

})(angular.module('mgCrud'));
