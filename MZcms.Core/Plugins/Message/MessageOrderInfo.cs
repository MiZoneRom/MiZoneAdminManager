using System;
using System.Runtime.CompilerServices;

namespace MZcms.Core.Plugins.Message
{
	public class MessageOrderInfo
	{
		public string OrderId
		{
			get;
			set;
		}

		public decimal RefundMoney
		{
			get;
			set;
		}

		public string ShippingCompany
		{
			get;
			set;
		}

		public string ShippingNumber
		{
			get;
			set;
		}

		public long ShopId
		{
			get;
			set;
		}

		public string ShopName
		{
			get;
			set;
		}

		public string SiteName
		{
			get;
			set;
		}

		public decimal TotalMoney
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public MessageOrderInfo()
		{
		}
	}
}