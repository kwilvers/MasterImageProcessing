﻿namespace ImageProcessing.Smoothing
{
    /**
	* @overview Filtre moyenneur de connexité 4 et de taille 3x3
	* @specfields name:String //"Mean Filter - Conexity 4 - Size 3x3"
	*/
    public class MeanFilterC4S3 : ConvolutionFilterBase
    {

        public override string Name => "Mean Filter - Conexity 4 - Size 3x3";
        
        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k = new double[,]{
                { 0, 1, 0 },
                { 1, 1, 1 },
                { 0, 1, 0 }
            };

            this.AddKernel(k, Math2.Div(1, 5), KernelOrientation.None);
        }
    }
}