using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace ImageProcessingTests.Segmentation
{
    [TestClass]
    public class CircleTest
    {
        [TestMethod]
        public void HoughtCircleTest()
        {
            Mat v = Cv2.ImRead(@".\echantillon.png");
            Mat output = new Mat();
            Mat gray = new Mat();

            //Convert in gray
            Cv2.CvtColor(v, gray, ColorConversionCodes.BGR2GRAY);

            //Get circles from the gray image
            //src_gray: Input image(grayscale)
            //circles: A vector that stores sets of 3 values: for each detected circle.
            //CV_HOUGH_GRADIENT: Define the detection method.Currently this is the only one available in OpenCV
            //dp = 1: The inverse ratio of resolution
            //14.5 distance minimum entre les cercles
            //200 seuil haut pour la méthode de Canny 
            //100 Seuil pour la détection de centre
            //13 et 15 tailles minimum et maximum du rayon des cercles à détecter
            var circles = Cv2.HoughCircles(gray, HoughMethods.Gradient, 1, 30, 200, 10, 13, 15);

            //Draw the circle in the mask
            foreach (var circle in circles)
                Cv2.Circle(v, (int)circle.Center.X, (int)circle.Center.Y, (int)circle.Radius, new Scalar(0,255,0), 2);

            Cv2.ImWrite(@".\HoughtCircleTest.png", v);
        }
    }
}
