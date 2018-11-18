namespace ImageProcessing.Smoothing
{
    /**
	* @overview Filtre médian de taille 7x7
	* @specfields name:String //"Median Filter - Size 7x7"
	*/
    public class MedianFilterS7 : ConvolutionFilterBase
    {

        public override string Name => "Median Filter - Size 7x7";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k = new double[,]{
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1 }
            };

            this.AddKernel(k, 1, KernelOrientation.None);
        }
    }
}