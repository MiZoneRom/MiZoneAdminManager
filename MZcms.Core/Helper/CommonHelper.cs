using System;
using System.Text;
using System.Text.RegularExpressions;

namespace MZcms.Core.Helper
{
	public class CommonHelper
	{
		private static string[] _weekdays;

		private static Regex _tbbrRegex;

		static CommonHelper()
		{
			string[] strArrays = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
			CommonHelper._weekdays = strArrays;
			CommonHelper._tbbrRegex = new Regex("\\s*|\\t|\\r|\\n", RegexOptions.IgnoreCase);
		}

		public CommonHelper()
		{
		}

		public static string ClearTBBR(string str)
		{
			string str1;
			str1 = (string.IsNullOrEmpty(str) ? string.Empty : CommonHelper._tbbrRegex.Replace(str, ""));
			return str1;
		}

		public static long ConvertIPToInt64(string ip)
		{
			long num = Convert.ToInt64(ip.Replace(".", string.Empty));
			return num;
		}

		public static string DeleteNullOrSpaceRow(string s)
		{
			string str;
			if (!string.IsNullOrEmpty(s))
			{
				string[] strArrays = StringHelper.SplitString("\r\n");
				StringBuilder stringBuilder = new StringBuilder();
				string[] strArrays1 = strArrays;
				for (int i = 0; i < strArrays1.Length; i++)
				{
					string str1 = strArrays1[i];
					if (!string.IsNullOrWhiteSpace(str1))
					{
						stringBuilder.AppendFormat("{0}\r\n", str1);
					}
				}
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Remove(stringBuilder.Length - 2, 2);
				}
				str = stringBuilder.ToString();
			}
			else
			{
				str = "";
			}
			return str;
		}

		public static string EscapeRegex(string s)
		{
			string[] strArrays = new string[] { "\\", ".", "+", "*", "?", "{", "}", "[", "^", "]", "$", "(", ")", "=", "!", "<", ">", "|", ":" };
			string[] strArrays1 = strArrays;
			strArrays = new string[] { "\\\\", "\\.", "\\+", "\\*", "\\?", "\\{", "\\}", "\\[", "\\^", "\\]", "\\$", "\\(", "\\)", "\\=", "\\!", "\\<", "\\>", "\\|", "\\:" };
			string[] strArrays2 = strArrays;
			for (int i = 0; i < strArrays1.Length; i++)
			{
				s = s.Replace(strArrays1[i], strArrays2[i]);
			}
			return s;
		}

		public static string GetChineseDate()
		{
			return DateTime.Now.ToString("yyyy月MM日dd");
		}

		public static string GetDate()
		{
			return DateTime.Now.ToString("yyyy-MM-dd");
		}

		public static string GetDateTime()
		{
			return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		}

		public static string GetDateTimeMS()
		{
			return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
		}

		public static string GetDateTimeU()
		{
			return string.Format("{0:U}", DateTime.Now);
		}

		public static string GetDay()
		{
			return DateTime.Now.Day.ToString("00");
		}

		public static string GetDayOfWeek()
		{
			return DateTime.Now.DayOfWeek.ToString();
		}

		public static string GetEmailProvider(string email)
		{
			string str;
			int num = email.LastIndexOf('@');
			str = (num <= 0 ? string.Empty : email.Substring(num + 1));
			return str;
		}

		public static string GetHour()
		{
			return DateTime.Now.Hour.ToString("00");
		}

