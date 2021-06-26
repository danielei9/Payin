angular
.module('app')
.controller('ticketPayController', ['$scope', '$ionicPopup', '$ionicHistory',
  function($scope, $ionicPopup, $ionicHistory) {
    $scope.accept2 = function(item) {
      $scope.accept()
      .then(function() {
        $ionicPopup.alert({
          title: 'Pago realizado',
          template:
            '<div>Pago realizado con éxito. Gracias por usar Pay[in]</div>',
          okType: 'button-payin'
        })
        .then(function() {
          $ionicHistory.goBack();
        });
      });
    };
  }]
);