using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Smoothing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests
{
    [TestClass]
    public class MeanFilterTest
    {
        [TestMethod()]
        public void ConvolveMeanFilterC4S3Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = Convolution.Convolve(v, new MeanFilterC4S3());
            resConv.Output.Save(@".\MeanFilterC4S3Test.png");
        }

        [TestMethod()]
        public void ConvolveMeanFilterC8S3Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = Convolution.Convolve(v, new MeanFilterC8S3());
            resConv.Output.Save(@".\MeanFilterC8S3Test.png");
        }

        [TestMethod()]
        public void ConvolveMeanFilterC24S5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = Convolution.Convolve(v, new MeanFilterC24S5());
            resConv.Output.Save(@".\MeanFilterC24S5Test.png");
        }

        [TestMethod()]
        public void ConvolveMeanFilterC48S7Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = Convolution.Convolve(v, new MeanFilterC48S7());
            resConv.Output.Save(@".\MeanFilterC48S7Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayMeanFilterC4S3Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new MeanFilterC4S3());
            resConv.Output.Save(@".\GrayMeanFilterC4S3Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayMeanFilterC8S3Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new MeanFilterC8S3());
            resConv.Output.Save(@".\GrayMeanFilterC8S3Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayMeanFilterC24S5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new MeanFilterC24S5());
            resConv.Output.Save(@".\GrayMeanFilterC24S5Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayMeanFilterC48S7Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new MeanFilterC48S7());
            resConv.Output.Save(@".\GrayMeanFilterC48S7Test.png");
        }
    }
}
