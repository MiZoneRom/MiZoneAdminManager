using MZcms.Model;


namespace MZcms.Web.Framework
{
    /// <summary>
    /// 页面基类型
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
	{
		public SiteSettingsInfo SiteSetting
		{
			get
			{
				if (this.ViewContext.Controller is BaseController)
					return ((BaseController)this.ViewContext.Controller).CurrentSiteSetting;
				return Application.SiteSettingApplication.GetSiteSettings();
			}
		}

        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserMemberInfo CurrentUser
        {
            get
			{
				if (this.ViewContext.Controller is BaseController)
					return ((BaseController)this.ViewContext.Controller).CurrentUser;
				return BaseController.GetUser(Request);
            }
        }
        public string Generator
        {
            get
            {
                return "3.0";
            }
        }
    }
    /// <summary>
    /// 页面基类型
    /// </summary>
    public abstract class WebViewPage : WebViewPage<dynamic>
    {

    }
}
