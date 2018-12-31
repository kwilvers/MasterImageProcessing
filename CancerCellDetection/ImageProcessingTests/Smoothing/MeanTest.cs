using System.Drawing;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Smoothing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using OpenCvSharp.ML;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace ImageProcessingTests.Smoothing
{
    [TestClass]
    public class MeanFilterTest
    {
        [TestMethod()]
        public void ConvolveMeanFilterC4S3Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = Convolution.Convolve(v, new MeanFilterC4S3());//Connexité 4 taille 3
            resConv.Output.Save(@".\MeanFilterC4S3Test.png");
        }

        [TestMethod()]
        public void ConvolveMeanFilterC8S3Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = Convolution.Convolve(v, new MeanFilterC8S3()); //Connexité 8 taille 3
            resConv.Output.Save(@".\MeanFilterC8S3Test.png");
        }

        [TestMethod()]
        public void ConvolveMeanFilterC24S5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = Convolution.Convolve(v, new MeanFilterC24S5());
            resConv.Output.Save(@".\MeanFilterC24S5Test.png");
        }

        [TestMethod()]
        public void ConvolveMeanFilterC48S7Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var resConv = Convolution.Convolve(v, new MeanFilterC48S7());
            resConv.Output.Save(@".\MeanFilterC48S7Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayMeanFilterC4S3Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new MeanFilterC4S3());
            resConv.Output.Save(@".\GrayMeanFilterC4S3Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayMeanFilterC8S3Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new MeanFilterC8S3());
            resConv.Output.Save(@".\GrayMeanFilterC8S3Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayMeanFilterC24S5Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new MeanFilterC24S5());
            resConv.Output.Save(@".\GrayMeanFilterC24S5Test.png");
        }

        [TestMethod()]
        public void ConvolveGrayMeanFilterC48S7Test()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Average);
            var resConv = Convolution.Convolve(res, new MeanFilterC48S7());
            //Enregistrement de l'image de sortie
            resConv.Output.Save(@".\GrayMeanFilterC48S7Test.png");
        }

        [TestMethod]
        public void CvMeanS9Filter()
        {
            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");
            Mat output = new Mat();
            //Filtre moyenneur 9x9
            Cv2.Blur(v, output, new Size(9,9), new Point(-1,-1), BorderTypes.Default );
            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvMeanS9Filter.png", output);
        }

        [TestMethod]
        public void CvMeanS3C4Filter()
        {
            //MARCHE PAS !!!!!

            //Chargement de l'image
            Mat v = Cv2.ImRead(@".\echantillon.png");
            Mat output = new Mat();
            //Filtre moyenneur 9x9
            //Cv2.Blur(v, output, new Size(9, 9), new Point(-1, -1), BorderTypes.Default);
            var kd = new double[,]{
                { 1, 1, 1 },
                { 1, 1, 1 },
                { 1, 1, 1 }
            };

            Cv2.Filter2D(v, output, v.Depth(), InputArray.Create(1), new Point(-1, -1), (double)1/5, BorderTypes.Default);
            //Enregistrement de l'image de sortie
            Cv2.ImWrite(@".\CvMeanS3C4Filter.png", output);
        }
    }
}
