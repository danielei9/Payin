angular
.module('xp.deploy', ['ionic'])
.service('xpDeploy', ['$ionicPopup', 
    function($ionicPopup) {
        var deploy = new Ionic.Deploy();
        
        return {
            version: "",
            setChannel: function(channel) {
                console.log('Ionic Deploy: Set Channel! ', channel);
                deploy.setChannel(channel);
            },
            check: function(success) {
                var that = this;
                deploy.info()
                .then(
                    function(info) {
                        that.version = info.binary_version;
                        console.log('Ionic Info: ', info);
                    },
                    function() {},
                    function() {}
                );
                
                deploy.check()
                .then(
                    function(response) { // response will be true if there is an update
                        console.log('Ionic Deploy: Check Success! ', response);
                        if (response && success)
                            success(that.version);
                    },
                    function(error) {
                        console.log('Ionic Deploy: Check Error! ', error);
                    }
                );
            },
            update: function(version) {
                var that = this;
                deploy.download()
                .then(
                    function() {
                        console.log('Ionic Deploy: Download Success!');
                        deploy.extract()
                        .then(
                            function() {
                                console.log('Ionic Deploy: Extract Success! App updated in next reload');
                                $ionicPopup.confirm({
                                    title: 'Nueva versión Pay[in]',
                                    template: version ?
                                        'Se ha detectado una la versión ' + version + ', ¿desea actualizarla?':
                                        'Se ha detectado una nueva versión, ¿desea actualizarla?'
                                })
                                .then(function(res) {
                                    if (res) {
                                        deploy.load();
                                    }
                                });
                            }, function(error) {
                                console.log('Ionic Deploy: Extract Error! ', error);
                            }, function(progress) {
                                console.log('Ionic Deploy: Extract Progress... ', progress);
                            }
                        );
                    }, function(error) {
                        console.log('Ionic Deploy: Download Error! ', error);
                    }, function(progress) {
                        console.log('Ionic Deploy: Download Progress... ', progress);
                    }
                );
                // deploy.update() // App reloaded automatically
                // .then(
                //     function(result) {
                //         console.log('Ionic Deploy: Update an Reload Success! ', result);
                //     }, function(error) {
                //         console.log('Ionic Deploy: Update Error! ', error);
                //     }, function(progress) {
                //         console.log('Ionic Deploy: Progress... ', progress);
                //     }
                // );
            }
        };
    }
])
.directive('xpDeploy', function () {
    return {
        restrict: 'A',
        controller: ['$scope', '$ionicPlatform', 'xpDeploy', '$attrs',
            function($scope, $ionicPlatform, xpDeploy, $attrs) {
                var channel = $attrs['xpDeploy'];
                
                $scope.xpDeploy = {};
                angular.extend($scope.xpDeploy, xpDeploy);
                
                $ionicPlatform.ready(function() {
                    if (channel)
                        $scope.xpDeploy.setChannel(channel);
                        
                    $scope.xpDeploy.check(
                        $scope.xpDeploy.update
                    );
                });
            }
        ]
    }
});