using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MZcms.Model
{
	public class Privileges
	{
		public List<GroupActionItem> Privilege
		{
			get;
			set;
		}

		public Privileges()
		{
            Privilege = new List<GroupActionItem>();
		}
	}
}