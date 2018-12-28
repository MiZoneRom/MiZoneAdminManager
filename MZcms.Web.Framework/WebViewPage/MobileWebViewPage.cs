using MZcms.Model;


namespace MZcms.Web.Framework
{
    /// <summary>
    /// 页面基类型
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class MobileWebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
		public SiteSettings SiteSetting
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
        public Members CurrentUser
        {
            get
			{
				if (this.ViewContext.Controller is BaseController)
					return ((BaseController)this.ViewContext.Controller).CurrentUser;
				return BaseController.GetUser(Request);
            }
        }

		public CommonModel.Model.WeiXinShareArgs WeiXinShareArgs
		{
			get
			{
				if (this.ViewContext.Controller is BaseMobileController)
					return ((BaseMobileController)this.ViewContext.Controller).WeiXinShareArgs;
				return null;
			}
		}

        public string CurrentAreaName
        {
            get
            {
                string result = "m";
                result = ViewBag.AreaName;
                return result;
            }
        }
    }
    /// <summary>
    /// 页面基类型
    /// </summary>
    public abstract class MobileWebViewPage : MobileWebViewPage<dynamic>
    {

    }
}
