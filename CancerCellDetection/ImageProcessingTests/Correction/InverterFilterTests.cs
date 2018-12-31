using System.Drawing;
using ImageProcessing.Correction;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace ImageProcessingTests.Correction
{
    [TestClass()]
    public class InverterFilterTests
    {
        [TestMethod()]
        public void InvertTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = InverterFilter.Invert(v);
            res.Save(@".\InvertTest.png");
        }

        [TestMethod()]
        public void InvertGrayScaleTest()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            //Filtre inverse
            var resInv = InverterFilter.Invert(res);
            resInv.Save(@".\InvertGrayScaleTest.png");
        }


        [TestMethod]
        public void CvInverteFilter()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");
            Mat output = new Mat();
            //Filtre inverse
            Cv2.BitwiseNot(v, output);
            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvInverteFilter.png", output);
        }
    }
}