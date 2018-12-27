using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;

namespace MZcms.AOPProxy
{
	public class MethodParameters
	{
		internal object[] Argugemts
		{
			get;
			set;
		}

		public System.Exception Exception
		{
			get;
			set;
		}

		public FlowBehavior MethodFlow
		{
			get;
			set;
		}

		public object ReturnValue
		{
			get;
			set;
		}

		public MethodParameters(object[] agugemts)
		{
            MethodFlow = FlowBehavior.Continue;
            Argugemts = agugemts;
		}

		private object DeepColne(object obj)
		{
			return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(obj));
		}

		public object GetParameter(int index)
		{
			if (index < 0)
			{
				throw new AOPProxyException("获取参数时，参数序号必须大于或等于0");
			}
			if (index >= Argugemts.Length)
			{
				throw new AOPProxyException("获取参数时，参数序号超出参数列表参数个数");
			}
			return Argugemts[index];
		}

		public void SetParameter(int index, object value)
		{
			if (index < 0)
			{
				throw new AOPProxyException("设置参数时，参数序号必须大于或等于0");
			}
			if (index >= Argugemts.Length)
			{
				throw new AOPProxyException("设置参数时，参数序号超出参数列表参数个数");
			}
            Argugemts[index] = value;
		}
	}
}