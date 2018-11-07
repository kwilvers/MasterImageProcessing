using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageProcessing.Thresholding
{
    /**
	 * @overview Tout pixel dont le gradient est inférieur au seuil bas est mis à zéro, tout pixel dont le gradient est supérieur au
     * seuil haut est considéré comme un contour et mis à la valeur max.  Les pixels situés entre le seuil bas et le seuil haut sont
     * accepté comme contour et mis au maximum s’il possède un contour voisin dans le sens du gradient sinon ils sont rejeté et mis à 0.
     * G(x,y)_final = {0        si G(x,y)_initial ≤ Sb
     *                {255      si G(x,y)_initial ≥ Sh
     *                {0        si Sb < G(x,y)_initial < Sh  et voisin(G(x,y)==false)
     *                {255      si Sb < G(x,y)_initial < Sh  et voisin(G(x,y)==true) 
	*/
    public class HysteresisThresholdingFilter
    {
        /// <requires>source != null</requires>
        /// <effects>
        /// Seuillage des valeurs de l'image, les valeurs supérieur au seuil haut sont forcées à  255
        /// Les valeurs en dessous de la valeur du seuil bas sont forcée à 0
        /// 0 si la valeur est entre les seuils et n'est pas voisine d'un bord
        /// 255 si la valeur est entre les seuils et est voisine d'un bord
        /// </effects>
        /// <returns>Une bitmap dont les valeurs sont limitée a un seuil</returns>
        public static Bitmap ApplyBW(Bitmap source, int lowThreshold, int highThreshold)
        {
            Bitmap output = new Bitmap(source);
            BitmapData data = output.LockBits(new Rectangle(0, 0, output.Width, output.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            IntPtr ptr = data.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(data.Stride) * output.Height;
            byte[] rgb = new byte[bytes];
            byte[] rgb2 = new byte[bytes];
            byte[] rgb3 = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            int lt = lowThreshold;
            int ht = highThreshold;

            for (int i = 0; i < rgb.Length; i += 3)
            {
                //image secondaire contenant les contours candidats situés entre les seuils
                rgb2[i] = (byte)(lt <= rgb[i] && rgb[i] <= ht ? 255 : 0);
                rgb2[i + 1] = (byte)(lt <= rgb[i + 1] && rgb[i + 1] <= ht ? 255 : 0);
                rgb2[i + 2] = (byte)(lt <= rgb[i + 2] && rgb[i +2 ] <= ht ? 255 : 0);

                //Image principale contenant les contours sûres
                rgb[i] = (byte) (rgb[i] >= ht ? 255 : 0);
                rgb[i + 1] = (byte)(rgb[i + 1] >= ht ? 255 : 0);
                rgb[i + 2] = (byte)(rgb[i + 2] >= ht ? 255 : 0);
            }

  

            int padding = 1;
            int stride = data.Stride;
            int calcOffset = 0;
            int byteOffset = 0;

            for (int row = padding; row < source.Height - padding; row++)
            {
                //foreach lines
                for (int line = padding; line < source.Width - padding; line++)
                {
                    byteOffset = row * stride + line * 3;
                    bool hasNeighbour = false;

                    rgb3[byteOffset] = rgb[byteOffset];
                    rgb3[byteOffset + 1] = rgb[byteOffset + 1];
                    rgb3[byteOffset + 2] = rgb[byteOffset + 2];

                    if (rgb2[byteOffset] == 255)
                    {
                        //foreach row in kernel
                        for (int neighbourRow = -padding; neighbourRow <= padding; neighbourRow++)
                        {
                            //foreach line in kernel
                            for (int neighbourLine = -padding; neighbourLine <= padding; neighbourLine++)
                            {
                                calcOffset = byteOffset +
                                             (neighbourLine * 3) +
                                             (neighbourRow * stride);

                                if (rgb[calcOffset] == 255)
                                    hasNeighbour = true;

                            }
                        }

                        if (hasNeighbour)
                        {
                            rgb3[byteOffset] =  255;
                            rgb3[byteOffset + 1] = 255;
                            rgb3[byteOffset + 2] = 255;
                        }
                    }
                }
            }




            //Copy changed RGB values back to bitmap
            Marshal.Copy(rgb3, 0, ptr, bytes);

            output.UnlockBits(data);
            return output;
        }

        public static Bitmap Apply(Bitmap source, int lowThreshold, int highThreshold)
        {
            Bitmap output = new Bitmap(source);
            BitmapData data = output.LockBits(new Rectangle(0, 0, output.Width, output.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            IntPtr ptr = data.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(data.Stride) * output.Height;
            byte[] rgb = new byte[bytes];
            byte[] rgb2 = new byte[bytes];
            byte[] rgb3 = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            int lt = lowThreshold;
            int ht = highThreshold;

            for (int i = 0; i < rgb.Length; i += 3)
            {
                //image secondaire contenant les contours candidats situés entre les seuils
                rgb2[i] = (byte)(lt <= rgb[i] && rgb[i] <= ht ? 255 : 0);
                rgb2[i + 1] = (byte)(lt <= rgb[i + 1] && rgb[i + 1] <= ht ? 255 : 0);
                rgb2[i + 2] = (byte)(lt <= rgb[i + 2] && rgb[i + 2] <= ht ? 255 : 0);

                //Image principale contenant les contours sûres
                rgb[i] = (byte)(rgb[i] >= ht ? rgb[i] : 0);
                rgb[i + 1] = (byte)(rgb[i + 1] >= ht ? rgb[i + 1] : 0);
                rgb[i + 2] = (byte)(rgb[i + 2] >= ht ? rgb[i + 2] : 0);
            }



            int padding = 1;
            int stride = data.Stride;
            int calcOffset = 0;
            int byteOffset = 0;

            for (int row = padding; row < source.Height - padding; row++)
            {
                //foreach lines
                for (int line = padding; line < source.Width - padding; line++)
                {
                    byteOffset = row * stride + line * 3;
                    bool hasNeighbour = false;

                    rgb3[byteOffset] = rgb[byteOffset];
                    rgb3[byteOffset + 1] = rgb[byteOffset + 1];
                    rgb3[byteOffset + 2] = rgb[byteOffset + 2];

                    if (rgb2[byteOffset] == 255)
                    {
                        //foreach row in kernel
                        for (int neighbourRow = -padding; neighbourRow <= padding; neighbourRow++)
                        {
                            //foreach line in kernel
                            for (int neighbourLine = -padding; neighbourLine <= padding; neighbourLine++)
                            {
                                calcOffset = byteOffset +
                                             (neighbourLine * 3) +
                                             (neighbourRow * stride);

                                if (rgb[calcOffset] == 255)
                                    hasNeighbour = true;

                            }
                        }

                        if (hasNeighbour)
                        {
                            rgb3[byteOffset] = rgb[byteOffset];
                            rgb3[byteOffset + 1] = rgb[byteOffset + 1];
                            rgb3[byteOffset + 2] = rgb[byteOffset + 2];
                        }
                    }
                }
            }




            //Copy changed RGB values back to bitmap
            Marshal.Copy(rgb3, 0, ptr, bytes);

            output.UnlockBits(data);
            return output;
        }

    }
}