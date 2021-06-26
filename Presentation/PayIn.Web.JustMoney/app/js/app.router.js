angular.module('justmoney')
.config(function ($stateProvider) {
	$stateProvider
		.state('home', { url: '', templateUrl: 'PrepaidCard' })
		// Account
		.state('accountlogin', { url: '/Account/Login', templateUrl: 'Account/Login' })
        .state('accountforgotpassword', { url: '/Account/ForgotPassword', templateUrl: 'Account/ForgotPassword', params: { email: '' } })
        .state('accountconfirmforgotpassword', { url: '/Account/ConfirmForgotPassword', templateUrl: 'Account/ConfirmForgotPassword', params: { userId: '', code: '' } })
		.state('accountresetpassword', {
			url: '/Account/ResetPassword', templateUrl: 'Account/ResetPassword', params: { email: '' }, controller: function ($scope, $stateParams, $state) {
				$scope.params = $stateParams;
			}
		})
		// PrepaidCard
		.state('prepaidcard', { url: '/PrepaidCard', templateUrl: 'PrepaidCard' })
		.state('prepaidcardcreate', { url: '/PrepaidCard/Create', templateUrl: 'PrepaidCard/Create' })
		.state('prepaidcardcreateandregister', { url: '/PrepaidCard/CreateAndRegister', templateUrl: 'PrepaidCard/CreateAndRegister' })
		.state('prepaidcardaddcard', { url: '/PrepaidCard/AddCard', templateUrl: 'PrepaidCard/AddCard' })
		.state('prepaidcardcreatecard', { url: '/PrepaidCard/CreateCard', templateUrl: 'PrepaidCard/CreateCard' })
		.state('prepaidcardregistercard', { url: '/PrepaidCard/RegisterCard', templateUrl: 'PrepaidCard/RegisterCard' })
		.state('prepaidcardenabledisable', { url: '/PrepaidCard/EnableDisable', templateUrl: 'PrepaidCard/EnableDisable' })
		.state('prepaidcardloadfunds', { url: '/PrepaidCard/LoadFunds', templateUrl: 'PrepaidCard/LoadFunds' })
		.state('prepaidcardlog', { url: '/PrepaidCard/Log', templateUrl: 'PrepaidCard/Log' })
		.state('prepaidcardpointsofrecharge', { url: '/PrepaidCard/PointsOfRecharge', templateUrl: 'PrepaidCard/PointsOfRecharge' })
		.state('prepaidcardrechargecard', { url: '/PrepaidCard/RechargeCard', templateUrl: 'PrepaidCard/RechargeCard' })
		.state('prepaidcardrechargedcard', { url: '/PrepaidCard/RechargedCard', templateUrl: 'PrepaidCard/RechargedCard' })
		.state('prepaidcardrechargedcarderror', { url: '/PrepaidCard/RechargedCardError', templateUrl: 'PrepaidCard/RechargedCardError' })
		.state('prepaidcardsharefunds', { url: '/PrepaidCard/ShareFunds', templateUrl: 'PrepaidCard/ShareFunds' })
		.state('prepaidcardtransfer', { url: '/PrepaidCard/Transfer', templateUrl: 'PrepaidCard/Transfer' })
		.state('prepaidcardupdate', { url: '/PrepaidCard/Update', templateUrl: 'PrepaidCard/Update' })
		.state('prepaidcardupgrade', { url: '/PrepaidCard/Upgrade/{id}', templateUrl: 'PrepaidCard/Upgrade' })
        ;
});