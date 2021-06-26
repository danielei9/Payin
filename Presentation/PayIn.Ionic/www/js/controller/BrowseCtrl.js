angular.module('app')
.controller('BrowseCtrl', function($scope, $ionicSideMenuDelegate, $cordovaGeolocation){

  $ionicSideMenuDelegate.canDragContent(false)
  $scope.map = {center: {latitude: 40.1451, longitude: -99.6680 }, zoom: 8 };
  $scope.options = {scrollwheel: true};
  $scope.markers = []
  // get position of user and then set the center of the map to that position
  $cordovaGeolocation
    .getCurrentPosition()
    .then(function (position) {
      var lat  = position.coords.latitude
      var long = position.coords.longitude
      $scope.map = {center: {latitude: lat, longitude: long}, zoom: 16 };
      //just want to create this loop to make more markers
      for(var i=0; i<3; i++) {
        $scope.markers.push({
            id: $scope.markers.length,
            latitude: lat + (i * 0.002),
            longitude: long + (i * 0.002),
            title: 'm' + i
        })
      }

    }, function(err) {
      // error
    });
});