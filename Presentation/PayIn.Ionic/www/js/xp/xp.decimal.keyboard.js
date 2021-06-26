angular
    .module('xp.decimal.keyboard', ['dbaq.ionNumericKeyboard', 'ionic'])
    .directive('xpDecimalKeyboard', function() {
    // http://market.ionic.io/plugins/ion-numeric-keyboard
    return {
        restrict: 'E',
        template: '<ion-numeric-keyboard options="options"></ion-numeric-keyboard>',
        scope: false,
        controller: function($scope, $element, $cordovaKeyboard) {
            var model = '';

            $scope.showDecimalKeyBoard = function(val) {
                model = val;
                setTimeout(function() { $element.css("visibility", 'visible'); }, 500);
            };
            $scope.hideDecimalKeyBoard = function() {
                $element.css("visibility", 'hidden');
            };
            $scope.options = {
                leftControl: '.',
                rightControl: '<i class="icon ion-backspace-outline"></i></button>',
                onKeyPress: function(value, source) {
                    var input = $scope.$eval(model) || '';

                    if (source === 'LEFT_CONTROL' && input.indexOf('.') === -1) {
                        input += value;
                        $scope.$eval(model + "= '" + input + "'");
                    }
                    else if (source === 'RIGHT_CONTROL') {
                        input = input.substr(0, input.length - 1);
                        $scope.$eval(model + "= '" + input + "'");
                    }
                    else if (source === 'NUMERIC_KEY') {
                        input += value;
                        $scope.$eval(model + "= '" + input + "'");
                    }
                }
            };
        }
    };
})
    .directive('xpDecimalInput', function() {
    // http://market.ionic.io/plugins/ion-numeric-keyboard
    return {
        restrict: 'A',
        link: function($scope, $element, $attrs) {
            var model = $attrs.ngModel;
            var items = $('form input, form select, form textarea');
            angular.forEach(items, function(item) {
                if (item !== $element[0]) {
                    $(item).bind('focus', function() {
                        $scope.hideDecimalKeyBoard();
                    });
                }
            });
            $element.bind('focus', function() {
                $scope.showDecimalKeyBoard(model);
            });
            $element[0].style.background = '#ffffff';
        }
    };
});