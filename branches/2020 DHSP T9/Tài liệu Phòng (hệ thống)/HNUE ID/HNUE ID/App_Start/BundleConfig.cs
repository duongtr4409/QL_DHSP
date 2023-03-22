using System.Web.Optimization;

namespace Ums.Website
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;
            bundles.Add(new ScriptBundle("~/bundles/script").Include("~/Scripts/*.js"));
            bundles.Add(new StyleBundle("~/content/style").Include("~/Styles/*.css"));
        }
    }
}
