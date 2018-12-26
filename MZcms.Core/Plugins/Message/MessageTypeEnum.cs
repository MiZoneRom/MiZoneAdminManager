using System;
using System.ComponentModel;

namespace MZcms.Core.Plugins.Message
{
	public enum MessageTypeEnum
	{
		[Description("订单创建时")]
		OrderCreated = 1,
		[Description("订单付款时")]
		OrderPay = 2,
		[Description("订单发货")]
		OrderShipping = 3,
		[Description("订单退款")]
		OrderRefund = 4,
		[Description("找回密码")]
		FindPassWord = 5,
		[Description("店铺审核")]
		ShopAudited = 6,
		[Description("开店成功")]
		ShopSuccess = 7
	}
}