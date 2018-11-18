namespace ImageProcessing.Smoothing
{
    /**
	* @overview Filtre gaussien et de taille 5x5
	* @specfields name:String //"Gaussian Filter - 98 - Size 5x5"
	*/
    public class GaussianFilter98S5 : ConvolutionFilterBase
    {

        public override string Name => "Mean Filter - 98 - Size 5x5";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k = new double[,]{
                { 1, 2,  3, 2, 1 },
                { 2, 6,  8, 6, 2 },
                { 3, 8, 10, 8, 3 },
                { 2, 6,  8, 6, 2 },
                { 1, 2,  3, 2, 1 }
            };

            this.AddKernel(k, (double)1/98, KernelOrientation.None);
        }
    }
}