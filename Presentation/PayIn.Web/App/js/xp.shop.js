'use strict';
angular.module('xp.shop', ['app', 'xp', 'xp.communication', 'textAngular', 'ngImgCrop', 'ngSanitize', 'ui.select', 'ngDialog'])
.config(['$stateProvider', '$urlRouterProvider',
	function ($stateProvider, $urlRouterProvider) {
		$urlRouterProvider
			.otherwise('');

		$stateProvider
			// Shop 
			.state('shopbyconcession', { url: '/Shop/ByConcession/{id}', templateUrl: 'Shop/ByConcession' })
			;
	}
])
;