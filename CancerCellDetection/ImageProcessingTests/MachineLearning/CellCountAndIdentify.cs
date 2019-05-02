using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using Point = OpenCvSharp.Point;

namespace ImageProcessingTests.MachineLearning
{
    [TestClass]
    public class CellCountAndIdentify
    {
        [TestMethod]
        public void GetMetadata()
        {
            //Image image = new Bitmap(@".\metadata.png");
            Image image = new Bitmap(@"C: \Users\karl\Pictures\Untitled.png");
            // Get the PropertyItems property from image.
            PropertyItem[] propItems = image.PropertyItems;
            int count = 0;

            foreach (var propItem in propItems)
            {
                Console.WriteLine("Property Item " + count.ToString());
                Console.WriteLine("   iD: 0x" + propItem.Id.ToString("x"));
                Console.WriteLine("   type: " + propItem.Type.ToString());
                Console.WriteLine("   length: " + propItem.Len.ToString() + " bytes");

                if (propItem.Id == 0xc697) this.ShowData(propItem.Value);

                count++;
            }
        }

        private void ShowData(byte[] propItemValue)
        {
            Console.WriteLine("Skip 140 bytes ");
            for (int i = 140; i < propItemValue.Length; i+=4)
            {
                byte[] b = new byte[4];
                Buffer.BlockCopy(propItemValue, i, b, 0, 4);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(b);

                int value = BitConverter.ToInt32(b, 0);
                Console.WriteLine("int: {0}", value);
            }
        }

        [TestMethod]
        public void ExportCells()
        {
            //Read the image
            Mat v = Cv2.ImRead(@".\metadata.png");
            List<Point> listPoints = new List<Point>();
            
            //Read the CSV file
            using (var reader = new StreamReader(@".\metadata.csv"))
            {
                //Première ligne de titre
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line != null)
                    {
                        var values = line.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        listPoints.Add(new Point(int.Parse( values[1]), int.Parse(values[2])));
                    }
                }
            }

            //Parcourt des points extrait du fichier CSV
            foreach (var p in listPoints)
            {
                //Obtient un rectangle de la taille de la région d'intéret (ROI)
                Rect r = this.GetROI(p, v.Height, v.Width);
                //Decoupe la ROI
                var cell = new Mat(v, r);
                //Enregistrement de la ROI dans une image individuelle
                Cv2.ImWrite(@".\metadata\" + p.GetHashCode() + ".png", cell);
            }

            //Affiche sur l'image d'origine les rectangles
            foreach (var p in listPoints)
            {
                var pt1 = GetTopLeftPoint(p);
                var pt2 = GetBottomRightPoint(p, v.Height, v.Width);
                Cv2.Rectangle(v, pt1, pt2, Scalar.Red, 2);
                Cv2.Circle(v, p, 2, Scalar.Red, 2);
            }

            //Save l'image
            Cv2.ImWrite(@".\rectangle.png", v);
        }

        private Rect GetROI(Point p, int vHeight, int vWidth)
        {
            var pt1 = GetTopLeftPoint(p);

            var rec = new Rect(pt1.X, pt1.Y, 150, 150);

            if (rec.Right > vWidth)
                rec.Width = rec.Width - (rec.Right - vWidth) - 1;

            if (rec.Bottom > vHeight)
                rec.Height = rec.Height - (rec.Bottom - vHeight) - 1;

            return rec;
        }

        private static Point GetTopLeftPoint(Point p)
        {
            var pt = new Point(p.X - 75, p.Y - 75);
            if (pt.X < 0)
                pt.X = 0;
            if (pt.Y < 0)
                pt.Y = 0;
            return pt;
        }

        private static Point GetBottomRightPoint(Point p, int vHeight, int vWidth)
        {
            var pt = new Point(p.X + 75, p.Y + 75);
            if (pt.X > vWidth)
                pt.X = vWidth;
            if (pt.Y > vHeight)
                pt.Y = vHeight;
            return pt;
        }
    }
}
