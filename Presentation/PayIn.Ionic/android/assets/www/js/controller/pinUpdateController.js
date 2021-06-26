angular
.module('app')
.controller('pinUpdateController', ['$scope','$ionicPopup', '$state',
  function($scope, $ionicPopup, $state) {
    $scope.confirm = function() {
     $ionicPopup.alert({
          title: 'Se requiere atención',
          template:
            '<div>Pay[in] PIN actualizado correctamente</div>',
          okType: 'button-payin'
        });
    };
   
  }
]);