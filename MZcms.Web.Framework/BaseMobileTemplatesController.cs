using MZcms.Core;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace MZcms.Web.Framework
{
	public abstract class BaseMobileTemplatesController : BaseMobileController
	{
		protected BaseMobileTemplatesController()
		{
		}

		protected override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			ViewResult result = filterContext.Result as ViewResult;
			if (result != null)
			{
				string str = "Default";
				if (base.PlatformType == MZcms.Core.PlatformType.IOS || base.PlatformType == MZcms.Core.PlatformType.Android)
				{
					str = "APP";
				}
				string str1 = (string.IsNullOrEmpty(str) ? "" : str);
				string str2 = filterContext.RequestContext.RouteData.Values["Controller"].ToString();
				string str3 = filterContext.RequestContext.RouteData.Values["Action"].ToString();
				if (string.IsNullOrWhiteSpace(result.ViewName))
				{
					result.ViewName = string.Format("~/Areas/Mobile/Templates/{0}/Views/{1}/{2}.cshtml", str1, str2, str3);
					return;
				}
			}
			base.OnResultExecuting(filterContext);
		}
	}
}