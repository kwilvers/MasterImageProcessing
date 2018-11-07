using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageProcessing.Thresholding
{
    /**
	 * @overview Méthode d'abstraction de seuillage d'une image en tronquant les valeurs supérieur
     * au seuil à la valeur du seuil
     * G(x,y)_final={S                 si G(x,y)_initial ≥ S
     *              {G(x,y)_initial    si G(x,y)_initial < S
	*/
    public class TruncatedThresholdingFilter
    {
        /// <requires>source != null</requires>
        /// <effects>Seuillage des valeurs de l'image, les valeurs supérieur au seuil sont forcée à la valeur du seuil sinon elle reste inchangée</effects>
        /// <returns>Une bitmap dont les valeurs sont limitée a un seuil</returns>
        public static Bitmap Apply(Bitmap source, int threshold)
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
                rgb[i] = (byte) (rgb[i] >= threshold ? threshold : rgb[i]);
                rgb[i + 1] = (byte)(rgb[i + 1] >= threshold ? threshold : rgb[i + 1]);
                rgb[i + 2] = (byte)(rgb[i + 2] >= threshold ? threshold : rgb[i + 2]);
            }

            //Copy changed RGB values back to bitmap
            Marshal.Copy(rgb, 0, ptr, bytes);

            output.UnlockBits(data);
            return output;
        }

    }
}