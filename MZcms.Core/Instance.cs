using Autofac;
using Autofac.Builder;
using System;

namespace MZcms.Core
{
	public class Instance
	{
		public Instance()
		{
		}

		public static T Get<T>(string classFullName)
		{
			T t;
			ContainerBuilder containerBuilder = new ContainerBuilder();
			IContainer container = null;
			try
			{
				try
				{
					Type type = Type.GetType(classFullName);
					containerBuilder.RegisterType(type).As<T>();
					container = containerBuilder.Build(ContainerBuildOptions.None);
					t = container.Resolve<T>();
				}
				catch (Exception exception)
				{
					throw new InstanceCreateException("创建实例异常", exception);
				}
			}
			finally
			{
				if (container != null)
				{
					container.Dispose();
				}
			}
			return t;
		}
	}
}