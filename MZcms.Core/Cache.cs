using Autofac;
using Autofac.Builder;
using Autofac.Configuration;
using System;

namespace MZcms.Core
{
	public static class Cache
	{
		private static object cacheLocker;

		private static ICache cache;

		public static int TimeOut
		{
			get
			{
				return Cache.cache.TimeOut;
			}
			set
			{
				lock (Cache.cacheLocker)
				{
					Cache.cache.TimeOut = value;
				}
			}
		}

		static Cache()
		{
			Cache.cacheLocker = new object();
			Cache.cache = null;
			Cache.Load();
		}

		public static object Get(string key)
		{
			object obj;
			obj = (!string.IsNullOrWhiteSpace(key) ? Cache.cache.Get(key) : null);
			return obj;
		}

		public static void Insert(string key, object data)
		{
			if ((string.IsNullOrWhiteSpace(key) ? false : data != null))
			{
				lock (Cache.cacheLocker)
				{
					Cache.cache.Insert(key, data);
				}
			}
		}

		public static void Insert(string key, object data, int cacheTime)
		{
			if ((string.IsNullOrWhiteSpace(key) ? false : data != null))
			{
				lock (Cache.cacheLocker)
				{
					Cache.cache.Insert(key, data, cacheTime);
				}
			}
		}

		public static void Insert(string key, object data, DateTime cacheTime)
		{
			if ((string.IsNullOrWhiteSpace(key) ? false : data != null))
			{
				lock (Cache.cacheLocker)
				{
					Cache.cache.Insert(key, data, cacheTime);
				}
			}
		}

		private static void Load()
		{
			ContainerBuilder containerBuilder = new ContainerBuilder();
			containerBuilder.RegisterType<ICache>();
			containerBuilder.RegisterModule(new ConfigurationSettingsReader("autofac"));
			IContainer container = null;
			try
			{
				try
				{
					container = containerBuilder.Build(ContainerBuildOptions.None);
					Cache.cache = container.Resolve<ICache>();
				}
				catch (Exception exception)
				{
					throw new CacheRegisterException("注册缓存服务异常", exception);
				}
			}
			finally
			{
				if (container != null)
				{
					container.Dispose();
				}
			}
		}

		public static void Remove(string key)
		{
			if (!string.IsNullOrWhiteSpace(key))
			{
				lock (Cache.cacheLocker)
				{
					Cache.cache.Remove(key);
				}
			}
		}
	}
}