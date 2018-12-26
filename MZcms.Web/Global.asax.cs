using Autofac;
using MZcms.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
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

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AreaRegistrationOrder.RegisterAllAreasOrder();
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        private void SetupResolveRules(ContainerBuilder builder)
        {
            Assembly assembly = Assembly.Load("MZcms.IServices");
            Assembly assembly1 = Assembly.Load("MZcms.Services");
            Assembly[] assemblyArray = new Assembly[] { assembly, assembly1 };
            builder.RegisterAssemblyTypes(assemblyArray).Where((Type t) => t.Name.EndsWith("Service")).AsImplementedInterfaces<object>();
        }

    }
}
