<script src="~/Vendors/angulr/vendor/angular/angular.js"></script>
angular
    .module('Store')
    .controller('shopGetAllConcessionsController', ['$http', '$scope', function ($http, $scope) {
        $http.get('/Api/Shop').then(function (data) {
            $scope.events = data.data;
        });
    }])