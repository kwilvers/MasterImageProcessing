using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Segmentation;
using ImageProcessing.Thresholding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace ImageProcessingTests._FinalTest
{
    [TestClass]
    public class FinalTest
    {
        private int _counter = 0;
        public TestContext TestContext { get; set; }
        public string TestDir { get; set; }
        public string TestRoiDir { get; set; }
        Scalar[] Colors = {Scalar.Red, Scalar.Lime, Scalar.Cyan, Scalar.Gold, Scalar.Fuchsia};

        [TestInitialize]
        public void Init()
        {
            this.TestDir = @".\" + this.TestContext.TestName;
            this.TestRoiDir = @".\" + this.TestContext.TestName + @"\ROI";
            //if (Directory.Exists(this.TestRoiDir))
            //    Directory.Delete(this.TestRoiDir, true);
            if (Directory.Exists(this.TestDir))
                Directory.Delete(this.TestDir, true);

            //Directory.CreateDirectory(this.TestDir);
            Directory.CreateDirectory(this.TestRoiDir);
        }

        [TestMethod]
        public void ThresholdTest()
        {
            Bench.ComputeCpuMemoryUsage(() =>
            {
                //Lecture
                //Mat original = Cv2.ImRead(@".\Echantillon.png");
                Mat original = Cv2.ImRead(@"D:\repos\Photos tests invasion-migration\17T Invasion 090617\17T M01 1 Inv_.jpg");

                Mat output = new Mat();

                //Amélioration des contrastes
                output = this.ContrastCorrection(original, 1.1);

                //Correction gamma
                output = this.GammaCorrection(output, 0.8);

                //Filtre bilatéral de lissage
                output = this.BilateralFilter(output, 19);

                //Trouver les cerlces
                var circles = this.DetectCircle(output);
                //Create matrice for the mask
                Mat circleMask = new Mat(output.Size(), MatType.CV_8U);
                this.DrawCircle(circleMask, circles, 5);

                //Lissage par InPainting
                output = this.Inpaint(output, circleMask, 30);
                circleMask.Dispose();

                //Seuillage par bande de couleur
                Scalar lowerColor = new Scalar(0, 100, 0);
                Scalar higherColor = new Scalar(180, 255, 255);
                output = this.ColorThreshold(output, lowerColor, higherColor, 13, 17);

                //Trouver cellules ?
                var contours = this.FindContour(output);
                foreach (var contour in contours)
                {
                    var x1 = contour.Min(c => c.X);
                    var y1 = contour.Min(c => c.Y);
                    var x2 = contour.Max(c => c.X);
                    var y2 = contour.Max(c => c.Y);
                    if (Math2.EuclideanDistance(x1, y1, x2, y2) > 25)
                    {
                        var r = new Rect(x1, y1, x2 - x1, y2 - y1);
                        var cell = new Mat(output, r);
                        this.SaveROI(cell);
                    }
                }

                var cnt1 = contours.Length;

                //Seuillage par bande de couleur
                lowerColor = new Scalar(130, 0, 0);
                higherColor = new Scalar(180, 255, 175);
                output = this.ColorThreshold(output, lowerColor, higherColor);

                //Dilatation et erosion
                this.Dilate(7, output);

                //Trouver les contours
                contours = this.FindContour(output);

                //Identifier les noyaux
                for (int i = 0; i < contours.Length; i++)
                    Cv2.DrawContours(original, contours, i, this.Colors[i % 5], -1);

                this.Save(original);

                var cnt2 = contours.Length;

                //var p = Path.Combine(TestDir, "count.txt");
                File.AppendAllText(@".\count.txt", this.TestContext.TestName + Environment.NewLine);
                File.AppendAllText(@".\count.txt", "cellules : " + cnt1 + Environment.NewLine + "noyaux : " + cnt2 + Environment.NewLine);

            }, 0);
        }

        [TestMethod]
        public void ThresholdSimpleInPaintTest()
        {
            Bench.ComputeCpuMemoryUsage(() =>
            {
                //Lecture
                //Mat original = Cv2.ImRead(@".\Echantillon.png");
                Mat original = Cv2.ImRead(@"D:\repos\Photos tests invasion-migration\17T Invasion 090617\17T M01 1 Inv_.jpg");

                Mat output = new Mat();

                //Amélioration des contrastes
                output = this.ContrastCorrection(original, 1.5);

                //Correction gamma
                //output = this.GammaCorrection(output, 0.8);

                //Filtre bilatéral de lissage
                output = this.BilateralFilter(output, 19);

                //Trouver les cerlces
                var circles = this.DetectCircle(output, 20, 8, 11);
                //Create matrice for the mask
                this.DrawCircle(output, circles, 0);

                //Seuillage par bande de couleur
                Scalar lowerColor = new Scalar(0, 100, 0);
                Scalar higherColor = new Scalar(180, 255, 255);
                output = this.ColorThreshold(output, lowerColor, higherColor, 13, 17);

                //Trouver cellules ?
                var contours = this.FindContour(output);
                foreach (var contour in contours)
                {
                    var x1 = contour.Min(c => c.X);
                    var y1 = contour.Min(c => c.Y);
                    var x2 = contour.Max(c => c.X);
                    var y2 = contour.Max(c => c.Y);
                    //Cv2.Rectangle(v, new Point(x1, y1), new Point(x2, y2), Scalar.Red, 2);
                    if (Math2.EuclideanDistance(x1, y1, x2, y2) > 25)
                    {
                        var r = new Rect(x1, y1, x2 - x1, y2 - y1);
                        var cell = new Mat(output, r);
                        this.SaveROI(cell);
                    }
                }

                var cnt1 = contours.Length;

                //Seuillage par bande de couleur
                lowerColor = new Scalar(130, 0, 0);
                higherColor = new Scalar(180, 255, 175);
                output = this.ColorThreshold(output, lowerColor, higherColor);

                //Dilatation et erosion
                this.Dilate(7, output);

                //Trouver les contours
                contours = this.FindContour(output);

                //Identifier les noyaux
                for (int i = 0; i < contours.Length; i++)
                    Cv2.DrawContours(original, contours, i, this.Colors[i%5], 2);

                this.Save(original);

                var cnt2 = contours.Length;

                //var p = Path.Combine(TestDir, "count.txt");
                File.AppendAllText(@".\count.txt", this.TestContext.TestName + Environment.NewLine);
                File.AppendAllText(@".\count.txt", "cellules : " + cnt1 + Environment.NewLine + "noyaux : " + cnt2 + Environment.NewLine);
            }, 0);
        }

        [TestMethod]
        public void ThresholdSimpleInPaintKmeanTest()
        {
            Bench.ComputeCpuMemoryUsage(() =>
            {
                //Lecture
                //Mat original = Cv2.ImRead(@".\Echantillon.png");
                Mat original = Cv2.ImRead(@"D:\repos\Photos tests invasion-migration\17T Invasion 090617\17T M01 1 Inv_.jpg");

                Mat output = original.Clone();

                //Trouver les cerlces
                //var circles = this.DetectCircle(output, 20, 8, 11);
                ////Create matrice for the mask
                //this.DrawCircle(output, circles, 0);

                //Filtre k-mean
                output = this.Kmean(output, 10);

                //Seuillage par bande de couleur
                //Scalar lowerColor = new Scalar(163, 0, 0);
                //Scalar higherColor = new Scalar(180, 255, 255);
                Scalar lowerColor = new Scalar(0, 100, 0);
                Scalar higherColor = new Scalar(180, 255, 255);
                output = this.ColorThreshold(output, lowerColor, higherColor, 7, 7);

                //Ouverture
                output = this.Erode(7, output);
                output = this.Dilate(7, output);

                //Trouver cellules ?
                var contours = this.FindContour(output);
                foreach (var contour in contours)
                {
                    var x1 = contour.Min(c => c.X);
                    var y1 = contour.Min(c => c.Y);
                    var x2 = contour.Max(c => c.X);
                    var y2 = contour.Max(c => c.Y);
                    //Cv2.Rectangle(v, new Point(x1, y1), new Point(x2, y2), Scalar.Red, 2);
                    if (Math2.EuclideanDistance(x1, y1, x2, y2) > 25)
                    {
                        var r = new Rect(x1, y1, x2 - x1, y2 - y1);
                        var cell = new Mat(original, r);
                        this.SaveROI(cell);
                    }
                }

                var cnt1 = contours.Length;

                //Seuillage par bande de couleur
                lowerColor = new Scalar(163, 148, 0);
                higherColor = new Scalar(179, 255, 255);
                output = this.ColorThreshold(output, lowerColor, higherColor);

                //Ouverture
                output = this.Erode(7, output);
                output = this.Dilate(7, output);

                //Trouver les contours
                contours = this.FindContour(output);

                //Identifier les noyaux
                for (int i = 0; i < contours.Length; i++)
                    Cv2.DrawContours(original, contours, i, this.Colors[i % 5], -1);

                this.Save(original);

                var cnt2 = contours.Length;

                //var p = Path.Combine(TestDir, "count.txt");
                File.AppendAllText(@".\count.txt", this.TestContext.TestName + Environment.NewLine);
                File.AppendAllText(@".\count.txt", "cellules : " + cnt1 + Environment.NewLine + "noyaux : " + cnt2 + Environment.NewLine);

            }, 0);
        }

        [TestMethod]
        public void KmeanTest()
        {
            Bench.ComputeCpuMemoryUsage(() =>
            {
                //Lecture
                //Mat original = Cv2.ImRead(@".\Echantillon.png");
                //Mat original = Cv2.ImRead(@"D:\repos\Memoire\CancerCellDetection\ImageProcessingTests\bin\x64\Debug\0001Kmean.png");
                Mat original = Cv2.ImRead(@"D:\repos\Photos tests invasion-migration\17T Invasion 090617\17T M01 1 Inv_.jpg");

                Mat output = original.Clone();

                //Trouver les cerlces
                //var circles = this.DetectCircle(output, 20, 8, 11);
                ////Create matrice for the mask
                //this.DrawCircle(output, circles, 0);

                //Filtre k-mean
                output = this.Kmean(output, 10);

                //Seuillage par bande de couleur
                //Scalar lowerColor = new Scalar(163, 0, 0);
                //Scalar higherColor = new Scalar(180, 255, 255);
                Scalar lowerColor = new Scalar(0, 14, 0);
                Scalar higherColor = new Scalar(180, 255, 255);
                output = this.ColorThreshold(output, lowerColor, higherColor, 9, 11);


                //Trouver cellules ?
                var contours = this.FindContour(output);
                foreach (var contour in contours)
                {
                    var x1 = contour.Min(c => c.X);
                    var y1 = contour.Min(c => c.Y);
                    var x2 = contour.Max(c => c.X);
                    var y2 = contour.Max(c => c.Y);
                    //Cv2.Rectangle(v, new Point(x1, y1), new Point(x2, y2), Scalar.Red, 2);
                    if (Math2.EuclideanDistance(x1, y1, x2, y2) > 25)
                    {
                        var r = new Rect(x1, y1, x2 - x1, y2 - y1);
                        var cell = new Mat(original, r);
                        this.SaveROI(cell);
                    }
                }

                var cnt1 = contours.Length;

                //Seuillage par bande de couleur
                //lowerColor = new Scalar(163, 148, 0);
                //higherColor = new Scalar(179, 255, 255);
                lowerColor = new Scalar(0, 0, 38);
                higherColor = new Scalar(255, 255, 120);
                output = this.ColorThreshold(output, lowerColor, higherColor, 4, 4);


                //Trouver les contours
                contours = this.FindContour(output);

                //Identifier les noyaux
                for (int i = 0; i < contours.Length; i++)
                    Cv2.DrawContours(original, contours, i, this.Colors[i % 5], -1);

                this.Save(original);

                var cnt2 = contours.Length;

                //var p = Path.Combine(TestDir, "count.txt");
                File.AppendAllText(@".\count.txt", this.TestContext.TestName + Environment.NewLine);
                File.AppendAllText(@".\count.txt", "cellules : " + cnt1 + Environment.NewLine + "noyaux : " + cnt2 + Environment.NewLine);

            }, 0);
        }


        [TestMethod]
        public void ThresholdSimpleInPaintSpeedKmeanTest()
        {
            Bench.ComputeCpuMemoryUsage(() =>
            {
                //Lecture
                //Mat original = Cv2.ImRead(@".\Echantillon.png");
                Mat original = Cv2.ImRead(@"D:\repos\Memoire\CancerCellDetection\ImageProcessingTests\bin\x64\Debug\0002Kmean.png");

                Mat output = original.Clone();



                //Seuillage par bande de couleur
                //Scalar lowerColor = new Scalar(163, 0, 0);
                //Scalar higherColor = new Scalar(180, 255, 255);
                Scalar lowerColor = new Scalar(0, 100, 0);
                Scalar higherColor = new Scalar(180, 255, 255);
                output = this.ColorThreshold(output, lowerColor, higherColor, 0, 0);

                //Ouverture
                output = this.Erode(7, output);
                output = this.Dilate(7, output);

                //Trouver cellules ?
                var contours = this.FindContour(output);
                foreach (var contour in contours)
                {
                    var x1 = contour.Min(c => c.X);
                    var y1 = contour.Min(c => c.Y);
                    var x2 = contour.Max(c => c.X);
                    var y2 = contour.Max(c => c.Y);
                    //Cv2.Rectangle(v, new Point(x1, y1), new Point(x2, y2), Scalar.Red, 2);
                    if (Math2.EuclideanDistance(x1, y1, x2, y2) > 25)
                    {
                        var r = new Rect(x1, y1, x2 - x1, y2 - y1);
                        var cell = new Mat(original, r);
                        this.SaveROI(cell);
                    }
                }

                var cnt1 = contours.Length;

                //Seuillage par bande de couleur
                lowerColor = new Scalar(163, 148, 0);
                higherColor = new Scalar(179, 255, 255);
                output = this.ColorThreshold(output, lowerColor, higherColor);

                //Ouverture
                output = this.Erode(7, output);
                output = this.Dilate(7, output);

                //Trouver les contours
                contours = this.FindContour(output);

                //Identifier les noyaux
                for (int i = 0; i < contours.Length; i++)
                    Cv2.DrawContours(original, contours, i, this.Colors[i % 5], -1);

                this.Save(original);

                var cnt2 = contours.Length;

                //var p = Path.Combine(TestDir, "count.txt");
                File.AppendAllText(@".\count.txt", this.TestContext.TestName + Environment.NewLine);
                File.AppendAllText(@".\count.txt", "cellules : " + cnt1 + Environment.NewLine + "noyaux : " + cnt2 + Environment.NewLine);

            }, 0);
        }


        string Save(Mat v, [CallerMemberName]string function="")
        {
            this._counter++;
            string name = this._counter.ToString("D4") + function + ".png";
            var s = Path.Combine(this.TestDir, name);
            //Enregistrement de l'image de sortie
            Cv2.ImWrite(s, v);
            return s;
        }

        void SaveROI(Mat v)
        {
            string name = Guid.NewGuid() + ".png";
            var s = Path.Combine(this.TestRoiDir, name);

            Cv2.ImWrite(s, v);
        }

        public Mat ContrastCorrection(Mat v, double alpha)
        {
            //Chargement de l'image
            Mat output = new Mat();

            //Augmente le contraste de 10%
            v.ConvertTo(output, v.Depth(), alpha, 0);

            //Enregistrement de l'image de sortie
            this.Save(output);

            return output;
        }

        public Mat GammaCorrection(Mat v, double gamma)
        {
            //Chargement de l'image
            Mat output = new Mat();

            //Création de la table lut en fonction du facteur de correction gamma
            byte[] lookUpTable = new byte[256];
            for (int i = 0; i < 256; ++i)
                lookUpTable[i] = (byte)Math.Round(Math.Pow(i / 255.0, gamma) * 255.0);
            //Application de la correction gamma
            Cv2.LUT(v, lookUpTable, output);

            //Enregistrement de l'image de sortie
            this.Save(output);

            return output;
        }

        public Mat BilateralFilter(Mat v, int kernelSize)
        {
            Mat output = new Mat();
            
            //Bilateral blur
            Cv2.BilateralFilter(v, output, kernelSize, kernelSize * 2, kernelSize / 2);
            
            //Enregistrement de l'image de sortie
            this.Save(output);

            return output;
        }
        
        public CircleSegment[] DetectCircle(Mat v, int distanceMin=15, int radiusMin=13, int radiusMax=15)
        {
            Mat gray = new Mat();

            //Convert in gray
            Cv2.CvtColor(v, gray, ColorConversionCodes.BGR2GRAY);

            //Get circles from the gray image
            //src_gray: Input image(grayscale)
            //circles: A vector that stores sets of 3 values: for each detected circle.
            //CV_HOUGH_GRADIENT: Define the detection method.Currently this is the only one available in OpenCV
            //dp = 1: The inverse ratio of resolution
            //14.5 distance minimum entre les cercles
            //200 seuil haut pour la méthode de Canny 
            //100 Seuil pour la détection de centre
            //13 et 15 tailles minimum et maximum du rayon des cercles à détecter
            return  Cv2.HoughCircles(gray, HoughMethods.Gradient, 1, distanceMin, 200, 10, radiusMin, radiusMax);

        }

        public Mat DrawCircle(Mat v, CircleSegment[] circles, int increaseRadiusBy)
        {
            //Draw the circle in the mask
            foreach (var circle in circles)
                Cv2.Circle(v, (int)circle.Center.X, (int)circle.Center.Y, (int)circle.Radius + increaseRadiusBy, new Scalar(255,255,255), -1);

            this.Save(v);

            return v;
        }

        public Mat Inpaint(Mat v, Mat mask, int radius)
        {
            Mat output = new Mat();

            //Taille de kernel 
            Cv2.Inpaint(v, mask, output, 25, InpaintMethod.NS);

            //Enregistrement de l'image de sortie
            this.Save(output);

            return output;
        }

        private Mat ColorThreshold(Mat v, Scalar lowerColor, Scalar higherColor, int erodeSize=0, int dilateSize=0)
        {
            Mat output = new Mat();
            Mat mask = new Mat();

            Mat hsv = new Mat();
            //Convertion RGB vers HSB
            Cv2.CvtColor(v, hsv, ColorConversionCodes.BGR2HSV);
            this.Save(hsv);

            //Seuillage par bande de couleur
            Cv2.InRange(hsv, lowerColor, higherColor, mask);
            this.Save(mask);
            hsv.Dispose();

            mask = this.Erode(erodeSize, mask);

            mask = this.Dilate(dilateSize, mask);

            //Intersection du masque et de l'image originale
            Cv2.BitwiseAnd(v, v, output, mask);
            mask.Dispose();

            //Enregistrement de l'image de sortie
            this.Save(output);

            return output;
        }

        private Mat Dilate(int dilateSize, Mat v)
        {
            //Dilatation
            if (dilateSize > 0)
            {
                v = v.Dilate(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(dilateSize, dilateSize)));
                this.Save(v);
            }

            return v;
        }

        private Mat Erode(int erodeSize, Mat v)
        {
            //Erosion
            if (erodeSize > 0)
            {
                v = v.Erode(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(erodeSize, erodeSize)));
                this.Save(v);
            }

            return v;
        }

        public Point[][] FindContour(Mat v)
        {
            Mat output = new Mat();
            Mat gray = new Mat();
            Point[][] contours;
            HierarchyIndex[] hierarchy;

            //Convert in gray
            Cv2.CvtColor(v, gray, ColorConversionCodes.BGR2GRAY);

            //Filtre gaussien
            Cv2.GaussianBlur(gray, output, new Size(7, 7), 5, 5, BorderTypes.Default);
            this.Save(v);

            //Méthode de Otsu
            int thOtsu = (int)OtsuThresholding.Compute(BitmapConverter.ToBitmap(v));

            //Détecteur de contours de Canny
            Cv2.Canny(output, output, thOtsu / 3, thOtsu);
            this.Save(output);

            //Enumération des contours
            Cv2.FindContours(output, out contours, out hierarchy, RetrievalModes.List, ContourApproximationModes.ApproxTC89KCOS);

            ////Dessine les contours en vert
            //for (var i = 0; i < contours.Length; i++)
            //    Cv2.DrawContours(v, contours, i, new Scalar(0, 255, 0), 2);

            //this.Save(v);

            return contours;
        }
        
        public Mat Kmean(Mat v, int categoryCount)
        {
            Mat output = new Mat();
            
            //Réduction du nombre de couleur pour obtenir des "blobs"
            KMeans.Proceed(v, output, categoryCount, true, Scalar.Black);

            this.Save(output);

            return output;
        }
    }
}
