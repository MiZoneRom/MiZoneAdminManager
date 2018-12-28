using MZcms.Core.Helper;
using MZcms.Model;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MZcms.Web.Framework
{
	public abstract class BaseMemberController : BaseWebController
	{
		protected BaseMemberController()
		{
		}

		protected int GetRouteInt(string key, int defaultValue)
		{
			return TypeHelper.ObjectToInt(base.RouteData.Values[key], defaultValue);
		}

		protected int GetRouteInt(string key)
		{
			return GetRouteInt(key, 0);
		}

		protected string GetRouteString(string key, string defaultValue)
		{
			object item = base.RouteData.Values[key];
			if (item == null)
			{
				return defaultValue;
			}
			return item.ToString();
		}

		protected string GetRouteString(string key)
		{
			return GetRouteString(key, "");
		}

		protected override void Initialize(RequestContext requestContext)
		{
			base.Initialize(requestContext);
		}

		protected override void OnAuthorization(AuthorizationContext filterContext)
		{
			base.OnAuthorization(filterContext);
			if (filterContext.IsChildAction)
			{
				return;
			}
			if (base.CurrentUser == null || base.CurrentUser.Disabled)
			{
				if (WebHelper.IsAjax())
				{
					BaseController.Result result = new BaseController.Result()
					{
						msg = "登录超时,请重新登录！",
						success = false
					};
					filterContext.Result = base.Json(result);
					return;
				}
				HttpRequestBase request = filterContext.HttpContext.Request;
				string str = HttpUtility.HtmlEncode(request.RawUrl.ToString());
				RedirectToRouteResult action = base.RedirectToAction("", "Login", new { area = "Web", returnUrl = str });
				if (!base.IsMobileTerminal)
				{
					filterContext.Result = action;
				}
			}
		}
	}
}