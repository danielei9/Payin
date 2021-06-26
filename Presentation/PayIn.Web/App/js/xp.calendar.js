app
.directive('xpCalendar', function () {	
	return {
		restrict: 'AE',
		scope: true,
		//transclude: true,
		link: function ($scope, $element, $attrs) {
			$scope.$watch('data', function (list) {					
					$scope.events.length = 0;
					$scope.eventSources = [];
					angular.forEach(list, function (item) {
						if (item.checkDuration || item.sumChecks != '00:00:00') {
							$scope.events.push({
								id: item.id,
								title: item.checkDuration ?
									moment(item.sumChecks, 'HH:mm:ss').format('HH:mm') + " / " + moment(item.checkDuration, 'HH:mm:ss').format('HH:mm') :
									moment(item.sumChecks, 'HH:mm:ss').format('HH:mm'),
								start: item.date,
								//end: end,
								allDay: true,
								className: [''],
								url: item.id ?
									"/#/ControlPlanning/Update/" + item.id :
									"",
								deleteUrl: ""
							});
						}

						angular.forEach(item.items, function (region) {
							var start = moment(region.start || region.end, 'YYYY-MM-DDTHH:mmZ');
							var end = moment(region.end || region.start, 'YYYY-MM-DDTHH:mmZ');

							var className = 'b-l b-2x b-dark';
							var updateUrl = "";							
							//var deleteUrl = "";
							if (region.type == 0) {
								className = 'b-l b-2x b-orange';								
								updateUrl = "/#/ControlPlanningItem/Update/" + region.id;
								//deleteUrl = "ControlPlanningItem/Delete";
							} else if (region.type == 1) {							
								className='b-l b-2x b-info';
							} else if (region.type == 2) {								
								className='b-l b-2x b-success';
								updateUrl = "/#/ControlPlanningCheck/Update/" + region.id;
								//deleteUrl = "controlplanningcheckdelete";
							}

							$scope.events.push({
								id: region.id,
								title: region.title,
								start: start,
								startText: region.start ? moment(region.start, 'YYYY-MM-DDTHH:mmZ').format('YYYY-MM-DD HH:mm') : '',
								end: end,
								endText: region.end ? moment(region.end, 'YYYY-MM-DDTHH:mmZ').format('YYYY-MM-DD HH:mm') : '',
								allDay: false,
								className: [className],
								location: region.location,
								info: region.info,
								url: updateUrl,								
								//deleteUrl: deleteUrl,
								entranceCheckType: region.entranceCheckType,
								exitCheckType: region.exitCheckType
							});
						});
					});						
			});			
		},
		controller: ['$scope', '$compile', function ($scope, $compile) {
			/* event source that pulls from google.com */
			$scope.eventSource = {
				// url: "http://www.google.com/calendar/feeds/usa__en%40holiday.calendar.google.com/public/basic", festivos de EEUU
				//url: "http://www.google.com/calendar/feeds/spain__es%40holiday.calendar.google.com/public/basic", //festivos de España
				className: 'gcal-event',           // an option!
				currentTimezone: 'Europe/Madrid' // an option!            

			};

			// event source that contains custom events on the scope 
			$scope.events = [];

			$scope.overlay = $('.fc-overlay');
			$scope.alertOnMouseOver = function (event, jsEvent, view) {
				$scope.event = event;
				$scope.overlay.removeClass('left right').find('.arrow').removeClass('left right top pull-up');

				//if ($scope.overlay.find('a.delete').length > 0)
				//	$scope.overlay.find('a.delete').remove();
				//if (event.deleteUrl) {
				//	var elem = event.end ?
				//		angular.element(
				//			"<a data-xp-modal=\"" + event.deleteUrl + "\" data-xp-id='" + event.id + "' data-xp-arguments='{\"since\":\"" + event.start.format('YYYY-MM-DD HH:mm') + "\",\"until\":\"" + event.end.format('YYYY-MM-DD HH:mm') + "\"}' class=\"delete pull-right\" data-xp-navigate>" +
				//			"<i class=\"glyphicon glyphicon-trash\" style=\"font-size:14px;\"></i>" +
				//			"</a>") :
				//		angular.element(
				//			"<a data-xp-modal=\"" + event.deleteUrl + "\" data-xp-id='" + event.id + "' data-xp-arguments='{\"date\":\"" + event.start.format('YYYY-MM-DD') + "\"}' class=\"delete pull-right\" data-xp-navigate>" +
				//			"<i class=\"glyphicon glyphicon-trash\" style=\"font-size:14px;\"></i>" +
				//			"</a>");
				//	$compile(elem)($scope);
				//	$scope.overlay.find('h4').append(elem);
				//}

				var wrap = $(jsEvent.target).closest('.fc-event');
				var cal = wrap.closest('.calendar');
				var left = wrap.offset().left -cal.offset().left;
				var right = cal.width() -(wrap.offset().left -cal.offset().left +wrap.width());
				if (right > $scope.overlay.width())
					$scope.overlay.addClass('left').find('.arrow').addClass('left pull-up')
				else if (left > $scope.overlay.width())
					$scope.overlay.addClass('right').find('.arrow').addClass('right pull-up');
				else
					$scope.overlay.find('.arrow').addClass('top');
				(wrap.find('.fc-overlay').length == 0) && wrap.append($scope.overlay);
			}

			/* config object */
			$scope.uiConfig = {
				calendar: {
					height: 550,
					//editable: true,
					editable: false,
					lang: 'es',
					header: {
						left: 'prev',
						center: 'title',
						right: 'next'
					},
					firstDay: 1,
					//dayClick: $scope.alertOnEventClick,
					//eventDrop: $scope.alertOnDrop,
						//eventResize: $scope.alertOnResize,
					//eventClick: function(a,b,c) {
					//	window.location.href = 'www.google.es';
					//},
					eventMouseover: $scope.alertOnMouseOver,
					viewRender: function (view, element) {						
						$scope.arguments.since = view.start.format('YYYY-MM-DD');
						$scope.arguments.until = view.end.format('YYYY-MM-DD');
						$scope.search();						
					}
				}
			};

			/* Change View */
			$scope.changeView = function (view, calendar) {
				$('.calendar').fullCalendar('changeView', view);
			};

			$scope.today = function (calendar) {
				$('.calendar').fullCalendar('today');
			};

			$scope.filteredItems = {				
				events: $scope.events
			};

			//Event sources array			
			$scope.eventSources = [];	
			$scope.filteredItems.events = $scope.events;
			$scope.eventSources.push($scope.filteredItems);			
		}]
	};
})