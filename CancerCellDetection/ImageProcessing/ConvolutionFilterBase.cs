using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageProcessing
{
    /**
	* @overview Classe de base d'un filtre de convolution, IMMUTABLE 
	* @specfields Kernel:double[,] la matrice représentant le filtre
	* @specfields name:string le nom du filtre 
	* @derivedfields Size:double la taille du kernel (largeur ou hauteur)
	* @derivedfields Padding:double (la taille du kernel-1) divisé par deux
	* @invariant Le kernel est carré, la hauteur et largeur sont identique
	*/
    public abstract class ConvolutionFilterBase
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

        private readonly List<KernelItem> kernels;
        public IEnumerable<KernelItem> Kernels => kernels;

        public int Size => Kernels.First().Size;

        public int Padding => (this.Size - 1) / 2;

        public bool ForceAbsoluteValue { get; set; }

        public byte Offset { get; set; }

        protected ConvolutionFilterBase()
        {
            ForceAbsoluteValue = true;
            Offset = 0;
            this.kernels = new List<KernelItem>();
            InitKernels();
            RepOk();
        }

        /**
		* @requires kernels!= null 
		* @modifies Kernels est modifier pour contenir le(s) kernel(s)
		* @effects Initialise la liste de(s) kernel(s)
		*/
        protected abstract void InitKernels();

        protected void AddKernel(double[,] kernel, double factor, KernelOrientation orientation)
        {
            KernelItem k = new KernelItem(orientation, factor, kernel);
            this.kernels.Add(k);
        }

        /** Défini comment les objets sont représentés
		* La relation entre C : la rep (variable d’instance) et A : le commentaire de l’overview
		*/
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name);
            sb.Append(":[");
            foreach (var kernel in kernels)
            {
                sb.Append(kernel.ToString());
                sb.Append(Environment.NewLine);
            }
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
            if (Kernels == null || !Kernels.Any() )
                return false;

            return Kernels.All(kernel => kernel.RepOk());
        }
    }
}