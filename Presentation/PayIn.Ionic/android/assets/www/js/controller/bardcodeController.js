
angular.module('app')
.controller('BarcodeCtrl', function($scope, $cordovaBarcodeScanner) {
  $scope.scan = function() {
          $cordovaBarcodeScanner.scan().then(function(imageData) {
              alert(imageData.text);
              //("Barcode Format -> " + imageData.format);
              //console.log("Cancelled -> " + imageData.cancelled);
          }, function(error) {
              //console.log("An error occurred -> " + error);
              alert("An error occurred. Try Again!");
          });
      };
/*  document.addEventListener("deviceready", function () {

    $cordovaBarcodeScanner
      .scan()
      .then(function(barcodeData) {
        // Success! Barcode data is here
        alert(barcodeData);
      }, function(error) {
        alert("An error occurred. Try again!");
      });
  }, false);*/
});