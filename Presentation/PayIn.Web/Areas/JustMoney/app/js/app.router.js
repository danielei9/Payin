angular.module('justmoney')
.config(function ($stateProvider) {
	$stateProvider
	.state({ name: 'home', url: '', template: '' })
        .state({ name: 'usergetall', url: 'JustMoney/User', templateUrl: 'JustMoney/User' })
	;
});