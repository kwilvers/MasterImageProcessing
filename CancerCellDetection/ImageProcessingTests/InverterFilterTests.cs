using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests
{
    [TestClass()]
    public class InverterFilterTests
    {
        [TestMethod()]
        public void InvertTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = InverterFilter.Invert(v);
            res.Save(@".\InvertTest.png");
        }

        [TestMethod()]
        public void InvertGrayScaleTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resInv = InverterFilter.Invert(res);
            resInv.Save(@".\InvertGrayScaleTest.png");
        }
    }
}