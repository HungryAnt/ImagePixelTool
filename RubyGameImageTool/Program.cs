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
            if (args.Count() != 1)
            {
                Console.WriteLine("need argument: image_dir");
                return;
            }
            String imageDir = args[0];
            DirectoryInfo rootDir = new DirectoryInfo(imageDir);
            FileInfo[] fileInfos = rootDir.GetFiles();
            foreach (var fileInfo in fileInfos)
            {
                modifyBmp(fileInfo.FullName);
            }
            Console.WriteLine("Done! ~ Ant robot");
        }

        static void modifyBmp(String path)
        {
            var bitmapSource = ImageUtility.LoadBitmapImageAndCloseFile(path);
            var newBitmapSource = ImageUtility.TransferPixels(
                bitmapSource, Color.FromRgb(0, 0, 248), Color.FromRgb(255, 0, 255));
            ImageUtility.SaveBitmap(path, newBitmapSource);
        }
    }
}
