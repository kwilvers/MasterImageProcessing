using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessing.Correction;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessing.Thresholding;
using OpenCvSharp;

namespace ImageProcessing.Correction.Tests
{
    [TestClass()]
    public class ColorIsolationTests
    {
        [TestMethod()]
        public void IsolateBlueRedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = ColorIsolation.Isolate(v, false, true, false);
            res.Save(@".\IsolateBlueRedTest.png");
        }

        [TestMethod()]
        public void IsolateGreenRedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = ColorIsolation.Isolate(v, false, false, true);
            res.Save(@".\IsolateGreenRedTest.png");
        }
        [TestMethod()]
        public void IsolateGreenBlueTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = ColorIsolation.Isolate(v, true, false, false);
            res.Save(@".\IsolateGreenBlueTest.png");
        }

        [TestMethod()]
        public void IsolateGreenTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = ColorIsolation.Isolate(v, true, false, true);
            res.Save(@".\IsolateGreenTest.png");
        }

        [TestMethod()]
        public void IsolateRedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = ColorIsolation.Isolate(v, false, true, true);
            res.Save(@".\IsolateRedTest.png");
        }

        [TestMethod()]
        public void IsolateBlueTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = ColorIsolation.Isolate(v, true, true, false);
            res.Save(@".\IsolateBlueTest.png");
        }

        [TestMethod()]
        public void IsolateRedThresoldTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = ColorIsolation.Isolate(v, false, true, true);
            var th = ZeroThresholdingFilter.Apply(res, 160, true);
            th.Save(@".\IsolateRedThresoldTest.png");
        }

        [TestMethod()]
        public void IsolateGreenThresoldTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = ColorIsolation.Isolate(v, true, false, true);
            var th = ZeroThresholdingFilter.Apply(res, 160, true);
            th.Save(@".\IsolateGreenThresoldTest.png");
        }

        [TestMethod()]
        public void IsolateBlueRedThresoldTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = ColorIsolation.Isolate(v, false, true, false);
            var th = ZeroThresholdingFilter.Apply(res, 160, true);
            th.Save(@".\IsolateBlueRedThresoldTest.png");
        }

        [TestMethod()]
        public void IsolateBlueThresoldTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = ColorIsolation.Isolate(v, true, true, false);
            var th = ZeroThresholdingFilter.Apply(res, 160, true);
            th.Save(@".\IsolateBlueThresoldTest.png");
        }

        [TestMethod()]
        public void CVIsolateBlueRedThresoldTest()
        {
            Mat v = Cv2.ImRead(@".\echantillon.png");
            var res = ColorIsolation.Isolate(v, false, true, false);
            res = ZeroThresholdingFilter.Apply(res, 160, true);
            Cv2.ImWrite(@".\CVIsolateBlueRedTest.png", res);
        }
    }
}