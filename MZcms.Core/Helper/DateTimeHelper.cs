using System;
using System.Text;

namespace MZcms.Core.Helper
{
	public class DateTimeHelper
	{
		public DateTimeHelper()
		{
		}

		public static DateTime GetStartDayOfWeeks(int year, int month, int index)
		{
			DateTime minValue;
			if (!(year < 1600 ? false : year <= 9999))
			{
				minValue = DateTime.MinValue;
			}
			else if (!(month < 0 ? false : month <= 12))
			{
				minValue = DateTime.MinValue;
			}
			else if (index >= 1)
			{
				DateTime dateTime = new DateTime(year, month, 1);
				int num = 7;
				if (Convert.ToInt32(dateTime.DayOfWeek.ToString("d")) > 0)
				{
					num = Convert.ToInt32(dateTime.DayOfWeek.ToString("d"));
				}
				DateTime dateTime1 = dateTime.AddDays(1 - num);
				DateTime dateTime2 = dateTime1.AddDays(index * 7);
				minValue = ((dateTime2 - dateTime.AddMonths(1)).Days <= 0 ? dateTime2 : DateTime.MinValue);
			}
			else
			{
				minValue = DateTime.MinValue;
			}
			return minValue;
		}

		public static string GetWeekSpanOfMonth(int year, int month)
		{
			string str;
			if (!(year < 1600 ? false : year <= 9999))
			{
				str = "";
			}
			else if ((month < 0 ? false : month <= 12))
			{
				StringBuilder stringBuilder = new StringBuilder();
				int num = 1;
				while (num < 5)
				{
					DateTime dateTime = new DateTime(year, month, 1);
					int num1 = 7;
					if (Convert.ToInt32(dateTime.DayOfWeek.ToString("d")) > 0)
					{
						num1 = Convert.ToInt32(dateTime.DayOfWeek.ToString("d"));
					}
					DateTime dateTime1 = dateTime.AddDays(1 - num1);
					DateTime dateTime2 = dateTime1.AddDays(num * 7);
					if ((dateTime2 - dateTime.AddMonths(1)).Days <= 0)
					{
						stringBuilder.Append(dateTime2.ToString("yyyy-MM-dd"));
						stringBuilder.Append(" ~ ");
						DateTime dateTime3 = dateTime2.AddDays(6);
						stringBuilder.Append(dateTime3.ToString("yyyy-MM-dd"));
						stringBuilder.Append(Environment.NewLine);
						num++;
					}
					else
					{
						str = "";
						return str;
					}
				}
				str = stringBuilder.ToString();
			}
			else
			{
				str = "";
			}
			return str;
		}
	}
}