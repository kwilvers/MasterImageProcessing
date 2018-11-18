using System;
using System.Drawing;
using ImageProcessing;
using ImageProcessing.Detection;
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
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new RobinsonFilter());
            resConv.Save(@".\GrayRobinsonFilterTest.png");
            var resInv = InverterFilter.Invert(resConv);
            resInv.Save(@".\GrayRobinsonFilterInvertedTest.png");
        }
    }
}
