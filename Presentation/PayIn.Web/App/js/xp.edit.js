// Example of how to set default values for all dialogs
app.config(['ngDialogProvider', function (ngDialogProvider) {
	ngDialogProvider.setDefaults({
		className: 'ngdialog-theme-default',
		plain: false,
		showClose: true,
		closeByDocument: true,
		closeByEscape: true,
		appendTo: false,
		preCloseCallback: function (value) {
			var nestedConfirmDialog = ngDialog.openConfirm({
				template: '\
                <p>Are you sure you want to close the parent dialog?</p>\
                <div class="ngdialog-buttons">\
                    <button type="button" class="ngdialog-button ngdialog-button-secondary" ng-click="closeThisDialog(0)">No</button>\
                    <button type="button" class="ngdialog-button ngdialog-button-primary" ng-click="confirm(1)">Yes</button>\
                </div>',
				plain: true
			});	
			// NOTE: return the promise from openConfirm
			return nestedConfirmDialog;
		}
	});
}]);

app.controller('xpEdit', function ($scope, $rootScope, ngDialog, $timeout, $filter) {

	$scope.$watch('confirmValue', function (value) {	
		$rootScope.valor = value;
	}, true);

	$scope.$watch('confirmBlock', function (value) {
		$rootScope.valor = value;
	}, true);

	$scope.$watch('confirmCard', function (value) {
		$rootScope.valor = value;
	}, true);
	
	$scope.openConfirm = function (key) {
		$rootScope.key = key;
		ngDialog.openConfirm({
			template: 'modalDialogId',
			className: 'ngdialog-theme-default',
			scope : $scope
		});		
	};

	$scope.openConfirmBlock = function (key) {
		$rootScope.key = key;		
		ngDialog.openConfirm({
			template: 'modalDialogBlockId',
			className: 'ngdialog-theme-default',
			scope: $scope
		});
	};

	$scope.openConfirmCard = function () {		
		ngDialog.openConfirm({
			template: 'modalDialogCopyCardId',
			className: 'ngdialog-theme-card',
			scope: $scope
		});
	};
	
	$scope.openConfirm2 = function () {		
		ngDialog.openConfirm({
			template: 'secondDialogId',
			className: 'ngdialog-theme-default',
			scope: $scope
		});		

		for (var i = 0; i < $scope.arguments.modifyValues.length; i++)
		{
			if ($scope.arguments.modifyValues[i].key == $rootScope.key)
			{
				$scope.arguments.modifyValues.splice(i, 1);
				break;
		    }
		}

		$scope.arguments.modifyValues.push({
			key: $rootScope.key,
			value: $rootScope.valor
		});		
	};

	$scope.openConfirmBlock2 = function () {
		ngDialog.openConfirm({
			template: 'secondDialogBlockId',
			className: 'ngdialog-theme-default',
			scope: $scope
		});

		$scope.arguments.modifyBlock[$rootScope.key] = $rootScope.valor;
		var elementId = "#" + $rootScope.key;
		$(elementId).css('background-color', 'yellow');
		$scope.arguments.modifyCard = true;
	};

	$scope.openConfirmCard2 = function () {
		ngDialog.openConfirm({
			template: 'secondDialogCardId',
			className: 'ngdialog-theme-default',
			scope: $scope
		});
		$scope.arguments.modifyBlock = [];		
		$rootScope.valor = $rootScope.valor.split("\r\n").join("");
		$rootScope.valor = $rootScope.valor.split("\n").join("");

	    var i = 0;
		for (i = 0; i < $rootScope.valor.length; i = i + 32)		
			$scope.arguments.modifyBlock.push($rootScope.valor.substring(i, i+32));	

		$scope.arguments.modifyCard = true;
	}

	$scope.closeSecond = function () {
		ngDialog.close();
	}
});
