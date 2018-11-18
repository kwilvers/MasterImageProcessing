namespace ImageProcessing.Smoothing
{
    /**
	* @overview Filtre médian de taille 3x3
	* @specfields name:String //"Median Filter - Size 3x3"
	*/
    public class MedianFilterS3 : ConvolutionFilterBase
    {

        public override string Name => "Median Filter - Size 3x3";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k = new double[,]{
                { 1, 1, 1 },
                { 1, 1, 1 },
                { 1, 1, 1 }
            };

            this.AddKernel(k, 1, KernelOrientation.None);
        }
    }
}