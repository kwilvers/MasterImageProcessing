using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using OpenCvSharp;

namespace ImageProcessing.Correction
{
    /**
    * @overview Méthode d'abstraction de correction gamma de l'image 
    */
    public static class ColorIsolation
    {
        /// <requires>source != null</requires>
        /// <effects>Correction gamma de l'image source</effects>
        /// <returns>Une bitmap corrigé en fonction du facteru gammaFactor</returns>
        public static Bitmap Isolate(Bitmap source, bool removeRed=false, bool removeGreen = false, bool removeBlue = false)
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
                rgb[i] = removeBlue ? (byte)0 : rgb[i];
                rgb[i + 1] = removeGreen ? (byte)0 : rgb[i + 1];
                rgb[i + 2] = removeRed ? (byte)0 : rgb[i + 2];
            }

            //Copy changed RGB values back to bitmap
            Marshal.Copy(rgb, 0, ptr, bytes);

            output.UnlockBits(data);
            return output;
        }

        public static Mat Isolate(Mat input, bool removeRed = false, bool removeGreen = false, bool removeBlue = false)
        {
            Mat dest = input.Clone();

            int width = input.Cols;
            int height = input.Rows;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var v = dest.At<Vec3b>(y, x);

                    v.Item0 = removeBlue ? (byte)0 : v.Item0;
                    v.Item1 = removeGreen ? (byte)0 : v.Item1;
                    v.Item2 = removeRed ? (byte)0 : v.Item2;
                    
                    dest.Set(y, x, v);
                }
            }

            return dest;
        }

        public static Mat ParallelIsolate(Mat input, bool removeRed = false, bool removeGreen = false, bool removeBlue = false)
        {
            Mat dest = input.Clone();

            int width = input.Cols;
            int height = input.Rows;

            Parallel.For(0, height, y =>
            {
                for (int x = 0; x < width; x++)
                {
                    var v = dest.At<Vec3b>(y, x);

                    v.Item0 = removeBlue ? (byte) 0 : v.Item0;
                    v.Item1 = removeGreen ? (byte) 0 : v.Item1;
                    v.Item2 = removeRed ? (byte) 0 : v.Item2;

                    dest.Set(y, x, v);
                }
            });

            return dest;
        }
    }
}