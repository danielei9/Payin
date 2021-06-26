/* global QRCode */
/* global StatusBar */

angular.module('app', ['ionic','ionic.service.core','ionic.service.push', 'ngCordova', 'xp', 'ngMessages'])
.run(function($ionicPlatform, $rootScope) {
  $ionicPlatform.ready(function() {
    // Hide the accessory bar by default (remove this to show the accessory bar above the keyboard
    // for form inputs)
    if(window.cordova && window.cordova.plugins && window.cordova.plugins.Keyboard) {
      cordova.plugins.Keyboard.hideKeyboardAccessoryBar(true);
    }
    if(window.StatusBar) {
      StatusBar.styleDefault();
    }
  });
  $rootScope.app =
  {
    //apiBaseUrl: 'http://localhost:8080/', // La baseURL es la raíz
    //apiBaseUrl: 'http://172.31.56.23:8080/', // La baseURL es la raíz
    apiBaseUrl: 'http://payin-test.cloudapp.net/',
    //apiBaseUrl: 'https://control.pay-in.es/',
    clientId: 'PayInAndroidNativeApp',
    clientSecret: 'PayInAndroidNativeApp@123456'
  };
})
.config(function($stateProvider, $urlRouterProvider) {
  $stateProvider
    .state('app1',                   { url: '' ,                                         templateUrl: 'templates/ticket/getAll.html',            controller:'ActiveRegisterPushToken'})
    .state('app2',                   { url: '/',                                         templateUrl: 'templates/ticket/getAll.html',            controller:'ActiveRegisterPushToken'})
    .state('paymentmediagetall',     { url: '/PaymentMedia',                             templateUrl: 'templates/paymentMedia/getAll.html'})
    .state('paymentmediaselectcard', { url: '/PaymentMedia/Select?{ticketId:\d*}',       templateUrl: 'templates/paymentMedia/selectCard.html',  params: { 'paymentMedias': []}})
    .state('paymentmediacreate',     { url: '/PaymentMedia/create',                      templateUrl: 'templates/paymentMedia/create.html'})
    .state('accountlogin',           { url: '/Account/Login',                            templateUrl: 'templates/account/login.html'})
    .state('ticketget',              { url: '/Ticket/{id:\d*}',                          templateUrl: 'templates/ticket/get.html',               controller: 'QRCtrl'})
    .state('paymentmediaTickets',    { url: '/PaymentMedia/Tickets',                     templateUrl: 'templates/ticket/getAll.html'})
    .state('paymentmediaHasPayment', { url: '/PaymentMedia/CreatePaymentUser',           templateUrl: 'templates/paymentMedia/createPaymentUser.html'})
    .state('paymentmediaConfirmation',{ url: '/PaymentMedia/confirmation',               templateUrl: 'templates/paymentMedia/confirmation.html'})
    .state('accountregister',        { url: '/Account/Register',                         templateUrl: 'templates/account/register.html'})
    .state('accountconfirmemail',    { url: '/Account/ConfirmEmail',                     templateUrl: 'templates/account/confirmEmail.html'})
    .state('updatepassword',         { url: '/Account/UpdatePassword',                   templateUrl: 'templates/account/updatePassword.html'})
    .state('updatepin',              { url: '/Account/UpdatePin',                        templateUrl: 'templates/account/updatePin.html'})
    .state('policy',                 { url: '/Policy',                                   templateUrl: 'templates/policy/policy.html'})
    .state('accountprofile',         { url: '/Account/Profile',                          templateUrl: 'templates/account/profile.html'})
    .state('otherwise',              { url: '/otherwise',                                templateUrl: 'templates/ticket/getAll.html'})
    .state('ticketpay' ,             { url: '/Ticket/Pay/{id:\d*}?{paymentMediaId:\d*}', templateUrl: 'templates/ticket/pay.html'})
  $urlRouterProvider.otherwise('/Account/Login');
})
.controller('LoadingCtrl', function($scope, $ionicLoading) {
  $scope.show = function() {
    $ionicLoading.show({
      template: 'Cargando...'
    })
  };
  $scope.hide = function(){
    $ionicLoading.hide();
  }
})
.controller('BarcodeCtrl', function($scope,$state,$ionicPlatform, $cordovaBarcodeScanner) {
  $scope.scanBarcode = function() {
      $cordovaBarcodeScanner.scan().then(function(imageData) {
          //Comprobar si es un QR de Payin
          $scope.CheckPayIn(imageData.text);
      }, function(error) {
          console.log("An error happened -> " + error);
      });
    /*$scope.scanBarcode = function() {
      $scope.CheckPayIn("pay[in]/ticket:{\"id\":17}");
    };*/
    $scope.CheckPayIn = function (str) {
      //pay[in]/ticket:{"id":1029}
      var split = str.split("/");
      var cmd = split[1].split(":");
      if(split[0]=="pay[in]" && cmd[0]=="ticket"){
        var json =cmd[1]+":"+cmd[2];
        var obj = JSON.parse(json);
        $state.go("ticketget", { id: obj.id }); 
      }
      else alert("No es un código reconocible de la app");  
    };
  }
})
.controller("QRCtrl",function($scope) {
    $scope.$on('$ionicView.loaded',(function(){
        var split = location.hash.split('/');
        var id = split[split.length-1];
        new QRCode(document.getElementById("qrcode"), "pay[in]/ticket:{\"id\":"+id+"}");
    }));
})
.controller('SelectCardCtrl', function($scope, $state) {
  $scope.ticketpay = function(id) {
    $state.go("ticketpay",{ id: $scope.temp.ticketId, paymentMediaId: id})
  }
})
.controller("PayTicketCtrl",function($scope, $state, $cordovaDialogs) {
  $scope.pay = function(hasPayment, paymentMedia) {
    if ($scope.arguments.hasPayment) {
      $state.go("paymentmediaselectcard",{ ticketId: $scope.arguments.id, paymentMedias : $scope.arguments.paymentMedias})
    } else {
      var msg = "Previamente a poder crear tarjetas debe definir su usuario para utilizar pagos";
      var tit = "Se requiere Atención";
      var buttonArray = ['CERRAR','CONFIRMAR'];
      
      $cordovaDialogs.confirm(msg, tit, buttonArray)
      .then(function(buttonIndex) {
        // no button = 0, 'CERRAR' = 1, 'CONFIRMAR' = 2
        var btnIndex = buttonIndex;
        if(btnIndex==2)
            $state.go("paymentmediaHasPayment")
      });
    }
  };
})
.controller("hasPaymentCtrl",function($scope, $state, $ionicPopup) {})
.factory('TokenService', function() {
  return {
      token : null
  };
})
.controller("ActiveRegisterPushToken",function($scope,$ionicPush,TokenService) {
  $ionicPush.init({
    "debug": true,
    "onNotification": function(notification) {
      var payload = notification.payload;
      console.log(notification, payload);
      navigator.notification.alert(notification.text,"");
    },
    "onRegister": function(data) {
      console.log(data.token);
      TokenService.token = data.token;
    }
  });
  if(TokenService.token==null){
    $ionicPush.register();
  }
});
