using System;
using System.Drawing;
using ImageProcessing.Correction;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace ImageProcessingTests.Correction
{
    [TestClass()]
    public class GammaCorrectionTests
    {
        [TestMethod()]
        public void Convert03Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GammaCorrection.Correct(v, 0.3);
            res.Save(@".\GammaCorrection03Tests.png");
        }

        [TestMethod()]
        public void Convert24Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GammaCorrection.Correct(v, 2.4);
            res.Save(@".\GammaCorrection24Tests.png");
        }

        [TestMethod()]
        public void GammaCorrectionGray03Tests()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            //Application de la correction gamma
            var resInv = GammaCorrection.Correct(res, 0.3);
            resInv.Save(@".\GammaCorrectionGray03Tests.png");
        }

        [TestMethod()]
        public void GammaCorrectionGray24Tests()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resInv = GammaCorrection.Correct(res, 2.4);
            resInv.Save(@".\GammaCorrectionGray24Tests.png");
        }

        [TestMethod]
        public void CvGammaCorrection()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");
            Mat output = new Mat();

            //Création de la table lut en fonction du facteur de correction gamma
            byte[] lookUpTable = new byte[256];
            double gamma = 0.5;
            for (int i = 0; i < 256; ++i)
                lookUpTable[i] = (byte)Math.Round(Math.Pow(i / 255.0, gamma) * 255.0);
            //Application de la correction gamma
            Cv2.LUT(v, lookUpTable, output);
            
            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvGammaCorrection.png", output);
        }
    }
}