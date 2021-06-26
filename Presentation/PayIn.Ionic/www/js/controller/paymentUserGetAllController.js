angular
    .module('app')
    .factory('paymentUserGetAllFactoryFactory', [function () {
		return {
            'vinculate': function() {
                var that = this;
                
                that.apiUrl = that.app.apiBaseUrl + "Mobile/PaymentUser/v1/AcceptAssignment"
                    .replace("{0}", (that.app.tenant ? "/" + that.app.tenant : ""))
                    .replace("//", "/");
                
                that.accept()
                    .then(function() {
                        that.$parent.search();
                    });        
            },
            'reject': function() {
                var that = this;
                
                that.apiUrl = that.app.apiBaseUrl + "Mobile/PaymentUser/v1/RejectAssignment"
                    .replace("{0}", (that.app.tenant ? "/" + that.app.tenant : ""))
                    .replace("//", "/");
                
                that.accept()
                .then(function() {
                    that.$parent.search();
                });
            },
            'block': function() {
                var that = this;
                
                that.apiUrl = that.app.apiBaseUrl + "Mobile/PaymentUser/v1/Block"
                    .replace("{0}", (that.app.tenant ? "/" + that.app.tenant : ""))
                    .replace("//", "/");
                
                that.accept()
                .then(function() {
                    that.$parent.search();
                });
            }
		};
	}])
    .controller('paymentUserGetAllController', 
        ['$scope', '$ionicActionSheet', '$timeout', 'xpPut', 'paymentUserGetAllFactoryFactory',
        function($scope, $ionicActionSheet, $timeout, xpPut, paymentUserGetAllFactoryFactory) {
            $scope.accept2 = function() {
                var scope = $scope.$new();
                angular.extend(scope, xpPut, paymentUserGetAllFactoryFactory);
                scope.id = scope.item.id;
                scope.goBack = 0;
                
                if (scope.item.state == 1) { // Active
                    var hideActive = $ionicActionSheet.show({
                        titleText : 'Vinculación de empresa',
                        buttons : [
                            { text: 'Bloquear' }
                        ],
                        destructiveText : 'Rechazar',
                        destructiveButtonClicked : function() {
                            scope.reject();
                        },
                        buttonClicked : function(index) {
                            if (index == 0) scope.block();
                            return true;
                        }
                    });
                    $timeout(function() {
                        hideActive();
                    }, 2000); // 2s
                } else if (scope.item.state == 2) { // Pending
                    var hidePending = $ionicActionSheet.show({
                        titleText : 'Vinculación de empresa',
                        buttons : [
                            { text: 'Aceptar' },
                            { text: 'Bloquear' }
                        ],
                        destructiveText : 'Rechazar',
                        destructiveButtonClicked : function() {
                            scope.reject();
                        },
                        buttonClicked : function(index) {
                            if (index == 0) scope.vinculate();
                            if (index == 1) scope.block();
                            return true;
                        }
                    });
                    $timeout(function() {
                        hidePending();
                    }, 2000); // 2s
                } else if (scope.item.state == 4) { // Blocked
                    var hideBlocked = $ionicActionSheet.show({
                        titleText : 'Vinculación de empresa',
                        buttons : [
                            { text: 'Desbloquear y vincular' }
                        ],
                        buttonClicked : function(index) {
                            if (index == 0) scope.vinculate();
                            return true;
                        }
                    });
                    $timeout(function() {
                        hideBlocked();
                    }, 2000); // 2s
                }
            };
        }
    ]);