(function () {
	'use strict';

	angular.
		module('app').
		config(['$stateProvider', provider]);

	function provider($stateProvider) {
		$stateProvider.state({ name: 'myAccount', url: '/myAccount', templateUrl: '/app/templates/myAccount.html' });
	}
})();