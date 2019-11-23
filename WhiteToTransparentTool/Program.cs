using BGDesigner.Models.Pixel;
using Gods.Foundation.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WhiteToTransparentTool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() != 2)
            {
                Console.WriteLine("need arguments: in_file out_file");
                return;
            }

            string inFile = args[0];
            string outFile = args[1];

            BitmapSource bitmapSource = LoadBitmapImageAndCloseFile(inFile);
            BitmapSource newBitmapSource = Convert(bitmapSource);
            ImageUtility.SavePng(outFile, newBitmapSource);
        }

        public static BitmapSource LoadBitmapImageAndCloseFile(string absoluteImagePath)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(absoluteImagePath, UriKind.Absolute);
            image.EndInit();
            return new FormatConvertedBitmap(image, PixelFormats.Bgra32, null, 0);
        }

        private static BitmapSource Convert(BitmapSource bitmapSource)
        {
            // bitmapSource = ImageUtility.ConvertToBgra32Format(bitmapSource);

            int pixelWidth, pixelHeight, stride;
            byte[] pixelsData = ImageUtility.GetBitmapSourcePixelsData(bitmapSource, out pixelWidth, out pixelHeight, out stride);

            for (int x = 0; x < pixelWidth; x++)
            {
                for (int y = 0; y < pixelHeight; y++)
                {
                    byte alpha, blue, green, red;
                    PixelUtility.GetPixelBgra(pixelsData, x, y, stride, out blue, out green, out red, out alpha);
                    if (blue == 255 && green == 255 && red == 255)
                    {
                        PixelUtility.SetPixelBgra(pixelsData, x, y, stride, 0, 0, 0, 0);
                        continue;
                    }

                    if (alpha < 255 || blue == 255 || green == 255 || red == 255)
                    {
                        PixelUtility.SetPixelBgra(pixelsData, x, y, stride, blue, green, red, alpha);
                        continue;
                    }

                    // blue:255 alpha:blue
                    // green:255 alpha:green
                    // red:255 alpha:red

                    byte averageValue = (byte)((blue + green + red) / 3);

                    byte newAlpha = (byte)(255 - averageValue);
                    byte newBlue = (byte)0;
                    byte newGreen = (byte)0;
                    byte newRed = (byte)0;

                    PixelUtility.SetPixelBgra(pixelsData, x, y, stride, newBlue, newGreen, newRed, newAlpha);
                }
            }

            WriteableBitmap writeableBitmap = new WriteableBitmap(bitmapSource);
            writeableBitmap.WritePixels(
                new Int32Rect(0, 0, pixelWidth, pixelHeight),
                pixelsData, stride, 0);
            return writeableBitmap;
        }
    }
}
