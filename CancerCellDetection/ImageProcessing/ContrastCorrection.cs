﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace ImageProcessing
{
    /**
    * @overview Méthode d'abstraction de correction gamma de l'image 
    */
    public class ContrastCorrection
    {
        /// <requires>source != null</requires>
        /// <effects>Correction gamma de l'image source</effects>
        /// <returns>Une bitmap corrigé en fonction du facteru gammaFactor</returns>
        public static Bitmap Correct(Bitmap source, double threshold)
        {
            Bitmap output = new Bitmap(source);
            BitmapData data = output.LockBits(new Rectangle(0, 0, output.Width, output.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            IntPtr ptr = data.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(data.Stride) * output.Height;
            byte[] rgb = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            //compute contrast factor
            double contrastFactor = Math.Pow((100.0 + threshold) / 100.0, 2);

            for (int i = 0; i < rgb.Length; i += 3)
            {
                rgb[i] = ApplyFactor(rgb[i], contrastFactor);
                rgb[i + 1] = ApplyFactor(rgb[i + 1], contrastFactor);
                rgb[i + 2] = ApplyFactor(rgb[i + 2], contrastFactor);
            }

            //Copy changed RGB values back to bitmap
            Marshal.Copy(rgb, 0, ptr, bytes);

            output.UnlockBits(data);
            return output;
        }

        private static byte ApplyFactor(byte pixel, double contrast)
        {
            double d = (((pixel / 255.0 - 0.5)*contrast)+0.5)*255.0;
            
            return (byte) (d > 255 ? 255 : d < 0 ? 0 : d);
        }

    }
}