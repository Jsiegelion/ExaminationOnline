using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Mammothcode.Core.File
{
    /// <summary>
    /// 图片公用类
    /// 创建人：孙佳杰  创建时间:2015.3.23
    /// </summary>
    public class ImageUtil
    {

        /// <summary>
        /// 功能描述：图像压缩可以处理通用型格式
        /// 创建人：甘春雨
        /// 创建时间：2015年11月27日13:56:16
        /// </summary>
        /// <param name="sFile"></param>
        /// <param name="outPath"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static bool GetPicThumbnail(string sFile, string outPath, int flag)
        {
            bool isSuccess = false;
            byte[] streamBytes = SetImageToByteArray(sFile);
            try
            {
                isSuccess = StreamThumbnail(streamBytes, outPath, flag);
            }
            catch
            {
            }
            return isSuccess;
        }

        #region 流图像压缩
        /// <summary>
        /// 功能描述：字节流图像压缩
        /// 创建人：甘春雨
        /// 创建时间：2015年11月27日13:56:16
        /// </summary>
        /// <param name="inStream"></param>
        /// <param name="outPath"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static bool StreamThumbnail(byte[] inStream, string outPath, int flag)
        {
            System.Drawing.Image iSource = SetByteToImage(inStream);
            ImageFormat tFormat = iSource.RawFormat;
            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    iSource.Save(outPath, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else
                {
                    iSource.Save(outPath, tFormat);
                }
                //System.IO.File.Delete(sFile);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                iSource.Dispose();
            }
        }
        /// <summary>
        /// 功能描述：输入流图像压缩
        /// 创建人：甘春雨
        /// 创建时间：2015年11月27日13:56:16
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="outPath"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static bool StreamThumbnail(System.IO.Stream inputStream, string outPath, int flag)
        {
            byte[] streamBytes = StreamToBytes(inputStream);
            bool isSuccess = false;
            try
            {
                isSuccess = StreamThumbnail(streamBytes, outPath, flag);
            }
            catch
            {
            }
            return isSuccess;
        }
        #endregion

        #region 图像压缩
        /// <summary>
        /// 功能描述：图像压缩-只能处理ipg格式
        /// 创建人：甘春雨
        /// 创建时间：2015年11月26日09:04:49
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <param name="quality">压缩率</param>
        /// <returns></returns>
        public static byte[] CompressionImage(Stream fileStream, long quality)
        {
            using (System.Drawing.Image img = System.Drawing.Image.FromStream(fileStream))
            {
                using (Bitmap bitmap = new Bitmap(img))
                {
                    ImageCodecInfo CodecInfo = GetEncoder(img.RawFormat);
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, quality);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitmap.Save(ms, CodecInfo, myEncoderParameters);
                        myEncoderParameters.Dispose();
                        myEncoderParameter.Dispose();
                        return ms.ToArray();
                    }
                }
            }
        }
        /// <summary>
        /// 功能描述：图像压缩-只能处理ipg格式
        /// 创建人：甘春雨
        /// 创建时间：2015年11月26日09:04:49
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="quality">压缩率</param>
        /// <returns></returns>
        public static byte[] CompressionImage(string path, long quality)
        {
            byte[] fsb = SetImageToByteArray(path);
            Stream fs = ByteToStream(fsb);
            return CompressionImage(fs, quality);
        }
        /// <summary>
        /// 功能描述：将图片压缩并保存为到另一个地址-只能处理ipg格式
        /// 创建人：甘春雨
        /// 创建时间：2015年11月26日09:24:26
        /// </summary>
        /// <param name="oldPath">原来图片的地址</param>
        /// <param name="quality">图片质量</param>
        /// <param name="newPath">新图片的地址</param>
        /// <returns></returns>
        public static bool CompressionImageSave(string oldPath, long quality, string newPath)
        {
            bool isSuccess = false;

            try
            {
                byte[] fsbBytes = CompressionImage(oldPath, quality);
                Image img = SetByteToImage(fsbBytes);
                img.Save(newPath);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSuccess;
        }
        /// <summary>
        /// 功能描述：不详-图片压缩使用的
        /// 创建人：甘春雨
        /// 创建时间：2015年11月26日09:09:38
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        #endregion

        #region 图片水印


        /// <summary>
        /// 文字水印
        /// </summary>
        /// <param name="imgPath">原图绝对路径</param>
        /// <param name="savePath">生成图绝对路径</param>
        /// <param name="markText">水印文字</param>
        /// <param name="wmPosition">水印位置</param>
        /// <param name="transparency">透明度(0-1)</param>
        /// <param name="fontSize">文字大小,0 则随图片自动调整大小</param>
        /// <returns></returns>
        public static bool AddTextWaterMark(string imgPath, string savePath, string markText, WaterMarkPosition wmPosition, float transparency, float fontSize)
        {
            Bitmap image = null;
            Graphics g = null;
            try
            {

                //创建一个图片对象用来装载要被添加水印的图片
                Image image1 = Image.FromStream(ByteToStream(SetImageToByteArray(imgPath)));
                image = new Bitmap(image1);
                image1.Dispose();
                g = Graphics.FromImage(image);
                //设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;


                //************* 文字水印 *****************
                int imgWidth = image.Width;
                int imgHeight = image.Height;


                int[] sizes = new int[] { 48, 36, 28, 24, 16, 14, 12, 10 };

                Font font = null;
                SizeF crSize = new SizeF();

                if (fontSize == 0)
                {

                    //利用一个循环语句来选择我们要添加文字的型号
                    //直到它的长度比图片的宽度小
                    for (int i = 0; i < sizes.Length; i++)
                    {
                        font = new Font("宋体", sizes[i], FontStyle.Bold);

                        //测量用指定的 Font 对象绘制并用指定的 StringFormat 对象格式化的指定字符串。
                        crSize = g.MeasureString(markText, font);

                        // ushort 关键字表示一种整数数据类型
                        if ((ushort)crSize.Width < (ushort)imgWidth * 0.5)
                            break;
                    }

                }
                else
                {
                    font = new Font("宋体", fontSize, FontStyle.Bold); //定义字体 ;
                    crSize = g.MeasureString(markText, font);
                }


                int xpos = 0;
                int ypos = 0;

                switch (wmPosition)
                {
                    case WaterMarkPosition.Left_Top:
                        xpos = (int)(image.Width * (float).01);
                        ypos = (int)(image.Height * (float).01);
                        break;

                    case WaterMarkPosition.Right_Top:
                        xpos = (int)((image.Width * (float).99) - (crSize.Width));
                        ypos = (int)(image.Height * (float).01);
                        break;

                    case WaterMarkPosition.Middle:
                        xpos = (int)((image.Width * (float).50) - (crSize.Width / 2));
                        ypos = (int)((image.Height * (float).50) - (crSize.Height / 2));
                        break;

                    case WaterMarkPosition.Left_Bottom:
                        xpos = (int)(image.Width * (float).01);
                        ypos = (int)((image.Height * (float).99) - crSize.Height);
                        break;

                    case WaterMarkPosition.Right_Bottom:
                        xpos = (int)((image.Width * (float).99) - (crSize.Width));
                        ypos = (int)((image.Height * (float).99) - crSize.Height);
                        break;
                }

                g.DrawString(markText, font, new SolidBrush(Color.FromArgb((int)(transparency * 255), 0, 0, 0)), xpos + 1, ypos + 1);
                g.DrawString(markText, font, new SolidBrush(Color.FromArgb((int)(transparency * 255), 255, 255, 255)), xpos, ypos);


                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.MimeType.IndexOf("jpeg") > -1)
                    {
                        ici = codec;
                    }
                }
                EncoderParameters encoderParams = new EncoderParameters();
                long[] qualityParam = new long[1];

                qualityParam[0] = 80;

                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
                encoderParams.Param[0] = encoderParam;

                if (ici != null)
                {
                    image.Save(savePath, ici, encoderParams);
                }
                else
                {
                    image.Save(savePath);
                }

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }



        }




        /// <summary>
        /// 图片水印
        /// </summary>
        /// <param name="imgPath">原图绝对路径</param>
        /// <param name="savePath">生成图绝对路径</param>
        /// <param name="waterMarkPath">水印图片路径</param>
        /// <param name="wmPosition">水印位置</param>
        /// <param name="transparency">透明度(0-1)</param>
        /// <returns></returns>
        public static bool AddImageWaterMark(string imgPath, string savePath, string waterMarkPath, WaterMarkPosition wmPosition, float transparency)
        {

            Bitmap image = null;
            Graphics g = null;
            Image watermark = null;
            ImageAttributes imageAttributes = null;

            try
            {

                //创建一个图片对象用来装载要被添加水印的图片
                Image image1 = Image.FromStream(ByteToStream(SetImageToByteArray(imgPath)));
                image = new Bitmap(image1);
                image1.Dispose();
                g = Graphics.FromImage(image);
                //设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;


                watermark = new Bitmap(waterMarkPath);
                //if (watermark.Height >= image.Height || watermark.Width >= image.Width)
                //{
                //    //如果水印图片高宽超过原始图片则返回

                //    g.Dispose();
                //    image.Dispose();
                //    watermark.Dispose();
                //    return false;
                //}

                imageAttributes = new ImageAttributes();
                ColorMap colorMap = new ColorMap();

                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                ColorMap[] remapTable = { colorMap };

                imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);



                float[][] colorMatrixElements = {
                                            new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                            new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                            new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                            new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
                                            new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                        };

                ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                int xpos = 0;
                int ypos = 0;

                switch (wmPosition)
                {
                    case WaterMarkPosition.Left_Top:
                        xpos = (int)(image.Width * (float).01);
                        ypos = (int)(image.Height * (float).01);
                        break;

                    case WaterMarkPosition.Right_Top:
                        xpos = (int)((image.Width * (float).99) - (watermark.Width));
                        ypos = (int)(image.Height * (float).01);
                        break;

                    case WaterMarkPosition.Middle:
                        xpos = (int)((image.Width * (float).50) - (watermark.Width / 2));
                        ypos = (int)((image.Height * (float).50) - (watermark.Height / 2));
                        break;

                    case WaterMarkPosition.Left_Bottom:
                        xpos = (int)(image.Width * (float).01);
                        ypos = (int)((image.Height * (float).99) - watermark.Height);
                        break;

                    case WaterMarkPosition.Right_Bottom:
                        xpos = (int)((image.Width * (float).99) - (watermark.Width));
                        ypos = (int)((image.Height * (float).99) - watermark.Height);
                        break;
                }

                g.DrawImage(watermark, new Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);





                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.MimeType.IndexOf("jpeg") > -1)
                    {
                        ici = codec;
                    }
                }
                EncoderParameters encoderParams = new EncoderParameters();
                long[] qualityParam = new long[1];

                qualityParam[0] = 80;

                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
                encoderParams.Param[0] = encoderParam;

                if (ici != null)
                {
                    image.Save(savePath, ici, encoderParams);
                }
                else
                {
                    image.Save(savePath);
                }

                g.Dispose();
                image.Dispose();
                watermark.Dispose();
                imageAttributes.Dispose();


                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (g != null)
                    g.Dispose();
                if (image != null)
                    image.Dispose();
                if (watermark != null)
                    watermark.Dispose();
                if (imageAttributes != null)
                    imageAttributes.Dispose();
            }


        }
        #endregion

        #region 缩略图

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="imgPath">原图路径</param>
        /// <param name="savePath">生成图路径</param>
        /// <param name="width">缩略图宽</param>
        /// <param name="height">缩略图高</param>
        /// <returns></returns>
        public static bool CreateSmallImage(string imgPath, string savePath, int width, int height)
        {
            Image image = null;
            Bitmap bmp = null;
            Graphics g = null;
            try
            {
                //创建一个图片对象用来装载要被添加水印的图片
                image = Image.FromFile(imgPath);

                int oldWidth = image.Width;
                int oldHeight = image.Height;
                int newWidth = oldWidth;
                int newHeight = oldHeight;

                if (width > oldWidth && height > oldHeight)
                {
                    //如果原图偏小,如果生成路径需要覆盖原图，则直接返回
                    if (imgPath == savePath)
                    {
                        image.Dispose();
                        return true;
                    }

                    newWidth = oldWidth;
                    newHeight = oldHeight;
                }
                else if (width == 0)
                {
                    if (oldHeight > height)
                    {
                        newHeight = height;
                        newWidth = oldWidth * height / oldHeight;
                    }
                }
                else if (height == 0)
                {
                    if (oldWidth > width)
                    {
                        newWidth = width;
                        newHeight = oldHeight * width / oldWidth;

                    }
                }
                else
                {
                    if (oldWidth / oldHeight >= width / height)
                    {
                        newWidth = width;
                        newHeight = width * oldHeight / oldWidth;
                    }
                    else
                    {
                        newHeight = height;
                        newWidth = oldWidth * height / oldHeight;
                    }
                }



                bmp = new Bitmap(newWidth, newHeight);
                g = Graphics.FromImage(bmp);

                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.Clear(Color.Transparent);
                g.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight), 0, 0, oldWidth, oldHeight, GraphicsUnit.Pixel);
                image.Dispose();
                bmp.Save(savePath, ImageFormat.Jpeg);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {

                if (g != null)
                    g.Dispose();
                if (image != null)
                    image.Dispose();
                if (bmp != null)
                    bmp.Dispose();
            }
        }

        #endregion

        #region 获取图片对象
        /// <summary>
        /// 获取图片对象
        /// </summary>
        /// <param name="picName"></param>
        /// <returns></returns>
        public static System.Drawing.Image GetObjImg(string picName)
        {
            Image objImg = null;
            byte[] imgStreams = SetImageToByteArray(picName);
            if (imgStreams != null && imgStreams.Length > 0)
            {
                objImg = SetByteToImage(imgStreams);
            }

            return objImg;
        }
        #endregion

        #region 设置图片大小
        /// <summary>
        /// 设置图片大小
        /// 创建人：孙佳杰
        /// 创建时间：2012-5-31
        /// </summary>
        public static Boolean SetImgSize(string path, int maxWidth, int maxHeight)
        {
            int flag = 0;
            Image objImg = Image.FromFile(path);
            int width = objImg.Width;
            int height = objImg.Height;
            if (width > maxWidth)
            {
                width = maxWidth;
                flag++;
            }
            if (height > maxHeight)
            {
                height = maxHeight;
                flag++;
            }
            if (objImg != null)
            {
                objImg.Dispose();
            }
            if (flag > 0)
            {
                return CreateSmallImage(path, path, width, height);
            }

            return true;

        }
        #endregion

        /// <summary>
        /// 图片按比例缩放
        /// </summary>
        /// <param name="b"></param>
        /// <param name="destHeight"></param>
        /// <param name="destWidth"></param>
        /// <returns></returns>
        public static Bitmap GetThumbnail(Bitmap b, int destHeight, int destWidth)
        {
            System.Drawing.Image imgSource = b;
            System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
            int sW = 0, sH = 0;
            // 按比例缩放           
            int sWidth = imgSource.Width;
            int sHeight = imgSource.Height;
            if (sHeight > destHeight || sWidth > destWidth)
            {
                if ((sWidth * destHeight) > (sHeight * destWidth))
                {
                    sW = destWidth;
                    sH = (destWidth * sHeight) / sWidth;
                }
                else
                {
                    sH = destHeight;
                    sW = (sWidth * destHeight) / sHeight;
                }
            }
            else
            {
                sW = sWidth;
                sH = sHeight;
            }
            Bitmap outBmp = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.Transparent);
            // 设置画布的描绘质量         
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgSource, new Rectangle((destWidth - sW) / 2, (destHeight - sH) / 2, sW, sH), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            // 以下代码为保存图片时，设置压缩质量     
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            imgSource.Dispose();
            return outBmp;
        }
        //=========== 私有方法

        #region 将文件转换成流
        //public byte[] SetImageToByteArray(string fileName, ref string fileSize)
        /**/
        /// <summary>
        /// 将文件转换成流
        /// </summary>
        /// <param name="fileName">文件全路径</param>
        /// <returns></returns>
        public static byte[] SetImageToByteArray(string fileName)
        {
            byte[] image = null;
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open);
                FileInfo fileInfo = new FileInfo(fileName);
                //fileSize = Convert.ToDecimal(fileInfo.Length / 1024).ToString("f2") + " K";
                int streamLength = (int)fs.Length;
                image = new byte[streamLength];
                fs.Read(image, 0, streamLength);
                fs.Close();
                return image;
            }
            catch
            {
                return image;
            }
        }
        #endregion

        #region 将stream转回Byte数组
        /// <summary>
        /// 功能描述：将stream转回Byte数组
        /// 创建人：甘春雨
        /// 创建时间:2015年11月26日09:13:36
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static byte[] StreamToBytes(Stream stream)
        {

            byte[] bytes = new byte[stream.Length];

            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始

            stream.Seek(0, SeekOrigin.Begin);

            return bytes;

        }
        #endregion

        #region 将 byte[] 转成 Stream
        /// <summary>
        /// 功能描述：将 byte[] 转成 Stream
        /// 创建人：甘春雨
        /// 创建时间：2015年11月26日09:14:30
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>

        private static Stream BytesToStream(byte[] bytes)
        {

            Stream stream = new MemoryStream(bytes);

            return stream;

        }
        #endregion

        #region 将byte转换成MemoryStream类型
        /**/
        /// <summary>
        /// 将byte转换成MemoryStream类型
        /// </summary>
        /// <param name="mybyte">byte[]变量</param>
        /// <returns></returns>
        private static MemoryStream ByteToStream(byte[] mybyte)
        {
            MemoryStream mymemorystream = new MemoryStream(mybyte, 0, mybyte.Length);
            return mymemorystream;
        }
        #endregion

        #region 将byte转换成Image文件
        /**/
        /// <summary>
        /// 将byte转换成Image文件
        /// </summary>
        /// <param name="mybyte">byte[]变量</param>
        /// <returns></returns>
        private static System.Drawing.Image SetByteToImage(byte[] mybyte)
        {
            System.Drawing.Image image;
            MemoryStream mymemorystream = new MemoryStream(mybyte, 0, mybyte.Length);
            image = System.Drawing.Image.FromStream(mymemorystream);
            return image;
        }
        #endregion

    }


    /**/
    /// <summary>
    /// 水印的类型
    /// </summary>
    public enum WaterMarkType
    {
        /**/
        /// <summary>
        /// 文字水印
        /// </summary>
        TextMark,
        /**/
        /// <summary>
        /// 图片水印
        /// </summary>
        ImageMark
    };

    /**/
    /// <summary>
    /// 水印的位置
    /// </summary>
    public enum WaterMarkPosition
    {
        /**/
        /// <summary>
        /// 左上角
        /// </summary>
        Left_Top,
        /**/
        /// <summary>
        /// 左下角
        /// </summary>
        Left_Bottom,
        /**/
        /// <summary>
        /// 右上角
        /// </summary>
        Right_Top,
        /**/
        /// <summary>
        /// 右下角
        /// </summary>
        Right_Bottom,
        /// <summary>
        /// 中间
        /// </summary>
        Middle
    };
}
