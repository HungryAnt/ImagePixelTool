using BGDesigner.Models.Pixel;
using Gods.Foundation.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BlackToTransparentTool
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

            BitmapSource bitmapSource = ImageUtility.LoadBitmapImageAndCloseFile(inFile);
            BitmapSource newBitmapSource = Convert(bitmapSource);
            ImageUtility.SavePng(outFile, newBitmapSource);
        }

        private static BitmapSource Convert(BitmapSource bitmapSource)
        {
            bitmapSource = ImageUtility.ConvertToBgra32Format(bitmapSource);

            int pixelWidth, pixelHeight, stride;
            byte[] pixelsData = ImageUtility.GetBitmapSourcePixelsData(bitmapSource, out pixelWidth, out pixelHeight, out stride);

            for (int x = 0; x < pixelWidth; x++)
            {
                for (int y = 0; y < pixelHeight; y++)
                {
                    byte alpha, blue, green, red;
                    PixelUtility.GetPixelBgra(pixelsData, x, y, stride, out blue, out green, out red, out alpha);
                    if (blue == 0 && green == 0 && red == 0)
                    {
                        PixelUtility.SetPixelBgra(pixelsData, x, y, stride, 0, 0, 0, 0);
                        continue;
                    }

                    // blue:255 alpha:blue
                    // green:255 alpha:green
                    // red:255 alpha:red

                    byte newMaxAlpha = Math.Max(Math.Max(blue, green), red);
                    byte newBlue = (byte) (255 * blue / newMaxAlpha);
                    byte newGreen = (byte) (255 * green / newMaxAlpha);
                    byte newRed = (byte) (255 * red / newMaxAlpha);

                    PixelUtility.SetPixelBgra(pixelsData, x, y, stride, newBlue, newGreen, newRed, newMaxAlpha);
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
