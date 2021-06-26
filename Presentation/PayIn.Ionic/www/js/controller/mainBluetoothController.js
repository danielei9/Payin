/// <reference path="../../../typings/angularjs/angular.d.ts"/>
angular
.module('app')
.controller('mainBluetoothController', ['$scope', '$ionicPopup', '$ionicHistory',
    function($scope, $ionicPopup, $ionicHistory) {
        $scope.select = function(address, name) {
            $scope.xpSmartband.stopScan();
            
            localStorage.smartBandAddress = address;
            localStorage.smartBandName = name;
              
            $ionicHistory.goBack(-1);
        };
    }
]);