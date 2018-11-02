using System;
using System.Text;

namespace ImageProcessing
{
    /**
	* @overview Convolution d'une image au format RGB
	* @specfields nom:type //éléments nommé repris dans un n-uplet
	* @derivedfields nom:type //élément dérivé des @specfields
	* @invariant description des invariants abstrait qui doivent être vérifié à tout moment
	*/
    public class RgbConvolution
    {
        // La fonction d’abstraction est 
        // FA(c) = c0 + c1x + c2x^2 + ... 
        // où 
        // ci = c.trms[i] si 0 <= i < c.trms.length 
        // = 0 sinon
        //

        // Invariant de représentation
        // Eliminer les valeurs pour lesquelles FA n’a pas de signification (null) : c.els != null
        // Eliminer les valeurs qui produisent des résultat erronés IR(c) : pas de doublons, c.x < 0
        // Enoncer les contraintes requises par les structures de données et algorithme : tableau trié
        // Identifier les variables d’instance qui doivent être synchro : size représente la taille de la liste
        // Identifier les valeurs qui déclencheraient une exception : référence null, division par 0
        // Identifier les contraintes imposées par le domaine d’application @invariant : un fou blanc sur une case blanche, un transfert d’argent toujours positif
        //  c.els ≠ null && 
        //  ∀ int i,j · Σ (0<=i<j<c.els.size() ⇒ 
        //  c.els.get(i) ≠ c.els.get(j))
        //
    
        
        
        

        /** Défini comment les objets sont représentés
		* La relation entre C : la rep (variable d’instance) et A : le commentaire de l’overview
		*/
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            throw new NotImplementedException();

            return sb.ToString();
        }

        /** Vérifie l'invariant de représentation
		* A partir de test unitaire
		* A partir de chaque constructeur ou mutateur
          if (!RepOk())
			  throw new ArgumentException("The representation is invalide");
		*/
        public bool RepOk()
        {
            throw new NotImplementedException();
        }

        
    }
}