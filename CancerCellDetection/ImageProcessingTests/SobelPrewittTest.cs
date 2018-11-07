using System;
using System.Drawing;
using ImageProcessing;
using ImageProcessing.Detection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessing.Tests
{
    [TestClass]
    public class SobelCannyTest
    {
        [TestMethod()]
        public void ConvolveGraySobelFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            resConv.Save(@".\GraySobelFilterTest.png");
        }

        [TestMethod()]
        public void ConvolveGraySobelFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            var resInv = InverterFilter.Invert(resConv);
            resInv.Save(@".\GraySobelFilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolveSobelShapeTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\ech.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            var resInv = InverterFilter.Invert(resConv);
            resInv.Save(@".\GraySobelFilterInvertedShapeTest.png");
        }

        [TestMethod()]
        public void ConvolveGraySobelO4FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter4O());
            var resInv = InverterFilter.Invert(resConv);
            resInv.Save(@".\GraySobelO4FilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolveGrayPrewittFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new PrewittFilter());
            var resInv = InverterFilter.Invert(resConv);
            resInv.Save(@".\GrayPrewittFilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolvePrewittO4FilterShapeTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\ech.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new PrewittFilter4O());
            var resInv = InverterFilter.Invert(resConv);
            resInv.Save(@".\GrayPrewitt4OInvertedShape.png");
        }

        [TestMethod()]
        public void ConvolveGrayPrewittO4FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new PrewittFilter4O());
            var resInv = InverterFilter.Invert(resConv);
            resInv.Save(@".\GrayPrewittO4FilterInvertedTest.png");
        }
    }
}
