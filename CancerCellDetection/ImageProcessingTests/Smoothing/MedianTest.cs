using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Smoothing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace ImageProcessingTests.Smoothing
{
    [TestClass]
    public class MedianTest
    {
        [TestMethod()]
        public void ConvolveGrayMedianFilterS3Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = ConvolutionMedian.Convolve(res, new MedianFilterS3());
            resConv.Save(@".\GrayMedianFilterS3Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayMedianFilterS5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = ConvolutionMedian.Convolve(res, new MedianFilterS5());
            resConv.Save(@".\GrayMedianFilterS5Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayMedianFilterS7Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = ConvolutionMedian.Convolve(res, new MedianFilterS7());
            resConv.Save(@".\GrayMedianFilterS7Test.png");
        }

        [TestMethod()]
        public void ConvolveMedianFilterS3Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = ConvolutionMedian.Convolve(v, new MedianFilterS3());
            resConv.Save(@".\MedianFilterS3Test.png");
        }

        [TestMethod()]
        public void ConvolveMedianFilterS5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = ConvolutionMedian.Convolve(v, new MedianFilterS5());
            resConv.Save(@".\MedianFilterS5Test.png");
        }

        [TestMethod()]
        public void ConvolveMedianFilterS7Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = ConvolutionMedian.Convolve(v, new MedianFilterS7());
            resConv.Save(@".\MedianFilterS7Test.png");
        }

        [TestMethod]
        public void CvMedianS9Filter()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");
            Mat output = new Mat();
            //Filtre median 5x5
            Cv2.MedianBlur(v, output, 5);
            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvMedianS9Filter.png", output);
        }
    }
}
