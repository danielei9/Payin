angular
    .module('app')
    .controller('ticketGetAllController', 
                ['$rootScope', '$scope', '$ionicPlatform', '$ionicSideMenuDelegate', 'xpCommunication', '$state','$http',
                 function($rootScope, $scope, $ionicPlatform, $ionicSideMenuDelegate, xpCommunication, $state, $http) {
                     ionic.DomUtil.ready(function() {
                         var date = new Date().getHours();
                         if (localStorage.oldDate == null || localStorage.oldDate == "") {
                             localStorage.oldDate = date;
                         }

                         if (localStorage.oldDate <= date) {
                             localStorage.oldDate = date + 6;

                             var api = "mobile/main/v1";
                             var apiUrl = $scope.app.apiBaseUrl + api
                             .replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
                             .replace("//", "/");
                             xpCommunication
                                 .get(apiUrl, "", {})
                                 .success(function (data) {
                                 if (!$scope.authentication.hasRole('CommercePayment')) {
                                     if (data.tickets.length === 0) {
                                         $scope.goCards();
                                     }
                                 }
                             })
                                 .error($scope.error.bind($scope));
                         }
                     });

                     $scope.openLeft = function() {
                         $ionicSideMenuDelegate.toggleLeft();
                     };
                     $scope.hasCommercePayment = function(){
                         var commerce = false;
                         var roles = $scope.authentication.roles;
                         var index = roles.indexOf("CommercePayment");
                         if (index != -1)
                             commerce =  true;
                         return commerce;
                     };
                     $scope.hasPaymentWorker= function(){
                         var worker = false;
                         var roles = $scope.authentication.roles;
                         var index = roles.indexOf("PaymentWorker");
                         if (index != -1)
                             worker =  true;
                         return worker;
                     };

                     $scope.goCards = function() {
                         var api = "mobile/user/v1/hasPayment";
                         var apiUrl = $scope.app.apiBaseUrl + api
                         .replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
                         .replace("//", "/");
                         xpCommunication
                             .get(apiUrl, "", {})
                             .success(function (data) {
                             if (data.hasPayment) {
                                 if($scope.data.length === 0){
                                     $state.go('wizardUser');
                                 }else {
                                     $state.go('paymentmediagetall');
                                 }
                             } else {
                                 $state.go('wizard');
                             }
                         })
                             .error($scope.error.bind($scope));
                     };
                 }                        
                ])