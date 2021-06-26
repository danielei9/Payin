angular
    .module('app')
    .service('transportCardGetAllRecharges', 
             ['xpNavigation', '$state', '$ionicPopup', 'xpInitialScope', 'xpGet',
              function(xpNavigation,$state, $ionicPopup, xpInitialScope, xpGet) {
                  return {
                      'createTicket2':function(temp, ticket, price) {
                          /*
                          price.off = 0;*/
                          var that = this;
                          /*var descuento = 0;
                          //if(X === tuin){
                              $ionicPopup.prompt({
                                  title: 'Recargas y Actualizaciones',
                                  template: 'La fecha de validez y canje de sus títulos está agotada. Se sobreescribirá los datos de la tarjeta con la nueva recarga, ¿está seguro que quiere continuar?',
                                  okType: 'button-payin'
                              }).then(function(caixa) {
                                      price.off = (1-price.price) * caixa;
                                      price.price = caixa;
*/
                          that.createTicket3(temp, ticket, price);
                          //                              }
                          /*});
                          }else{
                              that.createTicket3(temp, ticket, price);*/
                          //   }
                          //)
                      },
                      'isTuiN': function(temp, ticket, price, that){
                          if(ticket.code > 1270 && ticket.code < 1278){
                              $ionicPopup.prompt({
                                  title: 'TuIN',
                                  template: 'Introduce el importe a recargar. El mínimo es '+ticket.tuiNMin+ '€ y el máximo es de ' + ticket.tuiNMax+'€',
                                  inputType: 'number',
                                  inputPlaceholder: 'Importe',
                                  okType: 'button-payin'
                              }).then(function(res) {
                                  if(res == null || res < ticket.tuiNMin || res > ticket.tuiNMax){
                                      alert("La cantidad solicitada no es válida");
                                  }else{
                                      price.price = res;
                                      that.createTicket2(temp, ticket, price)
                                  }
                              });
                          }else{
                              that.createTicket2(temp, ticket, price);
                          }
                      },
                      'createTicket3':function(temp, ticket, price) {

                          var that = this;
                          var scope = that.$new();
                          angular.extend(scope, xpInitialScope, xpGet);
                          scope.apiUrl = scope.app.apiBaseUrl + "mobile/ticket/v2"
                              .replace("{0}", (scope.app.tenant ? "/" + scope.app.tenant : ""))
                              .replace("//", "/");
                          scope.arguments.lines=[];
                          scope.arguments.lines.push(JSON.stringify({
                              lines:{
                                  title:ticket.name + ' ' + price.zoneName,
                                  amount:price.price,
                                  quantity:1
                              }
                          }));
                          if (price.changePrice) {
                              scope.arguments.lines.push(JSON.stringify({
                                  lines:{
                                      title:'Actualización billetes',
                                      amount:price.changePrice,
                                      quantity:1
                                  }
                              }));
                          }
                          /* if (price.off) {
                              that.arguments.lines.push({
                                  title:'Descuento',
                                  amount:-1*price.off,
                                  quantity:1
                              });
                          }*/
                          scope.arguments.reference = ticket.paymentConcessionId;
                          scope.arguments.transportPrice = price.id;
                          scope.arguments.date = moment(Date()).format('YYYY-MM-DD HH:mm:ss');
                          scope.arguments.concessionId= 1;

                          scope.search()
                              .then(function(data) {
                              if (data) {
                                  xpNavigation.go("ticketget", {
                                      data: data,
                                      id: data.data[0].id, 
                                      cardId: temp.cardId, 
                                      cardType: temp.cardType, 
                                      cardScript: temp.cardScript,
                                      operationId: temp.operationId,
                                      code: ticket.code, 
                                      quantity: 1,
                                      operationType: temp.operationType,
                                      priceId: price.id, 
                                      slot: price.slot,
                                      rechargeType : price.rechargeType});
                              }
                          });
                      },
                      'createTicket':function(temp, ticket, rechargeTitles) {
                          var that = this;   
                          var exists = false; 
                          var ticket2;        
                          if(Array.isArray(rechargeTitles)){
                              angular.forEach(rechargeTitles, function(ticket, key) {                           
                                  if(ticket.code == rechargeTitles[key].code && exists == false)   
                                  {                    
                                      ticket2 = rechargeTitles[key];
                                      exists = true;
                                  }

                              }); 
                              price = ticket2.prices[0];   
                          }else{
                              price = rechargeTitles;
                          }
                          if(price.rechargeType == 1){
                              // Cargar
                              that.isTuiN(temp, ticket, price, that)
                          }
                          else if( price.rechargeType == 2){
                              // Cargar
                              that.isTuiN(temp, ticket, price, that)
                          }
                          else if(price.rechargeType == 4) {
                              //Replace
                              $ionicPopup.confirm({
                                  title: 'Recargas y Actualizaciones',
                                  template: '<div>Con la recarga actual se eliminarán los títulos ya existentes, ¿desea continuar?</div>',
                                  okType: 'button-payin'
                              }).then(function(buttonOK) {
                                  if(buttonOK){
                                      that.isTuiN(temp, ticket, price, that)
                                  }
                              });
                          }
                          else if( price.rechargeType == 5) {
                              //RechargeAndUpdatePrice
                              $ionicPopup.confirm({
                                  title: 'Recargas y Actualizaciones',
                                  template: '<div>La recarga actual actualizará la tarifa de los títulos recargados, ¿desea continuar?</div>',
                                  okType: 'button-payin'
                              }).then(function(buttonOK) {
                                  if(buttonOK){
                                      that.isTuiN(temp, ticket, price, that)
                                  }
                              });
                          }
                          else if(price.rechargeType == 6) {
                              //RechargeAndUpdateZone
                              $ionicPopup.confirm({
                                  title: 'Recargas y Actualizaciones',
                                  template: '<div>La recarga actual implica un cambio de zona del título seleccionado, ¿desea continuar?</div>',
                                  okType: 'button-payin'
                              }).then(function(buttonOK) {
                                  if(buttonOK){
                                      that.isTuiN(temp, ticket, price, that)
                                  }
                              });
                          }else if(price.rechargeType == 8) {
                              //RechargeExpiredPrice
                              $ionicPopup.confirm({
                                  title: 'Recargas y Actualizaciones',
                                  template: '<div>Sus títulos estan caducados y no se pueden aprovechar. La recarga actual implica eliminarán los títulos ya existentes, ¿desea continuar?</div>',
                                  okType: 'button-payin'
                              }).then(function(buttonOK) {
                                  if(buttonOK){
                                      that.isTuiN(temp, ticket, price, that)
                                  }
                              });
                          }

                      }
                  };
              }
             ])
    .controller('transportCardGetAllRechargesController', ['$scope', 'transportCardGetAllRecharges',
                                                           function($scope, transportCardGetAllRecharges) {       
                                                               angular.extend($scope, transportCardGetAllRecharges);
                                                           }
                                                          ]);