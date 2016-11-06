using System.Web;
using System.Web.Optimization;

namespace SportsManager
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            /********* Javascript Bundles *********/
            
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/phone").Include(
                      "~/Scripts/intlTelInput.min.js",
                      "~/Scripts/untils.js"));

            /********* CSS Bundles *********/

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/bootstrap.css",
                      "~/Content/css/Site.css"));

            bundles.Add(new StyleBundle("~/Content/css/jqueryui").Include(
                      "~/Content/themes/base/core.css",
                      "~/Content/themes/base/autocomplete.css",
                      "~/Content/themes/base/datepicker.css",
                      "~/Content/themes/base/theme.css",
                      "~/Content/themes/base/jquery-ui.css"));
        }
    }
}
