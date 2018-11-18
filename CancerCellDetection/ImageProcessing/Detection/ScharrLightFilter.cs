namespace ImageProcessing.Detection
{
    /**
	* @overview Filtre de détection de Scharr
	* @specfields name:String //"Scharr Filter"
	*/
    public class ScharrLightFilter : ConvolutionFilterBase
    {
        public override string Name => "Light Scharr Filter";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k1 = new double[,]{
                { -1, 0, 1 },
                { -3, 0, 3 },
                { -1, 0, 1 }
            };
            this.AddKernel(k1, (double)1/10, KernelOrientation.East);

            var k3 = new double[,]{
                { -1, -3, -1 },
                {  0,  0,  0 },
                {  1,  3,  1 }
            };
            this.AddKernel(k3, (double)1/10, KernelOrientation.North);
        }
    }
}