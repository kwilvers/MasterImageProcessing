using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.Smoothing
{
    public class GaussianFilter : ConvolutionFilterBase
    {
        private readonly int length;
        private readonly double weight;
        public override string Name { get; }
        
        public GaussianFilter()
        {
            this.length = 3;
            this.weight = 1.5;
            this.InitKernels();
        }

        public GaussianFilter(int length, double weight)
        {
            this.length = length;
            this.weight = weight;
            this.ClearKernel();
            this.InitKernels();
        }

        protected override void InitKernels()
        {
            var k = Calculate(length > 0 ? length : 3, Math.Abs(weight) > double.Epsilon ? weight : 1.5);
            this.AddKernel(k, 1, KernelOrientation.None);
        }

        public static double[,] Calculate(int length, double weight)
        {
            double[,] Kernel = new double[length, length];
            double sumTotal = 0;


            int kernelRadius = length / 2;
            double distance = 0;


            double calculatedEuler = 1.0 /
                                     (2.0 * Math.PI * Math.Pow(weight, 2));


            for (int filterY = -kernelRadius;
                filterY <= kernelRadius; filterY++)
            {
                for (int filterX = -kernelRadius;
                    filterX <= kernelRadius; filterX++)
                {
                    distance = ((filterX * filterX) +
                                (filterY * filterY)) /
                               (2 * (weight * weight));


                    Kernel[filterY + kernelRadius,
                            filterX + kernelRadius] =
                        calculatedEuler * Math.Exp(-distance);


                    sumTotal += Kernel[filterY + kernelRadius,
                        filterX + kernelRadius];
                }
            }


            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    Kernel[y, x] = Kernel[y, x] *
                                   (1.0 / sumTotal);
                }
            }


            return Kernel;
        }

    }
}
