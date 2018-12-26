using System;
using System.Runtime.CompilerServices;

namespace MZcms.Core.Plugins.Payment
{
	public class PaymentPara
	{
		public string out_refund_no
		{
			get;
			set;
		}

		public string out_trade_no
		{
			get;
			set;
		}

		public decimal refund_fee
		{
			get;
			set;
		}

		public decimal total_fee
		{
			get;
			set;
		}

		public PaymentPara()
		{
		}
	}
}