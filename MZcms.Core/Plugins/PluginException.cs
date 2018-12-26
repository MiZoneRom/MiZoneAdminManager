using MZcms.Core;
using System;

namespace MZcms.Core.Plugins
{
	public class PluginException : MZcmsException
	{
		public PluginException()
		{
			Log.Info(Message, this);
		}

		public PluginException(string message) : base(message)
		{
			Log.Info(message, this);
		}

		public PluginException(string message, Exception inner) : base(message, inner)
		{
			Log.Info(message, inner);
		}
	}
}