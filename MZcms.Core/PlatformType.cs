using System;
using System.ComponentModel;

namespace MZcms.Core
{
	public enum PlatformType
	{
		[Description("PC")]
		PC = 0,
		[Description("微信")]
		WeiXin = 1,
		[Description("Android")]
		Android = 2,
		[Description("IOS")]
		IOS = 3,
		[Description("触屏")]
		Wap = 4,
		[Description("移动端")]
		Mobile = 99
	}
}