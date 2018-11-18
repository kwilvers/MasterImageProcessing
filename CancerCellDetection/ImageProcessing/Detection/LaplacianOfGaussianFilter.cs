namespace ImageProcessing.Detection
{
    /**
	* @overview Filtre de détection laplacien
	* @specfields name:String //"Laplacian Filter kernel size 4, connexity 4"
	*/
    public class LaplacianOfGaussianFilter : ConvolutionFilterBase
    {
        public override string Name => "Laplacian Filter kernel size 4, connexity 4";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k1 = new double[,]{
                {  0, 1, 1,   2,   2,   2, 1, 1, 0 },
                {  1, 2, 4,   5,   5,   5, 4, 2, 1 },
                {  1, 4, 5,   3,   0,   3, 5, 4, 1 },
                {  2, 5, 3, -12, -24, -12, 3, 5, 2 },
                {  2, 5, 0, -24, -40, -24, 0, 5, 2 },
                {  2, 5, 3, -12, -24, -12, 3, 5, 2 },
                {  1, 4, 5,   3,   0,   3, 5, 4, 1 },
                {  1, 2, 4,   5,   5,   5, 4, 2, 1 },
                {  0, 1, 1,   2,   2,   2, 1, 1, 0 }
            };

            this.AddKernel(k1, (double)1, KernelOrientation.West);
        }
    }
}