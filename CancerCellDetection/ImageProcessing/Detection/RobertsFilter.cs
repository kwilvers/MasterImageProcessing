namespace ImageProcessing.Detection
{
    /**
	* @overview Filtre de détection de Roberts
	* @specfields name:String //"Roberts Filter"
	*/
    public class RobertsFilter : ConvolutionFilterBase
    {
        public override string Name => "Roberts Filter";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k1 = new double[,]{
                { 1,  0 },
                { 0, -1 }
            };
            var k2 = new double[,]{
                {  0, 1 },
                { -1, 0 }
            };

            this.AddKernel(k1, 1, KernelOrientation.EasternNorth);
            this.AddKernel(k2, 1, KernelOrientation.WesternNorth);
        }
    }
}