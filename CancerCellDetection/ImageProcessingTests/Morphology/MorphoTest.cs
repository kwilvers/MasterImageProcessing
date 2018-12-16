using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using ImageProcessing.Morphology;
using ImageProcessing.Smoothing;
using ImageProcessing.Thresholding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests.Morphology
{
    [TestClass]
    public class MorphoTest
    {
        [TestMethod()]
        public void DilateRoungBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            byte otsuTh = (byte) OtsuThresholding.Compute(sobl.Output);
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");
            var ero = Dilate.Apply(th, new RoundStructuredElement(), otsuTh);
            ero.Save(@".\DilateRoundBinaryGrayGaussianSobelTest.png");
        }

        [TestMethod()]
        public void ErodeRoundBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");
            var ero = Erode.Apply(th, new RoundStructuredElement(), otsuTh);
            ero.Save(@".\ErodeRoundBinaryGrayGaussianSobelTest.png");
        }

        [TestMethod()]
        public void DilateCrossBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");
            var ero = Dilate.Apply(th, new CrossStructuredElement(), otsuTh);
            ero.Save(@".\DilateCrossBinaryGrayGaussianSobelTest.png");
        }

        [TestMethod()]
        public void ErodeCrossBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");
            var ero = Erode.Apply(th, new CrossStructuredElement(), otsuTh);
            ero.Save(@".\ErodeCrossBinaryGrayGaussianSobelTest.png");
        }

        [TestMethod()]
        public void DilateSquareBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");
            var ero = Dilate.Apply(th, new SquareStructuredElement(), otsuTh);
            ero.Save(@".\DilateSquareBinaryGrayGaussianSobelTest.png");
        }

        [TestMethod()]
        public void ErodeSquareBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");
            var ero = Erode.Apply(th, new SquareStructuredElement(), otsuTh);
            ero.Save(@".\ErodeSquareBinaryGrayGaussianSobelTest.png");
        }


        [TestMethod()]
        public void OpenCrossBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");
            var ero = Erode.Apply(th, new CrossStructuredElement(), otsuTh);
            var dil = Dilate.Apply(ero, new CrossStructuredElement(), otsuTh);
            dil.Save(@".\OpenCrossBinaryGrayGaussianSobelTest.png");
        }

        [TestMethod()]
        public void CloseCrossBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");
            var dil = Dilate.Apply(th, new CrossStructuredElement(), otsuTh);
            //dil.Save(@".\DilateCrossBinaryGrayGaussianSobelTest.png");
            var ero = Erode.Apply(dil, new CrossStructuredElement(), otsuTh);
            ero.Save(@".\CloseCrossBinaryGrayGaussianSobelTest.png");
        }


        [TestMethod()]
        public void InteriorCrossBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");

            var ero = Erode.Apply(th, new CrossStructuredElement(), otsuTh);
            ero.Save(@".\ero.png");

            var inter = Morpho.Sub(th, ero);
            inter.Save(@".\InteriorCrossBinaryGrayGaussianSobelTest.png");

            var inv = InverterFilter.Invert(inter);
            inv.Save(@".\InvInteriorCrossBinaryGrayGaussianSobelTest.png");
        }


        [TestMethod()]
        public void ExteriorCrossBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");

            var dil = Dilate.Apply(th, new CrossStructuredElement(), otsuTh);
            dil.Save(@".\dil.png");

            var ext = Morpho.Sub(dil, th);
            ext.Save(@".\ExteriorCrossBinaryGrayGaussianSobelTest.png");

            var inv = InverterFilter.Invert(ext);
            inv.Save(@".\InvExteriorCrossBinaryGrayGaussianSobelTest.png");
        }



        [TestMethod()]
        public void MorphoGradientCrossBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");

            var ero = Erode.Apply(th, new CrossStructuredElement(), otsuTh);
            ero.Save(@".\ero.png");

            var dil = Dilate.Apply(th, new CrossStructuredElement(), otsuTh);
            dil.Save(@".\dil.png");

            var inter = Morpho.Sub(th, ero);
            inter.Save(@".\InteriorCrossBinaryGrayGaussianSobelTest.png");

            var ext = Morpho.Sub(dil, th);
            ext.Save(@".\ExteriorCrossBinaryGrayGaussianSobelTest.png");

            var grad = Morpho.Add(inter, ext);

            var inv = InverterFilter.Invert(grad);
            inv.Save(@".\InvMorphoGradientCrossBinaryGrayGaussianSobelTest.png");
        }


        [TestMethod()]
        public void MorphoGradientRoundBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");

            var ero = Erode.Apply(th, new RoundStructuredElement(), otsuTh);
            ero.Save(@".\ero.png");

            var dil = Dilate.Apply(th, new RoundStructuredElement(), otsuTh);
            dil.Save(@".\dil.png");

            var inter = Morpho.Sub(th, ero);
            inter.Save(@".\InteriorRoundBinaryGrayGaussianSobelTest.png");

            var ext = Morpho.Sub(dil, th);
            ext.Save(@".\ExteriorRoundBinaryGrayGaussianSobelTest.png");

            var grad = Morpho.Add(inter, ext);

            var inv = InverterFilter.Invert(grad);
            inv.Save(@".\InvMorphoGradientRoundBinaryGrayGaussianSobelTest.png");
        }

    }
}
