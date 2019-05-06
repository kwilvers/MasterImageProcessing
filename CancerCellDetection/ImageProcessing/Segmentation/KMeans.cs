using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenCvSharp;

namespace ImageProcessing.Segmentation
{
    public class KMeans
    {
        public static void Proceed(Mat input, Mat result, int k, bool skipBackground=false, Scalar background = default(Scalar))
        {
            using (Mat points = new Mat())
            {
                using (Mat labels = new Mat())
                {
                    using (Mat centers = new Mat())
                    {
                        int width = input.Cols;
                        int height = input.Rows;

                        points.Create(width * height, 1, MatType.CV_32FC3);
                        centers.Create(k, 1, points.Type());
                        result.Create(height, width, input.Type());

                        int i = 0;
                        for(int y = 0; y < height; y ++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                var v = input.At<Vec3b>(y, x);
                                //SI background continue
                                if (skipBackground && v.Item0 == (byte)background.Val0
                                    && v.Item1 == (byte)background.Val1 && v.Item2 == (byte)background.Val2)
                                {
                                    continue;
                                }

                                Vec3f vec3f = new Vec3f
                                {
                                    Item0 = v.Item0,
                                    Item1 = v.Item1,
                                    Item2 = v.Item2
                                };

                                points.Set<Vec3f>(i, vec3f);
                                i++;
                            }
                        }

                        Mat points2 = new Mat();
                        points2.Create(i, 1, MatType.CV_32FC3);
                        for(int j=0; j<i; j++)
                            points2.Set<Vec3f>(j, points.At<Vec3f>(j));

                        //Calcul du kmean
                        Cv2.Kmeans(points2, k, labels, new TermCriteria(CriteriaType.Eps | CriteriaType.MaxIter, 10, 1.0), 3, KMeansFlags.RandomCenters, centers);

                        i = 0;
                        //Application des couleurs des centroïdes
                        for(int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                var v = input.At<Vec3b>(y, x);

                                //SI background set la valeur du backgroud dans le résultatcontinue
                                if (skipBackground && v.Item0 == (byte)background.Val0
                                                   && v.Item1 == (byte)background.Val1 && v.Item2 == (byte)background.Val2)
                                {
                                    result.Set<Vec3b>(y, x, v);
                                    continue;
                                }

                                int idx = labels.Get<int>(i);

                                Vec3b vec3b = new Vec3b();

                                int tmp = Convert.ToInt32(Math.Round(centers.At<Vec3f>(idx).Item0));
                                tmp = tmp > 255 ? 255 : tmp < 0 ? 0 : tmp;
                                vec3b.Item0 = Convert.ToByte(tmp);

                                tmp = Convert.ToInt32(Math.Round(centers.At<Vec3f>(idx).Item1));
                                tmp = tmp > 255 ? 255 : tmp < 0 ? 0 : tmp;
                                vec3b.Item1 = Convert.ToByte(tmp);

                                tmp = Convert.ToInt32(Math.Round(centers.At<Vec3f>(idx).Item2));
                                tmp = tmp > 255 ? 255 : tmp < 0 ? 0 : tmp;
                                vec3b.Item2 = Convert.ToByte(tmp);

                                result.Set<Vec3b>(y, x, vec3b);
                                i++;
                            }
                        }
                    }
                }
            }

        }

    }
}
