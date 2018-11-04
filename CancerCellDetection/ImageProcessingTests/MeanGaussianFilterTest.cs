using System;
using System.Drawing;
using ImageProcessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessing.Tests
{
    [TestClass]
    public class MeanFilterTest
    {
        [TestMethod()]
        public void ConvolveMeanFilterC4S3Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = Convolution.Convolve(v, new MeanFilterC4S3());
            resConv.Save(@".\MeanFilterC4S3Test.png");
        }

        [TestMethod()]
        public void ConvolveMeanFilterC8S3Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = Convolution.Convolve(v, new MeanFilterC8S3());
            resConv.Save(@".\MeanFilterC8S3Test.png");
        }

        [TestMethod()]
        public void ConvolveMeanFilterC24S5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = Convolution.Convolve(v, new MeanFilterC24S5());
            resConv.Save(@".\MeanFilterC24S5Test.png");
        }


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
            resConv4.Save(@".\GaussianFilter300S5Test.png");
        }
    }
}
