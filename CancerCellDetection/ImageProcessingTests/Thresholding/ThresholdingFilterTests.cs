using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessing.Thresholding;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessing.Detection;

namespace ImageProcessing.Thresholding.Tests
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
            var resThr = HysteresisThresholdingFilter.Apply(resConv, 30, 100);
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\HysteresisThresholdingFilterTest.png");
        }

        [TestMethod()]
        public void HysteresisThresholdingFilter5080Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            var resThr = HysteresisThresholdingFilter.Apply(resConv, 30, 150);
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\HysteresisThresholdingFilter5080Test.png");
        }

        [TestMethod()]
        public void BinaryThresholdingFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            var resThr = BinaryThresholdingFilter.Apply(resConv, 100);
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\BinaryThresholdingFilterTest.png");
        }

        [TestMethod()]
        public void TruncatedThresholdingFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            var resThr = TruncatedThresholdingFilter.Apply(resConv, 100);
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\TruncatedThresholdingFilterTest.png");
        }

        [TestMethod()]
        public void ZeroThresholdingFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            var resThr = ZeroThresholdingFilter.Apply(resConv, 100, false);
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\ZeroThresholdingFilterTest.png");
        }

        [TestMethod()]
        public void ZeroThresholdingFilterMaxTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            var resThr = ZeroThresholdingFilter.Apply(resConv, 100, true);
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\ZeroThresholdingFilterMaxTest.png");
        }

        [TestMethod()]
        public void OtsuThresholdingFilter()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            resConv.Save(@".\OtsuThresholdingFilter1.png");
            int th = (int) OtsuThresholding.Compute(resConv);
            var resThr = HysteresisThresholdingFilter.Apply(resConv, th/2, th);
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\OtsuThresholdingFilter.png");
        }

    }
}