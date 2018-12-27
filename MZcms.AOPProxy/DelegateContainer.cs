using System;
using System.Collections;

namespace MZcms.AOPProxy
{
	internal class DelegateContainer
	{
		private static Hashtable eventHandles;

		static DelegateContainer()
		{
			DelegateContainer.eventHandles = new Hashtable();
		}

		public DelegateContainer()
		{
		}

		public static void AddHandle(string typeName, string targetMethodName, InterceptionType handleType, object handle)
		{
			string str = string.Format("{0}${1}${2}", typeName, targetMethodName, handleType.ToString());
			DelegateContainer.eventHandles[str] = handle;
		}

		public static object GetHandle(string typeName, string targetMethodName, InterceptionType handleType)
		{
			string str = string.Format("{0}${1}${2}", typeName, targetMethodName, handleType.ToString());
			return DelegateContainer.eventHandles[str];
		}
	}
}