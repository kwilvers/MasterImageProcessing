using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageProcessing
{
    /**
	* @overview Méthode d'abstraction de conversion d'une image en filtre inverse
	*/
    public class InverterFilter
    {
        /// <requires>source != null</requires>
        /// <effects>Invertion de l'image source</effects>
        /// <returns>Une bitmap inversée</returns>
        public static Bitmap Invert(Bitmap source)
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
                rgb[i] = (byte)(255 - rgb[i]);
                rgb[i + 1] = (byte) (255 - rgb[i + 1]);
                rgb[i + 2] = (byte)(255 - rgb[i + 2]);
            }

            //Copy changed RGB values back to bitmap
            Marshal.Copy(rgb, 0, ptr, bytes);

            output.UnlockBits(data);
            return output;
        }

    }
}