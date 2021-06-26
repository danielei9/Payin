/// <reference path="../../../typings/angularjs/angular.d.ts"/>
angular
.module('app')
.controller('mainErrorController', ['$scope', '$ionicPopup', function($scope, $ionicPopup) {
//  $ionicModal.fromTemplateUrl('modalErrorModal.html',
//    function(modal) {
//      $scope.modal = modal;
//    },
//    {
//      scope: $scope,
//      animation: 'slide-in-up',
//      focusFirstInput: true
//    }
//  );
//        
//  $scope.showError = function() {
//    $scope.modal.show();
//  };
//  $scope.hideError = function() {
//    $scope.modal.hide();
//  };

  $scope.$on('error', function(event, args) {
    if (args && args.length) {
      $ionicPopup.alert({
        title: 'Error',
        template: args[0]
      });
    }
  });
}]);