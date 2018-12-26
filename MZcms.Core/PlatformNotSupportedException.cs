using System;

namespace MZcms.Core
{
	public class PlatformNotSupportedException : MZcmsException
	{
		public PlatformNotSupportedException(PlatformType platformType) : base(string.Concat("不支持", platformType.ToDescription(), "平台"))
		{
			Log.Info(Message, this);
		}
	}
}