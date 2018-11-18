using System;
using System.Drawing;
using ImageProcessing;
using ImageProcessing.Detection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests
{
    [TestClass]
    public class KirschTest
    {
        [TestMethod()]
        public void ConvolveGrayKirschFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new KirschFilter());
            var resInv = InverterFilter.Invert(resConv);
            resInv.Save(@".\GrayKirschFilterInvertedTest.png");
        }
    }
}
