using System;

namespace MZcms.Core
{
	public class InstanceCreateException : MZcmsException
	{
		public InstanceCreateException()
		{
		}

		public InstanceCreateException(string message) : base(message)
		{
		}

		public InstanceCreateException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}