		public static string GetHtmlBS(int count)
		{
			string str;
			if (count == 1)
			{
				str = "&nbsp;&nbsp;&nbsp;&nbsp;";
			}
			else if (count == 2)
			{
				str = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
			}
			else if (count != 3)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < count; i++)
				{
					stringBuilder.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
				}
				str = stringBuilder.ToString();
			}
			else
			{
				str = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
			}
			return str;
		}

		public static string GetHtmlSpan(int count)
		{
			string str;
			if (count <= 0)
			{
				str = "";
			}
			else if (count == 1)
			{
				str = "<span></span>";
			}
			else if (count == 2)
			{
				str = "<span></span><span></span>";
			}
			else if (count != 3)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < count; i++)
				{
					stringBuilder.Append("<span></span>");
				}
				str = stringBuilder.ToString();
			}
			else
			{
				str = "<span></span><span></span><span></span>";
			}
			return str;
		}

		public static int GetIndexInArray(string s, string[] array, bool ignoreCase)
		{
			int num;
			if ((string.IsNullOrEmpty(s) || array == null ? false : array.Length != 0))
			{
				int num1 = 0;
				string str = null;
				if (ignoreCase)
				{
					s = s.ToLower();
				}
				string[] strArrays = array;
				int num2 = 0;
				while (num2 < strArrays.Length)
				{
					string str1 = strArrays[num2];
					str = (!ignoreCase ? str1 : str1.ToLower());
					if (!(s == str))
					{
						num1++;
						num2++;
					}
					else
					{
						num = num1;
						return num;
					}
				}
				num = -1;
			}
			else
			{
				num = -1;
			}
			return num;
		}

		public static int GetIndexInArray(string s, string[] array)
		{
			return CommonHelper.GetIndexInArray(s, array, false);
		}

		public static string GetMonth()
		{
			return DateTime.Now.Month.ToString("00");
		}

		public static string GetTime()
		{
			return DateTime.Now.ToString("HH:mm:ss");
		}

		public static string GetUniqueString(string s)
		{
			return CommonHelper.GetUniqueString(s, ",");
		}

		public static string GetUniqueString(string s, string splitStr)
		{
			string str = CommonHelper.ObjectArrayToString(CommonHelper.RemoveRepeaterItem(StringHelper.SplitString(s, splitStr)), splitStr);
			return str;
		}

		public static string GetWeek()
		{
			return CommonHelper._weekdays[(int)DateTime.Now.DayOfWeek];
		}

		public static string GetYear()
		{
			return DateTime.Now.Year.ToString();
		}

		public static string HideEmail(string email)
		{
			string str;
			int num = email.LastIndexOf('@');
			if (num == 1)
			{
				str = string.Concat("*", email.Substring(num));
			}
			else if (num != 2)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(email.Substring(0, 2));
				for (int i = num - 2; i > 0; i--)
				{
					stringBuilder.Append("*");
				}
				stringBuilder.Append(email.Substring(num));
				str = stringBuilder.ToString();
			}
			else
			{
				str = string.Concat(email[0], "*", email.Substring(num));
			}
			return str;
		}

		public static string HideMobile(string mobile)
		{
			string str = string.Concat(mobile.Substring(0, 3), "*****", mobile.Substring(8));
			return str;
		}

		public static string IntArrayToString(int[] array, string splitStr)
		{
			string str;
			if ((array == null ? false : array.Length != 0))
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < array.Length; i++)
				{
					stringBuilder.AppendFormat("{0}{1}", array[i], splitStr);
				}
				str = stringBuilder.Remove(stringBuilder.Length - splitStr.Length, splitStr.Length).ToString();
			}
			else
			{
				str = "";
			}
			return str;
		}

		public static string IntArrayToString(int[] array)
		{
			return CommonHelper.IntArrayToString(array, ",");
		}

		public static bool IsInArray(string s, string[] array, bool ignoreCase)
		{
			return CommonHelper.GetIndexInArray(s, array, ignoreCase) > -1;
		}

		public static bool IsInArray(string s, string[] array)
		{
			return CommonHelper.IsInArray(s, array, false);
		}

		public static bool IsInArray(string s, string array, string splitStr, bool ignoreCase)
		{
			bool flag = CommonHelper.IsInArray(s, StringHelper.SplitString(array, splitStr), ignoreCase);
			return flag;
		}

		public static bool IsInArray(string s, string array, string splitStr)
		{
			bool flag = CommonHelper.IsInArray(s, StringHelper.SplitString(array, splitStr), false);
			return flag;
		}

		public static bool IsInArray(string s, string array)
		{
			bool flag = CommonHelper.IsInArray(s, StringHelper.SplitString(array, ","), false);
			return flag;
		}

		public static string ObjectArrayToString(object[] array, string splitStr)
		{
			string str;
			if ((array == null ? false : array.Length != 0))
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < array.Length; i++)
				{
					stringBuilder.AppendFormat("{0}{1}", array[i], splitStr);
				}
				str = stringBuilder.Remove(stringBuilder.Length - splitStr.Length, splitStr.Length).ToString();
			}
			else
			{
				str = "";
			}
			return str;
		}

		public static string ObjectArrayToString(object[] array)
		{
			return CommonHelper.ObjectArrayToString(array, ",");
		}

		public static string[] RemoveArrayItem(string[] array, string removeItem, bool removeBackspace, bool ignoreCase)
		{
			string[] strArrays;
			if ((array == null ? true : array.Length <= 0))
			{
				strArrays = array;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (ignoreCase)
				{
					removeItem = removeItem.ToLower();
				}
				string str = "";
				string[] strArrays1 = array;
				for (int i = 0; i < strArrays1.Length; i++)
				{
					string str1 = strArrays1[i];
					str = (!ignoreCase ? str1 : str1.ToLower());
					if (str != removeItem)
					{
						stringBuilder.AppendFormat("{0}_", (removeBackspace ? str1.Trim() : str1));
					}
				}
				strArrays = StringHelper.SplitString(stringBuilder.Remove(stringBuilder.Length - 1, 1).ToString(), "_");
			}
			return strArrays;
		}

		public static string[] RemoveArrayItem(string[] array)
		{
			return CommonHelper.RemoveArrayItem(array, "", true, false);
		}

		public static int[] RemoveRepeaterItem(int[] array)
		{
			int i;
			int[] numArray;
			if ((array == null ? false : array.Length >= 2))
			{
				Array.Sort<int>(array);
				int num = 1;
				for (i = 1; i < array.Length; i++)
				{
					if (array[i] != array[i - 1])
					{
						num++;
					}
				}
				int[] numArray1 = new int[num];
				numArray1[0] = array[0];
				int num1 = 1;
				for (i = 1; i < array.Length; i++)
				{
					if (array[i] != array[i - 1])
					{
						int num2 = num1;
						num1 = num2 + 1;
						numArray1[num2] = array[i];
					}
				}
				numArray = numArray1;
			}
			else
			{
				numArray = array;
			}
			return numArray;
		}

		public static string[] RemoveRepeaterItem(string[] array)
		{
			int i;
			string[] strArrays;
			if ((array == null ? false : array.Length >= 2))
			{
				Array.Sort<string>(array);
				int num = 1;
				for (i = 1; i < array.Length; i++)
				{
					if (array[i] != array[i - 1])
					{
						num++;
					}
				}
				string[] strArrays1 = new string[num];
				strArrays1[0] = array[0];
				int num1 = 1;
				for (i = 1; i < array.Length; i++)
				{
					if (array[i] != array[i - 1])
					{
						int num2 = num1;
						num1 = num2 + 1;
						strArrays1[num2] = array[i];
					}
				}
				strArrays = strArrays1;
			}
			else
			{
				strArrays = array;
			}
			return strArrays;
		}

		public static string[] RemoveStringItem(string s, string splitStr)
		{
			string[] strArrays = CommonHelper.RemoveArrayItem(StringHelper.SplitString(s, splitStr), "", true, false);
			return strArrays;
		}

		public static string[] RemoveStringItem(string s)
		{
			string[] strArrays = CommonHelper.RemoveArrayItem(StringHelper.SplitString(s), "", true, false);
			return strArrays;
		}

		public static string StringArrayToString(string[] array, string splitStr)
		{
			return CommonHelper.ObjectArrayToString(array, splitStr);
		}

		public static string StringArrayToString(string[] array)
		{
			return CommonHelper.StringArrayToString(array, ",");
		}

		public static decimal SubDecimal(decimal dec, int pointCount)
		{
			string str = dec.ToString();
			decimal num = TypeHelper.StringToDecimal(str.Substring(0, str.IndexOf('.') + pointCount + 1));
			return num;
		}

		public static string TBBRTrim(string str)
		{
			string empty;
			if (string.IsNullOrEmpty(str))
			{
				empty = string.Empty;
			}
			else
			{
				string str1 = str.Trim();
				char[] chrArray = new char[] { '\r' };
				string str2 = str1.Trim(chrArray);
				chrArray = new char[] { '\n' };
				string str3 = str2.Trim(chrArray);
				chrArray = new char[] { '\t' };
				empty = str3.Trim(chrArray);
			}
			return empty;
		}
	}
}