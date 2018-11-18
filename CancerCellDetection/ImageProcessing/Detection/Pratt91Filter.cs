namespace ImageProcessing.Detection
{
    /**
	* @overview Filtre de détection de Roberts
	* @specfields name:String //"Pratt91 Filter"
	*/
    public class Pratt91Filter : ConvolutionFilterBase
    {
        public override string Name => "Pratt91 Filter";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k1 = new double[,]{
                {  0, -1,  0 },
                { -1,  9, -1 },
                {  0, -1,  0 }
            };

            this.AddKernel(k1, 1, KernelOrientation.West);
        }
    }
}