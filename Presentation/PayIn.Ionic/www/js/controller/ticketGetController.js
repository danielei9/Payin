angular
.module('app')
.controller('ticketGetController', ['$scope', '$ionicPopup', '$ionicHistory',
    function($scope, $ionicPopup, $ionicHistory) {
        $scope.showNoTicket = function(data) {
            if (data.data.length == 0) {
                $ionicPopup.alert({
                    title: 'Cargar Ticket',
                    template: '<div>El ticket no es accesible o ya no se puede pagar.</div>',
                    okType: 'button-payin'
                })
                .then(function() {
                    $ionicHistory.goBack();
                });
            }
        };
    }
]);