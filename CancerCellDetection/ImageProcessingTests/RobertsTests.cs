﻿using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests
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
            resConv.Output.Save(@".\RobertsFilter.png");
        }

        [TestMethod()]
        public void ConvolveGrayRobertsFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new RobertsFilter());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\RobertsFilterInverted.png");
        }

    }
}