using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MZcms.Model
{
	public class GroupActionItem
	{
		public string GroupName
		{
			get;
			set;
		}

		public List<ActionItem> Items
		{
			get;
			set;
		}

		public GroupActionItem()
		{
            Items = new List<ActionItem>();
		}
	}
}