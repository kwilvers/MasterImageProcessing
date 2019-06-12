using System;
using System.Drawing;
using System.IO;
using System.Threading;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using ImageProcessing.Smoothing;
using ImageProcessing.Thresholding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace ImageProcessingTests.Detection
{
    [TestClass]
    public class CannyTest
    {
        Mat blur = new Mat();
        int lowThreshold = 42;
        String windowName = "karl";

        [TestMethod]
        public void cvCannyTest()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.Grayscale);

            //Matrice de gradient X et Y
            Mat output = new Mat();

            //Gaussian blur
            Cv2.GaussianBlur(v, blur, new OpenCvSharp.Size(7, 7), 5, 5, BorderTypes.Default);
            //Convolution par kernel
            Cv2.Canny(blur, output, lowThreshold, lowThreshold*3, 3);

            Cv2.NamedWindow(windowName, WindowMode.AutoSize);
            CvTrackbarCallback2 onChange = OnChange;
            Cv2.CreateTrackbar("Min threshold", windowName, ref lowThreshold, 100, onChange);
            OnChange(0, 0);
            
            Cv2.ImWrite(@".\cvCannyTest.png", output);

            Cv2.WaitKey();
        }

        private void OnChange(int pos, object userdata)
        {
            Mat output = new Mat();
            //Convolution par kernel
            Cv2.Canny(blur, output, lowThreshold, lowThreshold * 3, 3);
            Cv2.ImWrite(@".\cvCannyTest.png", output);
            Cv2.ImShow(windowName, output);
        }

        [TestMethod]
        public void CannyBt709Gaussian159Sobel4Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            //Convertion en niveau de gris selon la norme Bt709
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            res.Save(@".\CannyBt709Test.png");
            //Application d'un filtre gaussien de taille 5
            var resGaus = Convolution.Convolve(res, new GaussianFilter159S5());
            resGaus.Output.Save(@".\CannyBt709Gaussian159Test.png");
            //Filtre de détection de Sobel de quatre orientation
            var resSobel = Convolution.Convolve(resGaus.Output, new SobelFilter4O(), true);
            resSobel.Output.Save(@".\CannyBt709Gaussian159SobelTest.png");
            var b = resSobel.DirectionToBitmap();
            b.Save(@".\toDirectionBitmap.png");
            //Supression des non maximas sur trois pixel
            var max = NonMaximumSuppression.Apply(resSobel.Output, resSobel.Directions);
            max.Save(@"CannyBt709Gaussian159SobelMaxTest.png");
            //Calcul du seuil maximum par la méthode de Otsu
            int th = (int)OtsuThresholding.Compute(max);
            double min = (double)th / 2;
            //Seuillage par histérésis
            var resThr = HysteresisThresholdingFilter.Apply(max, (int) min, th);

            resThr.Save(@".\CannyBt709Gaussian159Sobel4Test.png");
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\CannyBt709Gaussian159Sobel2InverseTest.png");
        }


        [TestMethod]
        public void CannyBt709Gaussian159Sobel2InverseTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            //Convertion en niveau de gris selon la norme Bt709
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            res.Save(@".\CannyBt709Test.png");
            //Application d'un filtre gaussien de taille 5
            var resGaus = Convolution.Convolve(res, new GaussianFilter159S5());
            resGaus.Output.Save(@".\CannyBt709Gaussian159Test.png");
            //Filtre de détection de Sobel de quatre orientation
            var resSobel = Convolution.Convolve(resGaus.Output, new SobelFilter(), true);
            resSobel.Output.Save(@".\CannyBt709Gaussian159Sobel2Test.png");
            var b = resSobel.DirectionToBitmap();
            b.Save(@".\toDirectionBitmap.png");
            //Supression des non maximas sur trois pixel
            var max = NonMaximumSuppression.Apply(resSobel.Output, resSobel.Directions);
            max.Save(@"CannyBt709Gaussian159SobelMaxTest.png");
            //Calcul du seuil maximum par la méthode de Otsu
            int th = (int)OtsuThresholding.Compute(max);
            double min = (double)th / 2;
            //Seuillage par histérésis
            var resThr = HysteresisThresholdingFilter.Apply(max, (int)min, th);
            resThr.Save(@".\CannyBt709Gaussian159Sobel2Test.png");
            //Filtre inverse
            var resInv = InverterFilter.Invert(resThr);
            resInv.Save(@".\CannyBt709Gaussian159Sobel2InverseTest.png");
        }

        [TestMethod]
        public void CannyBt709Gaussian159Sobel2Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\ech.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            res.Save(@".\CannyBt709Test.png");
            var resGaus = Convolution.Convolve(res, new GaussianFilter159S5());
            resGaus.Output.Save(@".\CannyBt709Gaussian159Test.png");
            var resSobel = Convolution.Convolve(resGaus.Output, new SobelFilter4O(), true);
            resSobel.Output.Save(@".\CannyBt709Gaussian159SobelTest.png");
            File.WriteAllBytes(@".\directions.bin", resSobel.Directions);
            var b = resSobel.DirectionToBitmap();
            b.Save(@".\toDirectionBitmap.png");
            var max = NonMaximumSuppression.Apply(resSobel.Output, resSobel.Directions);
            int th = (int)OtsuThresholding.Compute(max);
            var resThr = HysteresisThresholdingFilter.Apply(max, 40, 120);
            resThr.Save(@".\CannyBt709Gaussian159Sobel2Test.png");
        }
    }
}
