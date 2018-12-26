using MZcms.Core;
using MZcms.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Web;

namespace MZcms.Core.Plugins.Payment
{
	public interface IPaymentPlugin : IPlugin
	{
		string HelpImage
		{
			get;
		}

		string Logo
		{
			get;
			set;
		}

		string PluginListUrl
		{
			set;
		}

		UrlType RequestUrlType
		{
			get;
		}

		IEnumerable<PlatformType> SupportPlatforms
		{
			get;
		}

		string ConfirmPayResult();

		void Disable(PlatformType platform);

		void Enable(PlatformType platform);

		PaymentInfo EnterprisePay(EnterprisePayPara para);

		FormData GetFormData();

		string GetRequestUrl(string returnUrl, string notifyUrl, string orderId, decimal totalFee, string productInfo, string openId = null);

		bool IsEnable(PlatformType platform);

		PaymentInfo ProcessNotify(HttpRequestBase context);

		PaymentInfo ProcessRefundFee(PaymentPara para);

		PaymentInfo ProcessReturn(HttpRequestBase context);

		void SetFormValues(IEnumerable<KeyValuePair<string, string>> values);
    }
}