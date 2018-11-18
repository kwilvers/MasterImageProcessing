namespace ImageProcessing.Smoothing
{
    /**
	* @overview Filtre gaussien de taille 5x5
	* @specfields name:String //"Gaussian Filter - 273 - Size 5x5"
	*/
    public class GaussianFilter273S5 : ConvolutionFilterBase
    {

        public override string Name => "Mean Filter - 273 - Size 5x5";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k = new double[,]{
                { 1,  4,  7,  4, 1 },
                { 4, 16, 26, 16, 4 },
                { 7, 26, 41, 26, 7 },
                { 4, 16, 26, 16, 4 },
                { 1,  4,  7,  4, 1 }
            };

            this.AddKernel(k, (double)1/273, KernelOrientation.None);
        }
    }
}