using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ImageProcessing;
using ImageProcessing.Correction;
using ImageProcessing.Morphology;
using ImageProcessing.Segmentation;
using ImageProcessing.Smoothing;
using ImageProcessing.Thresholding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace ImageProcessingTests.Segmentation
{
    [TestClass()]
    public class PixelCountTests
    {
        [TestMethod]
        public void PixelCountBlack()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\echantillon.png");
            var res = GammaCorrection.Correct(v, 0.3);

            Color c = Color.FromArgb(200, 147, 233);
            var resThr = BandThresholdingFilter.Apply(res, c, 100, 60);
            var ero = Erode.Apply(resThr, new RoundStructuredElement(), 128);
            ero = Erode.Apply(ero, new RoundStructuredElement(), 128);
            var dil = Dilate.Apply(ero, new RoundStructuredElement(), 128);
            dil = Dilate.Apply(dil, new RoundStructuredElement(), 128);
            dil = Dilate.Apply(dil, new CrossStructuredElement(), 128);
            var sub = Morpho.Intersec(v, dil);

            double count = PixelCount.Count(sub);
            double black = PixelCount.Count(sub, Color.Black);
            double rest = count - black;

            double bp = black / count * 100;
            double rp = rest / count * 100;
        }

        [TestMethod]
        public void PixelCountBlack100()
        {
            Bitmap v = (Bitmap)Bitmap.FromFile(@".\bw.png");

            double count = PixelCount.Count(v);
            double black = PixelCount.Count(v, Color.Black);
            double rest = count - black;

            double bp = black / count * 100;
            double rp = rest / count * 100;
        }

        [TestMethod]
        public void PixelCountCV()
        {
            Mat v = Cv2.ImRead(@".\bw.png");

            var cnt = PixelCount.Count(v, Scalar.Black);
            Assert.AreEqual(100, cnt);
        }

        [TestMethod]
        public void PixelParallelCountCV()
        {
            Mat v = Cv2.ImRead(@".\bw.png");
            //Mat v = Cv2.ImRead(@"D:\repos\Photos tests invasion-migration\17T Invasion 090617\02ParallelApply.png");

            var cnt = PixelCount.ParallelCount(v, Scalar.Black);
            Assert.AreEqual(100, cnt);
        }


        public Dictionary<string, int> GetCsvLines(string path, bool firstLineContainsTitle)
        {
            Dictionary<string, int> dico = new Dictionary<string, int>(); 

            //Read the CSV file
            using (var reader = new StreamReader(path))
            {
                //Première ligne de titre
                if( firstLineContainsTitle)
                    reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line != null)
                    {
                        var values = line.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if( values.Length == 2 )
                            dico.Add(values[0], int.Parse(values[1]));
                    }
                }
            }

            return dico;
        }

        [TestMethod]
        public void CountPixelInDir()
        {
            var path = @"D:\repos\Photos tests invasion-migration";

            var directories = Directory.GetDirectories(path);

            //parcourt les répertoires
            foreach (var directory in directories)
            {
                //Retrouve les fichiers
                var files = Directory.GetFiles(directory, "*_.jpg");

                //Lit le fichier csv contenant les résultats connus
                var imageCount = this.GetCsvLines(Path.Combine(directory, "count.csv"), false);
                File.Delete(Path.Combine(directory, "dataset.csv"));
                File.AppendAllText(Path.Combine(directory, "dataset.csv"), "Name,Count,ColorIsolation,ColorThreshold,Gray,kmean" + Environment.NewLine);

                foreach (KeyValuePair<string, int> pair in imageCount)
                {
                    //Retrouve l'image à partir de la liste des fichiers connus
                    var imageName = files.FirstOrDefault(f => f.Contains(pair.Key));

                    if (imageName != null)
                    {
                        //Filtre gaussien
                        Mat v = Cv2.ImRead(imageName);

                        //Filtre gaussien 9x9
                        Cv2.GaussianBlur(v, v, new OpenCvSharp.Size(5, 5), 0, 0, BorderTypes.Default);

                        var res = new Mat();
                        //Filtre rouge bleu
                        //res = this.ColorIsolationCount(v, directory, pair.Key);
                        var cntCI = PixelCount.ParallelCount(res, Scalar.Black);

                        //Seuillage couleur
                        //res = this.ColorThresholdCount(v, directory, pair.Key);
                        var cntTHC = PixelCount.ParallelCount(res, Scalar.Black);

                        //Niveau de gris
                        //res = this.GrayThresholdCount(v, directory, pair.Key);
                        var cntGray = PixelCount.ParallelCount(res, Scalar.Black);

                        //Filtre kmean
                        res = this.KMeanThresholdCount(v, directory, pair.Key);
                        var cntkmean = PixelCount.ParallelCount(res, Scalar.Black);

                        //Niveau de gris

                        File.AppendAllText(Path.Combine(directory, "dataset.csv"), pair.Key + "," + pair.Value + "," + cntCI+ "," + cntTHC + "," + cntGray + "," + cntkmean + Environment.NewLine);
                    }

                    ///////////////////////////   a  bouger//////////////////////
                    ///////////////////////////   a  bouger//////////////////////
                    //return;
                }


                ///////////////////////////   a  bouger//////////////////////
                ///////////////////////////   a  bouger//////////////////////
                return;
            }
        }

        private Mat ColorThresholdCount(Mat v, string directory, string name)
        {
            Mat output = new Mat();
            Mat mask = new Mat();

            Mat hsv = new Mat();
            //Convertion RGB vers HSB
            Cv2.CvtColor(v, hsv, ColorConversionCodes.BGR2HSV);
            Cv2.ImWrite(Path.Combine(directory, @".\010" + name + ".png"), hsv);

            //Déclaration des seuil de couleurs HSB
            var lowerColor = new Scalar(0, 100, 0);
            var higherColor = new Scalar(180, 255, 255);

            //Seuillage par bande de couleur
            Cv2.InRange(hsv, lowerColor, higherColor, mask);
            Cv2.ImWrite(Path.Combine(directory, @".\020" + name + ".png"), mask);
            hsv.Dispose();

            //Erosion
            mask = mask.Erode(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(13, 13)));
            Cv2.ImWrite(Path.Combine(directory, @".\030" + name + ".png"), mask);
            //Dilatation
            mask = mask.Dilate(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(17, 17)));
            Cv2.ImWrite(Path.Combine(directory, @".\040" + name + ".png"), mask);

            //Intersection du masque et de l'image originale
            Cv2.BitwiseAnd(v, v, output, mask);
            mask.Dispose();

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(Path.Combine(directory, @".\050" + name + ".png"), output);

            return output;
        }

        private Mat GrayThresholdCount(Mat v, string directory, string name)
        {
            Mat output = new Mat();
            Mat mask = new Mat();

            Mat gray = new Mat();
            //Convertion RGB vers HSB
            Cv2.CvtColor(v, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.ImWrite(Path.Combine(directory, @".\110"+name+".png"), gray);


            //Seuillage gris
            Cv2.Threshold(gray, output, 120, 255, ThresholdTypes.TozeroInv);
            Cv2.ImWrite(Path.Combine(directory, @".\120" + name + ".png"), output);
            gray.Dispose();

            return output;
        }

        private Mat KMeanThresholdCount(Mat v, string directory, string name)
        {
            Mat output = new Mat();
            Mat mask = new Mat();

            Mat hsv = new Mat();
            //Convertion RGB vers HSB
            Cv2.CvtColor(v, hsv, ColorConversionCodes.BGR2HSV);
            Cv2.ImWrite(Path.Combine(directory, @".\210" + name + ".png"), hsv);

            //Déclaration des seuil de couleurs HSB
            var lowerColor = new Scalar(0, 100, 0);
            var higherColor = new Scalar(180, 255, 255);

            //Seuillage par bande de couleur
            Cv2.InRange(hsv, lowerColor, higherColor, mask);
            Cv2.ImWrite(Path.Combine(directory, @".\220" + name + ".png"), mask);
            hsv.Dispose();

            //Erosion
            mask = mask.Erode(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(13, 13)));
            Cv2.ImWrite(Path.Combine(directory, @".\230" + name + ".png"), mask);
            //Dilatation
            mask = mask.Dilate(Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(17, 17)));
            Cv2.ImWrite(Path.Combine(directory, @".\240" + name + ".png"), mask);

            //Intersection du masque et de l'image originale
            Cv2.BitwiseAnd(v, v, output, mask);

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(Path.Combine(directory, @".\250" + name + ".png"), output);

            //Calcul du Kmean
            KMeans.Proceed(output, mask, 2, true, Scalar.Black);

            //Enregistrement de l'image de sortie
            Cv2.ImWrite(Path.Combine(directory, @".\260" + name + ".png"), mask);

            return mask;
        }

        private Mat ColorIsolationCount(Mat v, string directory, string name)
        {
            //MARCHE PAS...
            var res = ColorIsolation.ParallelIsolate(v, false, true, false);
            Cv2.ImWrite(Path.Combine(directory, @".\310" + name + ".png"), res);
            res = ZeroThresholdingFilter.ParallelApply(res, 160, true);
            Cv2.ImWrite(Path.Combine(directory, @".\320" + name + ".png"), res);

            return res;
        }
    }
}