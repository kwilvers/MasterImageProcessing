using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var resConv = Convolution.Convolve(res, new KirschFilter());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayKirschFilterInvertedTest.png");
        }
    }
}
