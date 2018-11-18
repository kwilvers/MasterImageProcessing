namespace ImageProcessing.Detection
{
    /**
	* @overview Filtre de détection de Kirsch
	* @specfields name:String //"Robinson Filter"
	*/
    public class RobinsonFilter : ConvolutionFilterBase
    {
        public override string Name => "Robinson Filter";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k1 = new double[,]{
                { 1, 1, -1 },
                { 1, 2, -1 },
                { 1, 1, -1 }
            };
            this.AddKernel(k1, (double)1 / 5, KernelOrientation.West);

            var k2 = new double[,]{
                { 1,  1,  1 },
                { 1,  2, -1 },
                { 1, -1, -1 }
            };
            this.AddKernel(k2, (double)1 / 5, KernelOrientation.WesternNorth);

            var k3 = new double[,]{
                {  1,  1,  1 },
                {  1,  2,  1 },
                { -1, -1, -1 }
            };
            this.AddKernel(k3, (double)1 / 5, KernelOrientation.North);

            var k4 = new double[,]{
                {  1,  1, 1 },
                { -1,  2, 1 },
                { -1, -1, 1 }
            };
            this.AddKernel(k4, (double)1 / 5, KernelOrientation.EasternNorth);

            var k5 = new double[,]{
                { -1, 1, 1 },
                { -1, 2, 1 },
                { -1, 1, 1 }
            };
            this.AddKernel(k5, (double)1 / 5, KernelOrientation.East);

            var k6 = new double[,]{
                { -1, -1, 1 },
                { -1,  2, 1 },
                {  1,  1, 1 }
            };
            this.AddKernel(k6, (double)1 / 5, KernelOrientation.EasternSouth);

            var k7 = new double[,]{
                { -1, -1, -1 },
                {  1,  2,  1 },
                {  1,  1,  1 }
            };
            this.AddKernel(k7, (double)1 / 5, KernelOrientation.South);

            var k8 = new double[,]{
                {  1, -1, -1 },
                {  1,  2, -1 },
                {  1,  1,  1 }
            };
            this.AddKernel(k8, (double)1 / 5, KernelOrientation.WesternSouth);

        }
    }
}