using Himall.Core;
using System;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;

namespace MZcms.Web.Framework
{
	public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
	{
		public ApiExceptionFilterAttribute()
		{
		}

		public override void OnException(HttpActionExecutedContext context)
		{
			HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
			if (!(context.Exception is HimallException))
			{
				httpResponseMessage.Content = new StringContent(string.Concat("Success = false, Error = 102, ErrorMsg =", context.Exception.Message), Encoding.GetEncoding("UTF-8"), "application/json");
			}
			else
			{
				httpResponseMessage.Content = new StringContent(string.Concat("Success = false, Error = 101, ErrorMsg =", context.Exception.Message), Encoding.GetEncoding("UTF-8"), "application/json");
			}
			context.Response = httpResponseMessage;
		}
	}
}