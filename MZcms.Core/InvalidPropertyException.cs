using System;

namespace MZcms.Core
{
	public class InvalidPropertyException : MZcmsException
	{
		public InvalidPropertyException()
		{
		}

		public InvalidPropertyException(string message) : base(message)
		{
		}

		public InvalidPropertyException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}