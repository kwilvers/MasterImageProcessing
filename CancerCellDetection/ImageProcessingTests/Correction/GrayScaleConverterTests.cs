using System;
using System.Diagnostics;
using System.Drawing;
using ImageProcessing.Correction;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace ImageProcessingTests.Correction
{
    [TestClass()]
    public class GrayScaleConverterTests
    {

        [TestMethod()]
        public void ColorAverageTest()
        {
            var sw = Stopwatch.StartNew();
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            Console.WriteLine(sw.Elapsed);
            //Convertion en niveau de gris selon la moyenne
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            Console.WriteLine(sw.Elapsed);
            res.Save(@".\ColorAverageTest.png");
            Console.WriteLine(sw.Elapsed);
            sw.Stop();
        }

        [TestMethod()]
        public void Bt709Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            //Convertion en niveau de gris selon la norme BT709
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
            res.Save(@".\Bt709Test.png");
        }

        [TestMethod()]
        public void FromRedTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            //Convertion en niveau de gris selon une composante
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromRed);
            res.Save(@".\FromRedTest.png");
        }

        [TestMethod()]
        public void FromGreenTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromGreen);
            res.Save(@".\FromGreen.png");
        }

        [TestMethod()]
        public void FromBlueTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromBlue);
            res.Save(@".\FromBlue.png");
        }

        [TestMethod()]
        public void FromBlueAndGreenTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromBlueAndGreen);
            res.Save(@".\FromBlueAndGreen.png");
        }

        [TestMethod()]
        public void FromRedAndBlueTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromRedAndBlue);
            res.Save(@".\FromRedAndBlue.png");
        }

        [TestMethod()]
        public void FromRedAndGreenTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromRedAndGreen);
            res.Save(@".\FromRedAndGreen.png");
        }


        [TestMethod()]
        public void FromBrightnessTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            //Convertion en niveau de gris selon la luminance
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromBrightness);
            res.Save(@".\FromBrightness.png");
        }

        [TestMethod()]
        public void FromUChrominanceTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromUChrominance);
            res.Save(@".\FromUChrominance.png");
        }

        [TestMethod()]
        public void FromVChrominanceTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromVChrominance);
            res.Save(@".\FromVChrominance.png");
        }





        [TestMethod]
        public void CvGrayRec601ConversionFilter()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");
            Mat output = new Mat();
            //Convertion en niveau de gris Rec601
            Cv2.CvtColor(v, output, ColorConversionCodes.RGB2GRAY);
            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvGrayRec601ConversionFilter.png", output);
        }

        [TestMethod()]
        public void cvColorAverageTest()
        {
            //Chargement de l'image
            try
            {
                Mat v = Cv2.ImRead(@".\echantillon.png");
                //Convertion en niveau de gris selon la moyenne
                var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);

                //Enregistrement de l'image de sortie
                Cv2.ImWrite(@".\cvColorAverageTest.png", res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        [TestMethod()]
        public void cvBt709Test()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");
            //Convertion en niveau de gris selon la moyenne
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\cvBt709Test.png", res);

        }

        [TestMethod()]
        public void cvFromRedTest()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");
            //Convertion en niveau de gris selon la moyenne
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromRed);

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\cvFromRedTest.png", res);
        }

        [TestMethod()]
        public void cvFromGreenTest()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");
            //Convertion en niveau de gris selon la moyenne
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromGreen);

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\cvFromGreenTest.png", res);
        }

        [TestMethod()]
        public void cvFromBlueTest()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");
            //Convertion en niveau de gris selon la moyenne
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromBlue);

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\cvFromBlueTest.png", res);
        }

        [TestMethod()]
        public void cvFromBlueAndGreenTest()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");
            //Convertion en niveau de gris selon la moyenne
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromBlueAndGreen);

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\cvFromBlueAndGreenTest.png", res);
        }

        [TestMethod()]
        public void cvFromRedAndBlueTest()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");
            //Convertion en niveau de gris selon la moyenne
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromRedAndBlue);

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\cvFromRedAndBlueTest.png", res);
        }

        [TestMethod()]
        public void cvFromRedAndGreenTest()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");
            //Convertion en niveau de gris selon la moyenne
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromRedAndGreen);

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\cvFromRedAndGreenTest.png", res);
        }


        [TestMethod()]
        public void cvFromBrightnessTest()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");
            //Convertion en niveau de gris selon la moyenne
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.FromBrightness);

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\cvFromBrightnessTest.png", res);
        }

    }
}