angular
.module('app')
.factory('paymentConcessionCampaignGetAllFactoryFactory', ['xpInitialScope', function (xpInitialScope) {
    return {
        'vinculate': function() {
            var that = this;
                            
            that.apiUrl = that.app.apiBaseUrl + "Mobile/PaymentConcessionCampaign/v1/AcceptAssignment"
                .replace("{0}", (that.app.tenant ? "/" + that.app.tenant : ""))
                .replace("//", "/");
            
            that.accept()
            .then(function() {
                that.$parent.search();
            });
        },
        'reject': function() {
            var that = this;
            
            that.apiUrl = that.app.apiBaseUrl + "Mobile/PaymentConcessionCampaign/v1/RejectAssignment"
                .replace("{0}", (that.app.tenant ? "/" + that.app.tenant : ""))
                .replace("//", "/");
            
            that.accept()
            .then(function() {
                that.$parent.search();
            });
        }
    };
}])
.controller('paymentConcessionCampaignGetAllController',
    ['$scope', '$ionicActionSheet', '$timeout', 'xpPut', 'paymentConcessionCampaignGetAllFactoryFactory',
    function($scope, $ionicActionSheet, $timeout, xpPut, paymentConcessionCampaignGetAllFactoryFactory) {
        $scope.accept2 = function() {
            var scope = $scope.$new();
            angular.extend(scope, xpPut, paymentConcessionCampaignGetAllFactoryFactory);
            scope.id = scope.item.id;
            scope.goBack = 0;
            
            if (scope.item.state == 1) { // Active
                var hideActive = $ionicActionSheet.show({
                    titleText : 'Vinculación a campañas',
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
                    titleText : 'Vinculación a campañas',
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
                }, 2000); // 2s
            }
        };
    }
]);