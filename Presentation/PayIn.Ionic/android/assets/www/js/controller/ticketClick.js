angular
.module('app')
.controller('ticketClickController', function($scope,$state) {
  $scope.tap = function(item) {
    $state.go("ticketget", { id: item });
  }
});