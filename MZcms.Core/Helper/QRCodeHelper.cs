using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.qrcode.decoder;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace MZcms.Core.Helper
{
	public class QRCodeHelper
	{
		public QRCodeHelper()
		{
		}

		public static Bitmap Create(string content)
		{
			MultiFormatWriter multiFormatWriter = new MultiFormatWriter();
			ByteMatrix byteMatrix = multiFormatWriter.encode(content, BarcodeFormat.QR_CODE, 300, 300);
			return byteMatrix.ToBitmap();
		}

		public static Bitmap Create(string content, string imagePath)
		{
			Image image = Image.FromFile(imagePath);
			Bitmap bitmap = QRCodeHelper.Create(content, image);
			image.Dispose();
			return bitmap;
		}

		public static Bitmap Create(string content, Image centralImage)
		{
			MultiFormatWriter multiFormatWriter = new MultiFormatWriter();
			Hashtable hashtables = new Hashtable()
			{
				{ EncodeHintType.CHARACTER_SET, "UTF-8" },
				{ EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H }
			};
			ByteMatrix byteMatrix = multiFormatWriter.encode(content, BarcodeFormat.QR_CODE, 300, 300, hashtables);
			Bitmap bitmap = byteMatrix.ToBitmap();
			Image image = centralImage;
			Size encodeSize = multiFormatWriter.GetEncodeSize(content, BarcodeFormat.QR_CODE, 300, 300);
			int num = Math.Min((int)(encodeSize.Width / 3.5), image.Width);
			int num1 = Math.Min((int)(encodeSize.Height / 3.5), image.Height);
			int width = (bitmap.Width - num) / 2;
			int height = (bitmap.Height - num1) / 2;
			Bitmap bitmap1 = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
			Graphics graphic = Graphics.FromImage(bitmap1);
			try
			{
				graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphic.SmoothingMode = SmoothingMode.HighQuality;
				graphic.CompositingQuality = CompositingQuality.HighQuality;
				graphic.DrawImage(bitmap, 0, 0);
			}
			finally
			{
				if (graphic != null)
				{
                    graphic.Dispose();
				}
			}
			Graphics graphic1 = Graphics.FromImage(bitmap1);
			graphic1.FillRectangle(Brushes.White, width, height, num, num1);
			graphic1.DrawImage(image, width, height, num, num1);
			return bitmap1;
		}
	}
}