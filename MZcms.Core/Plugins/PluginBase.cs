using System;
using System.Runtime.CompilerServices;

namespace MZcms.Core.Plugins
{
	public abstract class PluginBase
	{
		public MZcms.Core.Plugins.PluginInfo PluginInfo
		{
			get;
			set;
		}

		protected PluginBase()
		{
		}
	}
}