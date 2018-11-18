using System;
using System.Drawing;
using ImageProcessing;
using ImageProcessing.Smoothing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests
{
    [TestClass]
    public class GaussianTest
    {

        [TestMethod()]
        public void ConvolveGaussianFilter140S7Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = Convolution.Convolve(v, new GaussianFilter140S7());
            resConv.Save(@".\GaussianFilter140S7Test.png");
        }

        [TestMethod()]
        public void ConvolveGaussianFilter300S5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = Convolution.Convolve(v, new GaussianFilter300S5());
            resConv.Save(@".\GaussianFilter300S5Test.png");
        }

        [TestMethod()]
        public void ConvolveMultiGaussianFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = Convolution.Convolve(v, new GaussianFilter300S5());
            var resConv2 = Convolution.Convolve(resConv, new GaussianFilter140S7());
            var resConv3 = Convolution.Convolve(resConv2, new GaussianFilter140S7());
            var resConv4 = Convolution.Convolve(resConv3, new GaussianFilter140S7());
            resConv4.Save(@".\MultiGaussianFilterTest.png");
        }

        [TestMethod()]
        public void ConvolveGrayGaussianFilter140S7Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new GaussianFilter140S7());
            resConv.Save(@".\GaussianFilter140S7Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayGaussianFilter159S5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new GaussianFilter159S5());
            resConv.Save(@".\GaussianFilter159S5Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayGaussianFilter16S3Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new GaussianFilter16S3());
            resConv.Save(@".\GaussianFilter16S3Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayGaussianFilter273S5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new GaussianFilter273S5());
            resConv.Save(@".\GaussianFilter273S5Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayGaussianFilter300S5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new GaussianFilter300S5());
            resConv.Save(@".\GaussianFilter300S5Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayGaussianFilter52S5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new GaussianFilter52S5());
            resConv.Save(@".\GaussianFilter52S5Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayGaussianFilter98S5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            res.Save(@".\echantillonGray.png");
            var resConv = Convolution.Convolve(res, new GaussianFilter98S5());
            resConv.Save(@".\GaussianFilter98S5Test.png");
        }

    }
}
