using MZcms.Application;
using MZcms.Core;
using MZcms.Model;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace MZcms.Web.Framework
{
    /// <summary>
    /// 商城后台基础控制器类
    /// </summary>
    public abstract class BaseSellerController : BaseWebController
    {
        #region 字段
        private SiteSettings _sitesetting = null;
        #endregion

        #region 属性
        /// <summary>
        /// 当前站点配置
        /// </summary>
        public new SiteSettings CurrentSiteSetting
        {
            get
            {
                if (_sitesetting != null)
                {
                    return _sitesetting;
                }
                else
                {
                    _sitesetting = SiteSettingApplication.GetSiteSettings();

                }
                return _sitesetting;
            }
        }

        #endregion


        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            var t = ConfigurationManager.AppSettings["IsInstalled"];
            if (!(null == t || bool.Parse(t)))
            {
                return;
            }
            //不能应用在子方法上
            if (filterContext.IsChildAction)
                return;

            base.OnAuthorization(filterContext);

            //检查登录状态    //检查授权情况    //跳转到第几部//检查当前商家注册情况 //检查店铺是否过期
            if (CheckLoginStatus(filterContext) && CheckAuthorization(filterContext))
                return;
        }

        /// <summary>
        /// 检查授权情况
        /// </summary>
        /// <param name="filterContext"></param>
        bool CheckAuthorization(AuthorizationContext filterContext)
        {
            var flag = true;
            object[] actionFilter = filterContext.ActionDescriptor.GetCustomAttributes(typeof(UnAuthorize), false);
            if (actionFilter.Length == 1)
                return true;

            string controllerName = filterContext.RouteData.Values["controller"].ToString();
            string actionName = filterContext.RouteData.Values["action"].ToString();

            if (CurrentSellerManager.SellerPrivileges == null || CurrentSellerManager.SellerPrivileges.Count == 0 || !SellerPermission.CheckPermissions(CurrentSellerManager.SellerPrivileges, controllerName, actionName) )
            {
                if (Core.Helper.WebHelper.IsAjax())
                {
                    Result result = new Result();
                    result.msg = "你没有访问的权限！";
                    result.success = false;
                    filterContext.Result = Json(result);
                    flag = false;
                }
                else
                {
                    //跳转到错误页
                    var result = new ViewResult()
                    {
                        ViewName = "NoAccess"
                    };
                    result.TempData.Add("Message", "你没有权限访问此页面");
                    result.TempData.Add("Title", "你没有权限访问此页面！");
                    filterContext.Result = result;
                    flag = false;
                }
            }
            return flag;
        }

        /// <summary>
        /// 检查登录状态
        /// </summary>
        /// <param name="filterContext"></param>
        bool CheckLoginStatus(AuthorizationContext filterContext)
        {
            var flag = true;
            if (CurrentSellerManager == null && CurrentUser == null)
            {
                if (Core.Helper.WebHelper.IsAjax())
                {
                    Result result = new Result();
                    result.msg = "登录超时,请重新登录！";
                    result.success = false;
                    filterContext.Result = Json(result);
                    flag = false;
                }
                else
                {
                    HttpRequestBase bases = (HttpRequestBase)filterContext.HttpContext.Request;
                    string url = bases.RawUrl.ToString();
                    string returnurl = System.Web.HttpUtility.HtmlEncode(url);
                    var result = RedirectToAction("", "Login", new
                    {
                        area = "web"
                    });//不带跳转了
                    filterContext.Result = result;
                    flag = false;
                    //跳转到登录页
                }
            }
            else if (CurrentUser != null && CurrentSellerManager == null)
            {
                var result = RedirectToAction("EditProfile0", "ShopProfile", new
                {
                    area = "SellerAdmin"
                });
                filterContext.Result = result;
                flag = false;
            }
            return flag;
        }

    }
}
