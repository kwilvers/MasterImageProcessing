namespace ImageProcessing.Detection
{
    /**
	* @overview Filtre de détection verticale
	* @specfields name:String //"Vertical Filter"
	*/
    public class VerticalFilter : ConvolutionFilterBase
    {

        public override string Name => "Vertical Filter";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k = new double[,]{
                { 1, 0, -1 },
                { 1, 0, -1 },
                { 1, 0, -1 }
            };

            this.AddKernel(k, 1, KernelOrientation.East);
        }
    }
}