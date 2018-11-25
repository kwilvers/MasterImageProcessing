namespace ImageProcessing.Morphology
{
    /**
	* @overview Element structurant en forme de croix
	* @specfields name:String //"Cross structured element"
	*/
    public class CrossStructuredElement : ConvolutionFilterBase
    {
        public override string Name => "Cross structured element";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k1 = new double[,]{
                { 0, 1, 0 },
                { 1, 1, 1 },
                { 0, 1, 0 }
            };

            this.AddKernel(k1, 1, KernelOrientation.None);
        }
    }
}