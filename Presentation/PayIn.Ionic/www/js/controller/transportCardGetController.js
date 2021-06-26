angular.module('app')
    .service('transportCardGet',
             ['$state','xpPost', '$ionicPopup',
              function($state, xpPost, $ionicPopup) {
                  return {

                      'ultValidation': function(){
                          var that = this;
                          if(that.temp.controlShow != 1) {
                              that.temp.controlShow = 1;

                              that.temp.nameButton = 'Mostrar titulos';
                              that.temp.nameButton1 = "Últimas recargas";
                          }else{
                              that.temp.controlShow = 0;
                              that.temp.nameButton1 = "Últimas recargas";
                              that.temp.nameButton = "Últimas validaciones";
                          }

                      },
                      'ultRecharge': function(){
                          var that = this;
                          if(that.temp.controlShow != 2) {
                              that.temp.controlShow = 2;
                              that.temp.nameButton1 = 'Mostrar titulos';
                              that.temp.nameButton = "Últimas validaciones";

                          }else{
                              that.temp.controlShow = 0;
                              that.temp.nameButton1 = "Últimas recargas";
                              that.temp.nameButton = "Últimas validaciones";
                          }

                      },

                      'recharge': function(recharges, charges, uid, type, script) {
                          $state.go('transportCardGetAllRecharges', { 
                              recharges: recharges,
                              charges: charges,
                              cardId: uid,
                              cardType: type,
                              cardScript: script
                          });
                      },

                      'revoke': function() {
                          var scope = this.$new();
                          angular.extend(scope, xpPost);
                          scope.apiUrl = this.app.apiBaseUrl + "Mobile/TransportOperation/v1/Revoke";
                          angular.extend(scope.arguments, scope.platform.device);
                          scope.goBack = 0;

                          scope.arguments = {
                              "operationId": scope.temp.operationId,
                              "cardId": scope.temp.cardId,
                              "cardType": scope.temp.cardType,
                              "script": scope.temp.cardScript,
                              "imei": scope.platform.device.imei
                          };
                          scope.arguments.device = scope.arguments.device || {};
                          angular.extend(scope.arguments.device, scope.platform.device);

                          var myPopup = $ionicPopup.prompt({
                              title: 'Devoluciones',
                              template: 'Se dispone a anular la última recarga de tarjeta que ha realizado, si desea continuar introduzca el código Pin',
                              inputType: 'password',
                              inputPlaceholder: 'Introduce tu código PIN',
                              cancelType:'button',
                              okType: 'button-payin'
                          });
                          myPopup.then(function(pin) {
                              if (pin !== undefined && pin !== "") {  
                                  scope.accept()
                                      .then(function(data) {
                                      angular.extend(scope.arguments, pin)
                                      if (data) {
                                          scope.xpNfc.execute(data.scripts[0].card, data.scripts[0].keys, data.scripts[0].script, data.operation);
                                          $state.go('transportCardRecharge' );
                                      }
                                  })
                              } 
                              else if (pin !== undefined && pin === "") {
                                  $ionicPopup.alert({
                                      title: 'Error',
                                      template: 'Debes introducir un código PIN.',
                                      okType: 'button-payin'
                                  });
                              }
                          });
                      }
                  };
              }
             ])
    .controller('transportCardGetController', ['$scope', 'transportCardGet',
                                               function($scope, transportCardGet) {       
                                                   angular.extend($scope, transportCardGet);
                                               }
                                              ]);