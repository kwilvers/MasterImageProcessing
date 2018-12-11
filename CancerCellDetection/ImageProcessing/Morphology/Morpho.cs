using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.Morphology
{
    public static class Morpho
    {
        public static Bitmap Sub(Bitmap sourceBitmapA, Bitmap sourceBitmapB)
        {
            BitmapData sourceDataA = sourceBitmapA.LockBits(new Rectangle(0, 0, sourceBitmapA.Width, sourceBitmapA.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            BitmapData sourceDataB = sourceBitmapB.LockBits(new Rectangle(0, 0, sourceBitmapA.Width, sourceBitmapA.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);


            byte[] pixelBufferA = new byte[sourceDataA.Stride * sourceDataA.Height];
            byte[] pixelBufferB = new byte[sourceDataA.Stride * sourceDataA.Height];
            byte[] resultBuffer = new byte[sourceDataA.Stride * sourceDataA.Height];

            Marshal.Copy(sourceDataA.Scan0, pixelBufferA, 0, pixelBufferA.Length);
            Marshal.Copy(sourceDataB.Scan0, pixelBufferB, 0, pixelBufferB.Length);
            Marshal.Copy(sourceDataA.Scan0, resultBuffer, 0, pixelBufferA.Length);

            sourceBitmapA.UnlockBits(sourceDataA);
            sourceBitmapB.UnlockBits(sourceDataB);

            for (int i = 0; i < resultBuffer.Length; i ++)
            {
                var res = pixelBufferA[i] - pixelBufferB[i];
                resultBuffer[i] = res == 0 ? (byte)0 : (byte)255;
            }


            Bitmap resultBitmap = new Bitmap(sourceBitmapA.Width, sourceBitmapA.Height);


            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }

        public static Bitmap Add(Bitmap sourceBitmapA, Bitmap sourceBitmapB)
        {
            BitmapData sourceDataA = sourceBitmapA.LockBits(new Rectangle(0, 0, sourceBitmapA.Width, sourceBitmapA.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            BitmapData sourceDataB = sourceBitmapB.LockBits(new Rectangle(0, 0, sourceBitmapA.Width, sourceBitmapA.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);


            byte[] pixelBufferA = new byte[sourceDataA.Stride * sourceDataA.Height];
            byte[] pixelBufferB = new byte[sourceDataA.Stride * sourceDataA.Height];
            byte[] resultBuffer = new byte[sourceDataA.Stride * sourceDataA.Height];

            Marshal.Copy(sourceDataA.Scan0, pixelBufferA, 0, pixelBufferA.Length);
            Marshal.Copy(sourceDataB.Scan0, pixelBufferB, 0, pixelBufferB.Length);
            Marshal.Copy(sourceDataA.Scan0, resultBuffer, 0, pixelBufferA.Length);

            sourceBitmapA.UnlockBits(sourceDataA);
            sourceBitmapB.UnlockBits(sourceDataB);

            for (int i = 0; i < resultBuffer.Length; i++)
            {
                var res = pixelBufferA[i] + pixelBufferB[i];
                resultBuffer[i] = res == 0 ? (byte)0 : (byte)255;
            }


            Bitmap resultBitmap = new Bitmap(sourceBitmapA.Width, sourceBitmapA.Height);


            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                    resultBitmap.Width, resultBitmap.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }

        public static Bitmap Intersec(Bitmap source, Bitmap mask)
        {
            BitmapData sourceData = source.LockBits(new Rectangle(0, 0, source.Width, source.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            BitmapData maskData = mask.LockBits(new Rectangle(0, 0, source.Width, source.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);


            byte[] sourceBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] maskBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, sourceBuffer.Length);
            Marshal.Copy(maskData.Scan0, maskBuffer, 0, maskBuffer.Length);
            Marshal.Copy(sourceData.Scan0, resultBuffer, 0, sourceBuffer.Length);

            source.UnlockBits(sourceData);
            mask.UnlockBits(maskData);

            for (int i = 0; i < resultBuffer.Length; i++)
            {
                //var res = pixelBufferA[i] - pixelBufferB[i];
                //resultBuffer[i] = res == 0 ? (byte)0 : (byte)255;
                resultBuffer[i] = (byte) (maskBuffer[i] > 0 ? sourceBuffer[i] : 0);
            }


            Bitmap resultBitmap = new Bitmap(source.Width, source.Height);


            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                    resultBitmap.Width, resultBitmap.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }

    }
}
