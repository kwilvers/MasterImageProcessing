using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.Tests
{
    [TestClass()]
    public class ColorHSBTests
    {
        [TestMethod()]
        public void FromRGBTest()
        {
            ColorHSB c = ColorHSB.FromRGB(200,147,233);

            Assert.AreEqual(277, c.H);
            Assert.AreEqual(37, c.S);
            Assert.AreEqual(91, c.B);
        }
    }
}