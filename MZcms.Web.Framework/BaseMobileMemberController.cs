using MZcms.Core;
using MZcms.Core.Helper;
using MZcms.Entity;
using MZcms.IServices;
using MZcms.Model;
using System;
using System.Web;
using System.Web.Mvc;

namespace MZcms.Web.Framework
{
    /// <summary>
    /// 移动成员控制器
    /// </summary>
	public abstract class BaseMobileMemberController : BaseMobileTemplatesController
    {
        protected BaseMobileMemberController()
        {
        }

        /// <summary>
        /// 绑定的OpenID用户
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
		private bool BindOpenIdToUser(ActionExecutingContext filterContext)
        {

            string str;
            bool flag = true;
            IMobileOAuth weixinOAuth = null;
            if (base.PlatformType == MZcms.Core.PlatformType.WeiXin)
            {
                weixinOAuth = new WeixinOAuth();
            }

            string.Format("/m-{0}/Login/Entrance?returnUrl={1}", base.PlatformType.ToString(), HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.ToString()));

            if (weixinOAuth == null || GetRequestType(filterContext.HttpContext.Request) != MZcms.Core.PlatformType.WeiXin)
            {
                flag = false;
            }
            else
            {
                WXShopInfo wXShopInfo = new WXShopInfo();
                string item = filterContext.HttpContext.Request["shop"];
                MemberOpenIds.AppIdTypeEnum appIdTypeEnum = MemberOpenIdInfo.AppIdTypeEnum.Normal;

                if (!string.IsNullOrEmpty(item))
                {
                    long num = 0;
                    long.TryParse(item, out num);
                    if (num > 0)
                    {
                        wXShopInfo = ServiceHelper.Create<IVShopService>().GetVShopSetting(num);
                    }
                }

                if (string.IsNullOrEmpty(wXShopInfo.AppId) || string.IsNullOrEmpty(wXShopInfo.AppSecret))
                {
                    WXShopInfo wXShopInfo1 = new WXShopInfo()
                    {
                        AppId = base.CurrentSiteSetting.WeixinAppId,
                        AppSecret = base.CurrentSiteSetting.WeixinAppSecret,
                        Token = base.CurrentSiteSetting.WeixinToken
                    };
                    wXShopInfo = wXShopInfo1;
                    appIdTypeEnum = MemberOpenIdInfo.AppIdTypeEnum.Payment;
                }

                MobileOAuthUserInfo userInfoBequiet = weixinOAuth.GetUserInfo_bequiet(filterContext, out str, wXShopInfo);

                if (!string.IsNullOrWhiteSpace(str))
                {
                    filterContext.Result = Redirect(str);
                }
                else
                {
                    flag = false;

                    if (userInfoBequiet != null && !string.IsNullOrWhiteSpace(userInfoBequiet.OpenId))
                    {

                        string isMale = "true";
                        if (userInfoBequiet.sex == "2")
                        {
                            isMale = "false";
                        }

                        if (appIdTypeEnum == MemberOpenIdInfo.AppIdTypeEnum.Payment)
                        {
                            string str1 = SecureHelper.AESEncrypt(userInfoBequiet.OpenId, "Mobile");
                            WebHelper.SetCookie("MZcms-User_OpenId", str1);
                        }

                        IMemberService memberService = ServiceHelper.Create<IMemberService>();
                        UserMemberInfo memberByOpenId = null;

                        if (memberByOpenId == null)
                        {
                            memberByOpenId = memberService.GetMemberByOpenId(userInfoBequiet.LoginProvider, userInfoBequiet.OpenId);
                        }

                        if (memberByOpenId == null)
                        {

                            memberService.BindMember(base.CurrentUser.Id, "MZcms.Plugin.OAuth.WeiXin", userInfoBequiet.OpenId, appIdTypeEnum, userInfoBequiet.Headimgurl, isMale, userInfoBequiet.UnionId);

                        }
                        else
                        {


                            string str2 = UserCookieEncryptHelper.Encrypt(memberByOpenId.Id, "Mobile");
                            WebHelper.SetCookie("MZcms-User", str2);
                        }
                    }
                }
            }
            return flag;
        }

        private MZcms.Core.PlatformType GetRequestType(HttpRequestBase request)
        {

            MZcms.Core.PlatformType platformType = MZcms.Core.PlatformType.Wap;
            if (request.UserAgent.ToLower().Contains("micromessenger"))
            {
                platformType = MZcms.Core.PlatformType.WeiXin;
            }
            return platformType;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool flag;
            if (filterContext.IsChildAction)
            {
                return;
            }
            if (base.CurrentUser == null)
            {

                flag = (!WebHelper.IsAjax() ? ProcessInvalidUser_NormalRequest(filterContext) : ProcessInvalidUser_Ajax(filterContext));
                if (flag)
                {
                    return;
                }
            }
            //else if (!WebHelper.IsAjax() && BindOpenIdToUser(filterContext))
            //{
            // Log.Debug("");

            // return;
            // }
            base.OnActionExecuting(filterContext);
        }

        private bool ProcessInvalidUser_Ajax(ActionExecutingContext filterContext)
        {
            BaseController.Result result = new BaseController.Result()
            {
                msg = "登录超时,请重新登录！",
                success = false
            };
            filterContext.Result = base.Json(result);
            return true;
        }

