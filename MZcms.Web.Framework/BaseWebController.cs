using MZcms.Core.Helper;
using MZcms.Entity;
using MZcms.IServices;
using MZcms.Model;
using System;

namespace MZcms.Web.Framework
{
    public abstract class BaseWebController : BaseController
    {

        public Members CurrentUser
        {
            get
            {
                long num = UserCookieEncryptHelper.Decrypt(WebHelper.GetCookie("MZcms-User"), "Web");
                if (num == 0)
                {
                    return null;
                }
                return ServiceHelper.Create<IMemberService>().GetMember(num);
            }
        }

        protected BaseWebController()
        {

        }
    }
}