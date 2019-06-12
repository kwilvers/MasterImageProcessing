using System;
using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using ImageProcessing.Morphology;
using ImageProcessing.Thresholding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace ImageProcessingTests.Thresholding
{
    [TestClass()]
    public class ThresholdingFilterTests
    {
        [TestMethod()]
        public void HysteresisThresholdingFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            var resThr = HysteresisThresholdingFilter.Apply(resConv.Output, 40, 80);
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\HysteresisThresholdingFilter4080Test.png");
        }

        [TestMethod()]
        public void HysteresisThresholdingFilter5080Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            //Seuillage par Hystérésis 
            var resThr = HysteresisThresholdingFilter.Apply(resConv.Output, 50, 100);
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\HysteresisThresholdingFilter50100Test.png");
        }

        [TestMethod()]
        public void BinaryThresholdingFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            //Seuillage binaire
            var resThr = BinaryThresholdingFilter.Apply(resConv.Output, 60);
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\BinaryThresholdingFilterTest.png");
        }

        [TestMethod()]
        public void TruncatedThresholdingFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            //Seuillage tronqué
            var resThr = TruncatedThresholdingFilter.Apply(resConv.Output, 60);
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\TruncatedThresholdingFilterTest.png");
        }

        [TestMethod()]
        public void ZeroThresholdingFilterTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            //Seuillage par zéro
            var resThr = ZeroThresholdingFilter.Apply(resConv.Output, 60, false);
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\ZeroThresholdingFilterTest.png");
        }

        [TestMethod()]
        public void ZeroThresholdingFilterMaxTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            //Seuillage par zéro inverse
            var resThr = ZeroThresholdingFilter.Apply(resConv.Output, 60, true);
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\ZeroThresholdingFilterMaxTest.png");
        }

        [TestMethod()]
        public void OtsuThresholdingFilter()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new SobelFilter());
            //Calcul du seuil par la méthode de Otsu
            int th = (int) OtsuThresholding.Compute(resConv.Output);
            //Seuillage par hystérésis calculé par la méthode de Otsu
            var resThr = HysteresisThresholdingFilter.Apply(resConv.Output, th/2, th);
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\OtsuThresholdingFilter"+th+".png");
        }

        [TestMethod]
        public void BandThresholdingTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GammaCorrection.Correct(v, 0.3);

            Color c = Color.FromArgb(200, 147, 233);
            //Seuillage de bande de couleur
            var resThr = BandThresholdingFilter.Apply(res, c, 100, 60);

            resThr.Save(@".\BandThresholdingFilterTest.png");
        }

        [TestMethod]
        public void BandThresholdingOpenTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            //Correction gamma pour amélioration
            var res = GammaCorrection.Correct(v, 0.3);
            //Seuillage par bande de couleur RGB
            Color c = Color.FromArgb(200, 147, 233);
            var resThr = BandThresholdingFilter.Apply(res, c, 100, 60);
            //Erosions du masque
            resThr.Save(@".\10BandThresholdingOpenTest.png");
            var ero = Erode.Apply(resThr, new RoundStructuredElement(), 128);
            ero.Save(@".\20BandThresholdingOpenTest.png");
            ero = Erode.Apply(ero, new RoundStructuredElement(), 128);
            ero.Save(@".\31BandThresholdingOpenTest.png");
            //Dilatations du masque
            var dil = Dilate.Apply(ero, new RoundStructuredElement(), 128);
            dil.Save(@".\40BandThresholdingOpenTest.png");
            dil = Dilate.Apply(dil, new RoundStructuredElement(), 128);
            dil.Save(@".\51BandThresholdingOpenTest.png");
            dil = Dilate.Apply(dil, new CrossStructuredElement(), 128);
            dil.Save(@".\52BandThresholdingOpenTest.png");
            //Intersection du masque et de l'image originale
            var sub = Morpho.Intersec(v, dil);
            sub.Save(@".\60BandThresholdingOpenSubTest.png");
        }

        [TestMethod]
        public void CvThresholdBinaryTest()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.Grayscale);
            Mat output = new Mat();
            //Seuillage binaire
            Cv2.Threshold(v, output, 60, 255, ThresholdTypes.Binary);
            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvThresholdBinaryTest.png", output);
            //Seuillage binaire inverse
            Cv2.Threshold(v, output, 60, 255, ThresholdTypes.BinaryInv);
            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvThresholdBinaryInvTest.png", output);
        }

        [TestMethod]
        public void CvThresholdZeroTest()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.Grayscale);
            Mat output = new Mat();
            //Seuillage par zéro
            Cv2.Threshold(v, output, 60, 255, ThresholdTypes.Tozero);
            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvThresholdZero.png", output);
            //Seuillage par zéro inverse
            Cv2.Threshold(v, output, 60, 255, ThresholdTypes.TozeroInv);
            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvThresholdZeroTest.png", output);
        }


        [TestMethod]
        public void CvThresholdTruncTest()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.Grayscale);
            Mat output = new Mat();
            //Seuillage tronqué
            Cv2.Threshold(v, output, 60, 255, ThresholdTypes.Trunc);
            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvThresholdTruncTest.png", output);
        }

        [TestMethod]
        public void CvThresholdOtsuTest()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.Grayscale);
            Mat output = new Mat();
            //Seuillage par hystérésis et la méthode de Otsu
            Cv2.Threshold(v, output, 60, 255, ThresholdTypes.Otsu);
            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvThresholdOtsuTest.png", output);
        }

        [TestMethod]
        public void CvColorBandTest()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");
            Mat hsv = new Mat();

            //Convertion RGB vers HSB
            Cv2.CvtColor(v, hsv, ColorConversionCodes.BGR2HSV);
            Cv2.ImWrite(@".\CvColorBandHsvTest.png", hsv);

            Mat mask = new Mat();
            //Déclaration des seuil de couleurs HSB
            var lowher_color = new Scalar(114, 64,0);
            var higher_color = new Scalar(180, 255, 194);

            //Seuillage par bande de couleur
            Cv2.InRange(hsv, lowher_color, higher_color, mask);
            Cv2.ImWrite(@".\CvColorBandMaskTest.png", mask);

            Mat output = new Mat();
            Cv2.BitwiseAnd(v, v, output, mask);
            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvColorBandTest1.png", output);

            //Erosion
            var erode = mask.Erode(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(11, 11)));
            Cv2.ImWrite(@".\CvColorBandErodeTest.png", erode);
            //Dilatation
            var morpho = erode.Dilate(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(15, 15)));
            Cv2.ImWrite(@".\CvColorBandMorphoTest.png", morpho);
            output = new Mat();

            //Intersection du masque et de l'image originale
            Cv2.BitwiseAnd(v, v, output, morpho );

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvColorBandTest2.png", output);
        }



        [TestMethod]
        public void CvColorBandWithoutErosionTest()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");
            Mat hsv = new Mat();

            //Convertion RGB vers HSB
            Cv2.CvtColor(v, hsv, ColorConversionCodes.BGR2HSV);
            Cv2.ImWrite(@".\CvColorBandHsvTest.png", hsv);

            Mat mask = new Mat();
            //Déclaration des seuil de couleurs HSB
            var lowher_color = new Scalar(114, 64, 0);
            var higher_color = new Scalar(180, 255, 194);

            //Seuillage par bande de couleur
            Cv2.InRange(hsv, lowher_color, higher_color, mask);
            Cv2.ImWrite(@".\CvColorBandMaskTest.png", mask);

            Mat output = new Mat();
            //Intersection du masque et de l'image originale
            Cv2.BitwiseAnd(v, v, output, mask);

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvColorBandTest.png", output);
        }




        [TestMethod]
        public void Cv2TimesColorBandTest()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");

            Mat first = ColorBandTest(v, new Scalar(114, 64, 0), new Scalar(180, 255, 194), 11, 15);
            Cv2.ImWrite(@".\first.png", first);
            //Mat second = ColorBandTest(first, new Scalar(169, 156, 0), new Scalar(180, 255, 255), 3, 5);
            Mat second = ColorBandTest(first, new Scalar(130, 0, 0), new Scalar(180, 255, 160), 7, 9);
            Cv2.ImWrite(@".\second.png", second);



            //CONTOUR
            Mat blur = new Mat();
            Point[][] contours;
            HierarchyIndex[] hierarchy;

            //Filtre gaussien
            Cv2.GaussianBlur(second, blur, new OpenCvSharp.Size(7, 7), 5, 5, BorderTypes.Default);
            Cv2.ImWrite(@".\70FindContourBlur.png", blur);

            //Méthode de Otsu
            Bitmap v2 = (Bitmap)Bitmap.FromFile(@".\70FindContourBlur.png");
            int thOtsu = (int)OtsuThresholding.Compute(v2);

            //Détecteur de contours de Canny
            Cv2.Canny(blur, second, thOtsu / 3, thOtsu);
            Cv2.ImWrite(@".\80FindContourCanny.png", second);

            //Enumération des contours
            Cv2.FindContours(second, out contours, out hierarchy, RetrievalModes.List, ContourApproximationModes.ApproxTC89KCOS);

            //Dessine les contours en vert
            for (var i = 0; i < contours.Length; i++)
                Cv2.DrawContours(v, contours, i, new Scalar(0, 255, 0), 2);

            Cv2.ImWrite(@".\Cv2TimesColorBandTest.png", v);

        }

        [TestMethod]
        public void Cv2TimesReechanlillonedColorBandTest()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon_50percent.png");

            Mat first = ColorBandTest(v, new Scalar(117, 62, 0), new Scalar(180, 255, 255), 11, 15);
            Cv2.ImWrite(@".\first.png", first);
            //Mat second = ColorBandTest(first, new Scalar(169, 156, 0), new Scalar(180, 255, 255), 3, 5);
            Mat second = ColorBandTest(first, new Scalar(130, 0, 0), new Scalar(180, 255, 167), 4, 6);
            Cv2.ImWrite(@".\second.png", second);



            //CONTOUR
            Mat blur = new Mat();
            Point[][] contours;
            HierarchyIndex[] hierarchy;

            //Filtre gaussien
            Cv2.GaussianBlur(second, blur, new OpenCvSharp.Size(7, 7), 5, 5, BorderTypes.Default);
            Cv2.ImWrite(@".\70FindContourBlur.png", blur);

            //Méthode de Otsu
            Bitmap v2 = (Bitmap)Bitmap.FromFile(@".\70FindContourBlur.png");
            int thOtsu = (int)OtsuThresholding.Compute(v2);

            //Détecteur de contours de Canny
            Cv2.Canny(blur, second, thOtsu / 3, thOtsu);
            Cv2.ImWrite(@".\80FindContourCanny.png", second);

            //Enumération des contours
            Cv2.FindContours(second, out contours, out hierarchy, RetrievalModes.List, ContourApproximationModes.ApproxTC89KCOS);

            //Dessine les contours en vert
            for (var i = 0; i < contours.Length; i++)
                Cv2.DrawContours(v, contours, i, new Scalar(0, 255, 0), 1);

            Cv2.ImWrite(@".\Cv2TimesColorBandTest.png", v);

        }


        public Mat ColorBandTest(Mat input, Scalar l, Scalar h, int ero, int dil)
        {
            //Chargement de l'image
            Mat hsv = new Mat();

            //Convertion RGB vers HSB
            Cv2.CvtColor(input, hsv, ColorConversionCodes.BGR2HSV);
            Cv2.ImWrite(@".\CvColorBandHsvTest.png", hsv);

            Mat mask = new Mat();
            //Seuillage par bande de couleur
            Cv2.InRange(hsv, l, h, mask);
            Cv2.ImWrite(@".\CvColorBandMaskTest.png", mask);

            Mat output = new Mat();
            Cv2.BitwiseAnd(input, input, output, mask);
            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvColorBandTest1.png", output);

            //Erosion
            var erode = mask.Erode(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(ero, ero)));
            Cv2.ImWrite(@".\CvColorBandErodeTest.png", erode);
            //Dilatation
            var morpho = erode.Dilate(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(dil, dil)));
            Cv2.ImWrite(@".\CvColorBandMorphoTest.png", morpho);
            output = new Mat();

            //Intersection du masque et de l'image originale
            Cv2.BitwiseAnd(input, input, output, morpho);

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvColorBandTest2.png", output);

            return output;
        }

    }
}