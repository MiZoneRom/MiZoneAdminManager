using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace MZcms.Web.Framework
{
	public class XmlResult : ActionResult
	{
		private Type _dataType;

		private XmlResultType _type;

		private object Data
		{
			get;
			set;
		}

		private string Xml
		{
			get;
			set;
		}

		public XmlResult(object data)
		{
            _type = XmlResultType.Object;
            Data = data;
            _dataType = data.GetType();
		}

		public XmlResult(string xml)
		{
            _type = XmlResultType.String;
            Xml = xml;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			HttpResponseBase response = context.HttpContext.Response;
			response.ContentType = "text/xml";
			switch (_type)
			{
				case XmlResultType.Object:
				{
					if (Data == null)
					{
						break;
					}
					XmlSerializer xmlSerializer = new XmlSerializer(_dataType);
					MemoryStream memoryStream = new MemoryStream();
					xmlSerializer.Serialize(memoryStream, Data);
					response.Write(Encoding.UTF8.GetString(memoryStream.ToArray()));
					return;
				}
				case XmlResultType.String:
				{
					response.Write(Xml);
					break;
				}
				default:
				{
					return;
				}
			}
		}
	}
}