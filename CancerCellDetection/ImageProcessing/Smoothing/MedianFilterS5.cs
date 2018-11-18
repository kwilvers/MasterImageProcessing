namespace ImageProcessing.Smoothing
{
    /**
	* @overview Filtre médian de taille 5x5
	* @specfields name:String //"Median Filter - Size 5x5"
	*/
    public class MedianFilterS5 : ConvolutionFilterBase
    {

        public override string Name => "Median Filter - Size 5x5";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k = new double[,]{
                { 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1 }
            };

            this.AddKernel(k, 1, KernelOrientation.None);
        }
    }
}