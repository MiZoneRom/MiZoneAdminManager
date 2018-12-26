using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace MZcms.Core
{
	public static class EnumHelper
	{
		private static Hashtable enumDesciption;

		static EnumHelper()
		{
			EnumHelper.enumDesciption = EnumHelper.GetDescriptionContainer();
		}

		private static void AddToEnumDescription(Type enumType)
		{
			EnumHelper.enumDesciption.Add(enumType, EnumHelper.GetEnumDic(enumType));
		}

		private static string GetDescription(Type enumType, string enumText)
		{
			string item;
			if (!string.IsNullOrEmpty(enumText))
			{
				if (!EnumHelper.enumDesciption.ContainsKey(enumType))
				{
					EnumHelper.AddToEnumDescription(enumType);
				}
				object obj = EnumHelper.enumDesciption[enumType];
				if ((obj == null ? true : string.IsNullOrEmpty(enumText)))
				{
					throw new ApplicationException("不存在枚举的描述");
				}
				item = ((Dictionary<string, string>)obj)[enumText];
			}
			else
			{
				item = null;
			}
			return item;
		}

		private static Hashtable GetDescriptionContainer()
		{
			EnumHelper.enumDesciption = new Hashtable();
			return EnumHelper.enumDesciption;
		}

		private static Dictionary<string, string> GetEnumDic(Type enumType)
		{
			Dictionary<string, string> strs = new Dictionary<string, string>();
			FieldInfo[] fields = enumType.GetFields();
			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo fieldInfo = fields[i];
				if (fieldInfo.FieldType.IsEnum)
				{
					object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
					strs.Add(fieldInfo.Name, ((DescriptionAttribute)customAttributes[0]).Description);
				}
			}
			return strs;
		}

		private static bool IsIntType(double d)
		{
			return (int)d != d;
		}

		public static string ToDescription(this Enum value)
		{
			string description;
			if (value != null)
			{
				Type type = value.GetType();
				string name = Enum.GetName(type, value);
				description = EnumHelper.GetDescription(type, name);
			}
			else
			{
				description = "";
			}
			return description;
		}

		public static Dictionary<int, string> ToDescriptionDictionary<TEnum>()
		{
			Array values = Enum.GetValues(typeof(TEnum));
			Dictionary<int, string> nums = new Dictionary<int, string>();
			foreach (Enum value in values)
			{
				nums.Add(Convert.ToInt32(value), value.ToDescription());
			}
			return nums;
		}

		public static Dictionary<int, string> ToDictionary<TEnum>()
		{
			Array values = Enum.GetValues(typeof(TEnum));
			Dictionary<int, string> nums = new Dictionary<int, string>();
			foreach (Enum value in values)
			{
				nums.Add(Convert.ToInt32(value), value.ToString());
			}
			return nums;
		}

		public static SelectList ToSelectList<TEnum>(this TEnum enumObj, bool perfix = true, bool onlyFlag = false)
		{
			var values = 
				from TEnum e in Enum.GetValues(typeof(TEnum))
				select new { Id = Convert.ToInt32(e), Name = EnumHelper.GetDescription(typeof(TEnum), e.ToString()) };
			var list = values.ToList();
			var variable = new { Id = 0, Name = "请选择..." };
			if (perfix)
			{
				list.Insert(0, variable);
			}
			return new SelectList(list, "Id", "Name", enumObj);
		}
	}
}