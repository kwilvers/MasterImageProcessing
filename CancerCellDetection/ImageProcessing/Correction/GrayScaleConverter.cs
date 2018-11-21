using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageProcessing.Correction
{
    /**
	* @overview Méthode d'abstraction de conversion d'une image en niveau de gris
    * selon différentes méthodes
	*/
    public static class GrayScaleConverter
    {
        /// <summary>
        /// Enumération des méthodes de conversion d'une image RGB en niveau de gris
        /// </summary>
        public enum GrayConvertionMethod
        {
            Average,
            Rec601,
            Bt709,
            FromBrightness,
            FromUChrominance,
            FromVChrominance,
            FromRed,
            FromGreen,
            FromBlue,
            FromRedAndBlue,
            FromRedAndGreen,
            FromBlueAndGreen
        }

        /// <requires>source != null</requires>
        /// <effects>Conversion de l'image source en niveau de gris selon la méthode</effects>
        /// <returns>Une bitmap en niveau de gris</returns>
        public static Bitmap ToGray(Bitmap source, GrayConvertionMethod method)
        {
            Bitmap output = new Bitmap(source);
            BitmapData data = output.LockBits(new Rectangle(0, 0, output.Width, output.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            IntPtr ptr = data.Scan0;
            
            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(data.Stride) * output.Height;
            byte[] rgb = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgb, 0, bytes);

            for (int i = 0; i < rgb.Length; i += 3)
            {
                var g = ToGray(rgb[i+2], rgb[i+1], rgb[i], method);
                rgb[i + 2] = rgb[i + 1] = rgb[i] = g;
            }
            //Copy changed RGB values back to bitmap
            Marshal.Copy(rgb, 0, ptr, bytes);

            output.UnlockBits(data);
            return output;
        }

        /// <requires>R != null, G != null && B != null</requires>
        /// <returns>la valeur moyenne des composantes RGB</returns>
        public static byte ColorAverage(byte R, byte G, byte B)
        {
            int avg = (R + G + B) / 3;
            return (byte)avg;
        }

        /// <requires>c1 != null, c2 != null </requires>
        /// <returns>la valeur moyenne des composantes c1 et c2</returns>
        public static byte TwoColorAverage(byte c1, byte c2)
        {
            int avg = (c1 + c2) / 2;
            return (byte)avg;
        }

        ///The ITU-R BT.709 standard used for HDTV developed by the ATSC uses different color coefficients, computing the luma component as
        /// <requires>R != null, G != null && B != null</requires>
        /// <returns>Le résultat de 0.2126 * R + 0.7152 * G + 0.0722 * B</returns>
        public static byte Bt709(byte R, byte G, byte B)
        {
            double avg = (0.2126 * R + 0.7152 * G + 0.0722 * B);
            return (byte)avg;
        }

        /// Isolation de la luminance, le codage rec601
        /// n the Y'UV and Y'IQ models used by PAL and NTSC, the rec601 luma (Y') component is computed as
        /// <requires>R != null, G != null && B != null</requires>
        /// <returns>le résultat de 0.2126 * R + 0.7152 * G + 0.0722 * B</returns>
        public static byte FromBrightness(byte R, byte G, byte B)
        {
            double avg = (0.2126 * R + 0.7152 * G + 0.0722 * B);
            return (byte)avg;
        }

        /// Isolation de la chrominance de la composante U
        /// <requires>R != null, G != null && B != null</requires>
        /// <returns>Le résultat de 0.493 * (B - luminance)</returns>
        public static byte FromUChrominance(byte R, byte G, byte B)
        {
            var y = FromBrightness(R,G,B);
            double chrominance = 0.493 * (B - y);
            return (byte)chrominance;
        }

        /// Isolation de la chrominance de la composante V
        /// <requires>R != null, G != null && B != null</requires>
        /// <returns>Le résultat de 0.493 * (R - luminance)</returns>
        public static byte FromVChrominance(byte R, byte G, byte B)
        {
            var y = FromBrightness(R,G,B);
            double chrominance = 0.877 * (R - y);
            return (byte)chrominance;
        }

        /// <requires>R != null, G != null && B != null</requires>
        /// <returns>la composante R</returns>
        public static byte FromRed(byte R, byte G, byte B)
        {
            return R;
        }

        /// <requires>R != null, G != null && B != null</requires>
        /// <returns>la composantes G</returns>
        public static byte FromGreen(byte R, byte G, byte B)
        {
            return G;
        }

        /// <requires>R != null, G != null && B != null</requires>
        /// <returns>la composante B</returns>
        public static byte FromBlue(byte R, byte G, byte B)
        {
            return B;
        }

        /// <requires>R != null, G != null && B != null</requires>
        /// <returns>la valeur en niveau de gris de la valeur RGB selon la méthode t</returns>
        public static byte ToGray(byte R, byte G, byte B, GrayConvertionMethod t)
        {
            switch (t)
            {
                case GrayConvertionMethod.Average:
                    return ColorAverage(R,G,B);
                case GrayConvertionMethod.Bt709:
                    return Bt709(R, G, B);
                case GrayConvertionMethod.Rec601:
                case GrayConvertionMethod.FromBrightness:
                    return FromBrightness(R, G, B);
                case GrayConvertionMethod.FromUChrominance:
                    return FromUChrominance(R, G, B);
                case GrayConvertionMethod.FromVChrominance:
                    return FromVChrominance(R, G, B);
                case GrayConvertionMethod.FromRed:
                    return FromRed(R, G, B);
                case GrayConvertionMethod.FromGreen:
                    return FromGreen(R, G, B);
                case GrayConvertionMethod.FromBlue:
                    return FromBlue(R, G, B);
                case GrayConvertionMethod.FromRedAndBlue:
                    return TwoColorAverage(R, B);
                case GrayConvertionMethod.FromRedAndGreen:
                    return TwoColorAverage(R, G);
                case GrayConvertionMethod.FromBlueAndGreen:
                    return TwoColorAverage(G, B);
                default:
                    throw new ArgumentOutOfRangeException(nameof(t), t, null);
            }
        }
    }
}