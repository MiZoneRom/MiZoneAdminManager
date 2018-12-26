using MZcms.Core.Helper;
using MZcms.IServices;
using MZcms.Model;
using MZcms.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace MZcms.Web.Framework
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
	public class OperationLogAttribute : ActionFilterAttribute
	{
		public string ParameterNameList;

		public string Message
		{
			get;
			set;
		}

		public OperationLogAttribute()
		{
		}

		public OperationLogAttribute(string message, string parameterNameList = "")
		{
            Message = message;
            ParameterNameList = parameterNameList;
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.Exception != null)
			{
				return;
			}
			string str = filterContext.RouteData.Values["controller"].ToString();
			string str1 = filterContext.RouteData.Values["action"].ToString();
			object item = filterContext.RouteData.Values["area"];
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Concat(Message, ",操作记录:"));
			if (!string.IsNullOrEmpty(ParameterNameList))
			{
				Dictionary<string, string> strs = new Dictionary<string, string>();
				string parameterNameList = ParameterNameList;
				char[] chrArray = new char[] { ',', '|' };
				string[] strArrays = parameterNameList.Split(chrArray);
				for (int i = 0; i < strArrays.Length; i++)
				{
					string str2 = strArrays[i];
					ValueProviderResult value = filterContext.Controller.ValueProvider.GetValue(str2);
					if (value != null && !strs.ContainsKey(str2))
					{
						strs.Add(str2, value.AttemptedValue);
					}
				}
				foreach (KeyValuePair<string, string> keyValuePair in strs)
				{
					stringBuilder.AppendFormat("{0}:{1} ", keyValuePair.Key, keyValuePair.Value);
				}
			}
			LogInfo logInfo = new LogInfo()
			{
				Date = DateTime.Now,
				IPAddress = WebHelper.GetIP(),
				UserName = (filterContext.Controller as BaseAdminController).CurrentManager.UserName,
				PageUrl = string.Concat(str, "/", str1),
				Description = stringBuilder.ToString()
			};
			LogInfo logInfo1 = logInfo;
			Task.Factory.StartNew(() => Instance<IOperationLogService>.Create.AddPlatformOperationLog(logInfo1));
			base.OnActionExecuted(filterContext);
		}
	}
}