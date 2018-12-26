using System;
using System.Runtime.CompilerServices;

namespace MZcms.Core.Plugins.OAuth
{
	public class OAuthUserInfo
	{
		public bool? IsMale
		{
			get;
			set;
		}

		public string NickName
		{
			get;
			set;
		}

		public string OpenId
		{
			get;
			set;
		}

		public string RealName
		{
			get;
			set;
		}

		public string UnionId
		{
			get;
			set;
		}
        public string UnionOpenId
        {
            get;
            set;
        }
        public string HeadImgUrl
        {
            get;
            set;
        }
        public OAuthUserInfo()
		{
		}
	}
}