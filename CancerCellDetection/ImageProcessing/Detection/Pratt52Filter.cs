namespace ImageProcessing.Detection
{
    /**
	* @overview Filtre de détection de Roberts
	* @specfields name:String //"Pratt52 Filter"
	*/
    public class Pratt52Filter : ConvolutionFilterBase
    {
        public override string Name => "Pratt52 Filter";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k1 = new double[,]{
                {  0, -2,  0 },
                { -2,  5, -2 },
                {  0, -2,  0 }
            };

            this.AddKernel(k1, 1, KernelOrientation.West);
        }
    }
}