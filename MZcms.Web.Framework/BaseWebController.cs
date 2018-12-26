using MZcms.Core.Helper;
using MZcms.IServices;
using MZcms.Model;
using System;

namespace MZcms.Web.Framework
{
	public abstract class BaseWebController : BaseController
	{
		//public ISellerManager CurrentSellerManager
		//{
		//	get
		//	{
		//		ISellerManager sellerManager = null;
		//		long num = UserCookieEncryptHelper.Decrypt(WebHelper.GetCookie("MZcms-SellerManager"), "SellerAdmin");
		//		if (num != 0)
		//		{
		//			sellerManager = ServiceHelper.Create<IManagerService>().GetSellerManager(num);
		//		}
		//		return sellerManager;
		//	}
		//}

		//public UserMemberInfo CurrentUser
		//{
		//	get
		//	{
		//		long num = UserCookieEncryptHelper.Decrypt(WebHelper.GetCookie("MZcms-User"), "Web");
		//		if (num == 0)
		//		{
		//			return null;
		//		}
		//		return ServiceHelper.Create<IMemberService>().GetMember(num);
		//	}
		//}

        protected BaseWebController()
		{

        }
	}
}