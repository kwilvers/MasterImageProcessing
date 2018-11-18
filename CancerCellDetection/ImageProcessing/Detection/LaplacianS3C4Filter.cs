namespace ImageProcessing.Detection
{
    /**
	* @overview Filtre de détection laplacien
	* @specfields name:String //"Laplacian Filter kernel size 3, connexity 4"
	*/
    public class LaplacianS3C4Filter : ConvolutionFilterBase
    {
        public override string Name => "Laplacian Filter kernel size 3, connexity 4";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k1 = new double[,]{
                {  0, -1,  0 },
                { -1,  4, -1 },
                {  0, -1,  0 }
            };

            this.AddKernel(k1, (double)1, KernelOrientation.West);
            //this.Offset = 128;
        }
    }
}