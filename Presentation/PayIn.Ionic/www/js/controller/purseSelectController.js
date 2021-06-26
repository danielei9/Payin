angular
.module('app')
.controller('purseSelectController', ['$scope','$state' ,
  function($scope,$state) {
      $scope.active = false;
    $scope.pressButton = function() {
            if($scope.active ===  true){
                document.getElementById("walletButton").style.backgroundColor = "#16c3f1";
                $scope.active = false;
            } else {
                
                document.getElementById("walletButton").style.backgroundColor = "#E9AF30";
                $scope.active = true;
            }
    };
  }]
);