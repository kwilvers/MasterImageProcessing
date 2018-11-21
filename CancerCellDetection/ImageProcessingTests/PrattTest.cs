using System;
using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests
{
    [TestClass]
    public class PrattTest
    {
        [TestMethod()]
        public void ConvolvePratt51FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
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
    }
}
