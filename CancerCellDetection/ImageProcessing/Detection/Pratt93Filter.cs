namespace ImageProcessing.Detection
{
    /**
	* @overview Filtre de détection de Roberts
	* @specfields name:String //"Pratt93 Filter"
	*/
    public class Pratt93Filter : ConvolutionFilterBase
    {
        public override string Name => "Pratt93 Filter";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k1 = new double[,]{
                {  0, -3,  0 },
                { -3,  9, -3 },
                {  0, -3,  0 }
            };

            this.AddKernel(k1, 1, KernelOrientation.West);
        }
    }
}