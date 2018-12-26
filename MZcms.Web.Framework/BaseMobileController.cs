using Himall.Core;
using Himall.Core.Helper;
using Himall.IServices;
using Himall.Model;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MZcms.Web.Framework
{
    public abstract class BaseMobileController : BaseController
    {
        private static Dictionary<string, string> platformTypesStringMap;

        public UserMemberInfo CurrentUser
        {
            get
            {
                long num = UserCookieEncryptHelper.Decrypt(WebHelper.GetCookie("Himall-User"), "Mobile");
                if (num == 0)
                {
                    return null;
                }
                return ServiceHelper.Create<IMemberService>().GetMember(num);
            }
        }

        public UserMemberInfo CurrentUserAllPlatform
        {
            get
            {
                long num = UserCookieEncryptHelper.Decrypt(WebHelper.GetCookie("Himall-User"), "Mobile");
                if (num == 0)
                {
                    long num2 = UserCookieEncryptHelper.Decrypt(WebHelper.GetCookie("Himall-User"), "Web");
                    if (num2 != 0)
                    {
                        return ServiceHelper.Create<IMemberService>().GetMember(num2);
                    }
                    else
                    {
                        return null;
                    }
                }
                return ServiceHelper.Create<IMemberService>().GetMember(num);
            }
        }

        public Himall.Core.PlatformType PlatformType
        {
            get
            {
                string lower = base.RouteData.Values["platform"].ToString().ToLower();
                Dictionary<string, string> strs = BaseMobileController.platformTypesStringMap;
                Himall.Core.PlatformType platformType = Himall.Core.PlatformType.Mobile;
                if (strs.ContainsKey(lower))
                {
                    platformType = (Himall.Core.PlatformType)Enum.Parse(typeof(Himall.Core.PlatformType), strs[lower]);
                }
                return platformType;
            }
        }

        static BaseMobileController()
        {
        }

        public BaseMobileController()
        {
            if (BaseMobileController.platformTypesStringMap == null)
            {
                Dictionary<int, string> dictionary = EnumHelper.ToDictionary<Himall.Core.PlatformType>();
                BaseMobileController.platformTypesStringMap = new Dictionary<string, string>();
                foreach (KeyValuePair<int, string> value in dictionary)
                {
                    BaseMobileController.platformTypesStringMap[value.Value.ToLower()] = value.Value;
                }
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ((dynamic)base.ViewBag).AreaName = string.Format("m-{0}", PlatformType.ToString());
            ((dynamic)base.ViewBag).Logo = base.CurrentSiteSetting.Logo;
            ((dynamic)base.ViewBag).SiteName = base.CurrentSiteSetting.SiteName;
            string cookie = WebHelper.GetCookie("Himall-Mobile-AppType");
            string str = WebHelper.GetCookie("Himall-VShopId");
            if (cookie == string.Empty && filterContext.HttpContext.Request["shop"] != null)
            {
                cookie = filterContext.HttpContext.Request["shop"].ToString();
                long num = 0;
                if (long.TryParse(cookie, out num))
                {
                    WebHelper.SetCookie("Himall-VShopId", (ServiceHelper.Create<IVShopService>().GetVShopByShopId(num) ?? new VShopInfo()).Id.ToString());
                }
                WebHelper.SetCookie("Himall-Mobile-AppType", cookie);
            }
            ((dynamic)base.ViewBag).MAppType = cookie;
            ((dynamic)base.ViewBag).MVshopId = str;
            base.OnActionExecuting(filterContext);
        }
    }
}