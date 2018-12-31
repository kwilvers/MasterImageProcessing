using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace ImageProcessingTests.Detection
{
    [TestClass]
    public class KirschTest
    {
        [TestMethod()]
        public void ConvolveGrayKirschFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            //Filtre à 8 directions de Kirsch
            var resConv = Convolution.Convolve(res, new KirschFilter());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayKirschFilterInvertedTest.png");
        }

        [TestMethod]
        public void CvKirschFilter()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.Grayscale);

            //Matrice de gradient X et Y
            Mat output1 = new Mat(); Mat output2 = new Mat();
            Mat output3 = new Mat(); Mat output4 = new Mat();
            Mat abs1 = new Mat(); Mat abs2 = new Mat();
            Mat abs3 = new Mat(); Mat abs4 = new Mat();
            Mat output12 = new Mat(); Mat output34 = new Mat();
            Mat output = new Mat();

            //Creation des kernels
            var kernel1 = new float[,]{{ 5, -3, -3 },{ 5,  0, -3 },{ 5, -3, -3 }};
            var kernel2 = new float[,]{{  5,  5, -3 },{  5,  0, -3 },{ -3, -3, -3 }};
            var kernel3 = new float[,]{{  5,  5,  5 },{ -3,  0, -3 },{ -3, -3, -3 }};
            var kernel4 = new float[,]{{ -3,  5,  5 },{ -3,  0,  5 },{ -3, -3, -3 }};
            var k1 = new Mat(3, 3, MatType.CV_32F, kernel1);
            var k2 = new Mat(3, 3, MatType.CV_32F, kernel2);
            var k3 = new Mat(3, 3, MatType.CV_32F, kernel3);
            var k4 = new Mat(3, 3, MatType.CV_32F, kernel4);
            //Convolution par quatres kernels
            Cv2.Filter2D(v, output1, -1, k1);
            Cv2.Filter2D(v, output2, -1, k2);
            Cv2.Filter2D(v, output3, -1, k3);
            Cv2.Filter2D(v, output4, -1, k4);
            //Conversion en valeurs absolue 8 bits
            Cv2.ConvertScaleAbs(output1, abs1);
            Cv2.ConvertScaleAbs(output2, abs2);
            Cv2.ConvertScaleAbs(output3, abs3);
            Cv2.ConvertScaleAbs(output4, abs4);
            Cv2.AddWeighted(abs1, 0.5, abs2, 0.5, 0, output12);
            Cv2.AddWeighted(abs3, 0.5, abs4, 0.5, 0, output34);
            //Addition de quatre matrices dont le poids de chacune des matrices est identique
            Cv2.AddWeighted(output12, 0.5, output34, 0.5, 0, output);

            
            Cv2.ImWrite(@".\CvKirschFilter12.png", output12);
            Cv2.ImWrite(@".\CvKirschFilter34.png", output34);
            Cv2.ImWrite(@".\CvKirschFilter.png", output);
        }

    }
}
