using MZcms.Core.Helper;
using MZcms.IServices;
using MZcms.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MZcms.Web.Framework
{
    public abstract class BaseExpressController : BaseWebController
    {
        //public new SiteSettingsInfo CurrentSiteSetting
        //{
        //    get
        //    {
        //        return ServiceHelper.Create<ISiteSettingService>().GetSiteSettings();
        //    }
        //}

        protected BaseExpressController()
        {
        }

        private bool CheckLoginStatus(AuthorizationContext filterContext)
        {
            bool flag = true;
            //if (base.CurrentSellerManager == null && base.CurrentUser == null)
            //{
            //    if (!WebHelper.IsAjax())
            //    {
            //        HttpRequestBase request = filterContext.HttpContext.Request;
            //        string str = HttpUtility.HtmlEncode(request.RawUrl.ToString());
            //        RedirectToRouteResult action = base.RedirectToAction("", "Login", new { area = "ExpressAdmin", returnUrl = str });
            //        filterContext.Result = action;
            //        flag = false;
            //    }
            //    else
            //    {
            //        BaseController.Result result = new BaseController.Result()
            //        {
            //            msg = "登录超时,请重新登录！",
            //            success = false
            //        };
            //        filterContext.Result = base.Json(result);
            //        flag = false;
            //    }
            //}
            return flag;
        }
    }
}
