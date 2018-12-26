using System;
using System.Runtime.CompilerServices;

namespace MZcms.Core.Plugins.Message
{
	public class MessageContent
	{
		public string Bind
		{
			get;
			set;
		}

		public string FindPassWord
		{
			get;
			set;
		}

		public string OrderCreated
		{
			get;
			set;
		}

		public string OrderPay
		{
			get;
			set;
		}

		public string OrderRefund
		{
			get;
			set;
		}

		public string OrderShipping
		{
			get;
			set;
		}

		public string ShopAudited
		{
			get;
			set;
		}

		public string ShopSuccess
		{
			get;
			set;
		}

		public MessageContent()
		{
		}
	}
}