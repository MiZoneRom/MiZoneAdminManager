using System;

namespace MZcms.Core
{
	public class MZcmsException : ApplicationException
	{
		public MZcmsException()
		{
			Log.Info(Message, this);
		}

		public MZcmsException(string message) : base(message)
		{
			Log.Info(message, this);
		}

		public MZcmsException(string message, Exception inner) : base(message, inner)
		{
			Log.Info(message, inner);
		}
	}
}