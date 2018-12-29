using MZcms.Application;
using MZcms.CommonModel;
using MZcms.Core.Helper;
using System.Web.Mvc;

namespace MZcms.Web.Framework
{
    public abstract class BaseWebController : BaseController
    {

        public long UserId
        {
            get
            {
                if (CurrentUser != null)
                    return CurrentUser.Id;
                return 0;
            }
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.IsChildAction && !filterContext.HttpContext.Request.IsAjaxRequest())
            {
                //统计代码
                StatisticApplication.StatisticPlatVisitUserCount();
            }
            base.OnActionExecuting(filterContext);
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                return;
            }
            base.OnActionExecuted(filterContext);
        }

    }
}
