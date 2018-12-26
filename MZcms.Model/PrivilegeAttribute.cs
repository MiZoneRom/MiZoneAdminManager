using System;
using System.Runtime.CompilerServices;

namespace MZcms.Model
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple=true, Inherited=true)]
	public class PrivilegeAttribute : Attribute
	{
		public string Action
		{
			get;
			set;
		}

		public string Controller
		{
			get;
			set;
		}

		public string GroupName
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public int Pid
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public PrivilegeAttribute(string groupName, string name, int pid, string url, string controller, string action = "")
		{
            Name = name;
            GroupName = groupName;
            Pid = pid;
            Url = url;
            Controller = controller;
            Action = action;
		}

		public PrivilegeAttribute(string controller, string action = "")
		{
            Controller = controller;
            Action = action;
		}
	}
}