using System;
using System.Drawing;
using ImageProcessing.Correction;
using ImageProcessing.Morphology;
using ImageProcessing.Thresholding;
using ImageProcessingTests.Thresholding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace ImageProcessingTests.Segmentation
{
    [TestClass]
    public class ContourTest
    {
        [TestMethod]
        public void FindContourTest()
        {
            Mat v = Cv2.ImRead(@".\echantillon.png");
            Mat output = new Mat();
            Mat gray = new Mat();
            Mat blur = new Mat();
            Point[][] contours;
            HierarchyIndex[] hierarchy;

            //Convert in gray
            Cv2.CvtColor(v, gray, ColorConversionCodes.BGR2GRAY);

            //Filtre gaussien
            Cv2.GaussianBlur(v, blur, new OpenCvSharp.Size(7, 7), 5, 5, BorderTypes.Default);
            Cv2.ImWrite(@".\blur.png", v);
            
            //Méthode de Otsu
            Bitmap v2 = (Bitmap)Bitmap.FromFile(@".\blur.png");
            int thOtsu = (int)OtsuThresholding.Compute(v2);

            //Détecteur de contours de Canny
            Cv2.Canny(blur, output, thOtsu/3,thOtsu);

            //Enumération des contours
            Cv2.FindContours(output, out contours, out hierarchy, RetrievalModes.List, ContourApproximationModes.ApproxTC89KCOS);

            //Dessine les contours en vert
            for(var i =0; i<contours.Length; i++)
                Cv2.DrawContours(v, contours, i, new Scalar(0,255,0), 2);

            Cv2.ImWrite(@".\FindContourTest.png", v);
        }

        [TestMethod]
        public void FindContourInpaintTest()
        {
            InPaintTest t = new InPaintTest();
            t.cvInpaintDetectCircleTeleaTest();

            Mat v = Cv2.ImRead(@".\cvInpaintDetectCircleTeleaTest.png");
            Mat output = new Mat();
            Mat gray = new Mat();
            Mat blur = new Mat();
            Point[][] contours;
            HierarchyIndex[] hierarchy;

            //Convert in gray
            Cv2.CvtColor(v, gray, ColorConversionCodes.BGR2GRAY);

            //Filtre gaussien
            Cv2.GaussianBlur(v, blur, new OpenCvSharp.Size(7, 7), 5, 5, BorderTypes.Default);
            Cv2.ImWrite(@".\blur.png", v);

            //Méthode de Otsu
            Bitmap v2 = (Bitmap)Bitmap.FromFile(@".\blur.png");
            int thOtsu = (int)OtsuThresholding.Compute(v2);

            //Détecteur de contours de Canny
            Cv2.Canny(blur, output, thOtsu / 3, thOtsu);

            //Enumération des contours
            Cv2.FindContours(output, out contours, out hierarchy, RetrievalModes.List, ContourApproximationModes.ApproxTC89KCOS);

            //Dessine les contours en vert
            for (var i = 0; i < contours.Length; i++)
                Cv2.DrawContours(v, contours, i, new Scalar(0, 255, 0), 2);

            Cv2.ImWrite(@".\FindContourInpaintTest.png", v);
        }

        [TestMethod]
        public void FindContourColorThresholdTest()
        {
            ThresholdingFilterTests t = new ThresholdingFilterTests();
            t.BandThresholdingOpenTest();

            Mat v = Cv2.ImRead(@".\60BandThresholdingOpenSubTest.png");
            Mat output = new Mat();
            Mat gray = new Mat();
            Mat blur = new Mat();
            Point[][] contours;
            HierarchyIndex[] hierarchy;

            //Convert in gray
            Cv2.CvtColor(v, gray, ColorConversionCodes.BGR2GRAY);

            //Filtre gaussien
            Cv2.GaussianBlur(v, blur, new OpenCvSharp.Size(7, 7), 5, 5, BorderTypes.Default);
            Cv2.ImWrite(@".\blur.png", v);

            //Méthode de Otsu
            Bitmap v2 = (Bitmap)Bitmap.FromFile(@".\blur.png");
            int thOtsu = (int)OtsuThresholding.Compute(v2);

            //Détecteur de contours de Canny
            Cv2.Canny(blur, output, thOtsu / 3, thOtsu);

            //Enumération des contours
            Cv2.FindContours(output, out contours, out hierarchy, RetrievalModes.List, ContourApproximationModes.ApproxTC89KCOS);

            v = Cv2.ImRead(@".\echantillon.png");
            //Dessine les contours en vert
            for (var i = 0; i < contours.Length; i++)
                Cv2.DrawContours(v, contours, i, new Scalar(0, 255, 0), 2);

            Cv2.ImWrite(@".\FindContourColorThresholdTest.png", v);
        }


        [TestMethod]
        public void FindContourColorThresholdInpaintTest()
        {
            //INPAINT
            Mat v = Cv2.ImRead(@".\echantillon.png");
            Mat output = new Mat();
            Mat gray = new Mat();

            //Convert in gray
            Cv2.CvtColor(v, gray, ColorConversionCodes.BGR2GRAY);

            //Get circles from the gray image
            var circles = Cv2.HoughCircles(gray, HoughMethods.Gradient, 1, 14.5, 200, 10, 13, 15);

            //Create matrice for the mask
            Mat mask = new Mat(v.Size(), MatType.CV_8U);

            //Draw the circle in the mask
            foreach (var circle in circles)
                Cv2.Circle(mask, (int)circle.Center.X, (int)circle.Center.Y, (int)circle.Radius + 5, new Scalar(255), -1);

            //Taille de kernel 
            Cv2.Inpaint(v, mask, output, 25, InpaintMethod.NS);
            Cv2.ImWrite(@".\10FindContourInpaint.png", mask);
            Cv2.ImWrite(@".\20FindContourInpaint.png", output);


            //BAND THRESHOLD
            //Convertion RGB vers HSB
            Mat hsv = new Mat();
            Cv2.CvtColor(output, hsv, ColorConversionCodes.BGR2HSV);
            Cv2.ImWrite(@".\30FindContourHSV.png", hsv);

            //Déclaration des seuil de couleurs HSB
            var lowher_color = new Scalar(60, 10, 10);
            var higher_color = new Scalar(280, 255, 220);

            //Seuillage par bande de couleur
            Cv2.InRange(hsv, lowher_color, higher_color, mask);
            Cv2.ImWrite(@".\40FindContourInRange.png", mask);

            //Erosion
            var erode = mask.Erode(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(17, 17)));
            //erode = erode.Erode(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(3, 3)));
            Cv2.ImWrite(@".\41FindContourErode.png", erode);
            //Dilatation
            var morpho = erode.Dilate(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(17, 17)));
            morpho = morpho.Dilate(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(3, 3)));
            Cv2.ImWrite(@".\50FindContourErodeDilate.png", morpho);

            //Intersection du masque et de l'image originale
            Mat bw = new Mat();
            Cv2.BitwiseAnd(output, output, bw, morpho);
            Cv2.ImWrite(@".\60FindContourBandColor.png", bw);



            //CONTOUR
            Mat blur = new Mat();
            Point[][] contours;
            HierarchyIndex[] hierarchy;

            //Filtre gaussien
            Cv2.GaussianBlur(bw, blur, new OpenCvSharp.Size(7, 7), 5, 5, BorderTypes.Default);
            Cv2.ImWrite(@".\70FindContourBlur.png", blur);

            //Méthode de Otsu
            Bitmap v2 = (Bitmap)Bitmap.FromFile(@".\70FindContourBlur.png");
            int thOtsu = (int)OtsuThresholding.Compute(v2);

            //Détecteur de contours de Canny
            Cv2.Canny(blur, output, thOtsu / 3, thOtsu);
            Cv2.ImWrite(@".\80FindContourCanny.png", output);

            //Enumération des contours
            Cv2.FindContours(output, out contours, out hierarchy, RetrievalModes.List, ContourApproximationModes.ApproxTC89KCOS);

            //Dessine les contours en vert
            for (var i = 0; i < contours.Length; i++)
                Cv2.DrawContours(v, contours, i, new Scalar(0, 255, 0), 2);

            Cv2.ImWrite(@".\90FindContourColorThresholdInpaintTest.png", v);

        }



        [TestMethod]
        public void CvColorBandContourTest()
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
            Cv2.BitwiseAnd(v, v, output, morpho);

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvColorBandTest2.png", output);


            //Find contour
            Point[][] contours;
            HierarchyIndex[] hierarchy;

            Cv2.FindContours(morpho, out contours, out hierarchy, RetrievalModes.List, ContourApproximationModes.ApproxTC89KCOS);
            //Dessine les contours en vert
            for (var i = 0; i < contours.Length; i++)
                Cv2.DrawContours(v, contours, i, new Scalar(0, 255, 0), 2);

            Cv2.ImWrite(@".\FindContourColorThresholdTest.png", v);

        }



        [TestMethod]
        public void CvColorBandContourCannyTest()
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
            Cv2.BitwiseAnd(v, v, output, morpho);

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvColorBandTest2.png", output);



            //CONTOUR
            Mat blur = new Mat();
            Point[][] contours;
            HierarchyIndex[] hierarchy;

            //Filtre gaussien
            Cv2.GaussianBlur(output, blur, new OpenCvSharp.Size(7, 7), 5, 5, BorderTypes.Default);
            Cv2.ImWrite(@".\70FindContourBlur.png", blur);

            //Méthode de Otsu
            Bitmap v2 = (Bitmap)Bitmap.FromFile(@".\70FindContourBlur.png");
            int thOtsu = (int)OtsuThresholding.Compute(v2);

            //Détecteur de contours de Canny
            Cv2.Canny(blur, output, thOtsu / 3, thOtsu);
            Cv2.ImWrite(@".\80FindContourCanny.png", output);

            //Enumération des contours
            Cv2.FindContours(output, out contours, out hierarchy, RetrievalModes.List, ContourApproximationModes.ApproxTC89KCOS);

            //Dessine les contours en vert
            for (var i = 0; i < contours.Length; i++)
                Cv2.DrawContours(v, contours, i, new Scalar(0, 255, 0), 2);

            Cv2.ImWrite(@".\CvColorBandContourCannyTest.png", v);


        }

    }
}
