using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace ImageProcessingTests.Detection
{
    [TestClass]
    public class LaplacianTest
    {
        [TestMethod()]
        public void ConvolveGrayLaplacianS3C4FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            //Filtre laplacien
            var resConv = Convolution.Convolve(res, new LaplacianS3C4Filter());
            //resConv.Save(@".\GrayLaplacianS3C4FilterInvertedTest.png");
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayLaplacianS3C4FilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolveGrayLaplacianS3C8FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var resConv = Convolution.Convolve(res, new LaplacianS3C8Filter());
            //resConv.Save(@".\GrayLaplacianS3C4FilterInvertedTest.png");
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayLaplacianS3C8FilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolveGrayLaplacianS4C4FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var resConv = Convolution.Convolve(res, new LaplacianS4C4Filter());
            //resConv.Save(@".\GrayLaplacianS3C4FilterInvertedTest.png");
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayLaplacianS4C4FilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolveGrayLaplacianOfGaussianFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var resConv = Convolution.Convolve(res, new LaplacianOfGaussianFilter());
            //resConv.Save(@".\GrayLaplacianS3C4FilterInvertedTest.png");
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GrayLaplacianOfGaussianFilterInvertedTest.png");
        }

        [TestMethod]
        public void CvLaplacianFilter()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.Grayscale);

            //Matrice de gradient X et Y
            Mat output = new Mat();

            //Convolution par kernel
            Cv2.Laplacian(v, output, v.Depth(), 3, 5);

            Cv2.ImWrite(@".\CvLaplacianFilter.png", output);
        }

        [TestMethod]
        public void CvLaplacianOfGaussianFilter()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.Grayscale);

            //Matrice de gradient X et Y
            Mat blur = new Mat();
            Mat output = new Mat();

            //Gaussian blur
            Cv2.GaussianBlur(v, blur, new OpenCvSharp.Size(7, 7), 5, 5, BorderTypes.Default);
            //Convolution par kernel
            Cv2.Laplacian(blur, output, v.Depth(), 3, 5);

            Cv2.ImWrite(@".\CvLaplacianOfGaussianFilter.png", output);
        }
    }
}
