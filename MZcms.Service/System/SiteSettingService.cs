using MZcms.CommonModel;
using MZcms.Entity;
using MZcms.IServices;
using MZcms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;

namespace MZcms.Service
{
    public class SiteSettingService : ServiceBase, ISiteSettingService
    {
        public SiteSettings GetSiteSettings()
        {
            SiteSettings SiteSettings=null;

            if (Core.Cache.Exists(CacheKeyCollection.SiteSettings))//如果存在缓存，则从缓存中读取
                SiteSettings = Core.Cache.Get<SiteSettings>(CacheKeyCollection.SiteSettings);

			if (SiteSettings == null)
			{
				//否则从数据库中读取，并将配置存入至缓存

				//通过反射获取值
				var values = Context.SiteSettings.FindAll().ToArray();
				SiteSettings = new SiteSettings();

				var properties = SiteSettings.GetType().GetProperties();
				foreach (var property in properties)
				{
					if (property.Name != "Id")
					{
						var temp = values.FirstOrDefault(item => item.Key == property.Name);
						if (temp != null)
							property.SetValue(SiteSettings, Convert.ChangeType(temp.Value, property.PropertyType));
					}
				}

				Core.Cache.Insert(CacheKeyCollection.SiteSettings, SiteSettings);
			}

            return SiteSettings;
        }
        public SiteSettings GetSiteSettingsByObjectCache()
        {
            SiteSettings SiteSettings = null;
            ObjectCache cache = MemoryCache.Default;
                if (cache.Contains(CacheKeyCollection.SiteSettings))//如果存在缓存，则从缓存中读取
                    SiteSettings = cache.Get(CacheKeyCollection.SiteSettings) as SiteSettings;

            if (SiteSettings == null)
            {
                //否则从数据库中读取，并将配置存入至缓存

                //通过反射获取值
                var values = Context.SiteSettings.FindAll().ToArray();
                SiteSettings = new SiteSettings();

                var properties = SiteSettings.GetType().GetProperties();
                foreach (var property in properties)
                {
                    if (property.Name != "Id")
                    {
                        var temp = values.FirstOrDefault(item => item.Key == property.Name);
                        if (temp != null)
                            property.SetValue(SiteSettings, Convert.ChangeType(temp.Value, property.PropertyType));
                    }
                }

               // Core.Cache.Insert(CacheKeyCollection.SiteSettings, SiteSettings);
                var policy = new CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddSeconds(300) };
                cache.Add(CacheKeyCollection.SiteSettings, SiteSettings, policy);
            }

            return SiteSettings;
        }

        public void SetSiteSettings(SiteSettings SiteSettings)
        {
            PropertyInfo[] properties = SiteSettings.GetType().GetProperties();
            SiteSettings temp;
            string value;
            object obj;
            IEnumerable<SiteSettings> siteSettingInfos = Context.SiteSettings.FindAll().ToArray();
            foreach (var property in properties)
            {
                obj = property.GetValue(SiteSettings);
                if (obj != null)
                    value = obj.ToString();
                else
                    value = "";
                if (property.Name != "Id")
                {
                    temp = siteSettingInfos.FirstOrDefault(item => item.Key == property.Name);
                    if (temp == null)//数据库中不存在，则创建
                        Context.SiteSettings.Add(new SiteSettings() { Key = property.Name, Value = value });
                    else//存在则更新
                        temp.Value = value;
                }
            }

            //删除不存在的项
            var propertyNames = properties.Select(item => item.Name);
            Context.SiteSettings.RemoveRange(siteSettingInfos.Where(item => !propertyNames.Contains(item.Key)));
            Context.SaveChanges();
            Core.Cache.Remove(CacheKeyCollection.SiteSettings);
        }

        public void SaveSetting(string key, object value)
        {
            if (value == null)
                throw new ArgumentNullException("值不能为null");

            //检查Key是否存在
            PropertyInfo[] properties = typeof(SiteSettings).GetProperties();
            if (properties.FirstOrDefault(item => item.Name == key) == null)
                throw new ApplicationException("未找到" + key + "对应的配置项");

            var siteSetting = Context.SiteSettings.FindBy(item => item.Key == key).FirstOrDefault() ;
            if (siteSetting == null)
            {
                siteSetting = new SiteSettings();
                Context.SiteSettings.Add(siteSetting);
            }
            siteSetting.Key = key;
            siteSetting.Value = value.ToString();
            Context.SaveChanges();
            Core.Cache.Remove(CacheKeyCollection.SiteSettings);
        }


    }
}
