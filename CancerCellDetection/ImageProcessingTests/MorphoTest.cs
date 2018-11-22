using System;
using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using ImageProcessing.Morphology;
using ImageProcessing.Smoothing;
using ImageProcessing.Thresholding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests
{
    [TestClass]
    public class MorphoTest
    {
        [TestMethod()]
        public void EroBinaryNonMaximaGrayGaussianSobelInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            var otsuTh = OtsuThresholding.Compute(max);
            var th = HysteresisThresholdingFilter.Apply(max, otsuTh/2, otsuTh);
            th.Save(@".\otsu.png");
            var ero = Erosion.Apply(th, otsuTh+2);
            ero.Save(@".\ero.png");
            var resInv = InverterFilter.Invert(ero);
            resInv.Save(@".\EroBinaryNonMaximaGrayGaussianSobelInvertedTest.png");
        }
    }
}
