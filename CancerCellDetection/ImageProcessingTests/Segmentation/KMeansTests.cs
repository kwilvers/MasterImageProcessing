using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessing.Segmentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace ImageProcessing.Segmentation.Tests
{
    [TestClass()]
    public class KMeansTests
    {

        [TestMethod]
        public void cvKmean4()
        {
            Mat v = Cv2.ImRead(@".\echantillon.png");
            Mat output = new Mat();

            KMeans.Proceed(v, output, 10, true, Scalar.Black);

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\cvKmean4.png", output);
        }

        [TestMethod]
        public void cvKmeanLarge()
        {
            Mat v = Cv2.ImRead(@"D:\repos\Photos tests invasion-migration\1 test\250M04 1.png");
            Mat output = new Mat();

            KMeans.Proceed(v, output, 4, true, Scalar.Black);

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@"D:\repos\Photos tests invasion-migration\1 test\KMEAN.png", output);

        }


        [TestMethod]
        public void cvKmeanKarl()
        {
            var lst = new List<string>() { "DSC_7643.JPG", "DSC_7644.JPG", "DSC_7663.JPG", "DSC_7669.JPG" };
            lst.ForEach(s=>
            {
                Mat v = Cv2.ImRead(@".\"+s);
                Mat output = new Mat();

                KMeans.Proceed(v, output, 8);

                //Enregistrement de l'image de sortie
                Cv2.ImWrite(@".\new_"+s, output);
            });
        }


        [TestMethod]
        public void cvKmean4Processed()
        {
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

            Cv2.ImWrite(@".\10ccvKmean4DetectCircleMaskTest.png", mask);

            //Taille de kernel 
            Cv2.Inpaint(v, mask, output, 25, InpaintMethod.Telea);

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\20cvKmean4DetectCircleTest.png", output);



            Mat output2 = new Mat();

            //Augmente le contraste de 10%
            output.ConvertTo(output2, output2.Depth(), 1.05, 0);
            Cv2.ImWrite(@".\30cvKmean4ContrastTest.png", output2);



            Mat hsv = new Mat();
            //Convertion RGB vers HSB
            Cv2.CvtColor(output2, hsv, ColorConversionCodes.BGR2HSV);
            Cv2.ImWrite(@".\40cvKmean4HsvTest.png", hsv);

            //Déclaration des seuil de couleurs HSB
            var lowerColor = new Scalar(60, 10, 10);
            var higherColor = new Scalar(280, 255, 220);

            //Seuillage par bande de couleur
            Cv2.InRange(hsv, lowerColor, higherColor, mask);
            Cv2.ImWrite(@".\50cvKmean4BandMaskTest.png", mask);

            //Erosion
            var erode = mask.Erode(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(13, 13)));
            //Dilatation
            var morpho = erode.Dilate(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(17, 17)));
            Cv2.ImWrite(@".\60cvKmean4BandMorphoTest.png", morpho);

            Mat o = new Mat();
            //Intersection du masque et de l'image originale
            Cv2.BitwiseAnd(output2, output2, o, morpho);

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\70cvKmean4BandTest.png", o);


            KMeans.Proceed(o, output2, 4);

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\80cvKmean4Processed.png", output2);
        }


    }
}