using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace ImageProcessingTests.Detection
{
    [TestClass]
    public class PrattTest
    {
        [TestMethod()]
        public void ConvolvePratt51FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            //Filtre de Pratt
            var resConv = Convolution.Convolve(v, new Pratt51Filter());
            resConv.Output.Save(@".\Pratt51FilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolvePratt52FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = Convolution.Convolve(v, new Pratt52Filter());
            resConv.Output.Save(@".\Pratt52FilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolvePratt91FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = Convolution.Convolve(v, new Pratt91Filter());
            resConv.Output.Save(@".\Pratt91FilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolvePratt93FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = Convolution.Convolve(v, new Pratt93Filter());
            resConv.Output.Save(@".\Pratt93FilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolvePratt274FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = Convolution.Convolve(v, new Pratt274Filter());
            resConv.Output.Save(@".\Pratt274FilterInvertedTest.png");
        }


        [TestMethod()]
        public void ConvolveGrayPratt51FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new Pratt51Filter());
            resConv.Output.Save(@".\GrayPratt51FilterTest.png");
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayPratt51FilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolveGrayPratt52FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new Pratt52Filter());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayPratt52FilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolveGrayPratt91FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new Pratt91Filter());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayPratt91FilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolveGrayPratt93FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new Pratt93Filter());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayPratt93FilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolveGrayPratt274FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var resConv = Convolution.Convolve(res, new Pratt274Filter());
            resConv.Output.Save(@".\GrayPratt274FilterTest.png");
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayPratt274FilterInvertedTest.png");
        }


        [TestMethod]
        public void CvPrattFilter()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");

            //Matrice de gradient X et Y
            Mat abs1 = new Mat(); 
            Mat output = new Mat();

            //Creation des kernels
            //var kernel1 = new float[,] {{-1, -4, -1}, {-4, 27, -4}, {-1, -4, -1}};
            //var kernel1 = new float[,] {{0, -3, 0}, {-3, 12, -3}, {0, -3, 0}};
            //var kernel1 = new float[,]{{  0, -3,  0 },{ -3,  14, -3 },{  0, -3,  0 }};
            //var kernel1 = new float[,]{{  0, -1,  0 },{ -1,  9, -1 },{  0, -1,  0 }};

            //Création du kernel
            var kernel1 = new float[,] {{0, -1, 0}, {-1, 5, -1}, {0, -1, 0}};
            var k1 = new Mat(3, 3, MatType.CV_32F, kernel1);
            //Convolution par kernel
            Cv2.Filter2D(v, output, -1, k1);
            
            //Conversion en valeurs absolue 8 bits
            //Cv2.ConvertScaleAbs(output, abs1);
            
            Cv2.ImWrite(@".\CvPrattFilter.png", output);
        }

    }
}
