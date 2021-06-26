angular
    .module('app')
    .controller('serviceNotificationController', ['$scope','$ionicActionSheet', '$state', 'xpPut', function($scope, $ionicActionSheet, $state, xpPut) {
        /*$scope.openPaymentUser = function(id, message) {
            var scope = $scope.$new();
            angular.extend(scope, xpPut);
            scope.id = id;
            scope.goBack = 0;
            $scope.hasPaymentWorker= function(){
                var hasPaymentWorker = false;
                var roles = $scope.authentication.roles;
                var index = roles.indexOf("PaymentWorker");
                if (index != -1 && roles.length > 1)
                    hasPaymentWorker =  true;
                return hasPaymentWorker;
            }

            $scope.hasPaymentUser= function(){
                var hasPaymentUser = false;
                var roles = $scope.authentication.roles;
                var index = roles.indexOf("User");
                if (roles.length == 1)
                    hasPaymentUser =  true;
                return hasPaymentUser;
            }
            $scope.hasCommercePayment= function(){
                var hasCommercePayment = false;
                var roles = $scope.authentication.roles;
                var index = roles.indexOf("CommercePayment");
                if (index != -1 && roles.length > 1)
                    hasCommercePayment =  true;
                return hasCommercePayment;
            }

            $scope.showWorker = function() {


                $ionicActionSheet.show({
                    buttons: [
                        { text: 'Vincularse' },
                        {text: 'Rechazar'},
                    ],
                    titleText: 'Vinculación a empresa.',
                    cancelText: 'Cancelar',
                    cancel: function() {
                    },
                    buttonClicked: function(index) {
                        //Vincular == 0 Rechazar == 1
                        if(index === 0){
                            scope.apiUrl = scope.app.apiBaseUrl + "Mobile/PaymentWorker/v1"
                                .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                                .replace("//", "/");
                            scope.accept()
                                .then(function() {
                                $scope.search();}
                                     );
                        }
                        if(index === 1){
                            //v1/RejectAssignment/{id:int}
                            scope.apiUrl = scope.app.apiBaseUrl + "Mobile/PaymentWorker/v1/RejectAssignment"
                                .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                                .replace("//", "/");
                            scope.accept()
                                .then(function() {
                                $scope.search();}
                                     );
                        }
                        return true;
                    }
                });
            }
            $scope.showUser = function() {
                $ionicActionSheet.show({
                    buttons: [
                        { text: 'Vincularse' },
                        { text: 'Bloquear' },
                        {text: 'Rechazar'},
                    ],
                    titleText: 'Vinculación a empresa.',
                    cancelText: 'Cancelar',
                    cancel: function() {
                    },
                    buttonClicked: function(index) {
                        //Vinculares == 0 Bloquear == 1
                        if(index === 0){
                            scope.apiUrl = scope.app.apiBaseUrl + "Mobile/PaymentUser/v1/AcceptAssignment"
                                .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                                .replace("//", "/");
                            scope.accept()
                                .then(function() {
                                $scope.search();}
                                     );
                        }
                        if(index === 1){
                            scope.apiUrl = scope.app.apiBaseUrl + "Mobile/PaymentUser/v1/Block/"
                                .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                                .replace("//", "/");
                            scope.accept()
                                .then(function() {
                                $scope.search();}
                                     );
                        }
                        if(index === 2){
                            //v1/RejectAssignment/{id:int}
                            scope.apiUrl = scope.app.apiBaseUrl + "Mobile/PaymentUser/v1/RejectAssignment"
                                .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                                .replace("//", "/");
                            scope.accept()
                                .then(function() {
                                $scope.search();}
                                     );
                        }                    
                        return true;
                    }
                });
            }

            if( !$scope.hasPaymentWorker() && !$scope.hasCommercePayment()){
                $scope.showUser();} 
            
            else {
                    if($scope.hasPaymentWorker()){$scope.showWorker()} 
                    else {}
            }
            
        }; */
        
        $scope.redirectFunction = function (referenceClass){
            
            switch(referenceClass){
                case 'PaymentUser':
                    $state.go('paymentUserService');
                    break;
                case 'PaymentWorker':
                    $state.go('paymentWorkerService');
                    break;
            }
        };
    }]);