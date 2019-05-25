using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Smoothing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace ImageProcessingTests.Smoothing
{
    [TestClass]
    public class BilateralFilterTest
    {
        [TestMethod]
        public void cvBilateralS7Test()
        {
            Mat v = Cv2.ImRead(@".\echantillon.png");
            Mat output = new Mat();
            //Taille de kernel 
            int kernel_length = 15;
            //Bilateral blur
            Cv2.BilateralFilter(v, output, kernel_length, kernel_length * 2, kernel_length / 2);
            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\BilateralS7Test.png", output);
        }

        [TestMethod()]
        public void BilateralFilterS7W3Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            //Filtre bilatéral de taille 11 
            var resConv = Convolution.Convolve(v, new BilateralFilter(11, 5));
            resConv.Output.Save(@".\BilateralFilterS7W3Test.png");
        }
    }
}
