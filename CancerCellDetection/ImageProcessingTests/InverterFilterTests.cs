using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.Tests
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