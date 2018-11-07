using System;
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
    public class BinaryThresholdingFilter
    {
        /// <requires>source != null</requires>
        /// <effects>Seuillage des valeurs de l'image, les valeurs inférieures au seuil sont forcée à 0 sinon à 255</effects>
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
                rgb[i] = (byte) (rgb[i] < threshold ? 0 : 255);
                rgb[i + 1] = (byte)(rgb[i + 1] < threshold ? 0 : 255);
                rgb[i + 2] = (byte)(rgb[i + 2] < threshold ? 0 : 255);
            }

            //Copy changed RGB values back to bitmap
            Marshal.Copy(rgb, 0, ptr, bytes);

            output.UnlockBits(data);
            return output;
        }

    }
}