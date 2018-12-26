using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MZcms.Core.Plugins
{
	public class FormData
	{
		public IEnumerable<FormData.FormItem> Items
		{
			get;
			set;
		}

		public FormData()
		{
		}

		public class FormItem
		{
			public string DisplayName
			{
				get;
				set;
			}

			public bool IsRequired
			{
				get;
				set;
			}

			public string Name
			{
				get;
				set;
			}

			public FormData.FormItemType Type
			{
				get;
				set;
			}

			public string Value
			{
				get;
				set;
			}

			public FormItem()
			{
			}
		}

		public enum FormItemType
		{
			text = 1,
			checkbox = 2,
			password = 3
		}
	}
}