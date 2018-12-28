using System;
namespace MZcms.Core
{
	public class MetaInfo
	{
		public DateTime LastModifiedTime
		{
			get;
			set;
		}
		public long ContentLength
		{
			get;
			set;
		}
		public string ContentType
		{
			get;
			set;
		}
		public string ObjectType
		{
			get;
			set;
		}
	}
}
