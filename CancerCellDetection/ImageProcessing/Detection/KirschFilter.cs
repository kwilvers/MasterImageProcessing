namespace ImageProcessing.Detection
{
    /**
	* @overview Filtre de détection de Kirsch
	* @specfields name:String //"Kirsch Filter"
	*/
    public class KirschFilter : ConvolutionFilterBase
    {
        public override string Name => "Kirsch Filter";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k1 = new double[,]{
                { 5, -3, -3 },
                { 5,  0, -3 },
                { 5, -3, -3 }
            };
            this.AddKernel(k1, (double)1 / 15, KernelOrientation.West);

            var k2 = new double[,]{
                {  5,  5, -3 },
                {  5,  0, -3 },
                { -3, -3, -3 }
            };
            this.AddKernel(k2, (double)1 / 15, KernelOrientation.WesternNorth);

            var k3 = new double[,]{
                {  5,  5,  5 },
                { -3,  0, -3 },
                { -3, -3, -3 }
            };
            this.AddKernel(k3, (double)1 / 15, KernelOrientation.North);

            var k4 = new double[,]{
                { -3,  5,  5 },
                { -3,  0,  5 },
                { -3, -3, -3 }
            };
            this.AddKernel(k4, (double)1 / 15, KernelOrientation.EasternNorth);

            var k5 = new double[,]{
                { -3, -3, 5 },
                { -3,  0, 5 },
                { -3, -3, 5 }
            };
            this.AddKernel(k5, (double)1 / 15, KernelOrientation.East);

            var k6 = new double[,]{
                { -3, -3, -3 },
                { -3,  0,  5 },
                { -3,  5,  5 }
            };
            this.AddKernel(k6, (double)1 / 15, KernelOrientation.EasternSouth);

            var k7 = new double[,]{
                { -3, -3, -3 },
                { -3,  0, -3 },
                {  5,  5,  5 }
            };
            this.AddKernel(k7, (double)1 / 15, KernelOrientation.South);

            var k8 = new double[,]{
                { -3, -3, -3 },
                {  5,  0, -3 },
                {  5,  5, -3 }
            };
            this.AddKernel(k8, (double)1 / 15, KernelOrientation.WesternSouth);

        }
    }
}