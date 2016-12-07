using System.Web;
using System.Web.Optimization;

namespace S2B
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/js/jquery-ui-1.8.16.custom.min.js",
                      "~/Scripts/js/jquery.easing.1.3.js",
                      "~/Scripts/js/jquery.inview.min.js/",
                      "~/Scripts/js/jquery.sinusoid.js",
                      "~/Scripts/js/jquery.tmpl.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Styles/css").Include(
                      "~/Styles/bootstrap.css",
                      "~/Styles/css/animate.css",
                      "~/Styles/eden.css",
                      "~/Styles/style.css",
                      "~/Styles/default.css"

                      ));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(                       
                        "~/Scripts/angular.min.js",
                        "~/Scripts/js/VendaAngulaApp.js"
                        ));
        }
    }
}
