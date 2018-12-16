using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using ImageProcessing.Smoothing;
using ImageProcessing.Thresholding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests.Detection
{
    [TestClass]
    public class NonMaximumSuppressionTest
    {
        [TestMethod()]
        public void NonMaximaGrayGaussianSobelInvertedTest()
        {
            //Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\ech.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            gaus.Output.Save(@".\1gaustest.png");
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            sobl.Output.Save(@".\2gausSobeltest.png");
            var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            max.Save(@".\3NonMaximaGrayGaussianSobelTest.png");
            int th = (int)OtsuThresholding.Compute(max);
            var resThr = HysteresisThresholdingFilter.Apply(max, th / 2, th);
            resThr.Save(@".\4NonMaximaGrayGaussianSobelHisTest.png");
            var resBin = BinaryThresholdingFilter.Apply(max, th);
            resBin.Save(@".\9NonMaximaGrayGaussianSobelBinTest.png");
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\5NonMaximaGrayGaussianSobelHisInvertedTest.png");
        }
    }
}
