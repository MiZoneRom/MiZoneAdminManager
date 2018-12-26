using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MZcms.Model
{
	public class ActionItem
	{
		public List<MZcms.Model.Controllers> Controllers
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public int PrivilegeId
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public ActionItem()
		{
            Controllers = new List<Himall.Model.Controllers>();
		}
	}
}