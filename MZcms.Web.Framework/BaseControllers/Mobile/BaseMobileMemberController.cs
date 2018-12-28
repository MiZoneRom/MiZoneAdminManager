﻿using MZcms.Application;
using MZcms.Core;
using MZcms.Core.Helper;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MZcms.Web.Framework
{
    /// <summary>
    /// 移动端控制器基类(需要登录)
    /// </summary>
    public abstract class BaseMobileMemberController : BaseMobileTemplatesController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.AreaName = string.Format("m-{0}", PlatformType.ToString());
            //不能应用在子方法上
            if (filterContext.IsChildAction)
                return;
            if (CurrentUser == null || CurrentUser.Disabled)
            {
                #region//不需要登录
                if (IsAnonymity())
                {
                    return;
                }
                #endregion
                bool end;
                if (Core.Helper.WebHelper.IsAjax())//处理ajax请求的情况
                    end = ProcessInvalidUser_Ajax(filterContext);
                else
                    end = ProcessInvalidUser_NormalRequest(filterContext);//处理普通页面请求的情况
                if (end)
                    return;
            }
            else
            {
                bool end;
                if (!Core.Helper.WebHelper.IsAjax())
                {
                    end = BindOpenIdToUser(filterContext);//已经登录过站点，则验证openid是否绑定当前用户
                    if (end)
                        return;
                }
            }
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 一下页面满足匿名购买不需要验证用户登录
        /// </summary>
        /// <returns></returns>
        protected bool IsAnonymity()
        {
            string controller = GetRouteString("controller").ToLower();
            string action = GetRouteString("action").ToLower();
            string[] controllers = { "cart", "order" };
            string[] actions = { "editbranchproducttocart", "getbranchcartproducts", "clearbranchcartproducts", "clearbranchcartinvalidproducts", "expressinfo" };
            if (controllers.Contains(controller) && actions.Contains(action))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获得路由中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        protected string GetRouteString(string key, string defaultValue)
        {
            object value = RouteData.Values[key];
            if (value != null)
                return value.ToString();
            else
                return defaultValue;
        }

        /// <summary>
        /// 获得路由中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        protected string GetRouteString(string key)
        {
            return GetRouteString(key, "");
        }
        /// <summary>
        /// 处理Ajax请求的情况
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns>是否中断当前action提前结束</returns>
        bool ProcessInvalidUser_Ajax(ActionExecutingContext filterContext)
        {
            Result result = new Result();
            result.msg = "登录超时,请重新登录！";
            result.success = false;
            filterContext.Result = Json(result);
            return true;
        }

        /// <summary>
        /// 处理普通页面请求的情况
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns>是否中断当前action提前结束</returns>
        bool ProcessInvalidUser_NormalRequest(ActionExecutingContext filterContext)
        {
            bool end = true;
            //处理手动退出后不自动登录
            string actlogout = WebHelper.GetCookie(CookieKeysCollection.MZcms_ACTIVELOGOUT);

            //分析当前平台类型，并创建对应的登录接口
            IMobileOAuth imobileOauth = null;
            switch (PlatformType)
            {
                case Core.PlatformType.WeiXin:
                    imobileOauth = new WeixinOAuth();
                    break;
            }
            string normalLoginUrl = string.Format("/m-{0}/Login/Entrance?returnUrl={1}", PlatformType.ToString(), HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.ToString()));
            if (imobileOauth != null && GetRequestType(filterContext.HttpContext.Request) == Core.PlatformType.WeiXin)//找到了支持的登录接口
            {
                //可能的待跳转用户授权地址

                Model.WXShopInfo settings = new Model.WXShopInfo();
                string redirectUrl;
                //string strShopid = WebHelper.GetCookie(CookieKeysCollection.MZcms_SHOP);
                //long shopid = string.IsNullOrEmpty(strShopid) ? 0 : UserCookieEncryptHelper.Decrypt(strShopid, "Mobile");

                string strShopid = filterContext.HttpContext.Request["shop"];
                var AppidType = Model.MemberOpenIdInfo.AppIdTypeEnum.Normal;
                if (!string.IsNullOrEmpty(strShopid))
                {
                    long shopid = 0;
                    bool isLong = long.TryParse(strShopid, out shopid);
                    if (shopid > 0)
                    {
                        settings = VshopApplication.GetVShopSetting(shopid);
                    }
                }

                if (string.IsNullOrEmpty(settings.AppId) || string.IsNullOrEmpty(settings.AppSecret))
                {
                    settings = new Model.WXShopInfo()
                    {
                        AppId = CurrentSiteSetting.WeixinAppId,
                        AppSecret = CurrentSiteSetting.WeixinAppSecret,
                        Token = CurrentSiteSetting.WeixinToken
                    };
                    AppidType = Model.MemberOpenIdInfo.AppIdTypeEnum.Payment;//是平台Appid，可以作为付款（微信支付）
                }

                //获取当前用户信息
                var userInfo = imobileOauth.GetUserInfo(filterContext, out redirectUrl, settings);
                if (string.IsNullOrWhiteSpace(redirectUrl))//待跳转地址为空，说明已经经过了用户授权页面
                {
                    if (userInfo != null && !string.IsNullOrWhiteSpace(userInfo.OpenId))//用户信息不为空并且OpenId不为空，说明用户已经授权
                    {
                        if (AppidType == Model.MemberOpenIdInfo.AppIdTypeEnum.Payment)
                        {
                            var curMenberOpenId = Core.Helper.SecureHelper.AESEncrypt(userInfo.OpenId, "Mobile");
                            WebHelper.SetCookie(CookieKeysCollection.MZcms_USER_OpenID, curMenberOpenId);
                        }
                        //检查是否已经有用户绑定过该OpenId
                        //MZcms.Core.Log.Debug("InvalidUser LoginProvider=" + userInfo.LoginProvider);
                        //MZcms.Core.Log.Debug("InvalidUser OpenId=" + userInfo.OpenId);
                        //MZcms.Core.Log.Debug("InvalidUser UnionId=" + userInfo.UnionId);
                        Model.UserMemberInfo existUser = null;
                        //existUser = ServiceHelper.Create<IMemberService>().GetMemberByUnionId(userInfo.LoginProvider, userInfo.UnionId);
                        if (existUser == null)
                        {
                            if (actlogout != "1")
                            {
                                //existUser = ServiceHelper.Create<IMemberService>().GetMemberByOpenId(userInfo.LoginProvider, userInfo.OpenId);
                                existUser = MemberApplication.GetMemberByUnionId(userInfo.UnionId);
                            }
                        }

                        if (existUser != null)//已经有用户绑定过，直接标识为该用户
                        {
                            base.SetUserLoginCookie(existUser.Id);
                            Application.MemberApplication.UpdateLastLoginDate(existUser.Id);
                        }
                        else//未绑定过，则跳转至登录绑定页面
                        {
                            normalLoginUrl = string.Format("/m-{0}/Login/Entrance?openId={1}&serviceProvider={2}&nickName={3}&realName={4}&headimgurl={5}&returnUrl={6}&AppidType={7}&unionid={8}&sex={9}&city={10}&province={11}&country={12}",
                                PlatformType.ToString(),
                                userInfo.OpenId,
                               "MZcms.Plugin.OAuth.WeiXin",//使用同微信登录插件一致的名称， 以此保证微信信任与微信商城登录用户信息统一
                                HttpUtility.UrlEncode(userInfo.NickName),
                                HttpUtility.UrlEncode(userInfo.RealName),
                                HttpUtility.UrlEncode(userInfo.Headimgurl),
                                HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.ToString()),
                                AppidType,
                                userInfo.UnionId,
                                userInfo.Sex,
                                userInfo.City,
                                userInfo.Province,
                                userInfo.Country
                                 );
                            //跳转至登录绑定页面
                            var result = Redirect(normalLoginUrl);
                            filterContext.Result = result;
                        }

                    }
                    else//用户未授权，或者无法获取用户授权
                    {
                        //用户未授权，则跳转至普通登录页面
                        var result = Redirect(normalLoginUrl);
                        filterContext.Result = result;
                    }
                }
                else
                {//立即跳转到用户授权页面
                    var result = Redirect(redirectUrl);
                    filterContext.Result = result;

                }
            }
            else
            {//未找到对应的用户授权实现机制，则跳转至普通登录页面
                var result = Redirect(normalLoginUrl);
                filterContext.Result = result;
            }
            return end;
        }

        /// <summary>
        /// 为已登录过的用户(存在cookie)，绑定OpenId
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        bool BindOpenIdToUser(ActionExecutingContext filterContext)
        {
            bool end = true;

            //处理手动退出后不自动登录
            string actlogout = WebHelper.GetCookie(CookieKeysCollection.MZcms_ACTIVELOGOUT);

            //分析当前平台类型，并创建对应的登录接口
            IMobileOAuth imobileOauth = null;
            switch (PlatformType)
            {
                case Core.PlatformType.WeiXin:
                    imobileOauth = new WeixinOAuth();
                    break;
            }

            string normalLoginUrl = string.Format("/m-{0}/Login/Entrance?returnUrl={1}", PlatformType.ToString(), HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.ToString()));
            if (imobileOauth != null && GetRequestType(filterContext.HttpContext.Request) == Core.PlatformType.WeiXin)//找到了支持的登录接口
            {
                //可能的待跳转用户授权地址
                string redirectUrl;
                //string strShopid = WebHelper.GetCookie(CookieKeysCollection.MZcms_SHOP);
                //long shopid = string.IsNullOrEmpty(strShopid) ? 0 : UserCookieEncryptHelper.Decrypt(strShopid, "Mobile");
                Model.WXShopInfo settings = new Model.WXShopInfo();
                string strShopid = filterContext.HttpContext.Request["shop"];
                var AppidType = Model.MemberOpenIdInfo.AppIdTypeEnum.Normal;
                if (!string.IsNullOrEmpty(strShopid))
                {
                    Log.Warn(strShopid + ":" + filterContext.HttpContext.Request.Url.ToString());
                    long shopid = 0;
                    bool isLong = long.TryParse(strShopid, out shopid);
                    if (shopid > 0)
                    {
                        settings = VshopApplication.GetVShopSetting(shopid);
                    }
                }
                else
                    Log.Warn(filterContext.HttpContext.Request.Url.ToString());

                if (string.IsNullOrEmpty(settings.AppId) || string.IsNullOrEmpty(settings.AppSecret))
                {
                    settings = new Model.WXShopInfo()
                    {
                        AppId = CurrentSiteSetting.WeixinAppId,
                        AppSecret = CurrentSiteSetting.WeixinAppSecret,
                        Token = CurrentSiteSetting.WeixinToken
                    };
                    AppidType = Model.MemberOpenIdInfo.AppIdTypeEnum.Payment;//是平台Appid，可以作为付款（微信支付）
                }

                //获取当前用户信息
                var userInfo = imobileOauth.GetUserInfo_bequiet(filterContext, out redirectUrl, settings);

                if (string.IsNullOrWhiteSpace(redirectUrl))//待跳转地址为空，说明已经经过了用户授权页面
                {
                    end = false;//不再中断当前action
                    if (userInfo != null && !string.IsNullOrWhiteSpace(userInfo.OpenId))//用户信息不为空并且OpenId不为空，说明用户已经授权
                    {
                        if (AppidType == Model.MemberOpenIdInfo.AppIdTypeEnum.Payment)
                        {//记录平台公众号对应的OpenId
                            var curMenberOpenId = Core.Helper.SecureHelper.AESEncrypt(userInfo.OpenId, "Mobile");
                            WebHelper.SetCookie(CookieKeysCollection.MZcms_USER_OpenID, curMenberOpenId);
                        }

                        //MZcms.Core.Log.Debug("BindOpenIdToUser LoginProvider=" + userInfo.LoginProvider);
                        //MZcms.Core.Log.Debug("BindOpenIdToUser OpenId=" + userInfo.OpenId);
                        //MZcms.Core.Log.Debug("BindOpenIdToUser UnionId=" + userInfo.UnionId);
                        //检查是否已经有用户绑定过该OpenId
                        Model.UserMemberInfo existUser = null;
                        //existUser = member.GetMemberByUnionId(userInfo.LoginProvider, userInfo.UnionId);
                        if (existUser == null)
                        {
                            if (actlogout != "1")
                            {
                                //existUser = member.GetMemberByOpenId(userInfo.LoginProvider, userInfo.OpenId);
                                existUser = MemberApplication.GetMemberByUnionId(userInfo.UnionId);
                            }
                        }
                        if (existUser != null)
                        {
                            if (!string.IsNullOrEmpty(strShopid))
                            {
                                base.SetUserLoginCookie(existUser.Id);
                                Application.MemberApplication.UpdateLastLoginDate(existUser.Id);
                            }
                        }
                        else//未绑定过，则绑定当前用户
                        {
                            MemberApplication.BindMember(CurrentUser.Id, "MZcms.Plugin.OAuth.WeiXin", userInfo.OpenId, AppidType, userInfo.Sex, userInfo.Headimgurl, unionid: userInfo.UnionId);
                            //end = false;//不再中断当前action
                        }
                    }
                }
                else
                {//立即跳转到用户授权页面
                    var result = Redirect(redirectUrl);
                    filterContext.Result = result;
                }
            }
            else
            {
                end = false;
            }
            return end;
        }
        /// <summary>
        /// 发出请求的类型（浏览器、微信）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Core.PlatformType GetRequestType(HttpRequestBase request)
        {
            Core.PlatformType type = Core.PlatformType.Wap;
            if (request.UserAgent.ToLower().Contains("micromessenger"))
            {
                type = Core.PlatformType.WeiXin;
            }
            return type;
        }
    }
}
