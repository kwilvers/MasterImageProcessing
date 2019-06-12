using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using Size = OpenCvSharp.Size;

namespace ImageProcessingTests.Detection
{
    [TestClass]
    public class SobelTest
    {
        [TestMethod()]
        public void ConvolveGraySobelFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            //Filtre de Sobel
            var resConv = Convolution.Convolve(res, new SobelFilter());
            resConv.Output.Save(@".\GraySobelFilterTest.png");
        }

        [TestMethod()]
        public void ConvolveGraySobelFilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GraySobelFilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolveSobelShapeTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\ech.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GraySobelFilterInvertedShapeTest.png");
        }

        [TestMethod()]
        public void ConvolveGraySobelO4FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var resConv = Convolution.Convolve(res, new SobelFilter4O());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\GraySobelO4FilterInvertedTest.png");
        }

        [TestMethod()]
        public void ConvolveColorSobelO4FilterInvertedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            //var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            //Filtre de Sobel à 4 orientations
            var resConv = Convolution.Convolve(v, new SobelFilter4O());
            var resInv = InverterFilter.Invert(resConv.Output);
            resInv.Save(@".\ColorSobelO4FilterInvertedTest.png");
        }

        [TestMethod]
        public void CvSobelFilter()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.Grayscale);
            Mat con = new Mat();
            v.ConvertTo(con, v.Depth(), 1.1, 0);

            //Matrice de gradient X et Y
            Mat outputX = new Mat();
            Mat outputY = new Mat();
            //Calcul des gradient X et Y
            Cv2.Sobel(con, outputX, v.Depth(), 1, 0);
            Cv2.Sobel(con, outputY, v.Depth(), 0, 1);
            Mat output = outputX + outputY;

            Cv2.ImWrite(@".\CvSobelFilterX.png", outputX);
            Cv2.ImWrite(@".\CvSobelFilterY.png", outputY);
            Cv2.ImWrite(@".\CvSobelFilter.png", output);
        }

        [TestMethod]
        public void CvSobel2Filter()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.Grayscale);

            //Matrice de gradient X et Y
            Mat outputX = new Mat(); Mat outputY = new Mat();
            Mat absX = new Mat();    Mat absY = new Mat();
            Mat output = new Mat();
            //Calcul des gradient X et Y
            Cv2.Sobel(v, outputX, v.Depth(), 1, 0);
            Cv2.ConvertScaleAbs(outputX, absX);
            Cv2.Sobel(v, outputY, v.Depth(), 0, 1);
            Cv2.ConvertScaleAbs(outputY, absY);
            Cv2.AddWeighted(absX, 0.5, absY, 0.5, 0, output);
            
            Cv2.ImWrite(@".\CvSobelFilterX.png", outputX);
            Cv2.ImWrite(@".\CvSobelFilterY.png", outputY);
            Cv2.ImWrite(@".\CvSobel2Filter.png", output);
        }
        
        [TestMethod]
        public void CvSobel4Filter()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.AnyDepth);
            Mat contraste = new Mat();
            //Augmente le contraste de 10%
            v.ConvertTo(contraste, v.Depth(), 1.1, 0);
            Cv2.ImWrite(@".\10CvSobel4Filter.png", contraste);
            Mat gaus = new Mat();
            //Filtre gaussien 9x9
            Cv2.GaussianBlur(contraste, gaus, new Size(9, 9), 0, 0, BorderTypes.Default);
            Cv2.ImWrite(@".\20CvSobel4Filter.png", contraste);

            //Matrice de gradient X et Y
            Mat output1 = new MatOfDouble(); Mat output2 = new MatOfDouble();
            Mat output3 = new MatOfDouble(); Mat output4 = new MatOfDouble();
            Mat abs1 = new Mat(); Mat abs2 = new Mat();
            Mat abs3 = new Mat(); Mat abs4 = new Mat();
            Mat output12 = new Mat(); Mat output34 = new Mat();
            Mat output = new Mat();

            //Creation des kernels
            var kernel1 = new float[] { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
            var kernel2 = new float[] { -2, -1, 0, -1, 0, 1, 0, 1, 2 };
            var kernel3 = new float[] { -1, -2, -1, 0, 0, 0, 1, 2, 1 };
            var kernel4 = new float[] { 0, -1, -2, 1, 0, -1, 2, 1, 0 };
            var k1 = new Mat(3, 3, MatType.CV_32F, kernel1);
            var k2 = new Mat(3, 3, MatType.CV_32F, kernel2);
            var k3 = new Mat(3, 3, MatType.CV_32F, kernel3);
            var k4 = new Mat(3, 3, MatType.CV_32F, kernel4);

            //Convolution par quatres kernels
            Cv2.Filter2D(gaus, output1, -1, k1);
            Cv2.ImWrite(@".\31CvSobel4Filter.png", output1);
            Cv2.Filter2D(gaus, output2, -1, k2);
            Cv2.ImWrite(@".\32CvSobel4Filter.png", output2);
            Cv2.Filter2D(gaus, output3, -1, k3);
            Cv2.ImWrite(@".\33CvSobel4Filter.png", output3);
            Cv2.Filter2D(gaus, output4, -1, k4);
            Cv2.ImWrite(@".\34CvSobel4Filter.png", output4);
            ////Conversion en valeurs absolue 8 bits
            //Cv2.ConvertScaleAbs(output1, abs1);
            //Cv2.ConvertScaleAbs(output2, abs2);
            //Cv2.ConvertScaleAbs(output3, abs3);
            //Cv2.ConvertScaleAbs(output4, abs4);
            //Cv2.AddWeighted(abs1, 0.5, abs3, 0.5, 0, output12);
            //Cv2.AddWeighted(abs2, 0.5, abs4, 0.5, 0, output34);
            ////Addition de quatre matrices dont le poids de chacune des matrices est identique
            //Cv2.AddWeighted(output12, 0.5, output34, 0.5, 0, output);

            MatOfDouble md = new MatOfDouble();
            Cv2.Max(output1, output2, output12);
            Cv2.Max(output3, output4, output34);
            Cv2.Max(output12, output34, output);


            Cv2.ImWrite(@".\41CvSobel4Filter.png", output12);
            Cv2.ImWrite(@".\42CvSobel4Filter.png", output34);
            Cv2.ImWrite(@".\40CvSobel4Filter.png", output);
        }



        [TestMethod]
        public void CvSobel2GaussianFilter()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.Grayscale);
            Mat output = new Mat();
            v.ConvertTo(output, v.Depth(), 1.1, 0);
            Cv2.ImWrite(@".\CvSobel2GaussianFilterContrast.png", output);
            Cv2.GaussianBlur(output, output, new Size(3, 3), 0, 0, BorderTypes.Default);
            Cv2.ImWrite(@".\CvSobel2GaussianFilterBlur.png", output);

            //Matrice de gradient X et Y
            Mat outputX = new Mat();
            Mat outputY = new Mat();
            Mat absX = new Mat();
            Mat absY = new Mat();

            //Calcul des gradient X et Y
            Cv2.Sobel(output, outputX, v.Depth(), 1, 0);
            Cv2.ConvertScaleAbs(outputX, absX);

            Cv2.Sobel(output, outputY, v.Depth(), 0, 1);
            Cv2.ConvertScaleAbs(outputY, absY);

            Cv2.AddWeighted(absX, 0.5, absY, 0.5, 0, output);

            Cv2.ImWrite(@".\CvSobel2GaussianFilterX.png", outputX);
            Cv2.ImWrite(@".\CvSobel2GaussianFilterY.png", outputY);
            Cv2.ImWrite(@".\CvSobel2GaussianFilter.png", output);
        }

        [TestMethod]
        public void CvSobel4GaussianFilter()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.AnyDepth);
            Mat contraste = new Mat();
            //Augmente le contraste de 10%
            v.ConvertTo(contraste, v.Depth(), 1.5, 0);
            Cv2.ImWrite(@".\10CvSobel4Filter.png", contraste);
            Mat gaus = new Mat();
            //Filtre gaussien 9x9
            Cv2.GaussianBlur(contraste, gaus, new Size(3, 3), 0, 0, BorderTypes.Default);
            Cv2.ImWrite(@".\20CvSobel4Filter.png", contraste);

            //Matrice de gradient X et Y
            Mat output1 = new MatOfDouble(); Mat output2 = new MatOfDouble();
            Mat output3 = new MatOfDouble(); Mat output4 = new MatOfDouble();
            Mat output12 = new Mat(); Mat output34 = new Mat();
            Mat output = new Mat();

            //Creation des kernels
            var kernel1 = new float[] { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
            var kernel2 = new float[] { -2, -1, 0, -1, 0, 1, 0, 1, 2 };
            var kernel3 = new float[] { -1, -2, -1, 0, 0, 0, 1, 2, 1 };
            var kernel4 = new float[] { 0, -1, -2, 1, 0, -1, 2, 1, 0 };
            var k1 = new Mat(3, 3, MatType.CV_32F, kernel1);
            var k2 = new Mat(3, 3, MatType.CV_32F, kernel2);
            var k3 = new Mat(3, 3, MatType.CV_32F, kernel3);
            var k4 = new Mat(3, 3, MatType.CV_32F, kernel4);

            //Convolution par quatres kernels
            Cv2.Filter2D(gaus, output1, -1, k1);
            Cv2.ImWrite(@".\31CvSobel4Filter.png", output1);
            Cv2.Filter2D(gaus, output2, -1, k2);
            Cv2.ImWrite(@".\32CvSobel4Filter.png", output2);
            Cv2.Filter2D(gaus, output3, -1, k3);
            Cv2.ImWrite(@".\33CvSobel4Filter.png", output3);
            Cv2.Filter2D(gaus, output4, -1, k4);
            Cv2.ImWrite(@".\34CvSobel4Filter.png", output4);

            MatOfDouble md = new MatOfDouble();
            Cv2.Max(output1, output2, output12);
            Cv2.Max(output3, output4, output34);
            Cv2.Max(output12, output34, output);


            Cv2.ImWrite(@".\41CvSobel4Filter.png", output12);
            Cv2.ImWrite(@".\42CvSobel4Filter.png", output34);
            Cv2.ImWrite(@".\40CvSobel4Filter.png", output);
        }

    }
}
