using System;

namespace MZcms.Core.Plugins
{
	public interface IPlugin
	{
		string WorkDirectory
		{
			set;
		}

		void CheckCanEnable();
	}
}