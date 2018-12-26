using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace MZcms.Core.Plugins
{
    public class PluginInfo
    {
        public DateTime? AddedTime
        {
            get;
            set;
        }

        public string Author
        {
            get;
            set;
        }

        public string ClassFullName
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public int DisplayIndex
        {
            get;
            set;
        }

        public string DisplayName
        {
            get;
            set;
        }

        public bool Enable
        {
            get;
            set;
        }

        public string Logo
        {
            get;
            set;
        }

        public string MaxMZcmsVersion
        {
            get;
            set;
        }

        public string MinMZcmsVersion
        {
            get;
            set;
        }

        public string PluginDirectory
        {
            get;
            set;
        }

        public string PluginId
        {
            get;
            set;
        }

        [XmlIgnore]
        [JsonIgnore]
        public IEnumerable<PluginType> PluginTypes
        {
            get
            {
                string type = Type;
                char[] chrArray = new char[] { ',' };
                IEnumerable<PluginType> pluginTypes =
                    from item in type.Split(chrArray)
                    select (PluginType)int.Parse(item);
                return pluginTypes;
            }
        }

        public string Type
        {
            get;
            set;
        }

        public string Version
        {
            get;
            set;
        }

        public PluginInfo()
        {
        }
    }
}