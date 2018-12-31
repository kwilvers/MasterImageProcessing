using System.Drawing;
using ImageProcessing.Correction;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using Size = OpenCvSharp.Size;

namespace ImageProcessingTests.Correction
{
    [TestClass]
    public class ContrastTest
    {
        [TestMethod()]
        public void ContrastCorrection50Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            //Correction de contraste de +50%
            var res = ContrastCorrection.Correct(v, 50);
            res.Save(@".\ContrastCorrection50Test.png");
        }

        [TestMethod()]
        public void ContrastCorrectionminus50Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = ContrastCorrection.Correct(v, -50);
            res.Save(@".\ContrastCorrectionminus50Test.png");
        }

        [TestMethod()]
        public void ContrastCorrection50GrayTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var gray = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var res = ContrastCorrection.Correct(gray, 50);
            res.Save(@".\ContrastCorrection50GrayTest.png");
        }

        [TestMethod()]
        public void ContrastCorrectionminus50GrayTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var gray = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var res = ContrastCorrection.Correct(gray, -50);
            res.Save(@".\ContrastCorrectionminus50GrayTest.png");
        }

        [TestMethod]
        public void CvContrastCorrection()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");
            Mat output = new Mat();

            //Augmente le contraste de 10%
            v.ConvertTo(output, v.Depth(), 1.1, 0);
            //Diminue le contraste de 50%
            v.ConvertTo(output, v.Depth(), 0.5, 0);

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvContrastCorrection.png", output);
        }

        [TestMethod]
        public void CvContrast2Correction()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.ReducedColor8);
            Mat output = new Mat();

            var clahe = Cv2.CreateCLAHE(0.5, new Size(8, 8));
            clahe.Apply(v, output);
            //Cv2.EqualizeHist(v, output);
            

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvContrast2Correction.png", output);
        }
    }
}
