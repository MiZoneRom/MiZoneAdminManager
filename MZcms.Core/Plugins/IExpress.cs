using MZcms.Core.Plugins.Express;
using System;
using System.Collections.Generic;

namespace MZcms.Core.Plugins
{
	public interface IExpress : IPlugin
	{
		string BackGroundImage
		{
			get;
			set;
		}

		string DisplayName
		{
			get;
		}

		IEnumerable<ExpressPrintElement> Elements
		{
			get;
		}

		int Height
		{
			get;
		}

		string Kuaidi100Code
		{
			get;
		}

		string Logo
		{
			get;
			set;
		}

		string Name
		{
			get;
		}

		string TaobaoCode
		{
			get;
		}

		int Width
		{
			get;
		}

		bool CheckExpressCodeIsValid(string expressCode);

		string NextExpressCode(string currentExpressCode);

		void UpdatePrintElement(IEnumerable<ExpressPrintElement> printElements);
	}
}