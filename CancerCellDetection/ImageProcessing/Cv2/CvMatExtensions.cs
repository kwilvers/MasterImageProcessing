using AR.Common.FrameWork.MathLib.Utilities;
using csmatio.io;
using csmatio.types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenCvSharp;
using Point = OpenCvSharp.Point;

namespace AR.Vision.FrameWork.TMap.ArMMT
{
    public static class CvMatExtensions
    {
        public static JaggedArray<float> ToJaggedArray(this MatOfFloat mat)
        {
            var floats = new float[mat.Width* mat.Height];

            mat.GetArray(0, 0, floats);

            return new JaggedArray<float>(mat.Width, mat.Height, floats);
        }

        public static JaggedArray<byte> ToJaggedArray(this MatOfByte mat)
        {
            var bytes = new byte[mat.Width * mat.Height];

            mat.GetArray(0, 0, bytes);

            return new JaggedArray<byte>(mat.Width, mat.Height, bytes);
        }

        public static MatOfFloat ToMatOfFloat(this JaggedArray<float> ja)
        {
            return new MatOfFloat(ja.Height, ja.Width, ja.ToArray());
        }

        public static MatOfFloat ToMatOfFloat(this Mat mat)
        {
            return new MatOfFloat(mat);
        }

        /// <summary>
        /// Conversion d'une matrice de bytes en floats SANS scaling
        /// </summary>
        public static MatOfFloat ToMatOfFloat(this MatOfByte matOfByte)
        {
            var bytes = matOfByte.ToArray();
            //var floats = bytes.Select(x => (float) x).ToArray();
            var floats = new float[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                floats[i] = bytes[i];
            }

            return new MatOfFloat(matOfByte.Height, matOfByte.Width, floats);
        }

        /// <summary>
        /// Conversion d'un bitmap en matrice de bytes SANS scaling
        /// </summary>
        public static MatOfFloat ToMatOfFloat(this Bitmap b)
        {
            if (b.PixelFormat != PixelFormat.Format8bppIndexed)
                throw new Exception("image must be PixelFormat.Format8bppIndexed");

            return new MatOfByte(b.ToMat()).ToMatOfFloat();
        }

        public static MatOfByte ToMatOfByte(this JaggedArray<byte> ja)
        {
            return new MatOfByte(ja.Height, ja.Width, ja.ToArray());
        }

        public static MatOfByte NanMask(this Mat matrix)
        {
            return new MatOfByte(matrix.NotEquals(matrix));
        }

        public static void NanToZero(this Mat matrix, MatOfByte nanMask)
        {
            using (var zeroMat = new MatOfFloat(matrix.Size(), 0))
            {
                zeroMat.CopyTo(matrix, nanMask);
            }
        }

        public static void ZeroToNaN(this Mat matrix, MatOfByte nanMask)
        {
            using (var nanMat = new MatOfFloat(matrix.Size(), float.NaN))
            {
                nanMat.CopyTo(matrix, nanMask);
            }
        }

        public static Mat Conv2(this Mat matrix, Mat kernel)
        {
            using (var nanMask = NanMask(matrix))
            {
                matrix.NanToZero(nanMask);

                var filtered = matrix.Filter2D(-1, kernel.Flip(FlipMode.XY), new Point(-1, -1), 0, BorderTypes.Constant);

                filtered.ZeroToNaN(nanMask);

                return filtered;
            }
        }

        public static void ToMatFile(this JaggedArray<float> jaggedArray, string fileName, string matName)
        {
            var mlMat = new MLSingle(matName, jaggedArray.Container);

            var mlList = new List<MLArray> { mlMat };

            var mfw = new MatFileWriter(fileName, mlList, false);
        }

        public static Rectangle BoundingRect(this Bitmap b)
        {
            using (var mat = b.ToMat())
            using (var points = new Mat())
            {
                Cv2.FindNonZero(mat, points);
                var rect = Cv2.BoundingRect(points);
                return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
            }
        }
    }
}
