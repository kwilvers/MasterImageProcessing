﻿using System.Drawing;
using ImageProcessing;
using ImageProcessing.Detection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests
{
    [TestClass()]
    public class ScharrTests
    {
        [TestMethod()]
        public void ConvolveGrayScharrFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new ScharrFilter());
            var resInv = InverterFilter.Invert(resConv);
            resInv.Save(@".\GrayScharrFilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolveGrayScharrS5FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new ScharrS5Filter());
            var resInv = InverterFilter.Invert(resConv);
            resInv.Save(@".\GrayScharrS5FilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolveGrayScharrLightFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new ScharrLightFilter());
            var resInv = InverterFilter.Invert(resConv);
            resInv.Save(@".\GrayScharrLightFilterInvertedTest.png");
        }
    }
}