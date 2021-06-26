angular
.module('app')
.controller('transportCardGetAllController', ['$scope', '$ionicPopup', '$ionicActionSheet', '$timeout',
    function($scope, $ionicPopup, $ionicActionSheet, $timeout) {
        $scope.connectDevice = function() {
            $scope.showLoading("Conectando...");
            setTimeout(function() {
                if (localStorage["smartBandAddress"]) {
                    $scope.xpSmartband
                        .connect(localStorage["smartBandAddress"], localStorage["smartBandName"])
                        .then(
                            function () {
                                $scope.hideLoading();
                                //$scope.getAllCards();
                            },
                            function (result) {
                                $scope.hideLoading();
                                $ionicPopup.alert({
                                    title: "Bluetooth",
                                    template: "Connection error: " + result,
                                    okType: 'button-payin'
                                });
                            }
                        );
                } else {
                    $scope.hideLoading();
                }
            }, 100);
        };
        $scope.getAllCards = function() {
            $scope.showLoading("Listando...");
            setTimeout(function() {
                $scope.xpSmartband
                .getAllCards($scope.xpSmartband.address, $scope.xpSmartband.name)
                    .then(
                        function () {
                            $scope.hideLoading();
                        },
                        function (result) {
                            $scope.hideLoading();
                            $ionicPopup.alert({
                                title: "Bluetooth",
                                template: "Get cards error: " + result,
                                okType: 'button-payin'
                            });
                        }
                    );

                //$scope.xpNfc.getAllVC();
            }, 100);
        };
        $scope.createCard = function(deviceType) {
            $scope.showLoading("Creando...");
            setTimeout(function() {
                if (deviceType === 1) {
                    $scope.xpSmartband.createCard()
                    .then(
                        function(result) {
                            $scope.hideLoading();
                        },
                        function(result) {
                            $scope.hideLoading();
                            $ionicPopup.alert({
                                title: "Bluetooth",
                                template: "Create card error: " + result,
                                okType: 'button-payin'
                            });
                        }
                    );
                } else if (deviceType === 2) {
                    $scope.hideLoading();
                    // FALTA para movil
                }
            }, 100);
        };
        $scope.deleteCard = function(deviceType, entry) {
            $scope.showLoading("Eliminando...");
            setTimeout(function() {
                if (deviceType === 1) {
                    $scope.xpSmartband
                        .deleteCard(entry)
                        .then(
                            function() {
                                $scope.hideLoading();
                                $scope.getAllCards();
                            }, function() {
                                $scope.hideLoading();
                            }
                        );
                } else if (deviceType === 2) {
                    $scope.xpNfc
                        .deleteVC(item.id)
                        .then(function() {
                            // var index = $scope.data.indexOf(item);
                            // if (index > -1) {
                            //     $scope.data.splice(index, 1);
                            // }
                            $scope.hideLoading();
                            $scope.getAllCards();
                        }, function() {
                            $scope.hideLoading();
                        });
                }
            }, 100);
        };
        $scope.selectCard = function(deviceType, item) {
            var label = item.name || item.id;
            var hideSheet = $ionicActionSheet.show({
                'buttons': [
                    { text: 'Activar' }
                ],
                'destructiveText': 'Borrar',
                'titleText': 'Tarjeta transporte',
                'buttonClicked': function(index) {
                    // Activate
                    $scope.showLoading("Activando...");
                    setTimeout(function() {
                        if (deviceType === 1) {
                            $scope.xpSmartband
                                .activate(item.id)
                                .then(
                                    function(item) {
                                        angular.forEach($scope.xpSmartband.list, function(device) {
                                            if (device.deviceType === 1) {
                                                angular.forEach(device.cards, function(card) {
                                                    if (card.id === item.id)
                                                        card.isActivated = true;
                                                    else
                                                        card.isActivated = false;
                                                });
                                            }
                                        });
                                        
                                        $scope.hideLoading();
                                    }, function() {
                                        $scope.hideLoading();
                                    }
                                );
                        } else if (deviceType === 2) {
                            $scope.xpNfc
                                .activateVC(item.id)
                                .then(
                                    function(item) {
                                        angular.forEach($scope.xpSmartband.list, function(device) {
                                            if (device.deviceType === 1) {
                                                angular.forEach(device.cards, function(card) {
                                                    if (card.id === item.id)
                                                        card.isActivated = true;
                                                    else
                                                        card.isActivated = false;
                                                });
                                            }
                                        });
                                        
                                        $scope.hideLoading();
                                    }, function() {
                                        $scope.hideLoading();
                                    }
                                );
                        }
                    }, 100);
                        
                    return true;
                },
                'destructiveButtonClicked': function() {
                    $ionicPopup.confirm({
                        title: 'Borrar',
                        template: '<div>¿Desea borrar la tarjeta ' + label + '?</div>',
                        okType: 'button-payin'
                    })
                    .then(function(buttonOK) {
                        // no button = 0, 'CANCELAR' = 1, 'ACEPTAR' = 2
                        if(buttonOK) {
                            $scope.deleteCard(deviceType, item.id);
                        }
                    });
                    return true;
                }
            });

            $timeout(function() {
                hideSheet();
            }, 2000);
        };
    }
]);