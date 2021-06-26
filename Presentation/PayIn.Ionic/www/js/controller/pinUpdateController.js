angular
.module('app')
.controller('pinUpdateController', ['$scope','$ionicPopup', '$state','xpCommunication',
  function($scope, $ionicPopup, $state,xpCommunication) {
     $scope.confirm = function() {
        $scope.arguments.confirmPin = $scope.arguments.pin;
        if($scope.arguments.password)
          $scope.forgotPin().then(function() {
            $ionicPopup.alert({
                title: 'Se requiere atención',
                    template:
                      '<div>Pay[in] PIN actualizado correctamente</div>',
                    okType: 'button-payin'
                  });
        });
        else
          $scope.updatePin().then(function() {
            $ionicPopup.alert({
                title: 'Se requiere atención',
                    template:
                      '<div>Pay[in] PIN actualizado correctamente</div>',
                    okType: 'button-payin'
                  });
        });
     };
     $scope.checkHasPayment= function(){ 
        var api = "mobile/user/v1/hasPayment";
        var apiUrl = $scope.app.apiBaseUrl + api
          .replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
          .replace("//", "/");
        xpCommunication
          .get(apiUrl, "", {})
          .success(function (data) {
            if (data.hasPayment) {
                $state.go("updatepin");
            } else {
                $state.go('wizard');
            }
          })
          .error($scope.error.bind($scope));      
     };
  }
]);