using MZcms.Core.Helper;
using MZcms.IServices;
using MZcms.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace MZcms.Web.Framework
{
	public abstract class BaseAdminController : BaseController
	{
		public IPaltManager CurrentManager
		{
			get
			{
				IPaltManager platformManager = null;
				long num = UserCookieEncryptHelper.Decrypt(WebHelper.GetCookie("MZcms-PlatformManager"), "Admin");
				if (num != 0)
				{
					platformManager = ServiceHelper.Create<IManagerService>().GetPlatformManager(num);
				}
				return platformManager;
			}
		}

		protected BaseAdminController()
		{
		}

		protected override void Initialize(RequestContext requestContext)
		{
			base.Initialize(requestContext);
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext.IsChildAction)
			{
				return;
			}
			filterContext.RouteData.Values["controller"].ToString().ToLower();
			filterContext.RouteData.Values["action"].ToString().ToLower();
		}

		protected override void OnAuthorization(AuthorizationContext filterContext)
		{
			base.InitVisitorTerminal();
			string item = ConfigurationManager.AppSettings["IsInstalled"];
			if (item != null && !bool.Parse(item))
			{
				return;
			}
			if (filterContext.IsChildAction)
			{
				return;
			}
			if (CurrentManager == null)
			{
				if (!WebHelper.IsAjax())
				{
					RedirectToRouteResult action = base.RedirectToAction("", "Login", new { area = "admin" });
					filterContext.Result = action;
					return;
				}
				BaseController.Result result = new BaseController.Result()
				{
					msg = "登录超时,请重新登录！",
					success = false
				};
				filterContext.Result = base.Json(result);
				return;
			}
			if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(UnAuthorize), false).Length == 1)
			{
				return;
			}
			string lower = filterContext.RouteData.Values["controller"].ToString().ToLower();
			string str = filterContext.RouteData.Values["action"].ToString().ToLower();
			if (CurrentManager.AdminPrivileges == null || CurrentManager.AdminPrivileges.Count == 0 || !AdminPermission.CheckPermissions(CurrentManager.AdminPrivileges, lower, str))
			{
				if (WebHelper.IsAjax())
				{
					BaseController.Result result1 = new BaseController.Result()
					{
						msg = "你没有访问的权限！",
						success = false
					};
					filterContext.Result = base.Json(result1);
					return;
				}
				ViewResult viewResult = new ViewResult()
				{
					ViewName = "NoAccess"
				};
				viewResult.TempData.Add("Message", "你没有权限访问此页面");
				viewResult.TempData.Add("Title", "你没有权限访问此页面！");
				filterContext.Result = viewResult;
			}
		}

		protected ActionResult SuccessfulRedirectView(string viewName, object routeData = null)
		{
			return base.RedirectToAction(viewName, routeData);
		}
	}
}