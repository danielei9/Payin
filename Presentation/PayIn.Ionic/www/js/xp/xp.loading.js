angular
.module('xp.loading', [])
.service('xpLoading', ["$ionicLoading",
    function($ionicLoading) {
        return {
            show: function(text) {
                $ionicLoading.show({
                    template: text || 'Cargando...'
                })
            },
            hide: function() {
                $ionicLoading.hide();
                
                // No precargar el texto de PAGADO antes de saber la condicion.
                var aux = document.getElementById('getTicketPayed');
                if (aux != null)
                    aux.style.display = "inherit";
            }
        };
    }
])
.directive('xpLoading', function () {
    return {
        restrict: 'A',
        controller: ['$scope', 'xpLoading',
            function($scope, xpLoading) {
                angular.extend($scope, xpLoading);
            }
        ]
    }
});