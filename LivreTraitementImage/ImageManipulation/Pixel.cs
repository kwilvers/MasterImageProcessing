using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageManipulation
{
    //MUTABLE OU IMMUTABLE???
    public class Pixel
    {
        [StructLayout(LayoutKind.Explicit)]
        private struct ColorUnion
        {
            [FieldOffset(0)] public byte b;
            [FieldOffset(1)] public byte g;
            [FieldOffset(2)] public byte r;
            [FieldOffset(3)] public byte a;
            [FieldOffset(0)] public int intColor;
        }

        private ColorUnion color;
        public byte A { get => color.a; set => color.a = value; }
        public byte R { get => color.r; set => color.r = value; }
        public byte G { get => color.g; set => color.g = value; }
        public byte B { get => color.b; set => color.b = value; }

        public Pixel(Int32 intColor)
        {
            color = new ColorUnion
            {
                intColor = intColor
            };
        }

        public Pixel(byte[] argb)
        {
            if (argb.Length != 4) throw new ArgumentOutOfRangeException("Parameter ARGB must have 4 bytes");

            color = new ColorUnion
            {
                a = argb[0],
                r = argb[1],
                g = argb[2],
                b = argb[3]
            };
        }

        public Pixel(byte a, byte r, byte g, byte b)
        {
            color = new ColorUnion
            {
                a = a,
                r = r,
                g = g,
                b = b
            };
        }

        public Pixel(Color c)
        {
            color = new ColorUnion
            {
                a = c.A,
                r = c.R,
                g = c.G,
                b = c.B
            };
        }

        public Pixel(Pixel p)
        {
            color = new ColorUnion
            {
                a = p.A,
                r = p.R,
                g = p.G,
                b = p.B
            };
        }
        
        public Int32 ToInt()
        {
            return color.intColor;
        }

        public byte[] ToArgbBytes()
        {
            byte[] argb = new byte[]{color.a, color.r, color.g, color.b};
            return argb;
        }

        public Color ToColor()
        {
            Color c= Color.FromArgb(color.a, color.r, color.g, color.b);
            return c;
        }
    }
}
