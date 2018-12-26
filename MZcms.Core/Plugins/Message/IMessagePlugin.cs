using MZcms.Core.Plugins;
using System;
using System.Collections.Generic;

namespace MZcms.Core.Plugins.Message
{
	public interface IMessagePlugin : IPlugin
	{
		bool EnableLog
		{
			get;
		}

		bool IsSettingsValid
		{
			get;
		}

		string Logo
		{
			get;
		}

		string ShortName
		{
			get;
		}

		bool CheckDestination(string destination);

		void Disable(MessageTypeEnum e);

		void Enable(MessageTypeEnum e);

		Dictionary<MessageTypeEnum, StatusEnum> GetAllStatus();

		FormData GetFormData();

		StatusEnum GetStatus(MessageTypeEnum e);

		string SendMessageCode(string destination, MessageUserInfo info);

		string SendMessageOnFindPassWord(string destination, MessageUserInfo info);

		string SendMessageOnOrderCreate(string destination, MessageOrderInfo info);

		string SendMessageOnOrderPay(string destination, MessageOrderInfo info);

		string SendMessageOnOrderRefund(string destination, MessageOrderInfo info);

		string SendMessageOnOrderShipping(string destination, MessageOrderInfo info);

		string SendMessageOnShopAudited(string destination, MessageShopInfo info);

		string SendMessageOnShopSuccess(string destination, MessageShopInfo info);

		void SendMessages(string[] destination, string content, string title = "");

		string SendTestMessage(string destination, string content, string title = "");

		void SetAllStatus(Dictionary<MessageTypeEnum, StatusEnum> dic);

		void SetFormValues(IEnumerable<KeyValuePair<string, string>> values);
	}
}