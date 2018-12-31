using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using ImageProcessing.Morphology;
using ImageProcessing.Smoothing;
using ImageProcessing.Thresholding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using Size = OpenCvSharp.Size;

namespace ImageProcessingTests.Morphology
{
    [TestClass]
    public class MorphoTest
    {
        [TestMethod()]
        public void DilateRoungBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            //Applique un filtre gaussien
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            //Calcul des gradients
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            byte otsuTh = (byte) OtsuThresholding.Compute(sobl.Output);
            //Seuillage par hysterésis
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");
            //Dilatation
            var dilate = Dilate.Apply(th, new RoundStructuredElement(), otsuTh);
            dilate.Save(@".\DilateRoundBinaryGrayGaussianSobelTest.png");
        }

        [TestMethod()]
        public void ErodeRoundBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");
            var ero = Erode.Apply(th, new RoundStructuredElement(), otsuTh);
            ero.Save(@".\ErodeRoundBinaryGrayGaussianSobelTest.png");
        }

        [TestMethod()]
        public void DilateCrossBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");
            var ero = Dilate.Apply(th, new CrossStructuredElement(), otsuTh);
            ero.Save(@".\DilateCrossBinaryGrayGaussianSobelTest.png");
        }

        [TestMethod()]
        public void ErodeCrossBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");
            var ero = Erode.Apply(th, new CrossStructuredElement(), otsuTh);
            ero.Save(@".\ErodeCrossBinaryGrayGaussianSobelTest.png");
        }

        [TestMethod()]
        public void DilateSquareBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");
            var ero = Dilate.Apply(th, new SquareStructuredElement(), otsuTh);
            ero.Save(@".\DilateSquareBinaryGrayGaussianSobelTest.png");
        }

        [TestMethod()]
        public void ErodeSquareBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");
            var ero = Erode.Apply(th, new SquareStructuredElement(), otsuTh);
            ero.Save(@".\ErodeSquareBinaryGrayGaussianSobelTest.png");
        }


        [TestMethod()]
        public void OpenCrossBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");
            //Ouverture
            //Erosion
            var ero = Erode.Apply(th, new CrossStructuredElement(), otsuTh);
            //Dilatation
            var dil = Dilate.Apply(ero, new CrossStructuredElement(), otsuTh);
            dil.Save(@".\OpenCrossBinaryGrayGaussianSobelTest.png");
        }

        [TestMethod()]
        public void CloseCrossBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");
            //Fermeture
            //Dilatation
            var dil = Dilate.Apply(th, new CrossStructuredElement(), otsuTh);
            //Erosion
            var ero = Erode.Apply(dil, new CrossStructuredElement(), otsuTh);
            ero.Save(@".\CloseCrossBinaryGrayGaussianSobelTest.png");
        }


        [TestMethod()]
        public void InteriorCrossBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            //Filtre de Sobel
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            //Seuillage par binarisation
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");
            //Erosion
            var ero = Erode.Apply(th, new CrossStructuredElement(), otsuTh);
            ero.Save(@".\ero.png");
            //Contour intérieur
            var inter = Morpho.Sub(th, ero);
            inter.Save(@".\InteriorCrossBinaryGrayGaussianSobelTest.png");

            var inv = InverterFilter.Invert(inter);
            inv.Save(@".\InvInteriorCrossBinaryGrayGaussianSobelTest.png");
        }

        [TestMethod]
        public void CvInteriorEdge()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.AnyDepth);
            Mat contraste = new Mat();
            //Augmente le contraste de 10%
            v.ConvertTo(contraste, v.Depth(), 1.1, 0);
            Mat gaus = new Mat();
            //Filtre gaussien 9x9
            Cv2.GaussianBlur(contraste, gaus, new Size(9, 9), 0, 0, BorderTypes.Default);

            //Matrice de gradient X et Y
            Mat output1 = new MatOfDouble(); Mat output2 = new MatOfDouble();
            Mat output3 = new MatOfDouble(); Mat output4 = new MatOfDouble();
            Mat output12 = new Mat(); Mat output34 = new Mat();
            Mat output = new Mat(); Mat ero = new Mat();
            Mat th = new Mat();

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
            Cv2.Filter2D(v, output1, -1, k1);
            Cv2.Filter2D(v, output2, -1, k2);
            Cv2.Filter2D(v, output3, -1, k3);
            Cv2.Filter2D(v, output4, -1, k4);

            Cv2.Max(output1, output2, output12);
            Cv2.Max(output3, output4, output34);
            Cv2.Max(output12, output34, output);

            Cv2.ImWrite(@".\40CvInteriorEdge.png", output);

            //Seuillage par binarisation
            Cv2.Threshold(output, th, 0, 255, ThresholdTypes.Otsu|ThresholdTypes.Binary);
            Cv2.ImWrite(@".\41CvInteriorEdge.png", th);

            Mat element = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(3,3));
            //Erosion
            Cv2.MorphologyEx(th, ero, MorphTypes.Erode, element);
            Cv2.ImWrite(@".\50CvInteriorEdge.png", ero);
            //Soustraction de l'erodé, contour intérieur
            var interior = th - ero;
            Cv2.ImWrite(@".\60CvInteriorEdge.png", interior);
        }

        [TestMethod()]
        public void ExteriorCrossBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            //Seuillage par la méthode de Otsu
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            //Filtre de sobel
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");
            //Dilatation
            var dil = Dilate.Apply(th, new CrossStructuredElement(), otsuTh);
            dil.Save(@".\dil.png");
            //Contour extérieur
            var ext = Morpho.Sub(dil, th);
            ext.Save(@".\ExteriorCrossBinaryGrayGaussianSobelTest.png");

            var inv = InverterFilter.Invert(ext);
            inv.Save(@".\InvExteriorCrossBinaryGrayGaussianSobelTest.png");
        }

        [TestMethod]
        public void CvExteriorEdge()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.AnyDepth);
            Mat contraste = new Mat();
            //Augmente le contraste de 10%
            v.ConvertTo(contraste, v.Depth(), 1.1, 0);
            Mat gaus = new Mat();
            //Filtre gaussien 9x9
            Cv2.GaussianBlur(contraste, gaus, new Size(9, 9), 0, 0, BorderTypes.Default);

            //Matrice de gradient X et Y
            Mat output1 = new MatOfDouble(); Mat output2 = new MatOfDouble();
            Mat output3 = new MatOfDouble(); Mat output4 = new MatOfDouble();
            Mat output12 = new Mat(); Mat output34 = new Mat();
            Mat output = new Mat(); Mat dilate = new Mat();
            Mat th = new Mat();

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
            Cv2.Filter2D(v, output1, -1, k1);
            Cv2.Filter2D(v, output2, -1, k2);
            Cv2.Filter2D(v, output3, -1, k3);
            Cv2.Filter2D(v, output4, -1, k4);

            Cv2.Max(output1, output2, output12);
            Cv2.Max(output3, output4, output34);
            Cv2.Max(output12, output34, output);

            Cv2.ImWrite(@".\40CvExteriorEdge.png", output);

            //Seuillage par binarisation
            Cv2.Threshold(output, th, 0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);
            Cv2.ImWrite(@".\41CvExteriorEdge.png", th);

            Mat element = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(3, 3));
            //Dilatation
            Cv2.MorphologyEx(th, dilate, MorphTypes.Dilate, element);
            Cv2.ImWrite(@".\50CvExteriorEdge.png", dilate);
            //Soustraction de l'erodé, contour intérieur
            var interior = dilate-th;
            Cv2.ImWrite(@".\60CvExteriorEdge.png", interior);
        }

        [TestMethod()]
        public void MorphoGradientCrossBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");

            var ero = Erode.Apply(th, new CrossStructuredElement(), otsuTh);
            ero.Save(@".\ero.png");

            var dil = Dilate.Apply(th, new CrossStructuredElement(), otsuTh);
            dil.Save(@".\dil.png");

            var inter = Morpho.Sub(th, ero);
            inter.Save(@".\InteriorCrossBinaryGrayGaussianSobelTest.png");

            var ext = Morpho.Sub(dil, th);
            ext.Save(@".\ExteriorCrossBinaryGrayGaussianSobelTest.png");

            var grad = Morpho.Add(inter, ext);

            var inv = InverterFilter.Invert(grad);
            inv.Save(@".\InvMorphoGradientCrossBinaryGrayGaussianSobelTest.png");
        }
        
        [TestMethod()]
        public void MorphoGradientRoundBinaryGrayGaussianSobelTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            var gaus = Convolution.Convolve(res, new GaussianFilter159S5());
            //Sobel 4 orientation
            var sobl = Convolution.Convolve(gaus.Output, new SobelFilter4O(), true);
            //var max = NonMaximumSuppression.Apply(sobl.Output, sobl.Directions);
            byte otsuTh = (byte)OtsuThresholding.Compute(sobl.Output);
            //Seuillage par binarisation 
            var th = BinaryThresholdingFilter.Apply(sobl.Output, otsuTh);
            th.Save(@".\otsu.png");
            //Erosion
            var ero = Erode.Apply(th, new RoundStructuredElement(), otsuTh);
            ero.Save(@".\ero.png");
            //Dilatation
            var dil = Dilate.Apply(th, new RoundStructuredElement(), otsuTh);
            dil.Save(@".\dil.png");
            //Contour intérieur
            var inter = Morpho.Sub(th, ero);
            inter.Save(@".\InteriorRoundBinaryGrayGaussianSobelTest.png");
            //contour extérieur
            var ext = Morpho.Sub(dil, th);
            ext.Save(@".\ExteriorRoundBinaryGrayGaussianSobelTest.png");
            //Gradient morphologique
            var grad = Morpho.Add(inter, ext);

            var inv = InverterFilter.Invert(grad);
            inv.Save(@".\InvMorphoGradientRoundBinaryGrayGaussianSobelTest.png");
        }

        [TestMethod]
        public void CvMorphoGradient()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.AnyDepth);
            Mat contraste = new Mat();
            //Augmente le contraste de 10%
            v.ConvertTo(contraste, v.Depth(), 1.1, 0);
            Cv2.ImWrite(@".\10CvMorphoGradient.png", contraste);
            Mat gaus = new Mat();
            //Filtre gaussien 9x9
            Cv2.GaussianBlur(contraste, gaus, new Size(9, 9), 0, 0, BorderTypes.Default);
            Cv2.ImWrite(@".\20CvMorphoGradient.png", contraste);

            //Matrice de gradient X et Y
            Mat output1 = new MatOfDouble(); Mat output2 = new MatOfDouble();
            Mat output3 = new MatOfDouble(); Mat output4 = new MatOfDouble();
            Mat output12 = new Mat(); Mat output34 = new Mat();
            Mat output = new Mat(); Mat ero = new Mat();

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
            Cv2.Filter2D(v, output1, -1, k1);
            Cv2.ImWrite(@".\31CvMorphoGradient.png", output1);
            Cv2.Filter2D(v, output2, -1, k2);
            Cv2.ImWrite(@".\32CvMorphoGradient.png", output2);
            Cv2.Filter2D(v, output3, -1, k3);
            Cv2.ImWrite(@".\33CvMorphoGradient.png", output3);
            Cv2.Filter2D(v, output4, -1, k4);
            Cv2.ImWrite(@".\34CvMorphoGradient.png", output4);

            Cv2.Max(output1, output2, output12);
            Cv2.Max(output3, output4, output34);
            Cv2.Max(output12, output34, output);

            Cv2.ImWrite(@".\40CvMorphoGradient12.png", output12);
            Cv2.ImWrite(@".\41CvMorphoGradient34.png", output34);
            Cv2.ImWrite(@".\42CvMorphoGradient.png", output);

            Mat element = Cv2.GetStructuringElement(MorphShapes.Cross, new Size(5, 5));
            Cv2.MorphologyEx(output, ero, MorphTypes.Gradient, element);
            Cv2.ImWrite(@".\50CvMorphoGradient.png", ero);

        }

        [TestMethod]
        public void CvErode()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.Grayscale);
            Mat gaus = new Mat();
            //Filtre gaussien 9x9
            Cv2.GaussianBlur(v, gaus, new OpenCvSharp.Size(9, 9), 0, 0, BorderTypes.Default);

            //Matrice de gradient X et Y
            Mat outputX = new Mat(); Mat outputY = new Mat();
            Mat absX = new Mat(); Mat absY = new Mat();
            Mat sobel = new Mat(); Mat th = new Mat();
            //Calcul des gradient X et Y
            Cv2.Sobel(v, outputX, v.Depth(), 1, 0);
            Cv2.ConvertScaleAbs(outputX, absX);
            Cv2.Sobel(v, outputY, v.Depth(), 0, 1);
            Cv2.ConvertScaleAbs(outputY, absY);
            Cv2.AddWeighted(absX, 0.5, absY, 0.5, 0, sobel);
            Cv2.ImWrite(@".\CvErodeSobel.png", sobel);

            //Seuillage par hystérésis et la méthode de Otsu
            Cv2.Threshold(sobel, th, 60, 255, ThresholdTypes.Otsu);

            Mat output = new Mat();
            output = th.Erode(Cv2.GetStructuringElement(MorphShapes.Cross, new Size(3, 3)));
            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvErode.png", output);
        }

        [TestMethod]
        public void CvDilate()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.Grayscale);
            Mat gaus = new Mat();
            //Filtre gaussien 9x9
            Cv2.GaussianBlur(v, gaus, new OpenCvSharp.Size(9, 9), 0, 0, BorderTypes.Default);
            //Matrice de gradient X et Y
            Mat outputX = new Mat(); Mat outputY = new Mat();
            Mat absX = new Mat(); Mat absY = new Mat();
            Mat sobel = new Mat();
            //Calcul des gradients X et Y
            Cv2.Sobel(v, outputX, v.Depth(), 1, 0);
            Cv2.ConvertScaleAbs(outputX, absX);
            Cv2.Sobel(v, outputY, v.Depth(), 0, 1);
            Cv2.ConvertScaleAbs(outputY, absY);
            Cv2.AddWeighted(absX, 0.5, absY, 0.5, 0, sobel);
            Cv2.ImWrite(@".\CvDilateSobel.png", sobel);
            Mat output = new Mat();
            output = sobel.Dilate(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(5, 5)));

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvDilateElipse.png", output);
        }

        [TestMethod]
        public void CvOpen()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.Grayscale);
            Mat gaus = new Mat();
            //Filtre gaussien 9x9
            Cv2.GaussianBlur(v, gaus, new OpenCvSharp.Size(9, 9), 0, 0, BorderTypes.Default);
            //Matrice de gradient X et Y
            Mat outputX = new Mat(); Mat outputY = new Mat();
            Mat absX = new Mat(); Mat absY = new Mat();
            Mat sobel = new Mat();
            //Calcul des gradients X et Y
            Cv2.Sobel(v, outputX, v.Depth(), 1, 0);
            Cv2.ConvertScaleAbs(outputX, absX);
            Cv2.Sobel(v, outputY, v.Depth(), 0, 1);
            Cv2.ConvertScaleAbs(outputY, absY);
            Cv2.AddWeighted(absX, 0.5, absY, 0.5, 0, sobel);
            Cv2.ImWrite(@".\CvDilateSobel.png", sobel);
            Mat output = new Mat();
            //Ouverture
            //Erosion
            output = sobel.Erode(Cv2.GetStructuringElement(MorphShapes.Cross, new Size(3, 3)));
            //Dilatation
            output = output.Dilate(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(5, 5)));

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvDilateElipse.png", output);
        }

        [TestMethod]
        public void CvOpen2()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.AnyDepth);
            Mat contraste = new Mat();
            //Augmente le contraste de 10%
            v.ConvertTo(contraste, v.Depth(), 1.1, 0);
            Cv2.ImWrite(@".\10CvOpen2.png", contraste);
            Mat gaus = new Mat();
            //Filtre gaussien 9x9
            Cv2.GaussianBlur(contraste, gaus, new Size(9, 9), 0, 0, BorderTypes.Default);
            Cv2.ImWrite(@".\20CvOpen2.png", contraste);

            //Matrice de gradient X et Y
            Mat output1 = new MatOfDouble(); Mat output2 = new MatOfDouble();
            Mat output3 = new MatOfDouble(); Mat output4 = new MatOfDouble();
            Mat output12 = new Mat(); Mat output34 = new Mat();
            Mat output = new Mat();Mat ero = new Mat();

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
            Cv2.Filter2D(v, output1, -1, k1);
            Cv2.ImWrite(@".\31CvOpen2.png", output1);
            Cv2.Filter2D(v, output2, -1, k2);
            Cv2.ImWrite(@".\32CvOpen2.png", output2);
            Cv2.Filter2D(v, output3, -1, k3);
            Cv2.ImWrite(@".\33CvOpen2.png", output3);
            Cv2.Filter2D(v, output4, -1, k4);
            Cv2.ImWrite(@".\34CvOpen2.png", output4);

            Cv2.Max(output1, output2, output12);
            Cv2.Max(output3, output4, output34);
            Cv2.Max(output12, output34, output);

            Cv2.ImWrite(@".\40CvOpen212.png", output12);
            Cv2.ImWrite(@".\41CvOpen234.png", output34);
            Cv2.ImWrite(@".\42CvOpen2.png", output);

            Mat element = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(5,5));
            Cv2.MorphologyEx(output, ero, MorphTypes.Open, element);
            Cv2.ImWrite(@".\50CvOpen2.png", ero);

        }

        [TestMethod]
        public void CvClose()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.Grayscale);
            Mat gaus = new Mat();
            //Filtre gaussien 9x9
            Cv2.GaussianBlur(v, gaus, new OpenCvSharp.Size(9, 9), 0, 0, BorderTypes.Default);
            //Matrice de gradient X et Y
            Mat outputX = new Mat(); Mat outputY = new Mat();
            Mat absX = new Mat(); Mat absY = new Mat();
            Mat sobel = new Mat();
            //Calcul des gradients X et Y
            Cv2.Sobel(v, outputX, v.Depth(), 1, 0);
            Cv2.ConvertScaleAbs(outputX, absX);
            Cv2.Sobel(v, outputY, v.Depth(), 0, 1);
            Cv2.ConvertScaleAbs(outputY, absY);
            Cv2.AddWeighted(absX, 0.5, absY, 0.5, 0, sobel);
            Cv2.ImWrite(@".\CvDilateSobel.png", sobel);
            Mat output = new Mat();
            //Fermeture
            //Dilatation
            output = sobel.Dilate(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(5, 5)));
            //Erosion
            output = output.Erode(Cv2.GetStructuringElement(MorphShapes.Cross, new Size(3, 3)));

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvDilateElipse.png", output);
        }

        [TestMethod]
        public void CvClose2()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.AnyDepth);
            Mat contraste = new Mat();
            //Augmente le contraste de 10%
            v.ConvertTo(contraste, v.Depth(), 1.1, 0);
            Cv2.ImWrite(@".\10CvClose2.png", contraste);
            Mat gaus = new Mat();
            //Filtre gaussien 9x9
            Cv2.GaussianBlur(contraste, gaus, new Size(9, 9), 0, 0, BorderTypes.Default);
            Cv2.ImWrite(@".\20CvClose2.png", contraste);

            //Matrice de gradient X et Y
            Mat output1 = new MatOfDouble(); Mat output2 = new MatOfDouble();
            Mat output3 = new MatOfDouble(); Mat output4 = new MatOfDouble();
            Mat output12 = new Mat(); Mat output34 = new Mat();
            Mat output = new Mat(); Mat ero = new Mat();

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
            Cv2.Filter2D(v, output1, -1, k1);
            Cv2.ImWrite(@".\31CvClose2.png", output1);
            Cv2.Filter2D(v, output2, -1, k2);
            Cv2.ImWrite(@".\32CvClose2.png", output2);
            Cv2.Filter2D(v, output3, -1, k3);
            Cv2.ImWrite(@".\33CvClose2.png", output3);
            Cv2.Filter2D(v, output4, -1, k4);
            Cv2.ImWrite(@".\34CvClose2.png", output4);

            Cv2.Max(output1, output2, output12);
            Cv2.Max(output3, output4, output34);
            Cv2.Max(output12, output34, output);

            Cv2.ImWrite(@".\40CvClose212.png", output12);
            Cv2.ImWrite(@".\41CvClose234.png", output34);
            Cv2.ImWrite(@".\42CvClose2.png", output);

            Mat element = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(5, 5));
            Cv2.MorphologyEx(output, ero, MorphTypes.Close, element);
            Cv2.ImWrite(@".\50CvClose2.png", ero);

        }



    }
}
