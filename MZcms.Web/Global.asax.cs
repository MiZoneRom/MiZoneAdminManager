using FluentValidation.Mvc;
using MZcms.Core;
using MZcms.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MZcms.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("Areas/");


            //AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
            AreaRegistrationOrder.RegisterAllAreasOrder();
            BundleConfig.RegisterBundles(BundleTable.Bundles);



            RegistAtStart.RegistStrategies();
            ObjectContainer.ApplicationStart(new AutoFacContainer());
            RegistAtStart.RegistPlugins();
            //OnMZcmsStartMethodAttribute.Start();
            //MemberApplication.InitMessageQueue();

            //autoFac();
            //  ViewEngines.Engines.Insert(0, new TemplateVisualizationViewEngine());

            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new CustomValidatorFactory()));
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
        }

        /// <summary>
        /// 自定义验证工厂
        /// </summary>
        public class CustomValidatorFactory : FluentValidation.ValidatorFactoryBase
        {
            public override FluentValidation.IValidator CreateInstance(Type validatorType)
            {
                var type = validatorType.GetGenericArguments()[0];
                var validatorAttribute = type.GetCustomAttribute<FluentValidation.Attributes.ValidatorAttribute>();
                if (validatorAttribute != null)
                {
                    //创建验证实体
                    var obj = System.Activator.CreateInstance(validatorAttribute.ValidatorType);
                    return obj as FluentValidation.IValidator;
                }

                return null;
            }
        }

        protected void Application_End()
        {
            #region 访问首页，重启数据池
            string hosturl = SiteStaticInfo.CurDomainUrl;
#if DEBUG
            MZcms.Core.Log.Info(System.DateTime.Now.ToString() + " -  " + hosturl);
#endif
            if (!string.IsNullOrWhiteSpace(hosturl))
            {
                System.Net.HttpWebRequest myHttpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(hosturl);
                System.Net.HttpWebResponse myHttpWebResponse = (System.Net.HttpWebResponse)myHttpWebRequest.GetResponse();
            }
            #endregion
        }

    }
}
