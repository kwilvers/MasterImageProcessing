using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests.Detection
{
    [TestClass]
    public class PrewittTest
    {
        [TestMethod()]
        public void ConvolveGrayPrewittFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new PrewittFilter());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayPrewittFilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolvePrewittO4FilterShapeTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\ech.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new PrewittFilter4O());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayPrewitt4OInvertedShape.png");
        }

        [TestMethod()]
        public void ConvolveGrayPrewittO4FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new PrewittFilter4O());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayPrewittO4FilterInvertedTest.png");
        }
    }
}
