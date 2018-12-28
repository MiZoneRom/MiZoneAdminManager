using MZcms.Core;
using MZcms.Entity;
using MZcms.IServices;
using MZcms.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MZcms.Service
{
	public class SiteSettingService : ServiceBase, ISiteSettingService, IService, IDisposable
	{
		public SiteSettingService()
		{
		}

		public SiteSettings GetSiteSettings()
		{
			SiteSettings SiteSettings;
			if (Cache.Get("Cache-SiteSettings") == null)
			{
				IEnumerable<SiteSettings> array = context.SiteSettings.FindAll<SiteSettings>().ToArray();
				SiteSettings = new SiteSettings();
				PropertyInfo[] properties = SiteSettings.GetType().GetProperties();
				for (int i = 0; i < properties.Length; i++)
				{
					PropertyInfo propertyInfo = properties[i];
					if (propertyInfo.Name != "Id")
					{
						SiteSettings SiteSettings1 = array.FirstOrDefault((SiteSettings item) => item.Key == propertyInfo.Name);
						if (SiteSettings1 != null)
						{
							propertyInfo.SetValue(SiteSettings, Convert.ChangeType(SiteSettings1.Value, propertyInfo.PropertyType));
						}
					}
				}
				Cache.Insert("Cache-SiteSettings", SiteSettings);
			}
			else
			{
				SiteSettings = (SiteSettings)Cache.Get("Cache-SiteSettings");
			}
			return SiteSettings;
		}

		public void SaveSetting(string key, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("值不能为null");
			}
			if (typeof(SiteSettings).GetProperties().FirstOrDefault((PropertyInfo item) => item.Name == key) == null)
			{
				throw new ApplicationException(string.Concat("未找到", key, "对应的配置项"));
			}
			SiteSettings SiteSettings = context.SiteSettings.Where((SiteSettings item) => item.Key == key).FirstOrDefault();
			if (SiteSettings == null)
			{
				SiteSettings = new SiteSettings();
                context.SiteSettings.Add(SiteSettings);
			}
			SiteSettings.Key = key;
			SiteSettings.Value = value.ToString();
            context.SaveChanges();
			Cache.Remove("Cache-SiteSettings");
		}

		public void SetSiteSettings(SiteSettings SiteSettings)
		{
			string str;
			PropertyInfo[] properties = SiteSettings.GetType().GetProperties();
			IEnumerable<SiteSettings> array = context.SiteSettings.FindAll<SiteSettings>().ToArray();
			PropertyInfo[] propertyInfoArray = properties;
			for (int i = 0; i < propertyInfoArray.Length; i++)
			{
				PropertyInfo propertyInfo = propertyInfoArray[i];
				object value = propertyInfo.GetValue(SiteSettings);
				str = (value == null ? "" : value.ToString());
				if (propertyInfo.Name != "Id")
				{
					SiteSettings SiteSettings1 = array.FirstOrDefault((SiteSettings item) => item.Key == propertyInfo.Name);
					if (SiteSettings1 != null)
					{
						SiteSettings1.Value = str;
					}
					else
					{
						DbSet<SiteSettings> SiteSettingss = context.SiteSettings;
						SiteSettings SiteSettings2 = new SiteSettings()
						{
							Key = propertyInfo.Name,
							Value = str
						};
						SiteSettingss.Add(SiteSettings2);
					}
				}
			}
			IEnumerable<string> name = 
				from item in properties
                select item.Name;
            context.SiteSettings.RemoveRange(
				from item in array
				where !name.Contains<string>(item.Key)
				select item);
            context.SaveChanges();
			Cache.Remove("Cache-SiteSettings");
		}
	}
}