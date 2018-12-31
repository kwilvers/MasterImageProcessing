using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public class ConvolutionResult
    {
        #region Property : output

        public Bitmap Output { get; set; }

        #endregion Property : output

        #region Property : Directions

        public byte[] Directions { get; set; }

        #endregion Property : Directions

        public Bitmap DirectionToBitmap()
        {
            Bitmap output = new Bitmap(this.Output);
            BitmapData data = output.LockBits(new Rectangle(0, 0, output.Width, output.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            IntPtr ptr = data.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(data.Stride) * output.Height;
            byte[] rgb = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            int j = 0;
            for (int i = 0; i < rgb.Length; i += 3)
            {
                rgb[i] = 0;//blue
                rgb[i + 1] = 0; //green
                rgb[i + 2] = 0;//red

                if (this.Directions[j] == 0)
                {
                    rgb[i] = 255;//blue
                    rgb[i + 1] = 255; //green
                    rgb[i + 2] = 255;//red
                }
                if ( this.Directions[j] == 45)
                    rgb[i + 2] = 255;//red

                if(this.Directions[j] == 90)
                    rgb[i + 1] = 255; //green

                if(this.Directions[j] == 135)
                    rgb[i] = 255;//blue

                j+=3;
            }

            //Copy changed RGB values back to bitmap
            Marshal.Copy(rgb, 0, ptr, bytes);

            output.UnlockBits(data);
            return output;
        }
    }
}
