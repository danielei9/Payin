angular
.module('app')
.controller('passwordConfirmForgotController', ['$scope','$ionicPopup', '$state',
  function($scope, $ionicPopup, $state) {
    $scope.confirm = function() {
        $scope.arguments.confirmPassword = $scope.arguments.password;
        $scope.confirmForgotPassword()
        .then(function() {
         $ionicPopup.alert({
              title: 'Se requiere atención',
              template:
                '<div>Contraseña actualizada correctamente</div>',
              okType: 'button-payin'
              });
          });  
    };
  }
]);