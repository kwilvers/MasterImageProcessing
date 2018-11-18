namespace ImageProcessing.Smoothing
{
    /**
	* @overview Filtre gaussien de taille 3x3
	* @specfields name:String //"Gaussian Filter - 16 - Size 3x3"
	*/
    public class GaussianFilter16S3 : ConvolutionFilterBase
    {

        public override string Name => "Mean Filter - 16 - Size 3x3";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k = new double[,]{
                { 1, 2, 1 },
                { 2, 4, 2 },
                { 1, 2, 1 }
            };

            this.AddKernel(k, (double)1/16, KernelOrientation.None);
        }
    }
}