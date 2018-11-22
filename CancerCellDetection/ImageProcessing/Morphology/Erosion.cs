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
    public static class Erosion
    {

        public static Bitmap Apply(
                           this Bitmap sourceBitmap,
                           int matrixSize,
                           bool applyBlue = true,
                           bool applyGreen = true,
                           bool applyRed = true)
        {
            BitmapData sourceData =
                       sourceBitmap.LockBits(new Rectangle(0, 0,
                       sourceBitmap.Width, sourceBitmap.Height),
                       ImageLockMode.ReadOnly,
                       PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            
            byte[] resultBuffer = new byte[sourceData.Stride *sourceData.Height];
            
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0,pixelBuffer.Length);
            
            sourceBitmap.UnlockBits(sourceData);
            
            int filterOffset = (matrixSize - 1) / 2;
           
           
            for (int offsetY = filterOffset; offsetY <
                sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX <
                    sourceBitmap.Width - filterOffset; offsetX++)
                {
                    var byteOffset = offsetY *
                                     sourceData.Stride +
                                     offsetX * 4;
                    
                    byte blue = 255;
                    byte green = 255;
                    byte red = 255;

                    for (int filterY = -filterOffset;
                        filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset;
                            filterX <= filterOffset; filterX++)
                        {
                            var calcOffset = byteOffset +
                                             (filterX * 4) +
                                             (filterY * sourceData.Stride);


                            if (pixelBuffer[calcOffset] < blue)
                                blue = pixelBuffer[calcOffset];

                            if (pixelBuffer[calcOffset + 1] < green)
                                green = pixelBuffer[calcOffset + 1];

                            if (pixelBuffer[calcOffset + 2] < red)
                                red = pixelBuffer[calcOffset + 2];
                        }
                    }

                    if (applyBlue == false)
                        blue = pixelBuffer[byteOffset];

                    if (applyGreen == false)
                        green = pixelBuffer[byteOffset + 1];

                    if (applyRed == false)
                        red = pixelBuffer[byteOffset + 2];

                    resultBuffer[byteOffset] = blue;
                    resultBuffer[byteOffset + 1] = green;
                    resultBuffer[byteOffset + 2] = red;
                    resultBuffer[byteOffset + 3] = 255;
                }
            }


            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width,
                                             sourceBitmap.Height);


            BitmapData resultData =
                       resultBitmap.LockBits(new Rectangle(0, 0,
                       resultBitmap.Width, resultBitmap.Height),
                       ImageLockMode.WriteOnly,
                       PixelFormat.Format32bppArgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0,
                                       resultBuffer.Length);


            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }


        public static Bitmap Apply(
                   this Bitmap sourceBitmap,
                   int matrixSize, 
                   byte threshold)
        {
            BitmapData sourceData =
                       sourceBitmap.LockBits(new Rectangle(0, 0,
                       sourceBitmap.Width, sourceBitmap.Height),
                       ImageLockMode.ReadOnly,
                       PixelFormat.Format24bppRgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];

            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);

            int filterOffset = (matrixSize - 1) / 2;


            for (int offsetY = filterOffset; offsetY <
                sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX <
                    sourceBitmap.Width - filterOffset; offsetX++)
                {
                    var byteOffset = offsetY *
                                     sourceData.Stride +
                                     offsetX * 3;

                    byte blue = threshold;
                    byte green = threshold;
                    byte red = threshold;

                    for (int filterY = -filterOffset;
                        filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset;
                            filterX <= filterOffset; filterX++)
                        {
                            var calcOffset = byteOffset +
                                             (filterX * 3) +
                                             (filterY * sourceData.Stride);


                            if (pixelBuffer[calcOffset] < blue)
                                blue = pixelBuffer[calcOffset];

                            if (pixelBuffer[calcOffset + 1] < green)
                                green = pixelBuffer[calcOffset + 1];

                            if (pixelBuffer[calcOffset + 2] < red)
                                red = pixelBuffer[calcOffset + 2];
                        }
                    }

                    resultBuffer[byteOffset] = blue;
                    resultBuffer[byteOffset + 1] = green;
                    resultBuffer[byteOffset + 2] = red;
                }
            }


            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width,
                                             sourceBitmap.Height);


            BitmapData resultData =
                       resultBitmap.LockBits(new Rectangle(0, 0,
                       resultBitmap.Width, resultBitmap.Height),
                       ImageLockMode.WriteOnly,
                       PixelFormat.Format32bppArgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0,
                                       resultBuffer.Length);


            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }


    }
}
