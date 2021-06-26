angular
.module('xp.smartband', ['xp'])
.service('xpSmartband', ['$q', '$ionicPopup',
    function($q, $ionicPopup) {
        var scope = undefined;
        return {
            xpSmartband: {
                address: '',
                name: '',
                list: [],
                initialize: function(scope_) {
                    scope = scope_;
                },
                connecting: false,
                connected: false,
                connect: function(address, name) {
                    var deferred = $q.defer();
                    var that = this;

                    that.connecting = true;
                    CoolPlugin.connect(
                        function(result) {
                            console.log("xp.smartband.js: connect success " + result);


                            that.address = localStorage["smartBandAddress"];
                            that.name = localStorage["smartBandName"];
                            that.connected = true;
                            that.connecting = false;

                            deferred.resolve(result);
                        },
                        function(result) {
                            console.log("xp.smartband.js", "Connection error: " + result);

                            that.address = '';
                            that.name = '';
                            that.connected = false;
                            that.connecting = false;

                            deferred.reject(result);
                        },
                        address,
                        name
                    );
                    return deferred.promise;
                },
                stopScan: function() {
                    CoolPlugin.stopScan();
                },
                scanDevices: function() {
                    var deferred = $q.defer();
                    var that = this;

                    that.list.length = 0;
                    CoolPlugin.scanDevices(
                        function(result) {
                            console.log("xp.smartband.js: getAll success " + result);
                            deferred.resolve(result);
                        },
                        function(result) {
                            console.log("xp.smartband.js", "getAll error: " + result);
                            $ionicPopup.alert({
                                title: "Bluetooth",
                                template: "getAll error: " + result,
                                okType: 'button-payin'
                            });
                            deferred.reject(result);
                        },
                        function(ev) {
                            if (!_.some(that.list, function(item) {
                                return (item.id === ev.item.id);
                            })) {
                                that.list.push({
                                    id: ev.item.id,
                                    name: ev.item.name,
                                    connected: ev.item.connected,
                                    deviceType: ev.item.deviceType,
                                    cards: []
                                });
                                if (!scope.$$phase)
                                    scope.$apply();
                            }
                        }
                    );

                    return deferred.promise;
                },
                getAllCards: function(address, name) {
                    var deferred = $q.defer();
                    var that = this;

                    if (that.connected === false)
                    {
                        deferred.reject("Device not connected");
                        return deferred.promise;
                    }

                    var device = _.find(that.list, function(item) {
                        return (item.id === that.address);
                    });
                    if ((device) && (device.cards)) {
                        device.cards.length = 0;
                        if (!scope.$$phase)
                            scope.$apply();
                    }
                    var cards = CoolPlugin.getAllCards(
                        function(result) {
                            console.log("xp.smartband.js: connect success " + result);
                            deferred.resolve(result);
                        },
                        function(result) {
                            console.log("xp.smartband.js", "Connection error: " + result);
                            $ionicPopup.alert({
                                title: "Bluetooth",
                                template: "Connection error: " + result,
                                okType: 'button-payin'
                            });
                            deferred.reject(result);
                        },
                        function(ev) {
                            var device = _.find(that.list, function(item) {
                                return (item.id === that.address);
                            });

                            if (device) {
                                device.cards = ev.item;
                                if (!scope.$$phase)
                                    scope.$apply();
                            }
                        },
                        address,
                        name
                    );
                    return deferred.promise;
                },
                activate: function(id) {
                    var deferred = $q.defer();
                    var that = this;

                    if (that.connected === false)
                    {
                        deferred.reject("Device not connected");
                        return deferred.promise;
                    }

                    CoolPlugin.activate(
                        function(result) {
                            console.log("xp.smartband.js: activate success " + result);
                        },
                        function(result) {
                            console.log("xp.smartband.js", "activate error: " + result);
                            $ionicPopup.alert({
                                title: "Bluetooth",
                                template: "Connection error: " + result,
                                okType: 'button-payin'
                            });
                            deferred.reject(result);
                        },
                        function(result) {
                            console.log("xp.smartband.js", "activate finish: " + result.item.id);
                            $ionicPopup.alert({
                                title: "Bluetooth",
                                template: "Activation success",
                                okType: 'button-payin'
                            });
                            deferred.resolve(result.item);
                        },
                        id
                    );
                    return deferred.promise;
                },
                createCard: function(address, name) {
                    var deferred = $q.defer();
                    var that = this;

                    if (that.connected === false)
                    {
                        deferred.reject("Device not connected");
                        return deferred.promise;
                    }

                    CoolPlugin.createCard(
                        function(result) {
                            console.log("xp.smartband.js: Create card success " + result);
                            deferred.resolve(result);
                        },
                        function(result) {
                            console.log("Bluetooth", "Create card error: " + result);
                            $ionicPopup.alert({
                                title: "Bluetooth",
                                template: "Connection error: " + result,
                                okType: 'button-payin'
                            });
                            deferred.reject(result);
                        },
                        that.address,
                        that.name
                    );
                    return deferred.promise;
                },
                deleteCard: function(entry) {
                    var deferred = $q.defer();
                    var that = this;

                    if (that.connected === false)
                    {
                        deferred.reject("Device not connected");
                        return deferred.promise;
                    }

                    CoolPlugin.deleteCard(
                        function(result) {
                            console.log("xp.smartband.js: Delete card success " + result);
                            deferred.resolve(result);
                        },
                        function(result) {
                            console.log("Bluetooth", "Delete card error: " + result);
                            $ionicPopup.alert({
                                title: "Bluetooth",
                                template: "Connection error: " + result,
                                okType: 'button-payin'
                            });
                            deferred.reject(result);
                        },
                        entry
                    );
                    return deferred.promise;
                }
            }
        };
    }
])
.directive('xpSmartband', function () {
    return {
        restrict: 'A',
        controller: ['$ionicPlatform', '$scope', 'xpSmartband',
            function($ionicPlatform, $scope, xpSmartband) {
                angular.extend($scope, xpSmartband);
                $scope.xpSmartband.initialize($scope);

                localStorage["smartBandAddress"] = "";
                localStorage["smartBandName"] = "";
           }
        ]
    }
});