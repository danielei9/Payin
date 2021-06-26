angular.module('app')
.controller('plugin', function($scope,cordovaPlugin) {
                    
  $scope.LaunchPlugin = function () {
    cordovaPlugin.showToast('prueba de toast');
  }; 
   
});