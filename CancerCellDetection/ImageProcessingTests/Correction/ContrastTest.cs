using System.Drawing;
using ImageProcessing.Correction;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests.Correction
{
    [TestClass]
    public class ContrastTest
    {
        [TestMethod()]
        public void ContrastCorrection50Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = ContrastCorrection.Correct(v, 50);
            res.Save(@".\ContrastCorrection50Test.png");
        }

        [TestMethod()]
        public void ContrastCorrectionminus50Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = ContrastCorrection.Correct(v, -50);
            res.Save(@".\ContrastCorrectionminus50Test.png");
        }

        [TestMethod()]
        public void ContrastCorrection50GrayTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var gray = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var res = ContrastCorrection.Correct(gray, 50);
            res.Save(@".\ContrastCorrection50GrayTest.png");
        }

        [TestMethod()]
        public void ContrastCorrectionminus50GrayTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var gray = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var res = ContrastCorrection.Correct(gray, -50);
            res.Save(@".\ContrastCorrectionminus50GrayTest.png");
        }

    }
}
