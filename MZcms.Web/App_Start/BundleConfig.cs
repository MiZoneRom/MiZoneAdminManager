using System;
using System.Web.Optimization;

namespace MZcms.Web
{
	public class BundleConfig
	{
		public BundleConfig()
		{
		}

		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add((new ScriptBundle("~/bundles/jquery")).Include("~/Scripts/jquery-{version}.js", new IItemTransform[0]));
			bundles.Add((new ScriptBundle("~/bundles/jqueryval")).Include("~/Scripts/jquery.validate*", new IItemTransform[0]));
			ScriptBundle scriptBundle = new ScriptBundle("~/bundles/bootstrap");
			string[] strArrays = new string[] { "~/Scripts/bootstrap.js", "~/Scripts/respond.js" };
			bundles.Add(scriptBundle.Include(strArrays));
			StyleBundle styleBundle = new StyleBundle("~/Content/css");
			string[] strArrays1 = new string[] { "~/Content/bootstrap.css", "~/Content/site.css" };
			bundles.Add(styleBundle.Include(strArrays1));
			BundleTable.EnableOptimizations = true;
		}
	}
}