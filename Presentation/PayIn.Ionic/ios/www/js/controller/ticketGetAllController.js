angular
.module('app')
.controller('ticketGetAllController', ['$scope', '$ionicSideMenuDelegate', 'xpCommunication', '$state',
  function($scope, $ionicSideMenuDelegate, xpCommunication, $state) {
    $scope.openLeft = function() {
      $ionicSideMenuDelegate.toggleLeft();
    };
    $scope.goCards = function() {
      var api = "mobile/user/v1/hasPayment";
      var apiUrl = $scope.app.apiBaseUrl + api
        .replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
        .replace("//", "/");
      xpCommunication
        .get(apiUrl, "", {})
        .success(function (data) {
          if (data.hasPayment) {
            $state.go('paymentmediagetall');
          } else {
            $state.go('paymentmediaHasPayment');
          }
        })
        .error($scope.error.bind($scope));
    }
  }
]);