using System.Configuration;

namespace MZcms.Web.Framework
{
    /// <summary>
    /// 网站静态信息记录
    /// </summary>
    public static class SiteStaticInfo
    {
        /// <summary>
        /// 当前域名
        /// </summary>
        public static string CurDomainUrl {
            get
            {
                return ConfigurationManager.AppSettings["CurDomainUrl"];
            }
        }
    }
}
