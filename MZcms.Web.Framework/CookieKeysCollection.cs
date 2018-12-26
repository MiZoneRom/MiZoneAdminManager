using System;

namespace MZcms.Web.Framework
{
	public class CookieKeysCollection
	{
		public const string PLATFORM_MANAGER = "MZcms-PlatformManager";

		public const string SELLER_MANAGER = "MZcms-SellerManager";

		public const string MZcms_USER = "MZcms-User";

		public const string MZcms_USER_OpenID = "MZcms-User_OpenId";

		public const string MZcms_CART = "MZcms-CART";

		public const string MZcms_PRODUCT_BROWSING_HISTORY = "MZcms_ProductBrowsingHistory";

		public const string MZcms_LASTOPERATETIME = "MZcms_LastOpTime";

		public const string MobileAppType = "MZcms-Mobile-AppType";

		public const string MZcms_VSHOPID = "MZcms-VShopId";

        /// <summary>
        /// 用户角色(Admin)
        /// </summary>
        public const string USERROLE_ADMIN = "0";
        /// <summary>
        /// 用户角色(SellerAdmin)
        /// </summary>
        public const string USERROLE_SELLERADMIN = "1";
        /// <summary>
        /// 用户角色(User)
        /// </summary>
        public const string USERROLE_USER = "2";

        public CookieKeysCollection()
		{
		}
	}
}