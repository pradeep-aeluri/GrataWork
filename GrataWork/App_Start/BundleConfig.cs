using System.Web;
using System.Web.Optimization;

namespace GrataWork
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
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/angular-datatables").Include(
                "~/Scripts/angular-datatables/angular-datatables.js",
                "~/Scripts/angular-datatables/angular-datatables.util.js",
                "~/Scripts/angular-datatables/angular-datatables.options.js",
                "~/Scripts/angular-datatables/angular-datatables.instances.js",
                "~/Scripts/angular-datatables/angular-datatables.factory.js",
                "~/Scripts/angular-datatables/angular-datatables.renderer.js",
                "~/Scripts/angular-datatables/angular-datatables.directive.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular-gratawork").Include(
                "~/Scripts/angular/services/*.service.js",
                "~/Scripts/angular/controllers/*.controller.js"
                ));
        }
    }
}
