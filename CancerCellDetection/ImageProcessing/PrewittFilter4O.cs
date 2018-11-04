using System;
using System.Text;

namespace ImageProcessing
{
    /**
	* @overview Filtre de détection de prewit
	* @specfields name:String //"Prewitt Filter"
	*/
    public class PrewittFilter4O : ConvolutionFilterBase
    {
        public override string Name => "Prewitt4O Filter";

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

            var k2 = new double[,]{
                { 1, 1, 0 },
                { 1, 0, -1 },
                { 0, -1, -1 }
            };
            this.AddKernel(k2, 1, KernelOrientation.EasternNorth);
            
            var k3 = new double[,]{
                { 1, 1, 1 },
                { 0, 0, 0 },
                { -1, -1, -1 }
            };
            this.AddKernel(k3, 1, KernelOrientation.North);

            var k4 = new double[,]{
                { 0, 1, 1 },
                { -1, 0, 1 },
                { -1, -1, 0 }
            };
            this.AddKernel(k4, 1, KernelOrientation.WesternNorth);
        }
    }
}