using System;
using System.Web.Mvc;

namespace MZcms.Web
{
	public class FilterConfig
	{
		public FilterConfig()
		{
		}

		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new GZipAttribute());
			filters.Add(new HandleErrorAttribute());
		}
	}
}