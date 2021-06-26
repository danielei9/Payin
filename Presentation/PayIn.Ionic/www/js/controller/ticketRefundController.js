angular
.module('app')
.controller('ticketRefundController', ['$scope', '$ionicPopup', '$ionicHistory',
    function($scope, $ionicPopup, $ionicHistory) {
        $scope.accept2 = function(item, args) {
            $scope.hasCommercePayment= function() {
                var commerce = false;
                var roles = $scope.authentication.roles;
                var index = roles.indexOf("CommercePayment");
                if (index != -1)
                    commerce = true;
                return commerce;
            };
            $scope.hasPaymentWorker= function() {
                var worker = false;
                var roles = $scope.authentication.roles;
                var index = roles.indexOf("PaymentWorker");
                if (index != -1)
                    worker = true;
                return worker;
            };
            if($scope.hasCommercePayment() || $scope.hasPaymentWorker()) {
                var myPopup = $ionicPopup.prompt({
                    title:"Devoluciones",
                    template: 'Para proceder con la devolución del pago de '+ item.userName+' con un importe de '+item.amount+'€ debe introducir su PIN de Pay[in]',
                    inputType: 'password',
                    inputPlaceholder: 'Introduce tu código PIN',
                    cancelType:'button',
                    okType: 'button-payin'
                });
                myPopup.then(function(pin) {
                   if(pin !== undefined && pin !== ""){
                        $scope.arguments.id = $scope.item.id;
                        $scope.arguments.pin = pin;
                        $scope.accept()
                            .then(
                                function() {
                                    $ionicPopup.alert({
                                        title: 'Devolución realizada',
                                        template: '<div>Devolución realizada con éxito. Gracias por usar Pay[in]</div>',
                                        okType: 'button-payin'
                                    })
                                }
                            );
                    }
                   else if(pin !== undefined && pin === "") {
                        $ionicPopup.alert({
                            title: 'Error',
                            template: 'Debes introducir un código PIN.',
                            okType: 'button-payin'
                        });
                    }
                });
            }
        };
    }
]);