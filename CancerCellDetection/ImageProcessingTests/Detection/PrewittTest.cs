using System;
using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace ImageProcessingTests.Detection
{
    [TestClass]
    public class PrewittTest
    {
        [TestMethod()]
        public void ConvolveGrayPrewittFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            //Filtre de Prewitt
            var resConv = Convolution.Convolve(res, new PrewittFilter());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayPrewittFilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolvePrewittO4FilterShapeTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\ech.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            //Filtre de Prewitt à 4 orientations
            var resConv = Convolution.Convolve(res, new PrewittFilter4O());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayPrewitt4OInvertedShape.png");
        }

        [TestMethod()]
        public void ConvolveGrayPrewittO4FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new PrewittFilter4O());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayPrewittO4FilterInvertedTest.png");
        }


        [TestMethod]
        public void CvPrewittFilter()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.Grayscale);

            //Matrice de gradient X et Y
            Mat outputX = new Mat(); Mat outputY = new Mat();
            Mat absX = new Mat(); Mat absY = new Mat();
            Mat output = new Mat();
            //Calcul des gradient X et Y

            //Creation des kernels
            var kernelx = new float[,]{
                { 1, 0, -1 },
                { 1, 0, -1 },
                { 1, 0, -1 }
            };
            var kernely = new float[,]{
                { 1, 1, 1 },
                { 0, 0, 0 },
                { -1, -1, -1 }
            };
            var kx = new Mat(3, 3, MatType.CV_32F, kernelx);
            var ky = new Mat(3, 3, MatType.CV_32F, kernely);
            //Convolution par deux kernels
            Cv2.Filter2D(v, outputX, -1, kx);
            Cv2.Filter2D(v, outputY, -1, ky);
            //Conversion en valeurs absolue 8 bits
            Cv2.ConvertScaleAbs(outputX, absX);
            Cv2.ConvertScaleAbs(outputY, absY);
            //Addition de deux matrice dont le poids de chacune des matrice est identique
            Cv2.AddWeighted(absX, 0.5, absY, 0.5, 0, output);


            Cv2.ImWrite(@".\CvPrewittFilterX.png", outputX);
            Cv2.ImWrite(@".\CvPrewittFilterY.png", outputY);
            Cv2.ImWrite(@".\CvPrewittFilter.png", output);
        }
    }
}
