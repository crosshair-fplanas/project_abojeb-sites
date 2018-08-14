using System.Web;
using System.Web.Optimization;

namespace AbojebApi
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

            bundles.Add(new ScriptBundle("~/bundles/unobtrusive").Include(
                        "~/Scripts/jquery.unobtrusive*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/vendors/bootstrap/js/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/fontawesome").Include(
                      "~/Content/vendors/fontawesome/js/all.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/plugins").Include(
                      "~/Scripts/plugins/fastselect.standalone.min.js",
                      "~/Scripts/plugins/moment.min.js",
                      "~/Scripts/plugins/daterangepicker.js",
                      "~/Scripts/plugins/datatables.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/plugins/fastselect.min.css",
                      "~/Content/plugins/daterangepicker.css",
                      "~/Content/plugins/datatables.min.css",
                      "~/Content/vendors/bootstrap/css/bootstrap.css",
                      "~/Content/vendors/fontawesome/css/all.min.css",
                      "~/Content/styles/site.css",
                      "~/Content/styles/style.css"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                      "~/Scripts/app.js"));
        }
    }
}
