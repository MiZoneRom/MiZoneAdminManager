using Autofac;
using Autofac.Builder;
using Autofac.Configuration;
using Autofac.Configuration.Core;
using Autofac.Configuration.Elements;
using Himall.AOPProxy;
using Himall.IServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Proxies;

namespace MZcms.ServiceProvider
{
	public class Instance<T>
	where T : IService
	{
		private static object locker;

		private static Hashtable ServiceProviders;

		public static T Create
		{
			get
			{
				T t;
				ContainerBuilder containerBuilder = new ContainerBuilder();
				ConfigurationSettingsReader configurationSettingsReader = new ConfigurationSettingsReader("autofac");
				ComponentElement componentElement = configurationSettingsReader.SectionHandler.Components.FirstOrDefault((ComponentElement item) => item.Service.Contains(typeof(T).FullName));
				Instance<T>.GetServiceProviders();
				try
				{
					if (componentElement != null)
					{
						containerBuilder.RegisterType<T>();
						containerBuilder.RegisterModule(configurationSettingsReader);
					}
					else
					{
						string name = typeof(T).Name;
						string fullName = typeof(T).FullName;
						string str = fullName.Substring(0, fullName.LastIndexOf('.'));
						string str1 = Instance<T>.ServiceProviders[str] as string;
						if (str1 == null)
						{
							throw new ApplicationException(string.Concat("未配置", fullName, "的实现"));
						}
						char[] chrArray = new char[] { ',' };
						string str2 = str1.Split(chrArray)[0];
						char[] chrArray1 = new char[] { ',' };
						string str3 = str1.Split(chrArray1)[1];
						string str4 = name.Substring(1);
						string str5 = string.Format("{0}.{1}, {2}", str2, str4, str3);
						Type type = Type.GetType(str5);
						if (type == null)
						{
							throw new NotImplementedException(string.Concat("未找到", str5));
						}
						containerBuilder.RegisterType(type).As<T>();
					}
					T t1 = containerBuilder.Build(ContainerBuildOptions.None).Resolve<T>();
					string str6 = ConfigurationManager.AppSettings["IsAopProxy"];
					t = (string.IsNullOrEmpty(str6) || !Convert.ToBoolean(str6) ? t1 : (T)(new AopProxy<T>(t1)).GetTransparentProxy());
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					throw new ServiceInstacnceCreateException(string.Concat(typeof(T).Name, "服务实例创建失败"), exception);
				}
				return t;
			}
		}

		static Instance()
		{
			Instance<T>.locker = new object();
			Instance<T>.ServiceProviders = null;
		}

		public Instance()
		{
		}

		private static void GetServiceProviders()
		{
			if (Instance<T>.ServiceProviders == null)
			{
				lock (Instance<T>.locker)
				{
					if (Instance<T>.ServiceProviders == null)
					{
						Instance<T>.ServiceProviders = new Hashtable();
						foreach (Item item in (ConfigurationManager.GetSection("serviceProvider") as ServiceProviderConfig).Items)
						{
							if (Instance<T>.ServiceProviders.ContainsKey(item.Interface))
							{
								throw new ApplicationException(string.Concat("配置文件中最多只能配置一个", item.Interface, "的实现"));
							}
							Instance<T>.ServiceProviders.Add(item.Interface, string.Concat(item.NameSpace, ",", item.Assembly));
						}
					}
				}
			}
		}
	}
}