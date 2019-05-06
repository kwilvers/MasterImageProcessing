using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using SystemExpert;
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
        Mat HSVImage;

        public HoughCircleParam CircleParam { get; set; }
        public BlobDetectorParams BlobParam { get; set; }
        public InRangeParam RangeParam { get; set; }

        public DelegateCommand<String> ChangeFileCommand { get; set; }
        public DelegateCommand OpenFileCommand { get; set; }

        public DelegateCommand CopyCommand { get; set; }

        public String FileName { get; set; }

        public MainWindow()
        {
            this.CircleParam = new HoughCircleParam();
            this.BlobParam = new BlobDetectorParams();
            this.RangeParam = new InRangeParam();
            this.ChangeFileCommand = new DelegateCommand<string>(this.ChangeFileName);
            this.OpenFileCommand = new DelegateCommand(this.OpenFile);
            this.CopyCommand = new DelegateCommand(this.CopyToClipboard);

            this.FileName = @"..\..\..\..\images\Ech.PNG";

            this.InitializeComponent();

            this.CircleParam.PropertyChanged += this.CircleParam_PropertyChanged;
            this.BlobParam.PropertyChanged += this.BlobParam_PropertyChanged;
            this.RangeParam.Low.PropertyChanged += this.RangeParam_PropertyChanged;
            this.RangeParam.High.PropertyChanged += this.RangeParam_PropertyChanged;
            //ApplyCircle();
        }

        private void CopyToClipboard()
        {
            Clipboard.SetImage((BitmapSource) this.MyImage.Source);
            bool b = Clipboard.ContainsImage();
        }

        private void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true) this.FileName = openFileDialog.FileName;

            this.originalImage = Cv2.ImRead(this.FileName, ImreadModes.Color);
            this.HSVImage = new Mat();
            Cv2.CvtColor(this.originalImage, this.HSVImage, ColorConversionCodes.RGB2HSV);

            BitmapSource v = this.originalImage.ToBitmapSource();
            this.MyImage.Source = v;

            //var gray = image.CvtColor(ColorConversionCodes.BGR2GRAY);
            //Cv2.GaussianBlur(image, image, new OpenCvSharp.Size(9, 9), 2.0, 2.0);
        }

        private void ChangeFileName(string file)
        {
            this.FileName = file;
            this.ApplyBlob();
        }

        private void BlobParam_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.ApplyBlob();
        }

        private void CircleParam_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.ApplyCircle();
        }

        private void ApplyCircle()
        {
            if (this.originalImage == null)
                return; 

            //var fileName = @"..\..\..\..\images\Ech.PNG";
            double dp = this.CircleParam.Dp;
            double minDist = this.CircleParam.MinDist;
            double param1 = this.CircleParam.Param1;
            double param2 = this.CircleParam.Param2;
            int minRadius = this.CircleParam.MinRadius;
            int maxRadius = this.CircleParam.MaxRadius;

            var gray = this.originalImage.CvtColor(ColorConversionCodes.BGR2GRAY);
            var circles = Cv2.HoughCircles(gray, HoughMethods.Gradient, dp, minDist, param1, param2, minRadius, maxRadius);
            gray = null; 

            Mat image = this.originalImage.Clone();

            if (circles != null)
            {
                foreach (var c in circles)
                {
                    Cv2.Circle(image, (int)c.Center.X, (int)c.Center.Y, 3, new Scalar(0, 255, 0), -1, LineTypes.Link8, 0);

                    Cv2.Circle(image, (int)c.Center.X, (int)c.Center.Y, (int)c.Radius, new Scalar(0, 0, 255), 3, LineTypes.Link8, 0);
                }
            }

            BitmapSource v = image.ToBitmapSource();
            this.MyImage.Source = v;
            //this.pictureBox1.ImageIpl = image;
            //Cv2.ImShow("output", image);
            //Cv2.WaitKey(0);
        }

        //https://www.learnopencv.com/blob-detection-using-opencv-python-c/
        private void ApplyBlob()
        {
            var image = Cv2.ImRead(this.FileName, ImreadModes.Color);

            Mat gray = new Mat();
            Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);

            //Créer les paramètres de détection de blob
            var detector = SimpleBlobDetector.Create(this.BlobParam.Param);
            //Détection des blobs
            var keypoints = detector.Detect(gray);
            Mat im_with_keypoints = new Mat();
            //Dessine les cercles représentant les blobs
            
            Cv2.DrawKeypoints(image, keypoints, im_with_keypoints, new Scalar(0, 0, 0), DrawMatchesFlags.DrawRichKeypoints);

            foreach (var point in keypoints)
            {
                Cv2.Circle(im_with_keypoints, (int)point.Pt.X, (int)point.Pt.Y, (int)point.Size / 2, new Scalar(0, 255, 0), 2);
            }

            var v = im_with_keypoints.ToBitmapSource();
            this.MyImage.Source = v;

        }

        private void RangeParam_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ApplyInRange();
        }

        private void ApplyInRange()
        {
            Mat output = new Mat();
            Mat mask = new Mat();

            //Déclaration des seuils
            var lowher_color = new Scalar(this.RangeParam.Low.H, this.RangeParam.Low.S, this.RangeParam.Low.V);
            var higher_color = new Scalar(this.RangeParam.High.H, this.RangeParam.High.S, this.RangeParam.High.V);
            //Seuillage par couleur et création du masque
            Cv2.InRange(this.HSVImage, lowher_color, higher_color, mask);
            //Opération de masquage pour ne conserver que les pixels de seuillés
            Cv2.BitwiseAnd(this.originalImage, this.originalImage, output, mask);

            BitmapSource v = output.ToBitmapSource();
            this.MyImage.Source = v;
        }
    }
}
