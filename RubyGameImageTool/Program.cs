﻿using System;
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
                else if (args[1] == "png3")
                {
                    function = Function.ToPng3;
                }
                else if (args[1] == "png4")
                {
                    function = Function.ToPng4;
                }
                else if (args[1] == "png5")
                {
                    function = Function.ToPng5;
                }
                else if (args[1] == "png6") // 黑色背景
                {
                    function = Function.ToPng6;
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
                else if (function == Function.ToPng3)
                {
                    toPng(fileInfo.FullName, Color.FromRgb(0, 0, 248));
                }
                else if (function == Function.ToPng4)
                {
                    toPng(fileInfo.FullName, Color.FromRgb(0, 0, 255));
                }
                else if (function == Function.ToPng5)
                {
                    toPng(fileInfo.FullName, Color.FromRgb(0, 248, 0));
                }
                else if (function == Function.ToPng6)
                {
                    toPng(fileInfo.FullName, Color.FromRgb(0, 0, 0));
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
