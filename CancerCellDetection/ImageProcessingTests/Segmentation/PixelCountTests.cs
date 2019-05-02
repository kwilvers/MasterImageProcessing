using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Morphology;
using ImageProcessing.Segmentation;
using ImageProcessing.Smoothing;
using ImageProcessing.Thresholding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace ImageProcessingTests.Segmentation
{
    [TestClass()]
    public class PixelCountTests
    {
        [TestMethod]
        public void PixelCountBlack()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GammaCorrection.Correct(v, 0.3);

            Color c = Color.FromArgb(200, 147, 233);
            var resThr = BandThresholdingFilter.Apply(res, c, 100, 60);
            var ero = Erode.Apply(resThr, new RoundStructuredElement(), 128);
            ero = Erode.Apply(ero, new RoundStructuredElement(), 128);
            var dil = Dilate.Apply(ero, new RoundStructuredElement(), 128);
            dil = Dilate.Apply(dil, new RoundStructuredElement(), 128);
            dil = Dilate.Apply(dil, new CrossStructuredElement(), 128);
            var sub = Morpho.Intersec(v, dil);

            double count = PixelCount.Count(sub);
            double black = PixelCount.Count(sub, Color.Black);
            double rest = count - black;

            double bp = black / count * 100;
            double rp = rest / count * 100;
        }

        [TestMethod]
        public void PixelCountBlack100()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\bw.png");

            double count = PixelCount.Count(v);
            double black = PixelCount.Count(v, Color.Black);
            double rest = count - black;

            double bp = black / count * 100;
            double rp = rest / count * 100;
        }

        [TestMethod]
        public void PixelCountCV()
        {
            Mat v = Cv2.ImRead(@".\bw.png");

            var cnt = PixelCount.Count(v, Scalar.Black);
            Assert.AreEqual(100, cnt);
        }
    }
}