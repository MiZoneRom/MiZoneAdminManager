using MZcms.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace MZcms.Core.Plugins.OAuth
{
	public interface IOAuthPlugin : IPlugin
	{
		string Icon_Default
		{
			get;
		}

		string Icon_Hover
		{
			get;
		}

		string ShortName
		{
			get;
		}

		FormData GetFormData();

		string GetOpenLoginUrl(string returnUrl);

		OAuthUserInfo GetUserInfo(NameValueCollection queryString);

		string GetValidateContent();

		void SetFormValues(IEnumerable<KeyValuePair<string, string>> values);
	}
}