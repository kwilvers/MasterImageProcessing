using System;
using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Smoothing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace ImageProcessingTests.Smoothing
{
    [TestClass]
    public class GaussianTest
    {

        [TestMethod()]
        public void ConvolveGaussianFilter140S7Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = Convolution.Convolve(v, new GaussianFilter140S7());
            resConv.Output.Save(@".\GaussianFilter140S7Test.png");
        }

        [TestMethod()]
        public void ConvolveGaussianFilter300S5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = Convolution.Convolve(v, new GaussianFilter300S5());
            resConv.Output.Save(@".\GaussianFilter300S5Test.png");
        }

        [TestMethod()]
        public void ConvolveMultiGaussianFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = Convolution.Convolve(v, new GaussianFilter300S5());
            var resConv2 = Convolution.Convolve(resConv.Output, new GaussianFilter140S7());
            var resConv3 = Convolution.Convolve(resConv2.Output, new GaussianFilter140S7());
            var resConv4 = Convolution.Convolve(resConv3.Output, new GaussianFilter140S7());
            resConv4.Output.Save(@".\MultiGaussianFilterTest.png");
        }

        [TestMethod()]
        public void ConvolveGrayGaussianFilter140S7Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new GaussianFilter140S7());
            resConv.Output.Save(@".\GaussianFilter140S7Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayGaussianFilter159S5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new GaussianFilter159S5());
            resConv.Output.Save(@".\GaussianFilter159S5Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayGaussianFilter16S3Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new GaussianFilter16S3());
            resConv.Output.Save(@".\GaussianFilter16S3Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayGaussianFilter273S5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new GaussianFilter273S5());
            resConv.Output.Save(@".\GaussianFilter273S5Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayGaussianFilter300S5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new GaussianFilter300S5());
            resConv.Output.Save(@".\GaussianFilter300S5Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayGaussianFilter52S5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new GaussianFilter52S5());
            resConv.Output.Save(@".\GaussianFilter52S5Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayGaussianFilter98S5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            res.Save(@".\echantillonGray.png");
            var resConv = Convolution.Convolve(res, new GaussianFilter98S5());
            resConv.Output.Save(@".\GaussianFilter98S5Test.png");
        }

        [TestMethod()]
        public void GrayGaussianFilterCalculatedS7W15Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            res.Save(@".\echantillonGray.png");
            var resConv = Convolution.Convolve(res, new GaussianFilter(7, 1.5));
            resConv.Output.Save(@".\GrayGaussianFilterCalculatedS7W15Test.png");
        }

        [TestMethod()]
        public void GrayGaussianFilterCalculatedS7W1Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            res.Save(@".\echantillonGray.png");
            var resConv = Convolution.Convolve(res, new GaussianFilter(7, 1));
            resConv.Output.Save(@".\GrayGaussianFilterCalculatedS7W1Test.png");
        }

        [TestMethod()]
        public void GrayGaussianFilterCalculatedS7W5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            res.Save(@".\echantillonGray.png");
            var resConv = Convolution.Convolve(res, new GaussianFilter(7, 5));
            resConv.Output.Save(@".\GrayGaussianFilterCalculatedS7W5Test.png");
        }

        [TestMethod()]
        public void GrayGaussianFilterCalculatedS11W15Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            res.Save(@".\echantillonGray.png");
            var resConv = Convolution.Convolve(res, new GaussianFilter(11, 1.5));
            resConv.Output.Save(@".\GrayGaussianFilterCalculatedS11W15Test.png");
        }

        [TestMethod()]
        public void GrayGaussianFilterCalculatedS11W5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            res.Save(@".\echantillonGray.png");
            var resConv = Convolution.Convolve(res, new GaussianFilter(11, 5));
            resConv.Output.Save(@".\GrayGaussianFilterCalculatedS11W5Test.png");
        }

        [TestMethod()]
        public void GrayGaussianFilterCalculatedS25W25Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            res.Save(@".\echantillonGray.png");
            var f = new GaussianFilter(25, 5);
            Console.WriteLine(f.Kernels[0].ToString());
            //var resConv = Convolution.Convolve(res, f);
            //resConv.Output.Save(@".\GrayGaussianFilterCalculatedS11W5Test.png");
        }

        [TestMethod()]
        public void GrayContrastGaussianFilterCalculatedS11W5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var gray = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            gray.Save(@".\echantillonGray.png");
            var res = ContrastCorrection.Correct(gray, 50);
            res.Save(@".\GrayContrastFilterTest.png");
            //Filtre gaussien 11x11 Sigma 5
            var resConv = Convolution.Convolve(res, new GaussianFilter(11, 5));
            resConv.Output.Save(@".\GrayContrastGaussianFilterCalculatedS11W5Test.png");
        }

        [TestMethod]
        public void CvGaussianS9Filter()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");
            Mat output = new Mat();
            //Filtre gaussien 9x9
            Cv2.GaussianBlur(v, output, new OpenCvSharp.Size(9, 9), 0, 0, BorderTypes.Default);
            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvGaussianS9Filter.png", output);
        }

        [TestMethod]
        public void CvGaussianS11O5Filter()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");
            Mat output = new Mat();
            //Filtre gaussien 9x9 Sigma 5
            Cv2.GaussianBlur(v, output, new OpenCvSharp.Size(9, 9), 5, 5, BorderTypes.Default);
            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvGaussianS11O5Filter.png", output);
        }
    }
}
