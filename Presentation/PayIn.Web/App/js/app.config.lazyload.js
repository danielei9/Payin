// lazyload config

angular.module('app')
    /**
   * jQuery plugin config use ui-jq directive , config the js and css files that required
   * key: function name of the jQuery plugin
   * value: array of the css js file located
   */
  .constant('JQ_CONFIG', {
  	easyPieChart: ['Vendors/angulr/vendor/jquery/charts/easypiechart/jquery.easy-pie-chart.js'],
  	sparkline:    ['Vendors/angulr/vendor/jquery/charts/sparkline/jquery.sparkline.min.js'],
  	plot:					['Vendors/angulr/vendor/jquery/charts/flot/jquery.flot.min.js',
		               'Vendors/angulr/vendor/jquery/charts/flot/jquery.flot.resize.js',
		               'Vendors/angulr/vendor/jquery/charts/flot/jquery.flot.tooltip.min.js',
		               'Vendors/angulr/vendor/jquery/charts/flot/jquery.flot.spline.js',
		               'Vendors/angulr/vendor/jquery/charts/flot/jquery.flot.orderBars.js',
		               'Vendors/angulr/vendor/jquery/charts/flot/jquery.flot.pie.min.js'],
  	slimScroll:   ['Vendors/angulr/vendor/jquery/slimscroll/jquery.slimscroll.min.js'],
  	sortable:     ['Vendors/angulr/vendor/jquery/sortable/jquery.sortable.js'],
  	nestable:     ['Vendors/angulr/vendor/jquery/nestable/jquery.nestable.js',
		               'Vendors/angulr/vendor/jquery/nestable/nestable.css'],
  	//filestyle:    ['Vendors/bootstrap-filestyle/bootstrap-filestyle.js'],
  	slider:       ['Vendors/angulr/vendor/jquery/slider/bootstrap-slider.js',
		               'Vendors/angulr/vendor/jquery/slider/slider.css'],
  	chosen:       ['Vendors/angulr/vendor/jquery/chosen/chosen.jquery.min.js',
		               'Vendors/angulr/vendor/jquery/chosen/chosen.css'],
  	TouchSpin:    ['Vendors/angulr/vendor/jquery/spinner/jquery.bootstrap-touchspin.min.js',
		               'Vendors/angulr/vendor/jquery/spinner/jquery.bootstrap-touchspin.css'],
  	wysiwyg:      ['Vendors/angulr/vendor/jquery/wysiwyg/bootstrap-wysiwyg.js',
		               'Vendors/angulr/vendor/jquery/wysiwyg/jquery.hotkeys.js'],
  	dataTable:    ['Vendors/angulr/vendor/jquery/datatables/jquery.dataTables.min.js',
		               'Vendors/angulr/vendor/jquery/datatables/dataTables.bootstrap.js',
		               'Vendors/angulr/vendor/jquery/datatables/dataTables.bootstrap.css'],
  	vectorMap:    ['Vendors/angulr/vendor/jquery/jvectormap/jquery-jvectormap.min.js',
		               'Vendors/angulr/vendor/jquery/jvectormap/jquery-jvectormap-world-mill-en.js',
		               'Vendors/angulr/vendor/jquery/jvectormap/jquery-jvectormap-us-aea-en.js',
		               'Vendors/angulr/vendor/jquery/jvectormap/jquery-jvectormap.css'],
  	footable:     ['Vendors/angulr/vendor/jquery/footable/footable.all.min.js',
		               'Vendors/angulr/vendor/jquery/footable/footable.core.css']
  })
  // oclazyload config
  .config(['$ocLazyLoadProvider', function ($ocLazyLoadProvider) {
  	// We configure ocLazyLoad to use the lib script.js as the async loader
  	$ocLazyLoadProvider.config({
  		debug: false,
  		events: true,
  		modules: [
				{
					name: 'ngGrid',
					files: ['Vendors/angulr/vendor/modules/ng-grid/ng-grid.min.js',
					        'Vendors/angulr/vendor/modules/ng-grid/ng-grid.min.css',
					        'Vendors/angulr/vendor/modules/ng-grid/theme.css']
				},
				{
					name: 'ui.select',
					files: ['Vendors/angulr/vendor/modules/angular-ui-select/select.min.js',
					        'Vendors/angulr/vendor/modules/angular-ui-select/select.min.css']
				},
				//{
				//	name: 'angularFileUpload',
				//	files: ['Vendors/angular-file-upload/angular-file-upload.min.js']
				//},
				{
					name: 'ui.calendar',
					files: ['Vendors/angulr/vendor/modules/angular-ui-calendar/calendar.js']
				},
				{
					name: 'ngImgCrop',
					files: ['Vendors/ngImgCrop/ng-img-crop.js',
					        'Vendors/ngImgCrop/ng-img-crop.css']
				},				
				{
					name: 'angularBootstrapNavTree',
					files: ['Vendors/angulr/vendor/modules/angular-bootstrap-nav-tree/abn_tree_directive.js',
					        'Vendors/angulr/vendor/modules/angular-bootstrap-nav-tree/abn_tree.css']
				},
				{
					name: 'toaster',
					files: ['Vendors/angulr/vendor/modules/angularjs-toaster/toaster.js',
					        'Vendors/angulr/vendor/modules/angularjs-toaster/toaster.css']
				},
				{
					name: 'textAngular',
					files: [
                        'Vendors/angulr/vendor/modules/textAngular/textAngular-rangy.min.j',
                        'Vendors/angulr/vendor/modules/textAngular/textAngular-sanitize.min.js',
						'Vendors/angulr/vendor/modules/textAngular/textAngular.min.js'
					]
					//files: ['/bower_components/textAngular/dist/textAngular-rangy.min.js',
					//		'/bower_components/textAngular/dist/textAngular-sanitize.min.js',
					//	'/bower_components/textAngular/dist/textAngular.min.js']
					,
					serie: true
				},
				{
					name: 'vr.directives.slider',
					files: ['Vendors/angulr/vendor/modules/angular-slider/angular-slider.min.js',
					        'Vendors/angulr/vendor/modules/angular-slider/angular-slider.css']
				},
				{
					name: 'com.2fdevs.videogular',
					files: ['Vendors/angulr/vendor/modules/videogular/videogular.min.js']
				},
				{
					name: 'com.2fdevs.videogular.plugins.controls',
					files: ['Vendors/angulr/vendor/modules/videogular/plugins/controls.min.js']
				},
				{
					name: 'com.2fdevs.videogular.plugins.buffering',
					files: ['Vendors/angulr/vendor/modules/videogular/plugins/buffering.min.js']
				},
				{
					name: 'com.2fdevs.videogular.plugins.overlayplay',
					files: ['Vendors/angulr/vendor/modules/videogular/plugins/overlay-play.min.js']
				},
				{
					name: 'com.2fdevs.videogular.plugins.poster',
					files: ['Vendors/angulr/vendor/modules/videogular/plugins/poster.min.js']
				},
				{
					name: 'com.2fdevs.videogular.plugins.imaads',
					files: ['Vendors/angulr/vendor/modules/videogular/plugins/ima-ads.min.js']
				},
				{
					name: 'jszip',
					files: ['Vendors/jszip/jszip.min.js']
				}
  		]
  	});
  }])
;