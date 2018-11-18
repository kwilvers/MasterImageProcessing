namespace ImageProcessing.Detection
{
    /**
	* @overview Filtre de détection de Roberts
	* @specfields name:String //"Pratt93 Filter"
	*/
    public class Pratt274Filter : ConvolutionFilterBase
    {
        public override string Name => "Pratt274 Filter";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k1 = new double[,]{
                { -1, -4, -1 },
                { -4, 27, -4 },
                { -1, -4, -1 }
            };

            this.AddKernel(k1, (double)1/6, KernelOrientation.West);
        }
    }
}