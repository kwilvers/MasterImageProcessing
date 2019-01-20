using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Prism.Commands;
using Window = System.Windows.Window;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Mat originalImage;

        public HoughCircleParam CircleParam { get; set; }
        public BlobDetectorParams BlobParam { get; set; }

        public DelegateCommand<String> ChangeFileCommand { get; set; }
        public DelegateCommand OpenFileCommand { get; set; }


        public String FileName { get; set; }

        public MainWindow()
        {
            CircleParam = new HoughCircleParam();
            BlobParam = new BlobDetectorParams();
            ChangeFileCommand = new DelegateCommand<string>(ChangeFileName);
            OpenFileCommand = new DelegateCommand(OpenFile);

            FileName = @"..\..\..\..\images\Ech.PNG";

            InitializeComponent();

            CircleParam.PropertyChanged += CircleParam_PropertyChanged;
            BlobParam.PropertyChanged += BlobParam_PropertyChanged;

            //apply();
        }

        private void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                FileName = openFileDialog.FileName;

            originalImage = Cv2.ImRead(FileName, ImreadModes.Color);
            BitmapSource v = originalImage.ToBitmapSource();
            MyImage.Source = v;

            //var gray = image.CvtColor(ColorConversionCodes.BGR2GRAY);
            //Cv2.GaussianBlur(image, image, new OpenCvSharp.Size(9, 9), 2.0, 2.0);
        }

        private void ChangeFileName(string file)
        {
            FileName = file;
            applyBlob();
        }

        private void BlobParam_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            applyBlob();
        }

        private void CircleParam_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            apply();
        }

        private void apply()
        {
            //var fileName = @"..\..\..\..\images\Ech.PNG";


            double dp = CircleParam.Dp;
            double minDist = CircleParam.MinDist;
            double param1 = CircleParam.Param1;
            double param2 = CircleParam.Param2;
            int minRadius = CircleParam.MinRadius;
            int maxRadius = CircleParam.MaxRadius;

            var gray = originalImage.CvtColor(ColorConversionCodes.BGR2GRAY);
            var circles = Cv2.HoughCircles(gray, HoughMethods.Gradient, dp, minDist, param1, param2, minRadius, maxRadius);
            gray = null; 

            Mat image = originalImage.Clone();

            if (circles != null)
            {
                foreach (var c in circles)
                {
                    Cv2.Circle(image, (int)c.Center.X, (int)c.Center.Y, 3, new Scalar(0, 255, 0), -1, LineTypes.Link8, 0);

                    Cv2.Circle(image, (int)c.Center.X, (int)c.Center.Y, (int)c.Radius, new Scalar(0, 0, 255), 3, LineTypes.Link8, 0);
                }

            }

            BitmapSource v = image.ToBitmapSource();
            MyImage.Source = v;
            //this.pictureBox1.ImageIpl = image;
            //Cv2.ImShow("output", image);
            //Cv2.WaitKey(0);
        }

        //https://www.learnopencv.com/blob-detection-using-opencv-python-c/
        private void applyBlob()
        {
            //var fileName = @"..\..\..\..\images\Ech.PNG";

            var image = Cv2.ImRead(FileName, ImreadModes.Grayscale);

            var detector = SimpleBlobDetector.Create(BlobParam.Param);
            var keypoints = detector.Detect(image);
            Mat im_with_keypoints = new Mat();
            Cv2.DrawKeypoints(image, keypoints, im_with_keypoints, new Scalar(0, 0, 255), DrawMatchesFlags.DrawRichKeypoints);

            var v = im_with_keypoints.ToBitmapSource();
            MyImage.Source = v;
        }

    }
}
