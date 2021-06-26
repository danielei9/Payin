angular
.module('app')
.controller('accountRegisterController', ['$scope', '$ionicPopup', '$ionicHistory',
  function($scope, $ionicPopup, $ionicHistory) {
    $scope.register2 = function(item) {
      $scope.register()
      .then(function() {
        $ionicPopup.alert({
          title: 'Confirmación email',
          template:
            '<div>' +
            '<p>En breve recibirá un correo electrónico informándole de como continuar el registro</p>' +
            '<p>Si no encuentra este correo en su búzón de entrada verifique la carpeta de correo no deseado.</p>' +
            '<p>Si no lo recibe, puede volver a registrarse para que se le reenvíe el correo de confirmación o ponerse en contacto con system@pay-in.es</p>' +
            '</div>',
          okType: 'button-payin'
        })
        .then(function() {
          $ionicHistory.goBack();
        });
      });
    };
  }]
);