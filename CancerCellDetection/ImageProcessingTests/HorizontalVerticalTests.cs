using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests
{
    [TestClass()]
    public class HorizontalVerticalTests
    {
        [TestMethod()]
        public void ConvolveVerticalFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = Convolution.Convolve(v, new VerticalFilter());
            res.Output.Save(@".\VerticalFilter.png");
        }

        [TestMethod()]
        public void ConvolveGrayVerticalFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new VerticalFilter());
            resConv.Output.Save(@".\GrayVerticalFilter.png");
        }

        [TestMethod()]
        public void ConvolveGrayVerticalFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new VerticalFilter());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayVerticalFilterInverted.png");
        }


        [TestMethod()]
        public void ConvolveHorizontalFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = Convolution.Convolve(v, new HorizontalFilter());
            res.Output.Save(@".\HorizontalFilter.png");
        }

        [TestMethod()]
        public void ConvolveGrayHorizontalFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new HorizontalFilter());
            resConv.Output.Save(@".\GrayHorizontalFilter.png");
        }

        [TestMethod()]
        public void ConvolveGrayHorizontalFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new HorizontalFilter());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayHorizontalFilterInverted.png");
        }
    }
}