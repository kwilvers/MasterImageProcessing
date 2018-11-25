namespace ImageProcessing.Morphology
{
    /**
	* @overview Element structurant en forme de carré
	* @specfields name:String //"Square structured element"
	*/
    public class SquareStructuredElement : ConvolutionFilterBase
    {
        public override string Name => "Square structured element";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k1 = new double[,]{
                { 1, 1, 1 },
                { 1, 1, 1 },
                { 1, 1, 1 }
            };

            this.AddKernel(k1, 1, KernelOrientation.None);
        }
    }
}