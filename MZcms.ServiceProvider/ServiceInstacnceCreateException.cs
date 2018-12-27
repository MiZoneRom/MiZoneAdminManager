using MZcms.Core;
using System;

namespace MZcms.ServiceProvider
{
	public class ServiceInstacnceCreateException : MZcmsException
	{
		public ServiceInstacnceCreateException()
		{
		}

		public ServiceInstacnceCreateException(string message) : base(message)
		{
		}

		public ServiceInstacnceCreateException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}