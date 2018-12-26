using Himall.Core;
using Himall.Core.Helper;
using Himall.IServices;
using Himall.Model;
using System;
using System.Web;
using System.Web.Mvc;

namespace MZcms.Web.Framework
{
    /// <summary>
    /// 移动成员控制器
    /// </summary>
	public abstract class BaseMobileShopMemberController : BaseMobileTemplatesController
    {
        protected BaseMobileShopMemberController()
        {
        }

        private Himall.Core.PlatformType GetRequestType(HttpRequestBase request)
        {

            Himall.Core.PlatformType platformType = Himall.Core.PlatformType.Wap;
            if (request.UserAgent.ToLower().Contains("micromessenger"))
            {
                platformType = Himall.Core.PlatformType.WeiXin;
            }
            return platformType;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool flag;
            if (filterContext.IsChildAction)
            {
                return;
            }
            if (base.CurrentUser == null)
            {

                flag = (!WebHelper.IsAjax() ? ProcessInvalidUser_NormalRequest(filterContext) : ProcessInvalidUser_Ajax(filterContext));
                if (flag)
                {
                    return;
                }
            }
            base.OnActionExecuting(filterContext);
        }

        private bool ProcessInvalidUser_Ajax(ActionExecutingContext filterContext)
        {
            BaseController.Result result = new BaseController.Result()
            {
                msg = "登录超时,请重新登录！",
                success = false
            };
            filterContext.Result = base.Json(result);
            return true;
        }

        private bool ProcessInvalidUser_NormalRequest(ActionExecutingContext filterContext)
        {
            string str1 = string.Format("/m-{0}/Login/BindUser?returnUrl={1}", base.PlatformType.ToString(), HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.ToString()));
            filterContext.Result = Redirect(str1);
            return true;
        }

    }
}