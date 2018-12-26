using System.Web.Http;
using MZcms.Web.Framework;
using System.Web.Routing;
using System.Web;
using System.Web.Mvc;
using System.Web.Http.Routing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Dispatcher;

namespace MZcms.Web
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
            // Web API ���úͷ���

            // Web API ·��
            config.MapHttpAttributeRoutes();
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.Routes.MapHttpRoute(
                name: "DefaultOpenApi",
                routeTemplate: "openapi/Hishop.Open.Api.I{controller}.{action}/{id}",
                defaults: new { controller = "OPATest", action = "Get", id = RouteParameter.Optional, defaultnamespace = "openapi" }
            );

#if DEBUG
            MZcms.Core.Log.Debug("[OpenApi]RegRoute-DefaultOpenApi");
#endif

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { Controller = "Test", Action = "Get", id = RouteParameter.Optional, defaultnamespace = "api" }
            );

            config.Routes.MapHttpRoute(
                name: "SmallApi",
                routeTemplate: "SmallProgAPI/{controller}/{action}/{id}",
                defaults: new { Controller = "Test", Action = "Get", id = RouteParameter.Optional, defaultnamespace = "SmallProgAPI" }
            );

            config.Services.Replace(typeof(IHttpControllerSelector), new NamespaceHttpControllerSelector(config));
        }
	}
}