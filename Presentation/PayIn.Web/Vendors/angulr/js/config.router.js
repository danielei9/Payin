'use strict';

/**
 * Config for the router
 */
angular.module('app')
.run(
  ['$rootScope', '$state', '$stateParams',
    function ($rootScope, $state, $stateParams) {
      $rootScope.$state = $state;
      $rootScope.$stateParams = $stateParams;
    }
  ]
)
.config(
  ['$stateProvider', '$urlRouterProvider',
    function ($stateProvider, $urlRouterProvider) {
      $urlRouterProvider
				.otherwise('/app/dashboard-v1');
      $stateProvider
			.state('app', {
				abstract: true,
				url: '/app',
				templateUrl: 'Vendors/angulr/tpl/app.html'
			})
			.state('app.dashboard-v1', {
				url: '/dashboard-v1',
				templateUrl: 'Vendors/angulr/tpl/app_dashboard_v1.html',
				resolve: {
					deps: ['$ocLazyLoad',
						function ($ocLazyLoad) {
							return $ocLazyLoad.load(['Vendors/angulr/js/controllers/chart.js']);
						}]
					}
			})
			.state('app.dashboard-v2', {
				url: '/dashboard-v2',
				templateUrl: 'Vendors/angulr/tpl/app_dashboard_v2.html',
				resolve: {
					deps: ['$ocLazyLoad',
						function ($ocLazyLoad) {
							return $ocLazyLoad.load(['Vendors/angulr/js/controllers/chart.js']);
						}]
				}
			})
			.state('app.ui', {
				url: '/ui',
				template: '<div ui-view class="fade-in-up"></div>'
			})
			.state('app.ui.buttons', {
				url: '/buttons',
				templateUrl: 'Vendors/angulr/tpl/ui_buttons.html'
			})
			.state('app.ui.icons', {
				url: '/icons',
				templateUrl: 'Vendors/angulr/tpl/ui_icons.html'
			})
			.state('app.ui.grid', {
				url: '/grid',
				templateUrl: 'Vendors/angulr/tpl/ui_grid.html'
			})
			.state('app.ui.widgets', {
				url: '/widgets',
				templateUrl: 'Vendors/angulr/tpl/ui_widgets.html'
			})
			.state('app.ui.bootstrap', {
				url: '/bootstrap',
				templateUrl: 'Vendors/angulr/tpl/ui_bootstrap.html'
			})
			.state('app.ui.sortable', {
				url: '/sortable',
				templateUrl: 'Vendors/angulr/tpl/ui_sortable.html'
			})
			.state('app.ui.portlet', {
				url: '/portlet',
				templateUrl: 'Vendors/angulr/tpl/ui_portlet.html'
			})
			.state('app.ui.timeline', {
				url: '/timeline',
				templateUrl: 'Vendors/angulr/tpl/ui_timeline.html'
			})
			.state('app.ui.tree', {
				url: '/tree',
				templateUrl: 'Vendors/angulr/tpl/ui_tree.html',
				resolve: {
					deps: ['$ocLazyLoad',
						function ($ocLazyLoad) {
							return $ocLazyLoad.load('angularBootstrapNavTree').then(
								function () {
									return $ocLazyLoad.load('Vendors/angulr/js/controllers/tree.js');
								}
							);
						}
					]
				}
			})
			.state('app.ui.toaster', {
				url: '/toaster',
				templateUrl: 'Vendors/angulr/tpl/ui_toaster.html',
				resolve: {
					deps: ['$ocLazyLoad',
						function ($ocLazyLoad) {
							return $ocLazyLoad.load('toaster').then(
								function () {
									return $ocLazyLoad.load('Vendors/angulr/js/controllers/toaster.js');
								}
							);
						}
					]
				}
			})
			.state('app.ui.jvectormap', {
				url: '/jvectormap',
				templateUrl: 'Vendors/angulr/tpl/ui_jvectormap.html',
				resolve: {
					deps: ['$ocLazyLoad',
						function ($ocLazyLoad) {
							return $ocLazyLoad.load('Vendors/angulr/js/controllers/vectormap.js');
						}
					]
				}
			})
			.state('app.ui.googlemap', {
				url: '/googlemap',
				templateUrl: 'Vendors/angulr/tpl/ui_googlemap.html',
				resolve: {
					deps: ['uiLoad',
						function (uiLoad) {
							return uiLoad.load([
								'Vendors/angulr/js/app/map/load-google-maps.js',
								'Vendors/angulr/js/app/map/ui-map.js',
								'Vendors/angulr/js/app/map/map.js'
							]).then(
								function () {
									return loadGoogleMaps();
								}
							);
						}
					]
				}
			})
			.state('app.chart', {
				url: '/chart',
				templateUrl: 'Vendors/angulr/tpl/ui_chart.html',
				resolve: {
					deps: ['uiLoad',
						function (uiLoad) {
							return uiLoad.load('Vendors/angulr/js/controllers/chart.js');
						}
					]
				}
			})
			// table
			.state('app.table', {
				url: '/table',
				template: '<div ui-view></div>'
			})
			.state('app.table.static', {
				url: '/static',
				templateUrl: 'Vendors/angulr/tpl/table_static.html'
			})
			.state('app.table.datatable', {
				url: '/datatable',
				templateUrl: 'Vendors/angulr/tpl/table_datatable.html'
			})
			.state('app.table.footable', {
				url: '/footable',
				templateUrl: 'Vendors/angulr/tpl/table_footable.html'
			})
			.state('app.table.grid', {
				url: '/grid',
				templateUrl: 'Vendors/angulr/tpl/table_grid.html',
				resolve: {
					deps: ['$ocLazyLoad',
						function ($ocLazyLoad) {
							return $ocLazyLoad.load('ngGrid').then(
								function () {
									return $ocLazyLoad.load('Vendors/angulr/js/controllers/grid.js');
								}
							);
						}
					]
				}
			})
			// form
			.state('app.form', {
				url: '/form',
				template: '<div ui-view class="fade-in"></div>',
				resolve: {
					deps: ['uiLoad',
						function (uiLoad) {
							return uiLoad.load('Vendors/angulr/js/controllers/form.js');
						}
					]
				}
			})
			.state('app.form.elements', {
				url: '/elements',
				templateUrl: 'Vendors/angulr/tpl/form_elements.html'
			})
			.state('app.form.validation', {
				url: '/validation',
				templateUrl: 'Vendors/angulr/tpl/form_validation.html'
			})
			.state('app.form.wizard', {
				url: '/wizard',
				templateUrl: 'Vendors/angulr/tpl/form_wizard.html'
			})
			.state('app.form.fileupload', {
				url: '/fileupload',
				templateUrl: 'Vendors/angulr/tpl/form_fileupload.html',
				resolve: {
					deps: ['$ocLazyLoad',
						function ($ocLazyLoad) {
							return $ocLazyLoad.load('angularFileUpload').then(
								function () {
									return $ocLazyLoad.load('Vendors/angulr/js/controllers/file-upload.js');
								}
							);
						}
					]
				}
			})
			.state('app.form.imagecrop', {
				url: '/imagecrop',
				templateUrl: 'Vendors/angulr/tpl/form_imagecrop.html',
				resolve: {
					deps: ['$ocLazyLoad',
						function ($ocLazyLoad) {
							return $ocLazyLoad.load('ngImgCrop').then(
								function () {
									return $ocLazyLoad.load('Vendors/angulr/js/controllers/imgcrop.js');
								}
							);
						}
					]
				}
			})
			.state('app.form.select', {
				url: '/select',
				templateUrl: 'Vendors/angulr/tpl/form_select.html',
				controller: 'SelectCtrl',
				resolve: {
					deps: ['$ocLazyLoad',
						function ($ocLazyLoad) {
							return $ocLazyLoad.load('ui.select').then(
								function () {
									return $ocLazyLoad.load('Vendors/angulr/js/controllers/select.js');
								}
							);
						}
					]
				}
			})
			.state('app.form.slider', {
				url: '/slider',
				templateUrl: 'Vendors/angulr/tpl/form_slider.html',
				controller: 'SliderCtrl',
				resolve: {
					deps: ['$ocLazyLoad',
						function ($ocLazyLoad) {
							return $ocLazyLoad.load('vr.directives.slider').then(
								function () {
									return $ocLazyLoad.load('Vendors/angulr/js/controllers/slider.js');
								}
							);
						}
					]
				}
			})
			.state('app.form.editor', {
				url: '/editor',
				templateUrl: 'Vendors/angulr/tpl/form_editor.html',
				controller: 'EditorCtrl',
				resolve: {
					deps: ['$ocLazyLoad',
						function ($ocLazyLoad) {
							return $ocLazyLoad.load('textAngular').then(
								function () {
									return $ocLazyLoad.load('Vendors/angulr/js/controllers/editor.js');
								}
							);
						}
					]
				}
			})
			// pages
			.state('app.page', {
				url: '/page',
				template: '<div ui-view class="fade-in-down"></div>'
			})
			.state('app.page.profile', {
				url: '/profile',
				templateUrl: 'Vendors/angulr/tpl/page_profile.html'
			})
			.state('app.page.post', {
				url: '/post',
				templateUrl: 'Vendors/angulr/tpl/page_post.html'
			})
			.state('app.page.search', {
				url: '/search',
				templateUrl: 'Vendors/angulr/tpl/page_search.html'
			})
			.state('app.page.invoice', {
				url: '/invoice',
				templateUrl: 'tpl/page_invoice.html'
			})
			.state('app.page.price', {
				url: '/price',
				templateUrl: 'Vendors/angulr/tpl/page_price.html'
			})
			.state('app.docs', {
				url: '/docs',
				templateUrl: 'Vendors/angulr/tpl/docs.html'
			})
			// others
			.state('lockme', {
				url: '/lockme',
				templateUrl: 'Vendors/angulr/tpl/page_lockme.html'
			})
			.state('access', {
				url: '/access',
				template: '<div ui-view class="fade-in-right-big smooth"></div>'
			})
			.state('access.signin', {
				url: '/signin',
				templateUrl: 'Vendors/angulr/tpl/page_signin.html',
				resolve: {
					deps: ['uiLoad',
						function (uiLoad) {
							return uiLoad.load(['Vendors/angulr/js/controllers/signin.js']);
						}
					]
				}
			})
			.state('access.signup', {
				url: '/signup',
				templateUrl: 'Vendors/angulr/tpl/page_signup.html',
				resolve: {
					deps: ['uiLoad',
						function (uiLoad) {
							return uiLoad.load(['Vendors/angulr/js/controllers/signup.js']);
						}
					]
				}
			})
		
			.state('access.forgotPwd', {
				url: '/ForgotPwd',
				templateUrl: 'Vendors/angulr/tpl/page_forgotpwd.html'
			})
			.state('access.404', {
				url: '/404',
				templateUrl: 'Vendors/angulr/tpl/page_404.html'
			})
			// fullCalendar
			.state('app.calendar', {
				url: '/calendar',
				templateUrl: 'Vendors/angulr/tpl/app_calendar.html',
				// use resolve to load other dependences
				resolve: {
					deps: ['$ocLazyLoad', 'uiLoad',
						function ($ocLazyLoad, uiLoad) {
							return uiLoad.load(
								['Vendors/angulr/vendor/jquery/fullcalendar/fullcalendar.css',
									'Vendors/angulr/vendor/jquery/fullcalendar/theme.css',
									'Vendors/angulr/vendor/jquery/jquery-ui-1.10.3.custom.min.js',
									'Vendors/angulr/vendor/libs/moment.min.js',
									'Vendors/angulr/vendor/jquery/fullcalendar/fullcalendar.min.js',
									'Vendors/angulr/js/app/calendar/calendar.js']
							).then(
								function () {
									return $ocLazyLoad.load('ui.calendar');
								}
							)
						}
					]
				}
			})
			// mail
			.state('app.mail', {
				abstract: true,
				url: '/mail',
				templateUrl: 'Vendors/angulr/tpl/mail.html',
				// use resolve to load other dependences
				resolve: {
					deps: ['uiLoad',
						function (uiLoad) {
							return uiLoad.load(['Vendors/angulr/js/app/mail/mail.js',
																	'Vendors/angulr/js/app/mail/mail-service.js',
																	'Vendors/angulr/vendor/libs/moment.min.js']);
						}
					]
				}
			})
			.state('app.mail.list', {
				url: '/inbox/{fold}',
				templateUrl: 'Vendors/angulr/tpl/mail.list.html'
			})
			.state('app.mail.detail', {
				url: '/{mailId:[0-9]{1,4}}',
				templateUrl: 'Vendors/angulr/tpl/mail.detail.html'
			})
			.state('app.mail.compose', {
				url: '/compose',
				templateUrl: 'Vendors/angulr/tpl/mail.new.html'
			})
			.state('layout', {
				abstract: true,
				url: '/layout',
				templateUrl: 'Vendors/angulr/tpl/layout.html'
			})
			.state('layout.fullwidth', {
				url: '/fullwidth',
				views: {
					'': {
						templateUrl: 'Vendors/angulr/tpl/layout_fullwidth.html'
					},
					'footer': {
						templateUrl: 'Vendors/angulr/tpl/layout_footer_fullwidth.html'
					}
				},
				resolve: {
					deps: ['uiLoad',
						function (uiLoad) {
							return uiLoad.load(['Vendors/angulr/js/controllers/vectormap.js']);
						}
					]
				}
			})
			.state('layout.mobile', {
				url: '/mobile',
				views: {
					'': {
						templateUrl: 'Vendors/angulr/tpl/layout_mobile.html'
					},
					'footer': {
						templateUrl: 'Vendors/angulr/tpl/layout_footer_mobile.html'
					}
				}
			})
			.state('layout.app', {
				url: '/app',
				views: {
					'': {
						templateUrl: 'Vendors/angulr/tpl/layout_app.html'
					},
					'footer': {
						templateUrl: 'Vendors/angulr/tpl/layout_footer_fullwidth.html'
					}
				},
				resolve: {
					deps: ['uiLoad',
						function (uiLoad) {
							return uiLoad.load(['Vendors/angulr/js/controllers/tab.js']);
						}
					]
				}
			})
			.state('apps', {
				abstract: true,
				url: '/apps',
				templateUrl: 'Vendors/angulr/tpl/layout.html'
			})
			.state('apps.note', {
				url: '/note',
				templateUrl: 'Vendors/angulr/tpl/apps_note.html',
				resolve: {
					deps: ['uiLoad',
						function (uiLoad) {
							return uiLoad.load(['Vendors/angulr/js/app/note/note.js',
																	'Vendors/angulr/vendor/libs/moment.min.js']);
						}]
				}
			})
			.state('apps.contact', {
				url: '/contact',
				templateUrl: 'Vendors/angulr/tpl/apps_contact.html',
				resolve: {
					deps: ['uiLoad',
						function (uiLoad) {
							return uiLoad.load(['Vendors/angulr/js/app/contact/contact.js']);
						}]
				}
			})
			.state('app.weather', {
				url: '/weather',
				templateUrl: 'Vendors/angulr/tpl/apps_weather.html',
				resolve: {
					deps: ['$ocLazyLoad',
						function ($ocLazyLoad) {
							return $ocLazyLoad.load({
								name: 'angular-skycons',
								files: ['Vendors/angulr/js/app/weather/skycons.js',
												'Vendors/angulr/vendor/libs/moment.min.js',
												'Vendors/angulr/js/app/weather/angular-skycons.js',
												'Vendors/angulr/js/app/weather/ctrl.js']
							});
						}
					]
				}
			})
			.state('music', {
				url: '/music',
				templateUrl: 'Vendors/angulr/tpl/music.html',
				controller: 'MusicCtrl',
				resolve: {
					deps: ['$ocLazyLoad',
						function ($ocLazyLoad) {
							return $ocLazyLoad.load([
								'com.2fdevs.videogular',
								'com.2fdevs.videogular.plugins.controls',
								'com.2fdevs.videogular.plugins.overlayplay',
								'com.2fdevs.videogular.plugins.poster',
								'com.2fdevs.videogular.plugins.buffering',
								'Vendors/angulr/js/app/music/ctrl.js',
								'Vendors/angulr/js/app/music/theme.css'
							]);
						}]
				}
			})
			.state('music.home', {
				url: '/home',
				templateUrl: 'Vendors/angulr/tpl/music.home.html'
			})
			.state('music.genres', {
				url: '/genres',
				templateUrl: 'Vendors/angulr/tpl/music.genres.html'
			})
			.state('music.detail', {
				url: '/detail',
				templateUrl: 'Vendors/angulr/tpl/music.detail.html'
			})
			.state('music.mtv', {
				url: '/mtv',
				templateUrl: 'Vendors/angulr/tpl/music.mtv.html'
			})
			.state('music.mtvdetail', {
				url: '/mtvdetail',
				templateUrl: 'Vendors/angulr/tpl/music.mtv.detail.html'
			})
			.state('music.playlist', {
				url: '/playlist/{fold}',
				templateUrl: 'Vendors/angulr/tpl/music.playlist.html'
			})
    }
  ]
);