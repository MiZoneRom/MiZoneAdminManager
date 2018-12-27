using System;
using System.Collections.Generic;

namespace MZcms.AOPProxy
{
	public delegate void OnLogException(string methodName, Dictionary<string, object> parameters, Exception ex);
}