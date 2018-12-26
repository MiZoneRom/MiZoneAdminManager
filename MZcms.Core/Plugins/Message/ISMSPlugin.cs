using MZcms.Core.Plugins;
using System;

namespace MZcms.Core.Plugins.Message
{
	public interface ISMSPlugin : IMessagePlugin, IPlugin
	{
		string GetBuyLink();

		string GetLoginLink();

		string GetSMSAmount();
	}
}