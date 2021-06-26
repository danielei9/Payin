angular
    .module("app", ["ui.router", "ngAnimate"])
    .config(
    ['$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {

            $stateProvider
                .state('concessionEvents', { url: '/Shop/Concession/{id}', templateUrl: '/Shop/Concession' });
        }])
    .controller('shopGetAllConcessionsController', ['$http', '$scope', function ($http, $scope) {
        $http.get('/Api/Shop').then(function (data) {
            $scope.concession = data.data;
        });
    }])
