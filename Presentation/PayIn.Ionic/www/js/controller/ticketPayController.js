angular
    .module('app')
    .controller('ticketPayController', 
                ['$scope', '$ionicPopup', '$ionicHistory','$cordovaDialogs','$state','xpPost','$rootScope',
                 function($scope, $ionicPopup, $ionicHistory,$cordovaDialogs, $state,xpPost, $rootScope) {


                     $scope.accept2 = function(pin) {
                         var scope = $scope.$new();
                         angular.extend(scope, xpPost);
                         scope.apiUrl = $scope.app.apiBaseUrl + "Mobile/ticket/v3/pay";
                         scope.args = scope.arguments;
                         scope.arguments = {
                             "id": scope.arguments.id,
                             "operationId": scope.arguments.operationId,
                             "paymentMedias": scope.arguments.paymentMedias,
                             "promotions": scope.arguments.promotions,
                             "pin": scope.arguments.pin
                         };
                         scope.accept()
                             .then(function() {
                             /*$ionicPopup.alert({
                                 title: 'Pago realizado',
                                 template:
                                 '<div>Pago realizado con éxito. Gracias por usar Pay[in]</div>',
                                 okType: 'button-payin'
                             })
                                 .then(function() {*/
                                 scope.apiUrl = $scope.app.apiBaseUrl + "Mobile/TransportOperation/v1/Recharge";
                                 scope.id = scope.arguments.operationId;
                                 scope.arguments = scope.args;
                                 scope.accept()
                                     .then(function(data) {
                                     if (data) {
                                         $scope.xpNfc.execute(data.scripts[0].card, data.scripts[0].keys, data.scripts[0].script, data.operation, scope.temp.slot);
                                         $state.go('transportCardRecharge' );
                                     }
                                 //})
                             });
                         });
                     };

                     $scope.pay = function() {
                         var scope = $scope.$new();
                         angular.extend(scope, xpPost);
                         scope.apiUrl = $scope.app.apiBaseUrl + "Mobile/TransportOperation/v1/Recharge";
                         scope.arguments = {
                             "operationId": scope.arguments.operationId,
                             "ticketId": scope.arguments.id,
                             "paymentMedias": scope.arguments.paymentMedias,
                             "promotions": scope.arguments.promotions
                         };
                         scope.accept().then(function(data) {
                             if (data) {
                                 alert("Todo correcto");
                             }});
                     };

                     $scope.showTerms = function(){
                         if($scope.temp.showTerm == 1){
                             $scope.temp.showTerm = 0
                         }else{
                             $scope.temp.showTerm = 1
                         }
                     }

                     $scope.prePay = function() {
                         var scope = $scope.$new();
                         angular.extend(scope, xpPost);

                         //scope.apiUrl = $scope.app.apiBaseUrl + "Mobile/TransportOperation/v1/Recharge";0

                         scope.args = scope.arguments;
                         scope.arguments = {
                             "ticketId": scope.args.id,
                             "cardId": scope.temp.cardId,
                             "cardType": scope.temp.cardType,
                             "operationId": scope.temp.operationId,
                             "script": scope.temp.cardScript,
                             "code": scope.temp.code, 
                             "quantity": scope.temp.quantity,
                             "priceId": scope.temp.priceId,
                             "imei": scope.platform.device ? scope.platform.device.imei : "",
                             "rechargeType": scope.temp.rechargeType,
                             "promotions": scope.arguments.promotions
                         };
                         angular.extend(scope.arguments, scope.args)
                         scope.arguments.device = scope.arguments.device || {};
                         angular.extend(scope.arguments.device, scope.platform.device);

                         $state.go('ticketpay',{                             
                             "args": scope.arguments,
                             "ticketId":scope.arguments.id,
                             "paymentMedias": scope.arguments.paymentMedias,
                             "promotions": scope.arguments.promotions});
                     };

                 }
                ]
               );