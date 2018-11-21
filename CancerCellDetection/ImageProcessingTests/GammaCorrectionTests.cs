using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests
{
    [TestClass()]
    public class GammaCorrectionTests
    {
        [TestMethod()]
        public void Convert03Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GammaCorrection.Correct(v, 0.3);
            res.Save(@".\GammaCorrection03Tests.png");
        }

        [TestMethod()]
        public void Convert24Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GammaCorrection.Correct(v, 2.4);
            res.Save(@".\GammaCorrection24Tests.png");
        }

        [TestMethod()]
        public void GammaCorrectionGray03Tests()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resInv = GammaCorrection.Correct(res, 0.3);
            resInv.Save(@".\GammaCorrectionGray03Tests.png");
        }

        [TestMethod()]
        public void GammaCorrectionGray24Tests()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resInv = GammaCorrection.Correct(res, 2.4);
            resInv.Save(@".\GammaCorrectionGray24Tests.png");
        }
    }
}