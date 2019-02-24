using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace ImageProcessingTests.Segmentation
{
    [TestClass]
    public class InPaintTest
    {
        [TestMethod]
        public void cvInpaint3CircleTest()
        {
            Mat v = Cv2.ImRead(@".\echantillon.png");
            Mat mask = Cv2.ImRead(@".\mask_inpaint.png", ImreadModes.AnyDepth);
            Mat output = new Mat();
            Mat output2 = new Mat();
            //Taille de kernel 
            int kernel_length = 15;
            Cv2.Inpaint(v, mask, output, 100, InpaintMethod.Telea);
            //Bilateral blur
            //Cv2.BilateralFilter(output, output2, kernel_length, kernel_length * 2, kernel_length / 2);
            Cv2.GaussianBlur(output, output2, new OpenCvSharp.Size(5, 5), 5, 5, BorderTypes.Default);
            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\cvInpaint3CircleTest.png", output2);
        }

        [TestMethod]
        public void cvInpaintDetectCircleTest()
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
                Cv2.Circle(mask, (int) circle.Center.X, (int)circle.Center.Y, (int) circle.Radius+5, new Scalar(255), -1);

            Cv2.ImWrite(@".\cvInpaintDetectCircleMaskTest.png", mask);

            //Taille de kernel 
            Cv2.Inpaint(v, mask, output, 25, InpaintMethod.Telea);

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\cvInpaintDetectCircleTest.png", output);
        }

    }
}
