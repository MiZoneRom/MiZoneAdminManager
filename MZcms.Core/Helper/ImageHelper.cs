using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace MZcms.Core.Helper
{
    public class ImageHelper
    {
        private static bool _isloadjpegcodec;

        private static ImageCodecInfo _jpegcodec;

        static ImageHelper()
        {
            ImageHelper._isloadjpegcodec = false;
            ImageHelper._jpegcodec = null;
        }

        public ImageHelper()
        {
        }

        public static Font CreateFont(string fontFile, float fontSize, FontStyle fontStyle, GraphicsUnit graphicsUnit, byte gdiCharSet)
        {
            PrivateFontCollection privateFontCollection = new PrivateFontCollection();
            privateFontCollection.AddFontFile(fontFile);
            Font font = new Font(privateFontCollection.Families[0], fontSize, fontStyle, graphicsUnit, gdiCharSet);
            return font;
        }

        public static void CreateThumbnail(string sourceFilename, string destFilename, int width, int height)
        {
            Image image = Image.FromFile(sourceFilename);
            if ((image.Width > width ? true : image.Height > height))
            {
                int num = image.Width;
                int num1 = image.Height;
                float single = height / (float)num1;
                if (width / (float)num < single)
                {
                    single = width / (float)num;
                }
                width = (int)(num * single);
                height = (int)(num1 * single);
                Image bitmap = new Bitmap(width, height);
                Graphics graphic = Graphics.FromImage(bitmap);
                graphic.Clear(Color.White);
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.DrawImage(image, new Rectangle(0, 0, width, height), new Rectangle(0, 0, num, num1), GraphicsUnit.Pixel);
                EncoderParameters encoderParameter = new EncoderParameters();
                EncoderParameter encoderParameter1 = new EncoderParameter(Encoder.Quality, (long)100);
                encoderParameter.Param[0] = encoderParameter1;
                ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo imageCodecInfo = null;
                int num2 = 0;
                while (num2 < imageEncoders.Length)
                {
                    if (!imageEncoders[num2].FormatDescription.Equals("JPEG"))
                    {
                        num2++;
                    }
                    else
                    {
                        imageCodecInfo = imageEncoders[num2];
                        break;
                    }
                }
                bitmap.Save(destFilename, imageCodecInfo, encoderParameter);
                encoderParameter.Dispose();
                encoderParameter1.Dispose();
                image.Dispose();
                bitmap.Dispose();
                graphic.Dispose();
            }
            else
            {
                File.Copy(sourceFilename, destFilename, true);
                image.Dispose();
            }
        }

        public static MemoryStream GenerateCheckCode(out string checkCode)
        {
            int i;
            MemoryStream memoryStream;
            checkCode = string.Empty;
            ColorTranslator.FromHtml("#1AE61A");
            char[] chrArray = new char[] { '2', '3', '4', '5', '6', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            Random random = new Random();
            for (i = 0; i < 4; i++)
            {
                checkCode = string.Concat(checkCode, chrArray[random.Next(chrArray.Length)]);
            }
            Bitmap bitmap = new Bitmap(85, 30);
            Graphics graphic = Graphics.FromImage(bitmap);
            Random random1 = new Random(DateTime.Now.Millisecond);
            Brush solidBrush = new SolidBrush(Color.FromArgb(4683611));
            graphic.Clear(ColorTranslator.FromHtml("#EBFDDF"));
            StringFormat stringFormat = new StringFormat();
            try
            {
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                stringFormat.FormatFlags = StringFormatFlags.NoWrap;
                Matrix matrix = new Matrix();
                float width = -25f;
                float single = 0f;
                graphic.SmoothingMode = SmoothingMode.AntiAlias;
                for (i = 0; i < checkCode.Length; i++)
                {
                    int num = random1.Next(20, 24);
                    Font font = ImageHelper.CreateFont(IOHelper.GetMapPath("/fonts/checkCode.ttf"), num, FontStyle.Bold, GraphicsUnit.Point, 0);
                    char chr = checkCode[i];
                    SizeF sizeF = graphic.MeasureString(chr.ToString(), font);
                    matrix.RotateAt(random1.Next(-15, 10), new PointF(width + sizeF.Width / 2f, single + sizeF.Height / 2f));
                    graphic.Transform = matrix;
                    chr = checkCode[i];
                    graphic.DrawString(chr.ToString(), font, Brushes.Green, new RectangleF(width, single, bitmap.Width, bitmap.Height), stringFormat);
                    width = width + sizeF.Width * 3f / 5f;
                    single = single + 0f;
                    graphic.RotateTransform(0f);
                    matrix.Reset();
                    font.Dispose();
                }
            }
            finally
            {
                if (stringFormat != null)
                {
                    stringFormat.Dispose();
                }
            }
            Pen pen = new Pen(Color.Black, 0f);
            MemoryStream memoryStream1 = new MemoryStream();
            try
            {
                bitmap.Save(memoryStream1, ImageFormat.Png);
                memoryStream = memoryStream1;
            }
            finally
            {
                bitmap.Dispose();
                graphic.Dispose();
            }
            return memoryStream;
        }

        public static void GenerateImageWatermark(string originalPath, string watermarkPath, string targetPath, int position, int opacity, int quality)
        {
            float single;
            Image image = null;
            Image bitmap = null;
            ImageAttributes imageAttribute = null;
            Graphics graphic = null;
            try
            {
                try
                {
                    image = Image.FromFile(originalPath);
                    bitmap = new Bitmap(watermarkPath);
                    if ((bitmap.Height >= image.Height ? false : bitmap.Width < image.Width))
                    {
                        if ((quality < 0 ? true : quality > 100))
                        {
                            quality = 80;
                        }
                        single = ((opacity <= 0 ? true : opacity > 10) ? 0.5f : (float)opacity / 10f);
                        int width = 0;
                        int height = 0;
                        switch (position)
                        {
                            case 1:
                                {
                                    width = (int)(image.Width * 0.01f);
                                    height = (int)(image.Height * 0.01f);
                                    break;
                                }
                            case 2:
                                {
                                    width = (int)(image.Width * 0.5f - bitmap.Width / 2);
                                    height = (int)(image.Height * 0.01f);
                                    break;
                                }
                            case 3:
                                {
                                    width = (int)(image.Width * 0.99f - bitmap.Width);
                                    height = (int)(image.Height * 0.01f);
                                    break;
                                }
                            case 4:
                                {
                                    width = (int)(image.Width * 0.01f);
                                    height = (int)(image.Height * 0.5f - bitmap.Height / 2);
                                    break;
                                }
                            case 5:
                                {
                                    width = (int)(image.Width * 0.5f - bitmap.Width / 2);
                                    height = (int)(image.Height * 0.5f - bitmap.Height / 2);
                                    break;
                                }
                            case 6:
                                {
                                    width = (int)(image.Width * 0.99f - bitmap.Width);
                                    height = (int)(image.Height * 0.5f - bitmap.Height / 2);
                                    break;
                                }
                            case 7:
                                {
                                    width = (int)(image.Width * 0.01f);
                                    height = (int)(image.Height * 0.99f - bitmap.Height);
                                    break;
                                }
                            case 8:
                                {
                                    width = (int)(image.Width * 0.5f - bitmap.Width / 2);
                                    height = (int)(image.Height * 0.99f - bitmap.Height);
                                    break;
                                }
                            case 9:
                                {
                                    width = (int)(image.Width * 0.99f - bitmap.Width);
                                    height = (int)(image.Height * 0.99f - bitmap.Height);
                                    break;
                                }
                        }
                        ColorMap colorMap = new ColorMap()
                        {
                            OldColor = Color.FromArgb(255, 0, 255, 0),
                            NewColor = Color.FromArgb(0, 0, 0, 0)
                        };
                        ColorMap[] colorMapArray = new ColorMap[] { colorMap };
                        float[][] singleArray = new float[5][];
                        float[] singleArray1 = new float[] { 1f, default(float), default(float), default(float), default(float) };
                        singleArray[0] = singleArray1;
                        singleArray1 = new float[] { default(float), 1f, default(float), default(float), default(float) };
                        singleArray[1] = singleArray1;
                        singleArray1 = new float[] { default(float), default(float), 1f, default(float), default(float) };
                        singleArray[2] = singleArray1;
                        singleArray1 = new float[] { default(float), default(float), default(float), single, default(float) };
                        singleArray[3] = singleArray1;
                        singleArray1 = new float[] { default(float), default(float), default(float), default(float), 1f };
                        singleArray[4] = singleArray1;
                        ColorMatrix colorMatrix = new ColorMatrix(singleArray);
                        imageAttribute = new ImageAttributes();
                        imageAttribute.SetRemapTable(colorMapArray, ColorAdjustType.Bitmap);
                        imageAttribute.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                        graphic = Graphics.FromImage(image);
                        graphic.DrawImage(bitmap, new Rectangle(width, height, bitmap.Width, bitmap.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, imageAttribute);
                        EncoderParameters encoderParameter = new EncoderParameters();
                        EncoderParameter[] param = encoderParameter.Param;
                        Encoder encoder = Encoder.Quality;
                        long[] numArray = new long[] { quality };
                        param[0] = new EncoderParameter(encoder, numArray);
                        if (ImageHelper.GetJPEGCodec() == null)
                        {
                            image.Save(targetPath);
                        }
                        else
                        {
                            image.Save(targetPath, ImageHelper._jpegcodec, encoderParameter);
                        }
                    }
                    else
                    {
                        image.Save(targetPath);
                        return;
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            finally
            {
                if (graphic != null)
                {
                    graphic.Dispose();
                }
                if (imageAttribute != null)
                {
                    imageAttribute.Dispose();
                }
                if (bitmap != null)
                {
                    bitmap.Dispose();
                }
                if (image != null)
                {
                    image.Dispose();
                }
            }
        }

        public static void GenerateTextWatermark(string originalPath, string targetPath, string text, int textSize, string textFont, int position, int quality)
        {
            Image image = null;
            Graphics graphic = null;
            try
            {
                try
                {
                    image = Image.FromFile(originalPath);
                    graphic = Graphics.FromImage(image);
                    if ((quality < 0 ? true : quality > 100))
                    {
                        quality = 80;
                    }
                    Font font = new Font(textFont, textSize, FontStyle.Regular, GraphicsUnit.Pixel);
                    SizeF sizeF = graphic.MeasureString(text, font);
                    float width = 0f;
                    float height = 0f;
                    switch (position)
                    {
                        case 1:
                            {
                                width = image.Width * 0.01f;
                                height = image.Height * 0.01f;
                                break;
                            }
                        case 2:
                            {
                                width = image.Width * 0.5f - sizeF.Width / 2f;
                                height = image.Height * 0.01f;
                                break;
                            }
                        case 3:
                            {
                                width = image.Width * 0.99f - sizeF.Width;
                                height = image.Height * 0.01f;
                                break;
                            }
                        case 4:
                            {
                                width = image.Width * 0.01f;
                                height = image.Height * 0.5f - sizeF.Height / 2f;
                                break;
                            }
                        case 5:
                            {
                                width = image.Width * 0.5f - sizeF.Width / 2f;
                                height = image.Height * 0.5f - sizeF.Height / 2f;
                                break;
                            }
                        case 6:
                            {
                                width = image.Width * 0.99f - sizeF.Width;
                                height = image.Height * 0.5f - sizeF.Height / 2f;
                                break;
                            }
                        case 7:
                            {
                                width = image.Width * 0.01f;
                                height = image.Height * 0.99f - sizeF.Height;
                                break;
                            }
                        case 8:
                            {
                                width = image.Width * 0.5f - sizeF.Width / 2f;
                                height = image.Height * 0.99f - sizeF.Height;
                                break;
                            }
                        case 9:
                            {
                                width = image.Width * 0.99f - sizeF.Width;
                                height = image.Height * 0.99f - sizeF.Height;
                                break;
                            }
                    }
                    graphic.DrawString(text, font, new SolidBrush(Color.White), width + 1f, height + 1f);
                    graphic.DrawString(text, font, new SolidBrush(Color.Black), width, height);
                    EncoderParameters encoderParameter = new EncoderParameters();
                    EncoderParameter[] param = encoderParameter.Param;
                    Encoder encoder = Encoder.Quality;
                    long[] numArray = new long[] { quality };
                    param[0] = new EncoderParameter(encoder, numArray);
                    if (ImageHelper.GetJPEGCodec() == null)
                    {
                        image.Save(targetPath);
                    }
                    else
                    {
                        image.Save(targetPath, ImageHelper._jpegcodec, encoderParameter);
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            finally
            {
                if (graphic != null)
                {
                    graphic.Dispose();
                }
                if (image != null)
                {
                    image.Dispose();
                }
            }
        }

        public static ImageCodecInfo GetJPEGCodec()
        {
            ImageCodecInfo imageCodecInfo;
            if (!ImageHelper._isloadjpegcodec)
            {
                ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
                int num = 0;
                while (num < imageEncoders.Length)
                {
                    ImageCodecInfo imageCodecInfo1 = imageEncoders[num];
                    if (imageCodecInfo1.MimeType.IndexOf("jpeg") <= -1)
                    {
                        num++;
                    }
                    else
                    {
                        ImageHelper._jpegcodec = imageCodecInfo1;
                        break;
                    }
                }
                ImageHelper._isloadjpegcodec = true;
                imageCodecInfo = ImageHelper._jpegcodec;
            }
            else
            {
                imageCodecInfo = ImageHelper._jpegcodec;
            }
            return imageCodecInfo;
        }

        public static void TranserImageFormat(string originalImagePath, string newFormatImagePath, ImageFormat fortmat)
        {
            (new Bitmap(originalImagePath)).Save(newFormatImagePath, ImageFormat.Jpeg);
        }

        public static void SaveBase64ToImage(string urlData, string url)
        {
            urlData = urlData.Split(',')[1];//¹ýÂËÍ·²¿
            byte[] bytes = Convert.FromBase64String(urlData);

            using (MemoryStream ms2 = new MemoryStream(bytes))
            {
                System.Drawing.Bitmap bmp2 = new System.Drawing.Bitmap(ms2);
                bmp2.Save(url, System.Drawing.Imaging.ImageFormat.Png);
            }


        }

    }
}