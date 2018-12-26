using MZcms.IServices;
using MZcms.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MZcms.Web.Framework
{
    internal class WeixinOAuth : IMobileOAuth
    {
        public WeixinOAuth()
        {
        }

        private string GetResponseResult(string url)
        {
            string end;
            using (HttpWebResponse response = (HttpWebResponse)WebRequest.Create(url).GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        end = streamReader.ReadToEnd();
                    }
                }
            }
            return end;
        }

        //public MobileOAuthUserInfo GetUserInfo(ActionExecutingContext filterContext, out string redirectUrl)
        //{
        //    SiteSettingsInfo siteSettings = ServiceHelper.Create<ISiteSettingService>().GetSiteSettings();
        //    MobileOAuthUserInfo mobileOAuthUserInfo = null;
        //    redirectUrl = string.Empty;
        //    if (!string.IsNullOrEmpty(siteSettings.WeixinAppId))
        //    {
        //        string item = filterContext.HttpContext.Request["code"];
        //        if (string.IsNullOrEmpty(item))
        //        {
        //            string str = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect", siteSettings.WeixinAppId, HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.ToString()));
        //            redirectUrl = str;
        //        }
        //        else
        //        {
        //            string responseResult = GetResponseResult(string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", siteSettings.WeixinAppId, siteSettings.WeixinAppSecret, item));
        //            if (responseResult.Contains("access_token"))
        //            {
        //                JObject jObjects = JsonConvert.DeserializeObject(responseResult) as JObject;
        //                string[] strArrays = new string[] { "https://api.weixin.qq.com/sns/userinfo?access_token=", jObjects["access_token"].ToString(), "&openid=", jObjects["openid"].ToString(), "&lang=zh_CN" };
        //                string responseResult1 = GetResponseResult(string.Concat(strArrays));
        //                if (responseResult1.Contains("nickname"))
        //                {
        //                    JObject jObjects1 = JsonConvert.DeserializeObject(responseResult1) as JObject;
        //                    MobileOAuthUserInfo mobileOAuthUserInfo1 = new MobileOAuthUserInfo()
        //                    {
        //                        NickName = jObjects1["nickname"].ToString(),
        //                        RealName = jObjects1["nickname"].ToString(),
        //                        OpenId = jObjects1["openid"].ToString(),
        //                        UnionId = (jObjects1["unionid"] == null || string.IsNullOrWhiteSpace(jObjects1["unionid"].ToString()) ? jObjects1["openid"].ToString() : jObjects1["unionid"].ToString()),
        //                        Headimgurl = jObjects1["headimgurl"].ToString(),
        //                        LoginProvider = "MZcms.Plugin.OAuth.WeiXin"
        //                    };
        //                    mobileOAuthUserInfo = mobileOAuthUserInfo1;
        //                }
        //            }
        //        }
        //    }
        //    return mobileOAuthUserInfo;
        //}

        //public MobileOAuthUserInfo GetUserInfo(ActionExecutingContext filterContext, out string redirectUrl, WXShopInfo settings)
        //{
        //    MobileOAuthUserInfo mobileOAuthUserInfo = null;
        //    redirectUrl = string.Empty;
        //    if (!string.IsNullOrEmpty(settings.AppId))
        //    {
        //        string item = filterContext.HttpContext.Request["code"];
        //        if (string.IsNullOrEmpty(item))
        //        {
        //            string str = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect", settings.AppId, HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.ToString()));
        //            redirectUrl = str;
        //        }
        //        else
        //        {
        //            string responseResult = GetResponseResult(string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", settings.AppId, settings.AppSecret, item));
        //            if (responseResult.Contains("access_token"))
        //            {
        //                JObject jObjects = JsonConvert.DeserializeObject(responseResult) as JObject;
        //                string[] strArrays = new string[] { "https://api.weixin.qq.com/sns/userinfo?access_token=", jObjects["access_token"].ToString(), "&openid=", jObjects["openid"].ToString(), "&lang=zh_CN" };
        //                string responseResult1 = GetResponseResult(string.Concat(strArrays));
        //                if (responseResult1.Contains("nickname"))
        //                {
        //                    JObject jObjects1 = JsonConvert.DeserializeObject(responseResult1) as JObject;
        //                    MobileOAuthUserInfo mobileOAuthUserInfo1 = new MobileOAuthUserInfo()
        //                    {
        //                        NickName = jObjects1["nickname"].ToString(),
        //                        RealName = jObjects1["nickname"].ToString(),
        //                        OpenId = jObjects1["openid"].ToString(),
        //                        UnionId = (jObjects1["unionid"] == null || string.IsNullOrWhiteSpace(jObjects1["unionid"].ToString()) ? jObjects1["openid"].ToString() : jObjects1["unionid"].ToString()),
        //                        Headimgurl = jObjects1["headimgurl"].ToString(),
        //                        LoginProvider = "MZcms.Plugin.OAuth.WeiXin",
        //                        sex = jObjects1["sex"].ToString()
        //                    };
        //                    mobileOAuthUserInfo = mobileOAuthUserInfo1;
        //                }
        //            }
        //        }
        //    }
        //    return mobileOAuthUserInfo;
        //}

        //public MobileOAuthUserInfo GetUserInfo_bequiet(ActionExecutingContext filterContext, out string redirectUrl, WXShopInfo settings)
        //{
        //    MobileOAuthUserInfo mobileOAuthUserInfo = null;
        //    redirectUrl = string.Empty;
        //    if (!string.IsNullOrEmpty(settings.AppId))
        //    {
        //        string item = filterContext.HttpContext.Request["code"];
        //        if (string.IsNullOrEmpty(item))
        //        {
        //            string str = filterContext.HttpContext.Request.Url.ToString();
        //            str = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=STATE#wechat_redirect", settings.AppId, HttpUtility.UrlEncode(str));
        //            redirectUrl = str;
        //        }
        //        else
        //        {
        //            string responseResult = GetResponseResult(string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", settings.AppId, settings.AppSecret, item));
        //            if (responseResult.Contains("access_token"))
        //            {
        //                JObject jObjects = JsonConvert.DeserializeObject(responseResult) as JObject;
        //                MobileOAuthUserInfo mobileOAuthUserInfo1 = new MobileOAuthUserInfo()
        //                {
        //                    OpenId = jObjects["openid"].ToString(),
        //                    LoginProvider = "MZcms.Plugin.OAuth.WeiXin",
        //                    UnionId = (jObjects["unionid"] == null || string.IsNullOrWhiteSpace(jObjects["unionid"].ToString()) ? jObjects["openid"].ToString() : jObjects["unionid"].ToString())
        //                };
        //                mobileOAuthUserInfo = mobileOAuthUserInfo1;
        //            }
        //        }
        //    }
        //    return mobileOAuthUserInfo;
        //}

    }
}