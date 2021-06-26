angular
    .module('app')
    .controller('paymentMediaSelectCardController', ["$scope","$state",function($scope, $state) {
        $scope.activePaymentMedia = function(value) { 
            return value.order;
        };
        $scope.selectPaymentMedia = function(item) {
            var orders;
            var amount;
            if (item.order) {
                delete item.order;
            } else {
                orders = _.map($scope.data, function(x) { return x.order || 0; });
                amount = $scope.amount -
                    _.reduce($scope.data, function(total, pm) { 
                    if (pm.order)
                        return total + pm.balance;
                    else
                        return total; 
                }, 0);          
                if (amount > 0 && item.balance)
                    item.order = _.max(orders) + 1;
            }
        };
        $scope.selectCard = function(id) {
            var paymentMedias = _.filter($scope.data, function(item) { return item.order; });
            var cards = _.filter($scope.data, function(item) { return item.type === 4; });
            if ($scope.payed() >= $scope.amount) {
                $state.go("ticketpay", { ticketId: $scope.arguments.ticketId, paymentMedias: paymentMedias, noCard: true });
            } else {
                if (cards.length == 1) {
                    paymentMedias.push(cards[0]);
                    $state.go("ticketpay",{ ticketId: $scope.arguments.ticketId, paymentMedias: paymentMedias, noCard: false });
                } else if (cards.length == 0){
                    $state.go('wizardUser');
                }else {
                    $state.go("paymentmediaselectcard",{ ticketId: $scope.arguments.ticketId, paymentMedias : $scope.data, noCard: false });
                }
            }
            
        };
        $scope.selectCardFinal = function(id){
            var cards = _.filter($scope.data, function(item) { return item.id === id; });
            var paymentMedias = _.filter($scope.data, function(item) { return item.order; });
            paymentMedias.push(cards[0]);
            $state.go("ticketpay",{ ticketId: $scope.temp.ticketId, paymentMedias: paymentMedias });
        };
        $scope.ticketpay = function(id) {
            $state.go("ticketpay",{ ticketId: $scope.arguments.ticketId, paymentMedias : $scope.arguments.paymentMedias });
        };
        $scope.paymentMediaPayed = function(item) {
            if (!item.order)
                return 0;
            var amount = $scope.amount -
                _.reduce($scope.data, function(total, pm) { 
                    if (pm.order && pm.order < item.order)
                        return total + pm.balance;
                    else
                        return total; 
                }, 0);
            if (item.balance > 0 &&item.order) {
                return _.min([item.balance, amount]);
            }
            return 0;
        };
        $scope.payed = function() {
            return _.reduce($scope.data, function(total, pm){ return total + $scope.paymentMediaPayed(pm); }, 0);
        };
    }]);