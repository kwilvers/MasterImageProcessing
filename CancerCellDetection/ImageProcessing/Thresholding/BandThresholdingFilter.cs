﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageProcessing.Thresholding
{
    /**
	 * @overview Méthode d'abstraction de seuillage d'une image en deux classe binaire, le blanc et le noir
     * G(x,y)_final={255 si 〖G(x,y)〗_initial≥S
     *              {0   si 〖G(x,y)〗_initial<S
	*/
    public class BandThresholdingFilter
    {
        /// <requires>source != null</requires>
        /// <effects>Seuillage des valeurs de l'image, les valeurs inférieures au seuil sont forcée à 0 sinon à 255</effects>
        /// <returns>Une bitmap dont les valeurs sont limitée a un seuil</returns>
        public static Bitmap Apply(Bitmap source, Color mid, int tolerance, int tolerance2)
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
                if (!Math2.Between(rgb[i], mid.B - tolerance, mid.B + tolerance)
                    || !Math2.Between(rgb[i + 1], mid.G - tolerance, mid.G + tolerance)
                    || !Math2.Between(rgb[i + 2], mid.R - tolerance, mid.R + tolerance))
                {
                    rgb[i] = 0;
                    rgb[i + 1] = 0;
                    rgb[i + 2] = 0;
                    continue;
                }

                var b = Math.Abs(mid.B - rgb[i]);
                var g = Math.Abs(mid.G - rgb[i+1]);
                var r = Math.Abs(mid.R - rgb[i+2]);

                if (Math.Abs(b - r) > tolerance2 || Math.Abs(b - g) > tolerance2 || Math.Abs(r - g) > tolerance2)
                {
                    rgb[i] = 0;
                    rgb[i + 1] = 0;
                    rgb[i + 2] = 0;
                    continue;
                }

                //rgb[i] = mid.B;
                //rgb[i + 1] = mid.G;
                //rgb[i + 2] = mid.R;
                rgb[i] = 255;
                rgb[i + 1] = 255;
                rgb[i + 2] = 255;

                //rgb[i] = (byte) (rgb[i] < threshold ? 0 : 255);
                //rgb[i + 1] = (byte)(rgb[i + 1] < threshold ? 0 : 255);
                //rgb[i + 2] = (byte)(rgb[i + 2] < threshold ? 0 : 255);
            }

            //Copy changed RGB values back to bitmap
            Marshal.Copy(rgb, 0, ptr, bytes);

            output.UnlockBits(data);
            return output;
        }

    }
}