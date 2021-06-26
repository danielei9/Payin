angular.module('justmoney', ['ui.router', 'xp.communication', 'xp.authentication', 'xp'])
    .controller('AppCtrl', ['$rootScope', function ($rootScope) {
	    $rootScope.app = {
		    version: 'v5.15.07a',

		    apiBaseUrl: '', // La baseURL es la raiz
		    apiSecurityBaseUrl: '', // La baseURL de seguridad es la raiz
            tenant: "",
            selectedPage: ''
	    };
    }])
;