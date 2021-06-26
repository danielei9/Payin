angular
.module('app')
.controller('transportCardCreateController', ['$scope', '$ionicHistory',
    function($scope, $ionicHistory) {
        $scope.execute = function(item) {
            $scope.showLoading("Creando...");
            
            setTimeout(function() {
                $scope.xpNfc
                    .createVC(item.title, item.type, item.uidType || 2, item.perso, item.apps || [])
                    .then(function() {
                        $ionicHistory.goBack();
                        
                        $scope.hideLoading();
                    }, function() {
                        $scope.hideLoading();
                    });
            }, 100);
        };
    }
]);