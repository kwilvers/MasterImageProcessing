using System;
using System.Text;

namespace ImageProcessing
{
    /**
	* @overview Filtre moyenneur de connexité 24 et de taille 5x5
	* @specfields name:String //"Mean Filter - Conexity 24 - Size 5x5"
	*/
    public class MeanFilterC24S5 : ConvolutionFilterBase
    {
        /** Défini comment les objets sont représentés
		* La relation entre C : la rep (variable d’instance) et A : le commentaire de l’overview
		*/
        public override string Name => "Mean Filter - Conexity 24 - Size 5x5";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k = new double[,]{
                { 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1 }
            };

            this.AddKernel(k, Math2.Div(1, 25), KernelOrientation.None);
        }
    }
}