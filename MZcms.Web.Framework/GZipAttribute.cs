using System;
using System.Collections.Specialized;
using System.IO.Compression;
using System.Web;
using System.Web.Mvc;

namespace MZcms.Web
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
	public class GZipAttribute : ActionFilterAttribute
	{
		private bool isEnableCompression = true;

		public GZipAttribute()
		{
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			object[] customAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(NoCompress), false);
			if (filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(NoCompress), false).Length == 1 || customAttributes.Length == 1)
			{
                isEnableCompression = false;
			}
		}

		public override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			if (filterContext.Exception != null || !isEnableCompression)
			{
				return;
			}
			HttpResponseBase response = filterContext.HttpContext.Response;
			if (response.Filter is GZipStream || response.Filter is DeflateStream)
			{
				return;
			}
			string item = filterContext.HttpContext.Request.Headers["Accept-Encoding"];
			if (!string.IsNullOrEmpty(item))
			{
				item = item.ToLower();
				if (item.Contains("gzip") && response.Filter != null)
				{
					response.AppendHeader("Content-Encoding", "gzip");
					response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
					return;
				}
				if (item.Contains("deflate") && response.Filter != null)
				{
					response.AppendHeader("Content-Encoding", "deflate");
					response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
				}
			}
		}
	}
}