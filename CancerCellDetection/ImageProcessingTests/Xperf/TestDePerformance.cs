using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Detection;
using ImageProcessing.Smoothing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using Size = System.Drawing.Size;

namespace ImageProcessingTests.Xperf
{
    [TestClass]
    public class TestDePerformance
    {
        private static String fileName = @".\performance.csv";
        //public void Bench.ComputeCpuMemoryUsage(Action action, long length, int iteration = 1,
        //                                  [CallerMemberName] string caller = null)
        //{
        //    //Calcul du temps d'exécution pour n itération
        //    Stopwatch stopwatch = Stopwatch.StartNew();
        //    for(int i=0; i<iteration; i++)
        //        action();
        //    stopwatch.Stop();
        //    //Affichage des résultats dans la sortie standard
        //    Process proc = Process.GetCurrentProcess();
        //    CultureInfo ci = new CultureInfo("en-US");
        //    CultureInfo.CurrentCulture = ci;
        //    Console.WriteLine("Method : {0}", caller);
        //    Console.WriteLine("Time elapsed={0}", stopwatch.Elapsed);
        //    //Affichage de la charge mémoire maximum
        //    Console.WriteLine("Memory usage (bytes)={0}", proc.PeakWorkingSet64);
        //    Console.WriteLine("Memory usage (MByte)={0}", proc.PeakWorkingSet64/1048576);
        //    Console.WriteLine("Lenght : {0}", length);
        //    //Sauvegarde des résultats dans un fichier CSV
        //    string str = $"{caller};{stopwatch.Elapsed.TotalSeconds};{proc.PeakWorkingSet64};{proc.PeakWorkingSet64 / 1048576};{length};{iteration}";
        //    var writer = File.AppendText(@".\performance.csv");
        //    writer.AutoFlush = true;
        //    writer.WriteLine(str);
        //    writer.Close();
        //}

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            //if( File.Exists(fileName))
            //    File.Delete(fileName);
            File.WriteAllText(fileName, "Caller;Elapsed;Memory (byte);Memory (Mb);Pixel count;Iteration" + System.Environment.NewLine);
        }

        [TestMethod]
        public void SmallTestPerso()
        {
            Bench.ComputeCpuMemoryUsage(() =>
            {
                //Chargement de l'image
                Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
                //Convertion en niveau de gris
                var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
                //Application d'un filtre gaussien de 9x9
                var gaus = Convolution.Convolve(res, new GaussianFilter(9, 5));
                //Application d'un filtre de Sobel à 4 orientations
                var resConv = Convolution.Convolve(gaus.Output, new SobelFilter4O());
                //Sauvegarde du résultat
                resConv.Output.Save(@".\SmallTestPerso.png");
            }, 1235*809);
        }

