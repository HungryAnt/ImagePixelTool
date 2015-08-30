namespace BGDesigner.Models.Pixel
{
    public static class PixelUtility
    {

        public static void GetPixelBgr(byte[] data, int x, int y, int stride,
            out byte blue, out byte green, out byte red)
        {
            int index = stride * y + x * 3;
            blue = data[index];
            green = data[index + 1];
            red = data[index + 2];
        }

        public static void SetPixelBgr(byte[] data, int x, int y, int stride,
            byte blue, byte green, byte red)
        {
            int index = stride * y + x * 3;
            data[index] = blue;
            data[index + 1] = green;
            data[index + 2] = red;
        }

        public static void GetPixelBgra(byte[] data, int x, int y, int stride,
            out byte blue, out byte green, out byte red, out byte alpha)
        {
            int index = stride * y + x * 4;
            blue = data[index];
            green = data[index + 1];
            red = data[index + 2];
            alpha = data[index + 3];
        }

        public static void SetPixelBgra(byte[] data, int x, int y, int stride,
            byte blue, byte green, byte red, byte alpha)
        {
            int index = stride * y + x * 4;
            data[index] = blue;
            data[index + 1] = green;
            data[index + 2] = red;
            data[index + 3] = alpha;
        }
    }
}
