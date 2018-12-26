using MZcms.Core;
using MZcms.Core.Helper;
using MZcms.IServices;
using MZcms.Model;
using MZcms.ServiceProvider;
using System;
using System.Web;

namespace MZcms.Web.Framework
{
	public class UserCookieEncryptHelper
	{
		public UserCookieEncryptHelper()
		{
		}

		public static long Decrypt(string userIdCookie, string controllerName)
		{
			string userCookieKey = Instance<ISiteSettingService>.Create.GetSiteSettings().UserCookieKey;
			if (string.IsNullOrEmpty(userCookieKey))
			{
				Guid guid = Guid.NewGuid();
				userCookieKey = SecureHelper.MD5(guid.ToString());
				Instance<ISiteSettingService>.Create.SaveSetting("UserCookieKey", userCookieKey);
			}
			string empty = string.Empty;
			try
			{
				if (!string.IsNullOrWhiteSpace(userIdCookie))
				{
					userIdCookie = HttpUtility.UrlDecode(userIdCookie);
					userIdCookie = SecureHelper.DecodeBase64(userIdCookie);
					empty = SecureHelper.AESDecrypt(userIdCookie, userCookieKey);
					empty = empty.Replace(string.Concat(controllerName, ","), "");
				}
			}
			catch (Exception exception)
			{
				Log.Error(string.Format("解密用户标识Cookie出错，Cookie密文：{0}", userIdCookie), exception);
			}
			long num = 0;
			long.TryParse(empty, out num);
			return num;
		}

		public static string Encrypt(long userId, string controllerName)
		{
			string str;
			string userCookieKey = Instance<ISiteSettingService>.Create.GetSiteSettings().UserCookieKey;
			if (string.IsNullOrEmpty(userCookieKey))
			{
				Guid guid = Guid.NewGuid();
				userCookieKey = SecureHelper.MD5(guid.ToString());
				Instance<ISiteSettingService>.Create.SaveSetting("UserCookieKey", userCookieKey);
			}
			string empty = string.Empty;
			try
			{
				string str1 = string.Concat(controllerName, ",", userId.ToString());
				empty = SecureHelper.AESEncrypt(str1, userCookieKey);
				empty = SecureHelper.EncodeBase64(empty);
				str = empty;
			}
			catch (Exception exception)
			{
				Log.Error(string.Format("加密用户标识Cookie出错", empty), exception);
				throw;
			}
			return str;
		}
	}
}