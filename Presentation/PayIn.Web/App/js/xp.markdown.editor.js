'use strict';
app
    .directive('xpMarkdownEditor', [function () {
        return {
            restrict: 'A',
            //scope: true,
            //transclude: true,
            link: function ($scope, $element, $attrs) {
                var htmlModel = $attrs["ngModel"];
                var markdownModel = $attrs["xpMarkdownEditor"];

                var isIn = false;
                var lastHtml = "";
                $scope.$watch(markdownModel, function (markdown) {
                    if (!isIn) {
                        var converter = new showdown.Converter();
						var html = converter.makeHtml(markdown) || "";

						html = html.replace(new RegExp("'", 'g'), "\\'");
                        $scope.$eval(htmlModel + "='" + html + "'");
                        lastHtml = html;
                    }

                    isIn = false;
                });
				$scope.$watch(htmlModel, function (html) {
					html = html.replace(new RegExp(" </b></i>", 'g'), "</b></i> ");
					html = html.replace(new RegExp(" </i></b>", 'g'), "</i></b> ");
					html = html.replace(new RegExp(" </b>", 'g'), "</b> ");
					html = html.replace(new RegExp(" </strong>", 'g'), "</strong> ");
					html = html.replace(new RegExp(" </em>", 'g'), "</em> ");
					html = html.replace(new RegExp(" </i>", 'g'), "</i> ");

                    if (lastHtml != html) {
                        lastHtml = html;

                        var converter = new upndown();
                        converter.convert(html, function (err, markdown) {
                            if (err) {
                                console.error(err);
                            } else {
								isIn = true;

								markdown = markdown.replace(new RegExp("'", 'g'), "\\'");
                                $scope.$eval(markdownModel + "='" + markdown + "'");
                            }
                        });
                        // Al terminar este watch se lanza el anterior que pone el isIn a false automáticamente
                    }
                });
            }
        };
    }])
    .controller('wysiwygeditor', ['$scope', 'textAngularManager', function wysiwygeditor($scope, textAngularManager) {
        $scope.orightml = '<h2>Try me!</h2><p>textAngular is a super cool WYSIWYG Text Editor directive for AngularJS</p>' +
        '<p> <img class="ta-insert-video" ta-insert-video="http://www.youtube.com/embed/2maA1-mvicY" src="" allowfullscreen="true" width="300" frameborder="0" height="250" /></p >' +
            '<p><b>Features:</b></p> <ol><li>Automatic Seamless Two-Way-Binding</li>' + '<li>Super Easy <b>Theming</b> Options</li>' +
            '<li style="color: green;">Simple Editor Instance Creation</li><li>Safely Parses Html for Custom Toolbar Icons</li>' +
            '<li class="text-danger">Doesn&apos;t Use an iFrame</li><li>Works with Firefox, Chrome, and IE9+</li></ol> <p>' +
            '<b>Code at GitHub:</b> <a href="https://github.com/fraywing/textAngular">Here</a> </p>';
        $scope.htmlcontent = $scope.orightml;
        $scope.disabled = false;
        $scope.answer = function () {
            //console.log($scope.htmlcontent);
        }
    }]);