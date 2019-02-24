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
    public class KMeans
    {
        public Bitmap Clusterize(Bitmap source, int ncCluster)
        {
            Bitmap output = new Bitmap(source);
            BitmapData data = output.LockBits(new Rectangle(0, 0, output.Width, output.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            IntPtr ptr = data.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(data.Stride) * output.Height;
            byte[] rgb = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            for (int i = 0; i < rgb.Length; i += 3)
            {
            }

            //Copy changed RGB values back to bitmap
            Marshal.Copy(rgb, 0, ptr, bytes);

            output.UnlockBits(data);
            return output;
        }
    }
}
