using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using OpenCvSharp;

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


        public static Mat Apply(Mat input, int threshold, bool maximalPeak)
        {
            Mat dest = input.Clone();

            int width = input.Cols;
            int height = input.Rows;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (!maximalPeak)
                    {
                        var v = dest.At<Vec3b>(y, x);

                        v.Item0 = v.Item0 >= threshold ? v.Item0 : (byte)0;
                        v.Item1 = v.Item1 >= threshold ? v.Item1 : (byte)0;
                        v.Item2 = v.Item2 >= threshold ? v.Item2 : (byte)0;

                        dest.Set(y, x, v);
                    }
                    else
                    {
                        var v = dest.At<Vec3b>(y, x);

                        v.Item0 = v.Item0 >= threshold ? (byte)0 : v.Item0;
                        v.Item1 = v.Item1 >= threshold ? (byte)0 : v.Item1;
                        v.Item2 = v.Item2 >= threshold ? (byte)0 : v.Item2;

                        dest.Set(y, x, v);
                    }

                }
            }

            return dest;
        }

        public static Mat ParallelApply(Mat input, int threshold, bool maximalPeak)
        {
            Mat dest = input.Clone();

            int width = input.Cols;
            int height = input.Rows;

            Parallel.For(0, height, y =>
            {
                for (int x = 0; x < width; x++)
                {
                    if (!maximalPeak)
                    {
                        var v = dest.At<Vec3b>(y, x);

                        v.Item0 = v.Item0 >= threshold ? v.Item0 : (byte) 0;
                        v.Item1 = v.Item1 >= threshold ? v.Item1 : (byte) 0;
                        v.Item2 = v.Item2 >= threshold ? v.Item2 : (byte) 0;

                        dest.Set(y, x, v);
                    }
                    else
                    {
                        var v = dest.At<Vec3b>(y, x);

                        v.Item0 = v.Item0 >= threshold ? (byte) 0 : v.Item0;
                        v.Item1 = v.Item1 >= threshold ? (byte) 0 : v.Item1;
                        v.Item2 = v.Item2 >= threshold ? (byte) 0 : v.Item2;

                        dest.Set(y, x, v);
                    }

                }
            });

            return dest;
        }
    }
}