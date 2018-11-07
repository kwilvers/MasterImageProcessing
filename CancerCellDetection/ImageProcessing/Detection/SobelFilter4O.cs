namespace ImageProcessing.Detection
{
    /**
	* @overview Filtre de détection vertical
	* @specfields name:String //"Horizontal Filter"
	*/
    public class SobelFilter4O : ConvolutionFilterBase
    {

        public override string Name => "Sobel4O Filter";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k1 = new double[,]{
                { -1, 0, 1 },
                { -2, 0, 2 },
                { -1, 0, 2 }
            };
            this.AddKernel(k1, (double)1/4, KernelOrientation.East);

            var k2 = new double[,]{
                { -2, -1, 0 },
                { -1, 0, 1 },
                { 0, 1, 2 }
            };
            this.AddKernel(k2, (double)1/4, KernelOrientation.EasternNorth);

            var k3 = new double[,]{
                { -1, -2, -1 },
                { 0, 0, 0 },
                { 1, 2, 1 }
            };
            this.AddKernel(k3, (double)1 / 4, KernelOrientation.North);

            var k4 = new double[,]{
                { 0, -1, -2 },
                { 1, 0, -1 },
                { 2, 1, 0 }
            };
            this.AddKernel(k4, (double)1 / 4, KernelOrientation.WesternNorth);
        }
    }
}