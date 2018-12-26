using MZcms.IServices;
using MZcms.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;

namespace MZcms.Web.Framework
{
	public static class ServiceHelper
	{
		public static T Create<T>()
		where T : IService
		{
			T create = Instance<T>.Create;
			if (HttpContext.Current != null)
			{
				List<IService> item = HttpContext.Current.Session["_serviceInstace"] as List<IService>;
				if (item != null)
				{
					item.Add(create);
				}
				else
				{
					item = new List<IService>()
					{
						create
					};
				}
				HttpContext.Current.Session["_serviceInstace"] = item;
			}
			return create;
		}
	}
}