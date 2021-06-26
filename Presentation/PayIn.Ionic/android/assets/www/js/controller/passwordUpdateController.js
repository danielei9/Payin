angular
.module('app')
.controller('passwordUpdateController', ['$scope','$ionicPopup', '$state',
  function($scope, $ionicPopup, $state) {
    $scope.confirm = function() {
     $ionicPopup.alert({
          title: 'Se requiere atención',
          template:
            '<div>Contraseña actualizada correctamente</div>',
          okType: 'button-payin'
        });
    };
   
  }
]);