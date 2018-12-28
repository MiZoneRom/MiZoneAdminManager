﻿using System.Web.Mvc;

namespace MZcms.Web.Framework
{
    /// <summary>
    /// 门店授权权限验证
    /// </summary>
    public class StoreAuthorizationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 验证权限 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool isOpenStore = Application.SiteSettingApplication.GetSiteSettings() != null && Application.SiteSettingApplication.GetSiteSettings().IsOpenStore;
            if (!isOpenStore)
            {
                //跳转到错误页
                var result = new ViewResult()
                {
                    ViewName = "NoAccess"
                };
                result.TempData.Add("Message", "门店未授权，你没有权限访问此页面");
                result.TempData.Add("Title", "门店未授权，你没有权限访问此页面！");
                filterContext.Result = result;
            }
        }
    }

    /// <summary>
    /// PC端授权权限验证
    /// </summary>
    public class PCAuthorizationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 验证权限 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool isOpenPC = Application.SiteSettingApplication.GetSiteSettings() != null && Application.SiteSettingApplication.GetSiteSettings().IsOpenPC;
            if (!isOpenPC)
            {
                //跳转到错误页
                var result = new ViewResult()
                {
                    ViewName = "NoAccess"
                };
                result.TempData.Add("Message", "PC端未授权，你没有权限访问此页面");
                result.TempData.Add("Title", "PC端未授权，你没有权限访问此页面！");
                filterContext.Result = result;
            }
        }
    }

    /// <summary>
    /// H5端授权权限验证
    /// </summary>
    public class H5AuthorizationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 验证权限 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();

            bool isOpenH5 = Application.SiteSettingApplication.GetSiteSettings() != null && (Application.SiteSettingApplication.GetSiteSettings().IsOpenH5);
            if (!isOpenH5)
            {
                if (controllerName == "vshop")
                {
                    if (!Application.SiteSettingApplication.GetSiteSettings().IsOpenApp)
                    {
                        //跳转到错误页
                        var resultApp = new ViewResult()
                        {
                            ViewName = "NoAccess"
                        };
                        resultApp.TempData.Add("Message", "App端未授权，你没有权限访问此页面");
                        resultApp.TempData.Add("Title", "App端未授权，你没有权限访问此页面！");
                        filterContext.Result = resultApp;
                        return;
                    }
                }
                else if (controllerName == "vtemplate")
                {
                    if (!(Application.SiteSettingApplication.GetSiteSettings().IsOpenApp|| Application.SiteSettingApplication.GetSiteSettings().IsOpenH5 || Application.SiteSettingApplication.GetSiteSettings().IsOpenMallSmallProg || Application.SiteSettingApplication.GetSiteSettings().IsOpenMultiStoreSmallProg))
                    {
                        //跳转到错误页
                        var resultApp = new ViewResult()
                        {
                            ViewName = "NoAccess"
                        };
                        resultApp.TempData.Add("Message", "指定端未授权，你没有权限访问此页面");
                        resultApp.TempData.Add("Title", "指定端未授权，你没有权限访问此页面！");
                        filterContext.Result = resultApp;
                        return;
                    }
                }
                else
                {
                    //跳转到错误页
                    var result = new ViewResult()
                    {
                        ViewName = "NoAccess"
                    };
                    result.TempData.Add("Message", "H5端未授权，你没有权限访问此页面");
                    result.TempData.Add("Title", "H5端未授权，你没有权限访问此页面！");
                    filterContext.Result = result;
                }
            }
        }
    }

    /// <summary>
    /// APP端授权权限验证
    /// </summary>
    public class APPAuthorizationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 验证权限 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool isOpenApp = Application.SiteSettingApplication.GetSiteSettings() != null && Application.SiteSettingApplication.GetSiteSettings().IsOpenApp;
            if (!isOpenApp)
            {
                //跳转到错误页
                var result = new ViewResult()
                {
                    ViewName = "NoAccess"
                };
                result.TempData.Add("Message", "商城APP端未授权，你没有权限访问此页面");
                result.TempData.Add("Title", "商城APP端未授权，你没有权限访问此页面！");
                filterContext.Result = result;
            }
        }
    }

    /// <summary>
    /// 商城小程序授权权限验证
    /// </summary>
    public class MallSmallProgAuthorizationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 验证权限 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool isOpenMallSmallProg = Application.SiteSettingApplication.GetSiteSettings() != null && Application.SiteSettingApplication.GetSiteSettings().IsOpenMallSmallProg;
            if (!isOpenMallSmallProg)
            {
                //跳转到错误页
                var result = new ViewResult()
                {
                    ViewName = "NoAccess"
                };
                result.TempData.Add("Message", "商城小程序未授权，你没有权限访问此页面");
                result.TempData.Add("Title", "商城小程序未授权，你没有权限访问此页面！");
                filterContext.Result = result;
            }
        }
    }

    /// <summary>
    /// 多门店小程序授权权限验证
    /// </summary>
    public class MultiStoreSmallProgAuthorizationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 验证权限 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool isOpenMultiStoreSmallProg = Application.SiteSettingApplication.GetSiteSettings() != null && Application.SiteSettingApplication.GetSiteSettings().IsOpenMultiStoreSmallProg;
            if (!isOpenMultiStoreSmallProg)
            {
                //跳转到错误页
                var result = new ViewResult()
                {
                    ViewName = "NoAccess"
                };
                result.TempData.Add("Message", "多门店小程序未授权，你没有权限访问此页面");
                result.TempData.Add("Title", "多门店小程序未授权，你没有权限访问此页面！");
                filterContext.Result = result;
            }
        }
    }

    /// <summary>
    /// 营销模块授权权限验证
    /// </summary>
    public class MarketingAuthorizationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 验证权限 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentSiteInfo = Application.SiteSettingApplication.GetSiteSettings();
            if (currentSiteInfo != null)
            {
                string areaName = string.Empty, controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
                string actionName =filterContext.ActionDescriptor.ActionName.ToLower();
                var route = filterContext.RouteData;
                if (route != null)
                {
                    areaName = (route.DataTokens["area"] == null ? "" : route.DataTokens["area"].ToString().ToLower());
                }
                if (areaName == "selleradmin")
                {
                    switch (controllerName)
                    {
                        case "limittimebuy":
                            {
                                if (!(currentSiteInfo.IsOpenPC || currentSiteInfo.IsOpenH5 || currentSiteInfo.IsOpenApp || currentSiteInfo.IsOpenMallSmallProg))
                                {
                                    NoAccess(filterContext);
                                }
                            }
                            break;

                        case "fulldiscount":
                        case "coupon":
                            {
                                if (!(currentSiteInfo.IsOpenPC || currentSiteInfo.IsOpenH5 || currentSiteInfo.IsOpenApp || currentSiteInfo.IsOpenMallSmallProg || currentSiteInfo.IsOpenMultiStoreSmallProg))
                                {
                                    NoAccess(filterContext);
                                }
                            }
                            break;

                        case "collocation":
                            {
                                if (!(currentSiteInfo.IsOpenPC || currentSiteInfo.IsOpenH5))
                                {
                                    NoAccess(filterContext);
                                }
                            }
                            break;

                        case "shopbonus":
                        case "fightgroup":
                            {
                                if (!(currentSiteInfo.IsOpenApp || currentSiteInfo.IsOpenH5))
                                {
                                    NoAccess(filterContext);
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }
                else if (areaName == "admin")
                {
                    switch (controllerName)
                    {
                        case "limittimebuy":
                            {
                                if (!(currentSiteInfo.IsOpenPC || currentSiteInfo.IsOpenH5 || currentSiteInfo.IsOpenApp || currentSiteInfo.IsOpenMallSmallProg))
                                {
                                    NoAccess(filterContext);
                                }
                            }
                            break;

                        case "fulldiscount":
                        case "coupon":
                            {
                                if (!(currentSiteInfo.IsOpenPC || currentSiteInfo.IsOpenH5 || currentSiteInfo.IsOpenApp || currentSiteInfo.IsOpenMallSmallProg || currentSiteInfo.IsOpenMultiStoreSmallProg))
                                {
                                    NoAccess(filterContext);
                                }
                            }
                            break;

                        case "collocation":
                            {
                                if (!(currentSiteInfo.IsOpenPC || currentSiteInfo.IsOpenH5))
                                {
                                    NoAccess(filterContext);
                                }
                            }
                            break;

                        case "shopbonus":
                        case "fightgroup":
                            {
                                if (!(currentSiteInfo.IsOpenApp || currentSiteInfo.IsOpenH5))
                                {
                                    NoAccess(filterContext);
                                }
                            }
                            break;

                        case "signin":
                        case "bonus":
                            {
                                if (!(currentSiteInfo.IsOpenH5))
                                {
                                    NoAccess(filterContext);
                                }
                            }
                            break;

                        case "couponactivity":
                            {
                                if (!(currentSiteInfo.IsOpenH5 || currentSiteInfo.IsOpenPC || currentSiteInfo.IsOpenApp))
                                {
                                    NoAccess(filterContext);
                                }
                            }
                            break;

                        case "weiactivity":
                        case "weibigwheel":
                            {
                                if (!(currentSiteInfo.IsOpenH5 || currentSiteInfo.IsOpenApp))
                                {
                                    NoAccess(filterContext);
                                }
                            }
                            break;

                        case "gift":
                            {
                                if (actionName == "appmanage")//APP积分商城
                                {
                                    if (!(currentSiteInfo.IsOpenApp))
                                    {
                                        NoAccess(filterContext);
                                    }
                                }
                                else
                                {
                                    //其余为礼品管理
                                    if (!(currentSiteInfo.IsOpenApp || currentSiteInfo.IsOpenPC))
                                    {
                                        NoAccess(filterContext);
                                    }
                                }
                            }
                            break;

                        case "mobiletopic":
                            {
                                if (!(currentSiteInfo.IsOpenH5 || currentSiteInfo.IsOpenApp||currentSiteInfo.IsOpenMallSmallProg||currentSiteInfo.IsOpenMultiStoreSmallProg))
                                {
                                    NoAccess(filterContext);
                                }
                            }
                            break;

                        case "topic":
                            {
                                if (!(currentSiteInfo.IsOpenPC))
                                {
                                    NoAccess(filterContext);
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        private static void NoAccess(ActionExecutingContext filterContext)
        {
            //跳转到错误页
            var result = new ViewResult()
            {
                ViewName = "NoAccess"
            };
            result.TempData.Add("Message", "你没有权限访问此页面");
            result.TempData.Add("Title", "你没有权限访问此页面！");
            filterContext.Result = result;
        }
    }
}
