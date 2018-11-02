using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.Tests
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
        public void GammaCorrectionGrey03Tests()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resInv = GammaCorrection.Correct(res, 0.3);
            resInv.Save(@".\GammaCorrectionGrey03Tests.png");
        }

        [TestMethod()]
        public void GammaCorrectionGrey24Tests()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resInv = GammaCorrection.Correct(res, 2.4);
            resInv.Save(@".\GammaCorrectionGrey24Tests.png");
        }
    }
}