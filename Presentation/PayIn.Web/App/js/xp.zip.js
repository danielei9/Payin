/*
 * bootstrap-filestyle
 * doc: http://markusslima.github.io/bootstrap-filestyle/
 * github: https://github.com/markusslima/bootstrap-filestyle
 */

'use strict';
app
.directive('xpZip', [function () {
	function apply($scope, zip, model) {
		$scope.files[model] = zip.generate({ type: "blob" });
		if (!$scope.$$phase)
			$scope.$apply();
	}

	return {
		scope: true,
		link: function ($scope, $element, $attrs) {
			var model = $attrs['xpZip'];
			var download = $attrs['xpDownload'];
			var inputs = $element.find('input');
			var maxSize = 26214400; // 25MB						

			angular.forEach(inputs, function (input) {
				// Add disabled
				var disabled = $(input).attr('ng-disabled');
				$(input).filestyle({ buttonText: "" });
				var label = $element.find('label[for="' + model + '"]');
				if (disabled && label) {
					$scope.$watch(disabled, function () {
						var val = $scope.$eval(disabled);
						$(input).filestyle('disabled', val);
					});
				}

				// Download
				if (download) {
					var button = '<label ng-show="' + download + '" class="btn btn-default"><span class="icon-span-filestyle glyphicon glyphicon-save" ng-href="{{' + download + '}}"></span></label>';

					var buttonList = $element.find('span[class="group-span-filestyle input-group-btn"]')[0];
					buttonList.innerHTML = button + buttonList.innerHTML;
				}

				// On change
				$(input).change(function (args) {
					var zip = new JSZip();
					var readers = [];
					var workingFiles = 0;
					var finished = false;
						
					angular.forEach(args.currentTarget.files, function (file) {
						//File Size 
						if (file.size) {
							var option = $element.find('#' + model);
							var fileSize = file.size;
							var ngModel = $(input).data('$ngModelController');
							ngModel.$setValidity('sizeMaximum', !((fileSize > maxSize)));							
						}
						else
							ngModel.$setValidity('sizeMaximum', true);

						// Add File
						var reader = new FileReader();
						readers.push(reader);
						workingFiles++;
						$scope.isBusy++;

						// On loaded
						reader.onload = function (evt, evt2) {
							var fileName = file.name;			
							zip.file(fileName, evt.target.result);
							workingFiles--;
							$scope.isBusy--;
								
							if (finished && (workingFiles === 0)) {
								finished = false; // se pone a false para que entre solo una vez
								apply($scope, zip, model);
							}
						};

						// Read
						reader.readAsArrayBuffer(file);

						finished = true;
						if (finished && (workingFiles === 0)) {
							finished = false; // se pone a false para que entre solo una vez
							apply($scope, zip, model);
						}
					});
				});
			});
		}
	};
}])	