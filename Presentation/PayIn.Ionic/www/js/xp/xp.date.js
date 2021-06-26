/* global datePicker */
/* global ionic */

angular
.module('xp.date', [])
.directive('xpDate', ['$cordovaDatePicker',
    function($cordovaDatePicker) {
        // https://github.com/VitaliiBlagodir/cordova-plugin-datepicker
        function addElement(element, name) {
            var node = document.createElement(name);
            element.appendChild(node);
            return node;
        }
        function addAttribute(element, name, value) {
            var attr = document.createAttribute(name);
            attr.value = value;
            return attr;
        }
        
        var isWebView = ionic.Platform.isWebView();
        var isAndroid = ionic.Platform.isAndroid();
        var isIOS = ionic.Platform.isIOS();
        var isIPad = ionic.Platform.isIPad();
        var isWindowsPhone = ionic.Platform.isWindowsPhone();

        return {
            restrict: 'E',
            template: function($element, $attrs) {
                var name = $attrs.name;
                var model = $attrs.ngModel;

                if (isAndroid) {
                    return "<input type='text' class='inputMainStyle' id='birthday' ng-focus='openCalendar()' name='" + name + "'placeholder='aaaa-mm-dd' ng-model='" + model + "'>";
                } else {
                    return "<input type='text' class='inputMainStyle' name='" + name + "'placeholder='aaaa-mm-dd' ng-model='" + model + "'>";
                }
            },
            link: function($scope, $element, $attrs) {
                if (isAndroid) {
                    $scope.openCalendar = function () {
                        var model = $attrs.ngModel;
                        var nextFocusName = $attrs.xpNextFocus;
                        
                        var nextFocus = document.getElementsByName(nextFocusName);
                        
                        var date = $scope.$eval(model);
                        var options = {
                            date: new Date(date),
                            mode: 'date', // time or datetime
                            is24Hour: true
                        };

                        datePicker
                            .show(
                                options,
                                function(date) {
                                    var yyyy = date.getFullYear().toString();
                                    var mm = (date.getMonth()+1).toString(); // getMonth() is zero-based
                                    var dd  = date.getDate().toString();
                                    
                                    var text = "'" +
                                        yyyy + "-" + 
                                        (mm[1]?mm:"0"+mm[0]).toString() + "-" + 
                                        (dd[1]?dd:"0"+dd[0]).toString() + "'";
                                    $scope.$eval(model + "=" + text);
                                    if (!$scope.$$phase)
                                        $scope.$apply();
                                        
                                    nextFocus[0].focus();
                                },
                                function() {
                                    nextFocus[0].focus();
                                }
                            );
                    };
                }
            }
        };
    }
]);