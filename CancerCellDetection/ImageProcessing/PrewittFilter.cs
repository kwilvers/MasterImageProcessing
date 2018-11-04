using System;
using System.Text;

namespace ImageProcessing
{
    /**
	* @overview Filtre de détection de prewit
	* @specfields name:String //"Prewitt Filter"
	*/
    public class PrewittFilter : ConvolutionFilterBase
    {
        public override string Name => "Prewitt Filter";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k1 = new double[,]{
                { 1, 0, -1 },
                { 1, 0, -1 },
                { 1, 0, -1 }
            };
            this.AddKernel(k1, 1, KernelOrientation.East);

            var k3 = new double[,]{
                { 1, 1, 1 },
                { 0, 0, 0 },
                { -1, -1, -1 }
            };
            this.AddKernel(k3, 1, KernelOrientation.North);
        }
    }
}