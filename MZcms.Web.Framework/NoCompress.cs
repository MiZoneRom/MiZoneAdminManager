using System;

namespace MZcms.Web.Framework
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
	public class NoCompress : Attribute
	{
		public NoCompress()
		{
		}
	}
}