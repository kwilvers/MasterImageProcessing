using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.Tests
{
    [TestClass()]
    public class HorizontalVerticalTests
    {
        [TestMethod()]
        public void ConvolveVerticalFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = Convolution.Convolve(v, new VerticalFilter());
            res.Save(@".\VerticalFilter.png");
        }

        [TestMethod()]
        public void ConvolveGrayVerticalFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new VerticalFilter());
            resConv.Save(@".\GrayVerticalFilter.png");
        }

        [TestMethod()]
        public void ConvolveGrayVerticalFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new VerticalFilter());
            var resInv = InverterFilter.Invert(resConv);
            resInv.Save(@".\GrayVerticalFilterInverted.png");
        }


        [TestMethod()]
        public void ConvolveHorizontalFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = Convolution.Convolve(v, new HorizontalFilter());
            res.Save(@".\HorizontalFilter.png");
        }

        [TestMethod()]
        public void ConvolveGrayHorizontalFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new HorizontalFilter());
            resConv.Save(@".\GrayHorizontalFilter.png");
        }

        [TestMethod()]
        public void ConvolveGrayHorizontalFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new HorizontalFilter());
            var resInv = InverterFilter.Invert(resConv);
            resInv.Save(@".\GrayHorizontalFilterInverted.png");
        }
    }
}