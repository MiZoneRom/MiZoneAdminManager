using System;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Cache;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;

namespace MZcms.Core.Helper
{
	public class WebHelper
	{
		private static string[] _browserlist;

		private static string[] _searchenginelist;

		private static Regex _metaregex;

		static WebHelper()
		{
			string[] strArrays = new string[] { "ie", "chrome", "mozilla", "netscape", "firefox", "opera", "konqueror" };
			WebHelper._browserlist = strArrays;
			strArrays = new string[] { "baidu", "google", "360", "sogou", "bing", "msn", "sohu", "soso", "sina", "163", "yahoo", "jikeu" };
			WebHelper._searchenginelist = strArrays;
			WebHelper._metaregex = new Regex("<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase | RegexOptions.Multiline);
		}

		public WebHelper()
		{
		}

		public static void DeleteCookie(string name)
		{
			HttpCookie httpCookie = new HttpCookie(name)
			{
				Expires = DateTime.Now.AddYears(-1)
			};
			HttpContext.Current.Response.AppendCookie(httpCookie);
		}

		public static string GetBrowserName()
		{
			string str;
			string browser = HttpContext.Current.Request.Browser.Browser;
			str = (!string.IsNullOrEmpty(browser) ? browser.ToLower() : "未知");
			return str;
		}

		public static string GetBrowserType()
		{
			string str;
			string type = HttpContext.Current.Request.Browser.Type;
			str = (!string.IsNullOrEmpty(type) ? type.ToLower() : "未知");
			return str;
		}

		public static string GetBrowserVersion()
		{
			string version = HttpContext.Current.Request.Browser.Version;
			return (!string.IsNullOrEmpty(version) ? version : "未知");
		}

		public static string GetCookie(string name)
		{
			string str;
			HttpCookie item = HttpContext.Current.Request.Cookies[name];
			str = (item == null ? string.Empty : item.Value);
			return str;
		}

		public static string GetCookie(string name, string key)
		{
			string empty;
			HttpCookie item = HttpContext.Current.Request.Cookies[name];
			if ((item == null ? false : item.HasKeys))
			{
				string str = item[key];
				if (str != null)
				{
					empty = str;
					return empty;
				}
			}
			empty = string.Empty;
			return empty;
		}

		public static int GetFormInt(string key, int defaultValue)
		{
			int num = TypeHelper.StringToInt(HttpContext.Current.Request.Form[key], defaultValue);
			return num;
		}

		public static int GetFormInt(string key)
		{
			return WebHelper.GetFormInt(key, 0);
		}

		public static string GetFormString(string key, string defaultValue)
		{
			string item = HttpContext.Current.Request.Form[key];
			return (string.IsNullOrWhiteSpace(item) ? defaultValue : item);
		}

		public static string GetFormString(string key)
		{
			return WebHelper.GetFormString(key, "");
		}

		public static string GetHost()
		{
			return HttpContext.Current.Request.Url.Host;
		}

		public static string GetIP()
		{
			string empty = string.Empty;
			empty = (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] == null ? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString() : HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString());
			if ((string.IsNullOrEmpty(empty) ? true : !ValidateHelper.IsIP(empty)))
			{
				empty = "127.0.0.1";
			}
			return empty;
		}

		public static string GetOSName()
		{
			string platform = HttpContext.Current.Request.Browser.Platform;
			return (!string.IsNullOrEmpty(platform) ? platform : "未知");
		}

