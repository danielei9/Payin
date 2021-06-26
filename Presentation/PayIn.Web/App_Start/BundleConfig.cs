using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace PayIn.Web
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			// jQuery
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
				"~/Vendors/angulr/vendor/jquery/jquery.js",
				"~/Vendors/underscore/underscore.js"
			));

			// Angular
			bundles.Add(new ScriptBundle("~/bundles/angularjs").Include(
				"~/Vendors/angulr/vendor/angular/angular.js",
				"~/Vendors/angulr/vendor/angular/angular-animate/angular-animate.js",
				"~/Vendors/angulr/vendor/angular/angular-cookies/angular-cookies.js",
				"~/Vendors/angulr/vendor/angular/angular-resource/angular-resource.js",
				"~/Vendors/angulr/vendor/angular/angular-sanitize/angular-sanitize.js",
				"~/Vendors/angulr/vendor/angular/angular-touch/angular-touch.js",
				"~/Vendors/angulr/vendor/angular/angular-ui-router/angular-ui-router.js",
				"~/Vendors/angulr/vendor/modules/angular-ui-select/select.js",				
				"~/Vendors/angulr/vendor/angular/angular-ui-utils/ui-utils.js",
				"~/Vendors/angular-file-upload/angular-file-upload.js",
				"~/Vendors/bootstrap-filestyle/bootstrap-filestyle.js",
				"~/Vendors/angulr/js/app/map/load-google-maps.js",
				"~/Vendors/angulr/js/app/map/jsapi.js",
				"~/Vendors/angular-google-chart/ng-google-chart.js",
				"~/Vendors/angulr/vendor/angular/ngstorage/ngStorage.js",
				"~/Vendors/ngDialog/ngDialog.min.js",
				"~/Vendors/countup/angular-countUp.min.js",
				"~/Vendors/countup/countUp.min.js"
			));

			// Bootstrap
			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
				"~/Vendors/momentjs/moment-with-locales.min.js",
				"~/Vendors/momentjs/moment-timezone-with-data.min.js",
				"~/Scripts/bootstrap.js",
				"~/Vendors/angulr/vendor/angular/angular-bootstrap/ui-bootstrap-tpls.js",
				"~/Vendors/malot-bootstrap-datetimepicker/bootstrap-datetimepicker.js",
                "~/Vendors/bootstrap-table-develop/dist/bootstrap-table.min.js",
                "~/Vendors/bootstrap-table-develop/dist/locale/bootstrap-table-es-ES.min.js",
                "~/Vendors/ng-img-crop/ng-img-crop.js",
				"~/Vendors/jszip/jszip.js"
			));
			bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
				"~/Vendors/angulr/css/bootstrap.css",
				"~/Vendors/malot-bootstrap-datetimepicker/bootstrap-datetimepicker.min.css",
                "~/Vendors/bootstrap-table-develop/dist/bootstrap-table.min.css",
                "~/Vendors/ng-img-crop/ng-img-crop.css",
				"~/Vendors/ngDialog/ngDialog.min.css",
				"~/Vendors/ngDialog/ngDialog-theme-default.min.css",
				"~/Vendors/ngDialog/ngDialog-theme-card.min.css",
				"~/Vendors/flickity/flickity.css"
			));

			// Markdown
			bundles.Add(new ScriptBundle("~/bundles/markdown").Include(
				"~/Vendors/textAngular/dist/textAngular-rangy.min.js",
				"~/Vendors/textAngular/dist/textAngular-sanitize.min.js",
				"~/Vendors/textAngular/dist/textAngular.min.js",
				"~/Vendors/upndown/upndown.bundle.min.js",
				"~/Vendors/showdown/showdown.min.js"
                //"~/Vendors/angulr/vendor/modules/textAngular/textAngular-rangy.min.js",
                //"~/Vendors/angulr/vendor/modules/textAngular/textAngular-sanitize.min.js",
                //"~/Vendors/angulr/vendor/modules/textAngular/textAngular.min.js"
            ));
			bundles.Add(new StyleBundle("~/Content/markdown").Include(
				"~/Vendors/textAngular/dist/textAngular.css"
                //"~/Vendors/angulr/vendor/modules/textAngular/textAngular.css"
            ));

			// Lazyload
			bundles.Add(new ScriptBundle("~/bundles/lazyload").Include(
				"~/Vendors/angulr/vendor/angular/oclazyload/ocLazyLoad.js"
			));

			// Translate
			bundles.Add(new ScriptBundle("~/bundles/translate").Include(
				"~/Vendors/angulr/vendor/angular/angular-translate/angular-translate.old.js",
				"~/Vendors/angulr/vendor/angular/angular-translate/loader-static-files.js",
				"~/Vendors/angulr/vendor/angular/angular-translate/storage-cookie.js",
				"~/Vendors/angulr/vendor/angular/angular-translate/storage-local.js"
			));
			
			// Xp
			bundles.Add(new ScriptBundle("~/bundles/xp").Include(
 				"~/Vendors/angulr/js/app.js",				
				"~/app/js/app.config.js",				
				"~/app/js/app.config.lazyload.js",
				"~/app/js/app.config.router.js",
                "~/app/js/app.js",
				"~/app/js/xp.communication.js",
				"~/app/js/xp.authentication.js",
				"~/app/js/xp.redsys.js",
				"~/app/js/xp.js",
				"~/app/js/payin.js",
				"~/app/js/xp.file.js",
				"~/app/js/xp.zip.js",
                "~/app/js/xp.tree.js",
				"~/app/js/xp.map.js",
				"~/app/js/xp.chart.js",
				"~/app/js/xp.graph.js",
				"~/Vendors/angulr/js/services/ui-load.js",
				"~/Vendors/angulr/js/filters/fromNow.js",
				"~/Vendors/angulr/js/directives/setnganimate.js",
				"~/Vendors/angulr/js/directives/ui-butterbar.js",
				"~/Vendors/angulr/js/directives/ui-focus.js",
				"~/Vendors/angulr/js/directives/ui-fullscreen.js",
				"~/Vendors/angulr/js/directives/ui-jq.js",
				"~/Vendors/angulr/js/directives/ui-module.js",
				"~/Vendors/angulr/js/directives/ui-nav.js",
				"~/Vendors/angulr/js/directives/ui-scroll.js",
				"~/Vendors/angulr/js/directives/ui-shift.js",
				"~/Vendors/angulr/js/directives/ui-toggleclass.js",
				"~/Vendors/angulr/js/directives/ui-validate.js",
				"~/Vendors/angulr/js/controllers/bootstrap.js",
                "~/app/js/xp.markdown.editor.js"
            ));
			bundles.Add(new StyleBundle("~/Content/xp").Include(
				"~/Vendors/angulr/css/animate.css",
				//"~/Vendors/angulr/css/font-awesome.min.css",
				"~/Vendors/angulr/css/simple-line-icons.css",
				"~/Vendors/angulr/css/font.css",			
				"~/Vendors/angulr/css/app.css"
			));

			// Xp.authentication
			bundles.Add(new ScriptBundle("~/bundles/xp.authentication").Include(
				"~/Vendors/angulr/js/app.js",				
				"~/app/js/app.config.js",				
				"~/app/js/app.config.lazyload.js",
				"~/app/js/app.config.router.js",
				"~/app/js/app.js",
				"~/app/js/xp.communication.js",
				"~/app/js/xp.authentication.js",
				//"~/app/js/payin.js"
				"~/Vendors/angulr/js/services/ui-load.js",
				"~/Vendors/angulr/js/filters/fromNow.js",
				"~/Vendors/angulr/js/directives/setnganimate.js",
				"~/Vendors/angulr/js/directives/ui-butterbar.js",
				"~/Vendors/angulr/js/directives/ui-focus.js",
				"~/Vendors/angulr/js/directives/ui-fullscreen.js",
				"~/Vendors/angulr/js/directives/ui-jq.js",
				"~/Vendors/angulr/js/directives/ui-module.js",
				"~/Vendors/angulr/js/directives/ui-nav.js",
				"~/Vendors/angulr/js/directives/ui-scroll.js",
				"~/Vendors/angulr/js/directives/ui-shift.js",
				"~/Vendors/angulr/js/directives/ui-toggleclass.js",
				"~/Vendors/angulr/js/directives/ui-validate.js",
				"~/Vendors/angulr/js/controllers/bootstrap.js",
				"~/Vendors/angulr/vendor/libs/moment.min.js"
			));

			// Xp.shop
			bundles.Add(new ScriptBundle("~/bundles/xp.shop").Include(
				"~/Vendors/angulr/js/app.js",
				"~/app/js/app.config.js",
				"~/app/js/app.config.lazyload.js",
				"~/app/js/app.config.router.js",
				"~/app/js/app.js",
				"~/app/js/xp.communication.js",
				"~/app/js/xp.shop.js",
				//"~/app/js/payin.js"
				"~/Vendors/angulr/js/services/ui-load.js",
				"~/Vendors/angulr/js/filters/fromNow.js",
				"~/Vendors/angulr/js/directives/setnganimate.js",
				"~/Vendors/angulr/js/directives/ui-butterbar.js",
				"~/Vendors/angulr/js/directives/ui-focus.js",
				"~/Vendors/angulr/js/directives/ui-fullscreen.js",
				"~/Vendors/angulr/js/directives/ui-jq.js",
				"~/Vendors/angulr/js/directives/ui-module.js",
				"~/Vendors/angulr/js/directives/ui-nav.js",
				"~/Vendors/angulr/js/directives/ui-scroll.js",
				"~/Vendors/angulr/js/directives/ui-shift.js",
				"~/Vendors/angulr/js/directives/ui-toggleclass.js",
				"~/Vendors/angulr/js/directives/ui-validate.js",
				"~/Vendors/angulr/js/controllers/bootstrap.js",
				"~/Vendors/angulr/vendor/libs/moment.min.js"
			));

			// App
			bundles.Add(new ScriptBundle("~/bundles/app").Include(
			));
			bundles.Add(new StyleBundle("~/Content/app").Include(
#if VINAROS
				"~/app/payin.css",
				"~/app/vinaros.css"
#else
				"~/app/payin.css"
#endif
			));
			//bundles.Add(new ScriptBundle("~/bundles/app").Include(
				//"~/Vendors/mgCrud-master/dist/mgcrud.js",
				//"~/Scripts/sammy-{version}.js",
				//"~/Scripts/app/common.js",
				//"~/Scripts/app/app.datamodel.js",
				//"~/Scripts/app/app.viewmodel.js",
				//"~/Scripts/app/home.viewmodel.js",
				//"~/Scripts/app/_run.js"
			//));

			// Set EnableOptimizations to false for debugging. For more information,
			// visit http://go.microsoft.com/fwlink/?LinkId=301862
			BundleTable.EnableOptimizations = false;
		}
	}
}
