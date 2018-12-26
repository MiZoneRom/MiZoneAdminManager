using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MZcms.Core.Plugins.Payment
{
	public class PaymentInfo
	{
		public IEnumerable<long> OrderIds
		{
			get;
			set;
		}

		public string ResponseContentWhenFinished
		{
			get;
			set;
		}

		public DateTime? TradeTime
		{
			get;
			set;
		}

		public string TradNo
		{
			get;
			set;
		}

		public PaymentInfo()
		{
		}
	}
}