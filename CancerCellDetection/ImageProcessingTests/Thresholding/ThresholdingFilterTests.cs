using System;
using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using ImageProcessing.Morphology;
using ImageProcessing.Thresholding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
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
            var lowher_color = new Scalar(60, 10,10);
            var higher_color = new Scalar(280, 255, 220);
            //var lch = ColorHSB.FromRGB(122, 190, 114);
            //var lch = ColorHSB.FromRGB(60, 50, 50);
            //var lowher_color = new Scalar(lch.H, lch.S * 2.55, lch.B * 2.55);
            ////var hch = ColorHSB.FromRGB(41, 249, 121);
            //var hch = ColorHSB.FromRGB(50, 255,255);
            //var higher_color = new Scalar(hch.H, hch.S * 2.55, hch.B * 2.55);

            //Seuillage par bande de couleur
            Cv2.InRange(hsv, lowher_color, higher_color, mask);
            Cv2.ImWrite(@".\CvColorBandMaskTest.png", mask);

            //Erosion
            var erode = mask.Erode(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(13, 13)));
            //erode = erode.Erode(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(3, 3)));
            //Dilatation
            var morpho = erode.Dilate(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(17, 17)));
            //morpho = morpho.Dilate(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(3, 3)));
            //morpho = morpho.Dilate(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(3, 3)));
            Cv2.ImWrite(@".\CvColorBandMorphoTest.png", morpho);

            Mat output = new Mat();
            //Intersection du masque et de l'image originale
            Cv2.BitwiseAnd(v, v, output, morpho );

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvColorBandTest.png", output);
        }




        Mat hsv = new Mat();
        private Mat ori;
        int lowH = 50, highH = 150;
        int lowS = 0, highS = 250;
        int lowV = 0, highV = 250;
        String windowName = "karl";

        [TestMethod]
        public void CvColorBandUITest()
        {
            //Chargement de l'image
            ori = Cv2.ImRead(@".\echantillon.png");
            hsv = new Mat();
            Cv2.CvtColor(ori, hsv, ColorConversionCodes.RGB2HSV);


            Cv2.NamedWindow(windowName, WindowMode.AutoSize);
            CvTrackbarCallback2 onChange = OnChange;
            Cv2.CreateTrackbar("Low H", windowName, ref lowH, 360, OnChange2, new IntPtr(1));
            Cv2.CreateTrackbar("High H", windowName, ref highH, 360, OnChange2, new IntPtr(2));
            Cv2.CreateTrackbar("Low S", windowName, ref lowS, 255, OnChange2, new IntPtr(3));
            Cv2.CreateTrackbar("High S", windowName, ref highS, 255, OnChange2, new IntPtr(4));
            Cv2.CreateTrackbar("Low V", windowName, ref lowV, 255, OnChange2, new IntPtr(5));
            Cv2.CreateTrackbar("High V", windowName, ref highV, 255, onChange, new IntPtr(6));
            OnChange(0, 0);
            
            Cv2.WaitKey();
        }

        private void OnChange2(int pos, object userdata)
        {
        }

        private void OnChange(int pos, object userdata)
        {
            Mat output = new Mat();
            Mat mask = new Mat();

            //Déclaration des seuils
            var lowher_color = new Scalar(lowH, lowS, lowV);
            var higher_color = new Scalar(highH, highS, highV);
            //Seuillage par couleur et création du masque
            Cv2.InRange(hsv, lowher_color, higher_color, mask);
            //Opération de masquage pour ne conserver que les pixels de seuillés
            Cv2.BitwiseAnd(ori, ori, output, mask);

            Cv2.ImShow(windowName, output);
        }
    }
}