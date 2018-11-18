namespace ImageProcessing.Smoothing
{
    /**
	* @overview Filtre gaussien et de taille 5x5
	* @specfields name:String //"Gaussian Filter - 52 - Size 5x5"
	*/
    public class GaussianFilter52S5 : ConvolutionFilterBase
    {

        public override string Name => "Mean Filter - 52 - Size 5x5";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k = new double[,]{
                { 1, 1, 2, 1, 1 },
                { 1, 2, 4, 2, 1 },
                { 2, 4, 8, 4, 2 },
                { 1, 2, 4, 2, 1 },
                { 1, 1, 2, 1, 1 }
            };

            this.AddKernel(k, (double)1/52, KernelOrientation.None);
        }
    }
}