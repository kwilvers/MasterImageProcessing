using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace ImageProcessing
{
    /**
	* @overview brève description du type ainsi que sa mutabilité 
	* @specfields nom:type //éléments nommé repris dans un n-uplet
	* @derivedfields nom:type //élément dérivé des @specfields
	* @invariant description des invariants abstrait qui doivent être vérifié à tout moment
	*/
    public class Convolution
    {
        /// <summary>
        /// Espace de couleur à considérer lors de la convolution
        /// Gain de performance si niveau de gris
        /// </summary>
        public enum ConvolutionColorSpace
        {
            //Convolution sur chaqu'une des composantes de couleur
            RGB,
            //Convolution sur une composante (bleu) d'une image en niveau de gris
            GrayScale,
            //Essaie de déterminer la palette sur 10 % de l'image
            Auto
        }


        public static ConvolutionResult Convolve<T>(Bitmap sourceBitmap, T filter, bool computeDirection=false, ConvolutionColorSpace space = ConvolutionColorSpace.Auto)
            where T : ConvolutionFilterBase
        {
            if (space == ConvolutionColorSpace.Auto)
                space = TryToDetermineSpace(sourceBitmap);

            return space == ConvolutionColorSpace.RGB
                ? ConvolveRGB(sourceBitmap, filter, computeDirection)
                : ConvolveGrayScale(sourceBitmap, filter, computeDirection);
        }

        public static ConvolutionColorSpace TryToDetermineSpace(Bitmap source)
        {
            BitmapData data = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            IntPtr ptr = data.Scan0;

            int tenPercent = source.Height / 10;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(data.Stride) * tenPercent;
            byte[] rgb = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            int gray = 0;

            //if 10% of the image has pixel with the same valued component we suppose it's a gray scale image
            for (int i = 0; i < rgb.Length; i += 3)
            {
                if (rgb[i] == rgb[i + 1] && rgb[i + 1] == rgb[i + 2])
                    gray += 3;
            }

            source.UnlockBits(data);

            return rgb.Length == gray 
                    ? ConvolutionColorSpace.GrayScale 
                    : ConvolutionColorSpace.RGB;
        }


        public static ConvolutionResult ConvolveRGB<T>(Bitmap sourceBitmap, T filter, bool computeDirection)
                         where T : ConvolutionFilterBase
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                        sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);


            double[] blue = new double[filter.Kernels.Count()];
            double[] green = new double[filter.Kernels.Count()];
            double[] red = new double[filter.Kernels.Count()];

            var res = new ConvolutionResult();
            if (computeDirection)
                res.Directions = new byte[sourceData.Stride * sourceData.Height];


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

                    //foreach kernel
                    for (int i = 0; i < filter.Kernels.Count(); i++)
                    {
                        var kernel = filter.Kernels.ElementAt(i);
                        blue[i] = red[i] = green[i] = 0;

                        //foreach row in kernel
                        for (int filterRowIndex = -padding; filterRowIndex <= padding; filterRowIndex++)
                        {
                            //foreach line in kernel
                            for (int filterLineIndex = -padding; filterLineIndex <= padding; filterLineIndex++)
                            {

                                var k = kernel.Kernel;
                                calcOffset = byteOffset +
                                             (filterLineIndex * 3) +
                                             (filterRowIndex * sourceData.Stride);


                                blue[i] += (double)(pixelBuffer[calcOffset]) *
                                         k[filterRowIndex + padding, filterLineIndex + padding];


                                green[i] += (double)(pixelBuffer[calcOffset + 1]) *
                                          k[filterRowIndex + padding, filterLineIndex + padding];


                                red[i] += (double)(pixelBuffer[calcOffset + 2]) *
                                        k[filterRowIndex + padding, filterLineIndex + padding];
                            }
                        }


                        blue[i] = filter.ForceAbsoluteValue
                            ? Math.Abs(kernel.Factor * blue[i])
                            : kernel.Factor * blue[i];
                        blue[i] = blue[i] > 255 ? 255 : blue[i] < 0 ? 0 : blue[i];

                        green[i] = filter.ForceAbsoluteValue
                            ? Math.Abs(kernel.Factor * green[i])
                            : kernel.Factor * green[i];
                        green[i] = green[i] > 255 ? 255 : green[i] < 0 ? 0 : green[i];

                        red[i] = filter.ForceAbsoluteValue
                            ? Math.Abs(kernel.Factor * red[i])
                            : kernel.Factor * red[i];
                        red[i] = red[i] > 255 ? 255 : red[i] < 0 ? 0 : red[i];
                    }

                    if (filter.Kernels.Count() == 2)
                    {
                        if (computeDirection)
                        {
                            var x = (red[0] + green[0] + blue[0]) / 3;
                            var y = (red[1] + green[1] + blue[1]) / 3;
                            res.Directions[byteOffset] = ToDirection(x, y);
                        }

                        resultBuffer[byteOffset] = (byte)(Math.Sqrt(Math.Pow(blue[0], 2) + Math.Pow(blue[1], 2)) + filter.Offset);
                        resultBuffer[byteOffset + 1] = (byte)(Math.Sqrt(Math.Pow(green[0], 2) + Math.Pow(green[1], 2)) + filter.Offset);
                        resultBuffer[byteOffset + 2] = (byte)(Math.Sqrt(Math.Pow(red[0], 2) + Math.Pow(red[1], 2)) + filter.Offset);
                    }
                    else
                    {
                        if (computeDirection)
                            res.Directions[byteOffset] = ToDirection(red, green, blue);

                        resultBuffer[byteOffset] = (byte)(blue.Max(v => Math.Abs(v)) + filter.Offset);
                        resultBuffer[byteOffset + 1] = (byte) (green.Max(v => Math.Abs(v)) + filter.Offset);
                        resultBuffer[byteOffset + 2] = (byte) (red.Max(v => Math.Abs(v)) + filter.Offset);
                    }
                }
            }


            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);


            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            res.Output = resultBitmap;
            return res;
        }


        public static ConvolutionResult ConvolveGrayScale<T>(Bitmap sourceBitmap, T filter, bool computeDirection)
                         where T : ConvolutionFilterBase
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                        sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            var res = new ConvolutionResult();
            if (computeDirection)
                res.Directions = new byte[sourceData.Stride * sourceData.Height] ;

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);


            double[] gray = new double[filter.Kernels.Count()];


            int padding = filter.Padding;
            int calcOffset = 0;
            int byteOffset = 0;


            //Foreach rows
            for (int rowIndex = padding; rowIndex < sourceBitmap.Height - padding; rowIndex++)
            {
                //foreach lines
                for (int lineIndex = padding; lineIndex < sourceBitmap.Width - padding; lineIndex++)
                {
                    //Compute the current pixel byte offset
                    byteOffset = rowIndex * sourceData.Stride + lineIndex * 3;

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

                                var k = kernel.Kernel;
                                calcOffset = byteOffset +
                                             (filterLineIndex * 3) +
                                             (filterRowIndex * sourceData.Stride);


                                gray[i] += (double)(pixelBuffer[calcOffset]) *
                                         k[filterRowIndex + padding, filterLineIndex + padding];

                            }
                        }

                        gray[i] = filter.ForceAbsoluteValue
                            ? Math.Abs(kernel.Factor * gray[i])
                            : kernel.Factor * gray[i];
                        gray[i] = gray[i] > 255 ? 255 : gray[i] < 0 ? 0 : gray[i];
                    }

                    if (filter.Kernels.Count() == 2)
                    {
                        if (computeDirection)
                            res.Directions[byteOffset] = ToDirection(gray[0], gray[1]);

                        resultBuffer[byteOffset] = (byte)Math.Sqrt(Math.Pow(gray[0], 2) + Math.Pow(gray[1], 2));
                        resultBuffer[byteOffset + 1] = resultBuffer[byteOffset];
                        resultBuffer[byteOffset + 2] = resultBuffer[byteOffset];
                    }
                    else
                    {
                        //if (computeDirection)
                        //    res.Directions[byteOffset] = ToDirection(gray);
                        if (computeDirection)
                            res.Directions[byteOffset] = ToDirection(filter.Kernels, gray);

                        resultBuffer[byteOffset] = (byte)gray.Max(v => Math.Abs(v));
                        resultBuffer[byteOffset + 1] = resultBuffer[byteOffset];
                        resultBuffer[byteOffset + 2] = resultBuffer[byteOffset];
                    }
                }
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);


            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            res.Output = resultBitmap;
            return res;
        }

        private static byte ToDirection(double x, double y)
        {
            var dir = (byte) Math.Atan(Math.Abs(y) / Math.Abs(x));

            if (Math2.Between(dir, 22.5, 67.5) || Math2.Between(dir, 202.5, 247.5))
                return 45;

            if (Math2.Between(dir, 67.5, 112.5) || Math2.Between(dir, 247.5, 297.5))
                return 90;

            if (Math2.Between(dir, 112.5, 157.5) || Math2.Between(dir, 297.5, 337.5))
                return 135;

            return 0;
        }


        private static byte ToDirection(IEnumerable<KernelItem> kernels, double[] gray)
        {
            byte b=255;
            double maxima=0;
            var kernelItems = kernels as KernelItem[] ?? kernels.ToArray();
            for (int i = 0; i < kernelItems.Count(); i++ )
            {
                if (gray[i] > maxima)
                {
                    maxima = gray[i];
                    b = kernelItems[i].Orientation.ToValue();
                }
            }

            return b;
        }


        private static byte ToDirection(double[] red, double[] green, double[] blue)
        {
            var dir = new double[red.Length];
            for (int i = 0; i < red.Length; i++)
                dir[i] = (red[i] + green[i] + blue[i]) / 3;

            var index = dir.ToList().IndexOf(dir.Max());
            var x = index % 2 == 0 ? index : index - 1;
            var y = x + 1;
            return ToDirection(dir[x], dir[y]);
        }
    }
}