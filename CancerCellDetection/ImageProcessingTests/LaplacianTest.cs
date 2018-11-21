using System;
using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests
{
    [TestClass]
    public class LaplacianTest
    {
        [TestMethod()]
        public void ConvolveGrayLaplacianS3C4FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var resConv = Convolution.Convolve(res, new LaplacianS3C4Filter());
            //resConv.Save(@".\GrayLaplacianS3C4FilterInvertedTest.png");
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayLaplacianS3C4FilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolveGrayLaplacianS3C8FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var resConv = Convolution.Convolve(res, new LaplacianS3C8Filter());
            //resConv.Save(@".\GrayLaplacianS3C4FilterInvertedTest.png");
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayLaplacianS3C8FilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolveGrayLaplacianS4C4FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var resConv = Convolution.Convolve(res, new LaplacianS4C4Filter());
            //resConv.Save(@".\GrayLaplacianS3C4FilterInvertedTest.png");
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayLaplacianS4C4FilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolveGrayLaplacianOfGaussianFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var resConv = Convolution.Convolve(res, new LaplacianOfGaussianFilter());
            //resConv.Save(@".\GrayLaplacianS3C4FilterInvertedTest.png");
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayLaplacianOfGaussianFilterInvertedTest.png");
        }
    }
}
