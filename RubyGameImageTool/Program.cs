using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Gods.Foundation.Utils;

namespace RubyGameImageTool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() < 1)
            {
                Console.WriteLine("need argument: image_dir [png]");
                return;
            }

            String imageDir = args[0];
            Function function = Function.Origin;
            if (args.Length >= 2)
            {
                if (args[1] == "png")
                {
                    function = Function.ToPng;
                }
                else if (args[1] == "png2")
                {
                    function = Function.ToPng2;
                }
            }
            
            DirectoryInfo rootDir = new DirectoryInfo(imageDir);
            FileInfo[] fileInfos = rootDir.GetFiles("*.bmp");

            foreach (var fileInfo in fileInfos)
            {
                if (function == Function.Origin)
                {
                    modifyBmp(fileInfo.FullName);
                }
                else if (function == Function.ToPng)
                {
                    toPng(fileInfo.FullName, Color.FromRgb(0, 0, 248));
                }
                else if (function == Function.ToPng2)
                {
                    toPng(fileInfo.FullName, Color.FromRgb(255, 0, 255));
                }
            }
            Console.WriteLine("Done! ~ Ant robot");
        }

        static void modifyBmp(String path)
        {
            var bitmapSource = ImageUtility.LoadBitmapImageAndCloseFile(path);
            var newBitmapSource = ImageUtility.TransferPixels(
                bitmapSource,
                Color.FromRgb(0, 0, 248),
                Color.FromArgb(0, 0, 0, 0) // Color.FromRgb(255, 0, 255)
                );
            ImageUtility.SaveBitmap(path, newBitmapSource);
        }

        static void toPng(String path, Color alphaColor)
        {
            var bitmapSource = ImageUtility.LoadBitmapImageAndCloseFile(path);
            var newBitmapSource = ImageUtility.TransferPixels(
                bitmapSource,
                alphaColor,
                Color.FromArgb(0, 255, 255, 255),
                true
                );
            String pngFilePath = Path.ChangeExtension(path, ".png");
            ImageUtility.SavePng(pngFilePath, newBitmapSource);
        }
    }
}
