using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;

namespace MZcms.Core.Helper
{
	public class ZipHelper
    {
        private static string NotAllowExt = "," + ConfigurationManager.AppSettings["NotAllowExt"] + ",";

        public ZipHelper()
		{
		}

		public static ZipHelper.ZipInfo CreateZipFile(string filesPath, string zipFilePath)
		{
			int num;
			ZipHelper.ZipInfo zipInfo;
			if (Directory.Exists(filesPath))
			{
				try
				{
					string[] files = Directory.GetFiles(filesPath);
					ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(zipFilePath));
					try
					{
						zipOutputStream.SetLevel(9);
						byte[] numArray = new byte[4096];
						string[] strArrays = files;
						for (int i = 0; i < strArrays.Length; i++)
						{
							string str = strArrays[i];
							ZipEntry zipEntry = new ZipEntry(Path.GetFileName(str))
							{
								DateTime = DateTime.Now
							};
							zipOutputStream.PutNextEntry(zipEntry);
							FileStream fileStream = File.OpenRead(str);
							try
							{
								do
								{
									num = fileStream.Read(numArray, 0, numArray.Length);
									zipOutputStream.Write(numArray, 0, num);
								}
								while (num > 0);
							}
							finally
							{
								if (fileStream != null)
								{
									((IDisposable)fileStream).Dispose();
								}
							}
						}
						zipOutputStream.Finish();
						zipOutputStream.Close();
					}
					finally
					{
						if (zipOutputStream != null)
						{
							((IDisposable)zipOutputStream).Dispose();
						}
					}
					ZipHelper.ZipInfo zipInfo1 = new ZipHelper.ZipInfo()
					{
						Success = true,
						InfoMessage = "压缩成功"
					};
					zipInfo = zipInfo1;
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ZipHelper.ZipInfo zipInfo2 = new ZipHelper.ZipInfo()
					{
						Success = false,
						InfoMessage = exception.Message
					};
					zipInfo = zipInfo2;
				}
			}
			else
			{
				ZipHelper.ZipInfo zipInfo3 = new ZipHelper.ZipInfo()
				{
					Success = false,
					InfoMessage = "没有找到文件"
				};
				zipInfo = zipInfo3;
			}
			return zipInfo;
		}

		public static ZipHelper.ZipInfo UnZipFile(string zipFilePath)
		{
			ZipHelper.ZipInfo zipInfo;
			if (File.Exists(zipFilePath))
			{
				try
				{
					string str = zipFilePath.Replace(Path.GetExtension(zipFilePath), string.Empty);
					DateTime now = DateTime.Now;
					string str1 = string.Concat(str, "_", now.ToString("yyyyMMddHHmmssfff"));
					string empty = string.Empty;
					string fileName = string.Empty;
					ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(zipFilePath));
					try
					{
						while (true)
						{
							ZipEntry nextEntry = zipInputStream.GetNextEntry();
							ZipEntry zipEntry = nextEntry;
							if (nextEntry == null)
							{
								break;
							}
							empty = Path.GetDirectoryName(zipEntry.Name);
							fileName = Path.GetFileName(zipEntry.Name);
                            var ext = "," + Path.GetExtension(fileName).ToLower() + ",";
                            if (NotAllowExt.Contains(ext))
                            {
                                Log.Error("检测到非法zip解压文件后缀:" + ext);
                                continue;
                            }
                            if (empty.Length <= 0)
							{
								empty = str1;
							}
							else
							{
								empty = Path.Combine(str1, empty);
								if (!Directory.Exists(empty))
								{
									Directory.CreateDirectory(empty);
								}
							}
							if (fileName != string.Empty)
							{
								fileName = Path.Combine(empty, fileName);
								FileStream fileStream = File.Create(fileName);
								try
								{
									int num = 2048;
									byte[] numArray = new byte[2048];
									while (true)
									{
										num = zipInputStream.Read(numArray, 0, numArray.Length);
										if (num <= 0)
										{
											break;
										}
										fileStream.Write(numArray, 0, num);
									}
								}
								finally
								{
									if (fileStream != null)
									{
										((IDisposable)fileStream).Dispose();
									}
								}
							}
						}
					}
					finally
					{
						if (zipInputStream != null)
						{
							((IDisposable)zipInputStream).Dispose();
						}
					}
					ZipHelper.ZipInfo zipInfo1 = new ZipHelper.ZipInfo()
					{
						Success = true,
						InfoMessage = "解压成功",
						UnZipPath = str1
					};
					zipInfo = zipInfo1;
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ZipHelper.ZipInfo zipInfo2 = new ZipHelper.ZipInfo()
					{
						Success = false,
						InfoMessage = string.Concat("解压文件:", exception.Message)
					};
					zipInfo = zipInfo2;
				}
			}
			else
			{
				ZipHelper.ZipInfo zipInfo3 = new ZipHelper.ZipInfo()
				{
					Success = false,
					InfoMessage = "没有找到解压文件"
				};
				zipInfo = zipInfo3;
			}
			return zipInfo;
		}

		public class ZipInfo
		{
			public string InfoMessage
			{
				get;
				set;
			}

			public bool Success
			{
				get;
				set;
			}

			public string UnZipPath
			{
				get;
				set;
			}

			public ZipInfo()
			{
			}
		}
	}
}