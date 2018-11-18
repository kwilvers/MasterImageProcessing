using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ImageProcessing
{
    /**
	* @overview brève description du type ainsi que sa mutabilité 
	* @specfields nom:type //éléments nommé repris dans un n-uplet
	* @derivedfields nom:type //élément dérivé des @specfields
	* @invariant description des invariants abstrait qui doivent être vérifié à tout moment
	*/
    public class ConvolutionMedian 
    {
        public static Bitmap Convolve<T>(Bitmap sourceBitmap, T filter, Convolution.ConvolutionColorSpace space=Convolution.ConvolutionColorSpace.Auto)
                                 where T : ConvolutionFilterBase
        {
            if (space == Convolution.ConvolutionColorSpace.Auto)
                space = Convolution.TryToDetermineSpace(sourceBitmap);

            return space == Convolution.ConvolutionColorSpace.RGB 
                        ? ConvolveRGB(sourceBitmap, filter) 
                        : ConvolveGrayScale(sourceBitmap, filter);
        }


        public static Bitmap ConvolveRGB<T>(Bitmap sourceBitmap, T filter)
                         where T : ConvolutionFilterBase
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                        sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);


            double[] blue = new double[filter.Kernels.Count()];
            double[] green = new double[filter.Kernels.Count()];
            double[] red = new double[filter.Kernels.Count()];


            List<int> neighbourPixels = new List<int>();
            int padding = filter.Padding;
            int calcOffset = 0;
            int byteOffset = 0;
            byte[] middlePixel;

            //Foreach rows
            for (int rowIndex = padding; rowIndex < sourceBitmap.Height - padding; rowIndex++)
            {
                //foreach lines
                for (int lineIndex = padding; lineIndex < sourceBitmap.Width - padding; lineIndex++)
                {
                    byteOffset = rowIndex * sourceData.Stride + lineIndex * 4;

                    //foreach kernel
                    for (int i = 0; i < filter.Kernels.Count(); i++)
                    {
                        var kernel = filter.Kernels.ElementAt(i);
                        blue[i] = red[i] = green[i] = 0;
                        neighbourPixels.Clear();

                        //foreach row in kernel
                        for (int filterRowIndex = -padding; filterRowIndex <= padding; filterRowIndex++)
                        {
                            //foreach line in kernel
                            for (int filterLineIndex = -padding; filterLineIndex <= padding; filterLineIndex++)
                            {

                                var k = kernel.Kernel;
                                calcOffset = byteOffset +
                                             (filterLineIndex * 4) +
                                             (filterRowIndex * sourceData.Stride);
                                
                                neighbourPixels.Add(BitConverter.ToInt32(pixelBuffer, calcOffset));
                            }
                        }
                        neighbourPixels.Sort();

                        middlePixel = BitConverter.GetBytes(neighbourPixels[padding+1]);

                        resultBuffer[byteOffset] = middlePixel[0];
                        resultBuffer[byteOffset + 1] = middlePixel[1];
                        resultBuffer[byteOffset + 2] = middlePixel[2];
                        resultBuffer[byteOffset + 3] = middlePixel[3];
                    }
                }
            }


            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);


            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }

        public static Bitmap ConvolveGrayScale<T>(Bitmap sourceBitmap, T filter)
                         where T : ConvolutionFilterBase
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                        sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);


            double[] gray = new double[filter.Kernels.Count()];


            List<byte> neighbourPixels = new List<byte>();
            int padding = filter.Padding;
            int calcOffset = 0;
            int byteOffset = 0;


            //Foreach rows
            for (int rowIndex = padding; rowIndex < sourceBitmap.Height - padding; rowIndex++)
            {
                //foreach lines
                for (int lineIndex = padding; lineIndex < sourceBitmap.Width - padding; lineIndex++)
                {
                    byteOffset = rowIndex * sourceData.Stride + lineIndex * 3;
                    neighbourPixels.Clear();

                    //foreach kernel
                    for (int i = 0; i < filter.Kernels.Count(); i++)
                    {
                        var kernel = filter.Kernels.ElementAt(i);
                        gray[i] = 0;

                        //foreach row in kernel
                        for (int filterRowIndex = -padding; filterRowIndex <= padding; filterRowIndex++)
                        {
                            //foreach line in kernel
                            for (int filterLineIndex = -padding; filterLineIndex <= padding; filterLineIndex++)
                            {
                                calcOffset = byteOffset +
                                             (filterLineIndex * 3) +
                                             (filterRowIndex * sourceData.Stride);
                                
                                neighbourPixels.Add(pixelBuffer[calcOffset]);
                            }
                        }

                        neighbourPixels.Sort();

                        byte middlePixel = neighbourPixels[padding+1];
                        resultBuffer[byteOffset] = middlePixel;
                        resultBuffer[byteOffset + 1] = middlePixel;
                        resultBuffer[byteOffset + 2] = middlePixel;
                    }
                }
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);


            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }

    }
}