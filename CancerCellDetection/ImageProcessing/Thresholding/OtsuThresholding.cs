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
        public static double Compute1(Bitmap source)
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
                double betweenClassVariance = (class1Probability) * (1.0 - class1Probability) *
                                              Math.Pow(class1Mean - class2Mean, 2);

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

                class1MeanInit += (double) t * (double) histo[t];

                if (class1Probability != 0)
                {
                    class1MeanInit /= class1Probability;
                }
            }


            output.UnlockBits(data);
            return calculatedThreshold;
        }



        // Le nombre de niveaux d'intensité dans l'image.
        // La valeur standard pour l'image grise est 256. De 0 à 255.
        const int INTENSITY_LAYER_NUMBER = 256;

        // retourne l'histogramme par intensité d'image de 0 à 255 inclus
        private static int[] CalculateHist(byte[] image, int size)
        {
            int[] hist = new int[INTENSITY_LAYER_NUMBER];

            // Initialise l'histogramme avec des zéros
            for (int i = 0; i < INTENSITY_LAYER_NUMBER; ++i)
            {
                hist[i] = 0;
            }

            // calcule l'histogramme
            for (int i = 0; i < size; ++i)
            {
                ++hist[image[i]];
            }

            return hist;
        }

        // Calcule la somme de toutes les intensités
        private static int CalculateIntensitySum(byte[] image, int size)
        {
            int sum = 0;
            for (int i = 0; i < size; ++i)
            {
                sum += image[i];
            }

            return sum;
        }

        // La fonction renvoie le seuil de binarisation de l'image en demi-teinte avec le nombre total de pixels.
        // const IMAGE * image contient une intensité d'image de 0 à 255 inclus.
        // taille - le nombre de pixels dans l'image.
        public static int Compute(Bitmap image)
        {
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);

            IntPtr ptr = data.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int size = Math.Abs(data.Stride) * image.Height;
            byte[] rgb = new byte[size];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, size);


            int[] hist = CalculateHist(rgb, size);

            // Nécessaire pour recalculer rapidement la différence de variance
            int all_pixel_count = size;
            int all_intensity_sum = CalculateIntensitySum(rgb, size);

            int best_thresh = 0;
            double best_sigma = 0.0;

            int first_class_pixel_count = 0;
            int first_class_intensity_sum = 0;

            // itère la frontière entre les classes
            // thresh <INTENSITY_LAYER_NUMBER - 1, depuis  à 255, le dénominateur à l'intérieur des feuilles
            for (int thresh = 0; thresh < INTENSITY_LAYER_NUMBER - 1; ++thresh)
            {
                first_class_pixel_count += hist[thresh];
                first_class_intensity_sum += thresh * hist[thresh];

                double first_class_prob = first_class_pixel_count / (double) all_pixel_count;
                double second_class_prob = 1.0 - first_class_prob;

                double first_class_mean = first_class_intensity_sum / (double) first_class_pixel_count;
                double second_class_mean = (all_intensity_sum - first_class_intensity_sum)
                                           / (double) (all_pixel_count - first_class_pixel_count);

                double mean_delta = first_class_mean - second_class_mean;

                double sigma = first_class_prob * second_class_prob * mean_delta * mean_delta;

                if (sigma > best_sigma)
                {
                    best_sigma = sigma;
                    best_thresh = thresh;
                }
            }

            image.UnlockBits(data);

            return best_thresh;
        }

    }
}