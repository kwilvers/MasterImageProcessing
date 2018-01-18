using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //let fileName = "C:\Users\karl\OneDrive\ALL\UNIF\2eme - Mémoire\Echantillon.PNG"
            var fileName = @"C:\Users\kwilvers\OneDrive\ALL\UNIF\2eme - Mémoire\Echantillon.PNG";

            var image = Cv2.ImRead(fileName, ImreadModes.GrayScale);
            var output = InputOutputArray.Create(image.Clone());
            //let gray = Cv2.CvtColor(image, output, ColorConversionCodes.BGR2GRAY)
            var input = InputArray.Create(image);
            Cv2.GaussianBlur(image, image, new Size(9, 9), 2.0, 2.0);
            //let circles = Cv2.HoughCircles(input, HoughMethods.Gradient, 1.0, 5.0, 200.0, 100.0, 0, 0)
            input = InputArray.Create(output.GetMat());
            var circles = Cv2.HoughCircles(image, HoughMethods.Gradient, 1.0, 5, 200, 10, 10, 50);
            //let lines = Cv2.HoughLines(output, 1.0, Math.PI/180.0, 50, 0.0, 0.0)

            if(circles != null)
            {
                foreach (var c in circles)
                {
                    //Point center(Cv2.cvRound(c), cvRound(circles[i][1]));
                    //int radius = c.Radius;
                    // circle center 
                    Cv2.Circle(image, (int)c.Center.X, (int)c.Center.Y, 3, new Scalar(0, 255, 0), -1, LineTypes.Link8, 0);
                    // circle outline 
                    Cv2.Circle(image, (int)c.Center.X, (int)c.Center.Y, (int)c.Radius, new Scalar(0, 0, 255), 3, LineTypes.Link8, 0);
                    //circle( src, center, radius, Scalar(0,0,255), 3, 8, 0 );

                }
                //Cv2.Circle(image, Convert.ToInt32(c.Center.X), Convert.ToInt32(c.Center.Y), Convert.ToInt32(c.Radius), new Scalar(0.0, 255.0, 0.0), 4, LineTypes.AntiAlias, 0);

            }


            Cv2.ImShow("output", image);
            Cv2.WaitKey(0);

        }
    }
}
