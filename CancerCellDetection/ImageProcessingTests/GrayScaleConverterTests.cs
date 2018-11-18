using System;
using System.Diagnostics;
using System.Drawing;
using ImageProcessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests
{
    [TestClass()]
    public class GrayScaleConverterTests
    {

        [TestMethod()]
        public void ColorAverageTest()
        {
            var sw = Stopwatch.StartNew();
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            Console.WriteLine(sw.Elapsed);
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            Console.WriteLine(sw.Elapsed);
            res.Save(@".\ColorAverageTest.png");
            Console.WriteLine(sw.Elapsed);
            sw.Stop();
        }

        [TestMethod()]
        public void Bt709Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            res.Save(@".\Bt709.png");
        }

        [TestMethod()]
        public void FromRedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromRed);
            res.Save(@".\FromRed.png");
        }

        [TestMethod()]
        public void FromGreenTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromGreen);
            res.Save(@".\FromGreen.png");
        }

        [TestMethod()]
        public void FromBlueTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromBlue);
            res.Save(@".\FromBlue.png");
        }

        [TestMethod()]
        public void FromBlueAndGreenTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromBlueAndGreen);
            res.Save(@".\FromBlueAndGreen.png");
        }

        [TestMethod()]
        public void FromRedAndBlueTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromRedAndBlue);
            res.Save(@".\FromRedAndBlue.png");
        }

        [TestMethod()]
        public void FromRedAndGreenTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromRedAndGreen);
            res.Save(@".\FromRedAndGreen.png");
        }


        [TestMethod()]
        public void FromBrightnessTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromBrightness);
            res.Save(@".\FromBrightness.png");
        }

        [TestMethod()]
        public void FromUChrominanceTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromUChrominance);
            res.Save(@".\FromUChrominance.png");
        }

        [TestMethod()]
        public void FromVChrominanceTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromVChrominance);
            res.Save(@".\FromVChrominance.png");
        }
    }
}