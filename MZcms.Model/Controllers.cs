using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MZcms.Model
{
	public class Controllers
	{
		public List<string> ActionNames
		{
			get;
			set;
		}

		public string ControllerName
		{
			get;
			set;
		}

		public Controllers()
		{
            ActionNames = new List<string>();
		}
	}
}