angular
.module('app')
.controller('accountLoginController', ['$scope','xpAuthentication', function($scope, xpAuthentication) {
  $scope.login2 = function() {  
    $scope.login()
    .then(function() {
      // Add to signalR
      //$scope.startNotification();
    });
  };
}]);