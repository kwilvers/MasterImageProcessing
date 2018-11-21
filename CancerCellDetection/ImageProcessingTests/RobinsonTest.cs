using System;
using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using ImageProcessing.Thresholding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests
{
    [TestClass]
    public class RobinsonTest
    {
        [TestMethod()]
        public void ConvolveGrayRobinsonFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var resConv = Convolution.Convolve(res, new RobinsonFilter());
            resConv.Output.Save(@".\GrayRobinsonFilterTest.png");
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayRobinsonFilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolveGrayRobinsonFilterInvertedThTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var resConv = Convolution.Convolve(res, new RobinsonFilter());
            resConv.Output.Save(@".\GrayRobinsonFilterTest.png");
            int th = (int)OtsuThresholding.Compute(resConv.Output);
            var resTh = HysteresisThresholdingFilter.Apply(resConv.Output, th/2, th);
            resTh.Save(@".\GrayRobinsonFilterInvertedThTest.png");
            var resInv = InverterFilter.Invert(resTh);
            resInv.Save(@".\GrayRobinsonFilterInvertedTest.png");
        }
    }
}
