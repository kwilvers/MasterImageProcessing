namespace ImageProcessing.Smoothing
{
    /**
	* @overview Filtre gaussien et de taille 5x5
	* @specfields name:String //"Gaussian Filter - 159 - Size 5x5"
	*/
    public class GaussianFilter159S5 : ConvolutionFilterBase
    {

        public override string Name => "Gaussian Filter - 159 - Size 5x5";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k = new double[,]{
                { 2,  4,  5,  4, 2 },
                { 4,  9, 12,  9, 4 },
                { 5, 12, 15, 12, 5 },
                { 4,  9, 12,  9, 4 },
                { 2,  4,  5,  4, 2 }
            };

            this.AddKernel(k, (double)1/159, KernelOrientation.None);
        }
    }
}