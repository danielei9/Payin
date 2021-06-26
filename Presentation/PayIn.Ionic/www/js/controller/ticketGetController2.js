angular
    .module('app')
    .controller('ticketGetController2', 
                ['$scope', '$ionicPopup', '$ionicHistory','$cordovaDialogs','$state','xpGet','$rootScope', '$ionicSlideBoxDelegate', 'xpInitialScope',
                 function($scope, $ionicPopup, $ionicHistory,$cordovaDialogs, $state,xpGet, $rootScope, $ionicSlideBoxDelegate, xpInitialScope) {
                     $scope.codigoDescuento = function(){
                         var scope = $scope.$new();
                         angular.extend(scope, xpInitialScope, xpGet);
                         scope.apiUrl = $scope.app.apiBaseUrl + "Mobile/Promotion/Check"
                             .replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
                             .replace("//", "/");
                         $ionicPopup.prompt({
                             title: 'Código de Descuento',
                             template: 'Introduce tu código',
                             inputType: 'text',
                             inputPlaceholder: 'Código de Descuento',
                             okType: 'button-payin'
                         }).then(function(res) {
                             // $('#descuentoCheck').css('opacity', 1);
                             if(res != undefined){
                                 $scope.arguments.promotionCode = res;
                                 $scope.arguments.promotionCodeType = 0;
                                 scope.arguments = {
                                     "code": $scope.arguments.promotionCode,
                                     "promotionCodeType": $scope.arguments.promotionCodeType
                                 };
                                 scope.search()
                                     .then(function(data) {
                                     $scope.arguments.promotions = data.data;                                       
                                     $scope.arguments.promotionsCopy = angular.copy($scope.arguments.promotions);                                  
                                 })
                             }
                         });
                     }
                     $scope.deletePromotions = function(index){
                          $scope.arguments.promotions.splice(index, 1);

                          if($scope.arguments.promotionsCopy[index].class=="deleted")
                             $scope.arguments.promotionsCopy[index].class=""
                         else
                         $scope.arguments.promotionsCopy[index].class="deleted";
                     }
                     $scope.selectCreditCard = function(index){
                        $scope.arguments.paymentMedias.length = 0;
                         var payments = $scope.arguments.paymentMediasCopy[index];
                         $scope.arguments.paymentMedias.push(payments);
                     }
                     $scope.selectPurse = function(){   
                     }
                 }]);