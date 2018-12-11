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
    public static class Erode
    {
        public static Bitmap Apply<T>(Bitmap sourceBitmap, T filter, byte grayThreshold)
                 where T : ConvolutionFilterBase
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                        sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            Marshal.Copy(sourceData.Scan0, resultBuffer, 0, pixelBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);


            double[] gray = new double[filter.Kernels.Count()];

            int padding = filter.Padding;


            //Foreach rows
            for (int rowIndex = padding; rowIndex < sourceBitmap.Height - padding; rowIndex++)
            {
                //foreach lines
                for (int lineIndex = padding; lineIndex < sourceBitmap.Width - padding; lineIndex++)
                {
                    //Compute the current pixel byte offset
                    var byteOffset = rowIndex * sourceData.Stride + lineIndex * 3;

                    //Si le pixel est un fond ne rien faire
                    if (pixelBuffer[byteOffset] == 0)
                        continue;

                    //sinon c'est un contour
                    //foreach kernel
                    for (int i = 0; i < filter.Kernels.Count(); i++)
                    {
                        var kernel = filter.Kernels.ElementAt(i);
                        gray[i] = 0;
                        var hasNeighbour = false;


                        //foreach row in kernel
                        for (int filterRowIndex = -padding; filterRowIndex <= padding; filterRowIndex++)
                        {
                            //foreach line in kernel
                            for (int filterLineIndex = -padding; filterLineIndex <= padding; filterLineIndex++)
                            {
                                var k = kernel.Kernel;
                                if (k[filterRowIndex + padding, filterLineIndex + padding] == 0)
                                    continue;

                                //Compute the neighbour byte offset
                                var calcOffset = byteOffset +
                                                 (filterLineIndex * 3) +
                                                 (filterRowIndex * sourceData.Stride);
                                
                                //Si le pixel voisin est un contour
                                if (pixelBuffer[calcOffset] == 0)
                                {
                                    hasNeighbour = true;
                                    //break; //optimization
                                }
                            }
                        }

                        //Si au moins un des voisins est un contour le pixel de fond est dilaté
                        if (hasNeighbour)
                        {
                            resultBuffer[byteOffset] = 0;
                            resultBuffer[byteOffset + 1] = 0;
                            resultBuffer[byteOffset + 2] = 0;
                        }
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
