using System;
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
    public static class Convolution
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

        public static Bitmap Convolve<T>(Bitmap sourceBitmap, T filter, ConvolutionColorSpace space=ConvolutionColorSpace.Auto)
                                 where T : ConvolutionFilterBase
        {
            if (space == ConvolutionColorSpace.Auto)
                space = TryToDetermineSpace(sourceBitmap);

            return space == ConvolutionColorSpace.RGB 
                        ? ConvolveRGB(sourceBitmap, filter) 
                        : ConvolveGrayScale(sourceBitmap, filter);
        }

        private static ConvolutionColorSpace TryToDetermineSpace(Bitmap source)
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


        public static Bitmap ConvolveRGB<T>(Bitmap sourceBitmap, T filter)
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


                        blue[i] = kernel.Factor * blue[i];
                        blue[i] = blue[i] > 255 ? 255 : blue[i] < 0 ? 0 : blue[i];

                        green[i] = kernel.Factor * green[i];
                        green[i] = green[i] > 255 ? 255 : green[i] < 0 ? 0 : green[i];

                        red[i] = kernel.Factor * red[i];
                        red[i] = red[i] > 255 ? 255 : red[i] < 0 ? 0 : red[i];
                    }

                    if (filter.Kernels.Count() == 2)
                    {
                        resultBuffer[byteOffset] = (byte)Math.Sqrt(Math.Pow(blue[0], 2) + Math.Pow(blue[0], 2));
                        resultBuffer[byteOffset + 1] = (byte)Math.Sqrt(Math.Pow(green[0], 2) + Math.Pow(green[0], 2));
                        resultBuffer[byteOffset + 2] = (byte)Math.Sqrt(Math.Pow(red[0], 2) + Math.Pow(red[0], 2));
                        //resultBuffer[byteOffset + 3] = 255;
                    }
                    else
                    {
                        resultBuffer[byteOffset] = (byte)blue.Max(v => Math.Abs(v));
                        resultBuffer[byteOffset + 1] = (byte)green.Max(v => Math.Abs(v));
                        resultBuffer[byteOffset + 2] = (byte)red.Max(v => Math.Abs(v));
                        //resultBuffer[byteOffset + 3] = 255;
                    }
                    /*
                    //en fonction du résultat
                    resultBuffer[byteOffset] = (byte)(blue);
                    resultBuffer[byteOffset + 1] = (byte)(green);
                    resultBuffer[byteOffset + 2] = (byte)(red);
                    resultBuffer[byteOffset + 3] = 255;*/
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


                        gray[i] = kernel.Factor * gray[i];
                        gray[i] = gray[i] > 255 ? 255 : gray[i] < 0 ? 0 : gray[i];
                    }

                    if (filter.Kernels.Count() == 2)
                    {
                        resultBuffer[byteOffset] = (byte)Math.Sqrt(Math.Pow(gray[0], 2) + Math.Pow(gray[0], 2));
                        resultBuffer[byteOffset + 1] = resultBuffer[byteOffset];
                        resultBuffer[byteOffset + 2] = resultBuffer[byteOffset];
                    }
                    else
                    {
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


            return resultBitmap;
        }

    }
}