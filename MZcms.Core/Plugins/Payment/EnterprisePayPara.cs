using System;
using System.Runtime.CompilerServices;

namespace MZcms.Core.Plugins.Payment
{
	public class EnterprisePayPara
	{
		private bool checkname = false;

		public decimal amount
		{
			get;
			set;
		}

		public bool check_name
		{
			get
			{
				return checkname;
			}
			set
			{
                checkname = value;
			}
		}

		public string desc
		{
			get;
			set;
		}

		public string openid
		{
			get;
			set;
		}

		public string out_trade_no
		{
			get;
			set;
		}

		public string re_user_name
		{
			get;
			set;
		}

		public string spbill_create_ip
		{
			get;
			set;
		}

		public EnterprisePayPara()
		{
		}
	}
}