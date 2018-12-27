using System;
using System.Runtime.CompilerServices;

namespace MZcms.AOPProxy
{
	[AttributeUsage(AttributeTargets.Method)]
	public class Interception : Attribute
	{
		public string TargetMethodName
		{
			get;
			set;
		}

		public System.Type TargetType
		{
			get;
			set;
		}

		public InterceptionType Type
		{
			get;
			set;
		}

		public Interception(System.Type targetType, string targetMethodName, InterceptionType type)
		{
            TargetType = targetType;
            TargetMethodName = targetMethodName;
            Type = type;
		}
	}
}