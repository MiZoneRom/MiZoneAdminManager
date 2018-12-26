using Himall.Core.Helper;
using Himall.IServices;
using Himall.Model;
using System;
using System.Web.Mvc;

namespace MZcms.Web.Framework
{
	public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
	{
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

		protected WebViewPage()
		{
		}

		public override void InitHelpers()
		{
			base.InitHelpers();
		}
	}
}