        [TestMethod]
        public void SmallTestOpenCv()
        {
            Bench.ComputeCpuMemoryUsage(() =>
            {
                //Chargement de l'image en niveau de gris
                Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.AnyDepth);
                Mat gaus = new Mat();
                //Application d'un filtre gaussien de 9x9
                Cv2.GaussianBlur(v, gaus, new OpenCvSharp.Size(9, 9), 0, 0, BorderTypes.Default);
                //Déclaration des matrice
                Mat output1 = new MatOfDouble();
                Mat output2 = new MatOfDouble();
                Mat output3 = new MatOfDouble();
                Mat output4 = new MatOfDouble();
                Mat output12 = new Mat();
                Mat output34 = new Mat();
                Mat output = new Mat();
                //Creation des kernels
                var kernel1 = new float[] {-1, 0, 1, -2, 0, 2, -1, 0, 1};
                var kernel2 = new float[] {-2, -1, 0, -1, 0, 1, 0, 1, 2};
                var kernel3 = new float[] {-1, -2, -1, 0, 0, 0, 1, 2, 1};
                var kernel4 = new float[] {0, -1, -2, 1, 0, -1, 2, 1, 0};
                var k1 = new Mat(3, 3, MatType.CV_32F, kernel1);
                var k2 = new Mat(3, 3, MatType.CV_32F, kernel2);
                var k3 = new Mat(3, 3, MatType.CV_32F, kernel3);
                var k4 = new Mat(3, 3, MatType.CV_32F, kernel4);
                //Application d'un filtre de Sobel à 4 orientations
                Cv2.Filter2D(v, output1, -1, k1);
                Cv2.Filter2D(v, output2, -1, k2);
                Cv2.Filter2D(v, output3, -1, k3);
                Cv2.Filter2D(v, output4, -1, k4);
                //Maximisation des résultats locaux
                Cv2.Max(output1, output2, output12);
                Cv2.Max(output3, output4, output34);
                Cv2.Max(output12, output34, output);
                //Sauvegarde du résultat
                Cv2.ImWrite(@".\SmallTestOpenCv.png", output);
            }, 1235 * 809);
        }
        
        [TestMethod]
        public void SmallTestPerso3Times()
        {
            Bench.ComputeCpuMemoryUsage(() =>
            {
                Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
                var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
                var gaus = Convolution.Convolve(res, new GaussianFilter(9, 5));
                var resConv = Convolution.Convolve(gaus.Output, new SobelFilter4O());
                resConv.Output.Save(@".\SmallTestPerso.png");
            }, 1235 * 809, 3);
        }

        [TestMethod]
        public void SmallTestOpenCv3Times()
        {
            Bench.ComputeCpuMemoryUsage(() =>
            {
                //Chargement de l'image
                Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.AnyDepth);
                Mat gaus = new Mat();
                //Filtre gaussien 9x9
                Cv2.GaussianBlur(v, gaus, new OpenCvSharp.Size(9, 9), 0, 0, BorderTypes.Default);

                //Matrice de gradient X et Y
                Mat output1 = new MatOfDouble();
                Mat output2 = new MatOfDouble();
                Mat output3 = new MatOfDouble();
                Mat output4 = new MatOfDouble();
                Mat output12 = new Mat();
                Mat output34 = new Mat();
                Mat output = new Mat();

                //Creation des kernels
                var kernel1 = new float[] { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
                var kernel2 = new float[] { -2, -1, 0, -1, 0, 1, 0, 1, 2 };
                var kernel3 = new float[] { -1, -2, -1, 0, 0, 0, 1, 2, 1 };
                var kernel4 = new float[] { 0, -1, -2, 1, 0, -1, 2, 1, 0 };
                var k1 = new Mat(3, 3, MatType.CV_32F, kernel1);
                var k2 = new Mat(3, 3, MatType.CV_32F, kernel2);
                var k3 = new Mat(3, 3, MatType.CV_32F, kernel3);
                var k4 = new Mat(3, 3, MatType.CV_32F, kernel4);

                //Convolution par quatres kernels
                Cv2.Filter2D(v, output1, -1, k1);
                Cv2.Filter2D(v, output2, -1, k2);
                Cv2.Filter2D(v, output3, -1, k3);
                Cv2.Filter2D(v, output4, -1, k4);

                Cv2.Max(output1, output2, output12);
                Cv2.Max(output3, output4, output34);
                Cv2.Max(output12, output34, output);

                Cv2.ImWrite(@".\SmallTestOpenCv.png", output);
            }, 1235 * 809, 3);
        }

        [TestMethod]
        public void SmallTestPerso5Times()
        {
            Bench.ComputeCpuMemoryUsage(() =>
            {
                Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
                var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
                var gaus = Convolution.Convolve(res, new GaussianFilter(9, 5));
                var resConv = Convolution.Convolve(gaus.Output, new SobelFilter4O());
                resConv.Output.Save(@".\SmallTestPerso.png");
            }, 1235 * 809, 5);
        }

        [TestMethod]
        public void SmallTestOpenCv5Times()
        {
            Bench.ComputeCpuMemoryUsage(() =>
            {
                //Chargement de l'image
                Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.AnyDepth);
                Mat gaus = new Mat();
                //Filtre gaussien 9x9
                Cv2.GaussianBlur(v, gaus, new OpenCvSharp.Size(9, 9), 0, 0, BorderTypes.Default);

                //Matrice de gradient X et Y
                Mat output1 = new MatOfDouble();
                Mat output2 = new MatOfDouble();
                Mat output3 = new MatOfDouble();
                Mat output4 = new MatOfDouble();
                Mat output12 = new Mat();
                Mat output34 = new Mat();
                Mat output = new Mat();

                //Creation des kernels
                var kernel1 = new float[] { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
                var kernel2 = new float[] { -2, -1, 0, -1, 0, 1, 0, 1, 2 };
                var kernel3 = new float[] { -1, -2, -1, 0, 0, 0, 1, 2, 1 };
                var kernel4 = new float[] { 0, -1, -2, 1, 0, -1, 2, 1, 0 };
                var k1 = new Mat(3, 3, MatType.CV_32F, kernel1);
                var k2 = new Mat(3, 3, MatType.CV_32F, kernel2);
                var k3 = new Mat(3, 3, MatType.CV_32F, kernel3);
                var k4 = new Mat(3, 3, MatType.CV_32F, kernel4);

                //Convolution par quatres kernels
                Cv2.Filter2D(v, output1, -1, k1);
                Cv2.Filter2D(v, output2, -1, k2);
                Cv2.Filter2D(v, output3, -1, k3);
                Cv2.Filter2D(v, output4, -1, k4);

                Cv2.Max(output1, output2, output12);
                Cv2.Max(output3, output4, output34);
                Cv2.Max(output12, output34, output);

                Cv2.ImWrite(@".\SmallTestOpenCv.png", output);
            }, 1235 * 809, 5);
        }

        [TestMethod]
        public void SmallTestPerso10Times()
        {
            Bench.ComputeCpuMemoryUsage(() =>
            {
                Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
                var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
                var gaus = Convolution.Convolve(res, new GaussianFilter(9, 5));
                var resConv = Convolution.Convolve(gaus.Output, new SobelFilter4O());
                resConv.Output.Save(@".\SmallTestPerso.png");
            }, 1235 * 809, 10);
        }

        [TestMethod]
        public void SmallTestOpenCv10Times()
        {
            Bench.ComputeCpuMemoryUsage(() =>
            {
                //Chargement de l'image
                Mat v = Cv2.ImRead(@".\echantillon.png", ImreadModes.AnyDepth);
                Mat gaus = new Mat();
                //Filtre gaussien 9x9
                Cv2.GaussianBlur(v, gaus, new OpenCvSharp.Size(9, 9), 0, 0, BorderTypes.Default);

                //Matrice de gradient X et Y
                Mat output1 = new MatOfDouble();
                Mat output2 = new MatOfDouble();
                Mat output3 = new MatOfDouble();
                Mat output4 = new MatOfDouble();
                Mat output12 = new Mat();
                Mat output34 = new Mat();
                Mat output = new Mat();

                //Creation des kernels
                var kernel1 = new float[] { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
                var kernel2 = new float[] { -2, -1, 0, -1, 0, 1, 0, 1, 2 };
                var kernel3 = new float[] { -1, -2, -1, 0, 0, 0, 1, 2, 1 };
                var kernel4 = new float[] { 0, -1, -2, 1, 0, -1, 2, 1, 0 };
                var k1 = new Mat(3, 3, MatType.CV_32F, kernel1);
                var k2 = new Mat(3, 3, MatType.CV_32F, kernel2);
                var k3 = new Mat(3, 3, MatType.CV_32F, kernel3);
                var k4 = new Mat(3, 3, MatType.CV_32F, kernel4);

                //Convolution par quatres kernels
                Cv2.Filter2D(v, output1, -1, k1);
                Cv2.Filter2D(v, output2, -1, k2);
                Cv2.Filter2D(v, output3, -1, k3);
                Cv2.Filter2D(v, output4, -1, k4);

                Cv2.Max(output1, output2, output12);
                Cv2.Max(output3, output4, output34);
                Cv2.Max(output12, output34, output);

                Cv2.ImWrite(@".\SmallTestOpenCv.png", output);
            }, 1235 * 809, 10);
        }
        
        [TestMethod]
        public void MediumTestPerso()
        {
            Bench.ComputeCpuMemoryUsage(() =>
            {
                Bitmap v = (Bitmap)Bitmap.FromFile(@".\medium.jpg");
                var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
                var gaus = Convolution.Convolve(res, new GaussianFilter(9, 5));
                var resConv = Convolution.Convolve(gaus.Output, new SobelFilter4O());
                resConv.Output.Save(@".\MediumTestPerso.png");
            }, 6022*6022);
        }
        
        [TestMethod]
        public void MediumTestOpenCv()
        {
            Bench.ComputeCpuMemoryUsage(() =>
            {
                //Chargement de l'image
                Mat v = Cv2.ImRead(@".\medium.jpg", ImreadModes.AnyDepth);
                Mat gaus = new Mat();
                //Filtre gaussien 9x9
                Cv2.GaussianBlur(v, gaus, new OpenCvSharp.Size(9, 9), 0, 0, BorderTypes.Default);

                //Matrice de gradient X et Y
                Mat output1 = new MatOfDouble();
                Mat output2 = new MatOfDouble();
                Mat output3 = new MatOfDouble();
                Mat output4 = new MatOfDouble();
                Mat output12 = new Mat();
                Mat output34 = new Mat();
                Mat output = new Mat();

                //Creation des kernels
                var kernel1 = new float[] { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
                var kernel2 = new float[] { -2, -1, 0, -1, 0, 1, 0, 1, 2 };
                var kernel3 = new float[] { -1, -2, -1, 0, 0, 0, 1, 2, 1 };
                var kernel4 = new float[] { 0, -1, -2, 1, 0, -1, 2, 1, 0 };
                var k1 = new Mat(3, 3, MatType.CV_32F, kernel1);
                var k2 = new Mat(3, 3, MatType.CV_32F, kernel2);
                var k3 = new Mat(3, 3, MatType.CV_32F, kernel3);
                var k4 = new Mat(3, 3, MatType.CV_32F, kernel4);

                //Convolution par quatres kernels
                Cv2.Filter2D(v, output1, -1, k1);
                Cv2.Filter2D(v, output2, -1, k2);
                Cv2.Filter2D(v, output3, -1, k3);
                Cv2.Filter2D(v, output4, -1, k4);

                Cv2.Max(output1, output2, output12);
                Cv2.Max(output3, output4, output34);
                Cv2.Max(output12, output34, output);

                Cv2.ImWrite(@".\MediumTestOpenCv.png", output);
            }, 6022 * 6022);
        }

        [TestMethod]
        public void LargeTestPerso()
        {
            Bench.ComputeCpuMemoryUsage(() =>
            {
                Bitmap v = (Bitmap)Bitmap.FromFile(@".\Large.jpg");
                var res = GrayScaleConverter.ToGray(v, GrayScaleConverter.GrayConvertionMethod.Bt709);
                var gaus = Convolution.Convolve(res, new GaussianFilter(9, 5));
                var resConv = Convolution.Convolve(gaus.Output, new SobelFilter4O());
                resConv.Output.Save(@".\LargeTestPerso.png");
            }, 14048*14003);
        }

        [TestMethod]
        public void LargeTestOpenCv()
        {
            Bench.ComputeCpuMemoryUsage(() =>
            {
                //Chargement de l'image
                Mat v = Cv2.ImRead(@".\large.jpg", ImreadModes.AnyDepth);
                Mat gaus = new Mat();
                //Filtre gaussien 9x9
                Cv2.GaussianBlur(v, gaus, new OpenCvSharp.Size(9, 9), 0, 0, BorderTypes.Default);
                v.Dispose();
                v = null;

                //Matrice de gradient X et Y
                Mat output1 = new MatOfDouble();
                Mat output2 = new MatOfDouble();
                Mat output12 = new Mat();
                Mat output34 = new Mat();
                Mat output = new Mat();

                //Creation des kernels
                var kernel1 = new float[] { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
                var kernel2 = new float[] { -2, -1, 0, -1, 0, 1, 0, 1, 2 };
                var kernel3 = new float[] { -1, -2, -1, 0, 0, 0, 1, 2, 1 };
                var kernel4 = new float[] { 0, -1, -2, 1, 0, -1, 2, 1, 0 };
                var k1 = new Mat(3, 3, MatType.CV_32F, kernel1);
                var k2 = new Mat(3, 3, MatType.CV_32F, kernel2);
                var k3 = new Mat(3, 3, MatType.CV_32F, kernel3);
                var k4 = new Mat(3, 3, MatType.CV_32F, kernel4);

                //Convolution par quatres kernels
                Cv2.Filter2D(gaus, output1, -1, k1);
                Cv2.Filter2D(gaus, output2, -1, k2);
                Cv2.Max(output1, output2, output12);
                Cv2.Filter2D(gaus, output1, -1, k3);
                Cv2.Filter2D(gaus, output2, -1, k4);

                //Cv2.Max(output1, output2, output12);
                Cv2.Max(output1, output2, output34);
                output1.Dispose();output2.Dispose();
                output1 = null;
                output2 = null;
                Cv2.Max(output12, output34, output);

                Cv2.ImWrite(@".\LargeTestOpenCv.png", output);
            }, 14048 * 14003);
        }
    }
}
