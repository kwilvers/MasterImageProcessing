using System;
using System.Text;

namespace ImageProcessing
{
    /**
	* @overview Filtre de détection vertical
	* @specfields name:String //"Horizontal Filter"
	*/
    public class HorizontalFilter : ConvolutionFilterBase
    {

        public override string Name => "Horizontal Filter";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k = new double[,]{
                { 1, 1, 1 },
                { 0, 0, 0 },
                { -1, -1, -1 }
            };

            this.AddKernel(k, 1, KernelOrientation.North);
        }
    }
}