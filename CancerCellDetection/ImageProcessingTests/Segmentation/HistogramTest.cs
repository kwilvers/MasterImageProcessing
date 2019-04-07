using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace ImageProcessingTests.Segmentation
{
    [TestClass]
    public class HistogramTest
    {
        [TestMethod]
        public void ShowHistogramTest()
        {
            Mat v = Cv2.ImRead(@".\echantillon.png");
            Mat[] bgr_planes = new Mat[3];
            Cv2.Split(v, out bgr_planes);

            /// Establish the number of bins
            //var histSize = new int[] { 256, 256, 256 };
            var histSize = new int[] { 256 };

            /// Set the ranges ( for B,G,R) )
            //var range = new Rangef[] { new Rangef(0, 255), new Rangef(0, 255), new Rangef(0, 255) };
            var range = new Rangef[] { new Rangef(0, 255) };

            bool uniform = true; bool accumulate = false;

            var b_hist = new Mat();
            var g_hist = new Mat();
            var r_hist = new Mat(); ;
            //var channels = new int[] { 0, 0, 0 };
            var channels = new int[] { 0 };

            /// Compute the histograms:
            Cv2.CalcHist(new Mat[] { bgr_planes[0] }, channels, new Mat(), b_hist, 1, histSize, range, true, false);
            Cv2.CalcHist(new Mat[] { bgr_planes[1] }, channels, new Mat(), g_hist, 1, histSize, range, true, false);
            Cv2.CalcHist(new Mat[] { bgr_planes[2] }, channels, new Mat(), r_hist, 1, histSize, range, true, false);

            // Draw the histograms for B, G and R
            int hist_w = 512;
            int hist_h = 400;
            int bin_w = (int)Math.Round((double)hist_w / (double)histSize[0]);

            Mat histImage = new Mat(hist_h, hist_w, MatType.CV_8UC3, new Scalar( 0,0,0) );


            /// Normalize the result to [ 0, histImage.rows ]
            Cv2.Normalize(b_hist, b_hist, 0, histImage.Rows, NormTypes.MinMax, -1, new Mat());
            Cv2.Normalize(g_hist, g_hist, 0, histImage.Rows, NormTypes.MinMax, -1, new Mat());
            Cv2.Normalize(r_hist, r_hist, 0, histImage.Rows, NormTypes.MinMax, -1, new Mat());


            /// Draw for each channel
            for (int i = 1; i < histSize[0]; i++)
            {
                Cv2.Line(histImage,
                    (int) (bin_w * (i - 1)), (int) (hist_h - Math.Round(b_hist.At<float>(i - 1))),
                    (int) (bin_w * (i)), (int) (hist_h - Math.Round(b_hist.At<float>(i))),
                    new Scalar(255, 0, 0), 1, LineTypes.Link8, 0);

                Cv2.Line(histImage,
                    (int)(bin_w * (i - 1)), (int)(hist_h - Math.Round(g_hist.At<float>(i - 1))),
                    (int)(bin_w * (i)), (int)(hist_h - Math.Round(g_hist.At<float>(i))),
                    new Scalar(0, 255, 0), 1, LineTypes.Link8, 0);

                Cv2.Line(histImage,
                    (int)(bin_w * (i - 1)), (int)(hist_h - Math.Round(r_hist.At<float>(i - 1))),
                    (int)(bin_w * (i)), (int)(hist_h - Math.Round(r_hist.At<float>(i))),
                    new Scalar(0, 0, 255), 1, LineTypes.Link8, 0);

                Cv2.ImWrite(@".\histogram.png", histImage);
            }

        }

        /*
         *
         int main( int argc, char** argv )
            {
              Mat src, dst;

              /// Load image
              src = imread( argv[1], 1 );

              if( !src.data )
                { return -1; }

              /// Separate the image in 3 places ( B, G and R )
              vector<Mat> bgr_planes;
              split( src, bgr_planes );

              /// Establish the number of bins
              int histSize = 256;

              /// Set the ranges ( for B,G,R) )
              float range[] = { 0, 256 } ;
              const float* histRange = { range };

              bool uniform = true; bool accumulate = false;

              Mat b_hist, g_hist, r_hist;

              /// Compute the histograms:
              calcHist( &bgr_planes[0], 1, 0, Mat(), b_hist, 1, &histSize, &histRange, uniform, accumulate );
              calcHist( &bgr_planes[1], 1, 0, Mat(), g_hist, 1, &histSize, &histRange, uniform, accumulate );
              calcHist( &bgr_planes[2], 1, 0, Mat(), r_hist, 1, &histSize, &histRange, uniform, accumulate );

              // Draw the histograms for B, G and R
              int hist_w = 512; int hist_h = 400;
              int bin_w = cvRound( (double) hist_w/histSize );

              Mat histImage( hist_h, hist_w, CV_8UC3, Scalar( 0,0,0) );

              /// Normalize the result to [ 0, histImage.rows ]
              normalize(b_hist, b_hist, 0, histImage.rows, NORM_MINMAX, -1, Mat() );
              normalize(g_hist, g_hist, 0, histImage.rows, NORM_MINMAX, -1, Mat() );
              normalize(r_hist, r_hist, 0, histImage.rows, NORM_MINMAX, -1, Mat() );

  /// Draw for each channel
  for( int i = 1; i < histSize; i++ )
  {
      line( histImage, Point( bin_w*(i-1), hist_h - cvRound(b_hist.at<float>(i-1)) ) ,
                       Point( bin_w*(i), hist_h - cvRound(b_hist.at<float>(i)) ),
                       Scalar( 255, 0, 0), 2, 8, 0  );
      line( histImage, Point( bin_w*(i-1), hist_h - cvRound(g_hist.at<float>(i-1)) ) ,
                       Point( bin_w*(i), hist_h - cvRound(g_hist.at<float>(i)) ),
                       Scalar( 0, 255, 0), 2, 8, 0  );
      line( histImage, Point( bin_w*(i-1), hist_h - cvRound(r_hist.at<float>(i-1)) ) ,
                       Point( bin_w*(i), hist_h - cvRound(r_hist.at<float>(i)) ),
                       Scalar( 0, 0, 255), 2, 8, 0  );
  }

  /// Display
  namedWindow("calcHist Demo", CV_WINDOW_AUTOSIZE );
  imshow("calcHist Demo", histImage );

  waitKey(0);

  return 0;
}
         */
    }
}
