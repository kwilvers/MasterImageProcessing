using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageProcessing.Correction
{
    /**
	* @overview brève description du type ainsi que sa mutabilité 
	* @specfields nom:type //éléments nommé repris dans un n-uplet
	* @derivedfields nom:type //élément dérivé des @specfields
	* @invariant description des invariants abstrait qui doivent être vérifié à tout moment
	*/
    public static class NonMaximumSuppression
    {
        public static Bitmap Apply(Bitmap sourceBitmap, byte[] directions, int size=3)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                        sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);


            int padding = size/2+1;


            //Foreach rows
            for (int rowIndex = padding; rowIndex < sourceBitmap.Height - padding; rowIndex++)
            {
                //foreach lines
                for (int cellIndex = padding; cellIndex < sourceBitmap.Width - padding; cellIndex++)
                {
                    var byteOffset = rowIndex * sourceData.Stride + cellIndex * 3;
                    var direction = directions[byteOffset];
                    int previous = 0, next = 0;

                    switch (direction)
                    {
                        case 0:
                            previous = rowIndex * sourceData.Stride + (cellIndex - 1) * 3;
                            next = rowIndex * sourceData.Stride + (cellIndex + 1) * 3;
                            break;
                        case 45:
                            previous = (rowIndex + 1) * sourceData.Stride + (cellIndex - 1) * 3;
                            next = (rowIndex - 1) * sourceData.Stride + (cellIndex + 1) * 3;
                            break;
                        case 90:
                            previous = (rowIndex-1) * sourceData.Stride + cellIndex * 3;
                            next = (rowIndex+1) * sourceData.Stride + cellIndex * 3;
                            break;
                        case 135:
                            previous = (rowIndex - 1) * sourceData.Stride + (cellIndex - 1) * 3;
                            next = (rowIndex + 1) * sourceData.Stride + (cellIndex + 1) * 3;
                            break;
                        default:
                            throw new ArgumentException("Not a normalized angle (0, 45, 90, 135)");
                    }

                    if (resultBuffer[byteOffset] < resultBuffer[previous] ||
                        resultBuffer[byteOffset] < resultBuffer[next])
                    {
                        resultBuffer[byteOffset] = 0;
                        resultBuffer[byteOffset + 1] = 0;
                        resultBuffer[byteOffset + 2] = 0;
                    }
                    else
                    {
                        resultBuffer[byteOffset] = pixelBuffer[byteOffset];
                        resultBuffer[byteOffset + 1] = pixelBuffer[byteOffset + 1];
                        resultBuffer[byteOffset + 2] = pixelBuffer[byteOffset + 2];
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