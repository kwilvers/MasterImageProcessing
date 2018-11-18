namespace ImageProcessing.Detection
{
    /**
	* @overview Filtre de détection de Scharr
	* @specfields name:String //"Scharr Filter"
	*/
    public class ScharrFilter : ConvolutionFilterBase
    {
        public override string Name => "Scharr Filter";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k1 = new double[,]{
                {  -3, 0, 3 },
                { -10, 0, 10 },
                {  -3, 0, 3 }
            };
            this.AddKernel(k1, (double)1 / 32, KernelOrientation.East);

            var k3 = new double[,]{
                { -3, -10, -3 },
                {  0,   0,  0 },
                {  3,  10,  3 }
            };
            this.AddKernel(k3, (double)1 / 32, KernelOrientation.North);
        }
    }
}