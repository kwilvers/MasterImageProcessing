using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.UserInterface;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            apply();
        }

        private void apply()
        {
            //let fileName = "C:\Users\karl\OneDrive\ALL\UNIF\2eme - Mémoire\Echantillon.PNG"
            var fileName = @"C:\Dev\Memoire\images\Echantillon.PNG";

            var image = Cv2.ImRead(fileName, ImreadModes.GrayScale);
            var output = InputOutputArray.Create(image.Clone());
            //let gray = Cv2.CvtColor(image, output, ColorConversionCodes.BGR2GRAY)
            var input = InputArray.Create(image);
            Cv2.GaussianBlur(image, image, new OpenCvSharp.Size(9, 9), 2.0, 2.0);
            //let circles = Cv2.HoughCircles(input, HoughMethods.Gradient, 1.0, 5.0, 200.0, 100.0, 0, 0)
            input = InputArray.Create(output.GetMat());

            double dp = (double)upDp.Value;
            double minDist = tbMinDist.Value;
            double param1 = tbParam1.Value;
            double param2 = tbParam2.Value;
            int minRadius = tbMinRadius.Value;
            int maxRadius = tbMaxRadius.Value;

            var circles = Cv2.HoughCircles(image, HoughMethods.Gradient, dp, minDist, param1, param2, minRadius, maxRadius);
            //let lines = Cv2.HoughLines(output, 1.0, Math.PI/180.0, 50, 0.0, 0.0)

            if (circles != null)
            {
                foreach (var c in circles)
                {
                    //Point center(Cv2.cvRound(c), cvRound(circles[i][1]));
                    //int radius = c.Radius;
                    // circle center 
                    Cv2.Circle(image, (int)c.Center.X, (int)c.Center.Y, 3, new Scalar(0, 255, 0), -1, LineTypes.Link8, 0);
                    // circle outline 
                    Cv2.Circle(image, (int)c.Center.X, (int)c.Center.Y, (int)c.Radius, new Scalar(0, 0, 255), 3, LineTypes.Link8, 0);
                    //circle( src, center, radius, Scalar(0,0,255), 3, 8, 0 );

                }
                //Cv2.Circle(image, Convert.ToInt32(c.Center.X), Convert.ToInt32(c.Center.Y), Convert.ToInt32(c.Radius), new Scalar(0.0, 255.0, 0.0), 4, LineTypes.AntiAlias, 0);

            }

            this.pictureBox1.ImageIpl = image;
            //Cv2.ImShow("output", image);
            //Cv2.WaitKey(0);
        }

        private void applyBlob()
        {
            //let fileName = "C:\Users\karl\OneDrive\ALL\UNIF\2eme - Mémoire\Echantillon.PNG"
            var fileName = @"C:\Dev\Memoire\images\Echantillon.PNG";

            var image = Cv2.ImRead(fileName, ImreadModes.GrayScale);


            //# Setup SimpleBlobDetector parameters.
            var param = new SimpleBlobDetector.Params();

            param.FilterByColor = cbByColor.Checked;
            param.BlobColor = 255;

            //# Change thresholds;
            param.MinThreshold = tbMinThreshold.Value; //10
            param.MaxThreshold = tbMaxThreshold.Value; //200

            //# Filter by Area.
            param.FilterByArea = cbByArea.Checked;
            param.MinArea = tbMinArea.Value;
            param.MaxArea = tbMaxArea.Value;

            //# Filter by Circularity
            param.FilterByCircularity = cbByCircularity.Checked;
            param.MinCircularity = 0.1f;

            // Filter by Convexity
            param.FilterByConvexity = cbByConvexity.Checked;
            param.MinConvexity = 0.87f;

            // Filter by Inertia
            param.FilterByInertia = cbByInertia.Checked;
            param.MinInertiaRatio = 0.01f;

            var detector = SimpleBlobDetector.Create(param);
            var keypoints = detector.Detect(image);
            Mat im_with_keypoints = new Mat();
            Cv2.DrawKeypoints(image, keypoints, im_with_keypoints, new Scalar(0, 0, 255), DrawMatchesFlags.DrawRichKeypoints);

            this.pictureBox1.ImageIpl = im_with_keypoints;
        }

        private void tb_Scroll(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            apply();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //http://www.learnopencv.com/blob-detection-using-opencv-python-c/
            applyBlob();
        }
    }
}
