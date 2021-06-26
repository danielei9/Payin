// this is a lazy load controller, 
// so start with "app." to register this controller

app.filter('propsFilter', function () {
	return function (items, props) {
		var out = [];

		if (angular.isArray(items)) {
			items.forEach(function (item) {
				var itemMatches = false;

				var keys = Object.keys(props);
				for (var i = 0; i < keys.length; i++) {
					var prop = keys[i];
					var text = props[prop].toLowerCase();
					if (item[prop].toString().toLowerCase().indexOf(text) !== -1) {
						itemMatches = true;
						break;
					}
				}

				if (itemMatches) {
					out.push(item);
				}
			});
		} else {
			// Let the output be the input untouched
			out = items;
		}

		return out;
	};
})
app.controller('SelectCtrl', function ($scope, $http, $timeout) {
	$scope.disabled = undefined;
	$scope.searchEnabled = undefined;

	$scope.enable = function () {
		$scope.disabled = false;
	};

	$scope.disable = function () {
		$scope.disabled = true;
	};

	$scope.enableSearch = function () {
		$scope.searchEnabled = true;
	}

	$scope.disableSearch = function () {
		$scope.searchEnabled = false;
	}

	$scope.clear = function () {
		$scope.person.selected = undefined;	
	};

	$scope.someGroupFn = function (item) {

		if (item.name[0] >= 'A' && item.name[0] <= 'M')
			return 'From A - M';

		if (item.name[0] >= 'N' && item.name[0] <= 'Z')
			return 'From N - Z';

	};
	var chk = document.getElementById('checkSelect');
	$scope.$watch('value1', function () {
		var index = 0;
		$scope.clear();
		$scope.arguments.PaymentUserIds = [];
		if (chk.checked) {
			angular.forEach($scope.temp.users, function (value, key) {
				$scope.arguments.PaymentUserIds[index] = value.id;
				index++;
			});
		}
	});
 

	var chk = document.getElementById('checkAll');
	$scope.arguments.campaignAll = false;
	$scope.$watch('value2', function () {
		var min = document.getElementById('min');
		var max = document.getElementById('max');

		min.setAttribute("disabled", true);
		max.setAttribute("disabled", true);
		$scope.arguments.campaignAll != $scope.arguments.campaignAll;
		
	});

	$scope.counter = 0;
	$scope.someFunction = function (item, model) {
		$scope.counter++;
		$scope.eventResult = { item: item, model: model };
	};

	$scope.removed = function (item, model) {
		$scope.lastRemoved = {
			item: item,
			model: model
		};
	};

	$scope.person = {};
	
	$scope.multipleDemo = {};	
});