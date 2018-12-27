using MZcms.AOPProxy;
using MZcms.Core;
using MZcms.IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MZcms.ServiceProvider
{
	public class LogInterception : IAOPInterception
	{
		public LogInterception()
		{
		}

		[Interception(typeof(IService), "LogException", InterceptionType.OnLogException)]
		public static void LogException(string methodName, Dictionary<string, object> parameters, Exception ex)
		{
			string str = "";
			JsonSerializerSettings jsonSerializerSetting = new JsonSerializerSettings()
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			};
			foreach (KeyValuePair<string, object> parameter in parameters)
			{
				str = string.Concat(str, string.Format("【 {0} : {1} 】，", parameter.Key, JsonConvert.SerializeObject(parameter.Value, jsonSerializerSetting)));
			}
			str = (string.IsNullOrEmpty(str) ? "" : str.Substring(0, str.Length - 1));
			Log.Error(string.Format("Method:{0} , Parameters:{1}", methodName, str), ex);
		}
	}
}