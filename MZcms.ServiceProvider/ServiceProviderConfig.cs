using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Xml;

namespace MZcms.ServiceProvider
{
	public class ServiceProviderConfig : IConfigurationSectionHandler
	{
		public List<Item> Items
		{
			get;
			set;
		}

		public ServiceProviderConfig()
		{
            Items = new List<Item>();
		}

		public object Create(object parent, object configContext, XmlNode section)
		{
			ServiceProviderConfig serviceProviderConfig = new ServiceProviderConfig();
			foreach (XmlNode xmlNodes in section.SelectNodes("item"))
			{
				if (xmlNodes == null || xmlNodes.Attributes == null)
				{
					continue;
				}
				Item item = new Item();
				XmlAttribute itemOf = xmlNodes.Attributes["interface"];
				if (itemOf == null)
				{
					throw new ApplicationException("配置文件中存在未指定接口的项");
				}
				item.Interface = itemOf.Value;
				XmlAttribute xmlAttribute = xmlNodes.Attributes["assembly"];
				XmlAttribute itemOf1 = xmlNodes.Attributes["namespace"];
				if (xmlAttribute == null)
				{
					throw new ApplicationException(string.Concat("配置文件接口", item.Interface, "未指定程序集"));
				}
				if (itemOf1 == null)
				{
					throw new ApplicationException(string.Concat("配置文件接口", item.Interface, "未指定命名空间"));
				}
				item.Assembly = xmlAttribute.Value;
				item.NameSpace = itemOf1.Value;
				serviceProviderConfig.Items.Add(item);
			}
			return serviceProviderConfig;
		}
	}
}