using System.Web;
using System.Web.Mvc;
using MZcms.Web.Framework;

namespace MZcms.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new GZipAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
