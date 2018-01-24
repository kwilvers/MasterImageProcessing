using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManipulation
{
    public static class PixelTransform
    {
        public enum GrayConvertionType
        {
            Average,
            Rec601,
            Bt709,
            FromRed,
            FromGreen,
            FromBlue,
            FromRedAndBlue,
            FromRedAndGreen,
            FromBlueAndGreen
        }

        public static Pixel IsolateRed(Pixel p)
        {
            return new Pixel(p.A, p.R,0,0);
        }
        public static Pixel IsolateGreen(Pixel p)
        {
            return new Pixel(p.A, 0, p.G, 0);
        }
        public static Pixel IsolateBlue(Pixel p)
        {
            return new Pixel(p.A, 0, 0, p.B);
        }

        public static Pixel ToGrayAverage(Pixel p)
        {
            int avg = (p.R + p.G + p.B) / 3;
            return new Pixel(p.A, (byte) avg, (byte) avg, (byte) avg);
        }
        public static Pixel ToGrayAverage(byte a, byte c1, byte c2)
        {
            int avg = (c1 + c2) / 2;
            return new Pixel(a, (byte)avg, (byte)avg, (byte)avg);
        }

        //n the Y'UV and Y'IQ models used by PAL and NTSC, the rec601 luma (Y') component is computed as
        public static Pixel ToGrayRec601(Pixel p)
        {
            double avg = (0.299 * p.R + 0.587 * p.G + 0.114 * p.B);
            return new Pixel(p.A, (byte)avg, (byte)avg, (byte)avg);
        }

        //The ITU-R BT.709 standard used for HDTV developed by the ATSC uses different color coefficients, computing the luma component as
        public static Pixel ToGrayBt709(Pixel p)
        {
            double avg = (0.2126 * p.R + 0.7152 * p.G + 0.0722 * p.B);
            return new Pixel(p.A, (byte)avg, (byte)avg, (byte)avg);
        }
        public static Pixel ToGrayFromRed(Pixel p)
        {
            return new Pixel(p.A, p.R, p.R, p.R);
        }
        public static Pixel ToGrayFromGreen(Pixel p)
        {
            return new Pixel(p.A, p.G, p.G, p.G);
        }
        public static Pixel ToGrayFromBlue(Pixel p)
        {
            return new Pixel(p.A, p.B, p.B, p.B);
        }

        public static Pixel ToGray(Pixel p, GrayConvertionType t)
        {
            switch (t)
            {
                case GrayConvertionType.Average:
                    return ToGrayAverage(p);
                case GrayConvertionType.Rec601:
                    return ToGrayRec601(p);
                case GrayConvertionType.Bt709:
                    return ToGrayBt709(p);
                case GrayConvertionType.FromRed:
                    return ToGrayFromRed(p);
                case GrayConvertionType.FromGreen:
                    return ToGrayFromGreen(p);
                case GrayConvertionType.FromBlue:
                    return ToGrayFromBlue(p);
                case GrayConvertionType.FromRedAndBlue:
                    return ToGrayAverage(p.A, p.R, p.B);
                case GrayConvertionType.FromRedAndGreen:
                    return ToGrayAverage(p.A, p.R, p.G);
                case GrayConvertionType.FromBlueAndGreen:
                    return ToGrayAverage(p.A, p.G, p.B);
                default:
                    throw new ArgumentOutOfRangeException(nameof(t), t, null);
            }
        }
    }
}
