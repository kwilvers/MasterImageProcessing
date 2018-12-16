using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests.Detection
{
    [TestClass]
    public class SobelTest
    {
        [TestMethod()]
        public void ConvolveGraySobelFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            resConv.Output.Save(@".\GraySobelFilterTest.png");
        }

        [TestMethod()]
        public void ConvolveGraySobelFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GraySobelFilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolveSobelShapeTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\ech.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GraySobelFilterInvertedShapeTest.png");
        }

        [TestMethod()]
        public void ConvolveGraySobelO4FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter4O());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GraySobelO4FilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolveColorSobelO4FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            //var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(v, new SobelFilter4O());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\ColorSobelO4FilterInvertedTest.png");
        }
    }
}
