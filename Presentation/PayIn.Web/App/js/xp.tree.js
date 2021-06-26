// http://bootstrap-table.wenzhixin.net.cn/
'use strict';
app
.directive('xpTree', function () {
	return {
		restrict: 'A',
		priority: 100,
		scope: true,
		link: function ($scope, $element, $attrs)
        {
            //$element
            //    .find('.collapse-toggle')
            //    .on('shown.bs.collapse', function () {
            //        $(this)
            //            .prev()
            //            .find(".glyphicon")
            //            .removeClass("glyphicon-chevron-right")
            //            .addClass("glyphicon-chevron-down");
            //    });
            //$element
            //    .find('.collapse-toggle')
            //    .on('hidden.bs.collapse', function () {
            //        $(this)
            //            .prev()
            //            .find(".glyphicon")
            //            .removeClass("glyphicon-chevron-down")
            //            .addClass("glyphicon-chevron-right");
            //    });

            //$($element)
            //    .find('.glyphicon')
            //    .click(function () {
            //        $(this)
            //            .toggleClass("glyphicon-chevron-down")
            //            .toggleClass("glyphicon-chevron-right");
            //        });
		},
		controller: ['$scope', '$attrs', '$element', function ($scope, $attrs, $element) {
		}]
	};
});