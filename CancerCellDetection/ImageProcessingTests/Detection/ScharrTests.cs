using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace ImageProcessingTests.Detection
{
    [TestClass()]
    public class ScharrTests
    {
        [TestMethod()]
        public void ConvolveGrayScharrFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            //Filtre de Scharr
            var resConv = Convolution.Convolve(res, new ScharrFilter());
            resConv.Output.Save(@".\GrayScharrFilter.png");
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayScharrFilterInverted.png");
        }

        [TestMethod()]
        public void ConvolveGrayScharrS5FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var resConv = Convolution.Convolve(res, new ScharrS5Filter());
            resConv.Output.Save(@".\GrayScharrS5Filter.png");
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayScharrS5FilterInverted.png");
        }

        [TestMethod()]
        public void ConvolveGrayScharrLightFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var resConv = Convolution.Convolve(res, new ScharrLightFilter());
            resConv.Output.Save(@".\GrayScharrLightFilter.png");
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayScharrLightFilterInverted.png");
        }

        [TestMethod]
        public void CvScharrFilter()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.Grayscale);

            //Matrice de gradient X et Y
            Mat outputX = new Mat(); Mat outputY = new Mat();
            Mat absX = new Mat(); Mat absY = new Mat();
            Mat output = new Mat();
            //Calcul des gradient X et Y
            Cv2.Scharr(v, outputX, v.Depth(), 1, 0);
            Cv2.ConvertScaleAbs(outputX, absX);
            Cv2.Scharr(v, outputY, v.Depth(), 0, 1);
            Cv2.ConvertScaleAbs(outputY, absY);
            Cv2.AddWeighted(absX, 0.5, absY, 0.5, 0, output);

            Cv2.ImWrite(@".\CvScharrFilterX.png", outputX);
            Cv2.ImWrite(@".\CvScharrFilterY.png", outputY);
            Cv2.ImWrite(@".\CvScharrFilter.png", output);
        }
    }
}