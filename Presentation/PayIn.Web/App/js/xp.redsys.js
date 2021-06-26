'use strict';
angular
	.module('xpRedsys', [])
	.service('xpRedsys', [
		function () {
			return {
				xpRedsys: {
				}
			};
		}
	])
	.directive('xpRedsys', ['$rootScope', '$http', 'xpRedsys', function ($rootScope, $http, xpRedsys) {
		return {
			restrict: 'E',
			replace: true,
			scope: false,
			template:
                '<div>' +
				    '<iframe frameborder="0" width="100%" style="height: 80vh; display: none;" name="bankFrame" id="bankFrame"></iframe>' +
				    '<form style="display:none" name="formPost" id="formPost" target="bankFrame"></form>' +
                '</div>',
			link: function ($scope, element, attrs) {
				angular.extend($scope, xpRedsys);
			}
		};
	}])
	;