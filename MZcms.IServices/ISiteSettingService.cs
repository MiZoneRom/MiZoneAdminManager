using MZcms.Entity;
using MZcms.Model;
using System;

namespace MZcms.IServices
{
	public interface ISiteSettingService : IService, IDisposable
	{
		SiteSettings GetSiteSettings();

		void SaveSetting(string key, object value);

		void SetSiteSettings(SiteSettings siteSettingsInfo);
	}
}