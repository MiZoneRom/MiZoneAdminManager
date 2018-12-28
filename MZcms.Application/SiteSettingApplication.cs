using System;
using System.Collections.Generic;
using MZcms.IServices;
using AutoMapper;
using MZcms.Core;
using MZcms.DTO;
using MZcms.Core.Plugins.Message;
using MZcms.Model;

namespace MZcms.Application
{
    public class SiteSettingApplication
    {

        private static ISiteSettingService _iSiteSettingService = ObjectContainer.Current.Resolve<ISiteSettingService>();

        /// <summary>
        /// 获取系统配置信息
        /// </summary>
        /// <returns></returns>
        public static  SiteSettings GetSiteSettings()
        {
            return _iSiteSettingService.GetSiteSettings();
        }

        /// <summary>
        /// 保存系统配置信息
        /// </summary>
        /// <param name="siteSettingsInfo">待保存的系统配置（该配置必须是完整的配置）</param>
        public static void SetSiteSettings(SiteSettings siteSettingsInfo)
        {
            _iSiteSettingService.SetSiteSettings(siteSettingsInfo);
        }

        /// <summary>
        /// 保存单个配置项
        /// </summary>
        /// <param name="key">配置项的Key（大小写敏感）</param>
        /// <param name="value">值</param>
       public static void SaveSetting(string key, object value)
        {
             _iSiteSettingService.SaveSetting(key, value);
        }

    }
}
