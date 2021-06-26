angular
.module('xp.navigation', [])
.service('xpNavigation', ['$rootScope', '$state', '$ionicHistory', 
    function ($rootScope, $state, $ionicHistory) {
        return {
            'goBackAndGo': function(backJumps, state, params)
            {
                var offState = $rootScope.$on('$stateChangeSuccess',
                    function(event, toState, toParams, fromState, fromParams) {
                    $state.go(state, params);
                    offState();
                    }
                );
                $ionicHistory.goBack(-1 * backJumps);
            },
            'goBack': function(backJumps)
            {
                $ionicHistory.goBack(-1 * backJumps);
            },
            'go': function(state, params)
            {
                $state.go(state, params);
            }
        };
    }
]);