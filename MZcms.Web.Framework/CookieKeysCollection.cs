using System;

namespace MZcms.Web.Framework
{
	public class CookieKeysCollection
	{
		public const string PLATFORM_MANAGER = "Himall-PlatformManager";

		public const string SELLER_MANAGER = "Himall-SellerManager";

		public const string HIMALL_USER = "Himall-User";

		public const string HIMALL_USER_OpenID = "Himall-User_OpenId";

		public const string HIMALL_CART = "HIMALL-CART";

		public const string HIMALL_PRODUCT_BROWSING_HISTORY = "Himall_ProductBrowsingHistory";

		public const string HIMALL_LASTOPERATETIME = "Himall_LastOpTime";

		public const string MobileAppType = "Himall-Mobile-AppType";

		public const string HIMALL_VSHOPID = "Himall-VShopId";

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