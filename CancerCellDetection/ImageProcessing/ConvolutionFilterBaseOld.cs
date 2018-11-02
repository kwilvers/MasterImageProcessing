using System;
using System.Text;

namespace ImageProcessing
{
    /**
	* @overview Classe de base d'un filtre de convolution, IMMUTABLE 
	* @specfields Kernel:double[,] la matrice représentant le filtre
	* @specfields name:string le nom du filtre 
	* @derivedfields Size:double la taille du kernel (largeur ou hauteur)
	* @derivedfields Padding:double la taille du kernel -1 divisé par deux
	* @invariant Le kernel est carré, la hauteur et largeur sont identique
	*/
    public abstract class ConvolutionFilterBaseOld
    {
        // La fonction d’abstraction est 
        // FA(c) = c.Kernel[i,j]  | 0 <= i,j < c.kernel.Lenght

        // Invariant de représentation
        //  kernel != null
        //  && kernel.width == kernel.height
        //  && Name != null
        //  && Mltiplier > 0 
        //  && Multilier = ∀ int i,j Σ kernel[i,j]
        //  && Size == kernel.lenght
        //  && Padding > 0 && Padding == (kernel.Length - 1) / 2

        public abstract string Name { get; }

        public double Multiplier { get; set; }

        private double[,] kernel;
        public double[,] Kernel
        {
            get => kernel;
            set
            {
                if (value == null || value.Length == 0 || value.Length != value.GetLength(0))
                    throw new ArgumentException("Kernel matrix must be a square");

                kernel = value;

                Multiplier = 0;
                for (var i = 0; i < Size; i++)
                    for (var j = 0; j < Size; j++)
                        Multiplier += kernel[i, j];

                if (Math.Abs(Multiplier) < float.Epsilon)
                    Multiplier = 1;

                if (!RepOk())
                    throw new ArgumentException("The representation is invalide");
            }
        }

        public int Size => kernel.Length;

        public int Padding => (kernel.Length - 1) / 2;

        /** Défini comment les objets sont représentés
		* La relation entre C : la rep (variable d’instance) et A : le commentaire de l’overview
		*/
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name);
            sb.Append(":[");
            for (var i = 0; i < Size; i++)
            {
                sb.Append(System.Environment.NewLine);
                sb.Append("[");
                for (var j = 0; j < Size; j++)
                {
                    sb.Append(kernel[i, j]);
                    if (j < Size-1)
                        sb.Append(",");
                }
                sb.Append("]");
            }

            sb.Append("]");

            return sb.ToString();
        }

        /** Vérifie l'invariant de représentation
		* A partir de test unitaire
		* A partir de chaque constructeur ou mutateur
		* if( repOK() == false ){
		* 	throw new InvalidOperationException("") ;
		* }
		*/
        public bool RepOk()
        {
            if (kernel == null || kernel.Length == kernel.GetLength(0) || String.IsNullOrWhiteSpace(Name)
                || Multiplier <= 0 || Size != kernel.Length)
                return false;

            double m = 0;
            for (var i = 0; i < Size; i++)
                for (var j = 0; j < Size; j++)
                    m += kernel[i, j];
            if( Math.Abs(m - Multiplier) > float.Epsilon)
                throw new ArgumentException("The representation is invalide");
            return true;
        }
    }
}