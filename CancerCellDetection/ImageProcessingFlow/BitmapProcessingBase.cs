using System.Drawing;

namespace ImageProcessingFlow
{
    /**
	* @overview Offre une interface commune permettant d'automatiser un workflow
	*/
    public abstract class BitmapProcessingBase
    {
        public abstract Bitmap Apply(Bitmap input);
        public abstract Bitmap Apply<T1, T2, T3>(Bitmap input, double param1, T2 param2, T3 param3);
    }
}
