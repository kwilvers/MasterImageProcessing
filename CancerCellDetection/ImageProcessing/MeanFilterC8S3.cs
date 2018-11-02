using System;
using System.Text;

namespace ImageProcessing
{
    /**
	* @overview Filtre moyenneur de connexité 8 et de taille 3x3
	* @specfields name:String //"Mean Filter - Conexity 8 - Size 3x3"
	*/
    public class MeanFilterC8S3 : ConvolutionFilterBase
    {
        /** Défini comment les objets sont représentés
		* La relation entre C : la rep (variable d’instance) et A : le commentaire de l’overview
		*/
        public override string Name => "Mean Filter - Conexity 8 - Size 3x3";

        /**
        * @see base.InitKernels();
        */
        protected override void InitKernels()
        {
            var k = new double[,]{
                { 1, 1, 1, },
                { 1, 1, 1, },
                { 1, 1, 1  }
            };

            this.AddKernel(k, Math2.Div(1, 9), KernelOrientation.None);
        }
    }
}