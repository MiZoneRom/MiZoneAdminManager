using Himall.Core.Helper;
using Himall.IServices;
using Himall.Model;
using System;

namespace MZcms.Web.Framework
{
	public abstract class BaseWebController : BaseController
	{
		public ISellerManager CurrentSellerManager
		{
			get
			{
				ISellerManager sellerManager = null;
				long num = UserCookieEncryptHelper.Decrypt(WebHelper.GetCookie("Himall-SellerManager"), "SellerAdmin");
				if (num != 0)
				{
					sellerManager = ServiceHelper.Create<IManagerService>().GetSellerManager(num);
				}
				return sellerManager;
			}
		}

		public UserMemberInfo CurrentUser
		{
			get
			{
				long num = UserCookieEncryptHelper.Decrypt(WebHelper.GetCookie("Himall-User"), "Web");
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