/*
 * The Following Code was developed by Dewald Esterhuizen
 * View Documentation at: http://softwarebydefault.com
 * Licensed under Ms-PL 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace ImageConvolutionFilters
{
    public static class ExtBitmap
    {
        public static Bitmap CopyToSquareCanvas(this Bitmap sourceBitmap, int canvasWidthLenght)
        {
            float ratio = 1.0f;
            int maxSide = sourceBitmap.Width > sourceBitmap.Height ? sourceBitmap.Width : sourceBitmap.Height;

            ratio = (float) maxSide / (float) canvasWidthLenght;

            Bitmap bitmapResult = (sourceBitmap.Width > sourceBitmap.Height
                ? new Bitmap(canvasWidthLenght, (int) (sourceBitmap.Height / ratio))
                : new Bitmap((int) (sourceBitmap.Width / ratio), canvasWidthLenght));

            using (Graphics graphicsResult = Graphics.FromImage(bitmapResult))
            {
                graphicsResult.CompositingQuality = CompositingQuality.HighQuality;
                graphicsResult.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsResult.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphicsResult.DrawImage(sourceBitmap,
                                        new Rectangle(0, 0, bitmapResult.Width, bitmapResult.Height),
                                        new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                                        GraphicsUnit.Pixel);
                graphicsResult.Flush();
            }

            return bitmapResult;
        }

        public static Bitmap ConvolutionFilter<T>(this Bitmap sourceBitmap, T filter)
            where T : ConvolutionFilterBase
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                                                          ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);

            double blue = 0.0;
            double green = 0.0;
            double red = 0.0;

            int filterSize = filter.FilterMatrix.GetLength(1);

            int filterOffset = (filterSize - 1) / 2;

            for (int offsetY = filterOffset; offsetY < sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX < sourceBitmap.Width - filterOffset; offsetX++)
                {
                    blue = 0;
                    green = 0;
                    red = 0;

                    var byteOffset = offsetY * sourceData.Stride + offsetX * 4;

                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            var calcOffset = byteOffset +
                                             (filterX * 4) +
                                             (filterY * sourceData.Stride);

                            blue += (double) (pixelBuffer[calcOffset]) *
                                    filter.FilterMatrix[filterY + filterOffset,
                                    filterX + filterOffset];

                            green += (double) (pixelBuffer[calcOffset + 1]) *
                                     filter.FilterMatrix[filterY + filterOffset,
                                     filterX + filterOffset];

                            red += (double) (pixelBuffer[calcOffset + 2]) *
                                   filter.FilterMatrix[filterY + filterOffset,
                                   filterX + filterOffset];
                        }
                    }

                    blue = filter.Factor * blue + filter.Bias;
                    green = filter.Factor * green + filter.Bias;
                    red = filter.Factor * red + filter.Bias;

                    blue = blue > 255 ? 255 : (blue < 0 ? 0 : blue);
                    green = green > 255 ? 255 : (green < 0 ? 0 : green);
                    red = red > 255 ? 255 : (red < 0 ? 0 : red);

                    resultBuffer[byteOffset] = (byte) (blue);
                    resultBuffer[byteOffset + 1] = (byte) (green);
                    resultBuffer[byteOffset + 2] = (byte) (red);
                    resultBuffer[byteOffset + 3] = 255;
                }
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height),
                                                          ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }
    }
}