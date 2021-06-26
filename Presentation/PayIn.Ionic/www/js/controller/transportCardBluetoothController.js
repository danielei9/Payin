angular
    .module('app')
    .controller('transportCardBluetoothController', 
                ['$rootScope', '$scope', '$ionicPlatform', 'xpCommunication', '$state','$http','$cordovaBluetoothSerial',
                 function($rootScope, $scope, $ionicPlatform, xpCommunication, $state, $http, $cordovaBluetoothSerial) {

                     $ionicPlatform.ready(function() {
                         /*var metawear = 
                            
                            serviceUUID: "6f00e8fe53b804ce",
                            txCharacteristic: "326a9001-85cb-9195-d9dd-464cfbbae75a", // transmit is from the phone's perspective
                            rxCharacteristic: "326a9006-85cb-9195-d9dd-464cfbbae75a"  // receive is from the phone's perspective
                        };*/
                         //console.log( window.device.uuid );
/*
                         ble.scan(
                             [ ],
                             3000,
                             function(device) {
                            //    console.log(JSON.stringify(device));
                                $scope.devices.push(device);
                             }
                         );*/
                         //ble.scan(, 200, $scope.onConnect,);
//                         
//                         $cordovaBluetoothSerial.discoverUnpaired().then(function(devices) {
//                             devices.forEach(function(devices) {
//                                 
//                             })
//                         });
//                         $cordovaBluetoothSerial.setDeviceDiscoveredListener().then(function(devices) {
//                             devices.forEach(function(devices) {
//                                 alert(devices.address + " " + devices.name);
//                             })
//                         });
                     });


                     $scope.connect = function(address){
                         ble.isEnabled(
                          bluetoothSerial.connect(address, $scope.onConnect, $scope.onDisconnect)
                             );
                     };


                     $scope.onConnect =  function() {
                         alert("connection established.");
                     }; 

                     $scope.onDisconnect = function(data) {
                                                 ble.isConnected(
                            'FFCA0B09-CB1D-4DC0-A1EF-31AFD3EDFB53',
                            function() {
                                console.log("Peripheral is connected");
                            },
                            function() {
                                console.log("Peripheral is *not* connected");
                            }
                        );
                     };
                 }                        
                ]);