namespace ImageProcessing.Detection
{
    /**
	* @overview Filtre de détection de Scharr de taille 5
	* @specfields name:String //"Prewitt Filter"
	*/
    public class ScharrS5Filter : ConvolutionFilterBase
    {
        public override string Name => "Scharr Filter, size 5x5";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k1 = new double[,]{
                { -1, -1, 0, 1, 1 },
                { -2, -2, 0, 2, 2 },
                { -3, -6, 0, 6, 3 },
                { -2, -2, 0, 2, 2 },
                { -1, -1, 0, 1, 1 }
            };
            this.AddKernel(k1, (double)1/60, KernelOrientation.East);

            var k3 = new double[,]{
                { -1, -2, -3, -2, -1 },
                { -1, -2, -6, -2, -1 },
                {  0,  0,  0,  0,  0 },
                {  1,  2,  6,  2,  1 },
                {  1,  2,  3,  2,  1 }
            };
            this.AddKernel(k3, (double)1/60, KernelOrientation.North);
        }
    }
}