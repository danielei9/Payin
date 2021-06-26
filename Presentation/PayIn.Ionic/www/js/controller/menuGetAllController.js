angular
.module('app')
.controller('menuGetAllController', ['$rootScope', '$scope', '$ionicPlatform', '$ionicSideMenuDelegate', 'xpCommunication', '$state', '$http', '$ionicPopup', '$ionicHistory',
    function($rootScope, $scope, $ionicPlatform, $ionicSideMenuDelegate, xpCommunication, $state, $http, $ionicPopup,$ionicHistory) {
       
          $ionicHistory.clearHistory();
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
                $state.go('paymentmediagetall');
            })
                .error($scope.error.bind($scope));
        };      
        $scope.transportClicked = function() {
            $scope.capabilities.checkNfcStatus()
            .then(function(status) {
                if ($scope.xpNfc.emulate) {
                    $state.go('transportCardGet', { cardId: $scope.xpNfc.cardId });
                } else if (
                    (status === $scope.capabilities.NFC_OK) ||
                    (status === $scope.capabilities.NFC_NO)
                ) {
                    $state.go('transportCardGetAll');
                // } else if (status === $scope.capabilities.NFC_NO) {
                //     // $ionicPopup.alert({
                //     //     title: 'NFC no compatible',
                //     //     template:
                //     //         '<div>' +
                //     //         '<p>Este teléfono tiene NFC no compatible. En unos días daremos servicio a tu teléfono. Gracias</p>' +
                //     //         '</div>',
                //     //     okType: 'button-payin'
                //     // });
                //      $ionicPopup.alert({
                //         title: 'NFC error',
                //         template:
                //             '<div>' +
                //             '<p>Este teléfono no tiene NFC. En unos días daremos servicio a tu teléfono. Gracias</p>' +
                //             '</div>',
                //         okType: 'button-payin'
                //     });
               } else if (status === $scope.capabilities.NFC_DISABLED) {
                    $ionicPopup.alert({
                        title: 'NFC deshabilitado',
                        template:
                            '<div>' +
                            '<p>Este telefono tiene desabilitado el NFC. Por favor activa el NFC en las opciones de teléfono.</p>' +
                            '</div>',
                        okType: 'button-payin'
                    });
                }
            },
            function(er)
            {
                $ionicPopup.alert({
                    title: 'NFC error',
                    template:
                        '<div>' +
                        '<p>Error' + er + '</p>' +
                        '</div>',
                    okType: 'button-payin'
                });
            });
        }
    }
]);