using MZcms.Core.Helper;
using MZcms.Core.Plugins;
using MZcms.Core.Plugins.Message;
using MZcms.Core.Plugins.OAuth;
using MZcms.Core.Plugins.Payment;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MZcms.Core
{
	public static class PluginsManagement
	{
		private static Dictionary<PluginType, List<PluginInfo>> IntalledPlugins;

		private static bool registed;

		static PluginsManagement()
		{
			PluginsManagement.IntalledPlugins = new Dictionary<PluginType, List<PluginInfo>>();
			PluginsManagement.registed = false;
			foreach (object value in Enum.GetValues(typeof(PluginType)))
			{
				PluginsManagement.IntalledPlugins.Add((PluginType)value, new List<PluginInfo>());
			}
		}

		private static PluginInfo AddPluginInfo(FileInfo dllFile)
		{
			PluginInfo fullName;
			string str = dllFile.Name.Replace(".dll", "");
			string str1 = string.Concat(IOHelper.GetMapPath("/plugins/configs/"), str, ".config");
			if (File.Exists(str1))
			{
				fullName = (PluginInfo)XmlHelper.DeserializeFromXML(typeof(PluginInfo), str1);
			}
			else
			{
				FileInfo[] files = dllFile.Directory.GetFiles("plugin.config", SearchOption.TopDirectoryOnly);
				if (files.Length <= 0)
				{
					throw new FileNotFoundException(string.Concat("未找到插件", str, "的配置文件"));
				}
				fullName = (PluginInfo)XmlHelper.DeserializeFromXML(typeof(PluginInfo), files[0].FullName);
				fullName.PluginId = str;
				fullName.PluginDirectory = dllFile.Directory.FullName;
				fullName.AddedTime = new DateTime?(DateTime.Now);
				XmlHelper.SerializeToXml(fullName, str1);
			}
			PluginsManagement.UpdatePluginList(fullName);
			return fullName;
		}

		private static PluginInfo DeepClone(PluginInfo plugin)
		{
			return JsonConvert.DeserializeObject<PluginInfo>(JsonConvert.SerializeObject(plugin));
		}

		public static void EnablePlugin(string pluginId, bool enable)
		{
			try
			{
				Plugin<IPlugin> plugin = PluginsManagement.GetPlugin<IPlugin>(pluginId);
				if (enable)
				{
					plugin.Biz.CheckCanEnable();
				}
				PluginsManagement.EnablePlugin_Private(pluginId, enable);
			}
			catch
			{
				throw;
			}
		}

		private static void EnablePlugin_Private(string pluginId, bool enable)
		{
			PluginInfo pluginInfo = PluginsManagement.GetPluginInfo(pluginId);
			if (pluginInfo == null)
			{
				throw new PluginNotFoundException(pluginId);
			}
			pluginInfo.Enable = enable;
			XmlHelper.SerializeToXml(pluginInfo, string.Concat(IOHelper.GetMapPath("/plugins/configs/"), pluginId, ".config"));
		}

		public static T GetInstalledPlugin<T>(string pluginId)
		where T : IPlugin
		{
			T t = default(T);
			PluginInfo pluginInfo = PluginsManagement.GetPluginInfo(pluginId);
			if (pluginInfo != null)
			{
				t = Instance.Get<T>(pluginInfo.ClassFullName);
			}
			return t;
		}

		public static IEnumerable<PluginInfo> GetInstalledPluginInfos(PluginType pluginType)
		{
			IEnumerable<PluginInfo> pluginInfos = 
				from item in PluginsManagement.IntalledPlugins[pluginType]
				select PluginsManagement.DeepClone(item);
			return pluginInfos;
		}

		public static IEnumerable<T> GetInstalledPlugins<T>(PluginType pluginType)
		where T : IPlugin
		{
			IEnumerable<PluginInfo> installedPluginInfos = PluginsManagement.GetInstalledPluginInfos(pluginType);
			T[] tArray = new T[installedPluginInfos.Count()];
			int num = 0;
			foreach (PluginInfo installedPluginInfo in installedPluginInfos)
			{
				int num1 = num;
				num = num1 + 1;
				tArray[num1] = Instance.Get<T>(installedPluginInfo.ClassFullName);
			}
			return tArray;
		}

		public static Plugin<T> GetPlugin<T>(string pluginId)
		where T : IPlugin
		{
			PluginInfo pluginInfo = PluginsManagement.GetPluginInfo(pluginId);
			Plugin<T> plugin = new Plugin<T>()
			{
				PluginInfo = pluginInfo,
				Biz = Instance.Get<T>(pluginInfo.ClassFullName)
			};
			return plugin;
		}

		private static IEnumerable<string> GetPluginFiles(string pluginDirectory)
		{
			if (!Directory.Exists(pluginDirectory))
			{
				throw new MZcmsException(string.Concat("未能找到指定的插件目录:", pluginDirectory));
			}
			return Directory.GetFiles(pluginDirectory, "*.dll", SearchOption.AllDirectories);
		}

		public static PluginInfo GetPluginInfo(string pluginId)
		{
			PluginInfo pluginInfo = null;
			foreach (List<PluginInfo> value in PluginsManagement.IntalledPlugins.Values)
			{
				pluginInfo = value.FirstOrDefault((PluginInfo item) => item.PluginId == pluginId);
				if (pluginInfo != null)
				{
					break;
				}
			}
			return pluginInfo;
		}

		public static IEnumerable<Plugin<T>> GetPlugins<T>()
		where T : IPlugin
		{
			IEnumerable<PluginInfo> installedPluginInfos = PluginsManagement.GetInstalledPluginInfos(PluginsManagement.GetPluginTypeByType(typeof(T)));
			int num = installedPluginInfos.Count();
			Plugin<T>[] pluginArray = new Plugin<T>[num];
			for (int i = 0; i < num; i++)
			{
				Plugin<T> plugin = new Plugin<T>()
				{
					Biz = Instance.Get<T>(installedPluginInfos.ElementAt<PluginInfo>(i).ClassFullName),
					PluginInfo = installedPluginInfos.ElementAt<PluginInfo>(i)
				};
				pluginArray[i] = plugin;
			}
			return pluginArray;
		}

		public static IEnumerable<Plugin<T>> GetPlugins<T>(bool onlyEnabled)
		where T : IPlugin
		{
			IEnumerable<Plugin<T>> plugins = PluginsManagement.GetPlugins<T>();
			if (onlyEnabled)
			{
				plugins = 
					from item in plugins
					where item.PluginInfo.Enable
					select item;
			}
			return plugins;
		}

		private static PluginType GetPluginTypeByType(Type pluginType)
		{
			PluginType pluginType1;
			if (pluginType == typeof(IPaymentPlugin))
			{
				pluginType1 = PluginType.PayPlugin;
			}
			else if (pluginType == typeof(IExpress))
			{
				pluginType1 = PluginType.Express;
			}
			else if (pluginType == typeof(IOAuthPlugin))
			{
				pluginType1 = PluginType.OauthPlugin;
			}
			else if (pluginType == typeof(IMessagePlugin))
			{
				pluginType1 = PluginType.Message;
			}
			else if (!(pluginType == typeof(ISMSPlugin)))
			{
				if (!(pluginType == typeof(IEmailPlugin)))
				{
					throw new NotSupportedException(string.Concat("暂不支持", pluginType.Name, "类型的插件"));
				}
				pluginType1 = PluginType.Email;
			}
			else
			{
				pluginType1 = PluginType.SMS;
			}
			return pluginType1;
		}

		private static Assembly InstallDll(string dllFileName)
		{
			DirectoryInfo directoryInfo;
			string str = dllFileName;
			FileInfo fileInfo = new FileInfo(dllFileName);
			directoryInfo = (string.IsNullOrWhiteSpace(AppDomain.CurrentDomain.DynamicDirectory) ? new DirectoryInfo(IOHelper.GetMapPath("")) : new DirectoryInfo(AppDomain.CurrentDomain.DynamicDirectory));
			str = string.Concat(directoryInfo.FullName, "\\", fileInfo.Name);
			Assembly assembly = null;
			PluginInfo pluginInfo = null;
			try
			{
				try
				{
					File.Copy(dllFileName, str, true);
				}
				catch
				{
					Guid guid = Guid.NewGuid();
					File.Move(str, string.Concat(str, guid.ToString("N"), ".locked"));
					File.Copy(dllFileName, str, true);
				}
				assembly = Assembly.Load(AssemblyName.GetAssemblyName(str));
				if (assembly.FullName.StartsWith("MZcms.Plugin"))
				{
					pluginInfo = PluginsManagement.AddPluginInfo(fileInfo);
					IPlugin fullName = Instance.Get<IPlugin>(pluginInfo.ClassFullName);
					fullName.WorkDirectory = fileInfo.Directory.FullName;
				}
			}
			catch (IOException oException)
			{
				Log.Error(string.Concat("插件复制失败(", dllFileName, ")！"), oException);
				if (pluginInfo != null)
				{
					PluginsManagement.RemovePlugin(pluginInfo);
				}
			}
			catch (Exception exception)
			{
				Log.Error(string.Concat("插件加载失败(", dllFileName, ")！"), exception);
				if (pluginInfo != null)
				{
					PluginsManagement.RemovePlugin(pluginInfo);
				}
			}
			return assembly;
		}

		public static void InstallPlugin(string pluginFullDirectory)
		{
			foreach (string pluginFile in PluginsManagement.GetPluginFiles(pluginFullDirectory))
			{
				try
				{
					PluginsManagement.InstallDll(pluginFile);
				}
				catch (Exception exception)
				{
					Log.Error(string.Concat("插件安装失败(", pluginFile, ")"), exception);
				}
			}
		}

		public static void RegistAtStart()
		{
			if (!PluginsManagement.registed)
			{
				PluginsManagement.registed = true;
				string mapPath = IOHelper.GetMapPath("/plugins");
				List<string> list = PluginsManagement.GetPluginFiles(mapPath).ToList();
				list.AddRange(PluginsManagement.GetPluginFiles(IOHelper.GetMapPath("/Strategies")));
				foreach (string str in list)
				{
					PluginsManagement.InstallDll(str);
				}
			}
		}

		private static void RemovePlugin(PluginInfo plugin)
		{
			foreach (PluginType pluginType in plugin.PluginTypes)
			{
				PluginsManagement.IntalledPlugins[pluginType].Remove(plugin);
			}
		}

		public static void UninstallPlugin(string pluginId)
		{
			throw new NotImplementedException();
		}

		public static void UnInstallPlugin(string classFullName)
		{
			List<PluginInfo> pluginInfos = new List<PluginInfo>();
			foreach (List<PluginInfo> value in PluginsManagement.IntalledPlugins.Values)
			{
				pluginInfos.AddRange(value);
			}
			PluginInfo pluginInfo = pluginInfos.FirstOrDefault((PluginInfo item) => item.ClassFullName == classFullName);
			if (pluginInfo != null)
			{
				foreach (PluginType pluginType in pluginInfo.PluginTypes)
				{
					PluginsManagement.IntalledPlugins[pluginType].Remove(pluginInfo);
				}
				try
				{
					Directory.Delete(pluginInfo.PluginDirectory, true);
				}
				catch
				{
					Log.Warn(string.Format("移除插件{0}时没有找到对应的插件目录", pluginInfo.PluginId));
				}
			}
			else
			{
				Log.Warn(string.Format("卸载插件{0}时没有找到插件信息", classFullName));
			}
		}

		private static void UpdatePluginList(PluginInfo plugin)
		{
			foreach (PluginType pluginType in plugin.PluginTypes)
			{
				PluginsManagement.IntalledPlugins[pluginType].Add(plugin);
			}
		}
	}
}