using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageProcessing.Thresholding
{
    /**
	 * @overview Méthode d'abstraction de conversion d'une image en filtre inverse
     * G(x,y)_final = {G(x,y)_initial   si G(x,y)_initial ≥ S
     *                {0                si G(x,y)_initial < S
     * Le seuillage par zéro peut être inversé
     * G(x,y)_final = {0                si G(x,y)_initial ≥ S
     *                {G(x,y)_initial   si G(x,y)_initial < S 
	*/
    public class ZeroThresholdingFilter
    {
        /// <requires>source != null</requires>
        /// <effects>Seuillage des valeurs de l'image, les valeurs inférieures au seuil sont forcée à 0 sinon elles conservent leur valeurs</effects>
        /// <returns>Une bitmap dont les valeurs sont limitée a un seuil</returns>
        public static Bitmap Apply(Bitmap source, int threshold, bool maximalPeak)
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
                if (!maximalPeak)
                {
                    rgb[i] = (byte)(rgb[i] >= threshold ? rgb[i] : 0);
                    rgb[i + 1] = (byte)(rgb[i + 1] >= threshold ? rgb[i + 1] : 0);
                    rgb[i + 2] = (byte)(rgb[i + 2] >= threshold ? rgb[i + 2] : 0);
                }
                else
                {
                    rgb[i] = (byte)(rgb[i] >= threshold ? 0 : rgb[i]);
                    rgb[i + 1] = (byte)(rgb[i + 1] >= threshold ? 0 : rgb[i + 1]);
                    rgb[i + 2] = (byte)(rgb[i + 2] >= threshold ? 0 : rgb[i + 2]);
                }
            }

            //Copy changed RGB values back to bitmap
            Marshal.Copy(rgb, 0, ptr, bytes);

            output.UnlockBits(data);
            return output;
        }

    }
}