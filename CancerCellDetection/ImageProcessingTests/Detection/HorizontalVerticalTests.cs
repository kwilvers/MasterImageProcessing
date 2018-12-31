using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using Point = OpenCvSharp.Point;

namespace ImageProcessingTests.Detection
{
    [TestClass()]
    public class HorizontalVerticalTests
    {
        [TestMethod()]
        public void ConvolveVerticalFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            //Filtre verticale
            var res = Convolution.Convolve(v, new VerticalFilter());
            res.Output.Save(@".\VerticalFilter.png");
        }

        [TestMethod()]
        public void ConvolveGrayVerticalFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new VerticalFilter());
            resConv.Output.Save(@".\GrayVerticalFilter.png");
        }

        [TestMethod()]
        public void ConvolveGrayVerticalFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new VerticalFilter());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayVerticalFilterInverted.png");
        }


        [TestMethod()]
        public void ConvolveHorizontalFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = Convolution.Convolve(v, new HorizontalFilter());
            res.Output.Save(@".\HorizontalFilter.png");
        }

        [TestMethod()]
        public void ConvolveGrayHorizontalFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new HorizontalFilter());
            resConv.Output.Save(@".\GrayHorizontalFilter.png");
        }

        [TestMethod()]
        public void ConvolveGrayHorizontalFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new HorizontalFilter());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayHorizontalFilterInverted.png");
        }

        [TestMethod]
        public void CvHorizontalFilter()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");
            Mat output = new Mat();
            //Filtre horizontale
            Mat kernelH = new Mat(1, 3, MatType.CV_32F);
            kernelH.Set(0,0,1.0f);
            kernelH.Set(0, 1,0.0f);
            kernelH.Set(0, 2,-1.0f);
            Cv2.Filter2D(v, output, v.Depth(), kernelH, new Point(-1, -1), 0, BorderTypes.Default);


            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvHorizontalFilter.png", output);
        }

        [TestMethod]
        public void CvVerticalFilter()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");
            Mat output = new Mat();
            //Filtre vertical
            Mat kernelH = new Mat(3, 1, MatType.CV_32F);
            kernelH.Set(0, 0,  1.0f);
            kernelH.Set(1, 0,  0.0f);
            kernelH.Set(2, 0, -1.0f);
            Cv2.Filter2D(v, output, v.Depth(), kernelH, new Point(-1, -1), 0, BorderTypes.Default);


            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvVerticalFilter.png", output);
        }
    }
}