using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BGDesigner.Models.Pixel;

namespace Gods.Foundation.Utils
{
    public static class ImageUtility
    {
        /// <summary>
        /// 从文件加载图片文件，加载完成关闭文件
        /// </summary>
        /// <param name="absolutImagePath"></param>
        /// <returns></returns>
        public static BitmapSource LoadBitmapImageAndCloseFile(string absoluteImagePath)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(absoluteImagePath, UriKind.Absolute);
            image.EndInit();
            return ConvertToBgr24FormatIfNecessary(image);
        }

        private static BitmapSource ConvertToBgr24FormatIfNecessary(BitmapSource originalImage)
        {
            if (originalImage.Format == PixelFormats.Bgr24)
            {
                return originalImage;
            }
            return new FormatConvertedBitmap(originalImage, PixelFormats.Bgr24, null, 0);
        }

        public static BitmapSource TransferPixels(BitmapSource bitmapSource, Color from, Color to)
        {
            int pixelWidth, pixelHeight, stride;
            byte[] pixelsData = GetBitmapSourcePixelsData(bitmapSource, out pixelWidth, out pixelHeight, out stride);
            for (int x = 0; x < pixelWidth; x++)
            {
                for (int y = 0; y < pixelHeight; y++)
                {
                    byte blue, green, red;
                    PixelUtility.GetPixelBgr(pixelsData, x, y, stride, out blue, out green, out red);
                    if (blue == from.B && green == from.G && red == from.R)
                    {
                        PixelUtility.SetPixelBgr(pixelsData, x, y, stride, to.B, to.G, to.R);
                    }
                }
            }

            WriteableBitmap writeableBitmap = new WriteableBitmap(bitmapSource);
            writeableBitmap.WritePixels(
                new Int32Rect(0, 0, pixelWidth, pixelHeight),
                pixelsData, stride, 0);
            return writeableBitmap;
        }

        public static byte[] GetBitmapSourcePixelsData(
            BitmapSource bitmapSource, out int pixelWidth, out int pixelHeight, out int stride)
        {
            pixelWidth = bitmapSource.PixelWidth;
            pixelHeight = bitmapSource.PixelHeight;
            stride = bitmapSource.Format.BitsPerPixel * pixelWidth / 8;
            byte[] data = new byte[stride * pixelHeight];
            bitmapSource.CopyPixels(data, stride, 0);
            return data;
        }

        public static void SaveBitmap(String fileName, BitmapSource bitmapSource)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                BmpBitmapEncoder bitmapEncoder = new BmpBitmapEncoder();
                bitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                bitmapEncoder.Save(stream);
            }
        }
    }
}
