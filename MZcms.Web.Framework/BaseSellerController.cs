using Himall.Core.Helper;
using Himall.IServices;
using Himall.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MZcms.Web.Framework
{
	public abstract class BaseSellerController : BaseWebController
	{
		public new SiteSettingsInfo CurrentSiteSetting
		{
			get
			{
				return ServiceHelper.Create<ISiteSettingService>().GetSiteSettings();
			}
		}

		protected BaseSellerController()
		{
		}

		private bool CheckAuthorization(AuthorizationContext filterContext)
		{
			bool flag = true;
			if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(UnAuthorize), false).Length == 1)
			{
				return true;
			}
			string str = filterContext.RouteData.Values["controller"].ToString();
			string str1 = filterContext.RouteData.Values["action"].ToString();
			if (base.CurrentSellerManager.SellerPrivileges == null || base.CurrentSellerManager.SellerPrivileges.Count == 0 || !SellerPermission.CheckPermissions(base.CurrentSellerManager.SellerPrivileges, str, str1))
			{
				if (!WebHelper.IsAjax())
				{
					ViewResult viewResult = new ViewResult()
					{
						ViewName = "NoAccess"
					};
					viewResult.TempData.Add("Message", "你没有权限访问此页面");
					viewResult.TempData.Add("Title", "你没有权限访问此页面！");
					filterContext.Result = viewResult;
					flag = false;
				}
				else
				{
					BaseController.Result result = new BaseController.Result()
					{
						msg = "你没有访问的权限！",
						success = false
					};
					filterContext.Result = base.Json(result);
					flag = false;
				}
			}
			return flag;
		}

		private bool CheckLoginStatus(AuthorizationContext filterContext)
		{
			bool flag = true;
			if (base.CurrentSellerManager == null && base.CurrentUser == null)
			{
				if (!WebHelper.IsAjax())
				{
					HttpRequestBase request = filterContext.HttpContext.Request;
					string str = HttpUtility.HtmlEncode(request.RawUrl.ToString());
					RedirectToRouteResult action = base.RedirectToAction("", "Login", new { area = "web", returnUrl = str });
					filterContext.Result = action;
					flag = false;
				}
				else
				{
					BaseController.Result result = new BaseController.Result()
					{
						msg = "登录超时,请重新登录！",
						success = false
					};
					filterContext.Result = base.Json(result);
					flag = false;
				}
			}
			else if (base.CurrentUser != null && base.CurrentSellerManager == null)
			{
				RedirectToRouteResult redirectToRouteResult = base.RedirectToAction("EditProfile0", "ShopProfile", new { area = "SellerAdmin" });
				filterContext.Result = redirectToRouteResult;
				flag = false;
			}
			return flag;
		}

		private bool CheckRegisterInfo(AuthorizationContext filterContext)
		{
			bool flag = true;
			if (filterContext.IsChildAction || WebHelper.IsAjax())
			{
				return flag;
			}
			filterContext.RouteData.Values["controller"].ToString().ToLower();
			string lower = filterContext.RouteData.Values["action"].ToString().ToLower();
			filterContext.RouteData.DataTokens["area"].ToString().ToLower();
			ShopInfo shop = ServiceHelper.Create<IShopService>().GetShop(base.CurrentSellerManager.ShopId, false);
			int valueOrDefault = (int)shop.Stage.GetValueOrDefault();
			ShopInfo.ShopStage? stage = shop.Stage;
			if (((stage.GetValueOrDefault() != ShopInfo.ShopStage.Finish ? true : !stage.HasValue) || shop.ShopStatus == ShopInfo.ShopAuditStatus.WaitConfirm) && filterContext.RequestContext.HttpContext.Request.HttpMethod.ToUpper() != "POST" && lower.IndexOf("step") != 0 && lower != string.Concat("EditProfile", valueOrDefault).ToLower())
			{
				RedirectToRouteResult action = base.RedirectToAction(string.Concat("EditProfile", valueOrDefault), "ShopProfile", new { area = "SellerAdmin" });
				filterContext.Result = action;
				flag = false;
			}
			return flag;
		}

		private bool CheckShopIsExpired(AuthorizationContext filterContext)
		{
			bool flag = true;
			if (ServiceHelper.Create<IShopService>().IsExpiredShop(base.CurrentSellerManager.ShopId))
			{
				ViewResult viewResult = new ViewResult()
				{
					ViewName = "IsExpired"
				};
				viewResult.TempData.Add("Message", "你的店铺已过期");
				viewResult.TempData.Add("Title", "你的店铺已过期！");
				filterContext.Result = viewResult;
				flag = false;
			}
			return flag;
		}

		protected override void OnAuthorization(AuthorizationContext filterContext)
		{
			base.OnAuthorization(filterContext);
			string item = ConfigurationManager.AppSettings["IsInstalled"];
			if (item != null && !bool.Parse(item))
			{
				return;
			}
			if (filterContext.IsChildAction)
			{
				return;
			}
			base.OnAuthorization(filterContext);
			if (CheckLoginStatus(filterContext) && CheckAuthorization(filterContext) && CheckRegisterInfo(filterContext))
			{
                CheckShopIsExpired(filterContext);
			}
		}
	}
}