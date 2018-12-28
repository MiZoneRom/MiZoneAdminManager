﻿using MZcms.Application;
using MZcms.CommonModel;
using MZcms.Core;
using MZcms.Core.Helper;
using MZcms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MZcms.Web.Framework
{
    /// <summary>
    /// 移动端控制器基类
    /// </summary>
    public abstract class BaseMobileController : BaseController
    {
        private static Dictionary<string, string> platformTypesStringMap;

        public BaseMobileController()
        {
            if (platformTypesStringMap == null)
            {
                var types = EnumHelper.ToDictionary<PlatformType>();
                platformTypesStringMap = new Dictionary<string, string>();
                foreach (var type in types)
                    platformTypesStringMap[type.Value.ToLower()] = type.Value;
            }
        }

        private CommonModel.Model.WeiXinShareArgs _weiXinShareArgs;

        public long UserId
        {
            get
            {
                if (CurrentUser == null)
                    return 0;
                return CurrentUser.Id;
            }
        }

        /// <summary>
        /// 当前访问的平台类型
        /// </summary>
        public PlatformType PlatformType
        {
            get
            {
                var platformTypeLowerString = RouteData.Values["platform"].ToString().ToLower();//获取平台类型传入值并转为小写

                var mapper = platformTypesStringMap;//获取枚举小写与标准值之间的对应关系

                //获取对应的枚举
                var platformType = PlatformType.Mobile;
                if (visitorTerminalInfo.Terminal == EnumVisitorTerminal.WeiXin)
                {
                    platformTypeLowerString = "weixin";
                }
                if (mapper.ContainsKey(platformTypeLowerString))
                    platformType = (PlatformType)Enum.Parse(typeof(PlatformType), mapper[platformTypeLowerString]);
                return platformType;
            }
        }

        public CommonModel.Model.WeiXinShareArgs WeiXinShareArgs
        {
            get
            {
                if (_weiXinShareArgs == null)
                    _weiXinShareArgs = Application.WXApiApplication.GetWeiXinShareArgs(this.HttpContext.Request.Url.AbsoluteUri);
                return _weiXinShareArgs;
            }
        }

        protected override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            ViewBag.AreaName = string.Format("m-{0}", PlatformType.ToString());
            ViewBag.Logo = CurrentSiteSetting.Logo;
            ViewBag.SiteName = CurrentSiteSetting.SiteName;
            //区分平台还是商家
            var MAppType = WebHelper.GetCookie(CookieKeysCollection.MobileAppType);
            var MVshopId = WebHelper.GetCookie(CookieKeysCollection.MZcms_VSHOPID);
            if (MAppType == string.Empty)
            {
                if (filterContext.HttpContext.Request["shop"] != null)
                {//微信菜单中是否存在店铺ID
                    MAppType = filterContext.HttpContext.Request["shop"].ToString();
                    long shopid = 0;
                    if (long.TryParse(MAppType, out shopid))
                    {//记录当前微店（从微信菜单进来，都带有shop参数）
                        var vshop = VshopApplication.GetVShopByShopId(shopid) ?? new VShopInfo() { };
                        WebHelper.SetCookie(CookieKeysCollection.MZcms_VSHOPID, vshop.Id.ToString());
                    }
                    WebHelper.SetCookie(CookieKeysCollection.MobileAppType, MAppType);
                }
            }
            ViewBag.MAppType = MAppType;
            ViewBag.MVshopId = MVshopId;
            if (!filterContext.IsChildAction && !filterContext.HttpContext.Request.IsAjaxRequest())
            {
                //统计代码
                StatisticApplication.StatisticPlatVisitUserCount();
            }
            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            foreach (var viewEngine in ViewEngineCollection)
            {
                if (viewEngine is RazorViewEngine)
                {
                    var razorViewEngine = (RazorViewEngine)viewEngine;
                    if (!razorViewEngine.ViewLocationFormats.Any(p => p.StartsWith("~/Areas/Mobile/Templates/Default")))
                    {
                        var viewLocationFormats = new[]
                        {
                            "~/Areas/Mobile/Templates/Default/Views/Shared/{0}.cshtml",
                            "~/Areas/Mobile/Templates/Default/Views/{1}/{0}.cshtml"
                        };
                        razorViewEngine.PartialViewLocationFormats = razorViewEngine.PartialViewLocationFormats.Concat(viewLocationFormats).ToArray();
                        razorViewEngine.ViewLocationFormats = razorViewEngine.ViewLocationFormats.Concat(viewLocationFormats).ToArray();
                    }
                }
            }
            base.OnActionExecuted(filterContext);
        }

        public ActionResult RedirectToUrl(string url)
        {
            return Redirect(url);
        }
    }
}
