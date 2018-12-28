using MZcms.Entity;
using System;
using System.Data.Entity;

namespace MZcms.Service
{
	public class ServiceBase
	{
		protected Entities context;

		public ServiceBase()
		{
            context = new Entities();
		}

		public void Dispose()
		{
			if (context != null)
			{
                context.Dispose();
			}
		}
	}
}