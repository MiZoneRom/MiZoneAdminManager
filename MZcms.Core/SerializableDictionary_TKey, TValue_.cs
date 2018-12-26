using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MZcms.Core
{
	[XmlRoot("SerializableDictionary")]
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
	{
		public SerializableDictionary()
		{
		}

		public SerializableDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary)
		{
		}

		public SerializableDictionary(IEqualityComparer<TKey> comparer) : base(comparer)
		{
		}

		public SerializableDictionary(int capacity) : base(capacity)
		{
		}

		public SerializableDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer)
		{
		}

		protected SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer xmlSerializer1 = new XmlSerializer(typeof(TValue));
			bool isEmptyElement = reader.IsEmptyElement;
			reader.Read();
			if (!isEmptyElement)
			{
				while (reader.NodeType != XmlNodeType.EndElement)
				{
					reader.ReadStartElement("item");
					reader.ReadStartElement("key");
					TKey tKey = (TKey)xmlSerializer.Deserialize(reader);
					reader.ReadEndElement();
					reader.ReadStartElement("value");
					TValue tValue = (TValue)xmlSerializer1.Deserialize(reader);
					reader.ReadEndElement();
					base.Add(tKey, tValue);
					reader.ReadEndElement();
					reader.MoveToContent();
				}
				reader.ReadEndElement();
			}
		}

		public void WriteXml(XmlWriter writer)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer xmlSerializer1 = new XmlSerializer(typeof(TValue));
			foreach (TKey key in base.Keys)
			{
				writer.WriteStartElement("item");
				writer.WriteStartElement("key");
				xmlSerializer.Serialize(writer, key);
				writer.WriteEndElement();
				writer.WriteStartElement("value");
				xmlSerializer1.Serialize(writer, base[key]);
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
		}
	}
}