angular
    .module('app')
    .service('transportCardGetAllCharges', 
             ['xpNavigation', '$state',
              function(xpNavigation, $state) {
                  return {
                      'selectTransport': function(temp, ticket, price,meanTransport){


                          var transportType = [];
                          angular.forEach(temp.charges, function(value, key) {
                              if(temp.charges[key].meanTransport === meanTransport)
                                  transportType.push(value);
                          });
                          temp.charges = transportType;

                          $state.go('transportCardGetAllCharges', {
                              charges:temp.charges,
                              cardId:temp.cardId,
                              Type: temp.cardType,
                              cardScript: temp.cardScript
                          })
                      },
                      'clickCharge':function(temp, ticket) {
                          var that = this;
                          if (ticket.prices.length===1) {
                              var price = ticket.prices[0];
                              that.createTicket(temp, ticket, price)
                          } else {
                              $state.go('transportCardGetAllPrices', {
                                  temp: temp,
                                  prices: ticket.prices,
                                  ticket: ticket,
                                  cardId: temp.cardId, 
                                  cardType: temp.cardType, 
                                  cardScript: temp.cardScript
                              });
                          }
                      }
                  };
              }
             ])
    .controller('transportCardGetAllChargesController', 
                ['$scope', 'transportCardGetAllCharges', 'transportCardGetAllRecharges',
                 function($scope, transportCardGetAllCharges, transportCardGetAllRecharges) {       
                     angular.extend($scope, transportCardGetAllCharges, transportCardGetAllRecharges);
                 }
                ]);
