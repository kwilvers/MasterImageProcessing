using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.Segmentation
{
    public static class PixelCount
    {
        public static int Count(Bitmap source)
        {
            var data = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(data.Stride) * source.Height;
            source.UnlockBits(data);
            
            return bytes/3;
        }

        public static int Count(Bitmap source, Color mid)
        {
            BitmapData data = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            IntPtr ptr = data.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(data.Stride) * source.Height;
            byte[] rgb = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            int cnt = 0;
            for (int i = 0; i < rgb.Length; i += 3)
            {
                if (mid.B == rgb[i] && mid.G == rgb[i + 1] && mid.R == rgb[i + 2])
                    cnt++;
            }

            source.UnlockBits(data);
            return cnt;
        }

    }
}
