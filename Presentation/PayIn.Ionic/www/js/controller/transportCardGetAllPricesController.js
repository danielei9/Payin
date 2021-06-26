angular
.module('app')
.controller('transportCardGetAllPricesController', ['$scope', 'transportCardGetAllRecharges',
    function($scope, transportCardGetAllRecharges) {       
        angular.extend($scope, transportCardGetAllRecharges);
    }
]);