		public static string GetOSType()
		{
			string str = "未知";
			string userAgent = HttpContext.Current.Request.UserAgent;
			if (userAgent.Contains("NT 6.1"))
			{
				str = "Windows 7";
			}
			else if (userAgent.Contains("NT 5.1"))
			{
				str = "Windows XP";
			}
			else if (userAgent.Contains("NT 6.2"))
			{
				str = "Windows 8";
			}
			else if (userAgent.Contains("android"))
			{
				str = "Android";
			}
			else if (userAgent.Contains("iphone"))
			{
				str = "IPhone";
			}
			else if (userAgent.Contains("Mac"))
			{
				str = "Mac";
			}
			else if (userAgent.Contains("NT 6.0"))
			{
				str = "Windows Vista";
			}
			else if (userAgent.Contains("NT 5.2"))
			{
				str = "Windows 2003";
			}
			else if (userAgent.Contains("NT 5.0"))
			{
				str = "Windows 2000";
			}
			else if (userAgent.Contains("98"))
			{
				str = "Windows 98";
			}
			else if (userAgent.Contains("95"))
			{
				str = "Windows 95";
			}
			else if (userAgent.Contains("Me"))
			{
				str = "Windows Me";
			}
			else if (userAgent.Contains("NT 4"))
			{
				str = "Windows NT4";
			}
			else if (userAgent.Contains("Unix"))
			{
				str = "UNIX";
			}
			else if (userAgent.Contains("Linux"))
			{
				str = "Linux";
			}
			else if (userAgent.Contains("SunOS"))
			{
				str = "SunOS";
			}
			return str;
		}

		public static NameValueCollection GetParmList(string data)
		{
			string str;
			string empty;
			NameValueCollection nameValueCollection = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
			if (!string.IsNullOrEmpty(data))
			{
				int length = data.Length;
				for (int i = 0; i < length; i++)
				{
					int num = i;
					int num1 = -1;
					while (i < length)
					{
						char chr = data[i];
						if (chr == '=')
						{
							if (num1 < 0)
							{
								num1 = i;
							}
						}
						else if (chr == '&')
						{
							break;
						}
						i++;
					}
					if (num1 < 0)
					{
						str = data.Substring(num, i - num);
						empty = string.Empty;
					}
					else
					{
						str = data.Substring(num, num1 - num);
						empty = data.Substring(num1 + 1, i - num1 - 1);
					}
					nameValueCollection[str] = empty;
					if ((i != length - 1 ? false : data[i] == '&'))
					{
						nameValueCollection[str] = string.Empty;
					}
				}
			}
			return nameValueCollection;
		}

		public static int GetQueryInt(string key, int defaultValue)
		{
			int num = TypeHelper.StringToInt(HttpContext.Current.Request.QueryString[key], defaultValue);
			return num;
		}

		public static int GetQueryInt(string key)
		{
			return WebHelper.GetQueryInt(key, 0);
		}

		public static string GetQueryString(string key, string defaultValue)
		{
			string item = HttpContext.Current.Request.QueryString[key];
			return (string.IsNullOrWhiteSpace(item) ? defaultValue : item);
		}

		public static string GetQueryString(string key)
		{
			return WebHelper.GetQueryString(key, "");
		}

		public static string GetRawUrl()
		{
			return HttpContext.Current.Request.RawUrl;
		}

		public static string GetRequestData(string url, string postData)
		{
			return WebHelper.GetRequestData(url, "post", postData);
		}

		public static string GetRequestData(string url, string method, string postData)
		{
			return WebHelper.GetRequestData(url, method, postData, Encoding.UTF8);
		}

		public static string GetRequestData(string url, string method, string postData, Encoding encoding)
		{
			return WebHelper.GetRequestData(url, method, postData, encoding, 20000);
		}

