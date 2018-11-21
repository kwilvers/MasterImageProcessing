using System;
using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using ImageProcessing.Smoothing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests
{
    [TestClass]
    public class NonMaximumSuppressionTest
    {
        [TestMethod()]
        public void NonMaximaGrayGaussianSobelInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            var resInv = InverterFilter.Invert(max);
            resInv.Save(@".\NonMaximaGrayGaussianSobelInvertedTest.png");
        }
    }
}
