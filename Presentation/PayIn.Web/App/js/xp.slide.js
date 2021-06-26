'use strict';
app.directive('xpSlide', function () {
	return {
		restrict: 'EA',
		priority: 100,		
		scope: true,
		link: function ($scope, $element, $attrs) {

			$scope.max;			

			$scope.$watch('data', function () {
				if ($scope.data[0]) {
					$scope.max = $scope.data[0].items.length;					 
				} else {
					$scope.max = 1;
				}
			});


			$element.bind('click', function (ev) {
				
				var tooltip = $('.slider.slider-horizontal').find('.tooltip.top');
				var min = $('.slider-track').find('.slider-handle.min-slider-handle.round');
				var inner = tooltip.find('.tooltip-inner');

				var max = $('.slider-track').find('.slider-handle.max-slider-handle.round');

				var data = inner.text();
				var split= data.split(":");

				var minimo = split[0] - 1;
				var maximo =( $scope.data[0].items.length - 1 )|| 1;
				
				
				var new_start = moment.utc($scope.data[0].items[minimo].date, [moment.ISO_8601]).format('YYYY-MM-DD HH:mm:ss');			
				var new_end = moment.utc($scope.data[0].items[maximo].date, [moment.ISO_8601]).format('YYYY-MM-DD HH:mm:ss');
				 
			

				$scope.arguments.start = new_start;
				$scope.arguments.end = new_end;

				$scope.$apply();
				

				$(".slider-selection").css('width', '100%');
				$(".slider-selection").css('left', '0%');
				$('.slider-track').children().eq(2).css('left', '100%');
				$('.slider-track').children().eq(1).css('left', '0%');		

			});							
		}
		
	};
});
