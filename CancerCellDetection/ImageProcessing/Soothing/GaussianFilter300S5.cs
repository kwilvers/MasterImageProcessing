namespace ImageProcessing.Soothing
{
    /**
	* @overview Filtre moyenneur de connexité 24 et de taille 5x5
	* @specfields name:String //"Mean Filter - Conexity 24 - Size 5x5"
	*/
    public class GaussianFilter300S5 : ConvolutionFilterBase
    {

        public override string Name => "Mean Filter - Conexity 24 - Size 5x5";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k = new double[,]{
                { 1,  4,  6,  4, 1 },
                { 4, 18, 30, 18, 4 },
                { 6, 30, 48, 30, 6 },
                { 4, 18, 30, 18, 4 },
                { 1,  4,  6,  4, 1 }
            };

            this.AddKernel(k, Math2.Div(1, 300), KernelOrientation.None);
        }
    }
}