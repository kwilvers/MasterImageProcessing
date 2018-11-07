namespace ImageProcessing.Soothing
{
    /**
	* @overview Filtre moyenneur de connexité 24 et de taille 5x5
	* @specfields name:String //"Mean Filter - Conexity 24 - Size 5x5"
	*/
    public class GaussianFilter140S7 : ConvolutionFilterBase
    {

        public override string Name => "Mean Filter - Conexity 24 - Size 5x5";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k = new double[,]{
                { 1, 1, 2, 2, 2, 1, 1 },
                { 1, 2, 2, 4, 2, 2, 1 },
                { 2, 2, 4, 8, 4, 2, 2 },
                { 2, 4, 8, 16, 8, 4, 2 },
                { 2, 2, 4, 8, 4, 2, 2 },
                { 1, 2, 2, 4, 2, 2, 1 },
                { 1, 1, 2, 2, 2, 1, 1 }
            };

            this.AddKernel(k, Math2.Div(1, 140), KernelOrientation.None);
        }
    }
}