using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageProcessing.Thresholding
{
    /**
	* @overview Méthode d'abstraction de conversion d'une image en filtre inverse
	*/
    public class OtsuThresholding
    {
        /// <requires>source != null && source is gra</requires>
        /// <effects>Invertion de l'image source</effects>
        /// <returns>Une bitmap inversée</returns>
        public static double Compute(Bitmap source)
        {
            Bitmap output = new Bitmap(source);
            BitmapData data = output.LockBits(new Rectangle(0, 0, output.Width, output.Height), ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);

            IntPtr ptr = data.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(data.Stride) * output.Height;
            byte[] rgb = new byte[bytes];
            double[] histo = new double[256];
            double pixelCount = source.Height * source.Width;

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            //Compte le nombre de valeur grise pour chaque niveau
            for (int i = 0; i < rgb.Length; i += 3)
                histo[rgb[i]] = histo[rgb[i]]++;

            double imageMean = 0;

            //calcul le pourcentage de probabilité
            for (int i = 0; i < 256; i++)
            { 
                histo[i] = histo[i] / pixelCount;
                imageMean += histo[i] * i;
            }

            double max = double.MinValue;
            // initial class probabilities
            double class1Probability = 0;
            double class2Probability = 1;

            // initial class 1 mean value
            double class1MeanInit = 0;
            double calculatedThreshold = 0;

            // check all thresholds
            for (int t = 0; (t < 256) && (class2Probability > 0); t++)
            {
                // calculate class means for the given threshold
                double class1Mean = class1MeanInit;
                double class2Mean = (imageMean - (class1Mean * class1Probability)) / class2Probability;

                // calculate between class variance
                double betweenClassVariance = (class1Probability) * (1.0 - class1Probability) * Math.Pow(class1Mean - class2Mean, 2);

                // check if we found new threshold candidate
                if (betweenClassVariance > max)
                {
                    max = betweenClassVariance;
                    calculatedThreshold = t;
                }

                // update initial probabilities and mean value
                class1MeanInit *= class1Probability;

                class1Probability += histo[t];
                class2Probability -= histo[t];

                class1MeanInit += (double)t * (double)histo[t];

                if (class1Probability != 0)
                {
                    class1MeanInit /= class1Probability;
                }
            }


            output.UnlockBits(data);
            return calculatedThreshold;
        }

    }
}