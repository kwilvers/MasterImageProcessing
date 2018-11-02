using System;
using System.Text;

namespace ImageProcessing
{
    /**
	* @overview Représente un élément kernel/filtre  
	* @specfields Orientation:KernelOrientation //Orientation du kernel
	* @specfields Factor:double                 //Facteur multiplicatif à appliquer lors de la convolution
	* @specfields Kernel:double[][]             //Tableau carré contenant le filtre de convolution
	* @derivedfields size:int                   //La hauteur/largeur du kernel
	* @invariant Factor > 0 
	* @invariant Orientation != null
	* @invariant Kernel != null
	*/
    public class KernelItem
    {
        // La fonction d’abstraction est 
        // FA(c) = [Kernel[0,0], Kernel[0,1], .., Kernel[n,n] ]  

        // Invariant de représentation
        // Orientation != null
        // && Factor > 0
        // && Kernel != null
        // && kernel.width == kernel.lenght[0]
        // && kernel.height == kernel.lenght[1]
        // && kernel.width == kernel.height
        // && Size == Kernel.lenght[0]


        public KernelOrientation Orientation { get; }
        public double Factor { get; }
        public double[,] Kernel { get; }

        public int Size => this.Kernel.GetLength(0);

        public KernelItem(KernelOrientation orientation, double factor, double[,] kernel)
        {
            Orientation = orientation;
            Factor = factor;
            Kernel = kernel;
        }

        /** Défini comment les objets sont représentés
		* La relation entre C : la rep (variable d’instance) et A : le commentaire de l’overview
		*/
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Orientation);
            sb.Append(":");
            sb.Append(Factor);
            sb.Append(":[");
            for (var i = 0; i < Size; i++)
            {
                sb.Append(Environment.NewLine);
                sb.Append("[");
                for (var j = 0; j < Size; j++)
                {
                    sb.Append(Kernel[i, j]);
                    if (j < Size - 1)
                        sb.Append(",");
                }
                sb.Append("]");
            }
            sb.Append("]");

            return sb.ToString();
        }

        /** Vérifie l'invariant de représentation
		*/
        public bool RepOk()
        {
            if (Factor <= 0) return false;
            if (Kernel == null) return false;
            if (Kernel.GetLength(0) != Kernel.GetLength(1)) return false;
            return true;
        }
    }
}