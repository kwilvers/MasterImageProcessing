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
            double factor = (double)1 / 2;

            var k1 = new double[,]{
                { -1, 0, 1 },
                { -2, 0, 2 },
                { -1, 0, 2 }
            };
            this.AddKernel(k1, factor, KernelOrientation.East);

            var k2 = new double[,]{
                { -2, -1, 0 },
                { -1, 0, 1 },
                { 0, 1, 2 }
            };
            this.AddKernel(k2, factor, KernelOrientation.EasternNorth);

            var k3 = new double[,]{
                { -1, -2, -1 },
                { 0, 0, 0 },
                { 1, 2, 1 }
            };
            this.AddKernel(k3, factor, KernelOrientation.North);

            var k4 = new double[,]{
                { 0, -1, -2 },
                { 1, 0, -1 },
                { 2, 1, 0 }
            };
            this.AddKernel(k4, factor, KernelOrientation.WesternNorth);
        }
    }
}