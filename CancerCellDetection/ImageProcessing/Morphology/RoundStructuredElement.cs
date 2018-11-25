namespace ImageProcessing.Morphology
{
    /**
	* @overview Element structurant en forme de rond
	* @specfields name:String //"Round structured element"
	*/
    public class RoundStructuredElement : ConvolutionFilterBase
    {
        public override string Name => "Round structured element";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k1 = new double[,]{
                { 0, 0, 0, 1, 0, 0, 0 },
                { 0, 1, 1, 1, 1, 1, 0 },
                { 0, 1, 1, 1, 1, 1, 0 },
                { 1, 1, 1, 1, 1, 1, 1 },
                { 0, 1, 1, 1, 1, 1, 0 },
                { 0, 1, 1, 1, 1, 1, 0 },
                { 0, 0, 0, 1, 0, 0, 0 },
            };

            this.AddKernel(k1, 1, KernelOrientation.None);
        }
    }
}