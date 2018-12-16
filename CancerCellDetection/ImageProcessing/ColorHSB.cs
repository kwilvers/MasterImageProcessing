using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public class ColorHSB
    {
        public int H { get; set; }
        public int S { get; set; }
        public int B { get; set; }

        public static ColorHSB FromRGB(double R, double G, double B)
        {
            ColorHSB c = new ColorHSB();

            double max = R > G ? R : G;
            max = max > B ? max : B;

            double min = R < G ? R : G;
            min = min < B ? min : B;

            double delta = max - min;

            //Hue calculation
            if (delta == 0.0)
                c.H = 0;
            else
            {
                double us = (double)1 / 6;
                double ut = (double)1 / 3;
                double dt = (double)2 / 3;

                if (max == R)
                    c.H = (int) Math.Round(us * ((G - B) / delta) * 360.0);
                else if (max == G)
                    c.H = (int) Math.Round((us * ((B - R) / delta) + ut) * 360.0);
                else
                    c.H = (int) Math.Round((us * ((R - G) / delta) + dt) * 360.0);
            }

            //Saturation calculation
            c.S = (int) Math.Round(max == 0.0 ? 0 : (double) (delta / max)*100.0);

            //Brigthness / Value Calculation
            c.B = (int) Math.Round(max/255.0*100.0);

            return c;
        }
    }
}
