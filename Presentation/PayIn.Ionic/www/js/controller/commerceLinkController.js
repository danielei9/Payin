angular
    .module('app')
    .controller('commerceLinkController', ['$scope','$ionicActionSheet', '$state', 'xpPut', function($scope, $ionicActionSheet, $state, xpPut) {
        $scope.accept2 = function(id, state) {
            var scope = $scope.$new();
            angular.extend(scope, xpPut);
            scope.id = id;
            scope.goBack = 0;
            
            function generate(state){
                switch(state){

                    case "1":
                        return [{text: "Bloquear"}, {text: "Rechazar"}];
                    case "2": 
                        return [{text:"Vincularse"}, {text: "Bloquear"}, {text: "Rechazar"}];
                    case "4": 
                        return [{text:"Vincularse"}, {text: "Rechazar"}];
                }
            }
            function generateWorker(state){
                switch(state){
                    case "1":
                        return [{text: "Rechazar"}];
                    case "2": 
                        return [{text:"Vincularse"}, {text: "Rechazar"}];
                }
            }

            function asignateFuncion(state){
                switch(state){
                    case "1":
                        return function(index) {
                            if(index === 0){
                                scope.apiUrl = scope.app.apiBaseUrl + "Mobile/PaymentUser/v1/Block"
                                    .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                                    .replace("//", "/");
                                scope.accept()
                                    .then(function() {
                                        $scope.search();
                                    });
                            }
                            if(index === 1){
                                scope.apiUrl = scope.app.apiBaseUrl + "Mobile/PaymentUser/v1/RejectAssignment"
                                    .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                                    .replace("//", "/");
                                scope.accept()
                                    .then(function() {
                                        $scope.search();
                                    });
                            }       
                            return true;
                        };
                    case "2":                       
                        return function(index) {
                            if(index === 0){
                                scope.apiUrl = scope.app.apiBaseUrl + "Mobile/PaymentUser/v1/AcceptAssignment"
                                    .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                                    .replace("//", "/");
                                scope.accept()
                                    .then(function() {
                                        $scope.search();
                                    });
                            }
                            if(index === 1){
                                scope.apiUrl = scope.app.apiBaseUrl + "Mobile/PaymentUser/v1/Block"
                                    .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                                    .replace("//", "/");
                                scope.accept()
                                    .then(function() {
                                        $scope.search();
                                    });
                            }
                            if(index === 2){
                                scope.apiUrl = scope.app.apiBaseUrl + "Mobile/PaymentUser/v1/RejectAssignment"
                                    .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                                    .replace("//", "/");
                                scope.accept()
                                    .then(function() {
                                        $scope.search();
                                    });
                            }   
                            return true;
                        };
                    case "4":
                        return function(index) {
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
                                scope.apiUrl = scope.app.apiBaseUrl + "Mobile/PaymentUser/v1/RejectAssignment"
                                    .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                                    .replace("//", "/");
                                scope.accept()
                                    .then(function() {
                                        $scope.search();
                                    });
                            }
                            return true;
                        };
                }
            }

            function asignateWorkerFuncion(state){
                switch(state){
                    case "1":
                        return function(index) {
                            if(index === 0){
                                scope.apiUrl = scope.app.apiBaseUrl + "Mobile/PaymentWorker/v1/RejectAssignment"
                                    .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                                    .replace("//", "/");
                                scope.accept()
                                    .then(function() {
                                    $scope.search();}
                                         );
                            }
                            return true;
                        };
                    case "2":                       
                        return function(index){
                            if(index === 0){
                                scope.apiUrl = scope.app.apiBaseUrl + "Mobile/PaymentWorker/v1/AcceptAssignment"
                                    .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                                    .replace("//", "/");
                                scope.accept()
                                    .then(function() {
                                        $scope.search();
                                    });
                            }
                            if(index === 1){
                                scope.apiUrl = scope.app.apiBaseUrl + "Mobile/PaymentWorker/v1/RejectAssignment"
                                    .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                                    .replace("//", "/");
                                scope.accept()
                                    .then(function() {
                                        $scope.search();
                                    });
                            }
                            return true;
                        };
                }
            }

            $scope.hasPaymentWorker= function(){
                var hasPaymentWorker = false;
                var roles = $scope.authentication.roles;
                var index = roles.indexOf("PaymentWorker");
                if (index != -1)
                    hasPaymentWorker =  true;
                return hasPaymentWorker;
            };

            $scope.hasPaymentUser= function(){
                var hasPaymentUser = false;
                var roles = $scope.authentication.roles;
                var index = roles.indexOf("PaymentUser");
                if (index != -1)
                    hasPaymentUser =  true;
                return hasPaymentUser;
            };

            $scope.showWorker = function(buttons, buttonClicked) {
                $ionicActionSheet.show({
                    buttons: buttons,
                    titleText: 'Vinculación a empresa.',
                    cancelText: 'Cancelar',
                    cancel: function() { },
                    buttonClicked: buttonClicked
                });
            };

            $scope.showUser = function(buttons, buttonClicked) {

                $ionicActionSheet.show({
                    buttons: buttons,
                    titleText: 'Vinculación a empresa.',
                    cancelText: 'Cancelar',
                    cancel: function() { },
                    buttonClicked: buttonClicked
                });
            };
            if(!$scope.hasPaymentWorker())
                $scope.showUser(
                    generate(state),
                    asignateFuncion(state)
                );
            else
                $scope.showWorker(
                    generateWorker(state),
                    asignateWorkerFuncion(state)
                );
        };
    }
]);