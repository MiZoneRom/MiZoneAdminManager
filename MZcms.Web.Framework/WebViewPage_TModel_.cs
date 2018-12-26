using MZcms.Core.Helper;
using MZcms.IServices;
using MZcms.Model;
using System;
using System.Web.Mvc;

namespace MZcms.Web.Framework
{
	public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
	{
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

		protected WebViewPage()
		{
		}

		public override void InitHelpers()
		{
			base.InitHelpers();
		}
	}
}