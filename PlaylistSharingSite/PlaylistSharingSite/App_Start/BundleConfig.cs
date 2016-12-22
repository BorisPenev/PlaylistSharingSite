using System.Web;
using System.Web.Optimization;

namespace PlaylistSharingSite
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jPlayer/lib/jquery.min.js",
                        "~/Scripts/jPlayer/dist/jquery.jplayer.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Customly Added
            //bundles.Add(new ScriptBundle("~/bundles/jPlayer").Include(
            //            "~/bundles/jPlayer/dist/jquery.jplayer.min.js",
            //            "~/bundles/jPlayer/lib/jquery.min.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/jPlayer/css/jplayer.blue.monday.min.css",
            //          "~/Content/jPlayer/image/jplayer.blue.monday.jpg",
            //          "~/Content/jPlayer/image/jplayer.blue.monday.seeking.gif",
            //          "~/Content/jPlayer/image/jplayer.blue.monday.video.play.png"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css",
                      "~/Content/jPlayer/css/jplayer.blue.monday.min.css",
                      "~/Content/jPlayer/image/jplayer.blue.monday.jpg",
                      "~/Content/jPlayer/image/jplayer.blue.monday.seeking.gif",
                      "~/Content/jPlayer/image/jplayer.blue.monday.video.play.png"));
        }
    }
}
