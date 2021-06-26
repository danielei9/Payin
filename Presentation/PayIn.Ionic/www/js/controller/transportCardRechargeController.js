/* global _ */
angular
.module('app')
.service('transportCardRecharge', ['$state', '$ionicHistory','xpNfc',
    function($state, $ionicHistory,xpNfc) {
        return {
            'execute': function(cardId, cardType, data) {
                var that = this;
                
                if (!that.temp.cardScript || !that.temp.cardScript.length) {
                    $state.go('transportCardGet', {cardId: cardId, cardType:cardType });
                    return;
                }
                if (!cardId) {
                    $state.go('transportCardGet');
                    return;
                }
                
                var script = [];
                angular.forEach(data.data, function(item) {
                    if (item.operation === 2)
                    {
                        var lectores = _
                            .where(that.temp.cardScript, { operation: 2, sector: item.sector, block: item.block })
                  
                        if (lectores.length > 0)
                            script.push(lectores[0]);
                        else
                            script.push(item);
                    } else
                        script.push(item);
                })
                that.arguments.script = script;
                
                this.$parent.accept();
            },
            'update': function(data) {
                xpNfc.execute(data.data);
                //$ionicHistory.goBack();
            }
        };
    }
])
.controller('transportCardRechargeController', ['$scope', 'transportCardRecharge',
    function($scope, transportCardRecharge) {
        angular.extend($scope, transportCardRecharge);
    }
]);
