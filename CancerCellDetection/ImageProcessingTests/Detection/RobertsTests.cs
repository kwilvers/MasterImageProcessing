using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using Point = System.Drawing.Point;

namespace ImageProcessingTests.Detection
{
    [TestClass()]
    public class RobertsTests
    {
        [TestMethod()]
        public void ConvolveGrayRobertsFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new RobertsFilter());
            resConv.Output.Save(@".\RobertsFilter.png");
        }

        [TestMethod()]
        public void ConvolveGrayRobertsFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            //Filtre de roberts
            var resConv = Convolution.Convolve(res, new RobertsFilter());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\RobertsFilterInverted.png");
        }

        [TestMethod]
        public void CvRobertsFilter()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.Grayscale);
            Mat output1 = new Mat();
            Mat output2 = new Mat();

            //Création des kernels
            var kernel1 = new Mat(2, 2, MatType.CV_32F);
            kernel1.Set(0, 0, 1.0f);kernel1.Set(0, 1, 0.0f);
            kernel1.Set(1, 0, 0.0f);kernel1.Set(1, 1, -1.0f);
            var kernel2 = new Mat(2, 2, MatType.CV_32F);
            kernel2.Set(0, 0, 0f);kernel2.Set(0, 1, 1.0f);
            kernel2.Set(1, 0, -1.0f);kernel2.Set(1, 1, 0.0f);

            //Convolution par les deux kernels
            Cv2.Filter2D(v, output1, v.Depth(), kernel1, new OpenCvSharp.Point(1, 1), 0, BorderTypes.Default);
            Cv2.Filter2D(v, output2, v.Depth(), kernel2, new OpenCvSharp.Point(1, 1), 0, BorderTypes.Default);
            //Adition des deux matrices
            Mat output = output1 + output2;

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvRobertsFilter1.png", output1);
            Cv2.ImWrite(@".\CvRobertsFilter2.png", output2);
            Cv2.ImWrite(@".\CvRobertsFilter.png", output);
        }

    }
}