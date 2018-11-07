using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessing.Detection;

namespace ImageProcessing.Tests
{
    [TestClass()]
    public class RobertsTests
    {
        [TestMethod()]
        public void ConvolveGrayRobertsFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new RobertsFilter());
            resConv.Save(@".\RobertsFilter.png");
        }

        [TestMethod()]
        public void ConvolveGrayRobertsFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new RobertsFilter());
            var resInv = InverterFilter.Invert(resConv);
            resInv.Save(@".\RobertsFilterInverted.png");
        }

    }
}