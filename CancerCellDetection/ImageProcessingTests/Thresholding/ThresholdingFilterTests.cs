using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using ImageProcessing.Morphology;
using ImageProcessing.Thresholding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests.Thresholding
{
    [TestClass()]
    public class ThresholdingFilterTests
    {
        [TestMethod()]
        public void HysteresisThresholdingFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            var resThr = HysteresisThresholdingFilter.Apply(resConv.Output, 40, 80);
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\HysteresisThresholdingFilter4080Test.png");
        }

        [TestMethod()]
        public void HysteresisThresholdingFilter5080Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            var resThr = HysteresisThresholdingFilter.Apply(resConv.Output, 50, 100);
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\HysteresisThresholdingFilter50100Test.png");
        }

        [TestMethod()]
        public void BinaryThresholdingFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            var resThr = BinaryThresholdingFilter.Apply(resConv.Output, 60);
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\BinaryThresholdingFilterTest.png");
        }

        [TestMethod()]
        public void TruncatedThresholdingFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            var resThr = TruncatedThresholdingFilter.Apply(resConv.Output, 60);
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\TruncatedThresholdingFilterTest.png");
        }

        [TestMethod()]
        public void ZeroThresholdingFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            var resThr = ZeroThresholdingFilter.Apply(resConv.Output, 60, false);
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\ZeroThresholdingFilterTest.png");
        }

        [TestMethod()]
        public void ZeroThresholdingFilterMaxTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            var resThr = ZeroThresholdingFilter.Apply(resConv.Output, 60, true);
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\ZeroThresholdingFilterMaxTest.png");
        }

        [TestMethod()]
        public void OtsuThresholdingFilter()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            int th = (int) OtsuThresholding.Compute(resConv.Output);
            var resThr = HysteresisThresholdingFilter.Apply(resConv.Output, th/2, th);
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\OtsuThresholdingFilter"+th+".png");
        }

        [TestMethod]
        public void BandThresholdingTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GammaCorrection.Correct(v, 0.3);

            //Color c = Color.FromArgb(155, 110, 226);
            //Color c = Color.FromArgb(178, 125, 223);
            //Color c = Color.FromArgb(135, 98, 206);
            Color c = Color.FromArgb(200, 147, 233);
            var resThr = BandThresholdingFilter.Apply(res, c, 100, 60);
            //var resInv = InverterFilter.Invert(resThr);
            resThr.Save(@".\BandThresholdingFilterTest.png");
        }

        [TestMethod]
        public void BandThresholdingOpenTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GammaCorrection.Correct(v, 0.3);

            //Color c = Color.FromArgb(155, 110, 226);
            //Color c = Color.FromArgb(178, 125, 223);
            //Color c = Color.FromArgb(135, 98, 206);
            Color c = Color.FromArgb(200, 147, 233);
            var resThr = BandThresholdingFilter.Apply(res, c, 100, 60);
            resThr.Save(@".\10BandThresholdingOpenTest.png");
            var ero = Erode.Apply(resThr, new RoundStructuredElement(), 128);
            ero.Save(@".\20BandThresholdingOpenTest.png");
            ero = Erode.Apply(ero, new RoundStructuredElement(), 128);
            ero.Save(@".\31BandThresholdingOpenTest.png");
            //ero = Erode.Apply(ero, new RoundStructuredElement(), 128);
            //ero.Save(@".\32BandThresholdingOpenTest.png");
            var dil = Dilate.Apply(ero, new RoundStructuredElement(), 128);
            dil.Save(@".\40BandThresholdingOpenTest.png");
            dil = Dilate.Apply(dil, new RoundStructuredElement(), 128);
            dil.Save(@".\51BandThresholdingOpenTest.png");
            dil = Dilate.Apply(dil, new CrossStructuredElement(), 128);
            dil.Save(@".\52BandThresholdingOpenTest.png");
            var sub = Morpho.Intersec(v, dil);
            sub.Save(@".\60BandThresholdingOpenSubTest.png");
        }
    }
}