        private bool ProcessInvalidUser_NormalRequest(ActionExecutingContext filterContext)
        {
            string str;
            bool flag = true;
            IMobileOAuth weixinOAuth = null;
            if (GetRequestType(filterContext.HttpContext.Request) == MZcms.Core.PlatformType.WeiXin)//如果是微信登录
            {
                weixinOAuth = new WeixinOAuth();
            }
            string str1 = string.Format("/m-{0}/Login/Entrance?returnUrl={1}", base.PlatformType.ToString(), HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.ToString()));
            if (weixinOAuth == null || GetRequestType(filterContext.HttpContext.Request) != MZcms.Core.PlatformType.WeiXin)//如果不是微信登录
            {
                filterContext.Result = Redirect(str1);
            }
            else//如果是微信登录
            {
                WXShopInfo wXShopInfo = new WXShopInfo();
                string item = filterContext.HttpContext.Request["shop"];
                MemberOpenIdInfo.AppIdTypeEnum appIdTypeEnum = MemberOpenIdInfo.AppIdTypeEnum.Normal;
                if (!string.IsNullOrEmpty(item))
                {
                    long num = 0;
                    long.TryParse(item, out num);
                    if (num > 0)
                    {
                        wXShopInfo = ServiceHelper.Create<IVShopService>().GetVShopSetting(num);
                    }
                }
                if (string.IsNullOrEmpty(wXShopInfo.AppId) || string.IsNullOrEmpty(wXShopInfo.AppSecret))
                {
                    WXShopInfo wXShopInfo1 = new WXShopInfo()
                    {
                        AppId = base.CurrentSiteSetting.WeixinAppId,
                        AppSecret = base.CurrentSiteSetting.WeixinAppSecret,
                        Token = base.CurrentSiteSetting.WeixinToken
                    };
                    wXShopInfo = wXShopInfo1;
                    appIdTypeEnum = MemberOpenIdInfo.AppIdTypeEnum.Payment;
                }
                MobileOAuthUserInfo userInfo = weixinOAuth.GetUserInfo(filterContext, out str, wXShopInfo);
                if (!string.IsNullOrWhiteSpace(str))
                {
                    filterContext.Result = Redirect(str);
                }
                else if (userInfo == null || string.IsNullOrWhiteSpace(userInfo.OpenId))
                {
                    filterContext.Result = Redirect(str1);
                }
                else
                {
                    if (appIdTypeEnum == MemberOpenIdInfo.AppIdTypeEnum.Payment)
                    {
                        string str2 = SecureHelper.AESEncrypt(userInfo.OpenId, "Mobile");
                        WebHelper.SetCookie("MZcms-User_OpenId", str2);
                    }
                    UserMemberInfo memberByUnionId = null;
                    if (memberByUnionId == null)
                    {
                        memberByUnionId = ServiceHelper.Create<IMemberService>().GetMemberByUnionId(userInfo.LoginProvider, userInfo.UnionId);

                        if (memberByUnionId != null && weixinOAuth != null)
                        {

                            MemberOpenIdInfo openidCheckItem = new MemberOpenIdInfo()
                            {
                                AppIdType = MemberOpenIdInfo.AppIdTypeEnum.Payment,
                                OpenId = userInfo.OpenId,
                                ServiceProvider = userInfo.LoginProvider,
                                UnionId = userInfo.UnionId,
                                UserId = memberByUnionId.Id,
                            };

                            ServiceHelper.Create<IMemberService>().CheckPaymentMemberInfo(openidCheckItem);

                        }

                    }

                    //string isMale = "true";
                    //if (userInfo.sex == "2")
                    //{
                    //    isMale = "false";
                    //}

                    if (memberByUnionId == null)
                    {
                        object[] objArray = new object[] { base.PlatformType.ToString(), userInfo.OpenId, "MZcms.Plugin.OAuth.WeiXin", HttpUtility.UrlEncode(userInfo.NickName), HttpUtility.UrlEncode(userInfo.RealName), HttpUtility.UrlEncode(userInfo.Headimgurl), HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.ToString()), appIdTypeEnum, userInfo.UnionId };
                        str1 = string.Format("/m-{0}/Login/Entrance?openId={1}&serviceProvider={2}&nickName={3}&realName={4}&headimgurl={5}&returnUrl={6}&AppidType={7}&unionid={8}", objArray);

                        //因为手机上链接自动小写 暂存进session
                        Session["serviceProvider"] = "MZcms.Plugin.OAuth.WeiXin";
                        Session["nickName"] = HttpUtility.UrlEncode(userInfo.NickName);
                        Session["headimgurl"] = HttpUtility.UrlEncode(userInfo.Headimgurl);
                        Session["openId"] = userInfo.OpenId;
                        Session["unionid"] = userInfo.UnionId;
                        Session["realName"] = userInfo.RealName;
                        Session["sex"] = userInfo.sex;

                        filterContext.Result = Redirect(str1);
                    }
                    else
                    {


                        //Log.Debug("检查更新");

                        ////如果昵称或性别与数据库不相同
                        //if (userInfo.NickName != memberByUnionId.Nick || isMale != memberByUnionId.IsMale)
                        //{

                        //    Log.Debug("更新信息");

                        //    //更新昵称及性别
                        //    memberByUnionId.Nick = userInfo.NickName;
                        //    memberByUnionId.IsMale = isMale;
                        //    ServiceHelper.Create<IMemberService>().UpdateMember(memberByUnionId);

                        //    //更新头像
                        //    ServiceHelper.Create<IMemberService>().UpdatePhoto(memberByUnionId.Id, userInfo.Headimgurl);

                        //}

                        string str3 = UserCookieEncryptHelper.Encrypt(memberByUnionId.Id, "Mobile");
                        WebHelper.SetCookie("MZcms-User", str3);
                    }
                }
            }
            return flag;
        }
    }
}