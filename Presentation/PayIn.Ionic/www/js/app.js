/* global Ionic */
/* global nfc */
/* global Connection */
/* global PushNotification */
/* global ionic */
/* global QRCode */
/* global StatusBar */
/// <reference path="../../typings/cordova/cordova.d.ts"/>
/// <reference path="../../typings/angularjs/angular.d.ts"/>
var db = null;
angular.module('app', ['ionic','ionic.service.core', 'ngCordova',  'ionic.service.push', 'xp', 'xp.nfc', 'ngMessages', 'ionic-datepicker', 'ion-floating-menu'])
    .run(["$ionicPlatform", "$rootScope", "$state", "$timeout", "$http", '$q', '$ionicPopup', '$ionicLoading', 
          function($ionicPlatform, $rootScope, $state, $timeout, $http, $q, $ionicPopup, $ionicLoading) {
              $ionicPlatform.ready(function() {
                  // Hide the accessory bar by default (remove this to show the accessory bar above the keyboard for form inputs)

                  /*var angular = require('angular');
                  var ngTouch = require('angular-touch');
                  var carousel  = require('angular-carousel');*/


                  $rootScope.map = { center: { latitude: 45, longitude: -73 }, zoom: 8 };
                  $timeout(function() {
                      if (navigator.splashscreen)
                          navigator.splashscreen.hide();
                  }, 300);

                  if (navigator.connection) {
                      function checkConnection() {
                          var networkState = navigator.connection.type;
                          var states = {};
                          if (networkState == navigator.connection.NONE) {
                              states[Connection.NONE] = 'No network connection';
                              alert(states[networkState]);
                          }
                      }
                      checkConnection();
                  }

                  if (window.cordova && window.cordova.plugins && window.cordova.plugins.Keyboard) {
                      cordova.plugins.Keyboard.hideKeyboardAccessoryBar(true);
                      //cordova.plugins.Keyboard.disableScroll(true);
                  }
                  if (window.StatusBar) {
                      StatusBar.styleDefault();
                  }
                  $ionicPlatform.onHardwareBackButton(function($scope) {
                      var state = location.hash;
                      if (state == "#/Account/Login" || state == "#/" || state == "") {
                          console.log("HardBackButton event");
                          navigator.app.exitApp();
                      }
                  });


                  // NFC Capabilities
                  $rootScope.capabilities = {
                      NFC_OK: "NFC_OK",
                      NFC_NO: "NO_NFC",
                      NFC_DISABLED: "NFC_DISABLED",
                      nfcStatus: "NO_NFC",
                      checkNfcStatus: function() {
                          var deferred = $q.defer();

                          if (typeof nfc !== "undefined") {
                              nfc.getNfcStatus(function(status) {
                                  console.log("NFC status " + status);
                                  $rootScope.capabilities.nfcStatus = status;

                                  deferred.resolve($rootScope.capabilities.nfcStatus);
                              });
                          } else {
                              deferred.resolve($rootScope.capabilities.nfcStatus);
                          }

                          return deferred.promise;
                      }
                  };
                  $rootScope.capabilities.checkNfcStatus();

                  //Plugin SQLite
                  //db = $cordovaSQLite.openDB("my.db");
                  //Plugin backgroundMode.
                  //cordova.plugins.backgroundMode.enable();
              });

              var deviceInfo = {};
              if (window.plugins) {
                  window.plugins.sim.getSimInfo(successCallback, errorCallback);

                  function successCallback(result) {
                      // console.log(result);  
                      deviceInfo = {
                          model: ionic.Platform.device().model,
                          platform :  ionic.Platform.device().platform,
                          uuid: ionic.Platform.device().uuid,
                          version:  ionic.Platform.device().version,
                          manufacturer: ionic.Platform.device().manufacturer,
                          serial: ionic.Platform.device().serial,
                          operator:  result.carrierName,                
                          imei: result.deviceId, 
                          suscriberId: result.subscriberId
                      };  
                      $rootScope.platform.device = deviceInfo;

                      window.MacAddress.getMacAddress(
                          function(macAddress) { deviceInfo.mac = macAddress; },
                          function(fail) { console.log(fail); }
                      );
                      $rootScope.platform.device = deviceInfo;
                  }

                  function errorCallback(error) {
                      console.log(error);
                  };
              }

              $rootScope.platform = {                   
                  current: ionic.Platform.platform(),
                  android: ionic.Platform.isAndroid(),
                  ios: ionic.Platform.isIOS() || ionic.Platform.isIPad(),
                  wp: ionic.Platform.isWindowsPhone(),
                  webview: ionic.Platform.isWebView()
              };
              // App Constants//
              $rootScope.app = {

                  //apiBaseUrl: 'http://localhost:8080/',
                  //apiBaseUrl: 'http://188.79.244.186:8080/',
                  //apiBaseUrl: 'http://192.168.1.131:8080/',
                  apiBaseUrl: 'http://payin-test.cloudapp.net/',
                  //apiBaseUrl: 'http://payin-homo.cloudapp.net/',
                  //apiBaseUrl: 'https://control.pay-in.es/',
                  version:
                  //'v4.1.0',
                  //'Homologación h6.1.18',
                  'Test',
                  configuration:
                  //'production',
                  //'homologation',
                  'test',
                  clientId: 'PayInAndroidNativeApp',
                  clientSecret: 'PayInAndroidNativeApp@123456'
              };

              $rootScope.alert = function(title, template) {
                  $ionicPopup.alert({
                      title: title,
                      template: template,
                      okType: 'button-payin'
                  });
              };
          }
         ])
    .config(['$stateProvider', '$urlRouterProvider', function($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('accountconfirmemail', { url: '/Account/ConfirmEmail', templateUrl: 'templates/account/confirmEmail.html' })
            .state('accountlogin', { url: '/Account/Login', templateUrl: 'templates/account/login.html' })
            .state('accountprofile', { url: '/Account/Profile', templateUrl: 'templates/account/profile.html' })
            .state('accountregister', { url: '/Account/Register', templateUrl: 'templates/account/register.html' })
            .state('ticketGetAll', { url: '/ticketGetAll', templateUrl: 'templates/ticket/getAll.html' })
            .state('app1', { url: '', templateUrl: 'templates/menu/getAll.html', controller: 'ActiveRegisterPushToken' })
            .state('app2', { url: '/', templateUrl: 'templates/menu/getAll.html', controller: 'ActiveRegisterPushToken' })
            .state('mainGetEntailments', { url: '/main/entailments', templateUrl: 'templates/main/getEntailments.html' })
            .state('paymentmediaConfirmation', { url: '/PaymentMedia/confirmation', templateUrl: 'templates/paymentMedia/confirmation.html' })
            .state('paymentmediacreate', { url: '/PaymentMedia/create', templateUrl: 'templates/paymentMedia/create.html' })
            .state('paymentmediagetall', { url: '/PaymentMedia', templateUrl: 'templates/paymentMedia/getAll.html' })
            .state('paymentmediaHasPayment', { url: '/PaymentMedia/HasPayment', templateUrl: 'templates/paymentMedia/createPaymentUser.html' })
            .state('paymentmediaselectcard', { url: '/PaymentMedia/Select?{ticketId:\d*}', templateUrl: 'templates/paymentMedia/selectCard.html', params: { 'paymentMedias': [] } })
            .state('paymentmediaTickets', { url: '/PaymentMedia/Tickets', templateUrl: 'templates/ticket/getAll.html' })
            .state('ticketget', { url: '/Ticket/{id:\d*}', templateUrl: 'templates/ticket/get.html', controller: 'QRCtrl', params: {data: [],id: 0 , cardId: 0, cardType: '', cardScript: '', operationId: 0, code: '', quantity:'','operationType': '', priceId: '', slot:'', rechargeType: ''}})
            .state('ticketcreate', { url: '/Ticket/create', templateUrl: 'templates/ticket/create.html' })               
            .state('updatepassword', { url: '/Account/UpdatePassword', templateUrl: 'templates/account/updatePassword.html' })
            .state('forgotpassword', { url: '/Account/ForgotPassword', templateUrl: 'templates/account/forgotPassword.html' })
            .state('confirmforgotpassword', { url: '/Account/ConfirmForgotPassword', templateUrl: 'templates/account/confirmForgotPassword.html' })
            .state('updatepin', { url: '/Account/UpdatePin', templateUrl: 'templates/account/updatePin.html' })
            .state('forgotpin', { url: '/Account/ForgotPin', templateUrl: 'templates/account/forgotPin.html' })
            .state('policy', { url: '/Policy', templateUrl: 'templates/policy/policy.html' })
            .state('paymentWorkerService', { url: '/paymentWorker', templateUrl: 'templates/paymentWorker/getAll.html' })
            .state('ticketpay', { url: '/Ticket/Pay/{ticketId:\d*}', templateUrl: 'templates/ticket/pay.html', params: { args: [], 'ticketId': 0, 'paymentMedias': [], promotions: [] } })                             
            .state('serviceNotificationGetAll', { url: '/ServiceNotification', templateUrl: 'templates/serviceNotification/getAll.html' })
            .state('ticketdue', { url: '/TicketDue', templateUrl: 'templates/ticket/due.html' })
            .state('wizard', { url: '/Wizard', templateUrl: 'templates/wizard/wizard.html' })
            .state('wizardUser', { url: '/WizardUser', templateUrl: 'templates/wizard/wizardUser.html' })
            .state('selectPurse', { url: '/SelectPurse?{ticketId:\d*}', templateUrl: 'templates/paymentMedia/selectPurse.html', params: { 'amount': 0, 'paymentMedias': [] } })
            .state('transportCardGet', { url: '/TransportCard?{cardId:\d*}&{cardType:\d*}', templateUrl: 'templates/transportCard/get.html', params: { 'cardId': '', 'cardType': '', 'cardScript': [], 'operationId': '', 'cardData': [] } })
            .state('transportCardGetAll', { url: '/TransportCards', templateUrl: 'templates/transportCard/getAll2.html' })
            .state('transportCardCreate', { url: '/TransportCardCreate', templateUrl: 'templates/transportCard/create.html' })
            .state('transportCardRecharge', { url: '/TransportCardRecharge?{cardId:\d*}&{cardType:\d*}', templateUrl: 'templates/transportCard/recharge.html', params: { 'cardId': '0', 'cardType': 0, 'cardData': [] } })
            .state('transportCardRetry', { url: '/TransportCardRetry', templateUrl: 'templates/transportCard/retry.html' })
            .state('transportCardMap', { url: '/mapa', templateUrl: 'templates/transportCard/map.html' })
            .state('transportCardGetAllPrices', {url: '/prices', templateUrl: 'templates/transportCard/getAllPrices.html', params: { 'temp': [],'prices': [], 'ticket': {}, 'cardId': '', 'cardType': 1, 'cardScript': [], 'operationType': '', priceId: ''}})
            .state('transportCardGetAllCharges', {url: '/titles', templateUrl: 'templates/transportCard/getAllCharges.html', params: { 'charges': [], 'cardId': '', 'cardType': 1, 'cardScript': [], 'operationType': '', priceId: ''}})
            .state('transportCardGetAllSelectTypeTitle', {url: '/titles', templateUrl: 'templates/transportCard/GetAllSelectTypeTitle.html', params: { 'charges': [], 'cardId': '', 'cardType': 1, 'cardScript': [], 'operationType': '', priceId: ''}})
            .state('transportCardGetAllCity', {url: '/City', templateUrl: 'templates/transportCard/getAllCity.html'})      
            .state('transportCardGetAllRecharges', {url: '/Zones', templateUrl: 'templates/transportCard/getAllRecharges.html', params: { 'recharges': [], 'charges': [], 'cardId': '', 'cardType': 1, 'cardScript': []}}) 
            .state('about', { url: '/aboutto', templateUrl: 'templates/about/about.html' })
            .state('check', { url: '/check', templateUrl: 'templates/check/getAll.html' })    
            .state('otherwise', { url: '/otherwise', templateUrl: 'templates/menu/getAll.html' });

        $urlRouterProvider.otherwise('/Account/Login');
    }])
    .controller('BarcodeCtrl', ["$scope", "$state", "$ionicPlatform", "$cordovaBarcodeScanner", "$http", function($scope, $state, $ionicPlatform, $cordovaBarcodeScanner, $http) {
        $scope.scanBarcode = function() {
            $cordovaBarcodeScanner.scan().then(function(imageData) {
                //Comprobar si es un QR de Payin
                $scope.CheckPayIn(imageData.text);
            }, function(error) {
                console.log("An error happened -> " + error);
            });
        };
        $scope.CheckPayIn = function(str) {
            //pay[in]/ticket:{"id":1029}
            var split = str.split("/");
            var cmd = split[1].split(":");
            if (split[0] == "pay[in]" && cmd[0] == "ticket") {
                var json = cmd[1] + ":" + cmd[2];
                try {
                    var obj = JSON.parse(json);
                    $state.go("ticketget", { id: obj.id });
                } catch (ex) { alert("El ticket no existe en el sistema.") }
            }
            else alert("No es un código reconocible de la app");
        };
    }])
    .controller("QRCtrl", ["$scope", "$state", function($scope, $state) {

        $scope.hasCommercePayment = function(){
            var commerce = false;
            var roles = $scope.authentication.roles;
            var index = roles.indexOf("CommercePayment");
            if (index != -1)
                commerce =  true;
            return commerce;
        };
        $scope.hasPaymentWorker= function(){
            var worker = false;
            var roles = $scope.authentication.roles;
            var index = roles.indexOf("PaymentWorker");
            if (index != -1)
                worker =  true;
            return worker;
        };
        var state = $state.href('ticketget', {}, { absolute: true });
        $scope.$on('$ionicView.loaded', (function($scope, $state) {
            var split = state.split('/');
            var id = split[split.length - 1];
            new QRCode(document.getElementById("qrcode"), "pay[in]/ticket:{\"id\":" + id + "}");
        }));
    }])
    .controller("hasPaymentCtrl", ["$scope", "$state", "$ionicPopup", "xpAuthentication", "xpInitialScope", function($scope, $state, $ionicPopup, xpAuthentication, xpInitialScope) {
        $scope.accept2 = function() {
            $scope.accept()
                .then(function() {
                angular.extend($scope, xpInitialScope, xpAuthentication);
                $scope.refreshToken().then(
                    $state.go("wizardUser")
                );
            })
        };
    }])
    .factory('TokenService', function() {
    return {
        token: null
    };
})
    .factory('HasPaymentUser', function() {
    return {
        has: null
    };
})
    .controller('DashCtrl', function($scope) {
    var deploy = new Ionic.Deploy();

    // Update app code with new release from Ionic Deploy
    $scope.doUpdate = function() {
        deploy.update().then(function(res) {
            console.log('Ionic Deploy: Update Success! ', res);
        }, function(err) {
            console.log('Ionic Deploy: Update error! ', err);
        }, function(prog) {
            console.log('Ionic Deploy: Progress... ', prog);
        });
    };
    // Check Ionic Deploy for new code
    $scope.checkForUpdates = function() {
        console.log('Ionic Deploy: Checking for updates');
        deploy.check().then(function(hasUpdate) {
            console.log('Ionic Deploy: Update available: ' + hasUpdate);
            $scope.hasUpdate = hasUpdate;
        }, function(err) {
            console.error('Ionic Deploy: Unable to check for updates', err);
        });
    }
})
    .controller("ActiveRegisterPushToken",
                ["$scope", "$ionicPlatform", "TokenService", "$http", "$cordovaToast",
                 function($scope, $ionicPlatform, TokenService, $http, $cordovaToast) {
                     $ionicPlatform.ready(function() {
                         if ((typeof PushNotification !== 'undefined') && ($scope.platform.android || $scope.platform.ios || $scope.platform.wp)) {
                             var push = PushNotification.init({
                                 android: {
                                     //gcm_sender
                                     senderID: "849435164237"
                                 },
                                 ios: {
                                     alert: "true",
                                     badge: true,
                                     sound: 'false'
                                 },
                                 windows: {}
                             });
                             push.on('notification', function(data) {
                                 console.log("Mensaje: " + data.message);
                                 console.log("Titulo: " + data.title);
                                 console.log("Cantidad: " + data.count);
                                 console.log("Sonido: " + data.sound);
                                 console.log("Imagen: " + data.image);
                                 console.log("Info Adicional: " + data.additionalData);
                                 //alert(data.message);
                                 $cordovaToast.showShortTop(data.message);
                             });
                             push.on('error', function(e) {
                                 console.log(e.message);
                             });
                             push.finish(
                                 function() {
                                     console.log('Finish success');
                                 }, function() {
                                     console.log('Finish error');
                                 }
                             );
                             if (localStorage["pushToken"]) {
                                 TokenService.token = localStorage["pushToken"];
                                 console.log("Token del localStorage:  " + TokenService.token);
                             }
                             else if (TokenService.token == null) {
                                 push.on('registration', function(data) {
                                     console.log(data.registrationId);
                                     TokenService.token = data.registrationId;
                                     localStorage["pushToken"] = data.registrationId;
                                     var api = "mobile/device/v1";
                                     var type = 4;
                                     if ($scope.platform.android) type = 1;
                                     else if ($scope.platform.ios) type = 2;
                                     else if ($scope.platform.wp) type = 3;

                                     var params = {
                                         token: data.registrationId,
                                         type: type,
                                         version: $scope.app.version,
                                         configuration: $scope.app.configuration
                                     };
                                     angular.extend(params, $scope.platform.device);

                                     $http.post($scope.app.apiBaseUrl + api, params)
                                         .then(function(res) {

                                         console.log("Registrado en el servidor el token: " + data.registrationId + " y type: " + type);
                                     });
                                 });
                             }
                         }
                     })
                 }
                ])
    .filter('xpQuantity', function() {
    return function (value, units) {
        if (value && (units === "€")) {
            return value.toFixed(2) + units;
        } else {
            return value + units;
        };
    }
}); 