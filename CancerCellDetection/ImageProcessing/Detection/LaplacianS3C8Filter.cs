namespace ImageProcessing.Detection
{
    /**
	* @overview Filtre de détection laplacien
	* @specfields name:String //"Laplacian Filter kernel size 3, connexity 8"
	*/
    public class LaplacianS3C8Filter : ConvolutionFilterBase
    {
        public override string Name => "Laplacian Filter kernel size 3, connexity 8";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k1 = new double[,]{
                { -1, -1, -1 },
                { -1,  8, -1 },
                { -1, -1, -1 }
            };

            this.AddKernel(k1, (double)1, KernelOrientation.West);
            //this.Offset = 128;
        }
    }
}