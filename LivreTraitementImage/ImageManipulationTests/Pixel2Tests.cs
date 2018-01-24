using System;
using System.Drawing;
using ImageManipulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageManipulationTests
{
    [TestClass()]
    public class Pixel2Tests
    {
        [TestMethod()]
        public void ToIntTest()
        {
            Pixel p = new Pixel(0, 255, 0, 0);
            Int32 i = p.ToInt();
            Int32 expected = 16711680;
            Assert.AreEqual(expected, i‬);
        }

        [TestMethod()]
        public void ToArgbBytesTest()
        {
            Pixel p = new Pixel(Color.FromArgb(0,255,0,0));
            byte[] b = p.ToArgbBytes();
            var actual = b[0];
            Assert.AreEqual(0, actual);
            actual = b[1];
            Assert.AreEqual(255, actual);
            actual = b[2];
            Assert.AreEqual(0, actual);
            actual = b[3];
            Assert.AreEqual(0, actual);
        }

        [TestMethod()]
        public void ToColorTest()
        {
            Assert.Fail();
        }
    }
}