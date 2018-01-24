using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManipulation
{
    public class PixelBof
    {
        private byte a;
        private byte r;
        private byte g;
        private byte b;
        private int intColor;

        public byte A
        {
            get { return a; }
            set
            {
                a = value;
                CreateInt();
            }
        }

        public byte R
        {
            get { return r; }
            set
            {
                r = value;
                CreateInt();
            }
        }

        public byte G
        {
            get { return g; }
            set
            {
                g = value;
                CreateInt();
            }
        }

        public byte B
        {
            get { return b; }
            set
            {
                b = value;
                CreateInt();
            }
        }

        public Int32 IntColor
        {
            get { return intColor; }
            set
            {
                intColor = value; 
                ExtractArgb();
            }
        }

        public PixelBof(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
            IntColor = A << 24 | R << 16 | G << 8 | B;
        }

        public Color ToColor()
        {
            return Color.FromArgb(A, R, G, B);
        }

        private void CreateInt()
        {
            intColor = A << 24 | R << 16 | G << 8 | B;
        }

        private void ExtractArgb()
        {
            a = (byte) (IntColor >> 24);
            r = (byte) (IntColor >> 16);
            g = (byte) (IntColor >> 8);
            b = (byte) (IntColor);
        }

    }
}
