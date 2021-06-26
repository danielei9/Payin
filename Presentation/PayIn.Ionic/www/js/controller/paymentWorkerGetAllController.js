angular
.module('app')
.factory('paymentWorkerGetAllFactoryFactory', ['xpInitialScope', 'xpAuthentication', function (xpInitialScope, xpAuthentication) {
    return {
        'vinculate': function() {
            var that = this;
            var scope = that.$new();
            angular.extend(scope, xpInitialScope, xpAuthentication);
                            
            that.apiUrl = that.app.apiBaseUrl + "Mobile/PaymentWorker/v1/AcceptAssignment"
                .replace("{0}", (that.app.tenant ? "/" + that.app.tenant : ""))
                .replace("//", "/");
            
            that.accept()
            .then(function() {
                var scope = that.$new();
                angular.extend(scope, xpInitialScope, xpAuthentication);

                scope.refreshToken()
                .then(function() {
                    that.$parent.search();
                });
            });
        },
        'reject': function() {
            var that = this;
            var scope = that.$new();
            angular.extend(scope, xpInitialScope, xpAuthentication);
            
            that.apiUrl = that.app.apiBaseUrl + "Mobile/PaymentWorker/v1/RejectAssignment"
                .replace("{0}", (that.app.tenant ? "/" + that.app.tenant : ""))
                .replace("//", "/");
            
            that.accept()
            .then(function() {

                scope.refreshToken()
                .then(function() {
                    that.$parent.search();
                });
            });
        }
    };
}])
.controller('paymentWorkerGetAllController',
    ['$scope', '$ionicActionSheet', '$timeout', 'xpPut', 'paymentWorkerGetAllFactoryFactory',
    function($scope, $ionicActionSheet, $timeout, xpPut, paymentWorkerGetAllFactoryFactory) {       
        $scope.accept2 = function() {
            var scope = $scope.$new();
            angular.extend(scope, xpPut, paymentWorkerGetAllFactoryFactory);
            scope.id = scope.item.id;
            scope.goBack = 0;
            
            if (scope.item.state == 1) { // Active
                var hideActive = $ionicActionSheet.show({
                    titleText : 'Vinculación de trabajadores',
                    destructiveText : 'Rechazar',
                    destructiveButtonClicked : function() {
                        scope.reject();
                    }
                });
                $timeout(function() {
                    hideActive();
                }, 2000); // 2s
            } else if (scope.item.state == 2) { // Pending
                var hidePending = $ionicActionSheet.show({
                    titleText : 'Vinculación de trabajadores',
                    buttons : [
                        { text: 'Aceptar' }
                    ],
                    destructiveText : 'Rechazar',
                    destructiveButtonClicked : function() {
                        scope.reject();
                    },
                    buttonClicked : function(index) {
                        if (index == 0) {
                            scope.vinculate();
                        }
                        return true;
                    }
                });
                $timeout(function() {
                    hidePending();
                }, 10000); // 2s
            }
        };
    }
]);