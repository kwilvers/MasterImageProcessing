using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

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
    }
}