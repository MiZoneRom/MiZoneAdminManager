using MZcms.Core;
using MZcms.Core.Helper;
using MZcms.IServices;
using MZcms.Model;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MZcms.Web.Framework
{
    public abstract class BaseController : Controller
    {
        public bool IsAutoJumpMobile;

        private int CKLoginTimeOut = 30;

        protected List<JumpUrlRoute> _JumpUrlRouteData
        {
            get;
            set;
        }

        public SiteSettingsInfo CurrentSiteSetting
        {
            get
            {
                return ServiceHelper.Create<ISiteSettingService>().GetSiteSettings();
            }
        }

        protected bool isCanClearLoginStatus
        {
            get
            {
                bool flag = true;
                DateTime? lastOperateTime = LastOperateTime;
                if (lastOperateTime.HasValue)
                {
                    TimeSpan now = DateTime.Now - lastOperateTime.Value;
                    if (now.TotalMinutes <= CKLoginTimeOut && now.TotalMinutes >= 0)
                    {
                        flag = false;
                    }
                }
                return flag;
            }
        }

        public bool IsMobileTerminal
        {
            get;
            set;
        }

        public List<JumpUrlRoute> JumpUrlRouteData
        {
            get
            {
                return _JumpUrlRouteData;
            }
        }

        protected DateTime? LastOperateTime
        {
            get
            {
                HttpCookie item = base.HttpContext.Request.Cookies["MZcms_LastOpTime"];
                DateTime? nullable = null;
                if (item != null)
                {
                    nullable = new DateTime?(DateTime.FromBinary(long.Parse(item.Value)));
                }
                return nullable;
            }
        }

        public VisitorTerminal visitorTerminalInfo
        {
            get;
            set;
        }

        public BaseController()
        {
            if (!IsInstalled())
            {
                base.RedirectToAction("/Web/Installer/Agreement");
                return;
            }
            ((dynamic)base.ViewBag).SEODescription = CurrentSiteSetting.Site_SEODescription;
            ((dynamic)base.ViewBag).SEOKeyword = CurrentSiteSetting.Site_SEOKeywords;
            ((dynamic)base.ViewBag).FlowScript = CurrentSiteSetting.FlowScript;
        }

        private void ClearLoginCookie()
        {
            HttpCookie item = base.HttpContext.Request.Cookies["MZcms-User"];
            if (item != null)
            {
                item.Expires = DateTime.Now.AddYears(-1);
                base.HttpContext.Response.AppendCookie(item);
            }
            item = base.HttpContext.Request.Cookies["MZcms-SellerManager"];
            if (item != null)
            {
                item.Expires = DateTime.Now.AddYears(-1);
                base.HttpContext.Response.AppendCookie(item);
            }
            item = base.HttpContext.Request.Cookies["MZcms_LastOpTime"];
            if (item != null)
            {
                item.Expires = DateTime.Now.AddYears(-1);
                base.HttpContext.Response.AppendCookie(item);
            }
        }

        private void DisposeService(System.Web.Mvc.ControllerContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                return;
            }
            List<IService> item = filterContext.HttpContext.Session["_serviceInstace"] as List<IService>;
            if (item != null)
            {
                foreach (IService service in item)
                {
                    try
                    {
                        service.Dispose();
                    }
                    catch (Exception exception1)
                    {
                        Exception exception = exception1;
                        Log.Error(string.Concat(service.GetType().ToString(), "释放失败！"), exception);
                    }
                }
                filterContext.HttpContext.Session["_serviceInstace"] = null;
            }
        }

        private Exception GerInnerException(Exception ex)
        {
            if (ex.InnerException == null)
            {
                return ex;
            }
            return GerInnerException(ex.InnerException);
        }

        public JumpUrlRoute GetRouteUrl(string controller, string action, string area, string url)
        {
            JumpUrlRoute item;
            string lower = controller;
            string str = action;
            string lower1 = area;
            InitJumpUrlRoute();
            JumpUrlRoute jumpUrlRoute = null;
            url = url.ToLower();
            lower = lower.ToLower();
            str = str.ToLower();
            lower1 = lower1.ToLower();
            List<JumpUrlRoute> jumpUrlRouteData = JumpUrlRouteData;
            if (!string.IsNullOrWhiteSpace(lower1))
            {
                jumpUrlRouteData = jumpUrlRouteData.FindAll((JumpUrlRoute d) => d.Area.ToLower() == lower1);
            }
            if (!string.IsNullOrWhiteSpace(lower))
            {
                jumpUrlRouteData = jumpUrlRouteData.FindAll((JumpUrlRoute d) => d.Controller.ToLower() == lower);
            }
            if (!string.IsNullOrWhiteSpace(str))
            {
                jumpUrlRouteData = jumpUrlRouteData.FindAll((JumpUrlRoute d) => d.Action.ToLower() == str);
            }
            if (jumpUrlRouteData.Count > 0)
            {
                item = jumpUrlRouteData[0];
            }
            else
            {
                item = null;
            }
            jumpUrlRoute = item;
            if (jumpUrlRoute == null)
            {
                JumpUrlRoute jumpUrlRoute1 = new JumpUrlRoute()
                {
                    PC = url,
                    WAP = url,
                    WX = url
                };
                jumpUrlRoute = jumpUrlRoute1;
            }
            return jumpUrlRoute;
        }


        public void InitJumpUrlRoute()
        {
            _JumpUrlRouteData = new List<JumpUrlRoute>();
            JumpUrlRoute jumpUrlRoute = new JumpUrlRoute()
            {
                Action = "Index",
                Area = "Web",
                Controller = "UserOrder",
                PC = "/userorder",
                WAP = "/member/orders",
                WX = "/member/orders"
            };
            _JumpUrlRouteData.Add(jumpUrlRoute);
            JumpUrlRoute jumpUrlRoute1 = new JumpUrlRoute()
            {
                Action = "Index",
                Area = "Web",
                Controller = "UserCenter",
                PC = "/usercenter",
                WAP = "/member/center",
                WX = "/member/center"
            };
            _JumpUrlRouteData.Add(jumpUrlRoute1);
            JumpUrlRoute jumpUrlRoute2 = new JumpUrlRoute()
            {
                Action = "Index",
                Area = "Web",
                Controller = "Login",
                PC = "/login",
                WAP = "/login/entrance",
                WX = "/login/entrance"
            };
            _JumpUrlRouteData.Add(jumpUrlRoute2);
            JumpUrlRoute jumpUrlRoute3 = new JumpUrlRoute()
            {
                Action = "Home",
                Area = "Web",
                Controller = "Shop",
                PC = "/shop",
                WAP = "/vshop/detail",
                WX = "/vshop/detail",
                IsSpecial = true
            };
            _JumpUrlRouteData.Add(jumpUrlRoute3);
            JumpUrlRoute jumpUrlRoute4 = new JumpUrlRoute()
            {
                Action = "Submit",
                Area = "Web",
                Controller = "Order",
                PC = "/order/submit",
                WAP = "/order/SubmiteByCart",
                WX = "/order/SubmiteByCart",
                IsSpecial = true
            };
            _JumpUrlRouteData.Add(jumpUrlRoute4);
        }

        protected void InitVisitorTerminal()
        {
            VisitorTerminal visitorTerminal = new VisitorTerminal();
            string userAgent = base.Request.UserAgent;
            if (string.IsNullOrWhiteSpace(userAgent))
            {
                userAgent = "";
            }
            userAgent = userAgent.ToLower();
            bool flag = userAgent.Contains("ipad");
            bool flag1 = userAgent.Contains("iphone os");
            bool flag2 = userAgent.Contains("midp");
            bool flag3 = userAgent.Contains("rv:1.2.3.4");
            flag3 = (flag3 ? flag3 : userAgent.Contains("ucweb"));
            bool flag4 = userAgent.Contains("android");
            bool flag5 = userAgent.Contains("windows ce");
            bool flag6 = userAgent.Contains("windows mobile");
            bool flag7 = userAgent.Contains("micromessenger");
            bool flag8 = userAgent.Contains("windows phone ");
            bool flag9 = userAgent.Contains("appwebview(ios)");
            visitorTerminal.Terminal = EnumVisitorTerminal.PC;
            if (flag || flag1 || flag2 || flag3 || flag4 || flag5 || flag6 || flag8)
            {
                visitorTerminal.Terminal = EnumVisitorTerminal.Moblie;
            }
            if (flag || flag1)
            {
                visitorTerminal.OperaSystem = EnumVisitorOperaSystem.IOS;
                visitorTerminal.Terminal = EnumVisitorTerminal.Moblie;
                if (flag)
                {
                    visitorTerminal.Terminal = EnumVisitorTerminal.PAD;
                }
                if (flag9)
                {
                    visitorTerminal.Terminal = EnumVisitorTerminal.IOS;
                }
            }
            if (flag4)
            {
                visitorTerminal.OperaSystem = EnumVisitorOperaSystem.Android;
                visitorTerminal.Terminal = EnumVisitorTerminal.Moblie;
            }
            if (flag7)
            {
                visitorTerminal.Terminal = EnumVisitorTerminal.WeiXin;
            }
            if (visitorTerminal.Terminal == EnumVisitorTerminal.Moblie || visitorTerminal.Terminal == EnumVisitorTerminal.PAD || visitorTerminal.Terminal == EnumVisitorTerminal.WeiXin || visitorTerminal.Terminal == EnumVisitorTerminal.IOS)
            {
                IsMobileTerminal = true;
            }
            visitorTerminalInfo = visitorTerminal;
        }

        private bool IsExistPage(string url)
        {
            bool flag = false;
            HttpWebResponse uRLResponse = WebHelper.GetURLResponse(url, "get", "", null, 20000);
            if (uRLResponse != null && (uRLResponse.StatusCode == HttpStatusCode.OK || uRLResponse.StatusCode == HttpStatusCode.Found || uRLResponse.StatusCode == HttpStatusCode.MovedPermanently))
            {
                flag = true;
            }
            return flag;
        }

        private bool IsInstalled()
        {
            string item = ConfigurationManager.AppSettings["IsInstalled"];
            if (item == null)
            {
                return true;
            }
            return bool.Parse(item);
        }
        //手机网址跳转
        protected void JumpMobileUrl(System.Web.Routing.RouteData route, string url = "")
        {
            string pathAndQuery = base.Request.Url.PathAndQuery;
            string wX = pathAndQuery;
            if (route == null)
            {
                return;
            }
            string lower = route.Values["controller"].ToString().ToLower();
            string str = route.Values["action"].ToString().ToLower();
            string str1 = (route.DataTokens["area"] == null ? "" : route.DataTokens["area"].ToString().ToLower());

            if (str1 == "mobile")
            {
                return;
            }
            if (str1 == "web")
            {
                IsAutoJumpMobile = true;
            }
            if (lower == "shoppano")//如果是全景店铺 不跳转
            {
                return;
            }
            if (str == "getcartproducts")
            {
                return;
            }
            if (lower == "game")//如果游戏
            {
                return;
            }
            if (IsAutoJumpMobile && IsMobileTerminal && Regex.Match(pathAndQuery, "\\/m(\\-.*)?").Length < 1)
            {
                JumpUrlRoute routeUrl = GetRouteUrl(lower, str, str1, pathAndQuery);
                switch (visitorTerminalInfo.Terminal)
                {
                    case EnumVisitorTerminal.WeiXin:
                        {
                            if (routeUrl != null)
                            {
                                wX = routeUrl.WX;
                            }
                            wX = string.Concat("/m-weixin", wX);

                            break;
                        }
                    case EnumVisitorTerminal.IOS:
                        {
                            if (routeUrl != null)
                            {
                                wX = routeUrl.WAP;
                            }
                            wX = string.Concat("/m-ios", wX);
                            break;
                        }
                    default:
                        {
                            if (routeUrl != null)
                            {
                                wX = routeUrl.WAP;
                            }
                            wX = string.Concat("/m-wap", wX);
                            break;
                        }
                }
                if (routeUrl.IsSpecial)
                {
                    if (routeUrl.PC.ToLower() == "/shop")
                    {
                        string str2 = route.Values["id"].ToString();
                        long num = 0;
                        long id = 0;
                        if (!long.TryParse(str2, out num))
                        {
                            num = 0;
                        }
                        if (num > 0)
                        {
                            VShopInfo vShopByShopId = ServiceHelper.Create<IVShopService>().GetVShopByShopId(num);
                            if (vShopByShopId != null)
                            {
                                id = vShopByShopId.Id;
                            }
                        }
                        wX = string.Concat(wX, "/", id.ToString());
                    }
                    if (routeUrl.PC.ToLower() == "/order/submit")
                    {
                        string empty = string.Empty;
                        object item = route.Values["cartitemids"];
                        empty = (item != null ? item.ToString() : base.Request.QueryString["cartitemids"]);
                        wX = string.Concat(wX, "/?cartItemIds=", empty);
                    }
                }
                if (!string.IsNullOrWhiteSpace(url))
                {
                    wX = url;
                }
                if (!IsExistPage(string.Concat(base.Request.Url.Scheme, "://", base.Request.Url.Authority, wX)))
                {
                    wX = (visitorTerminalInfo.Terminal != EnumVisitorTerminal.WeiXin ? "/m-wap/" : "/m-weixin/");
                }
                base.Response.Redirect(wX);
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            JumpMobileUrl(filterContext.RouteData, "");
            base.OnActionExecuting(filterContext);
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            InitVisitorTerminal();
            if (IsInstalled() && CurrentSiteSetting.SiteIsClose && filterContext.RouteData.Values["controller"].ToString().ToLower() != "admin")
            {
                filterContext.Result = new RedirectResult("/common/site/close");
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception exception = GerInnerException(filterContext.Exception);
            string message = exception.Message;
            base.OnException(filterContext);
            if (!(exception is MZcmsException))
            {
                string str = filterContext.RouteData.Values["controller"].ToString();
                string str1 = filterContext.RouteData.Values["action"].ToString();
                object item = filterContext.RouteData.DataTokens["area"];
                string str2 = string.Format("页面未捕获的异常：Area:{0},Controller:{1},Action:{2}", item, str, str1);
                Log.Error(str2, exception);
                message = "系统内部异常";
            }
            if (!WebHelper.IsAjax())
            {
                ViewResult viewResult = new ViewResult()
                {
                    ViewName = "Error"
                };
                viewResult.TempData.Add("Message", filterContext.Exception.ToString());
                viewResult.TempData.Add("Title", message);
                filterContext.Result = viewResult;
                filterContext.HttpContext.Response.StatusCode = 200;
                filterContext.ExceptionHandled = true;
                DisposeService(filterContext);
            }
            else
            {
                BaseController.Result result = new BaseController.Result()
                {
                    success = false,
                    msg = message,
                    status = -9999
                };
                filterContext.Result = base.Json(result);
                filterContext.HttpContext.Response.StatusCode = 200;
                filterContext.ExceptionHandled = true;
                DisposeService(filterContext);
            }
            if (exception is HttpRequestValidationException)
            {
                if (!WebHelper.IsAjax())
                {
                    ContentResult contentResult = new ContentResult()
                    {
                        Content = "<script src='/Scripts/jquery-1.11.1.min.js'></script>"
                    };
                    ContentResult contentResult1 = contentResult;
                    contentResult1.Content = string.Concat(contentResult1.Content, "<script src='/Scripts/jquery.artDialog.js'></script>");
                    ContentResult contentResult2 = contentResult;
                    contentResult2.Content = string.Concat(contentResult2.Content, "<script src='/Scripts/artDialog.iframeTools.js'></script>");
                    ContentResult contentResult3 = contentResult;
                    contentResult3.Content = string.Concat(contentResult3.Content, "<link href='/Content/artdialog.css' rel='stylesheet' />");
                    ContentResult contentResult4 = contentResult;
                    contentResult4.Content = string.Concat(contentResult4.Content, "<link href='/Content/bootstrap.min.css' rel='stylesheet' />");
                    ContentResult contentResult5 = contentResult;
                    contentResult5.Content = string.Concat(contentResult5.Content, "<script>$(function(){$.dialog.errorTips('您提交了非法字符！',function(){window.history.back(-1)},2);});</script>");
                    filterContext.Result = contentResult;
                }
                else
                {
                    BaseController.Result result1 = new BaseController.Result()
                    {
                        msg = "您提交了非法字符!"
                    };
                    filterContext.Result = base.Json(result1);
                }
                filterContext.HttpContext.Response.StatusCode = 200;
                filterContext.ExceptionHandled = true;
                DisposeService(filterContext);
            }
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            DisposeService(filterContext);
        }

        protected void SetLastOperateTime(DateTime? date = null)
        {
            if (!date.HasValue)
            {
                date = new DateTime?(DateTime.Now);
            }
            HttpCookie item = base.HttpContext.Request.Cookies["MZcms_LastOpTime"];
            DateTime.Now.AddYears(-1);
            if (item != null)
            {
                DateTime.FromBinary(long.Parse(item.Value));
            }
            else
            {
                item = new HttpCookie("MZcms_LastOpTime");
            }
            item.Value = date.Value.Ticks.ToString();
            base.HttpContext.Response.AppendCookie(item);
        }

        public class Result
        {
            public string msg
            {
                get;
                set;
            }

            public int status
            {
                get;
                set;
            }

            public bool success
            {
                get;
                set;
            }

            public Result()
            {
            }
        }
    }
}