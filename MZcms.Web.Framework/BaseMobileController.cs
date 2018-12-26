using MZcms.Core;
using MZcms.Core.Helper;
using MZcms.IServices;
using MZcms.Model;
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

        //public UserMemberInfo CurrentUser
        //{
        //    get
        //    {
        //        long num = UserCookieEncryptHelper.Decrypt(WebHelper.GetCookie("MZcms-User"), "Mobile");
        //        if (num == 0)
        //        {
        //            return null;
        //        }
        //        return ServiceHelper.Create<IMemberService>().GetMember(num);
        //    }
        //}

        //public UserMemberInfo CurrentUserAllPlatform
        //{
        //    get
        //    {
        //        long num = UserCookieEncryptHelper.Decrypt(WebHelper.GetCookie("MZcms-User"), "Mobile");
        //        if (num == 0)
        //        {
        //            long num2 = UserCookieEncryptHelper.Decrypt(WebHelper.GetCookie("MZcms-User"), "Web");
        //            if (num2 != 0)
        //            {
        //                return ServiceHelper.Create<IMemberService>().GetMember(num2);
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //        return ServiceHelper.Create<IMemberService>().GetMember(num);
        //    }
        //}

        public MZcms.Core.PlatformType PlatformType
        {
            get
            {
                string lower = base.RouteData.Values["platform"].ToString().ToLower();
                Dictionary<string, string> strs = BaseMobileController.platformTypesStringMap;
                MZcms.Core.PlatformType platformType = MZcms.Core.PlatformType.Mobile;
                if (strs.ContainsKey(lower))
                {
                    platformType = (MZcms.Core.PlatformType)Enum.Parse(typeof(MZcms.Core.PlatformType), strs[lower]);
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
                Dictionary<int, string> dictionary = EnumHelper.ToDictionary<MZcms.Core.PlatformType>();
                BaseMobileController.platformTypesStringMap = new Dictionary<string, string>();
                foreach (KeyValuePair<int, string> value in dictionary)
                {
                    BaseMobileController.platformTypesStringMap[value.Value.ToLower()] = value.Value;
                }
            }
        }

        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    ((dynamic)base.ViewBag).AreaName = string.Format("m-{0}", PlatformType.ToString());
        //    ((dynamic)base.ViewBag).Logo = base.CurrentSiteSetting.Logo;
        //    ((dynamic)base.ViewBag).SiteName = base.CurrentSiteSetting.SiteName;
        //    string cookie = WebHelper.GetCookie("MZcms-Mobile-AppType");
        //    string str = WebHelper.GetCookie("MZcms-VShopId");
        //    if (cookie == string.Empty && filterContext.HttpContext.Request["shop"] != null)
        //    {
        //        cookie = filterContext.HttpContext.Request["shop"].ToString();
        //        long num = 0;
        //        if (long.TryParse(cookie, out num))
        //        {
        //            WebHelper.SetCookie("MZcms-VShopId", (ServiceHelper.Create<IVShopService>().GetVShopByShopId(num) ?? new VShopInfo()).Id.ToString());
        //        }
        //        WebHelper.SetCookie("MZcms-Mobile-AppType", cookie);
        //    }
        //    ((dynamic)base.ViewBag).MAppType = cookie;
        //    ((dynamic)base.ViewBag).MVshopId = str;
        //    base.OnActionExecuting(filterContext);
        //}
    }
}