		public static string GetRequestData(string url, string method, string postData, Encoding encoding, int timeout)
		{
			string end;
			StreamReader streamReader;
			try
			{
				HttpWebResponse uRLResponse = WebHelper.GetURLResponse(url, method, postData, encoding, timeout);
				try
				{
					if (uRLResponse == null)
					{
						end = "error";
					}
					else if (encoding != null)
					{
						StreamReader streamReader1 = null;
						if ((uRLResponse.ContentEncoding == null ? true : !uRLResponse.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase)))
						{
							StreamReader streamReader2 = new StreamReader(uRLResponse.GetResponseStream(), encoding);
							streamReader1 = streamReader2;
							streamReader = streamReader2;
							try
							{
								try
								{
									end = streamReader1.ReadToEnd();
								}
								catch
								{
									end = "close";
								}
							}
							finally
							{
								if (streamReader != null)
								{
									((IDisposable)streamReader).Dispose();
								}
							}
						}
						else
						{
							StreamReader streamReader3 = new StreamReader(new GZipStream(uRLResponse.GetResponseStream(), CompressionMode.Decompress), encoding);
							streamReader1 = streamReader3;
							streamReader = streamReader3;
							try
							{
								end = streamReader1.ReadToEnd();
							}
							finally
							{
								if (streamReader != null)
								{
									((IDisposable)streamReader).Dispose();
								}
							}
						}
					}
					else
					{
						MemoryStream memoryStream = new MemoryStream();
						if ((uRLResponse.ContentEncoding == null ? true : !uRLResponse.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase)))
						{
							uRLResponse.GetResponseStream().CopyTo(memoryStream, 10240);
						}
						else
						{
							(new GZipStream(uRLResponse.GetResponseStream(), CompressionMode.Decompress)).CopyTo(memoryStream, 10240);
						}
						byte[] array = memoryStream.ToArray();
						string str = Encoding.Default.GetString(array, 0, array.Length);
						System.Text.RegularExpressions.Match match = WebHelper._metaregex.Match(str);
						string str1 = (match.Groups.Count > 2 ? match.Groups[2].Value : string.Empty);
						str1 = str1.Replace("\"", string.Empty).Replace("'", string.Empty).Replace(";", string.Empty);
						if (str1.Length > 0)
						{
							str1 = str1.ToLower().Replace("iso-8859-1", "gbk");
							encoding = Encoding.GetEncoding(str1);
						}
						else if (uRLResponse.CharacterSet.ToLower().Trim() == "iso-8859-1")
						{
							encoding = Encoding.GetEncoding("gbk");
						}
						else if (!string.IsNullOrEmpty(uRLResponse.CharacterSet.Trim()))
						{
							encoding = Encoding.GetEncoding(uRLResponse.CharacterSet);
						}
						else
						{
							encoding = Encoding.UTF8;
						}
						end = encoding.GetString(array);
					}
				}
				finally
				{
					if (uRLResponse != null)
					{
						((IDisposable)uRLResponse).Dispose();
					}
				}
			}
			catch
			{
				end = "error";
			}
			return end;
		}

		public static AspNetHostingPermissionLevel GetTrustLevel()
		{
			AspNetHostingPermissionLevel aspNetHostingPermissionLevel = AspNetHostingPermissionLevel.None;
			AspNetHostingPermissionLevel[] aspNetHostingPermissionLevelArray = new AspNetHostingPermissionLevel[] { AspNetHostingPermissionLevel.Unrestricted, AspNetHostingPermissionLevel.High, AspNetHostingPermissionLevel.Medium, AspNetHostingPermissionLevel.Low, AspNetHostingPermissionLevel.Minimal };
			AspNetHostingPermissionLevel[] aspNetHostingPermissionLevelArray1 = aspNetHostingPermissionLevelArray;
			for (int i = 0; i < aspNetHostingPermissionLevelArray1.Length; i++)
			{
				AspNetHostingPermissionLevel aspNetHostingPermissionLevel1 = aspNetHostingPermissionLevelArray1[i];
				try
				{
					(new AspNetHostingPermission(aspNetHostingPermissionLevel1)).Demand();
					aspNetHostingPermissionLevel = aspNetHostingPermissionLevel1;
					break;
				}
				catch (SecurityException)
				{
				}
			}
			return aspNetHostingPermissionLevel;
		}

		public static string GetUrl()
		{
			return HttpContext.Current.Request.Url.ToString();
		}

		public static string GetUrlReferrer()
		{
			string str;
			Uri urlReferrer = HttpContext.Current.Request.UrlReferrer;
			str = (!(urlReferrer == null) ? urlReferrer.ToString() : string.Empty);
			return str;
		}

		public static HttpWebResponse GetURLResponse(string url, string method = "get", string postData = "", Encoding encoding = null, int timeout = 20000)
		{
			HttpWebResponse response;
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}
			if ((url.Contains("http://") ? false : !url.Contains("https://")))
			{
				url = string.Concat("http://", url);
			}
			HttpWebRequest lower = (HttpWebRequest)WebRequest.Create(url);
			lower.Method = method.Trim().ToLower();
			lower.Timeout = timeout;
			lower.AllowAutoRedirect = true;
			lower.ContentType = "text/html";
			lower.Accept = "text/html, application/xhtml+xml, */*,zh-CN";
			lower.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
			lower.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
			try
			{
				if ((string.IsNullOrEmpty(postData) ? false : lower.Method == "post"))
				{
					byte[] bytes = encoding.GetBytes(postData);
					lower.ContentLength = (int)bytes.Length;
					lower.GetRequestStream().Write(bytes, 0, bytes.Length);
				}
				response = (HttpWebResponse)lower.GetResponse();
			}
			catch
			{
				response = null;
			}
			return response;
		}

		public static string HtmlDecode(string s)
		{
			return HttpUtility.HtmlDecode(s);
		}

		public static string HtmlEncode(string s)
		{
			return HttpUtility.HtmlEncode(s);
		}

		public static bool IsAjax()
		{
			return HttpContext.Current.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
		}

		public static bool IsBrowser()
		{
			bool flag;
			string browserName = WebHelper.GetBrowserName();
			string[] strArrays = WebHelper._browserlist;
			int num = 0;
			while (true)
			{
				if (num >= strArrays.Length)
				{
					flag = false;
					break;
				}
				else if (!browserName.Contains(strArrays[num]))
				{
					num++;
				}
				else
				{
					flag = true;
					break;
				}
			}
			return flag;
		}

		public static bool IsCrawler()
		{
			bool flag;
			bool crawler = HttpContext.Current.Request.Browser.Crawler;
			if (!crawler)
			{
				string urlReferrer = WebHelper.GetUrlReferrer();
				if (urlReferrer.Length > 0)
				{
					string[] strArrays = WebHelper._searchenginelist;
					int num = 0;
					while (num < strArrays.Length)
					{
						if (!urlReferrer.Contains(strArrays[num]))
						{
							num++;
						}
						else
						{
							flag = true;
							return flag;
						}
					}
				}
			}
			flag = crawler;
			return flag;
		}

		public static bool IsGet()
		{
			return HttpContext.Current.Request.HttpMethod == "GET";
		}

		public static bool IsMobile()
		{
			bool flag;
			if (!HttpContext.Current.Request.Browser.IsMobileDevice)
			{
				bool flag1 = false;
				flag = ((!bool.TryParse(HttpContext.Current.Request.Browser["IsTablet"], out flag1) ? true : !flag1) ? false : true);
			}
			else
			{
				flag = true;
			}
			return flag;
		}

		public static bool IsPost()
		{
			return HttpContext.Current.Request.HttpMethod == "POST";
		}

		public static void SetCookie(string name, string value)
		{
			HttpCookie item = HttpContext.Current.Request.Cookies[name];
			if (item == null)
			{
				item = new HttpCookie(name, value);
			}
			else
			{
				item.Value = value;
			}
			HttpContext.Current.Response.AppendCookie(item);
		}

		public static void SetCookie(string name, string value, DateTime dt)
		{
			HttpCookie item = HttpContext.Current.Request.Cookies[name];
			if (item == null)
			{
				item = new HttpCookie(name);
			}
			item.Value = value;
			item.Expires = dt;
			HttpContext.Current.Response.AppendCookie(item);
		}

		public static void SetCookie(string name, string key, string value)
		{
			HttpCookie item = HttpContext.Current.Request.Cookies[name];
			if (item == null)
			{
				item = new HttpCookie(name);
			}
			item[key] = value;
			HttpContext.Current.Response.AppendCookie(item);
		}

		public static void SetCookie(string name, string key, string value, DateTime dt)
		{
			HttpCookie item = HttpContext.Current.Request.Cookies[name];
			if (item == null)
			{
				item = new HttpCookie(name);
			}
			item[key] = value;
			item.Expires = dt;
			HttpContext.Current.Response.AppendCookie(item);
		}

		public static string UrlDecode(string s)
		{
			return HttpUtility.UrlDecode(s);
		}

		public static string UrlEncode(string s)
		{
			return HttpUtility.UrlEncode(s);
		}
	}
}