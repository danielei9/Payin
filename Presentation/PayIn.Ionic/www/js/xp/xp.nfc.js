/* global nfc */
angular
    .module('xp.nfc', ['xp'])
    .run(['$ionicPlatform', '$ionicLoading', '$ionicHistory', '$ionicPopup', '$state', '$rootScope', 'xpInitialScope', 'xpQuery', 'xpPost', 'xpPut', '$log', '$q', '$filter',
          function($ionicPlatform, $ionicLoading, $ionicHistory, $ionicPopup, $state, $rootScope, xpInitialScope, xpQuery, xpPost, xpPut, $log, $q, $filter) {
              var CARDTYPE_CLASSIC = 1;
              var CARDTYPE_DESFIRE = 2;
              var CARDTYPE_NFCA = 3;

              var OPERATIONTYPE_CHARGE = 1;
              var OPERATIONTYPE_RECHARGE = 2;
              var OPERATIONTYPE_READ = 3;
              var OPERATIONTYPE_REVOKE = 4;
              var OPERATIONTYPE_CONFIRM = 5;

              function getScript(uid, type,device) {
                  var deferred = $q.defer();

                  var scope = $rootScope.$new();
                  angular.extend(scope, xpInitialScope, xpQuery);
                  scope.apiUrl = scope.app.apiBaseUrl + 'mobile/transportOperation/v1/ReadInfo'
                      .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                      .replace("//", "/");

                  scope.arguments = {
                      mifareClassicCards: type === CARDTYPE_CLASSIC ? uid : '',
                      device : JSON.stringify(device), 
                      isRead: true
                  };
                  angular.extend(scope.arguments, scope.platform.device);
                  scope.goBack = 0;   
                  scope.search()
                      .then(
                      deferred.resolve,
                      deferred.reject
                  );

                  return deferred.promise;
              }
              function getData(uid, type, script, operationId) {
                  var deferred = $q.defer();

                  var scope = $rootScope.$new();
                  angular.extend(scope, xpInitialScope, xpPost);
                  scope.apiUrl = scope.app.apiBaseUrl + 'mobile/transportOperation/v1/ReadInfo'
                      .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                      .replace("//", "/");

                  scope.id = operationId;
                  scope.arguments = {
                      cardId: type === CARDTYPE_CLASSIC ? uid : '',
                      type: type,
                      script: script, 
                      isRead: true
                  };

                  angular.extend(scope.arguments, scope.platform.device);
                  scope.goBack = 0;
                  scope.accept()
                      .then(
                      deferred.resolve,
                      deferred.reject
                  );

                  return deferred.promise;
              }
              function getDataNoCompat(uid, type) {
                  var deferred = $q.defer();

                  var scope = $rootScope.$new();
                  angular.extend(scope, xpInitialScope, xpPost);
                  scope.apiUrl = scope.app.apiBaseUrl + 'mobile/transportOperation/v1/ClassicNoCompatible'
                      .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                      .replace("//", "/");

                  scope.arguments = {
                      cardId: uid,
                      type: type,
                  };
                  scope.goBack = 0;   
                  scope.accept()
                      .then(
                      deferred.resolve,
                      deferred.reject
                  );

                  return deferred.promise;
              }
              function showCard(uid, type, script, operationId, data) {
                  var deferred = $q.defer();

                  if (
                      ($state.current.name === 'transportCardGet') ||
                      ($state.current.name === 'transportCardRetry')) {
                      var history = $ionicHistory.viewHistory();
                      history.currentView = history.backView;
                  }
                  $state.go(
                      'transportCardGet', {
                          cardId: uid,
                          cardType: type,
                          cardScript: script,
                          cardData: data,
                          operationId: operationId
                      }, {
                          reload: 1
                      })
                      .then(
                      deferred.resolve,
                      deferred.reject
                  );

                  return deferred.promise;
              }
              function confirmAndReadInfo(operationId, uid, type, script, slot) {
                  var deferred = $q.defer();

                  var scope = $rootScope.$new();
                  angular.extend(scope, xpInitialScope, xpPut);
                  scope.apiUrl = scope.app.apiBaseUrl + 'mobile/transportOperation/v1/ConfirmAndReadInfo'
                      .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                      .replace("//", "/");

                  scope.id = operationId;

                  scope.arguments = {
                      cardId: type === CARDTYPE_CLASSIC ? uid : '',
                      type: type,
                      script: script,
                      slot: slot,
                      device: $rootScope.platform.device

                  };

                  scope.goBack = 0;
                  scope.accept()
                      .then(
                      deferred.resolve,
                      deferred.reject
                  );

                  return deferred.promise;
              }
              function confirm(operationId, data) {
                  var deferred = $q.defer();

                  var scope = $rootScope.$new();
                  angular.extend(scope, xpInitialScope, xpPut);
                  scope.apiUrl = scope.app.apiBaseUrl + 'mobile/transportOperation/v1/Confirm'
                      .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                      .replace("//", "/");
                  scope.id = operationId;

                  scope.goBack = 0;
                  scope.accept()
                      .then(

                      deferred.resolve,
                      deferred.reject
                  );

                  return deferred.promise;
              }

              $ionicPlatform.ready(function() {
                  if(typeof nfc !== 'undefined') {
                      nfc.addCallback(
                          function (event) { // Callback cuando se detecta una tarjeta
                              $log.log("xp.nfc.js: NFC Detection");
                              if (event.tag.type == CARDTYPE_NFCA) { // CARDTYPE_NFCA
                                  $ionicLoading.show({ template:'Buscando tarjeta en servidor.' });
                                  setTimeout(function() {
                                      getDataNoCompat(event.tag.uid, event.tag.type)
                                          .then(
                                          function(result) {
                                              if (result) {
                                                  showCard(event.tag.uid, event.tag.type, null, null, result)
                                                      .then(
                                                      function() {
                                                          $ionicLoading.hide();
                                                      },
                                                      function() {
                                                          $ionicLoading.hide();
                                                      }
                                                  );
                                              } else {
                                                  $ionicLoading.hide();
                                                  $rootScope.alert('Tarjeta no compatible', 'Esta tarjeta no es compatible o está dañada.');
                                              }
                                          },
                                          function(error) {
                                              $ionicLoading.hide();
                                              $rootScope.alert('Error', error.message);
                                          }
                                      );
                                  }, 100);
                              } else if (event.tag.type == CARDTYPE_CLASSIC) { // CARDTYPE_CLASSIC
                                  $ionicLoading.show({ template:'Leyendo tarjeta NFC.\nNo la separe del móvil.' })
                                  setTimeout(function() {
                                      if (event.tag.script) {
                                          // Detección de una tarjeta y tenia script que ejecutar
                                          showCard(event.tag.uid, event.tag.type, event.tag.script, event.tag.operationId)
                                          .then(
                                              function() { $ionicLoading.hide(); },
                                              function() { $ionicLoading.hide(); }
                                          );
                                      } else {
                                          // Detección de una tarjeta y no tenia script que ejecutar
                                          getScript(event.tag.uid, event.tag.type, $rootScope.platform.device)
                                          .then(
                                              function(result) {
                                                  $ionicLoading.show({ template:'Leyendo tarjeta NFC.\nNo la separe del móvil.' })
                                                  $rootScope.xpNfc
                                                  .execute(result.scripts[0].card, result.scripts[0].keys, result.scripts[0].script, result.operation)
                                                  .then(
                                                      function() { /* Todo ok, ya es el movil el que lo lanza */ },
                                                      function() { $ionicLoading.hide(); }
                                                  );
                                              },
                                              function() {
                                                  $ionicLoading.hide();
                                              }
                                          );
                                      }
                                  }, 100);
                              } else {
                                  $rootScope.alert('Tarjeta no compatible', 'Esta tarjeta no es compatible o está dañada.');
                              }
                          },
                          function (event) { // Callback de ejecuta un script desde cache 
                              console.log("NFC Readed");
                              $ionicLoading.show({ template:'Leyendo tarjeta NFC.\nNo la separe del móvil.' })
                              setTimeout(function() {
                                  if (event.tag.operationType === OPERATIONTYPE_REVOKE) {
                                      confirmAndReadInfo(event.tag.operationId, event.tag.uid, event.tag.type, event.tag.script, event.tag.slot)
                                      .then(
                                          function(data) {
                                              if (event.tag.operationType === OPERATIONTYPE_REVOKE) {
                                                  $ionicPopup.alert({
                                                      title: 'Devolución realizada',
                                                      template:
                                                      '<div>Devolución realizada con éxito. Gracias por usar Pay[in]</div>',
                                                      okType: 'button-payin'
                                                  });
                                              }

                                              var history = $ionicHistory.viewHistory();
                                              var previous = {};
                                              var count = 0;
                                              angular.forEach(history.views, function(view){
                                                  if (count > 0)
                                                      count++;
                                                  if (view.stateName === "transportCardGet"){
                                                      count++;
                                                  }
                                                  previous = view;
                                              });
                                              if (count > 0)
                                                  $ionicHistory.goBack(-count);
                                              setTimeout(function() {
                                                  $rootScope.xpNfc.execute(data.scripts[0].card, data.scripts[0].keys, data.scripts[0].script, event.tag.operationType, data.slot).then(
                                                      showCard(event.tag.uid, event.tag.type, event.tag.script, event.tag.operationId, data, event.tag.slot)
                                                      .then(
                                                          function() { $ionicLoading.hide(); },
                                                          function() { $ionicLoading.hide(); }
                                                      )
                                                  );
                                              }, 100);
                                          },
                                          function() { }
                                      );
                                  } else if (event.tag.operationType === OPERATIONTYPE_RECHARGE) {
                                      // TODO: Cambiar getData per confirmar i fer el getData
                                      confirmAndReadInfo(event.tag.operationId, event.tag.uid, event.tag.type, event.tag.script, event.tag.slot)
                                      .then(
                                          function(data) {
                                              if (event.tag.operationType === OPERATIONTYPE_RECHARGE)
                                              {
                                                  $ionicPopup.alert({
                                                      title: 'Recarga realizada',
                                                      template:
                                                      '<div>Recarga realizado con éxito. Gracias por usar Pay[in]</div>',
                                                      okType: 'button-payin'
                                                  });
                                              }

                                              var history = $ionicHistory.viewHistory();
                                              var previous = {};
                                              var count = 0;
                                              angular.forEach(history.views, function(view){
                                                  if (count > 0)
                                                      count++;
                                                  if(view.stateName === "transportCardGet"){
                                                      count++;
                                                  }
                                                  previous = view;
                                              });
                                              if (count > 0)
                                                  $ionicHistory.goBack(-count);
                                              setTimeout(function() {
                                                  $rootScope.xpNfc.execute(data.scripts[0].card, data.scripts[0].keys, data.scripts[0].script, event.tag.operationType, data.slot)
                                                  .then(
                                                      showCard(event.tag.uid, event.tag.type, event.tag.script, event.tag.operationId, data, event.tag.slot)
                                                      .then(
                                                          function() { $ionicLoading.hide(); },
                                                          function() { $ionicLoading.hide(); }
                                                      )
                                                  );
                                              }, 100);
                                          },
                                          function() { }
                                      );
                                  } else if (event.tag.operationType === OPERATIONTYPE_READ) {
                                      console.log("Read: Getting data");
                                      getData(event.tag.uid, event.tag.type, event.tag.script, event.tag.operationId).then(
                                          function(data){
                                              var found = $filter('filter')(data.scripts[0].script, { 'operation': 3 });
                                              if (found.length > 0) {
                                                  // Tiene alguna escritura de LN o LG
                                                  data.operation.type = OPERATIONTYPE_CONFIRM;
                                                  console.log("Read: Executting LG/LN");
                                                  $rootScope.xpNfc.execute(data.scripts[0].card, data.scripts[0].keys, data.scripts[0].script,data.operation,'-1')
                                                  .then(
                                                      function() {},
                                                      function() { $ionicLoading.hide(); }
                                                  );
                                              } else {
                                                  console.log("Read: Showing card");
                                                  // La lectura no ha escrito nada
                                                  showCard(event.tag.uid, event.tag.type, event.tag.script, event.tag.operationId, data, event.tag.slot)
                                                  .then(
                                                      function() { $ionicLoading.hide(); },
                                                      function() { $ionicLoading.hide(); }
                                                  )
                                              }
                                          },
                                          function() { $ionicLoading.hide(); }
                                      );
                                  }
                                  else if (event.tag.operationType === OPERATIONTYPE_CONFIRM) {
                                      // Confirm LG y LN
                                      console.log("Confirming LG/LG");
                                      confirmAndReadInfo(event.tag.operationId, event.tag.uid, event.tag.type, event.tag.script, event.tag.slot)
                                          .then(
                                          function(data) {
                                              console.log("Read: Showing card");
                                              showCard(event.tag.uid, event.tag.type, event.tag.script, event.tag.operationId, data, event.tag.slot)
                                                  .then(
                                                  function() { $ionicLoading.hide(); },
                                                  function() { $ionicLoading.hide(); }
                                              )
                                          },
                                          function() { $ionicLoading.hide(); }
                                      );
                                  };
                              }, 100);
                          },
                          function (error) { // Callback de error
                              console.log("NFC Error " + error);
                              var history = $ionicHistory.viewHistory();
                              var previous = {};
                              var count = 0;
                              angular.forEach(history.views, function(view){
                                  if (count > 0)
                                      count++;
                                  if(view.stateName === "transportCardGet"){
                                      //transportCardGet
                                      count++;
                                  }
                                  previous = view;
                              });
                              if (count > 0)
                                  $ionicHistory.goBack(-count);
                              $ionicLoading.show({ template: error.tag.error, duration: 4000 })
                              $state.go('transportCardRetry');
                          }
                      );
                  }
              });
          }
         ])
    .service('xpNfc', ['$rootScope', '$state', '$ionicPlatform', '$ionicHistory', '$q', '$ionicLoading',
                       function ($rootScope, $state, $ionicPlatform, $ionicHistory, $q, $ionicLoading) {
                           // Rellotge
                           // var cardString =
                           // "92CA810AD3880400C185149245700911,090030525800000000000000000000C6,000000000000000000000000000000B1,000000000000FF078069000000000000," +
                           // "311000000000000000200000000040A3,0100000002000000000000000000005A,0100000002000000000000000000005A,000000000000FF078069000000000000," +
                           // "2000000091000080B60B0084010000B4,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069000000000000," +
                           // "01C20000074000000020FCDFDF0400B8,69BB62DB05000000000000000000009D,000000000000000000000000000000B1,000000000000FF078069000000000000," +
                           // "000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069000000000000," +
                           // "00000000FFFFFFFF0000000000FF00FF,00000000FFFFFFFF0000000000FF00FF,000000000000000000000000000000B1,000000000000FF078069000000000000," +
                           // "00000000FFFFFFFF0000000000FF00FF,00000000FFFFFFFF0000000000FF00FF,000000000000000000000000000000B1,000000000000FF078069000000000000," +
                           // "000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069000000000000," +
                           // "000000000000000000000000000000B1,000000000000000000000000000000B1,00000002000000000000000000000008,000000000000FF078069000000000000," +
                           // "000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069000000000000," +
                           // "000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069000000000000," +
                           // "000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069000000000000," +
                           // "000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069000000000000," +
                           // "000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069000000000000," +
                           // "000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069000000000000," +
                           // "000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069000000000000";
                           // ???
                           var cardString = "4EE68983A28804004659949765103308,00000000000000000000000000000000,00000000000000000000000000000000,000000000000FF078069000000000000,31104CBA0000000000A0400000004055,130000000A039B8023BC001900780077,130000000A039B8023BC001900780077,000000000000FF078069000000000000,0000000014CB4A03B711C00FCE0B0003,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069000000000000,E10701C0004000000020FC0F000400DF,5AE9ADA36BC9CC0470840B8001C80054,000000000000000000000000000000B1,000000000000FF078069000000000000,030C00800020FCCFF304000040010049,030C00800020FCCFF304000040010049,000000000000000000000000000000B1,000000000000FF078069000000000000,00000000FFFFFFFF0000000001FE01FE,00000000FFFFFFFF0000000001FE01FE,000000000000000000000000000000B1,000000000000FF078069000000000000,00000000FFFFFFFF0000000001FE01FE,00000000FFFFFFFF0000000001FE01FE,000000000000000000000000000000B1,000000000000FF078069000000000000,1AAC8F237C40A1931000800000000021,13AC8F238C1A01A3A0286E00000800B8,18808123BC8081261034890008080050,000000000000FF078069000000000000,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000000000000000000000B1,000000000000FF078069000000000000,00000000000000000000000000000000,00000000000000000000000000000000,00000000000000000000000000000000,000000000000FF078069000000000000,00000000000000000000000000000000,00000000000000000000000000000000,00000000000000000000000000000000,000000000000FF078069000000000000,00000000000000000000000000000000,00000000000000000000000000000000,00000000000000000000000000000000,000000000000FF078069000000000000,00000000000000000000000000000000,00000000000000000000000000000000,00000000000000000000000000000000,000000000000FF078069000000000000,00000000000000000000000000000000,00000000000000000000000000000000,00000000000000000000000000000000,000000000000FF078069000000000000,00000000000000000000000000000000,00000000000000000000000000000000,00000000000000000000000000000000,000000000000FF078069000000000000,00000000000000000000000000000000,00000000000000000000000000000000,00000000000000000000000000000000,000000000000FF078069000000000000";
                           // Rellotge personalizado
                           //var cardString = ",090030525800000000000000000000C6,,-----------Sector 0-----------,1210443559C30000003900000000401D,41000000000339CEDB2DC900006EE040,,-----------Sector 1-----------,2000008091000000C71E008401000085,475245474F52494F0000000000003EA8,000000000080AC0600001143B383225A,-----------Sector 2-----------,01C400C0FF75000000E0E901000000A3,71ECA3631F00000080A43F000000005F,4841524F20204A4156414C4F5945534B,-----------Sector 3-----------,000000000000000000000000000000B1,000000000000000000000000000000B1,1880D833BDB8810810440000200000B2,-----------Sector 4-----------,00000000FFFFFFFF0000000000FF00FF,,1A80D89BDB7601A44030710008000036,-----------Sector 5-----------,00000000FFFFFFFF0000000000FF00FF,,,-----------Sector 6-----------,1880D833BDE0812E107407002800004D,1880D833BDD2810810440000300000C7,1A80D89B0B0801A4403071000800008A,-----------Sector 7-----------,,,,-----------Sector 8-----------,,,,-----------Sector 9-----------,,,,-----------Sector 10-----------,1B80D85BDC120124B1D8710008000019,1A80D85B1C2D919610000000000000E1,1880D8DB2DC9018E13084300080000DB,-----------Sector 11-----------,,,,-----------Sector 12-----------,,,,-----------Sector 13-----------,,,,-----------Sector 14-----------,,,,-----------Sector 15-----------";

                           var cardScript = cardString.split(',');
                           var tagType_;

                           function readCallback (nfcData) {
                               if ((nfcData.tag.script != null) && (nfcData.tag.script.length > 0))
                                   $ionicLoading.show({ template: 'Leyendo tarjeta...' })
                                   $state.go('transportCardGet', {
                                       cardId: nfcData.tag.id,
                                       cardType: tagType_, 
                                       cardScript: nfcData.tag.script
                                   });
                               setTimeout(function(){
                                   $ionicLoading.hide();
                               }, 1500);
                           }
                           function swap16(val) {
                               var temp = val
                               .replace(/^(.(..)*)$/, "0$1") // add a leading zero if needed
                               .match(/../g); // split number in groups of two
                               temp.reverse(); // reverse the groups
                               var result = temp.join(""); // join the groups back together
                               return result;
                           }

                           return {
                               busy: 0,
                               getAllVC: function() {
                                   var deferred = $q.defer();

                                   if (typeof nfc !== 'undefined') {
                                       nfc.getAllVC(
                                           function (result) { //win
                                               console.log("VC get all");
                                               deferred.resolve(result);
                                           },
                                           function (error) { //fail
                                               if(error=="-1"){
                                                   console.log("El dispositivo móvil no es compatible")
                                               }   
                                               //alert("Error getting all VC " + error);
                                               deferred.reject();
                                           });
                                   } else {
                                       deferred.reject("NFC not enabled");
                                   }

                                   return deferred.promise;
                               },
                               createVC: function(name, type, uidType, perso, apps) {
                                   var deferred = $q.defer();

                                   if (typeof nfc !== 'undefined') {
                                       nfc.createVC({
                                           name: name,
                                           type: type,
                                           uidType: uidType,
                                           perso: perso,
                                           apps: apps
                                       },
                                                    function (result) { // win
                                           console.log("VC created");
                                           deferred.resolve(result);
                                       },
                                                    function (error) { // fail
                                           alert("Error creating VC " + error);
                                           deferred.reject();
                                       });
                                   } else {
                                       deferred.reject("NFC not enabled");
                                   }

                                   return deferred.promise;
                               },
                               deleteVC: function(id) {
                                   var deferred = $q.defer();

                                   if (typeof nfc !== 'undefined') {
                                       nfc.deleteVC({
                                           id: id
                                       },
                                                    function (result) { // win
                                           console.log("VC deleted");
                                           deferred.resolve(result);
                                       },
                                                    function (error) { // fail
                                           alert("Error deleting VC " + error);
                                           deferred.reject();
                                       });
                                   } else {
                                       deferred.reject("NFC not enabled");
                                   }

                                   return deferred.promise;
                               },
                               activateVC: function(id) {
                                   var deferred = $q.defer();

                                   if (typeof nfc !== 'undefined') {
                                       nfc.activateVC({
                                           id: id
                                       },
                                                      function (result) { // win
                                           console.log("VC activated");
                                           deferred.resolve(result);
                                       },
                                                      function (error) { // fail
                                           alert("Error activating VC " + error);
                                           deferred.reject();
                                       });
                                   } else {
                                       deferred.reject("NFC not enabled");
                                   }

                                   return deferred.promise;
                               },
                               execute: function(card, keys, script, operation, slot) {
                                   var that = this;
                                   var deferred = $q.defer();

                                   if ($rootScope.xpNfc.emulate) {
                                       var result = []
                                       angular.forEach(script, function(item) {
                                           if (item.operation === 1) { // Autenticar
                                           } else if (item.operation === 2) { // Read
                                               item.data = cardScript[item.sector * 4 + item.block];
                                               result.push(item);
                                           } else if (item.operation === 3) { // Write
                                               cardScript[item.sector * 4 + item.block] = item.data;
                                           } else if (item.operation === 4) { // Check
                                           }
                                       });
                                       deferred.resolve(result);
                                   } else if (typeof nfc !== 'undefined') {
                                       nfc.execute(
                                           card || {},
                                           keys || [],
                                           script || [],
                                           operation || {},
                                           slot || 0,
                                           function (result) { // win
                                               console.log("Write EveryThing OK!");
                                               deferred.resolve(result);
                                           },
                                           function (reason) { // fail
                                               console.log("Error Reading Sector/Block " + reason);
                                               deferred.reject();
                                           });
                                   } else {
                                       deferred.reject("NFC not enabled");
                                   }

                                   return deferred.promise;
                               },
                               read: function(res, tagType){
                                   var deferred = $q.defer();

                                   if(typeof nfc !== 'undefined') {
                                       tagType_ = tagType;
                                       nfc.read(
                                           readCallback,
                                           res,
                                           function (result) {
                                               console.log("Reading Tag.");
                                               deferred.resolve(result);
                                           },
                                           function (reason) {
                                               console.log("Can´t read tag"+reason);
                                               $ionicLoading.show({ template: 'Has apartado la tarjeta demasiado pronto.' })
                                               setTimeout(function(){
                                                   $ionicLoading.hide();}, 1500);
                                               deferred.reject();
                                           }
                                       );
                                   } else {
                                       deferred.reject("NFC not enabled");
                                   }

                                   return deferred.promise;       
                               },
                               numUidToHexLE: function(value) {
                                   var val =
                                       value.toString().length > 10 ?
                                       parseInt(value.toString().substring(0,value.toString().length-2)) : 
                                   value;

                                   var val2 = swap16(
                                       parseInt(val)
                                       .toString(16)
                                       .toUpperCase()
                                   );

                                   return val2;
                               },
                               numToHexLE: function(value) {
                                   var val = swap16(
                                       parseInt(value)
                                       .toString(16)
                                       .toUpperCase()
                                   );

                                   return val;
                               },
                               numToHexBE: function(value) {
                                   var val = 
                                       parseInt(value)
                                   .toString(16)
                                   .toUpperCase()
                                   .replace(/^(.(..)*)$/, "0$1");

                                   return val;
                               }
                           };
                       }
                      ])
    .directive('xpNfc', function () {
    return {
        restrict: 'A',
        controller: ['$ionicPlatform', '$scope', '$rootScope', '$state', '$http', 'xpNfc', 'xpInitialScope', 'xpGet',
                     function($ionicPlatform, $scope, $rootScope, $state, $http, xpNfc, xpInitialScope, xpGet) {
                         var tagType=-1;
                         $ionicPlatform.ready(function() {
                             $rootScope.xpNfc = {
                                 emulate: false,
                                 cardId: '451B111A'
                                 //'1A846659' // Lista negra
                             };
                             angular.extend($rootScope.xpNfc, xpNfc);

                             if (typeof nfc === 'undefined')
                                 return;

                             nfc.addNdefFormatableListener(
                                 function(nfcEvent) {
                                     var tagTypeString = nfcEvent.tag.techTypes;
                                     setTimeout(
                                         function() { 
                                             tagType=-1
                                         },
                                         5000
                                     );
                                     if(tagType==-1){
                                         tagType = tagTypeString.indexOf("android.nfc.tech.MifareClassic");
                                         if(tagType==-1){
                                             tagType=tagTypeString.indexOf("import android.nfc.tech.IsoDep");
                                         }
                                         if(tagType!=-1){
                                             var scope = $scope.$new();
                                             angular.extend(scope, xpInitialScope, xpGet);
                                             scope.apiUrl = $scope.app.apiBaseUrl + "mobile/transportOperation/v1/ReadInfo"
                                                 .replace("{0}", ($scope.app.tenant ? "/" + $scope.app.tenant : ""))
                                                 .replace("//", "/");
                                             scope.arguments = {
                                                 "cardId":nfcEvent.tag.id,
                                                 "cardType":tagType, 
                                                 "isRead": true
                                             };
                                             scope.search()
                                                 .then(
                                                 function(res) {
                                                     xpNfc.read(res.data,tagType)
                                                         .then(
                                                         function(result){
                                                             console.log("Ok Read!"+result);
                                                         },
                                                         function(error){
                                                             console.log("Cant Read!"+error);  
                                                         }
                                                     );
                                                 },
                                                 function(error) {
                                                     console.log("Error: " + error);
                                                 }
                                             );
                                         }
                                     }
                                 },
                                 function () {
                                     console.log("Listening for addNdefFormatableListener Tags.");
                                 },
                                 function (reason) {
                                     console.log("No tiene habilitado el NFC en su dispositivo.");
                                 }
                             );
                         });
                     }
                    ]
    };
});