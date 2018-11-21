using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessing;
using ImageProcessing.Correction;

namespace ImageProcessingFlow
{
    public class GammaCorrectionActivity : BitmapProcessingBase
    {

        /// <requires>input != null</requires>
        /// <effects>Applique une correction gamma par défaut à l'image d'entrée</effects>
        /// <returns>Une bitmap représentant la bitmap d'entrée ayant subit une correction gamma de 1.7</returns>
        public override Bitmap Apply(Bitmap input)
        {
            return Apply<double, object, object>(input, 1.7, null, null);
        }

        /// <requires>input != null</requires>
        /// <effects>Applique une correction gamma à l'image d'entrée</effects>
        /// <returns>Une bitmap représentant la bitmap d'entrée ayant subit une correction gamma la valeur de param1</returns>
        public override Bitmap Apply<T1, T2, T3>(Bitmap input, double param1, T2 param2, T3 param3)
        {
            return GammaCorrection.Correct(input, param1);
        }
    }
}
