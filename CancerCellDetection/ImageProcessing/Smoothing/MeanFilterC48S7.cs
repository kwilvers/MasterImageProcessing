namespace ImageProcessing.Smoothing
{
    /**
	* @overview Filtre moyenneur de connexité 24 et de taille 5x5
	* @specfields name:String //"Mean Filter - Conexity 48 - Size 7x7"
	*/
    public class MeanFilterC48S7 : ConvolutionFilterBase
    {

        public override string Name => "Mean Filter - Conexity 48 - Size 7x7";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k = new double[,]{
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1 }
            };

            this.AddKernel(k, (double)1/49, KernelOrientation.None);
        }
    }
}