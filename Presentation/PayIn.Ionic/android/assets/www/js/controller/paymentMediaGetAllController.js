angular
.module('app')
.controller('paymentMediaGetAllController', ['$scope','$state', '$ionicActionSheet', '$timeout', '$ionicPopup', 'xpDelete', 
  function($scope,$state, $ionicActionSheet, $timeout, $ionicPopup, xpDelete) {
    $scope.delete = function(item) {
      var scope = $scope.$new();
      angular.extend(scope, xpDelete);
      scope.apiUrl = scope.app.apiBaseUrl + item.api
        .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
        .replace("//", "/");
      scope.id = item.id;
      scope.goBack = 0;
                
      var hideSheet = $ionicActionSheet.show({
        destructiveText: 'Borrar',
        titleText: 'Tarjeta',
        destructiveButtonClicked: function() {
          $ionicPopup.confirm({
            title: 'Borrar',
            template: '<div>¿Desea borrar la tarjeta ' + item.text + '?</div>',
            okType: 'button-payin'
          })
          .then(function() {
            scope.accept()
            .then(function() {
              $scope.search();
            });
          });
          return true;
        }
      });
    
      $timeout(function() {
        hideSheet();
      }, 2000);
    };
  }]
);