using log4net;
using System;
using System.Diagnostics;
using System.Reflection;

namespace MZcms.Core
{
	public static class Log
	{
		public static void Debug(object message)
		{
			LogManager.GetLogger(MZcms.Core.Log.GetCurrentMethodFullName()).Debug(message);
            Console.WriteLine(@DateTime.Now.ToString() + " " + "[Info]\t" + message + "\t");
        }

		public static void Debug(object message, Exception ex)
		{
			LogManager.GetLogger(MZcms.Core.Log.GetCurrentMethodFullName()).Debug(message, ex);
            Console.WriteLine(@DateTime.Now.ToString() + " " + "[Info]\t" + message + "\t");
        }

		public static void Error(object message)
		{
            Console.ForegroundColor = ConsoleColor.Red;
            LogManager.GetLogger(MZcms.Core.Log.GetCurrentMethodFullName()).Error(message);
            Console.WriteLine(@DateTime.Now.ToString() + " " + "[Info]\t" + message + "\t");
            Console.ResetColor();
        }

		public static void Error(object message, Exception exception)
		{
			LogManager.GetLogger(MZcms.Core.Log.GetCurrentMethodFullName()).Error(message, exception);
            Console.WriteLine(@DateTime.Now.ToString() + " " + "[Info]\t" + message + "\t");
        }

		private static string GetCurrentMethodFullName()
		{
			StackFrame frame;
			string str;
			string str1;
			bool flag;
			try
			{
				int num = 2;
				StackTrace stackTrace = new StackTrace();
				int length = stackTrace.GetFrames().Length;
				do
				{
					int num1 = num;
					num = num1 + 1;
					frame = stackTrace.GetFrame(num1);
					str = frame.GetMethod().DeclaringType.ToString();
					flag = (!str.EndsWith("Exception") ? false : num < length);
				}
				while (flag);
				string name = frame.GetMethod().Name;
				str1 = string.Concat(str, ".", name);
			}
			catch
			{
				str1 = null;
			}
			return str1;
		}

		public static void Info(object message)
		{
			LogManager.GetLogger(MZcms.Core.Log.GetCurrentMethodFullName()).Info(message);
            Console.WriteLine(@DateTime.Now.ToString() + " " + "[Info]\t" + message + "\t");
        }

		public static void Info(object message, Exception ex)
		{
			LogManager.GetLogger(MZcms.Core.Log.GetCurrentMethodFullName()).Info(message, ex);
            Console.WriteLine(@DateTime.Now.ToString() + " " + "[Info]\t" + message + "\t");
        }

		public static void Warn(object message)
		{
			LogManager.GetLogger(MZcms.Core.Log.GetCurrentMethodFullName()).Warn(message);
            Console.WriteLine(@DateTime.Now.ToString() + " " + "[Info]\t" + message + "\t");
        }

		public static void Warn(object message, Exception ex)
		{
			LogManager.GetLogger(MZcms.Core.Log.GetCurrentMethodFullName()).Warn(message, ex);
            Console.WriteLine(@DateTime.Now.ToString() + " " + "[Info]\t" + message + "\t");
        }
	}
}