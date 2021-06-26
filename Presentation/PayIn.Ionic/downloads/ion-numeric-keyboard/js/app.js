// Ionic Starter App

// angular.module is a global place for creating, registering and retrieving Angular modules
// 'starter' is the name of this angular module example (also set in a <body> attribute in index.html)
// the 2nd parameter is an array of 'requires'
angular.module('starter', ['ionic', 'dbaq.ionNumericKeyboard'])

// default ionic run()
.run(function($ionicPlatform) {
  $ionicPlatform.ready(function() {
    // Hide the accessory bar by default (remove this to show the accessory bar above the keyboard
    // for form inputs)
    if(window.cordova && window.cordova.plugins.Keyboard) {
      cordova.plugins.Keyboard.hideKeyboardAccessoryBar(true);
    }
    if(window.StatusBar) {
      StatusBar.styleDefault();
    }
  });
})

// basic routes configuration
.config(function($stateProvider, $urlRouterProvider) {
  $urlRouterProvider.otherwise('/home');

  $stateProvider.state('home', {
    url: '/home',
    templateUrl: 'views/home.html'
  }).state('default', {
    url: '/default',
    templateUrl: 'views/default.html',
    controller: 'PasscodeExampleController'
  }).state('white', {
    url: '/white',
    templateUrl: 'views/white.html',
    controller: 'PasscodeExampleController'
  }).state('slideup', {
    url: '/slideup',
    templateUrl: 'views/slideup.html',
    controller: 'PasscodeExampleController'
  }).state('flat', {
    url: '/flat',
    templateUrl: 'views/flat.html',
    controller: 'PasscodeExampleController'
  }).state('decimal', {
    url: '/decimal',
    templateUrl: 'views/decimal.html',
    controller: 'DecimalExampleController'
  });
})

// example controller
.controller('PasscodeExampleController', ['$scope', '$timeout', '$ionicLoading', function($scope, $timeout, $ionicLoading) {

  $scope.passcode = '';

  $scope.options = {
    rightControl: '<i class="icon ion-backspace-outline"></i></button>',
    onKeyPress: function(value, source) {
      if (source === 'RIGHT_CONTROL') {
        $scope.passcode = $scope.passcode.substr(0, $scope.passcode.length - 1);
      }
      else if (source === 'NUMERIC_KEY') {
        $scope.passcode += value;

        if ($scope.passcode.length == 4) {
          $ionicLoading.show({
            template: 'Verifying passcode ' + $scope.passcode
          });
          $timeout(function() {
            $scope.passcode = '';
            $ionicLoading.hide();
          }, 2000);
        }
      }
    }
  }
}])

// Decimal example controller
.controller('DecimalExampleController', ['$scope', '$timeout', '$ionicLoading', function($scope, $timeout, $ionicLoading) {

  $scope.number = '';

  $scope.options = {
    leftControl: '.',
    rightControl: '<i class="icon ion-close-round"></i></button>',
    onKeyPress: function(value, source) {
      if (source === 'LEFT_CONTROL') {
        if ($scope.number.indexOf('.') === -1) {
          $scope.number += value;
        }
      }
      else if (source === 'RIGHT_CONTROL') {
        $scope.number = $scope.number.substr(0, $scope.number.length - 1);
      }
      else if (source === 'NUMERIC_KEY') {
        $scope.number += value;
      }
    }
  }
}]);
